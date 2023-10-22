using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TMS.Models
{
    public partial class TblToolAssign
    {

        public int Id { get; set; }
        public string AssignCode { get; set; }
        public string Tsacode { get; set; }
        public string ToolCode { get; set; }
        public string ActionCode { get; set; }
        public string ActionTypeCode { get; set; }
        public decimal Quantity { get; set; }
        public DateTime ActionDate { get; set; }= DateTime.Now;
        public string Description { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public TblAction ActionCodeNavigation { get; set; }
        public TblActionType ActionTypeCodeNavigation { get; set; }
        public TblToolsSetup ToolCodeNavigation { get; set; }
        public TblTsasetup TsacodeNavigation { get; set; }
    

    }
}
