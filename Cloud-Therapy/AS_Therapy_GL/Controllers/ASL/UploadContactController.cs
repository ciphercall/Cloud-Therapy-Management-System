using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;
using AS_Therapy_GL.Models.ASL;
using AS_Therapy_GL.Models.DTO;

namespace AS_Therapy_GL.Controllers.ASL
{
    public class UploadContactController : AppController
    {
        private Therapy_GL_DbContext db = new Therapy_GL_DbContext();


        //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public UploadContactController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            ViewData["HighLight_Menu_PromotionForm"] = "High Light Menu";
        }




        public ASL_LOG aslLog = new ASL_LOG();

        // Delete ALL INFORMATION from Grid(Upload Contact data) TO Asl_lOG Database Table.
        public void Delete_Upload_LogData(ASL_PCONTACTS model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == model.COMPID && n.USERID == model.INSUSERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(model.COMPID);
            aslLog.USERID = model.INSUSERID;
            aslLog.LOGTYPE = "DELETE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = ipAddress.ToString();
            aslLog.LOGLTUDE = model.INSLTUDE;
            aslLog.TABLEID = "ASL_PCONTACTS";

            string groupName = "";
            var findGroupName = (from m in db.UploadGroupDbSet where m.COMPID == model.COMPID && m.GROUPID == model.GROUPID select m).ToList();
            foreach (var x in findGroupName)
            {
                groupName = x.GROUPNM.ToString();
            }

            aslLog.LOGDATA = Convert.ToString("When upload File then duplicate data deleted. Group Name: " + groupName + ",\nName: " + model.NAME + ",\nEmail: " + model.EMAIL + ",\nMobile No 1: " + model.MOBNO1 + ",\nMobile No 2: " + model.MOBNO2 + ",\nAddress: " + model.ADDRESS + ".");
            aslLog.USERPC = strHostName;

            db.AslLogDbSet.Add(aslLog);
            db.SaveChanges();
        }





        public ASL_DELETE AslDelete = new ASL_DELETE();

        // Delete ALL INFORMATION from TO ASL_DELETE Database Table.
        public void Delete_Upload_LogDelete(ASL_PCONTACTS model)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslDeleteDbSet where n.COMPID == model.COMPID && n.USERID == model.INSUSERID select n.DELSLNO).Max());
            if (maxSerialNo == 0)
            {
                AslDelete.DELSLNO = Convert.ToInt64("1");
            }
            else
            {
                AslDelete.DELSLNO = maxSerialNo + 1;
            }

            AslDelete.COMPID = Convert.ToInt64(model.COMPID);
            AslDelete.USERID = model.INSUSERID;
            AslDelete.DELSLNO = AslDelete.DELSLNO;
            AslDelete.DELDATE = Convert.ToString(date);
            AslDelete.DELTIME = Convert.ToString(time);
            AslDelete.DELIPNO = ipAddress.ToString();
            AslDelete.DELLTUDE = model.INSLTUDE;
            AslDelete.TABLEID = "ASL_PCONTACTS";

            string groupName = "";
            var findGroupName = (from m in db.UploadGroupDbSet where m.COMPID == model.COMPID && m.GROUPID == model.GROUPID select m).ToList();
            foreach (var x in findGroupName)
            {
                groupName = x.GROUPNM.ToString();
            }
            AslDelete.DELDATA = Convert.ToString("When upload File then duplicate data deleted. Group Name: " + groupName + ",\nName: " + model.NAME + ",\nEmail: " + model.EMAIL + ",\nMobile No 1: " + model.MOBNO1 + ",\nMobile No 2: " + model.MOBNO2 + ",\nAddress: " + model.ADDRESS + ".");
            AslDelete.USERPC = strHostName;

            db.AslDeleteDbSet.Add(AslDelete);
            db.SaveChanges();
        }




        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }



        private bool BdNumberValidate(string number)
        {
            try
            {
                if (number.Length > 13 || number.Length < 13)
                {
                    return false;
                }
                else
                {
                    string operatorCode = number.Substring(0, 5);
                    switch (operatorCode)
                    {
                        case "88018":
                        case "88017":
                        case "88019":
                        case "88016":
                        case "88011":
                        case "88015":
                            return true; //all of the operator in case is return true
                            break;
                        default:
                            return false; //other operator code return false
                    }
                }
            }
            catch
            {
                return false;
            }
        }









        // GET: /UploadDocument/
        public ActionResult UploadDocument()
        {
            return View();
        }

        // POST: /UploadDocument/
        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase file, ASL_PCONTACTS model)
        {
            DataSet ds = new DataSet();
            String fileLocation = "";
            Int64 count = 0;
            try
            {
                if (Request.Files["file"].ContentLength > 0)
                {

                    string fileExtension = System.IO.Path.GetExtension(Request.Files["file"].FileName);

                    if (fileExtension == ".xls" || fileExtension == ".xlsx")
                    {
                        fileLocation = string.Concat(Server.MapPath("~/UploadFile/CurrentFile/") + model.COMPID + Request.Files["file"].FileName);
                        if (System.IO.File.Exists(fileLocation))
                        {
                            System.IO.File.Delete(fileLocation);
                        }
                        Request.Files["file"].SaveAs(fileLocation);
                        string excelConnectionString = string.Empty;

                        //connection String for xls file format.
                        if (fileExtension == ".xls")
                        {
                            excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                        }
                        //connection String for xlsx file format.
                        else if (fileExtension == ".xlsx")
                        {
                            //excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                            excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source= " + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=1\"";
                        }
                        //Create Connection to Excel work book and add oledb namespace
                        OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                        excelConnection.Open();
                        DataTable dt = new DataTable();

                        dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                        if (dt == null)
                        {
                            return null;
                        }

                        String[] excelSheets = new String[dt.Rows.Count];
                        int t = 0;
                        //excel data saves in temp file here.
                        foreach (DataRow row in dt.Rows)
                        {
                            excelSheets[t] = row["TABLE_NAME"].ToString();
                            t++;
                        }
                        OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                        try
                        {
                            string query = string.Format("Select * from [{0}]", excelSheets[0]);
                            using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                            {
                                dataAdapter.Fill(ds);
                            }

                            List<UploadContactDTO> errorUploadList = new List<UploadContactDTO>();

                            ASL_PCONTACTS pContacts = new ASL_PCONTACTS();
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                //string conn = ConfigurationManager.ConnectionStrings["Store_GL_DbContext"].ConnectionString;
                                //SqlConnection con = new SqlConnection(conn);


                                string email = ds.Tables[0].Rows[i][1].ToString();
                                string mobileNo1 = ds.Tables[0].Rows[i][2].ToString();
                                string mobileNo2 = ds.Tables[0].Rows[i][3].ToString();
                                //string insertQuery = "";
                                if (IsValidEmail(email) || BdNumberValidate(mobileNo1) || BdNumberValidate(mobileNo2))
                                {
                                    if (check_ExcelData(ds.Tables[0].Rows[i], model) == "True")
                                    {
                                    }
                                    else // check_ExcelData Exists in database (return Flase)
                                    {
                                        delete_ExcelData(ds.Tables[0].Rows[i], model);
                                    }

                                    pContacts.COMPID = model.COMPID;
                                    pContacts.GROUPID = model.GROUPID;
                                    pContacts.NAME = ds.Tables[0].Rows[i][0].ToString();
                                    if (IsValidEmail(email))
                                    {
                                        pContacts.EMAIL = ds.Tables[0].Rows[i][1].ToString();
                                    }
                                    if (BdNumberValidate(mobileNo1))
                                    {
                                        pContacts.MOBNO1 = ds.Tables[0].Rows[i][2].ToString();
                                    }
                                    if (BdNumberValidate(mobileNo2))
                                    {
                                        pContacts.MOBNO2 = ds.Tables[0].Rows[i][3].ToString();
                                    }
                                    pContacts.ADDRESS = ds.Tables[0].Rows[i][4].ToString();

                                    pContacts.USERPC = strHostName;
                                    pContacts.INSIPNO = ipAddress.ToString();
                                    pContacts.INSTIME = Convert.ToDateTime(td);
                                    pContacts.INSUSERID = model.INSUSERID;
                                    pContacts.INSLTUDE = Convert.ToString(model.INSLTUDE);

                                    db.UploadContactDbSet.Add(pContacts);
                                    db.SaveChanges();
                                    count++;


                                    //insertQuery = "Insert into ASL_PCONTACTS(COMPID,GROUPID,NAME,EMAIL,MOBNO1,ADDRESS) Values('" +
                                    //    model.COMPID + "','" + model.GROUPID + "','" + ds.Tables[0].Rows[i][0].ToString() + "','" + ds.Tables[0].Rows[i][1].ToString() + "','" + ds.Tables[0].Rows[i][2].ToString() +
                                    //    "','" + ds.Tables[0].Rows[i][3].ToString() + "')";
                                    //con.Open();
                                    //SqlCommand cmd = new SqlCommand(insertQuery, con);
                                    //cmd.ExecuteNonQuery();
                                    //con.Close();
                                }
                                else
                                {
                                    errorUploadList.Add(new UploadContactDTO { NAME = ds.Tables[0].Rows[i][0].ToString(), EMAIL = ds.Tables[0].Rows[i][1].ToString(), MOBNO1 = ds.Tables[0].Rows[i][2].ToString(), MOBNO2 = ds.Tables[0].Rows[i][3].ToString(), ADDRESS = ds.Tables[0].Rows[i][4].ToString() });
                                    TempData["ErrorUploadList"] = errorUploadList;
                                }
                            }


                            excelConnection1.Close();
                            excelConnection.Close();
                            System.IO.FileInfo currentFile = new System.IO.FileInfo(fileLocation);
                            if (currentFile.Exists)
                            {
                                currentFile.Delete();
                            }

                            if (count != 0)
                            {
                                ViewBag.UploadMessage = "Upload successfully done! ";
                            }
                            else
                            {
                                ViewBag.UploadMessage = "Your upload file contains invalid data!!!";
                            }
                        }
                        catch
                        {
                            excelConnection1.Close();
                            excelConnection.Close();
                            System.IO.FileInfo currentFile = new System.IO.FileInfo(fileLocation);
                            if (currentFile.Exists)
                            {
                                currentFile.Delete();
                            }
                        }
                    }

                    //if (fileExtension.ToString().ToLower().Equals(".xml"))
                    //{
                    //    fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    //    if (System.IO.File.Exists(fileLocation))
                    //    {
                    //        System.IO.File.Delete(fileLocation);
                    //    }

                    //    Request.Files["FileUpload"].SaveAs(fileLocation);
                    //    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    //    // DataSet ds = new DataSet();
                    //    ds.ReadXml(xmlreader);
                    //    xmlreader.Close();
                    //}                                       
                }

            }
            catch //(Exception ex)
            {
                //Response.Write(ex.Message);
                //System.IO.FileInfo currentFile = new System.IO.FileInfo(fileLocation);
                //if (currentFile.Exists)
                //{
                //    currentFile.Delete();
                //}
                ViewBag.UploadMessage = "Excel file is not in correct Format.";
            }
            return View();
        }


















        protected string check_ExcelData(DataRow d, ASL_PCONTACTS model)
        {
            string Result = "";

            var name = d.ItemArray[0].ToString();// d["NAME"].ToString();
            var email = d.ItemArray[1].ToString();//d["EMAIL"].ToString();
            var mobileNo1 = d.ItemArray[2].ToString();//d["MOBILENO1"].ToString();

            var get_asl_Contact =
                (from m in db.UploadContactDbSet
                 where m.COMPID == model.COMPID && m.GROUPID == model.GROUPID && m.NAME == name && (m.EMAIL == email || m.MOBNO1 == mobileNo1)
                 select m).ToList();
            if (get_asl_Contact.Count != 0)
            {
                Result = "false";
            }
            else //count == 0
            {
                Result = "True";
            }

            return Result;
        }





        protected void delete_ExcelData(DataRow d, ASL_PCONTACTS model)
        {
            try
            {
                var name = d.ItemArray[0].ToString();// d["NAME"].ToString();
                var email = d.ItemArray[1].ToString();//d["EMAIL"].ToString();
                var mobileNo1 = d.ItemArray[2].ToString();//d["MOBILENO1"].ToString();
                var mobileNo2 = d.ItemArray[3].ToString();//d["MOBILENO2"].ToString();
                var address = d.ItemArray[4].ToString();//d["ADDRESS"].ToString();

                var remove = (from m in db.UploadContactDbSet
                              where m.COMPID == model.COMPID && m.GROUPID == model.GROUPID && m.NAME == name && (m.EMAIL == email || m.MOBNO1 == mobileNo1)
                              select m).FirstOrDefault();
                db.UploadContactDbSet.Remove(remove);
                db.SaveChanges();

                model.NAME = name;
                model.EMAIL = email;
                model.MOBNO1 = mobileNo1;
                model.MOBNO2 = mobileNo2;
                model.ADDRESS = address;
                Delete_Upload_LogData(model);
                Delete_Upload_LogDelete(model);
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
            }
        }









        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }


    }
}
