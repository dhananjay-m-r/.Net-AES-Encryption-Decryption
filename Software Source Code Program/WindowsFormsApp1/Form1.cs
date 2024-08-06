using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private static byte[] IV = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox2.UseSystemPasswordChar = true;
            textBox3.UseSystemPasswordChar = true;
        }

        private string Encrypt(string plainText, string Password, byte[] IV)
        {
            byte[] Key = Encoding.UTF8.GetBytes(Password);

            // Create a new AesManaged.    
            AesManaged aes = new AesManaged();
            aes.Key = Key;
            aes.IV = IV;

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write);

            byte[] InputBytes = Encoding.UTF8.GetBytes(plainText);
            cryptoStream.Write(InputBytes, 0, InputBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] Encrypted = memoryStream.ToArray();
            // Return encrypted data    
            return Convert.ToBase64String(Encrypted);
        }

        private string Decrypt(string plaintext, string Password, byte[] IV)
        {
            byte[] Key = Encoding.UTF8.GetBytes(Password);

            // Create a new AesManaged.    
            AesManaged aes = new AesManaged();
            aes.Key = Key;
            aes.IV = IV;

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Write);

            byte[] InputBytes = Convert.FromBase64String(plaintext);
            cryptoStream.Write(InputBytes, 0, InputBytes.Length);
            cryptoStream.FlushFinalBlock();

            byte[] Decrypted = memoryStream.ToArray();
            // Return encrypted data    
            return UTF8Encoding.UTF8.GetString(Decrypted, 0, Decrypted.Length);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = Encrypt(richTextBox1.Text, textBox2.Text, IV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                textBox4.Text = Decrypt(richTextBox2.Text, textBox3.Text, IV);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(textBox1.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Clipboard.SetDataObject(textBox4.Text);
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length != 16)
            {
                MessageBox.Show("You need to write at least 16 characters.",
                    "Insufficient Characters!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            if (((TextBox)sender).Text.Length != 16)
            {
                MessageBox.Show("You need to write at least 16 characters.",
                    "Insufficient Characters!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = false;
            }
            else if (!checkBox1.Checked)
            {
                textBox2.UseSystemPasswordChar = true;
            }
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                textBox3.UseSystemPasswordChar = false;
            }
            else if (!checkBox2.Checked)
            {
                textBox3.UseSystemPasswordChar = true;
            }
        }
    }
}
