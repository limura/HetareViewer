using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HetareViewer2
{

    /*
     * アーカイブ・ファイルをディレクトリと同じに扱うこれを(仮に) Binder と呼ぶ
     * FileBinder は File と Binder のどちらかである
     *  一つの Binder は FileBinder の配列として取得される。
     */
    interface FileBinder
    {
        /// <summary>
        /// このオブジェクトがBinder(内部にファイルを持っているか否か)かどうかを取得します。
        /// enum を返そうかと思ったけれど、interface内部 では enum が定義できないようだ……
        /// </summary>
        /// <returns>Binderである場合は true、その他なら false</returns>
        bool IsBinder();

        /// <summary>
        /// Type が Binder の場合、Binder内部に含まれる FileBinder のリストを取得します。
        /// 失敗したか Binder ではなかった場合、null が返ります
        /// </summary>
        /// <returns>展開された FileBinder の中身</returns>
        List<FileBinder> DumpBinder();

        /// <summary>
        /// Type が File の場合はファイルを開いて読み出し用の stream を返します。
        /// 呼び出し元は使用後には close() してください。
        /// Type が Binder の場合や失敗した時には null が返ります
        /// </summary>
        /// <returns></returns>
        System.IO.Stream Open();

        /// <summary>
        /// 設定されたパスを取得します
        /// </summary>
        /// <returns></returns>
        string GetPath();

        /// <summary>
        /// 自分の親ディレクトリに当たるFileBinderを取得します。
        /// 必要なら生成して返します。
        /// </summary>
        /// <returns></returns>
        FileBinder GetParentBinder();
    }
}
