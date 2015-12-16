using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Kartforandring.Controllers.Brister
{
    public class BristerController : ApiController
    {
        [HttpGet]
        public IEnumerable<BristStat> GetAllBristGeometryIsNull()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristGeometryIsNull();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristUtforareIsNullWhenStarted()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristUtforareIsNullWhenStarted();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristDiarieIsNull()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristDiarieIsNull();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristDiarieWrongFormat()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristDiarieWrongFormat();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristObjTypNotInherited()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristObjTypNotInherited();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristObjIsNull()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristObjIsNull();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristTypIsNull()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristTypIsNull();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristBygglovMissingReason()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristBygglovMissingReason();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristBygglovsbevakningarFinishNotDocumentedExecuted()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristBygglovsbevakningarFinishNotDocumentedExecuted();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristBygglovRedundant()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristBygglovRedundant();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristBestalldUtsattningNoBevakning()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristBestalldUtsattningNoBevakning();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllBristBestalldLageskontrollNoBevakning()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllBristBestalldLageskontrollNoBevakning();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }



        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningExternBestalldLageskontrollInTime()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningExternBestalldLageskontrollInTime();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningExternBestalldLageskontrollInTime(int antalDagar)
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningExternBestalldLageskontrollInTime(antalDagar);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningInternBestalldLageskontrollInTime()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningInternBestalldLageskontrollInTime();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningInternBestalldLageskontrollInTime(int antalDagar)
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningInternBestalldLageskontrollInTime(antalDagar);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningUtsattningWithLageskontrollUtfard()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningUtsattningWithLageskontrollUtfard();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningLageskontrollUtfardAndamalNotUtfard()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningLageskontrollUtfardAndamalNotUtfard();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllVarningLogisktRaderade()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllVarningLogisktRaderade();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }



        [HttpGet]
        public IEnumerable<BristStat> GetAllStatusChangedSince()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllStatusChangedSince();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllStatusChangedSince(string dateFilter)
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllStatusChangedSince(dateFilter);
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllStatusNbrOfChangesObjkod()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllStatusNbrOfChangesObjkod();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllStatusNbrOfChangesTypkod()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllStatusNbrOfChangesTypkod();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllStatusNbrTotal()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllStatusNbrTotal();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllStatusNbrAktiva()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllStatusNbrAktiva();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }



        [HttpGet]
        public IEnumerable<BristStat> GetAllFelBygglovNoBygglovsbevakning()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllFelBygglovNoBygglovsbevakning();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllFelBygglovsbevakningNoBygglov()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllFelBygglovsbevakningNoBygglov();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllFelBestalldUtsattningNoBevakning()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllFelBestalldUtsattningNoBevakning();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        public IEnumerable<BristStat> GetAllFelBestalldLageskontrollNoBevakning()
        {
            try
            {
                BristStat bristStats = new BristStat();
                return bristStats.GetAllFelBestalldLageskontrollNoBevakning();
            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}
