<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Edit.aspx.cs" Inherits="NtdEdit" ValidateRequest="false" %>
<!DOCTYPE html>
<html>
<head runat="server">
    <title>新臺幣定期存款專案利率申請 - 申請單</title>
    <style>
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

        .form { width: 100%; border-collapse: collapse; }
        .form th { width: 160px; background: #e3f2fd; color: #1565c0; text-align: right; padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form td { padding: 6px 10px; border: 1px solid #d0d0d0; }
        .form input[type=text], .form input[type=number], .form input[type=date], .form textarea, .form select {
            border: 1px solid #bdbdbd; border-radius: 3px; padding: 4px 6px; font-size: 13px; font-family: inherit;
        }
        .form textarea { width: 96%; }
        .form .req { color: #d32f2f; }

        .tbl { width: 100%; border-collapse: collapse; }
        .tbl th, .tbl td { border: 1px solid #d0d0d0; padding: 5px 6px; font-size: 12px; }
        .tbl th { background: #e3f2fd; font-weight: bold; color: #1565c0; text-align: center; }
        .tbl .num { text-align: right; }
        .tbl .ctr { text-align: center; }
        .tbl input, .tbl select { border: 1px solid #bdbdbd; border-radius: 3px; padding: 2px 4px; font-size: 12px; font-family: inherit; }

        .toolbar { padding: 8px 0; display: flex; gap: 6px; align-items: center; }
        .btn { display: inline-block; padding: 6px 16px; border: 1px solid #1565c0; background: #1565c0; color: #fff; border-radius: 3px; cursor: pointer; font-size: 13px; font-family: inherit; }
        .btn:hover { background: #0d47a1; }
        .btn-sub { background: #757575; border-color: #757575; }
        .btn-sub:hover { background: #424242; }
        .btn-ok { background: #388e3c; border-color: #388e3c; }
        .btn-ok:hover { background: #1b5e20; }
        .btn-query { background: #0277bd; border-color: #0277bd; color: #fff; border: 1px solid #0277bd; border-radius: 3px; padding: 4px 10px; cursor: pointer; font-size: 13px; font-family: inherit; }
        .btn-query:hover { background: #01579b; }

        .alert { padding: 10px 12px; margin-bottom: 12px; border-radius: 4px; }
        .alert-err  { background: #ffebee; color: #b71c1c; border: 1px solid #ef9a9a; }
        .alert-warn { background: #fff3e0; color: #e65100; border: 1px solid #ffcc80; }

        .contrib-tbl { border-collapse: collapse; width: 100%; }
        .contrib-tbl th, .contrib-tbl td { border: 1px solid #d0d0d0; padding: 5px 8px; text-align: center; font-size: 13px; }
        .contrib-tbl th { background: #e3f2fd; color: #1565c0; }
    </style>
    <script>
        function AddDetailRow() {
            var table = document.getElementById('DetailTable');
            var tbody = table.tBodies[0];
            var index = tbody.rows.length;
            var row = tbody.insertRow();
            row.innerHTML =
                '<td class="ctr">' + (index + 1) + '</td>' +
                '<td><select name="DepositType_' + index + '" style="width:115px">' +
                    '<option value="">--選擇--</option>' +
                    '<option value="定期存款">定期存款</option>' +
                    '<option value="定期儲蓄存款">定期儲蓄存款</option>' +
                '</select></td>' +
                '<td><input type="number" name="Amount_'     + index + '" min="0" step="0.0001" style="width:88px" /></td>' +
                '<td><input type="number" name="Rate_'       + index + '" min="0" step="0.0001" style="width:73px" /></td>' +
                '<td><input type="number" name="Period_'     + index + '" min="1" max="360"     style="width:53px" /></td>' +
                '<td><input type="date"   name="StartDate_'  + index + '" onchange="checkBusinessDay(this)" style="width:118px" /></td>' +
                '<td><input type="date"   name="EndDate_'    + index + '" onchange="checkBusinessDay(this)" style="width:118px" /></td>' +
                '<td><input type="number" name="NewAmount_'  + index + '" min="0" step="0.0001" style="width:83px" /></td>' +
                '<td><input type="number" name="RenewAmount_'+ index + '" min="0" step="0.0001" style="width:83px" /></td>' +
                '<td><input type="text"   name="Memo_'       + index + '" maxlength="500" style="width:100%;min-width:80px" /></td>' +
                '<td class="ctr"><a href="javascript:void(0)" onclick="DeleteDetailRow(this)">刪除</a></td>';
        }

        function DeleteDetailRow(anchor) {
            var row = anchor.parentNode.parentNode;
            row.parentNode.removeChild(row);
        }

        function checkBusinessDay(input) {
            if (!input.value) return;
            var d = new Date(input.value);
            var day = d.getUTCDay();
            if (day === 0 || day === 6) {
                alert('【注意】' + input.value + ' 為週末，起存日/到期日需為營業日，請確認。');
            }
        }

        function CountBranchNoteChars(textarea) {
            var max = 3000;
            if (textarea.value.length > max) textarea.value = textarea.value.substring(0, max);
            document.getElementById('BranchNoteCharCount').innerText = textarea.value.length;
        }
    </script>
</head>
<body>
<form id="Form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" />

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
                    <th>所屬集團</th>
                    <td colspan="3"><asp:TextBox ID="TxtGroupName" runat="server" Width="300" MaxLength="100" /></td>
                </tr>
            </table>

            <%-- 客戶統編查詢：UpdatePanel 做局部更新，不影響 JS 動態新增的明細列 --%>
            <asp:UpdatePanel ID="UpnlCustomer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table class="form">
                        <tr>
                            <th>客戶統編/身分證 <span class="req">*</span></th>
                            <td>
                                <asp:TextBox ID="TxtCustomerID" runat="server" Width="160" MaxLength="11" />
                                &nbsp;<asp:Button ID="BtnQueryCustomer" runat="server" Text="查詢客戶"
                                    CssClass="btn-query" OnClick="BtnQueryCustomer_Click" CausesValidation="false" />
                                <asp:Label ID="LblQueryMsg" runat="server" style="color:#b71c1c;font-size:12px;margin-left:6px" />
                            </td>
                            <th>客戶名稱</th>
                            <td>
                                <asp:TextBox ID="TxtCustomerName" runat="server" Width="220" MaxLength="100" ReadOnly="true"
                                    style="background:#f5f5f5;" />
                                <span style="font-size:11px;color:#888">（由統編自動帶入）</span>
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="BtnQueryCustomer" EventName="Click" />
                </Triggers>
            </asp:UpdatePanel>

            <table class="form">
                <tr>
                    <th>申請總金額 <span class="req">*</span></th>
                    <td colspan="3">
                        <asp:TextBox ID="TxtTotalAmount" runat="server" Width="200" TextMode="Number" />
                        新臺幣元
                    </td>
                </tr>
                <tr>
                    <th>是否屬利害關係人 <span class="req">*</span></th>
                    <td colspan="3">
                        <asp:RadioButtonList ID="RblIsRelated" runat="server"
                            RepeatDirection="Horizontal" RepeatLayout="Flow">
                            <asp:ListItem Value="Y" Text="是" />
                            <asp:ListItem Value="N" Text="否" />
                        </asp:RadioButtonList>
                    </td>
                </tr>
            </table>

            <h3 style="margin-top:18px;color:#1565c0">申請明細</h3>
            <table id="DetailTable" class="tbl">
                <thead>
                    <tr>
                        <th style="width:36px">#</th>
                        <th style="width:120px">存款種類 <span class="req">*</span></th>
                        <th style="width:95px">金額(億元)</th>
                        <th style="width:80px">利率(%)</th>
                        <th style="width:60px">期間(月)</th>
                        <th style="width:125px">起存日</th>
                        <th style="width:125px">到期日</th>
                        <th style="width:90px">新作(億元)</th>
                        <th style="width:90px">續作(億元)</th>
                        <th>備注</th>
                        <th style="width:42px">動作</th>
                    </tr>
                </thead>
                <tbody>
                    <asp:Repeater ID="RptDetail" runat="server">
                        <ItemTemplate>
                            <tr>
                                <td class="ctr"><%# Container.ItemIndex + 1 %></td>
                                <td>
                                    <select name='DepositType_<%# Container.ItemIndex %>' style="width:115px">
                                        <option value="">--選擇--</option>
                                        <option value="定期存款"     <%# Convert.ToString(Eval("DepositType")) == "定期存款"     ? "selected" : "" %>>定期存款</option>
                                        <option value="定期儲蓄存款" <%# Convert.ToString(Eval("DepositType")) == "定期儲蓄存款" ? "selected" : "" %>>定期儲蓄存款</option>
                                    </select>
                                </td>
                                <td><input type="number" name='Amount_<%# Container.ItemIndex %>'
                                    value='<%# Eval("Amount") %>' min="0" step="0.0001" style="width:88px" /></td>
                                <td><input type="number" name='Rate_<%# Container.ItemIndex %>'
                                    value='<%# Eval("ProposedRate") %>' min="0" step="0.0001" style="width:73px" /></td>
                                <td><input type="number" name='Period_<%# Container.ItemIndex %>'
                                    value='<%# Eval("PeriodMonth") %>' min="1" max="360" style="width:53px" /></td>
                                <td><input type="date" name='StartDate_<%# Container.ItemIndex %>'
                                    value='<%# Eval("StartDate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("StartDate")).ToString("yyyy-MM-dd") %>'
                                    onchange="checkBusinessDay(this)" style="width:118px" /></td>
                                <td><input type="date" name='EndDate_<%# Container.ItemIndex %>'
                                    value='<%# Eval("EndDate") == DBNull.Value ? "" : Convert.ToDateTime(Eval("EndDate")).ToString("yyyy-MM-dd") %>'
                                    onchange="checkBusinessDay(this)" style="width:118px" /></td>
                                <td><input type="number" name='NewAmount_<%# Container.ItemIndex %>'
                                    value='<%# Eval("NewAmount") == DBNull.Value ? "" : Convert.ToDecimal(Eval("NewAmount")).ToString("0.####") %>'
                                    min="0" step="0.0001" style="width:83px" /></td>
                                <td><input type="number" name='RenewAmount_<%# Container.ItemIndex %>'
                                    value='<%# Eval("RenewAmount") == DBNull.Value ? "" : Convert.ToDecimal(Eval("RenewAmount")).ToString("0.####") %>'
                                    min="0" step="0.0001" style="width:83px" /></td>
                                <td><input type="text" name='Memo_<%# Container.ItemIndex %>'
                                    value='<%# Eval("Memo") %>' maxlength="500" style="width:100%;min-width:80px" /></td>
                                <td class="ctr"><a href="javascript:void(0)" onclick="DeleteDetailRow(this)">刪除</a></td>
                            </tr>
                        </ItemTemplate>
                    </asp:Repeater>
                </tbody>
            </table>
            <div class="toolbar">
                <a href="javascript:void(0)" onclick="AddDetailRow()" class="btn btn-sub">＋ 新增一筆明細</a>
            </div>

            <%-- 貢獻度 --%>
            <h3 style="margin-top:18px;color:#1565c0">貢獻度</h3>
            <table class="contrib-tbl">
                <tr>
                    <th style="width:140px"></th>
                    <th>最近三個月存款實績(仟元)</th>
                    <th>最近六個月存款實績(仟元)</th>
                    <th>最近一年貢獻度(%)</th>
                    <th>關係戶往來實績(仟元)</th>
                </tr>
                <tr>
                    <td style="background:#e3f2fd;color:#1565c0;font-weight:bold;text-align:right;padding:5px 8px;">客戶存款</td>
                    <td><asp:TextBox ID="TxtContrib3M"  runat="server" TextMode="Number" Width="150" /></td>
                    <td><asp:TextBox ID="TxtContrib6M"  runat="server" TextMode="Number" Width="150" /></td>
                    <td><asp:TextBox ID="TxtContrib1Y"  runat="server" TextMode="Number" Width="100" /></td>
                    <td><asp:TextBox ID="TxtRelatedAmt" runat="server" TextMode="Number" Width="150" /></td>
                </tr>
            </table>

            <%-- 申請理由 & 申請單位說明 --%>
            <table class="form" style="margin-top:14px;">
                <tr>
                    <th>申請理由<br /><span style="font-size:11px;color:#888">(選填)</span></th>
                    <td colspan="3">
                        <asp:TextBox ID="TxtReason" runat="server" TextMode="MultiLine" Width="96%" Height="80"
                            MaxLength="3000" />
                    </td>
                </tr>
                <tr>
                    <th>申請單位說明<br /><span style="font-size:11px;color:#888">(限 3,000 字)</span></th>
                    <td colspan="3">
                        <asp:TextBox ID="TxtBranchNote" runat="server" TextMode="MultiLine" Width="96%" Height="100"
                            MaxLength="3000" onkeyup="CountBranchNoteChars(this)" onchange="CountBranchNoteChars(this)" />
                        <div style="font-size:11px;color:#888">已輸入字數：<span id="BranchNoteCharCount">0</span> / 3000</div>
                    </td>
                </tr>
            </table>

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
