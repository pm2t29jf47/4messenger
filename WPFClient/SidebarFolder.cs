using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFClient
{
    class SidebarFolder
    {
        public SidebarFolder() : this("NewFolder"){ }

        public SidebarFolder(string folderName)
            {
                if (folderName == null) return;
                FolderName = folderName;
            }

        public string FolderName { set; get; }
    }
}
