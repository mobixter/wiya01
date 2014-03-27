<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrItem.ascx.cs" Inherits="ContentAdmin.ctrItem" %>
<link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="custom" Namespace="ContentAdmin" Assembly="ContentAdmin" %>

<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="1" rules="none"
    style="border-collapse: collapse">
    <tr class="EditFormHeader">
        <td colspan="2">
            <b>
                <asp:Label ID="lblItem" runat="server" Text=""></asp:Label></b>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="2" cellspacing="2" width="100%" border="0">
                <tr>
                    <td valign="top" style="width: 180px;" class="textoBold">
                        <%=Resources.NewResource.txtStrId%>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtStrId" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtStrId" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td align="center" class="textoBold">
                        <%=Resources.NewResource.txtNombreItemEn%>
                    </td>
                    <td align="center" class="textoBold">
                        <%=Resources.NewResource.txtNombreItemPo%>
                    </td>
                    <td style="width: 250px;">
                    </td>
                    <td rowspan="6">
                        <table border="0" cellpadding="2" cellspacing="2" width="100%">
                            <tr>
                                <td valign="top" style="width: 150px;" class="textoBold">
                                    <%=Resources.NewResource.txtTipoItem%>
                                </td>
                                <td valign="top">
                                    <custom:ListControl ID="ctrTipoItem" runat="server" Post="true" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 150px;" class="textoBold">
                                    <%=Resources.NewResource.txtProveedor%>
                                </td>
                                <td valign="top">
                                    <custom:ListControl ID="ctrProveedor" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 148px;" class="textoBold">
                                    <%=Resources.NewResource.txtArtista%>
                                </td>
                                <td valign="top" class="texto" style="margin-left: 2px">
                                    <custom:ListControl ID="ctrArtista" runat="server" />
                                </td>            
                            </tr>
                            <tr>
                                <td valign="top" style="width: 148px;" class="textoBold">
                                    <%=Resources.NewResource.txtIsrcGrid%>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtIsrcGrid" runat="server" CssClass="texto" Text="" Width="213px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 148px;" class="textoBold">
                                    <%=Resources.NewResource.txtUpc%>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtUpc" runat="server" CssClass="texto" Text="" Width="213px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 148px;" class="textoBold">
                                    <%=Resources.NewResource.txtAdvisory%>
                                </td>
                                <td valign="top">
                                    <asp:DropDownList ID="cboAdvisory" runat="server" CssClass="texto" Enabled="True">
                                        <asp:ListItem Selected="True" Value="No"> No </asp:ListItem>
                                        <asp:ListItem Value="Yes"> Yes </asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                
                            <tr>
                                <td valign="top" style="width: 148px;" class="textoBold">
                                    <%=Resources.NewResource.txtChargeType%>
                                </td>
                                <td valign="top">
                                    <custom:ListControl ID="ctrChaegeType" runat="server" />
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="textoBold">
                                    <%=Resources.NewResource.txtKeyword%>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtKeyword" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" CssClass="LabelRequerido"
                                        ControlToValidate="txtKeyword" ErrorMessage='*'></asp:RequiredFieldValidator>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="textoBold">
                                    <%=Resources.NewResource.txtSetElemntId%>
                                </td>
                                <td valign="top">
                                    <asp:TextBox ID="txtSetElemntId" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" class="textoSmall" colspan="2">
                                    <%=Resources.NewResource.txtMsgKeyword%>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 180px;" class="textoBold">
                        <%=Resources.NewResource.txtNombreItem%>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtNombreSp" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombreSp" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtNombreEn" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombreEn" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td valign="top">
                        <asp:TextBox ID="txtNombrePo" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombrePo" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td style="width: 250px;">
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="textoBold">
                        <%=Resources.NewResource.txtDescripcion%>
                    </td>
                    <td class="style2">
                        <asp:TextBox ID="txtDescSp" runat="server" CssClass="texto" Text="" Width="215px" Height="60px"
                            TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtDescSp" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td  class="style2">
                        <asp:TextBox ID="txtDescEn" runat="server" CssClass="texto" Text="" Width="215px" Height="60px"
                            TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtDescEn" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td  class="style2">
                        <asp:TextBox ID="txtDescPo" runat="server" CssClass="texto" Text="" Width="215px" Height="60px"
                            TextMode="MultiLine"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtDescPo" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                    <td class="style3">
                    </td>
                </tr>
                <tr>
                    <td valign="top" class="textoBold" colspan="4" rowspan="5">
                        <div id="contentCategory" runat="server">
                            <table cellpadding="2" cellspacing="2" border="0" width="100%">
                                <tr align="center">
                                    <td class="texto">
                                        <b>Published category (Spanish *|* English *|* Portuguese)</b><br />
                                        <asp:ListBox ID="itemIdTodos" CssClass="texto" Width="400px" Rows="11" SelectionMode="Multiple"
                                            runat="server" Height="150px"></asp:ListBox>
                                    </td>
                                    <td align="center" valign="middle" class="texto">
                                        <input class="boton" onclick="javascript:Incluir('itemIdTodos', 'itemIdAsociados');" type="button" value=">>" /><br />
                                            <b>Add</b><br />
                                        <input class="boton" onclick="javascript:Regresar('itemIdTodos', 'itemIdAsociados');" type="button" value="<<" />
                                    </td>
                                    <td class="texto">
                                        <b>Associated category (Spanish *|* English *|* Portuguese)</b><br />
                                        <asp:ListBox ID="itemIdAsociados" CssClass="texto" Width="400px" Rows="11" SelectionMode="Multiple"
                                            runat="server" Height="150px"></asp:ListBox>
                                    </td>
                                </tr>
                                <tr align="center">
                                    <td class="texto">
                                        <b>Published platform</b><br />    
                                        <asp:ListBox ID="platformIdTodos" CssClass="texto" Width="180px" Rows="5" SelectionMode="Multiple"
                                            runat="server"></asp:ListBox>
                                    </td>
                                    <td align="center" valign="middle" class="texto">
                                        <input class="boton" onclick="javascript:Incluir('platformIdTodos', 'platformIdAsociados');" type="button" value=">>" /><br />
                                            <b>Add</b><br />
                                        <input class="boton" onclick="javascript:Regresar('platformIdTodos', 'platformIdAsociados');" type="button" value="<<" />
                                    </td>
                                    <td class="texto">
                                        <b>Associated platform</b><br />
                                        <asp:ListBox ID="platformIdAsociados" CssClass="texto" Width="180px" Rows="5" SelectionMode="Multiple"
                                            runat="server"></asp:ListBox>
                                    </td>
                                </tr>
                            </table>
                        </div> 
                    </td>
                    <td class="style3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td class="style3">
                        &nbsp;</td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td valign="top">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%">
                            <tr>
                                <td colspan="2" class="textoBold">
                                    Preview file (.jpg|.gif min: 480X480px Max: 640X640px): 
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadAsyncUpload runat="server" ID="rdUploadPreview" UploadedFilesRendering="BelowFileInput"
                                        AllowedFileExtensions=".gif,.jpeg,.jpg" OnInit="rdUploadPreview_Init" ViewStateMode="Enabled">
                                        <Localization Select="Upload File" />
                                    </telerik:RadAsyncUpload>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="textoBold">
                                    Thumbnail file (.jpg|.gif min: 480X480px Max: 640X640px):
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadAsyncUpload runat="server" ID="rdUploadThumbnail" UploadedFilesRendering="BelowFileInput"
                                        AllowedFileExtensions=".gif,.jpeg,.jpg" OnInit="rdUploadThumbnail_Init" 
                                        ViewStateMode="Enabled" MaxFileInputsCount="1">
                                        <Localization Select="Upload File" />
                                    </telerik:RadAsyncUpload>
                                </td>
                            </tr>
                            <tr>
                                <td style="height: 40px">
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2" class="textoBold">
                                    Master file (.zip):
                                </td>
                            </tr>
                            <tr>
                                <td valign="top" style="width: 150px;" class="textoBold">
                                    Minimal specifications:
                                </td>
                                <td valign="top" class="texto">
                                    <custom:ListControl ID="ListControl1" runat="server" />
                                    <asp:CheckBox ID="cbMinimal" runat="server" CssClass="texto" />
                                </td>
                            </tr>
                            <tr>
                                <td colspan="2">
                                    <telerik:RadAsyncUpload runat="server" ID="rdUploadFile" UploadedFilesRendering="BelowFileInput"
                                        OnInit="rdUploadFile_Init" AllowedFileExtensions=".zip" 
                                        MaxFileInputsCount="1">
                                        <Localization Select="Upload File" />
                                    </telerik:RadAsyncUpload>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnUpdate" 
                Text="Update"
                runat="server" CommandName="Update"  
                Visible='<%# !(DataItem is Telerik.Web.UI.GridInsertionObject) %>' 
                >
                
            
            </asp:Button>
            <asp:Button ID="btnInsert" Text="Insert"
                runat="server" CommandName="Insert" Visible='<%# DataItem is Telerik.Web.UI.GridInsertionObject %>'>
            </asp:Button>
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                CommandName="Cancel"></asp:Button>
        </td>
    </tr>
</table>
