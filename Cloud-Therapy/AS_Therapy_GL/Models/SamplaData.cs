using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace AS_Therapy_GL.Models
{
    public class SamplaData : DropCreateDatabaseIfModelChanges<Therapy_GL_DbContext>
    {
        protected override void Seed(Therapy_GL_DbContext context)
        {
            //context.AslCompanyDbSet.Add(new AslCompany() { COMPID = 101, COMPNM = "Savron Hospital", ADDRESS = "jamalkhan road,Chittagong", CONTACTNO = "8801745555555", EMAILID = "savron@gmail.com", WEBID = "www.savron.com", STATUS = "A" });
            //context.AslCompanyDbSet.Add(new AslCompany() { COMPID = 102, COMPNM = "Metro Hospital", ADDRESS = "GEC road,Chittagong", CONTACTNO = "8801744444444", EMAILID = "metro@gmail.com", WEBID = "www.metro.com", STATUS = "A" });

            //context.SaveChanges();

            //context.AslUsercoDbSet.Add(new AslUserco() { COMPID = 001, USERID = 10001, USERNM = "Alchemy Software(Piash)", DEPTNM = "Admin", OPTP = "AslSuperadmin", ADDRESS = "Goal pahar,Suborna, 203/b,Chittagong", MOBNO = "8801740545009", EMAILID = "superadmin01@gmail.com", LOGINBY = "EMAIL", LOGINID = "superadmin01@gmail.com", LOGINPW = "123", TIMEFR = "00:00", TIMETO = "23:59", STATUS = "A" });
            //context.AslUsercoDbSet.Add(new AslUserco() { COMPID = 002, USERID = 10002, USERNM = "Alchemy Software(Shamim)", DEPTNM = "Admin", OPTP = "AslSuperadmin", ADDRESS = "Goal pahar, 203/b,Chittagong", MOBNO = "8801775222222", EMAILID = "superadmin02@gmail.com", LOGINBY = "EMAIL", LOGINID = "superadmin02@gmail.com", LOGINPW = "123", TIMEFR = "00:00", TIMETO = "23:59", STATUS = "A" });

            //context.AslUsercoDbSet.Add(new AslUserco() { COMPID = 101, USERID = 10101, USERNM = "Raju Chowdhury", DEPTNM = "Admin", OPTP = "CompanyAdmin", ADDRESS = "jamalkhan road,Chittagong", MOBNO = "8801745555555", EMAILID = "savron01@gmail.com", LOGINBY = "EMAIL", LOGINID = "savron01@gmail.com", LOGINPW = "123", TIMEFR = "00:00", TIMETO = "23:59", STATUS = "A" });
            //context.AslUsercoDbSet.Add(new AslUserco() { COMPID = 101, USERID = 10102, USERNM = "Shamin ullah", DEPTNM = "Account", OPTP = "User", ADDRESS = "jamalkhan road,Chittagong", MOBNO = "8801744444445", EMAILID = "savron02@gmail.com", LOGINBY = "EMAIL", LOGINID = "savron02@gmail.com", LOGINPW = "123", TIMEFR = "00:00", TIMETO = "23:59", STATUS = "A" });
            //context.AslUsercoDbSet.Add(new AslUserco() { COMPID = 102, USERID = 10201, USERNM = "Riaz Talukdar", DEPTNM = "Admin", OPTP = "CompanyAdmin", ADDRESS = "GEC road,Chittagong", MOBNO = "8801744444444", EMAILID = "metro01@gmail.com", LOGINBY = "EMAIL", LOGINID = "metro01@gmail.com", LOGINPW = "123", TIMEFR = "00:00", TIMETO = "23:59", STATUS = "A" });

            //context.SaveChanges();

            //context.AslMenumstDbSet.Add(new ASL_MENUMST() { MODULEID = "01", MODULENM = "User Module" });
            //context.AslMenumstDbSet.Add(new ASL_MENUMST() { MODULEID = "02", MODULENM = "Therapy Module" });
            //context.AslMenumstDbSet.Add(new ASL_MENUMST() { MODULEID = "03", MODULENM = "Account Module" });


            //context.AslMenuDbSet.Add(new ASL_MENU() { MODULEID = "01", MENUTP = "F", MENUID = "F0101", MENUNM = "User Information", ACTIONNAME = "Create", CONTROLLERNAME = "AslUserCo" });
            //context.AslMenuDbSet.Add(new ASL_MENU() { MODULEID = "01", MENUTP = "R", MENUID = "R0101", MENUNM = "User Log Data List", ACTIONNAME = "GetCompanyUserLogData", CONTROLLERNAME = "UserReport" });


            //context.AslRoleDbSet.Add(new ASL_ROLE() { COMPID = 101, USERID = 10101, MODULEID = "01", MENUTP = "F", MENUID = "F0101", MENUNAME = "User Information", ACTIONNAME = "Create", CONTROLLERNAME = "AslUserCo", STATUS = "A", INSERTR = "A", UPDATER = "A", DELETER = "A" });
            //context.AslRoleDbSet.Add(new ASL_ROLE() { COMPID = 101, USERID = 10101, MODULEID = "01", MENUTP = "R", MENUID = "R0101", MENUNAME = "User Log Data List", ACTIONNAME = "GetCompanyUserLogData", CONTROLLERNAME = "UserReport", STATUS = "A", INSERTR = "I", UPDATER = "I", DELETER = "I" });
            ////.....................................
            //context.AslRoleDbSet.Add(new ASL_ROLE() { COMPID = 101, USERID = 10102, MODULEID = "01", MENUTP = "F", MENUID = "F0101", MENUNAME = "User Information", ACTIONNAME = "Create", CONTROLLERNAME = "AslUserCo", STATUS = "I", INSERTR = "I", UPDATER = "I", DELETER = "I" });
            //context.AslRoleDbSet.Add(new ASL_ROLE() { COMPID = 101, USERID = 10102, MODULEID = "01", MENUTP = "R", MENUID = "R0101", MENUNAME = "User Log Data List", ACTIONNAME = "GetCompanyUserLogData", CONTROLLERNAME = "UserReport", STATUS = "I", INSERTR = "I", UPDATER = "I", DELETER = "I" });
            ////.....................................
            //context.AslRoleDbSet.Add(new ASL_ROLE() { COMPID = 102, USERID = 10201, MODULEID = "01", MENUTP = "F", MENUID = "F0101", MENUNAME = "User Information", ACTIONNAME = "Create", CONTROLLERNAME = "AslUserCo", STATUS = "A", INSERTR = "A", UPDATER = "A", DELETER = "A" });
            //context.AslRoleDbSet.Add(new ASL_ROLE() { COMPID = 102, USERID = 10201, MODULEID = "01", MENUTP = "R", MENUID = "R0101", MENUNAME = "User Log Data List", ACTIONNAME = "GetCompanyUserLogData", CONTROLLERNAME = "UserReport", STATUS = "A", INSERTR = "I", UPDATER = "I", DELETER = "I" });


            // context.SaveChanges();
            //base.Seed(context);
        }
    }
}