using LILI_TTS.Controllers;
using LILI_TTS.Models;
using System;
using System.Collections.Generic;

namespace LILI_TTS.Models
{
    public class TsaAssignedInfoViewModel
    {
        public TsaAssignedInfoViewModel()
        {
            assignedTool = new List<TblToolAssign>();
        }

        public string Tsacode { get; set; }
        public string Designation { get; set; }
        public string AreaName { get; set; }
        public string RegionName { get; set; }
        public string htmlBuilder { get; set; }
        public List<TblToolAssign> assignedTool {  get; set; }

    }
}
