using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;
using WPFClient.OtherModels;
using ServiceInterface;

namespace WPFClient.SidebarFolders
{
    class DeletedFolder : SidebarFolder
    {
        public DeletedFolder()
        {
            FolderLabel = Properties.Resources.DeletedFolderLabel;
        }

        List<Message> deletedInboxMessages = new List<Message>();

        List<Message> viewedDeletedInboxMessages = new List<Message>();

        List<Message> deletedSentboxMessages = new List<Message>();

        List<MessageListItemModel> messageModels = new List<MessageListItemModel>();

        bool hasUnprocessedData = true;

        public override List<MessageListItemModel> GetFolderContent()
        {
            if (hasUnprocessedData)
            {
                hasUnprocessedData = false;
                messageModels.Clear();
                foreach (Message item in deletedInboxMessages)
                {
                    messageModels.Add(
                        new MessageListItemModel()
                        {
                            Message = item,
                            Viewed = false,
                            Type = MessageParentType.Inbox
                        });
                }
                foreach (Message item in viewedDeletedInboxMessages)
                {
                    messageModels.Add(
                        new MessageListItemModel()
                        {
                            Message = item,
                            Viewed = true,
                            Type = MessageParentType.Inbox
                        });
                }
                foreach (Message item in deletedSentboxMessages)
                {
                    messageModels.Add(
                        new MessageListItemModel()
                        {
                            Message = item,
                            Viewed = true,
                            Type = MessageParentType.Sentbox
                        });
                }            

            }         
            return messageModels;
        }

        public override void RefreshFolderContent()
        {
            hasUnprocessedData = true;
            UpdateMessages(FolderType.Inbox, MessageTypes.Deleted, deletedInboxMessages);        
            UpdateMessages(FolderType.Inbox, MessageTypes.Deleted | MessageTypes.Viewed, viewedDeletedInboxMessages);
            UpdateMessages(FolderType.Sentbox, MessageTypes.Deleted, deletedSentboxMessages);  
        }
    }
}
