using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Katana_Crypt
{
    public partial class KatanaCrypt : Form
    {
        private const int SaltSize = 32;
        private const int KeySize = 32;
        private const int IVSize = 16;
        private const int Iterations = 100_000;

        public KatanaCrypt()
        {
            InitializeComponent();
            EncryptBtn.Enabled = false;
            DecryptBtn.Enabled = false;
        }

        /// <summary>
        /// Handles the Mouse Enter and Mouse Leave events for all controls with a Tooltip.
        /// </summary>
        private void Control_MouseHover(object sender, EventArgs e)
        {
            if (sender is Control control && control.Tag != null)
            {
                ToolTipText.SetToolTip(control, control.Tag.ToString());
            }
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Control control)
            {
                ToolTipText.SetToolTip(control, string.Empty);
            }
        }

        /// <summary>
        /// Handles file selection and updates the UI buttons based on the file extension.
        /// </summary>
        private void SelectFileBtn_Click(object sender, EventArgs e)
        {
            var selectedFile = GetFileFromDialog();
            if (string.IsNullOrEmpty(selectedFile)) return;

            SelectFileLocTxt.Text = selectedFile;
            SetButtonStateBasedOnFileExtension();
        }

        /// <summary>
        /// Handles the encryption of the selected file.
        /// </summary>
        private async void EncryptBtn_Click(object sender, EventArgs e) => await ProcessFile(EncryptFile);

        /// <summary>
        /// Handles the decryption of the selected file.
        /// </summary>
        private async void DecryptBtn_Click(object sender, EventArgs e) => await ProcessFile(DecryptFile);

        /// <summary>
        /// Common file processing method that handles encryption and decryption.
        /// </summary>
        private async Task ProcessFile(Func<string, Task> fileProcessor)
        {
            if (string.IsNullOrEmpty(SelectFileLocTxt.Text))
                return;

            string password = PromptForPassword();
            if (string.IsNullOrEmpty(password))
                return;

            try
            {
                await fileProcessor.Invoke(password);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        /// <summary>
        /// Encrypts the selected file and saves it to a new location.
        /// </summary>
        private async Task EncryptFile(string password)
        {
            byte[] salt = GenerateSalt(), iv = GenerateIV();
            string filePath = SelectFileLocTxt.Text;
            string originalFileName = Path.GetFileName(filePath);
            string encryptedFilePath = GetEncryptedFilePath(filePath);

            using (var inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            using (var outputFileStream = new FileStream(encryptedFilePath, FileMode.Create, FileAccess.Write))
            {
                await WriteMetadata(outputFileStream, salt, iv, password, originalFileName);
                await EncryptFileData(inputFileStream, outputFileStream, password, salt, iv);
            }

            File.Delete(filePath);
            MessageBox.Show("File Encrypted!");
        }

        /// <summary>
        /// Decrypts the selected encrypted file and saves it to a new location.
        /// </summary>
        private async Task DecryptFile(string password)
        {
            string filePath = SelectFileLocTxt.Text;

            try
            {
                using (var inputFileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    byte[] salt = new byte[SaltSize], iv = new byte[IVSize];
                    await inputFileStream.ReadAsync(salt, 0, SaltSize);
                    await inputFileStream.ReadAsync(iv, 0, IVSize);

                    // Check HMAC validity
                    if (!IsHmacValid(inputFileStream, password, salt, iv))
                    {
                        MessageBox.Show("Incorrect password or the file is corrupted.");
                        return;
                    }

                    string originalFileName = await DecryptMetadata(inputFileStream, password, salt, iv);
                    string decryptedFilePath = Path.Combine(Path.GetDirectoryName(filePath), originalFileName);

                    await DecryptFileData(inputFileStream, decryptedFilePath, password, salt, iv);
                }

                File.Delete(filePath);
                MessageBox.Show("File Decrypted Successfully!");
            }
            catch (CryptographicException)
            {
                MessageBox.Show("Decryption failed. The password may be incorrect.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred during decryption: {ex.Message}");
            }
        }

        /// <summary>
        /// Encrypts the data from the input file stream and writes it to the output file stream.
        /// </summary>
        private async Task EncryptFileData(FileStream inputFileStream, FileStream outputFileStream, string password, byte[] salt, byte[] iv)
        {
            using (var cryptoStream = new CryptoStream(outputFileStream, CreateAes(password, salt, iv).CreateEncryptor(), CryptoStreamMode.Write))
            {
                await inputFileStream.CopyToAsync(cryptoStream);
                await cryptoStream.FlushAsync();
                cryptoStream.FlushFinalBlock();
            }
        }

        /// <summary>
        /// Decrypts the data from the input file stream and writes it to the specified output file.
        /// </summary>
        private async Task DecryptFileData(FileStream inputFileStream, string decryptedFilePath, string password, byte[] salt, byte[] iv)
        {
            using (var outputFileStream = new FileStream(decryptedFilePath, FileMode.Create, FileAccess.Write))
            using (var cryptoStream = new CryptoStream(outputFileStream, CreateAes(password, salt, iv).CreateDecryptor(), CryptoStreamMode.Write))
            {
                await inputFileStream.CopyToAsync(cryptoStream);
                await cryptoStream.FlushAsync();
                cryptoStream.FlushFinalBlock();
            }
        }

        /// <summary>
        /// Generates a file path for the encrypted file.
        /// </summary>
        private string GetEncryptedFilePath(string originalFilePath)
        {
            return Path.Combine(Path.GetDirectoryName(originalFilePath), Guid.NewGuid().ToString() + ".kenc");
        }

        /// <summary>
        /// Writes the file metadata (salt, iv, and original filename) to the output file stream.
        /// </summary>
        private async Task WriteMetadata(FileStream outputFileStream, byte[] salt, byte[] iv, string password, string originalFileName)
        {
            await outputFileStream.WriteAsync(salt, 0, salt.Length);
            await outputFileStream.WriteAsync(iv, 0, iv.Length);
            await outputFileStream.WriteAsync(ComputeHmac(password, salt, iv), 0, 32);

            byte[] encryptedMetadata = EncryptBytes(Encoding.UTF8.GetBytes(originalFileName), password, salt, iv);
            await outputFileStream.WriteAsync(BitConverter.GetBytes(encryptedMetadata.Length), 0, sizeof(int));
            await outputFileStream.WriteAsync(encryptedMetadata, 0, encryptedMetadata.Length);
        }

        /// <summary>
        /// Decrypts the file metadata (original filename) from the input file stream.
        /// </summary>
        private async Task<string> DecryptMetadata(FileStream inputFileStream, string password, byte[] salt, byte[] iv)
        {
            byte[] metadataLengthBytes = new byte[sizeof(int)];
            await inputFileStream.ReadAsync(metadataLengthBytes, 0, metadataLengthBytes.Length);

            int metadataLength = BitConverter.ToInt32(metadataLengthBytes, 0);
            byte[] encryptedMetadata = new byte[metadataLength];
            await inputFileStream.ReadAsync(encryptedMetadata, 0, metadataLength);

            byte[] decryptedMetadata = DecryptBytes(encryptedMetadata, password, salt, iv);
            return Encoding.UTF8.GetString(decryptedMetadata);
        }

        /// <summary>
        /// Validates the HMAC value to ensure data integrity.
        /// </summary>
        private bool IsHmacValid(FileStream inputFileStream, string password, byte[] salt, byte[] iv)
        {
            byte[] storedHmac = new byte[32];
            inputFileStream.ReadAsync(storedHmac, 0, storedHmac.Length).Wait();
            byte[] computedHmac = ComputeHmac(password, salt, iv);
            return storedHmac.SequenceEqual(computedHmac);
        }

        /// <summary>
        /// Sets the button state based on the file extension (Encrypt or Decrypt).
        /// </summary>
        private void SetButtonStateBasedOnFileExtension()
        {
            string fileExtension = Path.GetExtension(SelectFileLocTxt.Text).ToLower();
            EncryptBtn.Enabled = fileExtension != ".kenc";
            DecryptBtn.Enabled = fileExtension == ".kenc";
        }

        /// <summary>
        /// Opens a file dialog to select a file.
        /// </summary>
        private string GetFileFromDialog()
        {
            using (var ofd = new OpenFileDialog())
            {
                return ofd.ShowDialog() == DialogResult.OK ? ofd.FileName : null;
            }
        }

        /// <summary>
        /// Prompts the user for a password using the PasswordForm dialog.
        /// </summary>
        private string PromptForPassword()
        {
            using (var passwordForm = new PasswordForm())
            {
                return passwordForm.ShowDialog() == DialogResult.OK ? passwordForm.Password : null;
            }
        }

        /// <summary>
        /// Creates an AES instance with the given password, salt, and IV.
        /// </summary>
        private Aes CreateAes(string password, byte[] salt, byte[] iv)
        {
            var key = DeriveKey(password, salt);
            var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            return aes;
        }

        /// <summary>
        /// Derives a cryptographic key from the password and salt using PBKDF2.
        /// </summary>
        private byte[] DeriveKey(string password, byte[] salt)
        {
            using (var keyDerivationFunction = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256))
            {
                return keyDerivationFunction.GetBytes(KeySize);
            }
        }

        /// <summary>
        /// Generates a random salt value.
        /// </summary>
        private byte[] GenerateSalt()
        {
            var salt = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        /// <summary>
        /// Generates a random initialization vector (IV).
        /// </summary>
        private byte[] GenerateIV()
        {
            var iv = new byte[IVSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(iv);
            }
            return iv;
        }

        /// <summary>
        /// Encrypts the given byte array using the specified password, salt, and IV.
        /// </summary>
        private byte[] EncryptBytes(byte[] data, string password, byte[] salt, byte[] iv)
        {
            using (var aes = CreateAes(password, salt, iv))
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, aes.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Decrypts the given byte array using the specified password, salt, and IV.
        /// </summary>
        private byte[] DecryptBytes(byte[] data, string password, byte[] salt, byte[] iv)
        {
            using (var aes = CreateAes(password, salt, iv))
            using (var ms = new MemoryStream())
            using (var cryptoStream = new CryptoStream(ms, aes.CreateDecryptor(), CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Computes an HMAC value based on the password, salt, and IV.
        /// </summary>
        private byte[] ComputeHmac(string password, byte[] salt, byte[] iv)
        {
            using (var hmac = new HMACSHA256(DeriveKey(password, salt)))
            {
                hmac.TransformBlock(salt, 0, salt.Length, salt, 0);
                hmac.TransformFinalBlock(iv, 0, iv.Length);
                return hmac.Hash;
            }
        }
    }
}