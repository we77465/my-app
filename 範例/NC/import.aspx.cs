using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using System.Collections;
using System.Net;
using System.Data.SqlClient;


public partial class Program_NC_import : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

        string FES83_IP = System.Web.Configuration.WebConfigurationManager.AppSettings["NewFES67_IP"].ToString();
        ////正式環境
        //string loginID = "eForms";
        //string password = "ftpuser_AP";
        //測試環境
        //string loginID = "intrauser";
        //string password = "intrauser@CHB";
        string loginID = "eForms";
        string password = "ftpuser_AP";

        string FtpDownloadPath = "\\ecounter\\StoreRepQry\\";
        if (!IsPostBack)
        {

            //設定參數
            //string DistinctionFileName = "EBK_WWW_0001";
            //string FtpIP = System.Web.Configuration.WebConfigurationManager.AppSettings["NewFES67_IP"].ToString();
            //string FtpLoginID = System.Web.Configuration.WebConfigurationManager.AppSettings["NewFES67_LoginID"].ToString();
            //string FtpPassword = System.Web.Configuration.WebConfigurationManager.AppSettings["NewFES67_Password"].ToString();
            string localPath = @"E:\Portal\IntraWeb\Upload\NC\";



            //// 正式環境
            int lastindex = int.Parse(Request.QueryString[0]);
            var dt = DateTime.Now.AddDays(-lastindex).ToString("yyyyMM");
            string YYMM = (int.Parse(dt) - 191100).ToString();
            var dt2 = DateTime.Now.AddDays(-lastindex).ToString("yyyyMMdd");

            ////測試環境
            //var dt = "202511";
            //string YYMM = (int.Parse(dt) - 191100).ToString();
            //var dt2 = "20251103";




            string fileName = "EBK_WWW_0001." + dt2;
            //FES抓EBK_WWW_0001檔
            FTP.FTPclient ftp = new FTP.FTPclient(FES83_IP, loginID, password);
            if (ftp.FtpFileExists(FtpDownloadPath + fileName))
            {
                ftp.Download(FtpDownloadPath + fileName, localPath + fileName, true);
            }

            //ftp.Download(FtpDownloadPath + fileName, localPath + fileName, true);

            //string FtpDownloadPath = "/ecounter/StoreRepQry/";
            //string localPath = @"E:/Portal/IntraWeb/Upload/NC/";

            ////// 正式環境
            ////int lastindex = int.Parse(Request.QueryString[0]);
            ////var dt = DateTime.Now.AddDays(-lastindex).ToString("yyyyMM");
            ////string YYMM=(int.Parse(dt) - 191100).ToString();
            ////var dt2 = DateTime.Now.AddDays(-lastindex).ToString("yyyyMMdd");

            ////測試環境
            //var dt = "202512";
            //string YYMM = (int.Parse(dt) - 191100).ToString();
            //var dt2 = "20251209";
            ////var ip = "10.100.71.64"; // 測試環境 加這段


            //string fileName = "EBK_WWW_0001." + dt2;
            //FTP.FTPclient ftp = new FTP.FTPclient(FES83_IP, loginID, password);
            //if (ftp.FtpFileExists(FtpDownloadPath + fileName))
            //{
            //    ftp.Download(FtpDownloadPath + fileName, localPath + fileName, true);
            //}



            string filePath = Server.MapPath("../../../Upload/NC/EBK_WWW_0001." + dt2);
            string first = "1";
            //檔案欄位名稱: 案件編號,統編/身分證字號,申辦身分,申辦類別,公司登記名稱/申請人姓名,公司對外名稱,通訊地郵遞區號,通訊地縣市,通訊地鄉鎮市區,通訊地址,聯絡人姓名,聯絡電話,聯絡E-MAIL,欲往來分行代號,聯絡時間,營業內容,備註,建立時間
            string sDocNo = "";
            string sParty_Id = "";
            string sApply_Identity = "";
            string sApply_Item = "";
            string sParty_Name = "";
            string sOut_Party_Name = "";
            string sZip_Code = "";
            string sCity = "";
            string sArea = "";
            string sAddr = "";
            string sContact_Person = "";
            string sTEL = "";
            string sMAIL = "";
            string sBrno = "";
            string sContact_Time = "";
            string sBusiness_Content = "";
            string sRemark = "";
            string sCrt_Time = "";
            //string sAs_Of_Date = "";
            using (StreamReader sr = new StreamReader(filePath, Encoding.GetEncoding(950)))
            {
                string line;
                // Read the file line by line until the end
                while ((line = sr.ReadLine()) != null)
                {
                    if (first == "0")
                    { // 欄位名稱不導入
                        string[] subs = line.Split(',');
                        sDocNo = "NC" + subs[0];        //案件編號
                        sParty_Id = subs[1];          //統編/身分證字號
                        sApply_Identity = subs[2];    //申辦身分
                        sApply_Item = subs[3];        //申辦類別
                        sParty_Name = subs[4];        //公司登記名稱 / 申請人姓名
                        sOut_Party_Name = subs[5];    //公司對外名稱
                        sZip_Code = subs[6];          //通訊地郵遞區號
                        sCity = subs[7];              //通訊地縣市
                        sArea = subs[8];              //通訊地鄉鎮市區
                        sAddr = subs[9];              //通訊地址
                        sContact_Person = subs[10];   //聯絡人姓名
                        sTEL = subs[11];              //聯絡電話
                        sMAIL = subs[12];             //聯絡E - MAIL
                        sBrno = subs[13];             //欲往來分行代號
                        sContact_Time = subs[14];     //聯絡時間
                        sBusiness_Content = subs[15]; //營業內容
                        sRemark = subs[16];           //備註
                        sCrt_Time = subs[17];         //建立時間
                                                      //foreach (var sub in subs)
                                                      //{
                        //Response.Write(YYMM + "<br />");
                        //Response.Write(dt2 + "<br />");
                        //}
                        sdsNCModify.DeleteParameters["DocNo"].DefaultValue = sDocNo;
                        sdsNCModify.Delete();
                        sdsNCModify.InsertParameters["DocNo"].DefaultValue = sDocNo;
                        sdsNCModify.InsertParameters["Party_Id"].DefaultValue = sParty_Id;
                        sdsNCModify.InsertParameters["Apply_Identity"].DefaultValue = sApply_Identity;
                        sdsNCModify.InsertParameters["Apply_Item"].DefaultValue = sApply_Item;
                        sdsNCModify.InsertParameters["Party_Name"].DefaultValue = sParty_Name;
                        sdsNCModify.InsertParameters["Out_Party_Name"].DefaultValue = sOut_Party_Name;
                        sdsNCModify.InsertParameters["Zip_Code"].DefaultValue = sZip_Code;
                        sdsNCModify.InsertParameters["City"].DefaultValue = sCity;
                        sdsNCModify.InsertParameters["Area"].DefaultValue = sArea;
                        sdsNCModify.InsertParameters["Addr"].DefaultValue = sAddr;
                        sdsNCModify.InsertParameters["Contact_Person"].DefaultValue = sContact_Person;
                        sdsNCModify.InsertParameters["TEL"].DefaultValue = sTEL;
                        sdsNCModify.InsertParameters["MAIL"].DefaultValue = sMAIL;
                        sdsNCModify.InsertParameters["Brno"].DefaultValue = sBrno;
                        sdsNCModify.InsertParameters["Contact_Time"].DefaultValue = sContact_Time;
                        sdsNCModify.InsertParameters["Business_Content"].DefaultValue = sBusiness_Content;
                        sdsNCModify.InsertParameters["Remark"].DefaultValue = sRemark;
                        sdsNCModify.InsertParameters["Crt_Time"].DefaultValue = sCrt_Time;
                        sdsNCModify.InsertParameters["YYMM"].DefaultValue = YYMM;
                        sdsNCModify.Insert();
                    }
                    else
                    {
                        sdsNCDelete.DeleteParameters["Crt_Time"].DefaultValue = dt2;
                        sdsNCDelete.Delete();
                    }
                    first = "0";
                }
            }
        }

    }


    //void getFile(string url, NetworkCredential a_credentials, string localPath)
    //{
    //    FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(url + localPath);

    //    downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
    //    downloadRequest.Credentials = a_credentials;

    //    using (FtpWebResponse downloadResponse =
    //              (FtpWebResponse)downloadRequest.GetResponse())
    //    using (Stream sourceStream = downloadResponse.GetResponseStream())
    //    using (Stream targetStream = System.IO.File.Create(System.IO.Path.Combine("E:/Portal/IntraWeb/Upload/NC", localPath)))
    //    {
    //        byte[] buffer = new byte[10240];
    //        int read;
    //        while ((read = sourceStream.Read(buffer, 0, buffer.Length)) > 0)
    //        {
    //            targetStream.Write(buffer, 0, read);
    //        }
    //    }
    //}

}