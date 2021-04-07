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
    public class LinguisticModel
    {
        public int ID { get; set; }
        public string User_Language1 { get; set; }
        public string User_Spoken1 { get; set; }
        public string User_Writing1 { get; set; }

        public string User_Language2 { get; set; }
        public string User_Spoken2 { get; set; }
        public string User_Writing2 { get; set; }

        public string User_Language3 { get; set; }
        public string User_Spoken3 { get; set; }
        public string User_Writing3 { get; set; }

        public int User_ID { get; set; }

        public List<LinguisticModel> usersling { get; set; }
    }

}