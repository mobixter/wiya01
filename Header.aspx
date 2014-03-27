<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Header.aspx.cs" Inherits="ContentAdmin.Header" %>

<%@ Register src="Controls/ctrHeader.ascx" tagname="ctrHeader" tagprefix="SC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title><%=Resources.Resource.txtAplicacion%></title>
    <link href="ccs/Main.css" rel="stylesheet" type="text/css" />
    <link href="ccs/Header.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <SC:ctrHeader ID="ctrHeaderId" runat="server" />
    </div>
    </form>
</body>
</html>
