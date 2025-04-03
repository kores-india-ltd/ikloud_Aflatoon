using System.Data.Entity;
using System;


namespace ikloud_Aflatoon.Models
{
    public class UserAflatoonDbContext : DbContext
    {
        public UserAflatoonDbContext()
            : base("name=AflatoonEntities")
        {

        }
        public virtual DbSet<UserMaster> UserMasters { get; set; }
        public virtual DbSet<UserDomainMapping> UserDomainMappings { get; set; }
        public virtual DbSet<AppSecPolicy> AppSecPolicies { get; set; }
        public virtual DbSet<RoleMapping> RoleMappings { get; set; }
        public virtual DbSet<Domain> Domains { get; set; }
        public virtual DbSet<DomainMaster> DomainMaster { get; set; }
        public virtual DbSet<DomainUserMapMaster> DomainUserMapMasters { get; set; }
        public virtual DbSet<UserOrganizationMapping> UserOrganizationMappings { get; set; }
        public virtual DbSet<UserCustomerMapping> UserCustomerMappings { get; set; }


    }
}