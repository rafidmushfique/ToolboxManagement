using System;
using System.Collections.Generic;

namespace LILI_TTS.Models
{
    public partial class TblToolsSetup
    {
        public TblToolsSetup()
        {
            TblToolAssign = new HashSet<TblToolAssign>();
        }

        public int Id { get; set; }
        public string ToolCode { get; set; }
        public string ToolName { get; set; }
        public string Brand { get; set; }
        public string Unit { get; set; }
        public string Description { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public ICollection<TblToolAssign> TblToolAssign { get; set; }
    }
}
