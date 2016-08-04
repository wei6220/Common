using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;

namespace CommonLibrary.DBUtility
{
    public interface IDbHelper
    {
        int ExecuteSql(string SQLString, List<IDbDataParameter> SqlParams);
        IList QuerySql(string SqlString, List<IDbDataParameter> SqlParams);
        IList QuerySql(string SqlString, List<IDbDataParameter> SqlParams, Type type);
    }
}