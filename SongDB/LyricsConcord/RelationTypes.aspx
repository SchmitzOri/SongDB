<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="RelationTypes.aspx.cs" Inherits="LyricsConcord.RelationTypes" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <!-- Filter options -->
    <div class="form-group">
        <button id="open_modal_button" class="btn btn-primary" data-toggle="modal" data-target="#add_group">Add New Relation Type</button>
    </div>
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline" id="table" role="grid" aria-describedby="dataTables-songs">
        <thead>
            <tr>
                <th>Id</th>
                <th>Relation Type Name</th>
                <th>Actions</th>
            </tr>
        </thead>
        <tbody>
            <%foreach (var item in types.Types)
                {%>
            <tr id="<%:item.Id %>">
                <td><%:item.Id %></td>
                <td><%:item.TypeName %></td>
                <td>
                    <button class="btn delete" id="del_type">Delete Relation Type</button>
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
                            <h4 class="modal-title" id="myLargeModalLabel">New Relation Type</h4>
                        </div>
                    </div>
                </div>
                <div class="modal-body">
                    <div class="row">
                        <div class="form-group">
                            <label class="col-md-3 control-label">
                                Relation Type Name</label>
                            <div class="col-md-6">
                                <input name="new_name" id="new_name" type="text" class="form-control" placeholder="Relation Type Name">
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-primary" id="new_button">Add Relation Type</button>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Relation Types');
        });
        var t = $('table').DataTable({
            "columnDefs": [
                { "visible": false, "targets": 0 }
            ],
            pageLength: 25,
            ordering: false
        });

        $('#add_group').on('hidden.bs.modal', function () {
            $('#new_name').val('');
        });

        $('#new_button').on('click', function () {
            if ($('#new_name').val() == '') {
                bootbox.alert("Please enter a name for the group");
            } else {
                var jsonRequest = { name: $('#new_name').val() };
                $.ajax({
                    type: "POST",
                    url: "RelationTypes.aspx/AddRelationType",
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
                url: "RelationTypes.aspx/RelationTypeUseCount",
                data: JSON.stringify(jsonRequest),
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (msg) {
                    if (msg.d > 0) {
                        bootbox.alert("The relation type has " + msg.d + " relations, cannot delete");
                    }
                    else {
                        $.ajax({
                            type: "POST",
                            url: "RelationTypes.aspx/RelationTypeDelete",
                            data: JSON.stringify(jsonRequest),
                            contentType: "application/json; charset=utf-8",
                            dataType: "json",
                            success: function (msg) {
                                location.reload('true');
                            }
                        });
                    }
                }
            });
        });
    </script>
</asp:Content>
