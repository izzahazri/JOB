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
    public class ResumeListModel
    {
        public string ID { get; set; }
        public string User_Name { get; set; }
        public string User_Phone { get; set; }
        public string User_IC { get; set; }
        public string User_Email { get; set; }
        public string Uploaded_Resume { get; set; }
        public string User_Resume { get; set; }

        public List<ResumeListModel> resumelist { get; set; }
    }

}