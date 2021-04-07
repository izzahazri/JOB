using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;

namespace OJAWeb.Models
{
    public class ResumeModel
    {
        public int ID { get; set; }
        public string User_Resume { get; set; }
        public string Uploaded_Resume { get; set; }

        public HttpPostedFileBase ResumeFile { get; set; }
        public int User_ID { get; set; }
        public string Created_Date { get; set; }
        public List<ResumeModel> usersresume { get; set; }
    }

}