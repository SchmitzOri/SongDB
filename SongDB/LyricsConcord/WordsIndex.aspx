<%@ page title="" language="C#" masterpagefile="~/MP.Master" autoeventwireup="true" codebehind="WordsIndex.aspx.cs" inherits="LyricsConcord.WordsIndex" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
    <div class="form-group">
        <label>Choose song</label>
        <select id="song_id" class="form-control">
            <option value="">Choose...</option>
            <%foreach (var item in songs.Songs)
                {%>
            <option value="<%:item.Id %>"><%:item.Name %></option>
            <%} %>
        </select>
    </div>
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-words">
        <thead>
            <tr>
                <th>Location Id</th>
                <th>Word</th>
                <th>Song</th>
                <th># in Song</th>
                <th>Verse #</th>
                <th>Line in Verse</th>
            </tr>
        </thead>
        <tbody id="words">
        </tbody>
    </table>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Words Index');
            $('#song_id').change();
        });

        var t = $('#table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
        });

        $('#song_id').on('change', function () {
            var jsonRequest = { songId: null };

            if ($('#song_id').val() != '') {
                jsonRequest.songId = $('#song_id').val();

                $.ajax({
                    type: "POST",
                    url: "WordsIndex.aspx/GetLocations",
                    data: JSON.stringify(jsonRequest),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        t.clear().draw();
                        $.each(msg.d, function () {
                            t.row.add([this.Id, this.Word, this.Song, this.NumberInSong,
                            this.VerseNumber, this.LineInVerse]).draw(false);
                        })
                    },
                    error: function (xhr, status, error) {
                        alert(xhr.responseText);
                    }
                });
            } else {
                t.clear().draw();
            }

            return false;
        });
    </script>
</asp:content>