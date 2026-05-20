<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Main.aspx.cs" Inherits="Program_KL_Main" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <style type="text/css">
            .ui-dialog .ui-dialog-content { background: lightgrey; }
        .auto-style1 {
            width: 80%;
        }
        .auto-style2 {
            font-family: 微軟正黑體;
        }
        .auto-style8 {
            font-family: 微軟正黑體;
            color:black;
        }
        .auto-style3 {
            text-align: center;
        }
        #bPrint {
            font-family: 微軟正黑體;
            font-size: medium;
        }
        #bPrint0 {
            font-family: 微軟正黑體;
            font-size: medium;
        }
        #bPrintUn {
            font-family: 微軟正黑體;
            font-size: medium;
        }
        .auto-style9 {
            color: black;
        }
        .auto-style10 {
            width: 270px;
            text-align: center;
        }
        .auto-style11 {
            color: red;
        }
        </style>
</head>
<body bgcolor="#FFFFE7">
    <form id="form1" runat="server">
        <div>
            <table border="1" class="auto-style8">
            <tr>
                <td class="auto-style9">客戶統編：</td>
                <td height="30px">
                   <asp:TextBox ID="txtID" runat="server" CssClass="auto-style2" MaxLength="10" Width="100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="auto-style1" colspan="2" align="Center" height="26px">
                     <asp:Button ID="btnAdd" runat="server" OnClick="btnAdd_Click" style="font-family: 微軟正黑體; font-size: medium" Text="新增" />&nbsp; &nbsp; &nbsp; &nbsp;
                     <asp:Button ID="btnQuery" runat="server" OnClick="btnQuery_Click" style="font-family: 微軟正黑體; font-size: medium" Text="查詢" ValidationGroup="vQTY"/>
                       &nbsp; &nbsp; &nbsp; &nbsp;
                </td>
            </tr>
        </table>
            <br />
             <strong><span class="auto-style6">查詢明細</span></strong><br /> 
             <asp:GridView ID="gvData" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False" 
                    DataKeyNames="No" DataSourceID="sdsList" EnableModelValidation="True" BackColor="White" 
                    BorderColor="#E7E7FF" BorderStyle="None" BorderWidth="1px" CellPadding="3" PageSize="20" Width="100%"
                    EmptyDataText="查無待處理資料！" GridLines="Horizontal" style="font-size: medium; font-family: 微軟正黑體" 
                    OnRowCommand="gvData_RowCommand" >
                    <AlternatingRowStyle BackColor="#F7F7F7" />
                    <Columns>
                        <asp:TemplateField ShowHeader="False">
                            <ItemTemplate>
                                <asp:LinkButton ID="LinkDetail" runat="server" CausesValidation="False"  CommandArgument='<%# Eval("No") %>' CommandName="Select" Text="明細"></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:BoundField DataField="No" HeaderText="文件編號" SortExpression="No" ItemStyle-Font-Size="small" />
                        <asp:BoundField DataField="ID" HeaderText="客戶統編" SortExpression="ID" ItemStyle-Font-Size="small" />
                        <asp:BoundField DataField="Type" HeaderText="申請類型" SortExpression="Type" ItemStyle-Font-Size="small" />
                        <asp:BoundField DataField="StatusName" HeaderText="狀態" SortExpression="StatusName" ItemStyle-Font-Size="small" />
                    </Columns>
                    <FooterStyle BackColor="#B5C7DE" ForeColor="#4A3C8C" />
                    <HeaderStyle BackColor="#4A3C8C" Font-Bold="True" Font-Size="Medium" ForeColor="#F7F7F7" />
                    <PagerStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" HorizontalAlign="Right" />
                    <RowStyle BackColor="#E7E7FF" ForeColor="#4A3C8C" />
                    <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="#F7F7F7" />
                </asp:GridView>
          <asp:SqlDataSource ID="sdsList" runat="server" ConnectionString='<%$ ConnectionStrings:chb_iom %>'
            SelectCommand="select [No] ,ID ,[Type],case [Status] when '0' then '編制中' when '3' then '組長退回' when '5' then '處長退回' end [StatusName] from KL where Status in ('0','3','5') and Brno = @Brno and [ID] like @ID">
            <SelectParameters>
                <asp:ControlParameter ControlID="hfBrNo" Name="Brno" PropertyName="Value" />
                <asp:Parameter Name="ID" />
            </SelectParameters>
        </asp:SqlDataSource>
            <asp:HiddenField ID="hfBrNo" runat="server" />
        <asp:HiddenField ID="hfUserID" runat="server" />
        </div>
    </form>
</body>
</html>
