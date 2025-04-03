using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Web.Security;

namespace ikloud_Aflatoon.Models
{
    public class AppStResult : IValidatableObject
    {

        private UserAflatoonDbContext db = new UserAflatoonDbContext();

        [Required]
        public string name { get; set; }
        [Required]
        public string pass { get; set; }
        // public List<String> dom { get; set; }
        public bool validate(string lname)
        {
            var model = (from u in db.UserMasters
                         where u.LoginID == lname || lname == null
                         select new loginDet
                         {
                             id = u.ID,
                             firstname = u.FirstName,
                             title = u.Title
                         });

            //var resultset = from u in db.Users
            //              where u.LoginID.Equals(lname) && u.Active.Equals(1)
            //                  select new { u.ID, u.Title, u.FirstName};

            if (model.Count() != 0)
            {
                return true;
            }
            else
                return false;
            //Int32 cont =  Convert.ToInt16(resulset.Count());

        }

        public IEnumerable<ValidationResult>
            Validate(ValidationContext validationContext)
        {
            if (pass == null || name == null)
            {
                yield return new ValidationResult("Field should not be empty!");
            }
        }
    }
    public class loginDet
    {
        public int id { get; set; }
        public string title { get; set; }
        public string firstname { get; set; }
        public int custid { get; set; }
        //*********** set Method ***************
        public void settid(int id)
        {
            this.id = id;
        }
        public void settitle(string title)
        {
            this.title = title;
        }
        public void setfirstname(string firstname)
        {
            this.firstname = firstname;
        }
        //********* get Method **************
        public int getid()
        {
            return this.id;
        }
        public string gettitle()
        {
            return this.title;
        }
        public string getfirstname()
        {
            return this.firstname;
        }

    }
    public class session : HttpSessionStateBase
    {
        public int userid { get; set; }
    }
    //******************* Creating De Groups *************************

    public class AllFields
    {
        public int AppSettingID { get; set; }
        public int CustomerID { get; set; }
        ////public int MyProperty { get; set; }
        public int DEGRpID { get; set; }
        public string GroupName { get; set; }
        public int DEGRMID { get; set; }
        public string DEFLDName { get; set; }
        public bool CaptureSlipAccount { get; set; }
        public bool CaptureSlipAmount { get; set; }
        public bool CaptureChqAmount { get; set; }
        public bool CaptureChqDate { get; set; }
        public bool CaptureChqCrdAccount { get; set; }
        public bool CaptureChqDbtAccount { get; set; }
        public bool CaptureChqPayeeName { get; set; }
        public bool CaptureChqDraweeName { get; set; }
    }
    public class commonUserCreate
    {
        public string process { get; set; }
        public bool active { get; set; }
        public string Verflevelname { get; set; }
    }
    public class Dggrpname
    {
        public string dgname { get; set; }
        public string GroupName { get; set; }
    }
    public class changePassword
    {
        [Display(Name = "User Id")]
        public string UserId { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Enter Old Password")]
        public string OldPassword { get; set; }
        [Display(Name = "Enter New Password")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
        [Display(Name = "Confirm New Password")]
        [DataType(DataType.Password)]
        public string ConfrmPassword { get; set; }
        public int flg { get; set; }
        public int loginUsrid { get; set; }
        public int firstlogin { get; set; }
        public int policyid { get; set; }
        public int succ { get; set; }
        public int minpwdlength { get; set; }
        public int maxpwdlength { get; set; }
        public bool Aplhanumericmadate { get; set; }
        public bool Specialcharmandate { get; set; }

    }
    public class PolicyDetails
    {
        public string policyname { get; set; }
        public int pwdexprydat { get; set; }
        public int minpwdlength { get; set; }
        public int maxpwdlength { get; set; }
        public bool Aplhanumericmadate { get; set; }
        public bool Specialcharmandate { get; set; }
        public int Invalidattamtallow { get; set; }
        public int Deactivationdays { get; set; }
    }


}