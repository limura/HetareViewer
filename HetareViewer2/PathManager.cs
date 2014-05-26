using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SevenZip;
using System.Drawing;
using System.Collections;
using System.IO;

namespace HetareViewer2
{
    /// <summary>
    /// SetPath() によって設定されたパスから順に画像ファイルを読み出すためのclassです。
    /// 画像ファイルは通常の画像ファイル(.jpeg, .png, .gif等)と、
    /// アーカイブファイル(.zip, .rar等)に入っている画像ファイルのそれぞれが対象となります。
    /// Move(Next|Previous)(Image|Archive)() によって、次(または前)の画像ファイル(やアーカイブ)に移動することができます。
    /// 
    /// なお、このインタフェースにおいては、
    /// 「アーカイブ」という単語はアーカイブファイル(.zip, rar等)だけではなく、
    /// ディレクトリそのものも含んだものを指します。
    /// 
    /// なお、アーカイブファイル(.zip, .rar等)については「アーカイブファイル」と記述します。
    /// 
    /// アーカイブファイルに含まれるファイル名については、
    /// @"C:\some\directory\archive_file.zip|direcotry\file.jpg"
    /// といった形で "|" を使って区切ることとします。
    /// (多分Windows(NTFS?)であれば | はファイル名には使えないはずです)
    /// この不思議拡張されたパス文字列(ExtendedPath)は、GetPath() で取得されます。
    /// また、SetPath() を使って設定することもできます。
    /// 
    /// アーカイブを取得するための GetArchivePath() を使うことで
    /// 現在参照しているアーカイブへのパスを取得することができます。
    /// </summary>
    class ImageArchiveLoader
    {
        List<System.Drawing.Image> m_ImageCache = new List<Image>();

        // 現在開いている場所はディレクトリかアーカイブファイルか、
        enum CurrentPathType
        {
            CURRENT_PATH_TYPE_UNKNOWN,
            CURERNT_PATH_TYPE_DIRECTORY,
            CURRENT_PATH_TYPE_ARCHIVE_FILE,
        }

        enum PathType
        {
            /// <summary>
            /// 不明
            /// </summary>
            PATH_TYPE_UNKNOWN,
            /// <summary>
            /// ディレクトリ
            /// </summary>
            PATH_TYPE_DIRECTORY,
            /// <summary>
            /// その他のファイル
            /// </summary>
            PATH_TYPE_OTHER_FILE,
            /// <summary>
            /// 画像ファイル
            /// </summary>
            PATH_TYPE_IMAGE_FILE,
            /// <summary>
            /// アーカイブ・ファイル
            /// </summary>
            PATH_TYPE_ARCHIVE_FILE,
        }

        bool IsFile(string fileName)
        {
            FileInfo fileInfo = new FileInfo(fileName);
            // ファイルが存在しなければファイルではない
            if (!fileInfo.Exists)
            {
                return false;
            }

            return true;
        }
        bool IsDirectory(string directoryName)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(directoryName);
            // ディレクトリが存在しなければディレクトリではない
            if (!directoryInfo.Exists)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// target_extension が extension_list に含まれているかを確認します。
        /// </summary>
        /// <param name="target_extension">拡張子</param>
        /// <param name="extension_list">期待している拡張子のリスト</param>
        /// <returns></returns>
        bool IsExtensionEqual(string target_extension, string[] extension_list)
        {
            foreach (string extension in extension_list)
            {
                if (target_extension == extension)
                {
                    return true;
                }
            }
            return false;
        }

        PathType GetPathType(string path)
        {
            // とりあえずディレクトリならディレクトリ確定
            if (IsDirectory(path))
                return PathType.PATH_TYPE_DIRECTORY;

            // ディレクトリでもなくファイルでもないならよくわからない何か。
            if (!IsFile(path))
                return PathType.PATH_TYPE_UNKNOWN;

            // 対象のファイルの拡張子を取得しておく
            string target_extension = Path.GetExtension(path);
            // よくわからない拡張子ならUNKNOWN
            if (target_extension == null || target_extension == string.Empty)
                return PathType.PATH_TYPE_UNKNOWN;

            // 小文字にしておきます
            target_extension = target_extension.ToLower();

            // アーカイブ・ファイルであるかどうかを確認。
            if(IsExtensionEqual(target_extension, new string[]{ ".zip", ".rar", ".cbz", ".cbr" }))
            {
                 return PathType.PATH_TYPE_ARCHIVE_FILE;
            }

            // アーカイブ・ファイルであるかどうかを確認。
            if(IsExtensionEqual(target_extension, new string[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp", ".png" }))
            {
                return PathType.PATH_TYPE_IMAGE_FILE;
            }

            return PathType.PATH_TYPE_OTHER_FILE;
        }

        string[] GetDirectoryFilePathList(string directoryPath)
        {
            string[] path_list = Directory.GetFileSystemEntries(currentDirectory);
            Array.Sort(path_list);
            for (int i = 0; i < path_list.Length; ++i )
            {
                //path_list[i] = directoryPath + Path.PathSeparator + path_list[i];
            }
            return path_list;
        }

        string[] GetArchiveFilePathList(string archiveFilePath)
        {
            currentArchiveFileExtractor = new SevenZip.SevenZipExtractor(archiveFilePath);
            return currentArchiveFileExtractor.ArchiveFileNames.ToArray();
        }

        string GetDirectoryOrArchive(string extend_path)
        {
            int index = extend_path.LastIndexOfAny(new char[] { '|', '\\' });
            if (index < 0)
            {
                // @todo みつからなかったけれど、そのものを返す
                return extend_path;
            }
            return extend_path.Substring(0, index);
        }

        string currentDirectory = "";
        PathType currentDirectoryType = PathType.PATH_TYPE_UNKNOWN;
        int currentFileIndex = -1;
        string[] currentDirectoryFilePathList = new string[] {};
        SevenZip.SevenZipExtractor currentArchiveFileExtractor = null;

        public bool SetPath(string path)
        {
            PathType type = GetPathType(path);
            switch (type)
            {
                case PathType.PATH_TYPE_IMAGE_FILE:
                    currentDirectory = Path.GetDirectoryName(path);
                    currentDirectoryType = PathType.PATH_TYPE_DIRECTORY;
                    currentDirectoryFilePathList = GetDirectoryFilePathList(currentDirectory);
                    currentFileIndex = Array.IndexOf(currentDirectoryFilePathList, path);
                    break;
                case PathType.PATH_TYPE_DIRECTORY:
                    currentDirectory = path;
                    currentDirectoryType = PathType.PATH_TYPE_DIRECTORY;
                    currentDirectoryFilePathList = GetDirectoryFilePathList(path);
                    currentFileIndex = 0;
                    //SearchNextPath();
                    break;
                case PathType.PATH_TYPE_OTHER_FILE:
                    currentDirectory = Path.GetDirectoryName(path);
                    currentDirectoryType = PathType.PATH_TYPE_DIRECTORY;
                    currentDirectoryFilePathList = GetDirectoryFilePathList(path);
                    currentFileIndex = Array.IndexOf(currentDirectoryFilePathList, path);
                    //SearchNextPath();
                    break;
                case PathType.PATH_TYPE_ARCHIVE_FILE:
                    currentDirectory = path;
                    currentDirectoryType = PathType.PATH_TYPE_ARCHIVE_FILE;
                    currentDirectoryFilePathList = GetArchiveFilePathList(path);
                    Array.Sort(currentDirectoryFilePathList);
                    currentFileIndex = -1;
                    //SearchNextPath();
                    break;
                case PathType.PATH_TYPE_UNKNOWN:
                    return false;
            }
            return true;
        }

        /// <summary>
        /// 現在参照しているパス文字列を返します。
        /// ここで返されるパス文字列はアーカイブファイルの中のファイルまでを含んだ
        /// 「不思議拡張されたパス文字列」ですので注意してください。
        /// </summary>
        /// <returns>不思議拡張されたパス文字列</returns>
        public string GetPath()
        {
            return null;
        }

        /// <summary>
        /// 現在参照している画像ファイルを読み出します。
        /// </summary>
        /// <returns>取り出されたイメージ。失敗した場合は null を返します。</returns>
        public System.Drawing.Image GetImage()
        {
            if (m_ImageCache.Count <= 0)
                return null;

            System.Drawing.Image image = m_ImageCache.First();
            m_ImageCache.RemoveAt(0);
            return image;
        }

        /// <summary>
        /// 現在参照しているファイルのディレクトリへか、アーカイブファイルへのパスを取得します。
        /// </summary>
        /// <returns>ディレクトリかファイルへのパス文字列</returns>
        public string GetArchivePath()
        {
            return "";
        }

        /// <summary>
        /// 現在参照しているファイルの次の画像ファイルを参照するように内部状態を更新します。
        /// これ以上のファイルが存在しない場合など、内部状態の更新に失敗した場合は false を返します。
        /// </summary>
        /// <returns></returns>
        public bool MoveNextImage()
        {
            return false;
        }

        /// <summary>
        /// 現在参照しているファイルの前の画像ファイルを参照するように内部状態を更新します。
        /// これ以上のファイルが存在しない場合など、内部状態の更新に失敗した場合は false を返します。
        /// </summary>
        /// <returns></returns>
        public bool MovePreviousImage()
        {
            return false;
        }

        /// <summary>
        /// 現在参照しているディレクトリまたはアーカイブファイルの次のディレクトリまたはアーカイブファイルを参照するように内部状態を更新します。
        /// これ以上のファイルが存在しない場合など、内部状態の更新に失敗した場合は false を返します。
        /// </summary>
        /// <returns></returns>
        public bool MoveNextArchive()
        {
            return false;
        }

        /// <summary>
        /// 現在参照しているディレクトリまたはアーカイブファイルの前のディレクトリまたはアーカイブファイルを参照するように内部状態を更新します。
        /// これ以上のファイルが存在しない場合など、内部状態の更新に失敗した場合は false を返します。
        /// </summary>
        /// <returns></returns>
        public bool MovePreviousArchive()
        {
            return false;
        }

#if false

        private bool UpdateCache()
        {
            
            return true;
        }

        /// <summary>
        /// 現在のパス
        /// </summary>
        string currentPath = @"C:\";

        /// <summary>
        /// 現在のパスを設定します。失敗した場合は指定されたパスは無視され、元の値が使用されます。
        /// </summary>
        /// <param name="path">設定されるパス</param>
        /// <returns>成否</returns>
        public bool SetCurrentPath(string path)
        {
            currentPath = path;
            return false;
        }
        public string GetCurrentPath()
        {
            return currentPath;
        }

        bool SearchNextPath_ArchiveType()
        {
            // 負の値の場合はディレクトリを開いただけの状態のはずなので、次の値が 0 になるように -1 にしておく
            if (currentFileIndex < 0)
            {
                currentFileIndex = -1;
            }

            // 次のファイルの探索を開始。

            // 確認しているファイル・フォルダのリストがある間は探索します。
            while (++currentFileIndex < currentDirectoryFilePathList.Length)
            {
                string targetPath = currentDirectoryFilePathList[currentFileIndex];


            }

            return false;
        }
        bool SearchNextPath_DirectoryType()
        {
            // 負の値の場合はディレクトリを開いただけの状態のはずなので、次の値が 0 になるように -1 にしておく
            if (currentFileIndex < 0)
            {
                currentFileIndex = -1;
            }

            // 次のファイルの探索を開始。

            // 確認しているファイル・フォルダのリストがある間は探索します。
            while (++currentFileIndex < currentDirectoryFilePathList.Length)
            {
                string targetPath = currentDirectoryFilePathList[currentFileIndex];
                switch (GetPathType(targetPath))
                {
                        // アーカイブならそのように設定して次を探索
                    case PathType.PATH_TYPE_ARCHIVE_FILE:
                        currentDirectory = targetPath;
                        currentDirectoryType = PathType.PATH_TYPE_ARCHIVE_FILE;
                        currentDirectoryFilePathList = GetArchiveFilePathList(targetPath);
                        Array.Sort(currentDirectoryFilePathList);
                        currentFileIndex = -1;
                        return SearchNextPath_ArchiveType();
                        // 画像ファイルならそこで探索は終了
                    case PathType.PATH_TYPE_IMAGE_FILE:
                        return true;
                        // ディレクトリならそのように設定して次を探索
                    case PathType.PATH_TYPE_DIRECTORY:
                        currentDirectory = targetPath;
                        currentDirectoryType = PathType.PATH_TYPE_DIRECTORY;
                        currentDirectoryFilePathList = GetDirectoryFilePathList(targetPath);
                        currentFileIndex = -1;
                        return SearchNextPath_DirectoryType();
                        // それ以外なら単にインデックスを一つ増やして次を探索
                    case PathType.PATH_TYPE_OTHER_FILE:
                    default:
                        break;
                }
            }

            // インデックスがフォルダ内部のファイル・フォルダ数以上の値になっているということは、
            // ファイルの探索は終わったので一つ上のディレクトリに戻って探索する必要があります。
            if (currentFileIndex >= currentDirectoryFilePathList.Length)
            {
                // 今のディレクトリ名を覚えておいて、
                string directoryName = currentDirectory;
                // 一つ上のディレクトリに移動
                currentDirectory = Path.GetDirectoryName(currentDirectory);
                // ディレクトリ内のファイル・フォルダのリストを取得しなおす
                currentDirectoryFilePathList = GetDirectoryFilePathList(currentDirectory);
                // その中から、今まで見ていたディレクトリ名のもののインデックスを発見して、
                currentFileIndex = Array.IndexOf(currentDirectoryFilePathList, directoryName);
                currentDirectoryType = PathType.PATH_TYPE_DIRECTORY;
                // 自分を呼び出しなおして終了。
                return SearchNextPath_DirectoryType();
            }

            return false;
        }

        /// <summary>
        /// 現在のパスから次の画像ファイルへのパスを検索しておきます
        /// </summary>
        /// <returns></returns>
        public bool SearchNextPath()
        {
            switch (currentDirectoryType)
            {
                case PathType.PATH_TYPE_ARCHIVE_FILE:
                    if (SearchNextPath_ArchiveType() == true)
                    {
                        return true;
                    }
                    // アーカイブの中は探索し尽くしたので、アーカイブから抜けます。
                    string 
                    return SearchNextPath_DirectoryType();
                case PathType.PATH_TYPE_DIRECTORY:
                case PathType.PATH_TYPE_IMAGE_FILE:
                case PathType.PATH_TYPE_OTHER_FILE:
                    return SearchNextPath_DirectoryType();
                default:
                    return false;
            }
            return false;
        }
#endif
    }
}
