using System;
using System.Data;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;
using Presidencia.Ope_Con_Poliza_Ingresos.Datos;
using Presidencia.Cuentas_Contables.Negocio;
using Presidencia.Polizas.Negocios;
using Presidencia.Sessiones;
using Presidencia.Cls_Ope_Ing_Pasivos.Negocio;
using Presidencia.Cls_Ope_Ing_Ordenes_Pago.Negocio;
using System.IO;

namespace Presidencia.Bandeja_Solicitudes_Tramites.Datos
{
    

    public class Cls_Ope_Bandeja_Tramites_Datos
    {  
        #region Propieades
        private static DataTable Dt_Partidas_Poliza;
        private static DataTable Dt_Movimientos_Presupuestales;
        #endregion 
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Tramites
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y la devuelve el resultado en
        ///             un DataTable.
        ///PARAMETROS:
        ///             1. Solicitud.  Obtiene el Parametro para obtener el tipo de DataTable.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************     
        public static DataTable Consultar_DataTable(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
        {
            String Mi_SQL = null;
            DataTable Tabla = null;
            DataSet Data_Set = null;

            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            string Dependencia_ID_Ordenamiento = "";
            string Dependencia_ID_Ambiental = "";
            string Dependencia_ID_Urbanistico = "";
            string Dependencia_ID_Inmobiliario = "";
            string Dependencia_ID_Catastro = "";
            String Rol_Director_Ordenamiento = "";

            try
            {
                //  se consultan los parametros
                // consultar parámetros
                Obj_Parametros.Consultar_Parametros();

                //  validar las dependencias de orddenamiento
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;
               
                //  validar los roles de los directores de ordenamiento
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                    Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;
               


                if (Solicitud.P_Tipo_DataTable.Equals("BANDEJA_TRAMITES"))
                {
                    Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " AS SOLICITUD_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " AS CLAVE_SOLICITUD";
                    Mi_SQL += ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE_ACTIVIDAD";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " AS TRAMITE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " AS CONSECUTIVO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Complemento+ " AS COMPLEMENTO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS SOLICITO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS FECHA";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                    Mi_SQL += ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;

                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID;

                    Mi_SQL += " AND " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID;
                    Mi_SQL += " = " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;

                    //  filtro para los subprocesos del empleado
                    Mi_SQL = Mi_SQL + " AND " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + " IN ";
                    Mi_SQL = Mi_SQL + " ( SELECT " + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID + " FROM " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles;
                    Mi_SQL = Mi_SQL + " WHERE " + Tra_Subprocesos_Perfiles.Campo_Perfil_ID + " IN ";

                    Mi_SQL = Mi_SQL + " ( SELECT " + Ope_Tra_Perfiles_Empleado.Campo_Perfil_ID + " FROM " + Ope_Tra_Perfiles_Empleado.Tabla_Ope_Tra_Perfiles_Empleado;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Perfiles_Empleado.Campo_Empleado_ID + " IN ";
                    Mi_SQL = Mi_SQL + " ( SELECT " + Cat_Empleados.Campo_Empleado_ID + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + " = '" + Solicitud.P_Empleado_ID + "')))";

                    if (!String.IsNullOrEmpty(Solicitud.P_Estatus))
                    {
                        if (Solicitud.P_Estatus == "PENDIENTE_PROCESO")
                        {
                            Mi_SQL += " AND (" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus
                                + " = 'PENDIENTE' OR " +
                                Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " = 'PROCESO' ) ";
                        }
                        else
                            Mi_SQL += " AND (" + Ope_Tra_Solicitud.Campo_Estatus + " = '" + Solicitud.P_Estatus + "') ";

                        //  filtro para el insepctor de ordenamiento
                        if (!String.IsNullOrEmpty(Solicitud.P_Estatus_Persona_Inspecciona))
                        {
                            Mi_SQL += " AND (" + Ope_Tra_Solicitud.Campo_Persona_Inspecciona + " = '" + Solicitud.P_Estatus_Persona_Inspecciona + "') ";
                        }
                        //if (Solicitud.P_Dependencia_ID == Dependencia_ID_Ordenamiento
                        //|| Solicitud.P_Dependencia_ID == Dependencia_ID_Ambiental
                        //|| Solicitud.P_Dependencia_ID == Dependencia_ID_Inmobiliario
                        //|| Solicitud.P_Dependencia_ID == Dependencia_ID_Urbanistico)
                        //{
                        //    Mi_SQL += " and " + Ope_Tra_Solicitud.Campo_Empleado_ID + "='" + Solicitud.P_Empleado_ID + "'";
                        //}
                    }
                    Mi_SQL += " ORDER BY ESTATUS";
                }
                else if (Solicitud.P_Tipo_DataTable.Equals("ACTUALIZACION_SOLICITUD"))
                {
                    Mi_SQL = "SELECT " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + ", " + Cat_Tra_Subprocesos.Campo_Valor + ", " + Cat_Tra_Subprocesos.Campo_Orden;
                    Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Tipo_Actividad;
                    Mi_SQL += " FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Subprocesos.Campo_Tramite_ID + " IN";
                    Mi_SQL = Mi_SQL + " ( SELECT " + Ope_Tra_Solicitud.Campo_Tramite_ID + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "' )";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Tra_Subprocesos.Campo_Orden + "";
                }
                else if (Solicitud.P_Tipo_DataTable.Equals("CORREO_ELECTRONICO"))
                {
                    Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Campo_Correo_Electronico + " AS CORREO_ELECTRONICO FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "'";
                }
                else if (Solicitud.P_Tipo_DataTable.Equals("FUENTE_DATOS_PLANTILLAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Tra_Tramites.Campo_Costo + " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID + " IN ";
                    Mi_SQL = Mi_SQL + " ( SELECT " + Ope_Tra_Solicitud.Campo_Tramite_ID + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "')";
                    Double Costo_Tramite = Convert.ToDouble(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).ToString());
                    Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " AS CLAVE_SOLICITUD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " AS PORCENTAJE_AVANCE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " AS ESTATUS_SOLICITUD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Usuario_Creo + " AS ATENDIO_SOLICITUD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS FECHA_SOLICITUD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS NOMBRE_SOLICITANTE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " AS APELLIDO_PATERNO_SOLICITO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " AS APELLIDO_MATERNO_SOLICITO";
                    Mi_SQL = Mi_SQL + ", ( " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " ) AS NOMBRE_COMPLETO_SOLICITANTE";
                    Mi_SQL = Mi_SQL + ", NVL(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Correo_Electronico + ", '--------------') AS CORREO_ELECTRONICO_SOLICITANTE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE_SUBPROCESO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Clave_Tramite + " AS CLAVE_TRAMITE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " AS NOMBRE_TRAMITE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado + " AS TIEMPO_ESTIMADO_TRAMITE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Costo + " AS COSTO_TRAMITE";
                    if (Costo_Tramite > 0)
                    {
                        Mi_SQL = Mi_SQL + ", " + Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_No_Cuenta + " AS CUENTA_TRAMITE";
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + ", 'N/A' AS CUENTA_TRAMITE";
                    }
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                    if (Costo_Tramite > 0)
                    {
                        Mi_SQL = Mi_SQL + ", " + Cat_Cuentas.Tabla_Cat_Cuentas;
                    }
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID;
                    if (Costo_Tramite > 0)
                    {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Cuentas.Tabla_Cat_Cuentas + "." + Cat_Cuentas.Campo_Cuenta_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Cuenta_ID;
                    }
                    Mi_SQL = Mi_SQL + " AND " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID;
                    Mi_SQL = Mi_SQL + " = '" + Solicitud.P_Solicitud_ID + "'";
                }
                if (Mi_SQL != null)
                {
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Data_Set != null)
                {
                    Tabla = Data_Set.Tables[0];
                }
                else
                {
                    Tabla = new DataTable();
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Solicitudes_Dependencia
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************  
        public static DataTable Consultar_Solicitudes_Dependencia(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
        {
            String Mi_SQL = null;
            DataTable Tabla = null;
            DataSet Data_Set = null;
            Boolean Entro_Where = false;

            try
            {
                Mi_SQL = "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " AS SOLICITUD_ID, " +
                    "SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " AS CLAVE_SOLICITUD, " +
                    "SUBPROCESOS." + Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE_ACTIVIDAD, " +
                    "TRAMITES." + Cat_Tra_Tramites.Campo_Nombre + " AS TRAMITE, " +
                    "SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + " AS ESTATUS, " +
                    "SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo + " AS CONSECUTIVO, " +
                    "SOLICITUD." + Ope_Tra_Solicitud.Campo_Complemento + " AS COMPLEMENTO, " + 
                    "SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + " ||' '|| SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno + " ||' '|| SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS SOLICITO, " +
                    "SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS FECHA FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD";
                Mi_SQL += " RIGHT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITES";
                Mi_SQL += " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = TRAMITES." + Cat_Tra_Tramites.Campo_Tramite_ID;
                Mi_SQL += " LEFT OUTER JOIN " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " SUBPROCESOS";
                Mi_SQL += " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = SUBPROCESOS." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;

                if (!String.IsNullOrEmpty(Solicitud.P_Dependencia_ID))
                {
                    if (Entro_Where) Mi_SQL += " AND "; else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " TRAMITES." + Cat_Tra_Tramites.Campo_Dependencia_ID + " = " + Solicitud.P_Dependencia_ID;
                }
                if (!String.IsNullOrEmpty(Solicitud.P_Estatus))
                {
                    if (Entro_Where) Mi_SQL += " AND "; else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + " = '" + Solicitud.P_Estatus + "'";
                }
                Mi_SQL += " ORDER BY SOLICITUD_ID";

                if (Mi_SQL != null)
                {
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Data_Set != null)
                {
                    Tabla = Data_Set.Tables[0];
                }
                else
                {
                    Tabla = new DataTable();
                }

            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
            return Tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Solicitud_Director_Ordenamiento
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos y la devuelve el resultado en
        ///             un DataTable.
        ///PARAMETROS:
        ///             1. Solicitud.  Obtiene el Parametro para obtener el tipo de DataTable.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************     
        public static DataTable Consultar_Solicitud_Director_Ordenamiento(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            String Mi_SQL = "";
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " AS SOLICITUD_ID";
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " AS CLAVE_SOLICITUD";
                Mi_SQL += ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE_ACTIVIDAD";
                Mi_SQL += ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre + " AS TRAMITE";
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " AS ESTATUS";
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo + " AS CONSECUTIVO";
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno + "";
                Mi_SQL += " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno + "";
                Mi_SQL += " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS SOLICITO";
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS FECHA";
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Complemento + " AS COMPLEMENTO";
                
                Mi_SQL += " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;

                Mi_SQL += " left outer join  " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " on ";
                Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = ";
                Mi_SQL += Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID;

                Mi_SQL += " left outer join  " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos+ " on ";
                Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = ";
                Mi_SQL += Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;

                Mi_SQL += " where ";
                if (Negocio.P_Estatus == "PENDIENTE_PROCESO")
                {
                    Mi_SQL += "(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " ='PENDIENTE' or ";
                    Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " ='PROCESO' )";
                }
                else
                {
                    Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus + " ='" + Negocio.P_Estatus + "'";
                }
                Mi_SQL += " and " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID + "='" + Negocio.P_Dependencia_ID + "'";
                Mi_SQL += " and (TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ") >=";
                Mi_SQL += "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Inicio)) + "') and ";
                Mi_SQL += " TO_DATE(" + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo + ") <=";
                Mi_SQL += "TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Negocio.P_Fecha_Fin)) + "') )";
               
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
            return Dt_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Solicitud
        ///DESCRIPCIÓN: Hace una consulta a la Base de Datos de los datos a detalle de una
        ///             Solicitud.
        ///PARAMETROS:
        ///             1. Solicitud.   Obtiene el Parametro para obtener los Datos a Detalle
        ///                             de la Solicitud.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 15/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public static Cls_Ope_Bandeja_Tramites_Negocio Consultar_Datos_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
        {
            Cls_Ope_Bandeja_Tramites_Negocio R_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
            OracleDataReader Data_Reader = null;
            String Mi_SQL = null;
            DataSet dataSet = new DataSet();
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;   

            try
            {

                // si llego un Comando como parámetro, utilizarlo
                if (Solicitud.P_Comando_Oracle != null)    // si la conexión llego como parámetro, establecer como comando para utilizar
                {
                    Comando = Solicitud.P_Comando_Oracle;
                }
                else    // si no, crear nueva conexión y transacción
                {
                    Conexion.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion.Open();
                    Transaccion = Conexion.BeginTransaction();
                    Comando.Connection = Conexion;
                    Comando.Transaction = Transaccion;
                }

                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud;//0
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Nombre;//1

                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + " ||' '|| " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante;//2

                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Estatus;//3
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo;//4
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID;//5
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre;//6
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance;//7
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID;//8
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Dependencia_ID;//9
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Cuenta_Predial;//10
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Inspector_ID;//11
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Zona_ID;//12
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Empleado_ID;//13
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado;//14
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Tipo_Actividad;//15
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Condicion_Si;//16
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Condicion_No;//17
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Folio;//18
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Area_Dependencia;//19
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Actividad_Anterior;//20
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total;//21
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Contribuyente_Id;//22
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Cantidad;//23
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Base;//24
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Correo_Electronico;//25
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Consecutivo;//26
                Mi_SQL += ", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden;//27
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Direccion_Predio;//28
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Propietario_Predio;//29
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Calle_Predio;//30
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Numero_Predio;//31
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Manzana_Predio;//32
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Lote_Predio;//33
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Otros_Predio;//34
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio;//35
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin;//36
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Persona_Inspecciona;//37
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Inicio;//38
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Fin;//39
                Mi_SQL += ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Complemento;//40

                Mi_SQL = Mi_SQL + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;

                Mi_SQL += " left outer join " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " on ";
                Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=";
                Mi_SQL += Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID;

                Mi_SQL += " left outer join " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " on ";
                Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Subproceso_ID + "=";
                Mi_SQL += Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;

                Mi_SQL = Mi_SQL + " WHERE ";
                Mi_SQL += Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "'";
                R_Solicitud.P_Solicitud_ID = Solicitud.P_Solicitud_ID;

                Data_Reader = OracleHelper.ExecuteReader(Comando, CommandType.Text, Mi_SQL);

                while (Data_Reader.Read())
                {
                    R_Solicitud.P_Clave_Solicitud = Data_Reader.GetString(0);
                    R_Solicitud.P_Tramite = Data_Reader.GetString(1);
                    R_Solicitud.P_Solicito = Data_Reader.GetString(2);
                    R_Solicitud.P_Estatus = Data_Reader.GetString(3);
                    R_Solicitud.P_Fecha_Solicitud = Data_Reader.GetDateTime(4);
                    R_Solicitud.P_Subproceso_ID = Data_Reader.GetString(5);
                    R_Solicitud.P_Subproceso_Nombre = Data_Reader.GetString(6);
                    R_Solicitud.P_Porcentaje_Avance = Data_Reader.GetInt32(7);
                    R_Solicitud.P_Tramite_id = Data_Reader.GetString(8);
                    R_Solicitud.P_Dependencia_ID = Data_Reader.GetString(9);
                    R_Solicitud.P_Cuenta_Predial = !Data_Reader.IsDBNull(10) ? Data_Reader.GetString(10) : "";
                    R_Solicitud.P_Inspector_ID = !Data_Reader.IsDBNull(11) ? Data_Reader.GetString(11) : "";
                    R_Solicitud.P_Zona_ID = Data_Reader.FieldCount >= 13 && !Data_Reader.IsDBNull(12) ? Data_Reader.GetString(12) : "";
                    R_Solicitud.P_Empleado_ID = Data_Reader.FieldCount >= 14 && !Data_Reader.IsDBNull(13) ? Data_Reader.GetString(13) : "";
                    R_Solicitud.P_Tiempo_Estimado = Data_Reader.GetDouble(14);
                    R_Solicitud.P_Tipo_Actividad = Data_Reader.GetString(15);
                    R_Solicitud.P_Condicion_Si = !Data_Reader.IsDBNull(16) ? Data_Reader.GetDouble(16): 0;
                    R_Solicitud.P_Condicion_No = !Data_Reader.IsDBNull(17) ? Data_Reader.GetDouble(17) : 0;
                    R_Solicitud.P_Folio = Data_Reader.GetString(18);
                    R_Solicitud.P_Area_Dependencia = Data_Reader.GetString(19);
                    R_Solicitud.P_SubProceso_Anterior = !Data_Reader.IsDBNull(20) ? Data_Reader.GetString(20) : "";
                    R_Solicitud.P_Costo_Total = !Data_Reader.IsDBNull(21) ? Data_Reader.GetDouble(21) : 0;
                    R_Solicitud.P_Contribuyente_Id = !Data_Reader.IsDBNull(22) ? Data_Reader.GetString(22) : "";
                    R_Solicitud.P_Unidades = !Data_Reader.IsDBNull(23) ? Data_Reader.GetDouble(23) : 0;
                    R_Solicitud.P_Costo_Base = !Data_Reader.IsDBNull(24) ? Data_Reader.GetDouble(24) : 0;
                    R_Solicitud.P_Email = !Data_Reader.IsDBNull(25) ? Data_Reader.GetString(25) : "";
                    R_Solicitud.P_Consecutivo = !Data_Reader.IsDBNull(26) ? Data_Reader.GetString(26) : "";
                    R_Solicitud.P_Orden_Actividad = !Data_Reader.IsDBNull(27) ? Data_Reader.GetDouble(27) : 0;
                    R_Solicitud.P_Direccion_Predio = !Data_Reader.IsDBNull(28) ? Data_Reader.GetString(28) : "";
                    R_Solicitud.P_Propietario_Predio = !Data_Reader.IsDBNull(29) ? Data_Reader.GetString(29) : "";
                    R_Solicitud.P_Calle_Predio = !Data_Reader.IsDBNull(30) ? Data_Reader.GetString(30) : "";
                    R_Solicitud.P_Nuemro_Predio = !Data_Reader.IsDBNull(31) ? Data_Reader.GetString(31) : "";
                    R_Solicitud.P_Manzana_Predio = !Data_Reader.IsDBNull(32) ? Data_Reader.GetString(32) : "";
                    R_Solicitud.P_Lote_Predio = !Data_Reader.IsDBNull(33) ? Data_Reader.GetString(33) : "";
                    R_Solicitud.P_Otros_Predio = !Data_Reader.IsDBNull(34) ? Data_Reader.GetString(34) : "";
                    R_Solicitud.P_Fecha_Date_Vigencia_Inicio = !Data_Reader.IsDBNull(35) ? Data_Reader.GetDateTime(35):DateTime.Today;
                    R_Solicitud.P_Fecha_Date_Vigencia_Fin = !Data_Reader.IsDBNull(36) ? Data_Reader.GetDateTime(36) : DateTime.Today;
                    R_Solicitud.P_Persona_Inspecciona = !Data_Reader.IsDBNull(37) ? Data_Reader.GetString(37) : "";
                    R_Solicitud.P_Date_Fecha_Documento_Vigencia_Inicio = !Data_Reader.IsDBNull(38) ? Data_Reader.GetDateTime(38) : DateTime.Today;
                    R_Solicitud.P_Date_Fecha_Documento_Vigencia_Fin = !Data_Reader.IsDBNull(39) ? Data_Reader.GetDateTime(39) : DateTime.Today;
                    R_Solicitud.P_Complemento = !Data_Reader.IsDBNull(40) ? Data_Reader.GetString(40) : "";
                }
                Data_Reader.Close();

                //  para los datos de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Ope_Dato_ID + " AS OPE_DATO_ID";
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Dato_ID + " AS DATO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Nombre + " AS NOMBRE_DATO";
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Valor + " AS VALOR";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + ", " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Dato_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Dato_ID;
                Mi_SQL = Mi_SQL + " AND " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Solicitud_ID;
                Mi_SQL = Mi_SQL + " = '" + Solicitud.P_Solicitud_ID + "'";
                Mi_SQL += " AND " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Tipo_Dato;
                Mi_SQL += " = 'INICIAL'";
                Mi_SQL += " ORDER BY NOMBRE_DATO asc";

                dataSet = OracleHelper.ExecuteDataset(Comando, CommandType.Text, Mi_SQL);
              
                
                if (dataSet == null)
                {
                    R_Solicitud.P_Datos_Solicitud = new DataTable();
                }
                else
                {
                    R_Solicitud.P_Datos_Solicitud = dataSet.Tables[0];
                }
                dataSet = null;

                //  para los documentos de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos + "." + Ope_Tra_Documentos.Campo_Ope_Documento_ID + " AS OPE_DOCUMENTO_ID";
                Mi_SQL = Mi_SQL + ", " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID + " AS DETALLE_DOCUMENTO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Nombre + " AS NOMBRE_DOCUMENTO";
                Mi_SQL = Mi_SQL + ", " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos + "." + Ope_Tra_Documentos.Campo_URL + " AS URL";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos + ", " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos;
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Documento_ID + " IN";
                Mi_SQL = Mi_SQL + " ( SELECT " + Tra_Detalle_Documentos.Campo_Documento_ID + " FROM " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos;
                Mi_SQL = Mi_SQL + " WHERE " + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID + " IN";
                Mi_SQL = Mi_SQL + " ( SELECT " + Ope_Tra_Documentos.Campo_Detalle_Documento_ID + " FROM " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Documentos.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "') )";
                Mi_SQL = Mi_SQL + " AND " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID;
                Mi_SQL = Mi_SQL + " = " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos + "." + Ope_Tra_Documentos.Campo_Detalle_Documento_ID;
                Mi_SQL = Mi_SQL + " AND " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos + "." + Ope_Tra_Documentos.Campo_Solicitud_ID;
                Mi_SQL = Mi_SQL + " = '" + Solicitud.P_Solicitud_ID + "'";
                Mi_SQL = Mi_SQL + " AND " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Documento_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Documento_ID;

                dataSet = OracleHelper.ExecuteDataset(Comando, CommandType.Text, Mi_SQL);
                

                if (dataSet == null)
                {
                    R_Solicitud.P_Documentos_Solicitud = new DataTable();
                }
                else
                {
                    R_Solicitud.P_Documentos_Solicitud = dataSet.Tables[0];
                }
                dataSet = null;

                //  para los actividades de la solicitud
                Mi_SQL = "SELECT " + Cat_Tra_Subprocesos.Campo_Plantilla + " FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " = '" + R_Solicitud.P_Subproceso_ID + "'";
                Comando.CommandText = Mi_SQL;
                String Temporal = Comando.ExecuteScalar().ToString();

                if (!Temporal.Equals("00000"))
                {
                    Mi_SQL = "SELECT " + Cat_Tra_Plantillas.Campo_Plantilla_ID + " AS PLANTILLA_ID, " + Cat_Tra_Plantillas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Tra_Plantillas.Campo_Archivo + " AS ARCHIVO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Plantillas.Campo_Plantilla_ID + " = '" + Temporal + "'";

                    dataSet = OracleHelper.ExecuteDataset(Comando, CommandType.Text, Mi_SQL);
                   
                }
                if (dataSet == null)
                {
                    R_Solicitud.P_Plantillas_Subproceso = new DataTable();
                }
                else
                {
                    R_Solicitud.P_Plantillas_Subproceso = dataSet.Tables[0];
                }

                //  para las solicitudes complementarias
                Mi_SQL = "SELECT SOLICITUD." + Ope_Tra_Solicitud.Campo_Solicitud_ID + " AS SOLICITUD_ID, ";
                Mi_SQL += "SOLICITUD." + Ope_Tra_Solicitud.Campo_Clave_Solicitud + " AS CLAVE_SOLICITUD, ";
                Mi_SQL += "SUBPROCESOS. "+ Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE_ACTIVIDAD, ";
                Mi_SQL += "TRAMITES. " + Cat_Tra_Tramites.Campo_Nombre + " AS TRAMITE, ";
                Mi_SQL += "SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + " AS ESTATUS, ";
                Mi_SQL += "SOLICITUD." + Ope_Tra_Solicitud.Campo_Consecutivo + " AS CONSECUTIVO, ";
                Mi_SQL += "SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Paterno;
                Mi_SQL += " ||' '|| SOLICITUD." + Ope_Tra_Solicitud.Campo_Apellido_Materno;
                Mi_SQL += " ||' '|| SOLICITUD." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " AS SOLICITO, ";
                Mi_SQL += "SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo + " AS FECHA";

                Mi_SQL += " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD ";

                Mi_SQL += " LEFT OUTER JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITES ";
                Mi_SQL += "ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = TRAMITES." + Cat_Tra_Tramites.Campo_Tramite_ID;

                Mi_SQL += " LEFT OUTER JOIN " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " SUBPROCESOS ";
                Mi_SQL += "ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = SUBPROCESOS." + Cat_Tra_Subprocesos.Campo_Subproceso_ID;

                Mi_SQL += " WHERE SOLICITUD." + Ope_Tra_Solicitud.Campo_Complemento + " = '" + R_Solicitud.P_Solicitud_ID + "'";

                dataSet = OracleHelper.ExecuteDataset(Comando, CommandType.Text, Mi_SQL);
                if (dataSet == null)
                {
                    R_Solicitud.P_Solicitudes_Complementarias = new DataTable();
                }
                else
                {
                    R_Solicitud.P_Solicitudes_Complementarias = dataSet.Tables[0];
                }
                dataSet = null;
            }

             catch (OracleException Ex)
            {
                Transaccion.Rollback();

                throw new Exception("Consultar datos de la solicitud Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }

            finally
            {
                if (Data_Reader != null && !Data_Reader.IsClosed)
                {
                    Data_Reader.Close();
                }
                 if (Solicitud.P_Comando_Oracle == null)
                {
                    Conexion.Close();
                }
            }

            /*******************************************/
            return R_Solicitud;
            /*******************************************/
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Detalles_Tarjetas_Gasolina
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.Parametros.Contiene los parametros que se van a utilizar para
        ///                       hacer la consulta de la Base de Datos.
        ///CREO                 : Salvador Vázquez Camacho
        ///FECHA_CREO           : 30/Julio/2010
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Bandeja_Tramites_Negocio Consultar_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Parametros)
        {
            String Mi_SQL = null;
            Cls_Ope_Bandeja_Tramites_Negocio Obj_Cargado = new Cls_Ope_Bandeja_Tramites_Negocio();
            try
            {
                Mi_SQL = "SELECT * FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Parametros.P_Solicitud_ID + "'";
                OracleDataReader Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Reader.Read())
                {
                    Obj_Cargado.P_Solicitud_ID =       Reader.IsDBNull(0)  ? "" : Reader.GetString(0);
                    Obj_Cargado.P_Tramite_id =         Reader.IsDBNull(1)  ? "" : Reader.GetString(1);
                    Obj_Cargado.P_Clave_Solicitud =    Reader.IsDBNull(2)  ? "" : Reader.GetString(2);
                    Obj_Cargado.P_Porcentaje_Avance =  Reader.IsDBNull(3)  ? 0.0 : Reader.GetDouble(3);
                    Obj_Cargado.P_Estatus =            Reader.IsDBNull(4)  ? "" : Reader.GetString(4);
                    Obj_Cargado.P_Fecha_Entraga =      Reader.IsDBNull(5)  ? new DateTime() : Reader.GetDateTime(5); 
                    Obj_Cargado.P_Nombre =             Reader.IsDBNull(10)  ? "" : Reader.GetString(10);
                    Obj_Cargado.P_Apellido_Paterno =   Reader.IsDBNull(11)  ? "" : Reader.GetString(11);
                    Obj_Cargado.P_Apellido_Materno =   Reader.IsDBNull(12) ? "" : Reader.GetString(12);
                    Obj_Cargado.P_Subproceso_ID =      Reader.IsDBNull(13)  ? "" : Reader.GetString(13);
                    Obj_Cargado.P_Comentarios =        Reader.IsDBNull(14)  ? "" : Reader.GetString(14);
                    Obj_Cargado.P_Correo_Electronico = Reader.IsDBNull(15) ? "" : Reader.GetString(15);
                    Obj_Cargado.P_Cuenta_Predial =     Reader.IsDBNull(16) ? "" : Reader.GetString(16); 
                    Obj_Cargado.P_Inspector_ID =       Reader.IsDBNull(17) ? "" : Reader.GetString(17);
                    Obj_Cargado.P_Zona_ID =            Reader.IsDBNull(18) ? "" : Reader.GetString(18);
                    Obj_Cargado.P_Empleado_ID =        Reader.IsDBNull(19) ? "" : Reader.GetString(19);
                    Obj_Cargado.P_Folio =              Reader.IsDBNull(20) ? "" : Reader.GetString(20);
                }
                Reader.Close();
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Obj_Cargado;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Evaluar_Solicitud
        ///DESCRIPCIÓN: Actualiza los campos, estatus y avance de una solicitud evaluada.
        ///PARAMETROS:
        ///             1. Solicitud.   Solicitud con los parámetros para evaluarla.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 18/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        public static Cls_Ope_Bandeja_Tramites_Negocio Evaluar_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
        {
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Solicitud= new Cls_Ope_Bandeja_Tramites_Negocio();

            String Mensaje = "";
            String Estatus_Detalle = "";
            StringBuilder Mi_Sql = new StringBuilder();
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            double Condicion_Actividad = 0.0;
            String Elemento_ID = "";
            Double Avance = 0;
            String Subproceso_Siguiente = null;
            String Subproceso_Anterior = null;
            String Actividad_Anterior_ID = null;
            String Orden_Actividad = null;
            Boolean Ultimo = false;
            Boolean Primero = false;
            Boolean Redireccionar_Modulo = false;
            String Tipo_Actividad = null;
            string Dependencia_ID_Ordenamiento = "";
            string Dependencia_ID_Ambiental = "";
            string Dependencia_ID_Urbanistico = "";
            string Dependencia_ID_Inmobiliario = "";
            string Dependencia_ID_Catastro = "";
            String Rol_Director_Ordenamiento = "";



            // generar conexión y transacción si no llegó comando de oracle como parámetro
            if (Solicitud.P_Comando_Oracle == null)
            {
                Cn.ConnectionString = Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
            }
            else
            {
                Cmd = Solicitud.P_Comando_Oracle;
            }

            try
            {
                Negocio_Consultar_Solicitud.P_Solicitud_ID = Solicitud.P_Solicitud_ID;
                Negocio_Consultar_Solicitud = Negocio_Consultar_Solicitud.Consultar_Datos_Solicitud(); // Se obtienen los Datos a Detalle de la Solicitud Seleccionada


                // consultar parámetros
                Obj_Parametros.Consultar_Parametros();

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                    Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                    Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                    Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                    Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Catastro))
                    Dependencia_ID_Catastro = Obj_Parametros.P_Dependencia_ID_Catastro;

                // validar que la consulta haya regresado valor
                if (!string.IsNullOrEmpty(Obj_Parametros.P_Rol_Director_Ordenamiento))
                    Rol_Director_Ordenamiento = Obj_Parametros.P_Rol_Director_Ordenamiento;


                Solicitud.P_Tipo_DataTable = "ACTUALIZACION_SOLICITUD";
                DataTable Subprocesos = Consultar_DataTable(Solicitud);

                if (Solicitud.P_Estatus.Equals("REGRESAR") && Solicitud.P_Estatus != "PENDIENTE")
                {
                    //  si la variable no contiene la actividad anterior se busca cual es la anterior
                    if (String.IsNullOrEmpty(Solicitud.P_SubProceso_Anterior))
                    {
                        //  se comenzara de la ultima actividad a la primera
                        for (Int32 Contador = Subprocesos.Rows.Count - 1; Contador >= 0; Contador--)
                        {
                            if (Solicitud.P_Subproceso_ID.Equals(Subprocesos.Rows[Contador][0].ToString()))
                            {
                                //  sse obtiene el id de la actividad anterior
                                Actividad_Anterior_ID = Subprocesos.Rows[Contador - 1][0].ToString();
                                //Orden_Actividad=
                                break;
                            }
                        }
                    }
                    else
                    {
                        Actividad_Anterior_ID = Solicitud.P_SubProceso_Anterior;
                    }


                    //  se obtiene el porcentaje 
                    if (!String.IsNullOrEmpty(Actividad_Anterior_ID))
                    {
                        for (Int32 Contador = 0; Contador < Subprocesos.Rows.Count; Contador++)
                        {
                            if (Actividad_Anterior_ID.Equals(Subprocesos.Rows[Contador][0].ToString()))
                            {
                                if (Solicitud.P_Estatus.Equals("REGRESAR"))
                                {
                                    Avance = Avance + Convert.ToInt32(Subprocesos.Rows[Contador][1].ToString());
                                    Subproceso_Anterior = Subprocesos.Rows[Contador][0].ToString();
                                    Tipo_Actividad = Subprocesos.Rows[Contador][3].ToString();
                                    Orden_Actividad = Subprocesos.Rows[Contador][2].ToString();

                                    if (Subprocesos.Rows[Contador][2].ToString() == "1")
                                    {
                                        Primero = true;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                Avance = Avance + Convert.ToInt32(Subprocesos.Rows[Contador][1].ToString());
                            }

                        }// fin del for 
                    }

                    //  Buscara si se tiene alguna actividad con condicion
                    String Mi_SQL = "Select * from " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                    Mi_SQL += " Where (" + Cat_Tra_Subprocesos.Campo_Condicion_Si + "='" + Orden_Actividad + "'";
                    Mi_SQL += " or " + Cat_Tra_Subprocesos.Campo_Condicion_No + "='" + Orden_Actividad + "') ";
                    Mi_SQL += " and (" + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Solicitud.P_Tramite_id + "')";
                    DataTable Dt_Consulta_Actividad = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                    if (Dt_Consulta_Actividad != null && Dt_Consulta_Actividad.Rows.Count > 0)
                    {
                        Orden_Actividad = Dt_Consulta_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim();
                    }
                    else
                    {
                        Orden_Actividad = "";
                    }

                    if (Tipo_Actividad != "COBRO")
                    {
                        Mi_SQL = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ";
                        Mi_SQL += "" + Ope_Tra_Solicitud.Campo_Actividad_Anterior + " = '" + Orden_Actividad + "'";
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Costo_Total + " = '" + Solicitud.P_Costo_Total + "'";

                        //   para la primera fase que sere pendiente
                        if (Primero == true)
                        {

                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " = '0'";
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Zona_ID + " = ''";//   se quita la zona
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Empleado_ID + " = ''";//   se quita el empleado id 
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Estatus + " = 'PENDIENTE'";
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = '" + Subproceso_Anterior + "'";
                            Solicitud.P_Enviar_Correo_Electronico = false;
                            Estatus_Detalle = "PENDIENTE";
                        }

                        else
                        {
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Estatus + " = 'PROCESO'";
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " = '" + Avance.ToString() + "'";
                            Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = '" + Subproceso_Anterior + "'";
                        }

                        Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Usuario_Modifico + " = '" + Solicitud.P_Usuario + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "'";

                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();




                        /* para el historal */
                        Mi_Sql = new StringBuilder();

                        //  se limpia el historial de los comentarios detenidos
                        Estatus_Detalle = "TERMINADO";
                        Mi_Sql.Append("delete " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud);
                        Mi_Sql.Append(" Where " + Ope_Tra_Det_Solicitud.Campo_Estatus + "='DETENIDO'");
                        Mi_Sql.Append(" And " + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID + "='" + Solicitud.P_Solicitud_ID + "'");
                        Cmd.CommandText = Mi_Sql.ToString();
                        Cmd.ExecuteNonQuery();

                        Mi_Sql = new StringBuilder();
                        Elemento_ID = Consecutivo_ID(Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID, Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud, "10");

                        Mi_Sql.Append("INSERT INTO " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "(");
                        Mi_Sql.Append(Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Subproceso_ID);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Estatus);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Comentarios);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Fecha);

                        Mi_Sql.Append(") VALUES( ");

                        Mi_Sql.Append("'" + Elemento_ID + "' ");
                        Mi_Sql.Append(", '" + Solicitud.P_Solicitud_ID + "' ");
                        Mi_Sql.Append(", '" + Solicitud.P_Subproceso_ID + "' ");
                        Mi_Sql.Append(", '" + Estatus_Detalle + "' ");
                        Mi_Sql.Append(", '" + Solicitud.P_Comentarios + "' ");
                        Mi_Sql.Append(", SYSDATE ");
                        Mi_Sql.Append(")");
                        Cmd.CommandText = Mi_Sql.ToString();
                        Cmd.ExecuteNonQuery();

                        //  para los comentarios del historial interno de la dependencia
                        Mi_Sql = new StringBuilder();
                        Elemento_ID = Consecutivo_ID(Ope_Tra_Det_Solicitud_Interna.Campo_Detalle_Solicitud_Interna_ID, Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna, "10");

                        Mi_Sql.Append("INSERT INTO " + Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "(");
                        Mi_Sql.Append(Ope_Tra_Det_Solicitud_Interna.Campo_Detalle_Solicitud_Interna_ID);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Solicitud_ID);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Subproceso_ID);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Estatus);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Comentarios);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Fecha);
                        Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Usuario_Creo);

                        Mi_Sql.Append(") VALUES( ");

                        Mi_Sql.Append("'" + Elemento_ID + "' ");
                        Mi_Sql.Append(", '" + Solicitud.P_Solicitud_ID + "' ");
                        Mi_Sql.Append(", '" + Solicitud.P_Subproceso_ID + "' ");
                        Mi_Sql.Append(", '" + Estatus_Detalle + "' ");
                        Mi_Sql.Append(", '" + Solicitud.P_Comentarios_Internos + "' ");
                        Mi_Sql.Append(", SYSDATE ");
                        Mi_Sql.Append(", '" + Cls_Sessiones.Nombre_Empleado + "' ");
                        Mi_Sql.Append(")");
                        Cmd.CommandText = Mi_Sql.ToString();
                        Cmd.ExecuteNonQuery();

                    

                    }//     FIN DEL TIPO DE ACTIVIDAD DIFERENTE A COBRO
                    else
                    {

                    }
                }// fin de la comparacion de REGRESAR

                else
                {
                    //  para obtener el porcentaje de las actividades 
                    //  PARA LAS CONDICIONES
                    if (!String.IsNullOrEmpty(Solicitud.P_Respuesta_Condicion))
                    {
                        if (Solicitud.P_Respuesta_Condicion == "SI")
                            Condicion_Actividad = Solicitud.P_Condicion_Si;
                        else
                            Condicion_Actividad = Solicitud.P_Condicion_No;

                        for (Int32 Contador = 0; Contador < Subprocesos.Rows.Count; Contador++)
                        {
                            if (Condicion_Actividad - 1 == Convert.ToDouble(Subprocesos.Rows[Contador][2].ToString()))
                            {
                                if (Solicitud.P_Estatus.Equals("APROBAR"))
                                {
                                    Avance = Avance + Convert.ToInt32(Subprocesos.Rows[Contador][1].ToString());
                                    if ((Contador + 1) < Subprocesos.Rows.Count)
                                    {
                                        Subproceso_Siguiente = Subprocesos.Rows[Contador + 1][0].ToString();
                                    }
                                    else if ((Contador + 1) == Subprocesos.Rows.Count)
                                    {
                                        Ultimo = true;
                                        Avance = 100;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                Avance = Avance + Convert.ToInt32(Subprocesos.Rows[Contador][1].ToString());
                            }
                        }
                    }
                    else
                    {
                        for (Int32 Contador = 0; Contador < Subprocesos.Rows.Count; Contador++)
                        {
                            if (Solicitud.P_Subproceso_ID.Equals(Subprocesos.Rows[Contador][0].ToString()))
                            {
                                if (Solicitud.P_Estatus.Equals("APROBAR"))
                                {
                                    Avance = Avance + Convert.ToInt32(Subprocesos.Rows[Contador][1].ToString());
                                    if ((Contador + 1) < Subprocesos.Rows.Count)
                                    {
                                        Subproceso_Siguiente = Subprocesos.Rows[Contador + 1][0].ToString();

                                        //  validacion para mandar a modulo
                                        if ((!string.IsNullOrEmpty(Negocio_Consultar_Solicitud.P_Dependencia_ID) && Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Ordenamiento)
                                            || (!string.IsNullOrEmpty(Negocio_Consultar_Solicitud.P_Dependencia_ID) && Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Ambiental)
                                            || (!string.IsNullOrEmpty(Negocio_Consultar_Solicitud.P_Dependencia_ID) && Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Inmobiliario)
                                            || (!string.IsNullOrEmpty(Negocio_Consultar_Solicitud.P_Dependencia_ID) && Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Urbanistico))
                                        {
                                            if (Contador + 1 == Subprocesos.Rows.Count - 1)
                                                Redireccionar_Modulo = true;

                                        }
                                    }
                                    else if ((Contador + 1) == Subprocesos.Rows.Count)
                                    {
                                        Ultimo = true;
                                        Avance = 100;
                                    }
                                }
                                break;
                            }
                            else
                            {
                                Avance = Avance + Convert.ToInt32(Subprocesos.Rows[Contador][1].ToString());
                            }
                        }
                    }
                    String Mi_SQL = "";


                    if (Solicitud.P_Costo_Total != 0)
                    {
                        //  se actualiza el costo del tramite antes de generar el pasivo
                        String Mi_SQL_Costo = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ";
                        Mi_SQL_Costo += Ope_Tra_Solicitud.Campo_Costo_Total + " = '" + Solicitud.P_Costo_Total + "'";
                        Mi_SQL_Costo = Mi_SQL_Costo + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "'";
                        Cmd.CommandText = Mi_SQL_Costo;
                        Cmd.ExecuteNonQuery();
                    }

                    //  actualizacon de la solicitud normal
                    Mi_SQL = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ";
                    Mi_SQL = Mi_SQL + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + " = '" + Avance.ToString() + "'";
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Actividad_Anterior + " = '" + Solicitud.P_Subproceso_ID + "'";

                    //  fecha inicio de la vigencia (uso exclusivo de Ordenamiento Territorial)
                    if (!string.IsNullOrEmpty(Solicitud.P_Fecha_Vigencia_Inicio))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Inicio + " = '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Solicitud.P_Fecha_Vigencia_Inicio)) + "'";

                    //  fecha fin de la vigencia (uso exclusivo de Ordenamiento Territorial)
                    if (!string.IsNullOrEmpty(Solicitud.P_Fecha_Vigencia_Fin))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Vigencia_Fin + " = '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Solicitud.P_Fecha_Vigencia_Fin)) + "'";

                    //  fecha inicio de la vigencia (uso exclusivo de Ordenamiento Territorial)
                    if (!string.IsNullOrEmpty(Solicitud.P_Fecha_Documento_Vigencia_inicio))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Inicio + " = '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Solicitud.P_Fecha_Documento_Vigencia_inicio)) + "'";

                    //  fecha inicio de la vigencia (uso exclusivo de Ordenamiento Territorial)
                    if (!string.IsNullOrEmpty(Solicitud.P_Fecha_Documento_Vigencia_Fin))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Condicion_Documento_Vigencia_Fin + " = '" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Solicitud.P_Fecha_Documento_Vigencia_Fin)) + "'";

                    // agregar empleado_id y zona_id si se proporcionaron como parámetro
                    if (!string.IsNullOrEmpty(Solicitud.P_Zona_ID))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Zona_ID + " = '" + Solicitud.P_Zona_ID + "'";

                    if (!string.IsNullOrEmpty(Solicitud.P_Empleado_ID))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Empleado_ID + " = '" + Solicitud.P_Empleado_ID + "'";

                    if (!string.IsNullOrEmpty(Solicitud.P_Persona_Inspecciona))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Persona_Inspecciona + " = '" + Solicitud.P_Persona_Inspecciona + "'";

                    //  validacion para la ubicacion del expediente
                    if (!string.IsNullOrEmpty(Solicitud.P_Ubicacion_Expediente))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Ubicacion_Expediente + " = '" + Solicitud.P_Ubicacion_Expediente + "'";


                    if (Solicitud.P_Estatus.Equals("APROBAR"))
                    {
                        if (Ultimo)
                        {
                            Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Estatus + " = 'TERMINADO'";
                            Solicitud.P_Enviar_Correo_Electronico = true;
                            Solicitud.P_Estatus = "TERMINADO";
                            Estatus_Detalle = "TERMINADO";
                        }
                        else
                        {
                            Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Estatus + " = 'PROCESO'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Subproceso_ID + " = '" + Subproceso_Siguiente + "'";
                            Solicitud.P_Enviar_Correo_Electronico = true;
                            Estatus_Detalle = "PROCESO";
                            // Verificar que el tramite sea de Ordenamiento territorial para asignar un consecutivo
                            if (Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Ordenamiento ||
                                Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Ambiental ||
                                Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Urbanistico ||
                                Negocio_Consultar_Solicitud.P_Dependencia_ID == Dependencia_ID_Inmobiliario)
                            {
                                //  se agrega el consecutivo para ordenamiento
                                if (String.IsNullOrEmpty(Negocio_Consultar_Solicitud.P_Consecutivo))
                                {
                                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Consecutivo + " = '" + Consecutivo_ID(Ope_Tra_Solicitud.Campo_Consecutivo, Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud, "10") + "'";
                                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Folio + "='" + Solicitud.P_Folio + "/" + Convert.ToInt64(Consecutivo_ID(Ope_Tra_Solicitud.Campo_Consecutivo, Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud, "10")) + "/" + DateTime.Now.Year.ToString() + "' ";
                                }
                            }
                        }

                        // consultar tipo de subproceso siguiente Insercion del pasivo****************************************************
                        if (!string.IsNullOrEmpty(Subproceso_Siguiente))
                        {
                            DataTable Dt_Subproceso;
                            Cls_Cat_Tramites_Negocio Neg_Consulta_Subprocesos = new Cls_Cat_Tramites_Negocio();
                            Neg_Consulta_Subprocesos.P_Sub_Proceso_ID = Subproceso_Siguiente;
                            Dt_Subproceso = Neg_Consulta_Subprocesos.Consultar_Subprocesos_Tramite();
                            // validar que la consulta haya regresado resultados
                            if (Dt_Subproceso != null && Dt_Subproceso.Rows.Count > 0)
                            {
                                // dar de alta el pasivo para el pago si el subproceso siguiente es de tipo COBRO
                                foreach (DataRow Fila_Subproceso in Dt_Subproceso.Rows)
                                {
                                    if (Fila_Subproceso[Cat_Tra_Subprocesos.Campo_Tipo_Actividad].ToString() == "COBRO")
                                    {
                                        Alta_Pasivo_Solicitud(Solicitud.P_Solicitud_ID, "SOLICITUD TRAMITE", Cmd);
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    else if (Solicitud.P_Estatus.Equals("DETENER"))
                    {
                        Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Estatus + " = 'DETENIDO'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Comentarios + " = '" + Solicitud.P_Comentarios + "'";
                        Solicitud.P_Enviar_Correo_Electronico = true;
                        Solicitud.P_Estatus = "DETENIDO";
                        Estatus_Detalle = "DETENIDO";
                    }
                    else if (Solicitud.P_Estatus.Equals("CANCELAR"))
                    {
                        Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Estatus + " = 'CANCELADO'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Comentarios + " = '" + Solicitud.P_Comentarios + "'";
                        Solicitud.P_Enviar_Correo_Electronico = true;
                        Solicitud.P_Estatus = "CANCELADO";
                        Estatus_Detalle = "CANCELADO";
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Usuario_Modifico + " = '" + Solicitud.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Tra_Solicitud.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();


                    //  se compara el estatus diferente a CANCELADO o DETENIDO
                    if (Estatus_Detalle == "TERMINADO")
                    {
                        Estatus_Detalle = "FIN DEL TRAMITE";
                    }
                    else if (Estatus_Detalle == "PROCESO")
                    {
                        //  se limpia el historial de los comentarios detenidos
                        Estatus_Detalle = "TERMINADO";
                        Mi_Sql.Append("delete " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud);
                        Mi_Sql.Append(" Where " + Ope_Tra_Det_Solicitud.Campo_Estatus + "='DETENIDO'");
                        Mi_Sql.Append(" And " + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID + "='" + Solicitud.P_Solicitud_ID + "'");
                        Cmd.CommandText = Mi_Sql.ToString();
                        Cmd.ExecuteNonQuery();
                    }

                    //  para los comentarios del historial del ciudadano
                    Mi_Sql = new StringBuilder();
                    Elemento_ID = Consecutivo_ID(Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID, Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud, "10");

                    Mi_Sql.Append("INSERT INTO " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "(");
                    Mi_Sql.Append(Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Subproceso_ID);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Estatus);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Comentarios);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Fecha);

                    Mi_Sql.Append(") VALUES( ");

                    Mi_Sql.Append("'" + Elemento_ID + "' ");
                    Mi_Sql.Append(", '" + Solicitud.P_Solicitud_ID + "' ");
                    Mi_Sql.Append(", '" + Solicitud.P_Subproceso_ID + "' ");
                    Mi_Sql.Append(", '" + Estatus_Detalle + "' ");
                    Mi_Sql.Append(", '" + Solicitud.P_Comentarios + "' ");
                    Mi_Sql.Append(", SYSDATE ");
                    Mi_Sql.Append(")");
                    Cmd.CommandText = Mi_Sql.ToString();
                    Cmd.ExecuteNonQuery();

                    //  para los comentarios del historial interno de la dependencia
                    Mi_Sql = new StringBuilder();
                    Elemento_ID = Consecutivo_ID(Ope_Tra_Det_Solicitud_Interna.Campo_Detalle_Solicitud_Interna_ID, Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna, "10");

                    Mi_Sql.Append("INSERT INTO " + Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "(");
                    Mi_Sql.Append(Ope_Tra_Det_Solicitud_Interna.Campo_Detalle_Solicitud_Interna_ID);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Solicitud_ID);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Subproceso_ID);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Estatus);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Comentarios);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Fecha);
                    Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud_Interna.Campo_Usuario_Creo);

                    Mi_Sql.Append(") VALUES( ");

                    Mi_Sql.Append("'" + Elemento_ID + "' ");
                    Mi_Sql.Append(", '" + Solicitud.P_Solicitud_ID + "' ");
                    Mi_Sql.Append(", '" + Solicitud.P_Subproceso_ID + "' ");
                    Mi_Sql.Append(", '" + Estatus_Detalle + "' ");
                    Mi_Sql.Append(", '" + Solicitud.P_Comentarios_Internos + "' ");
                    Mi_Sql.Append(", SYSDATE ");
                    Mi_Sql.Append(", '" + Cls_Sessiones.Nombre_Empleado + "' ");
                    Mi_Sql.Append(")");
                    Cmd.CommandText = Mi_Sql.ToString();
                    Cmd.ExecuteNonQuery();

                    //  validacion para redireccionar a modulo
                    if (Redireccionar_Modulo == true)
                    {
                        StringBuilder Sql_Cadena = new StringBuilder();
                        Sql_Cadena.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " set ");
                        Sql_Cadena.Append(Ope_Tra_Solicitud.Campo_Empleado_ID + "='0000001102' ");
                        Sql_Cadena.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Solicitud.P_Solicitud_ID + "'");
                        Cmd.CommandText = Sql_Cadena.ToString();
                        Cmd.ExecuteNonQuery();
                    }
                    
                }//FIN DEL ELSE PRINCIPAL


                /******************  para cambiar el estatus de las solicitudes hijas    ***********************/
                Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Dias = new Cls_Ope_Bandeja_Tramites_Negocio();
                Negocio_Consultar_Dias.P_Solicitud_ID = Solicitud.P_Solicitud_ID;
                Negocio_Consultar_Dias.P_Comando_Oracle = Cmd;
                Negocio_Consultar_Dias = Negocio_Consultar_Dias.Consultar_Datos_Solicitud();

                Solicitud.P_Solicitud_ID = Solicitud.P_Solicitud_ID;
                Solicitud.P_Subproceso_ID = Negocio_Consultar_Dias.P_Subproceso_ID;
                Solicitud.P_Estatus = Negocio_Consultar_Dias.P_Estatus;
                Solicitud.P_Porcentaje_Avance = Negocio_Consultar_Dias.P_Porcentaje_Avance;
                Solicitud.P_Consecutivo = Negocio_Consultar_Dias.P_Consecutivo;

                Mi_Sql = new StringBuilder();

                Mi_Sql.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ");
                Mi_Sql.Append(Ope_Tra_Solicitud.Campo_Subproceso_ID + "='" + Solicitud.P_Subproceso_ID + "' ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Estatus + "='" + Solicitud.P_Estatus + "' ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + "='" + Solicitud.P_Porcentaje_Avance + "' ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Consecutivo + "='" + Solicitud.P_Consecutivo + "' ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Usuario_Modifico + "='" + Solicitud.P_Usuario + "' ");
                Mi_Sql.Append(", " + Ope_Tra_Solicitud.Campo_Fecha_Modifico + "=SYSDATE ");
                Mi_Sql.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Complemento + "='" + Solicitud.P_Solicitud_ID + "' ");
                Cmd.CommandText = Mi_Sql.ToString();
                Cmd.ExecuteNonQuery();


                if (Solicitud.P_Comando_Oracle == null)
                {
                    Trans.Commit();
                }

                if (Solicitud.P_Estatus != "REGRESAR")
                {
                    if (Solicitud.P_Enviar_Correo_Electronico)
                    {
                        Solicitud.P_Tipo_DataTable = "CORREO_ELECTRONICO";
                        DataTable DT_Temporal = Consultar_DataTable(Solicitud);
                        if (DT_Temporal != null && DT_Temporal.Rows.Count > 0)
                        {
                            Solicitud.P_Correo_Electronico = DT_Temporal.Rows[0]["CORREO_ELECTRONICO"].ToString();
                        }
                    }
                }
            }
            catch (OracleException Ex)
            {
                if (Solicitud.P_Comando_Oracle == null && Trans != null)
                {
                    Trans.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar";
                }
                else
                {
                    Mensaje = Ex.Message; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                if (Solicitud.P_Comando_Oracle == null && Trans != null)
                    Trans.Rollback();

                throw new Exception(Ex.ToString());
            } 
            finally
            {
                if (Solicitud.P_Comando_Oracle == null) 
                    Cn.Close();
            }
            return Solicitud;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar
        ///DESCRIPCIÓN          : Modificara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Fechas_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " set ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Campo_Fecha_Creo + "= SYSDATE");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Fecha_Entrega + "='" + String.Format("{0:dd/MM/yyyy}", Negocio.P_Fecha_Entraga) + "' ");
                Mi_SQL.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la modificacion. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Zona
        ///DESCRIPCIÓN          : Modificara la zona a la que pertenece la solicitud
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 11/Agosto/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Boolean Modificar_Zona(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            try
            {
                Mi_SQL.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " set ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Campo_Zona_ID + "='" + Negocio.P_Zona_ID + "' ");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Empleado_ID + "='" + Negocio.P_Empleado_ID + "' ");
                Mi_SQL.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar la modificacion. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION    : Eliminar_Pasivo
        /// DESCRIPCION             : Da de baja el pasivo con los datos de los Pasivos
        /// PARAMETROS: 
        /// CREO                    : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO              : 12/Diciembre/2012
        /// MODIFICO:
        /// FECHA_MODIFICO:
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private static void Eliminar_Pasivo(String Referencia, ref OracleCommand Cmmd)
        {
            Cls_Ope_Ing_Pasivos_Negocio Rs_Eliminar_Pasivo = new Cls_Ope_Ing_Pasivos_Negocio();
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL = null;
            try
            {
                if (Cmmd == null)
                {
                    // crear transaccion
                    Conexion_Base.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Conexion_Base.Open();
                    Transaccion_SQL = Conexion_Base.BeginTransaction();
                    Comando_SQL.Connection = Conexion_Base;
                    Comando_SQL.Transaction = Transaccion_SQL;
                }
                else
                {
                    Comando_SQL = Cmmd;
                }

                Rs_Eliminar_Pasivo.P_Referencia = Referencia;
                Rs_Eliminar_Pasivo.P_Estatus = "POR PAGAR";
                DataTable Dt_Consulta_Pasivo_ID = Rs_Eliminar_Pasivo.Consultar_Pasivos();

                if (Dt_Consulta_Pasivo_ID != null && Dt_Consulta_Pasivo_ID.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Consulta_Pasivo_ID.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Ope_Ing_Pasivo.Campo_Pasivo_ID].ToString()))
                        {
                            Rs_Eliminar_Pasivo.P_No_Pasivo = Registro[Ope_Ing_Pasivo.Campo_Pasivo_ID].ToString();
                            Rs_Eliminar_Pasivo.P_Cmmd = Comando_SQL;
                            Rs_Eliminar_Pasivo.Eliminar_Pasivo();
                        }
                    }
                }
            }

            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Eliminar_Poliza " + Ex.Message.ToString(), Ex);
            }
            finally
            {
                if (Comando_SQL == null)
                {
                    Conexion_Base.Close();
                }
            }
        }
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Alta_Pasivo_Solicitud
        /// DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un pasivo de solicitud de trámite
        /// PARÁMETROS:
        /// 		1. No_Solicitud: número de solicitud a consultar
        /// 		2. Origen: Texto con la descripción del origen del pasivo
        /// 		3. Cmd: Conexión a la base de datos
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 28-jun-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Alta_Pasivo_Solicitud(string No_Solicitud, string Origen, OracleCommand Cmd)
        {
            Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
            Cls_Ope_Bandeja_Tramites_Negocio Neg_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();            
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;           
            String Porcentaje_Penalizacion = "";
            String Solicitud_ID = "";
            String Inspector_ID = "";            
            String Mi_SQL;
            String Descripcion = "";
            String Referencia = "";
            String Poliza_Debe_Haber = "";
            String[] Datos_Poliza;
            int Cnt_Pasivos = 1; 
            Int32 Filas_Afectadas = 0;
            Double Suma_Costo_Solicitudes = 0.0;
            Double Costo_Bitacora = 0.0;
            Double Costo_Perito = 0.0;
            DataTable Dt_Poliza = new DataTable();
            DataTable Dt_Orden_Pago = new DataTable();
            P_Dt_Movimientos_Presupuestales = new DataTable();
            
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

                //  consulta para obtener la clave principal de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                  + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                  + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                  + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " NOMBRE_SOLICITANTE"
                  + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*"
                  + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total
                  + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Contribuyente_Id
                  + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                  + " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites
                  + " ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = "
                  + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID
                  + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " ='" + No_Solicitud + "'";

                Comando.CommandText = Mi_SQL;
                OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();

                if (Dtr_Datos_Solicitud.Read())
                {
                    Referencia = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString().Trim().ToUpper();
                }

                //  consulta para obtener todas las solicitudes referentes a la principal
                Mi_SQL = "SELECT * FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                Mi_SQL += " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + No_Solicitud + "'";
                Mi_SQL += " OR " + Ope_Tra_Solicitud.Campo_Complemento + "='" + No_Solicitud + "'";
                Mi_SQL += " ORDER BY " + Ope_Tra_Solicitud.Campo_Complemento + ", " + Ope_Tra_Solicitud.Campo_Solicitud_ID;
                DataTable Dt_Solicitudes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                if (Dt_Solicitudes != null && Dt_Solicitudes.Rows.Count > 0)
                {
                    Neg_Solicitud.P_Solicitud_ID = No_Solicitud;
                    Porcentaje_Penalizacion = Neg_Solicitud.Consultar_Penalizaciones();

                    // ******* Inicio se consultan los parametros ******************************
                    Obj_Parametros.Consultar_Parametros();

                    if (!String.IsNullOrEmpty(Obj_Parametros.P_Costo_Bitacora))
                        Costo_Bitacora = Convert.ToDouble(Obj_Parametros.P_Costo_Bitacora);

                    if (!String.IsNullOrEmpty(Obj_Parametros.P_Costo_Perito))
                        Costo_Perito = Convert.ToDouble(Obj_Parametros.P_Costo_Perito);
                    // ******* Fin se consultan los parametros *********************************


                    foreach (DataRow Registro in Dt_Solicitudes.Rows)
                    {
                        Double Costo_Unitario = 0.0;
                        Solicitud_ID = Registro[Ope_Tra_Solicitud.Campo_Solicitud_ID].ToString();
                        Inspector_ID = Registro[Ope_Tra_Solicitud.Campo_Inspector_ID].ToString();
                        Costo_Unitario = Generar_Pasivo(Solicitud_ID, Origen, Comando, Cnt_Pasivos, Referencia);
                        Suma_Costo_Solicitudes += Costo_Unitario;

                        //  se genera la poliza campo haber
                        //Dt_Orden_Pago = Generar_Tabla_Concepto(Dt_Orden_Pago, Comando, Solicitud_ID, "TRAMITE", Costo_Unitario);
                        Dt_Poliza = Generar_Tabla_Poliza(Solicitud_ID, Poliza_Debe_Haber, Comando, Dt_Poliza, Suma_Costo_Solicitudes, false, "");
                        Cnt_Pasivos++;

                    }

                    if (Porcentaje_Penalizacion != "")
                    {
                        Double Porcentaje_Costo = Suma_Costo_Solicitudes * ((Convert.ToDouble(Porcentaje_Penalizacion)) / 100);
                        Descripcion = "CARGO ADICIONAL POR CONSTRUCCION " + Porcentaje_Penalizacion + "%";
                        Generar_Pasivo_Cobro_Adicional(Solicitud_ID, Origen, Porcentaje_Costo, Descripcion, Comando, Cnt_Pasivos, Referencia);
                        Suma_Costo_Solicitudes = Suma_Costo_Solicitudes + (Suma_Costo_Solicitudes * ((Convert.ToDouble(Porcentaje_Penalizacion)) / 100));
                        Cnt_Pasivos++;

                        //  poliza campo haber y  detalle de la orden de pago
                        //Dt_Orden_Pago = Generar_Tabla_Concepto(Dt_Orden_Pago, Comando, Solicitud_ID, "CARGO CONSTRUCCION", Porcentaje_Costo);
                        Dt_Poliza = Generar_Tabla_Poliza(Solicitud_ID, Poliza_Debe_Haber, Comando, Dt_Poliza, Suma_Costo_Solicitudes, true, Porcentaje_Costo.ToString());
                    }

                    if (Inspector_ID != "")
                    {
                        Descripcion = "CARGO PERITO";
                        Suma_Costo_Solicitudes += Costo_Perito;
                        Generar_Pasivo_Cobro_Adicional(Solicitud_ID, Origen, Costo_Perito, Descripcion, Comando, Cnt_Pasivos, Referencia);

                        //  se genera la poliza campo haber
                        //Dt_Orden_Pago = Generar_Tabla_Concepto(Dt_Orden_Pago, Comando, Solicitud_ID, Descripcion, Costo_Perito);
                        Dt_Poliza = Generar_Tabla_Poliza(Solicitud_ID, Poliza_Debe_Haber, Comando, Dt_Poliza, Suma_Costo_Solicitudes, true, Costo_Perito.ToString());
                        Cnt_Pasivos++;

                        Descripcion = "CARGO BITACORA";
                        Suma_Costo_Solicitudes += Costo_Bitacora;
                        Generar_Pasivo_Cobro_Adicional(Solicitud_ID, Origen, Costo_Bitacora, Descripcion, Comando, Cnt_Pasivos, Referencia);
                        //  se genera la poliza campo haber
                        //Dt_Orden_Pago = Generar_Tabla_Concepto(Dt_Orden_Pago, Comando, Solicitud_ID, Descripcion, Costo_Bitacora);
                        Dt_Poliza = Generar_Tabla_Poliza(Solicitud_ID, Poliza_Debe_Haber, Comando, Dt_Poliza, Suma_Costo_Solicitudes, true, Costo_Bitacora.ToString());
                        Cnt_Pasivos++;
                    }
                    Poliza_Debe_Haber = "PRINCIPAL";
                    //  se genera la poliza campo debe
                    Dt_Poliza = Generar_Tabla_Poliza(Solicitud_ID, Poliza_Debe_Haber, Comando, Dt_Poliza, Suma_Costo_Solicitudes, false, "");

                    //  se da de alta la poliza
                    Datos_Poliza = Alta_Poliza(Suma_Costo_Solicitudes, Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Nombre].ToString(), Dt_Poliza, Comando, Referencia);



                    Mi_SQL = "Update " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " Set ";
                    Mi_SQL += Ope_Tra_Solicitud.Campo_No_Poliza + "='" + Datos_Poliza[0] + "'";
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Tipo_Poliza_Id + "='" + Datos_Poliza[1] + "'";
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Mes_Ano + "='" + Datos_Poliza[2] + "'";
                    Mi_SQL += " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + No_Solicitud + "'";
                    Mi_SQL += " OR " + Ope_Tra_Solicitud.Campo_Complemento + "='" + No_Solicitud + "'";
                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();

                    Dtr_Datos_Solicitud.Close();
                }

                if (Cmd == null)    // si la conexión no llego como parámetro, aplicar consultas
                {
                    Transaccion.Commit();
                }

                return Filas_Afectadas;
            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Agregar_Concepto_DataTable
        ///DESCRIPCIÓN          : Método para agregar los importes y su concepto a P_Dt_Conceptos_Orden
        ///PARAMETROS:     
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 29/Noviembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static DataTable Generar_Tabla_Concepto(DataTable P_Dt_Conceptos_Orden, OracleCommand Cmd, String Solicitud_ID, String Descripcion, Double Monto)
        {
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            String Mi_SQL = "";
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

                if (P_Dt_Conceptos_Orden != null && P_Dt_Conceptos_Orden.Rows.Count == 0)
                {
                    P_Dt_Conceptos_Orden = Crear_Dt_Conceptos_Orden();
                }
                DataRow Dr_Conceptos_Orden;

                 // consultar detalles de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " NOMBRE_SOLICITANTE"
                    + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*"
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites
                    + " ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = "
                    + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID
                    + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " ='" + Solicitud_ID + "'";

                Comando.CommandText = Mi_SQL;
                OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();

                if (Dtr_Datos_Solicitud.Read())
                {
                    DataTable Dt_Datos_Subconcepto = null;

                    // consultar concepto y subconcepto de la cuenta contable del trámite
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID
                        + "," + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID
                        + "," + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID
                        + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables
                        + " LEFT OUTER JOIN " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " ON "
                        + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID
                        + " LEFT OUTER JOIN " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " ON "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + " = "
                        + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID
                        + " WHERE TRIM(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave + ") = TRIM('"
                        + Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Cuenta_ID].ToString() + "')";
                    // ejecutar consulta
                    Dt_Datos_Subconcepto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                    Cls_Cat_Con_Cuentas_Contables_Negocio Negocio_Cuenta_Contebles = new Cls_Cat_Con_Cuentas_Contables_Negocio();
                    Negocio_Cuenta_Contebles.P_Cuenta_Contable_ID = Dt_Datos_Subconcepto.Rows[0][Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString(); ;
                    DataTable Dt_Cuenta = Negocio_Cuenta_Contebles.Consulta_Existencia_Cuenta_Contable();

                    Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consultar_Solicitud= new Cls_Ope_Bandeja_Tramites_Negocio();;
                    Negocio_Consultar_Solicitud.P_Solicitud_ID = Solicitud_ID;
                    Negocio_Consultar_Solicitud = Negocio_Consultar_Solicitud.Consultar_Datos_Solicitud();

                    Dr_Conceptos_Orden = P_Dt_Conceptos_Orden.NewRow();
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID] = P_Dt_Conceptos_Orden.Rows.Count + 1;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago] = "";
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año] = DateTime.Now.Year;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio] = "";
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID] = Dt_Datos_Subconcepto.Rows[0][Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID].ToString(); ; ;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron] = "";
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus] = "POR PAGAR";
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades] = Negocio_Consultar_Solicitud.P_Unidades;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe] = Monto.ToString();
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario] = 0;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total] = (Convert.ToDouble(Negocio_Consultar_Solicitud.P_Unidades) * Monto).ToString();
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia] = Descripcion + " - " + Negocio_Consultar_Solicitud.P_Tramite;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones] = Negocio_Consultar_Solicitud.P_Tramite;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Creo] = Cls_Sessiones.Nombre_Empleado;
                    Dr_Conceptos_Orden[Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Creo] = DateTime.Now;
                    P_Dt_Conceptos_Orden.Rows.Add(Dr_Conceptos_Orden);

                }
                Dtr_Datos_Solicitud.Close();

                //Hdn_Concepto_Orden_Pago_ID.Value = (P_Dt_Conceptos_Orden.Rows.Count + 1).ToString();
               

                return P_Dt_Conceptos_Orden;
            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Conceptos_Orden
        ///DESCRIPCIÓN          : Metodo que devuelve un DataTable con los campos indicados
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 06/Junio/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        private static DataTable Crear_Dt_Conceptos_Orden()
        {
            DataTable Dt_Conceptos_Orden = new DataTable();

            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Concepto_Orden_Pago_ID, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_No_Orden_Pago, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Año, typeof(Int32));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Folio, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_SubConcepto_Ing_ID, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Tipo_Pago_ID, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Garantia_Proceso_ID, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Banco_ID, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fuente_Financiamiento_ID, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Numero_Garantia, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Clave_Padron, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Multa, typeof(DateTime));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Estatus, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Unidades, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Importe, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Descuento_Importe, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Monto_Importe, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Honorarios, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Multas, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Moratorios, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Recargos, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Ajuste_Tarifario, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Total, typeof(Decimal));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Referencia, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Observaciones, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Usuario_Creo, typeof(String));
            Dt_Conceptos_Orden.Columns.Add(Ope_Ing_Conceptos_Ordenes_Pago.Campo_Fecha_Creo, typeof(DateTime));
            Dt_Conceptos_Orden.Columns.Add("MODIFICADO", typeof(Boolean));

            return Dt_Conceptos_Orden;
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Tabla_Poliza
        /// DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un pasivo de solicitud de trámite
        /// PARÁMETROS:
        /// 		1. No_Solicitud: número de solicitud a consultar
        /// 		2. Origen: Texto con la descripción del origen del pasivo
        /// 		3. Cmd: Conexión a la base de datos
        /// CREO: Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO: 26-Nov-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Generar_Tabla_Poliza(String Solicitud_ID, string Origen, OracleCommand Cmd,
                                                    DataTable Dt_Partidas_Poliza, double Suma_Total, Boolean Estado_Cargo_Adicional, 
                                                    String Cargo_Adicional)
        {
            String Mi_SQL;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            DataRow Dr_Conceptos;
            DataRow Dr_Movimientos_Presupuestales;
            String Haber = "";
            try
            {
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

                if (Dt_Partidas_Poliza != null && Dt_Partidas_Poliza.Rows.Count == 0)
                {
                    Dt_Partidas_Poliza = Crear_Dt_Partidas_Poliza();
                }

                if (P_Dt_Movimientos_Presupuestales != null && P_Dt_Movimientos_Presupuestales.Rows.Count == 0)
                {
                    P_Dt_Movimientos_Presupuestales = Crear_Dt_Movimientos_Presupuestales();
                }

                // consultar detalles de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " NOMBRE_SOLICITANTE"
                    + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*"
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites
                    + " ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = "
                    + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID
                    + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " ='" + Solicitud_ID + "'";

                Comando.CommandText = Mi_SQL;
                OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();

                if (Dtr_Datos_Solicitud.Read())
                {
                    DataTable Dt_Datos_Subconcepto = null;


                    // consultar concepto y subconcepto de la cuenta contable del trámite
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID
                        + "," + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID
                        + "," + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID
                        + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables
                        + " LEFT OUTER JOIN " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " ON "
                        + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID
                        + " LEFT OUTER JOIN " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " ON "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + " = "
                        + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID
                        + " WHERE TRIM(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave + ") = TRIM('"
                        + Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Cuenta_ID].ToString() + "')";
                    // ejecutar consulta
                    Dt_Datos_Subconcepto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];


                    Cls_Cat_Con_Cuentas_Contables_Negocio Negocio_Cuenta_Contebles = new Cls_Cat_Con_Cuentas_Contables_Negocio();
                    Negocio_Cuenta_Contebles.P_Cuenta_Contable_ID = Dt_Datos_Subconcepto.Rows[0][Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString(); ;
                    DataTable Dt_Cuenta = Negocio_Cuenta_Contebles.Consulta_Existencia_Cuenta_Contable();

                    if (Origen == "PRINCIPAL")
                    {
                        //  campo fijo
                        Dr_Conceptos = Dt_Partidas_Poliza.NewRow();
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Poliza.Rows.Count + 1;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = "00261";
                        Dr_Conceptos[Cat_Con_Cuentas_Contables.Campo_Cuenta] = "112200001";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Concepto] = "CUENTAS POR COBRAR A CORTO PLAZO";
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Debe] = Suma_Total;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Haber] = 0;
                        //Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID] = "00022";
                        //Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID] = "";
                        Dt_Partidas_Poliza.Rows.Add(Dr_Conceptos);
                    }
                    else
                    {
                        if (Estado_Cargo_Adicional == true)
                        {
                            Haber = Cargo_Adicional;
                        }
                        else
                        {
                            Haber = Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Costo_Total].ToString();
                        }

                        Dr_Conceptos = Dt_Partidas_Poliza.NewRow();
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Partida] = Dt_Partidas_Poliza.Rows.Count + 1;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID] = Dt_Datos_Subconcepto.Rows[0][Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();// Dr_Pasivos[Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID];
                        Dr_Conceptos[Cat_Con_Cuentas_Contables.Campo_Cuenta] = Dt_Cuenta.Rows[0][Cat_Con_Cuentas_Contables.Campo_Cuenta]; ;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Concepto] = Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Nombre].ToString();
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Debe] = 0;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Haber] = Haber;
                        Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Fuente_Financiamiento_ID] = "00022";
                        //Dr_Conceptos[Ope_Con_Polizas_Detalles.Campo_Dependencia_ID] = "";
                        Dt_Partidas_Poliza.Rows.Add(Dr_Conceptos);


                        //Llena el DataTable para los conceptos de la Afectación Presupuestal
                        Dr_Movimientos_Presupuestales = P_Dt_Movimientos_Presupuestales.NewRow();
                        Dr_Movimientos_Presupuestales["Fte_Financiamiento_ID"] = "00022";
                        Dr_Movimientos_Presupuestales[Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID] = "0000000654";
                        Dr_Movimientos_Presupuestales[Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID] = Dt_Datos_Subconcepto.Rows[0][Ope_Ing_Pasivo.Campo_Concepto_Ing_ID].ToString().Trim();
                        Dr_Movimientos_Presupuestales[Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID] = Dt_Datos_Subconcepto.Rows[0][Ope_Ing_Pasivo.Campo_SubConcepto_Ing_ID].ToString().Trim();
                        Dr_Movimientos_Presupuestales["Anio"] = DateTime.Now.Year;
                        Dr_Movimientos_Presupuestales["Importe"] = Haber;
                        P_Dt_Movimientos_Presupuestales.Rows.Add(Dr_Movimientos_Presupuestales);

                    }
                }

                return Dt_Partidas_Poliza;
            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Pasivo
        /// DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un pasivo de solicitud de trámite
        /// PARÁMETROS:
        /// 		1. No_Solicitud: número de solicitud a consultar
        /// 		2. Origen: Texto con la descripción del origen del pasivo
        /// 		3. Cmd: Conexión a la base de datos
        /// CREO: Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO: 26-Nov-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Double Generar_Pasivo(String Solicitud_ID, string Origen, OracleCommand Cmd, int Numero_Pasivo, String Referencia_Principal)
        {
            String Mi_SQL;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Boolean Filas_Afectadas = true;
            Double Suma_Costo_Solicitudes = 0.0;

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
                
                // consultar detalles de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " NOMBRE_SOLICITANTE"
                    + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*"
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites
                    + " ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = "
                    + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID
                    + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " ='" +Solicitud_ID + "'";

                Comando.CommandText = Mi_SQL;
                OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();
                // si hay datos para leer, agregar pasivo
                if (Dtr_Datos_Solicitud.Read())
                {
                    if (Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Costo_Total].ToString() != "")
                        Suma_Costo_Solicitudes = Convert.ToDouble(Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Costo_Total].ToString());

                    DataTable Dt_Datos_Subconcepto = null;
                    // consultar concepto y subconcepto de la cuenta contable del trámite
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID
                        + "," + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID
                        + "," + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID
                        + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables
                        + " LEFT OUTER JOIN " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " ON "
                        + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID
                        + " LEFT OUTER JOIN " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " ON "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + " = "
                        + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID
                        + " WHERE TRIM(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave + ") = TRIM('"
                        + Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Cuenta_ID].ToString() + "')";
                    // ejecutar consulta
                    Dt_Datos_Subconcepto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                    // validar que la consulta regresó valores y que hay por lo menos un concepto o un subconcepto
                    if (Dt_Datos_Subconcepto == null || Dt_Datos_Subconcepto.Rows.Count <= 0
                        || string.IsNullOrEmpty(Dt_Datos_Subconcepto.Rows[0][Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID].ToString())
                        || string.IsNullOrEmpty(Dt_Datos_Subconcepto.Rows[0][Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID].ToString()))
                    {
                        throw new Exception("No fue posible encontrar el subconcepto_id de la cuenta contable del trámite.");
                    }

                    // dar de alta el pasivo para el pago

                    var Insertar_Pasivo_Ingresos = new Cls_Ope_Ing_Pasivos_Negocio();
                    Insertar_Pasivo_Ingresos.P_Cmmd = Comando;

                    Insertar_Pasivo_Ingresos.P_Referencia = Referencia_Principal;
                    // eliminar pasivos con la misma referencia con estatus POR PAGAR

                    if (Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString() != "")
                        Eliminar_Pasivo(Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString().Trim().ToUpper(),ref Comando);

                    Insertar_Pasivo_Ingresos.P_Descripcion = Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Nombre].ToString();
                    Insertar_Pasivo_Ingresos.P_Fecha_Ingreso = DateTime.Now;
                    Insertar_Pasivo_Ingresos.P_Fecha_Vencimiento = DateTime.Now.AddDays(30);
                    Insertar_Pasivo_Ingresos.P_Estatus = "POR PAGAR";
                    Insertar_Pasivo_Ingresos.P_Cuenta_Predial_ID = "";
                    Insertar_Pasivo_Ingresos.P_Origen = Origen;
                    Insertar_Pasivo_Ingresos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Insertar_Pasivo_Ingresos.P_No_Concepto = Numero_Pasivo;
                    Insertar_Pasivo_Ingresos.P_Contribuyente = Dtr_Datos_Solicitud["NOMBRE_SOLICITANTE"].ToString();
                    Insertar_Pasivo_Ingresos.P_Concepto_Ing_ID = Dt_Datos_Subconcepto.Rows[0][Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID].ToString();
                    Insertar_Pasivo_Ingresos.P_SubConcepto_Ing_ID = Dt_Datos_Subconcepto.Rows[0][Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID].ToString();
                    Insertar_Pasivo_Ingresos.P_Dependencia_ID = Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Dependencia_ID].ToString();
                    Insertar_Pasivo_Ingresos.P_Monto = Convert.ToDecimal(Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Costo_Total].ToString());  //Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Costo].ToString();
                    Filas_Afectadas = Insertar_Pasivo_Ingresos.Alta_Pasivo();
                }
                Dtr_Datos_Solicitud.Close();

                return Suma_Costo_Solicitudes;
            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Generar_Pasivo_Cobro_Adicional
        /// DESCRIPCIÓN: Forma y ejecuta una consulta para insertar un pasivo de solicitud de trámite
        /// PARÁMETROS:
        /// 		1. No_Solicitud: número de solicitud a consultar
        /// 		2. Origen: Texto con la descripción del origen del pasivo
        /// 		3. Cmd: Conexión a la base de datos
        /// CREO: Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO: 26-Nov-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static void Generar_Pasivo_Cobro_Adicional(String Solicitud_ID, string Origen, Double Cargo_Adicional, String Descripcion, OracleCommand Cmd, int Numero_Pasivo, String Referencia_Principal)
        {
            String Mi_SQL;
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
            Boolean Filas_Afectadas = true;
            //Double Suma_Costo_Solicitudes = 0.0;

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

                // consultar detalles de la solicitud
                Mi_SQL = "SELECT " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Clave_Solicitud
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Paterno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Apellido_Materno
                    + " || ' ' || " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Nombre_Solicitante + " NOMBRE_SOLICITANTE"
                    + ", " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*"
                    + ", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Costo_Total
                    + " FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud
                    + " JOIN " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites
                    + " ON " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = "
                    + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID
                    + " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + " ='" + Solicitud_ID + "'";

                Comando.CommandText = Mi_SQL;
                OracleDataReader Dtr_Datos_Solicitud = Comando.ExecuteReader();
                // si hay datos para leer, agregar pasivo
                if (Dtr_Datos_Solicitud.Read())
                {
                    DataTable Dt_Datos_Subconcepto = null;
                    // consultar concepto y subconcepto de la cuenta contable del trámite
                    Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID
                        + "," + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID
                        + "," + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID
                        + " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables
                        + " LEFT OUTER JOIN " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " ON "
                        + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID + " = "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID
                        + " LEFT OUTER JOIN " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + " ON "
                        + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID + " = "
                        + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID
                        + " WHERE TRIM(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave + ") = TRIM('"
                        + Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Cuenta_ID].ToString() + "')";
                    // ejecutar consulta
                    Dt_Datos_Subconcepto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                    // validar que la consulta regresó valores y que hay por lo menos un concepto o un subconcepto
                    if (Dt_Datos_Subconcepto == null || Dt_Datos_Subconcepto.Rows.Count <= 0
                        || string.IsNullOrEmpty(Dt_Datos_Subconcepto.Rows[0][Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID].ToString())
                        || string.IsNullOrEmpty(Dt_Datos_Subconcepto.Rows[0][Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID].ToString()))
                    {
                        throw new Exception("No fue posible encontrar el subconcepto_id de la cuenta contable del trámite.");
                    }

                    // dar de alta el pasivo para el pago
                    var Insertar_Pasivo_Ingresos = new Cls_Ope_Ing_Pasivos_Negocio();
                    
                    Insertar_Pasivo_Ingresos.P_Cmmd = Comando;

                    Insertar_Pasivo_Ingresos.P_Referencia = Referencia_Principal;
                    // eliminar pasivos con la misma referencia con estatus POR PAGAR
                    if (Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString() != "")
                        Eliminar_Pasivo(Dtr_Datos_Solicitud[Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString().Trim().ToUpper(), ref Comando);
                    
                    Insertar_Pasivo_Ingresos.P_Descripcion = Descripcion;
                    Insertar_Pasivo_Ingresos.P_Fecha_Ingreso = DateTime.Now;
                    Insertar_Pasivo_Ingresos.P_Fecha_Vencimiento = DateTime.Now.AddDays(30);
                    Insertar_Pasivo_Ingresos.P_Estatus = "POR PAGAR";
                    Insertar_Pasivo_Ingresos.P_Cuenta_Predial_ID = "";
                    Insertar_Pasivo_Ingresos.P_Origen = Origen;
                    Insertar_Pasivo_Ingresos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Insertar_Pasivo_Ingresos.P_No_Concepto = Numero_Pasivo;
                    Insertar_Pasivo_Ingresos.P_Contribuyente = Dtr_Datos_Solicitud["NOMBRE_SOLICITANTE"].ToString();
                    Insertar_Pasivo_Ingresos.P_Concepto_Ing_ID = Dt_Datos_Subconcepto.Rows[0][Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID].ToString();
                    Insertar_Pasivo_Ingresos.P_SubConcepto_Ing_ID = Dt_Datos_Subconcepto.Rows[0][Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID].ToString();
                    Insertar_Pasivo_Ingresos.P_Dependencia_ID = Dtr_Datos_Solicitud[Cat_Tra_Tramites.Campo_Dependencia_ID].ToString();
                    Insertar_Pasivo_Ingresos.P_Monto = Convert.ToDecimal(Cargo_Adicional.ToString());
                    Filas_Afectadas = Insertar_Pasivo_Ingresos.Alta_Pasivo();

                }
                Dtr_Datos_Solicitud.Close();

            }
            catch (OracleException Ex)
            {
                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {

                Transaccion.Rollback();

                throw new Exception("Alta_Pasivo Error: " + Ex.Message);
            }
            finally
            {
                if (Cmd == null)
                {
                    Conexion.Close();
                }
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
        private static string[] Alta_Poliza(Double Monto_Total_Pagar, String Concepto_Poliza, DataTable Dt_Partidas_Poliza, OracleCommand Comando_Oracle, String Referencia_Tramite)
        {
            Cls_Ope_Con_Polizas_Negocio Polizas = new Cls_Ope_Con_Polizas_Negocio();
            DataTable Dt_Jefe_Dependencia = null;
            try
            {
                Polizas.P_Empleado_ID = Cls_Sessiones.Nombre_Empleado;
                Dt_Jefe_Dependencia = Polizas.Consulta_Empleado_Jefe_Dependencia();
                Polizas = null;

                Polizas = new Cls_Ope_Con_Polizas_Negocio();
                Polizas.P_Tipo_Poliza_ID = "00001";
                Polizas.P_Mes_Ano = DateTime.Today.ToString("MMyy");
                Polizas.P_Fecha_Poliza = DateTime.Today;
                Polizas.P_Concepto = Concepto_Poliza;
                Polizas.P_Total_Debe = Monto_Total_Pagar;
                Polizas.P_Total_Haber = Monto_Total_Pagar;
                Polizas.P_No_Partida = Dt_Partidas_Poliza.Rows.Count;
                Polizas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                Polizas.P_Dt_Detalles_Polizas = Dt_Partidas_Poliza;
                Polizas.P_Empleado_ID_Creo = Cls_Sessiones.Empleado_ID;
                Polizas.P_Empleado_ID_Autorizo = Cls_Sessiones.Empleado_ID;
                Polizas.P_Cmmd = Comando_Oracle;
                string[] Datos_Poliza = Polizas.Alta_Poliza(); 

                Cls_Ope_Con_Poliza_Ingresos_Datos.Alta_Movimientos_Presupuestales(
                   P_Dt_Movimientos_Presupuestales,
                   Comando_Oracle,
                   String.Empty,
                   Datos_Poliza[0],
                   Datos_Poliza[1],
                   Datos_Poliza[2],
                   Ope_Psp_Presupuesto_Ingresos.Campo_Por_Recaudar,
                   Ope_Psp_Presupuesto_Ingresos.Campo_Devengado);

                //Cls_Ope_Con_Poliza_Ingresos_Datos.Alta_Poliza_Ingresos(
                //    Dt_Partidas_Poliza,
                //    Comando_Oracle,
                //    String.Empty,
                //    Datos_Poliza[0],
                //    Datos_Poliza[1],
                //    Datos_Poliza[2],
                //    "DEVENGADO");

                return Datos_Poliza;
            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Poliza " + ex.Message, ex);
            }
        }

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
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Movimientos_Presupuestales
        ///DESCRIPCIÓN          : Metodo que devuelve un DataTable con los campos indicados
        ///PROPIEDADES:     
        ///CREO                 : Antonio Salvador Benavides Guardado
        ///FECHA_CREO           : 19/Septiembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        private static DataTable Crear_Dt_Movimientos_Presupuestales()
        {
            DataTable Dt_Movimientos_Presupuestales = new DataTable();

            //Agrega los campos que va a contener el DataTable
            Dt_Movimientos_Presupuestales.Columns.Add("Fte_Financiamiento_ID", typeof(System.String));
            Dt_Movimientos_Presupuestales.Columns.Add(Ope_Con_Polizas_Detalles.Campo_Proyecto_Programa_ID, typeof(System.String));
            Dt_Movimientos_Presupuestales.Columns.Add(Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID, typeof(System.String));
            Dt_Movimientos_Presupuestales.Columns.Add(Cat_Psp_SubConcepto_Ing.Campo_SubConcepto_Ing_ID, typeof(System.String));
            Dt_Movimientos_Presupuestales.Columns.Add("Anio", typeof(System.String));
            Dt_Movimientos_Presupuestales.Columns.Add("Importe", typeof(System.String));

            return Dt_Movimientos_Presupuestales;
        }
        private static DataTable P_Dt_Movimientos_Presupuestales
        {
            get
            {
                return Dt_Movimientos_Presupuestales;
            }
            set
            {
                Dt_Movimientos_Presupuestales = value.Copy();
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consecutivo_ID
        ///DESCRIPCIÓN          : consulta para obtener el consecutivo de una tabla
        ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
        ///                     2 Tabla: tabla del que se obtendra el consecutivo
        ///                     3 Tamaño: longitud del campo 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static String Consecutivo_ID(String Campo_Id, String Tabla, String Tamaño)
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            object Id; //Obtiene el ID con la cual se guardo los datos en la base de datos

            if (Tamaño.Equals("5"))
            {
                Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '00000')");
                Mi_SQL.Append(" FROM " + Tabla);

                Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "00001";
                }
                else
                {
                    Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Id) + 1);
                }
            }
            else if (Tamaño.Equals("10"))
            {
                Mi_SQL.Append("SELECT NVL(MAX (" + Campo_Id + "), '0000000000')");
                Mi_SQL.Append(" FROM " + Tabla);

                Id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                if (Convert.IsDBNull(Id))
                {
                    Consecutivo = "0000000001";
                }
                else
                {
                    Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Id) + 1);
                }
            }

            return Consecutivo;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Detalles_Solicitud
        ///DESCRIPCIÓN          : Crea un n
        ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
        ///                     2 Tabla: tabla del que se obtendra el consecutivo
        ///                     3 Tamaño: longitud del campo 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 05/Junio/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static void Alta_Detalles_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Solicitud)
        {

            OracleConnection Conexion = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            try
            {
                Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Trans = Conexion.BeginTransaction();
                Cmd.Connection = Conexion;
                Cmd.Transaction = Trans;
                StringBuilder Mi_Sql = new StringBuilder();
                String Elemento_ID = Consecutivo_ID(Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID, Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud, "10");

                Mi_Sql.Append("INSERT INTO " + Ope_Tra_Det_Solicitud.Tabla_Ope_Tra_Det_Solicitud + "(");
                Mi_Sql.Append(Ope_Tra_Det_Solicitud.Campo_Detalle_Solicitud_ID);
                Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Solicitud_ID);
                Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Subproceso_ID);
                Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Estatus);
                Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Comentarios);
                Mi_Sql.Append(", " + Ope_Tra_Det_Solicitud.Campo_Fecha);

                Mi_Sql.Append(") VALUES( ");

                Mi_Sql.Append("'" + Elemento_ID + "' ");
                Mi_Sql.Append(", '" + Solicitud.P_Solicitud_ID + "' ");
                Mi_Sql.Append(", '" + Solicitud.P_Subproceso_ID + "' ");
                Mi_Sql.Append(", '" + Solicitud.P_Estatus + "' ");
                Mi_Sql.Append(", '" + Solicitud.P_Comentarios + "' ");
                Mi_Sql.Append(", SYSDATE ");
                Mi_Sql.Append(")");
                Cmd.CommandText = Mi_Sql.ToString();
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                String Mensaje = "Error: ";
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "No existe un registro relacionado con esta operacion";
                        break;
                    case "923":
                        Mensaje = "Consulta SQL";
                        break;
                    case "12170":
                        Mensaje = "Conexion con el Servidor";
                        break;
                    default:
                        Mensaje = Ex.Message;
                        break;
                }
                throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
            }
            catch (DBConcurrencyException Ex)
            {
                if (Cmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Conexion != null)
                {
                    Conexion.Close();
                }
            }

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Datos_Dictamen
        ///DESCRIPCIÓN          : guardara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 20/Agosto/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Alta_Datos_Dictamen(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
                int Columnas = Negocio.P_Datos.Length / 2;

                for (int Contador_For = 0; Contador_For < Columnas; Contador_For++)
                {
                    Mi_SQL = new StringBuilder();

                    Mi_SQL.Append("INSERT INTO " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "(");
                    Mi_SQL.Append(Ope_Tra_Datos.Campo_Ope_Dato_ID);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Tramite_ID);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Solicitud_ID);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Dato_ID);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Valor);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Usuario_Creo);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Fecha_Creo);
                    Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Tipo_Dato);

                    Mi_SQL.Append(") VALUES (");

                    Mi_SQL.Append("'" + Obtener_Id_Consecutivo(Ope_Tra_Datos.Campo_Ope_Dato_ID, Ope_Tra_Datos.Tabla_Ope_Tra_Datos) + "'");
                    Mi_SQL.Append(", '" + Negocio.P_Tramite_id + "'");
                    Mi_SQL.Append(", '" + Negocio.P_Solicitud_ID + "'");
                    Mi_SQL.Append(", '" + Negocio.P_Datos[Contador_For, 0] + "'");
                    Mi_SQL.Append(", '" + Negocio.P_Datos[Contador_For, 1] + "'");
                    Mi_SQL.Append(", '" + Negocio.P_Usuario + "'");
                    Mi_SQL.Append(", SYSDATE");
                    Mi_SQL.Append(", 'FINAL'");
                    Mi_SQL.Append(")");

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                }

                Trans.Commit();
                Operacion_Completa = true;
            }

            catch (OracleException Ex)
            {
                Trans.Rollback();
            }

            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Datos_Dictamen
        ///DESCRIPCIÓN          : modificara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 20/Agosto/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Modificar_Datos_Dictamen(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            OracleConnection Conexion = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();
            Trans = Conexion.BeginTransaction();
            Cmd.Connection = Conexion;
            Cmd.Transaction = Trans;

            try
            {
                if (Negocio.P_Dt_Datos != null && Negocio.P_Dt_Datos.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Row in Negocio.P_Dt_Datos.Rows)
                    {
                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("UPDATE " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " SET ");
                        Mi_SQL.Append(Ope_Tra_Datos.Campo_Valor + " = '" + Dr_Row["VALOR"].ToString());
                        Mi_SQL.Append("', " + Ope_Tra_Datos.Campo_Usuario_Modifico + " = '" + Negocio.P_Usuario + "' ");
                        Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Fecha_Modifico + " = SYSDATE");
                        Mi_SQL.Append(" WHERE " + Ope_Tra_Datos.Campo_Ope_Dato_ID + " = '" + Dr_Row["OPE_DATO_ID"].ToString() + "'");
                        Cmd.CommandText = Mi_SQL.ToString();
                        Cmd.ExecuteNonQuery();
                    }
                }

                Trans.Commit();
                Operacion_Completa = true;
            }

            catch (OracleException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                String Mensaje = "Error: ";
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "No existe un registro relacionado con esta operacion";
                        break;
                    case "923":
                        Mensaje = "Consulta SQL";
                        break;
                    case "12170":
                        Mensaje = "Conexion con el Servidor";
                        break;
                    default:
                        Mensaje = Ex.Message;
                        break;
                }
                throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
            }
            catch (DBConcurrencyException Ex)
            {
                if (Cmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Conexion != null)
                {
                    Conexion.Close();
                }
            }

            return Operacion_Completa;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Modificar_Costo_Solicitud
        ///DESCRIPCIÓN          : modificara el registro
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 12/Noviembre/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Modificar_Costo_Solicitud(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            OracleConnection Conexion = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
            Conexion.Open();
            Trans = Conexion.BeginTransaction();
            Cmd.Connection = Conexion;
            Cmd.Transaction = Trans;

            try
            {
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Campo_Costo_Total + "='" + Negocio.P_Costo_Total + "'");
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Costo_Base + "='" + Negocio.P_Costo_Total + "'");
                Mi_SQL.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                Cmd.CommandText = Mi_SQL.ToString();
                Cmd.ExecuteNonQuery();

                Trans.Commit();
                Operacion_Completa = true;
            }

            catch (OracleException Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                String Mensaje = "Error: ";
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "No existe un registro relacionado con esta operacion";
                        break;
                    case "923":
                        Mensaje = "Consulta SQL";
                        break;
                    case "12170":
                        Mensaje = "Conexion con el Servidor";
                        break;
                    default:
                        Mensaje = Ex.Message;
                        break;
                }
                throw new Exception(Mensaje + "[" + Ex.ToString() + "]");
            }
            catch (DBConcurrencyException Ex)
            {
                if (Cmd == null)
                {
                    Trans.Rollback();
                }
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Conexion != null)
                {
                    Conexion.Close();
                }
            }

            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Id_Consecutivo
        ///DESCRIPCIÓN: crea una sentencia sql para insertar un Solicitud en la base de datos
        ///PARAMETROS: 1.-Campo_ID, nombre del campo de la tabla al cual se quiere sacar el ultimo valor
        ///            2.-Tabla, nombre de la tabla que se va a consultar
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 23/Agosto/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_Id_Consecutivo(String Campo_ID, String Tabla)
        {
            String Consecutivo = "";
            String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL = "SELECT NVL(MAX (" + Campo_ID + "),'0000000000') ";
            Mi_SQL = Mi_SQL + "FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Convert.IsDBNull(Obj))
            {
                Consecutivo = "0000000001";
            }
            else
            {
                Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Obj) + 1);
            }
            return Consecutivo;
        }
        #region Consultas
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Valor_Subproceso_ID
        /// COMENTARIOS:    consultara el valor del subproceso que se esta realizando
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     05/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Valor_Subproceso_ID(Cls_Ope_Bandeja_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT *");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" Where ");

                if (!String.IsNullOrEmpty(Datos.P_Subproceso_ID)) 
                    Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID + "='" + Datos.P_Subproceso_ID + "'");

                if (!String.IsNullOrEmpty(Datos.P_Orden))
                {
                    Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden + "='" + Datos.P_Orden + "'");
                    Mi_SQL.Append(" and " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Tramite_ID+ "='" + Datos.P_Tramite_id + "'");
                }
                
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        /// *******************************************************************************
        /// NOMBRE          : Consultar_Costo_Complemento
        /// COMENTARIOS     : Metodo que consultara la informacion del costo de las solicitudes complemento
        /// PROPIEDADES     : Negocio.- Valor de los campos a consultar 
        /// DESCRIPCIÓN     : 
        /// CREO            : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO      : 10/Noviembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Costo_Complemento(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("Select SUM(" + Ope_Tra_Solicitud.Campo_Costo_Total + ") as Costo_Complemento ");
                Mi_SQL.Append(" From " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);
                Mi_SQL.Append(" Where " + Ope_Tra_Solicitud.Campo_Complemento + "='" + Negocio.P_Solicitud_ID + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE          : Consultar_Costo_Principal
        /// COMENTARIOS     : Metodo que consultara la informacion del costo de la solicitud principal
        /// PROPIEDADES     : Negocio.- Valor de los campos a consultar 
        /// DESCRIPCIÓN     : 
        /// CREO            : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO      : 10/Noviembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Costo_Principal(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("Select SUM(" + Ope_Tra_Solicitud.Campo_Costo_Total + ") as Costo_Padre ");
                Mi_SQL.Append(" From " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);
                Mi_SQL.Append(" Where " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Solicitudes_Hija
        /// COMENTARIOS:    Metodo que consultara la informacion de los detalles de las solicitudes hijas
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     06/Noviembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Solicitudes_Hija(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * From " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);
                Mi_SQL.Append(" Where " + Ope_Tra_Solicitud.Campo_Complemento + "='" + Negocio.P_Solicitud_ID + "'");
             
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Datos_Contribuyente
        /// COMENTARIOS:    Metodo que consultara la informacion del contribuyente
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Septiembre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Datos_Contribuyente(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT *");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "='" + Negocio.P_Contribuyente_Id + "'");

              
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Anterior_Actividades_Condicional
        /// COMENTARIOS:    consultara el valor del subproceso que se esta realizando
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     31/Agosto/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Anterior_Actividades_Condicional(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            try
            {
                String Mi_SQL = "Select * from " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                Mi_SQL += " Where (" + Cat_Tra_Subprocesos.Campo_Condicion_Si + "='" + Negocio.P_Condicion_Si + "'";
                Mi_SQL += " or " + Cat_Tra_Subprocesos.Campo_Condicion_No + "='" + Negocio.P_Condicion_Si + "') ";
                Mi_SQL += " and (" + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Negocio.P_Tramite_id + "')";
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Tipo_Actividad
        /// COMENTARIOS:    consultara el tipo de actividad del subproceso que se esta realizando
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     31/Agosto/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Tipo_Actividad(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            try
            {
                String Mi_SQL = "Select * from " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                Mi_SQL += " Where " + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Negocio.P_Tramite_id + "'";
                Mi_SQL += " And " + Cat_Tra_Subprocesos.Campo_Orden + "='" + Negocio.P_Orden + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Datos_Finales
        /// COMENTARIOS:    consultara el valor de los datos finales
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Agosto/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Datos_Finales(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT *");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Tra_Datos.Tabla_Ope_Tra_Datos);
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                Mi_SQL.Append(" And " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Tipo_Dato + "='FINAL'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Penalizaciones
        /// COMENTARIOS:    consultara el valor de los datos finales
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Agosto/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static String Consultar_Penalizaciones(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            DataTable Dt_Consulta_Notificacion = new DataTable();
            DataTable Dt_Consulta_Avance = new DataTable();
            String Porcentaje = "";
            try
            {
                //  se consulta alguna notificacion
                Mi_SQL.Append("SELECT *");
                Mi_SQL.Append(" FROM " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana);
                Mi_SQL.Append(" Where " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion + " is not null ");
                Mi_SQL.Append(" And " + Ope_Ort_Formato_Admon_Urbana.Campo_Inspeccion_Notificacion + "='SI'");
                Mi_SQL.Append(" And " + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                Dt_Consulta_Notificacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                //  se consulta algun avance de obra
                Mi_SQL = new StringBuilder();
                Mi_SQL.Append("SELECT *");
                Mi_SQL.Append(" FROM " + Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana);
                Mi_SQL.Append(" Where " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID + " is not null ");
                Mi_SQL.Append(" And " + Ope_Ort_Formato_Admon_Urbana.Campo_Avance_Obra_ID + "!= '00000'");
                Mi_SQL.Append(" And " + Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                Dt_Consulta_Avance = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];


                if (Dt_Consulta_Notificacion != null && Dt_Consulta_Notificacion.Rows.Count > 0)
                {
                    Porcentaje = "50";
                }

                else if (Dt_Consulta_Avance != null && Dt_Consulta_Avance.Rows.Count > 0)
                {
                    Porcentaje = "30";
                }
                else
                {
                    Porcentaje = "";
                }

                return Porcentaje;
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Datos_Finales_Operacion
        /// COMENTARIOS:    consultara el valor de los datos finales
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Agosto/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Datos_Finales_Operacion(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            String Mi_SQL = ""; //Variable para las consultas
            try
            {
                Mi_SQL = "SELECT " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Ope_Dato_ID + " AS OPE_DATO_ID";
                Mi_SQL += ", " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Dato_ID + " AS DATO_ID";
                Mi_SQL += ", REPLACE(" + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Nombre + ",' ', '_') AS NOMBRE_DATO";
                Mi_SQL += ", " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Valor + " AS VALOR";
                Mi_SQL += " FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + ", " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite;
                Mi_SQL += " WHERE " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Dato_ID;
                Mi_SQL += " = " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + "." + Cat_Tra_Datos_Tramite.Campo_Dato_ID;
                Mi_SQL += " AND " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Solicitud_ID;
                Mi_SQL += " = '" + Negocio.P_Solicitud_ID + "'";
                Mi_SQL += " AND " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + "." + Ope_Tra_Datos.Campo_Tipo_Dato;

                if (!String.IsNullOrEmpty(Negocio.P_Tipo_Dato))
                {
                    if (Negocio.P_Tipo_Dato == "INICIAL")
                    {
                        Mi_SQL += " = 'INICIAL'";
                    }
                }
                else
                {
                    Mi_SQL += " = 'FINAL'";
                }
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Costo_Matriz
        /// COMENTARIOS:    consultara el valor de la matriz de costo
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     15/Octubre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Costo_Matriz(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            String Mi_SQL = ""; //Variable para las consultas
            try
            {
                Mi_SQL = "SELECT " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + ".*";
                Mi_SQL += " FROM " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo;
                Mi_SQL += " WHERE " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + "." + Ope_Tra_Matriz_Costo.Campo_Tramite_ID;
                Mi_SQL += " = '" + Negocio.P_Tramite_id + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Detalles_Plantillas
        /// COMENTARIOS:    Metodo que consultara la informacion de los detalles de las plantillas
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     28/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Detalles_Plantillas(Cls_Ope_Bandeja_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Plantilla_ID);
                Mi_SQL.Append(", " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Archivo);

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas);

                Mi_SQL.Append(" left outer join " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + " on ");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Plantilla_ID + "=");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + "='" + Datos.P_Subproceso_ID + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Detalles_Formatos
        /// COMENTARIOS:    Metodo que consultara la informacion de los detalles de las formatos
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     28/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Detalles_Formatos(Cls_Ope_Bandeja_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Formato_ID);
                Mi_SQL.Append(", " + Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Archivo);

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido);

                Mi_SQL.Append(" left outer join " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + " on ");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Formato_ID + "=");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + "='" + Datos.P_Subproceso_ID + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Tiempo_Estimado
        /// COMENTARIOS:    Metodo que consultara el tiempo estimado del proceso de la solicitud
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     25/Junio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Tiempo_Estimado(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tiempo_Estimado);
                Mi_SQL.Append(", " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Fecha_Creo);

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);

                Mi_SQL.Append(" left outer join " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " on ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Tramite_ID + "=");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + "." + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Actividades_Realizadas
        /// COMENTARIOS:    Metodo que consultara la informacion de los detalles de las formatos
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     20/Jinio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Actividades_Realizadas(Cls_Ope_Bandeja_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Datos.P_Tramite_id + "'");
                Mi_SQL.Append(" ORDER BY " + Cat_Tra_Subprocesos.Campo_Orden);

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Actividad_Condicional
        /// COMENTARIOS:    Metodo que consultara la informacion de la actividad condicional
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     27/Julio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Actividad_Condicional(Cls_Ope_Bandeja_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Datos.P_Tramite_id + "'");
                Mi_SQL.Append(" and (" + Cat_Tra_Subprocesos.Campo_Orden + "='" + Datos.P_Condicion_Si + "'");
                Mi_SQL.Append(" or " + Cat_Tra_Subprocesos.Campo_Orden + "='" + Datos.P_Condicion_No + "' )");
                Mi_SQL.Append(" ORDER BY " + Cat_Tra_Subprocesos.Campo_Orden);

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Datos_Cedula_Visita
        /// COMENTARIOS:    Metodo que consultara la informacion de la cedula de visita referente a una solicitud
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     08/Octubre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Datos_Cedula_Visita(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Tabla_Ope_Ort_Formato_Admon_Urbana);
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Ort_Formato_Admon_Urbana.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID+ "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Datos_Ficha_Revision
        /// COMENTARIOS:    Metodo que consultara la informacion de la cedula de visita referente a una solicitud
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     09/Octubre/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Datos_Ficha_Revision(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT * ");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Ort_Ficha_Revision.Tabla_Ope_Ort_Ficha_Revision);
                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Ort_Ficha_Revision.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Siguiente_Actividad
        /// COMENTARIOS:    Metodo que consultara la informacion de la siguiente actividad
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     15/Agosto/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Siguiente_Actividad(Cls_Ope_Bandeja_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + ".* ");
                Mi_SQL.Append("," + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Nombre + " as NOMBRE_PERFIL");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + " ON ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID + "=");
                Mi_SQL.Append(Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + "." + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID);

                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + " ON ");
                Mi_SQL.Append(Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles + "." + Tra_Subprocesos_Perfiles.Campo_Perfil_ID + "=");
                Mi_SQL.Append(Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Perfil_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Datos.P_Tramite_id + "'");
                Mi_SQL.Append(" and (to_number(" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden + ") > to_number('" + Datos.P_Orden_Actividad + "')" + ")");
                Mi_SQL.Append(" ORDER BY " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }


        /// *******************************************************************************
        /// NOMBRE:         Consultar_Comentarios_Internos
        /// COMENTARIOS:    consultara los comentarios internos
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     24/Julio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Comentarios_Internos(Cls_Ope_Bandeja_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Estatus);
                Mi_SQL.Append("," + Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Comentarios);
                Mi_SQL.Append("," + Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Usuario_Creo);
                Mi_SQL.Append("," + Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Fecha);
                Mi_SQL.Append("," + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Nombre + " as Actividad ");
                Mi_SQL.Append(", to_number(" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Valor + ") as Valor");

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna);

                Mi_SQL.Append(" left outer join " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " on ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Subproceso_ID);
                Mi_SQL.Append("=");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_SQL.Append(" Where ");
                Mi_SQL.Append(Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");


                Mi_SQL.Append(" Order by " + Ope_Tra_Det_Solicitud_Interna.Tabla_Ope_Tra_Det_Solicitud_Interna + "." + Ope_Tra_Det_Solicitud_Interna.Campo_Fecha + " desc");
                //  se ejecuta la consulta
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (OracleException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (DBConcurrencyException ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.Message);
            }
            finally
            {
            }
        }
        #endregion
    }

}