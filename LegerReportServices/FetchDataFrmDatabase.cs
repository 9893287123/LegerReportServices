using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegerReportServices
{
    public class FetchDataFrmDatabase:ConnectionDA
    {
        EventLog ev1 = new EventLog();
        public DataTable GetAllActiveUser()
        {
            // ev1.Source = "DMTLongPendingRecheck";
            //  ev1.WriteEntry("InData fetch");
            //Abhishek
            try
            {
                using (DataTable DT = new DataTable())
                {
                    List<UserModel> DisList = new List<UserModel>();

                    using (SqlConnection connect = new SqlConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "[cli].[TestingDatabase]";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 1000;
                            cmd.Parameters.AddWithValue("@Type", "ActivateUserList");
                            connect.ConnectionString = connectionString;
                            cmd.Connection = connect;
                            if (cmd.Connection.State != ConnectionState.Open)
                            {
                                cmd.Connection.Open();
                            }
                            SqlDataReader DR = cmd.ExecuteReader();
                            DT.Load(DR);
                            cmd.Connection.Close();

                        }
                    }
                    if (DT != null)
                    {
                        return DT;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public DataTable GetUserTransactionPreviesDate(string Atishayevendorid)
        {
            // ev1.Source = "DMTLongPendingRecheck";
            //   ev1.WriteEntry("InData fetch");
            //Abhishek
            try
            {
                using (DataTable DT = new DataTable())
                {
                    List<UserTransactionModel> DisList = new List<UserTransactionModel>();

                    using (SqlConnection connect = new SqlConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "[dbo].[USP_GetLedgerDetailsForMess]";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 1000;
                            cmd.Parameters.AddWithValue("@AtishayVendorID", Atishayevendorid);
                            connect.ConnectionString = connectionString;
                            cmd.Connection = connect;
                            if (cmd.Connection.State != ConnectionState.Open)
                            {
                                cmd.Connection.Open();
                            }
                            SqlDataReader DR = cmd.ExecuteReader();
                            DT.Load(DR);
                            cmd.Connection.Close();

                        }
                    }
                    if (DT != null)
                    {
                        return DT;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }
        public DataTable GetRechargeDetails(string Atishayevendorid, string Type)
        {
            try
            {
                using (DataTable DT = new DataTable())
                {
                    List<UserTransactionModel> DisList = new List<UserTransactionModel>();

                    using (SqlConnection connect = new SqlConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = "[dbo].[GetRechargeDetail_Preday]";
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.CommandTimeout = 1000;
                            cmd.Parameters.AddWithValue("@VendorId", Atishayevendorid);
                            cmd.Parameters.AddWithValue("@Type", Type);
                            connect.ConnectionString = connectionString;
                            cmd.Connection = connect;
                            if (cmd.Connection.State != ConnectionState.Open)
                            {
                                cmd.Connection.Open();
                            }
                            SqlDataReader DR = cmd.ExecuteReader();
                            DT.Load(DR);
                            cmd.Connection.Close();

                        }
                    }
                    if (DT != null)
                    {
                        return DT;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {

                return null;
            }
        }

    }
}
