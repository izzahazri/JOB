using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using OJAWeb.Models;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Security;
using System.Data;
using System.Web.UI.WebControls;
using System.Globalization;
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using Antlr.Runtime.Tree;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace OJAWeb.Controllers
{
    public class SuperadminController : Controller
    {
        [HttpGet]
        public ActionResult ExtendSession()
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        // GET: Superadmin
        public ActionResult Index()
        {
            string userID = Session["ID"].ToString();
            //string userShortName = Session["User_ShortName"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.Status_Application = '1' and JA.IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalProgress = reader["TotalOrders"].ToString();
                        ViewBag.toTalProgress = toTalProgress;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.Status_Application = '2' and JA.IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalReviewed = reader["TotalOrders"].ToString();
                        ViewBag.toTalReviewed = toTalReviewed;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.Status_Application = '3' and JA.IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalCallforIn = reader["TotalOrders"].ToString();
                        ViewBag.toTalCallforIn = toTalCallforIn;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.Status_Application = '4' and JA.IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalInterviewed = reader["TotalOrders"].ToString();
                        ViewBag.toTalInterviewed = toTalInterviewed;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.Status_Application = '5' and JA.IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalKeepInView = reader["TotalOrders"].ToString();
                        ViewBag.toTalKeepInView = toTalKeepInView;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.Status_Application = '6' and JA.IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalRejected = reader["TotalOrders"].ToString();
                        ViewBag.toTalRejected = toTalRejected;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = " SELECT COUNT(JA.Status_Application) TotalOrders FROM TblJob_Application JA WHERE JA.IsActive=0";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string toTalWithdraw = reader["TotalOrders"].ToString();
                        ViewBag.toTalWithdraw = toTalWithdraw;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "sp_job_list_view";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();

                    List<DataModel> reportlist = new List<DataModel>();

                    while (reader.Read())
                    {
                        string Position_Name = reader["Position_Name"].ToString();
                        double Total = Convert.ToDouble(reader["Total_Applicant"]);

                        DataModel uobj = new DataModel(Position_Name, Total);

                        reportlist.Add(uobj);
                    }

                    ViewBag.DataPoints = JsonConvert.SerializeObject(reportlist);
                    con.Close();
                }
            }

            //List<DataModel> dataPoints = new List<DataModel>();

            //dataPoints.Add(new DataModel("Samsung", 25));
            //dataPoints.Add(new DataModel("Micromax", 13));
            //dataPoints.Add(new DataModel("Lenovo", 8));
            //dataPoints.Add(new DataModel("Intex", 7));
            //dataPoints.Add(new DataModel("Reliance", 6.8));
            //dataPoints.Add(new DataModel("Others", 40.2));

            //ViewBag.DataPoints = JsonConvert.SerializeObject(dataPoints);

            return View();
        }

        [HttpPost]
        public JsonResult AjaxMethod()
        {
            List<object> chartData = new List<object>();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            chartData.Add(new object[]
                        {
                            "ShipCity", "TotalOrders"
                        });
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT Dep_Name, COUNT(JA.Depart_ID) TotalOrders FROM TblJob_Application JA LEFT JOIN TblMaster_Department MP ON MP.ID = JA.Depart_ID GROUP BY Dep_Name";

                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();

                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            chartData.Add(new object[]
                            {
                            sdr["Dep_Name"], sdr["TotalOrders"]
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Json(chartData);
        }

        // GET: Home
        public ActionResult JobRegion()
        {
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            RegionModel DropdownRegion = new RegionModel
            {
                Region_List = PopulateRegion()
            };

            return View(DropdownRegion);
        }

        [HttpPost]
        public ActionResult JobRegionList(RegionModel DropdownRegion)
        {
            DropdownRegion.Region_List = PopulateRegion();
            var selectedItem = DropdownRegion.Region_List.Find(p => p.Value == DropdownRegion.ID.ToString());
            var Region_ID = DropdownRegion.ID.ToString();

            List<JobRegionListModel> listJob = new List<JobRegionListModel>();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                //string query = "select Region_Name, Company_Name, DC_Code, Dep_Name from TblMaster_DC D LEFT JOIN TblMaster_Region R ON D.Region_ID = R.ID LEFT JOIN TblDCCompany_List DCD ON D.ID =  DCD.DC_ID LEFT JOIN TblMaster_Department DE ON DE.ID =  DCD.Depart_ID LEFT JOIN TBLMaster_Company MC ON MC.ID = DCD.Comp_ID WHERE D.Region_ID=@ID order by DC_Code asc";
                string query = "select Position_ID,Region_Name, Company_Name, DC_Code, Dep_Name, Position_Name from TblMaster_Position P LEFT JOIN TblMaster_DC D  ON D.ID = P.DC_ID LEFT JOIN  TblMaster_Department DE ON DE.ID = P.Depart_ID LEFT JOIN TblMaster_Region R ON D.Region_ID = R.ID LEFT JOIN TBLMaster_Company MC ON D.Comp_ID = MC.ID where P.IsOffer = 1 and D.Region_ID=@ID and P.Total_Vacancy >=1 order by Dep_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    cmd.Parameters.AddWithValue("@ID", Region_ID);
                    con.Open();
                    reader = cmd.ExecuteReader();

                    if (selectedItem != null)
                    {
                        while (reader.Read())
                        {
                            selectedItem.Selected = true;
                            JobRegionListModel jobRegionListModel = new JobRegionListModel
                            {
                                Region_Name = reader["Region_Name"].ToString(),
                                Company_Name = reader["Company_Name"].ToString(),
                                DC_Code = reader["DC_Code"].ToString(),
                                Dep_Name = reader["Dep_Name"].ToString(),
                                Position_Name = reader["Position_Name"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                            };
                            listJob.Add(jobRegionListModel);
                        }
                    }
                    con.Close();
                }
            }
            return View(listJob);
        }

        private static List<SelectListItem> PopulateRegion()
        {
            List<SelectListItem> region = new List<SelectListItem>();
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "Select DISTINCT R.ID, R.Region_Name from TblMaster_Region R LEFT JOIN TblMaster_DC DC ON R.ID = DC.Region_ID LEFT JOIN TblMaster_Position P ON DC.ID = P.DC_ID where P.IsOffer = 1 and P.Total_Vacancy >=1 order by Region_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            region.Add(new SelectListItem
                            {
                                Text = sdr["Region_Name"].ToString(),
                                Value = sdr["ID"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return region;
        }

        public ActionResult JobDescription(string id)
        {
            var Position_ID = id;

            JobDescripModel jobDesc = new JobDescripModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblJob_Description WHERE Position_ID='" + Position_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    List<JobDescripModel> userlist = new List<JobDescripModel>();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(Position_ID)))
                        {
                            JobDescripModel uobj = new JobDescripModel
                            {
                                User_Career_Level = reader["User_Career_Level"].ToString(),
                                User_Years_Exp = reader["User_Years_Exp"].ToString(),
                                Depart_Name = reader["Depart_Name"].ToString(),
                                User_Qualification = reader["User_Qualification"].ToString(),
                                Job_Type = reader["Job_Type"].ToString(),
                                Position_Title = reader["Position_Title"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                                Location_Job = reader["Location_Job"].ToString(),
                                Job_Description = reader["Job_Description"].ToString(),
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            userlist.Add(uobj);
                        }
                        jobDesc.JobDescription = userlist;
                    }
                    else
                    {
                        return RedirectToAction("EditDescription", "Superadmin");
                    }
                    con.Close();
                }
            }
            return View(jobDesc);
        }

        public ActionResult EditDescription(string id)
        {
            var Job_ID = id;

            string Depart_ID = "";
            string Career_ID = "";
            string Qua_ID = "";
            string JobType_ID = "";

            JobDescripModel jobDesc = new JobDescripModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT *, md.ID as Depart_ID, mc.ID as Career_ID, mq.ID as Qua_ID, mj.ID as JobType_ID FROM TblJob_Description jd LEFT JOIN TblMaster_Department md on md.Dep_Name = jd.Depart_Name LEFT JOIN Tblmaster_career mc on mc.Career_Name = jd.User_Career_Level LEFT JOIN TblMaster_Qualification mq on mq.Type_Qualification = jd.User_Qualification LEFT JOIN TblMaster_JobType mj on mj.Type_Job = jd.Job_Type WHERE jd.Position_ID = '" + Job_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    List<JobDescripModel> userlist = new List<JobDescripModel>();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(Job_ID)))
                        {
                            Depart_ID = reader["Depart_ID"].ToString();
                            Career_ID = reader["Career_ID"].ToString();
                            Qua_ID = reader["Qua_ID"].ToString();
                            JobType_ID = reader["JobType_ID"].ToString();

                            JobDescripModel uobj = new JobDescripModel
                            {
                                User_Career_Level = reader["User_Career_Level"].ToString(),
                                User_Years_Exp = reader["User_Years_Exp"].ToString(),
                                Depart_Name = reader["Depart_Name"].ToString(),
                                User_Qualification = reader["User_Qualification"].ToString(),
                                Job_Type = reader["Job_Type"].ToString(),
                                Position_Title = reader["Position_Title"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                                Location_Job = reader["Location_Job"].ToString(),
                                Job_Description = reader["Job_Description"].ToString(),
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            userlist.Add(uobj);
                        }
                        jobDesc.JobDescription = userlist;
                    }
                    else
                    {
                        JobDescripModel uobj = new JobDescripModel
                        {
                            User_Career_Level = null,
                            User_Years_Exp = null,
                            Depart_Name = null,
                            User_Qualification = null,
                            Job_Type = null,
                            Position_Title = null,
                            Position_ID = null,
                            Location_Job = null,
                            Job_Description = null
                        };
                        userlist.Add(uobj);
                    }
                    jobDesc.JobDescription = userlist;
                    con.Close();
                }
            }

            List<SelectListItem> department = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Dep_Name FROM TblMaster_Department WHERE IsActive=1 order by Dep_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            department.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Dep_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.DepList = department;
            ViewBag.SelectedDepartID = Depart_ID;

            List<SelectListItem> qua = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Type_Qualification FROM TblMaster_Qualification";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            qua.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Qualification"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.QuaList = qua;
            ViewBag.SelectedQuaID = Qua_ID;

            List<SelectListItem> career = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Career_Name FROM TblMaster_Career";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            career.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Career_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.CareerList = career;
            ViewBag.SelectedCareerID = Career_ID;

            List<SelectListItem> jobtype = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Type_Job FROM TblMaster_JobType";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            jobtype.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Job"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.TypeJobList = jobtype;
            ViewBag.SelectedJobID = JobType_ID;

            return View(jobDesc);
        }

        public ActionResult UpdateJobDescrip(JobDescripModel updateJobDescrip)
        {
            var Dep_Name = "";
            var Career_Name = "";
            var Type_Qualification = "";
            var Type_Job = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Dep_Name FROM TblMaster_Department WHERE ID='" + updateJobDescrip.Dep_Name + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Dep_Name = reader["Dep_Name"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Career_Name FROM TblMaster_Career WHERE ID ='" + updateJobDescrip.Career_Name + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Career_Name = reader["Career_Name"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Type_Qualification FROM TblMaster_Qualification WHERE ID ='" + updateJobDescrip.Type_Qualification + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Type_Qualification = reader["Type_Qualification"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Type_Job FROM TblMaster_JobType WHERE ID ='" + updateJobDescrip.Type_Job + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Type_Job = reader["Type_Job"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                string query3 = "UPDATE TblJob_Description SET User_Career_Level ='" + Career_Name + "', User_Years_Exp ='" + updateJobDescrip.User_Years_Exp + "',Depart_Name ='" + Dep_Name + "',Location_Job ='" + updateJobDescrip.Location_Job + "',User_Qualification ='" + Type_Qualification + "',Job_Type ='" + Type_Job + "',Job_Description ='" + updateJobDescrip.Job_Description + "' WHERE Position_ID='" + updateJobDescrip.Position_ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }
                con1.Close();
            }
            return RedirectToAction("JobManager");
        }

        public ActionResult JobApp()
        {
            string userID = Session["ID"].ToString();

            JobAppliedModel applyinfo = new JobAppliedModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT JA.ID as Job_ID,JA.User_ID, User_Name, MR.Region_Name,Dep_Name, DC_Code, Position_Name, SS.Status_Code,JA.Created_Date,JA.Position_ID from TblJob_Application JA LEFT JOIN TblMaster_Position P ON JA.Position_ID = P.Position_ID LEFT JOIN TblMaster_DC MD ON P.DC_ID = MD.ID LEFT JOIN TblMaster_Region MR ON MD.Region_ID = MR.ID LEFT JOIN TblMaster_Department MP ON P.Depart_ID = MP.ID LEFT JOIN TblSystem_Status SS ON SS.ID = JA.Status_Application WHERE JA.IsActive=1 ORDER BY JA.CREATED_DATE DESC";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<JobAppliedModel> userapplied = new List<JobAppliedModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            JobAppliedModel uobj = new JobAppliedModel
                            {
                                Job_ID = reader["Job_ID"].ToString(),
                                User_ID = reader["User_ID"].ToString(),
                                User_Name = reader["User_Name"].ToString(),
                                Region_Name = reader["Region_Name"].ToString(),
                                Depart_Name = reader["Dep_Name"].ToString(),
                                DC_Code = reader["DC_Code"].ToString(),
                                Position_Title = reader["Position_Name"].ToString(),
                                Status_Application = reader["Status_Code"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            userapplied.Add(uobj);
                        }
                    }
                    applyinfo.jobapplied = userapplied;
                    con.Close();
                }
            }
            return View(applyinfo);
        }

        public ActionResult JobWithdrawn()
        {
            string userID = Session["ID"].ToString();

            JobAppliedModel applyinfo = new JobAppliedModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT JA.ID as Job_ID,JA.User_ID, User_Name, MR.Region_Name,Dep_Name, DC_Code, Position_Name, SS.Status_Code,JA.Created_Date,JA.Position_ID, JA.Remark_Withdrawn, JA.Withdrawn_Date,JA.Withdrawn_By, JA.User_ID as ID_User from TblJob_Application JA LEFT JOIN TblMaster_Position P ON JA.Position_ID = P.Position_ID LEFT JOIN TblMaster_DC MD ON P.DC_ID = MD.ID LEFT JOIN TblMaster_Region MR ON MD.Region_ID = MR.ID LEFT JOIN TblMaster_Department MP ON P.Depart_ID = MP.ID LEFT JOIN TblSystem_Status SS ON SS.ID = JA.Status_Application WHERE JA.IsActive=0 ORDER BY JA.Withdrawn_Date DESC";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<JobAppliedModel> userapplied = new List<JobAppliedModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            JobAppliedModel uobj = new JobAppliedModel
                            {
                                ID_User = reader["ID_User"].ToString(),
                                Job_ID = reader["Job_ID"].ToString(),
                                User_ID = reader["User_ID"].ToString(),
                                User_Name = reader["User_Name"].ToString(),
                                Region_Name = reader["Region_Name"].ToString(),
                                Depart_Name = reader["Dep_Name"].ToString(),
                                DC_Code = reader["DC_Code"].ToString(),
                                Position_Title = reader["Position_Name"].ToString(),
                                Status_Application = reader["Status_Code"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                                Created_Date = reader["Created_Date"].ToString(),
                                Withdrawn_Date = reader["Withdrawn_Date"].ToString(),
                                Withdrawn_By = reader["Withdrawn_By"].ToString(),
                                Remark_Withdrawn = reader["Remark_Withdrawn"].ToString()
                            };
                            userapplied.Add(uobj);
                        }
                    }
                    applyinfo.jobapplied = userapplied;
                    con.Close();
                }
            }
            return View(applyinfo);
        }

        public ActionResult ViewJob(string id)
        {
            var Job_ID = id;
            var System_ID = "";

            JobApprovalModel jobapprove = new JobApprovalModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT JA.ID as Job_ID,UL.User_Name,User_Resume,Dep_Name,JA.Created_Date,Status_Application,JA.Position_ID,MP.Position_NAME,JA.User_ID,SS.ID AS System_ID, JA.Status_Application as Status_Code, JA.Interview_Date as Interview_Date, JA.Interview_Time as Interview_Time, JA.Interview_Venue as Interview_Venue from TblJob_Application JA LEFT JOIN TblMaster_Department MD ON MD.ID = JA.Depart_ID LEFT JOIN TblUser_Login UL ON UL.ID = JA.User_ID LEFT JOIN TblResume R ON R.User_ID = JA.User_ID   LEFT JOIN TblMaster_Position MP ON MP.Position_ID = JA.Position_ID LEFT JOIN TblSystem_Status SS ON SS.ID = JA.Status_Application WHERE JA.ID ='" + Job_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    System_ID = reader["System_ID"].ToString();

                    List<JobApprovalModel> userjob = new List<JobApprovalModel>();
                    if (!(String.IsNullOrEmpty(Job_ID)))
                    {
                        string s;
                        if (reader["User_Resume"].ToString() == "")
                        {
                            s = "NO FILE";
                        }
                        else
                        {
                            s = reader["User_Resume"].ToString();
                        }

                        var date = reader["Interview_Date"].ToString();
                        var datedt = DateTime.MinValue;

                        if (!string.IsNullOrEmpty(date) && !string.IsNullOrWhiteSpace(date))
                        {
                            datedt = Convert.ToDateTime(date);
                        }

                        JobApprovalModel uobj = new JobApprovalModel
                        {
                            Job_ID = reader["Job_ID"].ToString(),
                            User_Name = reader["User_Name"].ToString(),
                            User_Resume = s,
                            Dep_Name = reader["Dep_Name"].ToString(),
                            Created_Date = reader["Created_Date"].ToString(),
                            Status_Application = reader["Status_Application"].ToString(),
                            Position_ID = reader["Position_ID"].ToString(),
                            Position_Name = reader["Position_Name"].ToString(),
                            User_ID = reader["User_ID"].ToString(),
                            Status_Code = reader["Status_Code"].ToString() ?? null,
                            Interview_Date = datedt,
                            Interview_Time = reader["Interview_Time"].ToString() ?? null,
                            Interview_Venue = reader["Interview_Venue"].ToString() ?? null
                        };
                        userjob.Add(uobj);
                    }
                    jobapprove.jobapproval = userjob;

                    ViewBag.status = jobapprove.jobapproval.Select(x => x.Status_Application).FirstOrDefault();

                    con.Close();
                }
            }

            List<SelectListItem> Status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Status_Code FROM TblSystem_Status WHERE IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            Status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status_Code"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.SystemList = Status;
            ViewBag.SelectedSystemID = System_ID;

            return View(jobapprove);
        }

        public ActionResult ConfirmDelete(string id, string id2)
        {
            var Position_ID = id;
            var User_Name = id2;
            string fullname = "";
            string withdrawn_date = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        fullname = reader["User_Name"].ToString();
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE TblJob_Application SET Withdrawn_By='" + fullname + "',Withdrawn_Date ='" + withdrawn_date + "', IsActive = 0 WHERE User_Name='" + User_Name + "' and Position_ID ='" + Position_ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query))
                {
                    con.Open();
                    cmd1.Connection = con;
                    cmd1.ExecuteNonQuery();
                }
                con.Close();
            }
            return RedirectToAction("JobApp");
        }

        public ActionResult ListJob(string id)
        {
            var Status_Application = id;
            JobAppliedModel applyinfo = new JobAppliedModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT JA.ID AS Job_ID,JA.User_ID, User_Name, MR.Region_Name,Dep_Name, DC_Code, Position_Name, SS.Status_Code,JA.Created_Date,JA.Position_ID from TblJob_Application JA LEFT JOIN TblMaster_Position P ON JA.Position_ID = P.Position_ID LEFT JOIN TblMaster_DC MD ON P.DC_ID = MD.ID LEFT JOIN TblMaster_Region MR ON MD.Region_ID = MR.ID LEFT JOIN TblMaster_Department MP ON P.Depart_ID = MP.ID LEFT JOIN TblSystem_Status SS ON SS.ID=JA.Status_Application WHERE JA.IsActive=1 and Status_Application='" + Status_Application + "' ORDER BY JA.CREATED_DATE DESC";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<JobAppliedModel> userapplied = new List<JobAppliedModel>();
                    if (!(String.IsNullOrEmpty(id)))
                    {
                        while (reader.Read())
                        {
                            JobAppliedModel uobj = new JobAppliedModel
                            {
                                User_ID = reader["User_ID"].ToString(),
                                User_Name = reader["User_Name"].ToString(),
                                Job_ID = reader["Job_ID"].ToString(),
                                Region_Name = reader["Region_Name"].ToString(),
                                Depart_Name = reader["Dep_Name"].ToString(),
                                DC_Code = reader["DC_Code"].ToString(),
                                Position_Title = reader["Position_Name"].ToString(),
                                Status_Application = reader["Status_Code"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            userapplied.Add(uobj);
                        }
                    }
                    applyinfo.jobapplied = userapplied;
                    con.Close();
                }
            }
            return View(applyinfo);
        }

        public ActionResult ListsJob(string id)
        {
            var Status_Application = id;
            JobAppliedModel applyinfo = new JobAppliedModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT JA.Interview_Date, JA.Interview_Time, JA.Interview_Venue,JA.User_ID, JA.ID AS Job_ID, User_Name, MR.Region_Name,Dep_Name, DC_Code, Position_Name, SS.Status_Code,JA.Created_Date,JA.Position_ID from TblJob_Application JA LEFT JOIN TblMaster_Position P ON JA.Position_ID = P.Position_ID LEFT JOIN TblMaster_DC MD ON P.DC_ID = MD.ID LEFT JOIN TblMaster_Region MR ON MD.Region_ID = MR.ID LEFT JOIN TblMaster_Department MP ON P.Depart_ID = MP.ID LEFT JOIN TblSystem_Status SS ON SS.ID=JA.Status_Application WHERE JA.IsActive=1 and Status_Application='" + Status_Application + "' ORDER BY JA.CREATED_DATE DESC";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<JobAppliedModel> userapplied = new List<JobAppliedModel>();
                    if (!(String.IsNullOrEmpty(id)))
                    {
                        while (reader.Read())
                        {
                            JobAppliedModel uobj = new JobAppliedModel
                            {
                                User_ID = reader["User_ID"].ToString(),
                                User_Name = reader["User_Name"].ToString(),
                                Job_ID = reader["Job_ID"].ToString(),
                                Region_Name = reader["Region_Name"].ToString(),
                                Depart_Name = reader["Dep_Name"].ToString(),
                                DC_Code = reader["DC_Code"].ToString(),
                                Position_Title = reader["Position_Name"].ToString(),
                                Status_Application = reader["Status_Code"].ToString(),
                                Position_ID = reader["Position_ID"].ToString(),
                                Created_Date = reader["Created_Date"].ToString(),
                                Interview_Date = reader["Interview_Date"].ToString(),
                                Interview_Time = reader["Interview_Time"].ToString(),
                                Interview_Venue = reader["Interview_Venue"].ToString()
                            };
                            userapplied.Add(uobj);
                        }
                    }
                    applyinfo.jobapplied = userapplied;
                    con.Close();
                }
            }
            return View(applyinfo);
        }

        public ActionResult ResumeList()
        {
            string userID = Session["ID"].ToString();

            ResumeListModel resume = new ResumeListModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT User_Name,User_IC,User_Phone, User_Email,Uploaded_Resume,User_Resume,R.IsActive,UL.ID from TblUser_Login UL LEFT JOIN TblResume R ON R.User_ID = UL.ID WHERE R.IsActive=1 order by R.created_date desc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<ResumeListModel> resumesubmitted = new List<ResumeListModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            ResumeListModel uobj = new ResumeListModel
                            {
                                ID = reader["ID"].ToString(),
                                User_Name = reader["User_Name"].ToString(),
                                User_IC = reader["User_IC"].ToString(),
                                User_Phone = reader["User_Phone"].ToString(),
                                User_Email = reader["User_Email"].ToString(),
                                Uploaded_Resume = reader["Uploaded_Resume"].ToString(),
                                User_Resume = reader["User_Resume"].ToString()
                            };
                            resumesubmitted.Add(uobj);
                        }
                    }
                    resume.resumelist = resumesubmitted;
                    con.Close();
                }
            }
            return View(resume);
        }

        public ActionResult ConfirmDeleteResumeList(string id)
        {
            var ID = id;

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE TblResume SET IsActive = 0 WHERE User_ID='" + ID + "' and IsActive =1";
                using (SqlCommand cmd1 = new SqlCommand(query))
                {
                    con.Open();
                    cmd1.Connection = con;
                    cmd1.ExecuteNonQuery();
                }
                con.Close();
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE TblUser_Login SET IsActive = 0 WHERE ID='" + ID + "' and IsActive =1";
                using (SqlCommand cmd1 = new SqlCommand(query))
                {
                    con.Open();
                    cmd1.Connection = con;
                    cmd1.ExecuteNonQuery();
                }
                con.Close();
            }
            return RedirectToAction("ResumeList");
        }

        // DISPLAY
        // DISPLAY
        // DISPLAY
        [HttpGet]
        public ActionResult Credential()
        {
            string userID = Session["ID"].ToString();

            CredentialModel infoacc = new CredentialModel();
            List<CredentialModel> acclist = new List<CredentialModel>();


            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    string userShortName = reader["User_ShortName"].ToString();
                    ViewBag.User_ShortName = userShortName;

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            CredentialModel uobj = new CredentialModel();
                            uobj.User_LoginID = reader["User_LoginID"].ToString();
                            uobj.User_Password2 = Decrypt(reader["User_Password2"].ToString());
                            acclist.Add(uobj);
                        }
                        infoacc.usersacc = acclist;
                    }
                    else
                    {
                        CredentialModel uobj = new CredentialModel
                        {
                            User_LoginID = null,
                            User_Password2 = null
                        };
                        acclist.Add(uobj);
                    }
                    infoacc.usersacc = acclist;
                    con.Close();
                }
            }
            return View(infoacc);
        }

        [HttpPost]
        public ActionResult Credential(CredentialModel account)
        {
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query3 = "UPDATE TblUser_Login SET User_LoginID = '" + account.User_LoginID + "', User_Email = '" + account.User_LoginID + "', User_Password2 = '" + Encrypt(account.User_Password2) + "' WHERE ID='" + userID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    con1.Open();
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult SubmitStatus(JobApprovalModel approvalstatus)
        {
            string User_Name = "";
            string User_Email = "";
            string subject = "JOB APPLICATION ABX EXPRESS";
            string Position_Name = "";
            string Dep_Name = "";
            string dateInterview = "";
            string newtimeInterview = "";


            if (approvalstatus.Status_Code == "4")
            {
                dateInterview = approvalstatus.Interview_Date.Value.ToString("dd/MM/yyyy", new CultureInfo("en-US")) ?? null;
                int hourInt = 0;
                string ampm = String.Empty;
                string minutes = String.Empty;

                string timeinterview = approvalstatus.Interview_Time;

                string hours = timeinterview.Substring(0, 2);
                if (hours == "00")
                {
                    hours = "12";
                }

                hourInt = int.Parse(hours);


                //int minutesInt = int.Parse(minutes);

                minutes = timeinterview.Substring(3, 2);

                if (hourInt > 12)
                {
                    ampm = "PM";
                    hourInt = hourInt - 12;
                }
                else
                {
                    ampm = "AM";
                }

                newtimeInterview = hourInt + ":" + minutes + " " + ampm;
            }

            //string message = "kk";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + approvalstatus.User_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(approvalstatus.Status_Code)))
                    {
                        User_Name = reader["User_Name"].ToString();
                        User_Email = reader["User_Email"].ToString();
                    }
                    con1.Close();
                }
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string commandText = "SELECT MP.Position_Name,MD.Dep_Name from TblMaster_Position MP LEFT JOIN TblJob_Application JA ON JA.Position_ID = MP.Position_ID LEFT JOIN TblMaster_Department MD ON MD.ID = MP.Depart_ID WHERE JA.ID='" + approvalstatus.Job_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(approvalstatus.Status_Code)))
                    {
                        Position_Name = reader["Position_Name"].ToString();
                        Dep_Name = reader["Dep_Name"].ToString();
                        //User_Email = reader["User_Email"].ToString();
                    }
                    con1.Close();
                }
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                //string query3 = "UPDATE TblJob_Application SET Status_Application ='" + approvalstatus.Status_Code + "' WHERE User_ID='" + approvalstatus.User_ID + "' and Position_ID ='" + approvalstatus.Position_ID + "' and IsActive=1 ";
                string query3 = "UPDATE TblJob_Application SET Status_Application ='" + approvalstatus.Status_Code + "' WHERE ID='" + approvalstatus.Job_ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }

            if (approvalstatus.Status_Code == "7")
            {
                using (SqlConnection con1 = new SqlConnection(cs))
                {
                    con1.Open();

                    string query3 = "UPDATE TblMaster_Position SET Total_Vacancy = Total_Vacancy - 1 WHERE Position_ID ='" + approvalstatus.Position_ID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }


                    con1.Close();
                }
            }
            else if (approvalstatus.Status_Code == "4")
            {
                using (SqlConnection con1 = new SqlConnection(cs))
                {
                    con1.Open();

                    string query3 = "UPDATE TblJob_Application SET Interview_Date ='" + dateInterview + "',Interview_Time ='" + newtimeInterview + "',Interview_Venue ='" + approvalstatus.Interview_Venue + "' WHERE ID='" + approvalstatus.Job_ID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }


                    con1.Close();
                }
            }

            if (approvalstatus.Status_Code == "4")
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("recruitment@abxexpress.com.my", "HR & Admin Department");
                        var receiverEmail = new MailAddress(User_Email, User_Email);
                        var password = "Abc123";

                        var sub = subject;

                        var body = "Dear " + User_Name + ",<br /><br />";
                        //body += "<p style=\"border-style:outset;text-align:center;padding-top:10px;padding-bottom:10px\">";
                        //body += "<b> Applicant Name </b><br /> " + User_Name + " <br /><br />";
                        //body += "<b> Department </b><br /> " + Depart_ID + " <br /><br />";
                        //body += "<b> Position </b><br /> " + approvalstatus.Position_Name + " <br /><br />";
                        //body += "<b> Status Application </b><br /> " + approvalstatus.Status_Code + " <br /><br />";
                        //body += "</p>";
                        body += "Thank you for your interest in joining ABX Express (M) Sdn Bhd. ";
                        body += "We appreciate the time you have taken to apply for the position of <b>" + Position_Name + "</b> (<b>" + Dep_Name + "</b>).<br /><br />";
                        body += "After reviewing your profile, we would like to extend an interview invitation so that we can further discuss on";
                        body += " the above opportunity. The details of interview as follow :<br /><br />";
                        body += "Date of interview : <b>" + dateInterview + "</b><br />";
                        body += "Time of interview : <b>" + newtimeInterview + "</b><br />";
                        body += "Venue : <b>" + approvalstatus.Interview_Venue + "</b><br /><br />";
                        body += "We look forward to hearing from you.<br /><br /><br />";
                        body += "Yours sincerely,<br />ABX Express (M) Sdn Bhd<br />HR & Admin Department";

                        var smtp = new SmtpClient
                        {
                            Host = "mail.abxexpress.com.my",
                            Port = 587,
                            EnableSsl = true,
                            //DeliveryMethod = SmtpDeliveryMethod.Network,
                            //UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        })
                        {
                            smtp.Send(mess);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Some Error";
                }
            }
            else if (approvalstatus.Status_Code == "6")
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        var senderEmail = new MailAddress("recruitment@abxexpress.com.my", "HR & Admin Department");
                        var receiverEmail = new MailAddress(User_Email, User_Email);
                        var password = "Abc123";

                        var sub = subject;

                        var body = "Dear " + User_Name + ",<br /><br />";
                        body += "Thank you for your interest in applying for the position of <b>" + Position_Name + "</b> (<b>" + Dep_Name + "</b>).<br /><br />";
                        body += "Your interest in this position is most appreciated. While your working experience and skills are impressive,";
                        body += " due to high volume of competition of other potential applications, we regret to inform you that your job application";
                        body += " not been successful.<br /><br />";
                        body += "However, we well keep your resume in view for near future opportunity. We will be in touch with you";
                        body += " for further arrangements for any suitable openings.<br /><br />";
                        body += "Once again, we thank you for your interest.<br /><br /><br />";
                        body += "Yours sincerely,<br />ABX Express (M) Sdn Bhd<br />HR & Admin Department";

                        var smtp = new SmtpClient
                        {
                            Host = "mail.abxexpress.com.my",
                            Port = 587,
                            EnableSsl = true,
                            //DeliveryMethod = SmtpDeliveryMethod.Network,
                            //UseDefaultCredentials = false,
                            Credentials = new NetworkCredential(senderEmail.Address, password)
                        };
                        using (var mess = new MailMessage(senderEmail, receiverEmail)
                        {
                            Subject = subject,
                            Body = body,
                            IsBodyHtml = true
                        })
                        {
                            smtp.Send(mess);
                        }
                    }
                }
                catch (Exception ex)
                {
                    ViewBag.Error = "Some Error";
                }
            }
            return RedirectToAction("JobApp");
        }

        public ActionResult UserManager()
        {
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }
            return View();
        }

        public ActionResult CareerManager()
        {
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }
            return View();
        }

        public ActionResult UserProfile()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            LoginModel loginlist = new LoginModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Profile ORDER BY Profile_Name asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<LoginModel> login = new List<LoginModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            LoginModel uobj = new LoginModel
                            {
                                ID = reader["ID"].ToString(),
                                Profile_Name = reader["Profile_Name"].ToString(),
                                IsActive = s,
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            login.Add(uobj);
                        }
                    }
                    loginlist.loginmodel = login;
                    con.Close();
                }
            }
            return View(loginlist);
        }

        //public ActionResult SubmitProfile(ProfileModel submitprofile)
        //{
        //    string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
        //    using (SqlConnection con = new SqlConnection(cs))
        //    {
        //        string query = "INSERT INTO TblUser_Profile (Profile_Name,IsActive) VALUES(@Profile_Name,@IsActive)";
        //        using (SqlCommand cmd = new SqlCommand(query))
        //        {
        //            cmd.Connection = con;
        //            con.Open();
        //            cmd.Parameters.AddWithValue("@Profile_Name", submitprofile.Profile_Name);
        //            cmd.Parameters.AddWithValue("@IsActive", submitprofile.IsActive);

        //            foreach (SqlParameter Parameter in cmd.Parameters)
        //            {
        //                if (Parameter.Value == null)
        //                {
        //                    Parameter.Value = "NA";
        //                }
        //            }

        //            submitprofile.ID = Convert.ToInt32(cmd.ExecuteScalar());
        //            con.Close();
        //        }
        //    }
        //    return RedirectToAction("UserProfile");
        //}

        public ActionResult EditProfile(string id)
        {
            var Profile_ID = id;
            var t = "";

            LoginModel editProfile = new LoginModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Profile WHERE ID ='" + Profile_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader["IsActive"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<LoginModel> Edit = new List<LoginModel>();
                    if (!(String.IsNullOrEmpty(Profile_ID)))
                    {
                        LoginModel uobj = new LoginModel
                        {
                            Profile_Name = reader["Profile_Name"].ToString(),
                            //IsActive = t
                        };
                        Edit.Add(uobj);
                    }
                    editProfile.loginmodel = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editProfile);
        }

        public ActionResult UpdateProfile(LoginModel updateProfile)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                if (updateProfile.IsActive == "2")
                {
                    updateProfile.IsActive = "0";
                }

                string query3 = "UPDATE TblUser_Profile SET Profile_Name ='" + updateProfile.Profile_Name + "', IsActive ='" + updateProfile.IsActive + "' WHERE ID='" + updateProfile.ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }

            return RedirectToAction("UserProfile");
        }

        public ActionResult UserLogin()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            LoginModel loginlist = new LoginModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT UL.ID,User_Email,Profile_Name,UL.Created_Date,UL.IsActive from TblUser_Login UL LEFT JOIN TblUser_Profile UP ON UL.Profile_ID = UP.ID where UL.Profile_ID in ('2','3') ORDER BY UP.Profile_Name asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<LoginModel> login = new List<LoginModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            LoginModel uobj = new LoginModel
                            {
                                ID = reader["ID"].ToString(),
                                User_Email = reader["User_Email"].ToString(),
                                Profile_Name = reader["Profile_Name"].ToString(),
                                Created_Date = reader["Created_Date"].ToString(),
                                IsActive = s
                            };
                            login.Add(uobj);
                        }
                    }
                    loginlist.loginmodel = login;
                    con.Close();
                }
            }

            List<SelectListItem> profile = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Profile_Name FROM TblUser_Profile WHERE IsActive=1 order by Profile_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            profile.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Profile_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.ProfileList = profile;

            return View(loginlist);
        }

        public ActionResult SubmitUser(UserModel submituser)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblUser_Login (User_Email,User_Name,User_ShortName,User_LoginID,User_Password2,Profile_ID) VALUES(@User_Email,@User_Name,@User_ShortName,@User_Email,@User_Password2,@Profile_ID)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@User_Name", submituser.User_Name);
                    cmd.Parameters.AddWithValue("@User_ShortName", submituser.User_ShortName);
                    cmd.Parameters.AddWithValue("@User_LoginID", submituser.User_Email);
                    cmd.Parameters.AddWithValue("@User_Email", submituser.User_Email);
                    cmd.Parameters.AddWithValue("@User_Password2", Encrypt(submituser.User_Password2));
                    cmd.Parameters.AddWithValue("@Profile_ID", submituser.Profile_ID);

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return RedirectToAction("UserLogin");
        }

        public ActionResult EditUser(string id)
        {
            var User_ID = id;
            var Profile_ID = "";
            var t = "";

            LoginModel editProfile = new LoginModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT *,UL.IsActive as StatusActive, UP.Profile_Name, UP.ID as Profile_ID FROM TblUser_Login UL LEFT JOIN TblUser_Profile UP ON UL.Profile_ID = UP.ID  WHERE UL.ID ='" + User_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Profile_ID = reader["Profile_ID"].ToString();

                    if (reader["StatusActive"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<LoginModel> Edit = new List<LoginModel>();
                    if (!(String.IsNullOrEmpty(User_ID)))
                    {
                        LoginModel uobj = new LoginModel
                        {
                            User_Name = reader["User_Name"].ToString(),
                            User_ShortName = reader["User_ShortName"].ToString(),
                            User_Email = reader["User_Email"].ToString(),
                            User_Password2 = Decrypt(reader["User_Password2"].ToString()),
                            Profile_Name = reader["Profile_Name"].ToString()
                        };
                        Edit.Add(uobj);
                    }
                    editProfile.loginmodel = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> profile = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Profile_Name FROM TblUser_Profile WHERE IsActive=1 order by Profile_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            profile.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Profile_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.ProfileList = profile;
            ViewBag.SelectlistProfileID = Profile_ID;

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editProfile);
        }

        public ActionResult UpdateUser(LoginModel updateUser)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (updateUser.IsActive == "2")
                {
                    updateUser.IsActive = "0";
                }

                string query3 = "UPDATE TblUser_Login SET User_Name ='" + updateUser.User_Name + "', User_ShortName ='" + updateUser.User_ShortName + "',User_LoginID ='" + updateUser.User_Email + "',User_Email ='" + updateUser.User_Email + "',User_Password2 ='" + Encrypt(updateUser.User_Password2) + "',Profile_ID ='" + updateUser.Profile_Name + "',IsActive ='" + updateUser.IsActive + "' WHERE ID='" + updateUser.ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("UserLogin");
        }

        public ActionResult DeleteUser(string id)
        {
            var userid = id;
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                //string query3 = "UPDATE TblUser_Login SET IsActive = 0 WHERE ID='" + userid + "'";
                string query3 = "DELETE TblUser_Login WHERE ID='" + userid + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("UserLogin");
        }

        public ActionResult RegionManager()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            RegionManagerModel region = new RegionManagerModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Region ORDER BY Region_Name asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<RegionManagerModel> regionlist = new List<RegionManagerModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            RegionManagerModel uobj = new RegionManagerModel
                            {
                                ID = reader["ID"].ToString(),
                                Region_Name = reader["Region_Name"].ToString(),
                                IsActive = s,
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            regionlist.Add(uobj);
                        }
                    }
                    region.regionManager = regionlist;
                    con.Close();
                }
            }
            return View(region);
        }

        public ActionResult SubmitRegion(RegionManagerModel submitregion)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblMaster_Region (Region_Name,IsActive) VALUES(@Region_Name,@IsActive)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Region_Name", submitregion.Region_Name);
                    cmd.Parameters.AddWithValue("@IsActive", submitregion.IsActive);

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return RedirectToAction("RegionManager");
        }

        public ActionResult EditRegion(string id)
        {
            var Region_ID = id;
            var t = "";

            RegionManagerModel editRegion = new RegionManagerModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Region WHERE ID ='" + Region_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader["IsActive"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<RegionManagerModel> Edit = new List<RegionManagerModel>();
                    if (!(String.IsNullOrEmpty(Region_ID)))
                    {
                        RegionManagerModel uobj = new RegionManagerModel
                        {
                            Region_Name = reader["Region_Name"].ToString(),
                        };
                        Edit.Add(uobj);
                    }
                    editRegion.regionManager = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editRegion);
        }

        public ActionResult UpdateRegion(RegionManagerModel updateRegion)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (updateRegion.IsActive == "2")
                {
                    updateRegion.IsActive = "0";
                }

                string query3 = "UPDATE TblMaster_Region SET Region_Name ='" + updateRegion.Region_Name + "', IsActive ='" + updateRegion.IsActive + "' WHERE ID='" + updateRegion.ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }

            return RedirectToAction("RegionManager");
        }

        public ActionResult DeleteRegion(string id)
        {
            var region_id = id;
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                //string query3 = "UPDATE TblMaster_Region SET IsActive = 0 WHERE ID='" + region_id + "'";
                string query3 = "DELETE TblMaster_Region WHERE ID='" + region_id + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("RegionManager");
        }

        public ActionResult CompanyManager()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            CompManagerModel company = new CompManagerModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Company ORDER BY Company_Name asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<CompManagerModel> complist = new List<CompManagerModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            CompManagerModel uobj = new CompManagerModel
                            {
                                ID = reader["ID"].ToString(),
                                Company_Name = reader["Company_Name"].ToString(),
                                IsActive = s,
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            complist.Add(uobj);
                        }
                    }
                    company.compManager = complist;
                    con.Close();
                }
            }
            return View(company);
        }

        public ActionResult SubmitCompany(CompManagerModel submitcomp)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblMaster_Company (Company_Name,IsActive) VALUES(@Company_Name,@IsActive)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Company_Name", submitcomp.Company_Name);
                    cmd.Parameters.AddWithValue("@IsActive", submitcomp.IsActive);

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return RedirectToAction("CompanyManager");
        }

        public ActionResult EditCompany(string id)
        {
            var Comp_ID = id;
            var t = "";

            CompManagerModel editComp = new CompManagerModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Company WHERE ID ='" + Comp_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader["IsActive"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<CompManagerModel> Edit = new List<CompManagerModel>();
                    if (!(String.IsNullOrEmpty(Comp_ID)))
                    {
                        CompManagerModel uobj = new CompManagerModel
                        {
                            Company_Name = reader["Company_Name"].ToString(),
                        };
                        Edit.Add(uobj);
                    }
                    editComp.compManager = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editComp);
        }

        public ActionResult UpdateCompany(CompManagerModel updateComp)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (updateComp.IsActive == "2")
                {
                    updateComp.IsActive = "0";
                }

                string query3 = "UPDATE TblMaster_Company SET Company_Name ='" + updateComp.Company_Name + "', IsActive ='" + updateComp.IsActive + "' WHERE ID='" + updateComp.ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }

            return RedirectToAction("CompanyManager");
        }

        public ActionResult DeleteCompany(string id)
        {
            var comp_id = id;
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                //string query3 = "UPDATE TblMaster_Company SET IsActive = 0 WHERE ID='" + comp_id + "'";
                string query3 = "DELETE TblMaster_Company WHERE ID='" + comp_id + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("CompanyManager");
        }

        public ActionResult DepartManager()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            DepartManagerModel depart = new DepartManagerModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Department ORDER BY Dep_Name asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<DepartManagerModel> departlist = new List<DepartManagerModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            DepartManagerModel uobj = new DepartManagerModel
                            {
                                ID = reader["ID"].ToString(),
                                Dep_Name = reader["Dep_Name"].ToString(),
                                IsActive = s,
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            departlist.Add(uobj);
                        }
                    }
                    depart.departManager = departlist;
                    con.Close();
                }
            }
            return View(depart);
        }

        public ActionResult SubmitDepart(DepartManagerModel submitdepart)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblMaster_Department (Dep_Name,IsActive) VALUES(@Dep_Name,@IsActive)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Dep_Name", submitdepart.Dep_Name);
                    cmd.Parameters.AddWithValue("@IsActive", submitdepart.IsActive);

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return RedirectToAction("DepartManager");
        }

        public ActionResult EditDepart(string id)
        {
            var Depart_ID = id;
            var t = "";

            DepartManagerModel editDepart = new DepartManagerModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Department WHERE ID ='" + Depart_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader["IsActive"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<DepartManagerModel> Edit = new List<DepartManagerModel>();
                    if (!(String.IsNullOrEmpty(Depart_ID)))
                    {
                        DepartManagerModel uobj = new DepartManagerModel
                        {
                            Dep_Name = reader["Dep_Name"].ToString(),
                        };
                        Edit.Add(uobj);
                    }
                    editDepart.departManager = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editDepart);
        }

        public ActionResult UpdateDepart(DepartManagerModel updateDepart)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (updateDepart.IsActive == "2")
                {
                    updateDepart.IsActive = "0";
                }

                string query3 = "UPDATE TblMaster_Department SET Dep_Name ='" + updateDepart.Dep_Name + "', IsActive ='" + updateDepart.IsActive + "' WHERE ID='" + updateDepart.ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }

            return RedirectToAction("DepartManager");
        }

        public ActionResult DeleteDepart(string id)
        {
            var depart_id = id;
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                //string query3 = "UPDATE TblMaster_Department SET IsActive = 0 WHERE ID='" + depart_id + "'";
                string query3 = "DELETE TblMaster_Department WHERE ID='" + depart_id + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("DepartManager");
        }

        public ActionResult DCManager()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            DCManagerModel dc = new DCManagerModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT MD.ID, DC_Code, Region_Name,Company_Name,MD.Created_Date, MD.IsActive, MD.Remark FROM TblMaster_DC MD LEFT JOIN TblMaster_Region MR on MR.ID = MD.REGION_ID LEFT JOIN TblMaster_Company MC ON MC.ID = MD.COMP_ID ORDER BY DC_CODE asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<DCManagerModel> dclist = new List<DCManagerModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["IsActive"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            DCManagerModel uobj = new DCManagerModel
                            {
                                ID = reader["ID"].ToString(),
                                DC_Code = reader["DC_Code"].ToString(),
                                Region_ID = reader["Region_Name"].ToString(),
                                Comp_ID = reader["Company_Name"].ToString(),
                                IsActive = s,
                                Created_Date = reader["Created_Date"].ToString(),
                                Remark = reader["Remark"].ToString()
                            };
                            dclist.Add(uobj);
                        }
                    }
                    dc.dcManager = dclist;
                    con.Close();
                }
            }

            List<SelectListItem> DC = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, DC_Code FROM [TblMaster_DC] WHERE IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            DC.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["DC_Code"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.DCList = DC;

            List<SelectListItem> regions = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Region_Name FROM TblMaster_Region WHERE IsActive=1 ORDER BY Region_Name Asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            regions.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Region_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.RegionList = regions;

            List<SelectListItem> company = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Company_Name FROM TblMaster_Company WHERE IsActive=1 order by Company_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            company.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Company_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.CompList = company;

            return View(dc);
        }

        public ActionResult SubmitDC(DCManagerModel submitdC)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblMaster_DC (DC_Code,Region_ID,Comp_ID,Remark,IsActive) VALUES(@DC_Code,@Region_ID,@Comp_ID,@Remark,@IsActive)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@DC_Code", submitdC.DC_Code);
                    cmd.Parameters.AddWithValue("@Region_ID", submitdC.Region_ID);
                    cmd.Parameters.AddWithValue("@Comp_ID", submitdC.Comp_ID);
                    cmd.Parameters.AddWithValue("@Remark", submitdC.Remark);
                    cmd.Parameters.AddWithValue("@IsActive", submitdC.IsActive);

                    //foreach (SqlParameter Parameter in cmd.Parameters)
                    //{
                    //    if (Parameter.Value == null)
                    //    {
                    //        Parameter.Value = "NA";
                    //    }
                    //}
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return RedirectToAction("DCManager");
        }

        public ActionResult EditDC(string id)
        {
            var DC_ID = id;

            var Region_ID = "";

            var Company_ID = "";
            var t = "";

            DCManagerModel editDC = new DCManagerModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT MC.ID as Company_ID, MR.ID as Region_ID, DC_Code, Region_Name,Company_Name,MD.Created_Date, MD.IsActive as StatusActive, MD.Remark FROM TblMaster_DC MD LEFT JOIN TblMaster_Region MR on MR.ID = MD.REGION_ID LEFT JOIN TblMaster_Company MC ON MC.ID = MD.COMP_ID WHERE MD.ID ='" + DC_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Region_ID = reader["Region_ID"].ToString();
                    Company_ID = reader["Company_ID"].ToString();

                    if (reader["StatusActive"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<DCManagerModel> Edit = new List<DCManagerModel>();
                    if (!(String.IsNullOrEmpty(DC_ID)))
                    {
                        DCManagerModel uobj = new DCManagerModel
                        {
                            DC_Code = reader["DC_Code"].ToString(),
                            Region_Name = reader["Region_Name"].ToString(),
                            Comp_Name = reader["Company_Name"].ToString(),
                            Remark = reader["Remark"].ToString(),
                        };
                        Edit.Add(uobj);
                    }
                    editDC.dcManager = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> regions = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Region_Name FROM TblMaster_Region WHERE IsActive=1 ORDER BY Region_Name ASC ";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            regions.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Region_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.State = regions;
            ViewBag.SelectedStateID = Region_ID;

            List<SelectListItem> company = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Company_Name FROM TblMaster_Company WHERE IsActive=1 order by Company_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            company.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Company_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.CompList = company;
            ViewBag.SelectedCompID = Company_ID;

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editDC);
        }

        public ActionResult UpdateDC(DCManagerModel updateDC)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (updateDC.IsActive == "2")
                {
                    updateDC.IsActive = "0";
                }

                string query3 = "UPDATE TblMaster_DC SET DC_Code ='" + updateDC.DC_Code + "', Region_ID ='" + updateDC.Region_Name + "',Comp_ID ='" + updateDC.Comp_Name + "',Remark ='" + updateDC.Remark + "',IsActive ='" + updateDC.IsActive + "' WHERE ID='" + updateDC.ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }
                con1.Close();
            }
            return RedirectToAction("DCManager");
        }

        public ActionResult DeleteDC(string id)
        {
            var dc_id = id;
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                //string query3 = "UPDATE TblMaster_DC SET IsActive = 0 WHERE ID='" + dc_id + "'";
                string query3 = "DELETE TblMaster_DC WHERE ID='" + dc_id + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("DCManager");
        }

        public ActionResult JobManager()
        {
            var s = "";
            string userID = Session["ID"].ToString();

            JobManagerModel job = new JobManagerModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT MP.POSITION_ID, POSITION_NAME,DC_CODE,DEP_NAME,TOTAL_VACANCY,MP.ISOFFER,MP.Created_Date FROM TblMaster_Position MP LEFT JOIN TblMaster_DC MDC ON MDC.ID = MP.DC_ID LEFT JOIN TblMaster_Department MD ON MD.ID = MP.DEPART_ID ORDER BY DEP_NAME asc";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    //reader.Read();

                    List<JobManagerModel> joblist = new List<JobManagerModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        while (reader.Read())
                        {
                            if (reader["ISOFFER"].ToString() == "True")
                            {
                                s = "Yes";
                            }
                            else
                            {
                                s = "No";
                            }

                            JobManagerModel uobj = new JobManagerModel
                            {
                                Position_ID = reader["POSITION_ID"].ToString(),
                                Position_Name = reader["POSITION_NAME"].ToString(),
                                DC_ID = reader["DC_CODE"].ToString(),
                                Depart_ID = reader["DEP_NAME"].ToString(),
                                Total_Vacancy = reader["TOTAL_VACANCY"].ToString(),
                                IsOffer = s,
                                Created_Date = reader["Created_Date"].ToString()
                            };
                            joblist.Add(uobj);
                        }
                    }
                    job.jobManager = joblist;
                    con.Close();
                }
            }

            List<SelectListItem> DC = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, DC_Code FROM [TblMaster_DC] WHERE IsActive=1 ORDER BY DC_Code ASC";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            DC.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["DC_Code"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.DCList = DC;

            List<SelectListItem> department = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Dep_Name FROM TblMaster_Department WHERE IsActive=1 ORDER BY DEP_NAME ASC";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            department.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Dep_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.DepList = department;

            List<SelectListItem> qua = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Type_Qualification FROM TblMaster_Qualification";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            qua.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Qualification"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.QuaList = qua;

            List<SelectListItem> career = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Career_Name FROM TblMaster_Career";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            career.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Career_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.CareerList = career;

            List<SelectListItem> jobtype = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Type_Job FROM TblMaster_JobType order by Type_Job asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            jobtype.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Job"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.TypeJobList = jobtype;

            return View(job);
        }

        public ActionResult SubmitJob(JobManagerModel submitjob)
        {
            var Depart_ID = submitjob.Depart_ID;
            var Dep_Name = "";
            var Position_ID = "";
            var Career_Name = "";
            var Type_Qualification = "";
            var Type_Job = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblMaster_Position (Position_Name,DC_ID,Depart_ID,Total_Vacancy,IsOffer) VALUES(@Position_Name,@DC_ID,@Depart_ID,@Total_Vacancy,@IsOffer)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@Position_Name", submitjob.Position_Name);
                    cmd.Parameters.AddWithValue("@DC_ID", submitjob.DC_ID);
                    cmd.Parameters.AddWithValue("@Depart_ID", submitjob.Depart_ID);
                    cmd.Parameters.AddWithValue("@Total_Vacancy", submitjob.Total_Vacancy);
                    cmd.Parameters.AddWithValue("@IsOffer", submitjob.IsOffer);

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Dep_Name FROM TblMaster_Department WHERE ID='" + Depart_ID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Dep_Name = reader["Dep_Name"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Position_ID FROM TblMaster_Position WHERE Position_Name ='" + submitjob.Position_Name + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Position_ID = reader["Position_ID"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Career_Name FROM TblMaster_Career WHERE ID ='" + submitjob.Career_Name + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Career_Name = reader["Career_Name"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Type_Qualification FROM TblMaster_Qualification WHERE ID ='" + submitjob.Type_Qualification + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Type_Qualification = reader["Type_Qualification"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Type_Job FROM TblMaster_JobType WHERE ID ='" + submitjob.Type_Job + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Type_Job = reader["Type_Job"].ToString();
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblJob_Description (User_Career_Level,User_Years_Exp,Depart_Name,User_Qualification,Job_Type,Position_Title,Location_Job,Job_Description,Position_ID) VALUES(@Career_Name,@User_Years_Exp,@Dep_Name,@Type_Qualification,@Type_Job,@Position_Name,@Location_Job,@Job_Description,@Position_ID)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();
                    cmd.Parameters.AddWithValue("@Career_Name", Career_Name);
                    cmd.Parameters.AddWithValue("@User_Years_Exp", submitjob.User_Years_Exp);
                    cmd.Parameters.AddWithValue("@Dep_Name", Dep_Name);
                    cmd.Parameters.AddWithValue("@Type_Qualification", Type_Qualification);
                    cmd.Parameters.AddWithValue("@Type_Job", Type_Job);
                    cmd.Parameters.AddWithValue("@Position_Name", submitjob.Position_Name);
                    cmd.Parameters.AddWithValue("@Location_Job", submitjob.Location_Job);
                    cmd.Parameters.AddWithValue("@Job_Description", submitjob.Job_Description);
                    cmd.Parameters.AddWithValue("@Position_ID", Position_ID);

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }
                    var ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            return RedirectToAction("JobManager");
        }

        public ActionResult EditJob(string id)
        {
            var Job_ID = id;
            var Dep_ID = "";
            var DC_ID = "";
            var t = "";

            JobManagerModel editJob = new JobManagerModel();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT MDC.ID as DC_ID, MD.ID as Dep_ID, POSITION_ID,POSITION_NAME,DC_CODE,DEP_NAME,TOTAL_VACANCY,MP.ISOFFER as OfferStatus FROM TblMaster_Position MP LEFT JOIN TblMaster_DC MDC ON MDC.ID = MP.DC_ID LEFT JOIN TblMaster_Department MD ON MD.ID = MP.DEPART_ID WHERE MP.POSITION_ID ='" + Job_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    DC_ID = reader["DC_ID"].ToString();
                    Dep_ID = reader["Dep_ID"].ToString();

                    if (reader["OfferStatus"].ToString() == "True")
                    {
                        t = "1";
                    }
                    else
                    {
                        t = "2";
                    }

                    List<JobManagerModel> Edit = new List<JobManagerModel>();
                    if (!(String.IsNullOrEmpty(Job_ID)))
                    {
                        JobManagerModel uobj = new JobManagerModel
                        {
                            Position_ID = reader["POSITION_ID"].ToString(),
                            Position_Name = reader["POSITION_NAME"].ToString(),
                            DC_Code = reader["DC_CODE"].ToString(),
                            Depart_Name = reader["DEP_NAME"].ToString(),
                            Total_Vacancy = reader["Total_Vacancy"].ToString(),
                        };
                        Edit.Add(uobj);
                    }
                    editJob.jobManager = Edit;
                    con.Close();
                }
            }

            List<SelectListItem> DC = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, DC_Code FROM [TblMaster_DC] WHERE IsActive=1 order by DC_Code asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            DC.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["DC_Code"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.DCList = DC;
            ViewBag.SelectedDCID = DC_ID;

            List<SelectListItem> department = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Dep_Name FROM TblMaster_Department WHERE IsActive=1 order by Dep_Name asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            department.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Dep_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.DepList = department;
            ViewBag.SelectedDepID = Dep_ID;

            List<SelectListItem> status = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Status2";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            status.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Status"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.Status = status;
            ViewBag.SelectedStatus = t;

            return View(editJob);
        }

        public ActionResult UpdateJob(JobManagerModel updateJob)
        {
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (updateJob.IsOffer == "2")
                {
                    updateJob.IsOffer = "0";
                }

                string query3 = "UPDATE TblMaster_Position SET Position_Name ='" + updateJob.Position_Name + "', DC_ID ='" + updateJob.DC_Code + "',Depart_ID ='" + updateJob.Depart_Name + "',Total_Vacancy ='" + updateJob.Total_Vacancy + "',IsOffer ='" + updateJob.IsOffer + "' WHERE Position_ID='" + updateJob.Position_ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }
                con1.Close();
            }
            return RedirectToAction("JobManager");
        }

        public ActionResult DeleteJob(string id)
        {
            var Job_ID = id;

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();
                string query3 = "UPDATE TblMaster_Position SET IsActive = 0 WHERE Position_ID='" + Job_ID + "'";
                //string query3 = "DELETE TblMaster_Position WHERE Position_ID='" + Job_ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query3))
                {
                    cmd1.Connection = con1;
                    cmd1.ExecuteNonQuery();
                }

                con1.Close();
            }
            return RedirectToAction("JobManager");
        }

        // DOWNLOAD FILE
        // DOWNLOAD FILE
        // DOWNLOAD FILE
        public FileResult DownloadResume(string id)
        {
            string fileName = "";
            var Position_ID = id;
            string userID = Session["ID"].ToString();
            string User_Resume = "";
            string UserName = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT User_Resume,User_ShortName from TblResume R LEFT JOIN TblJob_Application JA ON JA.User_ID = R.User_ID LEFT JOIN TblUser_Login UL ON UL.ID = R.User_ID WHERE JA.Position_ID='" + id + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        User_Resume = reader["User_Resume"].ToString();
                        UserName = reader["User_ShortName"].ToString();
                    }
                    con.Close();
                }
            }

            string UploadPath = ConfigurationManager.AppSettings["UserResumePath"].ToString();
            string FullNameFile = UploadPath + User_Resume;

            string File_Name = User_Resume.Substring(0, User_Resume.LastIndexOf('.'));

            string extension = User_Resume.Substring(User_Resume.LastIndexOf('.'), User_Resume.Length - User_Resume.LastIndexOf('.'));


            byte[] fileBytes = System.IO.File.ReadAllBytes(FullNameFile);

            if (extension == ".pdf" || extension == ".PDF")
            {
                fileName = UserName + "-Resume.pdf";
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                fileName = UserName + "-Resume-" + User_Resume;
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }

        public FileResult DownloadResumeList(string id)
        {
            string fileName = "";
            var userID = id;
            string User_Resume = "";
            string UserName = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT User_Resume,User_ShortName from TblResume R LEFT JOIN TblUser_Login UL ON UL.ID = R.User_ID WHERE UL.ID ='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        User_Resume = reader["User_Resume"].ToString();
                        UserName = reader["User_ShortName"].ToString();
                    }
                    con.Close();
                }
            }

            string UploadPath = ConfigurationManager.AppSettings["UserResumePath"].ToString();
            string FullNameFile = UploadPath + User_Resume;

            string File_Name = User_Resume.Substring(0, User_Resume.LastIndexOf('.'));

            string extension = User_Resume.Substring(User_Resume.LastIndexOf('.'), User_Resume.Length - User_Resume.LastIndexOf('.'));

            byte[] fileBytes = System.IO.File.ReadAllBytes(FullNameFile);

            if (extension == ".pdf" || extension == ".PDF")
            {
                fileName = UserName + "-Resume.pdf";
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                fileName = UserName + "-Resume-" + User_Resume;
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }

        public ActionResult GetRegionList()
        {
            List<SelectListItem> regions = new List<SelectListItem>();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Region_Name FROM TblMaster_Region WHERE IsActive=1";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            regions.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Region_Name"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            return Json(regions, JsonRequestBehavior.AllowGet);
        }

        //[HttpGet]
        //public ActionResult Report()
        //{
        //    ViewBag.FromDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
        //    ViewBag.ToDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

        //    ReportModel report = new ReportModel();

        //    string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

        //    //List<ReportModel> reportlist = new List<ReportModel>();

        //    //report.reportinfo = null;

        //    //return View(report);

        //    return View();
        //}

        [HttpGet]
        public ActionResult Report()
        {
            ViewBag.FromDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            ViewBag.ToDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));
            string userID = Session["ID"].ToString();

            ReportModel report = new ReportModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            //List<ReportModel> reportlist = new List<ReportModel>();

            //report.reportinfo = null;

            //return View(report);
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "sp_job_list_view";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();

                    List<ReportModel> reportlist = new List<ReportModel>();

                    while (reader.Read())
                    {
                        ReportModel uobj = new ReportModel
                        {
                            Dep_Name = reader["Dep_Name"].ToString(),
                            Position_Name = reader["Position_Name"].ToString(),
                            Total = reader["Total_Applicant"].ToString(),
                            New = reader["New_Applicant"].ToString(),
                            Review = reader["Reviewed"].ToString(),
                            Call_Interview = reader["Call_For_Interview"].ToString(),
                            Interview = reader["Interviewed"].ToString(),
                            Keep_In_View = reader["Keep_In_View"].ToString(),
                            Accepted = reader["Accepted_Applicant"].ToString(),
                            Rejected = reader["Not_Accepted"].ToString(),
                            Withdrawn = reader["Withdrawn"].ToString()
                        };
                        reportlist.Add(uobj);
                    }
                    report.reportinfo = reportlist;
                    con.Close();
                }
            }
            return View(report);
        }

        [HttpPost]
        public ActionResult Report(ReportModel submitreport)
        {
            var DateFrom = submitreport.DateFrom;
            var DateTo = submitreport.DateTo;

            ReportModel report = new ReportModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "sp_job_list '" + DateFrom + "', '" + DateTo + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();

                    List<ReportModel> reportlist = new List<ReportModel>();

                    while (reader.Read())
                    {
                        ReportModel uobj = new ReportModel
                        {
                            Dep_Name = reader["Dep_Name"].ToString(),
                            Position_Name = reader["Position_Name"].ToString(),
                            Total = reader["Total_Applicant"].ToString(),
                            New = reader["New_Applicant"].ToString(),
                            Review = reader["Reviewed"].ToString(),
                            Call_Interview = reader["Call_For_Interview"].ToString(),
                            Interview = reader["Interviewed"].ToString(),
                            Keep_In_View = reader["Keep_In_View"].ToString(),
                            Accepted = reader["Accepted_Applicant"].ToString(),
                            Rejected = reader["Not_Accepted"].ToString(),
                            Withdrawn = reader["Withdrawn"].ToString()
                        };
                        reportlist.Add(uobj);
                    }
                    report.reportinfo = reportlist;
                    con.Close();
                }
            }
            return View(report);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Login", "Home");
        }

        public ActionResult Info(string id)
        {
            string userID = Session["ID"].ToString();

            var User_ID = id;

            InfoModel infodetail = new InfoModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(User_ID)))
                    {
                        string userShortName = reader["User_ShortName"].ToString();
                        ViewBag.User_ShortName = userShortName;
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT *, R.User_Resume from TblUser_Login UL LEFT JOIN TblResume R ON UL.ID= R.User_ID WHERE UL.ID ='" + User_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    List<InfoModel> info = new List<InfoModel>();
                    if (!(String.IsNullOrEmpty(User_ID)))
                    {
                        string s;
                        if (reader["User_Driving_Attach"].ToString() == "")
                        {
                            s = "NO FILE";
                        }
                        else
                        {
                            s = reader["User_Driving_Attach"].ToString();
                        }
                        InfoModel uobj = new InfoModel
                        {
                            User_ShortName = reader["User_ShortName"].ToString(),
                            User_Last_Name = reader["User_Last_Name"].ToString(),
                            User_IC = reader["User_IC"].ToString(),
                            User_Permanent_Address = reader["User_Permanent_Address"].ToString(),
                            User_Correspon_Address = reader["User_Correspon_Address"].ToString(),
                            User_Location = reader["User_Location"].ToString(),
                            User_Phone = reader["User_Phone"].ToString(),
                            User_Tel_Home = reader["User_Tel_Home"].ToString(),
                            User_Email = reader["User_Email"].ToString(),
                            User_Resume = reader["User_Resume"].ToString(),
                            User_Driving_License = reader["User_Driving_License"].ToString(),
                            User_Driving_Class = reader["User_Driving_Class"].ToString(),
                            User_Driving_Attach = s,
                            User_ID = User_ID
                        };
                        info.Add(uobj);
                    }
                    infodetail.usersinfo = info;
                    con.Close();
                }
            }

            return View(infodetail);
        }

        public FileResult DownloadDL(string id)
        {
            string fileName = "";
            //TempData["Message"] = "Please complete ALL profile details before applying job. Thank you.";

            string userID = id;
            string User_Driving_Attach = "";
            string UserName = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        User_Driving_Attach = reader["User_Driving_Attach"].ToString();
                        UserName = reader["User_ShortName"].ToString();
                    }

                    con.Close();
                }
            }
            string UploadPath = ConfigurationManager.AppSettings["UserDrivingL"].ToString();
            string FullNameFile = UploadPath + User_Driving_Attach;

            string File_Name = User_Driving_Attach.Substring(0, User_Driving_Attach.LastIndexOf('.'));
            string extension = User_Driving_Attach.Substring(User_Driving_Attach.LastIndexOf('.'), User_Driving_Attach.Length - User_Driving_Attach.LastIndexOf('.'));

            byte[] fileBytes = System.IO.File.ReadAllBytes(FullNameFile);

            if (extension == ".pdf" || extension == ".PDF")
            {
                fileName = UserName + "-DrivingLicense.pdf";
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                fileName = UserName + "-DrivingLicense-" + User_Driving_Attach;
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }

        private string Encrypt(string clearText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }

        private string Decrypt(string cipherText)
        {
            string EncryptionKey = "MAKV2SPBNI99212";
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
        }
    }
}