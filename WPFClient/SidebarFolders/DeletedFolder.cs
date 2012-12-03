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
            List<Message> deletedInboxMessages = App.ServiceWatcher.GetDeletedInboxMessages();
            List<Message> deletedSentboxMessages = App.ServiceWatcher.GetDeletedSentboxMessages();
            List<MessageListItemModel> messageModels = new List<MessageListItemModel>();
            foreach (Message item in deletedInboxMessages)
            {
                messageModels.Add(
                    new MessageListItemModel()
                    {
                        Message = item,
                        IsViewed = IsViewedMessage(item)
                    });
            }
            foreach (Message item in deletedSentboxMessages)
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
