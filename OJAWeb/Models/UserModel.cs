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
    public class UserModel
    {
        public int ID { get; set; }
        public string User_Name { get; set; }
        public string User_ShortName { get; set; }
        public string User_LoginID { get; set; }
        public string User_Password2 { get; set; }
        public string Profile_ID { get; set; }

    }

}