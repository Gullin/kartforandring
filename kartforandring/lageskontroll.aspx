<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lageskontroll.aspx.cs" Inherits="kartforandring.lageskontroll" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Beställning lägeskontroller</title>

    <link href="Styles/jquery-ui-1.9.2.custom/css/smoothness/jquery-ui-1.9.2.custom.min.css" rel="stylesheet" />
    <link href="Styles/page-UI-centering.css" rel="stylesheet" />
    <link href="Styles/page-UI-core.css" rel="stylesheet" />
    <link href="Styles/page-UI-jTable.css" rel="stylesheet" />

    <link href="Scripts/jtable/themes/lightcolor/gray/jtable.css" rel="stylesheet" />


    <!-- Settings -->
    <script type="text/javascript">
        var baseUrl = '<%=ResolveClientUrl("~")%>';
    </script>

    <script src='<%=ResolveClientUrl("Scripts/jquery-1.9.1.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/jquery-ui-1.9.2.min.js")%>'></script>

    <script src='<%=ResolveClientUrl("Scripts/jtable/jquery.jtable.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/jtable/extensions/jquery.jtable.aspnetpagemethods.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/jtable/localization/jquery.jtable.se.js")%>'></script>

    <script src='<%=ResolveClientUrl("Scripts/lageskontroll.js")%>'></script>
</head>

<body>
    <form id="form1" runat="server">
        <div id="container">
            <h1>För internbeställning av lägeskontroller</h1>
            <h3>Tabellen listar registrerade bygglov hos MBK- och GIS-avdelningen. <br />
                Kolumnen "Beställning" indikerar om lägeskontroll är beställd av Stadsarkitektavdelningen (Intern) eller byggherre (Extern).</h3>
            <div id="LageskontrollTableContainer"></div>
        </div>

        <div id="versionWrapper">
            <asp:Label ID="lblVersion" runat="server"></asp:Label>
        </div>

        <div id="copyrightWrapper">
            2014 - <asp:Label ID="lblCopyrightYear" runat="server" /> 
        </div>
    </form>
</body>
</html>
