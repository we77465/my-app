<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Query.aspx.cs" Inherits="Program_NC_Query" EnableEventValidation = "false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
        <strong>
            <asp:HiddenField ID="hidn_EmpNo" runat="server" />
        </strong>
<asp:HiddenField ID="hidn_BRNO" runat="server" />

            <table>
                        <tr>
                <td bgcolor="khaki" style="width: 20%">
                    申請日期：</td>
                <td bgcolor="khaki" align="left">

                        <asp:TextBox ID="txtDateS" runat="server" CssClass="auto-style2" MaxLength="10" Width="100px"></asp:TextBox><span class="auto-style11"></span>~
                        <asp:TextBox ID="txtDateE" runat="server" CssClass="auto-style2" MaxLength="10" Width="100px"></asp:TextBox><span class="auto-style11">(西元年月日 例： 20221231)</span><br />


                </td>
                            </tr>
                <tr>
                    <td bgcolor="khaki" style="width: 20%">
                    狀態：
                        </td>
                    <td>
                                        <asp:DropDownList ID="drListFlowStatus" runat="server" AutoPostBack="True" 
                        DataSourceID="dsFlowStatus" DataTextField="FlowStatus_NAME" DataValueField="FlowStatus_NAME">
                    </asp:DropDownList>
                    <asp:SqlDataSource ID="dsFlowStatus" runat="server" 
                        ConnectionString="<%$ ConnectionStrings:chb_eform %>" 
                        SelectCommand="SELECT '0.查詢全部' AS FlowStatus_NAME UNION SELECT A.FlowStatus_NAME FROM (
select DISTINCT case when FlowStatus = -2 then '1.編製中' when FlowStatus = 0 then '3.退回' when FlowStatus = 1 then '4.待簽核' when FlowStatus  >= 3 then '6.已決行' when isnull(FlowStatus,'') = '' then '1.編製中' end as FlowStatus_NAME from tblLogDetail WHERE DocType='NC') AS A
WHERE A.FlowStatus_NAME<>' '
ORDER BY FlowStatus_NAME ASC">
                    </asp:SqlDataSource>
                        </td>
                </tr>

                            <tr>
                                            <td align="center" bgcolor="khaki" colspan="2">
                    <asp:Button ID="Btn_Submit" runat="server" Text="送出查詢" OnClick="Btn_Submit_Click" />
                    <asp:Button ID="Btn_Excel" runat="server" Text="匯出EXCEL" OnClick="btnExcel_Click" />
                </td>
                                </tr>

                </table>


        <asp:GridView ID="gv已存在的表單" runat="server" AllowPaging="True" 
        AllowSorting="True" AutoGenerateColumns="False" DataKeyNames="DocNo" 
        DataSourceID="sdNC" Font-Names="標楷體" Font-Size="1.1em" PageSize="6" 
        Width="100%" BackColor="#DEBA84" BorderColor="#DEBA84" BorderStyle="None" 
        BorderWidth="1px" CellPadding="3" CellSpacing="2" OnRowCommand="GridView1_RowCommand"
        ShowHeaderWhenEmpty="True">
        <Columns> 

            <asp:BoundField DataField="DocNo" HeaderText="案件編號" ReadOnly="True" SortExpression="DocNo">                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>

            <asp:BoundField DataField="BRNAME" HeaderText="營業單位" ReadOnly="True" SortExpression="BRNAME" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>

                <asp:TemplateField HeaderText="員工編號" SortExpression="EmpId">
                    <ItemTemplate>
                            <asp:TextBox ID="tb員工編號" runat="server" Text='<%# Eval("EmpId") %>' Font-Size="X-Small" style="text-align:left" Width="100px" Rows="1" TextMode="MultiLine">
                            </asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>

            <asp:BoundField DataField="Crt_Time" HeaderText="申請日期" ReadOnly="True" SortExpression="Crt_Time" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField> 
            <asp:BoundField DataField="Modify_Date" HeaderText="異動日期" ReadOnly="True" SortExpression="Modify_Date" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>  
                <asp:TemplateField HeaderText="流程狀態" SortExpression="Flow_Type">
                    <ItemTemplate>

                    <asp:CheckBox ID="CheckFlow_Type1" runat="server" Text="1.未處理" Checked='<%# Eval("Flow_Type").ToString().Contains("1.未處理") %>'/><BR/>
                    <asp:CheckBox ID="CheckFlow_Type2" runat="server" Text="2.處理中" Checked='<%# Eval("Flow_Type").ToString().Contains("2.處理中") %>'/><BR/>
                    <asp:CheckBox ID="CheckFlow_Type3" runat="server" Text="3.已結案-欲合作" Checked='<%# Eval("Flow_Type").ToString().Contains("3.已結案-欲合作") %>'/><BR/>
                    <asp:CheckBox ID="CheckFlow_Type4" runat="server" Text="4.已結案-未合作" Checked='<%# Eval("Flow_Type").ToString().Contains("4.已結案-未合作") %>'/><BR/>

                    </ItemTemplate><HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Width="3%" /><ItemStyle HorizontalAlign="Left" VerticalAlign="Top"/>
                </asp:TemplateField>          



            <asp:BoundField DataField="狀態" HeaderText="狀態" ReadOnly="True" SortExpression="狀態" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>    
            <asp:TemplateField>
                <ItemTemplate>


                    <asp:LinkButton ID="lb修改" runat="server"                                                         
                        CommandArgument='<%# (String)Eval("DocNo") %>'                                                             
                        CommandName="MyEdit" 
                        Visible='<%# Convert.ToBoolean(Eval("是否可編輯")) %>' 
                        BackColor="Peru" BorderColor="BurlyWood" BorderStyle="Outset" ForeColor="White">修改</asp:LinkButton>
                    <asp:LinkButton ID="LinkButton1" runat="server"                                                         
                        CommandArgument='<%# (String)Eval("DocNo") %>'                                                             
                        CommandName="Send" 
                        Visible='<%# Convert.ToBoolean(Eval("是否可編輯")) &&  Convert.ToBoolean(Eval("是否可送簽")) %>' 
                        BackColor="Peru" BorderColor="BurlyWood" BorderStyle="Outset" ForeColor="White">送簽</asp:LinkButton>
                                        <asp:LinkButton ID="lb瀏覽" runat="server"
                        CommandArgument='<%# (String)Eval("DocNo") %>' 
                        CommandName="MyReadOnly" 
                        Visible='<%# Convert.ToBoolean(Eval("是否可瀏覽"))%>' BackColor="Peru" 
                        BorderColor="BurlyWood" BorderStyle="Outset" ForeColor="White">瀏覽</asp:LinkButton>
                </ItemTemplate>                                        
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />                                                    
            </asp:TemplateField>               

        </Columns>                                    
            <EmptyDataTemplate>
                <asp:Label ID="Label42" runat="server" Font-Names="標楷體" ForeColor="#990000" Text="無資料！"></asp:Label>                                                    
            </EmptyDataTemplate>                                            
            <FooterStyle BackColor="#F7DFB5" ForeColor="#8C4510" />
            <HeaderStyle BackColor="#A55129" Font-Bold="True" ForeColor="White" />
            <PagerStyle ForeColor="#009933" HorizontalAlign="Center" />
            <RowStyle BackColor="#FFF7E7" ForeColor="#8C4510" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#738A9C" Font-Bold="True" ForeColor="White" />                                           
        </asp:GridView>

      <asp:SqlDataSource ID="sdNC" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>"                    
            SelectCommand="
          
SELECT X.[DocNo]
      ,X.[BRNAME]
	  ,X.[EmpId]
	  ,X.Crt_Time
	  ,X.[Modify_Date]
	  ,X.[Flow_Type]
	  ,X.狀態
      ,'TRUE' as '是否可瀏覽'
      ,X.是否可編輯
      ,X.是否可送簽
FROM
(          
          
          SELECT A.[DocNo]
      ,B.[BRNAME]
	  ,ISNULL(A.[EmpId],@EmpId) as EmpId
	  ,convert(char,A.[Crt_Time],111) as Crt_Time
	  ,FORMAT(A.[Modify_Date], 'yyyy/MM/dd') as [Modify_Date]
	  ,A.[Flow_Type]
                , case  
                    when D.FlowStatus = -2 then '編製中' 
                    when D.FlowStatus = -1 then '總行退回' 
                    when D.FlowStatus = 0 then '退回' 
                    when D.FlowStatus = 1 then '待簽核'
                    when D.FlowStatus = 2 then '待決行' 
                    when D.FlowStatus  >= 3 then '已決行' 
                    when isnull(D.FlowStatus,'') = '' then '編製中'
                end as '狀態'
      ,case when 1 > cast(D.FlowStatus as integer) then 'TRUE' else 'FALSE' end as '是否可編輯'
      ,case when 1 > cast(D.FlowStatus as integer) then 'TRUE' else 'FALSE' end as '是否可送簽'
FROM NC A
    left join chb_pub..BRANCH B 
    ON A.Brno=B.BRNO
    left join tblLogDetail D
	ON A.DocNo=D.DocNo
WHERE A.Brno LIKE @Brno and convert(char,A.[Crt_Time],111)>=convert(char,@DateS,111) and convert(char,A.[Crt_Time],111)<=convert(char,@DateE,111)
) as X
WHERE X.狀態 LIKE @FlowStatus_text           
          ;"> 
            <SelectParameters>
                <asp:Parameter Name="EmpId" />
                <asp:Parameter Name="Brno" />
                <asp:Parameter Name="DateS" />
                <asp:Parameter Name="DateE" />
                <asp:Parameter Name="FlowStatus_text" />


            </SelectParameters> 
        </asp:SqlDataSource>
            <asp:SqlDataSource ID="sdNCModify" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>" 
            UpdateCommand="UPDATE NC 
                SET EmpId=@EmpId
                   ,Flow_Type=@Flow_Type
                   ,Modify_Date=GETDATE()
                WHERE DocNo=@DocNo;
                UPDATE tblLogDetail
                SET EmpId=@EmpId
                   ,FlowStatus=@FlowStatus
                WHERE DocNo=@DocNo">  

            <UpdateParameters>
                <asp:Parameter Name="DocNo"></asp:Parameter>
                <asp:Parameter Name="EmpId"></asp:Parameter>
                <asp:Parameter Name="Flow_Type"></asp:Parameter>
                <asp:Parameter Name="FlowStatus"></asp:Parameter>
            </UpdateParameters>

            </asp:SqlDataSource>

        </div>
    </form>
</body>
</html>
