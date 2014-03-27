<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrGroups.ascx.cs" Inherits="ContentAdmin.Controls.Forms.ctrGroups" %>
<link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
<table  cellspacing="2" cellpadding="1" width="100%" border="1" rules="none"
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
                        Name Group
                    </td>
                    <td>
                        <asp:TextBox ID="txtNombreSp" runat="server" CssClass="texto" Text="" Width="215px"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" CssClass="LabelRequerido"
                            ControlToValidate="txtNombreSp" ErrorMessage='Obligatorio'></asp:RequiredFieldValidator>
                  </td>
                  </tr>
            </table>
        </td>
    </tr>
    <tr>
        <td align="right" colspan="2">
            <asp:Button ID="btnInsert" Text="Insert" runat="server" CommandName="PerformInsert"
                Visible='true'></asp:Button>
            <asp:Button ID="btnCancel" Text="Cancel" runat="server" CausesValidation="False"
                CommandName="Cancel"></asp:Button>
        </td>
    </tr>
</table>
<style>
.rgHeader
{
    display:none;
    }
</style>
