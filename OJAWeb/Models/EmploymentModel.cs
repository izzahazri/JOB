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
    public class EmploymentModel
    {
        public int ID { get; set; }
        public string User_Company1 { get; set; }
        public string User_CompanyAddress1 { get; set; }
        public string User_Status_Employ1 { get; set; }
        public string User_From_Year1 { get; set; }
        public string User_To_Year1 { get; set; }
        public string User_LastPosition1 { get; set; }
        public string User_Reason1 { get; set; }

        public string User_Company2 { get; set; }
        public string User_CompanyAddress2 { get; set; }
        public string User_Status_Employ2 { get; set; }
        public string User_From_Year2 { get; set; }
        public string User_To_Year2 { get; set; }
        public string User_LastPosition2 { get; set; }
        public string User_Reason2 { get; set; }

        public string User_Company3 { get; set; }
        public string User_CompanyAddress3 { get; set; }
        public string User_Status_Employ3 { get; set; }
        public string User_From_Year3 { get; set; }
        public string User_To_Year3 { get; set; }
        public string User_LastPosition3 { get; set; }
        public string User_Reason3 { get; set; }

        public string User_Period { get; set; }
        public string User_Emp_Phone { get; set; }
        public int User_ID { get; set; }

        public List<EmploymentModel> usersemployment { get; set; }
    }

}