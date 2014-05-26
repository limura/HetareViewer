namespace HetareViewer2
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージ リソースが破棄される場合 true、破棄されない場合は false です。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.imagePictureBox = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.deleteButton = new System.Windows.Forms.Button();
            this.moveButton = new System.Windows.Forms.Button();
            this.prevBinderButton = new System.Windows.Forms.Button();
            this.nextBinderButton = new System.Windows.Forms.Button();
            this.rotateSelectComboBox = new System.Windows.Forms.ComboBox();
            this.PrevButton = new System.Windows.Forms.Button();
            this.NextButton = new System.Windows.Forms.Button();
            this.openButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imagePictureBox
            // 
            this.imagePictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.imagePictureBox.Location = new System.Drawing.Point(0, 33);
            this.imagePictureBox.Name = "imagePictureBox";
            this.imagePictureBox.Size = new System.Drawing.Size(403, 228);
            this.imagePictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.imagePictureBox.TabIndex = 0;
            this.imagePictureBox.TabStop = false;
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.Controls.Add(this.deleteButton);
            this.panel1.Controls.Add(this.moveButton);
            this.panel1.Controls.Add(this.prevBinderButton);
            this.panel1.Controls.Add(this.nextBinderButton);
            this.panel1.Controls.Add(this.rotateSelectComboBox);
            this.panel1.Controls.Add(this.PrevButton);
            this.panel1.Controls.Add(this.NextButton);
            this.panel1.Controls.Add(this.openButton);
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(403, 31);
            this.panel1.TabIndex = 1;
            // 
            // deleteButton
            // 
            this.deleteButton.Location = new System.Drawing.Point(348, 4);
            this.deleteButton.Name = "deleteButton";
            this.deleteButton.Size = new System.Drawing.Size(52, 23);
            this.deleteButton.TabIndex = 7;
            this.deleteButton.TabStop = false;
            this.deleteButton.Text = "削除...";
            this.deleteButton.UseVisualStyleBackColor = true;
            // 
            // moveButton
            // 
            this.moveButton.Location = new System.Drawing.Point(288, 4);
            this.moveButton.Name = "moveButton";
            this.moveButton.Size = new System.Drawing.Size(54, 23);
            this.moveButton.TabIndex = 6;
            this.moveButton.TabStop = false;
            this.moveButton.Text = "移動";
            this.moveButton.UseVisualStyleBackColor = true;
            // 
            // prevBinderButton
            // 
            this.prevBinderButton.Location = new System.Drawing.Point(147, 4);
            this.prevBinderButton.Name = "prevBinderButton";
            this.prevBinderButton.Size = new System.Drawing.Size(29, 23);
            this.prevBinderButton.TabIndex = 5;
            this.prevBinderButton.TabStop = false;
            this.prevBinderButton.Text = ">>";
            this.prevBinderButton.UseVisualStyleBackColor = true;
            this.prevBinderButton.Click += new System.EventHandler(this.prevBinderButton_Click);
            // 
            // nextBinderButton
            // 
            this.nextBinderButton.Location = new System.Drawing.Point(54, 4);
            this.nextBinderButton.Name = "nextBinderButton";
            this.nextBinderButton.Size = new System.Drawing.Size(29, 23);
            this.nextBinderButton.TabIndex = 4;
            this.nextBinderButton.TabStop = false;
            this.nextBinderButton.Text = "<<";
            this.nextBinderButton.UseVisualStyleBackColor = true;
            this.nextBinderButton.Click += new System.EventHandler(this.nextBinderButton_Click);
            // 
            // rotateSelectComboBox
            // 
            this.rotateSelectComboBox.FormattingEnabled = true;
            this.rotateSelectComboBox.Items.AddRange(new object[] {
            "標準",
            "時計回り90度",
            "反時計回り90度",
            "見開き2ページ"});
            this.rotateSelectComboBox.Location = new System.Drawing.Point(182, 6);
            this.rotateSelectComboBox.Name = "rotateSelectComboBox";
            this.rotateSelectComboBox.Size = new System.Drawing.Size(100, 20);
            this.rotateSelectComboBox.TabIndex = 3;
            this.rotateSelectComboBox.TabStop = false;
            this.rotateSelectComboBox.Text = "標準";
            this.rotateSelectComboBox.SelectedIndexChanged += new System.EventHandler(this.rotateSelectComboBox_SelectedIndexChanged);
            // 
            // PrevButton
            // 
            this.PrevButton.Location = new System.Drawing.Point(118, 4);
            this.PrevButton.Name = "PrevButton";
            this.PrevButton.Size = new System.Drawing.Size(23, 23);
            this.PrevButton.TabIndex = 2;
            this.PrevButton.TabStop = false;
            this.PrevButton.Text = ">";
            this.PrevButton.UseVisualStyleBackColor = true;
            this.PrevButton.Click += new System.EventHandler(this.PrevButton_Click);
            // 
            // NextButton
            // 
            this.NextButton.Location = new System.Drawing.Point(89, 4);
            this.NextButton.Name = "NextButton";
            this.NextButton.Size = new System.Drawing.Size(22, 23);
            this.NextButton.TabIndex = 1;
            this.NextButton.TabStop = false;
            this.NextButton.Text = "<";
            this.NextButton.UseVisualStyleBackColor = true;
            this.NextButton.Click += new System.EventHandler(this.NextButton_Click);
            // 
            // openButton
            // 
            this.openButton.Location = new System.Drawing.Point(4, 4);
            this.openButton.Name = "openButton";
            this.openButton.Size = new System.Drawing.Size(44, 23);
            this.openButton.TabIndex = 0;
            this.openButton.TabStop = false;
            this.openButton.Text = "開く";
            this.openButton.UseVisualStyleBackColor = true;
            this.openButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(403, 261);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.imagePictureBox);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "HetareViewer2";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.imagePictureBox)).EndInit();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox imagePictureBox;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button openButton;
        private System.Windows.Forms.Button NextButton;
        private System.Windows.Forms.Button PrevButton;
        private System.Windows.Forms.ComboBox rotateSelectComboBox;
        private System.Windows.Forms.Button prevBinderButton;
        private System.Windows.Forms.Button nextBinderButton;
        private System.Windows.Forms.Button deleteButton;
        private System.Windows.Forms.Button moveButton;
    }
}

