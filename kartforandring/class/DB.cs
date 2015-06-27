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
            return GetData(sqlSelectBygglovsbeslut(""));
        }


        public static DataTable GetBygglovsbeslutMedLageskontroll()
        {
            //TODO: Inte NULL? Värde kan visa om lägeskontroll ska göras (beslut finns) eller om kontrollen redan är gjord
            return GetData(sqlSelectBygglovsbeslut("AND f.bev_bygglov_lag IS NOT NULL "));
        }

        public static DataTable AddLageskontroll(Bygglovsbeslut lageskontroll)
        {
            try
            {
                if (!existBygglov(lageskontroll.Diarie))
                {
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
                DeleteLogicalData(sqlDeleteLogicalKartforandring("f.bev_bygglov_lag IS NOT NULL AND f.fid = " + fid.ToString()));
            }
            catch
            {
                throw;
            }
        }

        private static bool existBygglov(string diarienummer)
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


        private static DataTable GetData(string sql)
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

        private static DataTable AddData(Bygglovsbeslut lageskontroll)
        {
            try
            {
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
                OleDbCommand com = new OleDbCommand(sqlInsertLageskontroll(lageskontroll), con);

                // Inserts row
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


                    dt = createEmptyBygglovsbesllutsTable();

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

                    // Fill returning row
                    dr["FID"] = string.IsNullOrWhiteSpace(lageskontroll.Fid.ToString()) ? DBNull.Value : (object)lageskontroll.Fid;
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

        private static DataTable UpdateData(Bygglovsbeslut lageskontroll)
        {
            try
            {
                DataTable dt = new DataTable();

                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sqlUpdateLageskontroll(lageskontroll), con);

                com.Connection.Open();
                int affectedRows = com.ExecuteNonQuery();
                if (affectedRows > 0)
                {
                    dt = createEmptyBygglovsbesllutsTable();

                    DataRow dr = dt.NewRow();

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

                    // Fill returning row
                    dr["FID"] = string.IsNullOrWhiteSpace(lageskontroll.Fid.ToString()) ? DBNull.Value : (object)lageskontroll.Fid;
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

        private static void DeleteLogicalData(string sql)
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

        private static string sqlDeleteLogicalKartforandring(string where)
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

        private static string sqlSelectBygglovsbeslut(string filter)
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

        private static string sqlInsertLageskontroll(Bygglovsbeslut lageskontroll)
        {
            string skilje = ", ";

            string geometryColumn = lageskontroll.IsGeom == "0" || string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? "geom" + skilje : "";
            string geometry = lageskontroll.IsGeom == "0" || string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? createSdoGeometryFromRelationalData(lageskontroll) + skilje : "";


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
                                                             "'" + lageskontroll.tmpGuidKey.ToString().Replace("-","") + "'" +
                                                             ")";

            return sql;
        }

        private static string sqlUpdateLageskontroll(Bygglovsbeslut lageskontroll)
        {
            string skilje = ", ";

            string geometry = lageskontroll.IsGeom == "0" || string.IsNullOrWhiteSpace(lageskontroll.IsGeom) ? skilje + "f.geom = " + createSdoGeometryFromRelationalData(lageskontroll) + " " : "";


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

        private static DataTable createEmptyBygglovsbesllutsTable()
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

        private static string createSdoGeometryFromRelationalData(Bygglovsbeslut bygglovsbeslut)
        {
            string geometry = "";
            string sdoPrefix = "SDO_GEOMETRY(";
            string sdoSuffix = ")";
            string sdoGType = "";
            string sdoSRID = "";
            string sdoPoint = "";
            string sdoElemt = "";
            string sdoOrdinates = "";

            if (!string.IsNullOrWhiteSpace(bygglovsbeslut.Adress))
            {
                string sql = "SELECT a.geom.SDO_POINT.X AS X, a.geom.SDO_POINT.Y AS Y FROM lkr_gis.gis_v_beladress a WHERE a.adressplats_id = '" + bygglovsbeslut.Adress + "'";

                DataTable dt = new DataTable();
                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sql, con);
                OleDbDataReader dr;

                com.Connection.Open();
                dr = com.ExecuteReader();

                dt.Load(dr);

                dr.Close();
                dr.Dispose();

                if (dt.Rows.Count > 0)
                {
                    coord2SdoGeometryAsString(ref geometry, sdoPrefix, sdoSuffix, ref sdoGType, ref sdoSRID, ref sdoPoint, ref sdoElemt, ref sdoOrdinates, dt, false);
                }

                dt.Dispose();
            }
            else if (!string.IsNullOrWhiteSpace(bygglovsbeslut.AdressOmr))
            {
                // Sorteringsordning viktig för uppbyggnad av linje troligt följande adressområdet
                string sql = "SELECT b.geom.SDO_POINT.X AS X, b.geom.SDO_POINT.Y AS Y, b.adressplats_id AS adressplats_id, b.adressplats AS adressplats, b.littera AS littera " +
                             "FROM   lkr_gis.gis_v_adressomrade a, " +
                             "       lkr_gis.gis_v_beladress b " +
                             "WHERE  UPPER(a.adressomrade) = UPPER(b.adressomr) " +
                             "AND    a.adressomrades_id = '" + bygglovsbeslut.AdressOmr + "'" +
                             "ORDER BY TO_NUMBER(b.adressplats) ASC, b.littera ASC";

                DataTable dt = new DataTable();
                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sql, con);
                OleDbDataReader dr;

                com.Connection.Open();
                dr = com.ExecuteReader();

                dt.Load(dr);

                dr.Close();
                dr.Dispose();

                if (dt.Rows.Count > 0)
                {
                    coord2SdoGeometryAsString(ref geometry, sdoPrefix, sdoSuffix, ref sdoGType, ref sdoSRID, ref sdoPoint, ref sdoElemt, ref sdoOrdinates, dt, false);
                }

                dt.Dispose();
            }
            else if (!string.IsNullOrWhiteSpace(bygglovsbeslut.Fastighet.ToString()))
            {
                // Extraherar yttre elementet i polygonen (SDO_UTIL.EXTRACT(f.geom, 1, 1)) för att hanterar polygoner med hål (hål plockas bort)
                // Extraherar koordinaterna från MDSYS.SDO_GEOMETRY (SDO_UTIL.GETVERTICES())
                string sql = "SELECT t.X X, t.Y Y " +
                             "FROM lkr_gis.gis_v_fastytor f, " +
                             "     TABLE(SDO_UTIL.GETVERTICES(SDO_UTIL.EXTRACT(f.geom, 1, 1))) t " +
                             "WHERE fastighet_id = '" + bygglovsbeslut.Fastighet + "'";

                DataTable dt = new DataTable();
                OleDbConnection con = GetOleDbConncection();
                OleDbCommand com = new OleDbCommand(sql, con);
                OleDbDataReader dr;

                com.Connection.Open();
                dr = com.ExecuteReader();

                dt.Load(dr);

                dr.Close();
                dr.Dispose();

                if (dt.Rows.Count > 0)
                {
                    // Kontroll om att minst tre koordinater existerar (minst 4 då första och sista är samma). Annars går polygon ej att konstrueras.
                    bool isPolygon = false;
                    if (dt.Rows.Count > 3)
                    {
                        isPolygon = true;
                    }

                    coord2SdoGeometryAsString(ref geometry, sdoPrefix, sdoSuffix, ref sdoGType, ref sdoSRID, ref sdoPoint, ref sdoElemt, ref sdoOrdinates, dt, isPolygon);
                }

                dt.Dispose();
            }

            return geometry;
        }

        private static void coord2SdoGeometryAsString(ref string geometry, string sdoPrefix, string sdoSuffix, ref string sdoGType, ref string sdoSRID, ref string sdoPoint, ref string sdoElemt, ref string sdoOrdinates, DataTable dt, bool isPolygon)
        {
            sdoSRID = "NULL";

            if (dt.Rows.Count == 1)
            {
                // Punkt, vanlig enkel
                sdoGType = GeometrySdoGType.Point2D;
                sdoPoint = "SDO_POINT_TYPE(" + dt.Rows[0]["X"].ToString().Replace(",", ".") + "," + dt.Rows[0]["Y"].ToString().Replace(",", ".") + ",NULL)";
                sdoElemt = "NULL";

                sdoOrdinates = "NULL";
            }
            else
            {
                sdoPoint = "NULL";

                // Linje, noder kopplade med raka segment
                if (isPolygon)
                {
                    sdoGType = GeometrySdoGType.Polygon2D;
                    sdoElemt = "SDO_ELEM_INFO_ARRAY(1,1003,1)";
                }
                else
                {
                    sdoGType = GeometrySdoGType.Line2D;
                    sdoElemt = "SDO_ELEM_INFO_ARRAY(1,2,1)";
                }

                string coordinates = "";
                string tmpX, tmpY;
                foreach (DataRow row in dt.Rows)
                {
                    tmpX = row["X"].ToString().Replace(",", ".");
                    tmpY = row["Y"].ToString().Replace(",", ".");
                    if (coordinates == "")
                    {
                        coordinates = tmpX + "," + tmpY;
                    }
                    else
                    {
                        coordinates += ", " + tmpX + "," + tmpY;
                    }
                }
                sdoOrdinates = "sdo_ordinate_array (" + coordinates + ")";
            }

            geometry = sdoPrefix +
                sdoGType + "," +
                sdoSRID + "," +
                sdoPoint + "," +
                sdoElemt + "," +
                sdoOrdinates +
                sdoSuffix;
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

    public static class GeometrySdoGType
    {
        public static string Point2D { get { return "2001"; } }
        public static string Line2D { get { return "2002"; } }
        public static string Polygon2D { get { return "2003"; } }
        public static string Point3D { get { return "3001"; } }
        public static string Line3D { get { return "3002"; } }
        public static string Polygon3D { get { return "3003"; } }
    }
}