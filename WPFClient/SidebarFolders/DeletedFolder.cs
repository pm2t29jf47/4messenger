using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.SidebarFolders
{
    class DeletedFolder : SidebarFolder
    {
        public DeletedFolder()
        {
            FolderLabel = Properties.Resources.DeletedFolderLabel;
        }

        public override List<Entities.Message> GetFolderContent()
        {
            List<Message> messages = App.ServiceWatcher.GetDeletedMessages();
            return messages;
        }
    }
}
