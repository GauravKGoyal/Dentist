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

namespace Dentist.Controllers
{
    public class PatientNotesControllerSample : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientNotes
        public IQueryable<PatientNote> GetPatientNotes()
        {
            return db.PatientNotes;
        }

        // GET: api/PatientNotes/5
        [ResponseType(typeof(PatientNote))]
        public IHttpActionResult GetPatientNote(int id)
        {
            PatientNote patientNote = db.PatientNotes.Find(id);
            if (patientNote == null)
            {
                return NotFound();
            }

            return Ok(patientNote);
        }

        // PUT: api/PatientNotes/5
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

            db.Entry(patientNote).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
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

        // POST: api/PatientNotes
        [ResponseType(typeof(PatientNote))]
        public IHttpActionResult PostPatientNote(PatientNote patientNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PatientNotes.Add(patientNote);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = patientNote.Id }, patientNote);
        }

        // DELETE: api/PatientNotes/5
        [ResponseType(typeof(PatientNote))]
        public IHttpActionResult DeletePatientNote(int id)
        {
            PatientNote patientNote = db.PatientNotes.Find(id);
            if (patientNote == null)
            {
                return NotFound();
            }

            db.PatientNotes.Remove(patientNote);
            db.SaveChanges();

            return Ok(patientNote);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PatientNoteExists(int id)
        {
            return db.PatientNotes.Count(e => e.Id == id) > 0;
        }
    }
}