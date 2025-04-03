using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ikloud_Aflatoon.Models
{
    public class SodusrDbCon : DbContext
    {
        public SodusrDbCon()
            : base("name=AflatoonEntities")
        { }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        //public virtual DbSet<> SODs { get; set; }
        public virtual DbSet<IwSoD> IwSODs { get; set; }
        public virtual DbSet<OwSoD> OwSODs { get; set; }
        public virtual DbSet<ActivityLog> ActivityLogs { get; set; }
    }
}