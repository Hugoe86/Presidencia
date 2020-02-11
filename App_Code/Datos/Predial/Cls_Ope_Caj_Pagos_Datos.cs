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
using System.Collections.Generic;
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Caja_Pagos.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Sessiones;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Cls_Ope_Ing_Pasivos.Negocio;
using Presidencia.Cls_Ope_Ing_Ordenes_Pago.Negocio;
using Presidencia.Cls_Ope_Ing_Descuentos.Negocio;
using Presidencia.Polizas.Negocios;
using Presidencia.Ope_Con_Poliza_Ingresos.Datos;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using System.Net;
using System.IO;

namespace Presidencia.Caja_Pagos.Datos
{
    public class Cls_Ope_Caj_Pagos_Datos
    {
        public Cls_Ope_Caj_Pagos_Datos()
        {
        }

        #region Propieades
        private static DataTable Dt_Partidas_Poliza;

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Partidas_Poliza
        ///DESCRIPCIÓN          : Metodo que devuelve un DataTable con los campos indicados
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 15/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        private static DataTable Crear_Dt_Partidas_Poliza()
        {
            DataTable Dt_Partidas_Poliza = new DataTable();

            //Agrega los campos que va a contener el DataTable
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida, typeof(System.Int32));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Cat_Con_Cuentas_Contables.Campo_Cuenta, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Concepto, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Debe, typeof(System.Double));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Haber, typeof(System.Double));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Partida_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Dependencia_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID, typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add("MOMENTO_INICIAL", typeof(System.String));
            Dt_Partidas_Poliza.Columns.Add("MOMENTO_FINAL", typeof(System.String));

            return Dt_Partidas_Poliza;
        }

        private static DataTable P_Dt_Partidas_Poliza
        {
            get
            {
                return Dt_Partidas_Poliza;
            }
            set
            {
                Dt_Partidas_Poliza = value.Copy();
            }
        }
        #endregion

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Cuenta_Predial
        /// DESCRIPCION : Consulta los datos generales del banco que fue seleccionado por
        ///               el usuario
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Orden(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " ";
                Mi_SQL = Mi_SQL + " From " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL = Mi_SQL + " where ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "='";
                Mi_SQL = Mi_SQL + Datos.P_Orden_Variacion_ID + "' and ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                Mi_SQL = Mi_SQL + "='" + Datos.P_Anio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (dataset != null)
                {
                    Mi_SQL = " Update ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos;
                    Mi_SQL = Mi_SQL + " set " + Ope_Pre_Contrarecibos.Campo_Estatus + "='PAGADO'";
                    Mi_SQL = Mi_SQL + " where ";
                    Mi_SQL = Mi_SQL + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + "='" + dataset.Tables[0].Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo] + "'";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION    : Consultar_Orden_Variacion
        /// DESCRIPCION             : Consulta los datos generales del banco que fue seleccionado por el usuario
        /// PARAMETROS              : Datos: Contiene los datos de los parametros para la realización de la consulta
        /// CREO                    : Antonio Salvador Benavides Guardado
        /// FECHA_CREO              : 21/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Orden_Variacion(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
                Mi_SQL = Mi_SQL + "NVL(" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ",'0') AS No_Nota";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion;
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "='";
                Mi_SQL = Mi_SQL + Datos.P_Orden_Variacion_ID + "' AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio;
                Mi_SQL = Mi_SQL + "='" + Datos.P_Anio + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION    : Consultar_Datos_Calculo
        /// DESCRIPCION             : Consulta los datos generales del banco que fue seleccionado por el usuario
        /// PARAMETROS              : Datos: Contiene los datos de los parametros para la realización de la consulta
        /// CREO                    : Antonio Salvador Benavides Guardado
        /// FECHA_CREO              : 21/Septiembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Datos_Calculo(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " AS ANIO, ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Tasa_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado;
                Mi_SQL = Mi_SQL + ", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ";
                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + "='" + Datos.P_No_Calculo + "' AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "=" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ". " + Ope_Pre_Ordenes_Variacion.Campo_Anio + "=" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + ". " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "=" + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " AND ";
                Mi_SQL = Mi_SQL + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + "." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + "=" + Datos.P_Año_Calculo;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Obtener_Cuenta_Predial
        /// DESCRIPCION : Consulta los datos generales del banco que fue seleccionado por
        ///               el usuario
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Obtener_Cuenta_Predial(Cls_Ope_Caj_Pagos_Negocio Datos)
        {

            String Orden_Variacion = String.Format("{0:0000000000}", Convert.ToInt32(Datos.P_Orden_Variacion_ID));

            DataTable Dt_Traslado_Dominio = new DataTable();
            DataTable Tabla = new DataTable();
            try
            {
                String Mi_SQL = "SELECT  ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ",";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " ";
                Mi_SQL = Mi_SQL + " From " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " inner join ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " on ";
                Mi_SQL = Mi_SQL + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "=";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " ";
                Mi_SQL = Mi_SQL + " where ";
                Mi_SQL = Mi_SQL + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
                Mi_SQL = Mi_SQL + "='" + Orden_Variacion + "'";
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Tabla = dataset.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Tabla;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
        /// DESCRIPCION : Consulta la caja que tiene abierta el empleado para poder realizar
        ///               la recolección de la misma
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 19-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Caja_Empleado(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.

            try
            {
                //Consulta los datos generales de la caja que tiene abierta el empleado que requiere realizar la recolección del dinero
                Mi_SQL.Append("SELECT " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + " AS Caja, ");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Numero_De_Caja + ", ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id + ", ");
                Mi_SQL.Append("(" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Clave);
                Mi_SQL.Append("||' '||" + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Descripcion + ") AS Modulo, ");
                Mi_SQL.Append(Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_No_Turno);
                Mi_SQL.Append(" FROM " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + ", " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + ", " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Caja_Id + " = " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Caja_Id);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos + "." + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                Mi_SQL.Append(" AND " + Cat_Pre_Modulos.Tabla_Cat_Pre_Modulo + "." + Cat_Pre_Modulos.Campo_Modulo_Id + "=");
                Mi_SQL.Append(Cat_Pre_Cajas.Tabla_Cat_Pre_Caja + "." + Cat_Pre_Cajas.Campo_Modulo_Id);
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consulta_Cajero_General
        ///DESCRIPCIÓN          : Obtiene EL NOMBRE DEL CAJERO GENERAL
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vazquez
        ///FECHA_CREO           : 12/Octubre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static String Consulta_Cajero_General()
        {
            string Nombre_Cajero = string.Empty;
            String Mi_SQL = String.Empty;

            try
            {
                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " ||' " + "" + "'|| ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS EMPLEADO";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " INNER JOIN " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros;
                Mi_SQL = Mi_SQL + " ON " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros + "." + Ope_Pre_Parametros.Campo_Cajero_General_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;

                Nombre_Cajero = (string)OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar el los datos de los arqueos. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Nombre_Cajero;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Bancos_Ingresos
        /// DESCRIPCION : Consulta todos los bancos que estan asignados a Ingresos
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Bancos_Ingresos()
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta los bancos que pertenecen a ingresos
                Mi_SQL.Append("SELECT " + Cat_Nom_Bancos.Campo_Banco_ID + ", " + Cat_Nom_Bancos.Campo_Nombre);
                Mi_SQL.Append(" FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Bancos.Campo_Tipo + " = 'INGRESOS'");
                Mi_SQL.Append(" ORDER BY " + Cat_Nom_Bancos.Campo_Nombre);
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Pasivo_Cuenta_Predial
        /// DESCRIPCION : Consulta los folios de pago, segun la cuenta predial
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 22-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Pasivo_Cuenta_Predial(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta los datos generales del pasivo que se quiere consultar
                Mi_SQL.Append("SELECT distinct(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + "),");
                Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Descripcion + ",");
                Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Estatus + " ");
                //Mi_SQL.Append("(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + " + ");
                //Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Recargos + ") AS Total_Pagar, ");
                //Mi_SQL.Append("(" + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + "||' '||");
                //Mi_SQL.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ") AS Ingreso");

                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                //Mi_SQL.Append("," + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                //Mi_SQL.Append("," + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado);
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Cuenta_Predial + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Pasivo
        /// DESCRIPCION : Consulta todos los datos del recibo para poder realizar el pago
        ///               de acuerdo a la referencia que proporciono el usuario
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 22-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Pasivo(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta los datos generales del pasivo que se quiere consultar
                //if (Datos.P_Referencia.Trim().StartsWith("ING"))
                //{
                Mi_SQL.Append("SELECT " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".*, ");
                Mi_SQL.Append("(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + " + ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Recargos + ") AS Total_Pagar, ");
                Mi_SQL.Append("(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave + "||' '||");
                Mi_SQL.Append(Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion + ") AS Ingreso");
                Mi_SQL.Append(" FROM " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + ", " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID);
                Mi_SQL.Append(" = " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID);
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + " IN ('" + Datos.P_Referencia.Replace(",", "','") + "')");
                if (Datos.P_Estatus != "")
                {
                    Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".Estatus = '" + Datos.P_Estatus + "'");
                }
                if (Datos.P_No_Pago != "")
                {
                    Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".No_Pago = '" + Datos.P_No_Pago + "'");
                }
                Mi_SQL.Append(" ORDER BY " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pasivo);
                //}
                //else
                //{
                //    Mi_SQL.Append("SELECT " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".*, ");
                //    Mi_SQL.Append("(" + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Monto + " + ");
                //    Mi_SQL.Append(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Recargos + ") AS Total_Pagar, ");
                //    Mi_SQL.Append("(" + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Clave + "||' '||");
                //    Mi_SQL.Append(Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_Descripcion + ") AS Ingreso");
                //    Mi_SQL.Append(" FROM " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + ", " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                //    Mi_SQL.Append(" WHERE " + Cat_Pre_Claves_Ingreso.Tabla_Cat_Pre_Claves_Igreso + "." + Cat_Pre_Claves_Ingreso.Campo_SubConcepto_Ing_ID);
                //    Mi_SQL.Append(" = " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID);
                //    Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_Referencia + " IN ('" + Datos.P_Referencia.Replace(",", "','") + "')");
                //    if (Datos.P_Estatus != "")
                //    {
                //        Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".Estatus = '" + Datos.P_Estatus + "'");
                //    }
                //    if (Datos.P_No_Pago != "")
                //    {
                //        Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + ".No_Pago = '" + Datos.P_No_Pago + "'");
                //    }
                //    Mi_SQL.Append(" ORDER BY " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + "." + Ope_Ing_Pasivo.Campo_No_Pasivo);
                //}
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Detalles_Formas_Pago
        /// DESCRIPCION : Consultas las formas de pago que tuvo el ingreso que fue consultado
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 22-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Formas_Pago(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta las formas de pago que tuvo el ingreso pasivo
                Mi_SQL.Append("SELECT " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ".*, ");
                Mi_SQL.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Fecha_Creo + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Operacion + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_Estatus);
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + ", " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "." + Ope_Caj_Pagos_Detalles.Campo_No_Pago);
                Mi_SQL.Append(" = " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago);
                Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos + "." + Ope_Caj_Pagos.Campo_No_Pago + " = '" + Datos.P_No_Pago + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Plan_Pago_Banco
        /// DESCRIPCION : Consulta los datos generales del banco que fue seleccionado por
        ///               el usuario
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 23-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Plan_Pago_Banco(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta los detalles del banco que fue seleccionado por el usuario
                Mi_SQL.Append("SELECT * FROM " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);
                Mi_SQL.Append(" WHERE " + Cat_Nom_Bancos.Campo_Banco_ID + " = '" + Datos.P_Banco_ID + "'");
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Cuenta_Predial
        /// DESCRIPCION : Consulta la cuenta predial de acuerdo al id
        /// PARAMETROS  : Datos: Contiene los datos de los parametros para la realización
        ///                      de la consulta
        /// CREO        : Ismael Prieto Sánchez
        /// FECHA_CREO  : 15-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static String Consulta_Cuenta_Predial(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            try
            {
                //Consulta la cuenta predial
                Mi_SQL.Append("SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial);
                Mi_SQL.Append(" FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                Mi_SQL.Append(" WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'");
                return OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).ToString();
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error:[" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Pago_Caja
        /// DESCRIPCION : 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 22-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Pago_Caja(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Object No_Pago;      //Consecutivo del registro de la tabla en la base de datos
            Int32 No_Recibo;    //Obtiene el número de recibo que le pertence el pago del ingreso
            Object No_Operacion; //Obtiene el número de operacion que fue realizada durante en día de la caja
            Object Consecutivo;  //Obtiene el número de registro con el cual se va a dar de alta el detalle del pago en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            Cls_Ope_Caj_Pagos_Negocio Caja = new Cls_Ope_Caj_Pagos_Negocio();
            String Cuenta_Predial_ID = "";
            String No_Calculo = "";
            Int16 Año_Calculo = 0;
            Boolean Bandera_Entro = false;
            Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Dt_Clave = new DataTable();
            String Clave_Ingreso_Parametro_ID;
            String No_Convenio = "";
            Int32 No_Convenio_Pago = 0;
            String Contribuyente = "";
            String Concepto_Poliza = "";

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {

                Mi_SQL.Length = 0;
                P_Dt_Partidas_Poliza = Crear_Dt_Partidas_Poliza();

                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0000000000')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Pago = Comando_SQL.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(No_Pago))
                {
                    No_Pago = "0000000001";
                }
                else
                {
                    No_Pago = String.Format("{0:0000000000}", Convert.ToInt32(No_Pago) + 1);
                }
                Datos.P_No_Pago = No_Pago.ToString();

                Mi_SQL.Length = 0;
                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Operacion + "),'0')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Operacion = Comando_SQL.ExecuteOracleScalar().ToString();

                if (Convert.IsDBNull(No_Operacion))
                {
                    No_Operacion = Convert.ToInt32("1");
                }
                else
                {
                    No_Operacion = Convert.ToInt32(No_Operacion) + 1;
                }
                Mi_SQL.Length = 0;

                //Consulta el último No de Recibo que ha sido registrado durante el día
                if (Datos.P_No_Recibo == "")
                {
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Recibo + "), '0')");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                    Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                    Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    No_Recibo = Convert.ToInt32(Comando_SQL.ExecuteOracleScalar().ToString());

                    if (No_Recibo.ToString() == "0")
                    {
                        Mi_SQL.Length = 0;
                        //Consulta el No de Recibo inicial del turno que fue abierto
                        Mi_SQL.Append("SELECT " + Ope_Caj_Turnos.Campo_Recibo_Inicial + " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                        Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_Caja_Id + " = '" + Datos.P_Caja_ID + "'");
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'");
                        Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                        Comando_SQL.CommandText = Mi_SQL.ToString();
                        No_Recibo = Convert.ToInt32(Comando_SQL.ExecuteOracleScalar().ToString());
                    }
                    else
                    {
                        No_Recibo = Convert.ToInt32(No_Recibo) + 1;
                    }
                }
                else
                {
                    No_Recibo = Convert.ToInt32(Datos.P_No_Recibo);
                }
                Mi_SQL.Length = 0;

                //Actualiza el consecutivo del recibo
                Mi_SQL.Append("UPDATE " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos);
                Mi_SQL.Append(" SET " + Ope_Caj_Turnos.Campo_Contador_Recibo + " = " + (No_Recibo + 1));
                Mi_SQL.Append(" WHERE " + Ope_Caj_Turnos.Campo_No_Turno + " = '" + Datos.P_No_Turno + "'");
                Mi_SQL.Append(" AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                //Asigna la cuenta predial
                Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;

                //Consulta el campo Origen del Pasivo
                if (String.IsNullOrEmpty(Datos.P_Origen))
                {
                    Cls_Ope_Ing_Pasivos_Negocio Pasivos = new Cls_Ope_Ing_Pasivos_Negocio();
                    DataTable Dt_Pasivos;

                    Pasivos.P_Campos_Dinamicos = Ope_Ing_Pasivo.Campo_Origen;
                    Pasivos.P_Referencia = Datos.P_Referencia;
                    Dt_Pasivos = Pasivos.Consultar_Pasivos();
                    if (Dt_Pasivos != null)
                    {
                        if (Dt_Pasivos.Rows.Count > 0)
                        {
                            Datos.P_Origen = Dt_Pasivos.Rows[0][Ope_Ing_Pasivo.Campo_Origen].ToString();
                        }
                    }
                }

                //consulta la cuenta predial
                if (Datos.P_Referencia.StartsWith("TD") && Datos.P_Cuenta_Predial_ID != "")
                {
                    No_Calculo = Datos.P_Referencia.Substring(2);
                    Año_Calculo = 0;
                    if (No_Calculo.Length > 4)
                    {
                        Año_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
                        No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
                    }

                    Caja.P_No_Calculo = Convert.ToInt64(No_Calculo).ToString("0000000000");
                    Caja.P_Año_Calculo = Año_Calculo;
                    Caja.P_Comando_Oracle = Comando_SQL;
                    DataTable Dt_Orden_Variacion = Caja.Consultar_Datos_Calculo();

                    if (Dt_Orden_Variacion.Rows.Count > 0)
                    {
                        Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
                    }
                }

                Mi_SQL.Length = 0;
                //Inserta los datos en la tabla con los datos proporcionados por el usuario
                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append("(" + Ope_Caj_Pagos.Campo_No_Pago + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_No_Recibo + ", " + Ope_Caj_Pagos.Campo_No_Operacion + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_No_Turno + ", " + Ope_Caj_Pagos.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Monto_Corriente + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Monto_Recargos + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Tipo_Pago + ", " + Ope_Caj_Pagos.Campo_Total + ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Documento + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Empleado_ID + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")");
                Mi_SQL.Append(" VALUES ('" + No_Pago + "', " + No_Recibo + ", " + No_Operacion + ", '");
                Mi_SQL.Append(Datos.P_Caja_ID + "', '" + Datos.P_No_Turno + "', TO_DATE('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + "','DD-MM-YYYY'), 'PAGADO', ");
                Mi_SQL.Append(Datos.P_Monto_Corriente + ", " + Datos.P_Monto_Recargos + ", '");
                Mi_SQL.Append(Datos.P_Nombre_Usuario + "', ");
                if (Datos.P_Referencia.StartsWith("CDER") || Datos.P_Referencia.StartsWith("RDER"))
                {
                    Mi_SQL.Append("'DER SUP CONVENIO', ");
                    Concepto_Poliza = "CONVENIO DE DERECHOS DE SUPERVISION";
                }
                else if (Datos.P_Referencia.StartsWith("CFRA") || Datos.P_Referencia.StartsWith("RFRA"))
                {
                    Mi_SQL.Append("'FRACC CONVENIO', ");
                    Concepto_Poliza = "CONVENIO DE FRACCIONAMIENTOS";
                }
                else if (Datos.P_Referencia.StartsWith("CPRE") || Datos.P_Referencia.StartsWith("RPRE"))
                {
                    Mi_SQL.Append("'PREDIAL CONVENIO', ");
                    Concepto_Poliza = "CONVENIO DE PREDIAL";
                }
                else if (Datos.P_Referencia.StartsWith("CTRA") || Datos.P_Referencia.StartsWith("RTRA"))
                {
                    Mi_SQL.Append("'TRASLADO CONVENIO', ");
                    Concepto_Poliza = "CONVENIO DE TRASLADO";
                }
                else if (Datos.P_Referencia.StartsWith("DER"))
                {
                    if (Datos.P_Dt_Adeudos_Predial_Cajas == null && Datos.P_Dt_Adeudos_Predial_Cajas_Detalle == null)
                    {
                        Mi_SQL.Append("'DERECHOS SUPERVISION', ");
                        Concepto_Poliza = "DERECHOS DE SUPERVISION";
                    }
                    else
                    {
                        DataRow Fila = Datos.P_Dt_Adeudos_Predial_Cajas.Rows[0];
                        No_Convenio = Fila["NO_CONVENIO"].ToString();
                        No_Convenio_Pago = Convert.ToInt32(Fila["NO_PAGO"].ToString());
                        Mi_SQL.Append("'DER SUP CONVENIO', ");
                        Concepto_Poliza = "CONVENIO DE DERECHOS DE SUPERVISION";
                    }
                }
                else if (Datos.P_Referencia.StartsWith("IMP"))
                {
                    if (Datos.P_Dt_Adeudos_Predial_Cajas == null && Datos.P_Dt_Adeudos_Predial_Cajas_Detalle == null)
                    {
                        Mi_SQL.Append("'FRACCIONAMIENTOS', ");
                        Concepto_Poliza = "IMPUESTO DE FRACCIONAMIENTOS";
                    }
                    else
                    {
                        DataRow Fila = Datos.P_Dt_Adeudos_Predial_Cajas.Rows[0];
                        No_Convenio = Fila["NO_CONVENIO"].ToString();
                        No_Convenio_Pago = Convert.ToInt32(Fila["NO_PAGO"].ToString());
                        Mi_SQL.Append("'FRACC CONVENIO', ");
                        Concepto_Poliza = "CONVENIO DE FRACCIONAMIENTOS";
                    }
                }
                else if (Datos.P_Referencia.StartsWith("TD"))
                {
                    if (Datos.P_Dt_Adeudos_Predial_Cajas == null && Datos.P_Dt_Adeudos_Predial_Cajas_Detalle == null)
                    {
                        Mi_SQL.Append("'TRASLADO DOMINIO', ");
                        Concepto_Poliza = "TRASLADO DE DOMINIO";
                    }
                    else
                    {
                        DataRow Fila = Datos.P_Dt_Adeudos_Predial_Cajas.Rows[0];
                        No_Convenio = Fila["NO_CONVENIO"].ToString();
                        No_Convenio_Pago = Convert.ToInt32(Fila["NO_PAGO"].ToString());
                        Mi_SQL.Append("'TRASLADO CONVENIO', ");
                        Concepto_Poliza = "CONVENIO DE TRASLADO";
                    }
                }
                else if (Datos.P_Referencia.StartsWith("ING"))
                {
                    Mi_SQL.Append("'INGRESOS', ");
                    Concepto_Poliza = "INGRESOS";
                }
                else if (Datos.P_Referencia.StartsWith("OTRPAG"))
                {
                    Mi_SQL.Append("'OTROS PAGOS', ");
                    Concepto_Poliza = "OTROS PAGOS";
                }
                else if (Datos.P_Origen != null && Datos.P_Origen != "")
                {
                    Mi_SQL.Append("'TRAMITES', ");
                    Concepto_Poliza = "TRAMITES";
                }
                else if (Char.IsLetter(Datos.P_Referencia, 1))
                {
                    Mi_SQL.Append("'DOCUMENTOS', ");
                    Concepto_Poliza = "DOCUMENTOS";
                }
                else
                {
                    if (Datos.P_Dt_Adeudos_Predial_Cajas != null && Datos.P_Dt_Adeudos_Predial_Cajas.Rows.Count > 0)
                    {
                        if (Datos.P_Dt_Adeudos_Predial_Cajas.Rows[0]["NO_CONVENIO"].ToString() != "")
                        {
                            Mi_SQL.Append("'PREDIAL CONVENIO', ");
                            Concepto_Poliza = "CONVENIO DE PREDIAL";
                        }
                        else
                        {
                            Mi_SQL.Append("'PREDIAL', ");
                            Concepto_Poliza = "PREDIAL";
                        }
                    }
                    else
                    {
                        Mi_SQL.Append("'PREDIAL', ");
                        Concepto_Poliza = "PREDIAL";
                    }
                }
                Mi_SQL.Append(Datos.P_Total_Pagar + ", ");
                Mi_SQL.Append(Datos.P_Ajuste_Tarifario + ", ");
                Mi_SQL.Append("'" + Datos.P_Referencia + "', ");
                if (Cuenta_Predial_ID != "")
                {
                    Mi_SQL.Append("'" + Cuenta_Predial_ID + "', ");
                }
                else
                {
                    Mi_SQL.Append("null, ");
                }
                Mi_SQL.Append("'" + Datos.P_Empleado_ID + "', ");
                Mi_SQL.Append("SYSDATE)");

                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Datos.P_Dt_Formas_Pago.Rows)
                {
                    Mi_SQL.Length = 0;
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "),0)");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Consecutivo = Comando_SQL.ExecuteOracleScalar().ToString();
                    //Consecutivo = OracleHelper.ExecuteScalar(Transaccion_SQL, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = 1;
                    }
                    else
                    {
                        Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                    }
                    Mi_SQL.Length = 0;
                    if (Registro["Forma_Pago"].ToString() == "EFECTIVO") //Forma de Pago en Efectivo
                    {
                        //Inserción de forma de pago en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                        Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                    }
                    else //Forma de Pago en Banco
                    {
                        if (Registro["Forma_Pago"].ToString() == "BANCO")
                        {
                            //Inserción de forma de pago en la base de datos
                            Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + ", ");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_No_Transaccion + ", ");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion + ", " + Ope_Caj_Pagos_Detalles.Campo_Plan_Pago + ", ");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Meses + ", " + Ope_Caj_Pagos_Detalles.Campo_Monto + ", ");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Tarjeta_Bancaria + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                            Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Banco_ID"].ToString() + "', '");
                            Mi_SQL.Append(Registro["Forma_Pago"] + "', '" + Registro["No_Transaccion"].ToString() + "', '");
                            Mi_SQL.Append(Registro["No_Autorizacion"] + "', '" + Registro["Plan_Pago"] + "', ");
                            Mi_SQL.Append(Registro["Meses"].ToString() + ", " + Registro["Monto"].ToString() + ", '" + Registro["No_Tarjeta_Bancaria"] + "', " + Consecutivo + ")");
                        }
                        else //Forma de Pago en Cheque
                        {
                            if (Registro["Forma_Pago"].ToString() == "CHEQUE")
                            {
                                //Inserción de forma de pago en la base de datos
                                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                                Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Banco_ID + ", ");
                                Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_No_Cheque + ", ");
                                Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                                Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Banco_ID"].ToString() + "', '");
                                Mi_SQL.Append(Registro["Forma_Pago"].ToString() + "', '" + Registro["No_Cheque"] + "', ");
                                Mi_SQL.Append(Registro["Monto"] + ", " + Consecutivo + ")");
                            }
                            else //Forma de Pago de Transferencia
                            {
                                if (Registro["Forma_Pago"].ToString() == "TRANSFERENCIA")
                                {
                                    //Inserción de forma de pago en la base de datos
                                    Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                                    Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                                    Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Referencia_Transferencia + ", ");
                                    Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Banco_ID + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                                    Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                                    Mi_SQL.Append(Registro["Monto"].ToString() + ", '" + Registro["Referencia_Transferencia"].ToString() + "', '");
                                    Mi_SQL.Append(Registro["Banco_ID"].ToString() + "', " + Consecutivo + ")");
                                }

                                else //Ajuste tarifario
                                {
                                    if (Registro["Forma_Pago"].ToString() == "AJUSTE TARIFARIO") //Ajuste tarifario
                                    {
                                        //Inserción de forma de pago en la base de datos
                                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                                        Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                                    }
                                    else //Cambio
                                    {
                                        //Inserción de forma de pago en la base de datos
                                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                                        Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                                    }
                                }
                            }
                        }
                    }
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }
                Mi_SQL.Length = 0;
                //Elimina el ajuste tarifario
                //Mi_SQL.Append("DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                //Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Descripcion + " = 'AJUSTE TARIFARIO'");
                //Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                //Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                //Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                //Mi_SQL.Length = 0;
                //Ingresa el pasivo del ajuste tarifario
                if (Datos.P_Ajuste_Tarifario != 0)
                {
                    Mi_SQL.Append("SELECT NVL(" + Ope_Pre_Parametros.Campo_SubConcepto_Ajuste_Tarifa_ID + ", '')");
                    Mi_SQL.Append(" FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Clave_Ingreso_Parametro_ID = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Clave_Ingreso_Parametro_ID != "")
                    {
                        Rs_Claves_Ingreso.P_Clave_Ingreso_ID = Clave_Ingreso_Parametro_ID;
                        Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso_Por_ID();
                        // si se o obtuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            Mi_SQL.Length = 0;
                            Mi_SQL.Append("SELECT NVL(" + Ope_Ing_Pasivo.Campo_Contribuyente + ", '')");
                            Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                            Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                            Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                            Comando_SQL.CommandText = Mi_SQL.ToString();
                            Contribuyente = Comando_SQL.ExecuteOracleScalar().ToString();

                            //Ingresa el ajuste tarifario
                            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                            Rs_Modificar_Calculo.P_Cmd_Calculo = Comando_SQL;
                            Rs_Modificar_Calculo.P_Fecha_Tramite = Datos.P_Fecha_Pago.ToString();
                            Rs_Modificar_Calculo.P_Fecha_Vencimiento_Pasivo = Datos.P_Fecha_Pago.ToString();
                            Rs_Modificar_Calculo.P_Estatus = "POR PAGAR";
                            Rs_Modificar_Calculo.P_Nombre_Usuario = Datos.P_Nombre_Usuario;
                            Rs_Modificar_Calculo.P_Contribuyente = Contribuyente;
                            Rs_Modificar_Calculo.P_Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;
                            Rs_Modificar_Calculo.P_Referencia = Datos.P_Referencia;
                            Rs_Modificar_Calculo.P_Descripcion = "AJUSTE TARIFARIO";
                            Rs_Modificar_Calculo.P_SubConcepto_ID = Dt_Clave.Rows[0]["CLAVE_INGRESO_ID"].ToString();
                            Rs_Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0]["DEPENDENCIA_ID"].ToString();
                            Rs_Modificar_Calculo.P_Monto_Total_Pagar = Convert.ToString(Datos.P_Ajuste_Tarifario);
                            Rs_Modificar_Calculo.Alta_Pasivo();

                            Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                        }
                    }
                    else
                    {
                        throw new Exception("Error: No se puede realizar el pago, porque no esta establecida la clave de ajuste tarifario, favor de verificarlo.");
                    }

                    //Mi_SQL.Append("SELECT NVL(" + Ope_Pre_Parametros.Campo_Clave_Ing_Ajuste_Tarifa_ID + ", '')");
                    //Mi_SQL.Append(" FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros);
                    //Comando_SQL.CommandText = Mi_SQL.ToString();
                    //Clave_Ingreso_Parametro_ID = Comando_SQL.ExecuteOracleScalar().ToString();
                    //if (Clave_Ingreso_Parametro_ID != "")
                    //{
                    //    Rs_Claves_Ingreso.P_Clave_Ingreso_ID = Clave_Ingreso_Parametro_ID;
                    //    Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso_Por_ID();
                    //    // si se o obtuvieron resultados en la consulta de clave, dar de alta el pasivo
                    //    if (Dt_Clave.Rows.Count > 0)
                    //    {
                    //        //Ingresa el ajuste tarifario
                    //        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                    //        Rs_Modificar_Calculo.P_Cmd_Calculo = Comando_SQL;
                    //        Rs_Modificar_Calculo.P_Fecha_Tramite = Datos.P_Fecha_Pago.ToString();
                    //        Rs_Modificar_Calculo.P_Fecha_Vencimiento_Pasivo = Datos.P_Fecha_Pago.ToString();
                    //        Rs_Modificar_Calculo.P_Estatus = "POR PAGAR";
                    //        Rs_Modificar_Calculo.P_Nombre_Usuario = Datos.P_Nombre_Usuario;
                    //        Rs_Modificar_Calculo.P_Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;
                    //        Rs_Modificar_Calculo.P_Referencia = Datos.P_Referencia;
                    //        Rs_Modificar_Calculo.P_Descripcion = "AJUSTE TARIFARIO";
                    //        Rs_Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    //        Rs_Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    //        Rs_Modificar_Calculo.P_Monto_Total_Pagar = Convert.ToString(Datos.P_Ajuste_Tarifario);
                    //        Rs_Modificar_Calculo.Alta_Pasivo();
                    //    }
                    //}
                    //else
                    //{
                    //    throw new Exception("Error: No se puede realizar el pago, porque no esta establecida la clave de ajuste tarifario, favor de verificarlo.");
                    //}
                }

                Mi_SQL.Length = 0;
                Mi_SQL.Append("SELECT PASIVOS." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID);
                Mi_SQL.Append(", PASIVOS." + Ope_Ing_Pasivo.Campo_Concepto_Ing_ID);
                Mi_SQL.Append(", (NVL(PASIVOS." + Ope_Ing_Pasivo.Campo_Monto + ", 0) + NVL(PASIVOS." + Ope_Ing_Pasivo.Campo_Recargos + ", 0)) AS " + Ope_Ing_Pasivo.Campo_Monto);
                Mi_SQL.Append(", CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(", CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Dependencia_ID);
                Mi_SQL.Append(", CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Descripcion + " AS DESCRIPCION_CONCEPTO");
                Mi_SQL.Append(", SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion + " AS DESCRIPCION_SUBCONCEPTO");
                Mi_SQL.Append(", CUENTAS_CONTABLES." + Cat_Con_Cuentas_Contables.Campo_Cuenta);
                Mi_SQL.Append(", PASIVOS." + Ope_Ing_Pasivo.Campo_Origen);
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " PASIVOS");
                Mi_SQL.Append(", " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " CONCEPTOS");
                Mi_SQL.Append(", " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " SUBCONCEPTOS");
                Mi_SQL.Append(", " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " CUENTAS_CONTABLES");
                Mi_SQL.Append(" WHERE PASIVOS." + Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID + " = SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID);
                Mi_SQL.Append(" AND SUBCONCEPTOS." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID + " = CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID);
                Mi_SQL.Append(" AND CONCEPTOS." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID + " = CUENTAS_CONTABLES." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" AND PASIVOS." + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND PASIVOS." + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                DataSet Ds_Pasivos = OracleHelper.ExecuteDataset(Conexion_Base, CommandType.Text, Mi_SQL.ToString());
                if (Ds_Pasivos != null)
                {
                    if (Ds_Pasivos.Tables.Count > 0)
                    {
                        DataTable Dt_Pasivos = Ds_Pasivos.Tables[0];
                        DataRow Dr_Conceptos;
                        foreach (DataRow Dr_Pasivos in Dt_Pasivos.Rows)
                        {
                            Dr_Conceptos = P_Dt_Partidas_Poliza.NewRow();
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Partida] = P_Dt_Partidas_Poliza.Rows.Count + 1;
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Dr_Pasivos[Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID];
                            Dr_Conceptos[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Dr_Pasivos[Cat_Con_Cuentas_Contables.Campo_Cuenta];
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Concepto] = Dr_Pasivos["DESCRIPCION_SUBCONCEPTO"];
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Debe] = Dr_Pasivos[Ope_Ing_Pasivo.Campo_Monto];
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
                            Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID] = "00022";
                            //Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID] = "0000000654";
                            P_Dt_Partidas_Poliza.Rows.Add(Dr_Conceptos);
                        }
                        Dr_Conceptos = P_Dt_Partidas_Poliza.NewRow();
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Partida] = P_Dt_Partidas_Poliza.Rows.Count + 1;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = "00003";// Dr_Pasivos[Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID];
                        Dr_Conceptos[Cat_Con_Cuentas_Contables.Campo_Cuenta] = " 1127976747";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Concepto] = "BAJIO";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Haber] = Datos.P_Total_Pagar;
                        //Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID] = "";
                        //Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Dependencia_ID] = "";
                        P_Dt_Partidas_Poliza.Rows.Add(Dr_Conceptos);
                    }
                }

                Mi_SQL.Length = 0;
                //Actualiza el estatus del ingreso pasivo en la base de datos
                Mi_SQL.Append("UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" SET " + Ope_Ing_Pasivo.Campo_Estatus + " = 'PAGADO', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Recargos + " = " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_No_Recibo + " = " + No_Recibo + ", ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Pago + " = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Pago) + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                Mi_SQL.Length = 0;
                string Proteccion_Pago = "PAGADO/" + Convert.ToInt32(Datos.P_No_Caja) + "/" + Convert.ToInt32(No_Operacion) + "/" + String.Format("{0:yyyy.MM.dd}", Datos.P_Fecha_Pago) + "/" + String.Format("{0:HH:mm:ss}", Datos.P_Fecha_Pago) + "/" + String.Format("{0:###,##0.00}", Datos.P_Total_Pagar) + "/" + No_Recibo.ToString();
                if (Datos.P_Cuenta_Predial != "")
                {
                    Proteccion_Pago += "/" + Datos.P_Cuenta_Predial;
                }

                if (Datos.P_Referencia.StartsWith("CDER"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del convenio de derechos de supervisión A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Reestructura + " IS NULL");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del detalle del convenio  de derechos de supervisión A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + "='" + Datos.P_Referencia.Substring(4, 10) + "' AND ROWNUM=1) AND ");
                    Mi_SQL.Append(" " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("CFRA"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el ANTICIPO del Convenio A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del detalle del convenio A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Datos.P_Referencia.Substring(4, 10) + "' AND ROWNUM=1) AND ");
                    Mi_SQL.Append(" " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("CPRE"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el estatus de la CONSTANCIA A PAGADA
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " || ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " = ( "
                        + "SELECT * FROM ("
                        + "SELECT " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " || ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago
                        + " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial
                        + " WHERE TO_NUMBER(" + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + ") = ");
                    Mi_SQL.Append(Datos.P_Referencia.Substring(4, Datos.P_Referencia.Length - 4) + " AND ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR' ");
                    Mi_SQL.Append(" ORDER BY " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago);
                    Mi_SQL.Append(") WHERE ROWNUM = 1"
                        + ")");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("RTRA"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el ANTICIPO de la reestructura del Convenio de traslado de dominio A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anticipo_Reestructura + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de traslado de dominio A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + "='" + Datos.P_Referencia.Substring(4, 10) + "' AND ROWNUM=1 AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + Datos.P_Referencia.Substring(4, 10) + "')");
                    Mi_SQL.Append(") AND ");
                    Mi_SQL.Append(" " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + Datos.P_Referencia.Substring(4, 10) + "')");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("CTRA"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el ANTICIPO del Convenio de traslado de dominio A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                    Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del detalle del convenio de traslado de dominio A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Datos.P_Referencia.Substring(4, 10) + "' AND ROWNUM=1) AND ");
                    Mi_SQL.Append(" " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Reestructura + " IS NULL");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("RDER"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el ANTICIPO de la reestructura del Convenio de derechos de supervisión A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                    Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo_Reestructura + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de derechos de supervisión A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + "='" + Datos.P_Referencia.Substring(4, 10) + "' AND ROWNUM=1 AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + Datos.P_Referencia.Substring(4, 10) + "')");
                    Mi_SQL.Append(") AND ");
                    Mi_SQL.Append(" " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision + " WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " ='" + Datos.P_Referencia.Substring(4, 10) + "')");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("RFRA"))
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el ANTICIPO de la reestructura del Convenio de fraccionamiento A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                    Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo_Reestructura + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Mi_SQL.Length = 0;
                    //Actualiza el estatus del detalle de la reestructura del convenio de fraccionamiento A PAGADO
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                    Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = (SELECT " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + "='" + Datos.P_Referencia.Substring(4, 10) + "' AND ROWNUM=1 AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " ='" + Datos.P_Referencia.Substring(4, 10) + "')");
                    Mi_SQL.Append(") AND ");
                    Mi_SQL.Append(" " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + Datos.P_Referencia.Substring(4, 10) + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + "=(SELECT MAX(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Reestructura + ") FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos + " WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " ='" + Datos.P_Referencia.Substring(4, 10) + "')");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("DER"))
                {
                    Aplicar_Derechos_Supervision_Convenio(ref Comando_SQL, Datos.P_Dt_Adeudos_Predial_Cajas_Detalle, Datos.P_Dt_Adeudos_Predial_Cajas, Datos.P_Referencia, No_Pago.ToString(), No_Convenio, No_Convenio_Pago, Datos.P_Nombre_Usuario);

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("IMP"))
                {
                    Aplicar_Fraccionamiento_Convenio(ref Comando_SQL, Datos.P_Dt_Adeudos_Predial_Cajas_Detalle, Datos.P_Dt_Adeudos_Predial_Cajas, Datos.P_Referencia, No_Pago.ToString(), No_Convenio, No_Convenio_Pago, Datos.P_Nombre_Usuario);

                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("TD"))
                {
                    //Valida para afectar la cuenta con el primer pago
                    if (No_Convenio_Pago <= 1)
                    {
                        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                        //obterner el numero de orden de variacion
                        DataSet Ds_Cuenta;
                        No_Calculo = Datos.P_Referencia.Substring(2);
                        Año_Calculo = 0;
                        if (No_Calculo.Length > 4)
                        {
                            Año_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
                            No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
                        }

                        Caja.P_No_Calculo = Convert.ToInt64(No_Calculo).ToString("0000000000");
                        Caja.P_Año_Calculo = Año_Calculo;
                        DataTable Dt_Orden_Variacion = Caja.Consultar_Datos_Calculo();
                        M_Orden_Negocio.P_Cmmd = Comando_SQL;

                        if (Dt_Orden_Variacion.Rows.Count > 0)
                        {
                            Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
                            M_Orden_Negocio.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                            Caja.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                            Caja.P_Anio = Dt_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                            string Status = Dt_Orden_Variacion.Rows[0]["Estatus_Orden"].ToString().Trim();
                            DataTable Dt_Consultar_Orden_Variacion = Caja.Consultar_Orden_Variacion();
                            if (Status == "ACEPTADA" && Convert.ToDouble(Dt_Consultar_Orden_Variacion.Rows[0]["No_Nota"].ToString()) == 0)
                            {
                                M_Orden_Negocio.P_Cuenta_Predial = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial"].ToString().Trim();
                                M_Orden_Negocio.P_Contrarecibo = null;
                                M_Orden_Negocio.P_Estatus_Cuenta = "POR PAGAR";
                                M_Orden_Negocio.P_Agrupar_Dinamico = "True";
                                Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta();
                                Cargar_Datos(Cuenta_Predial_ID);
                                Cargar_Variacion(ref Comando_SQL, Dt_Orden_Variacion, Ds_Cuenta, Dt_Consultar_Orden_Variacion.Rows[0]["NO_CONTRARECIBO"].ToString().Trim());
                            }
                            else
                            {
                                M_Orden_Negocio.P_Año = Convert.ToInt32(Dt_Consultar_Orden_Variacion.Rows[0]["Anio"].ToString());
                                M_Orden_Negocio.P_Contrarecibo_Anio = Convert.ToInt16(Dt_Consultar_Orden_Variacion.Rows[0]["Anio"].ToString());
                                M_Orden_Negocio.P_Contrarecibo_No_Contrarecibo = Dt_Consultar_Orden_Variacion.Rows[0]["NO_CONTRARECIBO"].ToString().Trim();
                                M_Orden_Negocio.P_Contrarecibo_Estatus = "PAGADO";
                                M_Orden_Negocio.P_Contrarecibo_Usuario = Cls_Sessiones.Nombre_Empleado;
                                M_Orden_Negocio.Modificar_Contrarecibo();
                            }
                        }
                    }
                    Aplicar_Traslado_Convenio(ref Comando_SQL, Datos.P_Dt_Adeudos_Predial_Cajas_Detalle, Datos.P_Dt_Adeudos_Predial_Cajas, Datos.P_Referencia, No_Pago.ToString(), No_Convenio, No_Convenio_Pago, Datos.P_Nombre_Usuario);
                    Bandera_Entro = true;
                }
                else if (Datos.P_Referencia.StartsWith("ING"))
                {
                    Cls_Ope_Ing_Ordenes_Pago_Negocio Ordenes_Pago = new Cls_Ope_Ing_Ordenes_Pago_Negocio();
                    Ordenes_Pago.P_Estatus = "PAGADA";
                    Ordenes_Pago.P_Usuario = Datos.P_Nombre_Usuario;
                    Ordenes_Pago.P_No_Orden_Pago = Convert.ToInt64(Datos.P_Referencia.Trim().Substring(7)).ToString("0000000000");
                    Ordenes_Pago.P_Año = Convert.ToInt32(Datos.P_Referencia.Trim().Substring(3, 4));
                    Ordenes_Pago.P_Cmmd = Comando_SQL;
                    if (Ordenes_Pago.Modificar_Orden_Pago())
                    {
                        String No_Descuento;
                        Bandera_Entro = true;
                        No_Descuento = Obtener_Str_Dato_Consulta(ref Comando_SQL, Ope_Ing_Descuentos.Campo_No_Descuento, Ope_Ing_Descuentos.Tabla_Ope_Ing_Descuentos, Ope_Ing_Descuentos.Campo_Estatus + " = 'VIGENTE' AND " + Ope_Ing_Descuentos.Campo_Referencia + " = '" + Datos.P_Referencia + "'", "");
                        if (No_Descuento != null && No_Descuento != "")
                        {
                            Cls_Ope_Ing_Descuentos_Negocio Descuentos = new Cls_Ope_Ing_Descuentos_Negocio();
                            Bandera_Entro = false;
                            Descuentos.P_No_Descuento = No_Descuento;
                            Descuentos.P_Estatus = "APLICADO";
                            Descuentos.P_Referencia = Datos.P_Referencia;
                            Descuentos.P_Usuario = Datos.P_Nombre_Usuario;
                            Descuentos.P_Cmmd = Comando_SQL;
                            if (Descuentos.Modificar_Descuentos())
                            {
                                Bandera_Entro = true;
                            }
                        }
                    }


                    //Mi_SQL.Append("UPDATE " + Ope_Ing_Ordenes_Pago.Tabla_Ope_Ing_Ordenes_Pago);
                    //Mi_SQL.Append(" SET " + Ope_Ing_Ordenes_Pago.Campo_Estatus + " = 'PAGADA', ");
                    //Mi_SQL.Append(Ope_Ing_Ordenes_Pago.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    //Mi_SQL.Append(Ope_Ing_Ordenes_Pago.Campo_Fecha_Modifico + " = SYSDATE");
                    //Mi_SQL.Append(" WHERE " + Ope_Ing_Ordenes_Pago.Campo_Folio + " = '" + Datos.P_Referencia + "'");
                    //Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    //Int32 filas_afectadas = Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    //if (filas_afectadas > 0)
                    //{
                    //    Bandera_Entro = true;
                    //}
                }
                else if (Datos.P_Origen != null && Datos.P_Origen != "")
                {
                    Afectaciones_Por_Tipo_Pasivo(Datos.P_Origen, Datos.P_Referencia, Cls_Sessiones.Nombre_Empleado, Comando_SQL);
                    Bandera_Entro = true;
                }
                else if (Char.IsLetter(Datos.P_Referencia, 1) && !Datos.P_Referencia.StartsWith("OTRPAG"))
                {
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias);
                    Mi_SQL.Append(" SET " + Ope_Pre_Constancias.Campo_Estatus + " = 'PAGADA', ");
                    Mi_SQL.Append(Ope_Pre_Constancias.Campo_Proteccion_Pago + " = '" + Proteccion_Pago + "', ");
                    Mi_SQL.Append(Ope_Pre_Constancias.Campo_Usuario_Modifico + " = '" + Datos.P_Nombre_Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Constancias.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Constancias.Campo_Folio + " = '" + Datos.P_Referencia + "'");
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Int32 filas_afectadas = Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    if (filas_afectadas > 0)
                    {
                        Bandera_Entro = true;
                    }
                }
                else if (Datos.P_Cuenta_Predial_ID != "" && !Bandera_Entro) //Predial
                {
                    Aplicar_Adeudos_Predial(ref Comando_SQL, Datos.P_Dt_Adeudos_Predial_Cajas, Datos.P_Dt_Adeudos_Predial_Cajas_Detalle, Datos.P_Cuenta_Predial_ID, No_Pago.ToString(), Datos.P_Ajuste_Tarifario, Datos.P_Nombre_Usuario);
                    Bandera_Entro = true;
                }
                if (!Bandera_Entro && !Datos.P_Referencia.StartsWith("OTRPAG"))
                {
                    throw new Exception("Error: No es posible realizar el pago, debido a que no hay la sufiente información favor de verificarlo.");
                }

                if (Bandera_Entro)
                {
                    Alta_Poliza(Datos, Concepto_Poliza, Comando_SQL);
                }

                try
                {
                    Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                }
                catch
                {
                }
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]" + " SQL: " + Mi_SQL);
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
        ///DESCRIPCIÓN: asignar datos de cuenta a los controles
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private static Boolean Cargar_Datos(String Cuenta_Predial_ID)
        {
            Boolean Datos_Cargados = false;
            try
            {

                Busqueda_Propietarios(Cuenta_Predial_ID);
                Datos_Cargados = true;

            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.Message);
            }
            return Datos_Cargados;
        }

        private static void Busqueda_Propietarios(String Cuenta_Predial_ID)
        {
            DataSet Ds_Prop;
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            try
            {
                //string Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
                M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                //M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
                Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
                if (Ds_Prop.Tables[0].Rows.Count > 0)
                {
                    //Session.Remove("Ds_Prop_Datos");
                    //Session["Ds_Prop_Datos"] = Ds_Prop;
                    //Hdn_Propietario_ID.Value = Ds_Prop.Tables[0].Rows[0]["Propietario"].ToString().Trim();
                    //Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
                }
            }
            catch (Exception Ex)
            {
                //Mensaje_Error(Ex.Message);
            }

        }
        private static String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
        {
            String Periodo = "";
            int Indice = 0;
            Periodo_Corriente_Validado = false;
            Periodo_Rezago_Validado = false;

            if (Periodos.IndexOf("-") >= 0)
            {
                if (Periodos.Split('-').Length == 2)
                {
                    //Valida el segundo nodo del arreglo
                    if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                        Periodo += "-";
                        Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                        Periodo_Corriente_Validado = true;
                    }
                }
                else
                {
                    if (Periodos.Contains("/"))
                    {
                        Indice = Periodos.IndexOf("/");
                        Periodo = Periodos.Substring(Indice - 1, 1);
                        Periodo += "-";
                        Indice = Periodos.IndexOf("/", Indice + 1);
                        Periodo += Periodos.Substring(Indice - 1, 1);
                        Periodo_Rezago_Validado = true;
                    }
                    else
                    {
                        Periodo = Periodos.Substring(0, 3);
                        Periodo_Corriente_Validado = true;
                    }
                }
            }
            return Periodo;
        }

        private static DataTable Consultar_Variacion_Copropietarios(String Cuenta_Predial_ID, String Orden_Variacion_ID, Int32 Anio)
        {
            //DataSet Ds_Copropietarios_Cuenta;
            DataTable Dt_Copropietarios_Cuenta;
            DataSet Ds_Copropietarios_Variacion;
            DataTable Dt_Copropietarios_Variacion;
            DataTable Dt_Temp_Copropietarios = new DataTable();
            DataRow Dr_Temp_Copropietario;
            Boolean Copropietario_Nuevo;
            try
            {
                Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                M_Orden_Negocio.P_Orden_Variacion_ID = Orden_Variacion_ID;
                M_Orden_Negocio.P_Año = Anio;
                M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;// Session["Cuenta_Predial_ID"].ToString().Trim();
                Dt_Copropietarios_Cuenta = M_Orden_Negocio.Consulta_Co_Propietarios();
                Ds_Copropietarios_Variacion = M_Orden_Negocio.Consultar_Copropietarios_Variacion();
                if (Ds_Copropietarios_Variacion.Tables.Count > 0)
                {
                    Dt_Copropietarios_Variacion = Ds_Copropietarios_Variacion.Tables[0];
                    //if (Ds_Copropietarios_Cuenta.Tables.Count > 0)
                    {
                        //Dt_Copropietarios_Cuenta = Ds_Copropietarios_Cuenta.Tables[0];
                        if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                        {
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("RFC", typeof(String)));
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("NOMBRE_CONTRIBUYENTE", typeof(String)));
                            Dt_Temp_Copropietarios.Columns.Add(new DataColumn("ESTATUS_VARIACION", typeof(String)));
                            if (Dt_Copropietarios_Cuenta.Rows.Count > 0)
                            {
                                foreach (DataRow Copropietario_Cuenta in Dt_Copropietarios_Cuenta.Rows)
                                {
                                    Copropietario_Nuevo = false;
                                    foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                    {
                                        if (Copropietario_Cuenta[Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString() == Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString())
                                        {
                                            Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                            Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                            Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                            Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                            Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "ACTUAL";
                                            Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                        }
                                    }
                                }
                                foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                {
                                    Copropietario_Nuevo = true;
                                    foreach (DataRow Copropietario_Cuenta in Dt_Copropietarios_Cuenta.Rows)
                                    {
                                        if (Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() == Copropietario_Cuenta[Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString())
                                        {
                                            Copropietario_Nuevo = false;
                                        }
                                    }
                                    if (Copropietario_Nuevo)
                                    {
                                        Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                        Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                        Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                        Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                        Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
                                        Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                    }
                                }
                            }
                            else
                            {
                                foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                                {
                                    Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                                    Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                    Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                                    Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                                    Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
                                    Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Temp_Copropietarios;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Cargar_Variacion
        ///DESCRIPCIÓN          : Consulta los datos de la Orden de Variacción
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private static Boolean Cargar_Variacion(ref OracleCommand Cmd, DataTable Dt_Caja_Variacion, DataSet Ds_Cuenta, string No_ContraRecibo)
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            DataTable Dt_Orden_Variacion;
            DataTable Dt_Detalle_Orden_Variacion;
            Boolean Variacion_Cargada = false;

            if (Dt_Caja_Variacion.Rows.Count > 0)
            {
                Orden_Variacion.P_Incluir_Campos_Foraneos = true;
                Orden_Variacion.P_Orden_Variacion_ID = Dt_Caja_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                Orden_Variacion.P_Generar_Orden_Anio = Dt_Caja_Variacion.Rows[0]["Anio"].ToString().Trim();

                Dt_Orden_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion();
                Dt_Detalle_Orden_Variacion = Orden_Variacion.P_Generar_Orden_Dt_Detalles;

                if (Dt_Orden_Variacion.Rows.Count > 0)
                {
                    Orden_Variacion.P_Cmmd = Cmd;
                    String Tasa_ID = "";
                    try
                    {
                        Tasa_ID = Dt_Caja_Variacion.Rows[0]["Tasa_ID"].ToString().Trim();
                    }
                    catch
                    {
                    }
                    Aplicar_Variacion(ref Cmd, Dt_Orden_Variacion);
                    Orden_Variacion.P_Año = Convert.ToInt32(Dt_Orden_Variacion.Rows[0]["Anio"]);
                    Orden_Variacion.P_Contrarecibo_Anio = Convert.ToInt16(Dt_Orden_Variacion.Rows[0]["Anio"]);
                    Orden_Variacion.P_Contrarecibo_No_Contrarecibo = No_ContraRecibo;
                    Orden_Variacion.P_Contrarecibo_Estatus = "PAGADO";
                    Orden_Variacion.P_Contrarecibo_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Orden_Variacion.Modificar_Contrarecibo();
                    //poner debajo de MODIFICAR CONTRARECIBO EN CARGAR VARIACION
                    Orden_Variacion.Modificar_Calculo_Traslado();
                    Orden_Variacion.P_Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
                    Orden_Variacion.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                    Orden_Variacion.P_Grupo_Movimiento_ID = Dt_Orden_Variacion.Rows[0]["Grupo_Movimiento_ID"].ToString();
                    Orden_Variacion.P_Tipo_Predio_ID = Dt_Orden_Variacion.Rows[0]["Tipo_Predio_ID"].ToString();
                    //Arma el No de Nota
                    Int32 No_Nota_Consecutivo = Obtener_Dato_Consulta(ref Cmd, "NVL(MAX(" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + "), 0) + 1", Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion, Ope_Pre_Ordenes_Variacion.Campo_Anio + " = " + Orden_Variacion.P_Año + " AND " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " = '" + Orden_Variacion.P_Grupo_Movimiento_ID + "' AND " + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Orden_Variacion.P_Tipo_Predio_ID + "'", 1);
                    Int32 No_Nota_Inicila = Obtener_Dato_Consulta(ref Cmd, "NVL(MAX(" + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial + "), 0)", Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles, Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " = " + Orden_Variacion.P_Año + " AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " = '" + Orden_Variacion.P_Grupo_Movimiento_ID + "' AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " = '" + Orden_Variacion.P_Tipo_Predio_ID + "'", 1);
                    Orden_Variacion.P_No_Nota = No_Nota_Consecutivo > No_Nota_Inicila ? No_Nota_Consecutivo : No_Nota_Inicila;
                    Orden_Variacion.P_Fecha_Nota = DateTime.Now;
                    Orden_Variacion.P_No_Nota_Impreso = "NO";
                    Orden_Variacion.Modificar_Orden_Variacion();
                }
            }

            return Variacion_Cargada;
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Variacion
        ///DESCRIPCIÓN          : Guarda los datos de la Orden de Variación
        ///PARAMETROS: 
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 21/Agosto/2011
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************    
        private static Boolean Aplicar_Variacion(ref OracleCommand Cmmd, DataTable Dt_Orden_Variacion)
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Boolean Variacion_Aceptada = false;

            Ordenes_Variacion.P_Año = Convert.ToInt32(Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Anio]);
            Ordenes_Variacion.P_Orden_Variacion_ID = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion].ToString().Trim();
            Ordenes_Variacion.P_Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID].ToString();
            Ordenes_Variacion.P_Copropietario_Cuenta_Predial_ID = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID].ToString();
            Ordenes_Variacion.P_Contrarecibo_No_Contrarecibo = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo].ToString();
            Ordenes_Variacion.P_Cmmd = Cmmd;

            if (Ordenes_Variacion.Aplicar_Variacion_Orden())
            {
                Variacion_Aceptada = true;
            }
            return Variacion_Aceptada;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones)
        {
            return Obtener_Dato_Consulta(ref Cmmd, Campo, Tabla, Condiciones, 0);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones, Int32 Dato_Salida_Default)
        {
            String Mi_SQL;
            Int32 Dato_Consulta = 0;

            try
            {
                Mi_SQL = "SELECT " + Campo;
                if (Tabla != "")
                {
                    Mi_SQL += " FROM " + Tabla;
                }
                if (Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }

                //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Cmmd.CommandText = Mi_SQL;
                Dato_Consulta = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
                if (Convert.IsDBNull(Dato_Consulta))
                {
                    Dato_Consulta = 1;
                }
                else
                {
                    Dato_Consulta = Dato_Consulta + 1;
                }
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }

            if (Dato_Consulta == 0)
            {
                Dato_Consulta = Dato_Salida_Default;
            }
            return Dato_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Str_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 13/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Str_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones, String Dato_Salida_Default)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                Mi_SQL = "SELECT " + Campo;
                if (Tabla != "")
                {
                    Mi_SQL += " FROM " + Tabla;
                }
                if (Condiciones != "")
                {
                    Mi_SQL += " WHERE " + Condiciones;
                }

                //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Cmmd.CommandText = Mi_SQL;
                Dato_Consulta = Cmmd.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(Dato_Consulta))
                {
                    Dato_Consulta = "";
                }
            }
            catch //(OracleException Ex)
            {
                //Indicamos el mensaje 
                //throw new Exception(Ex.ToString());
            }

            if (Dato_Consulta == "")
            {
                Dato_Consulta = Dato_Salida_Default;
            }
            return Dato_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
        ///DESCRIPCIÓN          : Consulta los montos de un convenio o reestructura según sea el caso
        ///PARAMETROS:     
        ///CREO                 : Miguel Angel Bedolla Moreno
        ///FECHA_CREO           : 21/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static DataTable Obtener_Dato_Consulta(String cuenta_predial, Int32 Anio)
        {
            String Mi_SQL;
            DataTable Dt_Montos = new DataTable();

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0) as PAGO_1, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0) as PAGO_2, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0) as PAGO_3, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0) as PAGO_4, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0) as PAGO_5, ";
                Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0) as PAGO_6 ";
                Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial + "' AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + "=" + Anio;
                DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataset != null)
                {
                    Dt_Montos = dataset.Tables[0];
                }
            }
            catch
            {
            }
            finally
            {
            }

            return Dt_Montos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
        ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
        ///PARAMETROS:     
        ///CREO                 : Antonio Salvador Benvides Guardado
        ///FECHA_CREO           : 24/Agosto/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static String Obtener_Dato_Consulta(String cuenta_predial_id)
        {
            String Mi_SQL;
            String Dato_Consulta = "";

            try
            {
                Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial_id + "'";

                OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Dr_Dato.Read())
                {
                    if (Dr_Dato[0] != null)
                    {
                        Dato_Consulta = Dr_Dato[0].ToString();
                    }
                    else
                    {
                        Dato_Consulta = "";
                    }
                    Dr_Dato.Close();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato = null;
            }
            catch
            {
            }
            finally
            {
            }

            return Dato_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Adeudos_Predial
        ///DESCRIPCIÓN          : Realiza la aplicacion de los adeudos de predial
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 13/Octubre/2011
        ///MODIFICO:            : Armando Zavala Moreno
        ///FECHA_MODIFICO       : 09/Mayo/2012
        ///CAUSA_MODIFICACIÓN   : El pago de Honorarios no era correcto
        ///*******************************************************************************
        private static void Aplicar_Adeudos_Predial(ref OracleCommand Cmmd, DataTable Dt_Adeudos, DataTable Dt_Adeudos_Detalle, string Cuenta_Predial_ID, string No_Pago, double Ajuste_Tarifario, string Usuario)
        {
            String Mi_SQL; //Variable para ejecutar el query
            String No_Recargo;
            Double Monto_Corriente = 0;
            Double Monto_Rezago = 0;
            Double Monto_Honorarios = 0;
            Double Monto_Recargos = 0;
            Double Monto_Moratorios = 0;
            Double Monto_Descuento_Recargos = 0;
            Double Monto_Descuento_Moratorios = 0;
            Double Monto_Descuento_Honorarios = 0;
            Double Monto_Descuento_Pronto_Pago = 0;
            DataRow Registro_Perido;
            Int32 Bimestre_Inicial = 0;
            Int32 Bimestre_Final = 0;
            Int32 Anio_Inicial = 0;
            Int32 Anio_Final = 0;
            String Periodo_Corriente = "";
            String Periodo_Rezago = "";
            String No_Adeudo = "";
            String No_Pago_Convenio = "";
            String No_Pago_Convenio_Inicial = "";
            String No_Convenio = "";
            String No_Descuento = "";

            try
            {
                //Obtiene el periodo inicial y final
                if (Dt_Adeudos.Rows.Count > 0)
                {
                    //Selecciona el periodo inicial
                    Registro_Perido = Dt_Adeudos.Rows[0];
                    //Valida si trae periodo
                    if (!String.IsNullOrEmpty(Registro_Perido["BIMESTRE"].ToString()))
                    {
                        Bimestre_Inicial = Convert.ToInt32(Registro_Perido["BIMESTRE"].ToString());
                        Anio_Inicial = Convert.ToInt32(Registro_Perido["ANIO"].ToString());
                        //Selecciona el periodo final
                        Registro_Perido = Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1];
                        Bimestre_Final = Convert.ToInt32(Registro_Perido["BIMESTRE"].ToString());
                        Anio_Final = Convert.ToInt32(Registro_Perido["ANIO"].ToString());
                        //Asinga los periodos
                        if ((Anio_Inicial == Anio_Final && Anio_Inicial < DateTime.Now.Year) || (Anio_Inicial != Anio_Final && Anio_Final < DateTime.Now.Year))
                        {
                            if (Obtener_Dato_Consulta(Cuenta_Predial_ID) == "SI")
                            {
                                Boolean Bimestre_encontrado = false;
                                Boolean Cuota_Minima_Aplicable = false;
                                int indice = Bimestre_Final;
                                DataTable Dt_Adeudos_Ultimo_Anio = Obtener_Dato_Consulta(Cuenta_Predial_ID, Anio_Final);
                                foreach (DataRow Dr_Renglon_Actual in Dt_Adeudos_Ultimo_Anio.Rows)
                                {
                                    if (!Bimestre_encontrado)
                                    {
                                        if (indice == Bimestre_Final)
                                        {
                                            indice++;
                                            for (int i = indice; i < 7; i++)
                                            {
                                                if (Dr_Renglon_Actual["PAGO_" + indice].ToString() == "0")
                                                {
                                                    Cuota_Minima_Aplicable = true;
                                                }
                                                else
                                                {
                                                    Cuota_Minima_Aplicable = false;
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                    indice++;
                                }
                                if (Cuota_Minima_Aplicable)
                                {
                                    Periodo_Rezago = Bimestre_Inicial + "/" + Anio_Final + "-" + 6 + "/" + Anio_Final;
                                }
                                else
                                {
                                    Periodo_Rezago = Bimestre_Inicial + "/" + Anio_Final + "-" + Bimestre_Final + "/" + Anio_Final;
                                }
                            }
                            else
                            {
                                Periodo_Rezago = Bimestre_Inicial.ToString() + "/" + Anio_Inicial + "-" + Bimestre_Final + "/" + Anio_Final;
                            }
                        }
                        else
                        {
                            if (Anio_Inicial == Anio_Final && Anio_Inicial >= DateTime.Now.Year)
                            {
                                if (Obtener_Dato_Consulta(Cuenta_Predial_ID) == "SI")
                                {
                                    Boolean Bimestre_encontrado = false;
                                    Boolean Cuota_Minima_Aplicable = false;
                                    int indice = Bimestre_Final;
                                    DataTable Dt_Adeudos_Ultimo_Anio = Obtener_Dato_Consulta(Cuenta_Predial_ID, Anio_Final);
                                    foreach (DataRow Dr_Renglon_Actual in Dt_Adeudos_Ultimo_Anio.Rows)
                                    {
                                        if (!Bimestre_encontrado)
                                        {
                                            if (indice == Bimestre_Final)
                                            {
                                                indice++;
                                                for (int i = indice; i < 7; i++)
                                                {
                                                    if (Dr_Renglon_Actual["PAGO_" + indice].ToString() == "0")
                                                    {
                                                        Cuota_Minima_Aplicable = true;
                                                    }
                                                    else
                                                    {
                                                        Cuota_Minima_Aplicable = false;
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        indice++;
                                    }
                                    if (Cuota_Minima_Aplicable)
                                    {
                                        Periodo_Corriente = Bimestre_Inicial.ToString() + "/" + Anio_Inicial + "-" + 6 + "/" + Anio_Final;
                                    }
                                    else
                                    {
                                        Periodo_Corriente = Bimestre_Inicial.ToString() + "/" + Anio_Inicial + "-" + Bimestre_Final + "/" + Anio_Final;
                                    }
                                }
                                else
                                {
                                    Periodo_Corriente = Bimestre_Inicial.ToString() + "/" + Anio_Inicial + "-" + Bimestre_Final + "/" + Anio_Final;
                                }
                            }
                            else
                            {
                                if (Anio_Inicial < Anio_Final && Anio_Inicial < DateTime.Now.Year)
                                {
                                    Periodo_Rezago = Bimestre_Inicial + "/" + Anio_Inicial + "-6/" + (Anio_Final - 1);
                                }
                                if (Anio_Final >= DateTime.Now.Year)
                                {
                                    if (Obtener_Dato_Consulta(Cuenta_Predial_ID) == "SI")
                                    {
                                        Boolean Bimestre_encontrado = false;
                                        Boolean Cuota_Minima_Aplicable = false;
                                        int indice = Bimestre_Final;
                                        DataTable Dt_Adeudos_Ultimo_Anio = Obtener_Dato_Consulta(Cuenta_Predial_ID, Anio_Final);
                                        foreach (DataRow Dr_Renglon_Actual in Dt_Adeudos_Ultimo_Anio.Rows)
                                        {
                                            if (!Bimestre_encontrado)
                                            {
                                                if (indice == Bimestre_Final)
                                                {
                                                    indice++;
                                                    for (int i = indice; i < 7; i++)
                                                    {
                                                        if (Dr_Renglon_Actual["PAGO_" + indice].ToString() == "0")
                                                        {
                                                            Cuota_Minima_Aplicable = true;
                                                        }
                                                        else
                                                        {
                                                            Cuota_Minima_Aplicable = false;
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                            indice++;
                                        }
                                        if (Cuota_Minima_Aplicable)
                                        {
                                            Periodo_Corriente = "1/" + Anio_Final + "-" + 6 + "/" + Anio_Final;
                                        }
                                        else
                                        {
                                            Periodo_Corriente = "1/" + Anio_Final + "-" + Bimestre_Final + "/" + Anio_Final;
                                        }
                                    }
                                    else
                                    {
                                        Periodo_Corriente = "1/" + Anio_Final + "-" + Bimestre_Final + "/" + Anio_Final;
                                    }
                                }
                            }
                        }
                    }
                }
                //Recorre los adeudos de predial para aplicarlos
                if (Dt_Adeudos.Rows.Count > 0)
                {
                    if (Dt_Adeudos.Rows[0]["No_Pago"].ToString() != "")
                    {
                        No_Pago_Convenio = Dt_Adeudos.Rows[0]["No_Pago"].ToString();
                        No_Pago_Convenio_Inicial = Dt_Adeudos.Rows[0]["No_Pago"].ToString();
                    }
                    if (Dt_Adeudos.Rows[0]["No_Convenio"].ToString() != "")
                    {
                        No_Convenio = Dt_Adeudos.Rows[0]["No_Convenio"].ToString();
                    }
                    foreach (DataRow Registro in Dt_Adeudos.Rows)
                    {
                        //Valida si es de convenio o de predial
                        if (Registro["No_Convenio"].ToString() == "")
                        {
                            //Asigna el numero de adeudo
                            No_Adeudo = Registro["NO_ADEUDO"].ToString();

                            //Inserta el adeudo pagado
                            Mi_SQL = "INSERT INTO " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + " (" + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Adeudo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre;
                            Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Monto + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Usuario_Creo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Fecha_Creo + ")";
                            Mi_SQL += " VALUES ('" + No_Pago + "', '" + Registro["No_Adeudo"].ToString() + "', " + Registro["Anio"].ToString() + ", " + Registro["Bimestre"].ToString() + ", " + Registro["Monto"].ToString();
                            Mi_SQL += ", '" + Usuario + "', SYSDATE)";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();

                            //Actualiza el adeudo con el monto del pago
                            Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET";
                            if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 1)
                            {
                                Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1;
                            }
                            else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 2)
                            {
                                Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2;
                            }
                            else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 3)
                            {
                                Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3;
                            }
                            else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 4)
                            {
                                Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4;
                            }
                            else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 5)
                            {
                                Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5;
                            }
                            else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 6)
                            {
                                Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " = " + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6;
                            }
                            Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + Registro["NO_ADEUDO"] + "'";
                            Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                            Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                        else
                        {
                            //Valida si trae datos
                            if (!String.IsNullOrEmpty(Registro["Anio"].ToString()) && !String.IsNullOrEmpty(Registro["Bimestre"].ToString()) && !String.IsNullOrEmpty(Registro["Monto"].ToString()))
                            {
                                //Consulta el no de adeudo de acuerdo al año
                                Mi_SQL = "SELECT " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo;
                                Mi_SQL += " FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial;
                                Mi_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                                Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                                Cmmd.CommandText = Mi_SQL;
                                No_Adeudo = Cmmd.ExecuteScalar().ToString();

                                //Inserta el adeudo pagado
                                Mi_SQL = "INSERT INTO " + Ope_Caj_Pagos_Adeudos_Predial.Tabla_Ope_Caj_Pagos_Adeudos_Predial + " (" + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Pago + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Adeudo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Anio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Bimestre;
                                Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Monto + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_No_Convenio_Pago;
                                Mi_SQL += ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Usuario_Creo + ", " + Ope_Caj_Pagos_Adeudos_Predial.Campo_Fecha_Creo + ")";
                                Mi_SQL += " VALUES ('" + No_Pago + "', '" + No_Adeudo + "', " + Registro["Anio"].ToString() + ", " + Registro["Bimestre"].ToString() + ", " + Registro["Monto"].ToString();
                                Mi_SQL += ", '" + Registro["NO_CONVENIO"].ToString() + "', " + Registro["NO_PAGO"].ToString();
                                Mi_SQL += ", '" + Usuario + "', SYSDATE)";
                                Cmmd.CommandText = Mi_SQL;
                                Cmmd.ExecuteNonQuery();

                                //Actualiza el adeudo con el monto del pago
                                Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET";
                                if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 1)
                                {
                                    Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + " + " + Registro["Monto"].ToString();
                                }
                                else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 2)
                                {
                                    Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + " + " + Registro["Monto"].ToString();
                                }
                                else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 3)
                                {
                                    Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + " + " + Registro["Monto"].ToString();
                                }
                                else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 4)
                                {
                                    Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + " + " + Registro["Monto"].ToString();
                                }
                                else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 5)
                                {
                                    Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + " + " + Registro["Monto"].ToString();
                                }
                                else if (Convert.ToInt32(Registro["Bimestre"].ToString()) == 6)
                                {
                                    Mi_SQL += " " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " = " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + " + " + Registro["Monto"].ToString();
                                }
                                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " WHERE " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + No_Adeudo + "'";
                                Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                                Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                                Cmmd.CommandText = Mi_SQL;
                                Cmmd.ExecuteNonQuery();
                            }

                            //Actualiza el pago del convenio
                            if (No_Pago_Convenio != Registro["No_Pago"].ToString())
                            {
                                //Actualiza el anticipo si es la primer parcialidad
                                if (Convert.ToInt32(No_Pago_Convenio) == 1)
                                {
                                    Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                                    Mi_SQL += " SET " + Ope_Pre_Convenios_Predial.Campo_Anticipo + " = 'PAGADO', ";
                                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                                    Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                                    Mi_SQL += " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + Registro["NO_CONVENIO"].ToString() + "'";
                                    Cmmd.CommandText = Mi_SQL;
                                    Cmmd.ExecuteNonQuery();
                                }

                                //Actualiza el detalle
                                Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                                Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'";
                                Mi_SQL += ", " + Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                                Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + Registro["NO_CONVENIO"].ToString() + "'";
                                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " = " + No_Pago_Convenio;
                                Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'";
                                Cmmd.CommandText = Mi_SQL;
                                Cmmd.ExecuteNonQuery();
                                No_Pago_Convenio = Registro["No_Pago"].ToString();
                            }
                        }

                        //Actualiza el estatus del adeudo
                        if (No_Adeudo != "")
                        {
                            Mi_SQL = "UPDATE " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " SET " + Ope_Pre_Adeudos_Predial.Campo_Estatus + " = 'PAGADO', ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Adeudos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE ((" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ")";
                            Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ")";
                            Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ")";
                            Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ")";
                            Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ")";
                            Mi_SQL += " + (" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + " - " + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ")) = 0";
                            Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_No_Adeudo + " = '" + No_Adeudo + "'";
                            Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + " = " + Registro["ANIO"];
                            Mi_SQL += " AND " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                    if (No_Pago_Convenio != "")
                    {
                        //Actualiza el anticipo si es la primer parcialidad
                        if (Convert.ToInt32(No_Pago_Convenio_Inicial) == 1)
                        {
                            Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial;
                            Mi_SQL += " SET " + Ope_Pre_Convenios_Predial.Campo_Anticipo + " = 'PAGADO', ";
                            Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }

                        //Actualiza el estatus de la parcialidad del convenio
                        Mi_SQL = "UPDATE " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                        Mi_SQL += " SET " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'PAGADO'";
                        Mi_SQL += ", " + Ope_Pre_Detalles_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                        Mi_SQL += Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + " = " + No_Pago_Convenio;
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();

                        //Actualiza el convenio si ya esta completamente pagado
                        Mi_SQL = "SELECT NVL(COUNT(" + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago + "), '0')";
                        Mi_SQL += " FROM " + Ope_Pre_Detalles_Convenios_Predial.Tabla_Ope_Pre_Detalles_Convenios_Predial;
                        Mi_SQL += " WHERE " + Ope_Pre_Detalles_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                        Mi_SQL += " AND " + Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus + " = 'POR PAGAR'";
                        Cmmd.CommandText = Mi_SQL;
                        Int32 Cuenta_Pago_Convenio = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
                        if (Cuenta_Pago_Convenio == 0)
                        {
                            Mi_SQL = "UPDATE " + Ope_Pre_Convenios_Predial.Tabla_Ope_Pre_Convenios_Predial + " SET " + Ope_Pre_Convenios_Predial.Campo_Estatus + " = 'TERMINADO'";
                            Mi_SQL += ", " + Ope_Pre_Convenios_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Convenios_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " = '" + No_Convenio + "'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                }

                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos_Detalle.Rows)
                {
                    //Valida para el corriente
                    if (Registro["CONCEPTO"].ToString() == "CORRIENTE")
                    {
                        Monto_Corriente += Convert.ToDouble(Registro["MONTO"].ToString());
                    }
                    //Valida para el rezago
                    if (Registro["CONCEPTO"].ToString() == "REZAGO")
                    {
                        Monto_Rezago += Convert.ToDouble(Registro["MONTO"].ToString());
                    }
                    //Valida para el honorario
                    if (Registro["CONCEPTO"].ToString() == "HONORARIOS")
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro["MONTO"].ToString());
                    }
                    //Valida para los recargos
                    if (Registro["CONCEPTO"].ToString() == "RECARGOS")
                    {
                        Monto_Recargos += Convert.ToDouble(Registro["MONTO"].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro["CONCEPTO"].ToString() == "MORATORIOS")
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro["MONTO"].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTOS_RECARGOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro["MONTO"].ToString());
                        No_Descuento = Registro["REFERENCIA"].ToString();
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTOS_MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro["MONTO"].ToString());
                        No_Descuento = Registro["REFERENCIA"].ToString();
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTOS_HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro["MONTO"].ToString());
                        No_Descuento = Registro["REFERENCIA"].ToString();
                    }
                    //Valida para los descuentos de pronto pago
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTOS_CORRIENTES"))
                    {
                        Monto_Descuento_Pronto_Pago += Convert.ToDouble(Registro["MONTO"].ToString());
                    }
                }

                if (Monto_Recargos - Monto_Descuento_Recargos > 0)
                {
                    //Obtiene el consecutivo de la tabla
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recargos.Campo_No_Recargo + "),'0000000000')";
                    Mi_SQL += " FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;
                    Cmmd.CommandText = Mi_SQL;
                    No_Recargo = Cmmd.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Recargo))
                    {
                        No_Recargo = "0000000001";
                    }
                    else
                    {
                        No_Recargo = String.Format("{0:0000000000}", Convert.ToInt32(No_Recargo) + 1); ;
                    }

                    //Inserta el registro de los recargos
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos + "(" + Ope_Pre_Recargos.Campo_No_Recargo + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Cuenta_Predial_ID + ", " + Ope_Pre_Recargos.Campo_Anio_Inicial + ", " + Ope_Pre_Recargos.Campo_Bimestre_Inicial + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Anio + ", " + Ope_Pre_Recargos.Campo_Bimestre_Final + ", " + Ope_Pre_Recargos.Campo_Tipo + ", " + Ope_Pre_Recargos.Campo_No_Pago + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Rezago + ", " + Ope_Pre_Recargos.Campo_Monto + ", " + Ope_Pre_Recargos.Campo_Usuario_Creo + ", " + Ope_Pre_Recargos.Campo_Fecha_Creo + ")";
                    Mi_SQL += " VALUES (";
                    Mi_SQL += " '" + No_Recargo + "', '" + Cuenta_Predial_ID + "', " + Anio_Inicial + ", " + Bimestre_Inicial + ", ";
                    Mi_SQL += " " + Anio_Final + ", " + Bimestre_Final + ", 'ORDINARIOS', '" + No_Pago + "'," + Monto_Rezago + ", " + (Monto_Recargos - Monto_Descuento_Recargos) + ", ";
                    Mi_SQL += " '" + Usuario + "', SYSDATE)";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                if (Monto_Moratorios - Monto_Descuento_Moratorios > 0)
                {
                    //Obtiene el consecutivo de la tabla
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recargos.Campo_No_Recargo + "),'0000000000')";
                    Mi_SQL += " FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;
                    Cmmd.CommandText = Mi_SQL;
                    No_Recargo = Cmmd.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Recargo))
                    {
                        No_Recargo = "0000000001";
                    }
                    else
                    {
                        No_Recargo = String.Format("{0:0000000000}", Convert.ToInt32(No_Recargo) + 1); ;
                    }

                    //Inserta el registro de los recargos
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos + "(" + Ope_Pre_Recargos.Campo_No_Recargo + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Cuenta_Predial_ID + ", " + Ope_Pre_Recargos.Campo_Anio_Inicial + ", " + Ope_Pre_Recargos.Campo_Bimestre_Inicial + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Anio + ", " + Ope_Pre_Recargos.Campo_Bimestre_Final + ", " + Ope_Pre_Recargos.Campo_Tipo + ", " + Ope_Pre_Recargos.Campo_No_Pago + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Rezago + ", " + Ope_Pre_Recargos.Campo_Monto + ", " + Ope_Pre_Recargos.Campo_Usuario_Creo + ", " + Ope_Pre_Recargos.Campo_Fecha_Creo + ")";
                    Mi_SQL += " VALUES (";
                    Mi_SQL += " '" + No_Recargo + "', '" + Cuenta_Predial_ID + "', " + Anio_Inicial + ", " + Bimestre_Inicial + ", ";
                    Mi_SQL += " " + Anio_Final + ", " + Bimestre_Final + ", 'MORATORIOS', '" + No_Pago + "'," + Monto_Rezago + ", " + (Monto_Moratorios - Monto_Descuento_Moratorios) + ", ";
                    Mi_SQL += " '" + Usuario + "', SYSDATE)";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();
                }

                if (Monto_Honorarios > 0)
                {
                    //Obtiene el consecutivo de la tabla
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Pre_Recargos.Campo_No_Recargo + "),'0000000000')";
                    Mi_SQL += " FROM " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos;
                    Cmmd.CommandText = Mi_SQL;
                    No_Recargo = Cmmd.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Recargo))
                    {
                        No_Recargo = "0000000001";
                    }
                    else
                    {
                        No_Recargo = String.Format("{0:0000000000}", Convert.ToInt32(No_Recargo) + 1);
                    }

                    //Inserta el registro de los recargos
                    Mi_SQL = "INSERT INTO " + Ope_Pre_Recargos.Tabla_Ope_Pre_Recargos + "(" + Ope_Pre_Recargos.Campo_No_Recargo + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Cuenta_Predial_ID + ", " + Ope_Pre_Recargos.Campo_Anio_Inicial + ", " + Ope_Pre_Recargos.Campo_Bimestre_Inicial + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Anio + ", " + Ope_Pre_Recargos.Campo_Bimestre_Final + ", " + Ope_Pre_Recargos.Campo_Tipo + ", " + Ope_Pre_Recargos.Campo_No_Pago + ", ";
                    Mi_SQL += Ope_Pre_Recargos.Campo_Rezago + ", " + Ope_Pre_Recargos.Campo_Monto + ", " + Ope_Pre_Recargos.Campo_Usuario_Creo + ", " + Ope_Pre_Recargos.Campo_Fecha_Creo + ")";
                    Mi_SQL += " VALUES (";
                    Mi_SQL += " '" + No_Recargo + "', '" + Cuenta_Predial_ID + "', " + Anio_Inicial + ", " + Bimestre_Inicial + ", ";
                    Mi_SQL += " " + Anio_Final + ", " + Bimestre_Final + ", 'HONORARIOS', '" + No_Pago + "',0, " + Monto_Honorarios + ", ";
                    Mi_SQL += " '" + Usuario + "', SYSDATE)";
                    Cmmd.CommandText = Mi_SQL;
                    Cmmd.ExecuteNonQuery();

                    //Realizar el ciclo para cubrir el monto de honorarios
                    //de la tabla ope_pre_pae_honorarios, afectarias el campo 
                    //Monto_Pagado del mas antiguo al mas nuevo considerando el campo importe
                    //y que no este cubierto el importe – monto_pagado > 0   
                    Double Monto_Honorarios_Temporal = Monto_Honorarios;
                    DataTable Dt_Honorarios;
                    Cls_Ope_Pre_Pae_Honorarios_Negocio Neg_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
                    Neg_Honorarios.P_Campos_Dinamicos = Ope_Pre_Pae_Honorarios.Campo_Importe + " - NVL(" + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ",0) AS POR_PAGAR, NVL(" + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ",0) AS " + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ", " + Ope_Pre_Pae_Honorarios.Campo_No_Honorario;
                    Neg_Honorarios.P_Filtro = "(" + Ope_Pre_Pae_Honorarios.Campo_Importe + " - " + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + ">0) AND (SELECT " + Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id + " FROM " + Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + " WHERE " + Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa + " = " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios + "." + Ope_Pre_Pae_Honorarios.Campo_No_Detalle_Etapa + ")='" + Cuenta_Predial_ID + "'";
                    Dt_Honorarios = Neg_Honorarios.Consultar_Honorario();
                    foreach (DataRow Dr_Renglon in Dt_Honorarios.Rows)
                    {
                        if (Monto_Honorarios_Temporal > 0)
                        {
                            Mi_SQL = "UPDATE " + Ope_Pre_Pae_Honorarios.Tabla_Ope_Pre_Pae_Honorarios;
                            Mi_SQL += " SET " + Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado + "=";
                            if (Convert.ToDouble(Dr_Renglon["POR_PAGAR"].ToString()) <= Monto_Honorarios_Temporal)
                            {
                                Mi_SQL += (Convert.ToDouble(Dr_Renglon["POR_PAGAR"].ToString()) + Convert.ToDouble(Dr_Renglon[Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado].ToString()));
                                Monto_Honorarios_Temporal = Monto_Honorarios_Temporal - Convert.ToDouble(Dr_Renglon["POR_PAGAR"].ToString());
                            }
                            else
                            {
                                Mi_SQL += (Monto_Honorarios_Temporal + Convert.ToDouble(Dr_Renglon[Ope_Pre_Pae_Honorarios.Campo_Monto_Pagado].ToString()));
                                Monto_Honorarios_Temporal = 0;
                            }
                            Mi_SQL += ", " + Ope_Pre_Pae_Honorarios.Campo_Usuario_Modifico + "='" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Pae_Honorarios.Campo_Fecha_Modifico + "= SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Pae_Honorarios.Campo_No_Honorario + "='" + Dr_Renglon[Ope_Pre_Pae_Honorarios.Campo_No_Honorario].ToString() + "'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                        else
                        {
                            break;
                        }
                    }
                }


                if (!String.IsNullOrEmpty(No_Descuento))
                {
                    //Valida que no sea de convenio
                    if (No_Convenio == "")
                    {
                        //Actualiza el descuento aplicado
                        Mi_SQL = "UPDATE " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial;
                        if (Monto_Descuento_Recargos == 0 && Monto_Descuento_Moratorios == 0)
                        {
                            Mi_SQL += " SET " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'CANCELADO', ";
                        }
                        else
                        {
                            Mi_SQL += " SET " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'APLICADO', ";
                        }
                        Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_No_Pago + " = '" + No_Pago + "', ";
                        Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                        Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL += " WHERE " + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " = '" + No_Descuento + "'";
                        Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                        Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'VIGENTE'";
                        Cmmd.CommandText = Mi_SQL;
                        Cmmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //Valida que se aplique en el primer pago
                        if (Convert.ToInt32(No_Pago_Convenio_Inicial) == 1)
                        {
                            //Actualiza el descuento aplicado
                            Mi_SQL = "UPDATE " + Ope_Pre_Descuentos_Predial.Tabla_Ope_Pre_Descuentos_Predial;
                            Mi_SQL += " SET " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'APLICADO', ";
                            Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_No_Pago + " = '" + No_Pago + "', ";
                            Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Usuario_Modifico + " = '" + Usuario + "', ";
                            Mi_SQL += Ope_Pre_Descuentos_Predial.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial + " = '" + No_Descuento + "'";
                            Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'";
                            Mi_SQL += " AND " + Ope_Pre_Descuentos_Predial.Campo_Estatus + " = 'VIGENTE'";
                            Cmmd.CommandText = Mi_SQL;
                            Cmmd.ExecuteNonQuery();
                        }
                    }
                }

                //Actualiza el pago con los montos
                Mi_SQL = "UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                Mi_SQL += " SET " + Ope_Caj_Pagos.Campo_Monto_Corriente + " = " + Monto_Corriente;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Monto_Rezago + " = " + Monto_Rezago;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Honorarios + " = " + Monto_Honorarios;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " = " + Monto_Moratorios;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Monto_Recargos + " = " + Monto_Recargos;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Honorarios + " = " + Monto_Descuento_Honorarios;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Recargos + " = " + Monto_Descuento_Recargos;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " = " + Monto_Descuento_Moratorios;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Descuento_Pronto_Pago + " = " + Monto_Descuento_Pronto_Pago;
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Periodo_Corriente + " = '" + Periodo_Corriente + "'";
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Periodo_Rezago + " = '" + Periodo_Rezago + "'";
                Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + " = " + Ajuste_Tarifario;
                //Mi_SQL += ", " + Ope_Caj_Pagos.Campo_Tipo_Pago + " = 'PREDIAL'";
                if (No_Convenio != "")
                {
                    Mi_SQL += ", " + Ope_Caj_Pagos.Campo_No_Convenio + " = '" + No_Convenio + "'";
                }
                Mi_SQL += " WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + No_Pago + "'";
                Cmmd.CommandText = Mi_SQL;
                Cmmd.ExecuteNonQuery();
            }
            catch (OracleException Ex)
            {
                //Indicamos el mensaje 
                throw new Exception(Ex.ToString());
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Aplica_Traslado_Convenio
        ///DESCRIPCIÓN          : Realiza la aplicacion de los adeudos de predial
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 08/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Aplicar_Traslado_Convenio(ref OracleCommand Cmmd, DataTable Dt_Adeudos, DataTable Dt_Adeudos_Detalle, string Referencia, string No_Pago, string No_Convenio, Int32 No_Convenio_Pago, string Usuario)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Double Monto_Impuesto = 0;
            Double Monto_Impuesto_Division = 0;
            Double Monto_Constancias = 0;
            Double Monto_Multas = 0;
            Double Monto_Honorarios = 0;
            Double Monto_Recargos = 0;
            Double Monto_Moratorios = 0;
            Double Monto_Descuento_Recargos = 0;
            Double Monto_Descuento_Moratorios = 0;
            Double Monto_Descuento_Honorarios = 0;
            Double Monto_Descuento_Multas = 0;
            Double Ajuste_Tarifario = 0;
            String No_Calculo = "";
            Int32 Año_Calculo = 0;

            //Asigna los datos del calculo
            No_Calculo = Referencia.Substring(2);
            Año_Calculo = 0;
            if (No_Calculo.Length > 4)
            {
                Año_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
                No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
                No_Calculo = Convert.ToInt64(No_Calculo).ToString("0000000000");
            }

            Mi_SQL.Length = 0;
            //Valida si trae información el datatable
            if (Dt_Adeudos_Detalle != null)
            {
                //Afecta si tiene convenio
                if (No_Convenio != "" && No_Convenio_Pago >= 1)
                {
                    //Actualiza el ANTICIPO del Convenio de traslado de dominio A PAGADO
                    if (No_Convenio_Pago == 1)
                    {
                        Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio);
                        Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Anticipo + " = 'PAGADO', ");
                        Mi_SQL.Append(Ope_Pre_Convenios_Traslados_Dominio.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                        Mi_SQL.Append(Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'");
                        Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                    //Actualiza la parcialidad a PAGADO
                    foreach (DataRow Registro in Dt_Adeudos_Detalle.Rows)
                    {
                        Mi_SQL.Length = 0;
                        //Actualiza el estatus del detalle del convenio de traslado de dominio A PAGADO
                        Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                        Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'PAGADO', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago_Aplicado + " = '" + No_Pago + "', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + " = " + Registro["NO_PAGO"].ToString());
                        Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'");
                        Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'");
                        Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }
            }
            Mi_SQL.Length = 0;
            //Valida si trae información el datatable
            if (Dt_Adeudos != null)
            {
                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    //Valida para el impuesto de traslado
                    if (Registro["CONCEPTO"].ToString().Equals("IMPUESTO DE TRASLADO"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el impuesto de division y lotificacion
                    if (Registro["CONCEPTO"].ToString().Equals("IMPUESTO DE DIVISION"))
                    {
                        Monto_Impuesto_Division += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la constancia
                    if (Registro["CONCEPTO"].ToString().Contains("CONSTANCIA"))
                    {
                        Monto_Constancias += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la multa
                    if (Registro["CONCEPTO"].ToString().Contains("MULTA"))
                    {
                        Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el honorario
                    if (Registro["CONCEPTO"].ToString().Contains("HONORARIOS"))
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los recargos
                    if (Registro["CONCEPTO"].ToString().Contains("RECARGOS"))
                    {
                        Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro["CONCEPTO"].ToString().Contains("MORATORIOS"))
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO RECARGOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de multas
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO MULTA"))
                    {
                        Monto_Descuento_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    if (Registro["CONCEPTO"].ToString().StartsWith("AJUSTE TARIFARIO"))
                    {
                        Ajuste_Tarifario += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                }
            }
            else
            {
                //Realiza la consulta de los pasivos
                Mi_SQL.Append("SELECT " + Ope_Ing_Pasivo.Campo_Descripcion + ", " + Ope_Ing_Pasivo.Campo_Monto);
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "'");
                Cmmd.CommandText = Mi_SQL.ToString();
                OracleDataAdapter Da = new OracleDataAdapter(Cmmd);
                DataSet Ds = new DataSet();
                Da.Fill(Ds);
                Dt_Adeudos = Ds.Tables[0];
                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    //Valida para el impuesto de traslado
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("IMPUESTO DE TRASLADO"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el impuesto de División y Lotificación
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("IMPUESTO DE DIVISION"))
                    {
                        Monto_Impuesto_Division += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la constancia
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("CONSTANCIA"))
                    {
                        Monto_Constancias += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la multa
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("MULTA"))
                    {
                        Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el honorario
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("HONORARIOS"))
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los recargos
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("RECARGOS"))
                    {
                        Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("MORATORIOS"))
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO RECARGOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                        //Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de multas
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO MULTA"))
                    {
                        Monto_Descuento_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                        //Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("AJUSTE TARIFARIO"))
                    {
                        Ajuste_Tarifario += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                }
            }
            Mi_SQL.Length = 0;
            //Actualiza el pago con los montos
            Mi_SQL.Append("UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
            //Se inserta el impuesto de traslado de el campo monto_corriente
            Mi_SQL.Append(" SET " + Ope_Caj_Pagos.Campo_Monto_Corriente + " = " + Monto_Impuesto);
            //Se inserta el impuesto de división y lotificación de el campo monto_Impuesto_Division
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Impuesto_Division + " = " + Monto_Impuesto_Division);
            //Se inserta el importe por la constancia
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Constancia + " = " + Monto_Constancias);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Honorarios + " = " + Monto_Honorarios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " = " + Monto_Moratorios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Recargos + " = " + Monto_Recargos);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Multas + " = " + Monto_Multas);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + " = " + Ajuste_Tarifario);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Honorarios + " = " + Monto_Descuento_Honorarios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Recargos + " = " + Monto_Descuento_Recargos);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Multas + " = " + Monto_Descuento_Multas);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " = " + Monto_Descuento_Moratorios);
            if (No_Convenio != "")
            {
                Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_No_Convenio + " = '" + No_Convenio + "'");
            }
            Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + No_Pago + "'");
            Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
            Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

            Mi_SQL.Length = 0;
            if (No_Convenio != "")
            {
                //Actualiza el convenio si ya esta completamente pagado
                Mi_SQL.Append("SELECT NVL(COUNT(" + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago + "), '0')");
                Mi_SQL.Append(" FROM " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Detalles_Convenios_Traslados_Dominio);
                Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'");
                Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Estatus + " = 'POR PAGAR'");
                Cmmd.CommandText = Mi_SQL.ToString();
                Int32 Cuenta_Pago_Convenio = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
                if (Cuenta_Pago_Convenio == 0)
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el convenio
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " SET " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = 'TERMINADO'");
                    Mi_SQL.Append(", " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + No_Convenio + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                    //Actualiza el calculo
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " SET " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'PAGADO'");
                    Mi_SQL.Append(", " + Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + No_Calculo + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Año_Calculo);
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
                else
                {
                    //Actualiza el calculo
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " SET " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'PARCIAL'");
                    Mi_SQL.Append(", " + Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + No_Calculo + "'");
                    Mi_SQL.Append(" AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Año_Calculo);
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
            }
            else
            {
                //Actualiza el calculo
                Mi_SQL.Length = 0;
                Mi_SQL.Append("UPDATE " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " SET " + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'PAGADO'");
                Mi_SQL.Append(", " + Ope_Pre_Calculo_Imp_Traslado.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                Mi_SQL.Append(Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = '" + No_Calculo + "'");
                Mi_SQL.Append(" AND " + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = " + Año_Calculo);
                Cmmd.CommandText = Mi_SQL.ToString();
                Cmmd.ExecuteNonQuery();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Fraccionamiento_Convenio
        ///DESCRIPCIÓN          : Realiza la aplicacion de los convenios de fraccionamientos
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 13/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Aplicar_Fraccionamiento_Convenio(ref OracleCommand Cmmd, DataTable Dt_Adeudos, DataTable Dt_Adeudos_Detalle, string Referencia, string No_Pago, string No_Convenio, Int32 No_Convenio_Pago, string Usuario)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Double Monto_Impuesto = 0;
            Double Monto_Constancias = 0;
            Double Monto_Multas = 0;
            Double Monto_Honorarios = 0;
            Double Monto_Recargos = 0;
            Double Monto_Moratorios = 0;
            Double Monto_Descuento_Recargos = 0;
            Double Monto_Descuento_Moratorios = 0;
            Double Monto_Descuento_Honorarios = 0;
            Double Monto_Descuento_Multas = 0;
            Double Ajuste_Tarifario = 0;
            String No_Impuesto = "";
            Int32 Año_Calculo = 0;

            //Asigna los datos del calculo
            No_Impuesto = Referencia.Substring(3);
            Año_Calculo = 0;
            if (No_Impuesto.Length > 3)
            {
                Año_Calculo = 2000 + Convert.ToInt16(No_Impuesto.Substring(0, 2));
                No_Impuesto = No_Impuesto.Substring(2);
            }
            else if (No_Impuesto.Length == 3)
            {
                try
                {
                    Convert.ToInt32(No_Impuesto);
                    No_Impuesto = No_Impuesto.Substring(2);
                }
                catch { }

            }
            No_Impuesto = String.Format("{0:0000000000}", Convert.ToDouble(No_Impuesto));

            Mi_SQL.Length = 0;
            //Valida si trae información el datatable
            if (Dt_Adeudos_Detalle != null)
            {
                //Afecta si tiene convenio
                if (No_Convenio != "" && No_Convenio_Pago >= 1)
                {
                    //Actualiza el ANTICIPO del Convenio de fraccionamiento A PAGADO
                    if (No_Convenio_Pago == 1)
                    {
                        Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos);
                        Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Anticipo + " = 'PAGADO', ");
                        Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                        Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'");
                        Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                    //Actualiza la parcialidad a PAGADO
                    foreach (DataRow Registro in Dt_Adeudos_Detalle.Rows)
                    {
                        Mi_SQL.Length = 0;
                        //Actualiza el estatus del detalle del convenio de fraccionamiento A PAGADO
                        Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                        Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago_Aplicado + " = '" + No_Pago + "', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + " = " + Registro["NO_PAGO"].ToString());
                        Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'");
                        Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'");
                        Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }
            }
            Mi_SQL.Length = 0;
            //Valida si trae información el datatable
            if (Dt_Adeudos != null)
            {
                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    //Valida para el impuesto
                    if (Registro["CONCEPTO"].ToString().Contains("IMPUESTO"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la constancia
                    if (Registro["CONCEPTO"].ToString().Contains("CONSTANCIA"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la multa
                    if (Registro["CONCEPTO"].ToString().Contains("MULTAS"))
                    {
                        Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el honorario
                    if (Registro["CONCEPTO"].ToString().Contains("HONORARIOS"))
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los recargos
                    if (Registro["CONCEPTO"].ToString().Contains("RECARGOS"))
                    {
                        Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro["CONCEPTO"].ToString().Contains("RECARGOS MORATORIOS"))
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO ORDINARIOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de multas
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO MULTAS"))
                    {
                        Monto_Descuento_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    if (Registro["CONCEPTO"].ToString().StartsWith("AJUSTE TARIFARIO"))
                    {
                        Ajuste_Tarifario += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                }
            }
            else
            {
                //Realiza la consulta de los pasivos
                Mi_SQL.Append("SELECT " + Ope_Ing_Pasivo.Campo_Descripcion + ", " + Ope_Ing_Pasivo.Campo_Monto);
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "'");
                Cmmd.CommandText = Mi_SQL.ToString();
                OracleDataAdapter Da = new OracleDataAdapter(Cmmd);
                DataSet Ds = new DataSet();
                Da.Fill(Ds);
                Dt_Adeudos = Ds.Tables[0];
                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    //Valida para el impuesto
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("IMPUESTO"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la constancia
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("CONSTANCIA"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la multa
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("MULTAS"))
                    {
                        Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el honorario
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("HONORARIOS"))
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los recargos
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("RECARGOS ORDINARIOS"))
                    {
                        Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("RECARGOS MORATORIOS"))
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO ORDINARIOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                        //Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de multas
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO MULTAS"))
                    {
                        Monto_Descuento_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                        //Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("AJUSTE TARIFARIO"))
                    {
                        Ajuste_Tarifario += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                }
            }
            Mi_SQL.Length = 0;
            //Actualiza el pago con los montos
            Mi_SQL.Append("UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
            Mi_SQL.Append(" SET " + Ope_Caj_Pagos.Campo_Monto_Corriente + " = " + Monto_Impuesto);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Honorarios + " = " + Monto_Honorarios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " = " + Monto_Moratorios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Recargos + " = " + Monto_Recargos);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Multas + " = " + Monto_Multas);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + " = " + Ajuste_Tarifario);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Multas + " = " + Monto_Descuento_Multas);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Honorarios + " = " + Monto_Descuento_Honorarios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Recargos + " = " + Monto_Descuento_Recargos);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " = " + Monto_Descuento_Moratorios);
            if (No_Convenio != "")
            {
                Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_No_Convenio + " = '" + No_Convenio + "'");
            }
            Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + No_Pago + "'");
            Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
            Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

            Mi_SQL.Length = 0;
            if (No_Convenio != "")
            {
                //Actualiza el convenio si ya esta completamente pagado
                Mi_SQL.Append("SELECT NVL(COUNT(" + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago + "), '0')");
                Mi_SQL.Append(" FROM " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Tabla_Ope_Pre_Detalles_Convenios_Fraccionamientos);
                Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'");
                Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Estatus + " = 'POR PAGAR'");
                Cmmd.CommandText = Mi_SQL.ToString();
                Int32 Cuenta_Pago_Convenio = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
                if (Cuenta_Pago_Convenio == 0)
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el convenio
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Fraccionamientos.Tabla_Ope_Pre_Convenios_Fraccionamientos + " SET " + Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus + " = 'TERMINADO'");
                    Mi_SQL.Append(", " + Ope_Pre_Convenios_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio + " = '" + No_Convenio + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                    //Actualiza el calculo
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " SET " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PAGADO'");
                    Mi_SQL.Append(", " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + No_Impuesto + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
                else
                {
                    //Actualiza el calculo
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos + " SET " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PARCIAL'");
                    Mi_SQL.Append(", " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + No_Impuesto + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
            }
            else
            {
                //Actualiza el estatus del impuesto de fraccionamiento a PAGADO
                Mi_SQL.Length = 0;
                Mi_SQL.Append("UPDATE " + Ope_Pre_Impuestos_Fraccionamientos.Tabla_Ope_Pre_Impuestos_Fraccionamientos);
                Mi_SQL.Append(" SET " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Estatus + " = 'PAGADO', ");
                Mi_SQL.Append(Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                Mi_SQL.Append(Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " = '" + No_Impuesto + "'");
                Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Aplicar_Derechos_Supervision_Convenio
        ///DESCRIPCIÓN          : Realiza la aplicacion de los convenios de derechos de supervision
        ///PARAMETROS:          : Cmmd, pasa el command para ejecutar    
        ///CREO                 : Ismael Prieto Sánchez
        ///FECHA_CREO           : 14/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static void Aplicar_Derechos_Supervision_Convenio(ref OracleCommand Cmmd, DataTable Dt_Adeudos, DataTable Dt_Adeudos_Detalle, string Referencia, string No_Pago, string No_Convenio, Int32 No_Convenio_Pago, string Usuario)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Double Monto_Impuesto = 0;
            Double Monto_Constancias = 0;
            Double Monto_Multas = 0;
            Double Monto_Honorarios = 0;
            Double Monto_Recargos = 0;
            Double Monto_Moratorios = 0;
            Double Monto_Descuento_Recargos = 0;
            Double Monto_Descuento_Moratorios = 0;
            Double Monto_Descuento_Honorarios = 0;
            Double Monto_Descuento_Multas = 0;
            Double Ajuste_Tarifario = 0;
            String No_Impuesto = "";
            Int32 Año_Calculo = 0;

            //Asigna los datos del calculo
            No_Impuesto = Referencia.Substring(3);
            Año_Calculo = 0;
            if (No_Impuesto.Length > 3)
            {
                Año_Calculo = 2000 + Convert.ToInt16(No_Impuesto.Substring(0, 2));
                No_Impuesto = No_Impuesto.Substring(2);
            }
            else if (No_Impuesto.Length == 3)
            {
                try
                {
                    Convert.ToInt32(No_Impuesto);
                    No_Impuesto = No_Impuesto.Substring(2);
                }
                catch { }

            }
            No_Impuesto = String.Format("{0:0000000000}", Convert.ToDouble(No_Impuesto));

            Mi_SQL.Length = 0;
            //Valida si trae información el datatable
            if (Dt_Adeudos_Detalle != null)
            {
                //Afecta si tiene convenio
                if (No_Convenio != "" && No_Convenio_Pago >= 1)
                {
                    //Actualiza el ANTICIPO del Convenio de derechos de supervision A PAGADO
                    if (No_Convenio_Pago == 1)
                    {
                        Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision);
                        Mi_SQL.Append(" SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Anticipo + " = 'PAGADO', ");
                        Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                        Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'");
                        Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                    //Actualiza la parcialidad a PAGADO
                    foreach (DataRow Registro in Dt_Adeudos_Detalle.Rows)
                    {
                        Mi_SQL.Length = 0;
                        //Actualiza el estatus del detalle del convenio de derechos de supervision A PAGADO
                        Mi_SQL.Append("UPDATE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                        Mi_SQL.Append(" SET " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago_Aplicado + " = '" + No_Pago + "', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                        Mi_SQL.Append(Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + " = " + Registro["NO_PAGO"].ToString());
                        Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'");
                        Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'");
                        Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                        Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                    }
                }
            }
            Mi_SQL.Length = 0;
            //Valida si trae información el datatable
            if (Dt_Adeudos != null)
            {
                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    //Valida para el impuesto
                    if (Registro["CONCEPTO"].ToString().Contains("DERECHOS DE SUPERVISION"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la constancia
                    if (Registro["CONCEPTO"].ToString().Contains("CONSTANCIA"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la multa
                    if (Registro["CONCEPTO"].ToString().Contains("MULTAS"))
                    {
                        Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el honorario
                    if (Registro["CONCEPTO"].ToString().Contains("HONORARIOS"))
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los recargos
                    if (Registro["CONCEPTO"].ToString().Contains("RECARGOS"))
                    {
                        Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro["CONCEPTO"].ToString().Contains("RECARGOS MORATORIOS"))
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO ORDINARIOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de multas
                    if (Registro["CONCEPTO"].ToString().StartsWith("DESCUENTO MULTAS"))
                    {
                        Monto_Descuento_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    if (Registro["CONCEPTO"].ToString().StartsWith("AJUSTE TARIFARIO"))
                    {
                        Ajuste_Tarifario += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                }
            }
            else
            {
                //Realiza la consulta de los pasivos
                Mi_SQL.Append("SELECT " + Ope_Ing_Pasivo.Campo_Descripcion + ", " + Ope_Ing_Pasivo.Campo_Monto);
                Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "'");
                Cmmd.CommandText = Mi_SQL.ToString();
                OracleDataAdapter Da = new OracleDataAdapter(Cmmd);
                DataSet Ds = new DataSet();
                Da.Fill(Ds);
                Dt_Adeudos = Ds.Tables[0];
                //Recorre los detalles de los montos
                foreach (DataRow Registro in Dt_Adeudos.Rows)
                {
                    //Valida para el impuesto
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("IMPUESTO"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la constancia
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("CONSTANCIA"))
                    {
                        Monto_Impuesto += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para la multa
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Equals("MULTAS"))
                    {
                        Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para el honorario
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("HONORARIOS"))
                    {
                        Monto_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los recargos
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("RECARGOS ORDINARIOS"))
                    {
                        Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los moratorios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().Contains("RECARGOS MORATORIOS"))
                    {
                        Monto_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                    //Valida para los descuentos de recargos
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO ORDINARIOS"))
                    {
                        Monto_Descuento_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                        //Monto_Recargos += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de moratorios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO MORATORIOS"))
                    {
                        Monto_Descuento_Moratorios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de honorarios
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO HONORARIOS"))
                    {
                        Monto_Descuento_Honorarios += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    //Valida para los descuentos de multas
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("DESCUENTO MULTAS"))
                    {
                        Monto_Descuento_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                        //Monto_Multas += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString()) * (-1);
                    }
                    if (Registro[Ope_Ing_Pasivo.Campo_Descripcion].ToString().StartsWith("AJUSTE TARIFARIO"))
                    {
                        Ajuste_Tarifario += Convert.ToDouble(Registro[Ope_Ing_Pasivo.Campo_Monto].ToString());
                    }
                }
            }
            Mi_SQL.Length = 0;
            //Actualiza el pago con los montos
            Mi_SQL.Append("UPDATE " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
            Mi_SQL.Append(" SET " + Ope_Caj_Pagos.Campo_Monto_Corriente + " = " + Monto_Impuesto);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Honorarios + " = " + Monto_Honorarios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Recargos_Moratorios + " = " + Monto_Moratorios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Monto_Recargos + " = " + Monto_Recargos);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Multas + " = " + Monto_Multas);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Multas + " = " + Monto_Descuento_Multas);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Honorarios + " = " + Monto_Descuento_Honorarios);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Recargos + " = " + Monto_Descuento_Recargos);
            Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_Descuento_Moratorios + " = " + Monto_Descuento_Moratorios);
            if (No_Convenio != "")
            {
                Mi_SQL.Append(", " + Ope_Caj_Pagos.Campo_No_Convenio + " = '" + No_Convenio + "'");
            }
            Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_No_Pago + " = '" + No_Pago + "'");
            Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
            Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

            Mi_SQL.Length = 0;
            if (No_Convenio != "")
            {
                //Actualiza el convenio si ya esta completamente pagado
                Mi_SQL.Append("SELECT NVL(COUNT(" + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Pago + "), '0')");
                Mi_SQL.Append(" FROM " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Detalles_Convenios_Derechos_Supervision);
                Mi_SQL.Append(" WHERE " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'");
                Mi_SQL.Append(" AND " + Ope_Pre_Detalles_Convenios_Derechos_Supervision.Campo_Estatus + " = 'POR PAGAR'");
                Cmmd.CommandText = Mi_SQL.ToString();
                Int32 Cuenta_Pago_Convenio = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
                if (Cuenta_Pago_Convenio == 0)
                {
                    Mi_SQL.Length = 0;
                    //Actualiza el convenio
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Convenios_Derechos_Supervision.Tabla_Ope_Pre_Convenios_Derechos_Supervision + " SET " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Estatus + " = 'TERMINADO'");
                    Mi_SQL.Append(", " + Ope_Pre_Convenios_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Convenios_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Convenios_Derechos_Supervision.Campo_No_Convenio + " = '" + No_Convenio + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                    //Actualiza el calculo
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + " SET " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PAGADO'");
                    Mi_SQL.Append(", " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " = '" + No_Impuesto + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
                else
                {
                    //Actualiza el calculo
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("UPDATE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision + " SET " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PARCIAL'");
                    Mi_SQL.Append(", " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                    Mi_SQL.Append(Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                    Mi_SQL.Append(" WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " = '" + No_Impuesto + "'");
                    Cmmd.CommandText = Mi_SQL.ToString();
                    Cmmd.ExecuteNonQuery();
                }
            }
            else
            {
                //Actualiza el estatus del impuesto de derechos de supervision a PAGADO
                Mi_SQL.Length = 0;
                Mi_SQL.Append("UPDATE " + Ope_Pre_Impuestos_Derechos_Supervision.Tabla_Ope_Pre_Impuestos_Derechos_Supervision);
                Mi_SQL.Append(" SET " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_Estatus + " = 'PAGADO', ");
                Mi_SQL.Append(Ope_Pre_Impuestos_Derechos_Supervision.Campo_Usuario_Modifico + " = '" + Usuario + "', ");
                Mi_SQL.Append(Ope_Pre_Impuestos_Derechos_Supervision.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Pre_Impuestos_Derechos_Supervision.Campo_No_Impuesto_Derecho_Supervision + " = '" + No_Impuesto + "'");
                Cmmd.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Cmmd.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Turno
        /// DESCRIPCION : Consulta si hay algun turno abierto en la caja asignada para el
        ///               empleado
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 11-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Turno(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            String Mi_SQL; //Variable para ejecutar el query
            try
            {
                Mi_SQL = "SELECT " + Ope_Caj_Turnos.Campo_Contador_Recibo + ", " + Ope_Caj_Turnos.Campo_Aplicacion_Pago;
                Mi_SQL += " FROM " + Ope_Caj_Turnos.Tabla_Ope_Caj_Turnos;
                Mi_SQL += " WHERE " + Ope_Caj_Turnos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'";
                Mi_SQL += " AND " + Ope_Caj_Turnos.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "'";
                Mi_SQL += " AND " + Ope_Caj_Turnos.Campo_Estatus + " = 'ABIERTO'";
                //Mi_SQL += " AND " + Ope_Caj_Turnos.Campo_Fecha_Turno;
                //Mi_SQL += " BETWEEN TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')";
                //Mi_SQL += " AND TO_DATE ('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Turno)) + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Pago_Internet
        /// DESCRIPCION : 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Miguel Angel Bedolla Moreno
        /// FECHA_CREO  : 16/Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Pago_Internet(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Object No_Pago;      //Consecutivo del registro de la tabla en la base de datos
            Object No_Pasivo; //Consecutivo del registro de la tabla en la base de datos
            Int32 No_Recibo;    //Obtiene el número de recibo que le pertence el pago del ingreso
            Object No_Operacion; //Obtiene el número de operacion que fue realizada durante en día de la caja
            Object Consecutivo;  //Obtiene el número de registro con el cual se va a dar de alta el detalle del pago en la base de datos
            String Clave_Ingreso_Parametro_ID; //Almacena el id de la clave de ingreso del ajuste tarifario
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            Cls_Ope_Caj_Pagos_Negocio Caja = new Cls_Ope_Caj_Pagos_Negocio();
            String Cuenta_Predial_ID = "";
            Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Dt_Clave = new DataTable();

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL.Length = 0;
                //Elimina Pasivos Sin pagar
                Mi_SQL.Append("DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Cuenta_Predial + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                Comando_SQL.ExecuteNonQuery();

                //Alta de pasivos
                foreach (DataRow Dr_Actual in Datos.P_Dt_Pasivos.Rows)
                {
                    Mi_SQL.Length = 0;
                    //Consulta el último no de pasivo que fue registrado en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Ing_Pasivo.Campo_No_Pasivo + "),'0')");
                    Mi_SQL.Append(" FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    No_Pasivo = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Pasivo))
                    {
                        No_Pasivo = 1;
                    }
                    else
                    {
                        No_Pasivo = Convert.ToInt32(No_Pasivo) + 1;
                    }

                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                    Mi_SQL.Append("(" + Ope_Ing_Pasivo.Campo_No_Pasivo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Referencia);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Descripcion);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Monto);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Recargos);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Estatus);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_No_Recibo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Dependencia_ID);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Fecha_Creo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Usuario_Creo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Contribuyente);
                    Mi_SQL.Append(") VALUES(" + No_Pasivo);
                    Mi_SQL.Append(",'" + Dr_Actual["Cuenta_Predial"].ToString() + "'");
                    Mi_SQL.Append(",'" + Dr_Actual["Clave_Ingreso"].ToString() + "'");
                    Mi_SQL.Append(",'" + Dr_Actual["Descripcion"].ToString() + "'");
                    Mi_SQL.Append(",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dr_Actual["Fecha_Tramite"].ToString()).ToString("dd/MM/yyyy")) + "'");
                    Mi_SQL.Append(",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dr_Actual["Fecha_Vencimiento"].ToString()).ToString("dd/MM/yyyy")) + "'");
                    Mi_SQL.Append(",'" + Dr_Actual["Monto"].ToString() + "',0");
                    Mi_SQL.Append(",'" + Dr_Actual["Estatus"].ToString() + "'");
                    Mi_SQL.Append(",NULL");
                    Mi_SQL.Append(",'" + Dr_Actual["Dependencia"].ToString() + "',SYSDATE,'','" + Dr_Actual["Cuenta_Predial_Id"].ToString() + "','')");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Comando_SQL.ExecuteNonQuery();
                }

                Mi_SQL.Length = 0;
                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0000000000')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Pago = Comando_SQL.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(No_Pago))
                {
                    No_Pago = "0000000001";
                }
                else
                {
                    No_Pago = String.Format("{0:0000000000}", Convert.ToInt32(No_Pago) + 1);
                }
                Datos.P_No_Pago = No_Pago.ToString();

                Mi_SQL.Length = 0;
                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Operacion + "),'0')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Operacion = Comando_SQL.ExecuteOracleScalar().ToString();

                if (Convert.IsDBNull(No_Operacion))
                {
                    No_Operacion = Convert.ToInt32("1");
                }
                else
                {
                    No_Operacion = Convert.ToInt32(No_Operacion) + 1;
                }
                Mi_SQL.Length = 0;

                //Asigna la cuenta predial
                Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;

                Mi_SQL.Length = 0;
                //Inserta los datos en la tabla con los datos proporcionados por el usuario
                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append("(" + Ope_Caj_Pagos.Campo_No_Pago + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_No_Recibo + ", " + Ope_Caj_Pagos.Campo_No_Operacion + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_No_Turno + ", " + Ope_Caj_Pagos.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Monto_Corriente + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Monto_Recargos + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Tipo_Pago + ", " + Ope_Caj_Pagos.Campo_Total + ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Documento + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Empleado_ID + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")");
                Mi_SQL.Append(" VALUES ('" + No_Pago + "', NULL, " + No_Operacion + ", '");
                Mi_SQL.Append(Datos.P_Caja_ID + "', NULL, TO_DATE('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + "','DD-MM-YYYY'), 'PAGADO', ");
                Mi_SQL.Append(Datos.P_Monto_Corriente + ", " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append("NULL, ");
                Mi_SQL.Append("'PREDIAL', ");
                Mi_SQL.Append(Datos.P_Total_Pagar + ", ");
                Mi_SQL.Append("0, ");
                Mi_SQL.Append("'" + Datos.P_Referencia + "', ");
                Mi_SQL.Append("'" + Cuenta_Predial_ID + "', ");
                Mi_SQL.Append("NULL, ");
                Mi_SQL.Append("SYSDATE)");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                Mi_SQL.Length = 0;
                //Ingresa el pasivo del ajuste tarifario
                if (Datos.P_Ajuste_Tarifario != 0)
                {
                    Mi_SQL.Append("SELECT NVL(" + Ope_Pre_Parametros.Campo_Clave_Ing_Ajuste_Tarifa_ID + ", '')");
                    Mi_SQL.Append(" FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Clave_Ingreso_Parametro_ID = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Clave_Ingreso_Parametro_ID != "")
                    {
                        Rs_Claves_Ingreso.P_Clave_Ingreso_ID = Clave_Ingreso_Parametro_ID;
                        Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso_Por_ID();
                        // si se o obtuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            //Ingresa el ajuste tarifario
                            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                            Rs_Modificar_Calculo.P_Cmd_Calculo = Comando_SQL;
                            Rs_Modificar_Calculo.P_Fecha_Tramite = Datos.P_Fecha_Pago.ToString();
                            Rs_Modificar_Calculo.P_Fecha_Vencimiento_Pasivo = Datos.P_Fecha_Pago.ToString();
                            Rs_Modificar_Calculo.P_Estatus = "POR PAGAR";
                            Rs_Modificar_Calculo.P_Nombre_Usuario = Datos.P_Nombre_Usuario;
                            Rs_Modificar_Calculo.P_Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;
                            Rs_Modificar_Calculo.P_Referencia = Datos.P_Referencia;
                            Rs_Modificar_Calculo.P_Descripcion = "AJUSTE TARIFARIO";
                            Rs_Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Rs_Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Rs_Modificar_Calculo.P_Monto_Total_Pagar = Convert.ToString(Datos.P_Ajuste_Tarifario);
                            Rs_Modificar_Calculo.Alta_Pasivo();
                        }
                    }
                    else
                    {
                        throw new Exception("Error: No se puede realizar el pago, porque no esta establecida la clave de ajuste tarifario, favor de verificarlo.");
                    }
                }

                Mi_SQL.Length = 0;
                //Actualiza el estatus del ingreso pasivo en la base de datos
                Mi_SQL.Append("UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" SET " + Ope_Ing_Pasivo.Campo_Estatus + " = 'PAGADO', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Recargos + " = " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Pago + " = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Pago) + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = NULL, ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Crea la proteccion de pago
                string Proteccion_Pago = "PAGADO/" + Convert.ToInt32(Datos.P_No_Caja) + "/" + Convert.ToInt32(No_Operacion) + "/" + String.Format("{0:yyyy.MM.dd}", Datos.P_Fecha_Pago) + "/" + String.Format("{0:HH:mm:ss}", Datos.P_Fecha_Pago) + "/" + String.Format("{0:###,##0.00}", Datos.P_Total_Pagar);
                if (Datos.P_Cuenta_Predial != "")
                {
                    Proteccion_Pago += "/" + Datos.P_Cuenta_Predial;
                }

                //Aplica los pagos al adeudo
                Aplicar_Adeudos_Predial(ref Comando_SQL, Datos.P_Dt_Adeudos_Predial_Cajas, Datos.P_Dt_Adeudos_Predial_Cajas_Detalle, Datos.P_Cuenta_Predial_ID, No_Pago.ToString(), Datos.P_Ajuste_Tarifario, Datos.P_Nombre_Usuario);

                //Efectuar pago por internet!!!!! al banco Banorte
                //Variables de envío obligatorio
                string header = "Name=predio&Password=predio2005&ClientId=461&Mode=Y&TransType=Auth&Total=" + Datos.P_Banco_Total_Pagar + "&Number=" + Datos.P_Banco_No_Tarjeta + "&Expires=" + Datos.P_Banco_Expira_Tarjeta + "&Cvv2Indicator=1&Cvv2Val=" + Datos.P_Banco_Codigo_Seguridad + "&BillToFirstName=" + Datos.P_Banco_Titular_Banco + "";

                //Url para envío de los datos
                string uri = "https://eps.banorte.com/recibo";

                //Crea un request
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
                request.KeepAlive = false;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";

                //Convierte la cadena de string en bytes
                byte[] postBytes = Encoding.ASCII.GetBytes(header);

                //Configura el tipo de contenido del request
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = postBytes.Length;
                Stream requestStream = request.GetRequestStream();

                //Envia el request
                requestStream.Write(postBytes, 0, postBytes.Length);
                requestStream.Close();

                //Obtiene el codigo de respuesta
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                String[] Respuesta = response.Headers.ToString().Split(new String[] { "\r\n" }, System.StringSplitOptions.None); //En la posición 15 se encuentra el error...
                int i;
                for (i = 0; i < Respuesta.Length; i++)
                {
                    if (Respuesta[i].Contains("CcErrCode"))
                    {
                        break;
                    }
                }
                String Codigo = Respuesta[i].Split(':')[1].Trim();

                //Obtiene el codigo de autorizacion
                for (i = 0; i < Respuesta.Length; i++)
                {
                    if (Respuesta[i].Contains("AuthCode"))
                    {
                        break;
                    }
                }
                String Codigo_Autorizacion = Respuesta[i].Split(':')[1].Trim();

                //Cierra la respuesta
                response.Close();
                String Mensaje;

                //Valida el tipo de codigo
                switch (Codigo)
                {
                    case "1":
                        Mensaje = "Pago correcto .";
                        break;
                    case "50":
                        Mensaje = "Transacción declinada .";
                        throw new Exception(Mensaje);
                    case "54":
                        Mensaje = "Conexión fuera de tiempo .";
                        throw new Exception(Mensaje);
                    case "500":
                        Mensaje = "Fallo en el tiempo para respuesta .";
                        throw new Exception(Mensaje);
                    case "1002":
                        Mensaje = "Declinada - Transacción fraudulenta .";
                        throw new Exception(Mensaje);
                    case "1007":
                        Mensaje = "Monto no válido.";
                        throw new Exception(Mensaje);
                    case "1011":
                        Mensaje = "No. de tarjeta no válido .";
                        throw new Exception(Mensaje);
                    case "1050":
                        Mensaje = "Declinado - Fondos insuficientes .";
                        throw new Exception(Mensaje);
                    case "1051":
                        Mensaje = "Tarjeta del cliente vencida .";
                        throw new Exception(Mensaje);
                    case "2078":
                        Mensaje = "Tarjeta no activa .";
                        throw new Exception(Mensaje);
                    default:
                        Mensaje = "TRANSACCION RECHAZADA - Error desconocido... favor de comunicarse con su banco .";
                        throw new Exception(Mensaje);
                }
                //Asigna el codigo del banco
                Datos.P_Banco_Clave_Operacion = Codigo_Autorizacion;
                //Fin pago por internet

                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Datos.P_Dt_Formas_Pago.Rows)
                {
                    Mi_SQL.Length = 0;
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "),0)");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Consecutivo = Comando_SQL.ExecuteOracleScalar().ToString();
                    //Consecutivo = OracleHelper.ExecuteScalar(Transaccion_SQL, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = 1;
                    }
                    else
                    {
                        Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                    }
                    Mi_SQL.Length = 0;
                    if (Registro["Forma_Pago"].ToString() == "INTERNET") //Forma de Pago en Efectivo
                    {
                        //Inserción de forma de pago en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Autorizacion + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', '");
                        Mi_SQL.Append(Datos.P_Banco_Clave_Operacion + "', " + Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");

                    }
                    else
                    {
                        if (Registro["Forma_Pago"].ToString() == "AJUSTE TARIFARIO") //Ajuste tarifario
                        {
                            //Inserción de forma de pago en la base de datos
                            Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                            Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                            Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                        }
                    }
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]" + " SQL: " + Mi_SQL);
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Pago_Instituciones_Externas
        /// DESCRIPCIÓN: dar de alta información de pagos de predial en la base de datos
        /// PARÁMETROS:
        /// 		1. Datos: instancia de la clase de negocio con datos a insertar
        /// 		2. Lista_Cuentas: listado de cuentas a las que se les va a aplicar pago
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 30-mar-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Alta_Pago_Instituciones_Externas(Cls_Ope_Caj_Pagos_Negocio Datos, List<string> Lista_Cuentas)
        {
            string Consulta_SQL = "";
            Object No_Pago;      //Consecutivo del registro de la tabla en la base de datos
            Int32 No_Recibo;    //Obtiene el número de recibo que le pertence el pago del ingreso
            Object No_Operacion; //Obtiene el número de operacion que fue realizada durante en día de la caja
            Object Consecutivo;  //Obtiene el número de registro con el cual se va a dar de alta el detalle del pago en la base de datos
            OracleConnection Conexion_Base = new OracleConnection();
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL = null;
            String Cuenta_Predial_ID = "";
            Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Dt_Clave = new DataTable();
            DataTable Dt_Adeudos_Totales_Cuenta;
            DataTable Dt_Adeudos_Detallados_Cuenta;
            DataTable Dt_Pasivos_Cuenta;
            decimal Monto_Total_Pagado;

            // si llego un Comando como parametro, utilizarlo
            if (Datos.P_Comando_Oracle != null)    // si la conexion llego como parametro, establecer como comando para utilizar
            {
                Comando_SQL = Datos.P_Comando_Oracle;
            }
            else    // si no, crear nueva conexion y transaccion
            {
                Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos  
                if (Conexion_Base.State != ConnectionState.Open)
                {
                    Conexion_Base.Open(); //Abre la conexión a la base de datos            
                }
                Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
                Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
                Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos
            }

            try
            {

                foreach (string Cuenta_ID in Lista_Cuentas)
                {
                    Dt_Adeudos_Totales_Cuenta = (from Fila_Adeudos in Datos.P_Dt_Adeudos_Predial_Cajas.AsEnumerable()
                                                 where Fila_Adeudos.Field<string>("NO_CUENTA") == Cuenta_ID
                                                 select Fila_Adeudos).AsDataView().ToTable();
                    Dt_Adeudos_Detallados_Cuenta = (from Fila_Adeudos in Datos.P_Dt_Adeudos_Predial_Cajas_Detalle.AsEnumerable()
                                                    where Fila_Adeudos.Field<string>("NO_CUENTA") == Cuenta_ID
                                                    select Fila_Adeudos).AsDataView().ToTable();
                    Dt_Pasivos_Cuenta = (from Fila_Adeudos in Datos.P_Dt_Pasivos.AsEnumerable()
                                         where Fila_Adeudos.Field<string>("Cuenta_Predial_Id") == Cuenta_ID
                                         select Fila_Adeudos).AsDataView().ToTable();
                    // si alguna de las tablas no trae datos, generar excepcion
                    if (Dt_Adeudos_Totales_Cuenta.Rows.Count <= 0 || Dt_Adeudos_Detallados_Cuenta.Rows.Count <= 0 || Dt_Pasivos_Cuenta.Rows.Count <= 0)
                    {
                        throw new Exception("Alta_Pago_Instituciones_Externas: Tabla sin datos");
                    }

                    //Asigna la cuenta predial
                    Cuenta_Predial_ID = Dt_Pasivos_Cuenta.Rows[0]["Cuenta_Predial_Id"].ToString();

                    //Elimina Pasivos Sin pagar
                    Consulta_SQL = "DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                        + " WHERE " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "'"
                        + " AND " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Dt_Pasivos_Cuenta.Rows[0]["Cuenta_Predial"].ToString() + "'"
                        + " AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'";
                    Comando_SQL.CommandText = Consulta_SQL;
                    Comando_SQL.ExecuteNonQuery();

                    //Consulta el último no de pago que fue registrado en la base de datos
                    Consulta_SQL = "SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0000000000')"
                            + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos;
                    Comando_SQL.CommandText = Consulta_SQL;
                    No_Pago = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Convert.IsDBNull(No_Pago))
                    {
                        No_Pago = "0000000001";
                    }
                    else
                    {
                        No_Pago = String.Format("{0:0000000000}", Convert.ToInt32(No_Pago) + 1);
                    }
                    Datos.P_No_Pago = No_Pago.ToString();

                    // Consulta el último No_Operacion que fue registrado en la base de datos para la fecha de pago
                    Consulta_SQL = "SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Operacion + "),'0')"
                        + " FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                        + " WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'"
                        + " AND " + Ope_Caj_Pagos.Campo_Fecha
                        + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", (DateTime)Dt_Pasivos_Cuenta.Rows[0]["Fecha_Tramite"]) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')"
                        + " AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", (DateTime)Dt_Pasivos_Cuenta.Rows[0]["Fecha_Tramite"]) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')";
                    Comando_SQL.CommandText = Consulta_SQL;
                    No_Operacion = Comando_SQL.ExecuteOracleScalar().ToString();

                    if (Convert.IsDBNull(No_Operacion))
                    {
                        No_Operacion = Convert.ToInt32("1");
                    }
                    else
                    {
                        No_Operacion = Convert.ToInt32(No_Operacion) + 1;
                    }

                    // asignar el total pagado en la tabla Detalles pago
                    Monto_Total_Pagado = 0;
                    for (int Detalle_Pago = Dt_Adeudos_Detallados_Cuenta.Rows.Count - 1; Detalle_Pago > 0; Detalle_Pago--)
                    {
                        if (Dt_Adeudos_Detallados_Cuenta.Rows[Detalle_Pago]["CONCEPTO"].ToString() == "MONTO_TOTAL_PAGADO")
                        {
                            decimal.TryParse(Dt_Adeudos_Detallados_Cuenta.Rows[Detalle_Pago]["MONTO"].ToString(), out Monto_Total_Pagado);
                        }
                    }

                    // Inserta los datos del PAGO
                    Consulta_SQL = "INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos
                        + "(" + Ope_Caj_Pagos.Campo_No_Pago + ", "
                        + Ope_Caj_Pagos.Campo_No_Operacion + ", "
                        + Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_Fecha + ", "
                        + Ope_Caj_Pagos.Campo_Estatus + ", "
                        + Ope_Caj_Pagos.Campo_Usuario_Creo + ", "
                        + Ope_Caj_Pagos.Campo_Tipo_Pago + ", "
                        + Ope_Caj_Pagos.Campo_Documento + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + ", "
                        + Ope_Caj_Pagos.Campo_Total + ", "
                        + Ope_Caj_Pagos.Campo_Empleado_ID + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")"
                        + " VALUES ('" + No_Pago + "', " + No_Operacion + ", '"
                        + Datos.P_Caja_ID + "', TO_DATE('" + String.Format("{0:dd-MM-yyy}", (DateTime)Dt_Pasivos_Cuenta.Rows[0]["Fecha_Tramite"]) + "','DD-MM-YYYY'), 'PAGADO', '"
                        + Datos.P_Nombre_Usuario + "', "
                        + "'PREDIAL', "
                        + "'" + Dt_Pasivos_Cuenta.Rows[0]["Cuenta_Predial"].ToString() + "', "
                        + "'" + Cuenta_Predial_ID + "', "
                        + Monto_Total_Pagado + ", "
                        + "NULL, "
                        + "SYSDATE)";

                    Comando_SQL.CommandText = Consulta_SQL; // Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    // Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                    // Consulta el último CONSECUTIVO que fue registrado en Ope_Caj_Pagos_Detalles
                    Consulta_SQL = "SELECT NVL(MAX(" + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "),0)"
                        + " FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles;
                    Comando_SQL.CommandText = Consulta_SQL;
                    Consecutivo = Convert.ToInt64(Comando_SQL.ExecuteOracleScalar().ToString()) + 1;

                    // Inserción de FORMA DE PAGO en la base de datos
                    Consulta_SQL = "INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "("
                        + Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", "
                        + Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")"
                        + " VALUES ('" + No_Pago + "', 'INSTITUCION EXTERNA', "
                        + Monto_Total_Pagado + ", " + Consecutivo + ")";

                    Comando_SQL.CommandText = Consulta_SQL; // Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    // Ejecuta la inserción en memoria antes de pasarla a la base de datos

                    //Consulta el último No_Pasivo que fue registrado en la base de datos
                    Consulta_SQL = "SELECT NVL(MAX(" + Ope_Ing_Pasivo.Campo_No_Pasivo + "), 0)"
                            + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo;
                    Comando_SQL.CommandText = Consulta_SQL;
                    Int64 No_Pasivo = Convert.ToInt64(Comando_SQL.ExecuteOracleScalar().ToString());

                    //Alta de pasivos
                    foreach (DataRow Dr_Pasivo in Dt_Pasivos_Cuenta.Rows)
                    {
                        Consulta_SQL = "INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo
                            + "(" + Ope_Ing_Pasivo.Campo_No_Pasivo
                            + ", " + Ope_Ing_Pasivo.Campo_Referencia
                            + ", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID
                            + ", " + Ope_Ing_Pasivo.Campo_Descripcion
                            + ", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso
                            + ", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento
                            + ", " + Ope_Ing_Pasivo.Campo_Monto
                            + ", " + Ope_Ing_Pasivo.Campo_Recargos
                            + ", " + Ope_Ing_Pasivo.Campo_Estatus
                            + ", " + Ope_Ing_Pasivo.Campo_No_Pago
                            + ", " + Ope_Ing_Pasivo.Campo_Dependencia_ID
                            + ", " + Ope_Ing_Pasivo.Campo_Fecha_Creo
                            + ", " + Ope_Ing_Pasivo.Campo_Usuario_Creo
                            + ", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID
                            + ", " + Ope_Ing_Pasivo.Campo_Contribuyente
                            + ") VALUES(" + ++No_Pasivo
                            + ",'" + Dr_Pasivo["Cuenta_Predial"].ToString() + "'"
                            + ",'" + Dr_Pasivo["Clave_Ingreso"].ToString() + "'"
                            + ",'" + Dr_Pasivo["Descripcion"].ToString() + "'"
                            + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dr_Pasivo["Fecha_Tramite"].ToString()).ToString("dd/MM/yyyy")) + "'"
                            + ",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dr_Pasivo["Fecha_Vencimiento"].ToString()).ToString("dd/MM/yyyy")) + "'"
                            + ",'" + Dr_Pasivo["Monto"].ToString() + "',0"
                            + ",'" + Dr_Pasivo["Estatus"].ToString() + "'"
                            + ",''"//No_Recibo... no tiene...
                            + ",'" + Dr_Pasivo["Dependencia"].ToString() + "',SYSDATE,'','" + Cuenta_Predial_ID + "','')";
                        Comando_SQL.CommandText = Consulta_SQL;
                        Comando_SQL.ExecuteNonQuery();
                    }

                    // llamar método que aplica los adeudos de predial
                    Aplicar_Adeudos_Predial(ref Comando_SQL, Dt_Adeudos_Totales_Cuenta, Dt_Adeudos_Detallados_Cuenta, Cuenta_Predial_ID, No_Pago.ToString(), 0, Datos.P_Nombre_Usuario);

                }
                if (Datos.P_Comando_Oracle == null && Transaccion_SQL != null)    // si la conexion llego como parametro, establecer como comando para utilizar
                {
                    Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                }
            }
            catch (OracleException Ex)
            {
                if (Datos.P_Comando_Oracle == null && Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Consulta_SQL);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Datos.P_Comando_Oracle == null && Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]" + " SQL: " + Consulta_SQL);
            }
            catch (Exception Ex)
            {
                if (Datos.P_Comando_Oracle == null && Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Consulta_SQL);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : l
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Pago_PAE
        /// DESCRIPCION : 
        /// PARAMETROS  : Datos: Contiene los datos que serán insertados en la base de datos
        /// CREO        : Miguel Angel Bedolla Moreno
        /// FECHA_CREO  : 16/Diciembre/2011
        /// MODIFICO          :Armnado Zavala Moreno
        /// FECHA_MODIFICO    :08/Mayo/2012
        /// CAUSA_MODIFICACION:Para que acepte la caja PAE y el pago sea en efectivo
        ///*******************************************************************************
        public static void Alta_Pago_PAE(Cls_Ope_Caj_Pagos_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            Object No_Pago;      //Consecutivo del registro de la tabla en la base de datos
            Int32 No_Recibo;    //Obtiene el número de recibo que le pertence el pago del ingreso
            Object No_Operacion; //Obtiene el número de operacion que fue realizada durante en día de la caja
            Object Consecutivo;  //Obtiene el número de registro con el cual se va a dar de alta el detalle del pago en la base de datos
            String Clave_Ingreso_Parametro_ID; //Almacena el id de la clave de ingreso del ajuste tarifario
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;
            Cls_Ope_Caj_Pagos_Negocio Caja = new Cls_Ope_Caj_Pagos_Negocio();
            String Cuenta_Predial_ID = "";
            Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Dt_Clave = new DataTable();

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction();  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                Mi_SQL.Length = 0;
                //Elimina Pasivos Sin pagar
                Mi_SQL.Append("DELETE FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID + " = '" + Datos.P_Cuenta_Predial_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Cuenta_Predial + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                Comando_SQL.ExecuteNonQuery();

                //Alta de pasivos
                Int32 No_Pasivo = 0;
                No_Pasivo = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo, Ope_Ing_Pasivo.Campo_No_Pasivo, 10));
                foreach (DataRow Dr_Actual in Datos.P_Dt_Pasivos.Rows)
                {
                    Mi_SQL.Length = 0;
                    Mi_SQL.Append("INSERT INTO " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                    Mi_SQL.Append("(" + Ope_Ing_Pasivo.Campo_No_Pasivo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Referencia);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Clave_Ingreso_ID);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Descripcion);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Fecha_Ingreso);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Fecha_Vencimiento);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Monto);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Recargos);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Estatus);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_No_Recibo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Dependencia_ID);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Fecha_Creo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Usuario_Creo);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Cuenta_Predial_ID);
                    Mi_SQL.Append(", " + Ope_Ing_Pasivo.Campo_Contribuyente);
                    Mi_SQL.Append(") VALUES(" + No_Pasivo);
                    Mi_SQL.Append(",'" + Dr_Actual["Cuenta_Predial"].ToString() + "'");
                    Mi_SQL.Append(",'" + Dr_Actual["Clave_Ingreso"].ToString() + "'");
                    Mi_SQL.Append(",'" + Dr_Actual["Descripcion"].ToString() + "'");
                    Mi_SQL.Append(",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dr_Actual["Fecha_Tramite"].ToString()).ToString("dd/MM/yyyy")) + "'");
                    Mi_SQL.Append(",'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Dr_Actual["Fecha_Vencimiento"].ToString()).ToString("dd/MM/yyyy")) + "'");
                    Mi_SQL.Append(",'" + Dr_Actual["Monto"].ToString() + "',0");
                    Mi_SQL.Append(",'" + Dr_Actual["Estatus"].ToString() + "'");
                    Mi_SQL.Append(",NULL");
                    Mi_SQL.Append(",'" + Dr_Actual["Dependencia"].ToString() + "',SYSDATE,'','" + Dr_Actual["Cuenta_Predial_Id"].ToString() + "','')");
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Comando_SQL.ExecuteNonQuery();
                    No_Pasivo++;
                }

                Mi_SQL.Length = 0;
                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Pago + "),'0000000000')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Pago = Comando_SQL.ExecuteOracleScalar().ToString();
                if (Convert.IsDBNull(No_Pago))
                {
                    No_Pago = "0000000001";
                }
                else
                {
                    No_Pago = String.Format("{0:0000000000}", Convert.ToInt32(No_Pago) + 1);
                }
                Datos.P_No_Pago = No_Pago.ToString();

                Mi_SQL.Length = 0;
                //Consulta el último no de pago que fue registrado en la base de datos
                Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos.Campo_No_Operacion + "),'0')");
                Mi_SQL.Append(" FROM " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append(" WHERE " + Ope_Caj_Pagos.Campo_Caja_ID + " = '" + Datos.P_Caja_ID + "'");
                Mi_SQL.Append(" AND " + Ope_Caj_Pagos.Campo_Fecha + " BETWEEN TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS')");
                Mi_SQL.Append(" AND TO_DATE ('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + " 23:59:00', 'DD-MM-YYYY HH24:MI:SS')");
                Comando_SQL.CommandText = Mi_SQL.ToString();
                No_Operacion = Comando_SQL.ExecuteOracleScalar().ToString();

                if (Convert.IsDBNull(No_Operacion))
                {
                    No_Operacion = Convert.ToInt32("1");
                }
                else
                {
                    No_Operacion = Convert.ToInt32(No_Operacion) + 1;
                }
                Mi_SQL.Length = 0;

                //Asigna la cuenta predial
                Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;

                Mi_SQL.Length = 0;
                //Inserta los datos en la tabla con los datos proporcionados por el usuario
                Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos.Tabla_Ope_Caj_Pagos);
                Mi_SQL.Append("(" + Ope_Caj_Pagos.Campo_No_Pago + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_No_Recibo + ", " + Ope_Caj_Pagos.Campo_No_Operacion + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Caja_ID + ", " + Ope_Caj_Pagos.Campo_No_Turno + ", " + Ope_Caj_Pagos.Campo_Fecha + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Estatus + ", " + Ope_Caj_Pagos.Campo_Monto_Corriente + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Monto_Recargos + ", " + Ope_Caj_Pagos.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Tipo_Pago + ", " + Ope_Caj_Pagos.Campo_Total + ", " + Ope_Caj_Pagos.Campo_Ajuste_Tarifario + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Documento + ", " + Ope_Caj_Pagos.Campo_Cuenta_Predial_ID + ", ");
                Mi_SQL.Append(Ope_Caj_Pagos.Campo_Empleado_ID + ", " + Ope_Caj_Pagos.Campo_Fecha_Creo + ")");
                Mi_SQL.Append(" VALUES ('" + No_Pago + "', NULL, " + No_Operacion + ", '");
                Mi_SQL.Append(Datos.P_Caja_ID + "', NULL, TO_DATE('" + String.Format("{0:dd-MM-yyy}", Datos.P_Fecha_Pago) + "','DD-MM-YYYY'), 'PAGADO', ");
                Mi_SQL.Append(Datos.P_Monto_Corriente + ", " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append("NULL, ");
                Mi_SQL.Append("'PREDIAL', ");
                Mi_SQL.Append(Datos.P_Total_Pagar + ", ");
                Mi_SQL.Append("0, ");
                Mi_SQL.Append("'" + Datos.P_Referencia + "', ");
                Mi_SQL.Append("'" + Cuenta_Predial_ID + "', ");
                Mi_SQL.Append("NULL, ");
                Mi_SQL.Append("SYSDATE)");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos            

                Mi_SQL.Length = 0;
                //Ingresa el pasivo del ajuste tarifario
                if (Datos.P_Ajuste_Tarifario != 0)
                {
                    Mi_SQL.Append("SELECT NVL(" + Ope_Pre_Parametros.Campo_Clave_Ing_Ajuste_Tarifa_ID + ", '')");
                    Mi_SQL.Append(" FROM " + Ope_Pre_Parametros.Tabla_Ope_Pre_Parametros);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Clave_Ingreso_Parametro_ID = Comando_SQL.ExecuteOracleScalar().ToString();
                    if (Clave_Ingreso_Parametro_ID != "")
                    {
                        Rs_Claves_Ingreso.P_Clave_Ingreso_ID = Clave_Ingreso_Parametro_ID;
                        Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso_Por_ID();
                        // si se o obtuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            //Ingresa el ajuste tarifario
                            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                            Rs_Modificar_Calculo.P_Cmd_Calculo = Comando_SQL;
                            Rs_Modificar_Calculo.P_Fecha_Tramite = Datos.P_Fecha_Pago.ToString();
                            Rs_Modificar_Calculo.P_Fecha_Vencimiento_Pasivo = Datos.P_Fecha_Pago.ToString();
                            Rs_Modificar_Calculo.P_Estatus = "POR PAGAR";
                            Rs_Modificar_Calculo.P_Nombre_Usuario = Datos.P_Nombre_Usuario;
                            Rs_Modificar_Calculo.P_Cuenta_Predial_ID = Datos.P_Cuenta_Predial_ID;
                            Rs_Modificar_Calculo.P_Referencia = Datos.P_Referencia;
                            Rs_Modificar_Calculo.P_Descripcion = "AJUSTE TARIFARIO";
                            Rs_Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Rs_Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Rs_Modificar_Calculo.P_Monto_Total_Pagar = Convert.ToString(Datos.P_Ajuste_Tarifario);
                            Rs_Modificar_Calculo.Alta_Pasivo();
                        }
                    }
                    else
                    {
                        throw new Exception("Error: No se puede realizar el pago, porque no esta establecida la clave de ajuste tarifario, favor de verificarlo.");
                    }
                }

                Mi_SQL.Length = 0;
                //Actualiza el estatus del ingreso pasivo en la base de datos
                Mi_SQL.Append("UPDATE " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo);
                Mi_SQL.Append(" SET " + Ope_Ing_Pasivo.Campo_Estatus + " = 'PAGADO', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_No_Pago + " = '" + No_Pago + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Recargos + " = " + Datos.P_Monto_Recargos + ", ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Pago + " = '" + String.Format("{0:dd/MM/yyyy}", Datos.P_Fecha_Pago) + "', ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Usuario_Modifico + " = NULL, ");
                Mi_SQL.Append(Ope_Ing_Pasivo.Campo_Fecha_Modifico + " = SYSDATE");
                Mi_SQL.Append(" WHERE " + Ope_Ing_Pasivo.Campo_Referencia + " = '" + Datos.P_Referencia + "'");
                Mi_SQL.Append(" AND " + Ope_Ing_Pasivo.Campo_Estatus + " = 'POR PAGAR'");
                Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos

                //Crea la proteccion de pago
                string Proteccion_Pago = "PAGADO/" + Convert.ToInt32(Datos.P_No_Caja) + "/" + Convert.ToInt32(No_Operacion) + "/" + String.Format("{0:yyyy.MM.dd}", Datos.P_Fecha_Pago) + "/" + String.Format("{0:HH:mm:ss}", Datos.P_Fecha_Pago) + "/" + String.Format("{0:###,##0.00}", Datos.P_Total_Pagar);
                if (Datos.P_Cuenta_Predial != "")
                {
                    Proteccion_Pago += "/" + Datos.P_Cuenta_Predial;
                }

                //Aplica los pagos al adeudo
                Aplicar_Adeudos_Predial(ref Comando_SQL, Datos.P_Dt_Adeudos_Predial_Cajas, Datos.P_Dt_Adeudos_Predial_Cajas_Detalle, Datos.P_Cuenta_Predial_ID, No_Pago.ToString(), Datos.P_Ajuste_Tarifario, Datos.P_Nombre_Usuario);



                //Agrega los valores de los campos a los controles correspondientes de la forma
                foreach (DataRow Registro in Datos.P_Dt_Formas_Pago.Rows)
                {
                    Mi_SQL.Length = 0;
                    //Consulta el último no de pago que fue registrado en la base de datos
                    Mi_SQL.Append("SELECT NVL(MAX(" + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + "),0)");
                    Mi_SQL.Append(" FROM " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles);
                    Comando_SQL.CommandText = Mi_SQL.ToString();
                    Consecutivo = Comando_SQL.ExecuteOracleScalar().ToString();
                    //Consecutivo = OracleHelper.ExecuteScalar(Transaccion_SQL, CommandType.Text, Mi_SQL.ToString());

                    if (Convert.IsDBNull(Consecutivo))
                    {
                        Consecutivo = 1;
                    }
                    else
                    {
                        Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                    }
                    Mi_SQL.Length = 0;
                    if (Registro["Forma_Pago"].ToString() == "EFECTIVO") //Forma de Pago en Efectivo
                    {
                        //Inserción de forma de pago en la base de datos
                        Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                        Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                        Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                        Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                    }
                    else
                    {
                        if (Registro["Forma_Pago"].ToString() == "AJUSTE TARIFARIO") //Ajuste tarifario
                        {
                            //Inserción de forma de pago en la base de datos
                            Mi_SQL.Append("INSERT INTO " + Ope_Caj_Pagos_Detalles.Tabla_Ope_Caj_Pagos_Detalles + "(");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_No_Pago + ", " + Ope_Caj_Pagos_Detalles.Campo_Forma_Pago + ", ");
                            Mi_SQL.Append(Ope_Caj_Pagos_Detalles.Campo_Monto + ", " + Ope_Caj_Pagos_Detalles.Campo_Consecutivo + ")");
                            Mi_SQL.Append(" VALUES ('" + No_Pago + "', '" + Registro["Forma_Pago"].ToString() + "', ");
                            Mi_SQL.Append(Convert.ToDecimal(Registro["Monto"].ToString()) + ", " + Consecutivo + ")");
                        }
                    }
                    Comando_SQL.CommandText = Mi_SQL.ToString(); //Asigna la inserción para ser ejecutada
                    Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos
                }

                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]" + " SQL: " + Mi_SQL);
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message + " SQL: " + Mi_SQL);
            }
            finally
            {
                Conexion_Base.Close();
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION    : Alta_Poliza
        /// DESCRIPCION             : Da de Alta la poliza con los datos de los Pasivos
        /// PARAMETROS: 
        /// CREO                    : Antonio Salvador Benavides Guardado
        /// FECHA_CREO              : 15/Junio/2012
        /// MODIFICO:
        /// FECHA_MODIFICO:
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Alta_Poliza(Cls_Ope_Caj_Pagos_Negocio Datos, String Concepto_Poliza, OracleCommand Cmmd)
        {
            Cls_Ope_Con_Polizas_Negocio Polizas = new Cls_Ope_Con_Polizas_Negocio();
            DataTable Dt_Jefe_Dependencia = null;
            try
            {
                Polizas.P_Empleado_ID = Datos.P_Empleado_ID;
                Dt_Jefe_Dependencia = Polizas.Consulta_Empleado_Jefe_Dependencia();
                Polizas = null;

                Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Polizas.P_Tipo_Poliza_ID = "00001";
                Polizas.P_Mes_Ano = Datos.P_Fecha_Pago.ToString("MMyy");
                Polizas.P_Fecha_Poliza = Datos.P_Fecha_Pago;
                Polizas.P_Concepto = Concepto_Poliza;
                Polizas.P_Total_Debe = Datos.P_Total_Pagar;
                Polizas.P_Total_Haber = Datos.P_Total_Pagar;
                Polizas.P_No_Partida = P_Dt_Partidas_Poliza.Rows.Count;
                Polizas.P_Nombre_Usuario = Datos.P_Nombre_Usuario;
                Polizas.P_Dt_Detalles_Polizas = P_Dt_Partidas_Poliza;
                Polizas.P_Empleado_ID_Creo = Datos.P_Empleado_ID;
                Polizas.P_Empleado_ID_Autorizo = "";
                Polizas.P_Cmmd = Cmmd;
                string[] Datos_Poliza = Polizas.Alta_Poliza(); //Da de alta los datos de la Póliza proporcionados por el usuario en la BD

                //if (Datos.P_Fecha_Pago.Month < DateTime.Now.Month
                //    || Datos.P_Fecha_Pago.Year < DateTime.Now.Year)
                //{
                //    Alta_Poliza_Desfasada(Datos_Poliza, (DataTable)Session["Dt_Partidas_Poliza"]);
                //}
                Cls_Ope_Con_Poliza_Ingresos_Datos.Alta_Poliza_Ingresos(P_Dt_Partidas_Poliza, Cmmd, "", Datos_Poliza[0], Datos_Poliza[1], Datos_Poliza[2], "RECAUDADO");
            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Poliza " + ex.Message.ToString(), ex);
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Afectaciones_Por_Tipo_Pasivo
        /// DESCRIPCIÓN: Dependiendo del tipo de pasivo (campo ORIGEN) llama al método para hacer la afectación correspondiente
        ///             (p.ej.: pasivos de trámites: insertar registro de pago en bitácora del trámite)
        /// PARÁMETROS:
        /// 		1. Tipo_Pasivo: cadena de caracteres para identificar el pasivo y las afectaciones a hacer
        /// 		2. Referencia: referencia del pasivo para encontrar el id del trámite
        /// 		3. Nombre_Usuario: parámetro para insertar en el campos de control USUARIO_CREO
        /// 		4. Cmd: Conexión a la base de datos (para utilizar la misma transacción)
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 28-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Afectaciones_Por_Tipo_Pasivo(string Tipo_Pasivo, string Referencia, string Nombre_Usuario, OracleCommand Cmd)
        {
            String Mi_SQL;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Int32 Filas_Afectadas = 0;
            int No_Detalle_Solicitud;

            // validar que Tipo_Pasivo contenga texto
            if (string.IsNullOrEmpty(Tipo_Pasivo))
            {
                return 0;
            }

            try
            {
                // si llego un Comando como parámetro, utilizarlo
                if (Cmd != null)    // si la conexión llego como parámetro, establecer como comando para utilizar
                {
                    Comando = Cmd;
                }
                else    // si no, crear nueva conexión y transacción
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }
                Comando.CommandType = CommandType.Text;

                // si es una solicitud de trámite, insertar registro de pago en bitácora del trámite
                if (Tipo_Pasivo.Trim() == "SOLICITUD TRAMITE")
                {
                    Cls_Ope_Bandeja_Tramites_Negocio Neg_Actualizar_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();

                    // consultar el trámite con el folio igual a la referencia del pasivo
                    Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Campo_Solicitud_ID
                        + "," + Ope_Tra_Solicitud.Campo_Subproceso_ID
                        + "," + Ope_Tra_Solicitud.Campo_Estatus
                        + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                        + " WHERE UPPER(TRIM(" + Ope_Tra_Solicitud.Campo_Clave_Solicitud + ")) = UPPER(TRIM('" + Referencia + "'))";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.CommandType = CommandType.Text;
                    OracleDataReader Dtr_Datos_Solicitud = Cmd.ExecuteReader();

                    // si hay datos para leer, agregar pasivo
                    if (Dtr_Datos_Solicitud.Read())
                    {
                        // establecer parámetros para actualizar solicitud
                        Neg_Actualizar_Solicitud.P_Solicitud_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Estatus = "APROBAR";
                        Neg_Actualizar_Solicitud.P_Subproceso_ID = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString().Trim();
                        Neg_Actualizar_Solicitud.P_Comentarios = "RECEPCION DE PAGO";
                        Neg_Actualizar_Solicitud.P_Usuario = Nombre_Usuario;
                        // pasar comando de oracle para utilizar la misma transacción
                        Neg_Actualizar_Solicitud.P_Comando_Oracle = Cmd;
                        // llamar método que actualizar la solicitud
                        Neg_Actualizar_Solicitud = Neg_Actualizar_Solicitud.Evaluar_Solicitud();

                        //// ya se inserta bitácora en la llamada a Evaluar_Solicitud()
                        //Filas_Afectadas += Alta_Bitacora_Solicitud_Tramite(Obj_Solicitud.ToString(), "PAGADO", "RECEPCION DE PAGO", Cmd);
                    }
                }

                // si la conexión no llego como parámetro, aplicar consultas
                if (Cmd == null)
                {
                    Transaccion.Commit();
                }

                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Alta_Bitacora_Solicitud_Tramite Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Cmd == null)
                {
                    Transaccion.Rollback();
                }
                throw new Exception("Alta_Bitacora_Solicitud_Tramite Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }
    }
}