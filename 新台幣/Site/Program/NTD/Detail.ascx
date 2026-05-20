<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="NtdApplyDetail" %>

<%-- 此 UserControl 內嵌於 View / Approve，故自帶明細卡片所需樣式 --%>
<style>
    .ntd-detail .card { background: #fff; border: 1px solid #e0e0e0; padding: 18px; margin-bottom: 16px; }
    .ntd-detail .card-title { font-size: 16px; font-weight: bold; margin: 0 0 12px; padding-bottom: 8px; border-bottom: 2px solid #1565c0; color: #1565c0; }
    .ntd-detail .form { width: 100%; border-collapse: collapse; }
    .ntd-detail .form th { width: 160px; background: #e3f2fd; color: #1565c0; text-align: right; padding: 6px 10px; border: 1px solid #d0d0d0; }
    .ntd-detail .form td { padding: 6px 10px; border: 1px solid #d0d0d0; }
    .ntd-detail .tbl { width: 100%; border-collapse: collapse; }
    .ntd-detail .tbl th, .ntd-detail .tbl td { border: 1px solid #d0d0d0; padding: 6px 8px; font-size: 13px; }
    .ntd-detail .tbl th { background: #e3f2fd; font-weight: bold; color: #1565c0; text-align: center; }
    .ntd-detail .tbl .num { text-align: right; }
    .ntd-detail .tbl .ctr { text-align: center; }
    .ntd-detail .status { display: inline-block; padding: 2px 8px; border-radius: 12px; font-size: 12px; color: #fff; }
    .ntd-detail .status-0 { background: #9e9e9e; }
    .ntd-detail .status-1 { background: #1976d2; }
    .ntd-detail .status-2 { background: #0288d1; }
    .ntd-detail .status-4 { background: #f57c00; }
    .ntd-detail .status-6 { background: #c62828; }
    .ntd-detail .status-9 { background: #388e3c; }
</style>

<div class="ntd-detail">
<div class="card">
    <h2 class="card-title">
        申請單明細
        <span style="float:right; font-weight:normal; font-size:13px; color:#666">
            申請單號：<b><asp:Label ID="LblApplyNo" runat="server" /></b>
            &nbsp;|&nbsp;
            狀態：<asp:Label ID="LblStatus" runat="server" />
        </span>
    </h2>

    <table class="form">
        <tr>
            <th>申請單位</th>
            <td><asp:Label ID="LblBranch" runat="server" /></td>
            <th>申請日期</th>
            <td><asp:Label ID="LblApplyDate" runat="server" /></td>
        </tr>
        <tr>
            <th>客戶名稱</th>
            <td><asp:Label ID="LblCustomerName" runat="server" /></td>
            <th>客戶統編/身分證</th>
            <td><asp:Label ID="LblCustomerID" runat="server" /></td>
        </tr>
        <tr>
            <th>申請總金額</th>
            <td>
                <asp:Label ID="LblTotalAmount" runat="server" CssClass="num" /> 新臺幣元
            </td>
            <th>利率別代號</th>
            <td><asp:Label ID="LblRateType" runat="server" /></td>
        </tr>
        <tr>
            <th>申請理由</th>
            <td colspan="3"><asp:Label ID="LblReason" runat="server" /></td>
        </tr>
        <tr>
            <th>建單</th>
            <td><asp:Label ID="LblCreate" runat="server" /></td>
            <th>最後異動</th>
            <td><asp:Label ID="LblUpdate" runat="server" /></td>
        </tr>
    </table>

    <h3 style="margin-top:18px;color:#1565c0">利率明細</h3>
    <asp:GridView ID="GvDetail" runat="server" AutoGenerateColumns="false"
        CssClass="tbl" GridLines="None" EmptyDataText="無明細">
        <Columns>
            <asp:BoundField DataField="SeqNo"        HeaderText="#"        ItemStyle-CssClass="ctr" />
            <asp:BoundField DataField="PeriodMonth"  HeaderText="期間(月)" ItemStyle-CssClass="ctr" />
            <asp:BoundField DataField="Amount"       HeaderText="金額"     DataFormatString="{0:N0}"     ItemStyle-CssClass="num" />
            <asp:BoundField DataField="ProposedRate" HeaderText="利率(%)"  DataFormatString="{0:0.0000}" ItemStyle-CssClass="num" />
            <asp:BoundField DataField="Memo"         HeaderText="備註" />
        </Columns>
    </asp:GridView>
</div>

<div class="card" runat="server" id="PnlLog">
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
</div>
</div>
