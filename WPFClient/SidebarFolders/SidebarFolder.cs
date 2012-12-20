using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.OtherModels;
using System.ComponentModel;
using ServiceInterface;
using Entities.Additional;
using WPFClient.Additional;

namespace WPFClient.SidebarFolders
{
    class SidebarFolder : INotifyPropertyChanged
    {
        string folderLabel = "New folder";  

        public string FolderLabel 
        {
            set
            {
                if (string.Compare(folderLabel, value) != 0)
                {
                    folderLabel = value;
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("FolderLabel"));
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("DisplayedFolderLable"));                    
                }
            }
            get
            {
                return folderLabel;
            }
        }        

        public string DisplayedFolderLable 
        {
            get
            {
                if (CountOfUnviewedMessages == 0
                    || CountOfUnviewedMessages < 0)
                {
                    return FolderLabel;
                }
                else
                {
                    return FolderLabel
                        + SpecialWords.SpecialWords.Space
                        + SpecialWords.SpecialWords.LeftRoundBracket
                        + CountOfUnviewedMessages.ToString()
                        + SpecialWords.SpecialWords.RightRoundBracket;
                }
            }
        }

        string folderImage = "Images/folder.png";

        public string FolderImage 
        {
            set
            {
                if (string.Compare(folderImage, value) != 0)
                {
                    folderImage = value;
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("FolderImage"));
                }
            }
            get
            {
                return folderImage;
            }
        }        

        int countOfUnviewedMessages = 0;

        public int CountOfUnviewedMessages 
        {
            set
            {
                if (countOfUnviewedMessages != value)
                {
                    countOfUnviewedMessages = value;
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("DisplayedFolderLable"));
                    CreatePropertyChangedEvent(new PropertyChangedEventArgs("CountOfUnviewedMessages"));
                }
            }
            get
            {
                return countOfUnviewedMessages;
            }

        }

        public virtual List<MessageListItemModel> GetFolderContent()
        {
            return null;
        }

        public virtual void RefreshFolderContent()
        {
            
        }

        #region Property changed event

        public event PropertyChangedEventHandler PropertyChanged;

        void CreatePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        #endregion

        #region updating messages

        protected void UpdateMessages(FolderType folderType, MessageTypes messageTypes, List<Message> messages)
        {
            
            List<Entity> entities = new List<Entity>();
            entities.AddRange(messages);
            byte[] recentVersion = AdditionalTools.GetMaxTimestamp(entities);
            MessagesPack pack = App.proxy.GetMessages(folderType, messageTypes, recentVersion);
            List<Message> messagesBuf = new List<Message>();
            messagesBuf.AddRange(messages);
            if (!UpdateMessagesByPack(pack, messagesBuf))
            {
                TrimMessages(messagesBuf, folderType, messageTypes);
            }
            UploadFromMessagesBuf(messages, messagesBuf);
        }

        bool UpdateMessagesByPack(MessagesPack pack, List<Message> messagesBuf)
        {
            if (pack.Messages.Count > 0)
            {
                Message message;
                foreach (Message item in pack.Messages)
                {
                    message = messagesBuf.FirstOrDefault(row => row.Id == item.Id);
                    if (message == null)
                    {
                        messagesBuf.Add(item);
                    }
                    else
                    {
                        messagesBuf.Remove(message);
                        messagesBuf.Add(item);
                    }
                }
                /// Восстанавливаем нормальный порядок
                messagesBuf = messagesBuf.OrderBy(row => row.Date).ToList();
            }
            return (messagesBuf.Count == pack.CountInDB);
        }

        void TrimMessages(List<Message> messagesBuf, FolderType folderType, MessageTypes messageTypes)
        {
            List<int> idCollection = App.proxy.GetMessagesIds(folderType, messageTypes);
            List<Message> removed = new List<Message>();
            foreach (Message item in messagesBuf)
            {
                if (!idCollection.Contains((int)item.Id))
                {
                    removed.Add(item);
                }
            }
            foreach (Message item in removed)
            {
                messagesBuf.Remove(item);
            }
        }

        void UploadFromMessagesBuf(List<Message> messages, List<Message> messagesBuf)
        {
            messages.Clear();
            messages.AddRange(messagesBuf);
        }

        #endregion

        
    }    
}
