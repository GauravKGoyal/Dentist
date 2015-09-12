using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dentist.Controllers
{
    public class FileController : BaseController
    {
        // GET: File
        public ActionResult Get(int id)
        {
            if (id == 0)
            {
                return null;
            }

            var fileToRetrieve = ReadContext.Files.Find(id);
            return File(fileToRetrieve.Content, fileToRetrieve.ContentType);
        }
    }
}