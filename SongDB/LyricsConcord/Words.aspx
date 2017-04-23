<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Words.aspx.cs" Inherits="LyricsConcord.Words" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div class="form-group">
        <label>Song</label>
        <select id="song_id" class="form-control">
            <option value="">All...</option>
            <%foreach (var item in songs.Songs)
                {%>
            <option value="<%:item.Id %>"><%:item.Name %></option>
            <%} %>
        </select>
    </div>
    <button id="show_words" type="submit" class="btn btn-default">Show words</button>
    <table id="table">
        <thead>
            <tr>
                <th>Id</th>
                <th>Word</th>
                <th></th>
            </tr>
        </thead>
        <tbody>
        </tbody>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Words');
        });

        var t = $('#table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
        });

        $('#show_words').on('click', function () {
            var jsonRequest = { songId: null };

            if ($('#song_id').val() != '') {
                jsonRequest.songId = $('#song_id').val();
            }

            $.ajax({
                type: "POST",
                url: "Words.aspx/GetSongWords",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    t.clear().draw();
                    $.each(msg.d, function () {
                        t.row.add([this.Item1, this.Item2, '<a>blala</a>']).draw(false);
                    })
                }
            });

            return false;
        });
    </script>
</asp:Content>
