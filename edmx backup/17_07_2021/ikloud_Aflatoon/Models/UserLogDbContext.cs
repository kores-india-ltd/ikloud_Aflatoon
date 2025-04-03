using System.Data.Entity;
using System;

namespace ikloud_Aflatoon.Models
{
    public class UserLogDbContext:DbContext 
    {
       public UserLogDbContext()
           : base("name=AflatoonConnectionString")
        { }
       public virtual DbSet<UserMasterActivity> UserMasterActivities { get; set; }
       public virtual DbSet<PasswordHistory> PasswordHistories { get; set; }
       public virtual DbSet<LoginLogoutAudit> LoginLogoutAudits { get; set; }
       public virtual DbSet<UserMaster> UserMasters { get; set; }
    }
}