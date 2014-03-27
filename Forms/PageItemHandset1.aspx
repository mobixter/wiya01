<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageItemHandset1.aspx.cs"
    Inherits="PageItemHandset1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <style type="text/css">
        .MyImageButton
        {
            cursor: hand;
        }
        .EditFormHeader td
        {
            font-size: 14px;
            padding: 4px !important;
            color: #0066cc;
        }
    </style>
    <telerik:RadStyleSheetManager ID="RadStyleSheetManager1" runat="server" />
</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
    </telerik:RadAjaxManager>
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server">
        <Scripts>
        </Scripts>
    </telerik:RadScriptManager>
    <br />
    <telerik:RadGrid ID="RadGrid1" runat="server" AllowAutomaticDeletes="True" 
        AllowAutomaticInserts="True" AutoGenerateColumns="False" CellSpacing="0" 
        DataSourceID="ItemHandsets" GridLines="None">
<MasterTableView DataKeyNames="itemId,handsetId,fileId" DataSourceID="ItemHandsets">
<CommandItemSettings ExportToPdfText="Export to PDF"></CommandItemSettings>

<RowIndicatorColumn Visible="True" FilterControlAltText="Filter RowIndicator column">
<HeaderStyle Width="20px"></HeaderStyle>
</RowIndicatorColumn>

<ExpandCollapseColumn Visible="True" FilterControlAltText="Filter ExpandColumn column">
<HeaderStyle Width="20px"></HeaderStyle>
</ExpandCollapseColumn>

    <Columns>
        <telerik:GridTemplateColumn DataField="itemId" DataType="System.Int64" 
            FilterControlAltText="Filter itemId column" HeaderText="itemId" 
            SortExpression="itemId" UniqueName="itemId" Visible="False">
            <EditItemTemplate>
                <asp:TextBox ID="itemIdTextBox" runat="server" Text='<%# Bind("itemId") %>'></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="itemIdLabel" runat="server" Text='<%# Eval("itemId") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="handsetId" DataType="System.Int32" 
            FilterControlAltText="Filter handsetId column" HeaderText="handsetId" 
            SortExpression="handsetId" UniqueName="handsetId">
            <EditItemTemplate>
                <telerik:RadComboBox ID="RadComboBox1" Runat="server" DataSourceID="Handsets" 
                    DataTextField="handsetName" DataValueField="handsetId">
                </telerik:RadComboBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="handsetIdLabel" runat="server" Text='<%# Eval("handsetId") %>' 
                    Visible="False"></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="fileId" DataType="System.Int64" 
            FilterControlAltText="Filter fileId column" HeaderText="fileId" 
            SortExpression="fileId" UniqueName="fileId">
            <EditItemTemplate>
                <telerik:RadComboBox ID="RadComboBox2" Runat="server" DataSourceID="Files" 
                    DataTextField="fileId" DataValueField="fileName">
                </telerik:RadComboBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="fileIdLabel" runat="server" Text='<%# Eval("fileId") %>' 
                    Visible="False"></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="handsetName" 
            FilterControlAltText="Filter handsetName column" HeaderText="handsetName" 
            SortExpression="handsetName" UniqueName="handsetName">
            <EditItemTemplate>
                <asp:TextBox ID="handsetNameTextBox" runat="server" 
                    Text='<%# Bind("handsetName") %>' Visible="False"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="handsetNameLabel" runat="server" 
                    Text='<%# Eval("handsetName") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
        <telerik:GridTemplateColumn DataField="fileName" 
            FilterControlAltText="Filter fileName column" HeaderText="fileName" 
            SortExpression="fileName" UniqueName="fileName">
            <EditItemTemplate>
                <asp:TextBox ID="fileNameTextBox" runat="server" Text='<%# Bind("fileName") %>' 
                    Visible="False"></asp:TextBox>
            </EditItemTemplate>
            <ItemTemplate>
                <asp:Label ID="fileNameLabel" runat="server" Text='<%# Eval("fileName") %>'></asp:Label>
            </ItemTemplate>
        </telerik:GridTemplateColumn>
    </Columns>

<EditFormSettings>
<EditColumn FilterControlAltText="Filter EditCommandColumn column"></EditColumn>
</EditFormSettings>

<PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>
</MasterTableView>

<PagerStyle PageSizeControlType="RadComboBox"></PagerStyle>

<FilterMenu EnableImageSprites="False"></FilterMenu>
    </telerik:RadGrid>
    <telerik:RadWindowManager ID="RadWindowManager1" runat="server">
    </telerik:RadWindowManager>
    <asp:SqlDataSource DeleteCommand="DELETE FROM ItemHandset WHERE (itemId = @itemId) AND (handsetId = @handsetId) AND (fileId = @fileId)"
        InsertCommand="INSERT INTO ItemHandset (itemId, handsetId, fileId) VALUES (@itemId,@handsetId,@fileId)"
        SelectCommand="SELECT ItemHandset.itemId, ItemHandset.handsetId, ItemHandset.fileId, Handset.handsetName + ' - ' + Handset.handsetStrId AS handsetName, Files.fileName FROM ItemHandset INNER JOIN Handset ON ItemHandset.handsetId = Handset.handsetId INNER JOIN Files ON ItemHandset.itemId = Files.itemId AND ItemHandset.fileId = Files.fileId WHERE (ItemHandset.itemId = @itemId)"
        
        ConnectionString="<%$ ConnectionStrings:DB_ContentProviderConnectionString %>" 
        ID="ItemHandsets" runat="server">
        <DeleteParameters>
            <asp:Parameter Name="itemId" />
            <asp:Parameter Name="handsetId" />
            <asp:Parameter Name="fileId" />
        </DeleteParameters>
        <InsertParameters>
            <asp:Parameter Name="itemId" />
            <asp:Parameter Name="handsetId" />
            <asp:Parameter Name="fileId" />
        </InsertParameters>
        <SelectParameters>
            <asp:QueryStringParameter DefaultValue="64596" Name="itemId" 
                QueryStringField="ItemId" />
        </SelectParameters>
    </asp:SqlDataSource>
    <asp:SqlDataSource ID="Handsets" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DB_ContentProviderConnectionString %>" 
        
        SelectCommand="SELECT handsetId, handsetStrId + '-' + handsetName as handsetName , manufacturerId, handsetDate, handsetCompatible FROM Handset"></asp:SqlDataSource>
    <asp:SqlDataSource ID="Files" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DB_ContentProviderConnectionString %>" 
        SelectCommand="SELECT * FROM [Files] WHERE ([itemId] = @itemId)">
        <SelectParameters>
            <asp:QueryStringParameter Name="itemId" QueryStringField="itemId" 
                Type="Int64" />
        </SelectParameters>
    </asp:SqlDataSource>
    </form>
</body>
</html>
