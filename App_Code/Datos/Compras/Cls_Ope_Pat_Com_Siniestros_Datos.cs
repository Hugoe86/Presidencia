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
using Presidencia.Control_Patrimonial_Operacion_Siniestros.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Siniestros_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Siniestros.Datos
{

    public class Cls_Ope_Pat_Com_Siniestros_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Siniestro
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Siniestro.
        ///PARAMETROS           : 
        ///                     1.  Siniestro. Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 17/Enero/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Siniestros_Negocio Alta_Siniestro(Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Siniestro_ID = Obtener_ID_Consecutivo(Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros, Ope_Pat_Siniestros.Campo_Siniestro_ID, 10);
                String Mi_SQL = "INSERT INTO " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Siniestros.Campo_Siniestro_ID + ", " + Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Bien_ID + ", " + Ope_Pat_Siniestros.Campo_Aseguradora_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Tipo_Bien + ", " + Ope_Pat_Siniestros.Campo_Descripcion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Fecha + ", " + Ope_Pat_Siniestros.Campo_Parte_Averiguacion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Reparacion + ", " + Ope_Pat_Siniestros.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Responsable_Municipio + ", " + Ope_Pat_Siniestros.Campo_Numero_Reporte;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Consignado + ", " + Ope_Pat_Siniestros.Campo_Pago_Danio_Sindicos;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Usuario_Creo + ", " + Ope_Pat_Siniestros.Campo_Fecha_Creo + ")";
                Mi_SQL = Mi_SQL + " VALUES ('" + Siniestro_ID + "', '" + Siniestro.P_Tipo_Siniestro_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Siniestro.P_Bien_ID + "','" + Siniestro.P_Aseguradora_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Siniestro.P_Tipo_Bien + "','" + Siniestro.P_Descripcion + "'";
                Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Siniestro.P_Fecha) + "','" + Siniestro.P_Parte_Averiguacion + "'";
                Mi_SQL = Mi_SQL + ", '" + Siniestro.P_Reparacion + "', '" + Siniestro.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ", '" + ((Siniestro.P_Responsable_Municipio) ? "SI" : "NO") + "', '" + Siniestro.P_No_Reporte + "'";
                Mi_SQL = Mi_SQL + ", '" + ((Siniestro.P_Consignado) ? "SI" : "NO") + "', '" + ((Siniestro.P_Pago_Danos_Sindicos) ? "SI" : "NO") + "'";
                Mi_SQL = Mi_SQL + ", '" + Siniestro.P_Usuario_Nombre + "', SYSDATE)";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Siniestro.P_Observacion != null && Siniestro.P_Observacion.Trim().Length > 0) {
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones, Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones;
                    Mi_SQL = Mi_SQL + " (" + Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID + ", " + Ope_Pat_Sinies_Observaciones.Campo_Siniestro_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Observacion + ", " + Ope_Pat_Sinies_Observaciones.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Usuario_Creo + ", " + Ope_Pat_Sinies_Observaciones.Campo_Fecha_Creo + ")";
                    Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Siniestro_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Siniestro.P_Observacion + "', SYSDATE";
                    Mi_SQL = Mi_SQL + ",'" + Siniestro.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                Siniestro.P_Siniestro_ID = Siniestro_ID;
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar dar de Alta el Siniestro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Siniestro;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.  Siniestro. Contiene los parametros que se van a utilizar para
        ///                                     hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 17/Enero/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro) {
            String Mi_SQL = null;
            DataSet Ds_Siniestros = null;
            DataTable Dt_Siniestros = new DataTable();
            try {
                if (Siniestro.P_Tipo_DataTable.Equals("TIPOS_SINIESTROS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Tipos_Siniestros.Campo_Tipo_Siniestro_ID + " AS SINIESTRO_ID, " + Cat_Pat_Tipos_Siniestros.Campo_Descripcion + " AS DESCRIPCION ";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros + " ORDER BY " + Cat_Pat_Tipos_Siniestros.Campo_Descripcion;
                } else if (Siniestro.P_Tipo_DataTable.Equals("SINIESTROS")) {
                    Mi_SQL = "SELECT " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Siniestro_ID + " AS SINIESTRO_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros + "." + Cat_Pat_Tipos_Siniestros.Campo_Descripcion + " AS TIPO_SINIESTRO";
                    Mi_SQL = Mi_SQL + ", '['||''||" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Numero_Inventario + "";
                    Mi_SQL = Mi_SQL + "||''||']'||' '||" + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Nombre + " AS BIEN_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + ", " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros + ", " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipos_Siniestros.Tabla_Cat_Pat_Tipos_Siniestros + "." + Cat_Pat_Tipos_Siniestros.Campo_Tipo_Siniestro_ID + "";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos + "." + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Bien_ID + "";
                    if (Siniestro.P_Estatus != null && Siniestro.P_Estatus.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Estatus + "";
                        Mi_SQL = Mi_SQL + " = '" + Siniestro.P_Estatus + "'";
                    }
                    if (Siniestro.P_Buscar_Fecha) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Campo_Fecha + " >= '" + String.Format("{0:dd/MM/yyyy}", Siniestro.P_Fecha) + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Campo_Fecha + " < '" + String.Format("{0:dd/MM/yyyy}", (Siniestro.P_Fecha).AddDays(1).Date) + "'";
                    }
                    if (Siniestro.P_Aseguradora_ID != null && Siniestro.P_Aseguradora_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Aseguradora_ID + "";
                        Mi_SQL = Mi_SQL + " = '" + Siniestro.P_Aseguradora_ID + "'";
                    }
                    if (Siniestro.P_Descripcion != null && Siniestro.P_Descripcion.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Descripcion + "";
                        Mi_SQL = Mi_SQL + " LIKE '%" + Siniestro.P_Descripcion + "%'";
                    }
                    if (Siniestro.P_Numero_Inventario > (-1)) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Bien_ID + "";
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " = " + Siniestro.P_Numero_Inventario + ")";
                    }
                    if (Siniestro.P_Clave_Sistema > (-1)) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Siniestro_ID + "";
                        Mi_SQL = Mi_SQL + " LIKE '%" + Siniestro.P_Clave_Sistema.ToString() +"%'";
                    }
                    if (Siniestro.P_Dependencia != null && Siniestro.P_Dependencia.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "." + Ope_Pat_Siniestros.Campo_Bien_ID + "";
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " = " + Siniestro.P_Dependencia + ")";
                    }
                } 
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                    Ds_Siniestros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Siniestros != null) {
                    Dt_Siniestros = Ds_Siniestros.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Siniestros;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Siniestro
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Siniestro.
        ///PARAMETROS           :     
        ///                     1. Siniestro.  Contiene los parametros para actualizar el 
        ///                                     registro en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO           : 17/Enero/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static void Modificar_Siniestro(Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                String Mi_SQL = "UPDATE " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros;
                if (Siniestro.P_Tipo_Actualizacion.Trim().Equals("NORMAL")) { 
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID + " = '" + Siniestro.P_Tipo_Siniestro_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Descripcion + " = '" + Siniestro.P_Descripcion + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Fecha + " = '" + String.Format("{0:dd/MM/yyyy}", Siniestro.P_Fecha) + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Parte_Averiguacion + " = '" + Siniestro.P_Parte_Averiguacion + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Reparacion + " = '" + Siniestro.P_Reparacion + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Responsable_Municipio + " = '" + ((Siniestro.P_Responsable_Municipio) ? "SI" : "NO") + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Consignado + " = '" + ((Siniestro.P_Consignado) ? "SI" : "NO") + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Pago_Danio_Sindicos + " = '" + ((Siniestro.P_Pago_Danos_Sindicos) ? "SI" : "NO") + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Numero_Reporte + " = '" + Siniestro.P_No_Reporte + "'";
                } else {
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Siniestros.Campo_Estatus + " = '" + Siniestro.P_Estatus + "'";
                }
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Usuario_Modifico + " = '" + Siniestro.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Siniestros.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Siniestros.Campo_Siniestro_ID + " = '" + Siniestro.P_Siniestro_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                if (Siniestro.P_Observacion != null && Siniestro.P_Observacion.Trim().Length > 0) {
                    String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones, Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones;
                    Mi_SQL = Mi_SQL + " (" + Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID + ", " + Ope_Pat_Sinies_Observaciones.Campo_Siniestro_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Observacion + ", " + Ope_Pat_Sinies_Observaciones.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Usuario_Creo + ", " + Ope_Pat_Sinies_Observaciones.Campo_Fecha_Creo + ")";
                    Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Siniestro.P_Siniestro_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Siniestro.P_Observacion + "', SYSDATE";
                    Mi_SQL = Mi_SQL + ",'" + Siniestro.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]"; 
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]"; 
                    }
                } else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]"; 
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]"; 
                } else {
                    Mensaje = "Error al intentar Modificar el Siniestro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }
                
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Siniestro
        ///DESCRIPCIÓN          : Obtiene los Datos a Detalle de un Siniestro en Especifico.
        ///PARAMETROS           :   
        ///                     1. Parametros.   Siniestro que se va a ver a Detalle.
        ///CREO                 : Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO           : 17/Enero/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Siniestros_Negocio Consultar_Datos_Siniestro(Cls_Ope_Pat_Com_Siniestros_Negocio Parametros) {
            String Mi_SQL = "SELECT * FROM " + Ope_Pat_Siniestros.Tabla_Ope_Pat_Siniestros + "";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Siniestros.Campo_Siniestro_ID + " = '" + Parametros.P_Siniestro_ID + "'";
            Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro = new Cls_Ope_Pat_Com_Siniestros_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()){
                    Siniestro.P_Siniestro_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Siniestro_ID].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Siniestro_ID].ToString() : "";
                    Siniestro.P_Tipo_Siniestro_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Tipo_Siniestro_ID].ToString() : "";
                    Siniestro.P_Bien_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Bien_ID].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Bien_ID].ToString() : "";
                    Siniestro.P_Aseguradora_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Aseguradora_ID].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Aseguradora_ID].ToString() : "";
                    Siniestro.P_Tipo_Bien = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Tipo_Bien].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Tipo_Bien].ToString() : "";
                    Siniestro.P_Descripcion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Descripcion].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Descripcion].ToString() : "";
                    Siniestro.P_Fecha = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Fecha].ToString())) ? (DateTime)Data_Reader[Ope_Pat_Siniestros.Campo_Fecha] : new DateTime();
                    Siniestro.P_Parte_Averiguacion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Parte_Averiguacion].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Parte_Averiguacion].ToString() : "";
                    Siniestro.P_Reparacion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Reparacion].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Reparacion].ToString() : "";
                    Siniestro.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Estatus].ToString() : "";
                    Siniestro.P_Responsable_Municipio = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Responsable_Municipio].ToString())) ? (Data_Reader[Ope_Pat_Siniestros.Campo_Responsable_Municipio].ToString().Equals("SI") ? true : false) : false;
                    Siniestro.P_No_Reporte = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Numero_Reporte].ToString())) ? Data_Reader[Ope_Pat_Siniestros.Campo_Numero_Reporte].ToString() : "";
                    Siniestro.P_Consignado = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Consignado].ToString())) ? (Data_Reader[Ope_Pat_Siniestros.Campo_Consignado].ToString().Equals("SI") ? true : false) : false;
                    Siniestro.P_Pago_Danos_Sindicos = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Siniestros.Campo_Pago_Danio_Sindicos].ToString())) ? (Data_Reader[Ope_Pat_Siniestros.Campo_Pago_Danio_Sindicos].ToString().Equals("SI") ? true : false) : false;
                }
                Data_Reader.Close();
                DataSet Ds_Siniestro = null;
                if(Siniestro.P_Siniestro_ID != null && Siniestro.P_Siniestro_ID.Trim().Length > 0){
                    Mi_SQL = "SELECT " + Ope_Pat_Sinies_Observaciones.Campo_Observacion_ID + " AS OBSERVACION_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Usuario_Creo + " AS AUTOR_OBSERVACION";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Sinies_Observaciones.Campo_Observacion + " AS OBSERVACION";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Sinies_Observaciones.Tabla_Ope_Pat_Sinies_Observaciones;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Sinies_Observaciones.Campo_Siniestro_ID;
                    Mi_SQL = Mi_SQL + " = '" + Siniestro.P_Siniestro_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Sinies_Observaciones.Campo_Fecha + " DESC";
                    Ds_Siniestro = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Siniestro == null){
                    Siniestro.P_Dt_Observaciones = new DataTable();
                }else{
                    Siniestro.P_Dt_Observaciones = Ds_Siniestro.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar los datos del Siniestro. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Siniestro;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID) {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
            try {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            } catch (OracleException Ex) {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    }

}