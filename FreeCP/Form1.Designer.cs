using System.Drawing;
using System.Windows.Forms;

namespace FreeCP
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.BindButton = new System.Windows.Forms.Button();
            this.ReleaseButton = new System.Windows.Forms.Button();
            this.bindInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(18, 9);
            this.richTextBox1.Margin = new System.Windows.Forms.Padding(1);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(312, 122);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // BindButton
            // 
            this.BindButton.Location = new System.Drawing.Point(40, 136);
            this.BindButton.Margin = new System.Windows.Forms.Padding(1);
            this.BindButton.Name = "BindButton";
            this.BindButton.Size = new System.Drawing.Size(64, 29);
            this.BindButton.TabIndex = 1;
            this.BindButton.Text = "绑定";
            this.BindButton.UseVisualStyleBackColor = true;
            this.BindButton.Click += new System.EventHandler(this.BindButton_Click);
            // 
            // ReleaseButton
            // 
            this.ReleaseButton.Location = new System.Drawing.Point(242, 136);
            this.ReleaseButton.Margin = new System.Windows.Forms.Padding(1);
            this.ReleaseButton.Name = "ReleaseButton";
            this.ReleaseButton.Size = new System.Drawing.Size(64, 29);
            this.ReleaseButton.TabIndex = 2;
            this.ReleaseButton.Text = "解绑";
            this.ReleaseButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ReleaseButton.UseVisualStyleBackColor = true;
            this.ReleaseButton.Click += new System.EventHandler(this.ReleaseButton_Click);
            // 
            // bindInfo
            // 
            this.bindInfo.AutoSize = true;
            this.bindInfo.Location = new System.Drawing.Point(155, 144);
            this.bindInfo.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.bindInfo.Name = "bindInfo";
            this.bindInfo.Size = new System.Drawing.Size(29, 12);
            this.bindInfo.TabIndex = 3;
            this.bindInfo.Text = "状态";
            this.bindInfo.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(343, 180);
            this.Controls.Add(this.bindInfo);
            this.Controls.Add(this.ReleaseButton);
            this.Controls.Add(this.BindButton);
            this.Controls.Add(this.richTextBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "Form1";
            this.Text = "FreeCP";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox richTextBox1;
        private Button BindButton;
        private Button ReleaseButton;
        private Label bindInfo;
    }
}
