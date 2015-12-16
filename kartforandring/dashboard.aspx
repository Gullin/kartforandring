<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="dashboard.aspx.cs" Inherits="kartforandring.dashboard" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Kartförändringar | Kontrollpanel</title>

    <link href="Styles/page-UI-centering.css" rel="stylesheet" />
    <link href="Styles/page-UI-core.css" rel="stylesheet" />
    <link href="Styles/jquery-ui-1.9.2.custom/css/smoothness/jquery-ui-1.9.2.custom.min.css" rel="stylesheet" />
    <link href="Styles/octicons-3.1.0/octicons.css" rel="stylesheet" />

    <!-- Måste komma efter jQuery-UI-CSS //-->
    <link href="Styles/cg.custom.jquery-ui.css" rel="stylesheet" />
    <link href="Styles/page-UI-dashboard.css" rel="stylesheet" />

    <style>
    </style>

    <!-- Settings -->
    <script type="text/javascript">
        var baseUrl = '<%=ResolveClientUrl("~")%>';

        // Lokal Cache av statistik, fel, brister, varningar
        // Skapas som global variabel för sessioner
        //http://stackoverflow.com/questions/17104265/caching-a-jquery-ajax-response-in-javascript-browser
        var localCache = {
            data: {
                caption: null,
                type: null,
                posts: null
            },
            remove: function (id) {
                delete localCache.data[id];
            },
            exist: function (id) {
                return localCache.data.hasOwnProperty(id) && localCache.data[id] !== null;
            },
            get: function (id) {
                return localCache.data[id];
            },
            set: function (id, cachedData, callback) {
                localCache.remove(id);
                localCache.data[id] = cachedData;
                if ($.isFunction(callback)) callback(cachedData);
            }
        };
    </script>

    <script src='<%=ResolveClientUrl("Scripts/jquery-1.9.1.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/jquery-ui-1.9.2.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/spin.js/spin-2.3.2.min.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/spin.js/jquery.spin.js")%>'></script>

    <script src='<%=ResolveClientUrl("Scripts/cg.dashboard.js")%>'></script>
    <script src='<%=ResolveClientUrl("Scripts/cg.populate-chache.js")%>'></script>

    <script type="text/javascript">

        $(function () {
            $("#tabs").tabs();
        });

        function appendVerifieringIcon($iconCarrier, $verifingPart) {
            $verifingPart.prepend($iconCarrier);
        };

        $(document).ready(function () {
            $('body').spin('large');


            var timeoutTimeMs = 1000;
            var nbrInformationToWaitFor = 19;
            var nbrInformationObtained = 0;

            /* Fel */
            // Behållare för ikon
            var $felImg = $('<span>');
            $felImg.addClass('octicon octicon-stop iconSetting');
            var $felImgContainer = $('#felIcon');

            // Referens till DOM-delen för Verifieringar
            var $verifiering = $('#fel');
            // Behållare för de olika bristerna
            var $felContainer = $('<div>');
            $felContainer.addClass('verifingPart');
            var $felTable = $('<table>');
            $felContainer.append($felTable);
            $verifiering.append($felContainer);


            var bygglovNoBygglovsbevakningData = {
                caption: 'Bygglov utan något att bevaka',
            }
            var tdArrayBygglovNoBygglovsbevakning;
            putVerifyPost($felTable, bygglovNoBygglovsbevakningData.caption, function (array) {
                tdArrayBygglovNoBygglovsbevakning = array;
            });
            cacheAllFelBygglovNoBygglovsbevakning(bygglovNoBygglovsbevakningData, tdArrayBygglovNoBygglovsbevakning, function (localCache) {
                if (localCache.exist('AllFelBygglovNoBygglovsbevakningCache')) {
                    setTimeout(function () {
                        setAllFelBygglovNoBygglovsbevakning(localCache, tdArrayBygglovNoBygglovsbevakning, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($felImg, $felImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor)
                            {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var bygglovsbevakningNoBygglovData = {
                caption: 'Bygglovsbevakning utan att det är bygglov'
            }
            var tdArrayBygglovsbevakningNoBygglov;
            putVerifyPost($felTable, bygglovsbevakningNoBygglovData.caption, function (array) {
                tdArrayBygglovsbevakningNoBygglov = array;
            });
            cacheAllFelBygglovsbevakningNoBygglov(bygglovsbevakningNoBygglovData, tdArrayBygglovsbevakningNoBygglov, function (localCache) {
                if (localCache.exist('AllFelBygglovsbevakningNoBygglovCache')) {
                    setTimeout(function () {
                        setAllFelBygglovsbevakningNoBygglov(localCache, tdArrayBygglovsbevakningNoBygglov, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($felImg, $felImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var bestalldUtsattningNoBevakningData = {
                caption: 'Utsättning beställd, saknar krav eller ev. utförd'
            }
            var tdArrayBestalldUtsattningNoBevakning;
            putVerifyPost($felTable, bestalldUtsattningNoBevakningData.caption, function (array) {
                tdArrayBestalldUtsattningNoBevakning = array;
            });
            cacheAllFelBestalldUtsattningNoBevakning(bestalldUtsattningNoBevakningData, tdArrayBestalldUtsattningNoBevakning, function (localCache) {
                if (localCache.exist('AllFelBestalldUtsattningNoBevakningCache')) {
                    setTimeout(function () {
                        setAllFelBestalldUtsattningNoBevakning(localCache, tdArrayBestalldUtsattningNoBevakning, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($felImg, $felImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var bestalldLageskontrollNoBevakningData = {
                caption: 'Lägeskontroll beställd, saknar krav eller ev. utförd'
            }
            var tdArrayBestalldLageskontrollNoBevakning;
            putVerifyPost($felTable, bestalldLageskontrollNoBevakningData.caption, function (array) {
                tdArrayBestalldLageskontrollNoBevakning = array;
            });
            cacheAllFelBestalldLageskontrollNoBevakning(bestalldLageskontrollNoBevakningData, tdArrayBestalldLageskontrollNoBevakning, function (localCache) {
                if (localCache.exist('AllFelBestalldLageskontrollNoBevakningCache')) {
                    setTimeout(function () {
                        setAllFelBestalldLageskontrollNoBevakning(localCache, tdArrayBestalldLageskontrollNoBevakning, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($felImg, $felImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });



            /* Brister */
            // Behållare för ikon
            var $bristImg = $('<span>');
            $bristImg.addClass('octicon octicon-bug iconSetting');
            var $bristImgContainer = $('#bristIcon');

            // Referens till DOM-delen för brister
            var $verifiering = $('#brister');
            // Behållare för de olika bristerna
            var $bristsContainer = $('<div>');
            $bristsContainer.addClass('verifingPart');
            var $bristTable = $('<table>');
            $bristsContainer.append($bristTable);
            $verifiering.append($bristsContainer);

            
            var geometryIsNullData = {
                caption: 'Geometri saknas'
            }
            var tdArrayGeometryIsNull;
            putVerifyPost($bristTable, geometryIsNullData.caption, function (array) {
                tdArrayGeometryIsNull = array;
            });
            cacheAllBristGeometryIsNull(geometryIsNullData, tdArrayGeometryIsNull, function (localCache) {
                if (localCache.exist('AllBristGeometryIsNullCache')) {
                    setTimeout(function () {
                        setAllBristGeometryIsNull(localCache, tdArrayGeometryIsNull, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });
            

            var utforareIsNullWhenStartedData = {
                caption: 'Påbörjad utförare saknas'
            }
            var tdArrayUtforareIsNullWhenStarted;
            putVerifyPost($bristTable, utforareIsNullWhenStartedData.caption, function (array) {
                tdArrayUtforareIsNullWhenStarted = array;
            });
            cacheAllBristUtforareIsNullWhenStarted(utforareIsNullWhenStartedData, tdArrayUtforareIsNullWhenStarted, function (localCache) {
                if (localCache.exist('AllBristUtforareIsNullWhenStartedCache')) {
                    setTimeout(function () {
                        setAllBristUtforareIsNullWhenStarted(localCache, tdArrayUtforareIsNullWhenStarted, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var diarieIsNullData = {
                caption: 'Diarienummer saknas'
            }
            var tdArrayDiarieIsNull;
            putVerifyPost($bristTable, diarieIsNullData.caption, function (array) {
                tdArrayDiarieIsNull = array;
            });
            cacheAllBristDiarieIsNull(diarieIsNullData, tdArrayDiarieIsNull, function (localCache) {
                if (localCache.exist('AllBristDiarieIsNullCache')) {
                    setTimeout(function () {
                        setAllBristDiarieIsNull(localCache, tdArrayDiarieIsNull, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var diarieWrongFormatData = {
                caption: 'Diarienummer fel format'
            }
            var tdArrayDiarieWrongFormat;
            putVerifyPost($bristTable, diarieWrongFormatData.caption, function (array) {
                tdArrayDiarieWrongFormat = array;
            });
            cacheAllBristDiarieWrongFormat(diarieWrongFormatData, tdArrayDiarieWrongFormat, function (localCache) {
                if (localCache.exist('AllBristDiarieWrongFormatCache')) {
                    setTimeout(function () {
                        setAllBristDiarieWrongFormat(localCache, tdArrayDiarieWrongFormat, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var objTypNotInheritedData = {
                caption: 'Obj-/typkod ej gemensam högsta signifikanta siffra'
            }
            var tdArrayObjTypNotInherited;
            putVerifyPost($bristTable, objTypNotInheritedData.caption, function (array) {
                tdArrayObjTypNotInherited = array;
            });
            cacheAllBristObjTypNotInherited(objTypNotInheritedData, tdArrayObjTypNotInherited, function (localCache) {
                if (localCache.exist('AllBristObjTypNotInheritedCache')) {
                    setTimeout(function () {
                        setAllBristObjTypNotInherited(localCache, tdArrayObjTypNotInherited, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var objIsNullData = {
                caption: 'Objektkod saknas'
            }
            var tdArrayObjIsNull;
            putVerifyPost($bristTable, objIsNullData.caption, function (array) {
                tdArrayObjIsNull = array;
            });
            cacheAllBristObjIsNull(objIsNullData, tdArrayObjIsNull, function (localCache) {
                if (localCache.exist('AllBristObjIsNullCache')) {
                    setTimeout(function () {
                        setAllBristObjIsNull(localCache, tdArrayObjIsNull, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var typIsNullData = {
                caption: 'Typkod saknas'
            }
            var tdArrayTypIsNull;
            putVerifyPost($bristTable, typIsNullData.caption, function (array) {
                tdArrayTypIsNull = array;
            });
            cacheAllBristTypIsNull(typIsNullData, tdArrayTypIsNull, function (localCache) {
                if (localCache.exist('AllBristTypIsNullCache')) {
                    setTimeout(function () {
                        setAllBristTypIsNull(localCache, tdArrayTypIsNull, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var bygglovMissingReasonData = {
                caption: 'Bygglov saknar anledning till bevakning'
            }
            var tdArrayBygglovMissingReason;
            putVerifyPost($bristTable, bygglovMissingReasonData.caption, function (array) {
                tdArrayBygglovMissingReason = array;
            });
            cacheAllBristBygglovMissingReason(bygglovMissingReasonData, tdArrayBygglovMissingReason, function (localCache) {
                if (localCache.exist('AllBristBygglovMissingReasonCache')) {
                    setTimeout(function () {
                        setAllBristBygglovMissingReason(localCache, tdArrayBygglovMissingReason, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var bygglovsbevakningarFinishNotDocumentedExecutedData = {
                caption: 'Bygglov utförd, ej avslutad'
            }
            var tdArrayBygglovsbevakningarFinishNotDocumentedExecuted;
            putVerifyPost($bristTable, bygglovsbevakningarFinishNotDocumentedExecutedData.caption, function (array) {
                tdArrayBygglovsbevakningarFinishNotDocumentedExecuted = array;
            });
            cacheAllBristBygglovsbevakningarFinishNotDocumentedExecuted(bygglovsbevakningarFinishNotDocumentedExecutedData, tdArrayBygglovsbevakningarFinishNotDocumentedExecuted, function (localCache) {
                if (localCache.exist('AllBristBygglovsbevakningarFinishNotDocumentedExecutedCache')) {
                    setTimeout(function () {
                        setAllBristBygglovsbevakningarFinishNotDocumentedExecuted(localCache, tdArrayBygglovsbevakningarFinishNotDocumentedExecuted, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var bygglovRedundantData = {
                caption: 'Flera förekomster av samma diarie'
            }
            var tdArrayBygglovRedundant;
            putVerifyPost($bristTable, bygglovRedundantData.caption, function (array) {
                tdArrayBygglovRedundant = array;
            });
            cacheAllBristBygglovRedundant(bygglovRedundantData, tdArrayBygglovRedundant, function (localCache) {
                if (localCache.exist('AllBristBygglovRedundantCache')) {
                    setTimeout(function () {
                        setAllBristBygglovRedundant(localCache, tdArrayBygglovRedundant, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($bristImg, $bristImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });



            /* Varningar */
            // Behållare för ikon
            var $varningImg = $('<span>');
            $varningImg.addClass('octicon octicon-alert iconSetting');
            var $varningImgContainer = $('#varningIcon');

            // Referens till DOM-delen för varningar
            var $verifiering = $('#varningar');
            // Behållare för de olika varningarna
            var $varningContainer = $('<div>');
            $varningContainer.addClass('verifingPart');
            var $varningTable = $('<table>');
            $varningContainer.append($varningTable);
            $verifiering.append($varningContainer);


            var externBestalldLageskontrollInTimeData = {
                caption: 'Externt beställd lägeskontroll ej utförd inom måltid'
            }
            var tdArrayExternBestalldLageskontrollInTime;
            putVerifyPost($varningTable, externBestalldLageskontrollInTimeData.caption, function (array) {
                tdArrayExternBestalldLageskontrollInTime = array;
            });
            cacheAllVarningExternBestalldLageskontrollInTime(externBestalldLageskontrollInTimeData, tdArrayExternBestalldLageskontrollInTime, function (localCache) {
                if (localCache.exist('AllVarningExternBestalldLageskontrollInTimeCache')) {
                    setTimeout(function () {
                        setAllVarningExternBestalldLageskontrollInTime(localCache, tdArrayExternBestalldLageskontrollInTime, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($varningImg, $varningImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var internBestalldLageskontrollInTimeData = {
                caption: 'Internt beställd lägeskontroll ej utförd inom måltid'
            }
            var tdArrayInternBestalldLageskontrollInTime;
            putVerifyPost($varningTable, internBestalldLageskontrollInTimeData.caption, function (array) {
                tdArrayInternBestalldLageskontrollInTime = array;
            });
            cacheAllVarningInternBestalldLageskontrollInTime(internBestalldLageskontrollInTimeData, tdArrayInternBestalldLageskontrollInTime, function (localCache) {
                if (localCache.exist('AllVarningInternBestalldLageskontrollInTimeCache')) {
                    setTimeout(function () {
                        setAllVarningInternBestalldLageskontrollInTime(localCache, tdArrayInternBestalldLageskontrollInTime, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($varningImg, $varningImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var utsattningWithLageskontrollUtfardData = {
                caption: 'Utsättning ej uförd men lägeskontroll utförd'
            }
            var tdArrayUtsattningWithLageskontrollUtfard;
            putVerifyPost($varningTable, utsattningWithLageskontrollUtfardData.caption, function (array) {
                tdArrayUtsattningWithLageskontrollUtfard = array;
            });
            cacheAllVarningUtsattningWithLageskontrollUtfard(utsattningWithLageskontrollUtfardData, tdArrayUtsattningWithLageskontrollUtfard, function (localCache) {
                if (localCache.exist('AllVarningUtsattningWithLageskontrollUtfardCache')) {
                    setTimeout(function () {
                        setAllVarningUtsattningWithLageskontrollUtfard(localCache, tdArrayUtsattningWithLageskontrollUtfard, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($varningImg, $varningImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var lageskontrollUtfardAndamalNotUtfardData = {
                caption: 'Lägeskontroll utförd, ej ändamål satt'
            }
            var tdArrayLageskontrollUtfardAndamalNotUtfard;
            putVerifyPost($varningTable, lageskontrollUtfardAndamalNotUtfardData.caption, function (array) {
                tdArrayLageskontrollUtfardAndamalNotUtfard = array;
            });
            cacheAllVarningLageskontrollUtfardAndamalNotUtfard(lageskontrollUtfardAndamalNotUtfardData, tdArrayLageskontrollUtfardAndamalNotUtfard, function (localCache) {
                if (localCache.exist('AllVarningLageskontrollUtfardAndamalNotUtfardCache')) {
                    setTimeout(function () {
                        setAllVarningLageskontrollUtfardAndamalNotUtfard(localCache, tdArrayLageskontrollUtfardAndamalNotUtfard, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($varningImg, $varningImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            var logisktRaderadeData = {
                caption: 'Logiskt raderade poster (från beställningssajt)'
            }
            var tdArrayLogisktRaderade;
            putVerifyPost($varningTable, logisktRaderadeData.caption, function (array) {
                tdArrayLogisktRaderade = array;
            });
            cacheAllVarningLogisktRaderade(logisktRaderadeData, tdArrayLogisktRaderade, function (localCache) {
                if (localCache.exist('AllVarningLogisktRaderadeCache')) {
                    setTimeout(function () {
                        setAllVarningLogisktRaderade(localCache, tdArrayLogisktRaderade, function (result) {
                            if (result == 1) {
                                appendVerifieringIcon($varningImg, $varningImgContainer);
                            }
                            nbrInformationObtained++;
                            if (nbrInformationObtained >= nbrInformationToWaitFor) {
                                $('body').spin(false);
                            }
                        });
                    }, timeoutTimeMs);
                }
            });


            console.log(localCache);

        });

    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="container">
        <!--  //-->
        <div id="tabs">
            <ul>
                <li><a href="#oversikt">Översikt</a></li>
                <li><a href="#kartforandringar">Kartförändringar</a></li>
                <li><a href="#tabs-3">Inställningar</a></li>
            </ul>


            <div id="oversikt">
                <div class="dashboardPart">
                    <span class="dashboardPartHeader">Ärenden</span>
                    <div>INNEHÅLL</div>
                </div>
                <div class="dashboardPart">
                    <span class="dashboardPartHeader">Verifiering</span>
                    <div></div>
                </div>
            </div>

            <div id="kartforandringar">
                <div class="dashboardPart">
                    <span class="dashboardPartHeader">Fel</span>
                    <span id="felIcon" class="dashboardPartIcon"></span>
                    <div id="fel"></div>
                </div>
                <div class="dashboardPart">
                    <span class="dashboardPartHeader">Brister</span>
                    <span id="bristIcon" class="dashboardPartIcon"></span>
                    <div id="brister"></div>
                </div>
                <div class="dashboardPart">
                    <span class="dashboardPartHeader">Varningar</span>
                    <span id="varningIcon" class="dashboardPartIcon"></span>
                    <div id="varningar"></div>
                </div>
            </div>

            <div id="tabs-3">
                <div class="dashboardPart">
                    <span class="dashboardPartHeader">Inställningar</span>
                    <div></div>
                </div>
            </div>
        </div>

    </div>
    </form>
</body>
</html>
