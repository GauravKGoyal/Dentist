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
using System.Web.Http.OData;
using Dentist.Models;
using Dentist.Models.Patient;

namespace Dentist.Controllers
{
    public class NotesApiController : BaseApiController
    {
        // GET: api/NotesApi
        [EnableQuery]
        public IQueryable<Note> GetNotes()
        {

            return ReadContext.Notes;
        }

        // GET: api/NotesApi/5
        [ResponseType(typeof(Note))]
        public IHttpActionResult GetNote(int id)
        {
            Note note = ReadContext.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            return Ok(note);
        }

        // PUT: api/NotesApi/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutNote(int id, Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != note.Id)
            {
                return BadRequest();
            }

            WriteContext.Entry(note).State = EntityState.Modified;

            try
            {
                WriteContext.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NoteExists(id))
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

        // POST: api/NotesApi
        [ResponseType(typeof(Note))]
        public IHttpActionResult PostNote(Note note)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            WriteContext.Notes.Add(note);
            WriteContext.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = note.Id }, note);
        }

        // DELETE: api/NotesApi/5
        [ResponseType(typeof(Note))]
        public IHttpActionResult DeleteNote(int id)
        {
            Note note = WriteContext.Notes.Find(id);
            if (note == null)
            {
                return NotFound();
            }

            WriteContext.Notes.Remove(note);
            WriteContext.SaveChanges();

            return Ok(note);
        }

        private bool NoteExists(int id)
        {
            return ReadContext.Notes.Count(e => e.Id == id) > 0;
        }
    }
}