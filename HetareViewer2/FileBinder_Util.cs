using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace HetareViewer2
{
    class FileBinder_Util
    {
        string m_Path = null;
        FileBinder m_CurrentBinder = null;
        List<FileBinder> m_BinderList = null;
        int m_CurrentIndex = 0;

        public bool IsFileAssigned()
        {
            if (m_CurrentBinder == null)
            {
                return false;
            }
            return true;
        }

        public bool AssignFile(string path)
        {
            m_Path = path;

            // 指定されたパスがディレクトリであればそこの最初のファイルをターゲットにします
            if (Directory.Exists(path))
            {
                m_CurrentBinder = new FileBinder_Directory(path);
                m_BinderList = m_CurrentBinder.DumpBinder();
                // 一応最初のファイルを探します
                m_CurrentIndex = -1; // index を -1 にすることで最初のファイルを見に行きます。
                return NextFile();
            }
            // 指定されたパスがファイルであればそれを最初のターゲットにします
            if (File.Exists(path))
            {
                m_CurrentBinder = new FileBinder_Directory(Path.GetDirectoryName(path));
                m_BinderList = m_CurrentBinder.DumpBinder();
                int index = 0;
                foreach (FileBinder fb in m_BinderList)
                {
                    if (fb.GetPath() == path)
                    {
                        if (fb.IsBinder())
                        {
                            // 指定されたファイルがBinderだったということは、その中のファイルを辿らねばなりません。
                            m_CurrentBinder = fb;
                            m_BinderList = fb.DumpBinder();
                            m_CurrentIndex = -1;
                            return NextFile();
                        }
                        m_CurrentIndex = index;
                        break;
                    }
                    index++;
                }
                return true;
            }
            return false;
        }

        /// <summary>
        /// Binder であった時の次のファイルを設定します
        /// </summary>
        /// <returns></returns>
        bool NextFile_Child(FileBinder binder)
        {
            if (!binder.IsBinder())
            {
                return false;
            }
            List<FileBinder> binder_list = binder.DumpBinder();

            if (binder_list.Count <= 0)
            {
                // ディレクトリの中身がなかった
                return false;
            }
            if (binder_list[0].IsBinder())
            {
                // ディレクトリが先頭にあるので潜ります。
                return NextFile_Child(binder_list[0]);
            }
            // ファイルが先頭なのでここで探索は修了です。
            m_CurrentBinder = binder;
            m_BinderList = binder_list;
            m_CurrentIndex = 0;
            return true;
        }

        /// <summary>
        /// file_list の中にある target_path のファイルへのインデックスを取得します。
        /// 存在しない場合には -1 を返します。
        /// </summary>
        /// <param name="file_list">.GetPath() のできる FileBinder のリスト</param>
        /// <param name="target_path">.GetPath() と比較されるパス文字列</param>
        /// <returns>発見されたインデックス。存在しない場合やエラーした場合は -1</returns>
        int GetIndexOf(List<FileBinder> file_list, string target_path)
        {
            int index = 0;
            foreach (FileBinder fb in file_list)
            {
                if (fb.GetPath() == target_path)
                {
                    return index;
                }
                index++;
            }
            return -1;
        }

        bool NextFile_Parent(FileBinder parent_binder, string current_path)
        {
            if (parent_binder == null)
            {
                return false;
            }

            List<FileBinder> binder_list = parent_binder.DumpBinder();
            int index = GetIndexOf(binder_list, current_path);
            if (index < 0)
            {
                // 親ディレクトリに自分のファイルへのパスが無い……
                return false;
            }

            // とりあえず現在状態を更新して NextFile() を呼び出す。
            m_CurrentIndex = index;
            m_CurrentBinder = parent_binder;
            m_BinderList = binder_list;

            return NextFile();
        }

        public bool NextFile(int count = 1)
        {
            if ((m_CurrentIndex + count) >= m_BinderList.Count)
            {
                // 現在の BinderList のサイズを超えたということで、一つ上のディレクトリで状態を更新しようとしてみます。
                return NextFile_Parent(m_CurrentBinder.GetParentBinder(), m_CurrentBinder.GetPath());
            }

            m_CurrentIndex += count;
            // index が限界を超えていないのならファイルかディレクトリかアーカイブです。
            FileBinder target_fb = m_BinderList[m_CurrentIndex];
            if (target_fb.IsBinder())
            {
                // Binder なのでその中を再帰的に手繰ります。
                if (!NextFile_Child(target_fb))
                {
                    // 失敗したということは、中身にファイルが無かった。
                    // ので、また次を探索。
                    return NextFile();
                }
                return true;
            }

            // Binder でもないということはファイルです。
            return true;
        }

        /// <summary>
        /// 一つ前のファイルをたどる時に、ディレクトリを登る場合
        /// </summary>
        /// <returns></returns>
        bool PrevFile_ParentDirectory(FileBinder binder)
        {
            string current_path = binder.GetPath();
            m_CurrentBinder = new FileBinder_Directory(Path.GetDirectoryName(current_path));
            m_BinderList = m_CurrentBinder.DumpBinder();
            int index = 0;
            m_CurrentIndex = 0;
            bool hit = false;
            foreach (FileBinder fb in m_BinderList)
            {
                if (fb.GetPath() == current_path)
                {
                    hit = true;
                    break;
                }
                index++;
            }
            if (!hit)
            {
                Console.WriteLine("Fatal error. ファイルリストに前のフォルダが見当たりません。{0}", current_path);
                return false;
            }
            if (index <= 0)
            {
                // 元を辿ったけれどそれも先頭だった。
                return PrevFile_ParentDirectory(m_CurrentBinder);
            }
            // 一つ前が目標です。
            index -= 1;
            m_CurrentIndex = index;
            FileBinder target_fb = m_BinderList[m_CurrentIndex];
            if (target_fb.IsBinder())
            {
                if (!PrevFile_ChlidDirectory(target_fb))
                {
                    // 失敗したということは、ディレクトリの中身にファイルが無かった。
                    // ので、もう一つ前のもので探索を再開。
                    return PrevFile();
                }
                return true;
            }
            // ファイルであったので、ここで探索は終了です。
            return true;
        }

        bool PrevFile_ChlidDirectory(FileBinder binder)
        {
            List<FileBinder> binder_list = binder.DumpBinder();
            // Indexは最後を指します
            int index = binder_list.Count - 1;
            if (index < 0)
            {
                // そもそもBinderListの中身が無い。
                return false;
            }

            for( ; index >= 0; --index )
            {
                FileBinder target_fb = binder_list[index];
                if (!target_fb.IsBinder())
                {
                    // ファイルならそこで探索は終了です。
                    m_CurrentBinder = binder;
                    m_BinderList = binder_list;
                    m_CurrentIndex = index;
                    return true;
                }
                if (PrevFile_ChlidDirectory(target_fb))
                {
                    // 子を探索して発見できたのでこれでも終了です。
                    return true;
                }
            }
            // このBinderの中にはファイルが無かった。
            return false;
        }
        public bool PrevFile(int count = 1)
        {
            m_CurrentIndex -= count;
            if (m_CurrentIndex < 0)
            {
                // 負の値になったということは、ディレクトリの先頭まで戻りました。
                return PrevFile_ParentDirectory(m_CurrentBinder);
            }

            FileBinder target_fb = m_BinderList[m_CurrentIndex];
            if (target_fb.IsBinder())
            {
                return PrevFile_ChlidDirectory(target_fb);
            }
            return true;
        }

        FileBinder GetCurrentFileBinder()
        {
            if (m_CurrentIndex < 0 || m_CurrentIndex >= m_BinderList.Count)
            {
                Console.WriteLine("m_CurrentIndex が {0} であり、負か Count({1}) を超えています。", m_CurrentIndex, m_BinderList.Count);
                return null;
            }
            return m_BinderList[m_CurrentIndex];
        }

        public Stream Open()
        {
            FileBinder fb = GetCurrentFileBinder();
            if (fb == null)
            {
                Console.WriteLine("現在の Binder が取得できません。");
                return null;
            }
            if (fb.IsBinder())
            {
                Console.WriteLine("Binder に対してOpenしようとしてしまっています。");
                return null;
            }

            return fb.Open();
        }

        public string GetPath()
        {
            FileBinder fb = GetCurrentFileBinder();
            if (fb == null)
            {
                Console.WriteLine("現在の Binder が取得できません。");
                return null;
            }
            return fb.GetPath();
        }

        public bool IsBinder()
        {
            FileBinder fb = GetCurrentFileBinder();
            if (fb == null)
            {
                Console.WriteLine("現在の Binder が取得できません。");
                return false;
            }
            return fb.IsBinder();
        }

        /// <summary>
        /// 次のBinderに移動させます。
        /// </summary>
        /// <returns></returns>
        public bool NextBinder()
        {
            if (m_BinderList == null)
            {
                return false;
            }
            if (m_BinderList.Count <= 0)
            {
                return false;
            }
            m_CurrentIndex = m_BinderList.Count - 1;
            return NextFile();
        }

        /// <summary>
        /// 前のBinderに移動させます。
        /// </summary>
        /// <returns></returns>
        public bool PrevBinder()
        {
            m_CurrentIndex = 0;
            return PrevFile();
        }

        /// <summary>
        /// 現在参照しているファイルのファイル名と、そのファイルが Binder の中の何番目か、といった情報を付加した文字列を取得します
        /// </summary>
        /// <returns></returns>
        public string GetFileSummary()
        {
            FileBinder fb = GetCurrentFileBinder();
            if (fb == null)
            {
                Console.WriteLine("現在の Binder が取得できません。");
                return "unknown file";
            }
            if (fb.IsBinder())
            {
                Console.WriteLine("Binder 現在のファイルが Binder でした");
                return "unknown binder";
            }
            fb = fb.GetParentBinder();
            if (fb == null || fb.IsBinder() == false)
            {
                Console.WriteLine("上位の Binder がありません。");
                return "unknown parent binder";
            }

            string path = fb.GetPath();
            string[] p_list = path.Split('\\');
            if (p_list == null || p_list.Length <= 0)
            {
                return "unknown binder path name";
            }
            string file_name = p_list[p_list.Length - 1];
            return string.Format("{0} {1}/{2}", file_name, m_CurrentIndex + 1, m_BinderList.Count);
        }
    }
}
