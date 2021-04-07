﻿using System;
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
    public class DepartManagerModel
    {
        public string ID { get; set; }
        public string Dep_Name { get; set; }
        public string IsActive { get; set; }
        public string Created_Date { get; set; }
        public List<DepartManagerModel> departManager { get; set; }
    }

}