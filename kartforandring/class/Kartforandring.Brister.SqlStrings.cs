using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kartforandring.Brister
{

    /*
Bygglov	        Varning		Externt beställda lägeskontroller bör noteras som utförda inom en ganska snäv tid
Bygglov	        Varning		Internt beställda lägeskontroller bör noteras som utförda inom en tid
Bygglov	        Varning		Utsättning, existerar krav och lägeskontroll är utförd ska möjligen status ändras för utsättningskravet
Bygglov	        Varning		Ändamål, lägeskontroll utförd bör ändamål kunna sättas och status ändras till utförd
Övergripande	Varning		Logiskt raderad, kontrollera anledning och radera ev. fysisikt eller ta bort dubblett
Övergripande	Brist		Utförare saknas vid dokumenterat påbörjad
Övergripande	Brist		Geometri saknas
Bygglov	        Brist		Bygglovsdiarie saknas
Bygglov	        Brist		Bygglovsdiarie, fel format
Övergripande	Brist		Obj-/typkod strider mot gemensam högsta signifikanta siffra
Övergripande	Brist		Objektkod saknas
Övergripande	Brist		Typkod saknas
Bygglov	        Brist		Bygglov saknar anledning till bevakning
Bygglov	        Brist		Avslutat, bygglovsbevakningar utförda men ej dokumenterad som avslutad bevakning
Bygglov	        Brist		Bygglovsdiarie, flera förekomster
Övergripande	Status		Förändringar sedan ett visst datum
Övergripande	Status		Antal av varje förändring
Övergripande	Status		Antal av varje typ
Övergripande	Status		Totalt antal
Övergripande	Status		Antal aktiva/ej aktiva
Övergripande	Fel		    Bygglov men ingen bygglovsbevakning
Övergripande	Fel		    Bygglovsbevakning men inget bygglov
Bygglov	        Fel		    Utsättning, om beställning existerar ska bevakning finnas eller vara utförd
Bygglov	        Fel		    Lägeskontroll, om beställning existerar ska bevakning finnas eller vara utförd
    */
    internal static class SqlBristStrings
    {
        #region SQL-satser
        private static string _selectFid = "SELECT fid AS digit FROM kar_samling ";
        #region Brist
        // Geometri saknas
        private static string _geometryIsNull = _selectFid + "WHERE  geom IS NULL";
        // Utförare saknas vid dokumenterat påbörjad
        private static string _utforareIsNullWhenStarted = _selectFid + "WHERE bev_paborjat IS NOT NULL AND bev_utforare IS NULL";
        // Bygglovsdiarie saknas
        private static string _diarieIsNull = _selectFid + "WHERE kar_typ = 1020 AND bev_bygglov_diarie IS NULL";
        // Bygglovsdiarie, fel format
        private static string _diarieWrongFormat = _selectFid + @"WHERE NOT regexp_like(bev_bygglov_diarie, '^(19\d{2}\.\d{1,4})$|^(20\d{2}\.\d{1,4})$')";
        // Obj-/typkod strider mot gemensam högsta värdesiffra
        private static string _objTypNotInherited = _selectFid + "WHERE SUBSTR(kar_obj, 1, 1) <> SUBSTR(kar_typ, 1, 1)";
        // Objektkod saknas
        private static string _objIsNull = _selectFid + "WHERE kar_obj IS NULL";
        // Typkod saknas
        private static string _typIsNull = _selectFid + "WHERE kar_typ IS NULL";
        // Bygglov saknar anledning till bevakning
        private static string _bygglovMissingReason = _selectFid + "WHERE (kar_typ = 1020 OR bev_bygglov_diarie IS NOT NULL) " +
                                                                   "AND   (bev_bygglov_uts IS NULL AND bev_bygglov_lag IS NULL " +
                                                                   "AND    bev_bygglov_attefall IS NULL AND bev_bygglov_riv IS NULL " +
                                                                   "AND    bev_bygglov_andamal IS NULL)";
        // Avslutat, bygglovsbevakningar utförda men ej dokumenterad som avslutad bevakning
        private static string _bygglovsbevakningarFinishNotDocumentedExecuted = _selectFid + "WHERE  kar_typ = 1020 AND bev_avslutat IS NULL " +
                                                                                             "AND   (bev_bygglov_uts IS NULL OR bev_bygglov_uts = 20) " +
                                                                                             "AND   (bev_bygglov_lag IS NULL OR bev_bygglov_lag = 20) " +
                                                                                             "AND   (bev_bygglov_attefall IS NULL OR bev_bygglov_attefall = 20) " +
                                                                                             "AND   (bev_bygglov_riv IS NULL OR bev_bygglov_riv = 20) " +
                                                                                             "AND   (bev_bygglov_andamal IS NULL OR bev_bygglov_andamal = 20)";
        // Bygglovsdiarie, fler än en
        private static string _bygglovRedundant = "SELECT  f.fid digit, f.bev_bygglov_diarie VALUE_TEXT " +
                                                  "FROM    kar_samling f, " +
                                                  "       (SELECT bev_bygglov_diarie FROM kar_samling GROUP BY bev_bygglov_diarie HAVING COUNT(bev_bygglov_diarie) > 1) err " +
                                                  "WHERE   f.bev_bygglov_diarie = err.bev_bygglov_diarie";
        #endregion
        #region Varning
        /*VARNING*/
        // Externt beställda lägeskontroller bör noteras som utförda inom en ganska snäv tid
        private static string _externBestalldLageskontrollInTime = _selectFid + "WHERE kar_typ = 1020 AND bev_avslutat IS NULL AND bev_bygglov_lag_best = 20";
        // Internt beställda lägeskontroller bör noteras som utförda inom en tid
        private static string _internBestalldLageskontrollInTime = _selectFid + "WHERE kar_typ = 1020 AND bev_avslutat IS NULL AND bev_bygglov_lag_best = 10";
        // Utsättning, existerar krav och lägeskontroll är utförd ska möjligen status ändras för utsättningskravet
        private static string _utsattningWithLageskontrollUtfard = _selectFid + "WHERE kar_typ = 1020 AND bev_bygglov_lag = 20 AND bev_bygglov_uts = 10";
        // Ändamål, lägeskontroll utförd bör ändamål kunna sättas och status ändras till utförd
        private static string _lageskontrollUtfardAndamalNotUtfard = _selectFid + "WHERE kar_typ = 1020 AND bev_bygglov_lag = 20 AND bev_bygglov_andamal = 10";
        // Logiskt raderad, kontrollera anledning och radera ev. fysisikt eller ta bort dubblett
        private static string _logisktRaderade = _selectFid + "WHERE deleted = 1";
        #endregion
        #region Status
        /*STATUS*/
        // Förändringar sedan ett visst datum
        private static string _changedSince = _selectFid + "WHERE";
        // Antal av varje förändring
        private static string _nbrOfChangesObjkod = "SELECT kar_obj obj_kod, value obj_value, COUNT(*) digit " +
                                                    "FROM   kar_samling, " + 
                                                    "       kar_obj_domain_tbd " +
                                                    "WHERE  kar_samling.kar_obj  = kar_obj_domain_tbd.id(+) " +
                                                    "{0} " +
                                                    "GROUP BY kar_obj, value" +
                                                    "ORDER BY kar_obj, value";
        // Antal av varje typ
        private static string _nbrOfChangesTypkod = "SELECT kar_obj obj_kod, obj.value obj_value, kar_typ typ_kod, typ.value typ_value, COUNT(*) digit " +
                                                    "FROM   kar_samling, " +
                                                    "       kar_obj_domain_tbd obj, " +
                                                    "       kar_typ_domain_tbd typ " +
                                                    "WHERE  kar_samling.kar_obj  = kar_obj_domain_tbd.id(+) " +
                                                    "{0} " +
                                                    "GROUP BY kar_obj, value" +
                                                    "ORDER BY kar_obj, value";
        // Totalt antal
        private static string _nbrTotal = "SELECT COUNT(*) digit FROM kar_samling{0}";
        // Antal aktiva/ej aktiva
        private static string _nbrAktiva = "SELECT NVL2(bev_avslutat, 'Ja', 'Nej') VALUE_TEXT, COUNT(*) digit FROM kar_samling GROUP BY NVL2(bev_avslutat, 'Ja', 'Nej'){0}";
        #endregion
        #region Fel
        /*FEL*/
        // 
        private static string _bygglovNoBygglovsbevakning = _selectFid + "WHERE kar_typ = 1020 " +
                                                                         "AND   bev_bygglov_uts IS NULL " +
                                                                         "AND   bev_bygglov_lag IS NULL " +
                                                                         "AND   bev_bygglov_attefall IS NULL " +
                                                                         "AND   bev_bygglov_riv IS NULL " +
                                                                         "AND   bev_bygglov_andamal IS NULL";
        // 
        private static string _bygglovsbevakningNoBygglov = _selectFid + "WHERE (kar_typ <> 1020 " +
                                                                         "OR     kar_typ IS NULL) " +
                                                                         "AND   (bev_bygglov_uts IS NOT NULL " +
                                                                         "OR    bev_bygglov_lag IS NOT NULL " +
                                                                         "OR    bev_bygglov_attefall IS NOT NULL " +
                                                                         "OR    bev_bygglov_riv IS NOT NULL " +
                                                                         "OR    bev_bygglov_andamal IS NOT NULL)";
        // Utsättning, om beställning existerar ska bevakning finnas eller vara utförd
        private static string _bestalldUtsattningNoBevakning = _selectFid + "WHERE kar_typ = 1020 AND bev_avslutat IS NULL AND bev_bygglov_uts_best IN (10,20) AND bev_bygglov_uts IS NULL";
        // Lägeskontroll, om beställning existerar ska bevakning finnas eller vara utförd
        private static string _bestalldLageskontrollNoBevakning = _selectFid + "WHERE kar_typ = 1020 AND bev_avslutat IS NULL AND bev_bygglov_lag_best IN (10,20) AND bev_bygglov_lag IS NULL";
        #endregion
        #endregion



        #region Brist
        internal static string geometryIsNull()
        {
            return _geometryIsNull;
        }
        internal static string geometryIsNull(string filter)
        {
            return _geometryIsNull + " AND " + filter;
        }


        internal static string utforareIsNullWhenStarted()
        {
            return _utforareIsNullWhenStarted;
        }
        internal static string utforareIsNullWhenStarted(string filter)
        {
            return _utforareIsNullWhenStarted + " AND " + filter;
        }


        internal static string diarieIsNull()
        {
            return _diarieIsNull;
        }
        internal static string diarieIsNull(string filter)
        {
            return _diarieIsNull + " AND " + filter;
        }


        internal static string diarieWrongFormat()
        {
            return _diarieWrongFormat;
        }
        internal static string diarieWrongFormat(string filter)
        {
            return _diarieWrongFormat + " AND " + filter;
        }


        internal static string objTypNotInherited()
        {
            return _objTypNotInherited;
        }
        internal static string objTypNotInherited(string filter)
        {
            return _objTypNotInherited + " AND " + filter;
        }


        internal static string objIsNull()
        {
            return _objIsNull;
        }
        internal static string objIsNull(string filter)
        {
            return _objIsNull + " AND " + filter;
        }


        internal static string typIsNull()
        {
            return _typIsNull;
        }
        internal static string typIsNull(string filter)
        {
            return _typIsNull + " AND " + filter;
        }


        internal static string bygglovMissingReason()
        {
            return _bygglovMissingReason;
        }
        internal static string bygglovMissingReason(string filter)
        {
            return _bygglovMissingReason + " AND " + filter;
        }


        internal static string bygglovsbevakningarFinishNotDocumentedExecuted()
        {
            return _bygglovsbevakningarFinishNotDocumentedExecuted;
        }
        internal static string bygglovsbevakningarFinishNotDocumentedExecuted(string filter)
        {
            return _bygglovsbevakningarFinishNotDocumentedExecuted + " AND " + filter;
        }


        internal static string bygglovRedundant()
        {
            return _bygglovRedundant;
        }
        internal static string bygglovRedundant(string filter)
        {
            return _bygglovRedundant + " AND " + filter;
        }
        #endregion


        #region Varning
        /// <summary>
        /// Identifierar poster med externa lägeskontrollsbeställningar som inte är avslutade och som är äldre än dagens datum.
        /// </summary>
        /// <returns>Nycklar för identifierade poster</returns>
        internal static string externBestalldLageskontrollInTime()
        {
            return _externBestalldLageskontrollInTime;
        }
        /// <summary>
        /// Identifierar poster med externa lägeskontrollsbeställningar som inte är avslutade och som är äldre än ett visst antal dagar.
        /// </summary>
        /// <param name="filter">Tidsgränsen i dagar som något ska ha hänt med</param>
        /// <returns>Nycklar för identifierade poster</returns>
        internal static string externBestalldLageskontrollInTime(int filter)
        {
            return _externBestalldLageskontrollInTime + " AND (" +
                                                               "SYSDATE - date_created >= " + filter +
                                                        " OR    SYSDATE - date_modified >= " + filter +
                                                        " OR    SYSDATE - bev_inkommet >= " + filter +
                                                        " OR    SYSDATE - bev_paborjat >= " + filter +
                                                               ")";
        }


        /// <summary>
        /// Identifierar poster med interna lägeskontrollsbeställningar som inte är avslutade och som är äldre än dagens datum.
        /// </summary>
        /// <returns>Nycklar för identifierade poster</returns>
        internal static string internBestalldLageskontrollInTime()
        {
            return _internBestalldLageskontrollInTime;
        }
        /// <summary>
        /// Identifierar poster med intern lägeskontrollsbeställningar som inte är avslutade och som är äldre än ett visst antal dagar.
        /// </summary>
        /// <param name="filter">Tidsgränsen i dagar som något ska ha hänt med</param>
        /// <returns>Nycklar för identifierade poster</returns>
        internal static string internBestalldLageskontrollInTime(int filter)
        {
            return _internBestalldLageskontrollInTime + " AND (" +
                                                               "SYSDATE - date_created >= " + filter +
                                                        " OR    SYSDATE - date_modified >= " + filter +
                                                        " OR    SYSDATE - bev_inkommet >= " + filter +
                                                        " OR    SYSDATE - bev_paborjat >= " + filter +
                                                               ")";
        }


        internal static string utsattningWithLageskontrollUtfard()
        {
            return _utsattningWithLageskontrollUtfard;
        }
        internal static string utsattningWithLageskontrollUtfard(string filter)
        {
            return _utsattningWithLageskontrollUtfard + " AND " + filter;
        }


        internal static string lageskontrollUtfardAndamalNotUtfard()
        {
            return _lageskontrollUtfardAndamalNotUtfard;
        }
        internal static string lageskontrollUtfardAndamalNotUtfard(string filter)
        {
            return _lageskontrollUtfardAndamalNotUtfard + " AND " + filter;
        }


        internal static string logisktRaderade()
        {
            return _logisktRaderade;
        }
        internal static string logisktRaderade(string filter)
        {
            return _logisktRaderade + " AND " + filter;
        }
        #endregion


        #region Status
        internal static string changedSince()
        {
            string now = DateTime.Now.ToString("yyyy-MM-dd");
            return _changedSince + "WHERE date_created  <= TO_DATE('" + now + "','YYYY-MM-DD')" + 
                                   "OR    date_modified <= TO_DATE('" + now + "','YYYY-MM-DD')" + 
                                   "OR    bev_inkommet  <= TO_DATE('" + now + "','YYYY-MM-DD')" +
                                   "OR    bev_paborjat  <= TO_DATE('" + now + "','YYYY-MM-DD')" +
                                   "OR    bev_avslutat  <= TO_DATE('" + now + "','YYYY-MM-DD')";
        }
        internal static string changedSince(string filter)
        {
            return _changedSince + "WHERE date_created  <= TO_DATE('" + filter + "','YYYY-MM-DD')" +
                                   "OR    date_modified <= TO_DATE('" + filter + "','YYYY-MM-DD')" +
                                   "OR    bev_inkommet  <= TO_DATE('" + filter + "','YYYY-MM-DD')" +
                                   "OR    bev_paborjat  <= TO_DATE('" + filter + "','YYYY-MM-DD')" +
                                   "OR    bev_avslutat  <= TO_DATE('" + filter + "','YYYY-MM-DD')";
        }


        internal static string nbrOfChangesObjkod()
        {
            return String.Format(_nbrOfChangesObjkod, "").Trim();
        }
        internal static string nbrOfChangesObjkod(string filter)
        {
            return String.Format(_nbrOfChangesObjkod, " AND " + filter + " ").Trim();
        }


        internal static string nbrOfChangesTypkod()
        {
            return String.Format(_nbrOfChangesTypkod, "").Trim();
        }
        internal static string nbrOfChangesTypkod(string filter)
        {
            return String.Format(_nbrOfChangesTypkod, " AND " + filter + " ").Trim();
        }


        internal static string nbrTotal()
        {
            return String.Format(_nbrTotal, "").Trim();
        }
        internal static string nbrTotal(string filter)
        {
            return String.Format(_nbrTotal, " WHERE " + filter + " ").Trim();
        }


        internal static string nbrAktiva()
        {
            return String.Format(_nbrAktiva, "").Trim();
        }
        internal static string nbrAktiva(string filter)
        {
            return String.Format(_nbrAktiva, " WHERE " + filter + " ").Trim();
        }
        #endregion


        #region Fel
        internal static string bygglovNoBygglovsbevakning()
        {
            return _bygglovNoBygglovsbevakning;
        }
        internal static string bygglovNoBygglovsbevakning(string filter)
        {
            return _bygglovNoBygglovsbevakning + " AND " + filter;
        }


        internal static string bygglovsbevakningNoBygglov()
        {
            return _bygglovsbevakningNoBygglov;
        }
        internal static string bygglovsbevakningNoBygglov(string filter)
        {
            return _bygglovsbevakningNoBygglov + " AND " + filter;
        }


        internal static string bestalldUtsattningNoBevakning()
        {
            return _bestalldUtsattningNoBevakning;
        }
        internal static string bestalldUtsattningNoBevakning(string filter)
        {
            return _bestalldUtsattningNoBevakning + " AND " + filter;
        }


        internal static string bestalldLageskontrollNoBevakning()
        {
            return _bestalldLageskontrollNoBevakning;
        }
        internal static string bestalldLageskontrollNoBevakning(string filter)
        {
            return _bestalldLageskontrollNoBevakning + " AND " + filter;
        }
        #endregion
    }
}