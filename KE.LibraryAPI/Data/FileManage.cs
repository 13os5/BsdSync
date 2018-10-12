using System;
using System.ComponentModel;
using System.IO;


namespace KE.LibraryAPI.Data
{
 /* ผู้สร้าง : Mr.Boonhome Wongsuwan (บุญโฮม  วงสุวรรณ์)
  * วันที่สร้าง : 02/10/2018
  * รายละเอียด :
  *     จัดการเกี่ยวกับ Folder และ File
  *     ตรวจสอบ Folder และ File
  *     สร้าง Folder และ File
  *     ต่อไฟล์ , Copy Folder และ File , ย้ายไฟล์ , Copy ไฟล์
  * แก้ไขล่าสุด :
  * ################ History of the code ################
  * 1. New Class by Boonhome 02/10/2018
  * 2. 
  */
    public class FileManage : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;

        // Track whether Dispose has been called.
        private bool disposed = false;

        //Local resource
        private string _FilePath = string.Empty;

        private string _FolderPath = string.Empty;
        private FileStream StreamFile;
        private StreamWriter StreamWrite;
        private FileInfo mFile;

        public FileManage()
        {
        }

        public FileManage(IntPtr handle)
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
                    if (StreamWrite != null)
                    {
                        StreamWrite.Close();
                    }
                    if (StreamFile != null)
                    {
                        StreamFile.Close();
                    }

                    mFile = null;
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

        ~FileManage()
        {
            Dispose(false);
        }

        #endregion Dispose

        public bool ExistFolder(string PathName)
        {
            try
            {
                if (Directory.Exists(PathName))
                {
                    this.FolderPath = PathName;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ExistFile(string FileName)
        {
            try
            {
                if (File.Exists(FileName))
                {
                    this.FilePath = FileName;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateFolder(string PathName)
        {
            try
            {
                if (!ExistFolder(PathName))
                {
                    Directory.CreateDirectory(PathName);
                    this.FolderPath = PathName;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CreateFile(string FileName)
        {
            try
            {
                if (!ExistFile(FileName))
                {
                    string[] _tmp = FileName.Split('\\');
                    this.CreateFolder(FileName.Substring(0, FileName.Length - _tmp[_tmp.Length - 1].Length));

                    StreamFile = new FileStream(FileName, FileMode.CreateNew);
                    this.FilePath = FileName;
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (StreamFile != null)
                {
                    StreamFile.Close();
                }
            }
        }

        public Boolean AppendFile(string PathFile, string Message)
        {
            StreamFile = new FileStream(PathFile, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            StreamWrite = new StreamWriter(StreamFile);
            try
            {
                StreamWrite.WriteLine(Message);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                if (StreamWrite != null)
                {
                    StreamWrite.Close();
                }
                if (StreamFile != null)
                {
                    StreamFile.Close();
                }
            }
        }

        public Boolean CopyDirectory(DirectoryInfo SourceFile, DirectoryInfo DestinationFile)
        {
            try
            {
                if (!DestinationFile.Exists)
                {
                    DestinationFile.Create();
                }

                // Copy all files.
                FileInfo[] files = SourceFile.GetFiles();
                foreach (FileInfo file in files)
                {
                    file.CopyTo(Path.Combine(DestinationFile.FullName,
                        file.Name));
                }

                // Process subdirectories.
                DirectoryInfo[] dirs = SourceFile.GetDirectories();
                foreach (DirectoryInfo dir in dirs)
                {
                    // Get destination directory.
                    string destinationDir = Path.Combine(DestinationFile.FullName, dir.Name);

                    // Call CopyDirectory() recursively.
                    CopyDirectory(dir, new DirectoryInfo(destinationDir));
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public string[] GetFiles(string SourcePart)
        {
            try
            {
                return Directory.GetFiles(SourcePart);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string[] GetFolder(string SourcePart)
        {
            try
            {
                return Directory.GetDirectories(SourcePart);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Boolean MoveFile(string SourceFile, string DestinationPart)
        {
            FileInfo mFile = new FileInfo(SourceFile);
            try
            {
                if (this.ExistFile(SourceFile))
                {
                    File.Move(SourceFile, DestinationPart + "\\" + mFile.Name);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                mFile = null;
            }
        }

        public Boolean CopyFile(string SourceFile, string DestinationPart)
        {
            mFile = new FileInfo(SourceFile);
            try
            {
                if (this.ExistFile(SourceFile))
                {
                    File.Copy(SourceFile, DestinationPart + "\\" + mFile.Name, true);
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                mFile = null;
            }
        }

        public string[] ReadFile(string FileName)
        {
            string[] result = null;

            try
            {
                if (ExistFile(FileName))
                {
                    string[] lines = File.ReadAllLines(FileName);
                    result = lines;
                }
            }
            catch (Exception ex)
            {
                //wrwite log
            }

            return result;
        }

        #region Properties

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("Defines the string file path")]
        public string FilePath
        {
            get { return _FilePath; }
            set
            {
                if (_FilePath == value)
                    return;
                _FilePath = value;
            }
        }

        [Category("Appearance")]
        [DefaultValue("")]
        [Description("Defines the string folder path")]
        public string FolderPath
        {
            get { return _FolderPath; }
            set
            {
                if (_FolderPath == value)
                    return;
                _FolderPath = value;
            }
        }

        #endregion Properties
    }
}