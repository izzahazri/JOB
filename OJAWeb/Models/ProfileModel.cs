﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Configuration;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web.Mvc;

namespace OJAWeb.Models
{
    public class ProfileModel
    {
        public int ID { get; set; }
        public string Profile_Name { get; set; }
        public string IsActive { get; set; }

    }

}