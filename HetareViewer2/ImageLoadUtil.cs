using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetareViewer2
{
    class ImageLoadUtil
    {
        FileBinder_Util m_Util = new FileBinder_Util();

        bool IsImageFile(string path)
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
                case "jpeg":
                case "jpg":
                case "png":
                case "bmp":
                case "gif":
                case "tiff":
                    return true;
                default:
                    break;
            }
            return false;
        }

        public bool AssignFile(string path)
        {
            if( ! m_Util.AssignFile(path) )
            {
                return false;
            }
            // 画像ファイルに当たるまで次のファイルを探索します。
            while(!IsImageFile(m_Util.GetPath()))
            {
                if( !m_Util.NextFile() )
                {
                    Console.WriteLine("NextFile() に失敗しました。");
                    return false;
                }
            }
            return true;
        }

        public bool IsFileAssigned()
        {
            return m_Util.IsFileAssigned();
        }

        public bool NextFile(int count = 1)
        {
            // 画像ファイルに当たるまで次のファイルを探索します。
            if(!m_Util.NextFile(count))
            {
                return false;
            }
            while(!IsImageFile(m_Util.GetPath()))
            {
                if( !m_Util.NextFile() )
                {
                    Console.WriteLine("NextFile() に失敗しました。");
                    return false;
                }
            }
            return true;
        }

        public bool PrevFile(int count = 1)
        {
            // 画像ファイルに当たるまで次のファイルを探索します。
            if(!m_Util.PrevFile(count))
            {
                return false;
            }
            while (!IsImageFile(m_Util.GetPath()))
            {
                if (!m_Util.PrevFile())
                {
                    Console.WriteLine("PrevFile() に失敗しました。");
                    return false;
                }
            }
            return true;
        }

        public bool NextBinder()
        {
            if (!m_Util.NextBinder())
            {
                return false;
            }
            while (!IsImageFile(m_Util.GetPath()))
            {
                if (!m_Util.NextFile())
                {
                    Console.WriteLine("NextFile() に失敗しました。");
                    return false;
                }
            }
            return true;
        }
        public bool PrevBinder()
        {
            if (!m_Util.PrevBinder())
            {
                return false;
            }
            while (!IsImageFile(m_Util.GetPath()))
            {
                if (!m_Util.PrevFile())
                {
                    Console.WriteLine("PrevFile() に失敗しました。");
                    return false;
                }
            }
            return true;
        }

        public System.IO.Stream Open()
        {
            return m_Util.Open();
        }

        public string GetFileSummary()
        {
            return m_Util.GetFileSummary();
        }
    }
}
