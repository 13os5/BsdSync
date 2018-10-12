using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using KE.LibraryAPI.Data;

namespace KE.LibraryAPI.Security
{
/* ผู้สร้าง : Mr.Boonhome Wongsuwan (บุญโฮม  วงสุวรรณ์)
 * วันที่สร้าง : 02/10/2018
 * รายละเอียด : จัดการเกี่ยวกับเข้ารหัสและถอดรหัสกับไฟล์ต่างๆ
 * แก้ไขล่าสุด :
 * ################ History of the code ################
 *  1. New Class by Boonhome 02/10/2018
 *  2. 
*/
    public class CryptoHelpException : ApplicationException
    {
        public CryptoHelpException(string msg) : base(msg) { }
    }

    public class EncryptFile
    {
        public string inFile;
        public string outFile;
        public string keyCode;

        public EncryptFile()
        {
        }

        public EncryptFile(string iFile, string kCode)
        {
            //string oFile = iFile.Substring(0, iFile.Length - 4);
            string oFile = iFile + ".ecf";
            FileManage dir = new FileManage();
            dir.CreateFile(oFile);
            inFile = iFile;
            outFile = oFile;
            keyCode = kCode;
        }

        public EncryptFile(string iFile, string oFile, string kCode)
        {
            inFile = iFile;
            outFile = oFile;
            keyCode = kCode;
        }

        private const ulong FC_TAG = 0xFC010203040506CF;
        private const int BUFFER_SIZE = 128 * 1024;

        private bool CheckByteArrays(byte[] b1, byte[] b2)
        {
            if (b1.Length == b2.Length)
            {
                for (int i = 0; i < b1.Length; ++i)
                {
                    if (b1[i] != b2[i])
                        return false;
                }
                return true;
            }
            return false;
        }

        private SymmetricAlgorithm CreateRijndael(string password, byte[] salt)
        {
            PasswordDeriveBytes pdb = new PasswordDeriveBytes(password, salt, "SHA256", 1000);

            SymmetricAlgorithm sma = Rijndael.Create();
            sma.KeySize = 256;
            sma.Key = pdb.GetBytes(32);
            sma.Padding = PaddingMode.PKCS7;
            return sma;
        }

        private RandomNumberGenerator rand = new RNGCryptoServiceProvider();

        private byte[] GenerateRandomBytes(int count)
        {
            byte[] bytes = new byte[count];
            rand.GetBytes(bytes);
            return bytes;
        }

        /// <summary>
        /// This takes an input file and encrypts it into the output file
        /// </summary>
        /// <param name="inFile">the file to encrypt</param>
        /// <param name="outFile">the file to write the encrypted data to</param>
        /// <param name="password">the password for use as the key</param>
        /// <param name="callback">the method to call to notify of progress</param>
        public void Encrypt(string inFile, string keyCode)
        {
            try
            {
                EncryptFile Ency = new EncryptFile(inFile, keyCode);
                Thread Thd1 = new Thread(new ThreadStart(Ency.EncryptTextFile));
                Thd1.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void EncryptTextFile()
        {
            string password = keyCode;// "p@$5w0rd";
            try
            {
                using (FileStream fin = File.OpenRead(inFile), fout = File.OpenWrite(outFile))
                {
                    long lSize = fin.Length; // the size of the input file for storing
                    int size = (int)lSize;  // the size of the input file for progress
                    byte[] bytes = new byte[BUFFER_SIZE]; // the buffer
                    int read = -1; // the amount of bytes read from the input file
                    int value = 0; // the amount overall read from the input file for progress

                    // generate IV and Salt
                    byte[] IV = GenerateRandomBytes(16);
                    byte[] salt = GenerateRandomBytes(16);

                    // create the crypting object
                    SymmetricAlgorithm sma = this.CreateRijndael(password, salt);
                    sma.IV = IV;

                    // write the IV and salt to the beginning of the file
                    fout.Write(IV, 0, IV.Length);
                    fout.Write(salt, 0, salt.Length);

                    // create the hashing and crypto streams
                    HashAlgorithm hasher = SHA256.Create();
                    using (CryptoStream cout = new CryptoStream(fout, sma.CreateEncryptor(), CryptoStreamMode.Write),
                              chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                    {
                        // write the size of the file to the output file
                        BinaryWriter bw = new BinaryWriter(cout);
                        bw.Write(lSize);

                        // write the file cryptor tag to the file
                        bw.Write(FC_TAG);

                        // read and the write the bytes to the crypto stream in BUFFER_SIZEd chunks
                        while ((read = fin.Read(bytes, 0, bytes.Length)) != 0)
                        {
                            cout.Write(bytes, 0, read);
                            chash.Write(bytes, 0, read);
                            value += read;
                            // callback(0, size, value);
                        }
                        // flush and close the hashing object
                        chash.Flush();
                        chash.Close();

                        // read the hash
                        byte[] hash = hasher.Hash;

                        // write the hash to the end of the file
                        cout.Write(hash, 0, hash.Length);

                        // flush and close the cryptostream
                        cout.Flush();
                        cout.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// takes an input file and decrypts it to the output file
        /// </summary>
        /// <param name="inFile">the file to decrypt</param>
        /// <param name="outFile">the to write the decrypted data to</param>
        /// <param name="password">the password used as the key</param>
        /// <param name="callback">the method to call to notify of progress</param>
        public void Decrypt(string inFile, string outFile, string keyCode)
        {
            try
            {
                EncryptFile Decy = new EncryptFile(inFile, outFile, keyCode);
                Thread Thd2 = new Thread(new ThreadStart(Decy.DecryptTextFile));
                Thd2.Start();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void DecryptTextFile()
        {
            string password = keyCode;// "p@$5w0rd";
            try
            {
                // NOTE:  The encrypting algo was so much easier...

                // create and open the file streams
                using (FileStream fin = File.OpenRead(inFile), fout = File.OpenWrite(outFile))
                {
                    int size = (int)fin.Length; // the size of the file for progress notification
                    byte[] bytes = new byte[BUFFER_SIZE]; // byte buffer
                    int read = -1; // the amount of bytes read from the stream
                    int value = 0;
                    int outValue = 0; // the amount of bytes written out

                    // read off the IV and Salt
                    byte[] IV = new byte[16];
                    fin.Read(IV, 0, 16);
                    byte[] salt = new byte[16];
                    fin.Read(salt, 0, 16);

                    // create the crypting stream
                    SymmetricAlgorithm sma = this.CreateRijndael(password, salt);
                    sma.IV = IV;

                    value = 32; // the value for the progress
                    long lSize = -1; // the size stored in the input stream

                    // create the hashing object, so that we can verify the file
                    HashAlgorithm hasher = SHA256.Create();

                    // create the cryptostreams that will process the file
                    using (CryptoStream cin = new CryptoStream(fin, sma.CreateDecryptor(), CryptoStreamMode.Read),
                              chash = new CryptoStream(Stream.Null, hasher, CryptoStreamMode.Write))
                    {
                        // read size from file
                        BinaryReader br = new BinaryReader(cin);
                        lSize = br.ReadInt64();
                        ulong tag = br.ReadUInt64();

                        if (FC_TAG != tag)
                            throw new CryptoHelpException("File Corrupted!");

                        //determine number of reads to process on the file
                        long numReads = lSize / BUFFER_SIZE;

                        // determine what is left of the file, after numReads
                        long slack = (long)lSize % BUFFER_SIZE;

                        // read the buffer_sized chunks
                        for (int i = 0; i < numReads; ++i)
                        {
                            read = cin.Read(bytes, 0, bytes.Length);
                            fout.Write(bytes, 0, read);
                            chash.Write(bytes, 0, read);
                            value += read;
                            outValue += read;
                            //callback(0, size, value);
                        }

                        // now read the slack
                        if (slack > 0)
                        {
                            read = cin.Read(bytes, 0, (int)slack);
                            fout.Write(bytes, 0, read);
                            chash.Write(bytes, 0, read);
                            value += read;
                            outValue += read;
                            // callback(0, size, value);
                        }
                        // flush and close the hashing stream
                        chash.Flush();
                        chash.Close();

                        // flush and close the output file
                        fout.Flush();
                        fout.Close();

                        // read the current hash value
                        byte[] curHash = hasher.Hash;

                        // get and compare the current and old hash values
                        byte[] oldHash = new byte[hasher.HashSize / 8];
                        read = cin.Read(oldHash, 0, oldHash.Length);
                        if ((oldHash.Length != read) || (!CheckByteArrays(oldHash, curHash)))
                            throw new CryptoHelpException("File Corrupted!");
                    }

                    // make sure the written and stored size are equal
                    if (outValue != lSize)
                        throw new CryptoHelpException("File Sizes don't match!");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}