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
        SMSBO ObjBo = new SMSBO();
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


        public string GetparameterDefaultvalue(string Paramname)
        {
            string paramvalue = "0";
            DataSet ds = new DataSet();
            using (SqlConnection connect = new SqlConnection())
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.CommandText = "mst.AddDefaultParameter";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Type", "Defaultvalue");
                    cmd.Parameters.AddWithValue("@ParamName", Paramname);
                    connect.ConnectionString = connectionString;
                    cmd.Connection = connect;
                    if (connect.State != ConnectionState.Open)
                    {
                        connect.Open();
                    }
                    SqlDataAdapter adp = new SqlDataAdapter(cmd);
                    adp.Fill(ds);
                    connect.Close();
                }
            }
            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count > 0)
                {
                    paramvalue = ds.Tables[0].Rows[0]["paramvalue"].ToString();
                }
            }
            return paramvalue;
        }

        public SMSBO GETsmsExecutionDetails(string Type)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetSMSExecutionDetails";
                        cmd.Parameters.AddWithValue("@Type", Type);
                        conn.ConnectionString = connectionString;
                        cmd.Connection = conn;
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }
                        SqlDataReader DR = cmd.ExecuteReader();

                        if (DR != null)
                        {
                            if (DR.HasRows)
                            {
                                while (DR.Read())
                                {
                                    ObjBo.IsSMSExecuted = Convert.ToInt32(DR["IsSMSExecuted"].ToString());
                                    ObjBo.SMSExecutionTime = Convert.ToDateTime(DR["SMSExecutionTime"].ToString());
                                }

                            }
                        }
                    }

                }
            }

            catch (Exception e)
            {
                return null;
            }
            return ObjBo;
        }

        public SMSBO SaveExecutionDetails(int IsSMSExecuted, string AddType)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection())
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "SaveSMSExecutionDetails";
                        cmd.Parameters.AddWithValue("@IsSMSExecuted", IsSMSExecuted);
                        cmd.Parameters.AddWithValue("@Type", AddType);

                        conn.ConnectionString = connectionString;
                        cmd.Connection = conn;
                        if (cmd.Connection.State != ConnectionState.Open)
                        {
                            cmd.Connection.Open();
                        }
                        cmd.ExecuteScalar();
                        ObjBo.IsSMSExecuted = 1;
                    }

                }
            }

            catch (Exception e)
            {
                return null;
            }
            return ObjBo;
        }

    }
}
