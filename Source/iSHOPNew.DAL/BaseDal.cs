using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iSHOPNew.DAL
{
    public class BaseDal
    {
        //DefaultConnection
        private string connectionString = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Sql_Connectionstring"]].ConnectionString;
        //private string ishop_old_connectionString = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Sql_Connectionstring_Old"]].ConnectionString;
        private string ishop_connectionString_WithId = ConfigurationManager.ConnectionStrings[ConfigurationManager.AppSettings["Sql_Connectionstring_IdMapping"]].ConnectionString;

        /// <summary>
        /// get or Set the database connection string
        /// </summary>
        protected string ConnectionString
        {
            get
            {
                return this.connectionString;
            }
            set
            {
                this.connectionString = value;
            }
        }
        //protected string ISHOP_Old_ConnectionString
        //{
        //    get
        //    {
        //        return this.ishop_old_connectionString;
        //    }
        //    set
        //    {
        //        this.ishop_old_connectionString = value;
        //    }
        //}
        protected string ISHOP_ConnectionString_WithIdMapping
        {
            get
            {
                return this.ishop_connectionString_WithId;
            }
            set
            {
                this.ishop_connectionString_WithId = value;
            }
        }
    }
}