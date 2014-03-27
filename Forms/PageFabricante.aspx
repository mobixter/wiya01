<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageFabricante.aspx.cs" Inherits="ContentAdmin.PageFabricante" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server" id="mainForm" method="post">
    <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
    </telerik:RadScriptManager>
    <!-- content start -->
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
        <!--
            var hasChanges, inputs, dropdowns, editedRow;

            function RowClick(sender, eventArgs) {
                if (editedRow && hasChanges) {
                    hasChanges = false;
                    if (confirm("Update changes?")) {

                        $find("<%= RadGrid1.MasterTableView.ClientID %>").updateItem(editedRow);
                    }
                }
            }

            function RowDblClick(sender, eventArgs) {
                editedRow = eventArgs.get_itemIndexHierarchical();
                $find("<%= RadGrid1.MasterTableView.ClientID %>").editItem(editedRow);
            }

            function GridCommand(sender, args) {
                if (args.get_commandName() != "Edit") {
                    editedRow = null;
                }
            }

            function GridCreated(sender, eventArgs) {
                var gridElement = sender.get_element();
                var elementsToUse = [];
                inputs = gridElement.getElementsByTagName("input");
                for (var i = 0; i < inputs.length; i++) {
                    var lowerType = inputs[i].type.toLowerCase();
                    if (lowerType == "hidden" || lowerType == "button") {
                        continue;
                    }

                    Array.add(elementsToUse, inputs[i]);
                    inputs[i].onchange = TrackChanges;
                }

                dropdowns = gridElement.getElementsByTagName("select");
                for (var i = 0; i < dropdowns.length; i++) {
                    dropdowns[i].onchange = TrackChanges;
                }

                setTimeout(function () { if (elementsToUse[0]) elementsToUse[0].focus(); }, 100);
            }

            function TrackChanges(e) {
                hasChanges = true;
            }
     -->    
        </script>
        <script type="text/javascript">
            var popUp;
            function PopUpShowing(sender, eventArgs) {
                popUp = eventArgs.get_popUp();
                var gridWidth = sender.get_element().offsetWidth;
                var gridHeight = sender.get_element().offsetHeight;
                var popUpWidth = popUp.style.width.substr(0, popUp.style.width.indexOf("px"));
                var popUpHeight = popUp.style.height.substr(0, popUp.style.height.indexOf("px"));
                popUp.style.left = ((gridWidth - popUpWidth) / 2 + sender.get_element().offsetLeft).toString() + "px";
                popUp.style.top = "200px";
            } 
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="Label1" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" BackColor="White" runat="server"
        Transparency="40">
        <img alt="Loading..." src="../Images/wait.gif" style="border: 0; vertical-align: middle;
            margin-top: 250px" />
    </telerik:RadAjaxLoadingPanel>
    <div id="botonExportar">
        <asp:LinkButton ID="lnkExportar" runat="server" onclick="lnkExportar_Click">Exportar a excel</asp:LinkButton>
    </div>
    <div align="center">
        <div  align="left" style="width: 60%">
            <telerik:RadGrid ID="RadGrid1" ShowStatusBar="True" AllowSorting="True"
                PageSize="20" GridLines="None" AllowPaging="True" runat="server" AllowAutomaticUpdates="True"
                AutoGenerateColumns="False" OnNeedDataSource="RadGrid1_NeedDataSource" AllowFilteringByColumn="True"
                CellSpacing="0" OnItemCommand="RadGrid1_ItemCommand">
                <ExportSettings HideStructureColumns="true" />
                <MasterTableView TableLayout="Fixed" DataKeyNames="manufacturerId" EditMode="PopUp" CommandItemDisplay="None" >
                    <CommandItemSettings ShowExportToExcelButton="true" />
                    <EditFormSettings PopUpSettings-Modal="true" PopUpSettings-Width="800px" />
                    <CommandItemTemplate>
                        <asp:LinkButton ID="btnInsert" runat="server" CommandName="InitInsert" Visible='False'>
                        <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 5px;" alt="" src="../Images/AddRecord.gif" /><%=Resources.NewResource.txtAgregar%></asp:LinkButton>
                        <asp:LinkButton ID="btnActualizar" runat="server" CommandName="RebindGrid">
                        <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 0;" alt=""  src="../Images/Refresh.gif" /><%=Resources.NewResource.txtActualizar%></asp:LinkButton>
                    </CommandItemTemplate>
                    <Columns>
                        <telerik:GridBoundColumn UniqueName="manufacturerId" DataField="manufacturerId" HeaderText="Id"
                            ReadOnly="True" HeaderStyle-Width="10%" FilterControlWidth="40px" DataType="System.Int64">
                            <HeaderStyle Width="10%"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="manufacturerName" DataField="manufacturerName" HeaderText="Name"
                            FilterControlWidth="180px" DataType="System.String">
                        </telerik:GridBoundColumn>
                    </Columns>
                </MasterTableView>
                <ClientSettings>
                    <ClientEvents OnRowClick="RowClick" OnRowDblClick="RowDblClick" OnGridCreated="GridCreated"
                        OnCommand="GridCommand" />
                    <ClientEvents OnPopUpShowing="PopUpShowing" />
                </ClientSettings>
                <FilterMenu EnableImageSprites="False">
                </FilterMenu>
                <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                </HeaderContextMenu>
            </telerik:RadGrid>
            <br />
            <asp:Label ID="Label1" runat="server" EnableViewState="false" />
            <br />
        </div>
    </div>
    </form>
</body>
</html>
