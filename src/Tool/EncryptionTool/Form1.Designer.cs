
namespace EncryptionTool2
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
            this.decrypt = new System.Windows.Forms.Button();
            this.decryptText = new System.Windows.Forms.RichTextBox();
            this.keyString = new System.Windows.Forms.TextBox();
            this.Key = new System.Windows.Forms.Label();
            this.inputString = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.encryptText = new System.Windows.Forms.RichTextBox();
            this.encrypt = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.RichTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.copy = new System.Windows.Forms.Button();
            this.clearForm = new System.Windows.Forms.Button();
            this.OpenCSV = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // decrypt
            // 
            this.decrypt.Location = new System.Drawing.Point(658, 181);
            this.decrypt.Name = "decrypt";
            this.decrypt.Size = new System.Drawing.Size(75, 23);
            this.decrypt.TabIndex = 0;
            this.decrypt.Text = "Decrypt";
            this.decrypt.UseVisualStyleBackColor = true;
            this.decrypt.Click += new System.EventHandler(this.Decrypt_Click);
            // 
            // decryptText
            // 
            this.decryptText.Location = new System.Drawing.Point(414, 45);
            this.decryptText.Name = "decryptText";
            this.decryptText.Size = new System.Drawing.Size(319, 96);
            this.decryptText.TabIndex = 1;
            this.decryptText.Text = "";
            // 
            // keyString
            // 
            this.keyString.Location = new System.Drawing.Point(44, 152);
            this.keyString.Name = "keyString";
            this.keyString.Size = new System.Drawing.Size(689, 23);
            this.keyString.TabIndex = 2;
            // 
            // Key
            // 
            this.Key.AutoSize = true;
            this.Key.Location = new System.Drawing.Point(12, 155);
            this.Key.Name = "Key";
            this.Key.Size = new System.Drawing.Size(26, 15);
            this.Key.TabIndex = 3;
            this.Key.Text = "Key";
            this.Key.Click += new System.EventHandler(this.label1_Click);
            // 
            // inputString
            // 
            this.inputString.AutoSize = true;
            this.inputString.Location = new System.Drawing.Point(414, 13);
            this.inputString.Name = "inputString";
            this.inputString.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.inputString.Size = new System.Drawing.Size(125, 15);
            this.inputString.TabIndex = 4;
            this.inputString.Text = "Input string to decrypt";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(44, 13);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label1.Size = new System.Drawing.Size(125, 15);
            this.label1.TabIndex = 6;
            this.label1.Text = "Input string to encrypt";
            // 
            // encryptText
            // 
            this.encryptText.Location = new System.Drawing.Point(44, 45);
            this.encryptText.Name = "encryptText";
            this.encryptText.Size = new System.Drawing.Size(319, 96);
            this.encryptText.TabIndex = 5;
            this.encryptText.Text = "";
            // 
            // encrypt
            // 
            this.encrypt.Location = new System.Drawing.Point(288, 181);
            this.encrypt.Name = "encrypt";
            this.encrypt.Size = new System.Drawing.Size(75, 23);
            this.encrypt.TabIndex = 7;
            this.encrypt.Text = "Encrypt";
            this.encrypt.UseVisualStyleBackColor = true;
            this.encrypt.Click += new System.EventHandler(this.Encrypt_Click);
            // 
            // output
            // 
            this.output.Location = new System.Drawing.Point(12, 224);
            this.output.Name = "output";
            this.output.Size = new System.Drawing.Size(721, 65);
            this.output.TabIndex = 8;
            this.output.Text = "";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 199);
            this.label2.Name = "label2";
            this.label2.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label2.Size = new System.Drawing.Size(45, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "Output";
            // 
            // copy
            // 
            this.copy.Location = new System.Drawing.Point(622, 296);
            this.copy.Name = "copy";
            this.copy.Size = new System.Drawing.Size(110, 23);
            this.copy.TabIndex = 10;
            this.copy.Text = "Copy Output";
            this.copy.UseVisualStyleBackColor = true;
            this.copy.Click += new System.EventHandler(this.Copy_Click);
            // 
            // clearForm
            // 
            this.clearForm.Location = new System.Drawing.Point(541, 296);
            this.clearForm.Name = "clearForm";
            this.clearForm.Size = new System.Drawing.Size(75, 23);
            this.clearForm.TabIndex = 11;
            this.clearForm.Text = "Clear Form";
            this.clearForm.UseVisualStyleBackColor = true;
            this.clearForm.Click += new System.EventHandler(this.Button1_Click);
            // 
            // OpenCSV
            // 
            this.OpenCSV.Location = new System.Drawing.Point(460, 296);
            this.OpenCSV.Name = "OpenCSV";
            this.OpenCSV.Size = new System.Drawing.Size(75, 23);
            this.OpenCSV.TabIndex = 12;
            this.OpenCSV.Text = "Open CSV";
            this.OpenCSV.UseVisualStyleBackColor = true;
            this.OpenCSV.Click += new System.EventHandler(this.OpenCSV_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(745, 337);
            this.Controls.Add(this.OpenCSV);
            this.Controls.Add(this.clearForm);
            this.Controls.Add(this.copy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.output);
            this.Controls.Add(this.encrypt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.encryptText);
            this.Controls.Add(this.inputString);
            this.Controls.Add(this.Key);
            this.Controls.Add(this.keyString);
            this.Controls.Add(this.decryptText);
            this.Controls.Add(this.decrypt);
            this.Name = "Form1";
            this.Text = "EncryptionTool";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button decrypt;
        private System.Windows.Forms.RichTextBox decryptText;
        private System.Windows.Forms.TextBox keyString;
        private System.Windows.Forms.Label Key;
        private System.Windows.Forms.Label inputString;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RichTextBox encryptText;
        private System.Windows.Forms.Button encrypt;
        private System.Windows.Forms.RichTextBox output;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button copy;
        private System.Windows.Forms.Button clearForm;
        public System.Windows.Forms.Button OpenCSV;
    }
}

