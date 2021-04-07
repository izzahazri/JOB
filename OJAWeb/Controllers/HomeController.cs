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
using System.Net;

namespace OJAWeb.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Register(RegisterModel register)
        {
            string Full_Name = register.User_Name + " " + register.User_Last_Name;

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
            using (SqlConnection con = new SqlConnection(cs))
            {
                string query = "INSERT INTO TblUser_Login (User_LoginID, User_Name,User_ShortName, User_Last_Name,User_Phone, User_Tel_Home, User_Email, User_Password2, User_Permanent_Address,User_Correspon_Address, User_Location, User_IC, Profile_ID) VALUES(@User_LoginID, @User_Name,@User_ShortName,@User_Last_Name, @User_Phone, @User_Tel_Home, @User_Email, @User_Password2, @User_Permanent_Address,@User_Correspon_Address, @User_Location, @User_IC, @Profile_ID)";
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.Connection = con;
                    //cmd.CommandTimeout = 300000;
                    con.Open();
                    cmd.Parameters.AddWithValue("@User_LoginID", register.User_Email);
                    cmd.Parameters.AddWithValue("@User_Name", Full_Name);
                    cmd.Parameters.AddWithValue("@User_ShortName", register.User_Name);
                    cmd.Parameters.AddWithValue("@User_Last_Name", register.User_Last_Name);
                    cmd.Parameters.AddWithValue("@User_Phone", register.User_Phone);
                    cmd.Parameters.AddWithValue("@User_Tel_Home", register.User_Tel_Home);
                    cmd.Parameters.AddWithValue("@User_Email", register.User_Email);
                    cmd.Parameters.AddWithValue("@User_Password2", register.User_Password2);
                    cmd.Parameters.AddWithValue("@User_Permanent_Address", register.User_Permanent_Address);
                    cmd.Parameters.AddWithValue("@User_Correspon_Address", register.User_Correspon_Address);
                    cmd.Parameters.AddWithValue("@User_Location", register.User_Location);
                    cmd.Parameters.AddWithValue("@User_IC", register.User_IC);
                    cmd.Parameters.AddWithValue("@Profile_ID", "1");

                    foreach (SqlParameter Parameter in cmd.Parameters)
                    {
                        if (Parameter.Value == null)
                        {
                            Parameter.Value = "NA";
                        }
                    }

                    register.ID = Convert.ToInt32(cmd.ExecuteScalar());
                    con.Close();
                }
            }
            ModelState.Clear();
            ViewBag.Message = "Account registered successfully.";
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult VerifyEmail()
        {
            return View();
        }

        [HttpPost]
        public ActionResult VerifyEmail(LoginModel user)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblUser_Login WHERE User_LoginID=@User_LoginID and IsActive=1";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@User_LoginID", user.User_LoginID);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        string userLogin = reader["User_LoginID"].ToString();

                        if (!(String.IsNullOrEmpty(userLogin)))
                        {
                            return RedirectToAction("ChangePassword", "Home");
                        }
                        con.Close();
                    }
                }
            }
            catch (Exception)
            {
                TempData["Message"] = "Reset failed. User Email supplied does not exist.";
            }
            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(LoginModel user)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string query = "UPDATE TblUser_Login SET User_Password2='" + user.New_Password + "' WHERE User_LoginID = '" + user.User_LoginID + "'";
                    using (SqlCommand cmd = new SqlCommand(query))
                    {
                        con.Open();
                        cmd.Connection = con;
                        cmd.ExecuteNonQuery();
                    }
                    con.Close();
                }
                return RedirectToAction("Login", "Home");
            }
            catch (Exception)
            {
                TempData["Message"] = "Reset failed.";
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginModel user)
        {
            try
            {
                string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
                using (SqlConnection con = new SqlConnection(cs))
                {
                    string commandText = "SELECT * FROM TblUser_Login WHERE User_LoginID=@User_LoginID AND User_Password2 = @User_Password2 AND IsActive=1";
                    using (SqlCommand cmd = new SqlCommand(commandText))
                    {
                        SqlDataReader reader;
                        cmd.Connection = con;
                        cmd.Parameters.AddWithValue("@User_LoginID", user.User_LoginID);
                        cmd.Parameters.AddWithValue("@User_Password2", user.User_Password2);
                        con.Open();
                        reader = cmd.ExecuteReader();
                        reader.Read();

                        Session["ID"] = reader["ID"].ToString();
                        //Session["User_Name"] = reader["User_Name"].ToString();
                        //Session["User_ShortName"] = reader["User_ShortName"].ToString();

                        string userLogin = reader["User_LoginID"].ToString();
                        string userProfile = reader["Profile_ID"].ToString();

                        if (!(String.IsNullOrEmpty(userLogin)))
                        {
                            if (userProfile == "1")
                            {
                                //FormsAuthentication.SetAuthCookie(user.User_Name, false);
                                return RedirectToAction("Index", "Applicant");
                            }
                            else if (userProfile == "2")
                            {
                                //FormsAuthentication.SetAuthCookie(user.User_Name, false);
                                return RedirectToAction("Index", "Admin");
                            }
                            else if (userProfile == "3")
                            {
                                //FormsAuthentication.SetAuthCookie(user.User_Name, false);
                                return RedirectToAction("Index", "Superadmin");
                            }
                        }
                        //TempData["Message"] = "Login failed. Username or password supplied does not exist.";
                        con.Close();
                    }
                }
            }
            catch (Exception ex)
            //catch (Exception)
            {
                TempData["Message"] = "Login failed. User Email or Password supplied does not exist.";
            }
            return View();
        }

        public ActionResult JobRegion()
        {
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

            string cs = ConfigurationManager.ConnectionStrings["abxserver"].ConnectionString;
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
                //string query = "Select ID, Region_Name From TblMaster_Region";
                string query = "Select DISTINCT R.ID, R.Region_Name from TblMaster_Region R LEFT JOIN TblMaster_DC DC ON R.ID = DC.Region_ID LEFT JOIN TblMaster_Position P ON DC.ID = P.DC_ID where P.IsOffer = 1 order by Region_Name asc";
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

        public ActionResult Contact()
        {
            return View();
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return View("Login");
        }

        //[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        //public ActionResult Error()
        //{
        //    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        //}
    }
}