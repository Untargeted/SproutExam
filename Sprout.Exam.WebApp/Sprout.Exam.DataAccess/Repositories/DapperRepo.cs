using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.Extensions.Configuration;
using Sprout.Exam.DataAccess.Interface;

namespace Sprout.Exam.DataAccess.Repositories
{
    public class DapperRepo : IDapperClass
    {
        private readonly IConfiguration config_;
        private SqlConnection con;
        public DapperRepo(IConfiguration config)
        {
            config_ = config;
            con = new SqlConnection(config_.GetConnectionString("DefaultConnection"));
        }
        #region For Stored Procedures
        public bool ExecStoredProcNoParam(string StoredProc)
        {
            try
            {
                con.Open();

                con.Query(StoredProc, commandType: CommandType.StoredProcedure);

                con.Close();

                return true;
            }
            catch
            {
                return false;
            }

        }
        public List<T> ExecStoredProcReturnListNoParam<T>(string StoredProc)
        {
            try
            {
                con.Open();

                var res = con.Query<T>(StoredProc, commandType: CommandType.StoredProcedure);

                con.Close();

                return (List<T>)res;
            }
            catch (Exception e)
            {
                return null;
            }

        }
        public bool ExecStoredProcWithParam(DynamicParameters param, string StoredProc)
        {
            try
            {
                con.Open();

                int res = con.Execute(StoredProc, param, commandType: CommandType.StoredProcedure);

                con.Close();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }

        }
        public List<T> ExecStoredProcReturnListWithParam<T>(DynamicParameters param, string StoredProc)
        {
            try
            {
                con.Open();

                var res = con.Query<T>(StoredProc, param, commandType: CommandType.StoredProcedure);

                con.Close();

                return (List<T>)res;
            }
            catch(Exception e)
            {
                throw;
            }

        }
        public int ExecStoredProcReturnINT(string StoredProc)
        {
            try
            { 
                con.Open();

                var res = con.ExecuteScalar<int>(StoredProc, commandType: CommandType.StoredProcedure);

                con.Close();

                return (int)res;
            }
            catch (Exception ex)
            {
                return -1;
            }

        }
        #endregion
    }
}
