using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.InteropServices;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

namespace BsdServiceSync
{
    /* ผู้สร้าง : Mr.Boonhome Wongsuwan (บุญโฮม  วงสุวรรณ์)
   * วันที่สร้าง : 07/09/2015
   * รายละเอียด :จัดการเกี่ยวกับ Database ทั้งหมด เป็นส่วนประมวลผล
   * แก้ไขล่าสุด :
   * ################ History of the code ################
   * 
  */

    /// <summary>
    /// จัดการเกี่ยวกับ Database ทั้งหมด เป็นส่วนประมวลผล
    /// </summary>
    public class DBManage : IDisposable
    {
        // Pointer to an external unmanaged resource.
        private IntPtr handle;
        // Track whether Dispose has been called.
        private bool disposed = false;
        //Local resource

        //SQLServer
        private SqlConnection SqlConn;
        private SqlCommand SqlCmd;
        private SqlDataAdapter SqlDa;
        protected SqlTransaction SqlTrasactions;

        private DataTable objDt;
        private DataSet objDs;

        private string _ConnStr { get; set; }
        private bool _MakeTrasaction = false;

        public DBManage()
        {
            //this._ConnStr = ConfigurationManager.AppSettings["SQLServer"].ToString();
            this._ConnStr = ConfigurationManager.ConnectionStrings["BSDService"].ConnectionString;
            this.MakeTrasaction = false;
            this.Open();
        }

        public DBManage(string ConnectionString)
        {
            this._ConnStr = ConnectionString;
            this.MakeTrasaction = false;
            this.Open();
        }

        public DBManage(string ConnectionString, bool MakeTrasaction)
        {
            this._ConnStr = ConnectionString;
            this.MakeTrasaction = MakeTrasaction;
            this.Open();
        }

        public DBManage(bool MakeTrasaction)
        {
            this._ConnStr = ConfigurationManager.ConnectionStrings["BSDService"].ConnectionString;
            this.MakeTrasaction = MakeTrasaction;
            this.Open();
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
                }
                // Call the appropriate methods to clean up 
                // unmanaged resources here.
                // ex. Boolean , DataTable
                if (SqlConn != null)
                {
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
                if (SqlCmd != null)
                {
                    SqlCmd.Dispose();
                }

                if (SqlDa != null)
                {
                    SqlDa.Dispose();
                }

                if (objDt != null)
                {
                    objDt.Dispose();
                }

                if (objDs != null)
                {
                    objDs.Dispose();
                }

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
        ~DBManage()
        {
            Dispose(false);
        }

        #endregion

        #region Properties

        public string ConnectionStrings
        {
            get { return _ConnStr; }
        }

        private bool MakeTrasaction
        {
            get { return _MakeTrasaction; }
            set
            {
                if (_MakeTrasaction == value)
                    return;
                _MakeTrasaction = value;
            }
        }

        #endregion

        #region Operation

        /// <summary>
        /// Connection to  Database
        /// </summary>
        public bool Open()
        {
            try
            {
                this.SqlConn = new SqlConnection();
                if (this.SqlConn.State == ConnectionState.Open)
                {
                    this.SqlConn.Close();
                }
                this.SqlConn.ConnectionString = this.ConnectionStrings;
                if (this.MakeTrasaction == true)
                {
                    this.SqlConn.Open();
                    this.SqlTrasactions = this.SqlConn.BeginTransaction();
                    return true;
                }
                else
                {
                    this.SqlConn.Open();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //AppLog.Error("Open : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// Disconnection form Database
        /// </summary>
        public bool Close()
        {
            try
            {
                if (this.SqlConn.State == ConnectionState.Open)
                {
                    this.SqlConn.Close();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                //AppLog.Error("Close : ", ex, "DBManage");
                throw ex;
            }
        }

        public bool TransactionCommit()
        {
            try
            {
                if (this.MakeTrasaction == true)
                {
                    this.SqlTrasactions.Commit();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool TransactionRollBack()
        {
            try
            {
                if (this.MakeTrasaction == true)
                {
                    this.SqlTrasactions.Rollback();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region ExecuteReader


        /// <summary>
        /// Execute SQL By ExecuteNonQuery 
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string _SQL)
        {
            this.SqlCmd = new SqlCommand();
            try
            {
                this.SqlCmd.CommandType = CommandType.Text;
                this.SqlCmd.CommandText = _SQL;
                this.SqlCmd.Connection = this.SqlConn;
                if (this.MakeTrasaction == true)
                {
                    this.SqlCmd.Transaction = this.SqlTrasactions;
                }
                return this.SqlCmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteReader[2] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// จัดการในส่วนการเพิ่ม Add Parameters จากข้างนอกเข้ามาได้ หรือ ส่ง Sql Command เข้ามาได้
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <param name="Cmd">SqlCommand</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(string _SQL, SqlCommand Cmd)
        {
            try
            {
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = _SQL;
                Cmd.Connection = this.SqlConn;

                if (this.MakeTrasaction == true)
                {
                    Cmd.Transaction = this.SqlTrasactions;
                }
                return Cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteReader[3] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// จัดการในส่วนการหรือ ส่ง SqlCommand เข้ามาได้
        /// </summary>
        /// <param name="Cmd">SqlCommand</param>
        /// <returns>SqlDataReader</returns>
        public SqlDataReader ExecuteReader(SqlCommand Cmd)
        {
            try
            {
                Cmd.CommandType = CommandType.Text;
                Cmd.Connection = this.SqlConn;
                if (this.MakeTrasaction == true)
                {
                    Cmd.Transaction = this.SqlTrasactions;
                }
                return Cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteReader[4] : ", ex, "DBManage");
                throw ex;
            }
        }

        #endregion

        #region ExecuteNonQuery


        /// <summary>
        /// Execute SQL By ExecuteNonQuery 
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <returns>Integer</returns>
        public int ExecuteNonQuery(string _SQL)
        {
            this.SqlCmd = new SqlCommand();
            try
            {
                this.SqlCmd.CommandType = CommandType.Text;
                this.SqlCmd.CommandText = _SQL;
                this.SqlCmd.CommandTimeout = 5000;
                this.SqlCmd.Connection = this.SqlConn;
                if (this.MakeTrasaction == true)
                {
                    this.SqlCmd.Transaction = this.SqlTrasactions;
                }
                return this.SqlCmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteNonQuery[2] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// จัดการในส่วนการเพิ่ม Add Parameters จากข้างนอกเข้ามาได้ หรือ ส่ง SqlCommand เข้ามาได้
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <param name="Cmd">SqlCommand</param>
        /// <returns>int</returns>
        public int ExecuteNonQuery(string _SQL, SqlCommand Cmd)
        {
            try
            {
                Cmd.CommandText = _SQL;
                Cmd.CommandTimeout = 5000;
                Cmd.Connection = this.SqlConn;
                if (this.MakeTrasaction == true)
                {
                    Cmd.Transaction = this.SqlTrasactions;
                }
                return Cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteNonQuery[3] : ", ex, "DBManage");
                throw ex;
            }
        }

        #endregion

        #region ExecuteScalar


        /// <summary>
        /// Execute SQL By ExecuteScalar 
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <returns>Object</returns>
        public Object ExecuteScalar(string _SQL)
        {
            this.SqlCmd = new SqlCommand();
            try
            {
                this.SqlCmd.CommandType = CommandType.Text;
                this.SqlCmd.CommandText = _SQL;
                this.SqlCmd.Connection = this.SqlConn;
                if (this.MakeTrasaction == true)
                {
                    this.SqlCmd.Transaction = this.SqlTrasactions;
                }
                return SqlCmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteScalar[2] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// จัดการในส่วนการเพิ่ม Add Parameters จากข้างนอกเข้ามาได้ หรือ ส่ง SqlCommand เข้ามาได้
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <param name="Cmd">SqlCommand</param>
        /// <returns>Object</returns>
        public Object ExecuteScalar(string _SQL, SqlCommand Cmd)
        {
            try
            {
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = _SQL;
                Cmd.Connection = this.SqlConn;
                if (this.MakeTrasaction == true)
                {
                    Cmd.Transaction = this.SqlTrasactions;
                }
                return Cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteScalar[3] : ", ex, "DBManage");
                throw ex;
            }
        }

        #endregion

        #region ExecuteDataTable


        /// <summary>
        /// Execute SQL By ExecuteReader 
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string _SQL)
        {
            this.objDt = new DataTable();
            try
            {
                this.SqlDa = new SqlDataAdapter(_SQL, this.SqlConn);
                this.SqlDa.SelectCommand.CommandTimeout = 5000;
                this.SqlDa.Fill(this.objDt);
                if (this.objDt.Rows.Count > 0)
                {
                    return this.objDt;
                }
                return this.objDt;
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteDataTable[1] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// Execute SQL By ExecuteReader 
        /// </summary>
        /// <param name="_SQL">SqlCommand</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(SqlCommand Cmd)
        {
            this.objDt = new DataTable();
            try
            {
                Cmd.Connection = this.SqlConn;
                this.SqlDa = new SqlDataAdapter(Cmd);
                this.SqlDa.SelectCommand.CommandTimeout = 5000;
                this.SqlDa.Fill(this.objDt);
                if (this.objDt.Rows.Count > 0)
                {
                    return this.objDt;
                }
                return this.objDt;
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteDataTable[2] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// Exeute Data bypass to method
        /// </summary>
        /// <param name="Cmd">SqlCommand</param>
        /// <param name="Type">Query by DataAdapter or DataReader</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(SqlCommand Cmd, string Type)
        {
            this.objDt = new DataTable();
            try
            {
                if (Type.Equals("DataAdapter"))
                {
                    Cmd.Connection = this.SqlConn;
                    this.SqlDa = new SqlDataAdapter(Cmd);
                    this.SqlDa.SelectCommand.CommandTimeout = 5000;
                    this.SqlDa.Fill(this.objDt);
                    if (this.objDt.Rows.Count > 0)
                    {
                        return this.objDt;
                    }
                }
                else if (Type.Equals("DataReader"))
                {
                    this.objDt.Load(this.ExecuteReader(Cmd));
                    if (this.objDt.Rows.Count > 0)
                    {
                        return this.objDt;
                    }
                }
                return this.objDt;
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteDataTable[3] : ", ex, "DBManage");
                throw ex;
            }
        }

        /// <summary>
        /// Execute SQL By ExecuteReader Or DataAdapter
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <param name="Type">DataAdapter Or DataReader</param>
        /// <returns>DataTable</returns>
        public DataTable ExecuteDataTable(string _SQL, string Type)
        {
            this.objDt = new DataTable();
            try
            {
                if (Type.Equals("DataAdapter"))
                {
                    return this.ExecuteDataTable(_SQL);
                }
                else if (Type.Equals("DataReader"))
                {
                    this.objDt.Load(ExecuteReader(_SQL));
                    if (this.objDt.Rows.Count > 0)
                    {
                        return this.objDt;
                    }
                }
                return objDt;
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteDataTable[4] : ", ex, "DBManage");
                throw ex;
            }
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_SQL">SQL String</param>
        /// <param name="TableName">DataTable Name</param>
        /// <returns>DataSet</returns>
        public DataSet ExecuteDataSet(string _SQL, string TableName)
        {
            this.objDs = new DataSet();
            try
            {
                this.SqlDa = new SqlDataAdapter(_SQL, this.SqlConn);
                this.SqlDa.Fill(this.objDs, TableName);
                if (this.objDs.Tables[TableName].Rows.Count > 0)
                {
                    return this.objDs;
                }
                return this.objDs;
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteDataSet[1] : ", ex, "DBManage");
                throw ex;
            }
        }

        #endregion

        public DataTable ExecuteDataTableStored(SqlCommand cmd)
        {
            this.objDt = new DataTable();
            try
            {
                cmd.Connection = SqlConn;
                cmd.CommandType = CommandType.StoredProcedure;

                if (this.MakeTrasaction == true)
                {
                    cmd.Transaction = this.SqlTrasactions;
                }
                this.SqlDa = new SqlDataAdapter(cmd);
                this.SqlDa.SelectCommand.CommandTimeout = 5000;
                this.SqlDa.Fill(this.objDt);
                if (this.objDt.Rows.Count > 0)
                {
                    return this.objDt;
                }
                return this.objDt;
            }
            catch (Exception ex)
            {
                //AppLog.Error("ExecuteDataTableStored[1] : ", ex, "DBManage");
                throw ex;
            }
        }


    }
}


