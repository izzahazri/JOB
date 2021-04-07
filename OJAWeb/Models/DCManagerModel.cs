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
    public class DCManagerModel
    {
        public string ID { get; set; }
        public string DC_Code { get; set; }
        public string Region_ID { get; set; }
        public string Region_Name { get; set; }
        public string Comp_ID { get; set; }
        public string Comp_Name { get; set; }
        public string IsActive { get; set; }
        public string Created_Date { get; set; }
        public string Remark { get; set; }
        public List<DCManagerModel> dcManager { get; set; }
    }

}