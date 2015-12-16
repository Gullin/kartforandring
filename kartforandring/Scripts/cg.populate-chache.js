/* Fel */
function cacheAllFelBygglovNoBygglovsbevakning(data, tdArray, callback) {
    var cacheId = 'AllFelBygglovNoBygglovsbevakningCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllFelBygglovNoBygglovsbevakning',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            // Om cachad data med samma id existerar - avbryt
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'fel';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllFelBygglovNoBygglovsbevakning");
        }
    });
}; // SLUT cacheAllFelBygglovNoBygglovsbevakning



function cacheAllFelBygglovsbevakningNoBygglov(data, tdArray, callback) {
    var cacheId = 'AllFelBygglovsbevakningNoBygglovCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllFelBygglovsbevakningNoBygglov',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            // Om cachad data med samma id existerar - avbryt
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'fel';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllFelBygglovsbevakningNoBygglov");
        }
    });
}; // SLUT cacheAllFelBygglovsbevakningNoBygglov



function cacheAllFelBestalldUtsattningNoBevakning(data, tdArray, callback) {
    var cacheId = 'AllFelBestalldUtsattningNoBevakningCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllFelBestalldUtsattningNoBevakning',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            // Om cachad data med samma id existerar - avbryt
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'fel';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllFelBestalldUtsattningNoBevakning");
        }
    });
}; // SLUT cacheAllFelBestalldUtsattningNoBevakning



function cacheAllFelBestalldLageskontrollNoBevakning(data, tdArray, callback) {
    var cacheId = 'AllFelBestalldLageskontrollNoBevakningCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllFelBestalldLageskontrollNoBevakning',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            // Om cachad data med samma id existerar - avbryt
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'fel';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllFelBestalldLageskontrollNoBevakning");
        }
    });
}; // SLUT cacheAllFelBestalldLageskontrollNoBevakning




/* Brister */
function cacheAllBristGeometryIsNull(data, tdArray, callback) {
    var cacheId = 'AllBristGeometryIsNullCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristGeometryIsNull',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            // Om cachad data med samma id existerar - avbryt
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristGeometryIsNull");
        }
    });
}; // SLUT cacheAllBristGeometryIsNull



function cacheAllBristUtforareIsNullWhenStarted(data, tdArray, callback) {
    var cacheId = 'AllBristUtforareIsNullWhenStartedCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristUtforareIsNullWhenStarted',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristUtforareIsNullWhenStarted");
        }
    });
}; // SLUT cacheAllBristUtforareIsNullWhenStarted



function cacheAllBristDiarieIsNull(data, tdArray, callback) {
    var cacheId = 'AllBristDiarieIsNullCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristDiarieIsNull',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristDiarieIsNull");
        }
    });
}; // SLUT cacheAllBristDiarieIsNull



function cacheAllBristDiarieWrongFormat(data, tdArray, callback) {
    var cacheId = 'AllBristDiarieWrongFormatCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristDiarieWrongFormat',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristDiarieWrongFormat");
        }
    });
}; // SLUT cacheAllBristDiarieWrongFormat



function cacheAllBristObjTypNotInherited(data, tdArray, callback) {
    var cacheId = 'AllBristObjTypNotInheritedCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristObjTypNotInherited',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristObjTypNotInherited");
        }
    });
}; // SLUT cacheAllBristObjTypNotInherited



function cacheAllBristObjIsNull(data, tdArray, callback) {
    var cacheId = 'AllBristObjIsNullCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristObjIsNull',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristObjIsNull");
        }
    });
}; // SLUT cacheAllBristObjIsNull



function cacheAllBristTypIsNull(data, tdArray, callback) {
    var cacheId = 'AllBristTypIsNullCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristTypIsNull',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristTypIsNull");
        }
    });
}; // SLUT cacheAllBristTypIsNull



function cacheAllBristBygglovMissingReason(data, tdArray, callback) {
    var cacheId = 'AllBristBygglovMissingReasonCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristBygglovMissingReason',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristBygglovMissingReason");
        }
    });
}; // SLUT cacheAllBristBygglovMissingReason



function cacheAllBristBygglovsbevakningarFinishNotDocumentedExecuted(data, tdArray, callback) {
    var cacheId = 'AllBristBygglovsbevakningarFinishNotDocumentedExecutedCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristBygglovsbevakningarFinishNotDocumentedExecuted',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristBygglovsbevakningarFinishNotDocumentedExecuted");
        }
    });
}; // SLUT cacheAllBristBygglovsbevakningarFinishNotDocumentedExecuted



function cacheAllBristBygglovRedundant(data, tdArray, callback) {
    var cacheId = 'AllBristBygglovRedundantCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllBristBygglovRedundant',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'brist';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllBristBygglovRedundant");
        }
    });
}; // SLUT cacheAllBristBygglovRedundant




/* Varningar */
function cacheAllVarningExternBestalldLageskontrollInTime(data, tdArray, callback) {
    var cacheId = 'AllVarningExternBestalldLageskontrollInTimeCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllVarningExternBestalldLageskontrollInTime',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'varning';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllVarningExternBestalldLageskontrollInTime");
        }
    });
}; // SLUT cacheAllVarningExternBestalldLageskontrollInTime



//TODO: Antal dagar som parameter från när kontroll skall göras ifrån
//function cacheAllVarningExternBestalldLageskontrollInTime(data, tdArray, antalDagar, callback) {
//    var cacheId = 'AllVarningExternBestalldLageskontrollInTimeCache';

//    var $tdImage, $tdNbrBrist;
//    $tdImage = tdArray[0];
//    $tdNbrBrist = tdArray[1];

//    $.ajax({
//        type: "GET",
//        url: baseUrl + 'api/brister/GetAllVarningExternBestalldLageskontrollInTime',
//        contentType: "application/json; charset=UTF-8",
//        dataType: "json",
//        data: antalDagar,
//        beforeSend: function () {
//            if (localCache.exist(cacheId)) {
//                return true;
//            }
//        },
//        success: function (response) {
//            data.type = 'varning';
//            data.posts = response;
//            localCache.set(cacheId, data, null);
//            callback(localCache);
//        },
//        error: function () {
//            toggleLoadingImage(false, $tdImage);
//            $tdNbrBrist.text(" - ");
//            $tdNbrBrist.parent().addClass('uncertain');
//            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
//            $tdImage.addClass('octicon octicon-question uncertain');
//            alert("Fel!\nGetAllVarningExternBestalldLageskontrollInTime");
//        }
//    });
//}; // SLUT cacheAllVarningExternBestalldLageskontrollInTime



function cacheAllVarningInternBestalldLageskontrollInTime(data, tdArray, callback) {
    var cacheId = 'AllVarningInternBestalldLageskontrollInTimeCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllVarningInternBestalldLageskontrollInTime',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'varning';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllVarningInternBestalldLageskontrollInTime");
        }
    });
}; // SLUT cacheAllVarningInternBestalldLageskontrollInTime



//TODO: Antal dagar som parameter från när kontroll skall göras ifrån
//function cacheAllVarningInternBestalldLageskontrollInTime(data, tdArray, antalDagar, callback) {
//    var cacheId = 'AllVarningInternBestalldLageskontrollInTimeCache';

//    var $tdImage, $tdNbrBrist;
//    $tdImage = tdArray[0];
//    $tdNbrBrist = tdArray[1];

//    $.ajax({
//        type: "GET",
//        url: baseUrl + 'api/brister/GetAllVarningInternBestalldLageskontrollInTime',
//        contentType: "application/json; charset=UTF-8",
//        dataType: "json",
//        data: antalDagar,
//        beforeSend: function () {
//            if (localCache.exist(cacheId)) {
//                return true;
//            }
//        },
//        success: function (response) {
//            data.type = 'varning';
//            data.posts = response;
//            localCache.set(cacheId, data, null);
//            callback(localCache);
//        },
//        error: function () {
//            toggleLoadingImage(false, $tdImage);
//            $tdNbrBrist.text(" - ");
//            $tdNbrBrist.parent().addClass('uncertain');
//            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
//            $tdImage.addClass('octicon octicon-question uncertain');
//            alert("Fel!\nGetAllVarningInternBestalldLageskontrollInTime");
//        }
//    });
//}; // SLUT cacheAllVarningInternBestalldLageskontrollInTime



function cacheAllVarningUtsattningWithLageskontrollUtfard(data, tdArray, callback) {
    var cacheId = 'AllVarningUtsattningWithLageskontrollUtfardCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllVarningUtsattningWithLageskontrollUtfard',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'varning';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllVarningUtsattningWithLageskontrollUtfard");
        }
    });
}; // SLUT cacheAllVarningUtsattningWithLageskontrollUtfard



function cacheAllVarningLageskontrollUtfardAndamalNotUtfard(data, tdArray, callback) {
    var cacheId = 'AllVarningLageskontrollUtfardAndamalNotUtfardCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllVarningLageskontrollUtfardAndamalNotUtfard',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'varning';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllVarningLageskontrollUtfardAndamalNotUtfard");
        }
    });
}; // SLUT cacheAllVarningLageskontrollUtfardAndamalNotUtfard



function cacheAllVarningLogisktRaderade(data, tdArray, callback) {
    var cacheId = 'AllVarningLogisktRaderadeCache';

    var $tdImage, $tdNbrBrist;
    $tdImage = tdArray[0];
    $tdNbrBrist = tdArray[1];

    $.ajax({
        type: "GET",
        url: baseUrl + 'api/brister/GetAllVarningLogisktRaderade',
        contentType: "application/json; charset=UTF-8",
        dataType: "json",
        beforeSend: function () {
            if (localCache.exist(cacheId)) {
                return true;
            }
        },
        success: function (response) {
            data.type = 'varning';
            data.posts = response;
            localCache.set(cacheId, data, null);
            callback(localCache);
        },
        error: function () {
            toggleLoadingImage(false, $tdImage);
            $tdNbrBrist.text(" - ");
            $tdNbrBrist.parent().addClass('uncertain');
            $tdNbrBrist.parent().prop('title', 'Kunde ej kommunicera med webbtjänst');
            $tdImage.addClass('octicon octicon-question uncertain');
            alert("Fel!\nGetAllVarningLogisktRaderade");
        }
    });
}; // SLUT cacheAllVarningLogisktRaderade