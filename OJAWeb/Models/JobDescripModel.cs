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
    public class JobDescripModel
    {
        public string Position_ID { get; set; }
        public string User_Career_Level { get; set; }
        public string User_Years_Exp { get; set; }
        public string Depart_Name { get; set; }
        public string User_Qualification { get; set; }
        public string Job_Type { get; set; }
        public string Position_Title { get; set; }
        public string Location_Job { get; set; }

        [AllowHtml]
        public string Job_Description { get; set; }
        public string Created_Date { get; set; }
        public string Dep_Name { get; set; }
        public string Career_Name { get; set; }
        public string Type_Qualification { get; set; }
        public string Type_Job { get; set; }

        public List<JobDescripModel> JobDescription { get; set; }

        public IQueryable<SelectListItem> Job_List { get; set; }

    }

}