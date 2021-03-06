﻿/********************************************************************************
* File:DataAccessHelper.cs
* Author:austin chen
* Date:2008-4-21
* Copyright (c) fivemen company
*********************************************************************************/
using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using log4net;

namespace FT.DAL
{
    public abstract class DataAccessHelper:IDataAccess
    {
        protected ILog log = log4net.LogManager.GetLogger("FT.DAL");
        private static string connStr = string.Empty;

        /// <summary>
        /// 根据默认的配置DefaultConnString 创建一个DbConnection
        /// </summary>
        /// <returns>创建一个DbConnection</returns>
        protected IDbConnection CreateConn()
        {
            if (connStr.Length == 0)
            {
                object encrypt = System.Configuration.ConfigurationManager.AppSettings["DefaultDbEncrypt"];
                bool isEncrypt = Convert.ToBoolean(encrypt==null?"true":encrypt.ToString());
                string tmp = System.Configuration.ConfigurationManager.AppSettings["DefaultConnString"];
                log.Debug("DefaultConnString is:" + tmp);
                if (isEncrypt)
                {
                    connStr = FT.Commons.Security.SecurityFactory.GetSecurity().Decrypt(tmp);
                }
                else
                {
                    connStr = tmp;
                }
            }
           
            return this.CreateConn(connStr);
        }
        public abstract string LargerEqualDateString(string stringcolumn, DateTime before);

        public abstract string LowerEqualDateString(string stringcolumn, DateTime before);

        public abstract string LargerDateString(string stringcolumn, DateTime before);

        public abstract string LowerDateString(string stringcolumn, DateTime before);

        /// <summary>
        /// 字符串时间列得出对比的sql语句
        /// </summary>
        /// <param name="stringcolumn"></param>
        /// <param name="before"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public abstract string BetweenDateString(string stringcolumn, DateTime before, DateTime end);
        /// <summary>
        /// date的时间列对比出的sql语句
        /// </summary>
        /// <param name="column"></param>
        /// <param name="before"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public abstract string BetweenDate(string column, DateTime before, DateTime end);
        

        /// <summary>
        /// 根据连接字符串connString 创建一个DbConnection
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns>创建一个DbConnection</returns>
        public abstract IDbConnection CreateConn(string connString);

        /// <summary>
        /// 根据链接创建一个DbDataAdapter
        /// </summary>
        /// <returns>创建一个DbDataAdapter</returns>
        public abstract IDbDataAdapter CreateAdapter();

        /// <summary>
        /// 获取数据库方言
        /// </summary>
        /// <returns>方言</returns>
        public abstract DBDialect GetDialectType();

        /// <summary>
        /// 创建命令Builder
        /// </summary>
        /// <returns>命令Builder</returns>
        public abstract DbCommandBuilder CreateCommandBuilder(DbDataAdapter adapter);

        /// <summary>
        /// 创建一个数据命令
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>DbCommand</returns>
        public IDbCommand CreateCommand(string sql)
        {
           IDbCommand command= conn.CreateCommand();
           command.CommandText = sql;
            
           return command;
        }
         /// <summary>
        /// Sql连接
        /// </summary>
        protected IDbConnection conn;
        /// <summary>
        /// 默认构造函数
        /// </summary>
        public DataAccessHelper()
        {

            conn = this.CreateConn();
        }
        /// <summary>
        /// 带连接字符串的构造函数
        /// </summary>
        /// <param name="connString">连接字符串</param>
        public DataAccessHelper(string connString)
        {
            conn = this.CreateConn(connString);
        }

        /// <summary>
        /// 打开数据库连接
        /// </summary>
        /// <returns>打开成功返回true，失败返回false</returns>
        public bool Open()
        {
            if (conn == null || conn.State == ConnectionState.Closed)
            {
                try
                {
                    log.Debug("begin open the db connection");
                    conn.Open();
                    return true;
                }
                catch (Exception e)
                {
                    log.Info("查询打开数据库异常信息->" + e);
                    throw e;
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 根据连接字符串打开连接
        /// </summary>
        /// <param name="connString">连接字符串</param>
        /// <returns>打开成功返回true，失败返回false</returns>
        public bool Open(string connString)
        {
            try
            {
                
                conn = this.CreateConn(connString);
                log.Debug("begin open the db connection");
                conn.Open();
                return true;
            }
            catch (Exception e)
            {
                log.Info("查询打开数据库->" + connString + "异常信息->" + e);
                throw e;
                return false;
            }

        }
        /// <summary>
        /// 关闭连接
        /// </summary>
        /// <returns>返回关闭结果，成功为true，失败返回false</returns>
        public bool Close()
        {
            try
            {
                log.Debug("begin close the db connection");
                if (conn != null) conn.Close();
            }
            catch (Exception e)
            {
                log.Info(e);
                return (false);
            }
            return (true);
        }

        /// <summary>
        /// 根据sql语句返回DataReader,调用者需要负责连接的开关
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回一个DataReader对象或者null</returns>
        public IDataReader SelectDR(string sql)
        {
            log.Debug("SelectDR sql is:" + sql);
            IDataReader dr = null;
            try
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                dr = cmd.ExecuteReader();
            }
            catch (Exception e)
            {
                log.Info("查询sql执行出错->" + sql + "异常信息->" + e);
                return null;
            }
            return dr;
        }

        /// <summary>
        /// 根据sql语句返回SqlDataReader
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回一个SqlDataReader对象或者null</returns>
        public IDataReader SelectDRClosing(string sql)
        {
            log.Debug("SelectDRClosing sql is:" + sql );
            IDataReader dr = null;
            this.Open();
            try
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                dr = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            }
            catch (Exception e)
            {
                log.Info("查询sql执行出错->" + sql + "异常信息->" + e);
                return null;
            }
            return dr;
        }


        /// <summary>
        /// 根据表名和sql语句返回一个DataSet
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="tableName">表名</param>
        /// <returns>返回一个dataset或者null</returns>
        public DataSet SelectDS(string sql, string tableName)
        {
            DataTable dt = this.SelectDataTable(sql, tableName);
            if (dt == null)
                return null;
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            return ds;
        }

        /// <summary>
        /// 根据表名和sql语句返回一个DataTable
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <param name="tableName">表名</param>
        /// <returns>返回一个datatable或者null</returns>
        public DataTable SelectDataTable(string sql, string tableName)
        {
            // if (Tools.ValidSQL(sql) == false) return (null);
            log.Debug("SelectDataTable sql is:" + sql+" tableName is:"+tableName);
           // DataTable dt = new DataTable(tableName);
            DataSet ds = new DataSet();
          
            try
            {
                IDbDataAdapter oradp = this.CreateAdapter();
                IDbCommand command = conn.CreateCommand();
                command.CommandText = sql;
                //command.CommandText = sql.Replace("\r\n","");
                oradp.SelectCommand = command;
                oradp.Fill(ds);
            }
            catch (Exception e)
            {
                log.Info("查询sql执行出错->" + sql + "异常信息->" + e);
                return null;
            }
            return ds.Tables[0];
        }

        /// <summary>
        /// 功能描述：执行命令带上参数
        /// </summary>
        /// <param name="cmdText">命名</param>
        /// <param name="cmdParms">命令参数列表</param>
        /// <returns>是否执行成功</returns>
        public bool ExecuteSqlByParam(string cmdText, params System.Data.Common.DbParameter[] cmdParms)
        {
            log.Debug("ExecuteSql sql is:" + cmdText);
            this.Open();
            IDbCommand _Command = conn.CreateCommand();
            _Command.CommandText = cmdText;
            try
            {
                if (cmdParms != null)
                {
                    foreach (DbParameter parm in cmdParms)
                    {
                        log.Debug("param " + parm.ParameterName + " value is:" + parm.Value.ToString());
                        _Command.Parameters.Add(parm);
                    }
                }

                _Command.ExecuteNonQuery();
                _Command.Parameters.Clear();
                this.Close();
                return true;
            }
            catch (Exception e)
            {
                log.Error(e);
                this.Close();
                return false;
            }
        }

        /// <summary>
        /// 功能描述：执行存储过程
        /// </summary>
        /// <param name="cmdText">存储过程名称</param>
        /// <param name="cmdParms">存储过程参数列表</param>
        /// <returns>成功输出参数</returns>
        public int ExecuteProcedure(string cmdText, params System.Data.Common.DbParameter[] cmdParms)
        {
            log.Debug("ExecuteProcedure procedure name is:" + cmdText);
            this.Open();
            IDbCommand _Command = conn.CreateCommand();
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.CommandText = cmdText;
            int iRow = 0, iRet = 0;
            try
            {
                if (cmdParms != null)
                {
                    _Command.Parameters.Clear();
                    foreach (DbParameter parm in cmdParms)
                    {
                       // log.Debug("param " + parm.ParameterName + " direction:"+parm.Direction.ToString()+" value is:" +parm.Value!=null? parm.Value.ToString():"null");
                        _Command.Parameters.Add(parm);
                    }
                }

                iRow = _Command.ExecuteNonQuery();
                /*
                if (cmdParms != null && cmdParms.Length > 1)
                {
                    iRet = (int)cmdParms[cmdParms.Length - 1].Value;
                    if (iRet != 0)
                    {
                        // Logger.DebugLog("Excute Procedure:" + cmdText + "RetCode:" + iRet);
                        // Tools.WriteLog("ICS AdminPortal", "执行" + cmdText + "失败：RetCode = " + iRet, System.Diagnostics.EventLogEntryType.Error);
                        // return false;
                    }
                }
               */
                this.Close();
            }
            catch (Exception e)
            {
                log.Error(e);
                iRet= -1;
                this.Close();
            }
            log.Debug("ExecuteProcedure result is:" + iRet);
            return iRet;
            //if ( iRow >0 )	return true;
            //else return false;
        }

        /// <summary>
        /// 根据存储过程返回一个DataSet，一般供分页使用
        /// </summary>
        /// <param name="cmdText">存储过程的名称</param>
        /// <param name="cmdParms">参数</param>
        /// <returns>一个DataSet</returns>
        public DataSet SelectDSByProcedure(string cmdText, params DbParameter[] cmdParms)
        {
            log.Debug("SelectDSByProcedure procedure name is:" + cmdText);
            IDbCommand _Command = conn.CreateCommand();
            _Command.CommandType = CommandType.StoredProcedure;
            _Command.CommandText = cmdText;
            if (cmdParms != null)
            {
                foreach (DbParameter parm in cmdParms)
                {
                    if (parm.Value != null)
                    {
                        log.Debug("param " + parm.ParameterName + " value is:" + parm.Value.ToString());
                    }
                    else
                    {
                        log.Debug("param " + parm.ParameterName + " value is:null");
                    }
                    _Command.Parameters.Add(parm);
                }
            }
            IDbDataAdapter da = this.CreateAdapter();
            da.SelectCommand = _Command;
            //da.SelectCommand=_Command;

            DataSet ds = new DataSet();
            try
            {
                da.Fill(ds);
            }
            catch (Exception e)
            {
                log.Info("执行sql语句失败"+cmdText+"->" + e);
                return null;
            }
            return ds;

        }

        //public SqlTransaction getTransaction()
        //{
        //    return (conn.BeginTransaction());
        //}

        /// <summary>
        /// 执行sql语句
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>成功true，失败false</returns>
        public bool ExecuteSql(string sql)
        {
            // if (Tools.ValidSQL(sql) == false) return (false);
           //log.Debug("ExecuteSql sql is:" + sql);
            this.Open();
            StringBuilder sb = new StringBuilder(sql);

            int iRow = 0;
            IDbCommand cmd = conn.CreateCommand();
            try
            {
                cmd.CommandText = sb.Replace("\r\n", "").ToString();
                log.Debug("execute sql string is:"+sql);
                iRow = cmd.ExecuteNonQuery();
                this.Close();
            }
            catch (Exception e)
            {
                log.Info("执行sql语句失败"+sql+"->"+e);
                this.Close();
                return false;
            }
            
            return iRow>0;
        }

        /// <summary>
        /// 把sqlArray语句数组实现一个事务
        /// </summary>
        /// <param name="sqlArray">sqlArray语句数组</param>
        /// <returns>成功true，失败false</returns>
        public bool ExecuteTransaction(string[] sqlArray)
        {
            log.Debug("ExecuteTransaction sqlArray's Length:" + sqlArray.Length);
            if (sqlArray == null || sqlArray.Length == 0)
            {
                return false;
            }
            this.Open();
            IDbTransaction OraTrans = conn.BeginTransaction();
            IDbCommand cmd = conn.CreateCommand();
            cmd.Transaction = OraTrans;
            try
            {
                
                foreach (string temp in sqlArray)
                {
                     log.Debug("ExecuteTransaction sqlArray's item is:"+temp);
                    cmd.CommandText = temp;
                    cmd.ExecuteNonQuery();
                }
                OraTrans.Commit();
                this.Close();
                return true;
            }
            catch (Exception e)
            {
                OraTrans.Rollback();
                log.Error(e);
                this.Close();
                return false;
            }
        }


        /// <summary>
        /// 根据sql语句返回Scalar对象
        /// </summary>
        /// <param name="sql">sql语句</param>
        /// <returns>返回Scalar的第一个对象</returns>

        public object SelectScalar(string sql)
        {
            log.Debug("SelectScalar sql:"+sql);
            this.Open();
            object result;
            try
            {
                IDbCommand cmd = conn.CreateCommand();
                cmd.CommandText = sql;
                result = cmd.ExecuteScalar();
                this.Close();
                if (result == null || result is DBNull)
                {
                    log.Debug("SelectScalar result: is null");
                }
                else
                {
                    log.Debug("SelectScalar result:" +result.ToString());
                }
                return result;
            }
            catch (Exception e)
            {
                this.Close();
                log.Info("执行sql语句失败"+sql+"->" + e);
                return null;
            }
           

        }



        #region IDataAccess 成员

        protected string GetPageSqlByLimit(string sql, Pager pager, string order, bool isDesc)
        {
            string result = string.Empty;
            if (sql.StartsWith("select", true, System.Globalization.CultureInfo.CurrentCulture))
            {
                sql = sql.Substring(6);
            }
            int pageSize = pager.PageSize;
            int offset = (pager.CurrentPage - 1) * pager.PageSize;
            sql = "select " + sql + " order by " + order + " " + ((isDesc) ? "desc" : "asc") + " limit "+pager.PageSize.ToString()+","+offset.ToString();
          
            return result;
        }


        public abstract string GetPageSql(string sql, Pager pager,string order,bool isDesc);

        protected string GetPageSqlByTop(string sql, Pager pager,string order,bool isDesc)
        {
            string result = string.Empty;
            if(sql.StartsWith("select",true,System.Globalization.CultureInfo.CurrentCulture))
            {
                sql=sql.Substring(6);
            }
            int pageSize=pager.PageSize;
            if (pager.CurrentPage==1)
            {
                #region 第一页代码
                result = "select top " + pageSize + " " +sql  + " order by " + order + " " + ((isDesc) ? "desc" : "asc") + "";
                #endregion
            }
            else if (pager.CurrentPage>1&&pager.CurrentPage<pager.TotalPages)
            {

                //if ("id" == order)
                    //result = "select top " + pageSize + " " + field + " from " + tableName + " where " + primary + "" + ((isDesc) ? "<" : ">") + "(select " + ((isDesc) ? "min" : "max") + "(" + primary + ") from(select top " + pageSize * (currentPage - 1) + " " + primary + " from " + tableName + " " + ((condition != "") ? "where " + condition + "" : "") + " order by " + order + " " + ((isDesc) ? "desc" : "asc") + " )) " + ((condition != "") ? "and " + condition + "" : "") + " order by " + order + " " + ((isDesc) ? "desc" : "asc") + "";
                //else

                    result = "select top " + pageSize + " * from (select top " + (pager.TotalRecord - (pager.CurrentPage - 1) * pageSize) + " " + sql + " order by " + order + " " + ((isDesc) ? "asc" : "desc") + ") order by " + order + " " + ((isDesc) ? "desc" : "asc") + "";

            }
            else if (pager.CurrentPage==pager.TotalPages)
            {
                #region 最后页
                // sql = "select " + this.field + " from (select top " + (this.totalCount - (this.TotalPages() - 1) * this.pageSize) + " " + this.field + " from " + tableName + " " + ((condition != "") ? "where " + condition + "" : "") + " order by " + order + " " + ((isDesc) ? "asc" : "desc") + ") order by " + order + " " + ((isDesc) ? "desc" : "asc") + "";
                result = "select * from (select top " + (pager.TotalRecord - (pager.TotalPages - 1) * pageSize) + sql + " order by " + order + " " + ((isDesc) ? "asc" : "desc") + ") order by " + order + " " + ((isDesc) ? "desc" : "asc") + "";
                #endregion



            }
            return result;
        }

        #endregion
    }
}
