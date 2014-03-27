<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageDownloadZip.aspx.cs" Inherits="ContentAdmin.PageDownloadZip" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
    </telerik:RadScriptManager>

    <div align="center" style="padding: 20px 0px 0px 0px">
        <telerik:RadFileExplorer runat="server" ID="RadFileExplorerZip" Width="680px" 
            Height="490px" EnableFilteringOnEnterPressed="False" 
            VisibleControls="TreeView, Grid, Toolbar, ContextMenus" 
            onitemcommand="RadFileExplorerZip_ItemCommand">
            <Configuration ViewPaths="http://174.142.104.139/File/FileZip/" />
        </telerik:RadFileExplorer>
    </div>
    </form>
</body>
</html>
