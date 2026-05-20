<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Add.aspx.cs" Inherits="Program_KL_Add" %>

<%@ Register Src="~/Program/KL/Detail.ascx" TagPrefix="uc1" TagName="Detail" %>


<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
     <style type="text/css">
            .ui-dialog .ui-dialog-content { background: lightgrey; }
        #bPrint {
            font-family: 微軟正黑體;
            font-size: medium;
        }
        #bPrint0 {
            font-family: 微軟正黑體;
            font-size: medium;
        }
        #bPrintUn {
            font-family: 微軟正黑體;
            font-size: medium;
        }
        </style>
</head>
<body bgcolor="#FFFFE7">
    <form id="form1" runat="server">
        <div>
            <uc1:Detail runat="server" ID="Detail" />
        </div>
    </form>
</body>
</html>
