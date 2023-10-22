using System;
using System.Collections.Generic;

namespace LILI_TTS.Temp_Models
{
    public partial class TblTsatoolReceiveAttachment
    {
        public int Id { get; set; }
        public string AttachmentCode { get; set; }
        public DateTime AttachmentDate { get; set; }
        public string Tsacode { get; set; }
        public string AttachmentTypeCode { get; set; }
        public string Description { get; set; }
        public string Iuser { get; set; }
        public string Euser { get; set; }
        public DateTime Idate { get; set; }
        public DateTime? Edate { get; set; }
        public string FileName { get; set; }
        public string OriginalFileName { get; set; }
        public string Location { get; set; }
        public string DocumentType { get; set; }

        public TblAttachmentType AttachmentTypeCodeNavigation { get; set; }
        public TblTsatoolReceiveAttachment IdNavigation { get; set; }
        public TblTsasetup TsacodeNavigation { get; set; }
        public TblTsatoolReceiveAttachment InverseIdNavigation { get; set; }
    }
}
