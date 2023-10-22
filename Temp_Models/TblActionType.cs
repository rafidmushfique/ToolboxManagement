using System;
using System.Collections.Generic;

namespace LILI_TTS.Temp_Models
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
