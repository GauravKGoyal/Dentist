using Dentist.Models.Patient;
using Dentist.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using System.Data.Entity;
using AutoMapper.QueryableExtensions;

namespace Dentist.Controllers
{
    public class TreatmentPlansApiController : BaseApiController
    {
        // GET: api/TreatmentPlansApi
        [EnableQuery]
        public IQueryable<TreatmentPlanDto> GetTreatmentPlans()
        {
            var query = ReadContext.Set<TreatmentPlan>().Include(x => x.Treatments).ProjectTo<TreatmentPlanDto>().OrderByDescending(x => x.RecordedDate);
            return query;
        }

        // GET: api/TreatmentPlansApi/5
        [ResponseType(typeof(TreatmentPlanDto))]
        public IHttpActionResult GetTreatmentPlan(int id)
        {
            TreatmentPlan treatmentPlan = ReadContext.TreatmentPlans.Include(x => x.Treatments).FirstOrDefault(x => x.Id == id);
            if (treatmentPlan == null)
            {
                return NotFound();
            }

            var treatmentPlanDto = AutoMapper.Mapper.Map<TreatmentPlanDto>(treatmentPlan);
            return Ok(treatmentPlanDto);
        }

        // PUT: api/TreatmentPlansApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTreatmentPlan(TreatmentPlanDto treatmentPlanDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var treatmentPlanEnvelop = WriteContext.TreatmentPlans.Find(treatmentPlanDto.Id);
            if (treatmentPlanEnvelop == null)
            {
                return NotFound();
            }

            // delete notes
            var deletedNotesDto = treatmentPlanDto.Treatments.Where(treatmentDto => treatmentDto.ObjectState == "delete").ToList();
            deletedNotesDto.ForEach(treatmentDto =>
            {
                var treatment = new Treatment() { Id = treatmentDto.Id };
                WriteContext.Treatments.Attach(treatment);
                WriteContext.Treatments.Remove(treatment);
            });

            // add notes
            var addedNotesDto = treatmentPlanDto.Treatments.Where(treatmentDto => treatmentDto.ObjectState == "add").ToList();
            addedNotesDto.ForEach(treatmentDto =>
            {
                var treatment = AutoMapper.Mapper.Map<Treatment>(treatmentDto);
                treatmentPlanEnvelop.Treatments.Add(treatment);
            });

            // update notes
            var updatedNotesDto = treatmentPlanDto.Treatments.Where(treatmentDto => treatmentDto.ObjectState == "update").ToList();
            updatedNotesDto.ForEach(treatmentDto =>
            {
                var treatment = WriteContext.Treatments.First(x => x.Id == treatmentDto.Id);
                AutoMapper.Mapper.Map(treatmentDto, treatment);
            });

            if (!WriteContext.TrySaveChanges(ModelState))
            {
                return BadRequest(ModelState);
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/TreatmentPlansApi
        [ResponseType(typeof(TreatmentPlanDto))]
        public IHttpActionResult PostTreatmentPlan(TreatmentPlanDto treatmentPlanDto)
        {
            var isNewRecord = (treatmentPlanDto.Id == 0);
            if (!isNewRecord)
            {
                return BadRequest("Only new patient can be created using PostTreatmentPlan call");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var treatmentPlan = WriteContext.TreatmentPlans.Create();
            AutoMapper.Mapper.Map(treatmentPlanDto, treatmentPlan);
            treatmentPlan.RecordedDate = DateTime.Now;

            WriteContext.TreatmentPlans.Add(treatmentPlan);
            if (!WriteContext.TrySaveChanges(ModelState))
            {
                return BadRequest(ModelState);
            }

            return CreatedAtRoute("DefaultApi", new { id = treatmentPlanDto.Id }, treatmentPlanDto);
        }

        // DELETE: api/TreatmentPlansApi/5
        [ResponseType(typeof(TreatmentPlan))]
        public IHttpActionResult DeleteTreatmentPlan(int id)
        {
            TreatmentPlan treatmentPlan = WriteContext.TreatmentPlans.Include(x => x.Treatments).FirstOrDefault(x => x.Id == id);
            if (treatmentPlan == null)
            {
                return NotFound();
            }

            for (int i = treatmentPlan.Treatments.Count - 1; i > -1; i--)
            {
                WriteContext.Treatments.Remove(treatmentPlan.Treatments.ElementAt(i));
            }
            WriteContext.TreatmentPlans.Remove(treatmentPlan);

            if (!WriteContext.TrySaveChanges(ModelState))
            {
                return BadRequest(ModelState);
            }

            return Ok(treatmentPlan);
        }

        private bool TreatmentPlanExists(int id)
        {
            return ReadContext.TreatmentPlans.Count(e => e.Id == id) > 0;
        }
    }
}