using LILI_TTS.Models;
using System;
using System.Collections.Generic;

namespace LILI_TTS.Models
{
    public partial class TblAttachmentType
    {
        public TblAttachmentType()
        {
            TblTsatoolReceiveAttachment = new HashSet<TblTsatoolReceiveAttachment>();
        }

        public int Id { get; set; }
        public string AttachmentTypeCode { get; set; }
        public string AttachmentTypeName { get; set; }

        public ICollection<TblTsatoolReceiveAttachment> TblTsatoolReceiveAttachment { get; set; }
    }
}
