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
            List<Message> messages = App.ServiceWatcher.GetDeletedInboxMessages();
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
