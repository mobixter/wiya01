<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ctrCompatibles.ascx.cs" Inherits="ContentAdmin.Controls.Forms.ctrCompatibles" %>
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
                        Handset
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                        <div style="visibility:hidden">
                        <asp:TextBox runat="server" ID="textid"></asp:TextBox>
                        </div>
                        <script src="../../Scripts/jquery-1.4.1.js">
                        </script>
                        <script>
                            $(function () {

                                $("#RadGrid1_ctl00_ctl02_ctl04_EditFormControl_DropDownList1").change(function () {
                                    var drop = $(this).val()
                                    $("#RadGrid1_ctl00_ctl02_ctl04_EditFormControl_textid").val(drop)
                                
                                })

                                var drop = $("#RadGrid1_ctl00_ctl02_ctl04_EditFormControl_DropDownList1").val()
                                $("#RadGrid1_ctl00_ctl02_ctl04_EditFormControl_textid").val(drop)


                            })
                        </script>
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