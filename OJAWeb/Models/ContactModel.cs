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
    public class ContactModel
    {
        public int ID { get; set; }
        public string User_RName1 { get; set; }
        public string User_RPhone1 { get; set; }
        public string User_ROccu1 { get; set; }
        public string User_Known_Year1 { get; set; }
        public string User_RRelation1 { get; set; }
        public string User_RName2 { get; set; }
        public string User_RPhone2 { get; set; }
        public string User_ROccu2 { get; set; }
        public string User_Known_Year2 { get; set; }
        public string User_RRelation2 { get; set; }

        public string User_EName1 { get; set; }
        public string User_EPhone1 { get; set; }
        public string User_EAddress1 { get; set; }
        public string User_EOccu1 { get; set; }
        public string User_ERelation1 { get; set; }
        public string User_EName2 { get; set; }
        public string User_EPhone2 { get; set; }
        public string User_EAddress2 { get; set; }
        public string User_EOccu2 { get; set; }
        public string User_ERelation2 { get; set; }
        public int User_ID { get; set; }

        public List<ContactModel> usersContact { get; set; }
    }

}