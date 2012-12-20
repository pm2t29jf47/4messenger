using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.OtherModels;

namespace WPFClient.SidebarFolders
{
    class InboxFolder : SidebarFolder
    {
        public InboxFolder()
        {
            FolderLabel = Properties.Resources.InboxFolderLabel;
        }

        List<Message> inboxMessages = new List<Message>();

        List<Message> viewedInboxMessages = new List<Message>();

        List<MessageListItemModel> messageModels = new List<MessageListItemModel>();

        bool hasUnprocessedData = true;

        public override List<MessageListItemModel> GetFolderContent()
        {
            if (hasUnprocessedData)
            {
                hasUnprocessedData = false;
                messageModels.Clear();
                foreach (Message item in inboxMessages)
                {
                    messageModels.Add(
                        new MessageListItemModel()
                        {
                            Message = item,
                            Viewed = false,
                            Type = MessageParentType.Inbox
                        });
                }
                foreach (Message item in viewedInboxMessages)
                {
                    messageModels.Add(
                        new MessageListItemModel()
                        {
                            Message = item,
                            Viewed = true,
                            Type = MessageParentType.Inbox
                        });
                }
                messageModels = messageModels.OrderBy(row => row.Date).ToList();
            }
            return messageModels;
        }

        public void RefreshCountOfUnViewedMessages()
        {
            int result = inboxMessages.Count;

            if (base.CountOfUnviewedMessages != result)
            {
                base.CountOfUnviewedMessages = result;                
            }            
        }

        public override void RefreshFolderContent()
        {
            hasUnprocessedData = true;
            base.UpdateMessages(ServiceInterface.FolderType.Inbox, ServiceInterface.MessageTypes.Unknown, inboxMessages);
            base.UpdateMessages(ServiceInterface.FolderType.Inbox, ServiceInterface.MessageTypes.Viewed, viewedInboxMessages);
        }
    }
}
