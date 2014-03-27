<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrPlataforma.ascx.cs" Inherits="ContentAdmin.ctrPlataforma" %>
<link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register TagPrefix="custom" Namespace="ContentAdmin" Assembly="ContentAdmin" %>

<table id="Table2" cellspacing="2" cellpadding="1" width="100%" border="1" rules="none"
    style="border-collapse: collapse">
    <tr class="EditFormHeader">
        <td colspan="2">
            <b><asp:Label ID="lblItem" runat="server" Text=""></asp:Label></b>
        </td>
    </tr>
    <tr>
        <td>
            <table cellpadding="2" cellspacing="2" width="80%" border="0">
                <tr>
                    <td style="width: 150px;" class="textoBold">
                        <%=Resources.NewResource.txtNombrePlataforma%>
                    </td> 
                    <td>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombre" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td valign="top" style="width: 150px;" class="textoBold">
                        <%=Resources.NewResource.txtIdioma%>
                    </td>
                    <td valign="top">
                        <custom:ListControl ID="ctrIdiom" runat="server" Post="true" />
                    </td>
                </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnUpdate" Text="Update" runat="server" CommandName="Update" Visible='<%# !(DataItem is Telerik.Web.UI.GridInsertionObject) %>'>
            </asp:Button>
            <asp:Button ID="btnInsert" Text="Insert" runat="server" CommandName="PerformInsert"
                Visible='<%# DataItem is Telerik.Web.UI.GridInsertionObject %>' 
                onclick="btnInsert_Click"></asp:Button>
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                CommandName="Cancel"></asp:Button>
        </td>
    </tr>
</table>
