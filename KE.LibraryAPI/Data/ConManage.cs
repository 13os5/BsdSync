using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using KE.LibraryAPI.Security;
using KE.LibraryAPI.Configuration;
using System.Security.Cryptography;

namespace KE.LibraryAPI.Data
{
    public class ConManage : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;

        // Track whether Dispose has been called.
        private bool disposed = false;

        //Local resource
        private string _FilePath = string.Empty;

        private string _FolderPath = string.Empty;
        private FileStream StreamFile;

        public ConManage()
        {
        }

        public ConManage(IntPtr handle)
        {
            this.handle = handle;
        }

        #region Dispose

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            // Check to see if Dispose has already been called.
            if (!this.disposed)
            {
                if (disposing)
                {
                    // Dispose managed resources.
                    // if use new object class in this . Dispos object is here
                    if (StreamFile != null)
                    {
                        StreamFile.Close();
                    }
                }
                // Call the appropriate methods to clean up
                // unmanaged resources here.
                // ex. Boolean , DataTable

                // If disposing is false,
                // only the following code is executed.
                CloseHandle(handle);
                handle = IntPtr.Zero;
            }
            disposed = true;
        }

        // Use interop to call the method necessary
        // to clean up the unmanaged resource.
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);

        ~ConManage()
        {
            Dispose(false);
        }

        #endregion Dispose

        public StrConnEntity EncriptStrConn(StrConnEntity sce)
        {
            StrConnEntity strCE = new StrConnEntity();
            using (EncryptStr en = new EncryptStr())
            {
                strCE.conStr = en.Encrypt(sce.conStr, ConfigMange.GetAppSettings("Tey"));
                strCE.ip = en.Encrypt(sce.ip, ConfigMange.GetAppSettings("Tey"));
                strCE.db = en.Encrypt(sce.db, ConfigMange.GetAppSettings("Tey"));
                strCE.pwd = en.Encrypt(sce.pwd, ConfigMange.GetAppSettings("Tey"));
                strCE.current = en.Encrypt(sce.current, ConfigMange.GetAppSettings("Tey"));
            }
            return strCE;
        }

        public string EncriptStrConn(string txt)
        {
            string result = "";

            using (EncryptStr en = new EncryptStr())
            {
                result = en.Encrypt(txt, ConfigMange.GetAppSettings("Tey"));
            }
            return result;
        }


        public bool EncriptStrConn(StrConnEntity sce, string path, bool isCreate)
        {
            bool result = false;
            StrConnEntity strCE = new StrConnEntity();
            using (EncryptStr en = new EncryptStr())
            {
                strCE.conStr = en.Encrypt(sce.conStr, ConfigMange.GetAppSettings("Tey"));
                strCE.ip = en.Encrypt(sce.ip, ConfigMange.GetAppSettings("Tey"));
                strCE.db = en.Encrypt(sce.db, ConfigMange.GetAppSettings("Tey"));
                strCE.usn = en.Encrypt(sce.usn, ConfigMange.GetAppSettings("Tey"));
                strCE.pwd = en.Encrypt(sce.pwd, ConfigMange.GetAppSettings("Tey"));
            }

            if (isCreate)
            {
                using (FileManage fm = new FileManage())
                {
                    result = fm.CreateFile(path + @"\bnoc.int");
                    if (result)
                    {
                        fm.AppendFile(path + @"\bnoc.int", strCE.conStr);
                    }

                    result = fm.CreateFile(path + @"\bpi.str");
                    if (result)
                    {
                        fm.AppendFile(path + @"\bpi.str", strCE.ip);
                    }

                    result = fm.CreateFile(path + @"\bbd.agi");
                    if (result)
                    {
                        fm.AppendFile(path + @"\bbd.agi", strCE.db);
                    }

                    result = fm.CreateFile(path + @"\bnsu.dex");
                    if (result)
                    {
                        fm.AppendFile(path + @"\bnsu.dex", strCE.usn);
                    }

                    result = fm.CreateFile(path + @"\bdwp.luk");
                    if (result)
                    {
                        fm.AppendFile(path + @"\bdwp.luk", strCE.pwd);
                    }
                    return result;
                }
            }
            else
            {
                return result;
            }
        }

        //public bool CreateFile(string path, string str)
        //{
        //    bool result = false;
        //    using (StreamFile = new FileStream(path, FileMode.Create))
        //    {
        //        try
        //        {
        //            byte[] info = new UTF8Encoding(true).GetBytes(str);
        //            StreamFile.Write(info, 0, info.Length);
        //            result = true;
        //        }
        //        catch (Exception ex)
        //        {
        //            result = false;
        //        }
        //    }

        //    return result;
        //}

        //public StrConnEntity DecryptStrConn(StrConnEntity sce)
        //{
        //    StrConnEntity strCE = new StrConnEntity();
        //    using (EncryptStr en = new EncryptStr())
        //    {
        //        strCE.conStr = en.Decrypt(sce.conStr, ConfigMange.GetAppSettings("Tey"));
        //        strCE.ip = en.Decrypt(sce.ip, ConfigMange.GetAppSettings("Tey"));
        //        strCE.db = en.Decrypt(sce.db, ConfigMange.GetAppSettings("Tey"));
        //        strCE.pwd = en.Decrypt(sce.pwd, ConfigMange.GetAppSettings("Tey"));
        //        strCE.current = en.Decrypt(sce.current, ConfigMange.GetAppSettings("Tey"));
        //    }
        //    return strCE;
        //}

        public string DecryptStrConn(string cipherText, string keyCode)
        {
            string passPhrase = keyCode;// "p@$5w0rd";
            string saltValue = "s@1tV@luE";        // can be any string
            string hashAlgorithm = "SHA1";             // can be "MD5"
            int passwordIterations = 99;                  // can be any number
            string initVector = "@1B2c3D4e5F6g7H8"; // must be 16 bytes
            int keySize = 256;                // can be 192 or 128
            try
            {
                // Convert strings defining encryption key characteristics into byte
                // arrays. Let us assume that strings only contain ASCII codes.
                // If strings include Unicode characters, use Unicode, UTF7, or UTF8
                // encoding.
                byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
                byte[] saltValueBytes = Encoding.ASCII.GetBytes(saltValue);

                // Convert our ciphertext into a byte array.
                byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

                // First, we must create a password, from which the key will be
                // derived. This password will be generated from the specified
                // passphrase and salt value. The password will be created using
                // the specified hash algorithm. Password creation can be done in
                // several iterations.
                PasswordDeriveBytes password = new PasswordDeriveBytes(passPhrase, saltValueBytes, hashAlgorithm, passwordIterations);

                // Use the password to generate pseudo-random bytes for the encryption
                // key. Specify the size of the key in bytes (instead of bits).
                byte[] keyBytes = password.GetBytes(keySize / 8);

                // Create uninitialized Rijndael encryption object.
                RijndaelManaged symmetricKey = new RijndaelManaged();

                // It is reasonable to set encryption mode to Cipher Block Chaining
                // (CBC). Use default options for other symmetric key parameters.
                symmetricKey.Mode = CipherMode.CBC;

                // Generate decryptor from the existing key bytes and initialization
                // vector. Key size will be defined based on the number of the key
                // bytes.
                ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes);

                // Define memory stream which will be used to hold encrypted data.
                MemoryStream memoryStream = new MemoryStream(cipherTextBytes);

                // Define cryptographic stream (always use Read mode for encryption).
                CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

                // Since at this point we don't know what the size of decrypted data
                // will be, allocate the buffer long enough to hold ciphertext;
                // plaintext is never longer than ciphertext.
                byte[] plainTextBytes = new byte[cipherTextBytes.Length];

                // Start decrypting.
                int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);

                // Close both streams.
                memoryStream.Close();
                cryptoStream.Close();

                // Convert decrypted data into a string.
                // Let us assume that the original plaintext string was UTF8-encoded.
                string plainText = Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);

                // Return decrypted string.

                return plainText;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }

    public class StrConnEntity
    {
        public string conStr { get; set; }
        public string ip { get; set; }
        public string db { get; set; }
        public string usn { get; set; }
        public string pwd { get; set; }
        public string current { get; set; }
    }
}
