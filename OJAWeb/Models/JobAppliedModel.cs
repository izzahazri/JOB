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
    public class JobAppliedModel
    {
        public int ID { get; set; }
        public string ID_User { get; set; }
        public string JobApp_ID { get; set; }
        public string User_Name { get; set; }
        public string Region_Name { get; set; }
        public string Depart_Name { get; set; }
        public string DC_Code { get; set; }
        public string Position_Title { get; set; }
        public string Status_Application { get; set; }
        public string Status_Code { get; set; }
        public string Created_Date { get; set; }
        public string Withdrawn_Date { get; set; }
        public string Withdrawn_By { get; set; }
        public string Position_ID { get; set; }
        public string User_ID { get; set; }
        public string Job_ID { get; set; }
        public string Remark_Withdrawn { get; set; }
        public string Interview_Date { get; set; }
        public string Interview_Time { get; set; }
        public string Interview_Venue { get; set; }

        public List<JobAppliedModel> jobapplied { get; set; }
    }

}