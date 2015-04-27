<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="lageskontroll.aspx.cs" Inherits="kartforandring.lageskontroll" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link href="Styles/jquery-ui-1.9.2.custom/css/smoothness/jquery-ui-1.9.2.custom.min.css" rel="stylesheet" />
    <link href="Styles/page-UI-centering.css" rel="stylesheet" />
    <link href="Styles/page-UI-core.css" rel="stylesheet" />

    <link href="Scripts/jtable/themes/lightcolor/gray/jtable.css" rel="stylesheet" />

    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/jquery-ui-1.9.2.min.js"></script>

    <script src="Scripts/jtable/jquery.jtable.min.js"></script>
    <script src="Scripts/jtable/extensions/jquery.jtable.aspnetpagemethods.min.js"></script>
    <script src="Scripts/jtable/localization/jquery.jtable.se.js"></script>

    <script type="text/javascript">

        $(document).ready(function () {

            $.datepicker.setDefaults($.datepicker.regional["sv"]);

            //Prepare jtable plugin
            $('#LageskontrollTableContainer').jtable({
                title: 'Bygglov med lägeskontroll',
                sorting: true, //Enables sorting
                defaultSorting: 'LageskontrollBestallningText DESC', //Optional. Default sorting on first load.
                ajaxSettings: {
                    type: 'GET',
                    dataType: 'json',
                    contentType: 'application/json; charset=utf-8'
                },
                actions: {
                    listAction: '/api/kartforandring/lageskontroller',
                    createAction: function(postData)
                    {
                        return $.Deferred(function ($dfd) {
                            $.ajax({
                                url: '/api/kartforandring/skapalageskontroll',
                                type: 'PUT',
                                dataType: 'json',
                                data: postData,
                                success: function (data) {
                                    $dfd.resolve(data);
                                },
                                error: function () {
                                    $dfd.reject();
                                }
                            });
                        });
                    },
                    updateAction: '/api/kartforandring/uppdateralageskontroll',
                    deleteAction: function (postData) {
                        return $.Deferred(function ($dfd) {
                            $.ajax({
                                url: '/api/kartforandring/raderalageskontroll',
                                type: 'GET',
                                dataType: 'json',
                                data: postData,
                                success: function (data) {
                                    $dfd.resolve(data);
                                },
                                error: function () {
                                    $dfd.reject();
                                }
                            });
                        });
                    }
                },
                formCreated: function (event, data) {
                    console.log("formCreated");
                    var $form = data.form;
                    $form.find('[name="Diarie"]').attr('placeholder', 'ÅÅÅÅ.#');
                },
                formSubmitting: function (event, data) {
                    if (data.formType == "create") {
                        var $form = data.form;

                        var diarieValue = $form.find('[name="Diarie"]').val();
                        var arendeValue = $form.find('[name="Beskrivning"]').val();

                        var diariePattern = /^(19\d{2}\.\d{1,4})$|^(20\d{2}\.\d{1,4})$/;
                        if (!diariePattern.test(diarieValue)) {
                            alert("Inget giltligt format för diarienummer! \n (ÅÅÅÅ.#)");
                            return false;
                        }

                        if ($form.find('[name="Beskrivning"]').val() == "") {
                            alert("Ärendemening saknas i fält Ärende!");
                            return false;
                        }
                    }
                },
                fields: {
                    Fid: {
                        key: true,
                        create: false,
                        edit: false,
                        list: false
                    },
                    Diarie: {
                        title: 'Diarie',
                        width: '1%'
                    },
                    Beskrivning: {
                        title: 'Ärende'
                    },
                    Notering: {
                        title: 'Kommentar'
                    },
                    Inkommit: {
                        title: 'Registrerat',
                        type: 'date',
                        displayFormat: 'yy-mm-dd ',
                        create: false // Vid nyskapande sätts tidsstämpeln av SYSDATE i sql-sats
                    },
                    LageskontrollBestallning: {
                        title: 'Beställning',
                        options: '/api/kartforandring/domainlageskontrollordering'
                    },
                    AdressOmr: {
                        title: 'Adressområde',
                        options: '/api/kartforandring/domainadressomrade',
                        list: false
                    },
                    Adress: {
                        title: 'Belägenhetsadress',
                        dependsOn: 'AdressOmr',
                        options: function (data) {
                            if (data.source == 'list') {
                                //Return url of all countries for optimization. 
                                //This method is called for each row on the table and jTable caches options based on this url.
                                return '/api/kartforandring/domainbelagenhetsadress?AdressOmrId=0';
                            }

                            //This code runs when user opens edit/create form or changes continental combobox on an edit/create form.
                            //data.source == 'edit' || data.source == 'create'
                            return '/api/kartforandring/domainbelagenhetsadress?AdressOmrId=' + data.dependedValues.AdressOmr;
                        },
                        list: false
                    },
                    Fastighet: {
                        title: 'Fastighet',
                        options: '/api/kartforandring/domainfastighet',
                        list: false
                    }

                    //Name: {
                    //    title: 'Name',
                    //    width: '23%'
                    //},
                    //EmailAddress: {
                    //    title: 'Email address',
                    //    list: false
                    //},
                    //Password: {
                    //    title: 'User Password',
                    //    type: 'password',
                    //    list: false
                    //},
                    //Gender: {
                    //    title: 'Gender',
                    //    width: '13%',
                    //    options: { 'M': 'Male', 'F': 'Female' }
                    //},
                    //CityId: {
                    //    title: 'City',
                    //    width: '12%',
                    //    options: '/PagingAndSorting.aspx/GetCityOptions'
                    //},
                    //BirthDate: {
                    //    title: 'Birth date',
                    //    width: '15%',
                    //    type: 'date',
                    //    displayFormat: 'yy-mm-dd'
                    //},
                    //Education: {
                    //    title: 'Education',
                    //    list: false,
                    //    type: 'radiobutton',
                    //    options: { '1': 'Primary school', '2': 'High school', '3': 'University' }
                    //},
                    //About: {
                    //    title: 'About this person',
                    //    type: 'textarea',
                    //    list: false
                    //},
                    //IsActive: {
                    //    title: 'Status',
                    //    width: '12%',
                    //    type: 'checkbox',
                    //    values: { 'false': 'Passive', 'true': 'Active' },
                    //    defaultValue: 'true'
                    //},
                    //RecordDate: {
                    //    title: 'Record date',
                    //    width: '15%',
                    //    type: 'date',
                    //    displayFormat: 'dd.mm.yy',
                    //    create: false,
                    //    edit: false,
                    //    sorting: false //This column is not sortable!
                    //}
                }
            });

            //Load student list from server
            $('#LageskontrollTableContainer').jtable('load');
        });

    </script>

</head>

<body>
    <form id="form1" runat="server">
        <div id="container">
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
