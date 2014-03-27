<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageComercializacion.aspx.cs" CodeFile ="PageComercializacion.aspx.cs"
    Inherits="ContentAdmin.PageComercializacion" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="custom" Namespace="ContentAdmin" Assembly="ContentAdmin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form runat="server" id="mainForm" method="post">
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server">
    </telerik:RadStyleSheetManager>
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

            function onRowDropping(sender, args) {
                if (sender.get_id() == "<%=RadGrid1.ClientID %>") {
                    var node = args.get_destinationHtmlElement();
                    if (!isChildOf('<%=RadGrid2.ClientID %>', node) && !isChildOf('<%=RadGrid1.ClientID %>', node)) {
                        args.set_cancel(true);
                    }
                }
                else {
                    if (sender.get_id() == "<%=RadGrid2.ClientID %>") {
                        var node = args.get_destinationHtmlElement();
                        if (!isChildOf('<%=RadGrid1.ClientID %>', node) && !isChildOf('<%=RadGrid2.ClientID %>', node)) {
                            args.set_cancel(true);
                        }
                    }
                }
            }

            function isChildOf(parentId, element) {
                while (element) {
                    if (element.id && element.id.indexOf(parentId) > -1) {
                        return true;
                    }
                    element = element.parentNode;
                }
                return false;
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
                popUp.style.left = ((gridWidth - popUpWidth) / 3 + sender.get_element().offsetLeft).toString() + "px";
                popUp.style.top = ((gridHeight - popUpHeight) / 6 + sender.get_element().offsetTop).toString() + "px";
            } 
        </script>
    </telerik:RadCodeBlock>
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="RadGrid1">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="msg" />
                    <telerik:AjaxUpdatedControl ControlID="RadTreeViewResumen" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="RadGrid2">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="msg" />
                    <telerik:AjaxUpdatedControl ControlID="RadTreeViewResumen" LoadingPanelID="RadAjaxLoadingPanel" />
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="btnZip">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="RadGrid2" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="RadGrid1" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="msg" />
                    <telerik:AjaxUpdatedControl ControlID="RadTreeViewEntidad" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="RadTreeViewResumen" LoadingPanelID="RadAjaxLoadingPanel" />
                    <telerik:AjaxUpdatedControl ControlID="btnNuevo" />
                    <telerik:AjaxUpdatedControl ControlID="lblNombreArchivo" />
                    <telerik:AjaxUpdatedControl ControlID="txtFile" />
                    <telerik:AjaxUpdatedControl ControlID="RadFileExplorerZip" />
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    <telerik:RadAjaxLoadingPanel ID="RadAjaxLoadingPanel" BackColor="White" runat="server"
        Transparency="40">
        <img alt="Loading..." src="../Images/wait.gif" style="border: 0; vertical-align: middle;
            margin-top: 200px" />
    </telerik:RadAjaxLoadingPanel>
    <div align="left">
        <div class="exWrap">
            <div align="left" style="padding: 10px 6px 10px 10px">
                <table cellpadding="2" cellspacing="2" width="98%" border="0">
                    <tr>
                        <td style="width: 100px;" class="textoBold">
                            Plataforma
                        </td>
                        <td style="width: 250px;">
                            <custom:ListControl ID="ctrPlataforma" runat="server" />
                        </td>
                        <td>
                            <asp:LinkButton ID="lnkButton" runat="server" CssClass="texto" 
                                >Download file zip</asp:LinkButton>
                            
                            <asp:Label ID="lblPath" runat="server" Visible="False"></asp:Label>
                            <asp:Label ID="lblName" runat="server" Visible="False"></asp:Label>
                        </td>
                        <td style="width: 60px;" align="right">
                            <asp:Button ID="Button1" runat="server" CssClass="boton" onclick="Button1_Click" Text="Generar Zip" />                           
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <div style="width: 100%" align="center">
            <div style="float: left; padding: 15px 6px 0 0px;">
                            <style>
                                .RadComboBox .rcbArrowCell a 
                                {
                                    height: 23px;
                                }
                            </style>
                <telerik:RadGrid ID="RadGrid1" Width="651px" ShowStatusBar="True" AllowSorting="True"
                    PageSize="20" GridLines="None" AllowPaging="True" runat="server" AllowAutomaticUpdates="True"
                    AutoGenerateColumns="False" OnNeedDataSource="RadGrid1_NeedDataSource" OnRowDrop="RadGrid1_RowDrop" 
                    AllowFilteringByColumn="True" AllowMultiRowSelection="True" 
                    CellSpacing="0" OnItemCommand="RadGrid1_ItemCommand" >
                    <MasterTableView TableLayout="Fixed" DataKeyNames="itemId" CommandItemDisplay="Top">
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <CommandItemTemplate>
                            <asp:LinkButton ID="btnActualizar" runat="server" CommandName="RebindGrid">
                                <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 5px;" alt=""  src="../Images/Refresh.gif" /><%=Resources.NewResource.txtActualizar%></asp:LinkButton>
                        </CommandItemTemplate>
                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                        
                            <telerik:GridDragDropColumn HeaderStyle-Width="18px" Visible="True">
                                <HeaderStyle Width="18px"></HeaderStyle>
                            </telerik:GridDragDropColumn>
                            
                            <telerik:GridBoundColumn UniqueName="itemId" DataField="itemId" HeaderText="Id" ReadOnly="True"
                                HeaderStyle-Width="5%" FilterControlWidth="10px">
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="strNombre_Item" DataField="itemNameSp" HeaderText="Nombre"
                                HeaderStyle-Width="100px" FilterControlWidth="50px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="providerId" DataField="providerId" HeaderText="providerId"
                                HeaderStyle-Width="100px" Visible="false">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridBoundColumn >

                             <custom:FilteringColumn DataField="providerName" FilterControlWidth="40px" HeaderText="Provider"
                                Width="80">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%# Eval("providerName")%>
                                </ItemTemplate>
                            </custom:FilteringColumn>
                           
                            <telerik:GridBoundColumn UniqueName="categoryId" DataField="categoryId" HeaderText="categoryId"
                                HeaderStyle-Width="0%" Visible="false">
                                <HeaderStyle Width="0%"></HeaderStyle>
                                
                            </telerik:GridBoundColumn>
                            <custom:FilteringColumn DataField="categoryNameSp" SortExpression ="categoryNameSp" ShowSortIcon ="true"  FilterControlWidth="150px" HeaderText="Category"
                                Width="80" >
                                <HeaderStyle Width="100px" />                           
                                  
                                  
                                <ItemTemplate>
                                    <%# Eval("categoryNameSp")%>
                                </ItemTemplate>
                                
                            </custom:FilteringColumn>

                            
                            
                            <telerik:GridBoundColumn UniqueName="contentTypeId" DataField="contentTypeId" HeaderText="contentTypeId"
                                HeaderStyle-Width="0%" Visible="false">
                                <HeaderStyle Width="0%"></HeaderStyle>
                            </telerik:GridBoundColumn>

                            <custom:FilteringColumn DataField="contentTypeName" SortExpression ="contentTypeName"  FilterControlWidth="40px" Height="210"
                                HeaderText="Content Type" Width="80">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%# Eval("contentTypeName")%>
                                </ItemTemplate>
                            </custom:FilteringColumn>                       
       
                            <custom:FilteringColumn DataField="NoExport" SortExpression ="NoExport"  FilterControlWidth="40px" Height="210"
                                HeaderText="Enabled" Width="80">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                              <asp:CheckBox ID="CheckBox1"  runat="server" AutoPostBack="True" OnCheckedChanged="CheckChanged"  Checked='<%# Bind("NoExport") %>' >
                              </asp:CheckBox>
                            </ItemTemplate>
                            </custom:FilteringColumn>

                            <telerik:GridTemplateColumn HeaderStyle-Width="15%" AllowFiltering="false"  HeaderText="Preview">
                                <ItemTemplate>
                                    <img style="border: 1px"  alt='<%#Eval("itemNameSp") %>' width="60px" height="60px"
                                        src='../File/Preview/<%#Eval("previewName") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings AllowRowsDragDrop="True">
                        <ClientEvents OnRowClick="RowClick" OnGridCreated="GridCreated" OnCommand="GridCommand" />
                        <Selecting AllowRowSelect="True" EnableDragToSelectRows="True" />
                        <ClientEvents OnRowDropping="onRowDropping" />
                    </ClientSettings>
                    <PagerStyle PageButtonCount="5" />
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                    </HeaderContextMenu>
                </telerik:RadGrid>
            </div>
            <div style="float: left; padding: 15px 10px 0 25px;">
                <telerik:RadGrid ID="RadGrid2" Width="480px" ShowStatusBar="True" AllowSorting="True"
                    PageSize="20" GridLines="None" AllowPaging="True" runat="server" AllowAutomaticUpdates="True"
                    AutoGenerateColumns="False" OnNeedDataSource="RadGrid2_NeedDataSource" OnRowDrop="RadGrid2_RowDrop"
                    AllowFilteringByColumn="True" AllowMultiRowSelection="True" 
                    CellSpacing="0" OnItemCommand="RadGrid2_ItemCommand">
                    <MasterTableView TableLayout="Fixed" DataKeyNames="itemId" CommandItemDisplay="Top">
                        <EditFormSettings>
                            <EditColumn FilterControlAltText="Filter EditCommandColumn column">
                            </EditColumn>
                        </EditFormSettings>
                        <CommandItemTemplate>
                            <asp:LinkButton ID="btnActualizar" runat="server" CommandName="RebindGrid">
                                <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 5px;" alt=""  src="../Images/Refresh.gif" /><%=Resources.NewResource.txtActualizar%></asp:LinkButton>
                        </CommandItemTemplate>
                        <CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>
                        <RowIndicatorColumn FilterControlAltText="Filter RowIndicator column">
                        </RowIndicatorColumn>
                        <ExpandCollapseColumn FilterControlAltText="Filter ExpandColumn column">
                        </ExpandCollapseColumn>
                        <Columns>
                            <telerik:GridDragDropColumn HeaderStyle-Width="18px" Visible="True">
                                <HeaderStyle Width="18px"></HeaderStyle>
                            </telerik:GridDragDropColumn>
                            <telerik:GridBoundColumn UniqueName="itemId" DataField="itemId" HeaderText="Id" ReadOnly="True"
                                HeaderStyle-Width="5%" FilterControlWidth="15px">
                                <HeaderStyle Width="50px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="strNombre_Item" DataField="itemNameSp" HeaderText="Nombre"
                                HeaderStyle-Width="100px" FilterControlWidth="60px">
                                <HeaderStyle Width="100px"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <telerik:GridBoundColumn UniqueName="categoryId" DataField="categoryId" HeaderText="categoryId"
                                HeaderStyle-Width="0%" Visible="false">
                                <HeaderStyle Width="0%"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <custom:FilteringColumn DataField="categoryNameSp" SortExpression="categoryNameSp" FilterControlWidth="150px" HeaderText="Category"
                                Width="100">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%# Eval("categoryNameSp")%>
                                </ItemTemplate>
                            </custom:FilteringColumn>
                            <telerik:GridBoundColumn UniqueName="contentTypeId" DataField="contentTypeId" HeaderText="contentTypeId"
                                HeaderStyle-Width="0%" Visible="false">
                                <HeaderStyle Width="0%"></HeaderStyle>
                            </telerik:GridBoundColumn>
                            <custom:FilteringColumn DataField="contentTypeName" SortExpression ="contentTypeName"  FilterControlWidth="150px" Height="210"
                                HeaderText="Content Type" Width="100">
                                <HeaderStyle Width="100px" />
                                <ItemTemplate>
                                    <%# Eval("contentTypeName")%>
                                </ItemTemplate>
                            </custom:FilteringColumn>
                            <telerik:GridTemplateColumn HeaderStyle-Width="15%" AllowFiltering="false" HeaderText="Preview">
                                <ItemTemplate>
                                    <img style="border: 1px" alt='<%#Eval("itemNameSp") %>' width="60px" height="60px"
                                        src='../File/Preview/<%#Eval("previewName") %>' />
                                </ItemTemplate>
                                <HeaderStyle Width="15%"></HeaderStyle>
                            </telerik:GridTemplateColumn>
                        </Columns>
                    </MasterTableView>
                    <ClientSettings AllowRowsDragDrop="True">
                        <ClientEvents OnRowClick="RowClick" OnGridCreated="GridCreated" OnCommand="GridCommand" />
                        <Selecting AllowRowSelect="True" EnableDragToSelectRows="True" />
                        <ClientEvents OnRowDropping="onRowDropping" />
                    </ClientSettings>
                    <PagerStyle PageButtonCount="5" />
                    <FilterMenu EnableImageSprites="False">
                    </FilterMenu>
                    <HeaderContextMenu CssClass="GridContextMenu GridContextMenu_Default">
                    </HeaderContextMenu>
                </telerik:RadGrid>
            </div>
        </div>
        <%--<%=Resources.NewResource.txtActualizar%>--%>
    <div class="exMessage" runat="server" id="msg" visible="true" enableviewstate="false">
    </div>
    </div>
    </form>
</body>
</html>
