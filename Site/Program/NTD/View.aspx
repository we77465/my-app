<%@ Page Language="C#" AutoEventWireup="true" CodeFile="View.aspx.cs" Inherits="NtdView" %>
<%@ Register TagPrefix="ntd" TagName="ApplyDetail" Src="~/Program/NTD/Detail.ascx" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>新臺幣定期存款專案利率申請 - 檢視</title>
    <style>
        /* ===== 共用版型 (明細卡片樣式由 Detail.ascx 自帶) ===== */
        * { box-sizing: border-box; }
        body { font-family: "Microsoft JhengHei", "微軟正黑體", Arial, sans-serif; font-size: 14px; color: #222; background: #f4f6f9; margin: 0; }
        a { color: #1565c0; text-decoration: none; }
        a:hover { text-decoration: underline; }
        .topbar { background: #1565c0; color: #fff; padding: 10px 18px; display: flex; align-items: center; justify-content: space-between; }
        .topbar h1 { margin: 0; font-size: 18px; font-weight: 600; }
        .topbar .user { font-size: 13px; }
        .nav { background: #fff; padding: 0 18px; border-bottom: 1px solid #e0e0e0; }
        .nav a { padding: 12px 20px; color: #555; display: inline-block; border-bottom: 3px solid transparent; }
        .nav a:hover { background: #f0f4ff; }
        .container { padding: 18px; }
        .footer { text-align: center; color: #888; font-size: 12px; padding: 16px; }

        /* ===== 工具列 / 按鈕 ===== */
        .toolbar { padding: 8px 0; display: flex; gap: 6px; align-items: center; }
        .btn { display: inline-block; padding: 6px 16px; border: 1px solid #1565c0; background: #1565c0; color: #fff; border-radius: 3px; cursor: pointer; font-size: 13px; font-family: inherit; }
        .btn:hover { background: #0d47a1; }
        .btn-sub { background: #757575; border-color: #757575; }
        .btn-sub:hover { background: #424242; }

        /* ===== 操作訊息 ===== */
        .alert { padding: 10px 12px; margin-bottom: 12px; border-radius: 4px; }
        .alert-err { background: #ffebee; color: #b71c1c; border: 1px solid #ef9a9a; }
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
        <a href="<%= NavUrl("Approve.aspx") %>">簽核/決行</a>
        <a href="<%= NavUrl("Report.aspx") %>">報表</a>
    </div>
    <div class="container">
        <asp:Label ID="LblMessage" runat="server" Visible="false" />
        <ntd:ApplyDetail ID="UcApplyDetail" runat="server" />
        <div class="toolbar">
            <asp:Button ID="BtnBack"  runat="server" Text="返回" CssClass="btn btn-sub" OnClick="BtnBack_Click" />
            <asp:Button ID="BtnPrint" runat="server" Text="列印" CssClass="btn"
                OnClientClick="window.print(); return false;" />
        </div>
    </div>
    <div class="footer">© 經營管理處 - 新臺幣定期存款專案利率申請</div>
</form>
</body>
</html>
