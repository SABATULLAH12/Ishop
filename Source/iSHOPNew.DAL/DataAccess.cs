using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSHOPNew.DAL
{
    public class DataAccess : BaseDal
    {
        SqlDatabase database = null;
        DbCommand sqlCmd = null;
        DataSet dsData = null;

        public DataSet GetData(object[] objParameters, string[] objParametersId, SqlDbType[] paramType, string spName, out Exception excp)
        {
            dsData = new DataSet();
            excp = null;
            database = new SqlDatabase(this.ISHOP_ConnectionString_WithIdMapping);
            sqlCmd = database.DbProviderFactory.CreateCommand();
            using (sqlCmd = database.GetStoredProcCommand(spName))
            {
                for(int i = 0; i < objParameters.Length; i++)
                {
                    database.AddInParameter(sqlCmd, objParametersId[i], paramType[i], objParameters[i]);
                }

                sqlCmd.CommandTimeout = 1200000;

                try
                {
                    dsData = database.ExecuteDataSet(sqlCmd);
                }
                catch (Exception ex)
                {
                    dsData = null;
                    excp = ex;
                    /*throw ex;*/
                }
                return dsData;
            }
        }
        public DataSet GetData_WithIdMapping(object[] objParameters, string strSPName)
        {
            //DataSet ds;
            dsData = new DataSet();
            database = new SqlDatabase(this.ISHOP_ConnectionString_WithIdMapping);
            sqlCmd = database.DbProviderFactory.CreateCommand();
            InsertSelection(objParameters, strSPName);
            sqlCmd = database.GetStoredProcCommand(strSPName, objParameters);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandTimeout = 7200;

            try
            {
                dsData = database.ExecuteDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }
            return dsData;
        }
        private void InsertSelection(object[] objParameters, string strSPName)
        {
            string selection = string.Empty;
            try
            {
                if (objParameters != null && objParameters.Count() > 0)
                    selection = string.Join("','", objParameters);

                object[] obj = new object[] { "'" + selection + "'", strSPName };
                SaveData(obj, SPVariables.USP_Ishop_executionline);
            }
            catch (Exception ex)
            {

            }
        }
        public DataSet GetData(object[] objParameters, string strSPName)
        {
            //DataSet ds;
            dsData = new DataSet();
            //database = new SqlDatabase(this.ISHOP_Old_ConnectionString);
            database = new SqlDatabase(this.ISHOP_ConnectionString_WithIdMapping);
            sqlCmd = database.DbProviderFactory.CreateCommand();
            InsertSelection(objParameters, strSPName);
            sqlCmd = database.GetStoredProcCommand(strSPName, objParameters);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            sqlCmd.CommandTimeout = 7200;

            try
            {
                //InsertSelection(objParameters, strSPName);
                dsData = database.ExecuteDataSet(sqlCmd);
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }
            return dsData;
        }
        public DataSet GetData(string strSPName)
        {
            dsData = new DataSet();
            database = new SqlDatabase(this.ConnectionString);
            try
            {                
                dsData = database.ExecuteDataSet(strSPName);
            }
            catch (Exception ex)
            {
                dsData = null;
                throw ex;
            }
            return dsData;
        }
        public int SaveData(object[] objParameters, string strSPName)
        {
            SqlDatabase database = new SqlDatabase(this.ISHOP_ConnectionString_WithIdMapping);
            return database.ExecuteNonQuery(strSPName, objParameters);
        }
      
    }
}