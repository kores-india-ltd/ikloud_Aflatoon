using ikloud_Aflatoon;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

public class CMS_LocationMasterRepository
{
    string cs = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ConnectionString;
    private SqlConnection con;
    //To Handle connection related activities    
    private void connection()
    {
        string constr = ConfigurationManager.ConnectionStrings["iKloudProConnectionString2"].ToString();
        con = new SqlConnection(constr);

    }

    public bool AddLocationMasterData(CMS_LocationMaster_DBS_View obj, Int64 loginId)
    {
        try
        {
            connection();
            SqlCommand com = new SqlCommand("CMS_Add_LocationMasterData", con);
            com.CommandType = CommandType.StoredProcedure;
            com.Parameters.AddWithValue("@LocationCode", obj.LocationCode);
            com.Parameters.AddWithValue("@LocationName", obj.LocationName);
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
            //return RedirectToAction("Error", "Error", new { msg = message, popmsg = "CMS_Location Add Post - " + innerExcp });
        }

    }
    //To view location details with generic list     
    public List<CMS_LocationMaster_DBS_View> GetAllLocationMasterData()
    {
        connection();
        List<CMS_LocationMaster_DBS_View> LocList = new List<CMS_LocationMaster_DBS_View>();


        SqlCommand com = new SqlCommand("CMS_Get_LocationMasterData_All", con);
        com.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(com);
        DataTable dt = new DataTable();

        con.Open();
        da.Fill(dt);
        con.Close();
        //Bind LocModel generic list using dataRow     
        foreach (DataRow dr in dt.Rows)
        {

            LocList.Add(

                new CMS_LocationMaster_DBS_View
                {

                    ID = Convert.ToInt64(dr["ID"]),
                    LocationCode = Convert.ToString(dr["LocationCode"]),
                    LocationName = Convert.ToString(dr["LocationName"]),
                    IsActive = Convert.ToBoolean(dr["IsActive"])

                }
                );
        }

        return LocList;
    }
    //To Update Location details    
    public bool UpdateLocationMasterData(CMS_LocationMaster_DBS_View obj, Int64 loginId)
    {

        connection();
        SqlCommand com = new SqlCommand("CMS_Update_LocationMasterData", con);

        com.CommandType = CommandType.StoredProcedure;
        com.Parameters.AddWithValue("@ID", obj.ID);
        com.Parameters.AddWithValue("@LocationCode", obj.LocationCode);
        com.Parameters.AddWithValue("@LocationName", obj.LocationName);
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
    //To delete Location details    
    public bool DeleteLocationMasterData(Int64 Id)
    {

        connection();
        SqlCommand com = new SqlCommand("CMS_Delete_LocationMasterData", con);

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

    //To get Location details by id
    public CMS_LocationMaster_DBS_View GetLocationMasterDataById(Int64 Id)
    {

        connection();
        SqlCommand com = new SqlCommand("CMS_Get_LocationMasterData_ById", con);

        com.CommandType = CommandType.StoredProcedure;
        SqlDataAdapter da = new SqlDataAdapter(com);
        DataTable dt = new DataTable();

        con.Open();
        da.Fill(dt);
        con.Close();

        CMS_LocationMaster_DBS_View obj = new CMS_LocationMaster_DBS_View();

        if (dt.Rows.Count > 0)
        {
            obj = new CMS_LocationMaster_DBS_View
            {

                ID = Convert.ToInt64(dt.Rows[0]["ID"]),
                LocationCode = Convert.ToString(dt.Rows[0]["LocationCode"]),
                LocationName = Convert.ToString(dt.Rows[0]["LocationName"]),
                IsActive = Convert.ToBoolean(dt.Rows[0]["IsActive"])

            };
        }

        return obj;
    }
}