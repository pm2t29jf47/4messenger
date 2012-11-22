using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.SidebarFolders
{
    class InboxFolder : SidebarFolder
    {
        public InboxFolder()
        {
            FolderLabel = Properties.Resources.InboxFolderLabel;
        }

        public override List<Entities.Message> GetFolderContent()
        {
            List<Message> messages = App.Proxy.GetInboxMessages();
            FillMessages(messages);
            return messages;
        }
    }
}
