<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="LyricsConcord.Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <label>Upload Song</label>
    <label>Import database</label>
    <form id="upload_form" method="post" enctype="multipart/form-data" runat="server">
        <input type="file" id="File1" name="File1" runat="server">
        <input type="submit" id="Submit1" name="Submit1" value="Upload" runat="server" onserverclick="Submit1_ServerClick">
    </form>
    <label>Export database</label>
    <div class="form-group">
        <input type="button" id="export" name="export" value="Export">
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Import/Export');

            $('#export').on('click', function () {
                document.location = '<%=ConfigurationManager.AppSettings["ServiceURL"] + "ExportXML" %>';
            });
        });
    </script>
</asp:Content>
