using MIS_Service.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace MIS_Service.Controllers
{
    public class SQLServerConnector : ApiController
    {
        private SqlConnection sqlConnection;
        private string actionResult;
        int dataCount;
        string sql;
        public SQLServerConnector()
        {
            Initializer();
        }

        private void Initializer()
        {
            SqlConnectionStringBuilder Builder = new SqlConnectionStringBuilder();
            Builder.DataSource = "AUPA-SERVER207\\MSSQLSERVER207";
            Builder.InitialCatalog = "OAWebBase";
            Builder.UserID = "sa";
            Builder.Password = "#Aupa=234";
            String sqlConnectionString = Builder.ConnectionString;
            sqlConnection = new SqlConnection(sqlConnectionString);
        }
        private bool OpenConnection()
        {
            try
            {
                sqlConnection.Open();
                return true;
            }
            catch (SqlException ex)
            {
                switch (ex.Number)
                {
                    case 0:
                        //MessageBox.Show("Cannot connect to server.  Contact administrator");
                        break;
                    case 53:
                        break;
                    case 1045:
                        //MessageBox.Show("Invalid username/password, please try again");
                        break;
                }
                return false;
            }
        }
        private bool CloseConnection()
        {
            try
            {
                sqlConnection.Close();
                return true;
            }
            catch (SqlException ex)
            {
                //MessageBox.Show(ex.Message);
                Console.Write("MySqlException :" + ex.Message);
                return false;
            }
        }

        public int GetTotalCount()
        {
            String sqlString = "SELECT * FROM tic_file " +
                               "";
            OpenConnection();
            actionResult = "SUCCESS";
            int rowcount = 0;
            try
            {
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                rowcount = dataTable.Rows.Count;
            }
            catch (Exception ex)
            {
                string v = "FAIL" + ex.Message;
                actionResult = v;
            }
            finally
            {
                CloseConnection();
            }
            return rowcount;
        }

        public int GetTotalLogCount()
        {
            String sqlString = "SELECT * FROM tig_file " +
                               "";
            OpenConnection();
            actionResult = "SUCCESS";
            int rowcount = 0;
            try
            {
                DataTable dataTable = new DataTable();
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                sqlDataAdapter.Fill(dataTable);
                rowcount = dataTable.Rows.Count;
            }
            catch (Exception ex)
            {
                string v = "FAIL" + ex.Message;
                actionResult = v;
            }
            finally
            {
                CloseConnection();
            }
            return rowcount;
        }

        public String InsertPostData(PostDataObject postDataObjec)
        {
            String sqlString = "INSERT INTO tic_file ( " +
                                    " tic01, tic02, tic03, tic04, tic05, " +
                                    " tic06, tic07, tic08, tic09 " +
                                    ") VALUES ( " +
                                    " @val01, @val02, @val03, @val04, @val05, @val06, " +
                                    " @val07, @val08, @val09                                  " +
                                    ")";
            OpenConnection();
            actionResult = "SUCCESS";

            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                sqlCommand.Parameters.AddWithValue("@val01", postDataObjec.Tic01);
                sqlCommand.Parameters.AddWithValue("@val02", postDataObjec.Tic02);
                sqlCommand.Parameters.AddWithValue("@val03", postDataObjec.Tic03);
                sqlCommand.Parameters.AddWithValue("@val04", postDataObjec.Tic04);
                sqlCommand.Parameters.AddWithValue("@val05", postDataObjec.Tic05);
                sqlCommand.Parameters.AddWithValue("@val06", postDataObjec.Tic06);
                sqlCommand.Parameters.AddWithValue("@val07", postDataObjec.Tic07);
                sqlCommand.Parameters.AddWithValue("@val08", postDataObjec.Tic08);
                sqlCommand.Parameters.AddWithValue("@val09", postDataObjec.Tic09);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                actionResult = "FAIL" + ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return actionResult;
        }//End of Insert Into

        public List<PostDataObject> GetPostsList()
        {
            String sqlString = "SELECT * FROM tic_file" +
                               " ORDER BY tic01 DESC" +
                               "";
            List<PostDataObject> postsList = new List<PostDataObject>();

            OpenConnection();
            actionResult = "SUCCESS";

            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        PostDataObject postDataObject = new PostDataObject();

                        postDataObject.Tic01 = dataReader.GetString(dataReader.GetOrdinal("Tic01"));
                        postDataObject.Tic02 = dataReader.GetString(dataReader.GetOrdinal("Tic02"));
                        postDataObject.Tic03 = dataReader.GetString(dataReader.GetOrdinal("Tic03"));
                        postDataObject.Tic04 = dataReader.GetString(dataReader.GetOrdinal("Tic04"));
                        postDataObject.Tic05 = dataReader.GetString(dataReader.GetOrdinal("Tic05"));
                        postDataObject.Tic06 = dataReader.GetString(dataReader.GetOrdinal("Tic06"));
                        postDataObject.Tic07 = dataReader.GetString(dataReader.GetOrdinal("Tic07"));
                        postDataObject.Tic08 = dataReader.GetString(dataReader.GetOrdinal("Tic08"));
                        postDataObject.Tic09 = dataReader.GetString(dataReader.GetOrdinal("Tic09"));

                        postsList.Add(postDataObject);
                    }
                }
            }
            catch (Exception ex)
            {
                string v = "FAIL" + ex.Message;
                actionResult = v;
            }
            finally
            {
                CloseConnection();
            }

            return postsList;
        }//End of getPostsList

        public List<PostDataObject> GetTopPostsList(int num)
        {
            String sqlString = "SELECT TOP " + num +
                               "       tic01, tic02, tic03, tic04, tic05, " +
                               "       tic06, tic07, tic08 " +
                               " FROM tic_file " +
                               " ORDER BY tic01 DESC" +
                               "";
            List<PostDataObject> postsList = new List<PostDataObject>();

            OpenConnection();
            actionResult = "SUCCESS";

            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        PostDataObject postDataObject = new PostDataObject();

                        postDataObject.Tic01 = dataReader.GetString(dataReader.GetOrdinal("Tic01"));
                        postDataObject.Tic02 = dataReader.GetString(dataReader.GetOrdinal("Tic02"));
                        postDataObject.Tic03 = dataReader.GetString(dataReader.GetOrdinal("Tic03"));
                        postDataObject.Tic04 = dataReader.GetString(dataReader.GetOrdinal("Tic04"));
                        postDataObject.Tic05 = dataReader.GetString(dataReader.GetOrdinal("Tic05"));
                        postDataObject.Tic06 = dataReader.GetString(dataReader.GetOrdinal("Tic06"));
                        postDataObject.Tic07 = dataReader.GetString(dataReader.GetOrdinal("Tic07"));
                        postDataObject.Tic08 = dataReader.GetString(dataReader.GetOrdinal("Tic08"));
                        postDataObject.Tic09 = dataReader.GetString(dataReader.GetOrdinal("Tic09"));

                        postsList.Add(postDataObject);
                    }
                }
            }
            catch (Exception ex)
            {
                string v = "FAIL" + ex.Message;
                actionResult = v;
            }
            finally
            {
                CloseConnection();
            }

            return postsList;
        }//End of getTOPPostsList

        public List<PostDataObject> GetLimitPostsList(String str, String end)
        {
            String sqlString = "SELECT * FROM tic_file" +
                               " ORDER BY tic01 DESC" +
                               " OFFSET " + str + " ROWS FETCH NEXT " + end + " ROWS ONLY " +
                               "";
            List<PostDataObject> postsList = new List<PostDataObject>();

            OpenConnection();
            actionResult = "SUCCESS";

            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        PostDataObject postDataObject = new PostDataObject();

                        postDataObject.Tic01 = dataReader.GetString(dataReader.GetOrdinal("Tic01"));
                        postDataObject.Tic02 = dataReader.GetString(dataReader.GetOrdinal("Tic02"));
                        postDataObject.Tic03 = dataReader.GetString(dataReader.GetOrdinal("Tic03"));
                        postDataObject.Tic04 = dataReader.GetString(dataReader.GetOrdinal("Tic04"));
                        postDataObject.Tic05 = dataReader.GetString(dataReader.GetOrdinal("Tic05"));
                        postDataObject.Tic06 = dataReader.GetString(dataReader.GetOrdinal("Tic06"));
                        postDataObject.Tic07 = dataReader.GetString(dataReader.GetOrdinal("Tic07"));
                        postDataObject.Tic08 = dataReader.GetString(dataReader.GetOrdinal("Tic08"));
                        postDataObject.Tic09 = dataReader.GetString(dataReader.GetOrdinal("Tic09"));

                        postsList.Add(postDataObject);
                    }
                }
            }
            catch (Exception ex)
            {
                string v = "FAIL" + ex.Message;
                actionResult = v;
            }
            finally
            {
                CloseConnection();
            }

            return postsList;
        }//End of getLimitPostsList

      

        public List<PostDataObject> GetPostsListOnDemand(String sqlCriteria)
        {
            String sqlString = "SELECT * FROM tic_file WHERE " + sqlCriteria;
            List<PostDataObject> postsList = new List<PostDataObject>();

            OpenConnection();
            actionResult = "SUCCESS";
            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        PostDataObject postDataObject = new PostDataObject();

                        postDataObject.Tic01 = dataReader.GetString(dataReader.GetOrdinal("Tic01"));
                        postDataObject.Tic02 = dataReader.GetString(dataReader.GetOrdinal("Tic02"));
                        postDataObject.Tic03 = dataReader.GetString(dataReader.GetOrdinal("Tic03"));
                        postDataObject.Tic04 = dataReader.GetString(dataReader.GetOrdinal("Tic04"));
                        postDataObject.Tic05 = dataReader.GetString(dataReader.GetOrdinal("Tic05"));
                        postDataObject.Tic06 = dataReader.GetString(dataReader.GetOrdinal("Tic06"));
                        postDataObject.Tic07 = dataReader.GetString(dataReader.GetOrdinal("Tic07"));
                        postDataObject.Tic08 = dataReader.GetString(dataReader.GetOrdinal("Tic08"));
                        postDataObject.Tic09 = dataReader.GetString(dataReader.GetOrdinal("Tic09"));

                        postsList.Add(postDataObject);
                    }
                }
            }
            catch (Exception ex)
            {
                actionResult = "FAIL" + ex.Message;
            }
            finally
            {
                CloseConnection();
            }

            return postsList;
        }

        public String ConfirmedDelete(String postID)
        {
            String sqlString = "DELETE FROM tic_file WHERE tic01 = '" + postID + "'";
            int deletedRows;
            actionResult = "SUCCESS";
            try
            {
                OpenConnection();

                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                deletedRows = sqlCommand.ExecuteNonQuery();
                if (deletedRows == 0)
                {
                    actionResult = "FAIL";
                }
            }
            catch (Exception ex)
            {
                actionResult = "FAIL" + ex.Message;
            }
            finally
            {
                CloseConnection();
            }

            return actionResult;
        }

        public String ConfirmedEdit(PostDataObject postDataObject)
        {
            String sqlString = "UPDATE tic_file SET tic05 = @val01," +
                               "                    tic06 = @val02," +
                               "                    tic07 = @val03 " +
                               "WHERE tic01 = @val04 " +
                               "";
            actionResult = "SUCCESS";
            try
            {
                OpenConnection();

                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@val01", postDataObject.Tic05);
                sqlCommand.Parameters.AddWithValue("@val02", postDataObject.Tic06);
                sqlCommand.Parameters.AddWithValue("@val03", postDataObject.Tic07);
                sqlCommand.Parameters.AddWithValue("@val04", postDataObject.Tic01);
                sqlCommand.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                actionResult = "Fail" + ex.Message;
            }
            finally
            {
                CloseConnection();
            }


            return actionResult;
        }
     

        private string selectticDataSQL;
        public String getSelectticDataSQL(String postID)
        {
            this.selectticDataSQL =
                " SELECT * FROM tic_file WHERE tic01 = '" + postID + "'";

            return this.selectticDataSQL;
        }

        public DataTable GetDataTable(String sql)
        {
            DataTable dataTable = null;
            OpenConnection();
            try
            {
                SqlCommand sqlCommand = new SqlCommand(sql, this.sqlConnection);
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sql;
                sqlCommand.CommandType = CommandType.Text;

                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                dataTable = new DataTable();
                dataTable.Load(sqlDataReader);
            }
            catch(Exception ex)
            {
                Console.WriteLine("SQLConductor Exception " + ex.Message);
            }
            finally
            {
                CloseConnection();
            }
            return dataTable;
        }

        public int SelectTigRowCounts(string key1)
        {
            OpenConnection();
            dataCount = 0;
            try
            {
                sql = " Select count (tig01) from tig_file " +
                    " where tig01 ='" + key1 + "'" ;
                SqlCommand sqlCommand = new SqlCommand(sql, sqlConnection);
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sql;
                dataCount = Convert.ToInt16(sqlCommand.ExecuteScalar());
                if(dataCount == -1)
                {
                    dataCount = 0;
                }    

            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLServer Data Secure Error : " + ex.Message);
                sqlConnection.Close();
            }
            finally
            {
                sqlConnection.Close();
            }
            return dataCount;
        }

        public String InsertPostlog(PostDataLog postDataLog)
        {
            String sqlString = "INSERT INTO tig_file ( " +
                                    " tig01  , tig02  , tig03  , tig04  , tig05  , " +
                                    " tig06  , tig07  , tig08  , tig09   " +
                                    ") VALUES ( " +
                                    " @val01, @val02, @val03, @val04, @val05, @val06, " +
                                    " @val07, @val08, @val09                                  " +
                                    ")";
            OpenConnection();
            actionResult = "SUCCESS";

            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                sqlCommand.Parameters.AddWithValue("@val01", postDataLog.Tig01);
                sqlCommand.Parameters.AddWithValue("@val02", postDataLog.Tig02);
                sqlCommand.Parameters.AddWithValue("@val03", postDataLog.Tig03);
                sqlCommand.Parameters.AddWithValue("@val04", postDataLog.Tig04);
                sqlCommand.Parameters.AddWithValue("@val05", postDataLog.Tig05);
                sqlCommand.Parameters.AddWithValue("@val06", postDataLog.Tig06);
                sqlCommand.Parameters.AddWithValue("@val07", postDataLog.Tig07);
                sqlCommand.Parameters.AddWithValue("@val08", postDataLog.Tig08);
                sqlCommand.Parameters.AddWithValue("@val09", postDataLog.Tig09);

                sqlCommand.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                actionResult = "FAIL" + ex.Message;
            }
            finally
            {
                CloseConnection();
            }
            return actionResult;
        }//End of Insert Into

        public List<PostDataLog> GetLimitLogsList(String str, String end)
        {
            String sqlString = "SELECT * FROM tig_file" +
                               " ORDER BY tig01 DESC" +
                               " OFFSET " + str + " ROWS FETCH NEXT " + end + " ROWS ONLY " +
                               "";
            List<PostDataLog> postsList = new List<PostDataLog>();

            OpenConnection();
            actionResult = "SUCCESS";

            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        PostDataLog postDataLog = new PostDataLog();

                        postDataLog.Tig01 = dataReader.GetString(dataReader.GetOrdinal("tig01"));
                        postDataLog.Tig02 = dataReader.GetString(dataReader.GetOrdinal("tig02"));
                        postDataLog.Tig03 = dataReader.GetString(dataReader.GetOrdinal("tig03"));
                        postDataLog.Tig04 = dataReader.GetString(dataReader.GetOrdinal("tig04"));
                        postDataLog.Tig05 = dataReader.GetString(dataReader.GetOrdinal("tig05"));
                        postDataLog.Tig06 = dataReader.GetString(dataReader.GetOrdinal("tig06"));
                        postDataLog.Tig07 = dataReader.GetString(dataReader.GetOrdinal("tig07"));
                        postDataLog.Tig08 = dataReader.GetString(dataReader.GetOrdinal("tig08"));
                        postDataLog.Tig09 = dataReader.GetString(dataReader.GetOrdinal("tig09"));

                        postsList.Add(postDataLog);
                    }
                }
            }
            catch (Exception ex)
            {
                string v = "FAIL" + ex.Message;
                actionResult = v;
            }
            finally
            {
                CloseConnection();
            }

            return postsList;
        }//End of getLimitPostsList


        public List<PostDataLog> GetPostsLogOnDemand(String sqlCriteria)
        {
            String sqlString = "SELECT * FROM tig_file WHERE " + sqlCriteria;
            List<PostDataLog> postsLog = new List<PostDataLog>();

            OpenConnection();
            actionResult = "SUCCESS";
            try
            {
                SqlCommand sqlCommand = sqlConnection.CreateCommand();

                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = sqlString;

                SqlDataReader dataReader = sqlCommand.ExecuteReader();
                if (dataReader.HasRows)
                {
                    while (dataReader.Read())
                    {
                        PostDataLog postDataLog = new PostDataLog();

                        postDataLog.Tig01 = dataReader.GetString(dataReader.GetOrdinal("tig01"));
                        postDataLog.Tig02 = dataReader.GetString(dataReader.GetOrdinal("tig02"));
                        postDataLog.Tig03 = dataReader.GetString(dataReader.GetOrdinal("tig03"));
                        postDataLog.Tig04 = dataReader.GetString(dataReader.GetOrdinal("tig04"));
                        postDataLog.Tig05 = dataReader.GetString(dataReader.GetOrdinal("tig05"));
                        postDataLog.Tig06 = dataReader.GetString(dataReader.GetOrdinal("tig06"));
                        postDataLog.Tig07 = dataReader.GetString(dataReader.GetOrdinal("tig07"));
                        postDataLog.Tig08 = dataReader.GetString(dataReader.GetOrdinal("tig08"));
                        postDataLog.Tig09 = dataReader.GetString(dataReader.GetOrdinal("tig09"));

                        postsLog.Add(postDataLog);
                    }
                }
            }
            catch (Exception ex)
            {
                actionResult = "FAIL" + ex.Message;
            }
            finally
            {
                CloseConnection();
            }

            return postsLog;
        }
    }
}