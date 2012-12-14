using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entities.Additional
{
    public class MessagesPack
    {
        List<Message> NewMessages { get; set; }

        List<int> DeletedMessagesIds { get; set; }

        Dictionary<int, List<Recipient>> ChangedRecipients { get; set; }

        Dictionary<int, List<Recipient>> NewRecipients { get; set; }

        Dictionary<int, List<Recipient>> DeletedRecipients { get; set; }

        Dictionary<int, Employee> ChangedSender { get; set; }

       
    }
}
