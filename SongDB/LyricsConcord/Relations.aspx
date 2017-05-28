<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Relations.aspx.cs" Inherits="LyricsConcord.Relations" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Filter options -->
    <div class="form-group">
        <button id="open_modal_button" class="btn btn-primary" data-toggle="modal" data-target="#add_group">Add New Relation</button>
    </div>
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-songs">
        <thead>
            <tr>
                <th>Id</th>
                <th>Relation Type</th>
                <th>Word 1</th>
                <th>Word 2</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <%foreach (var item in relations.Relations)
                {%>
            <tr id="<%:item.Id %>">
                <td><%:item.Id %></td>
                <td><%:item.RelationType.TypeName%></td>
                <td><%:item.Word1.Item2 %></td>
                <td><%:item.Word2.Item2 %></td>
                <td>
                    <button class="btn delete">Delete Relation</button>
                </td>
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
                            <h4 class="modal-title" id="myLargeModalLabel">New Relation</h4>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Relation Type</label>
                            <div class="col-md-6">
                                <select class="form-control" id="relation_type">
                                    <option value="-1">Select relation type...</option>
                                    <%foreach (var item in relTypes.Types)
                                        {%>
                                    <option value="<%:item.Id %>"><%:item.TypeName %></option>
                                    <% }%>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Word 1</label>
                            <div class="col-md-6">
                                <select class="form-control" id="word1">
                                    <option value="-1">Select a word...</option>
                                    <%foreach (var item in words.Words)
                                        {%>
                                    <option value="<%:item.Item1 %>"><%:item.Item2 %></option>
                                    <% }%>
                                </select>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Word 2</label>
                            <div class="col-md-6">
                                <select class="form-control" id="word2">
                                    <option value="-1">Select a word...</option>
                                    <%foreach (var item in words.Words)
                                        {%>
                                    <option value="<%:item.Item1 %>"><%:item.Item2 %></option>
                                    <% }%>
                                </select>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="new_group_button">Add Relation</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Relations');
        });
        var t = $('table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
        });

        $('#open_modal_button').on('click', function () {
            $('#relation_type').val('-1');
            $('#word1').val('-1');
            $('#word2').val('-1');
        });

        $('#new_group_button').on('click', function () {
            if ($('#relation_type').val() == '-1') {
                bootbox.alert("Please select a relation type");
            }
            else if ($('#word1').val() == '-1' || $('#word2').val() == '-1') {
                bootbox.alert("Please select words for the relation");
            } else {
                var jsonRequest = { type: $('#relation_type').val(), word1: $('#word1').val(), word2: $('#word2').val() };
                $.ajax({
                    type: "POST",
                    url: "Relations.aspx/AddRelation",
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

        $('button.delete').on('click', function () {
            var data = t.row($(this).parents('tr')).data();
            toDel = data[0];
            jsonRequest = { id: toDel };

            $.ajax({
                type: "POST",
                url: "Relations.aspx/DeleteRelation",
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
