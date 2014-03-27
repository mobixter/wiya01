<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrCategoria.ascx.cs"
    Inherits="ContentAdmin.ctrCategoria" %>
<link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
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
                        <%=Resources.NewResource.txtNombreCategoriaSp%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreSp" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombreSp" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;" class="textoBold">
                        <%=Resources.NewResource.txtNombreCategoriaEn%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreEn" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombreEn" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td style="width: 150px;" class="textoBold">
                        <%=Resources.NewResource.txtNombreCategoriaPo%>
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombrePo" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombrePo" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
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
                Visible='<%# DataItem is Telerik.Web.UI.GridInsertionObject %>'></asp:Button>
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                CommandName="Cancel"></asp:Button>
        </td>
    </tr>
</table>
