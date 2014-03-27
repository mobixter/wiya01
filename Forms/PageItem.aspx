<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageItem.aspx.cs" CodeFile ="PageItem.aspx.cs" Inherits="ContentAdmin.PageItem"
    EnableEventValidation="false" %>

<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="custom" Namespace="ContentAdmin" Assembly="ContentAdmin" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        Telerik.Web.UI.RadAsyncUpload.Modules.Flash.isAvailable = function () { return false; }
    </script>
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
                popUp.style.top = "59px";
            } 
        </script>
        <script language="javascript" type="text/javascript">

            function objTodos(objTodoId, objAsociadoId) {
                var objList;
                for (var i = 0; i < document.forms[0].elements.length; i++) {
                    var obj = document.forms[0].elements[i];
                    if (obj.id.indexOf(objTodoId) != -1) {
                        objList = obj.id;
                    }
                }
                return objList;
            }
            function objAsociados(objAsociadoId) {
                var objList;
                for (var i = 0; i < document.forms[0].elements.length; i++) {
                    var obj = document.forms[0].elements[i];
                    if (obj.id.indexOf(objAsociadoId) != -1) {
                        objList = obj.id;
                    }
                }
                return objList;
            }

            function Incluir(objTodoId, objAsociadoId) {
                var i = 0;
                var objListTodos = objTodos(objTodoId);
                var objListAsociados = objAsociados(objAsociadoId);

                for (i = 0; i < document.getElementById(objListTodos).length; i++) {
                    if (document.getElementById(objListTodos)[i].selected == true) {
                        document.getElementById(objListAsociados).options[document.getElementById(objListAsociados).length] =
				        new Option(document.getElementById(objListTodos)[i].text, document.getElementById(objListTodos)[i].value);
                        document.getElementById(objListTodos).options[i].className = 'textoseleccionado';
                    }
                }
            }

            function Regresar(objTodoId, objAsociadoId) {
                var i, id;
                var objListAsociados = objAsociados(objAsociadoId);
                for (i = document.getElementById(objListAsociados).length - 1; i >= 0; i--) {
                    id = '';
                    if (document.getElementById(objListAsociados)[i].selected == true) {
                        id = document.getElementById(objListAsociados).options[i].value;
                        document.getElementById(objListAsociados).options[i] = null;
                    }
                    if (id != '') LimpiarColor(objListAsociados, id, objTodoId);
                }
            }

            function MarcarTodos(objAsociadoId) {
                var objListAsociados = objAsociados(objAsociadoId);
                var i = 0;
                for (i = 0; i < document.getElementById(objListAsociados).length; i++) {
                    if (document.getElementById(objListAsociados)[i].value != "") {
                        document.getElementById(objListAsociados)[i].selected = true;
                    }
                }
            }

            function LimpiarColor(obj, id, objTodoId) {
                var obj1;
                var i = 0;
                var existe = false;
                for (i = 0; i < document.getElementById(obj).length; i++) {
                    if (id == document.getElementById(obj).options[i].value) existe = true;
                }
                var objListTodos = objTodos(objTodoId);
                if (existe == false) {
                    i = 0;
                    for (i = 0; i < document.getElementById(objListTodos).length; i++) {
                        if (id == document.getElementById(objListTodos).options[i].value) {
                            document.getElementById(objListTodos).options[i].className = 'texto';
                        }
                    }
                }
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
        <asp:LinkButton ID="lnkExportar" runat="server" OnClick="lnkExportar_Click">Exportar a excel</asp:LinkButton>

    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Button ID="btDelete" runat="server" CssClass="texto" 
             Text="Delete" onclick="btDelete_Click" />

    </div>

    <div align="center">
        <div align="left" style="width: 90%">
            <telerik:RadGrid ID="RadGrid1" ShowStatusBar="True" AllowSorting="True" PageSize="10"
                GridLines="None" AllowPaging="True" runat="server" AllowAutomaticUpdates="True"
                AutoGenerateColumns="False" OnNeedDataSource="RadGrid1_NeedDataSource" AllowFilteringByColumn="True"
                CellSpacing="0" AutoGenerateEditColumn="False" OnUpdateCommand="RadGrid1_UpdateCommand"
                OnInsertCommand="RadGrid1_InsertCommand" 
                OnItemCommand="RadGrid1_ItemCommand">
                <ExportSettings HideStructureColumns="true" />
                <MasterTableView TableLayout="Fixed" DataKeyNames="itemId" EditMode="PopUp" CommandItemDisplay="Top">
                    <CommandItemSettings ShowExportToExcelButton="true" />
                    <EditFormSettings PopUpSettings-Modal="true" PopUpSettings-Width="1325px" PopUpSettings-Height="565px" />
                    <CommandItemTemplate>
                        <asp:LinkButton ID="btnInsert" runat="server" CommandName="InitInsert" Visible='False'>
                        <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 5px;" alt="" src="../Images/AddRecord.gif" /><%=Resources.NewResource.txtAgregar%></asp:LinkButton>
                        <asp:LinkButton ID="btnActualizar" runat="server" CommandName="RebindGrid">
                        <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 0;" alt=""  src="../Images/Refresh.gif" /><%=Resources.NewResource.txtActualizar%></asp:LinkButton>

                    </CommandItemTemplate>
                    <Columns>
                        
                          <telerik:GridTemplateColumn UniqueName="TemplateColumn" HeaderText="" HeaderStyle-Width="35px" AllowFiltering="false">
                            <ItemTemplate>
                              <asp:CheckBox ID="CheckBox1" runat="server" AutoPostBack="True"  OnCheckedChanged="CheckChanged" >
                              </asp:CheckBox>
                            </ItemTemplate>
                          </telerik:GridTemplateColumn>


                        <telerik:GridBoundColumn UniqueName="itemId" DataField="itemId" HeaderText="Id" ReadOnly="True"
                            HeaderStyle-Width="5%" FilterControlWidth="40px" DataType="System.Int64">
                            <HeaderStyle Width="8%"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="itemStrId" DataField="itemStrId" HeaderText="StrId"
                            FilterControlWidth="100px" DataType="System.String">
                            <HeaderStyle Width="13%"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn UniqueName="itemNameSp" DataField="itemNameSp" HeaderText="Name"
                            FilterControlWidth="150px" DataType="System.String">
                            <HeaderStyle Width="20%"></HeaderStyle>
                        </telerik:GridBoundColumn>
                        <custom:FilteringColumn DataField="providerName" FilterControlWidth="180px" Height="0"
                            HeaderText="Proveedor">
                            <HeaderStyle Width="17%"></HeaderStyle>
                            <ItemTemplate>
                                <%# Eval("providerName")%>
                            </ItemTemplate>
                        </custom:FilteringColumn>
                        <custom:FilteringColumn DataField="contentTypeName" FilterControlWidth="180px" Height="0"
                            HeaderText="ContentType">
                            <HeaderStyle Width="17%"></HeaderStyle>
                            <ItemTemplate>
                                <%# Eval("contentTypeName")%>
                            </ItemTemplate>
                        </custom:FilteringColumn>
                        <custom:FilteringColumn DataField="artistName" FilterControlWidth="180px" Height="0"
                            HeaderText="Artis">
                            <HeaderStyle Width="10%"></HeaderStyle>
                            <ItemTemplate>
                                <%# Eval("artistName")%>
                            </ItemTemplate>
                        </custom:FilteringColumn>
                        <telerik:GridHyperLinkColumn AllowSorting="False" 
                            DataNavigateUrlFields="itemId" 
                            DataNavigateUrlFormatString="PageItemHandset1.aspx?ItemId={0}" 
                            DataTextField="&quot;Files&quot;" DataTextFormatString="&quot;Formats&quot;" 
                            FilterControlAltText="Filter column column" Text="Files" UniqueName="column">
                            <HeaderStyle Width="7%"></HeaderStyle>
                        </telerik:GridHyperLinkColumn>
                        <telerik:GridTemplateColumn HeaderStyle-Width="80px" AllowFiltering="false" HeaderText="Preview">
                            <ItemTemplate>
                                <img style="border: 1px" alt='<%#Eval("itemNameSp") %>' width="60px" height="60px"
                                    src='../File/Preview/<%#Eval("previewName") %>' />
                            </ItemTemplate>
                        </telerik:GridTemplateColumn>

                    </Columns>
                    <EditFormSettings UserControlName="../Controls/Forms/ctrItem.ascx" EditFormType="WebUserControl">
                        <EditColumn UniqueName="EditCommandColumn1">
                        </EditColumn>
                    </EditFormSettings>
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
