using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace OJAWeb.Models
{
    public class RegisterModel
    {
        public int ID { get; set; }
        public string User_Name { get; set; }
        public string User_Last_Name { get; set; }
        public string User_IC { get; set; }
        public string User_Email { get; set; }
        public string User_Password2 { get; set; }
        public string User_Permanent_Address { get; set; }
        public string User_Correspon_Address { get; set; }
        public string User_Location { get; set; }
        public string User_Phone { get; set; }
        public string User_Tel_Home { get; set; }

        public string Region_ID { get; set; }
        public List<RegisterModel> register { get; set; }

    }

}