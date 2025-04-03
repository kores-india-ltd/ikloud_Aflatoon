using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ikloud_Aflatoon.Models
{
    public class UserBrMap
    {
       public int userid { get; set; }
       public int Branchid { get; set; }
    }
    public class selectbranches
    {
        public string domainname { get; set; }
        public string branchname { get; set; }
        public int brid { get; set; }
        public int domainID { get; set; }
    }
    public class SelectDomain
    {
        public string domainName { get; set; }
        //public IQueryable<String> modelOne;
        //public IQueryable<String> modelTwo;
    }
    public class SelectedUsrDomain
    {
        public string domainName { get; set; }
        //public IQueryable<String> modelOne;
        //public IQueryable<String> modelTwo;
        
    }
    public class Selectedverfnlvel
    {
        public string Levelname { get; set; }
        public int Lvlid { get; set; }
        //public IQueryable<String> modelOne;
        //public IQueryable<String> modelTwo;
    }
}