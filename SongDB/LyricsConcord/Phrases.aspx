<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Phrases.aspx.cs" Inherits="LyricsConcord.Phrases" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <label>Add phrase</label>
    <input type="text" id="new_phrase" placeholder="Enter new phrase" />
    <button class="btn add" id="add_btn">Add phrase</button>
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
            <tr id="<%:item.Id %>">
                <td><%:item.Id %></td>
                <td><%:item.Name %></td>
                <td>
                    <button class="btn delete" id="del_group">Delete Group</button>
                </td>
            </tr>
            <% }%>
        </tbody>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Groups');
        });
        var word_list = new Array();
        var edited = null;
        var t = $('table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
        });

        $('#open_modal_button').on('click', function () {
            edited = null;
            $('#myLargeModalLabel').html('New Group');
            $('#new_group_button').html('Add Group');
            $('#new_words').html('');
            word_list = new Array();
            $('#new_name').val('');
            $('#new_word_input').val('');
        });

        $('button.edit').on('click', function () {
            var data = t.row($(this).parents('tr')).data();
            edited = data[0];
            $('#myLargeModalLabel').html('Edit Group');
            $('#new_group_button').html('Update Group');
            $('#new_name').val(data[1]);
            $('#new_words').html('');
            word_list = new Array();
            $('#new_word_input').val('');

            var jsonRequest = { id: data[0] };
            $.ajax({
                type: "POST",
                url: "Groups.aspx/Words",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    $.each(msg.d, function (index, value) { addWord(value); });
                }
            });
        });

        $('#new_word').on('change', function () {
            var word = $(this).val();
            addWord(word);
        });

        $('#add_word').on('click', function () {
            addWord($('#new_word_input').val());
            return false;
        });

        function addWord(word) {
            if (word != "-1" && $.inArray(word, word_list) == -1) {
                $('#new_words').append('<button type="button" class="btn btn-labeled remove_word"><span class="btn-label"><i class="glyphicon glyphicon-remove"></i></span> ' + word + '</button>');
                word_list.push(word);
            }
        };

        $('body').on('click', 'button.remove_word', function () {
            word_list.splice($.inArray($(this).text().trim(), word_list), 1);
            $(this).remove();
        });

        $('#add_group').on('hidden.bs.modal', function () {
            word_list = new Array();
            $('#new_name').val('');
            $('#new_word_input').val('');
        });

        $('#new_group_button').on('click', function () {
            if ($('#new_name').val() == '') {
                bootbox.alert("Please enter a name for the group");
            }
            else if (word_list.length == 0) {
                bootbox.alert("Please select words for the group");
            } else {
                if (edited === null) {
                    var jsonRequest = { name: $('#new_name').val(), words: word_list };
                    $.ajax({
                        type: "POST",
                        url: "Groups.aspx/AddGroup",
                        data: JSON.stringify(jsonRequest),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            location.reload('true');
                        }
                    });
                }
                else {
                    var jsonRequest = { id: edited, name: $('#new_name').val(), words: word_list };
                    $.ajax({
                        type: "POST",
                        url: "Groups.aspx/UpdateGroup",
                        data: JSON.stringify(jsonRequest),
                        contentType: "application/json; charset=utf-8",
                        dataType: "json",
                        success: function (msg) {
                            location.reload('true');
                        }
                    });
                }
            }

            return false;
        });

        $('button.delete').on('click', function () {
            var data = t.row($(this).parents('tr')).data();
            toDel = data[0];
            jsonRequest = { id: toDel };

            $.ajax({
                type: "POST",
                url: "Groups.aspx/DeleteGroup",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    location.reload('true');
                }
            });
        });
    </script>
</asp:Content>
