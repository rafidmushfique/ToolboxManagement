using System;
using System.Collections.Generic;

namespace LILI_TTS.Models
{
    public partial class TblTsasetup
    {
        public TblTsasetup()
        {
            TblToolAssign = new HashSet<TblToolAssign>();
            TblTsatoolReceiveAttachment = new HashSet<TblTsatoolReceiveAttachment>();
        }

        public int Id { get; set; }
        public string Tsacode { get; set; }
        public string Tsaname { get; set; }
        public string Designation { get; set; }
        public string MobileNo { get; set; }
        public string AreaCode { get; set; }
        public string RegionCode { get; set; }
        public string Description { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }

        public TblArea AreaCodeNavigation { get; set; }
        public TblRegion RegionCodeNavigation { get; set; }
        public ICollection<TblToolAssign> TblToolAssign { get; set; }
        public ICollection<TblTsatoolReceiveAttachment> TblTsatoolReceiveAttachment { get; set; }
    }
}
