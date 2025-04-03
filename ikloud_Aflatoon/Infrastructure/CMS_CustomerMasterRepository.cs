using ikloud_Aflatoon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class CMS_CustomerMasterRepository
{
    string cs = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
    private SqlConnection con;
    //To Handle connection related activities    
    private void connection()
    {
        string constr = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ToString();
        con = new SqlConnection(constr);

    }

    public bool AddCustomerMasterData(CMS_CustomerMaster_DBS_View obj, Int64 loginId)
    {
        try
        {
            connection();
            SqlCommand com = new SqlCommand("CMS_Add_CustomerMasterData", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@CustomerCode", obj.CustomerCode);
            com.Parameters.AddWithValue("@CustomerName", obj.CustomerName);
            com.Parameters.AddWithValue("@CustomerAccountNo", obj.CustomerAccountNo);
            com.Parameters.AddWithValue("@IsActive", obj.IsActive == true ? 1 : 0);
            com.Parameters.AddWithValue("@CreatedBy", loginId);

            con.Open();
            int i = com.ExecuteNonQuery();
            con.Close();
            if (i >= 1)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        catch (Exception e)
        {
            string message = "";
            string innerExcp = "";
            if (e.Message != null)
                message = e.Message.ToString();
            if (e.InnerException != null)
                innerExcp = e.InnerException.Message;
            return false;
            //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Customer Add Post - " + innerExcp });
        }

    }
    //To view customer details with generic list     
    public List<CMS_CustomerMaster_DBS_View> GetAllCustomerMasterData()
    {
        connection();
        List<CMS_CustomerMaster_DBS_View> CustList = new List<CMS_CustomerMaster_DBS_View>();


        SqlCommand com = new SqlCommand("CMS_Get_CustomerMasterData_All", con);
        com.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(com);
        DataTable dt = new DataTable();

        con.Open();
        da.Fill(dt);
        con.Close();
        //Bind CustModel generic list using dataRow     
        foreach (DataRow dr in dt.Rows)
        {

            CustList.Add(

                new CMS_CustomerMaster_DBS_View
                {

                    ID = Convert.ToInt64(dr["ID"]),
                    CustomerCode = Convert.ToString(dr["CustomerCode"]),
                    CustomerName = Convert.ToString(dr["CustomerName"]),
                    CustomerAccountNo = Convert.ToString(dr["CustomerAccountNo"]),
                    IsActive = Convert.ToBoolean(dr["IsActive"])

                }
                );
        }

        return CustList;
    }
    //To Update Customer details    
    public bool UpdateCustomerMasterData(CMS_CustomerMaster_DBS_View obj, Int64 loginId)
    {

        connection();
        SqlCommand com = new SqlCommand("CMS_Update_CustomerMasterData", con);

        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@ID", obj.ID);
        com.Parameters.AddWithValue("@CustomerCode", obj.CustomerCode);
        com.Parameters.AddWithValue("@CustomerName", obj.CustomerName);
        com.Parameters.AddWithValue("@CustomerAccountNo", obj.CustomerAccountNo);
        com.Parameters.AddWithValue("@IsActive", obj.IsActive == true ? 1 : 0);
        com.Parameters.AddWithValue("@ModifiedBy", loginId);
        con.Open();
        int i = com.ExecuteNonQuery();
        con.Close();
        if (i >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    //To delete Customer details    
    public bool DeleteCustomerMasterData(Int64 Id)
    {

        connection();
        SqlCommand com = new SqlCommand("CMS_Delete_CustomerMasterData", con);

        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@ID", Id);

        con.Open();
        int i = com.ExecuteNonQuery();
        con.Close();
        if (i >= 1)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //To get Customer details by id
    public CMS_CustomerMaster_DBS_View GetCustomerMasterDataById(Int64 Id)
    {

        connection();
        SqlCommand com = new SqlCommand("CMS_Get_CustomerMasterData_ById", con);

        com.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(com);
        DataTable dt = new DataTable();

        con.Open();
        da.Fill(dt);
        con.Close();

        CMS_CustomerMaster_DBS_View obj = new CMS_CustomerMaster_DBS_View();

        if (dt.Rows.Count > 0)
        {
            obj = new CMS_CustomerMaster_DBS_View
            {

                ID = Convert.ToInt64(dt.Rows[0]["ID"]),
                CustomerCode = Convert.ToString(dt.Rows[0]["CustomerCode"]),
                CustomerName = Convert.ToString(dt.Rows[0]["CustomerName"]),
                CustomerAccountNo = Convert.ToString(dt.Rows[0]["CustomerAccountNo"]),
                IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"])

            };
        }

        return obj;
    }
}