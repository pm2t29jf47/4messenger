using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Additional
{
    public class VersionedMessage
    {
        public int Id { get; set; }

        public byte[] Version { get; set; }

        public VersionedEmployee Sender { get; set; }

        public List<VersionedRecipient> Recipients { get; set; }
    }
}
