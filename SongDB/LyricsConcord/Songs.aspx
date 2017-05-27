<%@ page title="" language="C#" masterpagefile="~/MP.Master" autoeventwireup="true" codebehind="Songs.aspx.cs" inherits="LyricsConcord.Songs" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <label>Upload Song</label>
    <form id="upload_form" method="post" enctype="multipart/form-data" runat="server">
        <input type="file" id="File1" name="File1" runat="server">
        <input type="submit" id="Submit1" name="Submit1" value="Upload" runat="server" onserverclick="Submit1_ServerClick" onclick="setTimeout(function () { $('#succ_msg').fadeOut() }, 5000)">
        <br />
        <div id="succ_msg" class="alert alert-success alert-dismissable" runat="server" hidden>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            The file has been uploaded
        </div>
        <div id="warn_msg" class="alert alert-warning alert-dismissable" runat="server" hidden>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            Please select a file to upload
        </div>
        <div id="err_msg" class="alert alert-danger alert-dismissable" runat="server" hidden>
            <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
            Errot uploading file
        </div>
    </form>
    <!-- Ajax doesn't work. Check Why
        <div class="form-group">
        <input type="file" id="song_file">
        <input type="submit" id="upload_btn" value="Upload">
    </div>
    -->
    <br />
    <!-- Filter options -->
    <div class="form-group">
        <label>Filter</label>
        <div class="radio">
            <label style="display:inline-block; margin-right:10px">
                <input type="radio" name="optionsRadios" id="optionName" value="song" checked>By song name
            </label>
            <div style="display:inline-block">
                <input class="form-control" name="filterVal" id="song" placeholder="Enter song name" disabled>
            </div>
        </div>
        <div class="radio">
            <label style="display:inline-block; margin-right:10px">
                <input type="radio" name="optionsRadios" id="optionArtist" value="artist">By artist name
            </label>
            <div style="display:inline-block">
                <input class="form-control" name="filterVal" id="artist" placeholder="Enter artist name" disabled>
            </div>
        </div>
        <button class="btn btn-primary" id="submitFilter">Get Songs</button>
    </div>

    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-songs">
        <thead>
            <tr>
                <th>Id</th>
                <th>Song</th>
                <th>Artist</th>
            </tr>
        </thead>
        <tbody id="songs">
            <%foreach (var item in songs.Songs)
              {%>
                    <tr id="<%:item.Id %>">
                        <td><%:item.Id %></td>
                        <td><%:item.Name %></td>
                        <td><%:item.ArtistName %></td>
                    </tr>
            <% }%>
        </tbody>
    </table>
</asp:content>

<asp:content id="Content2" contentplaceholderid="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Songs');
            //$('#succ_msg').attr('hidden', "hidden");
        });

        var t = $('#table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
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

        $("input[name='optionsRadios']").change(function () {
            $("input[name='filterVal']").attr('disabled', "disabled");
            $('#' + this.value).removeAttr('disabled');
        });

        // Filter songs
        $('#submitFilter').click(function () {
            var selectedFilter = $("input[name='optionsRadios']:checked").val();
            var id = '#' + selectedFilter;
            var filter = $(id).val();
            var songName = "";
            var artistName = "";

            if (selectedFilter == "song") {
                songName = filter;
            } else if (selectedFilter == "artist") {
                artistName = filter;
            }

            var jsonRequest = { partSongName: songName, partAritstName: artistName };
            
            $.ajax({
                type: "POST",
                url: "Songs.aspx/GetSongs",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    t.clear().draw();
                    $.each(msg.d, function () {
                        t.row.add([this.Id, this.Name, this.ArtistName]).draw(false);
                    })
                }
            });

            return false;
        });
    </script>
</asp:content>
