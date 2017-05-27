<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Export.aspx.cs" Inherits="LyricsConcord.Export" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <label>Upload Song</label>
    <label>Import database</label>
    <form id="upload_form" method="post" enctype="multipart/form-data" runat="server">
        <input type="file" id="File1" name="File1" runat="server">
        <input type="submit" id="Submit1" name="Submit1" value="Upload" runat="server" onserverclick="Submit1_ServerClick">
        <div id="succ_msg" class="alert alert-success alert-dismissable" runat="server" hidden>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            The import succeeded
        </div>
        <div id="warn_msg" class="alert alert-warning alert-dismissable" runat="server" hidden>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            Please select a file to import
        </div>
        <div id="err_msg" class="alert alert-danger alert-dismissable" runat="server" hidden>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            Errot importing file
        </div>
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
