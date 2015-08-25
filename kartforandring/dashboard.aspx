<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="kartforandring.dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kartförändringar | Kontrollpanel</title>

    <link href="Styles/page-UI-centering.css" rel="stylesheet" />
    <link href="Styles/page-UI-core.css" rel="stylesheet" />
    <link href="Styles/jquery-ui-1.9.2.custom/css/smoothness/jquery-ui-1.9.2.custom.min.css" rel="stylesheet" />
    <style>
        .ui-tabs .ui-tabs-nav li {
            text-align: center;
        }
            .ui-tabs .ui-tabs-nav li a {
                display: block;
                padding: 0.5em;
                text-decoration: none;
            }

        #tabs div
        {
            padding: 1em;
        }
        .dashboardPart
        {
            border: 1px solid rgb(220,220,220);
        }
        .dashboardPartHeader
        {
            position: relative;
            top: -2.75em;
            left: -0.5em;
        }
            .dashboardPartHeader span {
                background-color: white;
                line-height: 1.5em;
                padding: 0em 1em;
            }
    </style>

    <script src='<%=ResolveClientUrl("Scripts/jquery-1.9.1.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/jquery-ui-1.9.2.min.js")%>'></script>

    <script type="text/javascript">
        $(function () {
            $("#tabs").tabs();
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <!--  //-->
        <div id="tabs">
            <ul>
                <li><a href="#tabs-1">Brister</a></li>
                <li><a href="#tabs-2">Inställningar</a></li>
            </ul>
            <div id="tabs-1">
                <div class="dashboardPart">
                    <p class="dashboardPartHeader"><span>Översikt</span></p>
                    <div>INNEHÅLL</div>
                </div>
            </div>
            <div id="tabs-2">
                <div class="dashboardPart">
                    <p class="dashboardPartHeader"><span>Brister</span></p>
                    <div></div>
                </div>
            </div>
        </div>

    </div>
    </form>
</body>
</html>
