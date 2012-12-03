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
            List<Message> inboxMessages = App.ServiceWatcher.GetInboxMessages();    
            List<MessageListItemModel> messageModels = new List<MessageListItemModel>();
            foreach (Message item in inboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        IsViewed = IsViewedMessage(item)
                    });
            }
            return messageModels;
        }
    }
}
