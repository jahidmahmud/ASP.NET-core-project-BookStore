using Dapper;
using JMBookStore.DataAccess.Data;
using JMBookStore.Repositories.IRepository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JMBookStore.Repositories
{
    public class SP_Call : ISP_Call
    {
        private readonly ApplicationDbContext context;
        private static string connectionString = "";

        public SP_Call(ApplicationDbContext _context)
        {
            context = _context;
            connectionString = _context.Database.GetDbConnection().ConnectionString;
        }
        public void Dispose()
        {
            context.Dispose();
        }

        public void Execute(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection conn=new SqlConnection(connectionString))
            {
                conn.Open();
                conn.Execute(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<T> List<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                return conn.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
            }
        }

        public Tuple<IEnumerable<T1>, IEnumerable<T2>> List<T1, T2>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var result = SqlMapper.QueryMultiple(conn, procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
                var text1 = result.Read<T1>().ToList();
                var text2 = result.Read<T2>().ToList();
                if(text1!=null && text2 != null)
                {
                    return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(text1, text2);
                }
            }
            return new Tuple<IEnumerable<T1>, IEnumerable<T2>>(new List<T1>(), new List<T2>());
        }

        public T OneRecord<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                var value=conn.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure);
                return (T)Convert.ChangeType(value.FirstOrDefault(), typeof(T));
            }
        }

        public T single<T>(string procedureName, DynamicParameters param = null)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                return (T)Convert.ChangeType(conn.Query<T>(procedureName, param, commandType: System.Data.CommandType.StoredProcedure),typeof(T));
            }
        }
    }
}
