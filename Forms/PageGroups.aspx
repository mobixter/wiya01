<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageGroups.aspx.cs" Inherits="ContentAdmin.Forms.PageGroups" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="../Scripts/jquery-1.4.1.js"></script>
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager2" runat="server">
    </telerik:RadScriptManager>
    <div>
    <div style="width:600px;margin:auto">
    <h1>Add Handset to the Group</h1>
    <p style="color:Red" id="error" runat="server"></p>
        <telerik:RadGrid ID="RadGrid1" runat="server" OnNeedDataSource="RadGrid1_NeedDataSource"
        OnInsertCommand="RadGrid1_InsertCommand" AllowFilteringByColumn="True" 
            AllowPaging="True" AllowSorting="True" CellSpacing="0" GridLines="None" 
            AutoGenerateColumns="False" >
        <MasterTableView TableLayout="Fixed" DataKeyNames="Id" EditMode="PopUp" CommandItemDisplay="Top" >
                    <EditFormSettings UserControlName="../Controls/Forms/ctrCompatibles.ascx" EditFormType="WebUserControl">
                        <EditColumn UniqueName="EditCommandColumn1">
                        </EditColumn>
                    </EditFormSettings>
                    <CommandItemSettings ShowExportToExcelButton="true" />

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column"></RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column"></ExpandCollapseColumn>

                    <Columns>
                        <telerik:GridBoundColumn AllowFiltering="False" AllowSorting="False" 
                            DataField="IdHandset" Display="False" 
                            FilterControlAltText="Filter column column" HeaderText="Handset" 
                            UniqueName="column">
                            <HeaderStyle Width="100px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="Id" 
                            FilterControlAltText="Filter column2 column" HeaderText="Group" 
                            UniqueName="column2">
                            <HeaderStyle Width="100px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridBoundColumn DataField="NameHandset" 
                            FilterControlAltText="Filter column3 column" HeaderText="Handset Name" 
                            UniqueName="column3">
                            <HeaderStyle Width="300px" />
                        </telerik:GridBoundColumn>
                        <telerik:GridHyperLinkColumn AllowFiltering="False" AllowSorting="False" 
                            DataNavigateUrlFields="IdHandset,Id" 
                            DataNavigateUrlFormatString="elimhandset.aspx?id={0}&amp;group={1}" 
                            DataTextField="deleteCmd" FilterControlAltText="Filter column1 column" 
                            UniqueName="column1">
                             <HeaderStyle Width="100px" />
                        </telerik:GridHyperLinkColumn>
                    </Columns>

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
    <script>
        $(function () {


            $("#RadGrid1_ctl00 tbody tr").each(function (index) {

                var id = 0;
                var group;
                $(this).children("td").each(function (index) {

                    switch (index) {

                        case 0:
                            id = $(this).html();
                            break;
                        case 1:
                            group = $(this).html();
                            break;

                    }


                })
            })




        })


    </script>
</body>
</html>
