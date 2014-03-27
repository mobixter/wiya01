<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Main.aspx.cs" Inherits="ContentAdmin.Main" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<link href="ccs/Main.css" rel="stylesheet" type="text/css" />
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>
        <%=Resources.Resource.txtAplicacion%></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div class="texto">
            <p class="textoBold">
                GENERAL RULES</p>
            <p>
                - All content in the submission must have the same content type. (i.e., Ringtone
                or Wallpaper).<br />
                - You must submit a ZIP file containing an XML file with the item meta-data and
                all the content and preview files.
                <br />
                - XML file MUST be encoded in UTF-8 to properly upload.<br />
                - The following characters are invalid in the Display Name * / ? &lt; &gt; ! \ &quot;
                :<br />
                - The XML file must follow the the XML schemas defined for Android, JAVA, Ringtones,
                Video and Wallpaper. Please validate your XML against these schemas before uploading.<br />
                - You can generate the XML file yourself or use the Excel utility provided below.
                If you generate the XML file yourself please validate it against the XML schemas.<br />
                - Excel utility to generate XML file and schemas:
            </p>
            <p class="textoBold">
                Games:<br />
            </p>
            <p>
                http://184.106.12.89/ContentAdmin/xsd/Juego.xsd
            </p>
            <p class="textoBold">
                Wallpapers:<br />
            </p>
            <p>
                http://184.106.12.89/ContentAdmin/xsd/Wallpaper.xsd
            </p>
            <p class="textoBold">
                Ringtones/Fulltracks:<br />
            </p>
            <p>
                http://184.106.12.89/ContentAdmin/xsd/Ringtone.xsd
            </p>
            <p class="textoBold">
                Videos:<br />
            </p>
            <p>
                http://184.106.12.89/ContentAdmin/xsd/Video.xsd
            </p>
            <p class="textoBold">
                Excel utility:<br />
            </p>
            <p>
                http://www.wilaen.com/tracfone/Excel_utilities.zip
            </p>
            <p>
                - Follow the categories names for each type of content as follow, please provide
                them in Spanish, English and Portuguese.</p>
            <p>
                http://www.wilaen.com/tracfone/Tracfone%20-%20Content%20categories.xlsx<br />
            </p>
            <p class="textoBold">
                PRICE REFERENCE
                <br />
            </p>
            <p>
                Please, follow this price list as a reference for your content.<br />
                http://www.wilaen.com/tracfone/Tracfone%20-%20Content%20price%20list.xlsx<br />
            </p>
            <p class="textoBold">
                CONTENT SPECIFICATION<br />
            </p>
            <p>
                Please download and check the latest specification document:<br />
                http://www.wilaen.com/tracfone/Tracfone%20content%20asset.docx<br />
            </p>
            <p class="textoBold">
                TRACFONE HANDSETS<br />
            </p>
            <p>
                Please download and check the latest handset list:<br />
                http://www.wilaen.com/tracfone/Tracfone-StraightTalk-Net10%20-%20Handset%20Specification%20Sheet.xls<br />
            </p>
            <p>
                ---</p>
            <p class="textoBold">
                STEPS</p>
            <p>
                1. Prepare the zip package, with files and xml document.
                <br />
                2. Upload it in the section: UPLOAD CONTENT.<br />
                3. Check the log error. Any doubt please contact op@wilaen.com.<br />
                4. If everything is correct, you will see the content in the section: ITEM. Also,
                you can edit by yourself.<br />
                5. Let WILAEN Operation/Content team know that your content is ready to put it live.<br />
            </p>
        </div>
    </div>
    </form>
</body>
</html>
