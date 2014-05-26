using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HetareViewer2
{
    class FileBinder_File : FileBinder
    {
        string m_Path = null;
        public FileBinder_File(string path)
        {
            m_Path = path;
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
            if (m_Path == null)
            {
                return null;
            }

            System.IO.StreamReader reader = new StreamReader(m_Path);
            return reader.BaseStream;
        }

        public string GetPath()
        {
            return m_Path;
        }

        public FileBinder GetParentBinder()
        {
            if (m_Path == null)
            {
                return null;
            }
            return new FileBinder_Directory(Path.GetDirectoryName(m_Path));
        }
    }
}
