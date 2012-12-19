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

        List<Message> inboxMessages = new List<Message>();

        List<Message> viewedInboxMessages = new List<Message>();

        List<MessageListItemModel> messageModels = new List<MessageListItemModel>();

        bool hasUnprocessedData = true;

        public override List<MessageListItemModel> GetFolderContent()
        {
            if (hasUnprocessedData)
            {
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
                return messageModels;
            }
            else
            {
                return messageModels;
            }

            //List<MessageListItemModel> messageModels = new List<MessageListItemModel>();
            //foreach (Message item in App.ServiceWatcher.InboxMessages)
            //{
            //    messageModels.Add(
            //        new MessageListItemModel()
            //        {
            //            Message = item,
            //            Viewed = false,
            //            Type = MessageParentType.Inbox
            //        });
            //}
            //foreach (Message item in App.ServiceWatcher.ViewedInboxMessages)
            //{
            //    messageModels.Add(
            //        new MessageListItemModel()
            //        {
            //            Message = item,
            //            Viewed = true,
            //            Type = MessageParentType.Inbox
            //        });
            //}
            //messageModels = messageModels.OrderBy(row => row.Date).ToList();
            //return messageModels;
        }

        public void RefreshCountOfUnViewedMessages()
        {
            int result = inboxMessages.Count;

            if (base.CountOfUnviewedMessages != result)
            {
                base.CountOfUnviewedMessages = result;                
            }            
        }

        public void RefreshFolderContent()
        {


        }
    }
}
