﻿using System;
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
using AutoMapper.QueryableExtensions;

namespace Dentist.Controllers
{
    public class PatientNotesApiController : BaseApiController
    {
        // GET: api/PatientNotesApi
        public IQueryable<PatientNoteDto> GetPatientNotes()
        {
            var query = ReadContext.Set<PatientNote>().Include(x => x.Notes).ProjectTo<PatientNoteDto>();
            return query;
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
        public IHttpActionResult PutPatientNote(PatientNoteDto patientNoteDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientNoteEnvelop = WriteContext.PatientNotes.FirstOrDefault(x => x.Id == patientNoteDto.Id);
            if (patientNoteEnvelop == null)
            {
                return NotFound();
            }
            patientNoteEnvelop.RecordedDate = DateTime.Now;

            // delete notes
            var deletedNotesDto = patientNoteDto.Notes.Where(noteDto => noteDto.ObjectState == "delete").ToList();
            deletedNotesDto.ForEach(noteDto =>
            {
                var note = new Note() { Id = noteDto.Id };
                WriteContext.Notes.Attach(note);
                WriteContext.Notes.Remove(note);
            });

            // add notes
            var addedNotesDto = patientNoteDto.Notes.Where(noteDto => noteDto.ObjectState == "add").ToList();
            addedNotesDto.ForEach(noteDto =>
            {
                var note = AutoMapper.Mapper.Map<Note>(noteDto);
                patientNoteEnvelop.Notes.Add(note);
                note.RecordedDate = DateTime.Today;
            });

            // update notes
            var updatedNotesDto = patientNoteDto.Notes.Where(noteDto => noteDto.ObjectState == "update").ToList();
            updatedNotesDto.ForEach(noteDto =>
            {
                var note = WriteContext.Notes.First(x => x.Id == noteDto.Id);
                AutoMapper.Mapper.Map(noteDto, note);
                note.RecordedDate = DateTime.Today;
            });

            WriteContext.TrySaveChanges(ModelState);
            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/PatientNotesApi
        [ResponseType(typeof(PatientNoteDto))]
        public IHttpActionResult PostPatientNote(PatientNoteDto patientNoteDto)
        {
            var isNewRecord = (patientNoteDto.Id == 0);
            if (!isNewRecord)
            {
                return BadRequest("Only new patient can be created using PostPatientNote call");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var patientNote = WriteContext.PatientNotes.Create();
            AutoMapper.Mapper.Map(patientNoteDto, patientNote);
            patientNote.RecordedDate = DateTime.Today;
            patientNote.Notes.ForEach(note => note.RecordedDate = DateTime.Today);

            WriteContext.PatientNotes.Add(patientNote);
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