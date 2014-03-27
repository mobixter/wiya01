<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageDevices.aspx.cs" Inherits="ContentAdmin.PageDevices" Trace="false"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript" src="../Scripts/jquery-1.4.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
    </telerik:RadScriptManager>
    <div>
    <div align="left" style="width:30%;margin:auto">
    <h1>GROUPS</h1>
    <p runat="server" id="text12" style="color:Green"></p>
    <p runat="server" id="text11" style="color:Red"></p>
    
        <telerik:RadGrid ID="RadGrid1"  GridLines="None" AllowPaging="True" PageSize="20" 
        OnNeedDataSource="RadGrid1_NeedDataSource" runat="server" 
            AllowFilteringByColumn="True" AllowSorting="True" CellSpacing="0"
            OnInsertCommand="RadGrid1_InsertCommand" AutoGenerateColumns="False">
             <MasterTableView TableLayout="Fixed" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top" >
                    <CommandItemSettings ShowExportToExcelButton="true" />

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>

                    <Columns>
                        <telerik:GridBoundColumn DataField="Id" 
                            FilterControlAltText="Filter column column" HeaderText="ID" UniqueName="column">
                            <HeaderStyle Width="100px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Groups" 
                            FilterControlAltText="Filter column1 column" HeaderText="Group" 
                            UniqueName="column1">
                            <HeaderStyle Width="200px" /> 
                        </telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn AllowSorting="False" DataNavigateUrlFields="Id" 
                            DataNavigateUrlFormatString="PageGroups.aspx?IdH={0}" 
                            FilterControlAltText="Filter column2 column" UniqueName="column2" 
                            DataTextField="editCmd" AllowFiltering="False">
                        </telerik:GridHyperLinkColumn>
                    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>

<PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
                    <CommandItemTemplate>
                        <asp:LinkButton ID="btnInsert" runat="server" CommandName="InitInsert" Visible='true'>
                        <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 5px;" alt="" src="../Images/AddRecord.gif" /><%=Resources.NewResource.txtAgregar%></asp:LinkButton>
                        <asp:LinkButton ID="btnActualizar" runat="server" CommandName="RebindGrid">
                        <img style="border:0px;vertical-align:middle; padding: 5px 5px 5px 0;" alt=""  src="../Images/Refresh.gif" /><%=Resources.NewResource.txtActualizar%></asp:LinkButton>
                    </CommandItemTemplate>
                </MasterTableView>
            <ClientSettings>           
                 <Resizing AllowColumnResize="true" ResizeGridOnColumnResize="true" AllowResizeToFit="true" />            
            </ClientSettings>
<PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>

<FilterMenu EnableImageSprites="False"></FilterMenu>
        </telerik:RadGrid>
    </div>
    </div>
    </form>

</body>
</html>
