<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Detail.aspx.cs" Inherits="Program_NC_Detail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta http-equiv="x-ua-compatible" content="IE=9" />
    <title>線上申請收單業務</title>
    <link rel="stylesheet" type="text/css" href="../Common/css/Common.css" />
    <style type="text/css">
        .auto-style1 {
            height: 37px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <%--使用Alertfy請加入下面五行--%>
        <link href="../Common/css/alertify.min.css" rel="stylesheet" />
        <link href="../Common/css/default.min.css" rel="stylesheet" />
        <script src="../Common/js/jquery.min.js"></script>
        <script src="../Common/js/alertify.min.js"></script>
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <%--使用Alertfy請加入以上五行--%>

                    <asp:HiddenField ID="HFUserId" runat="server" />
<%--        <asp:HiddenField ID="HFUserInfo" runat="server" />
        <asp:HiddenField ID="HFBrno" runat="server" />--%>

         
                <table">
                    <tr>
                        <td font-family: '標楷體' style=" font-weight:bold">線上申請收單業務</td>

                    </tr>
               </table>
               <table style="width: 100%;">


                    <tr>
                        <td class="auto-style1">
                            No.
                            <asp:Label ID="Label_NO" runat="server"></asp:Label>
                        </td>

                      </tr>
             </table>
        <table>
                      <tr>
                        <td style='border-top: solid windowtext 1.0pt; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            申辦身分
                        </td>
                        <td style='border-top: solid windowtext 1.0pt; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Apply_Identity" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            申辦類別
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Apply_Item" runat="server"></asp:Label>
                        </td>

                      </tr>



                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            公司登記名稱/申請人姓名
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Party_Name" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            公司統一編號/申請人身分證字號
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Party_Id" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            公司對外名稱
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Out_Party_Name" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            公司營業地址/申請人通訊地址
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Addr" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            聯絡人姓名
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Contact_Person" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            聯絡電話
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_TEL" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            聯絡時間
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Contact_Time" runat="server"></asp:Label>
                        </td>

                      </tr>

                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            聯絡人E-Mail
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_MAIL" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            營業內容
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Business_Content" runat="server"></asp:Label>
                        </td>

                      </tr>
                      <tr>
                        <td style='border-top: none; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            備註
                        </td>
                        <td style='border-top: none; border-left: none; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                            <asp:Label ID="Label_Remark" runat="server"></asp:Label>
                        </td>

                      </tr>
<%--                   








--%>


<%--                    <tr>
                        <td colspan="4" style='border-top: solid windowtext 1.0pt; border-left: solid windowtext 1.0pt; border-bottom: solid windowtext 1.0pt; border-right: solid windowtext 1.0pt; padding: 0cm 1.4pt 0cm 1.4pt; font-family: 標楷體'>
                        申 請 人 填 寫 欄 位
                        </td>
                    </tr>
                    <tr>

                    </tr>--%>
                </table>
    </form>
    <asp:SqlDataSource ID="sdsNC" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>"
        SelectCommand="SELECT 
[DocNo]
,[Party_Id]
,[Apply_Identity]
,[Apply_Item]
,[Party_Name]
,[Out_Party_Name]
,[Zip_Code]
,[City]
,[Area]
,[Addr]
,[Contact_Person]
,[TEL]
,[MAIL]
,[Brno]
,[Contact_Time]
,[Business_Content]
,[Remark]
FROM NC
WHERE DocNo=@DocNo">
            <SelectParameters>
               <asp:Parameter Name="DocNo" />
            </SelectParameters>
    </asp:SqlDataSource>


</body>
</html>
