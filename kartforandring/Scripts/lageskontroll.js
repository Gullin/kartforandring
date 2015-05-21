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
            createAction: function (postData) {
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
            updateAction: function (postData) {
                return $.Deferred(function ($dfd) {
                    $.ajax({
                        url: '/api/kartforandring/uppdateralageskontroll',
                        type: 'POST',
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
            var $form = data.form;
            $form.find('[name="Diarie"]').attr('placeholder', 'ÅÅÅÅ.#');

            var $spanRequire = $('<span>');
            $spanRequire.css({
                "color": "red",
                "font-weight": "bold",
                "margin": "0px 1em",
                "cursor": "help"
            });
            $spanRequire.attr("title", "Värde krävs!");
            $spanRequire.text('*');

            var diarieValue = $form.find('[name="Diarie"]');
            var arendeValue = $form.find('[name="Beskrivning"]');
            diarieValue.after($spanRequire.clone());
            arendeValue.after($spanRequire.clone());
        },
        formSubmitting: function (event, data) {
            if (data.formType == "create") {
                var $form = data.form;

                var diarieValue = $form.find('[name="Diarie"]').val();
                var arendeValue = $form.find('[name="Beskrivning"]').val();
                var bestallningValue = $form.find('[name="LageskontrollBestallning"]').val();

                var diariePattern = /^(19\d{2}\.\d{1,4})$|^(20\d{2}\.\d{1,4})$/;
                if (!diariePattern.test(diarieValue)) {
                    alert("Inget giltligt format för diarienummer! \n (ÅÅÅÅ.#)");
                    return false;
                }

                if (arendeValue == "") {
                    alert("Ärendemening saknas i fält Ärende!");
                    return false;
                }

                if (bestallningValue == "") {
                    alert("Beställningsinitierar saknas i fältet Beställning!");
                    return false;
                }
            }
        },
        recordAdded: function (event, data) {
            $('#LageskontrollTableContainer').jtable('reload');
        },
        fields: {
            Fid: {
                key: true,
                create: false,
                edit: false,
                list: false
            },
            LevelOfPosition: {
                width: '1%',
                sorting: false,
                display: function (post) {
                    var imgSuffix = "";
                    var title = "";
                    var levelOfPositionInText = [" * Platsbestämd i text",
                                                 " * Adressområde",
                                                 " * Fastighet",
                                                 " * Adressplats",
                                                 " * Egen geometri"];
                    levelOfPositionIdx = 0;
                    $.each(post.record.LevelOfPosition, function (i, val) {
                        imgSuffix += val;
                        if (val == 1) {
                            title += "\n" + levelOfPositionInText[levelOfPositionIdx];
                        }
                        levelOfPositionIdx++;
                    });
                    if (title != "") {
                        title = "Lägesbunden genom\n" + title;
                    } else {
                        title = "Ingen lägesbestämning";
                    }
                    var $img = $("<img>");
                    $img.attr("src", "pic/positionNeedle/positionNeedle_" + imgSuffix + ".png");
                    $img.attr("title", title);
                    return $img;
                },
                create: false,
                edit: false
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
        }
    });

    //Load student list from server
    $('#LageskontrollTableContainer').jtable('load');
});