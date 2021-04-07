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
    public class CredentialModel
    {
        public int ID { get; set; }
        public string User_LoginID { get; set; }
        public string User_Email { get; set; }
        public string User_Password2 { get; set; }
        public List<CredentialModel> usersacc { get; set; }

    }

}