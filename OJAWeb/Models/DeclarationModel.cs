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
    public class DeclarationModel
    {
        public int ID { get; set; }
        public string Position_ID { get; set; }
        public string Depart_ID { get; set; }
        public string Position_Name { get; set; }
        public string DC_ID { get; set; }
        public int User_ID { get; set; }


        public List<DeclarationModel> getDataDeclare { get; set; }
    }

}