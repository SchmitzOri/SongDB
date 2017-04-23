<%@ Page Title="" Language="C#" MasterPageFile="~/MP.Master" AutoEventWireup="true" CodeBehind="Stats.aspx.cs" Inherits="LyricsConcord.Stats" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <thead>
            <tr>
                <th>Characters/Word</th>
            </tr>
        </thead>
        <tbody>
            <tr>
                <td><%:stat.CharsPerWord %></td>
            </tr>
        </tbody>
    </table>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ScriptsPlaceHolder" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $('h1.page-header').text('Statistics');
        });
    </script>
</asp:Content>
