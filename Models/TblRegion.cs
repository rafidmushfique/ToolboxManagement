using System;
using System.Collections.Generic;

namespace TMS.Models
{
    public partial class TblRegion
    {
        public TblRegion()
        {
            TblTsasetup = new HashSet<TblTsasetup>();
        }

        public int Id { get; set; }
        public string RegionCode { get; set; }
        public string RegionName { get; set; }

        public ICollection<TblTsasetup> TblTsasetup { get; set; }
    }
}
