using System;
using System.Text;

namespace Kartforandring.Exceptions
{
    internal class GeneralChangeException : Exception
    {
        public GeneralChangeException()
            : base("LKR-00001",
                new Exception("Ändringen kunde INTE genomföras. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)")) { }
    }


	internal class BygglovExistException : Exception
	{
        public BygglovExistException()
            : base("LKR-00002",
                new Exception("Bygglovet existerar redan.")) { }
        public BygglovExistException(string diarie)
            : base("LKR-00002",
                new Exception(String.Format("Bygglovet ({0}) existerar redan.", diarie))) { }
	}


    internal class DeletionException : Exception
    {
        public DeletionException()
            : base("LKR-00003",
                new Exception("Raderingen kunde INTE genomföras. Posten verkar inte existera i databas. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)")) { }
        public DeletionException(string diarie)
            : base("LKR-00003",
            new Exception(String.Format("Raderingen kunde INTE genomföras. Posten ({0}) verkar inte existera i databas. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)", diarie))) { }
    }

    
    internal class UpdateException : Exception
    {
        public UpdateException()
            : base("LKR-00004",
                new Exception("Uppdateringen kunde INTE genomföras. Posten verkar inte existera i databas. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)")) { }
        public UpdateException(string diarie)
            : base("LKR-00004",
                new Exception(String.Format("Uppdateringen kunde INTE genomföras. Posten ({0}) verkar inte existera i databas. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)", diarie))) { }
    }


    internal class TempIdException : Exception
    {
        public TempIdException()
            : base("LKR-00005",
                new Exception("Kunde inte erhålla temporär databasnyckel vid skapande av ny post. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)")) { }
        public TempIdException(string diarie)
            : base("LKR-00005",
                new Exception(String.Format("Kunde inte erhålla databasnyckel vid skapande av ny post (temp-ID: {0}). Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)", diarie))) { }
    }


    internal class ChangeException : Exception
    {
        public ChangeException()
            : base("LKR-00006",
                new Exception("Ändringen kunde INTE genomföras. Databasen överensstämmer inte med tabell. Prova igen och om problemet återkommer kontakta MBK- och GIS-avdelningen (gis@landskrona.se)")) { }
    }


    //throw new Exception("LKR-00007", new Exception("Diarie/aktbeteckning (" + lageskontroll.Diarie.ToString() + ") är i fel format. Ska bestå av ett årtal mellan 1900-2099, punkt och sedan löpnummer, tillåtet 1 - 4 tal (ÅÅÅÅ.####)."));
    internal class DiarieFormatException : Exception
    {
        public DiarieFormatException()
            : base("LKR-00007",
                new Exception("Diarie/aktbeteckning är i fel format. Ska bestå av ett årtal mellan 1900-2099, punkt och sedan löpnummer, tillåtet 1 - 4 tal (ÅÅÅÅ.####).")) { }
        public DiarieFormatException(string diarie)
            : base("LKR-00007",
                new Exception(String.Format("Diarie/aktbeteckning ({0}) är i fel format. Ska bestå av ett årtal mellan 1900-2099, punkt och sedan löpnummer, tillåtet 1 - 4 tal (ÅÅÅÅ.####).", diarie))) { }
    }

}