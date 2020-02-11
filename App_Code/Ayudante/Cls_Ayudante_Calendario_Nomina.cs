using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;

namespace Presidencia.Ayudante_Calendario_Nomina
{
    public class Cls_Ayudante_Calendario_Nomina
    {
        private String Anyo;
        private String Periodo;
        private String Nomina_ID;

        public String P_Anyo {
            get { return Consultar_Anio_Nomina_Actual(); }
            set { Anyo = value; }
        }

        public String P_Periodo {
            get { return Consulta_Detalle_Periodo_Actual(); }
            set { Periodo = value; }
        }

        public String P_Nomina_ID {
            get { return Consultar_Nomina_Actual(); }
            set { Nomina_ID = value; }
        }

        private String Consultar_Anio_Nomina_Actual() {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Aux = null;
            String Anio = String.Empty;

            try
            {
                Mi_SQL.Append("SELECT ANIO FROM CAT_NOM_CALENDARIO_NOMINAS WHERE ANIO=" + DateTime.Today.Year);

                Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Aux is DataTable) {
                    if (Dt_Aux.Rows.Count > 0) {
                        foreach (DataRow Fila in Dt_Aux.Rows) {
                            if (Fila is DataRow) {
                                if (!String.IsNullOrEmpty(Fila[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString())) {
                                    Anio = Fila[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el año actual de la nómina. Error: [" + Ex.Message + "]");
            }
            return Anio;
        }

        private  String Consulta_Detalle_Periodo_Actual()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Aux = null;
            String No_Nomina = String.Empty;

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(" TO_DATE (CONCAT(SYSDATE, ' 00:00:00') , 'DD-MM-YYYY HH24:MI:SS')  >= TO_DATE (CONCAT(" + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ", ' 00:00:00') , 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(" TO_DATE (CONCAT(SYSDATE, ' 23:59:00') , 'DD-MM-YYYY HH24:MI:SS') <= TO_DATE (CONCAT(" + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ", ' 23:59:00') , 'DD-MM-YYYY HH24:MI:SS')");

                Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Aux is DataTable)
                {
                    if (Dt_Aux.Rows.Count > 0)
                    {
                        foreach (DataRow Fila in Dt_Aux.Rows)
                        {
                            if (Fila is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Fila[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString()))
                                {
                                    No_Nomina = Fila[Cat_Nom_Nominas_Detalles.Campo_No_Nomina].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los detalles del periodo actual. Error: [" + Ex.Message + "]");
            }
            return No_Nomina;
        }

        private String Consultar_Nomina_Actual()
        {
            StringBuilder Mi_SQL = new StringBuilder();
            DataTable Dt_Aux = null;
            String Nomina_ID = String.Empty;

            try
            {
                Mi_SQL.Append("SELECT NOMINA_ID FROM CAT_NOM_CALENDARIO_NOMINAS WHERE ANIO=" + DateTime.Today.Year);

                Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Aux is DataTable)
                {
                    if (Dt_Aux.Rows.Count > 0)
                    {
                        foreach (DataRow Fila in Dt_Aux.Rows)
                        {
                            if (Fila is DataRow)
                            {
                                if (!String.IsNullOrEmpty(Fila[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID].ToString()))
                                {
                                    Nomina_ID = Fila[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID].ToString();
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar el año actual de la nómina. Error: [" + Ex.Message + "]");
            }
            return Nomina_ID;
        }
    }
}