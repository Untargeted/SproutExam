using Dapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Sprout.Exam.DataAccess.Interface
{
    public interface IDapperClass
    {
        bool ExecStoredProcNoParam(string StoredProc);
        List<T> ExecStoredProcReturnListNoParam<T>(string StoredProc);
        bool ExecStoredProcWithParam(DynamicParameters param, string StoredProc);
        List<T> ExecStoredProcReturnListWithParam<T>(DynamicParameters param, string StoredProc);
        int ExecStoredProcReturnINT(string StoredProc);
    }
}
