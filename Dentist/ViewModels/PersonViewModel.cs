﻿using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Web;
using System.Xml.Linq;
using Dentist.Enums;
using Dentist.Models;
using Kendo.Mvc.Infrastructure.Implementation;
using File = Dentist.Models.File;

namespace Dentist.ViewModels
{
    public class PersonViewModel
    {
        private File _uploadedAvatarFile;
        public int Id { get; set; }

        public HttpPostedFileBase UploadedAvatar { get; set; }

        public File UploadedAvatarFile
        {
            get
            {
                if (UploadedAvatar != null && UploadedAvatar.ContentLength > 0 && _uploadedAvatarFile == null)
                {
                    _uploadedAvatarFile = new File
                    {
                        FileName = UploadedAvatar.FileName,
                        ContentType = UploadedAvatar.ContentType,
                        FileType = FileType.Avatar,
                        CreatedDateTime = DateTime.Now
                    };
                    using (var reader = new BinaryReader(UploadedAvatar.InputStream))
                    {
                        _uploadedAvatarFile.Content = reader.ReadBytes(UploadedAvatar.ContentLength);
                    }
                }
                return _uploadedAvatarFile;
            }
        }

        public int? AvatarId { get; set; }

        public Title Title { get; set; }

        [Display(Name = "Frist Name")]
        [Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [Required]
        public string LastName { get; set; }


        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "DOB")]
        [DisplayFormat(DataFormatString = "{0:dd-MM-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateOfBirth { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Phone { get; set; }

        public int AddressId { get; set; }

        public AddressViewModel Address { get; set; }
        public bool IsDeleted { get; set; }

        protected virtual void CopyTo(Person person)
        {
        }
        protected virtual void CopyFrom(Person person)
        {
            AvatarId = person.Files.Count > 0 ? person.Files[person.Files.Count - 1].Id : (int?)null;
        }
    }
}