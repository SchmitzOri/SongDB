<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="LyricsConcord.Stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table class="table table-striped table-bordered table-hover dataTable no-footer dtr-inline">
        <thead>
            <tr>
                <th>Characters/Word</th>
                <th>Words/Row</th>
                <th>Rows/Verse</th>
                <th>Verses/Song</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><%:stat.CharsPerWord %></td>
                <td><%:stat.WordsInRow %></td>
                <td><%:stat.RowsInVerse %></td>
                <td><%:stat.VersesInSongs%></td>
            </tr>
        </tbody>
    </table>
    <div id="cloud"></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script src="../vendor/jqcloud2/jqcloud.min.js"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Statistics');
        });

        var words = [
        <% foreach (var item in stat.WordCloud)
        {%>
            { text: "<%: item.Item2%>", weight: <%: item.Item1%> },
        <%}%>
        ];

        $('#cloud').jQCloud(words, {
            width: 1000,
            height: 350
        });
    </script>
</asp:Content>
