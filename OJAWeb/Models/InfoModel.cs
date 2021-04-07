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
    public class InfoModel
    {
        public int ID { get; set; }
        public string User_ID { get; set; }
        public string User_ShortName { get; set; }
        public string User_Name { get; set; }
        public string User_Last_Name { get; set; }
        public string User_Phone { get; set; }
        public string User_Tel_Home { get; set; }
        public string User_Profile { get; set; }
        public string User_IC { get; set; }
        public string User_Email { get; set; }
        public string User_Permanent_Address { get; set; }
        public string User_Location { get; set; }
        public string User_Correspon_Address { get; set; }
        public string User_Nationality { get; set; }
        public string User_Religion { get; set; }
        public string User_Race { get; set; }
        public string User_Gender { get; set; }
        public string User_Age { get; set; }
        public string User_Marital_Status { get; set; }
        public string User_Date_Birth { get; set; }
        public string User_Driving_License { get; set; }
        public string User_Driving_Class { get; set; }
        public string User_Expected_Salary { get; set; }
        public string User_Driving_Attach { get; set; }
        public HttpPostedFileBase DrivingLicense { get; set; }
        public string User_Resume { get; set; }
        public HttpPostedFileBase ResumeFile { get; set; }

        public List<InfoModel> usersinfo { get; set; }
    }

}