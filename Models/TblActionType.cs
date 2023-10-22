using System;
using System.Collections.Generic;

namespace TMS.Models
{
    public partial class TblActionType
    {
        public TblActionType()
        {
            TblToolAssign = new HashSet<TblToolAssign>();
        }

        public int Id { get; set; }
        public string ActionTypeCode { get; set; }
        public string ActionTypeName { get; set; }

        public ICollection<TblToolAssign> TblToolAssign { get; set; }
    }
}
