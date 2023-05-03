using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using AS_Therapy_GL.Models;

namespace AS_Therapy_GL.Controllers
{
    public class ASL_MENUController : Controller
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

        public ASL_MENUController()
        {
            strHostName = System.Net.Dns.GetHostName();
            ipHostInfo = Dns.Resolve(Dns.GetHostName());
            ipAddress = ipHostInfo.AddressList[0];
            //td = DateTime.Now;
            td = TimeZoneInfo.ConvertTime(DateTime.Now, timeZoneInfo);
        }


        // GET: /ASL_MENU/
        [AcceptVerbs("GET")]
        [ActionName("Index")]
        public ActionResult Index()
        {
            ViewData["HighLight_Menu_InputForm"] = "Heigh Light Menu";
            var dt = (PageModel)TempData["data"];
            return View(dt);
        }


        [AcceptVerbs("POST")]
        [ActionName("Index")]
        public ActionResult IndexPost(PageModel model)
        {
            if (model.aslMenumst.MODULENM == null && model.aslMenu.MENUTP != null)
            {
                ViewBag.ModuleNameField = "Please input the Module Name. ";
                return View("Index");
            }
            else if (model.aslMenumst.MODULENM == null && model.aslMenu.MENUTP == null)
            {
                ViewBag.NullAllField = "Please input the Module Name and select the Menu Type field. ";
                return View("Index");
            }



            model.aslMenumst.USERPC = strHostName;
            model.aslMenumst.INSIPNO = ipAddress.ToString();
            model.aslMenumst.INSTIME = td;
            model.aslMenumst.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);
            var result =
                db.AslMenumstDbSet.Count(
                    d => d.MODULENM == model.aslMenumst.MODULENM && d.MODULEID == model.aslMenumst.MODULEID);
            if (result == 0)
            {
                var id = Convert.ToString(db.AslMenumstDbSet.Max(s => s.MODULEID));

                if (id == null)
                {
                    model.aslMenumst.MODULEID = Convert.ToString("01");

                    db.AslMenumstDbSet.Add(model.aslMenumst);
                    db.SaveChanges();

                    TempData["message"] = "Module Name: '" + model.aslMenumst.MODULENM +
                                          "' successfully saved.\n Please Create the Menu List.";

                    TempData["data"] = model;
                    return RedirectToAction("Index");
                    //return View("index", new { ID = model.aslMenumst.MODULEID });
                }
                else if (id != null)
                {
                    int nid = int.Parse(id) + 1;
                    if (nid < 10)
                    {
                        model.aslMenumst.MODULEID = Convert.ToString("0" + nid);
                        db.AslMenumstDbSet.Add(model.aslMenumst);
                        db.SaveChanges();

                        TempData["message"] = "Module Name: '" + model.aslMenumst.MODULENM +
                                              "' successfully saved.\n Please Create the Menu List.";

                        TempData["data"] = model;
                        return RedirectToAction("Index");

                    }
                    else if (nid < 100)
                    {
                        model.aslMenumst.MODULEID = Convert.ToString(nid);
                        db.AslMenumstDbSet.Add(model.aslMenumst);
                        db.SaveChanges();

                        TempData["message"] = "Module Name: '" + model.aslMenumst.MODULENM +
                                              "' successfully saved.\n Please Create the Menu List.";

                        TempData["data"] = model;
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        TempData["message"] = " Module entry not possible ";
                        return RedirectToAction("Index");
                    }
                }
            }

            else if (result > 0)
            {
                if (model.aslMenumst.MODULENM != null && model.aslMenu.MENUTP != null)
                {
                    TempData["message"] = "Get the Menu List";
                    TempData["data"] = model;
                    return RedirectToAction("Index");
                }
                else if (model.aslMenumst.MODULENM != null && model.aslMenu.MENUTP == null)
                {
                    ViewBag.MenuTypeField = "Please select the Menu Type. ";
                    return View("Index");
                }

            }

            return View("Index");


            //}

            //if (model.aslMenumst.MODULENM == null && model.aslMenu.MENUTP != null)
            //{
            //    ViewBag.ModuleNameField = "Please input the Module Name. ";
            //    return View("Index");
            //}
            //else if (model.aslMenumst.MODULENM == null && model.aslMenu.MENUTP == null)
            //{
            //    ViewBag.NullAllField = "Please input the Module Name and select the Menu Type field. ";
            //    return View("Index");
            //}
            //else
            //{
            //    return View("Index");
            //}

        }



        public JsonResult GetTodoLists(string ModId, string MenuType, string sidx, string sord, int page, int rows)  //Gets the todo Lists.
        {
            ASL_MENU aslMenuObj = new ASL_MENU();
            aslMenuObj.MODULEID = ModId;

            int pageIndex = Convert.ToInt32(page) - 1;
            int pageSize = rows;
            var todoListsResults = db.AslMenuDbSet
                .Where(a => a.MODULEID == ModId && a.MENUTP == MenuType)
                .Select(
                    a => new
                    {
                        a.Id,
                        a.SERIAL,
                        a.MENUID,
                        a.MENUNM,
                        a.ACTIONNAME,
                        a.CONTROLLERNAME
                    });

            int totalRecords = todoListsResults.Count();
            var totalPages = (int)Math.Ceiling((float)totalRecords / (float)rows);
            if (sord.ToUpper() == "DESC")
            {
                todoListsResults = todoListsResults.OrderByDescending(s => s.SERIAL);
                todoListsResults = todoListsResults.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                todoListsResults = todoListsResults.OrderBy(s => s.SERIAL);
                todoListsResults = todoListsResults.Skip(pageIndex * pageSize).Take(pageSize);
            }
            var jsonData = new
            {
                total = totalPages,
                page,
                records = totalRecords,
                rows = todoListsResults
            };
            return Json(jsonData, JsonRequestBehavior.AllowGet);
        }





        //insert a new row to the grid logic here 

        [HttpPost]
        public string Create(string ModId, string MenuType, [Bind(Exclude = "Id")] ASL_MENU objTodo)
        {
            objTodo.MODULEID = ModId;
            objTodo.MENUTP = MenuType;

            objTodo.USERPC = strHostName;
            objTodo.INSIPNO = ipAddress.ToString();
            objTodo.INSTIME = td;
            objTodo.INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

            string msg = "";
            try
            {
                if (ModelState.IsValid)
                {
                    var maxData = Convert.ToString((from n in db.AslMenuDbSet where n.MODULEID == ModId && n.MENUTP == MenuType select n.MENUID).Max());
                    if (maxData == null && MenuType == "F")
                    {

                        objTodo.MENUID = Convert.ToString("F" + ModId + "01");

                        db.AslMenuDbSet.Add(objTodo);
                        db.SaveChanges();

                        //When SUPER_Admin create a New Menu List (From/Report) then the Role database list add for all user. 
                        var qrM = from a in db.AslUsercoDbSet select a;
                        foreach (AslUserco a in qrM)
                        {
                            if (a.OPTP != "AslSuperadmin" && a.OPTP != "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    MODULEID = objTodo.MODULEID,
                                    SERIAL = objTodo.SERIAL,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "I",
                                    INSERTR = "I",
                                    UPDATER = "I",
                                    DELETER = "I",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                            if (a.OPTP == "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    MODULEID = objTodo.MODULEID,
                                    SERIAL = objTodo.SERIAL,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "A",
                                    INSERTR = "A",
                                    UPDATER = "A",
                                    DELETER = "A",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                        }
                        db.SaveChanges();

                        msg = "Saved Successfully";
                    }
                    else if (maxData != null && MenuType == "F")
                    {
                        var subString = Convert.ToString((from n in db.AslMenuDbSet where n.MODULEID == ModId && n.MENUTP == MenuType select n.MENUID.Substring(3, 2)).Max());

                        string id = Convert.ToString(subString);
                        int nid = int.Parse(id) + 1;

                        if (nid < 10)
                        {
                            objTodo.MENUID = Convert.ToString("F" + ModId + "0" + nid);
                            db.AslMenuDbSet.Add(objTodo);
                            db.SaveChanges();
                            msg = "Saved Successfully";

                        }
                        else if (nid < 100)
                        {
                            objTodo.MENUID = Convert.ToString("F" + ModId + nid);
                            db.AslMenuDbSet.Add(objTodo);
                            db.SaveChanges();
                            msg = "Saved Successfully";

                        }
                        else
                        {
                            msg = "Module entry not possible";
                        }



                        //When SUPER_Admin create a New Menu List (From/Report) then the Role database list add for all user. 
                        var qrM = from a in db.AslUsercoDbSet select a;
                        foreach (AslUserco a in qrM)
                        {
                            if (a.OPTP != "AslSuperadmin" && a.OPTP != "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    MODULEID = objTodo.MODULEID,
                                    SERIAL = objTodo.SERIAL,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "I",
                                    INSERTR = "I",
                                    UPDATER = "I",
                                    DELETER = "I",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                            if (a.OPTP == "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    MODULEID = objTodo.MODULEID,
                                    SERIAL = objTodo.SERIAL,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "A",
                                    INSERTR = "A",
                                    UPDATER = "A",
                                    DELETER = "A",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                        }
                        db.SaveChanges();



                    }
                    else if (maxData == null && MenuType == "R")
                    {

                        objTodo.MENUID = Convert.ToString("R" + ModId + "01");

                        db.AslMenuDbSet.Add(objTodo);
                        db.SaveChanges();



                        //When SUPER_Admin create a New Menu List (From/Report) then the Role database list add for all user. 
                        var qrM = from a in db.AslUsercoDbSet select a;
                        foreach (AslUserco a in qrM)
                        {
                            if (a.OPTP != "AslSuperadmin" && a.OPTP != "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    MODULEID = objTodo.MODULEID,
                                    SERIAL = objTodo.SERIAL,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "I",
                                    INSERTR = "I",
                                    UPDATER = "I",
                                    DELETER = "I",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                            if (a.OPTP == "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    MODULEID = objTodo.MODULEID,
                                    SERIAL = objTodo.SERIAL,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "A",
                                    INSERTR = "A",
                                    UPDATER = "A",
                                    DELETER = "A",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                        }
                        db.SaveChanges();

                        msg = "Saved Successfully";
                    }

                    else if (maxData != null && MenuType == "R")
                    {
                        var subString = Convert.ToString((from n in db.AslMenuDbSet where n.MODULEID == ModId && n.MENUTP == MenuType select n.MENUID.Substring(3, 2)).Max());
                        int nid = int.Parse(subString) + 1;

                        if (nid < 10)
                        {
                            objTodo.MENUID = Convert.ToString("R" + ModId + "0" + nid);
                            db.AslMenuDbSet.Add(objTodo);
                            db.SaveChanges();
                            msg = "Saved Successfully";

                        }
                        else if (nid < 100)
                        {
                            objTodo.MENUID = Convert.ToString("R" + ModId + nid);
                            db.AslMenuDbSet.Add(objTodo);
                            db.SaveChanges();
                            msg = "Saved Successfully";

                        }
                        else
                        {
                            msg = "Module entry not possible";

                        }

                        //When SUPER_Admin create a New Menu List (From/Report) then the Role database list add for all user. 
                        var qrM = from a in db.AslUsercoDbSet select a;
                        foreach (AslUserco a in qrM)
                        {
                            if (a.OPTP != "AslSuperadmin" && a.OPTP != "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    SERIAL = objTodo.SERIAL,
                                    MODULEID = objTodo.MODULEID,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "I",
                                    INSERTR = "I",
                                    UPDATER = "I",
                                    DELETER = "I",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                            if (a.OPTP == "CompanyAdmin")
                            {
                                ASL_ROLE role = new ASL_ROLE()
                                {

                                    COMPID = Convert.ToInt64(a.COMPID),
                                    USERID = Convert.ToInt64(a.USERID),
                                    SERIAL = objTodo.SERIAL,
                                    MODULEID = objTodo.MODULEID,
                                    MENUID = objTodo.MENUID,
                                    MENUTP = objTodo.MENUTP,
                                    INSUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]),
                                    INSIPNO = ipAddress.ToString(),
                                    INSTIME = td,
                                    USERPC = strHostName,
                                    STATUS = "A",
                                    INSERTR = "A",
                                    UPDATER = "A",
                                    DELETER = "A",
                                    MENUNAME = objTodo.MENUNM,
                                    ACTIONNAME = objTodo.ACTIONNAME,
                                    CONTROLLERNAME = objTodo.CONTROLLERNAME
                                };
                                db.AslRoleDbSet.Add(role);
                            }

                        }
                        db.SaveChanges();

                    }

                }
                else
                {
                    msg = "Validation data not successfull";
                }
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }




        public string Edit(string ModId, string MenuType, ASL_MENU objTodo)
        {
            string msg;
            try
            {
                var query =
                        from a in db.AslMenuDbSet
                        where (a.MENUID == objTodo.MENUID)
                        select a;

                foreach (ASL_MENU a in query)
                {
                    // Insert any additional changes to column values.
                    a.SERIAL = objTodo.SERIAL;
                    a.MENUNM = objTodo.MENUNM;
                    a.ACTIONNAME = objTodo.ACTIONNAME;
                    a.CONTROLLERNAME = objTodo.CONTROLLERNAME;
                    a.UPDIPNO = ipAddress.ToString();
                    a.UPDTIME = td;
                    a.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);

                }

                //all ASL-ROLE data modified
                var qrM = (from b in db.AslRoleDbSet where b.MENUID == objTodo.MENUID select b).OrderBy(x=>x.SERIAL);
                foreach (ASL_ROLE b in qrM)
                {
                    b.SERIAL = objTodo.SERIAL;
                    b.MENUNAME = objTodo.MENUNM;
                    b.ACTIONNAME = objTodo.ACTIONNAME;
                    b.CONTROLLERNAME = objTodo.CONTROLLERNAME;

                }

                db.SaveChanges();
                msg = "" + objTodo.MENUNM + "update Successfully.";
            }
            catch (Exception ex)
            {
                msg = "Error occured:" + ex.Message;
            }
            return msg;
        }

        //Delete Menu From Grid
        public string Delete(int Id)
        {
            ASL_MENU todolist = db.AslMenuDbSet.Find(Id);

            //User database list Delete
            var companAllInfo = from role in db.AslRoleDbSet
                                where role.MODULEID == todolist.MODULEID && role.MENUID == todolist.MENUID
                                    && role.MENUTP == todolist.MENUTP
                                select role;
            foreach (var remove in companAllInfo)
            {
                db.AslRoleDbSet.Remove(remove);
            }
            db.SaveChanges();

            db.AslMenuDbSet.Remove(todolist);
            db.SaveChanges();
            return "Deleted successfully";
        }


        
        // GET: /ASL_MENUMST/
        public ActionResult ShowModuleList()
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            return View(db.AslMenumstDbSet.ToList());
        }


       
        // GET: /ASL_MENUMST/Edit/5
        public ActionResult EditModuleList(string id = null)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            ASL_MENUMST asl_menumst = db.AslMenumstDbSet.Find(id);
            if (asl_menumst == null)
            {
                return HttpNotFound();
            }
            return View(asl_menumst);
        }

        
        // POST: /ASL_MENUMST/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditModuleList(ASL_MENUMST asl_menumst)
        {
            if (ModelState.IsValid)
            {
                //Get Ip ADDRESS,Time & user PC Name
                string strHostName = System.Net.Dns.GetHostName();
                IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
                IPAddress ipAddress = ipHostInfo.AddressList[0];


                asl_menumst.USERPC = strHostName;
                asl_menumst.UPDIPNO = ipAddress.ToString();
                asl_menumst.UPDTIME = td;

                //Insert User ID save ASL_MENUMST table attribute INSUSERID
                asl_menumst.UPDUSERID = Convert.ToInt64(System.Web.HttpContext.Current.Session["loggedUserID"]);


                db.Entry(asl_menumst).State = EntityState.Modified;
                db.SaveChanges();
                TempData["ModuleUpdateMessage"] = "Module name: '" + asl_menumst.MODULENM + "' update Successfully!";
                return RedirectToAction("ShowModuleList");
            }
            return View(asl_menumst);
        }

        // Get All menu Information
        public ActionResult ShowMenuList()
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            return View(db.AslMenuDbSet.ToList());
        }


        //
        // GET: /ASL_MENU/Delete/5

        public ActionResult DeleteModule(string id = null)
        {
            ViewData["HighLight_Menu_InformationForm"] = "Heigh Light Menu";
            ASL_MENUMST asl_menumst = db.AslMenumstDbSet.Find(id);
            if (asl_menumst == null)
            {
                return HttpNotFound();
            }
            return View(asl_menumst);
        }

        //
        // POST: /ASL_MENU/Delete/5

        [HttpPost, ActionName("DeleteModule")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteModuleConfirmed(string id)
        {
            ASL_MENUMST asl_menumst = db.AslMenumstDbSet.Find(id);

            //Seasrch all information from Menu Table,when it match to the Module ID
            var menuList = (from sub in db.AslMenuDbSet select sub)
               .Where(sub => sub.MODULEID == asl_menumst.MODULEID);
            foreach (var n in menuList)
            {
                db.AslMenuDbSet.Remove(n);
            }
            db.SaveChanges();

            db.AslMenumstDbSet.Remove(asl_menumst);
            db.SaveChanges();
            TempData["ModuleDeleteMessage"] = "Module name: '" + asl_menumst.MODULENM + "' delete Successfully!";
            return RedirectToAction("ShowModuleList");
        }


        //AutoComplete 
        [AcceptVerbs(HttpVerbs.Get)]
        public JsonResult ItemNameChanged(string changedText)
        {
            // var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            string itemId = "";
            var rt = db.AslMenumstDbSet.Where(n => n.MODULENM == changedText).Select(n => new
            {
                moduleId = n.MODULEID
            });

            foreach (var n in rt)
            {
                itemId = n.moduleId;
            }

            return Json(itemId, JsonRequestBehavior.AllowGet);

        }


        //AutoComplete
        public JsonResult TagSearch(string term)
        {
            //var compid = Convert.ToInt16(System.Web.HttpContext.Current.Session["loggedCompID"].ToString());
            var tags = from p in db.AslMenumstDbSet
                       select p.MODULENM;

            return this.Json(tags.Where(t => t.StartsWith(term)),
                            JsonRequestBehavior.AllowGet);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}