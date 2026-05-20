<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Detail.ascx.cs" Inherits="Program_KL_Detail" %>
<style type="text/css">
        .auto-style2 {
            width: 100%;
            margin:0;
            padding:0;
        }
        .auto-style4 {
            width: 270px;
            height: 30px;
        font-family: 微軟正黑體;
    }
        .auto-style5 {
        width: 100%;
        margin: 0;
        padding: 0;
        font-family: 微軟正黑體;
    }
    .auto-style6 {
        font-family: 微軟正黑體;
    }
    .auto-style7 {
        text-align: center;
        font-family: 微軟正黑體;
    }
        .auto-style8 {
        height: 20px;
        font-family: 微軟正黑體;
    }
        .auto-style11 {
        width: 100%;
    }
    .auto-check {
        color:red;
        font-family: 微軟正黑體;
    }
</style>
<div>
    <table class="auto-style2">
            <tr>
                <td style="text-align: center">
                    <asp:Button ID="btnSave" runat="server"  style="font-family: 微軟正黑體; font-size: medium" Text="儲存"  OnClick="btnSave_Click"/>&nbsp; &nbsp; &nbsp; &nbsp;
                    &nbsp;<asp:Button ID="btnSign" runat="server"  OnClick="btnSign_Click"  style="font-family: 微軟正黑體; font-size: medium" Text="儲存並送主管核可" />
                    </td>
            </tr>
        </table>
    <br /><br />
    <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
        <tr>
            <td colspan="3" style="text-align: center;"><strong>一億元以上授信案件通報單</strong><br />
                (請採條列式精簡陳述，並以不超2頁為原則)</td>
        </tr>
        <tr>
            <td style="width: 100%;" colspan="3">一、基本資料：</td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                    <tr>
                        <td style="width: 25%;">授信管理行：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtMangerBank" runat="server"></asp:TextBox></td>
                        <td style="width: 25%">授信首次往來日：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtFirstDay" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">戶名：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtName" runat="server"></asp:TextBox></td>
                        <td style="width: 25%">設立日期：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtSetDate" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 25%;">代表人：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtAgent" runat="server"></asp:TextBox></td>
                        <td style="width: 25%">統編：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtID" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">產業別/主要營業項目：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtIndustry" runat="server"></asp:TextBox>/ <asp:TextBox ID="txtMainItem" runat="server"></asp:TextBox></td>
                        <td style="width: 25%">實收資本額：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtCapital" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">主力商品：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtProduct" runat="server"></asp:TextBox></td>
                        <td style="width: 25%">本行評等：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtCHBRating" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">內／外銷比重：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtProportion1" runat="server"></asp:TextBox>% /<asp:TextBox ID="txtProportion2" runat="server"></asp:TextBox>%</td>
                        <td style="width: 25%">外部評等：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtExterRating" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 25%;">股票發行情形：</td>
                        <td style="width: 25%">
                            <asp:CheckBoxList ID="chkStock" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="上市" Value="0"></asp:ListItem>
                                <asp:ListItem Text="上櫃" Value="1"></asp:ListItem>
                                <asp:ListItem Text="興櫃" Value="2"></asp:ListItem>
                                <asp:ListItem Text="公發" Value="3"></asp:ListItem>
                                <asp:ListItem Text="非公發" Value="4"></asp:ListItem>
                            </asp:CheckBoxList></td>
                        <td style="width: 25%">股票代號/最近收盤價(日期)：</td>
                        <td style="width: 25%">
                            <asp:TextBox ID="txtStockNo" runat="server"></asp:TextBox>/<asp:TextBox ID="txtClosDate" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 25%;">企業規模：</td>
                        <td style="width: 25%">
                            <asp:CheckBoxList ID="chkEnterprise" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="大企業" Value="0"></asp:ListItem>
                                <asp:ListItem Text="中小企業" Value="1"></asp:ListItem>
                            </asp:CheckBoxList></td>
                        <td style="width: 25%">薪轉戶：</td>
                        <td style="width: 25%">
                           <asp:CheckBoxList ID="chkSalaryTransfer" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="是" Value="0"></asp:ListItem>
                                <asp:ListItem Text="否" Value="1"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" colspan="3">二、往來資訊：<br />(一)近一年往來實績：<span style="text-align:right">單位：新台幣億元</span></td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                    <tr>
                        <td style="width: 10%;">戶名</td>
                        <td style="width: 25%">存款平均餘額</td>
                        <td style="width: 25%">授信平均餘額</td>
                        <td style="width: 25%">外匯業務承作量(仟美元)</td>
                        <td style="width: 15%;">貢獻度</td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">借戶</td>
                        <td style="width: 25%"><asp:TextBox ID="txtAverage1_1" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtAverage1_2" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtAverage1_3" runat="server"></asp:TextBox></td>
                        <td style="width: 15%;"><asp:TextBox ID="txtAverage1_4" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 10%;">企業負責人</td>
                        <td style="width: 25%"><asp:TextBox ID="txtAverage2_1" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtAverage2_2" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtAverage2_3" runat="server"></asp:TextBox></td>
                        <td style="width: 15%;"><asp:TextBox ID="txtAverage2_4" runat="server"></asp:TextBox></td>
                    </tr>
                  </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" colspan="3">其他綜合業務貢獻說明：(含財富管理、保險、匯兌或其他週邊業務等)</td>
        </tr>
         <tr>
            <td style="width: 100%;" colspan="3">(二) 外匯實績<span style="text-align:right">單位：新台幣億元</span></td>
        </tr>
        <tr>
            <td colspan="3">
                 <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                      <tr>
                        <td style="width: 10%;">項目</td>
                        <td style="width: 25%">109年</td>
                        <td style="width: 25%">本行承作額</td>
                        <td style="width: 25%">110年XX月</td>
                        <td style="width: 15%;">本行承作額</td>
                    </tr>
                     <tr>
                        <td style="width: 10%;">進口</td>
                        <td style="width: 25%"><asp:TextBox ID="txtExchange1_1" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtExchange1_2" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtExchange1_3" runat="server"></asp:TextBox></td>
                        <td style="width: 15%;"><asp:TextBox ID="txtExchange1_4" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 10%;">出口</td>
                        <td style="width: 25%"><asp:TextBox ID="txtExchange2_1" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtExchange2_2" runat="server"></asp:TextBox></td>
                        <td style="width: 25%"><asp:TextBox ID="txtExchange2_3" runat="server"></asp:TextBox></td>
                        <td style="width: 15%;"><asp:TextBox ID="txtExchange2_4" runat="server"></asp:TextBox></td>
                    </tr>
                     </table>
            </td>
        </tr>
         <tr>
            <td style="width: 100%;" colspan="3">(三) 授信往來</td>
        </tr>
         <tr>
            <td style="width: 100%;" colspan="3"><asp:TextBox ID="txtCreditExchange" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
         <tr>
            <td style="width: 100%;" colspan="3">(四) 金融機構往來概況：<asp:TextBox ID="txtTranDate" runat="server" ></asp:TextBox><span style="text-align:right">單位：新台幣億元</span></td>
        </tr>
        <tr>
            <td style="width: 35%;" >往來金融機構共<asp:TextBox ID="txtTotalFina" runat="server" ></asp:TextBox>家</td>
             <td style="width: 35%;" >主力銀行(現放排序)</td>
             <td style="width: 30%;" ><asp:TextBox ID="txtBankShort" runat="server" ></asp:TextBox></td>
        </tr>
        <tr>
            <td colspan="3">
                 <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                     <tr>
                         <td>總額度</td>
                         <td><asp:TextBox ID="txtTotalAmount" runat="server" ></asp:TextBox></td>
                         <td>本行占比</td>
                          <td><asp:TextBox ID="txtCHBProp1" runat="server" ></asp:TextBox>%</td>
                         <td>現放總額</td>
                          <td><asp:TextBox ID="txtCurrentTotal" runat="server" ></asp:TextBox></td>
                         <td>本行占比</td>
                          <td><asp:TextBox ID="txtCHBProp2" runat="server" ></asp:TextBox>%</td>
                     </tr>
                     <tr>
                         <td colspan="3">加計本案額度放款占比</td>
                          <td><asp:TextBox ID="txtAdd1" runat="server" ></asp:TextBox>%</td>
                         <td><asp:TextBox ID="txtAdd2" runat="server" ></asp:TextBox></td>
                          <td><asp:TextBox ID="txtAdd3" runat="server" ></asp:TextBox></td>
                         <td><asp:TextBox ID="txtAdd4" runat="server" ></asp:TextBox></td>
                          <td><asp:TextBox ID="txtAdd5" runat="server" ></asp:TextBox></td>
                     </tr>
                     <tr>
                         <td colspan="2">最近一年存款平均餘額</td>
                          <td><asp:TextBox ID="txtRecentYear1" runat="server" ></asp:TextBox></td>
                         <td colspan="2">最近一年年報銀行存款</td>
                          <td><asp:TextBox ID="txtRecentYear2" runat="server" ></asp:TextBox></td>
                         <td>存款占比</td>
                          <td><asp:TextBox ID="txtRecentYear3" runat="server" ></asp:TextBox></td>
                     </tr>
                     </table>
            </td>
        </tr>
         <tr>
            <td style="width: 100%;" colspan="3">三、商機及授信情形說明</td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                    <tr>
                        <td>申請額度</td>
                        <td>主要資金用途</td>
                        <td>通報日</td>
                        <td>E-LOAN放行日</td>
                        <td>分行處理天數</td>
                        <td>核定日</td>
                        <td>審核天數</td>
                    </tr>
                     <tr>
                        <td><asp:TextBox ID="txtQuota" runat="server" ></asp:TextBox>億元</td>
                        <td><asp:TextBox ID="txtMainUse" runat="server" ></asp:TextBox></td>
                        <td><asp:TextBox ID="txtNotificationDay" runat="server" ></asp:TextBox></td>
                        <td><asp:TextBox ID="txtEloan" runat="server" ></asp:TextBox></td>
                        <td><asp:TextBox ID="txtProcessDay" runat="server" ></asp:TextBox></td>
                        <td><asp:TextBox ID="txtApprovalDate" runat="server" ></asp:TextBox></td>
                        <td><asp:TextBox ID="txtAuditDays" runat="server" ></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:CheckBoxList ID="chkType" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="5+2產業" Value="0"></asp:ListItem>
                                <asp:ListItem Text="電廠融資" Value="1"></asp:ListItem>
                                <asp:ListItem Text="都更危老" Value="2"></asp:ListItem>
                                <asp:ListItem Text="土建融" Value="3"></asp:ListItem>
                                <asp:ListItem Text="一般(如：週轉金、廠辦設備等)" Value="4"></asp:ListItem>
                            </asp:CheckBoxList></td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" colspan="3">商機簡述：(以5P簡述)<br /><asp:TextBox ID="txtDescript" runat="server" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
        </tr>
         <tr>
            <td style="width: 100%;" colspan="3">四、訪問記錄：</td>
        </tr>
        <tr>
            <td colspan="3">
                <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                    <tr>
                        <td style="width: 20%;" >訪問次數</td>
                        <td style="width: 20%;">1</td>
                        <td style="width: 20%;">2</td>
                        <td style="width: 20%;">3</td>
                        <td style="width: 20%;">4</td>
                    </tr>
                    <tr>
                        <td style="width: 20%;" >訪問日期</td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitDate1" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitDate2" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitDate3" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitDate4" runat="server"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td style="width: 20%;" >訪問對象</td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitObject1" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitObject2" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitObject3" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitObject4" runat="server"></asp:TextBox></td>
                    </tr>
                     <tr>
                        <td style="width: 20%;" >往訪人員</td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitors1" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitors2" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitors3" runat="server"></asp:TextBox></td>
                        <td style="width: 20%;"><asp:TextBox ID="txtVisitors4" runat="server"></asp:TextBox></td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td style="width: 100%;" colspan="3">
                <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                    <tr>
                        <td style="width: 25%;"> 通報日期：<asp:TextBox ID="txtNotificationDay2" runat="server" ></asp:TextBox></td>
                        <td style="width: 25%;"> 分行  經理：<asp:TextBox ID="txtManger" runat="server" ></asp:TextBox></td>
                        <td style="width: 25%;">負責人：<asp:TextBox ID="txtPrincipal" runat="server" ></asp:TextBox></td>
                        <td style="width: 25%;"> AO：<asp:TextBox ID="txtAO" runat="server" ></asp:TextBox></td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td >區營運處基於授信5P評估結果：
            </td>
            <td><asp:CheckBoxList ID="chk5P" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="具承作價值" Value="0"></asp:ListItem>
                                <asp:ListItem Text="建議暫不承作" Value="1"></asp:ListItem>
                            </asp:CheckBoxList></td>
            <td> (評估日：<asp:TextBox ID="txtEvaluationDay" runat="server" ></asp:TextBox>)</td>
        </tr>
        <tr>
            <td colspan="3">評估說明：<br /><asp:TextBox ID="txtRemarks" runat="server" Rows="5" TextMode="MultiLine" ></asp:TextBox></td>
        </tr>
        <tr>
            <td style="width: 100%;" colspan="3">
                <table class="auto-style5" style="padding: 0px; margin: 0px; width: 100%:" border="1">
                    <tr>
                        <td style="width: 25%;"> 區營運處：<asp:TextBox ID="txtCRC" runat="server" ></asp:TextBox></td>
                        <td style="width: 25%;"> 分行  經理：<asp:TextBox ID="txtManger2" runat="server" ></asp:TextBox></td>
                        <td style="width: 25%;">負責人：<asp:TextBox ID="txtPrincipal2" runat="server" ></asp:TextBox></td>
                        <td style="width: 25%;"> AO：<asp:TextBox ID="txtAO2" runat="server" ></asp:TextBox></td>
                    </tr>
                    </table>
            </td>
        </tr>
    </table>
</div>
<asp:SqlDataSource ID="sdsList" runat="server" ConnectionString='<%$ ConnectionStrings:chb_iom %>'
    SelectCommand="SELECT [No],[Brno],[BrName],[MangerBank],[FirstDay],[Name],[SetDate],[Agent],[ID],[Industry],[MainItem],[Capital],[Product],[CHBRating]
,[Proportion1],[Proportion2],[ExterRating],[Stock],[StockNo],[Enterprise],[SalaryTransfer]
,[Average1_1],[Average1_2],[Average1_3],[Average1_4],[Average2_1],[Average2_2],[Average2_3],[Average2_4],[Exchange1_1],[Exchange1_2],[Exchange1_3],[Exchange1_4],[Exchange2_1],[Exchange2_2],[Exchange2_3],[Exchange2_4]
,[CreditExchange],[TranDate],[TotalFina],[BankShort],[TotalAmount],[CHBProp1],[CurrentTotal],[CHBProp2]
,[Add1],[Add2],[Add3],[Add4],[Add5],[RecentYear1],[RecentYear2],[RecentYear3]
,[Quota],[MainUse],[NotificationDay],[Eloan],[ProcessDay],[ApprovalDate],[AuditDays],[Type]
,[Descript],[VisitDate1],[VisitDate2],[VisitDate3],[VisitDate4],[VisitObject1],[VisitObject2],[VisitObject3],[VisitObject4]
,[Visitors1],[Visitors2],[Visitors3],[Visitors4],[NotificationDay2],[Manger]
,[Principal],[AO],[5P],[EvaluationDay],[Remarks]
,[CRC],[Manger2],[Principal2],[AO2]
,[CrtID],[CrtName],[CrtDate],[UpdID],[UpdName],[UpdDate],[Status]
FROM [dbo].[KL]
WHERE [No] = @No"
    UpdateCommand="
    UPDATE [dbo].[KL]
SET [MangerBank] =@MangerBank,[FirstDay] =@FirstDay ,[Name] =@Name,[SetDate] =@SetDate,[Agent] =@Agent,[Industry] =@Industry,[MainItem] =@MainItem,[Capital] =@Capital,[Product] =@Product,[CHBRating] =@CHBRating
,[Proportion1] =@Proportion1,[Proportion2] =@Proportion2,[ExterRating] =@ExterRating
,[Stock] =@Stock,[StockNo] =@StockNo,[Enterprise] =@Enterprise,[SalaryTransfer] =@SalaryTransfer
,[Average1_1] =@Average1_1,[Average1_2] =@Average1_2,[Average1_3] =@Average1_3,[Average1_4] =@Average1_4,[Average2_1] =@Average2_1,[Average2_2] =@Average2_2,[Average2_3] =@Average2_3,[Average2_4] =@Average2_4
,[Exchange1_1] =@Exchange1_1,[Exchange1_2] =@Exchange1_2,[Exchange1_3] =@Exchange1_3,[Exchange1_4] =@Exchange1_4
,[Exchange2_1] =@Exchange2_1,[Exchange2_2] =@Exchange2_2,[Exchange2_3] =@Exchange2_3,[Exchange2_4] =@Exchange2_4
,[CreditExchange] =@CreditExchange,[TranDate] =@TranDate,[TotalFina] =@TotalFina,[BankShort] = @BankShort,[TotalAmount] =@TotalAmount,[CHBProp1] =@CHBProp1,[CurrentTotal] =@CurrentTotal,[CHBProp2] =@CHBProp2
,[Add1] =@Add1,[Add2] =@Add2,[Add3] =@Add3,[Add4] =@Add4,[Add5] =@Add5,[RecentYear1] =@RecentYear1,[RecentYear2] =@RecentYear2,[RecentYear3] =@RecentYear3
,[Quota] =@Quota,[MainUse] =@MainUse,[NotificationDay] =@NotificationDay,[Eloan] =@Eloan,[ProcessDay] =@ProcessDay,[ApprovalDate] =@ApprovalDate,[AuditDays] =@AuditDays,[Type] =@Type,[Descript] =@Descript
,[VisitDate1] =@VisitDate1,[VisitDate2] =@VisitDate2,[VisitDate3] =@VisitDate3,[VisitDate4] =@VisitDate4
,[VisitObject1] =@VisitObject1,[VisitObject2] =@VisitObject2,[VisitObject3] =@VisitObject3,[VisitObject4] =@VisitObject4
,[Visitors1] =@Visitors1,[Visitors2] =@Visitors2,[Visitors3] =@Visitors3,[Visitors4] =@Visitors4
,[NotificationDay2] =@NotificationDay2,[5P] =@5P,[EvaluationDay] =@EvaluationDay,[Remarks] =@Remarks
,[UpdID] =@UpdID,[UpdName] =@UpdName,[UpdDate] =	getdate(),[Status] ='0'
WHERE [No] = @No
    "
    InsertCommand="
    INSERT INTO [dbo].[KL](
[No],[Brno],[BrName],[MangerBank],[FirstDay],[Name],[SetDate],[Agent],[ID],[Industry],[MainItem],[Capital],[Product],[CHBRating]
,[Proportion1],[Proportion2],[ExterRating],[Stock],[StockNo],[Enterprise],[SalaryTransfer]
,[Average1_1],[Average1_2],[Average1_3],[Average1_4],[Average2_1],[Average2_2],[Average2_3],[Average2_4],[Exchange1_1],[Exchange1_2],[Exchange1_3],[Exchange1_4],[Exchange2_1],[Exchange2_2],[Exchange2_3],[Exchange2_4]
,[CreditExchange],[TranDate],[TotalFina],[BankShort],[TotalAmount],[CHBProp1],[CurrentTotal],[CHBProp2]
,[Add1],[Add2],[Add3],[Add4],[Add5],[RecentYear1],[RecentYear2],[RecentYear3]
,[Quota],[MainUse],[NotificationDay],[Eloan],[ProcessDay],[ApprovalDate],[AuditDays],[Type]
,[Descript],[VisitDate1],[VisitDate2],[VisitDate3],[VisitDate4],[VisitObject1],[VisitObject2],[VisitObject3],[VisitObject4]
,[Visitors1],[Visitors2],[Visitors3],[Visitors4],[NotificationDay2],[Manger]
,[Principal],[AO],[5P],[EvaluationDay],[Remarks]
,[CRC],[Manger2],[Principal2],[AO2]
,[CrtID],[CrtName],[CrtDate],[UpdID],[UpdName],[UpdDate],[Status]
)
VALUES
(
@No,@Brno,@BrName,@MangerBank,@FirstDay,@Name,@SetDate,@Agent,@ID,@Industry,@MainItem,@Capital,@Product,@CHBRating
,@Proportion1,@Proportion2,@ExterRating,@Stock,@StockNo,@Enterprise,@SalaryTransfer
,@Average1_1,@Average1_2,@Average1_3,@Average1_4,@Average2_1,@Average2_2,@Average2_3,@Average2_4,@Exchange1_1,@Exchange1_2,@Exchange1_3,@Exchange1_4,@Exchange2_1,@Exchange2_2,@Exchange2_3,@Exchange2_4
,@CreditExchange,@TranDate,@TotalFina,@BankShort,@TotalAmount,@CHBProp1,@CurrentTotal,@CHBProp2
,@Add1,@Add2,@Add3,@Add4,@Add5,@RecentYear1,@RecentYear2,@RecentYear3
,@Quota,@MainUse,@NotificationDay,@Eloan,@ProcessDay,@ApprovalDate,@AuditDays,@Type
,@Descript,@VisitDate1,@VisitDate2,@VisitDate3,@VisitDate4,@VisitObject1,@VisitObject2,@VisitObject3,@VisitObject4
,@Visitors1,@Visitors2,@Visitors3,@Visitors4,@NotificationDay2,@Manger
,@Principal,@AO,@5P,@EvaluationDay,@Remarks
,@CRC,@Manger2,@Principal2,@AO2
,@CrtID,@CrtName,getdate(),@UpdID,@UpdName,getdate(),'0'
)	 
    ">
    <SelectParameters>
        <asp:ControlParameter ControlID="hfNo" Name="No" PropertyName="Value" />
    </SelectParameters>
    <UpdateParameters>
		<asp:Parameter Name="MangerBank" /><asp:Parameter Name="FirstDay" /><asp:Parameter Name="Name" /><asp:Parameter Name="SetDate" /><asp:Parameter Name="Agent" /><asp:Parameter Name="Industry" />
        <asp:Parameter Name="MainItem" /><asp:Parameter Name="Capital" /><asp:Parameter Name="Product" /><asp:Parameter Name="CHBRating" />
        <asp:Parameter Name="Proportion1" /><asp:Parameter Name="Proportion2" /><asp:Parameter Name="ExterRating" /><asp:Parameter Name="Stock" /><asp:Parameter Name="StockNo" /><asp:Parameter Name="Enterprise" /><asp:Parameter Name="SalaryTransfer" />
        <asp:Parameter Name="Average1_1" /><asp:Parameter Name="Average1_2" /><asp:Parameter Name="Average1_3" /><asp:Parameter Name="Average1_4" />
        <asp:Parameter Name="Average2_1" /><asp:Parameter Name="Average2_2" /><asp:Parameter Name="Average2_3" /><asp:Parameter Name="Average2_4" />
        <asp:Parameter Name="Exchange1_1" /><asp:Parameter Name="Exchange1_2" /><asp:Parameter Name="Exchange1_3" /><asp:Parameter Name="Exchange1_4" />
        <asp:Parameter Name="Exchange2_1" /><asp:Parameter Name="Exchange2_2" /><asp:Parameter Name="Exchange2_3" /><asp:Parameter Name="Exchange2_4" />
        <asp:Parameter Name="CreditExchange" /><asp:Parameter Name="TranDate" /><asp:Parameter Name="TotalFina" /><asp:Parameter Name="BankShort" />
        <asp:Parameter Name="TotalAmount" /><asp:Parameter Name="CHBProp1" /><asp:Parameter Name="CurrentTotal" /><asp:Parameter Name="CHBProp2" />
        <asp:Parameter Name="Add1" /><asp:Parameter Name="Add2" /><asp:Parameter Name="Add3" /><asp:Parameter Name="Add4" /><asp:Parameter Name="Add5" />
        <asp:Parameter Name="RecentYear1" /><asp:Parameter Name="RecentYear2" /><asp:Parameter Name="RecentYear3" />
        <asp:Parameter Name="Quota" /><asp:Parameter Name="MainUse" /><asp:Parameter Name="NotificationDay" /><asp:Parameter Name="Eloan" /><asp:Parameter Name="ProcessDay" /><asp:Parameter Name="ApprovalDate" /><asp:Parameter Name="AuditDays" /><asp:Parameter Name="Type" /><asp:Parameter Name="Descript" />
        <asp:Parameter Name="VisitDate1" /><asp:Parameter Name="VisitDate2" /><asp:Parameter Name="VisitDate3" /><asp:Parameter Name="VisitDate4" />
        <asp:Parameter Name="VisitObject1" /><asp:Parameter Name="VisitObject2" /><asp:Parameter Name="VisitObject3" /><asp:Parameter Name="VisitObject4" />
        <asp:Parameter Name="Visitors1" /><asp:Parameter Name="Visitors2" /><asp:Parameter Name="Visitors3" /><asp:Parameter Name="Visitors4" />
        <asp:Parameter Name="NotificationDay2" /><asp:Parameter Name="5P" /><asp:Parameter Name="EvaluationDay" /><asp:Parameter Name="Remarks" />
        <asp:Parameter Name="UpdID" /><asp:Parameter Name="UpdName" />
        <asp:ControlParameter ControlID="hfNo" Name="No" PropertyName="Value" />
    </UpdateParameters>
    <InsertParameters>
        <asp:Parameter Name="No" Type="String"/>
        <asp:ControlParameter ControlID="hfBrNo" Name="Brno" PropertyName="Value" /><asp:Parameter Name="BrName" Type="String"/>
        <asp:Parameter Name="MangerBank" /><asp:Parameter Name="FirstDay" /><asp:Parameter Name="Name" /><asp:Parameter Name="SetDate" /><asp:Parameter Name="Agent" /><asp:Parameter Name="ID" /><asp:Parameter Name="Industry" />
        <asp:Parameter Name="MainItem" /><asp:Parameter Name="Capital" /><asp:Parameter Name="Product" /><asp:Parameter Name="CHBRating" />
        <asp:Parameter Name="Proportion1" /><asp:Parameter Name="Proportion2" /><asp:Parameter Name="ExterRating" /><asp:Parameter Name="Stock" /><asp:Parameter Name="StockNo" /><asp:Parameter Name="Enterprise" /><asp:Parameter Name="SalaryTransfer" />
        <asp:Parameter Name="Average1_1" /><asp:Parameter Name="Average1_2" /><asp:Parameter Name="Average1_3" /><asp:Parameter Name="Average1_4" />
        <asp:Parameter Name="Average2_1" /><asp:Parameter Name="Average2_2" /><asp:Parameter Name="Average2_3" /><asp:Parameter Name="Average2_4" />
        <asp:Parameter Name="Exchange1_1" /><asp:Parameter Name="Exchange1_2" /><asp:Parameter Name="Exchange1_3" /><asp:Parameter Name="Exchange1_4" />
        <asp:Parameter Name="Exchange2_1" /><asp:Parameter Name="Exchange2_2" /><asp:Parameter Name="Exchange2_3" /><asp:Parameter Name="Exchange2_4" />
        <asp:Parameter Name="CreditExchange" /><asp:Parameter Name="TranDate" /><asp:Parameter Name="TotalFina" /><asp:Parameter Name="BankShort" />
        <asp:Parameter Name="TotalAmount" /><asp:Parameter Name="CHBProp1" /><asp:Parameter Name="CurrentTotal" /><asp:Parameter Name="CHBProp2" />
        <asp:Parameter Name="Add1" /><asp:Parameter Name="Add2" /><asp:Parameter Name="Add3" /><asp:Parameter Name="Add4" /><asp:Parameter Name="Add5" />
        <asp:Parameter Name="RecentYear1" /><asp:Parameter Name="RecentYear2" /><asp:Parameter Name="RecentYear3" />
        <asp:Parameter Name="Quota" /><asp:Parameter Name="MainUse" /><asp:Parameter Name="NotificationDay" /><asp:Parameter Name="Eloan" /><asp:Parameter Name="ProcessDay" /><asp:Parameter Name="ApprovalDate" /><asp:Parameter Name="AuditDays" /><asp:Parameter Name="Type" /><asp:Parameter Name="Descript" />
        <asp:Parameter Name="VisitDate1" /><asp:Parameter Name="VisitDate2" /><asp:Parameter Name="VisitDate3" /><asp:Parameter Name="VisitDate4" />
        <asp:Parameter Name="VisitObject1" /><asp:Parameter Name="VisitObject2" /><asp:Parameter Name="VisitObject3" /><asp:Parameter Name="VisitObject4" />
        <asp:Parameter Name="Visitors1" /><asp:Parameter Name="Visitors2" /><asp:Parameter Name="Visitors3" /><asp:Parameter Name="Visitors4" />
        <asp:Parameter Name="NotificationDay2" /><asp:Parameter Name="Manger" /><asp:Parameter Name="Principal" /><asp:Parameter Name="AO" />
        <asp:Parameter Name="5P" /><asp:Parameter Name="EvaluationDay" /><asp:Parameter Name="Remarks" />
        <asp:Parameter Name="CRC" /><asp:Parameter Name="Manger2" /><asp:Parameter Name="Principal2" /><asp:Parameter Name="AO2" />
        <asp:Parameter Name="CrtID" Type="String"/><asp:Parameter Name="CrtName" Type="String"/><asp:Parameter Name="UpdID" Type="String"/><asp:Parameter Name="UpdName" Type="String"/>
    </InsertParameters>
</asp:SqlDataSource>
<asp:SqlDataSource ID="sdsSummit" runat="server" ConnectionString='<%$ ConnectionStrings:chb_iom %>'
    UpdateCommand="update KL set Status='1',[UpdID] = @UpdID ,[UpdName] = @UpdName,[UpdDate] =getdate()  where [No] = @No">
    <UpdateParameters>
        <asp:ControlParameter ControlID="hfNo" Name="No" PropertyName="Value" />
        <asp:Parameter Name="UpdID" Type="String"/><asp:Parameter Name="UpdName" Type="String"/>
    </UpdateParameters>
</asp:SqlDataSource>
<asp:HiddenField ID="hfBrNo" runat="server" />
<asp:HiddenField ID="hfBrName" runat="server" />
<asp:HiddenField ID="hfUserID" runat="server" />
<asp:HiddenField ID="hfUserName" runat="server" />
<asp:HiddenField ID="hfNo" runat="server" />
<asp:HiddenField ID="hfAction" runat="server" />
<asp:HiddenField ID="hfURL" runat="server" />