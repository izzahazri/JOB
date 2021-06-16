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
using Newtonsoft.Json;
using System.Net;
using System.Net.Mail;
using System.Globalization;
using System.Security.Cryptography;
using System.IO;
using System.Text;

namespace OJAWeb.Controllers
{
    public class AdminController : Controller
    {
        // GET: Admin
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
                    con.Close();
                }
            }
            return View(jobDesc);
        }

        public ActionResult Declaration(string id)
        {
            var Position_ID = id;

            DeclarationModel jobDeclare = new DeclarationModel();
            List<DeclarationModel> userdeclare = new List<DeclarationModel>();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Position WHERE Position_ID ='" + Position_ID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(Position_ID)))
                    {
                        DeclarationModel uobj = new DeclarationModel
                        {
                            Position_ID = reader["Position_ID"].ToString(),
                        };
                        userdeclare.Add(uobj);
                    }
                    jobDeclare.getDataDeclare = userdeclare;
                    con.Close();
                }
            }

            return View(jobDeclare);
        }


        //public ActionResult ThankYou(DeclarationModel data)
        public ActionResult ThankYou(string id)
        {
            var Position_ID = id;

            string userID = Session["ID"].ToString();
            string userName = "";
            string regionName = "";
            string DeaprtID = "";
            string dcID = "";

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
                        userName = reader["User_Name"].ToString();
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT Region_Name, Depart_ID, DC_ID from TblJob_Description D LEFT JOIN TblMaster_Position P ON D.Position_ID = P.Position_ID LEFT JOIN TblMaster_DC MD ON P.DC_ID = MD.ID LEFT JOIN TblMaster_Region MR ON MD.Region_ID = MR.ID LEFT JOIN TblMaster_Department MP ON P.Depart_ID = MP.ID WHERE D.Position_ID = '" + id + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        regionName = reader["Region_Name"].ToString();
                        DeaprtID = reader["Depart_ID"].ToString();
                        dcID = reader["DC_ID"].ToString();
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                //string query = "INSERT INTO TblJob_Application (User_Name,Region_Name,Depart_ID,DC_ID, Position_Title, User_ID, Status_Application, IsActive) VALUES(@User_Name,@RegionD_Name,@Depart_ID,@DC_ID, @Position_Title, @User_ID, @Status_Application, @IsActive)"; 
                string query = "INSERT INTO TblJob_Application (User_Name,Region_Name,Depart_ID,DC_ID, Position_ID, User_ID, Status_Application, IsActive) VALUES(@User_Name,@Region_Name,@Depart_ID,@DC_ID, @Position_ID, @User_ID, @Status_Application, @IsActive);";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    con.Open();

                    cmd.Parameters.AddWithValue("@User_Name", userName);
                    cmd.Parameters.AddWithValue("@Region_Name", regionName);
                    cmd.Parameters.AddWithValue("@Depart_ID", DeaprtID);
                    cmd.Parameters.AddWithValue("@DC_ID", dcID);
                    cmd.Parameters.AddWithValue("@Position_ID", id);
                    cmd.Parameters.AddWithValue("@Status_Application", "In Progress");
                    cmd.Parameters.AddWithValue("@User_ID", userID);
                    cmd.Parameters.AddWithValue("@IsActive", "1");

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

            return View();
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
                                Status_Code = reader["Status_Code"].ToString(),
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
                                User_Name = reader["User_Name"].ToString(),
                                User_ID = reader["User_ID"].ToString(),
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
            //string message = "kk";
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