<%@ Page Language="C#" AutoEventWireup="true" CodeFile="approve.aspx.cs" Inherits="Program_NC_approve" %>

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
     <asp:HiddenField ID="hfUserID" runat="server" />
                                <tr>

                        <td>
                            <asp:Button ID="btnSign" runat="server" OnClick="btnSign_Click" style="font-family: 微軟正黑體; font-size: medium" Text="放行" />
                        </td>
&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        <td>
                            <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" style="font-family: 微軟正黑體; font-size: medium" Text="退回" />
                        </td>
                    </tr>
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
            <asp:BoundField DataField="EmpId" HeaderText="員工編號" ReadOnly="True" SortExpression="EmpId" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>


            <asp:BoundField DataField="Crt_Time" HeaderText="申請日期" ReadOnly="True" SortExpression="Crt_Time" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField> 
            <asp:BoundField DataField="Modify_Date" HeaderText="異動日期" ReadOnly="True" SortExpression="Modify_Date" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>   

            <asp:BoundField DataField="Flow_Type" HeaderText="流程狀態" ReadOnly="True" SortExpression="Flow_Type" >                                                    
                <ItemStyle BorderColor="Gray" BorderStyle="Solid" BorderWidth="1px" />
            </asp:BoundField>       
                        <asp:TemplateField>
                <ItemTemplate>
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
          SELECT A.[DocNo]
      ,B.[BRNAME]
	  ,A.[EmpId]
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
      ,'TRUE' as '是否可瀏覽'
FROM NC A
    left join chb_pub..BRANCH B 
    ON A.Brno=B.BRNO
    left join tblLogDetail D
	ON A.DocNo=D.DocNo
WHERE A.[DocNo]=@DocNo;"> 
            <SelectParameters>
                <asp:Parameter Name="DocNo" />
            </SelectParameters> 
        </asp:SqlDataSource>
    <asp:SqlDataSource ID="sdsNC" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>"
        SelectCommand="select a.*,b.[FlowStatus] from [NC] a left join tblLogDetail b on a.DocNo=b.DocNo where a.[DocNo]=@DocNo"
        UpdateCommand="update tblLogDetail set Sign1= @Sign1,Action1 = @Action1,SignDate1 = GETDATE(),FlowStatus = @FlowStatus where DocNo= @DocNo and DocType='NC'
">
            <SelectParameters>
               <asp:Parameter Name="DocNo" />
            </SelectParameters>
            <UpdateParameters>
                        <asp:ControlParameter ControlID="hfUserID" Name="Sign1" PropertyName="Value" />
                        <asp:Parameter Name="Action1" />
                        <asp:Parameter Name="FlowStatus" />
                         <asp:QueryStringParameter Name="DocNo" />
            </UpdateParameters>  
     </asp:SqlDataSource>

            <asp:SqlDataSource ID="sdsUpd2" runat="server" ConnectionString='<%$ ConnectionStrings:chb_eform %>'
                     UpdateCommand="update tblLogDetail set Sign2= @Sign2,Action2 = @Action2,SignDate2 = GETDATE(),FlowStatus = @FlowStatus where DocNo= @DocNo and DocType='NC'"
                  >
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="hfUserID" Name="Sign2" PropertyName="Value" />
                        <asp:Parameter Name="Action2" />
                        <asp:Parameter Name="FlowStatus" />
                         <asp:QueryStringParameter Name="DocNo" />
                    </UpdateParameters>
                </asp:SqlDataSource>
             <asp:SqlDataSource ID="sdsUpd3" runat="server" ConnectionString='<%$ ConnectionStrings:chb_eform %>'
                     UpdateCommand="update tblLogDetail set Sign1= @Sign1,Action1 = 'B',SignDate1 = GETDATE(),FlowStatus = '0' where DocNo= @DocNo and DocType='NC'"
                  >
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="hfUserID" Name="Sign1" PropertyName="Value" />
                         <asp:QueryStringParameter Name="DocNo" />
                    </UpdateParameters>
                </asp:SqlDataSource>
            <asp:SqlDataSource ID="sdsUpd4" runat="server" ConnectionString='<%$ ConnectionStrings:chb_eform %>'
                     UpdateCommand="update tblLogDetail set  Sign2= @Sign2,Action2 = 'B',SignDate2 = GETDATE(),FlowStatus = '0' where DocNo= @DocNo and DocType='NC'"
                  >
                    <UpdateParameters>
                        <asp:ControlParameter ControlID="hfUserID" Name="Sign2" PropertyName="Value" />
                         <asp:QueryStringParameter Name="DocNo" />
                    </UpdateParameters>
                </asp:SqlDataSource>


        </div>
    </form>
</body>
</html>
