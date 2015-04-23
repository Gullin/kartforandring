using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;

namespace kartforandring
{
    /// <summary>
    /// Klass anpassad för att uppfylla javascript-ramverket jTable's JSON-specifikation.
    /// egenskaperna Result, Records och Message är specificerade begrepp.
    /// Innehåller primärt lista av objektet Bygglovsbeslut.
    /// </summary>
    public class jTableRecordBygglovsbeslut
    {
        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Ett objekt innehållande informationen som ska utgöra rader i jTable
        public Bygglovsbeslut Record { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<Bygglovsbeslut> Records { get; set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

        public IList<Bygglovsbeslut> sortBygglovsbeslut(string jtSorting)
        {
            try
            {
                if (!string.IsNullOrEmpty(jtSorting))
                {
                    string[] sorter = jtSorting.Split(new char[] { ' ' });
                    IList<Bygglovsbeslut> sortingListPlans = (List<Bygglovsbeslut>)this.Records;

                    if (sorter[1].ToString() == "ASC")
                    {

                        if (sorter[0].ToString() == "Fid")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Fid).ToList();
                        }
                        else if (sorter[0].ToString() == "IsGeom")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.IsGeom).ToList();
                        }
                        else if (sorter[0].ToString() == "KarObj")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.KarObj).ToList();
                        }
                        else if (sorter[0].ToString() == "KarObjText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.KarObjText).ToList();
                        }
                        else if (sorter[0].ToString() == "KarTyp")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.KarTyp).ToList();
                        }
                        else if (sorter[0].ToString() == "KarTypText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.KarTypText).ToList();
                        }
                        else if (sorter[0].ToString() == "UserModified")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.UserModified).ToList();
                        }
                        else if (sorter[0].ToString() == "DateModified")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.DateModified).ToList();
                        }
                        else if (sorter[0].ToString() == "Notering")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Notering).ToList();
                        }
                        else if (sorter[0].ToString() == "Beskrivning")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Beskrivning).ToList();
                        }
                        else if (sorter[0].ToString() == "Inkommit")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Inkommit).ToList();
                        }
                        else if (sorter[0].ToString() == "Paborjat")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Paborjat).ToList();
                        }
                        else if (sorter[0].ToString() == "Utforare")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Utforare).ToList();
                        }
                        else if (sorter[0].ToString() == "Avslutat")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Avslutat).ToList();
                        }
                        else if (sorter[0].ToString() == "Adress")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Adress).ToList();
                        }
                        else if (sorter[0].ToString() == "AdressText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AdressText).ToList();
                        }
                        else if (sorter[0].ToString() == "Fastighet")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Fastighet).ToList();
                        }
                        else if (sorter[0].ToString() == "FastighetText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.FastighetText).ToList();
                        }
                        else if (sorter[0].ToString() == "AdressOmr")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AdressOmr).ToList();
                        }
                        else if (sorter[0].ToString() == "AdressOmrText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AdressOmrText).ToList();
                        }
                        else if (sorter[0].ToString() == "PlatsOvrigt")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.PlatsOvrigt).ToList();
                        }
                        else if (sorter[0].ToString() == "Diarie")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Diarie).ToList();
                        }
                        else if (sorter[0].ToString() == "Utsattning")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Utsattning).ToList();
                        }
                        else if (sorter[0].ToString() == "UtsattningText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.UtsattningText).ToList();
                        }
                        else if (sorter[0].ToString() == "UtsattningBestallning")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.UtsattningBestallning).ToList();
                        }
                        else if (sorter[0].ToString() == "UtsattningBestallningText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.UtsattningBestallningText).ToList();
                        }
                        else if (sorter[0].ToString() == "Lageskontroll")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Lageskontroll).ToList();
                        }
                        else if (sorter[0].ToString() == "LageskontrollText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.LageskontrollText).ToList();
                        }
                        else if (sorter[0].ToString() == "LageskontrollBestallning")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.LageskontrollBestallning).ToList();
                        }
                        else if (sorter[0].ToString() == "LageskontrollBestallningText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.LageskontrollBestallningText).ToList();
                        }
                        else if (sorter[0].ToString() == "Attefall")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Attefall).ToList();
                        }
                        else if (sorter[0].ToString() == "AttefallText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AttefallText).ToList();
                        }
                        else if (sorter[0].ToString() == "Riv")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Riv).ToList();
                        }
                        else if (sorter[0].ToString() == "RivText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.RivText).ToList();
                        }
                        else if (sorter[0].ToString() == "Andamal")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.Andamal).ToList();
                        }
                        else if (sorter[0].ToString() == "AndamalText")
                        {
                            return this.Records = sortingListPlans.OrderBy(p => p.AndamalText).ToList();
                        }
                        else
                        {
                            throw new Exception("Ingen sorteringskolumn definierad för ASC-sortering");
                        }
                    }
                    else if (sorter[1].ToString() == "DESC")
                    {
                        if (sorter[0].ToString() == "Fid")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Fid).ToList();
                        }
                        else if (sorter[0].ToString() == "IsGeom")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.IsGeom).ToList();
                        }
                        else if (sorter[0].ToString() == "KarObj")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.KarObj).ToList();
                        }
                        else if (sorter[0].ToString() == "KarObjText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.KarObjText).ToList();
                        }
                        else if (sorter[0].ToString() == "KarTyp")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.KarTyp).ToList();
                        }
                        else if (sorter[0].ToString() == "KarTypText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.KarTypText).ToList();
                        }
                        else if (sorter[0].ToString() == "UserModified")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.UserModified).ToList();
                        }
                        else if (sorter[0].ToString() == "DateModified")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.DateModified).ToList();
                        }
                        else if (sorter[0].ToString() == "Notering")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Notering).ToList();
                        }
                        else if (sorter[0].ToString() == "Beskrivning")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Beskrivning).ToList();
                        }
                        else if (sorter[0].ToString() == "Inkommit")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Inkommit).ToList();
                        }
                        else if (sorter[0].ToString() == "Paborjat")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Paborjat).ToList();
                        }
                        else if (sorter[0].ToString() == "Utforare")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Utforare).ToList();
                        }
                        else if (sorter[0].ToString() == "Avslutat")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Avslutat).ToList();
                        }
                        else if (sorter[0].ToString() == "Adress")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Adress).ToList();
                        }
                        else if (sorter[0].ToString() == "AdressText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AdressText).ToList();
                        }
                        else if (sorter[0].ToString() == "Fastighet")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Fastighet).ToList();
                        }
                        else if (sorter[0].ToString() == "FastighetText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.FastighetText).ToList();
                        }
                        else if (sorter[0].ToString() == "AdressOmr")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AdressOmr).ToList();
                        }
                        else if (sorter[0].ToString() == "AdressOmrText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AdressOmrText).ToList();
                        }
                        else if (sorter[0].ToString() == "PlatsOvrigt")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.PlatsOvrigt).ToList();
                        }
                        else if (sorter[0].ToString() == "Diarie")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Diarie).ToList();
                        }
                        else if (sorter[0].ToString() == "Utsattning")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Utsattning).ToList();
                        }
                        else if (sorter[0].ToString() == "UtsattningText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.UtsattningText).ToList();
                        }
                        else if (sorter[0].ToString() == "UtsattningBestallning")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.UtsattningBestallning).ToList();
                        }
                        else if (sorter[0].ToString() == "UtsattningBestallningText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.UtsattningBestallningText).ToList();
                        }
                        else if (sorter[0].ToString() == "Lageskontroll")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Lageskontroll).ToList();
                        }
                        else if (sorter[0].ToString() == "LageskontrollText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.LageskontrollText).ToList();
                        }
                        else if (sorter[0].ToString() == "LageskontrollBestallning")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.LageskontrollBestallning).ToList();
                        }
                        else if (sorter[0].ToString() == "LageskontrollBestallningText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.LageskontrollBestallningText).ToList();
                        }
                        else if (sorter[0].ToString() == "Attefall")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Attefall).ToList();
                        }
                        else if (sorter[0].ToString() == "AttefallText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AttefallText).ToList();
                        }
                        else if (sorter[0].ToString() == "Riv")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Riv).ToList();
                        }
                        else if (sorter[0].ToString() == "RivText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.RivText).ToList();
                        }
                        else if (sorter[0].ToString() == "Andamal")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.Andamal).ToList();
                        }
                        else if (sorter[0].ToString() == "AndamalText")
                        {
                            return this.Records = sortingListPlans.OrderByDescending(p => p.AndamalText).ToList();
                        }
                        else
                        {
                            throw new Exception("Ingen sorteringskolumn definierad för DESC-sortering");
                        }
                    }
                    else
                    {
                        throw new Exception("Ingen sortering definierad (ASC|DESC)");
                    }
                }
                else
                {
                    throw new Exception("Inga parametervärden för sortering");
                }
            }
            catch
            {
                throw;
            }
        }

    }

    public class jTableOptionLageskontrollOrdering
    {

        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<DomainLageskontrollOrdering> Options { get; set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

    }

    public class jTableOptionAdressOmrade
    {

        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<DomainAdressOmrade> Options { get; set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

    }

    public class jTableOptionBelagenhetsAdress
    {

        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<DomainBelagenhetsAdress> Options { get; set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

    }
    
    public class jTableOptionFastighet
    {

        // Antar endast värden OK eller ERROR
        public string Result { get; set; }
        // Lista med objekt innehållande informationen som ska utgöra rader i jTable
        public IList<DomainFastighet> Options { get; set; }
        // Innehåller värde när egenskapen Result i sin tur har värde "ERROR"
        public string Message { get; set; }

    }

    public class jTableDomainBas
    {
        public Int32 Value { get; set; }
        public string DisplayText { get; set; }
    }

    

    public class ForandringarBas
    {
        public int Fid { get; set; }
        public string IsGeom { get; set; }
        public int? KarObj { get; set; }
        public string KarObjText { get; set; }
        public int? KarTyp { get; set; }
        public string KarTypText { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public string Notering { get; set; }
        public string Beskrivning { get; set; }
        public DateTime? Inkommit { get; set; }
        public DateTime? Paborjat { get; set; }
        public string Utforare { get; set; }
        public DateTime? Avslutat { get; set; }
        public string Adress { get; set; }
        public string AdressText { get; set; }
        public int? Fastighet { get; set; }
        public string FastighetText { get; set; }
        public string AdressOmr { get; set; }
        public string AdressOmrText { get; set; }
        public string PlatsOvrigt { get; set; }
    }
    

    public class Forandringar : ForandringarBas
	{
        public IList<Forandringar> GetAllaForandrings()
        {
            DataTable dt = DB.GetAllaKartforandringar();

            return MakeForandrings(dt);
        }

        private IList<Forandringar> MakeForandrings(DataTable dt)
        {
            IList<Forandringar> list = new List<Forandringar>();
            foreach (DataRow row in dt.Rows)
                list.Add(MakeForandring(row));

            return list;
        }

        private Forandringar MakeForandring(DataRow row)
        {
            Forandringar Andring = new Forandringar();

            if (!Convert.IsDBNull(row["FID"]))
            {
                Andring.Fid = int.Parse(row["FID"].ToString());
                Andring.Fastighet = Convert.IsDBNull(row["BEV_PLATS_FASTIGHET"]) ? (int?)null : int.Parse(row["BEV_PLATS_FASTIGHET"].ToString());
                Andring.IsGeom = Convert.IsDBNull(row["IS_GEOM"]) ? null : row["IS_GEOM"].ToString();
                Andring.KarObj = Convert.IsDBNull(row["KAR_OBJ"]) ? (int?)null : int.Parse(row["KAR_OBJ"].ToString());
                Andring.KarObjText = Convert.IsDBNull(row["KAR_OBJ_TEXT"]) ? null : row["KAR_OBJ_TEXT"].ToString();
                Andring.KarTyp = Convert.IsDBNull(row["KAR_TYP"]) ? (int?)null : int.Parse(row["KAR_TYP"].ToString());
                Andring.KarTypText = Convert.IsDBNull(row["KAR_TYP_TEXT"]) ? null : row["KAR_TYP_TEXT"].ToString();
                Andring.UserModified = Convert.IsDBNull(row["USER_MODIFIED"]) ? null : row["USER_MODIFIED"].ToString();
                Andring.DateModified = Convert.IsDBNull(row["DATE_MODIFIED"]) ? (DateTime?)null : DateTime.Parse(row["DATE_MODIFIED"].ToString());
                Andring.Notering = Convert.IsDBNull(row["BEV_NOTERING"]) ? null : row["BEV_NOTERING"].ToString();
                Andring.Beskrivning = Convert.IsDBNull(row["BEV_BESKRIVNING"]) ? null : row["BEV_BESKRIVNING"].ToString();
                Andring.Inkommit = Convert.IsDBNull(row["BEV_INKOMMET"]) ? (DateTime?)null : DateTime.Parse(row["BEV_INKOMMET"].ToString());
                Andring.Paborjat = Convert.IsDBNull(row["BEV_PABORJAT"]) ? (DateTime?)null : DateTime.Parse(row["BEV_PABORJAT"].ToString());
                Andring.Utforare = Convert.IsDBNull(row["BEV_UTFORARE"]) ? null : row["BEV_UTFORARE"].ToString();
                Andring.Avslutat = Convert.IsDBNull(row["BEV_AVSLUTAT"]) ? (DateTime?)null : DateTime.Parse(row["BEV_AVSLUTAT"].ToString());
                Andring.Adress = Convert.IsDBNull(row["BEV_PLATS_ADRESS"]) ? null : row["BEV_PLATS_ADRESS"].ToString();
                Andring.AdressText = Convert.IsDBNull(row["BEV_PLATS_ADRESS_TEXT"]) ? null : row["BEV_PLATS_ADRESS_TEXT"].ToString();
                Andring.Fastighet = Convert.IsDBNull(row["BEV_PLATS_FASTIGHET"]) ? (int?)null : int.Parse(row["BEV_PLATS_FASTIGHET"].ToString());
                Andring.FastighetText = Convert.IsDBNull(row["BEV_PLATS_FASTIGHET_TEXT"]) ? null : row["BEV_PLATS_FASTIGHET_TEXT"].ToString();
                Andring.AdressOmr = Convert.IsDBNull(row["BEV_PLATS_ADRESSOMR"]) ? null : row["BEV_PLATS_ADRESSOMR"].ToString();
                Andring.AdressOmrText = Convert.IsDBNull(row["BEV_PLATS_ADRESSOMR_TEXT"]) ? null : row["BEV_PLATS_ADRESSOMR_TEXT"].ToString();
                Andring.PlatsOvrigt = Convert.IsDBNull(row["BEV_PLATS_OVRIGT"]) ? null : row["BEV_PLATS_OVRIGT"].ToString();

                return Andring;
            }
            else
            {
                throw new Exception("Ingen nyckel för post.");
            }
        }
	}


    public class Bygglovsbeslut : ForandringarBas
    {
        public string Diarie { get; set; }
        public int? Utsattning { get; set; }
        public string UtsattningText { get; set; }
        public int? UtsattningBestallning { get; set; }
        public string UtsattningBestallningText { get; set; }
        public int? Lageskontroll { get; set; }
        public string LageskontrollText { get; set; }
        public int? LageskontrollBestallning { get; set; }
        public string LageskontrollBestallningText { get; set; }
        public int? Attefall { get; set; }
        public string AttefallText { get; set; }
        public int? Riv { get; set; }
        public string RivText { get; set; }
        public int? Andamal { get; set; }
        public string AndamalText { get; set; }

        public IList<Bygglovsbeslut> GetAllaBygglovsbeslut()
        {
            DataTable dt = DB.GetBygglovsbeslut();

            return MakeForandrings(dt);
        }

        public IList<Bygglovsbeslut> GetBygglovsbeslutMedLageskontroll()
        {
            DataTable dt = DB.GetBygglovsbeslutMedLageskontroll();

            return MakeForandrings(dt);
        }

        public Bygglovsbeslut AddLageskontroll(Bygglovsbeslut lageskontroll)
        {
            try
            {
                DataTable dt = DB.AddLageskontroll(lageskontroll);

                DataRow dr = dt.NewRow();
                dr = dt.Rows[0];

                return MakeForandring(dr);
            }
            catch
            {
                throw;
            }
        }

        public Bygglovsbeslut UpdateLageskontroll(Bygglovsbeslut lageskontroll)
        {
            try
            {
                DataTable dt = DB.UpdateLageskontroll(lageskontroll);

                DataRow dr = dt.NewRow();
                dr = dt.Rows[0];

                return MakeForandring(dr);
            }
            catch
            {
                throw;
            }
        }


        public void DeleteLageskontroll(int fid)
        {
            try
            {
                DB.DeleteLageskontroll(fid);
            }
            catch
            {
                throw;
            }
        }

        private IList<Bygglovsbeslut> MakeForandrings(DataTable dt)
        {
            IList<Bygglovsbeslut> list = new List<Bygglovsbeslut>();
            foreach (DataRow row in dt.Rows)
                list.Add(MakeForandring(row));

            return list;
        }

        private Bygglovsbeslut MakeForandring(DataRow row)
        {
            Bygglovsbeslut Andring = new Bygglovsbeslut();

            if (!Convert.IsDBNull(row["FID"]))
            {
                Andring.Fid = int.Parse(row["FID"].ToString());
                Andring.Fastighet = Convert.IsDBNull(row["BEV_PLATS_FASTIGHET"]) ? (int?)null : int.Parse(row["BEV_PLATS_FASTIGHET"].ToString());
                Andring.IsGeom = Convert.IsDBNull(row["IS_GEOM"]) ? null : row["IS_GEOM"].ToString();
                Andring.KarObj = Convert.IsDBNull(row["KAR_OBJ"]) ? (int?)null : int.Parse(row["KAR_OBJ"].ToString());
                Andring.KarObjText = Convert.IsDBNull(row["KAR_OBJ_TEXT"]) ? null : row["KAR_OBJ_TEXT"].ToString();
                Andring.KarTyp = Convert.IsDBNull(row["KAR_TYP"]) ? (int?)null : int.Parse(row["KAR_TYP"].ToString());
                Andring.KarTypText = Convert.IsDBNull(row["KAR_TYP_TEXT"]) ? null : row["KAR_TYP_TEXT"].ToString();
                Andring.UserModified = Convert.IsDBNull(row["USER_MODIFIED"]) ? null : row["USER_MODIFIED"].ToString();
                Andring.DateModified = Convert.IsDBNull(row["DATE_MODIFIED"]) ? (DateTime?)null : DateTime.Parse(row["DATE_MODIFIED"].ToString());
                Andring.Notering = Convert.IsDBNull(row["BEV_NOTERING"]) ? null : row["BEV_NOTERING"].ToString();
                Andring.Beskrivning = Convert.IsDBNull(row["BEV_BESKRIVNING"]) ? null : row["BEV_BESKRIVNING"].ToString();
                Andring.Inkommit = Convert.IsDBNull(row["BEV_INKOMMET"]) ? (DateTime?)null : DateTime.Parse(row["BEV_INKOMMET"].ToString());
                Andring.Paborjat = Convert.IsDBNull(row["BEV_PABORJAT"]) ? (DateTime?)null : DateTime.Parse(row["BEV_PABORJAT"].ToString());
                Andring.Utforare = Convert.IsDBNull(row["BEV_UTFORARE"]) ? null : row["BEV_UTFORARE"].ToString();
                Andring.Avslutat = Convert.IsDBNull(row["BEV_AVSLUTAT"]) ? (DateTime?)null : DateTime.Parse(row["BEV_AVSLUTAT"].ToString());
                Andring.Adress = Convert.IsDBNull(row["BEV_PLATS_ADRESS"]) ? null : row["BEV_PLATS_ADRESS"].ToString();
                Andring.AdressText = Convert.IsDBNull(row["BEV_PLATS_ADRESS_TEXT"]) ? null : row["BEV_PLATS_ADRESS_TEXT"].ToString();
                Andring.Fastighet = Convert.IsDBNull(row["BEV_PLATS_FASTIGHET"]) ? (int?)null : int.Parse(row["BEV_PLATS_FASTIGHET"].ToString());
                Andring.FastighetText = Convert.IsDBNull(row["BEV_PLATS_FASTIGHET_TEXT"]) ? null : row["BEV_PLATS_FASTIGHET_TEXT"].ToString();
                Andring.AdressOmr = Convert.IsDBNull(row["BEV_PLATS_ADRESSOMR"]) ? null : row["BEV_PLATS_ADRESSOMR"].ToString();
                Andring.AdressOmrText = Convert.IsDBNull(row["BEV_PLATS_ADRESSOMR_TEXT"]) ? null : row["BEV_PLATS_ADRESSOMR_TEXT"].ToString();
                Andring.PlatsOvrigt = Convert.IsDBNull(row["BEV_PLATS_OVRIGT"]) ? null : row["BEV_PLATS_OVRIGT"].ToString();


                Andring.Diarie = Convert.IsDBNull(row["BEV_BYGGLOV_DIARIE"]) ? null : row["BEV_BYGGLOV_DIARIE"].ToString();
                Andring.Utsattning = Convert.IsDBNull(row["BEV_BYGGLOV_UTS"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_UTS"].ToString());
                Andring.UtsattningText = Convert.IsDBNull(row["BEV_BYGGLOV_UTS_TEXT"]) ? null : row["BEV_BYGGLOV_UTS_TEXT"].ToString();
                Andring.UtsattningBestallning = Convert.IsDBNull(row["BEV_BYGGLOV_UTS_BEST"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_UTS_BEST"].ToString());
                Andring.UtsattningBestallningText = Convert.IsDBNull(row["BEV_BYGGLOV_UTS_BEST_TEXT"]) ? null : row["BEV_BYGGLOV_UTS_BEST_TEXT"].ToString();
                Andring.Lageskontroll = Convert.IsDBNull(row["BEV_BYGGLOV_LAG"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_LAG"].ToString());
                Andring.LageskontrollText = Convert.IsDBNull(row["BEV_BYGGLOV_LAG_TEXT"]) ? null : row["BEV_BYGGLOV_LAG_TEXT"].ToString();
                Andring.LageskontrollBestallning = Convert.IsDBNull(row["BEV_BYGGLOV_LAG_BEST"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_LAG_BEST"].ToString());
                Andring.LageskontrollBestallningText = Convert.IsDBNull(row["BEV_BYGGLOV_LAG_BEST_TEXT"]) ? null : row["BEV_BYGGLOV_LAG_BEST_TEXT"].ToString();
                Andring.Attefall = Convert.IsDBNull(row["BEV_BYGGLOV_ATTEFALL"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_ATTEFALL"].ToString());
                Andring.AttefallText = Convert.IsDBNull(row["BEV_BYGGLOV_ATTEFALL_TEXT"]) ? null : row["BEV_BYGGLOV_ATTEFALL_TEXT"].ToString();
                Andring.Riv = Convert.IsDBNull(row["BEV_BYGGLOV_RIV"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_RIV"].ToString());
                Andring.RivText = Convert.IsDBNull(row["BEV_BYGGLOV_RIV_TEXT"]) ? null : row["BEV_BYGGLOV_RIV_TEXT"].ToString();
                Andring.Andamal = Convert.IsDBNull(row["BEV_BYGGLOV_ANDAMAL"]) ? (int?)null : int.Parse(row["BEV_BYGGLOV_ANDAMAL"].ToString());
                Andring.AndamalText = Convert.IsDBNull(row["BEV_BYGGLOV_ANDAMAL_TEXT"]) ? null : row["BEV_BYGGLOV_ANDAMAL_TEXT"].ToString();

                return Andring;
            }
            else
            {
                throw new Exception("Ingen nyckel för post.");
            }

        }
    }



    public class DomainLageskontrollOrdering : jTableDomainBas
    {
        public IList<DomainLageskontrollOrdering> GetDomains()
        {
            DataTable dt = DB.GetLageskontrollOrderingDomain();

            return MakeForandrings(dt);
        }

        private IList<DomainLageskontrollOrdering> MakeForandrings(DataTable dt)
        {
            IList<DomainLageskontrollOrdering> list = new List<DomainLageskontrollOrdering>();
            foreach (DataRow row in dt.Rows)
                list.Add(MakeForandring(row));

            return list;
        }

        private DomainLageskontrollOrdering MakeForandring(DataRow row)
        {
            DomainLageskontrollOrdering Domain = new DomainLageskontrollOrdering();

            if (!Convert.IsDBNull(row["ID"]))
            {
                Domain.Value = int.Parse(row["ID"].ToString());
                Domain.DisplayText = row["VALUE"].ToString();

                return Domain;
            }
            else
            {
                throw new Exception("Ingen nyckel för post.");
            }

        }
    }


    public class DomainAdressOmrade : jTableDomainBas
    {
        public IList<DomainAdressOmrade> GetDomains()
        {
            DataTable dt = DB.GetAdressOmradeDomain();

            return MakeForandrings(dt);
        }

        private IList<DomainAdressOmrade> MakeForandrings(DataTable dt)
        {
            IList<DomainAdressOmrade> list = new List<DomainAdressOmrade>();
            foreach (DataRow row in dt.Rows)
                list.Add(MakeForandring(row));

            return list;
        }

        private DomainAdressOmrade MakeForandring(DataRow row)
        {
            DomainAdressOmrade Domain = new DomainAdressOmrade();

            if (!Convert.IsDBNull(row["ID"]))
            {
                Domain.Value = int.Parse(row["ID"].ToString());
                Domain.DisplayText = row["VALUE"].ToString();

                return Domain;
            }
            else
            {
                throw new Exception("Ingen nyckel för post.");
            }

        }
    }


    public class DomainBelagenhetsAdress : jTableDomainBas
    {
        public IList<DomainBelagenhetsAdress> GetDomains()
        {
            DataTable dt = DB.GetBelagenhetsAdressDomain();

            return MakeForandrings(dt);
        }

        private IList<DomainBelagenhetsAdress> MakeForandrings(DataTable dt)
        {
            IList<DomainBelagenhetsAdress> list = new List<DomainBelagenhetsAdress>();
            foreach (DataRow row in dt.Rows)
                list.Add(MakeForandring(row));

            return list;
        }

        private DomainBelagenhetsAdress MakeForandring(DataRow row)
        {
            DomainBelagenhetsAdress Domain = new DomainBelagenhetsAdress();

            if (!Convert.IsDBNull(row["ID"]))
            {
                Domain.Value = int.Parse(row["ID"].ToString());
                Domain.DisplayText = row["VALUE"].ToString();

                return Domain;
            }
            else
            {
                throw new Exception("Ingen nyckel för post.");
            }

        }
    }


    public class DomainFastighet : jTableDomainBas
    {
        public IList<DomainFastighet> GetDomains()
        {
            DataTable dt = DB.GetFastighetDomain();

            return MakeForandrings(dt);
        }

        private IList<DomainFastighet> MakeForandrings(DataTable dt)
        {
            IList<DomainFastighet> list = new List<DomainFastighet>();
            foreach (DataRow row in dt.Rows)
                list.Add(MakeForandring(row));

            return list;
        }

        private DomainFastighet MakeForandring(DataRow row)
        {
            DomainFastighet Domain = new DomainFastighet();

            if (!Convert.IsDBNull(row["ID"]))
            {
                Domain.Value = int.Parse(row["ID"].ToString());
                Domain.DisplayText = row["VALUE"].ToString();

                return Domain;
            }
            else
            {
                throw new Exception("Ingen nyckel för post.");
            }

        }
    }


}