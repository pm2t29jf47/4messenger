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
            DisplayedFolderLable = FolderLabel;
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
                }
            }
            get
            {
                return folderLabel;
            }
        }

        public string displayedFolderLable;

        public string DisplayedFolderLable { get; set; }

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

        public virtual List<Message> GetFolderContent()
        {
            return null;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void CreatePropertyChangedEvent(PropertyChangedEventArgs e)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, e);
        }

        public int CountOfUnviewedMessages 
        {
            set
            {
                if(value == 0 
                    || value < 0) 
                    return;
                FolderLabel = FolderLabel
                    + SpecialSymbols.SpecialSymbols.space
                    + SpecialSymbols.SpecialSymbols.leftCountOfStopper
                    + value.ToString()
                    + SpecialSymbols.SpecialSymbols.rightCountOfStopper;
            }

        }
    }
}
