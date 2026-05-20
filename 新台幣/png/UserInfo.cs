using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data.SqlClient;
using System.Net;
using System.Collections.Generic;
using System.Web.Configuration;
using Microsoft.Office.Interop.Excel;

/// <summary>
/// 使用者資訊
/// </summary>

public static class UserInfo
{
    /// <summary>取得登入使用者員編，SID:request的SID，request:直接給Request</summary>
    /// <returns>使用者員編</returns>
    public static String getUserId(string SID, HttpRequest request)
    {
        String strUsrId = "";
        String localIP = "";
        String serverIP = "";

        WebReference.CHBSCWebService soap = new WebReference.CHBSCWebService();
        localIP = request.UserHostAddress.ToString();
        serverIP = request.ServerVariables.Get("LOCAL_ADDR").ToString();

        //if (serverIP == "10.100.7.90") =================>ESI 修改AP位置
        if ((serverIP == "10.100.7.188") || (serverIP == "10.100.7.186"))
        {
            strUsrId = soap.getUIDBySIDAndIP(SID, localIP).ToString();
            if (string.IsNullOrEmpty(strUsrId))
                strUsrId = soap.getUIDBySIDAndIP(SID, "10.100.8.41").ToString();
        }
        else
        {
            strUsrId = soap.getUIDBySIDAndIP(SID, "10.100.8.41").ToString();
            if(string.IsNullOrEmpty(strUsrId))
                strUsrId = soap.getUIDBySIDAndIP(SID, localIP).ToString();
        }
        if (string.IsNullOrEmpty(strUsrId))
        {
            strUsrId = getUserId(SID);
        }
        if (string.IsNullOrEmpty(strUsrId))
        {
            strUsrId = request.Cookies["CHB_IW_"]["User_ID"].ToString();
        }
        return strUsrId;
    }

    /// <summary>取得登入使用者員編，SID:request的SID</summary>
    /// <returns>使用者員編</returns>
    public static String getUserId(string SID)
    {
        string SqlCmd, drString;
        drString = "";

        //string SIP = string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["IP"]) ? HttpContext.Current.Request.UserHostAddress.ToString() : HttpContext.Current.Request.QueryString["IP"];
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT [USR_ID] FROM [CHBSC].[dbo].[SC_SESSION] Where SESSION_ID=@SID AND DATEADD(s,@EXPIREDTIME, SESSION_LMT) > GETDATE()"; //取出使用者員編

                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@SID", SID);
                    cmd.Parameters.AddWithValue("@EXPIREDTIME", 68400);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            drString = dr["USR_ID"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            drString = getUserIdOG(SID);
        }
        if (string.IsNullOrEmpty(drString))
        {
            drString = getUserIdOG(SID);
        }
        return drString;
    }
    /// <summary>取得登入使用者員編，SID:request的SID</summary>
    /// <returns>使用者員編</returns>
    public static String getUserIdOG(string SID)
    {
        string SqlCmd, drString;
        drString = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["basel_iom"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT [USR_ID] FROM [OG_UAT].[dbo].[NornsUserSession] Where SESSION_ID=@SID AND DATEADD(s,@EXPIREDTIME, SESSION_LMT) > GETDATE()"; //取出使用者員編
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@SID", SID);
                    cmd.Parameters.AddWithValue("@EXPIREDTIME", 68400);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            drString = dr["USR_ID"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex2)
        {
            drString = drString + "——>查詢使用者員編錯誤：," + ex2.Message;
        }
        return drString;
    }

    /// <summary>將所有角色分配給該員編</summary>
    public static void RoleAllGive(string UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        String drString = "";
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = " INSERT INTO SC_ROL_USR SELECT     ROL_ID, '" + UsrId + "' AS Expr1  ";
        sqlString += " FROM SC_ROLE WHERE     (ROL_ID NOT IN (SELECT     ROL_ID  FROM SC_ROL_USR AS SC_ROL_USR_1 WHERE      (USR_ID = '" + UsrId + "'))) ";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
    }

    /// 取得登入員工檔倒入狀態根據日期
    public static List<string> getDwIntraLog(String RecordDate)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        List<string> drString = new List<string>();
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub1"].ToString());
        cnn.Open();
        String sqlString = " SELECT RecordDate, ErrorMessage, Count FROM DWINTRALOG  ";
        sqlString += " WHERE     (RecordDate = '" + RecordDate + "')";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        while (dr.Read() != false)
        {

            drString.Add(dr["RecordDate"].ToString() + "," + dr["ErrorMessage"].ToString() + "," + dr["Count"].ToString());
            //紀錄日期,錯誤訊息,記數

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }

    /// <summary>取得登入使用者姓名及所屬分行資料，UsrId:員工編號</summary>
    /// <returns>OU_ID分行代號,USR_NAME使用者名稱,OU_NAME分行名稱</returns>
    public static String getUserInfo(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        String drString = "";
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "Select  a.USR_ID,a.OU_ID, a.USR_NAME,b.OU_NAME from SC_USER a  ";
        sqlString += " inner join SC_ORG_UNIT b on a.OU_ID=b.OU_ID  where  a.USR_ID='" + UsrId + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            drString = dr["OU_ID"].ToString() + "," + dr["USR_NAME"].ToString() + "," + dr["OU_NAME"].ToString();

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }

    /// <summary>取得登入使用者所屬分行及員工姓名，UsrId:員工編號</summary>
    /// <returns>BRNO分行代號,NAME使用者名稱</returns>
    public static String getUserBrNo(string Usrid)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["basel_iom"].ToString()); //20260414 改成新權控的
        cnn.Open();
        String strUsrBrNo = "";

        String sqlString = "Select * from chb_pub..EMPLOYEE where STAFF='" + Usrid + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            strUsrBrNo = dr["BRNO"].ToString() + "," + dr["NAME"].ToString();

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return strUsrBrNo;
    }

    /// <summary>取得登入使用者所屬分行及員工姓名，UsrId:員工編號</summary>
    /// <returns>BRNO分行代號,職位等級</returns>
    public static String getUserBrNoAndLevel(string Usrid)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String strUsrBrNo = "";

        String sqlString = "Select BRNO,NAME,MANAGER_LEVEL from chb_pub..EMPLOYEE where STAFF='" + Usrid + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            strUsrBrNo = dr["BRNO"].ToString() + "," + dr["NAME"].ToString() + "," + dr["MANAGER_LEVEL"].ToString();

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return strUsrBrNo;
    }

    /// 取得登入使用者所屬分行資料及代碼
    public static String getUserBrNoInfoNew(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        String drString = "";
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub1"].ToString());
        cnn.Open();
        String sqlString = " select a.BRNONEW,a.BRNOOLD, a.BRNAME , b.NAME, b.STAFF from  ";
        sqlString += " BRANCH_CONVERT a inner join EMPLOYEE b on a.BRNOOLD = b.BRNO where b.STAFF='" + UsrId + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            drString = dr["BRNONEW"].ToString() + "," + dr["BRNAME"].ToString();//新分行代號,分行名稱

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }


    /// <summary>
    /// 取得登入使用者所屬分行代號、分行名稱及使用者姓名(合併版 & 優利代號)
    /// </summary>
    /// <param name="UsrId">員工編號</param>
    /// <returns>優利分行代號,優利分行名稱,使用者姓名</returns>
    public static String getUserBrNoInfoMergeU(String UsrId)
    {

        /// 範例：
        /// 國際營運處7F(0301)及2F(2113)合併→轉優利代號0166
        /// 商品策劃處(0551)及卡片業務營運中心(3138)合併→轉優利代號0215

        string SqlCmd, drString;
        drString = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT A.[BRNO],A.[BRNAME],B.[NAME] FROM [BRANCHMERGEU] AS A INNER JOIN [EMPLOYEEMERGEU] AS B ON A.[BRNO]=B.[BRNO] WHERE B.[STAFF]=@STAFF"; //取出表單編號最大值
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@STAFF", UsrId);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            drString = dr["BRNO"].ToString() + "," + dr["BRNAME"].ToString() + "," + dr["NAME"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            drString = drString + "——>查詢分行代號錯誤：," + ex.Message;
        }
        return drString;
    }


    /// <summary>取得登入使用者所屬分行資料及代碼(優利代號)，UsrId:員工編號</summary>
    /// <returns>BRNO優利分行代號,BRNAME優利分行名稱</returns>
    public static String getUserBrNoInfoU(String UsrId)
    {
        string SqlCmd, drString;
        drString = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "select a.BRNO, a.BRNAME from BRANCHTRANS a inner join EMPLOYEETRANS b on a.BRNO = b.BRNO where b.STAFF=@STAFF"; //取出表單編號最大值
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@STAFF", UsrId);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            drString = dr["BRNO"].ToString() + "," + dr["BRNAME"].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            drString = drString + "——>查詢分行代號錯誤：," + ex.Message;
        }
        return drString;
    }


    /// <summary>取得登入使用者所屬分行，UsrId:員工編號</summary>
    /// <returns>BRNO分行代號</returns>
    public static String getUserBrNos(string Usrid)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String strUsrBrNo = "";

        String sqlString = "Select BRNO from chb_pub..EMPLOYEE where STAFF='" + Usrid + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            strUsrBrNo = dr["BRNO"].ToString();

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return strUsrBrNo;
    }

	/* 姓名轉ID (若同名有多ID) 再查去識別化名單 add by sunny20241024 */
	///<summary>取得登入使用者Identity(ID)身分證，strName:員工姓名</summary>
	///<returns>Identity(ID)身分證</returns>
    public static List<String> getUserIdentityByName(string strName)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        List<String> strIdList = new List<String>();

        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["TheBlack"].ToString());
        cnn.Open();

        /* 程式姓名轉ID 新寫法 modify by sunny20241024 */
        //select PARTY_ID_REAL, PARTY_ID from TheBlack..V_CRC_LON_BLACK WHERE TableType = '2-NAME' and PARTY_ID like '%王小明%'
        String sqlString = "Select PARTY_ID_REAL, PARTY_ID from TheBlack..V_CRC_LON_BLACK WHERE TableType = '2-NAME' and PARTY_ID like '%" + strName + "%'";

        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();

        while (dr.Read() != false)
        {
            strIdList.Add(dr["PARTY_ID_REAL"].ToString());
        }

        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();

        return strIdList;
    }

    /// <summary>取得登入使用者所屬分行，UsrId:員工編號</summary>
    /// <returns>BRNO分行代號</returns>
    public static String getUserName(string Usrid)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sName = Usrid;

        String sqlString = "Select NAME from chb_pub..EMPLOYEE where STAFF='" + Usrid + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {
            sName = dr["NAME"].ToString();
        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return sName;
    }

    /// <summary>
    /// 取得登入使用者所屬分行代號(合併版 & 優利代號)
    /// </summary>
    /// <param name="UsrId">員工編號</param>
    /// <returns>BRNO優利分行代號</returns>
    public static String getUserBrNoMergeU(string UsrId)
    {

        /// 範例：
        /// 國際營運處7F(0301)及2F(2113)合併→轉優利代號0166
        /// 商品策劃處(0551)及卡片業務營運中心(3138)合併→轉優利代號0215

        string BRNO, SqlCmd;
        BRNO = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT [BRNO] FROM [EMPLOYEEMERGEU] WHERE [STAFF]=@STAFF"; //取出表單編號最大值
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@STAFF", UsrId);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            BRNO = dr[0].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BRNO = BRNO + "——>查詢分行代號錯誤：" + ex.Message;
        }
        return BRNO;
    }


    /// <summary>
    /// 取得單位類型
    /// </summary>
    /// <param name="Brno">單位代號(不分內網版/優利版)</param>
    /// <returns>H:總行單位/C:區營運處/B:國內營業單位、證券經紀商/O:海外營業單位</returns>
    public static String getBrnoType(string Brno)
    {

        string BRTYPE, SqlCmd;
        BRTYPE = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT [dbo].[ufn_getBrnoType] (@BRNO)";
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@BRNO", Brno);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            BRTYPE = dr[0].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BRTYPE = BRTYPE + "——>查詢單位類型發生錯誤：" + ex.Message;
        }
        return BRTYPE;
    }


    /// <summary>取得登入使用者所屬分行代號（優利），UsrId:員工編號</summary>
    /// <returns>BRNO優利分行代號</returns>
    public static String getUserBrNoU(string Usrid)
    {

        string BRNO, SqlCmd;
        BRNO = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "Select BRNO from EMPLOYEETRANS where STAFF=@STAFF"; //取出表單編號最大值
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@STAFF", Usrid);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            BRNO = dr[0].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BRNO = BRNO + "——>查詢分行代號錯誤：" + ex.Message;
        }
        return BRNO;
    }


    /// <summary>取得登入使用者現有角色，UsrId:員工編號</summary>
    /// <returns>ROL_NAME角色名稱</returns>
    public static List<String> getUserRoleNow(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        List<String> drString = new List<String>();
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT     b.ROL_NAME  FROM    SC_ROL_USR AS a LEFT OUTER JOIN";
        sqlString += " SC_ROLE AS b ON a.ROL_ID = b.ROL_ID WHERE     (a.USR_ID ='" + UsrId + "')";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        while (dr.Read() != false)
        {
            drString.Add(dr["ROL_NAME"].ToString());
        }

        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }

    /// <summary>取得登入使用者現有作業，UsrId:員工編號</summary>
    /// <returns>TSK_ID作業編號</returns>
    public static List<String> getUserTaskNow(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        List<String> drString = new List<String>();
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT    b.TSK_ID,b.BTN_ID  FROM    SC_ROL_USR AS a LEFT OUTER JOIN ";
        sqlString += " SC_ROL_TSK as b on a.ROL_ID=b.ROL_ID where a.USR_ID='" + UsrId.ToString() + "' ";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        while (dr.Read() != false)
        {


            drString.Add(dr["TSK_ID"].ToString());
        }

        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }

    /// <summary>取得登入使用者現有作業URL，UsrId:員工編號</summary>
    /// <returns>BTN_URL：作業URL</returns>
    public static List<String> getUserTaskURL(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        List<String> drString = new List<String>();
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = " SELECT    b.TSK_ID,b.BTN_ID,c.BTN_URL  FROM    (SC_ROL_TSK as b LEFT OUTER JOIN ";
        sqlString += " SC_ROL_USR AS a on a.ROL_ID=b.ROL_ID  ) LEFT OUTER JOIN ";
        sqlString += " SC_BUTTON as c on  b.TSK_ID=c.TSK_ID where a.USR_ID='" + UsrId.ToString() + "' ";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        while (dr.Read() != false)
        {


            drString.Add(dr["BTN_URL"].ToString());
        }

        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }

    /// <summary>取得登入使用者姓名及所屬分行資料(包含區審中心)，UsrId:員工編號</summary>
    /// <returns>BRNO分行代號,NAME使用者名稱,BRNAME分行名稱</returns>
    public static String getUserInfoByCrcCenter(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        String drString = "";
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub1"].ToString());
        cnn.Open();
        String sqlString = " select a.STAFF,a.NAME,a.BRNO ,b.BRNAME from EMPLOYEEMAP a ";
        sqlString += " LEFT OUTER	JOIN BRANCHMAP b ON a.BRNO = b.BRNO  where  a.STAFF='" + UsrId + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        if (dr.Read())
        {

            drString = dr["BRNO"].ToString() + "," + dr["NAME"].ToString() + "," + dr["BRNAME"].ToString();

        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return drString;

    }

    /// <summary>判斷使用者角色是否包含內網維護經辦(IntraMaintain)，UsrId:員工編號</summary>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool UserRoleContainIntraMaintain(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        String drString = "";
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT count(1) as INTRA from SC_ROL_USR WHERE USR_ID='" + UsrId + "' AND ROL_ID='IntraMaintain' ";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        bool isIntraMaintain = true;
        if (dr.Read())
        {
            if (dr["INTRA"].ToString() == "0")
            {
                isIntraMaintain = false;
            }
        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return isIntraMaintain;

    }

    /// <summary>判斷使用者角色是否包含管制員(ControlAdmin)，UsrId:員工編號</summary>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool UserRoleContainControlAdmin(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        String drString = "";
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT count(1) as INTRA from SC_ROL_USR WHERE USR_ID='" + UsrId + "' AND ROL_ID='ControlAdmin' ";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        bool isControlAdmin = true;
        if (dr.Read())
        {
            if (dr["INTRA"].ToString() == "0")
            {
                isControlAdmin = false;
            }
        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return isControlAdmin;

    }

    /// <summary>判斷使用者角色是否包含內網維護經辦(IntraMaintain)或管制員(ControlAdmin)，UsrId:員工編號</summary>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool UserRoleWithIntraOrAdmin(String UsrId)
    {
        bool isAdmin = UserRoleContainIntraMaintain(UsrId);
        if (isAdmin == false)
            isAdmin = UserRoleContainControlAdmin(UsrId);
        return isAdmin;
    }

    /// <summary>判斷使用者是否為資訊管理員(MGR_OU_USR)，UsrId:員工編號</summary>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool IdentifyMGR_OU_USR(String UsrId)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;

        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT count(1) as INTRA from SC_MGR_OU_USR WHERE USR_ID='" + UsrId + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        bool isMGR_OU_USR = true;
        if (dr.Read())
        {
            if (dr["INTRA"].ToString() == "0")
            {
                isMGR_OU_USR = false;
            }
        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return isMGR_OU_USR;

    }

    /* 2144紐約分行、2151洛杉磯分行、2175倫敦分行 這三間海外分行 的 資訊管理員 才有權限編輯 */
    /// <summary>判斷使用者是否為資訊管理員(MGR_OU_USR)，UsrId:員工編號，Brno：單位代號</summary>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool IdentifyMGR_OU_USR_Brno(String UsrId, String strBrno)
    {
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;

        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT count(1) as INTRA from SC_MGR_OU_USR WHERE USR_ID='" + UsrId + "' and OU_ID = '" + strBrno + "'";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        bool isMGR_OU_USR = true;
        if (dr.Read())
        {
            if (dr["INTRA"].ToString() == "0")
            {
                isMGR_OU_USR = false;
            }
        }
        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        return isMGR_OU_USR;

    }


    /// <summary>
    /// 檢查登入者身分
    /// </summary>
    /// <param name="Request">HttpRequest物件</param>
    /// <param name="Response">HttpResponse物件</param>
    /// <param name="Role">角色名稱需由設計人員指定 當Role=""時預設為 "Default" 角色</param>
    public static void CheckUser(HttpRequest Request, HttpResponse Response, string Role)
    {
        string SID = (Request.QueryString["SID"] == null) ? "" : Request.QueryString["SID"].ToString();
        string TaskId = (Request.QueryString["tid"] == null) ? "" : Request.QueryString["tid"].ToString();
        string UsrRoles = (Role == "") ? "Default" : Role.ToString();
        if (SID == "" && TaskId == "")
        {
            //Response.Redirect("/COMMON/unvaild.asp");
            Response.Redirect("/DotNet/Program/Error.aspx");
        }
        if (SID != "")
        {
            string UsrId = UserInfo.getUserId(Request.QueryString["SID"], Request);//取得使用者員編
            List<string> UsrTask = getUserTaskNow(UsrId);
            List<string> UsrRole = getUserRoleNow(UsrId);
            bool error_page = true;
            if (TaskId != "")
            {
                foreach (string temp in UsrTask)
                {
                    if (temp == TaskId)
                        error_page = false;
                }

            }
            else
            {
                if (!IdentifyUserRole(UserInfo.getUserId(SID, Request).ToString(), UsrRoles))
                    error_page = false;
                //   error_page = false;
                /*   List<string> UserUrl = getUserTaskURL(UsrId);
                   foreach (string tempURL in UserUrl)
                       if (Request.Url.ToString().Contains(tempURL))
                           error_page = false;*/
            }
            if (error_page == true)
                Response.Redirect("/COMMON/unvaild.asp");
        }
        else
        {
            Response.Redirect("/COMMON/unvaild.asp");
            //Response.Redirect("/DotNet/Program/Error.aspx");
        }
    }
    /// <summary>判斷使用者是否擁有該角色，UsrId:員工編號，Role:角色名稱</summary>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool IdentifyUserRole(string UsrId, string Role)
    {
        bool result = false;
        SqlConnection cnn;
        SqlCommand cmd;
        SqlDataReader dr;
        List<String> drString = new List<String>();
        cnn = new SqlConnection(ConfigurationManager.ConnectionStrings["chb_pub"].ToString());
        cnn.Open();
        String sqlString = "SELECT     b.ROL_NAME  FROM    SC_ROL_USR AS a LEFT OUTER JOIN";
        sqlString += " SC_ROLE AS b ON a.ROL_ID = b.ROL_ID WHERE     (a.USR_ID ='" + UsrId + "')";
        cmd = new SqlCommand(sqlString, cnn);
        dr = cmd.ExecuteReader();
        while (dr.Read() != false)
        {
            drString.Add(dr["ROL_NAME"].ToString());
        }

        dr.Dispose();
        cmd.Dispose();
        cnn.Close();
        cnn.Dispose();
        foreach (string temp in drString)
        {
            if (Role == temp)
                result = true;
        }
        return result;
    }

    /// <summary>判斷使用者是否擁有該角色代號，UsrId:員工編號，Role:角色代號</summary>
    /// <param name="UsrId">員工編號</param>
    /// <param name="Role">角色代號</param>
    /// <returns>成功回覆「true」，失敗回覆「false」</returns>
    public static bool IdentifyUserRoleID(string UsrId, string Role)
    {
        string IsRole, SqlCmd;
        bool result = false;
        IsRole = "0";

        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT COUNT(1) FROM SC_ROL_USR WHERE (USR_ID =@UsrId) and (ROL_ID=@Role)"; //查使用者角色
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@UsrId", UsrId);
                    cmd.Parameters.AddWithValue("@Role", Role);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            IsRole = dr[0].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            //IsRole = "——>查詢角色錯誤：" + ex.Message;
            IsRole = "0";
        }
        if (IsRole != "0")  //有該角色
        {
            result = true;
        }
        return result;
    }

    public static void GetTableInformation(string Conn)
    {
        /*選取特定的表格
         *
         * string[] restrictions = new string[4];
        restrictions[0] = "iom";//資料庫名稱
        restrictions[2] = "tblCourtRecord";//表格名稱
         */


        /*System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(conn);
        conn.Open();*/
        /*DataTable dt = conn.GetSchema("Restrictions"); //參數對照表
          DataTable dt = conn.GetSchema("Columns") //取得所有資料庫表格欄位資訊
          DataTable dt = conn.GetSchema("Columns",restrictions);//取得某張表格欄位資訊
          DataTable dt = conn.GetSchema("DataBases");  //取得所有資料庫名稱
          DataTable dt = conn.GetSchema("Tables");   //資料庫中的Table資訊
          DataTable dt = conn.GetSchema("Tables", restrictions);   //某個資料表中的Table資訊*/

        //conn.Close();
        /* GridView4.DataSource = dt;
         GridView4.DataBind();*/
    }
    public static System.Data.DataTable GetColumnName(string connectString, string DataBase, string TableName)
    {
        string[] restrictions = new string[4];
        restrictions[0] = DataBase;//資料庫名稱
        restrictions[2] = TableName;//表格名稱        
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectString);
        conn.Open();
		System.Data.DataTable dt = conn.GetSchema("Columns", restrictions);//取得某張表格欄位資訊                                 
        conn.Close();
        conn.Dispose();
        return dt;
    }
    public static List<string> GetColumnNames(string connectString, string DataBase, string TableName)
    {
        List<string> result = new List<string>();
        string[] restrictions = new string[4];
        restrictions[0] = DataBase;//資料庫名稱
        restrictions[2] = TableName;//表格名稱        
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectString);
        conn.Open();
		System.Data.DataTable dt = conn.GetSchema("Columns", restrictions);//取得某張表格欄位資訊                         
        DataRow[] dr = dt.Select(string.Empty, " ORDINAL_POSITION ASC ");
        for (int i = 0; i < dr.Length; i++)
            result.Add(dr[i]["COLUMN_NAME"].ToString());
        conn.Close();
        conn.Dispose();
        return result;
    }
    public static List<string> GetColumnTypes(string connectString, string DataBase, string TableName)
    {
        List<string> result = new List<string>();
        string[] restrictions = new string[4];
        restrictions[0] = DataBase;//資料庫名稱
        restrictions[2] = TableName;//表格名稱        
        System.Data.SqlClient.SqlConnection conn = new System.Data.SqlClient.SqlConnection(connectString);
        conn.Open();
		System.Data.DataTable dt = conn.GetSchema("Columns", restrictions);//取得某張表格欄位資訊                         
        DataRow[] dr = dt.Select(string.Empty, " ORDINAL_POSITION ASC ");
        for (int i = 0; i < dr.Length; i++)
            result.Add(dr[i]["DATA_TYPE"].ToString());
        conn.Close();
        conn.Dispose();
        return result;
    }

    /// <summary>
    /// 取得登入使用者所屬單位代號(合併版 & 內網代號)
    /// </summary>
    /// <param name="UsrId">員工編號</param>
    /// <returns>BRNO合併版內網單位代號</returns>
    public static String getUserBrNoMerge(string UsrId)
    {

        /// 範例：
        /// 國際營運處7F(0301)及2F(2113)合併→轉內網代號0301

        string BRNO, SqlCmd;
        BRNO = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT [BRNO] FROM [EMPLOYEEMERGE] WHERE [STAFF]=@STAFF"; //
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@STAFF", UsrId);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            BRNO = dr[0].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BRNO = BRNO + "——>查詢單位代號錯誤：" + ex.Message;
        }
        return BRNO;
    }
    /// <summary>
    /// 取得單位名稱(合併版)
    /// </summary>
    /// <param name="BRNO">單位代號（不分優利版或內網版）</param>
    /// <returns>BRNAME合併版單位名稱</returns>
    public static String getBrNoName(string BRNO)
    {

        /// 範例：
        /// 國際營運處7F(0301)及2F(2113)合併→轉內網代號0301

        string BRNAME, SqlCmd;
        BRNAME = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT [BRNAME]  FROM [dbo].[BRANCHMERGE] where BRNO=@BRNO union SELECT [BRNAME]  FROM [dbo].[BRANCHMERGEU] where BRNO=@BRNO"; //
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@BRNO", BRNO);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            BRNAME = dr[0].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            BRNAME = BRNAME + "——>查詢單位名稱錯誤：" + ex.Message;
        }
        return BRNAME;
    }
    /// <summary>將任何單位代號，轉換成優利單位代號及內網單位代號</summary>
    /// <param name="Brno">單位代號</param>
    /// <param name="transType">轉換成：1優利單位代號、2內網單位代號、3優利+內網單位代號</param>
    /// <returns>transType=1優利單位代號,transType=2內網單位代號,transType=3優利單位代號+,+內網單位代號</returns>
    public static String tranBrNoAll(String Brno, int transType)
    {
        string SqlCmd, drString;
        drString = "";
        try
        {
            using (SqlConnection cnn = new SqlConnection(WebConfigurationManager.ConnectionStrings["chb_pub1"].ToString()))
            {
                cnn.Open();
                SqlCmd = "SELECT b.BRNO,h.BRNOUNISYS FROM BRANCH b left join HEADQUARTERS h ON b.BRNO=h.BRNOINTRA and h.ALIVE='Y' and h.BRNOINTRA<>h.BRNOUNISYS where BRNO=@BRNO or BRNOUNISYS=@BRNO"; //取出表單編號最大值
                using (SqlCommand cmd = new SqlCommand(SqlCmd, cnn))
                {
                    cmd.Parameters.AddWithValue("@BRNO", Brno);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while ((dr.Read()))
                        {
                            switch (transType)
                            {
                                case 1:
                                    if (dr.IsDBNull(1))
                                    {
                                        drString = dr["BRNO"].ToString();
                                    }
                                    else
                                    {
                                        drString = dr["BRNOUNISYS"].ToString();
                                    }
                                    break;
                                case 2:
                                    drString = dr["BRNO"].ToString();
                                    break;
                                case 3:
                                    if (dr.IsDBNull(1))
                                    {
                                        drString = dr["BRNO"].ToString();
                                    }
                                    else
                                    {
                                        drString = dr["BRNO"].ToString() + "," + dr["BRNOUNISYS"].ToString();
                                    }
                                    break;
                            }
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            drString = drString + "——>查詢錯誤：," + ex.Message;
        }
        return drString;
    }
}
