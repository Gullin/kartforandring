using System.Data;
using System.Data.OleDb;
using Kartforandring;

namespace Kartforandring.Geometry
{
	public static class SdoGeometry
	{
        internal static string createSdoGeometryFromRelationalData(Bygglovsbeslut bygglovsbeslut)
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
                OleDbConnection con = DB.GetOleDbConncection();
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
                OleDbConnection con = DB.GetOleDbConncection();
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
                OleDbConnection con = DB.GetOleDbConncection();
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

        internal static void coord2SdoGeometryAsString(ref string geometry, string sdoPrefix, string sdoSuffix, ref string sdoGType, ref string sdoSRID, ref string sdoPoint, ref string sdoElemt, ref string sdoOrdinates, DataTable dt, bool isPolygon)
        {
            sdoSRID = "NULL";

            if (dt.Rows.Count == 1)
            {
                // Punkt, vanlig enkel
                sdoGType = SdoGType.Point2D;
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
                    sdoGType = SdoGType.Polygon2D;
                    sdoElemt = "SDO_ELEM_INFO_ARRAY(1,1003,1)";
                }
                else
                {
                    sdoGType = SdoGType.Line2D;
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

	}

    public static class SdoGType
    {
        public static string Point2D { get { return "2001"; } }
        public static string Line2D { get { return "2002"; } }
        public static string Polygon2D { get { return "2003"; } }
        public static string Point3D { get { return "3001"; } }
        public static string Line3D { get { return "3002"; } }
        public static string Polygon3D { get { return "3003"; } }
    }

}