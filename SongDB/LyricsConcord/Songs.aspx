<%@ page title="" language="C#" masterpagefile="~/MP.Master" autoeventwireup="true" codebehind="Songs.aspx.cs" inherits="LyricsConcord.Songs" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <label>Upload Song</label>
    <form id="upload_form" method="post" enctype="multipart/form-data" runat="server">
        <input type="file" id="File1" name="File1" runat="server">
        <input type="submit" id="Submit1" name="Submit1" value="Upload" runat="server" onserverclick="Submit1_ServerClick">
    </form>
    <!-- Ajax doesn't work. Check Why
        <div class="form-group">
        <input type="file" id="song_file">
        <input type="submit" id="upload_btn" value="Upload">
    </div>
    -->

    <div class="form-group">
        <label>Song</label>
        <ul id="song_id">
            <%foreach (var item in songs.Songs)
                {%>
            <li id="<%:item.Id %>"><%:item.Name %></li>
            <%} %>
        </ul>
    </div>
</asp:content>

<asp:content id="Content2" contentplaceholderid="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Songs');
        });

        // Ajax doesn't work. Check Why
        $('#upload_btn').click(function () {
            var file = $('#song_file')[0].files[0];
            var data = new FormData();

            if (file) {
                data.append("songFile", file);

                $.ajax({
                    url: "Words.aspx/UploadFile",
                    type: "POST",
                    data: data,
                    processData: false,
                    contentType: false,
                    success: function (msg) {
                        alert(msg.d);
                    },
                    error: function (err) {
                        alert("Error");
                    }
                });
            }

            return false;
        });

    </script>
</asp:content>
