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
    public class JobRegionListModel
    {
        public string Region_Name { get; set; }
        public string Company_Name { get; set; }
        public string DC_Code { get; set; }
        public string Dep_Name { get; set; }
        public string Position_Name { get; set; }
        public string Position_ID { get; set; }

        public List<JobRegionListModel> RegionList { get; set; }

    }

}