using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;

namespace WPFClient.SidebarFolders
{
    class DeletedFolder : SidebarFolder
    {
        public DeletedFolder()
        {
            FolderLabel = Properties.Resources.DeletedFolderLabel;
        }

        public override List<MessageListItemModel> GetFolderContent()
        {
            List<MessageListItemModel> messageModels = new List<MessageListItemModel>();  
            foreach (Message item in App.ServiceWatcher.DeletedInboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        Viewed = false,
                        Type = MessageType.inbox                     
                    });
            }
            foreach( Message item in App.ServiceWatcher.ViewedDeletedInboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        Viewed = true,
                        Type = MessageType.inbox
                    });
            }
            foreach (Message item in App.ServiceWatcher.DeletedSentboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        Viewed = true,
                        Type = MessageType.sentbox  
                    });
            }            
            return messageModels;
        }
    }
}
