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
using System.IO;
using System.Data;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace OJAWeb.Controllers
{
    public class ApplicantController : Controller
    {
        [HttpGet]
        public ActionResult ExtendSession()
        {
            System.Web.Security.FormsAuthentication.SetAuthCookie(User.Identity.Name, false);
            var data = new { IsSuccess = true };
            return Json(data, JsonRequestBehavior.AllowGet);
        }

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

            return View();
        }

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
            string userID = Session["ID"].ToString();

            JobDescripModel jobDesc = new JobDescripModel();

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
            string User_Resume = "";
            string User_Phone = "";
            string User_Driving_License = "";
            string User_Driving_Attach = "";
            string User_Driving_Class = "";
            string Status_Code = "";
            string IsActive = "";

            DeclarationModel jobDeclare = new DeclarationModel();
            List<DeclarationModel> userdeclare = new List<DeclarationModel>();

            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "  SELECT TOP 1 SS.Status_Code AS Status_Code,* FROM TblJob_Application JA LEFT JOIN TblSystem_Status SS ON SS.ID = JA.Status_Application WHERE JA.Position_ID ='" + Position_ID + "' and User_ID ='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == false)
                    {
                        Status_Code = null;
                        IsActive = null;
                    }
                    else
                    {
                        Status_Code = reader["Status_Code"].ToString();
                        IsActive = reader["IsActive"].ToString();
                    }
                    con.Close();
                }
            }

            if (Status_Code == "In Progress" && IsActive == "True")
            {
                TempData["Message"] = "Job had applied. Kindly wait for next response from us. Thank you.";

                return RedirectToAction("MyJob");
            }
            else
            {

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT *, User_Phone FROM TblUser_Login WHERE ID='" + userID + "'";
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
                            User_Phone = reader["User_Phone"].ToString();
                            User_Driving_License = reader["User_Driving_License"].ToString();
                            User_Driving_Attach = reader["User_Driving_Attach"].ToString();
                            User_Driving_Class = reader["User_Driving_Class"].ToString();
                        }
                        con.Close();
                    }
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "select User_Resume FROM TblResume R LEFT JOIN TblUser_Login UL ON UL.ID = R.User_ID WHERE UL.ID = '" + userID + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            if (!(String.IsNullOrEmpty(userID)))
                            {
                                User_Resume = reader["User_Resume"].ToString();
                            }
                        }
                        else
                        {
                            User_Resume = null;
                        }
                        con.Close();
                    }
                }

                ResumeModel inforesume = new ResumeModel();
                List<ResumeModel> resumelist = new List<ResumeModel>();

                if (User_Resume != null && User_Phone != null)
                {
                    if (User_Driving_License == "Yes")
                    {
                        if (User_Driving_Attach != "" && User_Driving_Class != "")
                        {
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
                        }
                        else
                        {
                            TempData["Message"] = "Please complete the Driving details. Thank you.";

                            return RedirectToAction("AboutMeEdit");


                            //InfoModel info = new InfoModel();

                            //using (SqlConnection con = new SqlConnection(cs))
                            //{
                            //    string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + userID + "'";

                            //    using (SqlCommand cmd = new SqlCommand(commandText))
                            //    {
                            //        SqlDataReader reader;
                            //        cmd.Connection = con;
                            //        con.Open();
                            //        reader = cmd.ExecuteReader();
                            //        reader.Read();

                            //        string userShortName = reader["User_ShortName"].ToString();
                            //        ViewBag.User_ShortName = userShortName;

                            //        List<InfoModel> userlist = new List<InfoModel>();
                            //        if (!(String.IsNullOrEmpty(userID)))
                            //        {
                            //            string s;
                            //            if (reader["User_Driving_Attach"].ToString() == "")
                            //            {
                            //                s = "NO LICENSE ATTACHED";
                            //            }
                            //            else
                            //            {
                            //                s = reader["User_Driving_Attach"].ToString();
                            //            }

                            //            InfoModel uobj = new InfoModel
                            //            {
                            //                User_ShortName = reader["User_ShortName"].ToString(),
                            //                User_Name = reader["User_Name"].ToString(),
                            //                User_Phone = reader["User_Phone"].ToString(),
                            //                User_Tel_Home = reader["User_Tel_Home"].ToString(),
                            //                User_Email = reader["User_Email"].ToString(),
                            //                User_IC = reader["User_IC"].ToString(),
                            //                User_Permanent_Address = reader["User_Permanent_Address"].ToString(),
                            //                User_Correspon_Address = reader["User_Correspon_Address"].ToString(),
                            //                User_Location = reader["User_Location"].ToString(),
                            //                User_Nationality = reader["User_Nationality"].ToString(),
                            //                User_Religion = reader["User_Religion"].ToString(),
                            //                User_Race = reader["User_Race"].ToString(),
                            //                User_Gender = reader["User_Gender"].ToString(),
                            //                User_Age = reader["User_Age"].ToString(),
                            //                User_Marital_Status = reader["User_Marital_Status"].ToString(),
                            //                User_Date_Birth = reader["User_Date_Birth"].ToString(),
                            //                User_Driving_License = reader["User_Driving_License"].ToString(),
                            //                User_Driving_Class = reader["User_Driving_Class"].ToString(),
                            //                User_Driving_Attach = s,
                            //                User_Expected_Salary = reader["User_Expected_Salary"].ToString()
                            //            };
                            //            userlist.Add(uobj);
                            //        }
                            //        info.usersinfo = userlist;
                            //        con.Close();
                            //    }
                            //}

                            //return View("AboutMe", info);
                        }
                    }
                    else
                    {
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
                    }
                }
                else
                {
                    TempData["Message"] = "Please upload your RESUME before applying job. Thank you.";

                    ResumeModel uobj = new ResumeModel
                    {
                        User_Resume = null
                    };
                    resumelist.Add(uobj);
                    inforesume.usersresume = resumelist;

                    return View("ResumeEdit", inforesume);
                }

                return View(jobDeclare);
            }
        }

        //public ActionResult ThankYou(DeclarationModel data)
        public ActionResult ThankYou(string id)
        {
            var Position_ID = id;

            string userID = Session["ID"].ToString();
            string userName = "";
            string regionName = "";
            string DepartID = "";
            string dcID = "";
            string Status_Code = "";
            string IsActive = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "  SELECT TOP 1 SS.Status_Code AS Status_Code,* FROM TblJob_Application JA LEFT JOIN TblSystem_Status SS ON SS.ID = JA.Status_Application WHERE JA.Position_ID ='" + Position_ID + "' and User_ID ='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == false)
                    {
                        Status_Code = null;
                        IsActive = null;
                    }
                    else
                    {
                        Status_Code = reader["Status_Code"].ToString();
                        IsActive = reader["IsActive"].ToString();
                    }
                    con.Close();
                }
            }

            if (Status_Code == "In Progress" && IsActive == "True")
            {
                TempData["Message"] = "Job had applied. Kindly wait for next response from us. Thank you.";

                return RedirectToAction("MyJob");
            }
            else
            {
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
                            DepartID = reader["Depart_ID"].ToString();
                            dcID = reader["DC_ID"].ToString();
                        }
                        con.Close();
                    }
                }

                using (SqlConnection con = new SqlConnection(cs))
                {
                    //string query = "INSERT INTO TblJob_Application (User_Name,Region_Name,Depart_ID,DC_ID, Position_Title, User_ID, Status_Application, IsActive) VALUES(@User_Name,@RegionD_Name,@Depart_ID,@DC_ID, @Position_Title, @User_ID, @Status_Application, @IsActive)"; 
                    string query = "INSERT INTO TblJob_Application (User_Name,Region_Name,Depart_ID,DC_ID, Position_ID, User_ID, Status_Application, IsActive) VALUES(@User_Name,@Region_Name,@Depart_ID,@DC_ID, @Position_ID, @User_ID, @Status_Application, @IsActive)";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        cmd.Connection = con;
                        con.Open();

                        cmd.Parameters.AddWithValue("@User_Name", userName);
                        cmd.Parameters.AddWithValue("@Region_Name", regionName);
                        cmd.Parameters.AddWithValue("@Depart_ID", DepartID);
                        cmd.Parameters.AddWithValue("@DC_ID", dcID);
                        cmd.Parameters.AddWithValue("@Position_ID", id);
                        cmd.Parameters.AddWithValue("@Status_Application", "1");
                        cmd.Parameters.AddWithValue("@User_ID", userID);
                        cmd.Parameters.AddWithValue("@IsActive", "1");

                        foreach (SqlParameter Parameter in cmd.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }

                        var ID = cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }

                return View();
            }
        }

        public ActionResult JobApp(string id)
        {
            var Position_ID = id;

            JobDescripModel jobDesc = new JobDescripModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
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

        public ActionResult ConfirmDelete(string id)
        {
            var JobApp_ID = id;

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

            List<SelectListItem> DC = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT ID, Remark FROM TblRemark_Withrawn ORDER BY Remark ASC";
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
                                Text = sdr["Remark"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.RemarkList = DC;

            JobAppliedModel infojob = new JobAppliedModel();
            List<JobAppliedModel> joblist = new List<JobAppliedModel>();

            JobAppliedModel uobj = new JobAppliedModel
            {
                JobApp_ID = JobApp_ID
            };
            joblist.Add(uobj);
            infojob.jobapplied = joblist;

            return View(infojob);
        }

        public ActionResult ProceedDelete(JobAppliedModel jobapp)
        {
            var Remark = "";
            ViewBag.FromDate = DateTime.Now.ToString("dd-MM-yyyy", new CultureInfo("en-US"));

            jobapp.Withdrawn_Date = ViewBag.FromDate;

            string fullname = "";

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
                string query = "SELECT Remark FROM TblRemark_Withrawn WHERE ID='" + jobapp.Remark_Withdrawn + "'";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    Remark = reader["Remark"].ToString();

                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "UPDATE TblJob_Application SET Withdrawn_By='" + fullname + "', IsActive =0, Remark_Withdrawn ='" + Remark + "',Withdrawn_Date ='" + jobapp.Withdrawn_Date + "' WHERE ID='" + jobapp.JobApp_ID + "'";
                using (SqlCommand cmd1 = new SqlCommand(query))
                {
                    con.Open();
                    cmd1.Connection = con;
                    cmd1.ExecuteNonQuery();
                }
                con.Close();
            }
            return RedirectToAction("MyJob");
        }

        public ActionResult Contact()
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

            ViewData["Message"] = "Your contact page.";

            return View();
        }

        // DISPLAY
        // DISPLAY
        // DISPLAY
        public ActionResult AboutMe()
        {
            string userID = Session["ID"].ToString();

            InfoModel info = new InfoModel();

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

                    List<InfoModel> userlist = new List<InfoModel>();
                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        string s;
                        if (reader["User_Driving_Attach"].ToString() == "")
                        {
                            s = "NO LICENSE ATTACHED";
                        }
                        else
                        {
                            s = reader["User_Driving_Attach"].ToString();
                        }

                        InfoModel uobj = new InfoModel
                        {
                            User_ShortName = reader["User_ShortName"].ToString(),
                            User_Name = reader["User_Name"].ToString(),
                            User_Phone = reader["User_Phone"].ToString(),
                            User_Tel_Home = reader["User_Tel_Home"].ToString(),
                            User_Email = reader["User_Email"].ToString(),
                            User_IC = reader["User_IC"].ToString(),
                            User_Permanent_Address = reader["User_Permanent_Address"].ToString(),
                            User_Correspon_Address = reader["User_Correspon_Address"].ToString(),
                            User_Location = reader["User_Location"].ToString(),
                            User_Nationality = reader["User_Nationality"].ToString(),
                            User_Religion = reader["User_Religion"].ToString(),
                            User_Race = reader["User_Race"].ToString(),
                            User_Gender = reader["User_Gender"].ToString(),
                            User_Age = reader["User_Age"].ToString(),
                            User_Marital_Status = reader["User_Marital_Status"].ToString(),
                            User_Date_Birth = reader["User_Date_Birth"].ToString(),
                            User_Driving_License = reader["User_Driving_License"].ToString(),
                            User_Driving_Class = reader["User_Driving_Class"].ToString(),
                            User_Driving_Attach = s,
                            User_Expected_Salary = reader["User_Expected_Salary"].ToString()
                        };
                        userlist.Add(uobj);
                    }
                    info.usersinfo = userlist;
                    con.Close();
                }
            }
            return View(info);
        }

        public ActionResult Education()
        {
            string School_ID = "";
            string School_ID2 = "";
            string Qua_ID = "";
            string Qua_ID2 = "";

            string Type_School = "NA";
            string Type_School2 = "NA";
            string Type_Qua = "NA";
            string Type_Qua2 = "NA";

            string userID = Session["ID"].ToString();

            EducationModel infoedu = new EducationModel();
            List<EducationModel> edulist = new List<EducationModel>();

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
                string commandText = "SELECT * FROM TblEducation WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            School_ID = reader["User_School1"].ToString();
                            School_ID2 = reader["User_School2"].ToString();
                            Qua_ID = reader["User_Qualification1"].ToString();
                            Qua_ID2 = reader["User_Qualification2"].ToString();
                        }
                    }
                    else
                    {
                        return RedirectToAction("EducationEdit", "Applicant");
                    }

                    con.Close();
                }
            }

            if (School_ID != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_School WHERE ID='" + School_ID + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Type_School = reader["Type_School"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            if (Qua_ID != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Qualification WHERE ID='" + Qua_ID + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Type_Qua = reader["Type_Qualification"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            if (School_ID2 != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_School WHERE ID='" + School_ID2 + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Type_School2 = reader["Type_School"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            if (Qua_ID2 != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Qualification WHERE ID='" + Qua_ID2 + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Type_Qua2 = reader["Type_Qualification"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblEducation WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            string s;
                            if (reader["User_Cert_File"].ToString() == "NA")
                            {
                                s = "NO CERTIFICATE ATTACHED";
                            }
                            else
                            {
                                s = reader["User_Cert_File"].ToString();
                            }

                            EducationModel uobj = new EducationModel
                            {
                                User_School1 = Type_School,
                                User_Institute1 = reader["User_Institute1"].ToString(),
                                User_From_Year1 = reader["User_From_Year1"].ToString(),
                                User_To_Year1 = reader["User_To_Year1"].ToString(),
                                User_Qualification1 = Type_Qua,
                                User_Cert_File = s,
                                User_School2 = Type_School2,
                                User_Institute2 = reader["User_Institute2"].ToString(),
                                User_From_Year2 = reader["User_From_Year2"].ToString(),
                                User_To_Year2 = reader["User_To_Year2"].ToString(),
                                User_Qualification2 = Type_Qua2,
                            };
                            edulist.Add(uobj);
                        }
                        infoedu.userseducation = edulist;
                    }
                    else
                    {
                        return RedirectToAction("EducationEdit", "Applicant");
                    }
                    con.Close();
                }
            }
            return View(infoedu);
        }

        public ActionResult Employment()
        {
            //string Period_ID = ""; ;
            //string Type_Period = "";
            string userID = Session["ID"].ToString();

            EmploymentModel infoemp = new EmploymentModel();
            List<EmploymentModel> emplist = new List<EmploymentModel>();

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
                string commandText = "SELECT * FROM TblEmployment WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            //Period_ID = reader["User_Period"].ToString();
                        }
                    }
                    else
                    {
                        return RedirectToAction("EmploymentEdit", "Applicant");
                    }

                    con.Close();
                }
            }

            //using (SqlConnection con = new SqlConnection(cs))
            //{
            //    string commandText = "SELECT * FROM TblMaster_Period WHERE ID='" + Period_ID + "'";
            //    using (SqlCommand cmd = new SqlCommand(commandText))
            //    {
            //        SqlDataReader reader;
            //        cmd.Connection = con;
            //        con.Open();
            //        reader = cmd.ExecuteReader();
            //        reader.Read();

            //        if (reader.HasRows == true)
            //        {
            //            Type_Period = reader["Type_Period"].ToString();
            //        }
            //        else
            //        {
            //            Type_Period = null;
            //        }

            //        con.Close();
            //    }
            //}

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblEmployment WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            EmploymentModel uobj = new EmploymentModel
                            {
                                User_Company1 = reader["User_Company1"].ToString(),
                                User_CompanyAddress1 = reader["User_CompanyAddress1"].ToString(),
                                User_Status_Employ1 = reader["User_Status_Employ1"].ToString(),
                                User_From_Year1 = reader["User_From_Year1"].ToString(),
                                User_To_Year1 = reader["User_To_Year1"].ToString(),
                                User_LastPosition1 = reader["User_LastPosition1"].ToString(),
                                User_Reason1 = reader["User_Reason1"].ToString(),

                                User_Company2 = reader["User_Company2"].ToString(),
                                User_CompanyAddress2 = reader["User_CompanyAddress2"].ToString(),
                                User_Status_Employ2 = reader["User_Status_Employ2"].ToString(),
                                User_From_Year2 = reader["User_From_Year2"].ToString(),
                                User_To_Year2 = reader["User_To_Year2"].ToString(),
                                User_LastPosition2 = reader["User_LastPosition2"].ToString(),
                                User_Reason2 = reader["User_Reason2"].ToString(),

                                User_Company3 = reader["User_Company3"].ToString(),
                                User_CompanyAddress3 = reader["User_CompanyAddress3"].ToString(),
                                User_Status_Employ3 = reader["User_Status_Employ3"].ToString(),
                                User_From_Year3 = reader["User_From_Year3"].ToString(),
                                User_To_Year3 = reader["User_To_Year3"].ToString(),
                                User_LastPosition3 = reader["User_LastPosition3"].ToString(),
                                User_Reason3 = reader["User_Reason3"].ToString(),

                                User_Period = reader["User_Period"].ToString(),
                                User_Emp_Phone = reader["User_Emp_Phone"].ToString(),
                            };
                            emplist.Add(uobj);
                        }
                        infoemp.usersemployment = emplist;
                    }
                    else
                    {
                        return RedirectToAction("EmploymentEdit", "Applicant");
                    }
                    con.Close();
                }
            }
            return View(infoemp);
        }
        public ActionResult Linguistic()
        {
            string userID = Session["ID"].ToString();

            LinguisticModel infoling = new LinguisticModel();
            List<LinguisticModel> linglist = new List<LinguisticModel>();

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
                string commandText = "SELECT * FROM TblLinguistics WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            LinguisticModel uobj = new LinguisticModel
                            {
                                User_Language1 = reader["User_Language1"].ToString(),
                                User_Spoken1 = reader["User_Spoken1"].ToString(),
                                User_Writing1 = reader["User_Writing1"].ToString(),

                                User_Language2 = reader["User_Language2"].ToString(),
                                User_Spoken2 = reader["User_Spoken2"].ToString(),
                                User_Writing2 = reader["User_Writing2"].ToString(),

                                User_Language3 = reader["User_Language3"].ToString(),
                                User_Spoken3 = reader["User_Spoken3"].ToString(),
                                User_Writing3 = reader["User_Writing3"].ToString()
                            };
                            linglist.Add(uobj);
                        }
                        infoling.usersling = linglist;
                    }
                    else
                    {
                        return RedirectToAction("LinguisticEdit", "Applicant");
                    }
                    con.Close();
                }
            }
            return View(infoling);
        }
        public ActionResult AddInfo()
        {
            string Member_ID = "";
            string Member_ID2 = "";
            string Relay_ID = "";
            string Relay_ID2 = "";

            string Member_Status = "NA";
            string Member_Status2 = "NA";
            string Relay_Status = "NA";
            string Relay_Status2 = "NA";

            string userID = Session["ID"].ToString();

            AddInfoModel infoadd = new AddInfoModel();
            List<AddInfoModel> addInfolist = new List<AddInfoModel>();

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
                string commandText = "SELECT * FROM TblAdditional_Info WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            Member_ID = reader["Status_Member1"].ToString();
                            Member_ID2 = reader["Status_Member2"].ToString();

                            Relay_ID = reader["Name_Relative_Friend_Status1"].ToString();
                            Relay_ID2 = reader["Name_Relative_Friend_Status2"].ToString();
                        }
                    }
                    else
                    {
                        return RedirectToAction("AddInfoEdit", "Applicant");
                    }
                    con.Close();
                }
            }

            if (Member_ID != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + Member_ID + "' AND Status='Membership'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Member_Status = reader["Type_Relation"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            if (Member_ID2 != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + Member_ID2 + "' AND Status='Membership'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Member_Status2 = reader["Type_Relation"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            if (Relay_ID != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + Relay_ID + "' AND Status='Other'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Relay_Status = reader["Type_Relation"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            if (Relay_ID2 != "NA")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + Relay_ID2 + "' AND Status='Other'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (reader.HasRows == true)
                        {
                            Relay_Status2 = reader["Type_Relation"].ToString();
                        }
                        else
                        {
                        }

                        con.Close();
                    }
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblAdditional_Info WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            AddInfoModel uobj = new AddInfoModel
                            {
                                User_Profess1 = reader["User_Profess1"].ToString(),
                                User_Date_Registered1 = reader["User_Date_Registered1"].ToString(),
                                Status_Member1 = Member_Status,
                                User_Profess2 = reader["User_Profess2"].ToString(),
                                User_Date_Registered2 = reader["User_Date_Registered2"].ToString(),
                                Status_Member2 = Member_Status2,

                                Name_Relative_Friend1 = reader["Name_Relative_Friend1"].ToString(),
                                Name_Relative_Friend_Depart1 = reader["Name_Relative_Friend_Depart1"].ToString(),
                                Name_Relative_Friend_Status1 = Relay_Status,
                                Name_Relative_Friend2 = reader["Name_Relative_Friend2"].ToString(),
                                Name_Relative_Friend_Depart2 = reader["Name_Relative_Friend_Depart2"].ToString(),
                                Name_Relative_Friend_Status2 = Relay_Status2,

                                User_Pregnant = reader["User_Pregnant"].ToString(),
                                User_Misconduct = reader["User_Misconduct"].ToString(),
                                User_Convicted_Law = reader["User_Convicted_Law"].ToString(),
                                User_Illness = reader["User_Illness"].ToString(),
                                User_Bankcrupt = reader["User_Bankcrupt"].ToString(),
                            };
                            addInfolist.Add(uobj);
                        }
                        infoadd.usersaddInfo = addInfolist;
                    }
                    else
                    {
                        return RedirectToAction("AddInfoEdit", "Applicant");
                    }
                    con.Close();
                }
            }
            return View(infoadd);
        }
        public ActionResult ContactInfo()
        {
            string RRelay_ID = "";
            string RRelay_ID2 = "";
            string ERelay_ID = "";
            string ERelay_ID2 = "";

            string RRelay_Status = "";
            string RRelay_Status2 = "";
            string ERelay_Status = "";
            string ERelay_Status2 = "";

            string userID = Session["ID"].ToString();

            ContactModel infocontact = new ContactModel();
            List<ContactModel> contactlist = new List<ContactModel>();

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
                string commandText = "SELECT * FROM TblContact WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            RRelay_ID = reader["User_RRelation1"].ToString();
                            RRelay_ID2 = reader["User_RRelation2"].ToString();

                            ERelay_ID = reader["User_ERelation1"].ToString();
                            ERelay_ID2 = reader["User_ERelation2"].ToString();
                        }
                    }
                    else
                    {
                        return RedirectToAction("ContactInfoEdit", "Applicant");
                    }
                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + RRelay_ID + "' AND Status='NotBlooded'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        RRelay_Status = reader["Type_Relation"].ToString();
                    }
                    else
                    {
                        RRelay_Status = null;
                    }

                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + RRelay_ID2 + "' AND Status='NotBlooded'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        RRelay_Status2 = reader["Type_Relation"].ToString();
                    }
                    else
                    {
                        RRelay_Status2 = null;
                    }

                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + ERelay_ID + "' AND Status='Other'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        ERelay_Status = reader["Type_Relation"].ToString();
                    }
                    else
                    {
                        ERelay_Status = null;
                    }

                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Relation WHERE ID='" + ERelay_ID2 + "' AND Status='Other'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        ERelay_Status2 = reader["Type_Relation"].ToString();
                    }
                    else
                    {
                        ERelay_Status2 = null;
                    }

                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblContact WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            ContactModel uobj = new ContactModel
                            {
                                User_RName1 = reader["User_RName1"].ToString(),
                                User_RPhone1 = reader["User_RPhone1"].ToString(),
                                User_ROccu1 = reader["User_ROccu1"].ToString(),
                                User_Known_Year1 = reader["User_KnowN_Year1"].ToString(),
                                User_RRelation1 = RRelay_Status,
                                User_RName2 = reader["User_RName2"].ToString(),
                                User_RPhone2 = reader["User_RPhone2"].ToString(),
                                User_ROccu2 = reader["User_ROccu2"].ToString(),
                                User_Known_Year2 = reader["User_KnowN_Year2"].ToString(),
                                User_RRelation2 = RRelay_Status2,

                                User_EName1 = reader["User_EName1"].ToString(),
                                User_EPhone1 = reader["User_EPhone1"].ToString(),
                                User_EAddress1 = reader["User_EAddress1"].ToString(),
                                User_EOccu1 = reader["User_EOccu1"].ToString(),
                                User_ERelation1 = ERelay_Status,
                                User_EName2 = reader["User_EName2"].ToString(),
                                User_EPhone2 = reader["User_EPhone2"].ToString(),
                                User_EAddress2 = reader["User_EAddress2"].ToString(),
                                User_EOccu2 = reader["User_EOccu2"].ToString(),
                                User_ERelation2 = ERelay_Status2

                            };
                            contactlist.Add(uobj);
                        }
                        infocontact.usersContact = contactlist;
                    }
                    else
                    {
                        return RedirectToAction("ContactInfoEdit", "Applicant");
                    }
                    con.Close();
                }
            }
            return View(infocontact);
        }
        public ActionResult Resume()
        {
            string userID = Session["ID"].ToString();

            ResumeModel inforesume = new ResumeModel();
            List<ResumeModel> resumelist = new List<ResumeModel>();

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
                string commandText = "SELECT * FROM TblResume WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            ResumeModel uobj = new ResumeModel
                            {
                                User_Resume = reader["User_Resume"].ToString(),
                                Uploaded_Resume = reader["Uploaded_Resume"].ToString()
                            };
                            resumelist.Add(uobj);
                        }
                        inforesume.usersresume = resumelist;
                    }
                    else
                    {
                        return RedirectToAction("ResumeEdit", "Applicant");
                    }
                    con.Close();
                }
            }
            return View(inforesume);
        }

        // EDIT
        // EDIT
        // EDIT
        [HttpGet]
        public ActionResult AboutMeEdit()
        {
            string userID = Session["ID"].ToString();
            string Region_Name = "";
            string Region_ID = "";

            List<InfoModel> userlist = new List<InfoModel>();
            InfoModel info = new InfoModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM [TblUser_Login] WHERE ID='" + userID + "'";

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
                            Region_Name = reader["User_Location"].ToString();

                            InfoModel uobj = new InfoModel();
                            uobj.User_ShortName = reader["User_ShortName"].ToString();
                            uobj.User_Last_Name = reader["User_Last_Name"].ToString();
                            uobj.User_Phone = reader["User_Phone"].ToString();
                            uobj.User_Tel_Home = reader["User_Tel_Home"].ToString();
                            uobj.User_Email = reader["User_Email"].ToString();
                            uobj.User_IC = reader["User_IC"].ToString();
                            uobj.User_Permanent_Address = reader["User_Permanent_Address"].ToString();
                            uobj.User_Correspon_Address = reader["User_Correspon_Address"].ToString();
                            uobj.User_Location = reader["User_Location"].ToString();

                            uobj.User_Nationality = reader["User_Nationality"].ToString();
                            uobj.User_Religion = reader["User_Religion"].ToString();
                            uobj.User_Race = reader["User_Race"].ToString();
                            uobj.User_Gender = reader["User_Gender"].ToString();
                            uobj.User_Age = reader["User_Age"].ToString();
                            uobj.User_Marital_Status = reader["User_Marital_Status"].ToString();
                            uobj.User_Date_Birth = reader["User_Date_Birth"].ToString();
                            uobj.User_Driving_License = reader["User_Driving_License"].ToString();
                            uobj.User_Driving_Class = reader["User_Driving_Class"].ToString();
                            uobj.User_Driving_Attach = reader["User_Driving_Attach"].ToString();
                            uobj.User_Expected_Salary = reader["User_Expected_Salary"].ToString();
                            userlist.Add(uobj);
                        }
                        info.usersinfo = userlist;
                    }
                    else
                    {
                        InfoModel uobj = new InfoModel
                        {
                            User_ShortName = null,
                            User_Nationality = null,
                            User_Religion = null,
                            User_Race = null,
                            User_Gender = null,
                            User_Age = null,
                            User_Marital_Status = null,
                            User_Date_Birth = null,
                            User_Driving_License = null,
                            User_Driving_Class = null,
                            User_Driving_Attach = null,
                            User_Expected_Salary = null,
                        };
                        userlist.Add(uobj);
                    }
                    info.usersinfo = userlist;

                    ViewBag.license = info.usersinfo.Select(x => x.User_Driving_License).FirstOrDefault();

                    con.Close();
                }
            }

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Region WHERE Region_Name='" + Region_Name + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        Region_ID = reader["ID"].ToString();
                    }
                    con.Close();
                }
            }

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
            ViewBag.SelectedRegion = Region_ID;

            return View(info);
        }

        [HttpPost]
        public ActionResult AboutMeEdit(InfoModel about)
        {
            string Full_Name = about.User_ShortName + " " + about.User_Last_Name;
            string userID = Session["ID"].ToString();
            string Region_Name = "";
            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;

            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblMaster_Region WHERE ID='" + about.User_Location + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        Region_Name = reader["Region_Name"].ToString();
                    }
                    con.Close();
                }
            }

            if (about.User_Driving_License == "Yes")
            {
                using (SqlConnection con1 = new SqlConnection(cs))
                {
                    string query1 = "UPDATE TblUser_Login SET User_ShortName = '" + about.User_ShortName + "',User_Last_Name = '" + about.User_Last_Name + "',User_Name = '" + Full_Name + "',User_Phone = '" + about.User_Phone + "',User_Tel_Home = '" + about.User_Tel_Home + "',User_Email = '" + about.User_Email + "',User_IC = '" + about.User_IC + "',User_Permanent_Address = '" + about.User_Permanent_Address + "',User_Correspon_Address = '" + about.User_Correspon_Address + "',User_Location = '" + Region_Name + "',User_Nationality = '" + about.User_Nationality + "',User_Religion = '" + about.User_Religion + "',User_Race = '" + about.User_Race + "',User_Gender = '" + about.User_Gender + "',User_Age = '" + about.User_Age + "',User_Marital_Status = '" + about.User_Marital_Status + "',User_Date_Birth = '" + about.User_Date_Birth + "',User_Driving_Class = '" + about.User_Driving_Class + "',User_Driving_License = '" + about.User_Driving_License + "',User_Driving_Attach = '" + about.User_Driving_Attach + "',User_Expected_Salary = '" + about.User_Expected_Salary + "' WHERE ID='" + userID + "'";
                    using (SqlCommand cmd = new SqlCommand(query1))
                    {
                        cmd.Connection = con1;
                        con1.Open();
                        cmd.ExecuteNonQuery();
                        con1.Close();
                    }
                }

                if (about.User_Driving_Class != null || about.DrivingLicense != null || about.User_Driving_Attach != null)
                {
                    if (about.DrivingLicense != null)
                    {
                        string FileName = Path.GetFileNameWithoutExtension(about.DrivingLicense.FileName);
                        string FileExtension = Path.GetExtension(about.DrivingLicense.FileName);

                        FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                        //string UploadPath = ConfigurationManager.AppSettings["UserDrivingL"].ToString();
                        var UploadPath = "DrivingLicense/";

                        if (!Directory.Exists(UploadPath))
                        {
                            Directory.CreateDirectory(UploadPath);
                        }

                        string AttachFile = UploadPath + FileName;
                        about.User_Driving_Attach = FileName;

                        AttachFile = Path.GetFullPath(AttachFile);
                        about.DrivingLicense.SaveAs(AttachFile);

                        if (about.User_Driving_Class != null)
                        {
                            string User_Driving_Class = about.User_Driving_Class;
                        }
                        else
                        {
                            using (SqlConnection con1 = new SqlConnection(cs))
                            {
                                string query1 = "UPDATE TblUser_Login SET User_ShortName = '" + about.User_ShortName + "',User_Last_Name = '" + about.User_Last_Name + "',User_Name = '" + Full_Name + "',User_Phone = '" + about.User_Phone + "',User_Tel_Home = '" + about.User_Tel_Home + "',User_Email = '" + about.User_Email + "',User_IC = '" + about.User_IC + "',User_Permanent_Address = '" + about.User_Permanent_Address + "',User_Correspon_Address = '" + about.User_Correspon_Address + "',User_Location = '" + Region_Name + "',User_Nationality = '" + about.User_Nationality + "',User_Religion = '" + about.User_Religion + "',User_Race = '" + about.User_Race + "',User_Gender = '" + about.User_Gender + "',User_Age = '" + about.User_Age + "',User_Marital_Status = '" + about.User_Marital_Status + "',User_Date_Birth = '" + about.User_Date_Birth + "',User_Driving_Class = '" + about.User_Driving_Class + "',User_Driving_License = '" + about.User_Driving_License + "',User_Driving_Attach = '" + about.User_Driving_Attach + "',User_Expected_Salary = '" + about.User_Expected_Salary + "' WHERE ID='" + userID + "'";
                                using (SqlCommand cmd = new SqlCommand(query1))
                                {
                                    cmd.Connection = con1;
                                    con1.Open();
                                    cmd.ExecuteNonQuery();
                                    con1.Close();
                                }
                            }

                            TempData["Message"] = "Please fill in Driving Class. Thank you.";
                            return RedirectToAction("AboutMeEdit");
                        }
                    }
                    else
                    {
                        //about.DrivingLicense = null;
                        if (about.User_Driving_Attach != null)
                        {
                            string User_Driving_Attach = about.User_Driving_Attach;
                            if (about.User_Driving_Class != null)
                            {
                                string User_Driving_Class = about.User_Driving_Class;
                            }
                            else
                            {
                                using (SqlConnection con1 = new SqlConnection(cs))
                                {
                                    string query1 = "UPDATE TblUser_Login SET User_ShortName = '" + about.User_ShortName + "',User_Last_Name = '" + about.User_Last_Name + "',User_Name = '" + Full_Name + "',User_Phone = '" + about.User_Phone + "',User_Tel_Home = '" + about.User_Tel_Home + "',User_Email = '" + about.User_Email + "',User_IC = '" + about.User_IC + "',User_Permanent_Address = '" + about.User_Permanent_Address + "',User_Correspon_Address = '" + about.User_Correspon_Address + "',User_Location = '" + Region_Name + "',User_Nationality = '" + about.User_Nationality + "',User_Religion = '" + about.User_Religion + "',User_Race = '" + about.User_Race + "',User_Gender = '" + about.User_Gender + "',User_Age = '" + about.User_Age + "',User_Marital_Status = '" + about.User_Marital_Status + "',User_Date_Birth = '" + about.User_Date_Birth + "',User_Driving_Class = '" + about.User_Driving_Class + "',User_Driving_License = '" + about.User_Driving_License + "',User_Driving_Attach = '" + about.User_Driving_Attach + "',User_Expected_Salary = '" + about.User_Expected_Salary + "' WHERE ID='" + userID + "'";
                                    using (SqlCommand cmd = new SqlCommand(query1))
                                    {
                                        cmd.Connection = con1;
                                        con1.Open();
                                        cmd.ExecuteNonQuery();
                                        con1.Close();
                                    }
                                }

                                TempData["Message"] = "Please fill in Driving Class. Thank you.";
                                return RedirectToAction("AboutMeEdit");
                            }
                        }
                        else
                        {
                            if (about.User_Driving_Class != null)
                            {
                                string User_Driving_Class = about.User_Driving_Class;
                                using (SqlConnection con1 = new SqlConnection(cs))
                                {
                                    string query1 = "UPDATE TblUser_Login SET User_ShortName = '" + about.User_ShortName + "',User_Last_Name = '" + about.User_Last_Name + "',User_Name = '" + Full_Name + "',User_Phone = '" + about.User_Phone + "',User_Tel_Home = '" + about.User_Tel_Home + "',User_Email = '" + about.User_Email + "',User_IC = '" + about.User_IC + "',User_Permanent_Address = '" + about.User_Permanent_Address + "',User_Correspon_Address = '" + about.User_Correspon_Address + "',User_Location = '" + Region_Name + "',User_Nationality = '" + about.User_Nationality + "',User_Religion = '" + about.User_Religion + "',User_Race = '" + about.User_Race + "',User_Gender = '" + about.User_Gender + "',User_Age = '" + about.User_Age + "',User_Marital_Status = '" + about.User_Marital_Status + "',User_Date_Birth = '" + about.User_Date_Birth + "',User_Driving_Class = '" + about.User_Driving_Class + "',User_Driving_License = '" + about.User_Driving_License + "',User_Driving_Attach = '" + about.User_Driving_Attach + "',User_Expected_Salary = '" + about.User_Expected_Salary + "' WHERE ID='" + userID + "'";
                                    using (SqlCommand cmd = new SqlCommand(query1))
                                    {
                                        cmd.Connection = con1;
                                        con1.Open();
                                        cmd.ExecuteNonQuery();
                                        con1.Close();
                                    }
                                }
                            }

                            TempData["Message"] = "Please upload Driving License attachment. Thank you.";
                            return RedirectToAction("AboutMeEdit");
                        }
                    }

                    using (SqlConnection con1 = new SqlConnection(cs))
                    {
                        string query1 = "UPDATE TblUser_Login SET User_ShortName = '" + about.User_ShortName + "',User_Last_Name = '" + about.User_Last_Name + "',User_Name = '" + Full_Name + "',User_Phone = '" + about.User_Phone + "',User_Tel_Home = '" + about.User_Tel_Home + "',User_Email = '" + about.User_Email + "',User_IC = '" + about.User_IC + "',User_Permanent_Address = '" + about.User_Permanent_Address + "',User_Correspon_Address = '" + about.User_Correspon_Address + "',User_Location = '" + Region_Name + "',User_Nationality = '" + about.User_Nationality + "',User_Religion = '" + about.User_Religion + "',User_Race = '" + about.User_Race + "',User_Gender = '" + about.User_Gender + "',User_Age = '" + about.User_Age + "',User_Marital_Status = '" + about.User_Marital_Status + "',User_Date_Birth = '" + about.User_Date_Birth + "',User_Driving_Class = '" + about.User_Driving_Class + "',User_Driving_License = '" + about.User_Driving_License + "',User_Driving_Attach = '" + about.User_Driving_Attach + "',User_Expected_Salary = '" + about.User_Expected_Salary + "' WHERE ID='" + userID + "'";
                        using (SqlCommand cmd = new SqlCommand(query1))
                        {
                            cmd.Connection = con1;
                            con1.Open();
                            cmd.ExecuteNonQuery();
                            con1.Close();
                        }
                    }

                    //if (about.DrivingLicense == null)
                    //{
                    //    TempData["Message"] = "Please upload Driving License attachment. Thank you.";
                    //    return RedirectToAction("AboutMeEdit");
                    //}
                    //if (about.User_Driving_Class == null)
                    //{
                    //    TempData["Message"] = "Please fill in Driving Class. Thank you.";
                    //    return RedirectToAction("AboutMeEdit");
                    //}
                }
                else
                {
                    //about.DrivingLicense = null;
                    TempData["Message"] = "Please fill in Driving Class and Driving License attachment. Thank you.";
                    return RedirectToAction("AboutMeEdit");
                }
            }
            else
            {
                about.User_Driving_Attach = null;
                about.User_Driving_Class = null;
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "UPDATE TblUser_Login SET User_ShortName = '" + about.User_ShortName + "',User_Last_Name = '" + about.User_Last_Name + "',User_Name = '" + Full_Name + "',User_Phone = '" + about.User_Phone + "',User_Tel_Home = '" + about.User_Tel_Home + "',User_Email = '" + about.User_Email + "',User_IC = '" + about.User_IC + "',User_Permanent_Address = '" + about.User_Permanent_Address + "',User_Correspon_Address = '" + about.User_Correspon_Address + "',User_Location = '" + Region_Name + "',User_Nationality = '" + about.User_Nationality + "',User_Religion = '" + about.User_Religion + "',User_Race = '" + about.User_Race + "',User_Gender = '" + about.User_Gender + "',User_Age = '" + about.User_Age + "',User_Marital_Status = '" + about.User_Marital_Status + "',User_Date_Birth = '" + about.User_Date_Birth + "',User_Driving_Class = '" + about.User_Driving_Class + "',User_Driving_License = '" + about.User_Driving_License + "',User_Driving_Attach = '" + about.User_Driving_Attach + "',User_Expected_Salary = '" + about.User_Expected_Salary + "' WHERE ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    cmd.Connection = con1;
                    con1.Open();
                    cmd.ExecuteNonQuery();
                    con1.Close();
                }
            }
            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("AboutMe");
        }

        [HttpGet]
        public ActionResult EducationEdit()
        {
            string userID = Session["ID"].ToString();
            string School_ID = "";
            string School_ID2 = "";
            string Qua_ID = "";
            string Qua_ID2 = "";

            EducationModel infoedu = new EducationModel();
            List<EducationModel> edulist = new List<EducationModel>();

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
                string commandText = "SELECT * FROM TblEducation WHERE User_ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            School_ID = reader["User_School1"].ToString();
                            School_ID2 = reader["User_School2"].ToString();
                            Qua_ID = reader["User_Qualification1"].ToString();
                            Qua_ID2 = reader["User_Qualification2"].ToString();

                            EducationModel uobj = new EducationModel();
                            uobj.User_School1 = reader["User_School1"].ToString();
                            uobj.User_Institute1 = reader["User_Institute1"].ToString();
                            uobj.User_From_Year1 = reader["User_From_Year1"].ToString();
                            uobj.User_To_Year1 = reader["User_To_Year1"].ToString();
                            uobj.User_Qualification1 = reader["User_Qualification1"].ToString();
                            uobj.User_Cert_File = reader["User_Cert_File"].ToString();
                            uobj.User_School2 = reader["User_School2"].ToString();
                            uobj.User_Institute2 = reader["User_Institute2"].ToString();
                            uobj.User_From_Year2 = reader["User_From_Year2"].ToString();
                            uobj.User_To_Year2 = reader["User_To_Year2"].ToString();
                            uobj.User_Qualification2 = reader["User_Qualification2"].ToString();
                            edulist.Add(uobj);
                        }
                        infoedu.userseducation = edulist;
                    }
                    else
                    {
                        EducationModel uobj = new EducationModel
                        {
                            User_School1 = null,
                            User_Institute1 = null,
                            User_From_Year1 = null,
                            User_To_Year1 = null,
                            User_Qualification1 = null,
                            User_Cert_File = null,
                            User_School2 = null,
                            User_Institute2 = null,
                            User_From_Year2 = null,
                            User_To_Year2 = null,
                            User_Qualification2 = null,
                        };
                        edulist.Add(uobj);
                    }
                    infoedu.userseducation = edulist;
                    con.Close();
                }
            }

            List<SelectListItem> school = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_School ORDER BY ID Asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            school.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_School"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.SchoolList = school;
            ViewBag.SelectedSchool = School_ID;
            ViewBag.SelectedSchool2 = School_ID2;

            List<SelectListItem> qua = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Qualification ORDER BY ID Asc";
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
            ViewBag.SelectedQua = Qua_ID;
            ViewBag.SelectedQua2 = Qua_ID2;

            return View(infoedu);
        }

        [HttpPost]
        public ActionResult EducationEdit(EducationModel education)
        {
            if (education.CertFile != null)
            {
                string FileName = Path.GetFileNameWithoutExtension(education.CertFile.FileName);
                string FileExtension = Path.GetExtension(education.CertFile.FileName);

                FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

                //string UploadPath = ConfigurationManager.AppSettings["UserCertPath"].ToString();
                var UploadPath = "EducationCert/";

                if (!Directory.Exists(UploadPath))
                {
                    Directory.CreateDirectory(UploadPath);
                }

                string AttachFile = UploadPath + FileName;
                education.User_Cert_File = FileName;

                AttachFile = Path.GetFullPath(AttachFile);
                education.CertFile.SaveAs(AttachFile);

            }
            else
            {
                education.CertFile = null;
            }

            var dataProcess = false;
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "SELECT * FROM TblEducation WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    dataProcess = reader.HasRows;
                }
                con1.Close();
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (dataProcess == true)
                {
                    string query3 = "UPDATE TblEducation SET User_School1 = '" + education.User_School1 + "',User_Institute1 = '" + education.User_Institute1 + "',User_From_Year1 = '" + education.User_From_Year1 + "',User_To_Year1 = '" + education.User_To_Year1 + "',User_Qualification1 = '" + education.User_Qualification1 + "',User_Cert_File = '" + education.User_Cert_File + "',User_School2 = '" + education.User_School2 + "',User_Institute2 = '" + education.User_Institute2 + "',User_From_Year2 = '" + education.User_From_Year2 + "',User_To_Year2 = '" + education.User_To_Year2 + "',User_Qualification2 = '" + education.User_Qualification2 + "' WHERE User_ID='" + userID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query2 = "INSERT INTO TblEducation (User_School1,User_Institute1, User_From_Year1, User_To_Year1, User_Qualification1, User_Cert_File, User_School2,User_Institute2, User_From_Year2,User_To_Year2, User_Qualification2, User_ID) VALUES(@User_School1,@User_Institute1, @User_From_Year1, @User_To_Year1, @User_Qualification1, @User_Cert_File, @User_School2,@User_Institute2, @User_From_Year2,@User_To_Year2, @User_Qualification2,@User_ID)";
                    using (SqlCommand cmd2 = new SqlCommand(query2))
                    {
                        cmd2.Connection = con1;

                        cmd2.Parameters.AddWithValue("@User_School1", education.User_School1);
                        cmd2.Parameters.AddWithValue("@User_Institute1", education.User_Institute1);
                        cmd2.Parameters.AddWithValue("@User_From_Year1", education.User_From_Year1);
                        cmd2.Parameters.AddWithValue("@User_To_Year1", education.User_To_Year1);
                        cmd2.Parameters.AddWithValue("@User_Qualification1", education.User_Qualification1);
                        cmd2.Parameters.AddWithValue("@User_Cert_File", education.User_Cert_File);
                        cmd2.Parameters.AddWithValue("@User_School2", education.User_School2);
                        cmd2.Parameters.AddWithValue("@User_Institute2", education.User_Institute2);
                        cmd2.Parameters.AddWithValue("@User_From_Year2", education.User_From_Year2);
                        cmd2.Parameters.AddWithValue("@User_To_Year2", education.User_To_Year2);
                        cmd2.Parameters.AddWithValue("@User_Qualification2", education.User_Qualification2);
                        cmd2.Parameters.AddWithValue("@User_ID", userID);

                        foreach (SqlParameter Parameter in cmd2.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }
                        education.ID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("Education");
        }


        [HttpGet]
        public ActionResult EmploymentEdit()
        {
            string Period_ID = "";
            string User_Period = "";
            string userID = Session["ID"].ToString();

            EmploymentModel infoemp = new EmploymentModel();
            List<EmploymentModel> emplist = new List<EmploymentModel>();

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
                string commandText = "SELECT * FROM TblEmployment WHERE User_ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            User_Period = reader["User_Period"].ToString();

                            EmploymentModel uobj = new EmploymentModel();
                            uobj.User_Company1 = reader["User_Company1"].ToString();
                            uobj.User_CompanyAddress1 = reader["User_CompanyAddress1"].ToString();
                            uobj.User_Status_Employ1 = reader["User_Status_Employ1"].ToString();
                            uobj.User_From_Year1 = reader["User_From_Year1"].ToString();
                            uobj.User_To_Year1 = reader["User_To_Year1"].ToString();
                            uobj.User_LastPosition1 = reader["User_LastPosition1"].ToString();
                            uobj.User_Reason1 = reader["User_Reason1"].ToString();

                            uobj.User_Company2 = reader["User_Company2"].ToString();
                            uobj.User_CompanyAddress2 = reader["User_CompanyAddress2"].ToString();
                            uobj.User_Status_Employ2 = reader["User_Status_Employ2"].ToString();
                            uobj.User_From_Year2 = reader["User_From_Year2"].ToString();
                            uobj.User_To_Year2 = reader["User_To_Year2"].ToString();
                            uobj.User_LastPosition2 = reader["User_LastPosition2"].ToString();
                            uobj.User_Reason2 = reader["User_Reason2"].ToString();

                            uobj.User_Company3 = reader["User_Company3"].ToString();
                            uobj.User_CompanyAddress3 = reader["User_CompanyAddress3"].ToString();
                            uobj.User_Status_Employ3 = reader["User_Status_Employ3"].ToString();
                            uobj.User_From_Year3 = reader["User_From_Year3"].ToString();
                            uobj.User_To_Year3 = reader["User_To_Year3"].ToString();
                            uobj.User_LastPosition3 = reader["User_LastPosition3"].ToString();
                            uobj.User_Reason3 = reader["User_Reason3"].ToString();

                            uobj.User_Period = reader["User_Period"].ToString();
                            uobj.User_Emp_Phone = reader["User_Emp_Phone"].ToString();
                            emplist.Add(uobj);
                        }
                        infoemp.usersemployment = emplist;
                    }
                    else
                    {
                        EmploymentModel uobj = new EmploymentModel
                        {
                            User_Company1 = null,
                            User_CompanyAddress1 = null,
                            User_Status_Employ1 = null,
                            User_From_Year1 = null,
                            User_To_Year1 = null,
                            User_LastPosition1 = null,
                            User_Reason1 = null,

                            User_Company2 = null,
                            User_CompanyAddress2 = null,
                            User_Status_Employ2 = null,
                            User_From_Year2 = null,
                            User_To_Year2 = null,
                            User_LastPosition2 = null,
                            User_Reason2 = null,

                            User_Company3 = null,
                            User_CompanyAddress3 = null,
                            User_Status_Employ3 = null,
                            User_From_Year3 = null,
                            User_To_Year3 = null,
                            User_LastPosition3 = null,
                            User_Reason3 = null,

                            User_Period = null,
                            User_Emp_Phone = null,
                        };
                        emplist.Add(uobj);
                    }
                    infoemp.usersemployment = emplist;

                    ViewBag.status1 = infoemp.usersemployment.Select(x => x.User_Status_Employ1).FirstOrDefault();
                    ViewBag.status2 = infoemp.usersemployment.Select(x => x.User_Status_Employ2).FirstOrDefault();
                    ViewBag.status3 = infoemp.usersemployment.Select(x => x.User_Status_Employ3).FirstOrDefault();

                    con.Close();
                }
            }

            if (User_Period != "NA" && User_Period != null && User_Period != "")
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Period WHERE Type_Period='" + User_Period + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            Period_ID = reader["ID"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            else { }


            List<SelectListItem> period = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Period ORDER BY ID Asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            period.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Period"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.PeriodList = period;
            ViewBag.SelectedPeriod = Period_ID;

            return View(infoemp);
        }

        [HttpPost]
        public ActionResult EmploymentEdit(EmploymentModel employment)
        {
            var dataProcess = false;
            string User_Period = "";
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "SELECT * FROM TblEmployment WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    dataProcess = reader.HasRows;
                }
                con1.Close();
            }

            if (employment.User_Period != null)
            {
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblMaster_Period WHERE ID='" + employment.User_Period + "'";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            User_Period = reader["Type_Period"].ToString();
                        }
                        con.Close();
                    }
                }
            }
            else
            {
                User_Period = "NA";
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                if (employment.User_Status_Employ1 == "Employed")
                {
                    employment.User_To_Year1 = null;
                }
                else if (employment.User_Status_Employ2 == "Employed")
                {
                    employment.User_To_Year2 = null;
                }
                else if (employment.User_Status_Employ3 == "Employed")
                {
                    employment.User_To_Year3 = null;
                }

                con1.Open();

                if (dataProcess == true)
                {
                    string query3 = "UPDATE TblEmployment SET User_Company1 = '" + employment.User_Company1 + "',User_CompanyAddress1 = '" + employment.User_CompanyAddress1 + "',User_Status_Employ1 = '" + employment.User_Status_Employ1 + "', User_From_Year1 = '" + employment.User_From_Year1 + "',User_To_Year1 = '" + employment.User_To_Year1 + "',User_LastPosition1 = '" + employment.User_LastPosition1 + "',User_Reason1 = '" + employment.User_Reason1 + "', User_Company2 = '" + employment.User_Company2 + "',User_CompanyAddress2 = '" + employment.User_CompanyAddress2 + "',User_Status_Employ2 = '" + employment.User_Status_Employ2 + "',User_From_Year2 = '" + employment.User_From_Year2 + "',User_To_Year2 = '" + employment.User_To_Year2 + "',User_LastPosition2 = '" + employment.User_LastPosition2 + "',User_Reason2 = '" + employment.User_Reason2 + "',User_Company3 = '" + employment.User_Company3 + "',User_CompanyAddress3 = '" + employment.User_CompanyAddress3 + "',User_Status_Employ3 = '" + employment.User_Status_Employ3 + "',User_From_Year3 = '" + employment.User_From_Year3 + "',User_To_Year3 = '" + employment.User_To_Year3 + "',User_LastPosition3 = '" + employment.User_LastPosition3 + "',User_Reason3 = '" + employment.User_Reason3 + "',User_Period = '" + User_Period + "',User_Emp_Phone = '" + employment.User_Emp_Phone + "' WHERE User_ID='" + userID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    if (employment.User_Status_Employ1 == "Employed")
                    {
                        employment.User_To_Year1 = null;
                    }
                    else if (employment.User_Status_Employ2 == "Employed")
                    {
                        employment.User_To_Year2 = null;
                    }
                    else if (employment.User_Status_Employ3 == "Employed")
                    {
                        employment.User_To_Year3 = null;
                    }

                    string query2 = "INSERT INTO TblEmployment (User_Company1,User_CompanyAddress1, User_Status_Employ1, User_From_Year1, User_To_Year1, User_LastPosition1, User_Reason1,User_Company2,User_CompanyAddress2, User_Status_Employ2, User_From_Year2, User_To_Year2, User_LastPosition2, User_Reason2,User_Company3,User_CompanyAddress3, User_Status_Employ3, User_From_Year3, User_To_Year3, User_LastPosition3, User_Reason3, User_Period,User_Emp_Phone, User_ID) VALUES(@User_Company1,@User_CompanyAddress1, @User_Status_Employ1, @User_From_Year1, @User_To_Year1, @User_LastPosition1, @User_Reason1,@User_Company2,@User_CompanyAddress2, @User_Status_Employ2, @User_From_Year2, @User_To_Year2, @User_LastPosition2, @User_Reason2,@User_Company3,@User_CompanyAddress3, @User_Status_Employ3, @User_From_Year3, @User_To_Year3, @User_LastPosition3, @User_Reason3, @User_Period,@User_Emp_Phone, @User_ID)";
                    using (SqlCommand cmd2 = new SqlCommand(query2))
                    {
                        cmd2.Connection = con1;

                        cmd2.Parameters.AddWithValue("@User_Company1", employment.User_Company1);
                        cmd2.Parameters.AddWithValue("@User_CompanyAddress1", employment.User_CompanyAddress1);
                        cmd2.Parameters.AddWithValue("@User_Status_Employ1", employment.User_Status_Employ1);
                        cmd2.Parameters.AddWithValue("@User_From_Year1", employment.User_From_Year1);
                        cmd2.Parameters.AddWithValue("@User_To_Year1", employment.User_To_Year1);
                        cmd2.Parameters.AddWithValue("@User_LastPosition1", employment.User_LastPosition1);
                        cmd2.Parameters.AddWithValue("@User_Reason1", employment.User_Reason1);

                        cmd2.Parameters.AddWithValue("@User_Company2", employment.User_Company2);
                        cmd2.Parameters.AddWithValue("@User_CompanyAddress2", employment.User_CompanyAddress2);
                        cmd2.Parameters.AddWithValue("@User_Status_Employ2", employment.User_Status_Employ2);
                        cmd2.Parameters.AddWithValue("@User_From_Year2", employment.User_From_Year2);
                        cmd2.Parameters.AddWithValue("@User_To_Year2", employment.User_To_Year2);
                        cmd2.Parameters.AddWithValue("@User_LastPosition2", employment.User_LastPosition2);
                        cmd2.Parameters.AddWithValue("@User_Reason2", employment.User_Reason2);

                        cmd2.Parameters.AddWithValue("@User_Company3", employment.User_Company3);
                        cmd2.Parameters.AddWithValue("@User_CompanyAddress3", employment.User_CompanyAddress3);
                        cmd2.Parameters.AddWithValue("@User_Status_Employ3", employment.User_Status_Employ3);
                        cmd2.Parameters.AddWithValue("@User_From_Year3", employment.User_From_Year3);
                        cmd2.Parameters.AddWithValue("@User_To_Year3", employment.User_To_Year3);
                        cmd2.Parameters.AddWithValue("@User_LastPosition3", employment.User_LastPosition3);
                        cmd2.Parameters.AddWithValue("@User_Reason3", employment.User_Reason3);

                        cmd2.Parameters.AddWithValue("@User_Period", User_Period);
                        cmd2.Parameters.AddWithValue("@User_Emp_Phone", employment.User_Emp_Phone);
                        cmd2.Parameters.AddWithValue("@User_ID", userID);

                        foreach (SqlParameter Parameter in cmd2.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }
                        employment.ID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("Employment");
        }

        [HttpGet]
        public ActionResult LinguisticEdit()
        {
            string userID = Session["ID"].ToString();

            LinguisticModel infoling = new LinguisticModel();
            List<LinguisticModel> linglist = new List<LinguisticModel>();

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
                string commandText = "SELECT * FROM TblLinguistics WHERE User_ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            LinguisticModel uobj = new LinguisticModel();
                            uobj.User_Language1 = reader["User_Language1"].ToString();
                            uobj.User_Spoken1 = reader["User_Spoken1"].ToString();
                            uobj.User_Writing1 = reader["User_Writing1"].ToString();

                            uobj.User_Language2 = reader["User_Language2"].ToString();
                            uobj.User_Spoken2 = reader["User_Spoken2"].ToString();
                            uobj.User_Writing2 = reader["User_Writing2"].ToString();

                            uobj.User_Language3 = reader["User_Language3"].ToString();
                            uobj.User_Spoken3 = reader["User_Spoken3"].ToString();
                            uobj.User_Writing3 = reader["User_Writing3"].ToString();
                            linglist.Add(uobj);
                        }
                        infoling.usersling = linglist;
                    }
                    else
                    {
                        LinguisticModel uobj = new LinguisticModel
                        {
                            User_Language1 = null,
                            User_Spoken1 = null,
                            User_Writing1 = null,

                            User_Language2 = null,
                            User_Spoken2 = null,
                            User_Writing2 = null,

                            User_Language3 = null,
                            User_Spoken3 = null,
                            User_Writing3 = null
                        };
                        linglist.Add(uobj);
                    }
                    infoling.usersling = linglist;
                    con.Close();
                }
            }
            return View(infoling);

        }

        [HttpPost]
        public ActionResult LinguisticEdit(LinguisticModel linguistic)
        {
            var dataProcess = false;
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "SELECT * FROM TblLinguistics WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    dataProcess = reader.HasRows;
                }
                con1.Close();
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (dataProcess == true)
                {
                    string query3 = "UPDATE TblLinguistics SET User_Language1 = '" + linguistic.User_Language1 + "',User_Spoken1 = '" + linguistic.User_Spoken1 + "',User_Writing1 = '" + linguistic.User_Writing1 + "',User_Language2 = '" + linguistic.User_Language2 + "',User_Spoken2 = '" + linguistic.User_Spoken2 + "',User_Writing2 = '" + linguistic.User_Writing2 + "',User_Language3 = '" + linguistic.User_Language3 + "',User_Spoken3 = '" + linguistic.User_Spoken3 + "',User_Writing3 = '" + linguistic.User_Writing3 + "' WHERE User_ID='" + userID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query2 = "INSERT INTO TblLinguistics (User_Language1,User_Spoken1, User_Writing1,User_Language2,User_Spoken2, User_Writing2,User_Language3,User_Spoken3, User_Writing3, User_ID) VALUES(@User_Language1,@User_Spoken1, @User_Writing1,@User_Language2,@User_Spoken2, @User_Writing2,@User_Language3,@User_Spoken3, @User_Writing3, @User_ID)";
                    using (SqlCommand cmd2 = new SqlCommand(query2))
                    {
                        cmd2.Connection = con1;

                        cmd2.Parameters.AddWithValue("@User_Language1", linguistic.User_Language1);
                        cmd2.Parameters.AddWithValue("@User_Spoken1", linguistic.User_Spoken1);
                        cmd2.Parameters.AddWithValue("@User_Writing1", linguistic.User_Writing1);

                        cmd2.Parameters.AddWithValue("@User_Language2", linguistic.User_Language2);
                        cmd2.Parameters.AddWithValue("@User_Spoken2", linguistic.User_Spoken2);
                        cmd2.Parameters.AddWithValue("@User_Writing2", linguistic.User_Writing2);

                        cmd2.Parameters.AddWithValue("@User_Language3", linguistic.User_Language3);
                        cmd2.Parameters.AddWithValue("@User_Spoken3", linguistic.User_Spoken3);
                        cmd2.Parameters.AddWithValue("@User_Writing3", linguistic.User_Writing3);

                        cmd2.Parameters.AddWithValue("@User_ID", userID);

                        foreach (SqlParameter Parameter in cmd2.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }
                        linguistic.ID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("Linguistic");
        }

        [HttpGet]
        public ActionResult AddInfoEdit()
        {
            string Member_ID = "";
            string Member_ID2 = "";
            string Relay_ID = "";
            string Relay_ID2 = "";

            string userID = Session["ID"].ToString();

            AddInfoModel infoadd = new AddInfoModel();
            List<AddInfoModel> addInfolist = new List<AddInfoModel>();

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
                string commandText = "SELECT * FROM TblAdditional_Info WHERE User_ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            Member_ID = reader["Status_Member1"].ToString();
                            Member_ID2 = reader["Status_Member2"].ToString();

                            Relay_ID = reader["Name_Relative_Friend_Status1"].ToString();
                            Relay_ID2 = reader["Name_Relative_Friend_Status2"].ToString();

                            AddInfoModel uobj = new AddInfoModel();
                            uobj.User_Profess1 = reader["User_Profess1"].ToString();
                            uobj.User_Date_Registered1 = reader["User_Date_Registered1"].ToString();
                            uobj.Status_Member1 = reader["Status_Member1"].ToString();
                            uobj.User_Profess2 = reader["User_Profess2"].ToString();
                            uobj.User_Date_Registered2 = reader["User_Date_Registered2"].ToString();
                            uobj.Status_Member2 = reader["Status_Member2"].ToString();

                            uobj.Name_Relative_Friend1 = reader["Name_Relative_Friend1"].ToString();
                            uobj.Name_Relative_Friend_Depart1 = reader["Name_Relative_Friend_Depart1"].ToString();
                            uobj.Name_Relative_Friend_Status1 = reader["Name_Relative_Friend_Status1"].ToString();
                            uobj.Name_Relative_Friend2 = reader["Name_Relative_Friend2"].ToString();
                            uobj.Name_Relative_Friend_Depart2 = reader["Name_Relative_Friend_Depart2"].ToString();
                            uobj.Name_Relative_Friend_Status2 = reader["Name_Relative_Friend_Status2"].ToString();

                            uobj.User_Pregnant = reader["User_Pregnant"].ToString();
                            uobj.User_Misconduct = reader["User_Misconduct"].ToString();
                            uobj.User_Convicted_Law = reader["User_Convicted_Law"].ToString();
                            uobj.User_Illness = reader["User_Illness"].ToString();
                            uobj.User_Bankcrupt = reader["User_Bankcrupt"].ToString();
                            addInfolist.Add(uobj);
                        }
                        infoadd.usersaddInfo = addInfolist;
                    }
                    else
                    {
                        AddInfoModel uobj = new AddInfoModel
                        {
                            User_Profess1 = null,
                            User_Date_Registered1 = null,
                            Status_Member1 = null,
                            User_Profess2 = null,
                            User_Date_Registered2 = null,
                            Status_Member2 = null,

                            Name_Relative_Friend1 = null,
                            Name_Relative_Friend_Depart1 = null,
                            Name_Relative_Friend_Status1 = null,
                            Name_Relative_Friend2 = null,
                            Name_Relative_Friend_Depart2 = null,
                            Name_Relative_Friend_Status2 = null,

                            User_Pregnant = null,
                            User_Misconduct = null,
                            User_Convicted_Law = null,
                            User_Illness = null,
                            User_Bankcrupt = null
                        };
                        addInfolist.Add(uobj);
                    }
                    infoadd.usersaddInfo = addInfolist;
                    con.Close();
                }
            }

            List<SelectListItem> relation1 = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Relation where Status = 'Membership' order by Type_Relation asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            relation1.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Relation"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.MemberList = relation1;
            ViewBag.SelectedMember = Member_ID;
            ViewBag.SelectedMember2 = Member_ID2;

            List<SelectListItem> relation2 = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Relation where Status = 'Other' order by Type_Relation asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            relation2.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Relation"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.OtherList = relation2;
            ViewBag.SelectedOther = Relay_ID;
            ViewBag.SelectedOther2 = Relay_ID2;

            return View(infoadd);
        }

        [HttpPost]
        public ActionResult AddInfoEdit(AddInfoModel addInfo)
        {
            var dataProcess = false;
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "SELECT * FROM TblAdditional_Info WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    dataProcess = reader.HasRows;
                }
                con1.Close();
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (dataProcess == true)
                {
                    string query3 = "UPDATE TblAdditional_Info SET User_Profess1 = '" + addInfo.User_Profess1 + "',User_Date_Registered1 = '" + addInfo.User_Date_Registered1 + "',Status_Member1 = '" + addInfo.Status_Member1 + "',User_Profess2 = '" + addInfo.User_Profess2 + "',User_Date_Registered2 = '" + addInfo.User_Date_Registered2 + "',Status_Member2 = '" + addInfo.Status_Member2 + "',Name_Relative_Friend1 = '" + addInfo.Name_Relative_Friend1 + "',Name_Relative_Friend_Depart1 = '" + addInfo.Name_Relative_Friend_Depart1 + "',Name_Relative_Friend_Status1 = '" + addInfo.Name_Relative_Friend_Status1 + "',Name_Relative_Friend2 = '" + addInfo.Name_Relative_Friend2 + "',Name_Relative_Friend_Depart2 = '" + addInfo.Name_Relative_Friend_Depart2 + "',Name_Relative_Friend_Status2 = '" + addInfo.Name_Relative_Friend_Status2 + "',User_Pregnant = '" + addInfo.User_Pregnant + "',User_Misconduct = '" + addInfo.User_Misconduct + "',User_Convicted_Law = '" + addInfo.User_Convicted_Law + "',User_Illness = '" + addInfo.User_Illness + "',User_Bankcrupt = '" + addInfo.User_Bankcrupt + "' WHERE User_ID='" + userID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query2 = "INSERT INTO TblAdditional_Info (User_Profess1,User_Date_Registered1, Status_Member1,User_Profess2,User_Date_Registered2, Status_Member2,Name_Relative_Friend1,Name_Relative_Friend_Depart1,Name_Relative_Friend_Status1,Name_Relative_Friend2,Name_Relative_Friend_Depart2,Name_Relative_Friend_Status2,User_Pregnant,User_Misconduct,User_Convicted_Law,User_Illness,User_Bankcrupt, User_ID) VALUES(@User_Profess1,@User_Date_Registered1, @Status_Member1,@User_Profess2,@User_Date_Registered2, @Status_Member2,@Name_Relative_Friend1,@Name_Relative_Friend_Depart1,@Name_Relative_Friend_Status1, @Name_Relative_Friend2,@Name_Relative_Friend_Depart2,@Name_Relative_Friend_Status2,@User_Pregnant,@User_Misconduct,@User_Convicted_Law,@User_Illness,@User_Bankcrupt,@User_ID)";
                    using (SqlCommand cmd2 = new SqlCommand(query2))
                    {
                        cmd2.Connection = con1;

                        cmd2.Parameters.AddWithValue("@User_Profess1", addInfo.User_Profess1);
                        cmd2.Parameters.AddWithValue("@User_Date_Registered1", addInfo.User_Date_Registered1);
                        cmd2.Parameters.AddWithValue("@Status_Member1", addInfo.Status_Member1);
                        cmd2.Parameters.AddWithValue("@User_Profess2", addInfo.User_Profess2);
                        cmd2.Parameters.AddWithValue("@User_Date_Registered2", addInfo.User_Date_Registered2);
                        cmd2.Parameters.AddWithValue("@Status_Member2", addInfo.Status_Member2);

                        cmd2.Parameters.AddWithValue("@Name_Relative_Friend1", addInfo.Name_Relative_Friend1);
                        cmd2.Parameters.AddWithValue("@Name_Relative_Friend_Depart1", addInfo.Name_Relative_Friend_Depart1);
                        cmd2.Parameters.AddWithValue("@Name_Relative_Friend_Status1", addInfo.Name_Relative_Friend_Status1);
                        cmd2.Parameters.AddWithValue("@Name_Relative_Friend2", addInfo.Name_Relative_Friend2);
                        cmd2.Parameters.AddWithValue("@Name_Relative_Friend_Depart2", addInfo.Name_Relative_Friend_Depart2);
                        cmd2.Parameters.AddWithValue("@Name_Relative_Friend_Status2", addInfo.Name_Relative_Friend_Status2);

                        cmd2.Parameters.AddWithValue("@User_Pregnant", addInfo.User_Pregnant);
                        cmd2.Parameters.AddWithValue("@User_Misconduct", addInfo.User_Misconduct);
                        cmd2.Parameters.AddWithValue("@User_Convicted_Law", addInfo.User_Convicted_Law);
                        cmd2.Parameters.AddWithValue("@User_Illness", addInfo.User_Illness);
                        cmd2.Parameters.AddWithValue("@User_Bankcrupt", addInfo.User_Bankcrupt);

                        cmd2.Parameters.AddWithValue("@User_ID", userID);

                        foreach (SqlParameter Parameter in cmd2.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }
                        addInfo.ID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("AddInfo");
        }

        [HttpGet]
        public ActionResult ContactInfoEdit()
        {
            string RRelay_ID = "";
            string RRelay_ID2 = "";
            string ERelay_ID = "";
            string ERelay_ID2 = "";

            string userID = Session["ID"].ToString();

            ContactModel infocontact = new ContactModel();
            List<ContactModel> contactlist = new List<ContactModel>();

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
                string commandText = "SELECT * FROM TblContact WHERE User_ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            RRelay_ID = reader["User_RRelation1"].ToString();
                            RRelay_ID2 = reader["User_RRelation2"].ToString();

                            ERelay_ID = reader["User_ERelation1"].ToString();
                            ERelay_ID2 = reader["User_ERelation2"].ToString();

                            ContactModel uobj = new ContactModel();
                            uobj.User_RName1 = reader["User_RName1"].ToString();
                            uobj.User_RPhone1 = reader["User_RPhone1"].ToString();
                            uobj.User_ROccu1 = reader["User_ROccu1"].ToString();
                            uobj.User_Known_Year1 = reader["User_KnowN_Year1"].ToString();
                            uobj.User_RRelation1 = reader["User_RRelation1"].ToString();
                            uobj.User_RName2 = reader["User_RName2"].ToString();
                            uobj.User_RPhone2 = reader["User_RPhone2"].ToString();
                            uobj.User_ROccu2 = reader["User_ROccu2"].ToString();
                            uobj.User_Known_Year2 = reader["User_KnowN_Year2"].ToString();
                            uobj.User_RRelation2 = reader["User_RRelation2"].ToString();

                            uobj.User_EName1 = reader["User_EName1"].ToString();
                            uobj.User_EPhone1 = reader["User_EPhone1"].ToString();
                            uobj.User_EAddress1 = reader["User_EAddress1"].ToString();
                            uobj.User_EOccu1 = reader["User_EOccu1"].ToString();
                            uobj.User_ERelation1 = reader["User_ERelation1"].ToString();
                            uobj.User_EName2 = reader["User_EName2"].ToString();
                            uobj.User_EPhone2 = reader["User_EPhone2"].ToString();
                            uobj.User_EAddress2 = reader["User_EAddress2"].ToString();
                            uobj.User_EOccu2 = reader["User_EOccu2"].ToString();
                            uobj.User_ERelation2 = reader["User_ERelation2"].ToString();
                            contactlist.Add(uobj);
                        }
                        infocontact.usersContact = contactlist;
                    }
                    else
                    {
                        ContactModel uobj = new ContactModel
                        {
                            User_RName1 = null,
                            User_RPhone1 = null,
                            User_ROccu1 = null,
                            User_Known_Year1 = null,
                            User_RRelation1 = null,
                            User_RName2 = null,
                            User_RPhone2 = null,
                            User_ROccu2 = null,
                            User_Known_Year2 = null,
                            User_RRelation2 = null,

                            User_EName1 = null,
                            User_EPhone1 = null,
                            User_EAddress1 = null,
                            User_EOccu1 = null,
                            User_ERelation1 = null,
                            User_EName2 = null,
                            User_EPhone2 = null,
                            User_EAddress2 = null,
                            User_EOccu2 = null,
                            User_ERelation2 = null,
                        };
                        contactlist.Add(uobj);
                    }
                    infocontact.usersContact = contactlist;
                    con.Close();
                }
            }

            List<SelectListItem> relation1 = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Relation where Status = 'NotBlooded' order by Type_Relation asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            relation1.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Relation"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.NotBloodList = relation1;
            ViewBag.SelectedNotBlood = RRelay_ID;
            ViewBag.SelectedNotBlood2 = RRelay_ID2;

            List<SelectListItem> relation2 = new List<SelectListItem>();
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "SELECT * FROM TblMaster_Relation where Status = 'Other' order by Type_Relation asc";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {
                            relation2.Add(new SelectListItem
                            {
                                Value = sdr["ID"].ToString(),
                                Text = sdr["Type_Relation"].ToString()
                            });
                        }
                    }
                    con.Close();
                }
            }
            ViewBag.OtherList = relation2;
            ViewBag.SelectedOther = ERelay_ID;
            ViewBag.SelectedOther2 = ERelay_ID2;

            return View(infocontact);

        }

        [HttpPost]
        public ActionResult ContactInfoEdit(ContactModel contact)
        {
            var dataProcess = false;
            string userID = Session["ID"].ToString();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "SELECT * FROM TblContact WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    dataProcess = reader.HasRows;
                }
                con1.Close();
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (dataProcess == true)
                {
                    string query3 = "UPDATE TblContact SET User_RName1 = '" + contact.User_RName1 + "',User_RPhone1 = '" + contact.User_RPhone1 + "',User_ROccu1 = '" + contact.User_ROccu1 + "',User_Known_Year1 = '" + contact.User_Known_Year1 + "',User_RRelation1 = '" + contact.User_RRelation1 + "',User_RName2 = '" + contact.User_RName2 + "',User_RPhone2 = '" + contact.User_RPhone2 + "',User_ROccu2 = '" + contact.User_ROccu2 + "',User_Known_Year2 = '" + contact.User_Known_Year2 + "',User_RRelation2 = '" + contact.User_RRelation2 + "',User_EName1 = '" + contact.User_EName1 + "',User_EPhone1 = '" + contact.User_EPhone1 + "',User_EAddress1 = '" + contact.User_EAddress1 + "',User_EOccu1 = '" + contact.User_EOccu1 + "',User_ERelation1 = '" + contact.User_ERelation1 + "',User_EName2 = '" + contact.User_EName2 + "',User_EPhone2 = '" + contact.User_EPhone2 + "',User_EAddress2 = '" + contact.User_EAddress2 + "',User_EOccu2 = '" + contact.User_EOccu2 + "',User_ERelation2 = '" + contact.User_ERelation2 + "' WHERE User_ID='" + userID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query2 = "INSERT INTO TblContact (User_RName1,User_RPhone1, User_ROccu1,User_Known_Year1, User_RRelation1,User_RName2,User_RPhone2, User_ROccu2,User_Known_Year2, User_RRelation2,User_EName1,User_EPhone1, User_EAddress1,User_EOccu1, User_ERelation1,User_EName2,User_EPhone2, User_EAddress2,User_EOccu2, User_ERelation2, User_ID) VALUES(@User_RName1,@User_RPhone1, @User_ROccu1,@User_Known_Year1, @User_RRelation1,@User_RName2,@User_RPhone2, @User_ROccu2,@User_Known_Year2, @User_RRelation2,@User_EName1,@User_EPhone1, @User_EAddress1,@User_EOccu1, @User_ERelation1,@User_EName2,@User_EPhone2, @User_EAddress2,@User_EOccu2, @User_ERelation2, @User_ID)";
                    using (SqlCommand cmd2 = new SqlCommand(query2))
                    {
                        cmd2.Connection = con1;

                        cmd2.Parameters.AddWithValue("@User_RName1", contact.User_RName1);
                        cmd2.Parameters.AddWithValue("@User_RPhone1", contact.User_RPhone1);
                        cmd2.Parameters.AddWithValue("@User_ROccu1", contact.User_ROccu1);
                        cmd2.Parameters.AddWithValue("@User_Known_Year1", contact.User_Known_Year1);
                        cmd2.Parameters.AddWithValue("@User_RRelation1", contact.User_RRelation1);
                        cmd2.Parameters.AddWithValue("@User_RName2", contact.User_RName2);
                        cmd2.Parameters.AddWithValue("@User_RPhone2", contact.User_RPhone2);
                        cmd2.Parameters.AddWithValue("@User_ROccu2", contact.User_ROccu2);
                        cmd2.Parameters.AddWithValue("@User_Known_Year2", contact.User_Known_Year2);
                        cmd2.Parameters.AddWithValue("@User_RRelation2", contact.User_RRelation2);

                        cmd2.Parameters.AddWithValue("@User_EName1", contact.User_EName1);
                        cmd2.Parameters.AddWithValue("@User_EPhone1", contact.User_EPhone1);
                        cmd2.Parameters.AddWithValue("@User_EAddress1", contact.User_EAddress1);
                        cmd2.Parameters.AddWithValue("@User_EOccu1", contact.User_EOccu1);
                        cmd2.Parameters.AddWithValue("@User_ERelation1", contact.User_ERelation1);
                        cmd2.Parameters.AddWithValue("@User_EName2", contact.User_EName2);
                        cmd2.Parameters.AddWithValue("@User_EPhone2", contact.User_EPhone2);
                        cmd2.Parameters.AddWithValue("@User_EAddress2", contact.User_EAddress2);
                        cmd2.Parameters.AddWithValue("@User_EOccu2", contact.User_EOccu2);
                        cmd2.Parameters.AddWithValue("@User_ERelation2", contact.User_ERelation2);
                        cmd2.Parameters.AddWithValue("@User_ID", userID);

                        foreach (SqlParameter Parameter in cmd2.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }
                        contact.ID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("ContactInfo");
        }

        [HttpGet]
        public ActionResult ResumeEdit()
        {
            string userID = Session["ID"].ToString();

            ResumeModel inforesume = new ResumeModel();
            List<ResumeModel> resumelist = new List<ResumeModel>();

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
                string commandText = "SELECT * FROM TblResume WHERE User_ID='" + userID + "'";

                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (reader.HasRows == true)
                    {
                        if (!(String.IsNullOrEmpty(userID)))
                        {
                            ResumeModel uobj = new ResumeModel();
                            uobj.User_Resume = reader["User_Resume"].ToString();
                            resumelist.Add(uobj);
                        }
                        inforesume.usersresume = resumelist;
                    }
                    else
                    {
                        ResumeModel uobj = new ResumeModel
                        {
                            User_Resume = null
                        };
                        resumelist.Add(uobj);
                    }
                    inforesume.usersresume = resumelist;
                    con.Close();
                }
            }
            return View(inforesume);

        }

        [HttpPost]
        public ActionResult ResumeEdit(ResumeModel resume)
        {
            string FileName = Path.GetFileNameWithoutExtension(resume.ResumeFile.FileName);
            string FileExtension = Path.GetExtension(resume.ResumeFile.FileName);

            FileName = DateTime.Now.ToString("yyyyMMdd") + "-" + FileName.Trim() + FileExtension;

            string dt = DateTime.Now.ToString("dd-MM-yyyy hh:mm tt");

            //string UploadPath = ConfigurationManager.AppSettings["UserResumePath"].ToString();
            var UploadPath = "Resume/";

            if (!Directory.Exists(UploadPath))
            {
                Directory.CreateDirectory(UploadPath);
            }

            string AttachFile = UploadPath + FileName;
            resume.User_Resume = FileName;
            resume.Uploaded_Resume = dt;

            //To copy and save file into server.  
            AttachFile = Path.GetFullPath(AttachFile);
            resume.ResumeFile.SaveAs(AttachFile);

            var dataProcess = false;
            string userID = Session["ID"].ToString();

            string updateDate = DateTime.Now.ToString("dd/MM/yyyy h:mm");

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con1 = new SqlConnection(cs))
            {
                string query1 = "SELECT * FROM TblResume WHERE User_ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(query1))
                {
                    SqlDataReader reader;
                    cmd.Connection = con1;
                    con1.Open();
                    reader = cmd.ExecuteReader();
                    dataProcess = reader.HasRows;
                }
                con1.Close();
            }

            using (SqlConnection con1 = new SqlConnection(cs))
            {
                con1.Open();

                if (dataProcess == true)
                {
                    //string query3 = "UPDATE TblResume SET User_Resume = '" + resume.User_Resume + "',Uploaded_Resume = '" + resume.Uploaded_Resume + "',Created_Date = '" + updateDate + "' WHERE User_ID='" + userID + "'";
                    string query3 = "UPDATE TblResume SET User_Resume = '" + resume.User_Resume + "',Uploaded_Resume = '" + resume.Uploaded_Resume + "' WHERE User_ID='" + userID + "'";
                    using (SqlCommand cmd1 = new SqlCommand(query3))
                    {
                        cmd1.Connection = con1;
                        cmd1.ExecuteNonQuery();
                    }
                }
                else
                {
                    string query2 = "INSERT INTO TblResume (User_Resume,Uploaded_Resume, User_ID,IsActive) VALUES(@User_Resume,@Uploaded_Resume, @User_ID,@IsActive)";
                    using (SqlCommand cmd2 = new SqlCommand(query2))
                    {
                        cmd2.Connection = con1;

                        cmd2.Parameters.AddWithValue("@User_Resume", resume.User_Resume);
                        cmd2.Parameters.AddWithValue("@Uploaded_Resume", resume.Uploaded_Resume);
                        cmd2.Parameters.AddWithValue("@User_ID", userID);
                        cmd2.Parameters.AddWithValue("@IsActive", "1");

                        foreach (SqlParameter Parameter in cmd2.Parameters)
                        {
                            if (Parameter.Value == null)
                            {
                                Parameter.Value = "NA";
                            }
                        }
                        resume.ID = Convert.ToInt32(cmd2.ExecuteScalar());
                    }
                }
                con1.Close();
            }

            ViewBag.Message = "Form updated successfully.";
            return RedirectToAction("Resume");
        }

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

        public ActionResult MyJob()
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
                string commandText = "SELECT JA.ID as JobApp_ID, JA.User_ID as user_ID, User_Name, MR.Region_Name,Dep_Name, DC_Code, Position_Name, SS.Status_Code,JA.Created_Date,JA.Position_ID from TblJob_Application JA LEFT JOIN TblMaster_Position P ON JA.Position_ID = P.Position_ID LEFT JOIN TblMaster_DC MD ON P.DC_ID = MD.ID LEFT JOIN TblMaster_Region MR ON MD.Region_ID = MR.ID LEFT JOIN TblMaster_Department MP ON P.Depart_ID = MP.ID LEFT JOIN TblSystem_Status SS ON SS.ID=JA.Status_Application WHERE JA.User_ID = '" + userID + "' and JA.IsActive=1 ORDER BY JA.CREATED_DATE DESC";

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
                                JobApp_ID = reader["JobApp_ID"].ToString(),
                                User_ID = reader["user_ID"].ToString(),
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

        // DOWNLOAD FILE
        // DOWNLOAD FILE
        // DOWNLOAD FILE

        public FileResult DownloadDL()
        {
            string fileName = "";
            //TempData["Message"] = "Please complete ALL profile details before applying job. Thank you.";

            string userID = Session["ID"].ToString();
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
            //string UploadPath = ConfigurationManager.AppSettings["UserDrivingL"].ToString();
            var UploadPath = "DrivingLicense/";

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

        public FileResult DownloadCert()
        {
            string fileName = "";
            string userID = Session["ID"].ToString();
            string User_Cert_File = "";
            string UserName = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT *, UL.User_ShortName FROM TblEducation TE LEFT JOIN TblUser_Login UL on UL.ID = TE.User_ID WHERE UL.ID='" + userID + "'";
                using (SqlCommand cmd = new SqlCommand(commandText))
                {
                    SqlDataReader reader;
                    cmd.Connection = con;
                    con.Open();
                    reader = cmd.ExecuteReader();
                    reader.Read();

                    if (!(String.IsNullOrEmpty(userID)))
                    {
                        User_Cert_File = reader["User_Cert_File"].ToString();
                        UserName = reader["User_ShortName"].ToString();
                    }
                    con.Close();
                }
            }

            //string UploadPath = ConfigurationManager.AppSettings["UserCertPath"].ToString();
            var UploadPath = "EducationCert/";
            string FullNameFile = UploadPath + User_Cert_File;

            string File_Name = User_Cert_File.Substring(0, User_Cert_File.LastIndexOf('.'));

            string extension = User_Cert_File.Substring(User_Cert_File.LastIndexOf('.'), User_Cert_File.Length - User_Cert_File.LastIndexOf('.'));

            byte[] fileBytes = System.IO.File.ReadAllBytes(FullNameFile);

            if (extension == ".pdf" || extension == ".PDF")
            {
                fileName = UserName + "-Certificates.pdf";
                return File(fileBytes, "application/octet-stream", fileName);
            }
            else
            {
                fileName = UserName + "-Certificates-" + User_Cert_File;
                return File(fileBytes, "application/octet-stream", fileName);
            }
        }

        public FileResult DownloadResume()
        {
            string fileName = "";
            string userID = Session["ID"].ToString();
            string User_Resume = "";
            string UserName = "";

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * , UL.User_ShortName FROM TblResume R LEFT JOIN TblUser_Login UL on UL.ID=R.User_ID WHERE UL.ID='" + userID + "'";
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

            //string UploadPath = ConfigurationManager.AppSettings["UserResumePath"].ToString();
            var UploadPath = "Resume/";
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
            var User_ID = id;

            InfoModel infodetail = new InfoModel();

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string commandText = "SELECT * FROM TblUser_Login WHERE ID='" + User_ID + "'";
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
                        //string s;
                        //if (reader["User_Resume"].ToString() == "")
                        //{
                        //    s = "NO FILE";
                        //}
                        //else
                        //{
                        //    s = reader["User_Resume"].ToString();
                        //}
                        InfoModel uobj = new InfoModel
                        {
                            User_Name = reader["User_Name"].ToString(),
                            User_IC = reader["User_IC"].ToString(),
                            User_Permanent_Address = reader["User_Permanent_Address"].ToString(),
                            User_Correspon_Address = reader["User_Correspon_Address"].ToString(),
                            User_Location = reader["User_Location"].ToString(),
                            User_Phone = reader["User_Phone"].ToString(),
                            User_Tel_Home = reader["User_Tel_Home"].ToString(),
                            User_Email = reader["User_Email"].ToString(),
                            User_Resume = reader["User_Resume"].ToString()
                        };
                        info.Add(uobj);
                    }
                    infodetail.usersinfo = info;
                    con.Close();
                }
            }

            return View(infodetail);
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