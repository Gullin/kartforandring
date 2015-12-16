using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Kartforandring
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
                    var sortExpressions = new List<Tuple<string, string>>();
                    string[] columns = jtSorting.Split(new char[] { ',' });
                    bool isDiarieSorted = false;
                    foreach (string column in columns)
                    {
                        string[] sorter = column.Trim().Split(new char[] { ' ' }, 2);
                        string fieldName = sorter[0].Trim();
                        string sortOrder = (sorter.Length > 1) ? sorter[1].Trim().ToLower() : "asc";
                        if ((sortOrder != "asc") && (sortOrder != "desc"))
                        {
                            throw new ArgumentException("Invalid sorting order");
                        }

                        
                        if (fieldName == "Diarie")
                        {
                            sortExpressions.Add(new Tuple<string, string>("DiarieAr", sortOrder));
                            sortExpressions.Add(new Tuple<string, string>("DiarieSerialNbr", sortOrder));
                            isDiarieSorted = true;
                        }
                        else
                        {
                            sortExpressions.Add(new Tuple<string, string>(fieldName, sortOrder));
                        }
                    }

                    IList<Bygglovsbeslut> currentListBygglovsbeslut = (List<Bygglovsbeslut>)this.Records;
                    if (isDiarieSorted)
                    {
                        // Konvertera objekt Bygglovsbeslut till objekt BygglovsbeslutDiarie
                        IList<BygglovsbeslutDiarie> sortingListBygglovsbeslutSplitDiarie = new List<BygglovsbeslutDiarie>();
                        string diariePattern = @"^(19\d{2}\.\d{1,4})$|^(20\d{2}\.\d{1,4})$";
                        foreach (Bygglovsbeslut bygglovBeslut in currentListBygglovsbeslut)
                        {
                            Type tBB = bygglovBeslut.GetType();
                            BygglovsbeslutDiarie bygglovsbeslutDiarie = new BygglovsbeslutDiarie();
                            Type tBBD = bygglovsbeslutDiarie.GetType();
                            foreach (PropertyInfo piBB in tBB.GetProperties())
                            {
                                if (piBB.Name == "Diarie")
                                {
                                    foreach (PropertyInfo piBBD in tBBD.GetProperties())
                                    {
                                        if (piBBD.Name == "Diarie")
                                        {
                                            piBBD.SetValue(bygglovsbeslutDiarie, piBB.GetValue(bygglovBeslut));
                                        }

                                        string diarie = piBB.GetValue(bygglovBeslut).ToString();
                                        if (System.Text.RegularExpressions.Regex.IsMatch(diarie, diariePattern))
                                        {
                                            string[] yearAndNbr = diarie.Split('.');
                                            if (piBBD.Name == "DiarieAr")
                                            {
                                                if (!string.IsNullOrWhiteSpace(yearAndNbr[0]))
                                                {
                                                    piBBD.SetValue(bygglovsbeslutDiarie, Convert.ToInt32(yearAndNbr[0]));
                                                }
                                            }
                                            if (piBBD.Name == "DiarieSerialNbr")
                                            {
                                                if (!string.IsNullOrWhiteSpace(yearAndNbr[1]))
                                                {
                                                    piBBD.SetValue(bygglovsbeslutDiarie, Convert.ToInt32(yearAndNbr[1]));
                                                }
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    foreach (PropertyInfo piBBD in tBBD.GetProperties())
                                    {
                                        if (piBBD.Name == piBB.Name && piBBD.PropertyType.Name == piBB.PropertyType.Name)
                                        {
                                            piBBD.SetValue(bygglovsbeslutDiarie, piBB.GetValue(bygglovBeslut));
                                        }
                                    }
                                }
                            }

                            sortingListBygglovsbeslutSplitDiarie.Add(bygglovsbeslutDiarie);
                        }

                        // Sortera bygglovsbeslut med diarie delat på år och löpnummer
                        sortingListBygglovsbeslutSplitDiarie = UtilityDynamicLinqMultiSorting.MultipleSort<BygglovsbeslutDiarie>(
                            sortingListBygglovsbeslutSplitDiarie,
                            sortExpressions)
                            .ToList<BygglovsbeslutDiarie>();

                        // Konvertera objekt BygglovsbeslutDiarie till objekt Bygglovsbeslut
                        IList<Bygglovsbeslut> sortedListBygglovsbeslut = new List<Bygglovsbeslut>();
                        foreach (BygglovsbeslutDiarie bygglovsbeslutDiarie in sortingListBygglovsbeslutSplitDiarie)
                        {
                            Type tBBD = bygglovsbeslutDiarie.GetType();
                            Bygglovsbeslut bygglovsbeslut = new Bygglovsbeslut();
                            Type tBB = bygglovsbeslut.GetType();
                            foreach (PropertyInfo piBBD in tBBD.GetProperties())
                            {
                                foreach (PropertyInfo piBB in tBB.GetProperties())
                                {
                                    if (piBBD.Name == piBB.Name && piBBD.PropertyType.Name == piBB.PropertyType.Name)
                                    {
                                        piBB.SetValue(bygglovsbeslut, piBB.GetValue(bygglovsbeslutDiarie));
                                    }
                                }
                            }
                            sortedListBygglovsbeslut.Add(bygglovsbeslut);
                        }

                        return this.Records = sortedListBygglovsbeslut;
                    }
                    else
                    {
                        return this.Records = UtilityDynamicLinqMultiSorting.MultipleSort<Bygglovsbeslut>(
                            currentListBygglovsbeslut,
                            sortExpressions)
                            .ToList<Bygglovsbeslut>();
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
        public Guid tmpGuidKey { get; set; }
        public string IsGeom { get; set; }
        public char[] LevelOfPosition { get; set; }
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

                // Består av 5 tecken
                // Position 1 = Övrigt
                // Position 2 = Adressområde
                // Position 3 = Fastighet
                // Position 4 = Adress
                // Position 5 = Objektets
                if (!Convert.IsDBNull(row["LEVEL_OF_POSITION"]))
                {
                    string levelOfPosition = row["LEVEL_OF_POSITION"].ToString();
                    if(levelOfPosition.Length != 5)
                    {
                        throw new Exception("Antalet tecken som signalerar kvalitet och status i positionering är fel.");
                    }
                    else
                        Andring.LevelOfPosition = levelOfPosition.ToCharArray();
                }
                else
                {
                    Andring.LevelOfPosition = new char[] { '0', '0', '0', '0', '0' };
                }

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

                // Består av 5 tecken
                // Position 1 = Övrigt
                // Position 2 = Adressområde
                // Position 3 = Fastighet
                // Position 4 = Adress
                // Position 5 = Objektets
                if (!Convert.IsDBNull(row["LEVEL_OF_POSITION"]))
                {
                    string levelOfPosition = row["LEVEL_OF_POSITION"].ToString();
                    if (levelOfPosition.Length != 5)
                    {
                        throw new Exception("Antalet tecken som signalerar kvalitet och status i positionering är fel.");
                    }
                    else
                        Andring.LevelOfPosition = levelOfPosition.ToCharArray();
                }
                else
                {
                    Andring.LevelOfPosition = new char[] { '0', '0', '0', '0', '0' };
                }

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


    public class BygglovsbeslutDiarie : Bygglovsbeslut
    {
        public int? DiarieAr { get; set; }
        public int? DiarieSerialNbr { get; set; }
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
        public new string Value { get; set; }

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
                Domain.Value = row["ID"].ToString();
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
        public new string Value { get; set; }

        public IList<DomainBelagenhetsAdress> GetDomains()
        {
            DataTable dt = DB.GetBelagenhetsAdressDomain();

            return MakeForandrings(dt);
        }

        public IList<DomainBelagenhetsAdress> GetDomains(string AdressOmrId)
        {
            DataTable dt = DB.GetBelagenhetsAdressDomain(AdressOmrId);

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
                Domain.Value = row["ID"].ToString();
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



    public class BristStat
    {
        public int Number { get; set; }
        public int ValueId1 { get; set; }
        public string ValueText1 { get; set; }
        public int ValueId2 { get; set; }
        public string ValueText2 { get; set; }

        internal IList<BristStat> GetAllBristGeometryIsNull()
        {
            DataTable dt = DB.GetAllBristGeometryIsNull();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristUtforareIsNullWhenStarted()
        {
            DataTable dt = DB.GetAllBristUtforareIsNullWhenStarted();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristDiarieIsNull()
        {
            DataTable dt = DB.GetAllBristDiarieIsNull();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristDiarieWrongFormat()
        {
            DataTable dt = DB.GetAllBristDiarieWrongFormat();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristObjTypNotInherited()
        {
            DataTable dt = DB.GetAllBristObjTypNotInherited();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristObjIsNull()
        {
            DataTable dt = DB.GetAllBristObjIsNull();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristTypIsNull()
        {
            DataTable dt = DB.GetAllBristTypIsNull();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristBygglovMissingReason()
        {
            DataTable dt = DB.GetAllBristBygglovMissingReason();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristBygglovsbevakningarFinishNotDocumentedExecuted()
        {
            DataTable dt = DB.GetAllBristBygglovsbevakningarFinishNotDocumentedExecuted();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristBygglovRedundant()
        {
            DataTable dt = DB.GetAllBristBygglovRedundant();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristBestalldUtsattningNoBevakning()
        {
            DataTable dt = DB.GetAllBristBestalldUtsattningNoBevakning();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllBristBestalldLageskontrollNoBevakning()
        {
            DataTable dt = DB.GetAllBristBestalldLageskontrollNoBevakning();

            return MakeList(dt);
        }

        internal IList<BristStat> GetAllVarningExternBestalldLageskontrollInTime()
        {
            DataTable dt = DB.GetAllVarningExternBestalldLageskontrollInTime();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllVarningExternBestalldLageskontrollInTime(int antalDagar)
        {
            DataTable dt = DB.GetAllVarningExternBestalldLageskontrollInTime(antalDagar);

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllVarningInternBestalldLageskontrollInTime()
        {
            DataTable dt = DB.GetAllVarningInternBestalldLageskontrollInTime();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllVarningInternBestalldLageskontrollInTime(int antalDagar)
        {
            DataTable dt = DB.GetAllVarningInternBestalldLageskontrollInTime(antalDagar);

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllVarningUtsattningWithLageskontrollUtfard()
        {
            DataTable dt = DB.GetAllVarningUtsattningWithLageskontrollUtfard();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllVarningLageskontrollUtfardAndamalNotUtfard()
        {
            DataTable dt = DB.GetAllVarningLageskontrollUtfardAndamalNotUtfard();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllVarningLogisktRaderade()
        {
            DataTable dt = DB.GetAllVarningLogisktRaderade();

            return MakeList(dt);
        }

        internal IList<BristStat> GetAllStatusChangedSince()
        {
            DataTable dt = DB.GetAllStatusChangedSince();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllStatusChangedSince(string dateFilter)
        {
            DataTable dt = DB.GetAllStatusChangedSince(dateFilter);

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllStatusNbrOfChangesObjkod()
        {
            DataTable dt = DB.GetAllStatusNbrOfChangesObjkod();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllStatusNbrOfChangesTypkod()
        {
            DataTable dt = DB.GetAllStatusNbrOfChangesTypkod();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllStatusNbrTotal()
        {
            DataTable dt = DB.GetAllStatusNbrTotal();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllStatusNbrAktiva()
        {
            DataTable dt = DB.GetAllStatusNbrAktiva();

            return MakeList(dt);
        }

        internal IList<BristStat> GetAllFelBygglovNoBygglovsbevakning()
        {
            DataTable dt = DB.GetAllFelBygglovNoBygglovsbevakning();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllFelBygglovsbevakningNoBygglov()
        {
            DataTable dt = DB.GetAllFelBygglovsbevakningNoBygglov();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllFelBestalldUtsattningNoBevakning()
        {
            DataTable dt = DB.GetAllFelBestalldUtsattningNoBevakning();

            return MakeList(dt);
        }
        internal IList<BristStat> GetAllFelBestalldLageskontrollNoBevakning()
        {
            DataTable dt = DB.GetAllFelBestalldLageskontrollNoBevakning();

            return MakeList(dt);
        }


        private IList<BristStat> MakeList(DataTable dt)
        {
            DataColumnCollection dcCollection = dt.Columns;
            IList<BristStat> list = new List<BristStat>();
            // Ordningen av if-satser är viktig eftersom samma kolumnnamn förekommer i 
            if (dcCollection.Contains("OBJ_KOD") && dcCollection.Contains("TYP_KOD"))
            {
                foreach (DataRow row in dt.Rows)
                    list.Add(MakeListItemNumberObjTyp(row));
                return list;
            }
            if (dcCollection.Contains("OBJ_KOD"))
            {
                foreach (DataRow row in dt.Rows)
                    list.Add(MakeListItemNumberObj(row));
                return list;
            }
            if (dcCollection.Contains("DIGIT") && dcCollection.Contains("VALUE_TEXT"))
            {
                foreach (DataRow row in dt.Rows)
                    list.Add(MakeListItemNumberValueText(row));
                return list;
            }
            if (dcCollection.Contains("DIGIT"))
            {
                foreach (DataRow row in dt.Rows)
                    list.Add(MakeListItemNumber(row));
                return list;
            }
            else
            {
                throw new Exception("Ingen matchning av kolumner.");
            }
        }

        private BristStat MakeListItemNumber(DataRow row)
        {
            BristStat Brist = new BristStat();
            try
            {
                if (!Convert.IsDBNull(row["DIGIT"]))
                {
                    Brist.Number = int.Parse(row["DIGIT"].ToString());
                }
                return Brist;
            }
            catch
            {
                throw;
            }
        }
        private BristStat MakeListItemNumberValueText(DataRow row)
        {
            BristStat Brist = new BristStat();
            try
            {
                if (!Convert.IsDBNull(row["DIGIT"]))
                {
                    Brist.Number = int.Parse(row["DIGIT"].ToString());
                }
                if (!Convert.IsDBNull(row["VALUE_TEXT"]))
                {
                    Brist.ValueText1 = row["VALUE_TEXT"].ToString();
                }
                return Brist;
            }
            catch
            {
                throw;
            }
        }
        private BristStat MakeListItemNumberObj(DataRow row)
        {
            BristStat Brist = new BristStat();
            try
            {
                if (!Convert.IsDBNull(row["DIGIT"]))
                {
                    Brist.Number = int.Parse(row["DIGIT"].ToString());
                }
                if (!Convert.IsDBNull(row["OBJ_KOD"]))
                {
                    Brist.ValueId1 = int.Parse(row["OBJ_KOD"].ToString());
                }
                if (!Convert.IsDBNull(row["OBJ_VALUE"]))
                {
                    Brist.ValueText1 = row["OBJ_VALUE"].ToString();
                }
                return Brist;
            }
            catch
            {
                throw;
            }
        }
        private BristStat MakeListItemNumberObjTyp(DataRow row)
        {
            BristStat Brist = new BristStat();
            try
            {
                if (!Convert.IsDBNull(row["DIGIT"]))
                {
                    Brist.Number = int.Parse(row["DIGIT"].ToString());
                }
                if (!Convert.IsDBNull(row["OBJ_KOD"]))
                {
                    Brist.ValueId1 = int.Parse(row["OBJ_KOD"].ToString());
                }
                if (!Convert.IsDBNull(row["OBJ_VALUE"]))
                {
                    Brist.ValueText1 = row["OBJ_VALUE"].ToString();
                }
                if (!Convert.IsDBNull(row["TYP_KOD"]))
                {
                    Brist.ValueId2 = int.Parse(row["TYP_KOD"].ToString());
                }
                if (!Convert.IsDBNull(row["TYP_VALUE"]))
                {
                    Brist.ValueText2 = row["TYP_VALUE"].ToString();
                }
                return Brist;
            }
            catch
            {
                throw;
            }
        }
    }
}