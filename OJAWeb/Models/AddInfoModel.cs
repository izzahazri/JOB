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
    public class AddInfoModel
    {
        public int ID { get; set; }
        public string User_Profess1 { get; set; }
        public string User_Date_Registered1 { get; set; }
        public string Status_Member1 { get; set; }

        public string User_Profess2 { get; set; }
        public string User_Date_Registered2 { get; set; }
        public string Status_Member2 { get; set; }

        public string Name_Relative_Friend1 { get; set; }
        public string Name_Relative_Friend_Depart1 { get; set; }
        public string Name_Relative_Friend_Status1 { get; set; }

        public string Name_Relative_Friend2 { get; set; }
        public string Name_Relative_Friend_Depart2 { get; set; }
        public string Name_Relative_Friend_Status2 { get; set; }

        public string User_Pregnant { get; set; }
        public string User_Misconduct { get; set; }
        public string User_Convicted_Law { get; set; }
        public string User_Illness { get; set; }
        public string User_Bankcrupt { get; set; }

        public int User_ID { get; set; }

        public List<AddInfoModel> usersaddInfo { get; set; }
    }

}