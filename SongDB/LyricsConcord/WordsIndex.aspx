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
    <br />
    <!-- Search word by location -->
    <div id="search_area" class="form-group collapse">
        <label>Find word by location</label>
        <div class="radio">
            <label style="display:inline-block; margin-right:10px">
                <input type="radio" name="optionsRadios" id="option1" value="1" checked>Option 1
            </label>
            <div style="display:inline-block">
                <input class="form-control" name="searchVal" id="num_in_song" placeholder="Enter word number in song">
            </div>
        </div>
        <div class="radio">
            <label style="display:inline-block; margin-right:10px">
                <input type="radio" name="optionsRadios" id="option2" value="2">Option 2
            </label>
            <div style="display:inline-block">
                <div><input class="form-control" name="searchVal" id="verse_num" placeholder="Enter verse number" disabled></div>
                <div><input class="form-control" name="searchVal" id="verse_line" placeholder="Enter line in verse" disabled></div>
            </div>
        </div>
        <button class="btn btn-primary" id="search">Find</button>
    </div>
    <br />
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
                $('#search')[0].disabled = false;
                $("input[name='searchVal']").val("");
                $('#search_area').fadeIn("slow", "linear");

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
                $('#search')[0].disabled = true;
                $('#search_area').fadeOut("slow", "linear");
            }

            return false;
        });

        $("#option1").change(function () {
            $("input[name='searchVal']").attr('disabled', "disabled");
            $('#num_in_song').removeAttr('disabled');
        });

        $("#option2").change(function () {
            $("input[name='searchVal']").attr('disabled', "disabled");
            $('#verse_num').removeAttr('disabled');
            $('#verse_line').removeAttr('disabled');
        });

        // Find word
        $('#search').click(function () {
            if ($('#song_id').val() == '') {
                t.clear().draw();
                return false;
            }

            var selectedOption = $("input[name='optionsRadios']:checked").val();

            var songId = $('#song_id').val();
            var numInSong = -1;
            var verseNum = -1;
            var lineInVerse = -1;

            if (selectedOption == "1") {
                if (isNaN($('#num_in_song').val())) {
                    $('#num_in_song').parent().addClass("has-error");
                    return false;
                } else {
                    $('#num_in_song').parent().removeClass("has-error");
                }

                numInSong = $('#num_in_song').val();
            } else {
                if (isNaN($('#verse_num').val())) {
                    $('#verse_num').parent().addClass("has-error");
                    return false;
                } else {
                    $('#verse_num').parent().removeClass("has-error");
                }

                if (isNaN($('#verse_line').val())) {
                    $('#verse_line').parent().addClass("has-error");
                    return false
                } else {
                    $('#verse_line').parent().removeClass("has-error");
                }

                verseNum = $('#verse_num').val();
                lineInVerse = $('#verse_line').val();
            }

            var jsonRequest = {
                songId: songId,
                numInSong: numInSong,
                verseNum: verseNum,
                lineInVerse: lineInVerse
            };

            $.ajax({
                type: "POST",
                url: "WordsIndex.aspx/GetWordByLocation",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    t.clear().draw();
                    $.each(msg.d, function () {
                        t.row.add([this.Id, this.Word, this.Song, this.NumberInSong,
                        this.VerseNumber, this.LineInVerse]).draw(false);
                    })
                }
            });

            return false;
        });
    </script>
</asp:content>