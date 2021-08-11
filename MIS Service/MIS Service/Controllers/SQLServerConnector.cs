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
            Builder.DataSource = "(LocalDB)\\MSSQLLocalDB";
            Builder.InitialCatalog = "OAWebBase";
            //Builder.UserID = "sa";
            //Builder.Password = "#Aupa=234";
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
            String sqlString = "SELECT * FROM aaa_file " +
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
            String sqlString = "INSERT INTO aaa_file ( " +
                                    " aaa01, aaa02, aaa03, aaa04, aaa05, " +
                                    " aaa06, aaa07, aaa08, aaa09 " +
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

                sqlCommand.Parameters.AddWithValue("@val01", postDataObjec.Aaa01);
                sqlCommand.Parameters.AddWithValue("@val02", postDataObjec.Aaa02);
                sqlCommand.Parameters.AddWithValue("@val03", postDataObjec.Aaa03);
                sqlCommand.Parameters.AddWithValue("@val04", postDataObjec.Aaa04);
                sqlCommand.Parameters.AddWithValue("@val05", postDataObjec.Aaa05);
                sqlCommand.Parameters.AddWithValue("@val06", postDataObjec.Aaa06);
                sqlCommand.Parameters.AddWithValue("@val07", postDataObjec.Aaa07);
                sqlCommand.Parameters.AddWithValue("@val08", postDataObjec.Aaa08);
                sqlCommand.Parameters.AddWithValue("@val09", postDataObjec.Aaa09);

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
            String sqlString = "SELECT * FROM aaa_file" +
                               " ORDER BY aaa01 DESC" +
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

                        postDataObject.Aaa01 = dataReader.GetString(dataReader.GetOrdinal("Aaa01"));
                        postDataObject.Aaa02 = dataReader.GetString(dataReader.GetOrdinal("Aaa02"));
                        postDataObject.Aaa03 = dataReader.GetString(dataReader.GetOrdinal("Aaa03"));
                        postDataObject.Aaa04 = dataReader.GetString(dataReader.GetOrdinal("Aaa04"));
                        postDataObject.Aaa05 = dataReader.GetString(dataReader.GetOrdinal("Aaa05"));
                        postDataObject.Aaa06 = dataReader.GetString(dataReader.GetOrdinal("Aaa06"));
                        postDataObject.Aaa07 = dataReader.GetString(dataReader.GetOrdinal("Aaa07"));
                        postDataObject.Aaa08 = dataReader.GetString(dataReader.GetOrdinal("Aaa08"));
                        postDataObject.Aaa09 = dataReader.GetString(dataReader.GetOrdinal("Aaa09"));

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
                               "       aaa01, aaa02, aaa03, aaa04, aaa05, " +
                               "       aaa06, aaa07, aaa08 " +
                               " FROM aaa_file " +
                               " ORDER BY aaa01 DESC" +
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

                        postDataObject.Aaa01 = dataReader.GetString(dataReader.GetOrdinal("Aaa01"));
                        postDataObject.Aaa02 = dataReader.GetString(dataReader.GetOrdinal("Aaa02"));
                        postDataObject.Aaa03 = dataReader.GetString(dataReader.GetOrdinal("Aaa03"));
                        postDataObject.Aaa04 = dataReader.GetString(dataReader.GetOrdinal("Aaa04"));
                        postDataObject.Aaa05 = dataReader.GetString(dataReader.GetOrdinal("Aaa05"));
                        postDataObject.Aaa06 = dataReader.GetString(dataReader.GetOrdinal("Aaa06"));
                        postDataObject.Aaa07 = dataReader.GetString(dataReader.GetOrdinal("Aaa07"));
                        postDataObject.Aaa08 = dataReader.GetString(dataReader.GetOrdinal("Aaa08"));
                        postDataObject.Aaa09 = dataReader.GetString(dataReader.GetOrdinal("Aaa09"));

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
            String sqlString = "SELECT * FROM aaa_file" +
                               " ORDER BY aaa01 DESC" +
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

                        postDataObject.Aaa01 = dataReader.GetString(dataReader.GetOrdinal("Aaa01"));
                        postDataObject.Aaa02 = dataReader.GetString(dataReader.GetOrdinal("Aaa02"));
                        postDataObject.Aaa03 = dataReader.GetString(dataReader.GetOrdinal("Aaa03"));
                        postDataObject.Aaa04 = dataReader.GetString(dataReader.GetOrdinal("Aaa04"));
                        postDataObject.Aaa05 = dataReader.GetString(dataReader.GetOrdinal("Aaa05"));
                        postDataObject.Aaa06 = dataReader.GetString(dataReader.GetOrdinal("Aaa06"));
                        postDataObject.Aaa07 = dataReader.GetString(dataReader.GetOrdinal("Aaa07"));
                        postDataObject.Aaa08 = dataReader.GetString(dataReader.GetOrdinal("Aaa08"));
                        postDataObject.Aaa09 = dataReader.GetString(dataReader.GetOrdinal("Aaa09"));

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
            String sqlString = "SELECT * FROM aaa_file WHERE " + sqlCriteria;
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

                        postDataObject.Aaa01 = dataReader.GetString(dataReader.GetOrdinal("Aaa01"));
                        postDataObject.Aaa02 = dataReader.GetString(dataReader.GetOrdinal("Aaa02"));
                        postDataObject.Aaa03 = dataReader.GetString(dataReader.GetOrdinal("Aaa03"));
                        postDataObject.Aaa04 = dataReader.GetString(dataReader.GetOrdinal("Aaa04"));
                        postDataObject.Aaa05 = dataReader.GetString(dataReader.GetOrdinal("Aaa05"));
                        postDataObject.Aaa06 = dataReader.GetString(dataReader.GetOrdinal("Aaa06"));
                        postDataObject.Aaa07 = dataReader.GetString(dataReader.GetOrdinal("Aaa07"));
                        postDataObject.Aaa08 = dataReader.GetString(dataReader.GetOrdinal("Aaa08"));
                        postDataObject.Aaa09 = dataReader.GetString(dataReader.GetOrdinal("Aaa09"));

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
            String sqlString = "DELETE FROM aaa_file WHERE aaa01 = '" + postID + "'";
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
            String sqlString = "UPDATE aaa_file SET aaa05 = @val01," +
                               "                    aaa06 = @val02," +
                               "                    aaa07 = @val03 " +
                               "WHERE aaa01 = @val04 " +
                               "";
            actionResult = "SUCCESS";
            try
            {
                OpenConnection();

                SqlCommand sqlCommand = new SqlCommand(sqlString, sqlConnection);
                sqlCommand.Parameters.AddWithValue("@val01", postDataObject.Aaa05);
                sqlCommand.Parameters.AddWithValue("@val02", postDataObject.Aaa06);
                sqlCommand.Parameters.AddWithValue("@val03", postDataObject.Aaa07);
                sqlCommand.Parameters.AddWithValue("@val04", postDataObject.Aaa01);
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

        public List<SelectListItem> getOfficeItem()
        {
            string sqlString = "SELECT * " +
                               "  FROM zzb_file " +
                               " ORDER BY zzb01 ASC" +
                               "";
            List<SelectListItem> officeDataItems = new List<SelectListItem>();

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
                        officeDataItems.Add(new SelectListItem
                        {
                            Text = dataReader["Zzb02"].ToString(),
                            Value = dataReader["Zzb01"].ToString()
                        });

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

            return officeDataItems;
        }

        private string selectaaaDataSQL;
        public String getSelectaaaDataSQL(String postID)
        {
            this.selectaaaDataSQL =
                " SELECT * FROM aaa_file WHERE aaa01 = '" + postID + "'";

            return this.selectaaaDataSQL;
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

        public int SelectHy_TicRowCounts(string key1)
        {
            OpenConnection();
            dataCount = 0;
            try
            {
                sql = " Select count (hy_tic01) from hy_tic_file " +
                    " where hy_tic01 ='" + key1 + "'" ;
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
            String sqlString = "INSERT INTO hy_tic_file ( " +
                                    " hy_tic01  , hy_tic02  , hy_tic03  , hy_tic04  , hy_tic05  , " +
                                    " hy_tic06  , hy_tic07  , hy_tic08  , hy_tic09   " +
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

                sqlCommand.Parameters.AddWithValue("@val01", postDataLog.Hy_tic01);
                sqlCommand.Parameters.AddWithValue("@val02", postDataLog.Hy_tic02);
                sqlCommand.Parameters.AddWithValue("@val03", postDataLog.Hy_tic03);
                sqlCommand.Parameters.AddWithValue("@val04", postDataLog.Hy_tic04);
                sqlCommand.Parameters.AddWithValue("@val05", postDataLog.Hy_tic05);
                sqlCommand.Parameters.AddWithValue("@val06", postDataLog.Hy_tic06);
                sqlCommand.Parameters.AddWithValue("@val07", postDataLog.Hy_tic07);
                sqlCommand.Parameters.AddWithValue("@val08", postDataLog.Hy_tic08);
                sqlCommand.Parameters.AddWithValue("@val09", postDataLog.Hy_tic09);

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
            String sqlString = "SELECT * FROM hy_tic_file" +
                               " ORDER BY hy_tic01 DESC" +
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

                        postDataLog.Hy_tic01 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic01"));
                        postDataLog.Hy_tic02 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic02"));
                        postDataLog.Hy_tic03 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic03"));
                        postDataLog.Hy_tic04 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic04"));
                        postDataLog.Hy_tic05 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic05"));
                        postDataLog.Hy_tic06 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic06"));
                        postDataLog.Hy_tic07 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic07"));
                        postDataLog.Hy_tic08 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic08"));
                        postDataLog.Hy_tic09 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic09"));

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
            String sqlString = "SELECT * FROM hy_tic_file WHERE " + sqlCriteria;
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

                        postDataLog.Hy_tic01 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic01"));
                        postDataLog.Hy_tic02 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic02"));
                        postDataLog.Hy_tic03 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic03"));
                        postDataLog.Hy_tic04 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic04"));
                        postDataLog.Hy_tic05 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic05"));
                        postDataLog.Hy_tic06 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic06"));
                        postDataLog.Hy_tic07 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic07"));
                        postDataLog.Hy_tic08 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic08"));
                        postDataLog.Hy_tic09 = dataReader.GetString(dataReader.GetOrdinal("Hy_tic09"));

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