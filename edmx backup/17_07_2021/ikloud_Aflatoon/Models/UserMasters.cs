using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ikloud_Aflatoon.Models
{

    //public partial class UserMaster
    //{
    //    [Key]
    //    public int ID { get; set; }
    //    [Required]
    //    [StringLength(20, MinimumLength = 4)]
    //    [Display(Name = "Login ID")]
    //    public string LoginID { get; set; }
    //    [Required]
    //    [StringLength(100, MinimumLength = 8)]
    //    [DataType(DataType.Password)]
    //    public string Password { get; set; }
    //    [Required]
    //    public string Title { get; set; }
    //    [Required]
    //    [StringLength(30, MinimumLength = 3)]
    //    [Display(Name = "First Name")]
    //    public string FirstName { get; set; }
    //    [StringLength(30, MinimumLength = 3)]
    //    [Display(Name = "Last Name")]
    //    public string LastName { get; set; }
    //    [DataType(DataType.Date)]
    //    [Display(Name = "Date of Birth")]
    //    public Nullable<System.DateTime> DOB { get; set; }
    //    [StringLength(50, MinimumLength = 10)]
    //    [Display(Name = "Email Address")]
    //    [DataType(DataType.EmailAddress)]
    //    public string EmailID { get; set; }
    //    [DefaultValue(true)]
    //    public bool Active { get; set; }
    //    [Display(Name = "Last Login")]
    //    public Nullable<System.DateTime> LastLogin { get; set; }
    //    [DefaultValue(0)]
    //    public int InvalidPasswordAttempts { get; set; }
    //    [DefaultValue(false)]
    //    public bool UserDeleted { get; set; }
    //    public int loginFlg { get; set; }
    //    [Required]
    //    public Nullable<System.DateTime> creationdate { get; set; }
    //    [Required]
    //    public Nullable<int> createdby { get; set; }
    //    public string sessionid { get; set; }
    //    public bool FirstLogin { get; set; }
    //    [Required]
    //    public int AppSecPolicieID { get; set; }
    //    [Required]
    //    [Display(Name = "User Type")]
    //    public int UsertType { get; set; }
    //    public Nullable<int> ModifedBy { get; set; }
    //    [Required]
    //    [Display(Name = "Location")]
    //    public string City { get; set; }
    //    public decimal L1StartLimit { get; set; }
    //    public decimal L1StopLimit { get; set; }
    //    public decimal L2StartLimit { get; set; }
    //    public decimal L2StopLimit { get; set; }
    //    public decimal L3StartLimit { get; set; }
    //    public decimal L3StopLimit { get; set; }
    //}
    //public partial class UserDomainMapping
    //{
    //    public virtual int ID { get; set; }
    //    public virtual UserMaster User { get; set; }
    //    public virtual Domain Domain { get; set; }
    //}
    //public partial class AppSecPolicies
    //{
    //    public int ID { get; set; }
    //    public string Name { get; set; }
    //    public int PwdExpiryDays { get; set; }
    //    [Range(0, 20)]
    //    public int MinPwdLength { get; set; }
    //    [Range(0, 20)]
    //    public int MaxPwdLength { get; set; }
    //    public bool AlphanumericMandatory { get; set; }
    //    public bool SpecialCharMandatory { get; set; }
    //    public int InvalidAttemptsAllowed { get; set; }
    //    public int DeactivationDays { get; set; }
    //}
    //public partial class RoleMapping
    //{
    //    public int ID { get; set; }
    //    [Required]
    //    public int UserID { get; set; }
    //    [Required]
    //    [StringLength(15)]
    //    public string Process { get; set; }
    //    public bool Active { get; set; }
    //}



}
