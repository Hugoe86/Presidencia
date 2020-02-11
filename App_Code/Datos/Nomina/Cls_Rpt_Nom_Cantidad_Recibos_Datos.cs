using System;
using System.IO;
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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Nomina_Reporte_Cantidad_Recibos.Negocio;

namespace Presidencia.Nomina_Reporte_Cantidad_Recibos.Datos
{
    public class Cls_Rpt_Nom_Cantidad_Recibos_Datos
    {
        #region Consulta
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Cantidad_Recibos_Impresos
        /// DESCRIPCION : Consulta la cantidad de recibos que se generaron
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 10/Abril/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Cantidad_Recibos_Impresos(Cls_Rpt_Nom_Cantidad_Recibos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("count( " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Recibo + ") as Cantidad_Recibos,");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina +",");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre+ "");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados+ "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" join " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID);
                Mi_SQL.Append("=" + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" WHERE ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" +Datos.P_Nomina_id + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina + "");

                //  filtro por tipo de nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID+ "='" + Datos.P_Tipo_Nomina_ID+ "'");
                }

                //  filtro por banco id
                if (!String.IsNullOrEmpty(Datos.P_Banco_ID))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID + "='" + Datos.P_Banco_ID + "'");
                }

                Mi_SQL.Append(" group by ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ",");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre );
                              
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        #endregion
    }
}