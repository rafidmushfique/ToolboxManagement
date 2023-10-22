using System;
using System.Collections.Generic;

namespace LILI_TTS.Temp_Models
{
    public partial class TblArea
    {
        public TblArea()
        {
            TblTsasetup = new HashSet<TblTsasetup>();
        }

        public int Id { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }

        public ICollection<TblTsasetup> TblTsasetup { get; set; }
    }
}
