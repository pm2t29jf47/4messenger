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
    }
}
