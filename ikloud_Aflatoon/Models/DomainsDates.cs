using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations;
namespace iKloud_Aflatoon.Models
{
    public class DomainsDates
    {
        public int DomainID { get; set; }
        public string DomainName { get; set; }
        [DataType(DataType.Date)]
        public DateTime ProcessDates { get; set; }
        public string ClearingType { get; set; }
        public DateTime PostDate { get; set; }
        public DateTime StaleDate { get; set; }
        public int CustomerID { get; set; }
    }
}