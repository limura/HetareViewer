using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;
using System.IO;

namespace HetareViewer2
{
    class FileBinder_7ZFile : FileBinder
    {
        string m_Path = null;
        SevenZip.SevenZipExtractor m_Extractor = null;

        public FileBinder_7ZFile(string path)
        {
            m_Path = path;
        }

        public bool IsBinder()
        {
            return true;
        }

        public List<FileBinder> DumpBinder()
        {
            if (m_Extractor == null)
            {
                m_Extractor = new SevenZip.SevenZipExtractor(m_Path);
            }

            List<FileBinder> result_list = new List<FileBinder>();
            List<ArchiveFileInfo> archive_file_info_list = null;
            try
            {
                archive_file_info_list = m_Extractor.ArchiveFileData.OrderBy(info => info.FileName).ToList();
            }
            catch (SevenZip.SevenZipException e)
            {
                // エラーしたらとりあえず空リストを返しておきます。
                return new List<FileBinder>();
            }
            archive_file_info_list.ForEach(fileInfo =>
            {
                string inner_path = m_Path + @"\" + fileInfo.FileName;
                FileBinder_7ZInnerFile inner_file = new FileBinder_7ZInnerFile(this, inner_path, fileInfo);
                result_list.Add(inner_file);
            });

            return result_list;
        }

        public System.IO.Stream Open()
        {
            return null;
        }

        public System.IO.Stream Open7Zfile(int index)
        {
            if (m_Extractor == null)
            {
                m_Extractor = new SevenZip.SevenZipExtractor(m_Path);
            }
            var mem_stream = new System.IO.MemoryStream();
            m_Extractor.ExtractFile(index, mem_stream);
            return mem_stream;
        }

        public string GetPath()
        {
            return m_Path;
        }

        public static bool IsSupportedFile(string path)
        {
            string lower_path = path.ToLower();
            int rindex = lower_path.LastIndexOf('.');
            if (rindex < 0 || lower_path.Length <= (rindex + 1))
            {
                return false;
            }
            string suffix = lower_path.Substring(rindex + 1);
            switch (suffix)
            {
                case "zip":
                case "rar":
                case "cbz":
                case "cbr":
                case "7z":
                    return true;
                default:
                    break;
            }
            return false;
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
