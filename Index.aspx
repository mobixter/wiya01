<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Index.aspx.cs" Inherits="ContentAdmin.Index" %>

<%@ Register src="Controls/ctrHeader.ascx" tagname="ctrHeader" tagprefix="SC" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head id="Head1" runat="server">
        <title><%=Resources.Resource.txtAplicacion%></title>
    </head>
    <frameset border="0" id="FrameSup" rows="120,30,*" framespacing='0' frameborder='0' tabindex='-1'>
        <frame src="Header.aspx" name='header' frameborder='0' scrolling='no'>
		<frame src="Menu.aspx" name='menu' frameborder='0' scrolling='no'>
		<frame src="Main.aspx" name='atrabajo' frameborder='0' scrolling='auto'>
	</frameset>
</html>