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
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;

namespace Presidencia.Cat_Parametros_Nomina.Datos
{
    public class Cls_Cat_Nom_Parametros_Datos
    {
        #region (Metodos)

        #region (Metodos Consulta)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Parametros_Nomina
        /// DESCRIPCION : Consulta y se trae todos los registros de la tabla de Parámetros de Nomina
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 6/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Parametros_Nomina(Cls_Cat_Nom_Parametros_Negocio Datos)
        {
            DataTable Dt_Parametros_Nomina = null;
            String Mi_Oracle = "";
            try
            {
                Mi_Oracle = "SELECT " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + ".* FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;

                if (!string.IsNullOrEmpty(Datos.P_Parametro_ID))
                {
                    Mi_Oracle += " WHERE " + Cat_Nom_Parametros.Campo_Parametro_ID + "='" + Datos.P_Parametro_ID + "'";
                }

                Dt_Parametros_Nomina = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar la tabla de parametros. Error:[" + Ex.Message + "]");
            }
            return Dt_Parametros_Nomina;
        }
        #endregion

        #region (Metodos Operacion [Alta - Modificar - Eliminar])
        public static Boolean Alta_Parametro_Nomina(Cls_Cat_Nom_Parametros_Negocio Datos)
        {
            Boolean Operacion_Completa;
            String Mi_Oracle = "";
            object Parametro_Nomina_ID = null;

            try
            {
                Mi_Oracle = "SELECT NVL(MAX(" + Cat_Nom_Parametros.Campo_Parametro_ID + "),'00000') ";
                Mi_Oracle = Mi_Oracle + "FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros;
                Parametro_Nomina_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);

                if (Convert.IsDBNull(Parametro_Nomina_ID))
                {
                    Datos.P_Parametro_ID = "00001";
                }
                else
                {
                    Datos.P_Parametro_ID = String.Format("{0:00000}", Convert.ToInt32(Parametro_Nomina_ID) + 1);
                }

                Mi_Oracle = "INSERT INTO " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + " (" +
                    Cat_Nom_Parametros.Campo_Parametro_ID + ", " +
                    Cat_Nom_Parametros.Campo_Zona_ID + ", " +
                    Cat_Nom_Parametros.Campo_Porcentaje_Prima_Vacacional + ", " +
                    Cat_Nom_Parametros.Campo_Porcentaje_Fondo_Retiro + ", " +
                    Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical + ", " +
                    Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1 + ", " +
                    Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2 + ", " +
                    Cat_Nom_Parametros.Campo_Usuario_Creo + ", " +
                    Cat_Nom_Parametros.Campo_Fecha_Creo + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Quinquenio + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Faltas + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Retardos + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Incapacidades + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Subsidio + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_ISR + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Despensa + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial + ", " +
                    Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo + ", " +
                    Cat_Nom_Parametros.Campo_Salario_Mensual_Maximo + ", " +
                    Cat_Nom_Parametros.Campo_Salario_Diario_Int_Topado + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_IMSS + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas + ", " +
                    Cat_Nom_Parametros.Campo_IP_Servidor + ", " +
                    Cat_Nom_Parametros.Campo_Nombre_Base_Datos + ", " +
                    Cat_Nom_Parametros.Campo_Usuario_SQL + ", " +
                    Cat_Nom_Parametros.Campo_Password_Base_Datos + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Vacaciones + ", " +
                    Cat_Nom_Parametros.Campo_Tipo_IMSS + ", " +
                    Cat_Nom_Parametros.Campo_Minutos_Dia + ", " +
                    Cat_Nom_Parametros.Campo_Minutos_Retardo + ", " +
                    Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + ", " +
                    Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Aplicar_Empleado + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro + ", " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple + ", " +
                    Cat_Nom_Parametros.Campo_Deduccion_ISSEG + ", " +
                    Cat_Nom_Parametros.Campo_Dias_IMSS + ", " +
                    Cat_Nom_Parametros.Campo_Tope_ISSEG + ", " +
                    Cat_Nom_Parametros.Campo_Proveedor_Fonacot +
                    ") VALUES(" +
                    "'" + Datos.P_Parametro_ID + "', " +
                    "'" + Datos.P_Zona_ID + "', " +
                    Datos.P_Porcentaje_Prima_Vacacional + ", " +
                    Datos.P_Porcentaje_Fondo_Retiro + ", " +
                    Datos.P_Porcentaje_Prima_Dominical + ", " +
                    "'" + Datos.P_Fecha_Prima_Vacacional_1 + "', " +
                    "'" + Datos.P_Fecha_Prima_Vacacional_2 + "', " +
                    "'" + Datos.P_Usuario_Creo + "', SYSDATE, " +
                    "'" + Datos.P_Percepcion_Quinquenio + "', " +
                    "'" + Datos.P_Percepcion_Prima_Vacacional + "', " +
                    "'" + Datos.P_Percepcion_Prima_Dominical + "', " +
                    "'" + Datos.P_Percepcion_Aguinaldo + "', " +
                    "'" + Datos.P_Percepcion_Dias_Festivos + "', " +
                    "'" + Datos.P_Percepcion_Horas_Extra + "', " +
                    "'" + Datos.P_Percepcion_Dia_Doble + "', " +
                    "'" + Datos.P_Percepcion_Dia_Domingo + "', " +
                    "'" + Datos.P_Percepcion_Ajuste_ISR + "', " +
                    "'" + Datos.P_Deduccion_Faltas + "', " +
                    "'" + Datos.P_Deduccion_Retardos + "', " +
                    "'" + Datos.P_Percepcion_Incapacidades + "', " +
                    "'" + Datos.P_Deduccion_Fondo_Retiro + "', " +
                    "'" + Datos.P_Percepcion_Subsidio + "', " +
                    "'" + Datos.P_Deduccion_ISR + "', " +
                    "'" + Datos.P_Percepcion_Despensa + "', " +
                    "'" + Datos.P_Percepcion_Sueldo_Normal + "', " +
                    "'" + Datos.P_Deduccion_Tipo_Desc_Orden_Judicial + "', " +
                    "'" + Datos.P_Salario_Limite_Prestamo + "', " +
                    "'" + Datos.P_Salario_Mensual_Maximo + "', " +
                    "'" + Datos.P_Salario_Diario_Integrado_Topado + "', " +
                    "'" + Datos.P_Deduccion_IMSS + "', " +
                    "'" + Datos.P_Percepcion_Prima_Antiguedad + "', " +
                    "'" + Datos.P_Percepcion_Indemnizacion + "', " +
                    "'" + Datos.P_Deduccion_Vacaciones_Tomadas_Mas + "', " +
                    "'" + Datos.P_Percepcion_Vacaciones_Pendientes_Pagar + "', " +
                    "'" + Datos.P_Deduccion_Aguinaldo_Pagado_Mas + "', " +
                    "'" + Datos.P_Deduccion_Prima_Vac_Pagada_Mas + "', " +
                    "'" + Datos.P_Deduccion_Sueldo_Pagado_Mas + "', " +
                    "'" + Datos.P_IP_Servidor + "', " +
                    "'" + Datos.P_Nombre_Base_Datos + "', " +
                    "'" + Datos.P_Usuario_SQL + "', " +
                    "'" + Datos.P_Password_Base_Datos + "', " +
                    "'" + Datos.P_Deduccion_Orden_Judicial_Aguinaldo + "', " +
                    "'" + Datos.P_Deduccion_Orden_Judicial_Prima_Vacacional + "', " +
                    "'" + Datos.P_Deduccion_Orden_Judicial_Indemnizacion + "', " +
                    "'" + Datos.P_Percepcion_Vacaciones + "', " +
                    "'" + Datos.P_Tipo_IMSS + "', " +
                    Datos.P_Minutos_Dia + ", " +
                    Datos.P_Minutos_Retardo + ", " +
                    Datos.P_ISSEG_Porcentaje_Prevision_Social_Multiple + ", " +
                    Datos.P_ISSEG_Porcentaje_Aplicar_Empleado + ", " +
                    "'" + Datos.P_Percepcion_Fondo_Retiro + "', " +
                    "'" + Datos.P_Percepcion_Prevision_Social_Multiple + "', " +
                    "'" + Datos.P_Deduccion_ISSEG + "', " +
                    "'" + Datos.P_Dias_IMSS + "', " +
                    Datos.P_Tope_ISSEG + ", " +
                    "'" + Datos.P_Proveedor_Fonacot + "'" +
                    ")";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Parametro_Nomina
        /// DESCRIPCION : Modifica el registro seleccionado.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 6/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Modificar_Parametro_Nomina(Cls_Cat_Nom_Parametros_Negocio Datos)
        {
            String Mi_Oracle = "";
            Boolean Operacion_Completa = false;
            try
            {
                Mi_Oracle = "UPDATE " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + " SET " +
                    Cat_Nom_Parametros.Campo_Dias_IMSS + "='" + Datos.P_Dias_IMSS + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Quinquenio + "='" + Datos.P_Percepcion_Quinquenio + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prima_Vacacional + "='" + Datos.P_Percepcion_Prima_Vacacional + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prima_Dominical + "='" + Datos.P_Percepcion_Prima_Dominical + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Aguinaldo + "='" + Datos.P_Percepcion_Aguinaldo + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Dias_Festivos + "='" + Datos.P_Percepcion_Dias_Festivos + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Horas_Extra + "='" + Datos.P_Percepcion_Horas_Extra + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Dia_Doble + "='" + Datos.P_Percepcion_Dia_Doble + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Dia_Domingo + "='" + Datos.P_Percepcion_Dia_Domingo + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Ajuste_ISR + "='" + Datos.P_Percepcion_Ajuste_ISR + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Incapacidades + "='" + Datos.P_Percepcion_Incapacidades + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prima_Antiguedad + "='" + Datos.P_Percepcion_Prima_Antiguedad + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Indemnizacion + "='" + Datos.P_Percepcion_Indemnizacion + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Subsidio + "='" + Datos.P_Percepcion_Subsidio + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Despensa + "='" + Datos.P_Percepcion_Despensa + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Sueldo_Normal + "='" + Datos.P_Percepcion_Sueldo_Normal + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Vacaciones_Pendientes_Pagar + "='" + Datos.P_Percepcion_Vacaciones_Pendientes_Pagar + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Vacaciones + "='" + Datos.P_Percepcion_Vacaciones + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Faltas + "='" + Datos.P_Deduccion_Faltas + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Retardos + "='" + Datos.P_Deduccion_Retardos + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Fondo_Retiro + "='" + Datos.P_Deduccion_Fondo_Retiro + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_ISR + "='" + Datos.P_Deduccion_ISR + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Tipo_Desc_Orden_Judicial + "='" + Datos.P_Deduccion_Tipo_Desc_Orden_Judicial + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_IMSS + "='" + Datos.P_Deduccion_IMSS + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Vacaciones_Tomadas_Mas + "='" + Datos.P_Deduccion_Vacaciones_Tomadas_Mas + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Aguinaldo_Pagado_Mas + "='" + Datos.P_Deduccion_Aguinaldo_Pagado_Mas + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Prima_Vacacional_Pagada_Mas + "='" + Datos.P_Deduccion_Prima_Vac_Pagada_Mas + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Sueldo_Pagado_Mas + "='" + Datos.P_Deduccion_Sueldo_Pagado_Mas + "', " +
                    Cat_Nom_Parametros.Campo_Zona_ID + "='" + Datos.P_Zona_ID + "', " +
                    Cat_Nom_Parametros.Campo_Porcentaje_Prima_Vacacional + "=" + Datos.P_Porcentaje_Prima_Vacacional + ", " +
                    Cat_Nom_Parametros.Campo_Porcentaje_Fondo_Retiro + "=" + Datos.P_Porcentaje_Fondo_Retiro + ", " +
                    Cat_Nom_Parametros.Campo_Porcentaje_Prima_Dominical + "=" + Datos.P_Porcentaje_Prima_Dominical + ", " +
                    Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1 + "='" + Datos.P_Fecha_Prima_Vacacional_1 + "', " +
                    Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2 + "='" + Datos.P_Fecha_Prima_Vacacional_2 + "', " +
                    Cat_Nom_Parametros.Campo_Salario_Limite_Prestamo + "=" + Datos.P_Salario_Limite_Prestamo + ", " +
                    Cat_Nom_Parametros.Campo_Salario_Mensual_Maximo + "='" + Datos.P_Salario_Mensual_Maximo + "', " +
                    Cat_Nom_Parametros.Campo_Salario_Diario_Int_Topado + "='" + Datos.P_Salario_Diario_Integrado_Topado + "', " +
                    Cat_Nom_Parametros.Campo_IP_Servidor + " ='" + Datos.P_IP_Servidor + "', " +
                    Cat_Nom_Parametros.Campo_Nombre_Base_Datos + " ='" + Datos.P_Nombre_Base_Datos + "', " +
                    Cat_Nom_Parametros.Campo_Usuario_SQL + " ='" + Datos.P_Usuario_SQL + "', " +
                    Cat_Nom_Parametros.Campo_Password_Base_Datos + " ='" + Datos.P_Password_Base_Datos + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Aguinaldo + " ='" + Datos.P_Deduccion_Orden_Judicial_Aguinaldo + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Prima_Vacacional + " ='" + Datos.P_Deduccion_Orden_Judicial_Prima_Vacacional + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_Orden_Judicial_Indemnizacion + " ='" + Datos.P_Deduccion_Orden_Judicial_Indemnizacion + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Fondo_Retiro + "='" + Datos.P_Percepcion_Fondo_Retiro + "', " +
                    Cat_Nom_Parametros.Campo_Percepcion_Prevision_Social_Multiple + "='" + Datos.P_Percepcion_Prevision_Social_Multiple + "', " +
                    Cat_Nom_Parametros.Campo_Deduccion_ISSEG + "='" + Datos.P_Deduccion_ISSEG + "', " +
                    Cat_Nom_Parametros.Campo_Tipo_IMSS + "='" + Datos.P_Tipo_IMSS + "', " +
                    Cat_Nom_Parametros.Campo_Minutos_Dia + "=" + Datos.P_Minutos_Dia + ", " +
                    Cat_Nom_Parametros.Campo_Minutos_Retardo + "=" + Datos.P_Minutos_Retardo + ", " +
                    Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Prevision_Social_Multiple + "=" + Datos.P_ISSEG_Porcentaje_Prevision_Social_Multiple + ", " +
                    Cat_Nom_Parametros.Campo_ISSEG_Porcentaje_Aplicar_Empleado + "=" + Datos.P_ISSEG_Porcentaje_Aplicar_Empleado + ", " +
                    Cat_Nom_Parametros.Campo_Tope_ISSEG + "=" + Datos.P_Tope_ISSEG + ", " +
                    Cat_Nom_Parametros.Campo_Proveedor_Fonacot + "='" + Datos.P_Proveedor_Fonacot + "', " +
                    Cat_Nom_Parametros.Campo_Usuario_Modifico + "='" + Datos.P_Usuario_Modifico + "', " +
                    Cat_Nom_Parametros.Campo_Fecha_Modifico + "= SYSDATE " +
                    " WHERE " +
                    Cat_Nom_Parametros.Campo_Parametro_ID + " ='" + Datos.P_Parametro_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
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
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Elimnar_Parametro_Nomina
        /// DESCRIPCION : Eliminar  el registro seleccionado.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 6/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static Boolean Elimnar_Parametro_Nomina(Cls_Cat_Nom_Parametros_Negocio Datos) {
            String Mi_Oracle="";
            Boolean Operacion_Completa=false;
            try
            {
                Mi_Oracle = "DELETE FROM " + Cat_Nom_Parametros.Tabla_Cat_Nom_Parametros + " WHERE " +
                    Cat_Nom_Parametros.Campo_Parametro_ID + "='" + Datos.P_Parametro_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Oracle);
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al eliminar el registro seleccionado. Error ["+ Ex.Message +"]");
            }
            return Operacion_Completa;
        }
        #endregion

        #endregion
    }
}