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
    public class JobManagerModel
    {
        public string Position_ID { get; set; }
        public string Position_Name { get; set; }
        public string DC_ID { get; set; }
        public string DC_Code { get; set; }
        public string Depart_ID { get; set; }
        public string Depart_Name { get; set; }
        public string Total_Vacancy { get; set; }
        public string IsOffer { get; set; }
        public string Created_Date { get; set; }

        public string Career_Name { get; set; }
        public string User_Years_Exp { get; set; }
        public string Location_Job { get; set; }
        public string Type_Qualification { get; set; }
        public string Type_Job { get; set; }

        [AllowHtml]
        public string Job_Description { get; set; }

        public List<JobManagerModel> jobManager { get; set; }
    }

}