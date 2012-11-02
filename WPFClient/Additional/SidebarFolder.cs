using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFClient
{
    class SidebarFolder
    {
        public SidebarFolder() : this("NewFolder") { }

        public SidebarFolder(string folderLabel)
        {
            FolderLabel = folderLabel;
        }

        public string FolderLabel { set; get; }
    }
}
