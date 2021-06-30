using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;

namespace QDCrypt {
    public partial class QDCrypt : Form {
        public QDCrypt() {
            InitializeComponent();
        }

        private void QDCrypt_Load(object sender, EventArgs e) {

        }

        private void btnEncrypt_Click(object sender, EventArgs e) {

            string hashKey = GetSHA256Hash(txtHashKey.Text);
            string key = hashKey.Substring(0, 32);
            string iv = hashKey.Substring(hashKey.Length - 16);

            txtResult.Text = AesEcnrypt(txtCipher.Text.Trim(), key, iv);
        }

        private void btnDecrypt_Click(object sender, EventArgs e) {

            string hashKey = GetSHA256Hash(txtHashKey.Text);
            string key = hashKey.Substring(0, 32);
            string iv = hashKey.Substring(hashKey.Length - 16);

            txtResult.Text = AesDecrypt(txtCipher.Text.Trim(), key, iv);
        }

        // Make Ctrl+A shortcut work on read only textbox
        private void txtResult_KeyDown(object sender, KeyEventArgs e) {
            if (e.Control && e.KeyCode == Keys.A) {
                txtResult.SelectAll();
            }
        }

        private string GetSHA256Hash(string dataToHash) {
            // Setup SHA
            SHA256Managed crypt = new SHA256Managed();
            string hash = string.Empty;

            // Compute hash
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(dataToHash), 0, Encoding.UTF8.GetByteCount(dataToHash));

            // Convert to hex
            foreach (byte bit in crypto)
                hash += bit.ToString("x2");

            return hash;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="key"></param> 32 character long string = 256 bits
        /// <param name="iv"></param> 16 character long string = 128 bits
        /// <returns></returns>
        private string AesEcnrypt(string plainText, string key, string iv) {

            byte[] textBytes = ASCIIEncoding.ASCII.GetBytes(plainText);

            AesCryptoServiceProvider crypt = new AesCryptoServiceProvider {
                BlockSize = 128,
                KeySize = 256,
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                IV = ASCIIEncoding.ASCII.GetBytes(iv),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };

            ICryptoTransform icrypt = crypt.CreateEncryptor(crypt.Key, crypt.IV);

            byte[] result = icrypt.TransformFinalBlock(textBytes, 0, textBytes.Length);
            icrypt.Dispose();

            return Convert.ToBase64String(result);
        }

        private string AesDecrypt(string cipher, string key, string iv) {

            byte[] cipherBytes;

            // Catch errors if cipher is not a Base64String.
            try {
                cipherBytes = Convert.FromBase64String(cipher);
            } catch (Exception) {

                return cipher;
            }

            AesCryptoServiceProvider crypt = new AesCryptoServiceProvider {
                BlockSize = 128,
                KeySize = 256,
                Key = ASCIIEncoding.ASCII.GetBytes(key),
                IV = ASCIIEncoding.ASCII.GetBytes(iv),
                Padding = PaddingMode.PKCS7,
                Mode = CipherMode.CBC
            };

            ICryptoTransform icrypt = crypt.CreateDecryptor(crypt.Key, crypt.IV);
            byte[] result;

            // Catch errors if the decryption failed.
            try {
                result = icrypt.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                icrypt.Dispose();
            } catch (Exception) {

                return cipher;
            }
            
            return ASCIIEncoding.ASCII.GetString(result);
        }

        private void btnClear_Click(object sender, EventArgs e) {
            txtCipher.Clear();
            txtResult.Clear();
        }

        private void btnAbout_Click(object sender, EventArgs e) {
            System.Diagnostics.Process.Start("https://github.com/crobengames/QDCrypt/blob/main/LICENSE");
        }

        #region Button Visual Effect

        private void btnEncrypt_MouseEnter(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnEncrypt, BtnType.Encrypt, BtnState.Hovered);
        }

        private void btnEncrypt_MouseLeave(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnEncrypt, BtnType.Encrypt, BtnState.Leave);
        }

        private void btnDecrypt_MouseEnter(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnDecrypt, BtnType.Decrypt, BtnState.Hovered);
        }

        private void btnDecrypt_MouseLeave(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnDecrypt, BtnType.Decrypt, BtnState.Leave);
        }

        private void btnClear_MouseEnter(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnClear, BtnType.Clear, BtnState.Hovered);
        }

        private void btnClear_MouseLeave(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnClear, BtnType.Clear, BtnState.Leave);
        }

        private void btnAbout_MouseEnter(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnAbout, BtnType.About, BtnState.Hovered);
        }

        private void btnAbout_MouseLeave(object sender, EventArgs e) {
            AppButtons.UpdateButtonImage(btnAbout, BtnType.About, BtnState.Leave);
        }

        #endregion /Button Visual Effect

    }
}
