using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;

namespace OJAWeb.Models
{
    public class LoginModel
    {
        public string ID { get; set; }
        public int Profile_ID { get; set; }
        public string User_LoginID { get; set; }
        public string User_Email { get; set; }
        public string User_ShortName { get; set; }
        public string User_Name { get; set; }
        public string User_Password2 { get; set; }
        public string New_Password { get; set; }
        public string Confirm_Password { get; set; }
        public string Profile_Name { get; set; }
        public string Created_Date { get; set; }
        public string IsActive { get; set; }

        public List<LoginModel> loginmodel { get; set; }
        public List<SelectListItem> Profile_List { get; set; }

    }

}