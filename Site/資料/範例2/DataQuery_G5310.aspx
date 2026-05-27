<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataQuery_G5310.aspx.cs" Inherits="Program_LargeMoney_DataQuery_G5310" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www. w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>大額資金通報(G5310)-查詢</title>
    <link rel="stylesheet" type="text/css" href="../Common/css/Common.css" />
    <style>
    body {
        font-family: Arial, sans-serif;
        margin: 0;
        padding: 20px;
        background-color: #f5f5f5;
    }

    .container {
        max-width: 1400px;
        margin: 0 auto;
        /* ★ 確保不裁切 */
        overflow: visible;
    }

    .search-panel {
        background-color: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        margin-bottom: 20px;
        /* ★ 關鍵：確保不裁切下拉選單 */
        overflow:  visible;
    }

    .search-panel h3 {
        margin-top: 0;
        color: #333;
        border-bottom: 2px solid #4CAF50;
        padding-bottom:  10px;
    }

    . search-row {
        display: flex;
        flex-wrap: wrap;
        gap: 15px;
        margin-bottom: 15px;
        /* ★ 關鍵：確保不裁切 */
        overflow:  visible;
    }

    .search-item {
        display: flex;
        align-items: center;
        gap: 8px;
        /* ★ 關鍵：確保不裁切 */
        overflow: visible;
        /* ★ 給下拉選單足夠空間 */
        position: relative;
        z-index: 1;
    }

    .search-item label {
        min-width: 80px;
        font-weight: bold;
        color: #555;
        white-space: nowrap;
    }

    /* ===== 輸入框樣式 ===== */
    .search-item input[type="text"],
    .search-item input[type="date"] {
        padding: 8px 12px;
        border: 1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        min-width: 120px;
    }

    /* ===== 下拉選單樣式 (修正版) ===== */
    .search-item select {
        padding: 8px 12px;
        border:  1px solid #ddd;
        border-radius: 4px;
        font-size: 14px;
        min-width: 150px;
        /* ★ 關鍵：確保原生下拉選單樣式 */
        -webkit-appearance: menulist;
        -moz-appearance:  menulist;
        appearance: menulist;
        background-color: white;
        cursor: pointer;
        /* ★ 不限制高度 */
        height: auto;
        line-height: normal;
    }

    .search-item input[type="text"]:focus,
    .search-item input[type="date"]:focus,
    .search-item select:focus {
        outline: none;
        border-color: #4CAF50;
        box-shadow: 0 0 5px rgba(76, 175, 80, 0.3);
    }

    /* ===== 按鈕區塊 ===== */
    .btn-container {
        text-align: center;
        padding: 15px 0;
    }

    .btn {
        padding: 10px 25px;
        margin: 0 8px;
        font-size: 14px;
        cursor: pointer;
        border: none;
        border-radius:  4px;
        transition: background-color 0.3s;
    }

    .btn-search {
        background-color: #4CAF50;
        color: white;
    }

    . btn-search:hover {
        background-color: #45a049;
    }

    .btn-reset {
        background-color: #f44336;
        color: white;
    }

    .btn-reset:hover {
        background-color: #da190b;
    }

    .btn-add {
        background-color: #2196F3;
        color: white;
    }

    .btn-add:hover {
        background-color: #1976D2;
    }

    .btn-export {
        background-color: #FF9800;
        color: white;
    }

    .btn-export:hover {
        background-color: #F57C00;
    }

    /* ===== 查詢結果區塊 ===== */
    .result-panel {
        background-color: white;
        padding: 20px;
        border-radius: 8px;
        box-shadow: 0 2px 4px rgba(0,0,0,0.1);
    }

    .result-panel h3 {
        margin-top: 0;
        color: #333;
        border-bottom: 2px solid #2196F3;
        padding-bottom:  10px;
    }

    .result-info {
        margin-bottom: 15px;
        color: #666;
    }

    .grid-container {
        overflow-x: auto;
    }

    /* ===== 資料表格 ===== */
    .data-grid {
        width: 100%;
        border-collapse:  collapse;
        font-size: 13px;
    }

    .data-grid th {
        background-color: #4CAF50;
        color: white;
        padding: 12px 8px;
        text-align: center;
        border:  1px solid #ddd;
        white-space: nowrap;
    }

    .data-grid td {
        padding: 10px 8px;
        border: 1px solid #ddd;
        text-align: center;
    }

    .data-grid tr:nth-child(even) {
        background-color: #f9f9f9;
    }

    .data-grid tr:hover {
        background-color: #e8f5e9;
    }

    /* ===== 操作按鈕 ===== */
    .action-btn {
        padding: 5px 10px;
        margin: 2px;
        font-size: 12px;
        cursor: pointer;
        border: none;
        border-radius:  3px;
        color: white !important;
        text-decoration: none;
        display: inline-block;
    }

    .btn-view {
        background-color: slategrey;
    }

    .btn-view:hover {
        background-color: #7B1FA2;
    }

    .btn-edit {
        background-color: #2196F3;
    }

    .btn-edit:hover {
        background-color: #1976D2;
    }

    .btn-release {
        background-color: #4CAF50;
    }

    .btn-release:hover {
        background-color: #388E3C;
    }

    .btn-delete {
        background-color:  #f44336;
    }

    .btn-delete:hover {
        background-color: #da190b;
    }

    /* ===== 狀態樣式 ===== */
    .status-pending {
        color: #FF9800;
        font-weight: bold;
    }

    .status-released {
        color:  #4CAF50;
        font-weight: bold;
    }

    /* ===== 金額樣式 ===== */
    .amount-in {
        color: #4CAF50;
        font-weight:  bold;
    }

    .amount-out {
        color: #f44336;
        font-weight: bold;
    }

    /* ===== 分頁 ===== */
    .pager {
        margin-top: 20px;
        text-align: center;
    }

    .pager a, .pager span {
        display: inline-block;
        padding: 8px 12px;
        margin:  0 3px;
        border: 1px solid #ddd;
        border-radius:  4px;
        text-decoration:  none;
        color: #333;
    }

    .pager a:hover {
        background-color: #4CAF50;
        color: white;
        border-color: #4CAF50;
    }

    .pager .current {
        background-color: #4CAF50;
        color: white;
        border-color: #4CAF50;
    }

    /* ===== 使用者資訊 ===== */
    .user-info {
        background-color: #e3f2fd;
        padding: 10px 15px;
        border-radius: 4px;
        margin-bottom: 15px;
        font-size: 14px;
    }

    .user-info span {
        margin-right: 20px;
    }

    .role-manager {
        color: #4CAF50;
        font-weight:  bold;
    }

    .role-normal {
        color:  #2196F3;
        font-weight: bold;
    }

    /* ===== 確保所有容器不裁切下拉選單 ===== */
    form, div, . container, .search-panel, .search-row, .search-item {
        overflow: visible ! important;
    }
</style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <!-- 使用者資訊 -->
            <div class="user-info" hidden>
                <span>員工編號: <asp:Label ID="lblEmpId" runat="server"></asp:Label></span>
                <span>姓名: <asp:Label ID="lblEmpName" runat="server"></asp:Label></span>
                <span>分行: <asp:Label ID="lblBrno" runat="server"></asp:Label></span>
                <span>角色: <asp:Label ID="lblRole" runat="server"></asp:Label></span>
            </div>

            <!-- 查詢條件區塊 -->
            <div class="search-panel">
                <h3>大額資金通報(G5310)-查詢條件</h3>
                
                <div class="search-row">
                    <div class="search-item">
                        <label>通報日期:</label>
                        <asp:TextBox ID="txtReportDateFrom" runat="server" TextMode="Date"></asp:TextBox>
                        <span>~</span>
                        <asp:TextBox ID="txtReportDateTo" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>交割日期:</label>
                        <asp:TextBox ID="txtStdayFrom" runat="server" TextMode="Date"></asp:TextBox>
                        <span>~</span>
                        <asp:TextBox ID="txtStdayTo" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
                
                <div class="search-row">
                    <div class="search-item">
                        <label>統一編號:</label>
                        <asp:TextBox ID="txtSearchUnino" runat="server" MaxLength="11"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>客戶名稱:</label>
                        <asp:TextBox ID="txtSearchCustomerName" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                     <div class="search-item">
                         <label>案件編號:</label>
                         <asp:TextBox ID="txtHostSrno" runat="server" MaxLength="4"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>暫時編號:</label>
                        <asp:TextBox ID="txtSearchSrno" runat="server" MaxLength="4"></asp:TextBox>
                    </div>
                     
                </div>
                
                <div class="search-row">
                    <div class="search-item">
                        <label>通報行: </label>
                        <asp:DropDownList ID="ddlSearchIbrno" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="search-item">
                        <label>設帳行:</label>
                        <asp:DropDownList ID="ddlSearchAcbrno" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="search-item">
                        <label>幣別:</label>
                        <asp:DropDownList ID="ddlSearchCurcd" runat="server">
                        </asp:DropDownList>
                    </div>
                    <div class="search-item">
                        <label>匯出/匯入:</label>
                        <asp:DropDownList ID="ddlSearchIocd" runat="server">
                            <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            <asp:ListItem Text="匯入" Value="1"></asp:ListItem>
                            <asp:ListItem Text="匯出" Value="2"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="search-row">
                    <div class="search-item">
                        <label>狀態:</label>
                        <asp:DropDownList ID="ddlSearchStatus" runat="server">
                            <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            <asp:ListItem Text="待放行" Value="0"></asp:ListItem>
                            <asp:ListItem Text="已放行" Value="1"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                
                <div class="btn-container">
                    <asp:Button ID="btnSearch" runat="server" Text="查詢" CssClass="btn btn-search" OnClick="BtnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="清除條件" CssClass="btn btn-reset" OnClick="BtnClear_Click" CausesValidation="false" />
                    <asp:Button ID="btnAdd" runat="server" Text="新增填報" CssClass="btn btn-add" OnClick="BtnAdd_Click" CausesValidation="false" />
                    <asp:Button ID="btnExport" runat="server" Text="匯出Excel" CssClass="btn btn-export" OnClick="BtnExport_Click" CausesValidation="false" />
                </div>
            </div>
            
            <!-- 查詢結果區塊 -->
            <div class="result-panel">
                <h3>查詢結果</h3>
                <div class="result-info">
                    <asp:Label ID="lblResultInfo" runat="server" Text="請輸入查詢條件後按查詢按鈕"></asp:Label>
                </div>
                
                <div class="grid-container">
                    <asp:GridView ID="gvData" runat="server" AutoGenerateColumns="False" 
                        CssClass="data-grid" AllowPaging="True" PageSize="20"
                        OnPageIndexChanging="GvData_PageIndexChanging"
                        OnRowCommand="GvData_RowCommand"
                        OnRowDataBound="GvData_RowDataBound"
                        DataKeyNames="ID"
                        EmptyDataText="查無資料">
                        <Columns>
                            <asp:TemplateField HeaderText="操作">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lnkView" runat="server" CommandName="ViewDetail" 
                                        CommandArgument='<%# Eval("ID") %>' CssClass="action-btn btn-view">檢視</asp:LinkButton>
                                    <asp:LinkButton ID="lnkEdit" runat="server" CommandName="EditData" 
                                        CommandArgument='<%# Eval("ID") %>' CssClass="action-btn btn-edit">編輯</asp:LinkButton>
                                    <asp:LinkButton ID="lnkRelease" runat="server" CommandName="ReleaseData" 
                                        CommandArgument='<%# Eval("ID") %>' CssClass="action-btn btn-release"
                                        OnClientClick="return confirm('確定要放行此筆資料嗎？');">放行</asp:LinkButton>
                                    <asp:LinkButton ID="lnkDelete" runat="server" CommandName="DeleteData" 
                                        CommandArgument='<%# Eval("ID") %>' CssClass="action-btn btn-delete"
                                        OnClientClick="return confirm('確定要刪除此筆資料嗎？');">刪除</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Host_SRNO" HeaderText="案件編號" />
                            <%--<asp:BoundField DataField="KIND" HeaderText="功能" />--%>
                            <asp:TemplateField HeaderText="狀態">
                                <ItemTemplate>
                                    <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--<asp:BoundField DataField="REPORT_DATE" HeaderText="通報日期" DataFormatString="{0:yyyy/MM/dd}" />
                            <asp:BoundField DataField="STDAY" HeaderText="交割日期" DataFormatString="{0:yyyy/MM/dd}" />--%>
                            <asp:BoundField DataField="UNINO" HeaderText="統一編號" />
                            <asp:BoundField DataField="Customer_Name" HeaderText="客戶名稱" />
                            <asp:TemplateField HeaderText="通報行">
                                <ItemTemplate>
                                    <asp:Label ID="lblIbrno" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="設帳行">
                                <ItemTemplate>
                                    <asp:Label ID="lblAcbrno" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CURCD" HeaderText="幣別" />
                            <asp:TemplateField HeaderText="金額">
                                <ItemTemplate>
                                    <asp:Label ID="lblAmt" runat="server"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="IOCD" HeaderText="匯出/匯入" />
                            <%--<asp:BoundField DataField="AMTWHERE" HeaderText="資金來源/去處" />--%>
                            <asp:BoundField DataField="EMPNM" HeaderText="經辦人員" />
                            <%--<asp:BoundField DataField="Created_Time" HeaderText="建立時間" DataFormatString="{0:yyyy/MM/dd HH:mm}" />--%>
                            <asp:BoundField DataField="ID" HeaderText="暫時編號" />
                        </Columns>
                        <PagerStyle CssClass="pager" />
                    </asp:GridView>
                </div>
            </div>
        </div>
    </form>
</body>
</html>