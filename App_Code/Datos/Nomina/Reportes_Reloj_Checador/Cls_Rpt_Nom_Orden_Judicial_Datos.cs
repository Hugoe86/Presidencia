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
using Presidencia.Nomina_Reportes_Orden_Judicial.Negocio;

namespace Presidencia.Nomina_Reportes_Orden_Judicial.Datos
{
    public class Cls_Rpt_Nom_Orden_Judicial_Datos
    {
        #region Consultas

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Orden_Judicial
        /// DESCRIPCION : Consulta todos los datos del Empleado que tenga orden juridica
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 22/Marzo/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Orden_Judicial(Cls_Rpt_Nom_Orden_Judicial_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta 
            String Mi_SQL_Comparacion = String.Empty;
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select ");
                //  para los datos de la orden judicial
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Orden_Judicial_ID + ",");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Tipo_Descuento_Orden_Judicial_Sueldo + " as Tipo_Descuento ,");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo + " as Porcentaje ,");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Bruto_Neto_Orden_Judicial_Sueldo + " as Tipo_Sueldo ,");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Beneficiario + ",");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ", ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", ");
                // para los datos del empleado
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ", ");
                //  para el nombre del empleado
                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");
                //  para el nombre de la dependencia
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " as Nombre_Dependencia, ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " as Clave_Dependencia, ");
                //  para los datos de la nomina
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + ",");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina + ",");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio + ",");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ",");
                //  para el totales del empleado
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ",");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Deducciones + ",");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ",");
                //  para los calculos del descuento
                Mi_SQL.Append("((" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ") * (" +
                   Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo +
                   ")/100) as Descuento_Bruto,");

                Mi_SQL.Append("((" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ") * (" +
                  Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo +
                  ")/100) as Descuento_Neto,");

                Mi_SQL.Append("((" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Percepciones + ") - (" +
                  Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo +
                  ")) as Descuento_Cantidad_Bruto,");

                Mi_SQL.Append("((" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ") - (" +
                 Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Cantidad_Porcentaje_Orden_Judicial_Sueldo +
                 ")) as Descuento_Cantidad_Neto ");

                Mi_SQL.Append(" FROM " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial);

                Mi_SQL.Append(" left outer join " + Cat_Empleados.Tabla_Cat_Empleados + " on ");
                Mi_SQL.Append(Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados+ " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID);

                Mi_SQL.Append(" left outer join " + Cat_Dependencias.Tabla_Cat_Dependencias + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append("=" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles+ " on ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" AND ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append(" left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " on ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                if(!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append(" WHERE " + Cat_Nom_Tab_Orden_Judicial.Tabla_Cat_Nom_Tab_Orden_Judicial + "." + Cat_Nom_Tab_Orden_Judicial.Campo_Empleado_ID +
                        " in ( SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " +
                        Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "') ");
                }

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL_Comparacion = Mi_SQL.ToString();
                    if (Mi_SQL_Comparacion.Contains("WHERE"))
                    {
                        Mi_SQL.Append(" AND " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                                Cat_Empleados.Campo_Tipo_Nomina_ID + " in ( SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                                " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                                "='" + Datos.P_Tipo_Nomina_ID + "') ");
                    }
                    else
                    {
                        Mi_SQL.Append(" WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." +
                              Cat_Empleados.Campo_Tipo_Nomina_ID + " in ( SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                              " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID +
                              "='" + Datos.P_Tipo_Nomina_ID + "') ");
                    }
                }

                Mi_SQL.Append(" Order by Nombre_Empleado");
                
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