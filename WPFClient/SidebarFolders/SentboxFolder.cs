using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.OtherModels;

namespace WPFClient.SidebarFolders
{
    class SentboxFolder : SidebarFolder
    {
        public SentboxFolder()
        {
            FolderLabel = Properties.Resources.SentboxFolderLabel;
        }

        List<Message> sentboxMessages = new List<Message>();

        List<MessageListItemModel> messageModels = new List<MessageListItemModel>();

        bool hasUnprocessedData = true;   

        public override List<MessageListItemModel> GetFolderContent()
        {
            if (hasUnprocessedData)
            {
                hasUnprocessedData = false;
                messageModels.Clear();
                foreach (Message item in sentboxMessages)
                {
                    messageModels.Add(
                        new MessageListItemModel()
                        {
                            Message = item,
                            Viewed = true, //отправленные письма не могут быть непрочитанными
                            Type = MessageParentType.Sentbox
                        });
                }                
            }
            return messageModels;            
        }

        public override void RefreshFolderContent()
        {
            hasUnprocessedData = true;
            base.UpdateMessages(ServiceInterface.FolderType.Sentbox, ServiceInterface.MessageTypes.Unknown, sentboxMessages);
        }
    }
}