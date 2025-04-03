using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ikloud_Aflatoon.Models
{
    public class CMSReferral
    {
        public Int64 ID { get; set; }
        public Int64 RawDataID { get; set; }
        public int Status { get; set; }
        public string ClientCode { get; set; }
        public string PayeeName { get; set; }
        public string SubcustomerCode { get; set; }
   
        public string FrontGrayImagePath { get; set; }
        public string BackGreyImagePath { get; set; }
        public int CustomerId { get; set; }
        public int DomainId { get; set; }
        public int ScanningNodeId { get; set; }
        public int SlipNo { get; set; }
        public int BatchNo { get; set; }
        public string BranchCode { get; set; }

    }
}