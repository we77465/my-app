<%@ Page Language="C#" AutoEventWireup="true" CodeFile="import.aspx.cs" Inherits="Program_NC_import" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>


    <asp:SqlDataSource ID="sdsNCDelete" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>"
      DeleteCommand="DELETE tblLogDetail
WHERE DocType='NC' AND DocNo IN
(
SELECT DocNo
FROM NC
WHERE Crt_Time=@Crt_Time
);DELETE NC WHERE Crt_Time=@Crt_Time;">
            <DeleteParameters>
               <asp:Parameter Name="Crt_Time" />
            </DeleteParameters>
     </asp:SqlDataSource>


    <asp:SqlDataSource ID="sdsNCModify" runat="server" ConnectionString="<%$ ConnectionStrings:chb_eform %>"
      DeleteCommand="DELETE NC WHERE DocNo=@DocNo;DELETE tblLogDetail WHERE DocNo=@DocNo AND DocType='NC';
        "
         InsertCommand="
         INSERT INTO [dbo].[NC]
         (
	[DocNo],
	[Party_Id],
	[Apply_Identity],
	[Apply_Item],
	[Party_Name],
	[Out_Party_Name],
	[Zip_Code],
	[City],
	[Area],
	[Addr],
	[Contact_Person],
	[TEL],
	[MAIL],
	[Brno],
	[Contact_Time],
	[Business_Content],
	[Remark],
	[Crt_Time],
    [As_Of_Date]
            )
     VALUES  
         (
	@DocNo,
	@Party_Id,
	@Apply_Identity,
	@Apply_Item,
	@Party_Name,
	@Out_Party_Name,
	@Zip_Code,
	@City,
	@Area,
	@Addr,
	@Contact_Person,
	@TEL,
	@MAIL,
	@Brno,
	@Contact_Time,
	@Business_Content,
	@Remark,
	@Crt_Time,
        GETDATE()
         );
         INSERT INTO [tblLogDetail] ([DocType],[DocNo],[BrNo],[EmpId],[RecordDate],[YYMM],[FlowStatus],[Ver]) 
                                    VALUES('NC',@DocNo,@Brno,'SYS',GETDATE(),@YYMM,'-2','1');">
            <DeleteParameters>
               <asp:Parameter Name="DocNo" />
            </DeleteParameters>
            <InsertParameters>
                 <asp:Parameter Name="DocNo" />
                 <asp:Parameter Name="Party_Id" />
                 <asp:Parameter Name="Apply_Identity" />
                 <asp:Parameter Name="Apply_Item" />
                 <asp:Parameter Name="Party_Name" />
                 <asp:Parameter Name="Out_Party_Name" />
                 <asp:Parameter Name="Zip_Code" />
                 <asp:Parameter Name="City" />
                 <asp:Parameter Name="Area" />
                 <asp:Parameter Name="Addr" />
                 <asp:Parameter Name="Contact_Person" />
                 <asp:Parameter Name="TEL" />
                 <asp:Parameter Name="MAIL" />
                 <asp:Parameter Name="Brno" />
                 <asp:Parameter Name="Contact_Time" />
                 <asp:Parameter Name="Business_Content" />
                 <asp:Parameter Name="Remark" />
                 <asp:Parameter Name="Crt_Time" /> 
                 <asp:Parameter Name="YYMM" /> 
            </InsertParameters>  
     </asp:SqlDataSource>


        </div>
    </form>
</body>
</html>
