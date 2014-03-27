<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrHeader.ascx.cs" Inherits="ContentAdmin.ctrHeader" %>
<link href="css/Header.css" rel="stylesheet" type="text/css" />
<table width="100%" height="100px" border="0" cellpadding="0" cellspacing ="0" style="height: 100px">
    <tr>
        <td>
            <img alt="SC" runat="server" src="../Images/Logo.png" id="imgSC" style="width: 144px; height: 86px" /></td>
        <td align="right">
            <img alt="" runat="server" id="imgAdm" src="../Images/titulo.jpg" style="width: 554px; height: 61px" /></td>
    </tr>
    <tr>
        <td colspan="2">
            <img alt="" src="../Images/pixel.gif" style="width: 1px; height: 2px" />
            <div class="BarraHeaderMenu" align="left">
                <asp:Label ID="lblSeccion" runat="server" CssClass="TituloSeccion"></asp:Label>
            </div>            
        </td>
    </tr>
</table>
    <img alt="" src="../Images/pixel.gif" style="width: 1px; height: 1px" />