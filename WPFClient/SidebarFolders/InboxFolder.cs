using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;

namespace WPFClient.SidebarFolders
{
    class InboxFolder : SidebarFolder
    {
        public InboxFolder()
        {
            FolderLabel = Properties.Resources.InboxFolderLabel;
        }

        public override List<MessageListItemModel> GetFolderContent()
        {
            List<MessageListItemModel> messageModels = new List<MessageListItemModel>();
            foreach (Message item in App.ServiceWatcher.InboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        Viewed = false
                    });
            }
            foreach (Message item in App.ServiceWatcher.ViewedInboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        Viewed = true
                    });
            }
            messageModels = messageModels.OrderBy(row => row.Date).ToList();
            return messageModels;
        }

        public void RefreshCountOfUnViewedMessages()
        {
            int result = App.ServiceWatcher.InboxMessages.Count;
            
            if (CountOfUnviewedMessages != result)
            {
                CountOfUnviewedMessages = result;                
            }            
        }
    }
}
