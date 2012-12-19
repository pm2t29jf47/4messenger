using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;
using System.ComponentModel;
using ServiceInterface;
using Entities.Additional;

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

        public virtual List<MessageListItemModel> GetFolderContent()
        {
            return null;
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

        public event PropertyChangedEventHandler PropertyChanged;

        void CreatePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        protected void UpdateMessages(FolderType folderType, MessageTypes messageTypes, List<Message> messages)
        {
            List<Entity> entities = new List<Entity>();
            entities.AddRange(messages);
            byte[] recentVersion = AdditionalTools.GetMaxTimestamp(entities);
            MessagesPack pack = App.proxy.GetMessages(folderType, messageTypes, recentVersion);
            if (!UpdateMessagesByPack(pack, messages))
            {
                List<int> idCollection = App.proxy.GetMessagesIds(folderType, messageTypes);
                TrimMessages(idCollection, messages);
            }
        }

        bool UpdateMessagesByPack(MessagesPack pack, List<Message> messages)
        {
            if (pack.Messages.Count > 0)
            {
                Message message;
                foreach (Message item in pack.Messages)
                {
                    message = messages.FirstOrDefault(row => row.Id == item.Id);
                    if (message == null)
                    {
                        messages.Add(item);
                    }
                    else
                    {
                        messages.Remove(message);
                        messages.Add(item);
                    }
                }
                /// Восстанавливаем нормальный порядок
                messages = messages.OrderBy(row => row.Date).ToList();
            }
            return (messages.Count == pack.CountInDB);
        }

        void TrimMessages(List<int> ids, List<Message> messages)
        {
            List<Message> removed = new List<Message>();
            foreach (Message item in messages)
            {
                if (!ids.Contains((int)item.Id))
                {
                    removed.Add(item);
                }
            }
            foreach (Message item in removed)
            {
                messages.Remove(item);
            }
        }
    }    
}
