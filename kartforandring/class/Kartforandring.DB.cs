using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace kartforandring
{
	public static class DB
	{

        public static DataTable GetAllaKartforandringar()
        {
            string sql = "SELECT f.fid AS fid, " +
                         "       NVL2(f.geom, 1, 0) AS is_geom, " +
                         "       NVL2(f.bev_plats_ovrigt, 1, 0) || NVL2(f.bev_plats_adressomr, 1, 0) || NVL2(f.bev_plats_fastighet, 1, 0) || NVL2(f.bev_plats_adress, 1, 0) || NVL2(f.geom, 1, 0) AS level_of_position, " +
                         "       f.kar_obj AS kar_obj, obj.value AS kar_obj_text, " +
                         "       f.kar_typ AS kar_typ, typ.value AS kar_typ_text, " +
                         "       f.user_modified AS user_modified, f.date_modified AS date_modified, " +
                         "       f.bev_notering AS bev_notering, f.bev_beskrivning AS bev_beskrivning, " +
                         "       f.bev_inkommet AS bev_inkommet, f.bev_paborjat AS bev_paborjat, " +
                         "       f.bev_utforare AS bev_utforare, f.bev_avslutat AS bev_avslutat, " +
                         "       f.bev_plats_adress AS bev_plats_adress, adr.beladress AS bev_plats_adress_text, " +
                         "       f.bev_plats_fastighet AS bev_plats_fastighet, NVL2(fr.trakt, fr.trakt || ' ' || fr.fbetnr, NULL) AS bev_plats_fastighet_text, " +
                         "       f.bev_plats_adressomr AS bev_plats_adressomr, adromr.adressomrade AS bev_plats_adressomr_text, " +
                         "       f.bev_plats_ovrigt AS bev_plats_ovrigt " +
                         "FROM   kar_samling f, " +
                         "       kar_obj_domain_tbd obj, " +
                         "       kar_typ_domain_tbd typ, " +
                         "       tefat.fir_fastigh fr, " +
                         "       lkr_gis.gis_v_adressomrade adromr, " +
                         "       lkr_gis.gis_v_beladress adr " +
                         "WHERE  f.kar_obj = obj.id(+) " +
                         "AND    f.kar_typ = typ.id(+) " +
                         "AND    f.bev_plats_fastighet = fr.fnr(+) " +
                         "AND    f.bev_plats_adress = adr.adressplats_id(+) " +
                         "AND    f.bev_plats_adressomr = adromr.adressomrades_id(+)";

            return GetData(sql);
        }

        public static DataTable GetBygglovsbeslut()
        {
            return GetData(sqlStrings.sqlSelectBygglovsbeslut(""));
        }

        public static DataTable GetBygglovsbeslutMedLageskontroll()
        {
            return GetData(sqlStrings.sqlSelectBygglovsbeslut("AND  f.bev_bygglov_lag = 10  " +
                                                              "AND  f.bev_avslutat IS NULL "));
        }

        public static DataTable AddLageskontroll(Bygglovsbeslut lageskontroll)
        {
            try
            {
                if (!existBygglov(lageskontroll.Diarie))
                {
                    nullifyRelationalPositions(ref lageskontroll);
                    return AddData(lageskontroll);
                }
                else
                {
                    throw new Exception("LKR-00002", new Exception("Bygglovet " + lageskontroll.Diarie.ToString() + " existerar redan."));
                }
            }
            catch
            {
                throw;
            }
        }

        public static DataTable UpdateLageskontroll(Bygglovsbeslut lageskontroll)
        {
            try
            {
                if (existBygglov(lageskontroll.Diarie))
                {
                    nullifyRelationalPositions(ref lageskontroll);
                    return UpdateData(lageskontroll);
                }
                else
                {
                    throw new Exception("LKR-00004", new Exception("Uppdateringen av Bygglov " + lageskontroll.Diarie.ToString() + " kunde INTE genomföras. Posten verkar inte finnas i databas längre. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)"));
                }
            }
            catch
            {
                throw;
            }
        }

        public static void DeleteLageskontroll(int fid)
        {
            try
            {
                DeleteLogicalData(sqlStrings.sqlDeleteLogicalKartforandring("f.bev_bygglov_lag IS NOT NULL AND f.fid = " + fid.ToString()));
            }
            catch
            {
                throw;
            }
        }

        internal static bool existBygglov(string diarienummer)
        {
            string sql = "SELECT COUNT(*) antal FROM kar_samling WHERE TRIM(bev_bygglov_diarie) = '" + diarienummer.Trim() + "'";

            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            return Convert.ToInt32(dt.Rows[0]["ANTAL"]) > 0 ? true : false;
        }

        internal static DataTable GetData(string sql)
        {
            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            return dt;
        }

        internal static DataTable AddData(Bygglovsbeslut lageskontroll)
        {
            try
            {
                string patternDiarie = @"^(19\d{2}\.\d{1,4})$|^(20\d{2}\.\d{1,4})$";
                if (!Regex.IsMatch(lageskontroll.Diarie, patternDiarie))
                {
                    throw new Exception("LKR-00007", new Exception("Diarie/aktbeteckning (" + lageskontroll.Diarie.ToString() + ") är i fel format. Ska bestå av ett årtal mellan 1900-2099, punkt och sedan löpnummer, tillåtet 1 - 4 tal (ÅÅÅÅ.####)."));
                }

                DataTable dt = new DataTable();
                lageskontroll.tmpGuidKey = Guid.NewGuid();
                
                // Kontrollera nycklar för adressområde, adressplats och fastighet. Sätts till null om ej stämmer
                string regexpPatternGuid = @"^[A-Z0-9]{8}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{4}-[A-Z0-9]{12}$";
                string regexpPatternFastighet = @"^\d{9}$";
                if(!Regex.IsMatch(lageskontroll.AdressOmr, regexpPatternGuid))
                {
                    lageskontroll.AdressOmr = null;
                }
                if (!Regex.IsMatch(lageskontroll.Adress, regexpPatternGuid))
                {
                    lageskontroll.Adress = null;
                }
                if (!Regex.IsMatch(lageskontroll.Fastighet.ToString(), regexpPatternFastighet))
                {
                    lageskontroll.Fastighet = null;
                }

                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sqlStrings.sqlInsertLageskontroll(lageskontroll), con);

                if (lageskontroll.AdressOmr != null || lageskontroll.Adress != null || lageskontroll.Fastighet != null)
                {
                    lageskontroll.IsGeom = "1";
                }

                // Inserts resultingRow
                com.Connection.Open();
                int affectedRows = com.ExecuteNonQuery();

                // Om rad har lagrats, skapa returneringsvärde för raden att presenteras i klienten
                if (affectedRows > 0)
                {
                    // Gets newly created database key instead of temporary guid-key
                    // Throw exception if no match for post and the given guid
                    DataTable tmpDtFid = new DataTable();
                    com.CommandText = "SELECT fid, bev_inkommet FROM kar_samling WHERE guid_tmp = '" + lageskontroll.tmpGuidKey.ToString().Replace("-", "") + "'";
                    OleDbDataReader tmpDrFid;   
                    tmpDrFid = com.ExecuteReader();
                    tmpDtFid.Load(tmpDrFid);
                    if (tmpDtFid.Rows.Count != 1) // Check if the DataTable returns any data from database
                    {
                        throw new Exception("LKR-00005", new Exception("Kunde inte erhålla databasnyckel vid skapande av ny post (temp-ID: " + lageskontroll.tmpGuidKey.ToString() + ")."));
                    } 


                    dt = createEmptyBygglovsbeslutTable();

                    DataRow dr = dt.NewRow();

                    lageskontroll.Fid = int.Parse(tmpDtFid.Rows[0]["FID"].ToString());
                    lageskontroll.Inkommit = Convert.IsDBNull(tmpDtFid.Rows[0]["BEV_INKOMMET"]) ? (DateTime?)null : DateTime.Parse(tmpDtFid.Rows[0]["BEV_INKOMMET"].ToString());

                    tmpDrFid.Close();
                    tmpDrFid.Dispose();

                    // Redefine level of positioning of object
                    StringBuilder levelOfPosition = new StringBuilder("00000");
                    string levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.PlatsOvrigt) ? "0" : "1";
                    levelOfPosition.Remove(0, 1);
                    levelOfPosition.Insert(0, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.AdressOmr) ? "0" : "1";
                    levelOfPosition.Remove(1, 1);
                    levelOfPosition.Insert(1, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.Fastighet.ToString()) ? "0" : "1";
                    levelOfPosition.Remove(2, 1);
                    levelOfPosition.Insert(2, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.Adress) ? "0" : "1";
                    levelOfPosition.Remove(3, 1);
                    levelOfPosition.Insert(3, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? "0" : "1";
                    levelOfPosition.Remove(4, 1);
                    levelOfPosition.Insert(4, levelOfPositionPart);

                    // Fill returning resultingRow
                    dr["FID"] = lageskontroll.Fid.ToString();
                    dr["IS_GEOM"] = string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? DBNull.Value : (object)lageskontroll.IsGeom;
                    dr["KAR_OBJ"] = string.IsNullOrWhiteSpace(lageskontroll.KarObj.ToString()) ? DBNull.Value : (object)lageskontroll.KarObj;
                    dr["KAR_OBJ_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.KarObjText) ? DBNull.Value : (object)lageskontroll.KarObjText;
                    dr["KAR_TYP"] = string.IsNullOrWhiteSpace(lageskontroll.KarTyp.ToString()) ? DBNull.Value : (object)lageskontroll.KarTyp;
                    dr["KAR_TYP_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.KarTypText) ? DBNull.Value : (object)lageskontroll.KarTypText;
                    dr["USER_MODIFIED"] = string.IsNullOrWhiteSpace(lageskontroll.UserModified) ? DBNull.Value : (object)lageskontroll.UserModified;
                    dr["DATE_MODIFIED"] = string.IsNullOrWhiteSpace(lageskontroll.DateModified.ToString()) ? DBNull.Value : (object)lageskontroll.DateModified;
                    dr["BEV_NOTERING"] = string.IsNullOrWhiteSpace(lageskontroll.Notering) ? DBNull.Value : (object)lageskontroll.Notering;
                    dr["BEV_BESKRIVNING"] = string.IsNullOrWhiteSpace(lageskontroll.Beskrivning) ? DBNull.Value : (object)lageskontroll.Beskrivning;
                    dr["BEV_INKOMMET"] = string.IsNullOrWhiteSpace(lageskontroll.Inkommit.ToString()) ? DBNull.Value : (object)lageskontroll.Inkommit;
                    dr["BEV_PABORJAT"] = string.IsNullOrWhiteSpace(lageskontroll.Paborjat.ToString()) ? DBNull.Value : (object)lageskontroll.Paborjat;
                    dr["BEV_UTFORARE"] = string.IsNullOrWhiteSpace(lageskontroll.Utforare) ? DBNull.Value : (object)lageskontroll.Utforare;
                    dr["BEV_AVSLUTAT"] = string.IsNullOrWhiteSpace(lageskontroll.Avslutat.ToString()) ? DBNull.Value : (object)lageskontroll.Avslutat;
                    dr["BEV_PLATS_ADRESS"] = string.IsNullOrWhiteSpace(lageskontroll.Adress) ? DBNull.Value : (object)lageskontroll.Adress;
                    dr["BEV_PLATS_ADRESS_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AdressText) ? DBNull.Value : (object)lageskontroll.AdressText;
                    dr["BEV_PLATS_FASTIGHET"] = string.IsNullOrWhiteSpace(lageskontroll.Fastighet.ToString()) ? DBNull.Value : (object)lageskontroll.Fastighet;
                    dr["BEV_PLATS_FASTIGHET_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.FastighetText) ? DBNull.Value : (object)lageskontroll.FastighetText;
                    dr["BEV_PLATS_ADRESSOMR"] = string.IsNullOrWhiteSpace(lageskontroll.AdressOmr) ? DBNull.Value : (object)lageskontroll.AdressOmr;
                    dr["BEV_PLATS_ADRESSOMR_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AdressOmrText) ? DBNull.Value : (object)lageskontroll.AdressOmrText;
                    dr["BEV_PLATS_OVRIGT"] = string.IsNullOrWhiteSpace(lageskontroll.PlatsOvrigt) ? DBNull.Value : (object)lageskontroll.PlatsOvrigt;
                    dr["BEV_BYGGLOV_DIARIE"] = string.IsNullOrWhiteSpace(lageskontroll.Diarie) ? DBNull.Value : (object)lageskontroll.Diarie;
                    dr["BEV_BYGGLOV_UTS"] = string.IsNullOrWhiteSpace(lageskontroll.Utsattning.ToString()) ? DBNull.Value : (object)lageskontroll.Utsattning;
                    dr["BEV_BYGGLOV_UTS_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.UtsattningText) ? DBNull.Value : (object)lageskontroll.UtsattningText;
                    dr["BEV_BYGGLOV_LAG"] = string.IsNullOrWhiteSpace(lageskontroll.Lageskontroll.ToString()) ? DBNull.Value : (object)lageskontroll.Lageskontroll;
                    dr["BEV_BYGGLOV_LAG_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.LageskontrollText) ? DBNull.Value : (object)lageskontroll.LageskontrollText;
                    dr["BEV_BYGGLOV_ATTEFALL"] = string.IsNullOrWhiteSpace(lageskontroll.Attefall.ToString()) ? DBNull.Value : (object)lageskontroll.Attefall;
                    dr["BEV_BYGGLOV_ATTEFALL_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AttefallText) ? DBNull.Value : (object)lageskontroll.AttefallText;
                    dr["BEV_BYGGLOV_RIV"] = string.IsNullOrWhiteSpace(lageskontroll.Riv.ToString()) ? DBNull.Value : (object)lageskontroll.Riv;
                    dr["BEV_BYGGLOV_RIV_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.RivText) ? DBNull.Value : (object)lageskontroll.RivText;
                    dr["BEV_BYGGLOV_ANDAMAL"] = string.IsNullOrWhiteSpace(lageskontroll.Andamal.ToString()) ? DBNull.Value : (object)lageskontroll.Andamal;
                    dr["BEV_BYGGLOV_ANDAMAL_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AndamalText) ? DBNull.Value : (object)lageskontroll.AndamalText;
                    dr["BEV_BYGGLOV_UTS_BEST"] = string.IsNullOrWhiteSpace(lageskontroll.UtsattningBestallning.ToString()) ? DBNull.Value : (object)lageskontroll.UtsattningBestallning;
                    dr["BEV_BYGGLOV_UTS_BEST_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.UtsattningBestallningText) ? DBNull.Value : (object)lageskontroll.UtsattningBestallningText;
                    dr["BEV_BYGGLOV_LAG_BEST"] = string.IsNullOrWhiteSpace(lageskontroll.LageskontrollBestallning.ToString()) ? DBNull.Value : (object)lageskontroll.LageskontrollBestallning;
                    dr["BEV_BYGGLOV_LAG_BEST_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.LageskontrollBestallningText) ? DBNull.Value : (object)lageskontroll.LageskontrollBestallningText;
                    dr["LEVEL_OF_POSITION"] = levelOfPosition.ToString();

                    dt.Rows.Add(dr);
                }
                else
                {
                    throw new Exception("LKR-00001", new Exception("Ändringen kunde INTE genomföras. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)"));
                }

                con.Close();
                con.Dispose();

                return dt;
            }
            catch
            {
                throw;
            }
        }

        internal static DataTable UpdateData(Bygglovsbeslut lageskontroll)
        {
            try
            {
                DataTable dt = new DataTable();

                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sqlStrings.sqlSelectBygglovsbeslut("AND f.fid = " + lageskontroll.Fid.ToString() + " "), con);
                OleDbDataReader dr;
                com.Connection.Open();
                dr = com.ExecuteReader();

                dt.Load(dr);

                dr.Close();
                dr.Dispose();

                if (dt.Rows.Count == 1)
                {
                    DataRow row = dt.Rows[0];
                    lageskontroll.IsGeom = row["IS_GEOM"].ToString();
                    lageskontroll.LevelOfPosition = row["LEVEL_OF_POSITION"].ToString().ToCharArray();
                    lageskontroll.PlatsOvrigt = row["BEV_PLATS_OVRIGT"].ToString();
                }
                else
                {
                    throw new Exception("LKR-00006", new Exception("Ändringen kunde INTE genomföras. Databasen överensstämmer inte med tabell. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)"));
                }

                com.CommandText = sqlStrings.sqlUpdateLageskontroll(lageskontroll);

                int affectedRows = com.ExecuteNonQuery();
                if (affectedRows > 0)
                {

                    dt = createEmptyBygglovsbeslutTable();

                    DataRow resultingRow = dt.NewRow();

                    // Redefine level of positioning of object
                    StringBuilder levelOfPosition = new StringBuilder(new string(lageskontroll.LevelOfPosition));
                    string levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.PlatsOvrigt) ? "0" : "1";
                    levelOfPosition.Remove(0, 1);
                    levelOfPosition.Insert(0, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.AdressOmr) ? "0" : "1";
                    levelOfPosition.Remove(1, 1);
                    levelOfPosition.Insert(1, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.Fastighet.ToString()) ? "0" : "1";
                    levelOfPosition.Remove(2, 1);
                    levelOfPosition.Insert(2, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.Adress) ? "0" : "1";
                    levelOfPosition.Remove(3, 1);
                    levelOfPosition.Insert(3, levelOfPositionPart);
                    levelOfPositionPart = string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? "0" : "1";
                    levelOfPosition.Remove(4, 1);
                    levelOfPosition.Insert(4, levelOfPositionPart);

                    // Fill returning resultingRow
                    resultingRow["FID"] = string.IsNullOrWhiteSpace(lageskontroll.Fid.ToString()) ? DBNull.Value : (object)lageskontroll.Fid;
                    resultingRow["IS_GEOM"] = string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? DBNull.Value : (object)lageskontroll.IsGeom;
                    resultingRow["KAR_OBJ"] = string.IsNullOrWhiteSpace(lageskontroll.KarObj.ToString()) ? DBNull.Value : (object)lageskontroll.KarObj;
                    resultingRow["KAR_OBJ_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.KarObjText) ? DBNull.Value : (object)lageskontroll.KarObjText;
                    resultingRow["KAR_TYP"] = string.IsNullOrWhiteSpace(lageskontroll.KarTyp.ToString()) ? DBNull.Value : (object)lageskontroll.KarTyp;
                    resultingRow["KAR_TYP_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.KarTypText) ? DBNull.Value : (object)lageskontroll.KarTypText;
                    resultingRow["USER_MODIFIED"] = string.IsNullOrWhiteSpace(lageskontroll.UserModified) ? DBNull.Value : (object)lageskontroll.UserModified;
                    resultingRow["DATE_MODIFIED"] = string.IsNullOrWhiteSpace(lageskontroll.DateModified.ToString()) ? DBNull.Value : (object)lageskontroll.DateModified;
                    resultingRow["BEV_NOTERING"] = string.IsNullOrWhiteSpace(lageskontroll.Notering) ? DBNull.Value : (object)lageskontroll.Notering;
                    resultingRow["BEV_BESKRIVNING"] = string.IsNullOrWhiteSpace(lageskontroll.Beskrivning) ? DBNull.Value : (object)lageskontroll.Beskrivning;
                    resultingRow["BEV_INKOMMET"] = string.IsNullOrWhiteSpace(lageskontroll.Inkommit.ToString()) ? DBNull.Value : (object)lageskontroll.Inkommit;
                    resultingRow["BEV_PABORJAT"] = string.IsNullOrWhiteSpace(lageskontroll.Paborjat.ToString()) ? DBNull.Value : (object)lageskontroll.Paborjat;
                    resultingRow["BEV_UTFORARE"] = string.IsNullOrWhiteSpace(lageskontroll.Utforare) ? DBNull.Value : (object)lageskontroll.Utforare;
                    resultingRow["BEV_AVSLUTAT"] = string.IsNullOrWhiteSpace(lageskontroll.Avslutat.ToString()) ? DBNull.Value : (object)lageskontroll.Avslutat;
                    resultingRow["BEV_PLATS_ADRESS"] = string.IsNullOrWhiteSpace(lageskontroll.Adress) ? DBNull.Value : (object)lageskontroll.Adress;
                    resultingRow["BEV_PLATS_ADRESS_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AdressText) ? DBNull.Value : (object)lageskontroll.AdressText;
                    resultingRow["BEV_PLATS_FASTIGHET"] = string.IsNullOrWhiteSpace(lageskontroll.Fastighet.ToString()) ? DBNull.Value : (object)lageskontroll.Fastighet;
                    resultingRow["BEV_PLATS_FASTIGHET_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.FastighetText) ? DBNull.Value : (object)lageskontroll.FastighetText;
                    resultingRow["BEV_PLATS_ADRESSOMR"] = string.IsNullOrWhiteSpace(lageskontroll.AdressOmr) ? DBNull.Value : (object)lageskontroll.AdressOmr;
                    resultingRow["BEV_PLATS_ADRESSOMR_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AdressOmrText) ? DBNull.Value : (object)lageskontroll.AdressOmrText;
                    resultingRow["BEV_PLATS_OVRIGT"] = string.IsNullOrWhiteSpace(lageskontroll.PlatsOvrigt) ? DBNull.Value : (object)lageskontroll.PlatsOvrigt;
                    resultingRow["BEV_BYGGLOV_DIARIE"] = string.IsNullOrWhiteSpace(lageskontroll.Diarie) ? DBNull.Value : (object)lageskontroll.Diarie;
                    resultingRow["BEV_BYGGLOV_UTS"] = string.IsNullOrWhiteSpace(lageskontroll.Utsattning.ToString()) ? DBNull.Value : (object)lageskontroll.Utsattning;
                    resultingRow["BEV_BYGGLOV_UTS_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.UtsattningText) ? DBNull.Value : (object)lageskontroll.UtsattningText;
                    resultingRow["BEV_BYGGLOV_LAG"] = string.IsNullOrWhiteSpace(lageskontroll.Lageskontroll.ToString()) ? DBNull.Value : (object)lageskontroll.Lageskontroll;
                    resultingRow["BEV_BYGGLOV_LAG_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.LageskontrollText) ? DBNull.Value : (object)lageskontroll.LageskontrollText;
                    resultingRow["BEV_BYGGLOV_ATTEFALL"] = string.IsNullOrWhiteSpace(lageskontroll.Attefall.ToString()) ? DBNull.Value : (object)lageskontroll.Attefall;
                    resultingRow["BEV_BYGGLOV_ATTEFALL_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AttefallText) ? DBNull.Value : (object)lageskontroll.AttefallText;
                    resultingRow["BEV_BYGGLOV_RIV"] = string.IsNullOrWhiteSpace(lageskontroll.Riv.ToString()) ? DBNull.Value : (object)lageskontroll.Riv;
                    resultingRow["BEV_BYGGLOV_RIV_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.RivText) ? DBNull.Value : (object)lageskontroll.RivText;
                    resultingRow["BEV_BYGGLOV_ANDAMAL"] = string.IsNullOrWhiteSpace(lageskontroll.Andamal.ToString()) ? DBNull.Value : (object)lageskontroll.Andamal;
                    resultingRow["BEV_BYGGLOV_ANDAMAL_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.AndamalText) ? DBNull.Value : (object)lageskontroll.AndamalText;
                    resultingRow["BEV_BYGGLOV_UTS_BEST"] = string.IsNullOrWhiteSpace(lageskontroll.UtsattningBestallning.ToString()) ? DBNull.Value : (object)lageskontroll.UtsattningBestallning;
                    resultingRow["BEV_BYGGLOV_UTS_BEST_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.UtsattningBestallningText) ? DBNull.Value : (object)lageskontroll.UtsattningBestallningText;
                    resultingRow["BEV_BYGGLOV_LAG_BEST"] = string.IsNullOrWhiteSpace(lageskontroll.LageskontrollBestallning.ToString()) ? DBNull.Value : (object)lageskontroll.LageskontrollBestallning;
                    resultingRow["BEV_BYGGLOV_LAG_BEST_TEXT"] = string.IsNullOrWhiteSpace(lageskontroll.LageskontrollBestallningText) ? DBNull.Value : (object)lageskontroll.LageskontrollBestallningText;
                    resultingRow["LEVEL_OF_POSITION"] = levelOfPosition;

                    dt.Rows.Add(resultingRow);
                }
                else
                {
                    throw new Exception("LKR-00001", new Exception("Ändringen kunde INTE genomföras. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)"));
                }

                con.Close();
                con.Dispose();

                return dt;
            }
            catch
            {
                throw;
            }
        }

        internal static void DeleteLogicalData(string sql)
        {
            try
            {
                DataTable dt = new DataTable();

                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sql, con);

                com.Connection.Open();
                int affectedRows = com.ExecuteNonQuery();
                if (affectedRows != 1)
                {
                    throw new Exception("LKR-00003", new Exception("Raderingen kunde INTE genomföras. Posten verkar inte finnas i databas längre. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)"));
                }
            }
            catch
            {
                throw;
            }
        }

        internal static DataTable createEmptyBygglovsbeslutTable()
        {
            DataTable dt = new DataTable();

            DataColumn cl = new DataColumn("FID", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("IS_GEOM", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("KAR_OBJ", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("KAR_OBJ_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("KAR_TYP", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("KAR_TYP_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("USER_MODIFIED", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("DATE_MODIFIED", System.Type.GetType("System.DateTime"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_NOTERING", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BESKRIVNING", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_INKOMMET", System.Type.GetType("System.DateTime"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PABORJAT", System.Type.GetType("System.DateTime"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_UTFORARE", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_AVSLUTAT", System.Type.GetType("System.DateTime"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_ADRESS", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_ADRESS_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_FASTIGHET", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_FASTIGHET_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_ADRESSOMR", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_ADRESSOMR_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_PLATS_OVRIGT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_DIARIE", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_UTS", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_UTS_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_LAG", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_LAG_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_ATTEFALL", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_ATTEFALL_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_RIV", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_RIV_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_ANDAMAL", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_ANDAMAL_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_UTS_BEST", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_UTS_BEST_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_LAG_BEST", System.Type.GetType("System.Int32"));
            dt.Columns.Add(cl);
            cl = new DataColumn("BEV_BYGGLOV_LAG_BEST_TEXT", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);
            cl = new DataColumn("LEVEL_OF_POSITION", System.Type.GetType("System.String"));
            dt.Columns.Add(cl);

            return dt;
        }

        internal static DataTable GetLageskontrollOrderingDomain()
        {
            string sql = "SELECT id, value FROM kar_bestallning_init_tbd ORDER BY value DESC";

            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;
            
            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            return dt;
        }

        internal static DataTable GetAdressOmradeDomain()
        {
            string sql = "SELECT adressomrades_id AS id, adressomrade AS value " +
                         "FROM gis_v_adressomrade " +
                         "ORDER BY adressomrade";

            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            dt = domainAddEmptyRowFirst(dt);

            return dt;
        }

        internal static DataTable GetBelagenhetsAdressDomain()
        {
            string sql = "SELECT adressplats_id AS id, beladress AS value " +
                         "FROM gis_v_beladress " +
                         "ORDER BY beladress";

            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            dt = domainAddEmptyRowFirst(dt);

            return dt;
        }

        internal static DataTable GetBelagenhetsAdressDomain(string AdressOmrId)
        {
            string sql = "SELECT adressplats_id AS id, beladress AS value " +
                         "FROM gis_v_beladress";
            if (!string.IsNullOrWhiteSpace(AdressOmrId))
            {
                sql += " WHERE adressomrades_id = '" + AdressOmrId + "'";
            }

            sql += " ORDER BY beladress";

            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            dt = domainAddEmptyRowFirst(dt);

            return dt;
        }

        internal static DataTable GetFastighetDomain()
        {
            string sql = "SELECT fastighet_id AS id, fastighet AS value " +
                         "FROM gis_v_fastighetsregister " +
                         "ORDER BY fastighet";

            DataTable dt = new DataTable();
            OleDbConnection con = GetOleDbConncection();
            OleDbCommand com = new OleDbCommand(sql, con);
            OleDbDataReader dr;

            com.Connection.Open();
            dr = com.ExecuteReader();

            dt.Load(dr);

            dr.Close();
            dr.Dispose();

            dt = domainAddEmptyRowFirst(dt);

            return dt;
        }

        /// <summary>
        /// Inserted empty row for possibility to not have value in a drop list.
        /// No key value is -1
        /// </summary>
        /// <param name="dt">DataTable with column named ID</param>
        internal static DataTable domainAddEmptyRowFirst(DataTable dt)
        {
            DataRow emptyDr = dt.NewRow();
            emptyDr["ID"] = -1;
            dt.Rows.InsertAt(emptyDr, 0);

            return dt;
        }

        internal static void nullifyRelationalPositions(ref Bygglovsbeslut post)
        {
            if (post.Adress == "-1")
            {
                post.Adress = "";
            }
            if (post.AdressOmr == "-1")
            {
                post.AdressOmr = "";
            }
            if (post.Fastighet == -1)
            {
                post.Fastighet = null;
            }
        }

        internal static OleDbConnection GetOleDbConncection()
        {
            string user = Environment.GetEnvironmentVariable("topospecialuser", EnvironmentVariableTarget.Machine);
            string password = Environment.GetEnvironmentVariable("topospecialpassword", EnvironmentVariableTarget.Machine);
            string service = Environment.GetEnvironmentVariable("topospecialservice", EnvironmentVariableTarget.Machine);
            string connectionStr = string.Empty;

            if (string.IsNullOrEmpty(user) || string.IsNullOrEmpty(password))
            {
                string appSettingConnection = ConfigurationManager.AppSettings["OracleOleDBConStringTOPO_SPECIAL"].ToString();
                if (!string.IsNullOrWhiteSpace(appSettingConnection))
                {
                    connectionStr = ConfigurationManager.AppSettings["OracleOleDBConStringTOPO_SPECIAL"];
                }
                else
                {
                    throw new Exception("Anslutningsuppgifter till databas existerar ej");
                }
            }
            else
            {
                connectionStr = "Provider=OraOLEDB.Oracle;Data Source=" + service + ";User Id=" + user + ";Password=" + password + ";";
            }

            return new OleDbConnection(connectionStr);
        }
    }

}