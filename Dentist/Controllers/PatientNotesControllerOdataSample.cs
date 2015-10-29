using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Routing;
using Dentist.Models;
using Dentist.Models.Patient;

namespace Dentist.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Dentist.Models.Patient;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<PatientNote>("PatientNotesControllerOdataSample");
    builder.EntitySet<Note>("Notes"); 
    builder.EntitySet<Patient>("People"); 
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class PatientNotesControllerOdataSample : ODataController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: odata/PatientNotesControllerOdataSample
        [EnableQuery]
        public IQueryable<PatientNote> GetPatientNotesControllerOdataSample()
        {
            return db.PatientNotes;
        }

        // GET: odata/PatientNotesControllerOdataSample(5)
        [EnableQuery]
        public SingleResult<PatientNote> GetPatientNote([FromODataUri] int key)
        {
            return SingleResult.Create(db.PatientNotes.Where(patientNote => patientNote.Id == key));
        }

        // PUT: odata/PatientNotesControllerOdataSample(5)
        public IHttpActionResult Put([FromODataUri] int key, Delta<PatientNote> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PatientNote patientNote = db.PatientNotes.Find(key);
            if (patientNote == null)
            {
                return NotFound();
            }

            patch.Put(patientNote);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientNoteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(patientNote);
        }

        // POST: odata/PatientNotesControllerOdataSample
        public IHttpActionResult Post(PatientNote patientNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PatientNotes.Add(patientNote);
            db.SaveChanges();

            return Created(patientNote);
        }

        // PATCH: odata/PatientNotesControllerOdataSample(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public IHttpActionResult Patch([FromODataUri] int key, Delta<PatientNote> patch)
        {
            Validate(patch.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            PatientNote patientNote = db.PatientNotes.Find(key);
            if (patientNote == null)
            {
                return NotFound();
            }

            patch.Patch(patientNote);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientNoteExists(key))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Updated(patientNote);
        }

        // DELETE: odata/PatientNotesControllerOdataSample(5)
        public IHttpActionResult Delete([FromODataUri] int key)
        {
            PatientNote patientNote = db.PatientNotes.Find(key);
            if (patientNote == null)
            {
                return NotFound();
            }

            db.PatientNotes.Remove(patientNote);
            db.SaveChanges();

            return StatusCode(HttpStatusCode.NoContent);
        }

        // GET: odata/PatientNotesControllerOdataSample(5)/Notes
        [EnableQuery]
        public IQueryable<Note> GetNotes([FromODataUri] int key)
        {
            return db.PatientNotes.Where(m => m.Id == key).SelectMany(m => m.Notes);
        }

        // GET: odata/PatientNotesControllerOdataSample(5)/Patient
        [EnableQuery]
        public SingleResult<Patient> GetPatient([FromODataUri] int key)
        {
            return SingleResult.Create(db.PatientNotes.Where(m => m.Id == key).Select(m => m.Patient));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientNoteExists(int key)
        {
            return db.PatientNotes.Count(e => e.Id == key) > 0;
        }
    }
}
