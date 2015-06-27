using kartforandring.Geometry;

namespace kartforandring
{
	public static class sqlStrings
	{
        internal static string sqlDeleteLogicalKartforandring(string where)
        {
            if (!string.IsNullOrEmpty(where))
            {
                where = " WHERE " + where;
            }

            string sql = "UPDATE topo_special.kar_samling f " +
                         "SET    f.deleted = 1, " +
                         "       f.user_modified = SYS_CONTEXT('USERENV','OS_USER'), " +
                         "       f.date_modified = SYSDATE" +
                         where;

            return sql;
        }

        internal static string sqlSelectBygglovsbeslut(string filter)
        {
            string sql = "SELECT f.fid AS fid, NVL2(f.geom, 1, 0) AS is_geom, " +
                         "       NVL2(f.bev_plats_ovrigt, 1, 0) || NVL2(f.bev_plats_adressomr, 1, 0) || NVL2(f.bev_plats_fastighet, 1, 0) || NVL2(f.bev_plats_adress, 1, 0) || NVL2(f.geom, 1, 0) AS level_of_position, " +
                         "       f.kar_obj AS kar_obj, obj.value AS kar_obj_text, " +
                         "       f.kar_typ AS kar_typ, typ.value AS kar_typ_text, " +
                         "       f.user_modified AS user_modified, f.date_modified AS date_modified, f.bev_notering AS bev_notering, " +
                         "       f.bev_beskrivning AS bev_beskrivning, f.bev_inkommet AS bev_inkommet, f.bev_paborjat AS bev_paborjat, " +
                         "       f.bev_utforare AS bev_utforare, f.bev_avslutat AS bev_avslutat, " +
                         "       f.bev_plats_adress AS bev_plats_adress, adr.beladress AS bev_plats_adress_text, " +
                         "       f.bev_plats_fastighet AS bev_plats_fastighet, NVL2(fr.trakt, fr.trakt || ' ' || fr.fbetnr, NULL) AS bev_plats_fastighet_text, " +
                         "       NVL(f.bev_plats_adressomr, adromradr.adressomrades_id) AS bev_plats_adressomr, adromr.adressomrade AS bev_plats_adressomr_text, " +
                         "       f.bev_plats_ovrigt AS bev_plats_ovrigt, " +
                         "       f.bev_bygglov_diarie AS bev_bygglov_diarie, f.bev_bygglov_uts AS bev_bygglov_uts, bev_uts.value AS bev_bygglov_uts_text, " +
                         "       f.bev_bygglov_lag AS bev_bygglov_lag, bev_lag.value AS bev_bygglov_lag_text, " +
                         "       f.bev_bygglov_attefall AS bev_bygglov_attefall, bev_attefall.value AS bev_bygglov_attefall_text, " +
                         "       f.bev_bygglov_riv AS bev_bygglov_riv, bev_riv.value AS bev_bygglov_riv_text, " +
                         "       f.bev_bygglov_andamal AS bev_bygglov_andamal, bev_andamal.value AS bev_bygglov_andamal_text, " +
                         "       f.bev_bygglov_uts_best AS bev_bygglov_uts_best, best_uts.value AS bev_bygglov_uts_best_text, " +
                         "       f.bev_bygglov_lag_best AS bev_bygglov_lag_best, best_lag.value AS bev_bygglov_lag_best_text " +
                         "FROM   kar_samling f, " +
                         "       kar_obj_domain_tbd obj, " +
                         "       kar_typ_domain_tbd typ, " +
                         "       tefat.fir_fastigh fr, " +
                         "       lkr_gis.gis_v_adressomrade adromr, " +
                         "       lkr_gis.gis_v_beladress adr, " +
                         "      (SELECT a.adressomrades_id AS adressomrades_id, b.adressplats_id AS adressplats_id " +
                         "       FROM lkr_gis.gis_v_adressomrade a, lkr_gis.gis_v_beladress b " +
                         "       WHERE UPPER(a.adressomrade) = UPPER(b.adressomr)) adromradr, " +
                         "       kar_bygglov_bev_tbd bev_uts, " +
                         "       kar_bygglov_bev_tbd bev_lag, " +
                         "       kar_bygglov_bev_tbd bev_attefall, " +
                         "       kar_bygglov_bev_tbd bev_riv, " +
                         "       kar_bygglov_bev_tbd bev_andamal, " +
                         "       kar_bestallning_init_tbd best_uts, " +
                         "       kar_bestallning_init_tbd best_lag " +
                         "WHERE  f.kar_obj = 100 " +
                         "AND    f.kar_typ = 1020 " +
                         "AND    f.kar_obj = obj.id(+) " +
                         "AND    f.kar_typ = typ.id(+) " +
                         "AND    f.bev_plats_fastighet = fr.fnr(+) " +
                         "AND    f.bev_plats_adress = adr.adressplats_id(+) " +
                         "AND    f.bev_plats_adressomr = adromr.adressomrades_id(+) " +
                         "AND    f.bev_plats_adress = adromradr.adressplats_id(+) " +
                         "AND    f.bev_bygglov_uts = bev_uts.id(+) " +
                         "AND    f.bev_bygglov_lag = bev_lag.id(+) " +
                         "AND    f.bev_bygglov_attefall = bev_attefall.id(+) " +
                         "AND    f.bev_bygglov_riv = bev_riv.id(+) " +
                         "AND    f.bev_bygglov_andamal = bev_andamal.id(+) " +
                         "AND    f.bev_bygglov_uts_best = best_uts.id(+) " +
                         filter +
                         "AND    f.bev_bygglov_lag_best = best_lag.id(+) " +
                         "AND    f.deleted = 0";

            return sql;
        }

        internal static string sqlInsertLageskontroll(Bygglovsbeslut lageskontroll)
        {
            string skilje = ", ";

            string geometryColumn = lageskontroll.IsGeom == "0" || string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? "geom" + skilje : "";
            string geometry = lageskontroll.IsGeom == "0" || string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? SdoGeometry.createSdoGeometryFromRelationalData(lageskontroll) + skilje : "";


            // Hanterar NULL för Oracle
            string beskrivning, notering, adressomr, adress, platsovrigt, diarie, fastighet, lageskontrollbestallning;

            beskrivning = string.IsNullOrEmpty(lageskontroll.Beskrivning) ? "NULL" : "'" + lageskontroll.Beskrivning + "'";
            notering = string.IsNullOrEmpty(lageskontroll.Notering) ? "NULL" : "'" + lageskontroll.Notering + "'";
            adressomr = string.IsNullOrEmpty(lageskontroll.AdressOmr) ? "NULL" : "'" + lageskontroll.AdressOmr + "'";
            adress = string.IsNullOrEmpty(lageskontroll.Adress) ? "NULL" : "'" + lageskontroll.Adress + "'";
            platsovrigt = string.IsNullOrEmpty(lageskontroll.PlatsOvrigt) ? "NULL" : "'" + lageskontroll.PlatsOvrigt + "'";
            diarie = string.IsNullOrEmpty(lageskontroll.Diarie) ? "NULL" : "'" + lageskontroll.Diarie + "'";

            fastighet = string.IsNullOrEmpty(lageskontroll.Fastighet.ToString()) ? "NULL" : lageskontroll.Fastighet.ToString();
            lageskontrollbestallning = string.IsNullOrEmpty(lageskontroll.LageskontrollBestallning.ToString()) ? "NULL" : lageskontroll.LageskontrollBestallning.ToString();

            string sql = "INSERT INTO topo_special.kar_samling (" +
                                                  "kar_obj" + skilje +
                                                  "kar_typ" + skilje +
                                                  "bev_beskrivning" + skilje +
                                                  "bev_notering" + skilje +
                                                  "bev_inkommet" + skilje +
                                                  "bev_plats_adressomr" + skilje +
                                                  "bev_plats_adress" + skilje +
                                                  "bev_plats_fastighet" + skilje +
                                                  "bev_plats_ovrigt" + skilje +
                                                  "bev_bygglov_diarie" + skilje +
                                                  "bev_bygglov_lag" + skilje +
                                                  "bev_bygglov_lag_best" + skilje +
                                                  geometryColumn +
                                                  "guid_tmp" +
                                                  ") VALUES (" +
                                                             "100" + skilje +
                                                             "1020" + skilje +
                                                             beskrivning + skilje +
                                                             notering + skilje +
                                                             "SYSDATE" + skilje +
                                                             adressomr + skilje +
                                                             adress + skilje +
                                                             fastighet + skilje +
                                                             platsovrigt + skilje +
                                                             diarie + skilje +
                                                             "10" + skilje +
                                                             lageskontrollbestallning + skilje +
                                                             geometry +
                                                             "'" + lageskontroll.tmpGuidKey.ToString().Replace("-", "") + "'" +
                                                             ")";

            return sql;
        }

        internal static string sqlUpdateLageskontroll(Bygglovsbeslut lageskontroll)
        {
            string skilje = ", ";

            string geometry = lageskontroll.IsGeom == "0" || string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? skilje + "f.geom = " + SdoGeometry.createSdoGeometryFromRelationalData(lageskontroll) + " " : "";


            // Hanterar NULL för Oracle
            string beskrivning, notering, adressomr, adress, platsovrigt, diarie, fastighet, lageskontrollbestallning;

            beskrivning = string.IsNullOrEmpty(lageskontroll.Beskrivning) ? "NULL" : "'" + lageskontroll.Beskrivning + "'";
            notering = string.IsNullOrEmpty(lageskontroll.Notering) ? "NULL" : "'" + lageskontroll.Notering + "'";
            adressomr = string.IsNullOrEmpty(lageskontroll.AdressOmr) ? "NULL" : "'" + lageskontroll.AdressOmr + "'";
            adress = string.IsNullOrEmpty(lageskontroll.Adress) ? "NULL" : "'" + lageskontroll.Adress + "'";
            platsovrigt = string.IsNullOrEmpty(lageskontroll.PlatsOvrigt) ? "NULL" : "'" + lageskontroll.PlatsOvrigt + "'";
            diarie = string.IsNullOrEmpty(lageskontroll.Diarie) ? "NULL" : "'" + lageskontroll.Diarie + "'";

            fastighet = string.IsNullOrEmpty(lageskontroll.Fastighet.ToString()) ? "NULL" : lageskontroll.Fastighet.ToString();
            lageskontrollbestallning = string.IsNullOrEmpty(lageskontroll.LageskontrollBestallning.ToString()) ? "NULL" : lageskontroll.LageskontrollBestallning.ToString();

            string sql = "UPDATE topo_special.kar_samling f " +
                         "SET f.bev_beskrivning = " + beskrivning + skilje +
                         "    f.bev_notering = " + notering + skilje +
                         "    f.user_modified = SYS_CONTEXT('USERENV','OS_USER')" + skilje +
                         "    f.date_modified = SYSDATE" + skilje +
                         "    f.bev_plats_adressomr = " + adressomr + skilje +
                         "    f.bev_plats_adress = " + adress + skilje +
                         "    f.bev_plats_fastighet = " + fastighet + skilje +
                         "    f.bev_plats_ovrigt = " + platsovrigt + skilje +
                         "    f.bev_bygglov_lag_best = " + lageskontrollbestallning + " " +
                         geometry +
                         "WHERE f.fid = " + lageskontroll.Fid;

            return sql;
        }
    }
}