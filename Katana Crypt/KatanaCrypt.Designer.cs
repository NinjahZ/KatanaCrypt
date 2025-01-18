namespace Katana_Crypt
{
    partial class KatanaCrypt
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            SelectFileBtn = new System.Windows.Forms.Button();
            SelectFileLocTxt = new System.Windows.Forms.TextBox();
            EncryptBtn = new System.Windows.Forms.Button();
            DecryptBtn = new System.Windows.Forms.Button();
            ToolTipText = new System.Windows.Forms.ToolTip(components);
            SuspendLayout();
            // 
            // SelectFileBtn
            // 
            SelectFileBtn.Location = new System.Drawing.Point(447, 14);
            SelectFileBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelectFileBtn.Name = "SelectFileBtn";
            SelectFileBtn.Size = new System.Drawing.Size(100, 27);
            SelectFileBtn.TabIndex = 0;
            SelectFileBtn.Tag = "Select a File to Encrypt/Decrypt";
            SelectFileBtn.Text = "Select File";
            SelectFileBtn.UseVisualStyleBackColor = true;
            SelectFileBtn.Click += SelectFileBtn_Click;
            SelectFileBtn.MouseLeave += Control_MouseLeave;
            SelectFileBtn.MouseHover += Control_MouseHover;
            // 
            // SelectFileLocTxt
            // 
            SelectFileLocTxt.Location = new System.Drawing.Point(14, 14);
            SelectFileLocTxt.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelectFileLocTxt.Name = "SelectFileLocTxt";
            SelectFileLocTxt.Size = new System.Drawing.Size(425, 23);
            SelectFileLocTxt.TabIndex = 1;
            SelectFileLocTxt.Tag = "The path to the selected file to Encrypt/Decrypt.";
            SelectFileLocTxt.MouseLeave += Control_MouseLeave;
            SelectFileLocTxt.MouseHover += Control_MouseHover;
            // 
            // EncryptBtn
            // 
            EncryptBtn.Location = new System.Drawing.Point(14, 44);
            EncryptBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            EncryptBtn.Name = "EncryptBtn";
            EncryptBtn.Size = new System.Drawing.Size(533, 27);
            EncryptBtn.TabIndex = 2;
            EncryptBtn.Tag = "Encrypts the selected file. Only non-.kenc files can be encrypted.";
            EncryptBtn.Text = "Encrypt File";
            EncryptBtn.UseVisualStyleBackColor = true;
            EncryptBtn.Click += EncryptBtn_Click;
            EncryptBtn.MouseLeave += Control_MouseLeave;
            EncryptBtn.MouseHover += Control_MouseHover;
            // 
            // DecryptBtn
            // 
            DecryptBtn.Location = new System.Drawing.Point(14, 77);
            DecryptBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DecryptBtn.Name = "DecryptBtn";
            DecryptBtn.Size = new System.Drawing.Size(533, 27);
            DecryptBtn.TabIndex = 3;
            DecryptBtn.Tag = "Decrypts the selected file. Only .kenc files can be decrypted.";
            DecryptBtn.Text = "Decrypt File";
            DecryptBtn.UseVisualStyleBackColor = true;
            DecryptBtn.Click += DecryptBtn_Click;
            DecryptBtn.MouseLeave += Control_MouseLeave;
            DecryptBtn.MouseHover += Control_MouseHover;
            // 
            // KatanaCrypt
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(561, 119);
            Controls.Add(DecryptBtn);
            Controls.Add(EncryptBtn);
            Controls.Add(SelectFileLocTxt);
            Controls.Add(SelectFileBtn);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "KatanaCrypt";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "KatanaCrypt";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button SelectFileBtn;
        private System.Windows.Forms.TextBox SelectFileLocTxt;
        private System.Windows.Forms.Button EncryptBtn;
        private System.Windows.Forms.Button DecryptBtn;
        private System.Windows.Forms.ToolTip ToolTipText;
    }
}

