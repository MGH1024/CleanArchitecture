using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace EncryptionTool2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void Encrypt_Click(object sender, EventArgs e)
        {
            var key = keyString.Text;

            var textToEncrypt = encryptText.Text;

            var encryptedText = Protector.Encrypt(textToEncrypt, key);

            output.Text = encryptedText;
        }

        private void Decrypt_Click(object sender, EventArgs e)
        {
            var key = keyString.Text;

            var textToDecrypt = decryptText.Text;

            var decryptedText = Protector.Decrypt(textToDecrypt, key);

            output.Text = decryptedText;
        }

        private void Copy_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(output.Text);
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            decryptText.Text = "";
            encryptText.Text = "";
            output.Text = "";
        }

        private void OpenCSV_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new()
            {
                Filter = "csv files (*.csv)|*.csv"
            };

            DialogResult result = openFileDialog.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK) // Test result.
            {
                string file = openFileDialog.FileName;
                try
                {
                    string text = File.ReadAllText(file);
                    string[] lines = text.Split(
                        new string[] { Environment.NewLine },
                        StringSplitOptions.None
                    );

                    var resultLines = new List<string>();

                    foreach (var line in lines)
                    {
                        resultLines.Add(Protector.Encrypt(line, keyString.Text));
                    }

                    SaveFileDialog saveDialog = new()
                    {
                        Filter = "csv files (*.csv)|*.csv",
                        Title = "Save encrypted CSV..."
                    };

                    DialogResult dialogResult = saveDialog.ShowDialog();
                    if (dialogResult == DialogResult.OK)
                    {
                        String fileName = saveDialog.FileName;
                        File.WriteAllLines(fileName, resultLines.ToArray());
                    }

                }
                catch (IOException ex)
                {
                    MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
