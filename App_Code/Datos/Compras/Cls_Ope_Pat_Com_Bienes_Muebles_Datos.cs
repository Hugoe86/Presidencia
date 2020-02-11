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
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using System.IO;
/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Bienes_Muebles_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Datos {

    public class Cls_Ope_Pat_Com_Bienes_Muebles_Datos {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Bien_Mueble
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Bien Mueble
        ///PARAMETROS           : 
        ///                     1.  Bien_Mueble.Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 26/Noviembre/2010 
        ///MODIFICO             : Salvador Hernández Ramírez
        ///FECHA_MODIFICO       : 04/Febrero/2011 
        ///CAUSA_MODIFICACIÓN   : Se modifico este método, ya que se insertaron en la base de datos mas valores, y se implemento el metodo "Alta_Bitacora" para registrar los insert y update en la BD
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Alta_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble)
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

            String Mi_SQL = "";
            try {
                if (Bien_Mueble.P_No_Requisicion != null)  // Se actuaaliza el Producto
                {
                    Mi_SQL = " UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Req_Producto.Campo_Resguardado + " = 'SI'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = '" + Bien_Mueble.P_Producto_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Bien_Mueble.P_No_Requisicion + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }

                Int64 No_Invetario = Consulta_Consecutivo_Inventario(Bien_Mueble.P_Operacion);
                Bien_Mueble.P_Numero_Inventario = No_Invetario.ToString();

                String Bien_Mueble_ID = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles, Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID, 10);
                
                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Producto_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Operacion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Procedencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Donador_ID + ", " + Ope_Pat_Bienes_Muebles.Campo_Nombre + ", " + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", " + Ope_Pat_Bienes_Muebles.Campo_Garantia+ ", " + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + ", " + Ope_Pat_Bienes_Muebles.Campo_Area_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Material_ID + ", " + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + ", " + Ope_Pat_Bienes_Muebles.Campo_Factura;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", " + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", " + Ope_Pat_Bienes_Muebles.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Motivo_Baja + ", " + Ope_Pat_Bienes_Muebles.Campo_Estado;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Observadores;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ", " + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ", " + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion;
                if (Bien_Mueble.P_Ascencendia != null && Bien_Mueble.P_Ascencendia.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID;
                }
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Zona_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Proveniente;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Bien_Mueble_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Producto_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Operacion + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Procedencia + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Donador_ID + "', '" + Bien_Mueble.P_Nombre_Producto + "', '" + Bien_Mueble.P_Modelo + "', '" + Bien_Mueble.P_Garantia + "', '" + Bien_Mueble.P_Marca_ID + "'"; 
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Dependencia_ID + "','" + Bien_Mueble.P_Area_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Material_ID + "','" + Bien_Mueble.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + "," + Bien_Mueble.P_Numero_Inventario + ",'" + Bien_Mueble.P_Factura + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Numero_Serie + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Costo_Actual + "','" + Bien_Mueble.P_Costo_Actual + "','" + Bien_Mueble.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Motivo_Baja + "','" + Bien_Mueble.P_Estado + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "'," + Bien_Mueble.P_Cantidad + ", SYSDATE , '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Adquisicion_) + "'";
                if (Bien_Mueble.P_Ascencendia != null && Bien_Mueble.P_Ascencendia.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Ascencendia + "'";
                }
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Numero_Inventario + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Zona + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Proveniente + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Clase_Activo_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Clasificacion_ID + "'";
                Mi_SQL = Mi_SQL + ", SYSDATE ";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la consulta 1      
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx", Bien_Mueble_ID, Mi_SQL);  // Se da de alta el insert en la tabla "APL_BITACORA" de la BD


                // Asignar consulta para ingresar la factura
                Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " (";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Marca_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Modelo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Garantia + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Serie + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Contra_Recibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles, Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario, 10)) + ", ";
                Mi_SQL = Mi_SQL + No_Invetario + ", ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Marca_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Modelo + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Garantia + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Color_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Material_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Numero_Serie + "', ";
                Mi_SQL = Mi_SQL + 0 + ", ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Operacion + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Usuario_ID + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE)";

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación

                if (Bien_Mueble.P_Resguardantes != null && Bien_Mueble.P_Resguardantes.Rows.Count > 0){

                    if (Bien_Mueble.P_Operacion == "RESGUARDO")
                    {
                        String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                        for (Int32 Cnt = 0; Cnt < Bien_Mueble.P_Resguardantes.Rows.Count; Cnt++)
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_Mueble_ID + "', 'BIEN_MUEBLE','" + Bien_Mueble.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "', 'VIGENTE', '" + Bien_Mueble.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Estado + "','" + Bien_Mueble.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + ",'SI','" + Bien_Mueble.P_Observaciones + "'";
                            Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                            ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery(); // Se ejecuta la consulta 2
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx", ID_Consecutivo, Mi_SQL); // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                        }
                    }else if (Bien_Mueble.P_Operacion == "RECIBO") // Si se inserta un Recibo
                    {
                        String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos, Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID, 50);
                        for (Int32 Cnt = 0; Cnt < Bien_Mueble.P_Resguardantes.Rows.Count; Cnt++)
                        {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + ", " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Tipo + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Almacen_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus + ", " + Ope_Pat_Bienes_Recibos.Campo_Comentarios;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estado + ", " + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Alta + ", " + Ope_Pat_Bienes_Recibos.Campo_Observaciones;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_Mueble_ID + "', 'BIEN_MUEBLE','" + Bien_Mueble.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "', 'VIGENTE', '" + Bien_Mueble.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Estado + "','" + Bien_Mueble.P_Dependencia_ID + "'";
                            Mi_SQL = Mi_SQL + ",'SI','" + Bien_Mueble.P_Observaciones + "'";
                            Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                            ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery(); // Se ejecuta la consulta 2
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx", ID_Consecutivo, Mi_SQL); // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                        }
                    }
                }

                String ID_Consecutivo_Archivo = "";
                if (Bien_Mueble.P_Archivo != null && Bien_Mueble.P_Archivo.Trim().Length > 0)
                {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'BIEN_MUEBLE', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien_Mueble.P_Archivo) + "', 'NORMAL', '" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Bien_Mueble.P_Bien_Mueble_ID = Bien_Mueble_ID;

                Trans.Commit();
                if (Bien_Mueble.P_Archivo != null && Bien_Mueble.P_Archivo.Trim().Length > 0) {
                    Bien_Mueble.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien_Mueble.P_Archivo);
                }

            } catch (OracleException Ex) {
                Trans.Rollback();
                // Variable para el mensaje 
                // Configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
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
                    Mensaje = "Error al intentar dar de Alta un Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
              
            }finally {
                 Cn.Close();
            }
            return Bien_Mueble;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///PARAMETROS           : 
        ///                     1.  Bien_Mueble.    Contiene los parametros que se van a utilizar para
        ///                                         hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 26/Noviembre/2010 
        ///MODIFICO             : Salvador Hernández Ramírez
        ///FECHA_MODIFICO       : 03/Febrero/2011 
        ///CAUSA_MODIFICACIÓN   : Se agregó codigo para consultar para Modelos y Marcas y
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble) {
            String Mi_SQL = null;
            DataSet Ds_Bien_Mueble = null;
            DataTable Dt_Bien_Mueble = new DataTable();
            Boolean Entro_Where = false;
            try {
                if (Bien_Mueble.P_Tipo_DataTable.Equals("BIENES_MUEBLES")) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " AS PRODUCTO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " AS PRODUCTO_CLAVE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS PRODUCTO_NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + "";
                    Mi_SQL = Mi_SQL + " AND (" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " LIKE '%" + Bien_Mueble.P_Producto_ID + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " LIKE '%" + Bien_Mueble.P_Producto_ID + "%'" + ")";
                
                
                } else if(Bien_Mueble.P_Tipo_DataTable.Equals("PRODUCTOS")){
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " AS PRODUCTO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " AS CLAVE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + " AS COSTO_PRODUCTO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Tipo + " ='BIEN_MUEBLE'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " AND (" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " LIKE '%" + Bien_Mueble.P_Producto_ID + "%'";
                    Mi_SQL = Mi_SQL + " OR " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " LIKE '%" + Bien_Mueble.P_Producto_ID + "%')";

                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("DEPENDENCIAS")) {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Clave + "||'-'|| " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("DEPENDENCIAS_SIN_CLAVE")) {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("ZONAS")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Zonas.Campo_Zona_ID + " AS ZONA_ID, " + Cat_Pat_Zonas.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " WHERE " + Cat_Pat_Zonas.Campo_Estatus + " = 'VIGENTE' ORDER BY " + Cat_Pat_Zonas.Campo_Zona_ID;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("AREAS")) {
                    Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID + " AS AREA_ID, " + Cat_Areas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Areas.Tabla_Cat_Areas + " ORDER BY " + Cat_Areas.Campo_Nombre;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("MATERIALES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Materiales.Campo_Material_ID + " AS MATERIAL_ID, " + Cat_Pat_Materiales.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " ORDER BY " + Cat_Pat_Materiales.Campo_Descripcion;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("MODELOS")) {
                    Mi_SQL = "SELECT " + Cat_Com_Modelos.Campo_Modelo_ID + " AS MODELO_ID, " + Cat_Com_Modelos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " ORDER BY " + Cat_Com_Modelos.Campo_Nombre;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("MARCAS")) {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + " AS MARCA_ID, " + Cat_Com_Marcas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Marcas.Campo_Nombre;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("COLORES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Colores.Campo_Color_ID + " AS COLOR_ID, " + Cat_Pat_Colores.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " ORDER BY " + Cat_Pat_Colores.Campo_Descripcion;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("CLASIFICACIONES")) {
                    Mi_SQL = "SELECT " + Cat_Pat_Clasificaciones.Campo_Clasificacion_ID + " AS CLASIFICACION_ID, " + Cat_Pat_Clasificaciones.Campo_Descripcion + " AS DESCRIPCION";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Clasificaciones.Tabla_Cat_Pat_Clasificaciones + " ORDER BY " + Cat_Pat_Clasificaciones.Campo_Descripcion;
                }else if (Bien_Mueble.P_Tipo_DataTable.Equals("MARCAS")){
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Campo_Marca_ID + ", " + Cat_Com_Marcas.Campo_Nombre;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ORDER BY " + Cat_Com_Marcas.Campo_Nombre;
                }else if (Bien_Mueble.P_Tipo_DataTable.Equals("MODELOS")){
                    Mi_SQL = "SELECT " + Cat_Com_Modelos.Campo_Modelo_ID + ", " + Cat_Com_Modelos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " ORDER BY " + Cat_Com_Modelos.Campo_Nombre;
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("PROVEEDORES")) {
                    Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID + " AS PROVEEDOR_ID, " + Cat_Com_Proveedores.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ORDER BY " + Cat_Com_Proveedores.Campo_Nombre;
                }
                else if (Bien_Mueble.P_Tipo_DataTable.Trim().Equals("EMPLEADOS"))
                {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Bien_Mueble.P_Dependencia_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Campo_Estatus + " = 'ACTIVO' ORDER BY NOMBRE";
                }
                else if (Bien_Mueble.P_Tipo_DataTable.Trim().Equals("EMPLEADOS_DEPENDENCIAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                    Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEDO";
                    Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " AS APELLIDO_PATERNO";
                    Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS APELLIDO_MATERNO";
                    Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_COMPLETO";
                    Mi_SQL += ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "";
                    Mi_SQL += " ||' - '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL += " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS EMPLEADO";
                    Mi_SQL += " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    if (Bien_Mueble.P_Dependencia_ID != null && Bien_Mueble.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) {
                            Mi_SQL += " AND ";
                        } else {
                            Mi_SQL += " WHERE ";
                        }
                        Mi_SQL += Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " ='" + Bien_Mueble.P_Dependencia_ID + "'";
                    }
                    Mi_SQL += " ORDER BY NOMBRE_COMPLETO";  
                } else if (Bien_Mueble.P_Tipo_DataTable.Equals("BIENES")) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NO_INVENTARIO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion + " AS COLOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                    Mi_SQL = Mi_SQL + ", DECODE(" + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + "";
                    Mi_SQL = Mi_SQL + ", 'DEFINITIVA', 'BAJA (DEFINITIVA)', 'TEMPORAL', 'BAJA (TEMPORAL)', 'VIGENTE', 'VIGENTE') AS ESTATUS";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " AS OPERACION";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                    Mi_SQL = Mi_SQL + " ON " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID;
                    if (Bien_Mueble.P_Numero_Inventario_Anterior != null && Bien_Mueble.P_Numero_Inventario_Anterior.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Bien_Mueble.P_Numero_Inventario_Anterior + "%'";
                    }
                    if (Bien_Mueble.P_Numero_Inventario != null && Bien_Mueble.P_Numero_Inventario.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Numero_Inventario + "'";
                    }
                    if (Bien_Mueble.P_Nombre_Producto != null && Bien_Mueble.P_Nombre_Producto.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Nombre_Producto + "'";
                    }
                    if (Bien_Mueble.P_Dependencia_ID != null && Bien_Mueble.P_Dependencia_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Dependencia_ID + "'";
                    }
                    if (Bien_Mueble.P_Modelo != null && Bien_Mueble.P_Modelo.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Bien_Mueble.P_Modelo + "%'";
                    }
                    if (Bien_Mueble.P_Marca_ID != null && Bien_Mueble.P_Marca_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Marca_ID + "'";
                    }
                    if (Bien_Mueble.P_Estatus != null && Bien_Mueble.P_Estatus.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Estatus + "'";
                    }
                    if (Bien_Mueble.P_Estado != null && Bien_Mueble.P_Estado.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Estado + "'";
                    }
                    if (Bien_Mueble.P_Factura != null && Bien_Mueble.P_Factura.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura;
                        Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Factura + "'";
                    }
                    if (Bien_Mueble.P_Numero_Serie != null && Bien_Mueble.P_Numero_Serie.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Bien_Mueble.P_Numero_Serie + "%'";
                    }
                    if (Bien_Mueble.P_RFC_Resguardante != null && Bien_Mueble.P_RFC_Resguardante.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_RFC + " LIKE '%" + Bien_Mueble.P_RFC_Resguardante + "%' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'"+ " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                    }
                    if (Bien_Mueble.P_No_Empleado_Resguardante != null && Bien_Mueble.P_No_Empleado_Resguardante.Trim().Length > 0)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN (SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Campo_No_Empleado + " LIKE '%" + Bien_Mueble.P_No_Empleado_Resguardante + "%' )";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                    }
                    if (Bien_Mueble.P_Resguardante_ID != null && Bien_Mueble.P_Resguardante_ID.Trim().Length > 0) {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                        Mi_SQL = Mi_SQL + " IN (SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Bien_Mueble.P_Resguardante_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'" + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE')";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre;
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                    Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien_Mueble != null) {
                    Dt_Bien_Mueble = Ds_Bien_Mueble.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Bien_Mueble;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Bien_Mueble
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Bien Mueble.
        ///PARAMETROS:     
        ///             1. Bien_Mueble. Contiene los parametros para actualizar el registro en la Base de Datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:     27/Noviembre/2010 
        ///MODIFICO:       Salvador Hernández Ramírez
        ///FECHA_MODIFICO  04/Febrero/2011
        ///CAUSA_MODIFICACIÓN  Se implemento el metodo "Alta_Bitacora" para registrar los Insert y Update en la Base de Datos
        ///*******************************************************************************
        public static void Modificar_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble) {
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
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal_1 = Consultar_Detalles_Bien_Mueble(Bien_Mueble);
                String Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " = '" + Bien_Mueble.P_Material_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " = '" + Bien_Mueble.P_Dependencia_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Area_ID + " = '" + Bien_Mueble.P_Area_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " = '" + Bien_Mueble.P_Marca_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Procedencia + " = '" + Bien_Mueble.P_Procedencia + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID + " = '" + Bien_Mueble.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Modelo + " = '" + Bien_Mueble.P_Modelo + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Garantia + " = '" + Bien_Mueble.P_Garantia + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " = '" + Bien_Mueble.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Factura + " = '" + Bien_Mueble.P_Factura + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " = '" + Bien_Mueble.P_Numero_Serie + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Bien_Mueble.P_Estatus + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Motivo_Baja + " = '" + Bien_Mueble.P_Motivo_Baja + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Estado + " = '" + Bien_Mueble.P_Estado + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Observadores + " = '" + Bien_Mueble.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Zona_ID + " = '" + Bien_Mueble.P_Zona + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID + " = '" + Bien_Mueble.P_Clase_Activo_ID + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID + " = '" + Bien_Mueble.P_Clasificacion_ID+ "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Mueble.P_Bien_Mueble_ID, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD

                if (Bien_Mueble.P_Operacion == "RESGUARDO") {
                    if (Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE")) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Bien_Mueble);

                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";

                            String Bien_Resguardado_Id = "" + Temporal.P_Resguardantes.Rows[Contador][0].ToString(); // Esta asignacion se realiza para guardar el identificador del bien resguardado en la tabla de bitacoras
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardado_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD

                        }

                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Bien_Mueble, Temporal_1);

                        //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                        if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                            String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                            for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_Mueble.P_Bien_Mueble_ID + "', 'BIEN_MUEBLE','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Estado + "','" + Bien_Mueble.P_Dependencia_ID + "'";
                                Mi_SQL = Mi_SQL + ",'SI','" + Bien_Mueble.P_Observaciones + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                                ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", ID_Consecutivo, Mi_SQL); // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                            }
                        }
                    } else {
                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                            String Bien_Resguardo_Id = "" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString();
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardo_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }
                    }
                } else if (Bien_Mueble.P_Operacion == "RECIBO") {

                    if (Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE"))  {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Bien_Mueble);

                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";

                            String Bien_Resguardado_Id = "" + Temporal.P_Resguardantes.Rows[Contador][0].ToString(); // Esta asignacion se realiza para guardar el identificador del bien resguardado en la tabla de bitacoras
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardado_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }

                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Bien_Mueble, Temporal_1);

                        //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                        if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                            String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos, Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID, 50);
                            for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + ", " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Tipo + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Almacen_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus + ", " + Ope_Pat_Bienes_Recibos.Campo_Comentarios;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estado + ", " + Ope_Pat_Bienes_Recibos.Campo_Dependencia_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Modificacion + ", " + Ope_Pat_Bienes_Recibos.Campo_Observaciones;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_Mueble.P_Bien_Mueble_ID + "', 'BIEN_MUEBLE','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Estado + "','" + Bien_Mueble.P_Dependencia_ID + "'";
                                Mi_SQL = Mi_SQL + ",'SI','" + Bien_Mueble.P_Observaciones + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                                ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", ID_Consecutivo, Mi_SQL); // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                            }
                        }
                    } else {
                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                            String Bien_Resguardo_Id = "" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString();
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardo_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }
                    }


                }


                String ID_Consecutivo_Archivo = "";
                if (Bien_Mueble.P_Archivo != null && Bien_Mueble.P_Archivo.Trim().Length > 0) {
                    ID_Consecutivo_Archivo = Obtener_ID_Consecutivo(Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes, Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID, 50);
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes + " ( " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Tipo_Archivo + ", " + Ope_Pat_Archivos_Bienes.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Fecha_Creo + " ) VALUES ( " + Convert.ToInt32(ID_Consecutivo_Archivo) + ", '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + " , 'BIEN_MUEBLE', SYSDATE, '" + Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien_Mueble.P_Archivo) + "', 'NORMAL', '" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                Trans.Commit();
                if (Bien_Mueble.P_Archivo != null && Bien_Mueble.P_Archivo.Trim().Length > 0) {
                    Bien_Mueble.P_Archivo = Convert.ToInt32(ID_Consecutivo_Archivo).ToString().Trim() + Path.GetExtension(Bien_Mueble.P_Archivo);
                }
                Actualizar_Bienes_Muebles_Dependientes(Bien_Mueble);
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
                    Mensaje = "Error al intentar Modificar el Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }
                   
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Bienes_Muebles_Dependientes
        ///DESCRIPCIÓN: Actualiza los bienes dependientes
        ///PARÁMETROS:     
        ///             1. Bien_Mueble. Objeto del cual se actualizarán sus registros.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static void Actualizar_Bienes_Muebles_Dependientes(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble) {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            String Mi_SQL = null;
            DataTable Dt_Datos = null;
            DataSet Ds_Datos = null;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            try {
                Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Proveniente + " = 'BIEN_MUEBLE'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = 'VIGENTE'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null && Ds_Datos.Tables.Count > 0) {
                    Dt_Datos = Ds_Datos.Tables[0];
                    for (Int32 Contador = 0; Contador < Dt_Datos.Rows.Count; Contador++) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        BM_Negocio.P_Bien_Mueble_ID = Dt_Datos.Rows[Contador][Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID].ToString();
                        BM_Negocio = BM_Negocio.Consultar_Detalles_Bien_Mueble();
                        BM_Negocio.P_Resguardantes = Bien_Mueble.P_Resguardantes;
                        BM_Negocio.P_Dependencia_ID = Bien_Mueble.P_Dependencia_ID;
                        BM_Negocio.P_Usuario_Nombre = Bien_Mueble.P_Usuario_Nombre;
                        BM_Negocio.P_Usuario_ID = Bien_Mueble.P_Usuario_ID;
                        BM_Negocio.Modificar_Bien_Mueble();
                    }
                }
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
                    Mensaje = "Error al intentar Modificar. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }            
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Bien_Mueble
        ///DESCRIPCIÓN: Obtiene los Datos a Detalle de un Bien Mueble en Especifico.
        ///PARAMETROS:   
        ///             1. Parametros.   Bien Mueble que se va a ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Consultar_Datos_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Parametros) {
            String Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID + " AS PRODUCTO_ID";
            Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA";
            Mi_SQL = Mi_SQL + ", " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + "";
            Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS AREA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Material_ID + " AS MATERIAL";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Color_ID + " AS COLOR";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Factura + " AS FACTURA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + " AS NUMERO_SERIE";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS COSTO_ACTUAL";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " AS ESTATUS";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + "";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID + "";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Garantia + "";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Observadores + " AS OBSERVACIONES";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Motivo_Baja + " AS MOTIVO_BAJA";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " AS FECHA_ADQUISICION";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID + " AS PARENT_ID";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Operacion + "";
            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + " AS CANTIDAD";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + ", " + Cat_Areas.Tabla_Cat_Areas + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + " AND " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + "";
            Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Area_ID + "";
            if (!Parametros.P_Buscar_Numero_Inventario) {
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Bien_Mueble_ID + "'";
            } else {
                Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + "";
                Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
            }
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble   = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()){
                    Bien_Mueble.P_Bien_Mueble_ID = (Data_Reader["BIEN_MUEBLE_ID"] != null) ? Data_Reader["BIEN_MUEBLE_ID"].ToString().Trim() : "";
                    if (Data_Reader["PRODUCTO_ID"] != null) { Bien_Mueble.P_Producto_ID = Data_Reader["PRODUCTO_ID"].ToString().Trim(); }
                    if (Data_Reader["DEPENDENCIA"] != null) { Bien_Mueble.P_Dependencia_ID = Data_Reader["DEPENDENCIA"].ToString().Trim(); }
                    if (Data_Reader["AREA"] != null) { Bien_Mueble.P_Area_ID = Data_Reader["AREA"].ToString().Trim(); }
                    if (Data_Reader["MATERIAL"] != null) { Bien_Mueble.P_Material_ID = Data_Reader["MATERIAL"].ToString().Trim(); }
                    if (Data_Reader["COLOR"] != null) { Bien_Mueble.P_Color_ID = Data_Reader["COLOR"].ToString().Trim(); }
                    if (Data_Reader["FACTURA"] != null) { Bien_Mueble.P_Factura = Data_Reader["FACTURA"].ToString().Trim(); }
                    if (Data_Reader["NUMERO_SERIE"] != null) { Bien_Mueble.P_Numero_Serie = Data_Reader["NUMERO_SERIE"].ToString().Trim(); }
                    if (Data_Reader["COSTO_ACTUAL"] != null) { Bien_Mueble.P_Costo_Actual = Convert.ToDouble(Data_Reader["COSTO_ACTUAL"]); }
                    if (Data_Reader["ESTATUS"] != null) { Bien_Mueble.P_Estatus = Data_Reader["ESTATUS"].ToString().Trim(); }
                    if (Data_Reader["ESTADO"] != null) { Bien_Mueble.P_Estado = Data_Reader["ESTADO"].ToString().Trim(); }
                    if (Data_Reader["NUMERO_INVENTARIO"] != null) { Bien_Mueble.P_Numero_Inventario = Data_Reader["NUMERO_INVENTARIO"].ToString().Trim(); }
                    if (Data_Reader["OBSERVACIONES"] != null) { Bien_Mueble.P_Observaciones = Data_Reader["OBSERVACIONES"].ToString().Trim(); }
                    if (Data_Reader["MOTIVO_BAJA"] != null) { Bien_Mueble.P_Motivo_Baja = Data_Reader["MOTIVO_BAJA"].ToString().Trim(); }
                    if (Data_Reader["FECHA_ADQUISICION"] != null) { Bien_Mueble.P_Fecha_Adquisicion = Data_Reader["FECHA_ADQUISICION"].ToString(); ; }
                    if (Data_Reader["PARENT_ID"] != null) { Bien_Mueble.P_Ascencendia = Data_Reader["PARENT_ID"].ToString().Trim(); }
                    if (Data_Reader["CANTIDAD"] != null) { Bien_Mueble.P_Cantidad = Convert.ToInt32(Data_Reader["CANTIDAD"]); }
                    if (Data_Reader["OPERACION"] != null) { Bien_Mueble.P_Operacion= Convert.ToString(Data_Reader["OPERACION"].ToString().Trim()); }
                    if (Data_Reader["MODELO"] != null) { Bien_Mueble.P_Modelo = Convert.ToString(Data_Reader["MODELO"]); }
                    if (Data_Reader["GARANTIA"] != null) { Bien_Mueble.P_Garantia= Convert.ToString(Data_Reader["GARANTIA"]); }
                }

                Data_Reader.Close();
                //OBTIENE DATOS MAS GENERALES DEL BIEN MUEBLE
                if (Bien_Mueble.P_Producto_ID != null && Bien_Mueble.P_Producto_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + " AS COSTO_INICIAL";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Producto_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID + "";
                } else {
                    Mi_SQL = "SELECT " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " || ' - ' || " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", 0.0 AS COSTO_INICIAL";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS NOMBRE_PRODUCTO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + "";
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + "";
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID + "";
                }
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    Bien_Mueble.P_Producto_ID = (Data_Reader["NOMBRE_PRODUCTO"] != null) ? Data_Reader["NOMBRE_PRODUCTO"].ToString() : "";
                    Bien_Mueble.P_Marca_ID = (Data_Reader["MARCA"] != null) ? Data_Reader["MARCA"].ToString() : "";
                    Bien_Mueble.P_Modelo_ID = (Data_Reader["MODELO"] != null) ? Data_Reader["MODELO"].ToString() : "";
                    Bien_Mueble.P_Costo_Inicial = (Data_Reader["COSTO_INICIAL"] != null) ? Convert.ToDouble(Data_Reader["COSTO_INICIAL"]) : 0;
                }
                Data_Reader.Close();

                DataSet Ds_Bien_Mueble = null;
                
                if (Bien_Mueble.P_Operacion == "RESGUARDO")
                {
                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0)
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID + " AS EMPLEADO_ALMACEN_ID";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Bien_Mueble == null)
                    {
                        Bien_Mueble.P_Resguardantes = new DataTable();
                    }
                    else
                    {
                        Bien_Mueble.P_Resguardantes = Ds_Bien_Mueble.Tables[0];
                    }
                    Ds_Bien_Mueble = null;

                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) // Se consultan los Resguardantes que estan dados de baja
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_FINAL";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final;
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Bien_Mueble == null)
                    {
                        Bien_Mueble.P_Historial_Resguardos = new DataTable();
                    }
                    else
                    {
                        Bien_Mueble.P_Historial_Resguardos = Ds_Bien_Mueble.Tables[0];
                    }
                }

                else if (Bien_Mueble.P_Operacion == "RECIBO")
                {
                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0)
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Almacen_ID + " AS EMPLEADO_ALMACEN_ID";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Bien_Mueble == null)
                    {
                        Bien_Mueble.P_Resguardantes = new DataTable();
                    }
                    else
                    {
                        Bien_Mueble.P_Resguardantes = Ds_Bien_Mueble.Tables[0];
                    }
                    Ds_Bien_Mueble = null;

                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) // Se consultan los Resguardantes que estan dados de baja
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " AS FECHA_FINAL";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final;
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }

                    if (Ds_Bien_Mueble == null) {
                        Bien_Mueble.P_Historial_Resguardos = new DataTable();
                    } else {
                        Bien_Mueble.P_Historial_Resguardos = Ds_Bien_Mueble.Tables[0];
                    }
                }

                Ds_Bien_Mueble = null;

                if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID + " AS ARCHIVO_BIEN_ID, " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + " AS ARCHIVO, '' AS DESCRIPCION FROM " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Archivos_Bienes.Campo_Tipo + " = 'BIEN_MUEBLE' AND " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + "='" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " DESC";
                    Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien_Mueble == null) {
                    Bien_Mueble.P_Dt_Historial_Archivos = new DataTable();
                } else {
                    Bien_Mueble.P_Dt_Historial_Archivos = Ds_Bien_Mueble.Tables[0];
                }
                Ds_Bien_Mueble = null;
                if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + " AS CANTIDAD";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID + " AS MODELO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID;
                    Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien_Mueble == null) {
                    Bien_Mueble.P_Dt_Bienes_Dependientes = new DataTable();
                } else {
                    Bien_Mueble.P_Dt_Bienes_Dependientes = Ds_Bien_Mueble.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar los datos del Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Bien_Mueble;
        }
                        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Bien_Mueble
        ///DESCRIPCIÓN: Obtiene los Datos a Detalle de un Bien Mueble en Especifico.
        ///PARAMETROS:   
        ///             1. Parametros.   Bien Mueble que se va a ver a Detalle.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 27/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Consultar_Detalles_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Parametros) {
            Boolean Entro_Where = false;
            String Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
            if (Parametros.P_Bien_Mueble_ID != null && Parametros.P_Bien_Mueble_ID.Trim().Length > 0) {
                if (!Entro_Where) {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Bien_Mueble_ID + "'";
                    Entro_Where = true;
                } else {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Bien_Mueble_ID + "'";
                }
            }
             if (Parametros.P_Numero_Inventario != null && Parametros.P_Numero_Inventario.Trim().Length > 0) {
                if (!Entro_Where) {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                    Entro_Where = true;
                } else {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario + "'";
                }
            }
             if (Parametros.P_Numero_Inventario_Anterior != null && Parametros.P_Numero_Inventario_Anterior.Trim().Length > 0) {
                if (!Entro_Where) {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + "";
                    Mi_SQL = Mi_SQL + " = '" + Parametros.P_Numero_Inventario_Anterior + "'";
                    Entro_Where = true;
                } else {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + "";
                    Mi_SQL = Mi_SQL + " LIKE '%" + Parametros.P_Numero_Inventario_Anterior + "'";
                }
            }
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble   = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            OracleDataReader Data_Reader;
            try{
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()){
                    Bien_Mueble.P_Bien_Mueble_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Producto_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Producto_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Producto_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Dependencia_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Area_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Area_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Area_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Material_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Material_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Material_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Color_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Color_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Color_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Numero_Inventario = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario].ToString().Trim() : "";
                    Bien_Mueble.P_Numero_Inventario_Anterior = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior].ToString().Trim() : "";
                    Bien_Mueble.P_Factura = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Factura].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Factura].ToString().Trim() : "";
                    Bien_Mueble.P_Numero_Serie = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Numero_Serie].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Numero_Serie].ToString().Trim() : "";
                    Bien_Mueble.P_Costo_Inicial = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Costo_Alta].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Costo_Alta]) : 0.0;
                    Bien_Mueble.P_Costo_Actual = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Costo_Actual].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Costo_Actual]) : 0.0;
                    Bien_Mueble.P_Estatus = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Estatus].ToString().Trim() : "";
                    Bien_Mueble.P_Motivo_Baja = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Motivo_Baja].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Motivo_Baja].ToString().Trim() : "";
                    Bien_Mueble.P_Estado = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Estado].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Estado].ToString().Trim() : "";
                    Bien_Mueble.P_Fecha_Adquisicion_ = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion].ToString())) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion]) : new DateTime();
                    Bien_Mueble.P_Nombre_Producto = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Nombre].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Nombre].ToString().Trim() : "";
                    Bien_Mueble.P_Marca_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Marca_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Marca_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Modelo_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Modelo_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Modelo_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Modelo = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Modelo].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Modelo].ToString().Trim() : "";
                    Bien_Mueble.P_Garantia = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Garantia].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Garantia].ToString().Trim() : "";
                    Bien_Mueble.P_Observaciones = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Observadores].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Observadores].ToString().Trim() : "";
                    Bien_Mueble.P_Operacion = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Operacion].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Operacion].ToString().Trim() : "";
                    Bien_Mueble.P_Ascencendia = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Procedencia = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Procedencia].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Procedencia].ToString().Trim() : "";
                    Bien_Mueble.P_Proveedor_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Zona = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Zona_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Zona_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Clase_Activo_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Clase_Activo_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Clasificacion_ID = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Clasificacion_ID].ToString().Trim() : "";
                    Bien_Mueble.P_Proveniente = (!String.IsNullOrEmpty(Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Proveniente].ToString())) ? Data_Reader[Ope_Pat_Bienes_Muebles.Campo_Proveniente].ToString().Trim() : "";
                }
                Data_Reader.Close();
                if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT (" + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + " ||' ['|| TO_CHAR(" + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ", 'DD/MM/YYYY') ||']') AS CREO";
                    Mi_SQL = Mi_SQL + ", (" + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + " ||' ['|| TO_CHAR(" + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + ", 'DD/MM/YYYY') ||']') AS MODIFICO";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                     Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     while (Data_Reader.Read()) {
                         Bien_Mueble.P_Dato_Creacion = (!String.IsNullOrEmpty(Data_Reader["CREO"].ToString())) ? Data_Reader["CREO"].ToString() : "";
                         Bien_Mueble.P_Dato_Modificacion = (!String.IsNullOrEmpty(Data_Reader["MODIFICO"].ToString())) ? Data_Reader["MODIFICO"].ToString() : "";
                     }
                     Data_Reader.Close();
                }
                //OBTIENE DATOS MAS GENERALES DEL BIEN MUEBLE
                if (Bien_Mueble.P_Producto_ID != null && Bien_Mueble.P_Producto_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Costo + " AS COSTO_INICIAL, " + Cat_Com_Productos.Campo_Clave + " AS CLAVE ";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + "";
                    Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Producto_ID + "'";
                }
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                while (Data_Reader.Read()) {
                    //Bien_Mueble.P_Costo_Inicial = (Data_Reader["COSTO_INICIAL"] != null) ? Convert.ToDouble(Data_Reader["COSTO_INICIAL"]) : 0;
                    Bien_Mueble.P_Clave_Producto = (!String.IsNullOrEmpty(Data_Reader["CLAVE"].ToString())) ? Data_Reader["CLAVE"].ToString() : "";
                }
                Data_Reader.Close();


                DataSet Ds_Bien_Mueble = null;

                if (Bien_Mueble.P_Operacion == "RESGUARDO")
                {
                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0)
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID + " AS EMPLEADO_ALMACEN_ID";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    }
                    if (Ds_Bien_Mueble == null)
                    {
                        Bien_Mueble.P_Resguardantes = new DataTable();
                    }
                    else
                    {
                        Bien_Mueble.P_Resguardantes = Ds_Bien_Mueble.Tables[0];
                    }
                    Ds_Bien_Mueble = null;
                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0)
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " AS FECHA_FINAL";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final;
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                }
                else if (Bien_Mueble.P_Operacion == "RECIBO")
                {
                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0)
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Almacen_ID + " AS EMPLEADO_ALMACEN_ID";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Comentarios+ " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }  if (Ds_Bien_Mueble == null) {
                        Bien_Mueble.P_Resguardantes = new DataTable();
                    }
                    else {
                        Bien_Mueble.P_Resguardantes = Ds_Bien_Mueble.Tables[0];
                    }
                    Ds_Bien_Mueble = null;

                    if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0)
                    {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID+ " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " AS DEPENDENCIA_ID";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + " AS FECHA_INICIAL";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " AS FECHA_FINAL";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos+ "." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Bien_Mueble.P_Bien_Mueble_ID + "";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + "." + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final;
                        Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                }

                if (Ds_Bien_Mueble == null) {
                    Bien_Mueble.P_Historial_Resguardos = new DataTable();
                } else {
                    Bien_Mueble.P_Historial_Resguardos = Ds_Bien_Mueble.Tables[0];
                }

                Ds_Bien_Mueble = null;
                if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Archivos_Bienes.Campo_Archivo_Bien_ID + " AS ARCHIVO_BIEN_ID, " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " AS FECHA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Archivos_Bienes.Campo_Archivo + " AS ARCHIVO, '' AS DESCRIPCION FROM " + Ope_Pat_Archivos_Bienes.Tabla_Ope_Pat_Archivos_Bienes;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Archivos_Bienes.Campo_Tipo + " = 'BIEN_MUEBLE' AND " + Ope_Pat_Archivos_Bienes.Campo_Bien_ID + "='" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Archivos_Bienes.Campo_Fecha + " DESC";
                    Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien_Mueble == null) {
                    Bien_Mueble.P_Dt_Historial_Archivos = new DataTable();
                } else {
                    Bien_Mueble.P_Dt_Historial_Archivos = Ds_Bien_Mueble.Tables[0];
                }
                Ds_Bien_Mueble = null;
                if (Bien_Mueble.P_Bien_Mueble_ID != null && Bien_Mueble.P_Bien_Mueble_ID.Trim().Length > 0) {
                    Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " AS BIEN_MUEBLE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + " AS NO_INVENTARIO_ANTERIOR";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NO_INVENTARIO_SIAS";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Modelo + " AS MODELO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estado + " AS ESTADO";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " ON " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID;
                    Mi_SQL = Mi_SQL + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Proveniente;
                    Mi_SQL = Mi_SQL + " = 'BIEN_MUEBLE'";
                    Ds_Bien_Mueble = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Bien_Mueble == null) {
                    Bien_Mueble.P_Dt_Bienes_Dependientes = new DataTable();
                } else {
                    Bien_Mueble.P_Dt_Bienes_Dependientes = Ds_Bien_Mueble.Tables[0];
                }
            }catch (Exception Ex){
                String Mensaje = "Error al intentar consultar los datos del Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Bien_Mueble;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Diferencia_Resguardos
        ///DESCRIPCIÓN: Saca la diferencia de unos resguardantes a otros.
        ///PARAMETROS:     
        ///             1. Actuales.        Bien Mueble como esta actualmente en la Base de Datos.
        ///             2. Actualizados.    Bien Mueble como quiere que quede al Actualizarlo.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Obtener_Diferencia_Resguardos(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Comparar, Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Base_Comparacion) {
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Resguardos = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();

            //SE OBTIENEN LOS NUEVOS RESGUARDANTES QUE SE DIERON DE ALTA PARA UN BIEN MUEBLE
            DataTable Dt_Tabla = new DataTable();
            Dt_Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.Int32"));
            Dt_Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            Dt_Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
            Dt_Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));

            if (Comparar.P_Resguardantes != null) {
                for (int Contador_1 = 0; Contador_1 < Comparar.P_Resguardantes.Rows.Count; Contador_1++) {
                    Boolean Eliminar = true;
                    if (Base_Comparacion.P_Resguardantes != null) { 
                        for (int Contador_2 = 0; Contador_2 < Base_Comparacion.P_Resguardantes.Rows.Count; Contador_2++) {
                            if (Comparar.P_Resguardantes.Rows[Contador_1][1].ToString().Equals(Base_Comparacion.P_Resguardantes.Rows[Contador_2][1].ToString())) {
                                Eliminar = false;
                                break;
                            }
                        }
                    }
                    if (Eliminar) {
                        DataRow Fila = Dt_Tabla.NewRow();
                        Fila["BIEN_RESGUARDO_ID"] = Convert.ToInt32(Comparar.P_Resguardantes.Rows[Contador_1][0]);
                        Fila["EMPLEADO_ID"] = Comparar.P_Resguardantes.Rows[Contador_1][1].ToString();
                        Fila["NOMBRE_EMPLEADO"] = Comparar.P_Resguardantes.Rows[Contador_1][2].ToString();
                        Fila["COMENTARIOS"] = Comparar.P_Resguardantes.Rows[Contador_1][3].ToString();
                        Dt_Tabla.Rows.Add(Fila);
                    }
                }
            }

            Resguardos.P_Resguardantes = Dt_Tabla;
            return Resguardos;
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

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Consecutivo_Inventario
        /// DESCRIPCION:            Método utilizado para consultar las ordenes de compra que se encuentren en estatus "SURTIDA"
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Int64 Consulta_Consecutivo_Inventario(String Operacion) {
            String Mi_SQL = String.Empty; //Variable para las consultas
            Object Consecutivo;
            Int64 No_Consecutivo;

            try {
                // Consulta 
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + " = '" + Operacion + "'";
                
                //Ejecutar consulta
                Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Consecutivo != null && Convert.IsDBNull(Consecutivo) == false)
                    No_Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                else
                    No_Consecutivo = 1;

                return No_Consecutivo;
            } catch (OracleException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (DBConcurrencyException ex) {
                throw new Exception("Error: " + ex.Message);
            } catch (Exception ex) {
                throw new Exception("Error: " + ex.Message);
            } finally {
            }
        }
 
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Migrar_Bien_Mueble
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Bien Mueble por migración.
        ///PARAMETROS           : 
        ///                     1.  Bien_Mueble.Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 26/Noviembre/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Alta_Migrar_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble)
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

            String Mi_SQL = "";
            try {

                Int64 No_Invetario = Consulta_Consecutivo_Inventario(Bien_Mueble.P_Operacion);
                Bien_Mueble.P_Numero_Inventario = No_Invetario.ToString();

                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Producto_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Operacion;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Procedencia;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Donador_ID + ", " + Ope_Pat_Bienes_Muebles.Campo_Nombre + ", " + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", " + Ope_Pat_Bienes_Muebles.Campo_Garantia+ ", " + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + ", " + Ope_Pat_Bienes_Muebles.Campo_Area_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Material_ID + ", " + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + ", " + Ope_Pat_Bienes_Muebles.Campo_Factura;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", " + Ope_Pat_Bienes_Muebles.Campo_Estatus;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Motivo_Baja + ", " + Ope_Pat_Bienes_Muebles.Campo_Estado;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Observadores;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ", " + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ", " + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion;
                if (Bien_Mueble.P_Ascencendia != null && Bien_Mueble.P_Ascencendia.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID;
                }
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior;
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario;
                Mi_SQL = Mi_SQL + ") VALUES ('" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Producto_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Proveedor_ID + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Operacion + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Procedencia + "'";
                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Donador_ID + "', '" + Bien_Mueble.P_Nombre_Producto + "', '" + Bien_Mueble.P_Modelo + "', '" + Bien_Mueble.P_Garantia + "', '" + Bien_Mueble.P_Marca_ID + "'"; 
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Dependencia_ID + "','" + Bien_Mueble.P_Area_ID + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Material_ID + "','" + Bien_Mueble.P_Color_ID + "'";
                Mi_SQL = Mi_SQL + "," + Bien_Mueble.P_Numero_Inventario + ",'" + Bien_Mueble.P_Factura + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Numero_Serie + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Costo_Actual + "','" + Bien_Mueble.P_Estatus + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Motivo_Baja + "','" + Bien_Mueble.P_Estado + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Observaciones + "'";
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "'," + Bien_Mueble.P_Cantidad + ", '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Creo) + "' , '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Adquisicion_) + "'";
                if (Bien_Mueble.P_Ascencendia != null && Bien_Mueble.P_Ascencendia.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Ascencendia + "'";
                }
                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Numero_Inventario_Anterior + "'";
                Mi_SQL = Mi_SQL + ",'" +  String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Inventario_) + "'";
                Mi_SQL = Mi_SQL + ")";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la consulta 1      
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Alta_Bienes_Muebles.aspx", Bien_Mueble_ID, Mi_SQL);  // Se da de alta el insert en la tabla "APL_BITACORA" de la BD


                // Asignar consulta para ingresar la factura
                Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " (";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Marca_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Modelo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Garantia + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Serie + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Contra_Recibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles, Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario, 10)) + ", ";
                Mi_SQL = Mi_SQL + No_Invetario + ", ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Marca_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Modelo + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Garantia + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Color_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Material_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Numero_Serie + "', ";
                Mi_SQL = Mi_SQL + 0 + ", ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Operacion + "', ";
                Mi_SQL = Mi_SQL + "'" + Bien_Mueble.P_Usuario_ID + "', ";
                Mi_SQL = Mi_SQL + "SYSDATE)";

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación
          

                Trans.Commit();

            } catch (OracleException Ex) {
                Trans.Rollback();
                // Variable para el mensaje 
                // Configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
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
                    Mensaje = "Error al intentar dar de Alta un Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
              
            }finally {
                 Cn.Close();
            }
            return Bien_Mueble;
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Migrar_Resguardos_Bien_Mueble
        ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Bien Mueble por migración.
        ///PARAMETROS           : 
        ///                     1.  Bien_Mueble.Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 26/Noviembre/2010 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Alta_Migrar_Resguardos_Bien_Mueble(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble)  {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            String Mi_SQL = "";
            try {
                if (Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE")) { 
                    Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "";
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + " = '" + Bien_Mueble.P_Dependencia_ID + "'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); 
                }

                if(Bien_Mueble.P_Operacion.Trim().Equals("RESGUARDO")){
                    Int32 Identificador = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 20));
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                    Mi_SQL = Mi_SQL + "( " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                    if (!Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE")) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Identificador + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Tipo + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Adquisicion_) + "'";
                    if (!Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE")) {
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Creo) + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Observaciones + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", 'IMPLEMENTACION DE SIAS'";
                    Mi_SQL = Mi_SQL + ", SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); 

                } else if(Bien_Mueble.P_Operacion.Trim().Equals("RECIBO")){
                    Int32 Identificador = Convert.ToInt32(Obtener_ID_Consecutivo(Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos, Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID, 20));
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                    Mi_SQL = Mi_SQL + "( " + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial;
                    if (Bien_Mueble.P_Estatus.Trim().Equals("BAJA")) {
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final;
                    }
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Comentarios;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Creo;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Identificador + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Tipo + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Adquisicion_) + "'";
                    if (Bien_Mueble.P_Estatus.Trim().Equals("BAJA")) {
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Bien_Mueble.P_Fecha_Creo) + "'";
                    }
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Observaciones + "'";
                    Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", 'IMPLEMENTACION DE SIAS'";
                    Mi_SQL = Mi_SQL + ", SYSDATE)";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); 
                }
                Trans.Commit();
            } catch (OracleException Ex) {
                Trans.Rollback();
                // Variable para el mensaje 
                // Configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152) {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }  else if (Ex.Code == 2627) {
                    if (Ex.Message.IndexOf("PRIMARY") != -1) {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    } else if (Ex.Message.IndexOf("UNIQUE") != -1) {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }  else if (Ex.Code == 547) {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                } else if (Ex.Code == 515) {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                } else {
                    Mensaje = "Error al intentar dar de Alta un Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
            return Bien_Mueble;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar_Estatus_Bienes
        ///DESCRIPCIÓN          : Actualiza los Estatus de los Bienes Muebles.
        ///PARAMETROS           : 
        ///                     1.  Parametros.Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 14/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static void Actualizar_Estatus_Bienes(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Parametros) {
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
                String Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                if (Parametros.P_Estatus != null) {
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = '" + Parametros.P_Estatus + "'";
                } else {
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Muebles.Campo_Estatus + " = ''";
                }
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = '" + Parametros.P_Bien_Mueble_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
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
                    Mensaje = "Error al intentar Modificar el Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }
        
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar_Estatus_Bienes
        ///DESCRIPCIÓN          : Actualiza los Estatus de los Bienes Muebles.
        ///PARAMETROS           : 
        ///                     1.  Parametros.Contiene los parametros que se van a dar de
        ///                                     Alta en la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 14/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static void Actualizar_Bienes_Migracion(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Parametros) {
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
                if (Parametros.P_Bien_Mueble_ID != null && Parametros.P_Bien_Mueble_ID.Trim().Length > 0) { 
                    String Mi_SQL = "UPDATE OPE_PAT_BIENES_MUEBLES SET";
                    Mi_SQL = Mi_SQL + " OBSERVACIONES = '" + Parametros.P_Observaciones + "'";
                    Mi_SQL = Mi_SQL + ", USUARIO_MODIFICO = '" + Parametros.P_Usuario_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", FECHA_MODIFICO = '" + String.Format("{0:dd/MM/yyyy}", Parametros.P_Fecha_Creo) + "'";
                    Mi_SQL = Mi_SQL + " WHERE BIEN_MUEBLE_ID = '" + Parametros.P_Bien_Mueble_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                }
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
                    Mensaje = "Error al intentar Modificar el Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Empleados_Resguardos
        ///DESCRIPCIÓN          : Obtiene empleados de la Base de Datos y los regresa en un 
        ///                       DataTable de acuerdo a los filtros pasados.
        ///PARAMETROS           : Parametros.  Contiene los parametros que se van a utilizar para
        ///                                         hacer la consulta de la Base de Datos.
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 24/Octubre/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static DataTable Consultar_Empleados_Resguardos(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Parametros) {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            try {
                Mi_SQL = "SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " AS NO_EMPLEADO";
                Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") AS NOMBRE";
                Mi_SQL = Mi_SQL + ", TRIM(" + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + "";
                Mi_SQL = Mi_SQL + " ||' - '|| " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") AS DEPENDENCIA";
                Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " AS ESTATUS";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " IN ('" + Parametros.P_Estatus_Empleado + "')";
                Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Empleado + " = 'EMPLEADO'";
                if (Parametros.P_Dependencia_ID != null && Parametros.P_Dependencia_ID.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID.Trim() + "'";
                }
                if (Parametros.P_RFC_Resguardante != null && Parametros.P_RFC_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + " = '" + Parametros.P_RFC_Resguardante.Trim() + "'";
                }
                if (Parametros.P_No_Empleado_Resguardante != null && Parametros.P_No_Empleado_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Convertir_A_Formato_ID(Convert.ToInt32(Parametros.P_No_Empleado_Resguardante.Trim()), 6) + "'";
                }
                if (Parametros.P_Nombre_Resguardante != null && Parametros.P_Nombre_Resguardante.Trim().Length > 0) {
                    Mi_SQL = Mi_SQL + " AND (TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") LIKE '%" + Parametros.P_Nombre_Resguardante.Trim() + "%'";
                    Mi_SQL = Mi_SQL + " OR TRIM(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + ") LIKE '%" + Parametros.P_Nombre_Resguardante.Trim() + "%')";
                }
                Mi_SQL = Mi_SQL + " ORDER BY NOMBRE";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null) {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            } catch (Exception Ex) {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Bien_Mueble
        ///DESCRIPCIÓN: Actualiza en la Base de Datos un Bien Mueble.
        ///PARAMETROS:     
        ///             1. Bien_Mueble. Contiene los parametros para actualizar el registro en la Base de Datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO:     27/Noviembre/2010 
        ///MODIFICO:       Salvador Hernández Ramírez
        ///FECHA_MODIFICO  04/Febrero/2011
        ///CAUSA_MODIFICACIÓN  Se implemento el metodo "Alta_Bitacora" para registrar los Insert y Update en la Base de Datos
        ///*******************************************************************************
        public static void Modificar_Bien_Mueble_Secundarios(Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble) {
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
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal_1 = Consultar_Detalles_Bien_Mueble(Bien_Mueble);
                String Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Muebles.Campo_Bien_Parent_ID + " = '" + Bien_Mueble.P_Ascencendia + "'";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Muebles.Campo_Proveniente + " = '" + Bien_Mueble.P_Proveniente + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Muebles.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " = '" + Bien_Mueble.P_Bien_Mueble_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Mueble.P_Bien_Mueble_ID, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD

                if (Bien_Mueble.P_Operacion == "RESGUARDO") {
                    if (Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE")) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Bien_Mueble);

                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";

                            String Bien_Resguardado_Id = "" + Temporal.P_Resguardantes.Rows[Contador][0].ToString(); // Esta asignacion se realiza para guardar el identificador del bien resguardado en la tabla de bitacoras
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardado_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD

                        }

                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Bien_Mueble, Temporal_1);

                        //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                        if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                            String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                            for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Modificacion + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_Mueble.P_Bien_Mueble_ID + "', 'BIEN_MUEBLE','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Estado + "','" + Bien_Mueble.P_Dependencia_ID + "'";
                                Mi_SQL = Mi_SQL + ",'SI','" + Bien_Mueble.P_Observaciones + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                                ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", ID_Consecutivo, Mi_SQL); // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                            }
                        }
                    } else {
                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                            String Bien_Resguardo_Id = "" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString();
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardo_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }
                    }
                } else if (Bien_Mueble.P_Operacion == "RECIBO") {

                    if (Bien_Mueble.P_Estatus.Trim().Equals("VIGENTE"))  {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Bien_Mueble);

                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";

                            String Bien_Resguardado_Id = "" + Temporal.P_Resguardantes.Rows[Contador][0].ToString(); // Esta asignacion se realiza para guardar el identificador del bien resguardado en la tabla de bitacoras
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardado_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }

                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Bien_Mueble, Temporal_1);

                        //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                        if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                            String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos, Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID, 50);
                            for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + ", " + Ope_Pat_Bienes_Recibos.Campo_Bien_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Tipo + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Recibos.Campo_Empleado_Almacen_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Estatus + ", " + Ope_Pat_Bienes_Recibos.Campo_Comentarios;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", " + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + ", " + Ope_Pat_Bienes_Resguardos.Campo_Observaciones;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Recibos.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Bien_Mueble.P_Bien_Mueble_ID + "', 'BIEN_MUEBLE','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                                Mi_SQL = Mi_SQL + ", '" + Bien_Mueble.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Estado + "','" + Bien_Mueble.P_Dependencia_ID + "'";
                                Mi_SQL = Mi_SQL + ",'SI','" + Bien_Mueble.P_Observaciones + "'";
                                Mi_SQL = Mi_SQL + ",'" + Bien_Mueble.P_Usuario_Nombre + "', SYSDATE)";
                                ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);

                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", ID_Consecutivo, Mi_SQL); // Se da de alta el insert en la tabla "APL_BITACORA" de la BD
                            }
                        }
                    } else {
                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Baja + " = 'SI'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Bien_Mueble.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                            String Bien_Resguardo_Id = "" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString();
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Bienes_Muebles.aspx", Bien_Resguardo_Id, Mi_SQL); // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }
                    }


                }
                Trans.Commit();
                Actualizar_Bienes_Muebles_Dependientes(Bien_Mueble);
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
                    Mensaje = "Error al intentar Modificar el Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            } finally {
                Cn.Close();
            }
        }

    }

}