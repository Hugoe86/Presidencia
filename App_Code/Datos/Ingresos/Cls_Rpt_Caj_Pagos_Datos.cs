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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text;
using Presidencia.Reporte_Pago_Cajas_Diario.Negocio;

namespace Presidencia.Reporte_Pago_Cajas_Diario.Datos
{
    public class Cls_Rpt_Caj_Pagos_Datos
    {
        #region (Métodos)
        internal static System.Data.DataTable Consultar_Ingresos_Diarios(Cls_Rpt_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Ingresos_Diarios = null;

            try
            {
                Query.Append("SELECT ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Clave + " AS RAMA, ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Clave + " AS GRUPO, ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + " AS INGRESO, ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID + " AS PASIVO, ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + ", ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha);

                Query.Append(" FROM ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " ON ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Rama_ID + "=");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Rama_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " ON ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Grupo_ID + "=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID + "=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" WHERE ");

                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha);
                Query.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Query.Append(" AND ");
                Query.Append("TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");

                Dt_Ingresos_Diarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos diarios de presidencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Ingresos_Diarios;
        }

        internal static System.Data.DataTable Consultar_Rama(Cls_Rpt_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Ingresos_Diarios = null;

            try
            {
                Query.Append("SELECT ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Clave + " AS RAMA, ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Nombre + ", ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS MONTO");

                Query.Append(" FROM ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " ON ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Rama_ID + "=");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Rama_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " ON ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Grupo_ID + "=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID + "=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + " ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + "=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" WHERE ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha);
                Query.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Query.Append(" AND ");
                Query.Append(" TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");

                Query.Append(" GROUP BY ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Clave + ", ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Nombre);

                Dt_Ingresos_Diarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos diarios de presidencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Ingresos_Diarios;
        }

        internal static System.Data.DataTable Consultar_Grupos(Cls_Rpt_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Ingresos_Diarios = null;

            try
            {
                Query.Append("SELECT ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Clave + " AS GRUPO, ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Nombre + ", ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS MONTO");

                Query.Append(" FROM ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " ON ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Rama_ID + "=");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Rama_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " ON ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Grupo_ID + "=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID + "=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + " ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + "=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" WHERE ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Clave + "='" + Datos.P_Clave_Rama + "'");
                Query.Append(" AND ");

                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha);
                Query.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Query.Append(" AND ");
                Query.Append(" TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");

                Query.Append(" GROUP BY ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Clave + ", ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Nombre);

                Dt_Ingresos_Diarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos diarios de presidencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Ingresos_Diarios;
        }

        internal static System.Data.DataTable Consultar_Ingresos(Cls_Rpt_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Query = new StringBuilder();
            DataTable Dt_Ingresos_Diarios = null;

            try
            {
                Query.Append("SELECT ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + " AS INGRESO, ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + " AS NOMBRE, ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS MONTO");

                Query.Append(" FROM ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + " ON ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Rama_ID + "=");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Rama_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + " ON ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Grupo_ID + "=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Grupo_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID + "=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + " ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID + "=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" RIGHT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + " ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + "=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" WHERE ");
                Query.Append(Cat_Pre_Ramas.Tabla_Cat_Pre_Ramas + "." + Cat_Pre_Ramas.Campo_Clave + "='" + Datos.P_Clave_Rama + "'");
                Query.Append(" AND ");
                Query.Append(Cat_Pre_Grupos.Tabla_Cat_Pre_Grupos + "." + Cat_Pre_Grupos.Campo_Clave + "='" + Datos.P_Clave_Grupo + "'");
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha);
                Query.Append(" BETWEEN TO_DATE ('" + Datos.P_Fecha_Inicio_Busqueda + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Query.Append(" AND ");
                Query.Append(" TO_DATE ('" + Datos.P_Fecha_Fin_Busqueda + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')");

                Query.Append(" GROUP BY ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + ", ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + ", ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion);

                Dt_Ingresos_Diarios = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos diarios de presidencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Ingresos_Diarios;
        }
        #endregion

        #region (Reporte Cierre de Caja)
        /// *********************************************************************************************************************
        /// Nombre: Rpt_Caj_Ingresos
        /// 
        /// Descripción: Método que obtiene las tablas que se usaran para realizar la consulta.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        internal static DataSet Rpt_Caj_Ingresos(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacena la consulta.
            DataSet Ds_Ingresos_Dependencia = null;//Variable que almacena las tablas de ingresos.
            DataTable Dt_Listado_Ingresos = null;//Variable que almacenara un listado de los ingresos al cierre de caja.
            DataTable Dt_Listado_Agrupado_Tipo_Pago = null;//Variable que almacenara un ana lista de los ingresos agrupados por forma de pago.
            DataTable Dt_Monto_Total = null;//Variable que almacenara el total de ingresos al cierre de caja.

            try
            {
                Ds_Ingresos_Dependencia = new DataSet();//Creamos el objeto que almacenara las tablas de ingresos al cierre de cajas.

                //Consultamos las tablas de ingresos al cierre de caja.
                Dt_Listado_Ingresos = Consultar_Listado_Ingresos(Dependencia_ID, Turno_ID, Caja_ID);
                Dt_Listado_Agrupado_Tipo_Pago = Consultar_Listado_Agrupado_Tipo_Pago(Dependencia_ID, Turno_ID, Caja_ID);
                Dt_Monto_Total = Consultar_Monto_Total(Dependencia_ID, Turno_ID, Caja_ID);
                //Nombramos las qeu alimentaran el reporte.
                Dt_Listado_Ingresos.TableName = "INGRESO";
                Dt_Listado_Agrupado_Tipo_Pago.TableName = "INGRESOS_AGRUPADOS_TIPO_PAGO";
                Dt_Monto_Total.TableName = "TOTAL";
                //Agregamos las tablas de ingresos a la estructura que las almacenara.
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Listado_Ingresos.Copy());
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Listado_Agrupado_Tipo_Pago.Copy());
                Ds_Ingresos_Dependencia.Tables.Add(Dt_Monto_Total.Copy());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia al realizar el cierre de caja. Error: [" + Ex.Message + "]");
            }
            return Ds_Ingresos_Dependencia;
        }
        /// *********************************************************************************************************************
        /// Nombre: Consultar_Listado_Ingresos
        /// 
        /// Descripción: Metodo que obtiene un listado de los ingresos que se tuviron al realizar el cierre de caja.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        private static DataTable Consultar_Listado_Ingresos(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Listado_Ingresos = null;//Variable que almacenara el listado de ingresos al realziar el cierre de caja.

            try
            {
                Query.Append("SELECT ");
                Query.Append("(");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave);
                Query.Append("|| ' -- ' ||");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre);
                Query.Append(") AS DEPENDENCIA, ");
                Query.Append("(");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave);
                Query.Append("|| ' -- ' ||");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion);
                Query.Append(") AS INGRESO, ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto);
                Query.Append(", ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);

                Query.Append(" FROM ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso);
                Query.Append(" ON ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Query.Append(" ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);
                Query.Append("=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Turno_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                }

                Dt_Listado_Ingresos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Listado_Ingresos;
        }
        /// *********************************************************************************************************************
        /// Nombre: Consultar_Listado_Agrupado_Tipo_Pago
        /// 
        /// Descripción: Método que consultara los ingresos agrupados por su forma de pago.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        private static DataTable Consultar_Listado_Agrupado_Tipo_Pago(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Listado_Agrupado_Tipo_Pago = null;//Variable que almacenara el listado de ingresos agrupados por 

            try
            {
                Query.Append("SELECT ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);
                Query.Append(", ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS MONTO"); 

                Query.Append(" FROM ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso);
                Query.Append(" ON ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Query.Append(" ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);
                Query.Append("=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Turno_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                }

                Query.Append(" GROUP BY ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago);

                Dt_Listado_Agrupado_Tipo_Pago = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Listado_Agrupado_Tipo_Pago;
        }
        /// *********************************************************************************************************************
        /// Nombre: Consultar_Monto_Total
        /// 
        /// Descripción: Metodo que consulta el monto total de ingresos al cierre de caja.
        /// 
        /// Parámetros: Datos.- Objeto que contiene los datos necesarios para realizar la consulta.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 25/Agosto/2011
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// *********************************************************************************************************************
        private static DataTable Consultar_Monto_Total(String Dependencia_ID, String Turno_ID, String Caja_ID)
        {
            StringBuilder Query = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Monto_Total = null;//Variable que almacenara el total de ingresos al realziar el cierre de caja.

            try
            {
                Query.Append("SELECT ");
                Query.Append("SUM(" + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_Monto + ") AS TOTAL");

                Query.Append(" FROM ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso);
                Query.Append(" ON ");
                Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Query.Append(" ON ");
                Query.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave_Ingreso_ID);
                Query.Append("=");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Query.Append(" ON ");
                Query.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Pasivo_ID);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pasivo);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);
                Query.Append("=");
                Query.Append(Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Query.Append(" ON ");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Turno);
                Query.Append("=");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);

                Query.Append(" LEFT OUTER JOIN ");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas);
                Query.Append(" ON ");
                Query.Append("(");
                Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Query.Append(" AND ");
                Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_ID);
                Query.Append("=");
                Query.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Cajas + "." + Cat_Pre_Cajas.Campo_Caja_ID);
                Query.Append(")");

                if (!String.IsNullOrEmpty(Dependencia_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Turno_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno + "='" + Turno_ID + "'");
                    }
                }

                if (!String.IsNullOrEmpty(Caja_ID))
                {
                    if (Query.ToString().Contains("WHERE"))
                    {
                        Query.Append(" AND ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                    else
                    {
                        Query.Append(" WHERE ");
                        Query.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Caja_ID + "='" + Caja_ID + "'");
                    }
                }

                Dt_Monto_Total = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Query.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los ingresos de presidencia de alguna dependencia. Error: [" + Ex.Message + "]");
            }
            return Dt_Monto_Total;
        }
        #endregion
    }
}
