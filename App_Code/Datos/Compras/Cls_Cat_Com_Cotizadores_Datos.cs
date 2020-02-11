using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;
using Presidencia.Cotizadores.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Text;
///malo compu///
namespace Presidencia.Cotizadores.Datos
{
    public class Cls_Cat_Com_Cotizadores_Datos
    {
        
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Nombre_Giros
        ///DESCRIPCIÓN: Busca un elemento dentro del grid view de acuerdo al nombre de la colonia
        ///PARAMETROS: 
        ///CREO: Jacqueline Ramìrez Sierra
        ///FECHA_CREO: 11/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Cotizadores(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            String Mi_SQL = "SELECT EMP." + Cat_Empleados.Campo_No_Empleado +
                            ", COT." + Cat_Com_Cotizadores.Campo_Empleado_ID +
                            ", COT." + Cat_Com_Cotizadores.Campo_Nombre_Completo +
                            ", COT." + Cat_Com_Cotizadores.Campo_Correo +
                            " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores + " COT" +
                            " JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " EMP" +
                            " ON EMP." + Cat_Empleados.Campo_Empleado_ID +
                            "= COT." + Cat_Com_Cotizadores.Campo_Empleado_ID;
                           
            //Filtro por nombre de Empleado
            if (Cotizadores.P_Nombre_Completo != null)
            {
                Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Com_Cotizadores.Campo_Nombre_Completo + ") LIKE UPPER('%" + Cotizadores.P_Nombre_Completo + "%')";
            }

            if (Cotizadores.P_Empleado_ID != null)
            {
                Mi_SQL = "SELECT " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores + ".* ";
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores + "." + Cat_Com_Cotizadores.Campo_Empleado_ID + ") AS " + Cat_Empleados.Campo_No_Empleado; 
                Mi_SQL = Mi_SQL + ", (SELECT " + Cat_Empleados.Campo_Nombre;
                Mi_SQL = Mi_SQL + "||' '|| " + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "=";
                Mi_SQL = Mi_SQL + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores + "." + Cat_Com_Cotizadores.Campo_Empleado_ID + ") AS COTIZADOR"; 
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Cotizadores.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_Empleado_ID.Trim() + "'";
            }

            if (Cotizadores.P_No_Empleado != null)
            {
                Mi_SQL = "SELECT * FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores ;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Cotizadores.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_No_Empleado.Trim() + "')";
                    
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Cotizadores.Campo_Nombre_Completo;

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

        }

     

        public static DataTable Consultar_Nombre_Empleado(Cls_Cat_Com_Cotizadores_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            ", " + Cat_Empleados.Campo_Nombre +
                            " || ' ' || " + Cat_Empleados.Campo_Apellido_Paterno +
                            " || ' ' || " + Cat_Empleados.Campo_Apellido_Materno + " AS NOMBRE" +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado +
                            " ='" + Clase_Negocio.P_No_Empleado.ToString() + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        public static void Alta_Cotizadores(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            try
            {

            String Mi_SQL = "INSERT INTO " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores +
                            " (" + Cat_Com_Cotizadores.Campo_Empleado_ID + ", " +
                            Cat_Com_Cotizadores.Campo_Nombre_Completo + ", " +
                            Cat_Com_Cotizadores.Campo_Correo + ", " +
                            Cat_Com_Cotizadores.Campo_Password_Correo + ", " +
                            Cat_Com_Cotizadores.Campo_IP_Correo_Saliente + ", " + 
                            Cat_Com_Cotizadores.Campo_Usuario_Creo + ", " +
                            Cat_Com_Cotizadores.Campo_Fecha_Creo +
                            ") VALUES( (SELECT " + Cat_Empleados.Campo_Empleado_ID +
                            " FROM " + Cat_Empleados.Tabla_Cat_Empleados +
                            " WHERE " + Cat_Empleados.Campo_No_Empleado + 
                            "='" + Cotizadores.P_No_Empleado.Trim()+ "')" +
                            ",'" + Cotizadores.P_Nombre_Completo +
                            "','" + Cotizadores.P_Correo +
                            "','" + Cotizadores.P_Password +
                            "','" + Cotizadores.P_Direccion_IP +
                            "','" + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";

            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

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

        public static void Alta_Detalle_Cotizador(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            String Mi_SQL = "";
            try{

            for (int i = 0; i < Cotizadores.P_Dt_Giros.Rows.Count; i++)
            {
                Mi_SQL = "INSERT INTO " + Cat_Com_Det_Cotizadores.Tabla_Cat_Com_Det_Cotizadores +
                                    " (" + Cat_Com_Det_Cotizadores.Campo_Empleado_ID +
                                    ", " + Cat_Com_Det_Cotizadores.Campo_Giro_ID +
                                    ", " + Cat_Com_Cotizadores.Campo_Usuario_Creo +
                                    ", " + Cat_Com_Cotizadores.Campo_Fecha_Creo +
                                    ") VALUES('" + Cotizadores.P_Empleado_ID +
                                    "','" + Cotizadores.P_Dt_Giros.Rows[i]["Giro_ID"] +
                                    "','" + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
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

        public static DataTable Consultar_Detalle_Cotizador(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Det_Cotizadores.Campo_Giro_ID +
                            ", (SELECT " + Cat_Sap_Concepto.Campo_Clave +
                            " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                            " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID +
                            "=DET." + Cat_Com_Det_Cotizadores.Campo_Giro_ID + ")" + " AS CLAVE " +
                            ", (SELECT " + Cat_Sap_Concepto.Campo_Descripcion +
                            " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +
                            " WHERE " + Cat_Sap_Concepto.Campo_Concepto_ID +
                            "=DET." + Cat_Com_Det_Cotizadores.Campo_Giro_ID + ")" + " AS DESCRIPCION " +
                            " FROM " + Cat_Com_Det_Cotizadores.Tabla_Cat_Com_Det_Cotizadores + " DET " +
                            " WHERE " + Cat_Com_Det_Cotizadores.Campo_Empleado_ID +
                            "= '" + Cotizadores.P_Empleado_ID + "'";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;

        }

        public static void Modificar_Cotizadores(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty;
                 //Ejecutar consulta
              
                Mi_SQL = "UPDATE " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores + " SET ";
                Mi_SQL = Mi_SQL + Cat_Com_Cotizadores.Campo_Correo;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_Correo  +"'";
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Cotizadores.Campo_Password_Correo;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_Password + "'";
                Mi_SQL = Mi_SQL + ", " + Cat_Com_Cotizadores.Campo_IP_Correo_Saliente;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_Direccion_IP + "'";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Cotizadores.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_No_Empleado + "')";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Cotizadores
        ///DESCRIPCIÓN:Elimina un giro proveedor en la base de datos
        ///PARAMETROS:  1.- Cls_Cat_Com_Cotizadores_Negocio
        ///CREO: Jacqueline Ramirez Sierra
        ///FECHA_CREO: 09/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Eliminar_Cotizadores(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;            
            String Mensaje = String.Empty; //Variable para el mensaje de error

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

               
                //BORRA DETALLES COTIZADOR
                Mi_SQL = "";
                Mi_SQL = "DELETE FROM " + Cat_Com_Cotizadores.Tabla_Cat_Com_Cotizadores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Det_Cotizadores.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + "=(SELECT " + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_No_Empleado;
                Mi_SQL = Mi_SQL + "='" + Cotizadores.P_No_Empleado + "')";

                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                //Ejecutar transaccion
                Obj_Transaccion.Commit();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {

                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }
        
        #endregion

        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Detalles
        ///DESCRIPCIÓN: eliminar los conceptos asignados al cotizador
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 14/Marzo/2011 06:48:59 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************        
        public static void Eliminar_Detalles(Cls_Cat_Com_Cotizadores_Negocio Cotizadores)
        {
            //Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; //Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error            
            //Se eliminan los detalles giros correspondientes al Cotizador
            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                Mi_SQL = "";
                Mi_SQL = "DELETE " + Cat_Com_Det_Cotizadores.Tabla_Cat_Com_Det_Cotizadores +
                     " WHERE " + Cat_Com_Det_Cotizadores.Campo_Empleado_ID +
                     " = '" + Cotizadores.P_Empleado_ID + "'";

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteNonQuery();                                              
                
                Obj_Transaccion.Commit();
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }

                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }
    }
}
