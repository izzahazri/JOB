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
    public class EducationModel
    {
        public int ID { get; set; }
        public string User_School1 { get; set; }
        public string User_Institute1 { get; set; }
        public string User_From_Year1 { get; set; }
        public string User_To_Year1 { get; set; }
        public string User_Qualification1 { get; set; }

        public string User_School2 { get; set; }
        public string User_Institute2 { get; set; }
        public string User_From_Year2 { get; set; }
        public string User_To_Year2 { get; set; }
        public string User_Qualification2 { get; set; }

        public string User_Cert_File { get; set; }
        public HttpPostedFileBase CertFile { get; set; }
        public int User_ID { get; set; }

        public List<EducationModel> userseducation { get; set; }
    }

}