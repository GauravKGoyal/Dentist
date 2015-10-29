using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Dentist.Models;
using Dentist.Models.Patient;
using Dentist.ViewModels;

namespace Dentist.Controllers
{
    public class PatientNotesApiController : BaseApiController
    {
        // GET: api/PatientNotesApi
        public IQueryable<PatientNote> GetPatientNotes()
        {
            return ReadContext.PatientNotes;
        }

        // GET: api/PatientNotesApi/5
        [ResponseType(typeof(PatientNote))]
        public IHttpActionResult GetPatientNote(int id)
        {
            PatientNote patientNote = ReadContext.PatientNotes.Find(id);
            if (patientNote == null)
            {
                return NotFound();
            }

            return Ok(patientNote);
        }

        // PUT: api/PatientNotesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPatientNote(int id, PatientNote patientNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != patientNote.Id)
            {
                return BadRequest();
            }

            WriteContext.Entry(patientNote).State = EntityState.Modified;

            try
            {
                WriteContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientNoteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PatientNotesApi
        [ResponseType(typeof(PatientNoteDto))]
        public IHttpActionResult PostPatientNote(PatientNoteDto patientNoteDto)
        {
            if (!ModelState.IsValid)
            {                
                return BadRequest(ModelState);
            }

            var isNewRecord = (patientNoteDto.Id == 0);
            if (isNewRecord)
            {
                var patientNote = AutoMapper.Mapper.Map<PatientNote>(patientNoteDto);
                patientNote.RecordedDate = DateTime.Today;
                patientNote.Notes.ForEach(note => note.RecordedDate = DateTime.Today);
                WriteContext.PatientNotes.Add(patientNote);
            }
            else
            {
                var patientNoteEnvelop = WriteContext.PatientNotes.Find(patientNoteDto.Id);
                var addedNotes = patientNoteDto.Notes.Where(noteDto => noteDto.ObjectState == "add");
                var updatedNotes = patientNoteDto.Notes.Where(noteDto => noteDto.ObjectState == "update");
                var deletedNotes = patientNoteDto.Notes.Where(noteDto => noteDto.ObjectState == "delete");
                patientNoteEnvelop.Notes.RemoveAll(note => deletedNotes.Any(dn => dn.Id == note.Id));
                //patientNoteEnvelop.Notes.Add(new Note() {});
            }
            WriteContext.TrySaveChanges(ModelState);

            return CreatedAtRoute("DefaultApi", new { id = patientNoteDto.Id }, patientNoteDto);
        }

        // DELETE: api/PatientNotesApi/5
        [ResponseType(typeof(PatientNote))]
        public IHttpActionResult DeletePatientNote(int id)
        {
            PatientNote patientNote = WriteContext.PatientNotes.Find(id);
            if (patientNote == null)
            {
                return NotFound();
            }

            WriteContext.PatientNotes.Remove(patientNote);
            WriteContext.SaveChanges();

            return Ok(patientNote);
        }

        private bool PatientNoteExists(int id)
        {
            return ReadContext.PatientNotes.Count(e => e.Id == id) > 0;
        }
    }
}