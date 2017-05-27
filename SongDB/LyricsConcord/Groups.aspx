<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Groups.aspx.cs" Inherits="LyricsConcord.Groups" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Filter options -->
    <div class="form-group">
        <button class="btn btn-primary" data-toggle="modal" data-target="#add_group">Add New Group</button>
    </div>
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-songs">
        <thead>
            <tr>
                <th>Id</th>
                <th>Group Name</th>
            </tr>
        </thead>
        <tbody>
            <%foreach (var item in groups.Groups)
                {%>
            <tr id="<%:item.Id %>">
                <td><%:item.Id %></td>
                <td><%:item.Name %></td>
            </tr>
            <% }%>
        </tbody>
    </table>

    <div class="modal fade bs-example-modal-lg" tabindex="-1" role="dialog" aria-labelledby="wordContext" id="add_group">
        <div class="modal-dialog modal-lg" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">×</span></button>
                    <div class="row">
                        <div class="col-lg-3">
                            <h4 class="modal-title" id="myLargeModalLabel">New Group</h4>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Group Name</label>
                            <div class="col-md-6">
                                <input name="new_name" id="new_name" type="text" class="form-control" placeholder="Group Name">
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Words</label>
                            <div class="col-md-6">
                                <span id="new_words"></span>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Add Word</label>
                            <div class="col-md-6">
                                <select class="form-control" id="new_word">
                                    <option value="-1">Select a words to add...</option>
                                    <%foreach (var item in words.Words)
                                        {%>
                                    <option><%:item.Item2 %></option>
                                    <% }%>
                                </select>
                                <input name="new_word_input" id="new_word_input" type="text" class="form-control" placeholder="Word">
                                <button id="add_word" class="btn">Add word</button>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="new_group_button">Add Group</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Groups');
        });
        var word_list = new Array();
        var t = $('table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
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
                if ($('#new_words').html() != '') {
                    $('#new_words').append(', ');
                }
                $('#new_words').append(word);
                word_list.push(word);
            }
        };

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

            return false;
        });

    </script>
</asp:Content>
