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
    public class ReportModel
    {
        public string DateFrom { get; set; }
        public string DateTo { get; set; }
        public string Dep_Name { get; set; }
        public string Position_Name { get; set; }
        public string Total { get; set; }
        public string New { get; set; }
        public string Review { get; set; }
        public string Call_Interview { get; set; }
        public string Interview { get; set; }
        public string Keep_In_View { get; set; }
        public string Accepted { get; set; }
        public string Rejected { get; set; }
        public string Withdrawn { get; set; }
        public List<ReportModel> reportinfo { get; set; }

    }

}