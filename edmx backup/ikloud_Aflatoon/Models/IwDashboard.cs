using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
//using System.Data.EntityModel;
using System.ComponentModel.DataAnnotations;

namespace ikloud_Aflatoon.Models
{
    public class IWSummary
    {//--------------------------------IWSummery
          [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ProcDate { get; set; }
        public string SetlmntDate { get; set; }
        public int BatchNo { get; set; }
        public int ReturnCount { get; set; }
        public int TotalCount { get; set; }
        public decimal ReturnAmt { get; set; }

    }
    public class PXFSummary
    {
        //------------------------------------PXFSummery
        public string SessionNo { get; set; }
        public string FileName { get; set; }
        public int FileCount { get; set; }
        public decimal FileAmount { get; set; }
     }
    public class PXFS
    {
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }
        public string SessionNo { get; set; }
        public int NoOfFiles { get; set; }
        public int FileInstrumentCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class IWBatchsummary
    {
        //-----------------------------------IWBatchsummery
        public int BatchNo { get; set; }
          [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime ProcDate { get; set; }
        public int BatchCount { get; set; }
        public string BatchAmount { get; set; }

        public string BatchDesc { get; set; }
              
    }
    public class IWBatchs
    {
        
        public int NoOfBatches { get; set; }
        public int TotalCount { get; set; }
        public decimal TotalAmount { get; set; }
    }
    public class IWDEStatus//-----------------------------------IWDEStatus
    { 

        //----------------------------------MICR
       
        public int MicrPendingDE { get; set; }
        public int MicrCompletedDE { get; set; }
        public int MicrRejectedDE { get; set; }
      //  public int AmtCorectDE { get; set; }

        //----------------------------------Amount
        public int AmtTotal { get; set; }

        public int AmtPendingDE { get; set; }
        public int AmtCompletedDE { get; set; }
        public int AmtRejectedDE { get; set; }
        public int AmtCorectDE { get; set; }

        public int AmtPendingQC { get; set; }
        public int AmtCompletedQC { get; set; }
        public int AmtLockQC { get; set; }
        public int AmtCorectQC { get; set; }


         //----------------------------------Date
         public int DTotal { get; set; }

         public int DPendingDE { get; set; }
         public int DCompletedDE { get; set; }
         public int DRejectedDE { get; set; }
         public int DCorectDE { get; set; }

         public int DPendingQC { get; set; }
         public int DCompletedQC { get; set; }
         public int DRejectedQC { get; set; }
         public int DCorectQC { get; set; }

      

         //----------------------------------AccNO
         public int ATotal { get; set; }

         public int APendingDE { get; set; }
         public int ACompletedDE { get; set; }
         public int ARejectedDE { get; set; }
         public int ACorectDE { get; set; }

         public int APendingQC { get; set; }
         public int ACompletedQC { get; set; }
         public int ALockQC { get; set; }
         public int ACorectQC { get; set; }

        

         //----------------------------------PayeeName
         public int PTotal { get; set; }

         public int PPendingDE { get; set; }
         public int PCompletedDE { get; set; }
         public int PLockDE { get; set; }
         public int PRejectedDE { get; set; }

         public int PPendingQC { get; set; }
         public int PCompletedQC { get; set; }
         public int PLockQC { get; set; }
         public int PCorectQC { get; set; }

        //-----------------------------------TechVerf
         public int ATotalVerf { get; set; }
         public int APendingVerf { get; set; }
         public int AHoldVerf { get; set; }
         public int ARjctVerf { get; set; }
         public int AAcceptedVerf { get; set; }
         public int ALockVerf { get; set; }
         public int ACorectVerf { get; set; }
         //-----------------------------------SignVerf
         public int ATotalsVerf { get; set; }
         public int APendingsVerf { get; set; }
         public int AHoldsVerf { get; set; }
         public int ARjctsVerf { get; set; }
         public int AAcceptedsVerf { get; set; }
         public int ALocksVerf { get; set; }
         public int ACorectsVerf { get; set; }
         //-----------------------------------ReVerf
         public int ATotalRVerf { get; set; }
         public int APendingRVerf { get; set; }
         public int AHoldRVerf { get; set; }
         public int ARjctRVerf { get; set; }
         public int AAcceptedRVerf { get; set; }
         public int ALockRVerf { get; set; }
         public int ACorectRVerf { get; set; }

        //------------- Extra Field -------- By Abid-----
         public string DomainName { get; set; }
         public int TotalIW { get; set; }

     }
    public class ChartVerf
    {
        //-----------------------------------TechVerf
        public int cTotalVerf { get; set; }
        public int cPendingVerf { get; set; }
        public int cHoldVerf { get; set; }
        public int cRjctVerf { get; set; }
        public int cAcceptedVerf { get; set; }
        public int cLockVerf { get; set; }
        public int cCorectVerf { get; set; }
        //-----------------------------------SignVerf
        public int cTotalsVerf { get; set; }
        public int cPendingsVerf { get; set; }
        public int cHoldsVerf { get; set; }
        public int cRjctsVerf { get; set; }
        public int cAcceptedsVerf { get; set; }
        public int cLocksVerf { get; set; }
        public int cCorectsVerf { get; set; }

    }
    public class IwSearch
    {
    }
    public class IwLockUnlock
    {
    }
}