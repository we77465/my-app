<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DataView_G5310.aspx.cs" Inherits="Program_LargeMoney_DataView_G5310" %>

<! DOCTYPE html>

<html xmlns="http://www. w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>大額資金通報(G5310)-檢視</title>
    <link rel="stylesheet" type="text/css" href="../Common/css/Common.css" />
    <style>
                /* 針對 DataQuery_G5310.aspx */
        .search-item select {
            width: 180px;  /* 設定固定寬度或適當的寬度 */
            min-width: 120px;
            max-width: 250px;
        }

        /* 針對 DataAdd_G5310.aspx */
        select {
            width: auto;  /* 改為 auto 或特定寬度 */
            min-width: 150px;
            max-width: 300px;
        }

        /* 添加下拉選項樣式 */
        select option {
            padding: 8px;
            white-space: nowrap;  /* 防止文字換行 */
            overflow: hidden;
            text-overflow: ellipsis;
        }

        /* 針對下拉選單展開時的樣式 */
        select:focus {
            width: auto;
            min-width: 200px;  /* 展開時有足夠寬度 */
        }
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 20px;
            background-color: #f5f5f5;
        }

        .container {
            max-width: 1000px;
            margin: 0 auto;
        }

        .view-panel {
            background-color: white;
            padding: 20px;
            border-radius: 8px;
            box-shadow: 0 2px 4px rgba(0,0,0,0.1);
        }

        .view-panel h3 {
            margin-top: 0;
            color: #333;
            border-bottom: 2px solid #9C27B0;
            padding-bottom: 10px;
            text-align: center;
        }

        .view-table {
            width: 100%;
            border-collapse: collapse;
            margin-top: 20px;
        }

        . view-table th {
            background-color: #f5f5f5;
            padding: 12px 15px;
            text-align: left;
            border: 1px solid #ddd;
            width: 20%;
            color: #555;
            font-weight: bold;
        }

        .view-table td {
            padding: 12px 15px;
            border: 1px solid #ddd;
            width: 30%;
        }

        .section-header {
            background-color: #9C27B0 !important;
            color: white !important;
            text-align: center !important;
            font-size: 14px;
        }

        .status-pending {
            color: #FF9800;
            font-weight: bold;
            font-size: 16px;
        }

        . status-released {
            color: #4CAF50;
            font-weight: bold;
            font-size: 16px;
        }

        .amount-in {
            color: #4CAF50;
            font-weight: bold;
            font-size: 16px;
        }

        .amount-out {
            color: #f44336;
            font-weight: bold;
            font-size: 16px;
        }

        .btn-container {
            text-align: center;
            padding: 20px;
            margin-top: 20px;
        }

        .btn {
            padding: 10px 30px;
            margin: 0 10px;
            font-size: 14px;
            cursor: pointer;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            display: inline-block;
        }

        .btn-back {
            background-color: #2196F3;
            color: white;
        }

        . btn-back:hover {
            background-color: #1976D2;
        }

        . btn-edit {
            background-color: #FF9800;
            color: white;
        }

        . btn-edit:hover {
            background-color: #F57C00;
        }

        . btn-release {
            background-color: #4CAF50;
            color: white;
        }

        .btn-release:hover {
            background-color: #388E3C;
        }

        .btn-print {
            background-color: #607D8B;
            color: white;
        }

        . btn-print:hover {
            background-color: #455A64;
        }

        .highlight-value {
            font-size: 15px;
            font-weight: bold;
        }

        @media print {
            .btn-container {
                display: none;
            }
            body {
                background-color: white;
                padding: 0;
            }
            .view-panel {
                box-shadow: none;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <div class="view-panel">
                <h3>大額資金通報(G5310)-檢視</h3>
                
                <table class="view-table">
                    <!-- 基本資訊 -->
                    <tr>
                        <th colspan="4" class="section-header">基本資訊</th>
                    </tr>
                    
                    <tr>
                        <th>主機案件編號</th>
                        <td colspan="3"><asp:Label ID="lblHostSrno" runat="server" CssClass="highlight-value"></asp:Label></td>
                        <th>主機狀態</th>
                        <td><asp:Label ID="lblHostStatus" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>案件編號</th>
                        <td><asp:Label ID="lblSrno" runat="server" CssClass="highlight-value"></asp:Label></td>
                        <th>狀態</th>
                        <td><asp:Label ID="lblStatus" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>功能</th>
                        <td><asp:Label ID="lblKind" runat="server"></asp:Label></td>
                        <th>案件種類</th>
                        <td><asp:Label ID="lblApType" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>通報日期</th>
                        <td><asp:Label ID="lblReportDate" runat="server"></asp:Label></td>
                        <th>交割日期</th>
                        <td><asp:Label ID="lblStday" runat="server"></asp:Label></td>
                    </tr>

                    <!-- 客戶資訊 -->
                    <tr>
                        <th colspan="4" class="section-header">客戶資訊</th>
                    </tr>
                    <tr>
                        <th>統一編號</th>
                        <td><asp:Label ID="lblUnino" runat="server" CssClass="highlight-value"></asp:Label></td>
                        <th>客戶名稱</th>
                        <td><asp:Label ID="lblCustomerName" runat="server"></asp:Label></td>
                    </tr>

                    <!-- 交易資訊 -->
                    <tr>
                        <th colspan="4" class="section-header">交易資訊</th>
                    </tr>
                    <tr>
                        <th>通報行</th>
                        <td><asp:Label ID="lblIbrno" runat="server"></asp:Label></td>
                        <th>掛帳行</th>
                        <td><asp:Label ID="lblAcbrno" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>幣別</th>
                        <td><asp:Label ID="lblCurcd" runat="server"></asp:Label></td>
                        <th>存匯行幣別</th>
                        <td><asp:Label ID="lblBkcur" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>存匯行流水號</th>
                        <td><asp:Label ID="lblBksrno" runat="server"></asp:Label></td>
                        <th>匯出/匯入</th>
                        <td><asp:Label ID="lblIocd" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>金額</th>
                        <td colspan="3"><asp:Label ID="lblAmt" runat="server" CssClass="highlight-value"></asp:Label></td>
                    </tr>

                    <!-- 資金說明 -->
                    <tr>
                        <th colspan="4" class="section-header">資金說明</th>
                    </tr>
                    <tr>
                        <th>資金來源/去處</th>
                        <td colspan="3"><asp:Label ID="lblAmtwhere" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>變動原因內容</th>
                        <td colspan="3"><asp:Label ID="lblReason" runat="server"></asp:Label></td>
                    </tr>

                    <!-- 經辦人員 -->
                    <tr>
                        <th colspan="4" class="section-header">經辦人員</th>
                    </tr>
                    <tr>
                        <th>員工編號</th>
                        <td><asp:Label ID="lblEmpno" runat="server"></asp:Label></td>
                        <th>員工姓名</th>
                        <td><asp:Label ID="lblEmpnm" runat="server"></asp:Label></td>
                    </tr>

                    <!-- 系統資訊 -->
                    <tr>
                        <th colspan="4" class="section-header">系統資訊</th>
                    </tr>
                    <tr>
                        <th>建立者</th>
                        <td><asp:Label ID="lblCreatedBy" runat="server"></asp:Label></td>
                        <th>建立時間</th>
                        <td><asp:Label ID="lblCreatedTime" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>修改者</th>
                        <td><asp:Label ID="lblModifiedBy" runat="server"></asp:Label></td>
                        <th>修改時間</th>
                        <td><asp:Label ID="lblModifiedTime" runat="server"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>放行者</th>
                        <td><asp:Label ID="lblApprovedBy" runat="server"></asp:Label></td>
                        <th>放行時間</th>
                        <td><asp:Label ID="lblApprovedTime" runat="server"></asp:Label></td>
                    </tr>
                </table>

                <div class="btn-container">
                    <asp:Button ID="btnBack" runat="server" Text="返回查詢" CssClass="btn btn-back" OnClick="BtnBack_Click" CausesValidation="false" />
                    <button type="button" class="btn btn-print" onclick="window.print();">列印</button>
                </div>
            </div>
        </div>
    </form>
</body>
</html>