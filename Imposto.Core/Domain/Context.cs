using Imposto.Core.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Imposto.Core.Domain
{
    public sealed class Context : IDisposable
    {
        private readonly SqlConnection adoConection;

        public Context()
        {
            try
            {
                string conectionString = String.Empty;
                conectionString = ConfigurationManager.ConnectionStrings["TesteConfig"].ConnectionString;
                adoConection = new SqlConnection(conectionString);
                adoConection.Open();
            }
            catch (System.Exception)
            {
                adoConection.Close();
                throw;
            }
        }

        public void ExecuteCommand(string query)
        {
            var cmdCommand = new SqlCommand()
            {
                CommandText = query,
                CommandType = System.Data.CommandType.Text,
                Connection = adoConection
            };

            cmdCommand.ExecuteNonQuery();

            return;
        }

        public Info ExecuteCommand(string nameProcedure, List<SqlParameter> parameters)
        {
            Info retorno = new Info();

            var cmdCommand = new SqlCommand(nameProcedure)
                {
                    CommandType = CommandType.StoredProcedure,
                    Connection = adoConection
                };

            foreach (var itemParameter in parameters)
            {
                cmdCommand.Parameters.Add(itemParameter);
            }
            SqlParameter returnValue = new SqlParameter();
            returnValue.Direction = ParameterDirection.ReturnValue;
            cmdCommand.Parameters.Add(returnValue);

            cmdCommand.ExecuteNonQuery();
            retorno.Codigo = returnValue.Value is DBNull ? 0 : (int)returnValue.Value;
         
            return retorno;
        }

        public void ExecuteComandWithReturn(string query)
        {
            SqlDataReader sqldtrd;

            var cmdCommand = new SqlCommand(query, adoConection);
            sqldtrd = cmdCommand.ExecuteReader();
            
            return;
        }
        
        public void Dispose()
        {
            if (adoConection.State == System.Data.ConnectionState.Open)
            {
                adoConection.Close();
            }
            GC.Collect();
        }
    }
}
