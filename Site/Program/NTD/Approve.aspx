<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Approve.aspx.cs" Inherits="NtdApprove" %>
<%@ Register TagPrefix="ntd" TagName="ApplyDetail" Src="~/Program/NTD/Detail.ascx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>新臺幣定期存款專案利率申請 - 簽核/決行</title>
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

        /* ===== 待處理 GridView ===== */
        .tbl { width: 100%; border-collapse: collapse; }
        .tbl th, .tbl td { border: 1px solid #d0d0d0; padding: 6px 8px; font-size: 13px; }
        .tbl th { background: #e3f2fd; font-weight: bold; color: #1565c0; text-align: center; }
        .tbl tr:nth-child(even) td { background: #fafafa; }
        .tbl tr:hover td { background: #fff8e1; }
        .tbl .num { text-align: right; }
        .tbl .ctr { text-align: center; }

        /* ===== 簽核作業 form ===== */
        .form { width: 100%; border-collapse: collapse; }
        .form th { width: 160px; background: #e3f2fd; color: #1565c0; text-align: right; padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form td { padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form select, .form textarea { border: 1px solid #bdbdbd; border-radius: 3px; padding: 4px 6px; font-size: 13px; font-family: inherit; }
        .form .req { color: #d32f2f; }

        /* ===== 按鈕 / 工具列 ===== */
        .toolbar { padding: 8px 0; display: flex; gap: 6px; align-items: center; }
        .btn { display: inline-block; padding: 6px 16px; border: 1px solid #1565c0; background: #1565c0; color: #fff; border-radius: 3px; cursor: pointer; font-size: 13px; font-family: inherit; }
        .btn:hover { background: #0d47a1; }
        .btn-sub { background: #757575; border-color: #757575; }
        .btn-sub:hover { background: #424242; }
        .btn-ok { background: #388e3c; border-color: #388e3c; }
        .btn-ok:hover { background: #1b5e20; }
        .btn-danger { background: #d32f2f; border-color: #d32f2f; }
        .btn-danger:hover { background: #b71c1c; }

        /* ===== 狀態徽章 ===== */
        .status { display: inline-block; padding: 2px 8px; border-radius: 12px; font-size: 12px; color: #fff; }
        .status-0 { background: #9e9e9e; }
        .status-1 { background: #1976d2; }
        .status-2 { background: #0288d1; }
        .status-4 { background: #f57c00; }
        .status-6 { background: #c62828; }
        .status-9 { background: #388e3c; }

        /* ===== 操作訊息 ===== */
        .alert { padding: 10px 12px; margin-bottom: 12px; border-radius: 4px; }
        .alert-info { background: #e3f2fd; color: #0d47a1; border: 1px solid #90caf9; }
        .alert-ok   { background: #e8f5e9; color: #1b5e20; border: 1px solid #a5d6a7; }
        .alert-err  { background: #ffebee; color: #b71c1c; border: 1px solid #ef9a9a; }
        .alert-warn { background: #fff3e0; color: #e65100; border: 1px solid #ffcc80; }
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
        <a href="<%= NavUrl("Query.aspx") %>">查詢</a>
        <a href="<%= NavUrl("Approve.aspx") %>" class="active">簽核/決行</a>
        <a href="<%= NavUrl("Report.aspx") %>">報表</a>
    </div>

    <div class="container">
        <asp:Label ID="LblMessage" runat="server" Visible="false" />

        <asp:MultiView ID="MvApprove" runat="server" ActiveViewIndex="0">

        <asp:View ID="VwPendingList" runat="server">
            <div class="card">
                <h2 class="card-title">待我處理的申請單</h2>
                <div class="alert alert-info">
                    您的職位：<b><%= Me.Title %></b>
                    &nbsp;|&nbsp; 可處理狀態：<b><%= GetMyHandlingStatusName() %></b>
                </div>
                <asp:GridView ID="GvPendingList" runat="server" AutoGenerateColumns="false"
                    CssClass="tbl" GridLines="None" EmptyDataText="目前無待處理的單"
                    OnRowCommand="GvPendingList_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="ApplyNo"      HeaderText="申請單號"  ItemStyle-CssClass="ctr" />
                        <asp:BoundField DataField="ApplyDate"    HeaderText="申請日期"  DataFormatString="{0:yyyy-MM-dd}" ItemStyle-CssClass="ctr" />
                        <asp:BoundField DataField="BranchName"   HeaderText="單位"     ItemStyle-CssClass="ctr" />
                        <asp:BoundField DataField="CustomerName" HeaderText="客戶名稱" />
                        <asp:BoundField DataField="TotalAmount"  HeaderText="申請金額"  DataFormatString="{0:N0}" ItemStyle-CssClass="num" />
                        <asp:TemplateField HeaderText="狀態" ItemStyle-CssClass="ctr">
                            <ItemTemplate>
                                <span class='status status-<%# Eval("Status") %>'><%# StatusName(Convert.ToInt32(Eval("Status"))) %></span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="CreateUserName" HeaderText="建單人" ItemStyle-CssClass="ctr" />
                        <asp:TemplateField HeaderText="動作" ItemStyle-CssClass="ctr">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" Text="處理"
                                    CommandName="HandleItem"
                                    CommandArgument='<%# Eval("ApplyNo") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </asp:View>

        <asp:View ID="VwHandle" runat="server">
            <ntd:ApplyDetail ID="UcApplyDetail" runat="server" />

            <div class="card">
                <h2 class="card-title">簽核作業</h2>
                <table class="form">
                    <asp:PlaceHolder ID="PhRateType" runat="server" Visible="false">
                    <tr>
                        <th>利率別代號 <span class="req">*</span></th>
                        <td colspan="3">
                            <asp:DropDownList ID="DdlRateType" runat="server" />
                            <span style="color:#888">(總行科長決行需填此欄)</span>
                        </td>
                    </tr>
                    </asp:PlaceHolder>
                    <tr>
                        <th>簽核意見</th>
                        <td colspan="3">
                            <asp:TextBox ID="TxtComment" runat="server" TextMode="MultiLine"
                                Width="96%" Height="80" MaxLength="500" />
                        </td>
                    </tr>
                </table>
                <div class="toolbar" style="margin-top:14px">
                    <asp:Button ID="BtnApprove" runat="server" Text="放行" CssClass="btn btn-ok"
                        OnClick="BtnApprove_Click"
                        OnClientClick="return confirm('確定放行至下一關？');" />
                    <asp:Button ID="BtnReject"  runat="server" Text="退回" CssClass="btn btn-danger"
                        OnClick="BtnReject_Click"
                        OnClientClick="return confirm('確定退回給經辦修改？');" />
                    <asp:Button ID="BtnCancel"  runat="server" Text="返回列表" CssClass="btn btn-sub"
                        OnClick="BtnCancel_Click" CausesValidation="false" />
                </div>
            </div>
        </asp:View>

        </asp:MultiView>
    </div>
    <div class="footer">© 經營管理處 - 新臺幣定期存款專案利率申請</div>
</form>
</body>
</html>
