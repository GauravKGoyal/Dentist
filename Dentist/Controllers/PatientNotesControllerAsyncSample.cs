using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Dentist.Models;
using Dentist.Models.Patient;

namespace Dentist.Controllers
{
    public class PatientNotesControllerAsyncSample : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/PatientNotes
        public IQueryable<PatientNote> GetPatientNotes()
        {
            return db.PatientNotes;
        }

        // GET: api/PatientNotes/5
        [ResponseType(typeof(PatientNote))]
        public async Task<IHttpActionResult> GetPatientNote(int id)
        {
            PatientNote patientNote = await db.PatientNotes.FindAsync(id);
            if (patientNote == null)
            {
                return NotFound();
            }

            return Ok(patientNote);
        }

        // PUT: api/PatientNotes/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutPatientNote(int id, PatientNote patientNote)
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
                await db.SaveChangesAsync();
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
        public async Task<IHttpActionResult> PostPatientNote(PatientNote patientNote)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PatientNotes.Add(patientNote);
            await db.SaveChangesAsync();

            return CreatedAtRoute("DefaultApi", new { id = patientNote.Id }, patientNote);
        }

        // DELETE: api/PatientNotes/5
        [ResponseType(typeof(PatientNote))]
        public async Task<IHttpActionResult> DeletePatientNote(int id)
        {
            PatientNote patientNote = await db.PatientNotes.FindAsync(id);
            if (patientNote == null)
            {
                return NotFound();
            }

            db.PatientNotes.Remove(patientNote);
            await db.SaveChangesAsync();

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