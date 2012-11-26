using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Entities;
using WPFClient.Additional;

namespace WPFClient.SidebarFolders
{
    class SidebarFolder
    {
        public SidebarFolder()
        {
            FolderLabel = "New folder";
            FolderImage = "Images/folder.png";
        }

        public string FolderLabel { set; get; }

        public string FolderImage { get; set; }

        public virtual List<Message> GetFolderContent()
        {
            return null;
        }
    }
}
