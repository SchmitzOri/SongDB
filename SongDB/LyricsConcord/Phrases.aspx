<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Phrases.aspx.cs" Inherits="LyricsConcord.Phrases" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <label>Add phrase</label>
    <div class="form-group">
        <div style="display:inline-block; width:500px"><input type="text" class="form-control" id="new_phrase" placeholder="Enter new phrase" /></div>
        <div style="display:inline-block"><button class="btn btn-primary" id="add_phrase">Add phrase</button></div>
    </div>
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-songs">
        <thead>
            <tr>
                <th>Id</th>
                <th>Phrase</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <%foreach (var item in phrases.Phrases)
                {%>
            <tr id="<%:item.PhraseId %>">
                <td><%:item.PhraseId %></td>
                <td><%:item.Phrase %></td>
                <td>
                    <div style="display:inline-block"><button class="btn delete" id="del_phrase">Delete Phrase</button></div>
                    <div style="display:inline-block"><button class="btn show" id="<%:item.Phrase %>" data-toggle="modal" data-target=".bs-example-modal-lg">Show Context</button></div>
                </td>
            </tr>
            <% }%>
        </tbody>
    </table>

    <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="wordContext" id="word_context">
      <div class="modal-dialog modal-lg" role="document">
          <div class="modal-content">
              <div class="modal-header">
                  <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                  <div class="row">
                      <div class="col-lg-3"><h4 class="modal-title" id="myLargeModalLabel"></h4></div>
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
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Phrase');
        });

        var t = $('table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
        });

        var phrase = null,
            phraseSongs = null,
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

        $('#add_phrase').on('click', function () {
            if ($('#new_phrase').val() != '') {
                var jsonRequest = { phrase: $('#new_phrase').val() };

                $.ajax({
                    type: "POST",
                    url: "Phrases.aspx/AddPhrase",
                    data: JSON.stringify(jsonRequest),
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (msg) {
                        location.reload('true');
                    }
                });
            }
        });

        $('button.delete').on('click', function () {
            var data = t.row($(this).parents('tr')).data();
            var toDel = data[0];
            var jsonRequest = { id: toDel };

            $.ajax({
                type: "POST",
                url: "Phrases.aspx/DeletePhrase",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    location.reload('true');
                }
            });
        });

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
                if (currentIndex == 0 && currentSong == 0) {
                    $prevBtn[0].disabled = true;
                } else if (currentIndex < 0) {
                    prevSong();

                    if (currentSong == 0 && currentIndex == 0) {
                        $prevBtn[0].disabled = true;
                    }
                }
                if (currentIndex >= $results.length - 1 && currentSong + 1 >= phraseSongs.length) {
                    $nextBtn[0].disabled = true;
                } else if (currentIndex > $results.length - 1) {
                    nextSong('');

                    if (currentIndex >= $results.length - 1 && currentSong + 1 >= phraseSongs.length) {
                        $nextBtn[0].disabled = true;
                    }
                }

                jumpTo();
            }
        });

        $('#word_context').on('shown.bs.modal', function (e) {
            phrase = $(e.relatedTarget).attr('id');
            var jsonRequest = { phrase: phrase };

            $.ajax({
                type: "POST",
                url: "Phrases.aspx/GetPhraseLocs",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    phraseSongs = msg.d;
                    currentSong = -1;

                    if (phraseSongs.length == 0) {
                        $('h4.modal-title').text("No song found");
                        $('div.modal-body').text("");
                        $nextBtn[0].disabled = true;
                        $prevBtn[0].disabled = true;

                    } else {
                        // Show first context
                        nextSong('');
                    }
                },
                error: function (xhr, status, error) {
                    alert(xhr.responseText);
                }
            });
        });

        $('#word_context').on('hide.bs.modal', function (e) {
            phrase = null;
            phraseSongs = null;
            currentSong = -1;
            currentIndex = 0;
            $nextBtn[0].disabled = false;
            $prevBtn[0].disabled = false;
        });

        function nextSong(songId) {
            var songName = null;
            var songId = null;
            currentSong++;

            if (currentSong < phraseSongs.length) {
                songId = phraseSongs[currentSong].SongId;
                songName = phraseSongs[currentSong].SongName;
            } else {
                return;
            }

            currentIndex = 0;
            songLyrics(songId, songName, true);
        }

        function prevSong() {
            var songName = null;
            var songId = null;
            currentSong--;

            if (currentSong >= 0) {
                songId = phraseSongs[currentSong].SongId;
                songName = phraseSongs[currentSong].SongName;
            } else {
                return;
            }

            songLyrics(songId, songName, false);
        }

        function songLyrics(songId, songName, isNext) {
            var lyricsRequest = { songId: songId };
            $.ajax({
                type: "POST",
                url: "Phrases.aspx/GetSongLyrics",
                data: JSON.stringify(lyricsRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {
                    $('h4.modal-title').text(songName);
                    $('div.modal-body').html(res.d.replace(/\n/g, "<br />"));

                    // Mark the first phrase
                    var searchVal = phrase;
                    $content.unmark({
                        done: function () {
                            $content.mark(searchVal, {
                                "accuracy": {
                                    "value": "exactly",
                                    "limiters": [",", "."]
                                },
                                separateWordSearch: false,
                                ignorePunctuation: ["'", "`"],
                                done: function () {
                                    $results = $content.find("mark");
                                    if (isNext) {
                                        currentIndex = 0;
                                    } else {
                                        currentIndex = $results.length - 1;
                                    }
                                    jumpTo();
                                }
                            });
                        }
                    });

                    if (currentSong == 0 && currentIndex == 0) {
                        $prevBtn[0].disabled = true;
                        if (currentSong == phraseSongs.length - 1 &&
                            currentIndex + 1 > $results.length - 1) {
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
</asp:Content>
