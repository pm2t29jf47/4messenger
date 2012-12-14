using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Additional
{
    public class VersionedEmployee
    {
        public string Username { get; set; }

        public byte[] Version { get; set; }

        public List<VersionedMessage> SentMessages { get; set; }

        public List<VersionedRecipient> InRecipients { get; set; }
    }
}
