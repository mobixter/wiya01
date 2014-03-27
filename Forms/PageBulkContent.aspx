<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PageBulkContent.aspx.cs" Inherits="ContentAdmin.PageBulkContent" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="Telerik.Web.UI" Namespace="Telerik.Web.UI" TagPrefix="telerik" %>
<%@ Register TagPrefix="qsf" Namespace="QuickStart" Assembly="ContentAdmin" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <link href="../ccs/qsf.css" rel="stylesheet" type="text/css" />
    <link href="../ccs/Main.css" rel="stylesheet" type="text/css" />
    <telerik:RadCodeBlock ID="RadCodeBlock1" runat="server">
        <script type="text/javascript">
            function pageLoad() {
                logEvent("pageLoad fired", "info");
            }

            function writeLog() {
                logEvent("pageLoad fired...", "info");
            }

            function OnRequestStart(sender, args) {
                logEvent("OnRequestStart (" + args.get_eventTarget() + ") fired", "info");
                var statusLabel = document.getElementById("<%= statusLabel.ClientID %>");
                var sendMessage = document.getElementById("<%= sendContent.ClientID %>");
                var saveDraft = document.getElementById("<%= clearLog.ClientID %>");
                
                if (args.get_eventTarget().indexOf("clearLog") != -1) {
                    if (!window.confirm("Seguro?"))
                        return false; // Cancel ajax request.
                    statusLabel.innerHTML = "Closing...";
                }
                else if (args.get_eventTarget().indexOf("sendContent") != -1)
                    statusLabel.innerHTML = "Sending content...";
                else
                    statusLabel.innerHTML = "Cleaning log...";

                sendMessage.disabled = true;
                saveDraft.disabled = true;
            }

            function OnResponseEnd(sender, args) {
                logEvent("OnResponseEnd fired", "info");
                var sendMessage = document.getElementById("<%= sendContent.ClientID %>");
                var saveDraft = document.getElementById("<%= clearLog.ClientID %>");
                
                saveDraft.disabled = false;
                sendMessage.disabled = false;
                
                <%= postBackStr %>;
            }

            function __doPostBack(eventTarget, eventArgument) {
                if (!theForm.onsubmit || (theForm.onsubmit() != false)) {
                    theForm.__EVENTTARGET.value = eventTarget;
                    theForm.__EVENTARGUMENT.value = eventArgument;
                    theForm.submit();
                }
            }
        </script>
    </telerik:RadCodeBlock>
    <script type="text/javascript">
        function fnShowMessage() {
            alert(" Invoke Javascript function from Server Side Code Behind ");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <telerik:RadScriptManager ID="RadScriptManager1" runat="server" />
    <telerik:RadAjaxManager ID="RadAjaxManager1" runat="server">
        <ClientEvents OnResponseEnd="OnResponseEnd" OnRequestStart="OnRequestStart"></ClientEvents>
        <AjaxSettings>
            <telerik:AjaxSetting AjaxControlID="clearLog">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="statusLabel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="sendContent">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="statusLabel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
            <telerik:AjaxSetting AjaxControlID="restoreDraft">
                <UpdatedControls>
                    <telerik:AjaxUpdatedControl ControlID="statusLabel"></telerik:AjaxUpdatedControl>
                </UpdatedControls>
            </telerik:AjaxSetting>
        </AjaxSettings>
    </telerik:RadAjaxManager>
    
    <div style="float:left">
        <telerik:RadAsyncUpload runat="server" ID="rdUploadFile" UploadedFilesRendering="BelowFileInput"
            TargetFolder="uploads"
            AllowedFileExtensions=".zip" 
            MaxFileInputsCount="1" oninit="rdUploadFile_Init">
            <Localization Select="Upload File" />
        </telerik:RadAsyncUpload>    
    </div>
    <div>
        <asp:CheckBox CssClass="texto" ID="checkBox" runat="server" 
            Text="Update content (Only Data)" />
    &nbsp;&nbsp;&nbsp;<asp:CheckBox ID="chReplace" runat="server" CssClass="texto" Text="Replace" 
            />&nbsp;&nbsp;&nbsp;
        <asp:Label ID="lblError" runat="server" CssClass="texto"></asp:Label>
    </div>

    <qsf:EventLogConsole ID="EventLogConsole1" runat="server" AllowClear="false" />
    <%--<telerik:RadComboBox ID="rcb" runat="server" />--%>
    
    <div style="height: 25px; padding-top: 280px;
        text-align: center;">
        <asp:Button Text="Send Content" OnClick="sendContent_Click" runat="server" ID="sendContent"
            Style="width: 116px;"></asp:Button>
        <asp:Button Text="Clear Log" OnClick="clearLog_Click" runat="server" ID="clearLog"
            Style="width: 116px;"></asp:Button>
        <asp:Button Text="Download Log" OnClick="downloadLog_Click" runat="server" ID="downloadLog"
            Style="width: 116px;"></asp:Button>
        <br />
        <br />
        <asp:Label ID="statusLabel" runat="server" Style="color: #698901; margin-top: 13px;
            display: block;"></asp:Label>
    </div>

    </form>
</body>
</html>

