using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;
using System.ComponentModel;

namespace WPFClient.SidebarFolders
{
    class SidebarFolder : INotifyPropertyChanged
    {
        public SidebarFolder()
        {
            FolderLabel = "New folder";       
            FolderImage = "Images/folder.png";
        }

        string folderLabel;

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
                        + SpecialSymbols.SpecialSymbols.space
                        + SpecialSymbols.SpecialSymbols.leftCountOfStopper
                        + CountOfUnviewedMessages.ToString()
                        + SpecialSymbols.SpecialSymbols.rightCountOfStopper;
                }
            }
        }

        string folderImage = string.Empty;

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

        public void CreatePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        protected bool IsViewedMessage(Message message)
        {
            if (message != null
                && message.EDRecipient_MessageId != null)
            {
                Recipient result = message.EDRecipient_MessageId.FirstOrDefault(row => string.Compare(row.RecipientUsername, App.Username) == 0);
                if (result != null)
                    return result.Viewed;
                return false;         
            }
            return false;
        }
    }
}
