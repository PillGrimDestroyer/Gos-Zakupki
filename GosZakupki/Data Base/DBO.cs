using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;

namespace GosZakupki.Data_Base
{
    class DBO
    {
        public SqlCommand getCommand(SqlConnection connection, string commandText)
        {
            SqlCommand command = new SqlCommand(commandText, connection);
            return command;
        }

        public SqlParameter getParameter(string parameter, object value)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, value ?? DBNull.Value)
            {
                Direction = ParameterDirection.Input
            };

            return parameterObject;
        }

        public SqlParameter getParameterOut(string parameter, SqlDbType type, object value = null, ParameterDirection parameterDirection = ParameterDirection.InputOutput)
        {
            SqlParameter parameterObject = new SqlParameter(parameter, type);

            if (type == SqlDbType.NVarChar || type == SqlDbType.VarChar || type == SqlDbType.NText || type == SqlDbType.Text)
            {
                parameterObject.Size = -1;
            }

            parameterObject.Direction = parameterDirection;
            parameterObject.Value = value ?? DBNull.Value;

            return parameterObject;
        }

        public int executeNonQuery(string procedureName, List<SqlParameter> parameters)
        {
            DataBase dataBase = new DataBase();
            int returnValue = -1;

            try
            {
                using (SqlConnection connection = dataBase.getConnection())
                {
                    SqlCommand cmd = getCommand(connection, procedureName);

                    if (parameters != null && parameters.Count > 0)
                        cmd.Parameters.AddRange(parameters.ToArray());

                    returnValue = cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to ExecuteNonQuery for " + procedureName, ex);
            }

            return returnValue;
        }

        public object executeScalar(string procedureName, List<SqlParameter> parameters)
        {
            DataBase dataBase = new DataBase();
            object returnValue = null;

            try
            {
                using (SqlConnection connection = dataBase.getConnection())
                {
                    SqlCommand cmd = getCommand(connection, procedureName);

                    if (parameters != null && parameters.Count > 0)
                    {
                        cmd.Parameters.AddRange(parameters.ToArray());
                    }

                    returnValue = cmd.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to ExecuteScalar for " + procedureName, ex);
            }

            return returnValue;
        }

        public SqlDataReader getDataReader(string procedureName, List<DbParameter> parameters)
        {
            DataBase dataBase = new DataBase();
            SqlDataReader dataReader;

            try
            {
                SqlCommand cmd = getCommand(dataBase.getConnection(), procedureName);

                if (parameters != null && parameters.Count > 0)
                {
                    cmd.Parameters.AddRange(parameters.ToArray());
                }

                dataReader = cmd.ExecuteReader();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to GetDataReader for " + procedureName, ex);
            }

            return dataReader;
        }
    }
}
