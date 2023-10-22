using System;
using System.Collections.Generic;

namespace LILI_TTS.Models
{
    public partial class TblAction
    {
        public TblAction()
        {
            TblToolAssign = new HashSet<TblToolAssign>();
        }

        public int Id { get; set; }
        public string ActionCode { get; set; }
        public string ActionName { get; set; }

        public ICollection<TblToolAssign> TblToolAssign { get; set; }
    }
}
