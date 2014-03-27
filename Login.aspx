<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="ContentAdmin.Login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title><%=Resources.Resource.txtAplicacion%></title>
    <script type="text/javascript" language="javascript">
    <!--
        if (self != top) top.location = self.location;

        function foco() { 
            document.getElementById("txtLogin").focus();
        }

        function LimpiarMensajeError() {
            if (document.getElementById("lblError")) {
                document.getElementById("lblError").style.visibility = "hidden";
            }
        }

        function LimpiarMensajeError2() {
            if (document.getElementById("lblError")) {
                document.getElementById("lblError").style.visibility = "hidden";
            }
        }
    //-->
    </script>
    <link href="ccs/Main.css" rel="stylesheet" type="text/css" />
    <link href="ccs/Login.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        #imgLogo
        {
            height: 133px;
            width: 266px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div align="center">
        <div class="imgLogo">
            <img alt="SC" runat="server" src="~/Images/logo.png" id="imgLogo"  />
        </div>
        <asp:UpdatePanel ID="UpdatePanelLog" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>
                <asp:Panel ID="pnlLog" runat="server" CssClass="contenedorTablaLogin">
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtLogin" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:Resource, txtCampoRequerido %>" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword" Display="None" SetFocusOnError="true" ErrorMessage="<%$ Resources:Resource, txtCampoRequerido %>" />
                    
    				<table width="100%" height="90" border="0" cellpadding="0" cellspacing="0" bgcolor="">
                      <tr>
                        <td width="40%" height="90" align="left" valign="top">
                            <table width="100%" height="100%" border="0" cellpadding="0" cellspacing="0">
                              <tr>
                                <td width="100%" height="80" align="center" valign="middle">
                                    <asp:UpdatePanel ID="UpdatePanelError" runat="server" UpdateMode="Conditional" RenderMode="Block">
                                      <ContentTemplate>
                                        <div >
                                          <asp:Label ID="lblError" runat="server" CssClass="lblError" Visible="False" Width="180px" meta:resourcekey="lblError" />
                                        </div>
                                      </ContentTemplate>
                                      <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnLogin" EventName="Click"  />
                                        <asp:AsyncPostBackTrigger ControlID="lnkbtnOlvido" EventName="Click"  />
                                      </Triggers>
                                    </asp:UpdatePanel>
                                </td>
                              </tr>
                              <tr>
                                <td width="100%" height="20" align="left" valign="top">
                                    <asp:LinkButton ID="lnkbtnOlvido" runat="server" CausesValidation="false" Visible="false" meta:resourcekey="lnkbtnOlvido" />
                                </td>
                              </tr>
                            </table>
                        </td>
                        <td width="60%" height="90" align="left" valign="top">
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                              <tr>
                                <td width="100%" height="30" align="right" valign="middle" class="negrita">
                                    <asp:Label ID="lblLogin" runat="server" TabIndex="0" meta:resourcekey="lblLogin" />:&nbsp;
                                    <asp:TextBox ID="txtLogin" runat="server" />
                                </td>
                              </tr>
                              <tr>
                                <td width="100%" height="30" align="right" valign="middle" class="negrita">
                                    <asp:Label ID="lblPassword" runat="server" meta:resourcekey="lblPassword" />:&nbsp;
                                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" />
                                </td>
                              </tr>
                              <tr>
                                <td width="100%" height="30" align="right" valign="middle">
                                    <asp:Button ID="btnLogin" runat="server" OnClientClick="LimpiarMensajeError()" 
                                        meta:resourcekey="btnLogin" onclick="btnLogin_Click" />
                                </td>
                              </tr>
                            </table>
                        </td>
                      </tr>
                    </table>
                </asp:Panel>
                
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkbtnOlvido" EventName="Click"  />
                <asp:AsyncPostBackTrigger ControlID="lnkbtnBack" EventName="Click"  />
            </Triggers>
        </asp:UpdatePanel>
        
    	<asp:UpdatePanel ID="UpdatePanelRemind" runat="server" UpdateMode="Conditional" RenderMode="Inline">
            <ContentTemplate>  
                <asp:Panel ID="pnlRemind" runat="server" CssClass="contenedorTablaLogin" Visible="false">        
        
        			<table width="100%" height="90" border="0" cellpadding="0" cellspacing="0">
                      <tr>
                        <td width="100%" height="80" align="center" valign="middle">
                            
                            <asp:UpdatePanel ID="UpdatePanelRemind2" runat="server" UpdateMode="Conditional" RenderMode="Inline">
                                <ContentTemplate>
                                    <asp:Label ID="lblError2" runat="server" CssClass="lblError" Visible="false" Width="250" />
                                    <asp:Label ID="lblMensaje" runat="server" meta:resourcekey="lblMensaje" /><br /><br />
                                    <asp:TextBox ID="txtEmail" runat="server" Width="200" />
                                    <asp:Button ID="btnEnviar" Text="Enviar" runat="server" OnClientClick="LimpiarMensajeError2()" meta:resourcekey="btnEnviar" />
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnEnviar" EventName="Click" />
                                </Triggers>
                            </asp:UpdatePanel>
                            
                        </td>
                      </tr>
                      <tr>
                        <td width="100%" height="20" align="left" valign="top">
                            <asp:LinkButton ID="lnkbtnBack" runat="server" CausesValidation="false" meta:resourcekey="lnkbtnBack" />
                        </td>
                      </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="lnkbtnOlvido" EventName="Click"  />
                <asp:AsyncPostBackTrigger ControlID="lnkbtnBack" EventName="Click"  />
            </Triggers>
        </asp:UpdatePanel>

        <asp:UpdatePanel ID="UpdatePanelWait" runat="server" UpdateMode="Always">
            <ContentTemplate>
              <asp:UpdateProgress ID="UpdateProgress" DisplayAfter="0" runat="server">
                  <ProgressTemplate>                                                                
                      <div class="pleaseWait"><img src="Images/wait.gif" align="absmiddle" runat="server" 
                              id="Img1" />&nbsp;<%=Resources.Resource.txtWait%></div>
                  </ProgressTemplate>                              
              </asp:UpdateProgress>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
