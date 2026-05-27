<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataList.aspx.cs" Inherits="Program_LargeMoney_DataList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>大額資金通報-查詢</title>
    <link rel="stylesheet" type="text/css" href="../Common/css/Common.css" />
    <style>
        .container {
            width: 95%;
            margin: 20px auto;
            font-family: Arial, sans-serif;
        }
        
        .search-panel {
            background-color: #f5f5f5;
            padding: 20px;
            border-radius: 5px;
            margin-bottom: 20px;
            border: 1px solid #ddd;
        }

        .search-row {
            display: flex;
            flex-wrap: wrap;
            gap: 15px;
            margin-bottom: 15px;
            align-items: center;
        }

        .search-item {
            display: flex;
            align-items: center;
            gap: 8px;
        }

        .search-item label {
            font-weight: bold;
            white-space: nowrap;
        }

        .search-item input,
        .search-item select {
            padding: 6px 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }

        .btn-search {
            background-color: #4CAF50;
            color: white;
            padding: 8px 25px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-search:hover {
            background-color: #45a049;
        }

        .btn-reset {
            background-color: #f44336;
            color: white;
            padding: 8px 25px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-reset:hover {
            background-color: #da190b;
        }

        .btn-add {
            background-color: #2196F3;
            color: white;
            padding: 8px 25px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
        }

        .btn-add:hover {
            background-color: #0b7dda;
        }

        .grid-container {
            overflow-x: auto;
            background-color: white;
            border-radius: 5px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .data-grid {
            width: 100%;
            border-collapse: collapse;
            min-width: 1200px;
        }

        .data-grid th {
            background-color: dimgrey;
            color: white;
            padding: 12px 8px;
            text-align: center;
            font-weight: bold;
            position: sticky;
            top: 0;
            z-index: 10;
        }

        .data-grid td {
            padding: 10px 8px;
            border-bottom: 1px solid #ddd;
            text-align: center;
        }

        .data-grid tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .data-grid tr:hover {
            background-color: #f5f5f5;
        }

        .btn-edit, .btn-delete {
            padding: 5px 12px;
            margin: 0 3px;
            border: none;
            border-radius: 3px;
            cursor: pointer;
            font-size: 13px;
        }

        .btn-edit {
            background-color: #2196F3;
            color: white;
        }

        .btn-edit:hover {
            background-color: #0b7dda;
        }

        .btn-delete {
            background-color: #f44336;
            color: white;
        }

        .btn-delete:hover {
            background-color: #da190b;
        }

        .no-data {
            text-align: center;
            padding: 40px;
            color: #999;
            font-size: 16px;
        }

        .record-count {
            margin: 10px 0;
            font-weight: bold;
            color: #333;
        }

        .amount-export {
            color: #f44336;
            font-weight: bold;
        }

        .amount-import {
            color: black;
            font-weight: bold;
        }

        .header-title {

            color: black;
            padding: 15px;
            text-align: center;
            font-size: 20px;
            font-weight: bold;
            border-radius: 5px;
            margin-bottom: 20px;
        }

        .pager {
            text-align: center;
            padding: 15px;
            background-color: #f5f5f5;
            border-radius: 5px;
            margin-top: 10px;
        }

        .pager a,
        .pager span {
            padding: 5px 10px;
            margin: 0 3px;
            border: 1px solid #ddd;
            background-color: white;
            color: #333;
            text-decoration: none;
            border-radius: 3px;
        }

        .pager a:hover {
            background-color: #4CAF50;
            color: white;
        }

        .pager span {
            background-color: #4CAF50;
            color: white;
            font-weight: bold;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="header-title">
                大額資金通報-查詢
            </div>

            <!-- 查詢條件區 -->
            <div class="search-panel">
                <div class="search-row">
                    <div class="search-item">
                        <label>統一編號：</label>
                        <asp:TextBox ID="txtSearchTaxID" runat="server" MaxLength="25"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>客戶名稱：</label>
                        <asp:TextBox ID="txtSearchCustomerName" runat="server" MaxLength="50"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>幣別：</label>
                        <asp:TextBox ID="txtSearchCurrency" runat="server" MaxLength="10"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>匯出/匯入：</label>
                        <asp:DropDownList ID="ddlSearchExportImport" runat="server">
                            <asp:ListItem Text="全部" Value=""></asp:ListItem>
                            <asp:ListItem Text="匯出" Value="Export"></asp:ListItem>
                            <asp:ListItem Text="匯入" Value="Import"></asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div>
                <div class="search-row">
                    <div class="search-item">
                        <label>通報日起：</label>
                        <asp:TextBox ID="txtSearchNotifyDateStart" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>通報日迄：</label>
                        <asp:TextBox ID="txtSearchNotifyDateEnd" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>交割日起：</label>
                        <asp:TextBox ID="txtSearchDeliveryDateStart" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                    <div class="search-item">
                        <label>交割日迄：</label>
                        <asp:TextBox ID="txtSearchDeliveryDateEnd" runat="server" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
                <div class="search-row">
                    <asp:Button ID="btnSearch" runat="server" Text="查詢"  OnClick="BtnSearch_Click" />
                    <asp:Button ID="btnClearSearch" runat="server" Text="清除條件" OnClick="BtnClearSearch_Click" />
                    <asp:Button ID="btnAddNew" runat="server" Text="新增資料"  OnClick="BtnAddNew_Click" CausesValidation="false" />
                </div>
            </div>

            <!-- 記錄數量 -->
            <div class="record-count">
                <asp:Label ID="lblRecordCount" runat="server" Text=""></asp:Label>
            </div>

            <!-- 資料列表區 -->
            <div class="grid-container">
                <asp:GridView ID="gvLargeMoneyData" runat="server" 
                    CssClass="data-grid"
                    AutoGenerateColumns="False" 
                    AllowPaging="True" 
                    PageSize="20"
                    OnPageIndexChanging="GvLargeMoneyData_PageIndexChanging"
                    OnRowCommand="GvLargeMoneyData_RowCommand"
                    OnRowDataBound="GvLargeMoneyData_RowDataBound"
                    DataKeyNames="ID"
                    EmptyDataText="查無資料">
                    <Columns>
                        <asp:BoundField DataField="ID" HeaderText="序號" ItemStyle-Width="50px" />
                        <asp:BoundField DataField="Tax_ID_Number" HeaderText="統一編號" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Customer_Name" HeaderText="客戶名稱" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="Notify_Date" HeaderText="通報日" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Delivery_Date" HeaderText="交割日" DataFormatString="{0:yyyy-MM-dd}" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Notify_Bank" HeaderText="通報行" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Account_Bank" HeaderText="設帳行" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Currency" HeaderText="幣別" ItemStyle-Width="60px" />
                        <asp:BoundField DataField="Bank_Code" HeaderText="存匯行代號" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Amount" HeaderText="金額" DataFormatString="{0:N2}" ItemStyle-Width="120px" />
                        <asp:BoundField DataField="Export_Import" HeaderText="匯出/匯入" ItemStyle-Width="80px" />
                        <asp:BoundField DataField="Funds_Sources_Destinations" HeaderText="資金來源/去處" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="Change_Reason" HeaderText="變動原因" ItemStyle-Width="150px" />
                        <asp:BoundField DataField="Created_By" HeaderText="建立者" ItemStyle-Width="100px" />
                        <asp:BoundField DataField="Created_Time" HeaderText="建立時間" DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-Width="130px" />
                        <asp:TemplateField HeaderText="操作" ItemStyle-Width="120px">
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" Text="編輯" 
                                    
                                    CommandName="EditRow" 
                                    CommandArgument='<%# Eval("ID") %>' />
                                <asp:Button ID="btnDelete" runat="server" Text="刪除" 
                                    CssClass="btn-delete" 
                                    CommandName="DeleteRow" 
                                    CommandArgument='<%# Eval("ID") %>'
                                    OnClientClick="return confirm('確定要刪除此筆資料嗎？');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <PagerStyle CssClass="pager" />
                    <EmptyDataTemplate>
                        <div class="no-data">查無符合條件的資料</div>
                    </EmptyDataTemplate>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>

