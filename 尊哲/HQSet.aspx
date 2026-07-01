<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HQSet.aspx.cs" Inherits="Program_EF_HQSet" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>存放央行及存放銀行同業未達帳調節表</title>
    <link rel="stylesheet" type="text/css" href="../Common/css/Common.css" />
</head>
<body>
    <form id="form1" runat="server">
        <asp:Image ID="Image1" runat="server" ImageUrl="../Common/jpg/arrowredbig.jpg" />
        &nbsp;<span class="style1"><strong style="font-size: large"><asp:Label ID="LBTypeName" runat="server" Text="TypeName"></asp:Label>—彙總表</strong></span><br />
        <br />
        <span style="color:#990000; font-weight:bold;">■ 所屬<asp:Label ID="LBPeriod" runat="server" Text="年月"></asp:Label>：</span><asp:TextBox ID="TBYYMM" runat="server"></asp:TextBox>
        <asp:Button ID="BTQuery" runat="server" Text="查詢" OnClick="BTQuery_Click" />
        (請使用民國<asp:Label ID="LBPeriodPS" runat="server" Text="年月YYYMM"></asp:Label>)
        <br />
        <br />
        <span class="style2">
            <asp:Button ID="BTExcel" runat="server" OnClick="BTExcel_Click" Text="匯出EXCEL" />
        </span>
        <asp:Label ID="msg" runat="server" style="font-weight: 700; color: #FF0000"></asp:Label>
        <br />
        <br />
        <div style="text-align:center; margin-bottom:6px;">
            <asp:Label ID="LBReportTitle" runat="server"
                style="font-weight:bold; font-size:large;"></asp:Label><br />
            <asp:Label ID="LBReportDate" runat="server"></asp:Label>
        </div>
        <asp:GridView ID="GridView1" runat="server" CellPadding="4" DataSourceID="DSSourceQuery2"
            ForeColor="#333333" GridLines="None" EmptyDataText="無資料" AutoGenerateColumns="True"
            Width="100%" OnRowDataBound="GridView1_RowDataBound" OnDataBound="GridView1_DataBound">
            <AlternatingRowStyle BackColor="White" />
            <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="White" HorizontalAlign="Center" />
            <PagerStyle BackColor="#FFCC66" ForeColor="#333333" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
            <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
        </asp:GridView>
        <br />
        <asp:HiddenField ID="HFDocType" runat="server" />

        <asp:SqlDataSource ID="DSDocType" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>"
            SelectCommand="SELECT d.TypeName, e.HQBrnoU, e.MinusMonth, e.HQRole, e.QuerySQL2, ISNULL(e.Period, '') AS Period FROM tblDocType AS d LEFT OUTER JOIN tblDocTypeExtend AS e ON d.DocType = e.DocType WHERE (d.DocType = @DocType)">
            <SelectParameters>
                <asp:ControlParameter ControlID="HFDocType" Name="DocType" PropertyName="Value" Type="String" />
            </SelectParameters>
        </asp:SqlDataSource>

        <asp:SqlDataSource ID="DSSourceQuery2" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>">
            <SelectParameters>
                <asp:Parameter Name="YYMM"></asp:Parameter>
                <asp:ControlParameter ControlID="HFDocType" DefaultValue="" Name="DocType" PropertyName="Value" />
            </SelectParameters>
        </asp:SqlDataSource>
    </form>
</body>
</html>
