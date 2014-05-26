using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Compression;
//using System.IO.Compression.FileSystem;
using SevenZip;

namespace HetareViewer2
{
    public partial class Form1 : Form
    {
        ImageLoadUtil m_ImageLoadUtil = new ImageLoadUtil();
        RotateFlipType m_RotateType = RotateFlipType.RotateNoneFlipNone;

        enum PreloadType
        {
            Next,
            Prev,
        };
        PreloadType m_PreloadType = PreloadType.Next;
        Image m_NextImage = null;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "対応ファイル|*.zip;*.rar;*.cbz;*.cbr;*.7z;*.jpg;*.jpeg;*.png;*.gif;*.bmp;*.tiff|アーカイブファイル|*.zip;*.rar;*.cbz;*.cbr;*.7z|画像ファイル|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            dialog.Title = "開くファイルを選んでください";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                m_ImageLoadUtil.AssignFile(dialog.FileName);
                AssignImage(PreloadType.Next);
            }
        }

        private void AssignImage(PreloadType preload_type)
        {
            if (m_NextImage != null && preload_type == m_PreloadType)
            {
                imagePictureBox.Image = m_NextImage;
            }
            else
            {
                System.Drawing.Image img = System.Drawing.Image.FromStream(m_ImageLoadUtil.Open());
                img.RotateFlip(m_RotateType);
                imagePictureBox.Image = img;
            }
            this.Text = string.Format("HetareViewer2 {0}", m_ImageLoadUtil.GetFileSummary());

            m_PreloadType = preload_type;
            switch (preload_type)
            {
                case PreloadType.Next:
                    break;
                case PreloadType.Prev:
                    break;
            }
        }

        void NextFile(int count = 1)
        {
            m_ImageLoadUtil.NextFile(count);
            AssignImage(PreloadType.Next);
        }
        void PrevFile(int count = 1)
        {
            m_ImageLoadUtil.PrevFile(count);
            AssignImage(PreloadType.Prev);
        }
        void NextBinder()
        {
            m_ImageLoadUtil.NextBinder();
            AssignImage(PreloadType.Next);
        }
        void PrevBinder()
        {
            m_ImageLoadUtil.PrevBinder();
            AssignImage(PreloadType.Prev);
        }


        private void NextButton_Click(object sender, EventArgs e)
        {
            NextFile();
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            PrevFile();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (m_ImageLoadUtil.IsFileAssigned() == false)
            {
                return;
            }

            if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Right)
            {
                if (e.Control)
                {
                    PrevBinder();
                    return;
                }
                int prev_count = 1;
                if (e.Shift)
                {
                    prev_count = 5;
                }
                PrevFile(prev_count);
            }
            else if (e.KeyCode == Keys.Down || e.KeyCode == Keys.Left)
            {
                if (e.Control)
                {
                    NextBinder();
                    return;
                }
                int next_count = 1;
                if (e.Shift)
                {
                    next_count = 5;
                }
                NextFile(next_count);
            }
        }

        private void rotateSelectComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (rotateSelectComboBox.Text)
            {
                case "標準":
                    m_RotateType = RotateFlipType.RotateNoneFlipNone;
                    AssignImage(m_PreloadType);
                    break;
                case "時計回り90度":
                    m_RotateType = RotateFlipType.Rotate90FlipNone;
                    AssignImage(m_PreloadType);
                    break;
                case "反時計回り90度":
                    m_RotateType = RotateFlipType.Rotate270FlipNone;
                    AssignImage(m_PreloadType);
                    break;
                case "見開き2ページ":
                    m_RotateType = RotateFlipType.RotateNoneFlipNone;
                    AssignImage(m_PreloadType);
                    break;
                default:
                    break;
            }
        }

        private void nextBinderButton_Click(object sender, EventArgs e)
        {
            NextBinder();
        }

        private void prevBinderButton_Click(object sender, EventArgs e)
        {
            PrevBinder();
        }
    }
}
