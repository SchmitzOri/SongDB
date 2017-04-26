<%@ page title="" language="C#" masterpagefile="~/MP.Master" autoeventwireup="true" codebehind="Words.aspx.cs" inherits="LyricsConcord.Words" %>

<asp:content id="Content1" contentplaceholderid="ContentPlaceHolder1" runat="server">
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
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-words">
        <thead>
            <tr>
                <th>Id</th>
                <th>Word</th>
                <th>Show Word Context</th>
            </tr>
        </thead>
        <tbody id="words">
        </tbody>
    </table>
    <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="wordContext" id="word_context">
      <div class="modal-dialog modal-lg" role="document">
          <div class="modal-content">
              <div class="modal-header">
                  <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                  <div class="row">
                      <div class="col-lg-3"><h4 class="modal-title" id="myLargeModalLabel">Large modal</h4></div>
                      <div class="col-lg-1"><button class="btn btn-primary" data-search="prev">prev</button></div>
                      <div class="col-lg-1"><button class="btn btn-primary" data-search="next">next</button></div>
                  </div>
              </div> 
              <div class="modal-body" id="file_content">
              </div>
          </div>
      </div>
    </div>

    <style type="text/css">
    mark {
      background: yellow;
    }

    mark.current {
      background: orange;
    }
    </style>
</asp:content>
<asp:content id="Content2" contentplaceholderid="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Words');
            $('#song_id').change();
        });

        var word = null,
            wordId = null,
            wordSongs = null,
            currentSong = -1;

        // prev button
        var $prevBtn = $("button[data-search='prev']"),
            // next button
            $nextBtn = $("button[data-search='next']"),
            // the context where to search
            $content = $("div.modal-body"),
            // jQuery object to save <mark> elements
            $results,
            // the class that will be appended to the current
            // focused element
            currentClass = "current",
            // top offset for the jump (the search bar)
            offsetTop = 50,
            // the current index of the focused element
            currentIndex = 0;

        /**
         * Jumps to the element matching the currentIndex
         */
        function jumpTo() {
            if ($results.length) {
                var position,
                    $current = $results.eq(currentIndex);
                $results.removeClass(currentClass);
                if ($current.length) {
                    $current.addClass(currentClass);
                    position = $current.offset().top - offsetTop;
                    $('#word_context').scrollTop(position);
                }
            }
        }

        /**
         * Next and previous search jump to
         */
        $nextBtn.add($prevBtn).on("click", function () {
            if ($results.length) {
                currentIndex += $(this).is($prevBtn) ? -1 : 1;
                $nextBtn[0].disabled = false;
                $prevBtn[0].disabled = false;
                if (currentIndex < 0) {
                    if ($('#song_id').val() != '' || currentSong == 0) {
                        $prevBtn[0].disabled = true;
                    } else {
                        prevSong();

                        if (currentSong == 0) {
                            $prevBtn[0].disabled = true;
                        }
                    }
                }
                if (currentIndex > $results.length - 1) {
                    if ($('#song_id').val() != '' || currentSong + 1 == wordSongs.length) {
                        $nextBtn[0].disabled = true;
                    } else {
                        nextSong('');

                        if (currentSong + 1 == wordSongs.length) {
                            $nextBtn[0].disabled = true;
                        } 
                    }
                }
                jumpTo();
            }
        });

        $('#word_context').on('shown.bs.modal', function (e) {
            word = $(e.relatedTarget).attr('name');
            wordId = $(e.relatedTarget).attr('id');
            var songRequest = { wordId: wordId };

            if ($('#song_id').val() != '') {
                nextSong($('#song_id').val());
            } else {
                $.ajax({
                    type: "POST",
                    url: "Words.aspx/GetWordSongs",
                    data: JSON.stringify(songRequest),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        wordSongs = msg.d;
                        currentSong = -1;

                        // Show first context
                        nextSong('');
                    },
                    error: function (xhr, status, error) {
                        alert(xhr.responseText);
                    }
                });
            }
        })

        $('#word_context').on('hide.bs.modal', function (e) {
            word = null;
            wordId = null;
            wordSongs = null;
            currentSong = -1;
            currentIndex = 0;
        })

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
                        t.row.add([this.Item1, this.Item2,
                            '<button type="button" id=' + this.Item1 + ' name= ' + this.Item2 +
                            ' class="btn" data-toggle="modal" data-target=".bs-example-modal-lg">show context</button>']).draw(false);
                    })
                }
            });

            return false;
        });

        function nextSong(songId) {
            var songName = null;
            currentSong++;

            if (songId != '') {
                songName = $('#song_id')[0].selectedOptions[0].text;
            } else if (currentSong < wordSongs.length) {
                songId = wordSongs[currentSong].Id;
                songName = wordSongs[currentSong].Name;
            } else {
                return;
            }

            songLyrics(songId, songName);
        }

        function prevSong() {
            var songName = null;
            var songId = null;
            currentSong--;

            if (currentSong >= 0) {
                songId = wordSongs[currentSong].Id;
                songName = wordSongs[currentSong].Name;
            } else {
                return;
            }

            songLyrics(songId, songName);
        }

        function songLyrics(songId, songName) {
            var lyricsRequest = { songId: songId };
            $.ajax({
                type: "POST",
                url: "Words.aspx/GetSongLyrics",
                data: JSON.stringify(lyricsRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('h4.modal-title').text(songName);
                    $('div.modal-body').html(res.d.replace(/\n/g, "<br />"));

                    // Mark the first word
                    var searchVal = word;
                    $content.unmark({
                        done: function () {
                            $content.mark(searchVal, {
                                accuracy: "exactly",
                                done: function () {
                                    $results = $content.find("mark");
                                    currentIndex = 0;
                                    jumpTo();
                                }
                            });
                        }
                    });

                    if (($('#song_id').val() != '' || currentSong == 0) && $results.length) {
                        $prevBtn[0].disabled = true;
                        if ($('#song_id').val() != '' && currentIndex + 1 > $results.length - 1) {
                            $nextBtn[0].disabled = true;
                        }
                    }
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                }
            });
        }
    </script>
</asp:content>