using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;

namespace WPFClient.SidebarFolders
{
    class SentboxFolder : SidebarFolder
    {
        public SentboxFolder()
        {
            FolderLabel = Properties.Resources.SentboxFolderLabel;
        }

        public override List<Entities.Message> GetFolderContent()
        {
            List<Message> messages = App.ServiceWatcher.GetSentboxMessages();          
             return messages;
        }
    }
}