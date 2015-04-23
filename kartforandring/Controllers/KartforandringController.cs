using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace kartforandring.Controllers
{
    public class KartforandringController : ApiController
    {
        [HttpGet]
        public IEnumerable<Forandringar> Kartforandringar()
        {
            Forandringar AllForandings = new Forandringar();
            //IList<Forandringar> forandrings = new List<Forandringar>();
            return AllForandings.GetAllaForandrings();
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);

        [HttpGet]
        public IEnumerable<Bygglovsbeslut> Bygglovsbeslut()
        {
            Bygglovsbeslut Bygglovs = new Bygglovsbeslut();
            //IList<Bygglovsbeslut> forandrings = new List<Bygglovsbeslut>();
            return Bygglovs.GetAllaBygglovsbeslut();
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);

        [HttpGet]
        public jTableRecordBygglovsbeslut Lageskontroller(string jtSorting)
        {
            jTableRecordBygglovsbeslut jt = new jTableRecordBygglovsbeslut();
            try
            {
                Bygglovsbeslut Bygglov = new Bygglovsbeslut();
                IList<Bygglovsbeslut> Bygglovs = new List<Bygglovsbeslut>();
                Bygglovs = Bygglov.GetBygglovsbeslutMedLageskontroll();
                jt.Result = "OK";
                jt.Records = Bygglovs;
                jt.sortBygglovsbeslut(jtSorting);
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";
                jt.Message = ex.Message;
                return jt;
            }
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);

        [HttpGet]
        public jTableOptionLageskontrollOrdering DomainLageskontrollOrdering()
        {
            jTableOptionLageskontrollOrdering jt = new jTableOptionLageskontrollOrdering();
            try
            {
                DomainLageskontrollOrdering DomainLageskontrollOrdering = new DomainLageskontrollOrdering();
                IList<DomainLageskontrollOrdering> DomainLageskontrollOrderings = new List<DomainLageskontrollOrdering>();
                DomainLageskontrollOrderings = DomainLageskontrollOrdering.GetDomains();
                jt.Result = "OK";
                jt.Options = DomainLageskontrollOrderings;
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";
                jt.Message = ex.Message;
                return jt;
            }
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);

        [HttpGet]
        public jTableOptionAdressOmrade DomainAdressOmrade()
        {
            jTableOptionAdressOmrade jt = new jTableOptionAdressOmrade();
            try
            {
                DomainAdressOmrade DomainAdressOmrade = new DomainAdressOmrade();
                IList<DomainAdressOmrade> DomainAdressOmrades = new List<DomainAdressOmrade>();
                DomainAdressOmrades = DomainAdressOmrade.GetDomains();
                jt.Result = "OK";
                jt.Options = DomainAdressOmrades;
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";
                jt.Message = ex.Message;
                return jt;
            }
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);

        [HttpGet]
        public jTableOptionBelagenhetsAdress DomainBelagenhetsAdress(string AdressOmr)
        {
            jTableOptionBelagenhetsAdress jt = new jTableOptionBelagenhetsAdress();
            try
            {
                DomainBelagenhetsAdress DomainBelagenhetsAdress = new DomainBelagenhetsAdress();
                IList<DomainBelagenhetsAdress> DomainBelagenhetsAdresses = new List<DomainBelagenhetsAdress>();
                DomainBelagenhetsAdresses = DomainBelagenhetsAdress.GetDomains();
                jt.Result = "OK";
                jt.Options = DomainBelagenhetsAdresses;
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";
                jt.Message = ex.Message;
                return jt;
            }
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);

        [HttpGet]
        public jTableOptionFastighet DomainFastighet()
        {
            jTableOptionFastighet jt = new jTableOptionFastighet();
            try
            {
                DomainFastighet DomainFastighet = new DomainFastighet();
                IList<DomainFastighet> DomainFastighets = new List<DomainFastighet>();
                DomainFastighets = DomainFastighet.GetDomains();
                jt.Result = "OK";
                jt.Options = DomainFastighets;
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";
                jt.Message = ex.Message;
                return jt;
            }
        }
        //throw new HttpResponseException(HttpStatusCode.NotFound);
        
        [HttpPut]
        public jTableRecordBygglovsbeslut SkapaLageskontroll(Bygglovsbeslut record)
        {
            jTableRecordBygglovsbeslut jt = new jTableRecordBygglovsbeslut();
            try
            {
                Bygglovsbeslut Bygglov = new Bygglovsbeslut();
                Bygglov = Bygglov.AddLageskontroll(record);
                jt.Result = "OK";
                jt.Record = Bygglov;
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";

                if (ex.Message.Substring(0, 4) == "LKR-")
                {
                    jt.Message = ex.InnerException.Message.ToString();
                }
                else
                {
                    jt.Message = ex.Message;
                }

                return jt;
            }
        }

        [HttpPost]
        public jTableRecordBygglovsbeslut UppdateraLageskontroll(Bygglovsbeslut record)
        {
            jTableRecordBygglovsbeslut jt = new jTableRecordBygglovsbeslut();
            try
            {
                Bygglovsbeslut Bygglov = new Bygglovsbeslut();
                Bygglov = Bygglov.UpdateLageskontroll(record);
                jt.Result = "OK";
                jt.Record = Bygglov;
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";

                if (ex.Message.Substring(0, 4) == "LKR-")
                {
                    jt.Message = ex.InnerException.Message.ToString();
                }
                else
                {
                    jt.Message = ex.Message;
                }

                return jt;
            }
        }

        [HttpGet]
        public jTableRecordBygglovsbeslut RaderaLageskontroll(int fid)
        {
            jTableRecordBygglovsbeslut jt = new jTableRecordBygglovsbeslut();
            try
            {
                Bygglovsbeslut Bygglov = new Bygglovsbeslut();
                Bygglov.DeleteLageskontroll(fid);
                jt.Result = "OK";
                return jt;
            }
            catch (Exception ex)
            {
                jt.Result = "ERROR";

                if (ex.Message.Substring(0, 4) == "LKR-")
                {
                    jt.Message = ex.InnerException.Message.ToString();
                }
                else
                {
                    jt.Message = ex.Message;
                }

                return jt;
            }
        }

    }
}
