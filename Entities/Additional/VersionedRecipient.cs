using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Additional
{
    public class VersionedRecipient
    {
        public string RecipientUsername { get; set; }

        public int MessageId { get; set; }

        public byte[] Version;

        public VersionedEmployee RecipientEmployee { get; set; }

        public VersionedMessage Message { get; set; }
    }
}
