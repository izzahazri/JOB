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
    public class JobApprovalModel
    {
        public string User_Name { get; set; }
        public string User_ID { get; set; }
        public string Job_ID { get; set; }
        public string User_Resume { get; set; }
        public string Dep_Name { get; set; }
        public string Depart_ID { get; set; }
        public string Created_Date { get; set; }
        public string Status_Application { get; set; }
        public string Status_Code { get; set; }
        public string Position_ID { get; set; }
        public string Position_Name { get; set; }
        public DateTime Interview_Date { get; set; }
        public string Interview_Time { get; set; }
        public string Interview_Venue { get; set; }


        public List<JobApprovalModel> jobapproval { get; set; }

    }

}