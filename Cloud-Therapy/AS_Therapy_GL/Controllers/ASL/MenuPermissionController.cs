using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.DataAccess;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class MenuPermissionController : AppController
    {
           //Datetime formet
        IFormatProvider dateformat = new System.Globalization.CultureInfo("fr-FR", true);
        TimeZoneInfo timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
        public DateTime td;

        Therapy_GL_DbContext db = new Therapy_GL_DbContext();
        //Get Ip ADDRESS,Time & user PC Name
        public string strHostName;
        public IPHostEntry ipHostInfo;
        public IPAddress ipAddress;

        public MenuPermissionController()
        {
            //if (System.Web.HttpContext.Current.Request.Cookies["UI"] != null)
            //{

            //}
            //else
            //{
            //    PermissionFrom();
            //}

            //HttpCookie decodedCookie1 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CI"]);
            //HttpCookie decodedCookie2 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UI"]);
            //HttpCookie decodedCookie3 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UT"]);
            //HttpCookie decodedCookie4 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["UN"]);
            //HttpCookie decodedCookie5 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["US"]);
            //HttpCookie decodedCookie6 = CookieSecurityProvider.Decrypt(System.Web.HttpContext.Current.Request.Cookies["CS"]);
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);

            ViewData["HighLight_Menu_InputForm"] = "Heigh Light Menu";
        }





        public ASL_LOG aslLog = new ASL_LOG();
        // Insert ALL INFORMATION when User update the permission list.
        public void Update_ASl_Role_LogData(ASL_LOG aslLogRef)
        {
            TimeZoneInfo timeZoneInfo;
            timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("Central Asia Standard Time");
            DateTime PrintDate = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
            var date = Convert.ToString(PrintDate.ToString("dd-MMM-yyyy"));
            var time = Convert.ToString(PrintDate.ToString("hh:mm:ss tt"));

            Int64 maxSerialNo = Convert.ToInt64((from n in db.AslLogDbSet where n.COMPID == aslLogRef.COMPID && n.USERID == aslLogRef.USERID select n.LOGSLNO).Max());
            if (maxSerialNo == 0)
            {
                aslLog.LOGSLNO = Convert.ToInt64("1");
            }
            else
            {
                aslLog.LOGSLNO = maxSerialNo + 1;
            }

            aslLog.COMPID = Convert.ToInt64(aslLogRef.COMPID);
            aslLog.USERID = aslLogRef.USERID;
            aslLog.LOGTYPE = "UPDATE";
            aslLog.LOGSLNO = aslLog.LOGSLNO;
            aslLog.LOGDATE = Convert.ToDateTime(date);
            aslLog.LOGTIME = Convert.ToString(time);
            aslLog.LOGIPNO = aslLogRef.LOGIPNO;
            aslLog.LOGLTUDE = aslLogRef.LOGLTUDE;
            aslLog.TABLEID = "ASL_ROLE";
            aslLog.LOGDATA = aslLogRef.LOGDATA;
            aslLog.USERPC = aslLogRef.USERPC;
            db.AslLogDbSet.Add(aslLog);
        }





        // GET: /MenuPermission/
        [AcceptVerbs("GET")]
        public ActionResult PermissionFrom()
        {
           var dt = (RoleModel)TempData["data"];
           return View(dt);
           
        }






        [AcceptVerbs("POST")]
        public ActionResult PermissionFrom(RoleModel roleModel, string command)
        {
            if (command == "Update")
            {
                string msg;
                try
                {
                    var query =
                            from a in db.AslRoleDbSet
                            where (a.MENUID == roleModel.AslRole.MENUID && a.USERID == roleModel.AslRole.USERID
                            && a.COMPID == roleModel.AslRole.COMPID && a.MODULEID == roleModel.AslRole.MODULEID)
                            select a;

                    if (roleModel.AslRole.STATUS == null)
                    {
                        roleModel.AslRole.STATUS = "I";
                    }
                    if (roleModel.AslRole.INSERTR == null)
                    {
                        roleModel.AslRole.INSERTR = "I";
                    }
                    if (roleModel.AslRole.UPDATER == null)
                    {
                        roleModel.AslRole.UPDATER = "I";
                    }
                    if (roleModel.AslRole.DELETER == null)
                    {
                        roleModel.AslRole.DELETER = "I";
                    }


                    string moduleName = "";
                    var getModuleName = from a in db.AslMenumstDbSet
                                        where a.MODULEID == roleModel.AslMenumst.MODULEID
                                        select new { a.MODULENM };
                    foreach (var x in getModuleName)
                    {
                        moduleName = x.MODULENM.ToString();
                    }



                    string companyName = "";
                    var getCompanyName = from a in db.AslCompanyDbSet
                                         where a.COMPID == roleModel.AslCompany.COMPID
                                         select new { a.COMPNM };
                    foreach (var x in getCompanyName)
                    {
                        companyName = x.COMPNM;
                    }


                    foreach (ASL_ROLE a in query)
                    {
                        // Insert any additional changes to column values.
                        a.STATUS = roleModel.AslRole.STATUS;
                        if (a.STATUS == "A")
                        {
                            a.INSERTR = "A";
                            a.UPDATER = "A";
                            a.DELETER = "A";
                        }
                        else if (a.STATUS == "I")
                        {
                            a.INSERTR = "I";
                            a.UPDATER = "I";
                            a.DELETER = "I";

                        }
                        
                        a.INSUSERID = a.INSUSERID;
                        a.INSIPNO = a.INSIPNO;
                        a.INSTIME = a.INSTIME;
                        a.USERPC = strHostName;
                        a.UPDIPNO = ipAddress.ToString();
                        a.UPDTIME = Convert.ToDateTime(td);
                        //a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                        a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                        TempData["aslRoleLogData"] = Convert.ToString("Changed permission menu to this Company: " + companyName + ",\nModule Name: " + moduleName + ",\nMenu Type: " + a.MENUTP + ",\nMenu ID: " + a.MENUID + ",\nMenuName: " + a.MENUNAME + ",\nStatus: " + a.STATUS + ".");

                    }

                    ASL_LOG aslLogref = new ASL_LOG();
                    aslLogref.USERPC = strHostName;
                    aslLogref.LOGIPNO = ipAddress.ToString();
                    //aslLogref.COMPID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["CI"].Value);
                    aslLogref.COMPID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedCompID"]);
                    aslLogref.LOGLTUDE = roleModel.AslUserco.UPDLTUDE;
                    //Update User ID save ASL_ROLE table attribute UPDUSERID
                    //aslLogref.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                    aslLogref.USERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
                    aslLogref.LOGDATA = TempData["aslRoleLogData"].ToString();

                    Update_ASl_Role_LogData(aslLogref);


                    //Companies user statues updatec(if user exists)
                    var companiesUserList =
                           (from a in db.AslRoleDbSet
                            where (a.MENUID == roleModel.AslRole.MENUID && a.USERID != roleModel.AslRole.USERID
                            && a.COMPID == roleModel.AslRole.COMPID && a.MODULEID == roleModel.AslRole.MODULEID)
                            select a).ToList();
                    foreach (ASL_ROLE data in companiesUserList)
                    {
                        // Insert any additional changes to column values.
                        data.STATUS = "I";
                        data.INSERTR = "I";
                        data.UPDATER = "I";
                        data.DELETER = "I";

                        data.INSUSERID = data.INSUSERID;
                        data.INSIPNO = data.INSIPNO;
                        data.INSTIME = data.INSTIME;
                        data.USERPC = strHostName;
                        data.UPDIPNO = ipAddress.ToString();
                        data.UPDTIME = Convert.ToDateTime(td);
                        //a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Request.Cookies["UI"].Value);
                        data.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                    }


                    db.SaveChanges();
                    TempData["message"] = "Saved Sucessfully!";
                }

                catch (Exception ex)
                {
                    TempData["message"] = "Error occured:" + ex.Message;
                }

                TempData["AslRoleModel"] = null;
                TempData["data"] = roleModel;
                TempData["menuType"] = roleModel.AslRole.MENUTP;
                TempData["userId"] = roleModel.AslRole.USERID;
                TempData["moduleID"] = roleModel.AslRole.MODULEID;
                return RedirectToAction("PermissionFrom");
            }

            TempData["AslRoleModel"] = null;

            if (roleModel.AslMenu.MENUTP != null && roleModel.AslMenumst.MODULEID != null && roleModel.AslCompany.COMPID != null && roleModel.AslRole.USERID!=null)
            {
                TempData["data"] = roleModel;
                TempData["menuType"] = roleModel.AslMenu.MENUTP;
                TempData["userId"] = roleModel.AslRole.USERID;
                TempData["moduleID"] = roleModel.AslMenumst.MODULEID;

                return RedirectToAction("PermissionFrom");
            }
            else
            {
                ViewBag.ErrorFieldMessage = "Please select all DropdownList value ";
                return View("PermissionFrom");
            }
            
        }




        public ActionResult EditRoleUpdate(Int64 asl_roleId, Int64 companyID, string moduleId, string menuId, Int64 userId, string ForR, RoleModel roleModel)
        {
            roleModel.AslRole = db.AslRoleDbSet.Find(asl_roleId);
            roleModel.AslCompany.COMPID = companyID;
            roleModel.AslUserco.USERID = userId;

            var result2 = from n in db.AslMenumstDbSet where n.MODULEID == moduleId select new { n.MODULENM, n.MODULEID };
            foreach (var v in result2)
            {
                TempData["moduleName"] = v.MODULENM;
                roleModel.AslMenumst.MODULENM = v.MODULENM;
                roleModel.AslMenumst.MODULEID = v.MODULEID;
            }

            var result3 = from n in db.AslMenuDbSet where n.MENUID == menuId && n.MENUTP == ForR select new { n.MENUTP, n.MODULEID, n.MENUNM };
            foreach (var v in result3)
            {
                roleModel.AslMenu.MENUTP = v.MENUTP;
                roleModel.AslMenu.MENUNM = v.MENUNM;
            }

            var resultRoleModel = from m in db.AslRoleDbSet
                                  where m.COMPID == companyID && m.USERID == userId
                                        && m.MODULEID == moduleId && m.MENUID == menuId && m.MENUTP == ForR
                                  select new { m.ASL_ROLEId, m.COMPID, m.USERID, m.MODULEID, m.MENUTP, m.MENUID, m.STATUS, m.INSERTR, m.UPDATER, m.DELETER };
            foreach (var VARIABLE in resultRoleModel)
            {
                roleModel.AslRole.ASL_ROLEId = VARIABLE.ASL_ROLEId;
                roleModel.AslRole.COMPID = VARIABLE.COMPID;
                roleModel.AslRole.USERID = VARIABLE.USERID;
                roleModel.AslRole.MODULEID = VARIABLE.MODULEID;
                roleModel.AslRole.MENUTP = VARIABLE.MENUTP;
                roleModel.AslRole.MENUID = VARIABLE.MENUID;
                roleModel.AslRole.STATUS = VARIABLE.STATUS;
                roleModel.AslRole.INSERTR = VARIABLE.INSERTR;
                roleModel.AslRole.UPDATER = VARIABLE.UPDATER;
                roleModel.AslRole.DELETER = VARIABLE.DELETER;
            }
            TempData["AslRoleModel"] = roleModel.AslRole;


            TempData["data"] = roleModel;
            TempData["menuType"] = ForR;
            TempData["userId"] = roleModel.AslRole.USERID;
            TempData["moduleID"] = roleModel.AslMenumst.MODULEID;
            TempData["menuId"] = roleModel.AslRole.MENUID;

            return RedirectToAction("PermissionFrom");
        }







        //Get User Name when Username Dropdownlist Changed.
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult CompanyNameChanged(Int64 changedDropDown)
        {
            Int64 CompanyAdminID=0;
            Int64 userID = Convert.ToInt64((from m in db.AslUsercoDbSet where m.COMPID == changedDropDown && m.OPTP == "CompanyAdmin" select m.USERID).Min());
            CompanyAdminID = userID;

            var result = new { USERID = CompanyAdminID, };
            return Json(result, JsonRequestBehavior.AllowGet);

        }




        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
