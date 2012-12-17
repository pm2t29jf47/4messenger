using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;

namespace WPFClient.SidebarFolders
{
    class SentboxFolder : SidebarFolder
    {
        public SentboxFolder()
        {
            FolderLabel = Properties.Resources.SentboxFolderLabel;
        }

        public override List<MessageListItemModel> GetFolderContent()
        {
            List<Message> messages = App.ServiceWatcher.SentboxMessages;
             List<MessageListItemModel> messageModels = new List<MessageListItemModel>();
             foreach (Message item in messages)
             {
                 messageModels.Add(
                     new MessageListItemModel()
                     {
                         Message = item
                     });
             }
             return messageModels;
        }
    }
}