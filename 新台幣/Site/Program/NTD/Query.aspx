<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="NtdQuery" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>新臺幣定期存款專案利率申請 - 查詢</title>
    <style>
        /* ===== 共用版型 ===== */
        * { box-sizing: border-box; }
        body { font-family: "Microsoft JhengHei", "微軟正黑體", Arial, sans-serif; font-size: 14px; color: #222; background: #f4f6f9; margin: 0; }
        a { color: #1565c0; text-decoration: none; }
        a:hover { text-decoration: underline; }
        .topbar { background: #1565c0; color: #fff; padding: 10px 18px; display: flex; align-items: center; justify-content: space-between; }
        .topbar h1 { margin: 0; font-size: 18px; font-weight: 600; }
        .topbar .user { font-size: 13px; }
        .nav { background: #fff; padding: 0 18px; border-bottom: 1px solid #e0e0e0; }
        .nav a { padding: 12px 20px; color: #555; display: inline-block; border-bottom: 3px solid transparent; }
        .nav a.active { color: #1565c0; border-bottom-color: #1565c0; font-weight: bold; }
        .nav a:hover { background: #f0f4ff; }
        .container { padding: 18px; }
        .card { background: #fff; border: 1px solid #e0e0e0; padding: 18px; margin-bottom: 16px; }
        .card-title { font-size: 16px; font-weight: bold; margin: 0 0 12px; padding-bottom: 8px; border-bottom: 2px solid #1565c0; color: #1565c0; }
        .footer { text-align: center; color: #888; font-size: 12px; padding: 16px; }

        /* ===== 查詢條件 form ===== */
        .form { width: 100%; border-collapse: collapse; }
        .form th { width: 110px; background: #e3f2fd; color: #1565c0; text-align: right; padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form td { padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form input, .form select { border: 1px solid #bdbdbd; border-radius: 3px; padding: 4px 6px; font-size: 13px; font-family: inherit; }

        /* ===== 結果 GridView ===== */
        .tbl { width: 100%; border-collapse: collapse; }
        .tbl th, .tbl td { border: 1px solid #d0d0d0; padding: 6px 8px; font-size: 13px; }
        .tbl th { background: #e3f2fd; font-weight: bold; color: #1565c0; text-align: center; }
        .tbl tr:nth-child(even) td { background: #fafafa; }
        .tbl tr:hover td { background: #fff8e1; }
        .tbl .num { text-align: right; }
        .tbl .ctr { text-align: center; }

        /* ===== 按鈕 / 工具列 ===== */
        .toolbar { padding: 8px 0; display: flex; gap: 6px; align-items: center; }
        .btn { display: inline-block; padding: 6px 16px; border: 1px solid #1565c0; background: #1565c0; color: #fff; border-radius: 3px; cursor: pointer; font-size: 13px; font-family: inherit; }
        .btn:hover { background: #0d47a1; }
        .btn-sub { background: #757575; border-color: #757575; }
        .btn-sub:hover { background: #424242; }
        .btn-warn { background: #f57c00; border-color: #f57c00; }
        .btn-warn:hover { background: #e65100; }

        /* ===== 狀態徽章 ===== */
        .status { display: inline-block; padding: 2px 8px; border-radius: 12px; font-size: 12px; color: #fff; }
        .status-0 { background: #9e9e9e; }
        .status-1 { background: #1976d2; }
        .status-2 { background: #0288d1; }
        .status-4 { background: #f57c00; }
        .status-6 { background: #c62828; }
        .status-9 { background: #388e3c; }
    </style>
</head>
<body>
<form id="Form1" runat="server">
    <div class="topbar">
        <h1>新臺幣定期存款專案利率申請</h1>
        <div class="user"><asp:Label ID="LblUser" runat="server" /></div>
    </div>
    <div class="nav">
        <a href="<%= NavUrl("Main.aspx") %>">待處理</a>
        <a href="<%= NavUrl("Query.aspx") %>" class="active">查詢</a>
        <a href="<%= NavUrl("Approve.aspx") %>">簽核/決行</a>
        <a href="<%= NavUrl("Report.aspx") %>">報表</a>
    </div>

    <div class="container">
        <div class="card">
            <h2 class="card-title">查詢條件</h2>
            <table class="form">
                <tr>
                    <th>申請日期</th>
                    <td>
                        <asp:TextBox ID="TxtDateFrom" runat="server" TextMode="Date" />
                        ~ <asp:TextBox ID="TxtDateTo" runat="server" TextMode="Date" />
                    </td>
                    <th>單位代號</th>
                    <td><asp:DropDownList ID="DdlBranch" runat="server" /></td>
                </tr>
                <tr>
                    <th>流程狀態</th>
                    <td>
                        <asp:DropDownList ID="DdlStatus" runat="server">
                            <asp:ListItem Value="-1" Text="全部" />
                            <asp:ListItem Value="0"  Text="草稿/退回" />
                            <asp:ListItem Value="1"  Text="待襄理放行" />
                            <asp:ListItem Value="2"  Text="待經理放行" />
                            <asp:ListItem Value="4"  Text="待總行經辦審核" />
                            <asp:ListItem Value="6"  Text="待總行科長決行" />
                            <asp:ListItem Value="9"  Text="結案" />
                        </asp:DropDownList>
                    </td>
                    <th>客戶名稱</th>
                    <td><asp:TextBox ID="TxtCustomerKeyword" runat="server" /></td>
                </tr>
                <tr>
                    <th>申請單號</th>
                    <td colspan="3"><asp:TextBox ID="TxtApplyNoKeyword" runat="server" Width="200" /></td>
                </tr>
            </table>
            <div class="toolbar">
                <asp:Button ID="BtnQuery"  runat="server" Text="查詢"        CssClass="btn"          OnClick="BtnQuery_Click" />
                <asp:Button ID="BtnReset"  runat="server" Text="清除"        CssClass="btn btn-sub"  OnClick="BtnReset_Click" />
                <asp:Button ID="BtnExport" runat="server" Text="匯出 Excel"  CssClass="btn btn-warn" OnClick="BtnExport_Click" />
            </div>
        </div>

        <div class="card">
            <h2 class="card-title">查詢結果 (<asp:Label ID="LblResultCount" runat="server" Text="0" /> 筆)</h2>
            <asp:GridView ID="GvResult" runat="server" AutoGenerateColumns="false"
                CssClass="tbl" GridLines="None" EmptyDataText="無資料"
                AllowPaging="true" PageSize="20"
                OnPageIndexChanging="GvResult_PageIndexChanging">
                <Columns>
                    <asp:TemplateField HeaderText="申請單號" ItemStyle-CssClass="ctr">
                        <ItemTemplate>
                            <a href='<%# NavUrl("View.aspx?no=" + Eval("ApplyNo")) %>'><%# Eval("ApplyNo") %></a>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="ApplyDate"    HeaderText="申請日期" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-CssClass="ctr" />
                    <asp:BoundField DataField="BranchName"   HeaderText="單位"     ItemStyle-CssClass="ctr" />
                    <asp:BoundField DataField="CustomerName" HeaderText="客戶名稱" />
                    <asp:BoundField DataField="TotalAmount"  HeaderText="申請金額" DataFormatString="{0:N0}" ItemStyle-CssClass="num" />
                    <asp:TemplateField HeaderText="狀態" ItemStyle-CssClass="ctr">
                        <ItemTemplate>
                            <span class='status status-<%# Eval("Status") %>'><%# StatusName(Convert.ToInt32(Eval("Status"))) %></span>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="RateTypeCode"   HeaderText="利率別" ItemStyle-CssClass="ctr" />
                    <asp:BoundField DataField="CreateUserName" HeaderText="建單人" ItemStyle-CssClass="ctr" />
                </Columns>
                <PagerStyle CssClass="ctr" />
            </asp:GridView>
        </div>
    </div>
    <div class="footer">© 經營管理處 - 新臺幣定期存款專案利率申請</div>
</form>
</body>
</html>
