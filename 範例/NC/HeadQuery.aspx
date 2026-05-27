<%@ Page Language="C#" AutoEventWireup="true" CodeFile="HeadQuery.aspx.cs" Inherits="Program_NC_HeadQuery" EnableEventValidation = "false" %>

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
                    <td bgcolor="khaki" >申請單位:
                    </td>
                    <td>
                    <asp:DropDownList ID="drpBrno" runat="server" AppendDataBoundItems="True" DataSourceID="sdsBrno" DataTextField="BRNAME" AutoPostBack="True"
                                             DataValueField="BRNO">
                                         </asp:DropDownList>
                    </td>
                </tr>

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

            <asp:BoundField DataField="狀態" HeaderText="狀態" ReadOnly="True" SortExpression="狀態" >                                                    
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
            SelectCommand="SELECT X.[DocNo]
      ,X.[BRNAME]
	  ,X.[EmpId]
	  ,X.Crt_Time
	  ,X.[Modify_Date]
	  ,X.[Flow_Type]
	  ,X.狀態
      ,'TRUE' as '是否可瀏覽'
FROM
(         
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
FROM NC A
    left join chb_pub..BRANCH B 
    ON A.Brno=B.BRNO
    left join tblLogDetail D
	ON A.DocNo=D.DocNo
WHERE A.Brno LIKE @Brno and convert(char,A.[Crt_Time],111)>=convert(char,@DateS,111) and convert(char,A.[Crt_Time],111)<=convert(char,@DateE,111)
          ) as X
WHERE X.狀態 LIKE @FlowStatus_text ;"> 
            <SelectParameters>
                <asp:Parameter Name="Brno" />
                <asp:Parameter Name="DateS" />
                <asp:Parameter Name="DateE" />
                <asp:Parameter Name="FlowStatus_text" />
            </SelectParameters> 
        </asp:SqlDataSource>
            <asp:SqlDataSource ID="sdsBrno" runat="server" ConnectionString="<%$ ConnectionStrings:chb_pub1 %>"
                SelectCommand="SELECT a.BRNO as BRNO, a.BRNAME as BRNAME
                               FROM (
                                   SELECT BRNO, BRNO + '-' + BRNAME AS BRNAME
                                   FROM BRANCH
                                   UNION
                                   SELECT '-001','-001-全部查詢'
                               ) as a
                               ORDER BY a.BRNO">
              <SelectParameters>
              </SelectParameters>
            </asp:SqlDataSource>
        </div>
    </form>
</body>
</html>
