using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;

namespace HetareViewer2
{
    class FileBinder_7ZInnerFile : FileBinder
    {
        FileBinder_7ZFile m_Parent = null;
        string m_Path = null;
        SevenZip.ArchiveFileInfo m_FileInfo;
        public FileBinder_7ZInnerFile(FileBinder_7ZFile parent, string path, SevenZip.ArchiveFileInfo file_info)
        {
            m_Parent = parent;
            m_Path = path;
            m_FileInfo = file_info;
        }

        public bool IsBinder()
        {
            return false;
        }

        public List<FileBinder> DumpBinder()
        {
            return null;
        }

        public System.IO.Stream Open()
        {
            if (m_Parent == null)
            {
                return null;
            }

            return m_Parent.Open7Zfile(m_FileInfo.Index);
        }

        public string GetPath()
        {
            return m_Path;
        }

        public FileBinder GetParentBinder()
        {
            return m_Parent;
        }
    }
}
