using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using SevenZip;

namespace HetareViewer2
{
    class FileBinder_Directory : FileBinder
    {
        string m_Path = null;
        
        public FileBinder_Directory(string path)
        {
            m_Path = path;
        }

        public bool IsBinder()
        {
            return true;
        }

        bool IsUnSupportedFileOrDirectory(string path)
        {
            string file_name = System.IO.Path.GetFileName(path);
            switch (file_name)
            {
                case ".AppleDouble":
                    return true;
            }
            return false;
        }

        public List<FileBinder> DumpBinder()
        {
            if (m_Path == null)
            {
                return null;
            }

            string[] file_directory_list = Directory.GetFileSystemEntries(m_Path);
            Array.Sort(file_directory_list);

            List<FileBinder> result_list = new List<FileBinder>();
            foreach (string file_directory in file_directory_list)
            {
                string target_file = file_directory;

                // 一応必要無いファイルはこの時点で弾いてしまいます。
                if (IsUnSupportedFileOrDirectory(file_directory))
                {
                    continue;
                }

                if (Directory.Exists(file_directory))
                {
                    // ディレクトリだった
                    FileBinder_Directory new_binder = new FileBinder_Directory(target_file);
                    result_list.Add(new_binder);
                    continue;
                }
                if (!File.Exists(target_file))
                {
                    // ファイルじゃないっぽいので無視する
                    continue;
                }
                if (FileBinder_7ZFile.IsSupportedFile(target_file))
                {
                    // 7Zip で解凍できそうな何かだった
                    FileBinder_7ZFile archiver_file = new FileBinder_7ZFile(target_file);
                    result_list.Add(archiver_file);
                    continue;
                }
                // ファイルっぽいのでファイルとして追加する
                FileBinder_File new_file = new FileBinder_File(target_file);
                result_list.Add(new_file);
            }
            return result_list;
        }

        public System.IO.Stream Open()
        {
            return null;
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
