<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Menu.aspx.cs" Inherits="ContentAdmin.Menu" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>
        <%=Resources.Resource.txtAplicacion%></title>
    <script type="text/javascript" src="Scripts/Menu.js"></script>
    <link href="ccs/Menu.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Panel ID="PanelMenu" runat="server" Direction="NotSet" CssClass="menu">
            </asp:Panel>
    </div>
    <div class="imgLine" >
    </div>
    </form>
</body>
</html>
