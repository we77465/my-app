<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="NtdEdit" ValidateRequest="false" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>新臺幣定期存款專案利率申請 - 申請單</title>
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

        /* ===== 申請主檔 form ===== */
        .form { width: 100%; border-collapse: collapse; }
        .form th { width: 160px; background: #e3f2fd; color: #1565c0; text-align: right; padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form td { padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form input[type=text], .form input[type=number], .form input[type=date], .form textarea { border: 1px solid #bdbdbd; border-radius: 3px; padding: 4px 6px; font-size: 13px; font-family: inherit; }
        .form textarea { width: 96%; min-height: 120px; }
        .form .req { color: #d32f2f; }

        /* ===== 利率明細表 + 簽核紀錄表 ===== */
        .tbl { width: 100%; border-collapse: collapse; }
        .tbl th, .tbl td { border: 1px solid #d0d0d0; padding: 6px 8px; font-size: 13px; }
        .tbl th { background: #e3f2fd; font-weight: bold; color: #1565c0; text-align: center; }
        .tbl .num { text-align: right; }
        .tbl .ctr { text-align: center; }
        .tbl input { border: 1px solid #bdbdbd; border-radius: 3px; padding: 3px 5px; font-size: 13px; }

        /* ===== 按鈕 / 工具列 ===== */
        .toolbar { padding: 8px 0; display: flex; gap: 6px; align-items: center; }
        .btn { display: inline-block; padding: 6px 16px; border: 1px solid #1565c0; background: #1565c0; color: #fff; border-radius: 3px; cursor: pointer; font-size: 13px; font-family: inherit; }
        .btn:hover { background: #0d47a1; }
        .btn-sub { background: #757575; border-color: #757575; }
        .btn-sub:hover { background: #424242; }
        .btn-ok { background: #388e3c; border-color: #388e3c; }
        .btn-ok:hover { background: #1b5e20; }

        /* ===== 操作訊息 ===== */
        .alert { padding: 10px 12px; margin-bottom: 12px; border-radius: 4px; }
        .alert-err  { background: #ffebee; color: #b71c1c; border: 1px solid #ef9a9a; }
        .alert-warn { background: #fff3e0; color: #e65100; border: 1px solid #ffcc80; }
    </style>
    <script>
        // 新增一筆空白明細
        function AddDetailRow() {
            var table = document.getElementById('DetailTable');
            var tbody = table.tBodies[0];
            var index = tbody.rows.length;
            var row = tbody.insertRow();
            row.innerHTML =
                '<td class="ctr">' + (index + 1) + '</td>' +
                '<td><input type="number" name="Period_'   + index + '" min="1" max="120" style="width:80px" /></td>' +
                '<td><input type="number" name="Amount_'   + index + '" min="0" step="1" style="width:140px" /></td>' +
                '<td><input type="number" name="Rate_'     + index + '" min="0" step="0.0001" style="width:100px" /></td>' +
                '<td><input type="text"   name="Memo_'     + index + '" maxlength="500" style="width:96%" /></td>' +
                '<td class="ctr"><a href="javascript:void(0)" onclick="DeleteDetailRow(this)">刪除</a></td>';
        }
        function DeleteDetailRow(anchor) {
            var row = anchor.parentNode.parentNode;
            row.parentNode.removeChild(row);
        }
        // 申請理由字數限制 (需求 §3：3,000 字)
        function CountReasonChars(textarea) {
            var max = 3000;
            if (textarea.value.length > max) textarea.value = textarea.value.substring(0, max);
            document.getElementById('ReasonCharCount').innerText = textarea.value.length;
        }
    </script>
</head>
<body>
<form id="Form1" runat="server">
    <div class="topbar">
        <h1>新臺幣定期存款專案利率申請</h1>
        <div class="user"><asp:Label ID="LblUser" runat="server" /></div>
    </div>
    <div class="nav">
        <a href="<%= NavUrl("Main.aspx") %>" class="active">待處理</a>
        <a href="<%= NavUrl("Query.aspx") %>">查詢</a>
        <a href="<%= NavUrl("Approve.aspx") %>">簽核/決行</a>
        <a href="<%= NavUrl("Report.aspx") %>">報表</a>
    </div>

    <div class="container">
        <asp:Label ID="LblMessage" runat="server" Visible="false" />

        <div class="card">
            <h2 class="card-title">
                <asp:Literal ID="LitTitle" runat="server" Text="新增申請" />
                <span style="float:right; font-weight:normal; font-size:13px; color:#666">
                    申請單號：<asp:Label ID="LblApplyNo" runat="server" Text="(系統產生)" />
                    &nbsp;|&nbsp;
                    狀態：<asp:Label ID="LblStatus" runat="server" />
                </span>
            </h2>

            <table class="form">
                <tr>
                    <th>申請單位</th>
                    <td><asp:Label ID="LblBranch" runat="server" /></td>
                    <th>申請日期</th>
                    <td><asp:TextBox ID="TxtApplyDate" runat="server" TextMode="Date" /></td>
                </tr>
                <tr>
                    <th>客戶名稱 <span class="req">*</span></th>
                    <td><asp:TextBox ID="TxtCustomerName" runat="server" Width="240" MaxLength="100" /></td>
                    <th>客戶統編/身分證</th>
                    <td><asp:TextBox ID="TxtCustomerID" runat="server" Width="200" MaxLength="20" /></td>
                </tr>
                <tr>
                    <th>申請總金額 <span class="req">*</span></th>
                    <td colspan="3">
                        <asp:TextBox ID="TxtTotalAmount" runat="server" Width="200" TextMode="Number" />
                        新臺幣元
                    </td>
                </tr>
                <tr>
                    <th>申請理由<br /><span style="font-size:11px;color:#888">(限 3,000 字)</span></th>
                    <td colspan="3">
                        <asp:TextBox ID="TxtReason" runat="server" TextMode="MultiLine" Width="96%" Height="120"
                            MaxLength="3000" onkeyup="CountReasonChars(this)" onchange="CountReasonChars(this)" />
                        <div style="font-size:11px;color:#888">已輸入字數：<span id="ReasonCharCount">0</span> / 3000</div>
                    </td>
                </tr>
            </table>

            <h3 style="margin-top:18px;color:#1565c0">利率明細</h3>
            <table id="DetailTable" class="tbl">
                <thead>
                    <tr>
                        <th style="width:50px">#</th>
                        <th style="width:100px">期間(月)</th>
                        <th style="width:160px">金額</th>
                        <th style="width:120px">利率(%)</th>
                        <th>備註</th>
                        <th style="width:60px">動作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RptDetail" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="ctr"><%# Container.ItemIndex + 1 %></td>
                                <td><input type="number" name='Period_<%# Container.ItemIndex %>' value='<%# Eval("PeriodMonth") %>' min="1" max="120" style="width:80px" /></td>
                                <td><input type="number" name='Amount_<%# Container.ItemIndex %>' value='<%# Eval("Amount") %>' min="0" step="1" style="width:140px" /></td>
                                <td><input type="number" name='Rate_<%#   Container.ItemIndex %>' value='<%# Eval("ProposedRate") %>' min="0" step="0.0001" style="width:100px" /></td>
                                <td><input type="text"   name='Memo_<%#   Container.ItemIndex %>' value='<%# Eval("Memo") %>' maxlength="500" style="width:96%" /></td>
                                <td class="ctr"><a href="javascript:void(0)" onclick="DeleteDetailRow(this)">刪除</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div class="toolbar">
                <a href="javascript:void(0)" onclick="AddDetailRow()" class="btn btn-sub">＋ 新增一筆明細</a>
            </div>

            <div class="toolbar" style="margin-top:18px; border-top:1px solid #e0e0e0; padding-top:14px">
                <asp:Button ID="BtnSaveDraft" runat="server" Text="儲存草稿"
                    CssClass="btn btn-sub" OnClick="BtnSaveDraft_Click" />
                <asp:Button ID="BtnSubmit"    runat="server" Text="送出申請"
                    CssClass="btn btn-ok"  OnClick="BtnSubmit_Click"
                    OnClientClick="return confirm('送出後將進入簽核流程，確定送出？');" />
                <asp:Button ID="BtnBack"      runat="server" Text="返回列表"
                    CssClass="btn btn-sub" OnClick="BtnBack_Click" CausesValidation="false" />
            </div>
        </div>

        <asp:Panel ID="PnlLog" runat="server" CssClass="card" Visible="false">
            <h2 class="card-title">簽核紀錄</h2>
            <asp:GridView ID="GvLog" runat="server" AutoGenerateColumns="false"
                CssClass="tbl" GridLines="None" EmptyDataText="無紀錄">
                <Columns>
                    <asp:BoundField DataField="ActionTime"     HeaderText="時間"  DataFormatString="{0:yyyy-MM-dd HH:mm}" ItemStyle-CssClass="ctr" />
                    <asp:BoundField DataField="ActionUser"     HeaderText="員編"  ItemStyle-CssClass="ctr" />
                    <asp:BoundField DataField="ActionUserName" HeaderText="姓名"  ItemStyle-CssClass="ctr" />
                    <asp:BoundField DataField="ActionType"     HeaderText="動作"  ItemStyle-CssClass="ctr" />
                    <asp:TemplateField HeaderText="狀態">
                        <ItemTemplate>
                            <%# StatusName(Convert.ToInt32(Eval("FromStatus"))) %>
                            &nbsp;&rarr;&nbsp;
                            <%# StatusName(Convert.ToInt32(Eval("ToStatus"))) %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:BoundField DataField="Comment" HeaderText="意見" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
    <div class="footer">© 經營管理處 - 新臺幣定期存款專案利率申請</div>
</form>
</body>
</html>
