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
using Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using System.Collections.Generic;
/// <summary>
/// Summary description for Cls_Ope_Pat_Com_Partes_Vehiculos_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Operacion_Partes_Vehiculos.Datos {

    public class Cls_Ope_Pat_Com_Partes_Vehiculos_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Parte_Vehiculo
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo registro 
            ///                       de Parte Vehiculo.
            ///PARAMETROS           : 
            ///                    1.  Parte.   Contiene los parametros que se van a dar de
            ///                                     Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 02/Febrero/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Alta_Parte(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte)
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
                String Parte_ID = Obtener_ID_Consecutivo(Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos, Ope_Pat_Partes_Vehiculos.Campo_Parte_ID, 50);
                try {
                    //Se guardan el registro de Parte en la Base de Datos

                    Int64 No_Invetario = Consulta_Consecutivo_Inventario();
                    Parte.P_Numero_Inventario = No_Invetario.ToString();


                    String Mi_SQL = "INSERT INTO " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos;
                    Mi_SQL = Mi_SQL + " (" + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Nombre + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Marca_ID + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Modelo + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Costo +
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Material_ID +
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Color_ID +
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Cantidad + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Estado + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Estatus + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Comentarios + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Producto_ID + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Usuario_Creo + 
                                      ", " + Ope_Pat_Partes_Vehiculos.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Convert.ToInt32(Parte_ID) + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + ",'" + Parte.P_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Marca + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Modelo + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Costo + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Material + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Color + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Cantidad + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Estado + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Numero_Inventario + "'";
                    Mi_SQL = Mi_SQL + ",'" + Parte.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Comentarios + "'";
                    Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Parte.P_Fecha_Adquisicion) + "'";
                    Mi_SQL = Mi_SQL + ",'" + Parte.P_Producto_ID + "'";
                    Mi_SQL = Mi_SQL + ", '" + Parte.P_Usuario_Nombre + "'";
                    Mi_SQL = Mi_SQL + ", SYSDATE)"; 
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    //Se guardan sus resguardantes
                    if (Parte.P_Resguardantes != null && Parte.P_Resguardantes.Rows.Count > 0) {
                        String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                        for (Int32 Cnt = 0; Cnt < Parte.P_Resguardantes.Rows.Count; Cnt++) {
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                            Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Convert.ToInt32(Parte_ID) + "', 'PARTE_VEHICULO','" + Parte.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                            Mi_SQL = Mi_SQL + ", '" + Parte.P_Usuario_ID + "', 'VIGENTE', '" + Parte.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                            Mi_SQL = Mi_SQL + ",'" + Parte.P_Usuario_Nombre + "', SYSDATE)";
                            ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                    }


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
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES(" + No_Invetario + ", ";
                    Mi_SQL = Mi_SQL + No_Invetario + ", ";
                    Mi_SQL = Mi_SQL + "'" + Parte.P_Producto_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Parte.P_Marca + "', ";
                    Mi_SQL = Mi_SQL + "'" + Parte.P_Modelo + "', ";
                    Mi_SQL = Mi_SQL + "'', ";
                    Mi_SQL = Mi_SQL + "'" + Parte.P_Color + "', ";
                    Mi_SQL = Mi_SQL + "'" + Parte.P_Material + "', ";
                    Mi_SQL = Mi_SQL + "'', ";
                    Mi_SQL = Mi_SQL + 0 + ", ";
                    Mi_SQL = Mi_SQL + "'" + Parte.P_Usuario_Nombre + "', ";
                    Mi_SQL = Mi_SQL + "SYSDATE)";

                    Trans.Commit();
                    Parte.P_Parte_ID = Convert.ToInt32(Parte_ID);
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
                        Mensaje = "Error al intentar dar de Alta una Parte del Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
                return Parte;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Parte
            ///DESCRIPCIÓN: Actualiza en la Base de Datos de Parte Vehiculo.
            ///PARAMETROS:     
            ///             1. Parte. Contiene los parametros para actualizar el registro
            ///                         en la Base de Datos.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Marzo/2010 
            ///MODIFICO             : 
            ///FECHA_MODIFICO       : 
            ///CAUSA_MODIFICACIÓN   : 
            ///*******************************************************************************
            public static void Modificar_Parte(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte) {
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
                    Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Temporal_1 = Consultar_Datos_Parte_Vehiculo(Parte);
                    Mi_SQL = "UPDATE " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario + " = '" + Parte.P_Numero_Inventario + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID + " = '" + Parte.P_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Cantidad + " = " + Parte.P_Cantidad;
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Material_ID + " = '" + Parte.P_Material + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Color_ID + " = '" + Parte.P_Color + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Costo + " = '" + Parte.P_Costo + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion + " = '" + String.Format("{0:dd/MM/yyyy}", Parte.P_Fecha_Adquisicion) + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Estado + " = '" + Parte.P_Estado + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Estatus + " = '" + Parte.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Comentarios + " = '" + Parte.P_Comentarios + "'";
                    if (!Parte.P_Estatus.Trim().Equals("VIGENTE")) { 
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Campo_Motivo_Baja + " = '" + Parte.P_Motivo_Baja + "'";
                    }
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID + " = '" + Parte.P_Parte_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    if (Parte.P_Estatus.Trim().Equals("VIGENTE")) {
                        Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Temporal = Obtener_Diferencia_Resguardos(Temporal_1, Parte);

                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Parte.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }

                        Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Temporal_2 = Obtener_Diferencia_Resguardos(Parte, Temporal_1);

                        //SE DAN DE ALTA LOS NUEVOS RESGUARDANTES
                        if (Temporal_2.P_Resguardantes != null && Temporal_2.P_Resguardantes.Rows.Count > 0) {
                            String ID_Consecutivo = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos, Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID, 50);
                            for (Int32 Cnt = 0; Cnt < Temporal_2.P_Resguardantes.Rows.Count; Cnt++) {
                                Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                                Mi_SQL = Mi_SQL + " (" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", " + Ope_Pat_Bienes_Resguardos.Campo_Comentarios;
                                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ")";
                                Mi_SQL = Mi_SQL + " VALUES (" + Convert.ToInt32(ID_Consecutivo) + ",'" + Parte.P_Parte_ID + "', 'PARTE_VEHICULO','" + Temporal_2.P_Resguardantes.Rows[Cnt][1].ToString() + "', SYSDATE";
                                Mi_SQL = Mi_SQL + ", '" + Parte.P_Usuario_ID + "', 'VIGENTE', '" + Temporal_2.P_Resguardantes.Rows[Cnt][3].ToString() + "'";
                                Mi_SQL = Mi_SQL + ",'" + Parte.P_Usuario_Nombre + "', SYSDATE)";
                                ID_Consecutivo = Convertir_A_Formato_ID(Convert.ToInt32(ID_Consecutivo) + 1, 50);
                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                            }
                        }
                    } else {
                        //SE DAN DE BAJA LOS RESGURADANTES ANTERIORES
                        for (Int32 Contador = 0; Contador < Temporal_1.P_Resguardantes.Rows.Count; Contador++) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Modifico + " = '" + Parte.P_Usuario_Nombre + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " = '" + Temporal_1.P_Resguardantes.Rows[Contador][0].ToString() + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
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
                        Mensaje = "Error al intentar Modificar la Parte. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Parte_Vehiculo
            ///DESCRIPCIÓN: Obtiene los Datos a Detalle de una Parte en Especifico.
            ///PARAMETROS:   
            ///             1. Parametros.   Parte que se va a ver a Detalle.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Consultar_Datos_Parte_Vehiculo(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parametros) {
                String Mi_SQL = "SELECT * FROM " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID + " = '" + Parametros.P_Parte_ID + "'"; 
                Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                OracleDataReader Data_Reader;
                try
                {
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Parte.P_Parte_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Parte_ID].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Parte_ID]) : 0;
                        Parte.P_Vehiculo_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID].ToString() : "";
                        Parte.P_Producto_ID = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Producto_ID].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Producto_ID].ToString() : "";
                        Parte.P_Nombre = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Nombre].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Nombre].ToString() : "";
                        Parte.P_Marca = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Marca_ID].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Marca_ID].ToString() : "";
                        Parte.P_Modelo = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Modelo].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Modelo].ToString() : "";
                        Parte.P_Costo = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Costo].ToString())) ? Convert.ToDouble(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Costo]) : 0.0;
                        Parte.P_Material = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Material_ID].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Material_ID].ToString() : "";
                        Parte.P_Color = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Color_ID].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Color_ID].ToString() : "";
                        Parte.P_Cantidad = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Cantidad].ToString())) ? Convert.ToInt32(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Cantidad]) : 0;
                        Parte.P_Estado = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estado].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estado].ToString() : "";
                        Parte.P_Numero_Inventario = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario].ToString() : "";
                        Parte.P_Estatus = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estatus].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estatus].ToString() : "";
                        Parte.P_Comentarios = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Comentarios].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Comentarios].ToString() : "";
                        Parte.P_Motivo_Baja = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Motivo_Baja].ToString())) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Motivo_Baja].ToString() : "";
                        Parte.P_Fecha_Adquisicion = (!string.IsNullOrEmpty(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion].ToString()))? Convert.ToDateTime(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion]) : new DateTime();
                    }
                    Data_Reader.Close();
                    DataSet Ds_Partes = null;
                    if (Parte.P_Parte_ID != (-1) && Parte.P_Parte_ID > 0) {
                        Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                        Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                        Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = '" + Parte.P_Parte_ID + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                        Mi_SQL = Mi_SQL + " = 'PARTE_VEHICULO'";
                        Ds_Partes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Partes == null) {
                        Parte.P_Resguardantes = new DataTable();
                    }
                    else {
                        Parte.P_Resguardantes = Ds_Partes.Tables[0];
                    }
                    Ds_Partes = null;
                    if (Parte.P_Parte_ID != null && Parte.P_Parte_ID > (-1)) {
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
                        Mi_SQL = Mi_SQL + " = 'PARTE_VEHICULO'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                        Mi_SQL = Mi_SQL + " = " + Parte.P_Parte_ID + "";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Final;
                        Ds_Partes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Partes == null) {
                        Parte.P_Historial_Resguardos = new DataTable();
                    } else {
                        Parte.P_Historial_Resguardos = Ds_Partes.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los datos de la Parte. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Parte;
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Listado_Partes_Vehiculo
            ///DESCRIPCIÓN: Obtiene los Datos a Detalle de una Parte en Especifico.
            ///PARAMETROS:   
            ///             1. Parametros.   Parte que se va a ver a Detalle.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 02/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static List<Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio> Consultar_Listado_Partes_Vehiculo(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parametros) {
                List<Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio> Listado_Partes = new List<Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio>();

                String Mi_SQL = "SELECT * FROM " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID + " = '" + Parametros.P_Vehiculo_ID + "'";
                
                OracleDataReader Data_Reader;
                try {
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    while (Data_Reader.Read()) {
                        Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();
                        Parte.P_Parte_ID = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Parte_ID] != null) ? Convert.ToInt32(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Parte_ID]) : 0;
                        Parte.P_Vehiculo_ID = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID].ToString() : "";
                        Parte.P_Nombre = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Nombre] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Nombre].ToString() : "";
                        Parte.P_Marca = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Marca_ID] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Marca_ID].ToString() : "";
                        Parte.P_Modelo = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Modelo_ID] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Modelo_ID].ToString() : "";
                        Parte.P_Costo = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Costo] != null) ? Convert.ToDouble(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Costo]) : 0.0;
                        Parte.P_Material = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Material_ID] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Material_ID].ToString() : "";
                        Parte.P_Color = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Color_ID] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Color_ID].ToString() : "";
                        Parte.P_Cantidad = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Cantidad] != null) ? Convert.ToInt32(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Cantidad]) : 0;
                        Parte.P_Estado = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estado] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estado].ToString() : "";
                        Parte.P_Numero_Inventario = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario].ToString() : "";
                        Parte.P_Estatus = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estatus] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Estatus].ToString() : "";
                        Parte.P_Comentarios = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Comentarios] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Comentarios].ToString() : "";
                        Parte.P_Motivo_Baja = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Motivo_Baja] != null) ? Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Motivo_Baja].ToString() : "";
                        Parte.P_Fecha_Adquisicion = (Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion] != null) ? Convert.ToDateTime(Data_Reader[Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion]) : new DateTime();
                        DataSet Ds_Partes = null;
                        if (Parte.P_Parte_ID != (-1) && Parte.P_Parte_ID > 0) {
                            Mi_SQL = "SELECT " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + " AS BIEN_RESGUARDO_ID";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " AS EMPLEADO_ID";
                            Mi_SQL = Mi_SQL + ", " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "";
                            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "";
                            Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO";
                            Mi_SQL = Mi_SQL + "," + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " AS COMENTARIOS";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + ", " + Cat_Empleados.Tabla_Cat_Empleados;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID;
                            Mi_SQL = Mi_SQL + " = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID;
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID;
                            Mi_SQL = Mi_SQL + " = '" + Parte.P_Parte_ID + "'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Estatus;
                            Mi_SQL = Mi_SQL + " = 'VIGENTE'";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + "." + Ope_Pat_Bienes_Resguardos.Campo_Tipo;
                            Mi_SQL = Mi_SQL + " = 'PARTE_VEHICULO'";
                            Ds_Partes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        }
                        if (Ds_Partes == null) {
                            Parte.P_Resguardantes = new DataTable();
                        } else {
                            Parte.P_Resguardantes = Ds_Partes.Tables[0];
                        }
                        Listado_Partes.Add(Parte);
                    }
                    Data_Reader.Close();
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los datos de la Parte. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Listado_Partes;
            }            

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Parte
            ///DESCRIPCIÓN          : Elimina un Registro de una Parte de Vehiculo
            ///PARAMETROS           : 
            ///                     1.  Parte.    Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 02/Febrero/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Parte(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte)
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
                    //Se eliminan sus registros de resguardo
                    String Mi_SQL = "DELETE FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " = '" + Parte.P_Parte_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'PARTE_VEHICULO'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    //Se elimina el registro de la base de datos del Campo de Partes
                    Mi_SQL = "DELETE FROM " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID + " = '" + Parte.P_Parte_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar la Parte del Vehiculo. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar la Parte del Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Listar_Productos_Partes
            ///DESCRIPCIÓN          : Hace una listado de los productos que cumplen con ciertos
            ///                         Filtros.
            ///PARAMETROS           : 
            ///                     1.  Parte.    Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 01/Marzo/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Listar_Productos_Partes(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte) { 
                String Mi_SQL = null;
                DataSet Ds_Productos = null;
                DataTable Dt_Productos = new DataTable();
                try {
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " AS PRODUCTO_ID";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " AS CLAVE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + ", 'INDISTINTO' AS MARCA";
                    Mi_SQL = Mi_SQL + ", 'INDISTINTO' AS MODELO";
                    //Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA";
                    //Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;// +", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;
                    Mi_SQL = Mi_SQL + " WHERE " /*+ Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " AND "*/ + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Tipo;
                    Mi_SQL = Mi_SQL + " = 'PARTE_VEHICULO'";
                    if (Parte.P_Clave_Interna != null && Parte.P_Clave_Interna.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Parte.P_Clave_Interna.Trim() + "%'";
                    }
                    if (Parte.P_Nombre != null && Parte.P_Nombre.Trim().Length > 0) { 
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Parte.P_Nombre.Trim() + "%'";
                    }
                    //if (Parte.P_Marca != null && Parte.P_Marca.Trim().Length > 0) { 
                    //    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                    //    Mi_SQL = Mi_SQL + " = '" + Parte.P_Marca.Trim() + "'";
                    //}
                    //if (Parte.P_Modelo != null && Parte.P_Modelo.Trim().Length > 0) { 
                    //    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                    //    Mi_SQL = Mi_SQL + " = '" + Parte.P_Modelo.Trim() + "'";
                    //}
                    Ds_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Productos != null) {
                        Dt_Productos = Ds_Productos.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Productos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
            ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
            ///PARAMETROS:     
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
            ///PARAMETROS:     
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
                for (Int32 Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Listado_Partes_Vehiculos
            ///DESCRIPCIÓN          : Hace una listado de los productos que cumplen con ciertos
            ///                         Filtros.
            ///PARAMETROS           : 
            ///                     1.  Parte.    Contiene los parametros que se van a utilizar para
            ///                                     hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 07/Marzo/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Listado_Partes_Vehiculos(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Parte) {
                String Mi_SQL = null;
                DataSet Ds_Partes = null;
                DataTable Dt_Partes = new DataTable();
                try {
                    Mi_SQL = "SELECT " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID + " AS PARTE_ID";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario + " AS PARTE_CLAVE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Nombre + " AS PARTE_NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA_NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Modelo + " AS MODELO_NOMBRE";
                    Mi_SQL = Mi_SQL + ", " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Estatus + " AS ESTATUS";
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + ", " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                    if (Parte.P_Numero_Inventario != null && Parte.P_Numero_Inventario.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Numero_Inventario;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Parte.P_Numero_Inventario + "%'";
                    }
                    if (Parte.P_Numero_Inventario_Vehiculo != null && Parte.P_Numero_Inventario_Vehiculo.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Numero_Inventario + " LIKE '%" + Parte.P_Numero_Inventario_Vehiculo + "%')";
                    }
                   if (Parte.P_Marca != null && Parte.P_Marca.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Marca_ID;
                        Mi_SQL = Mi_SQL + " = '" + Parte.P_Marca + "'";
                    }
                    if (Parte.P_Modelo != null && Parte.P_Modelo.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Modelo;
                        Mi_SQL = Mi_SQL + " LIKE '%" + Parte.P_Modelo + "%'";
                    }
                    if (Parte.P_Material != null && Parte.P_Material.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Material_ID;
                        Mi_SQL = Mi_SQL + " = '" + Parte.P_Material + "'";
                    }
                    if (Parte.P_Color != null && Parte.P_Color.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Color_ID;
                        Mi_SQL = Mi_SQL + " = '" + Parte.P_Color + "'";
                    }
                    if (Parte.P_Buscar_Fecha_Adquisicion) {
                        Mi_SQL = Mi_SQL + " AND (" + Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion + " >= '" + String.Format("{0:dd/MM/yyyy}", Parte.P_Fecha_Adquisicion) + "'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Campo_Fecha_Adquisicion + " < '" + String.Format("{0:dd/MM/yyyy}", (Parte.P_Fecha_Adquisicion).AddDays(1).Date) + "')";
                    }
                    if (Parte.P_Estatus != null && Parte.P_Estatus.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Estatus;
                        Mi_SQL = Mi_SQL + " = '" + Parte.P_Estatus + "'";
                    }
                    if (Parte.P_Dependencia_ID != null && Parte.P_Dependencia_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Vehiculos.Campo_Vehiculo_ID + " FROM " + Ope_Pat_Vehiculos.Tabla_Ope_Pat_Vehiculos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculos.Campo_Dependencia_ID + " LIKE '%" + Parte.P_Dependencia_ID + "%')";
                    }
                    if (Parte.P_RFC_Resguardante != null && Parte.P_RFC_Resguardante.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID;
                        Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'PARTE_VEHICULO' AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " IN ( SELECT " + Cat_Empleados.Campo_Empleado_ID;
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_RFC + " LIKE '%" + Parte.P_RFC_Resguardante + "%'))";
                    }
                    if (Parte.P_Resguardante_ID != null && Parte.P_Resguardante_ID.Trim().Length > 0) {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Partes_Vehiculos.Tabla_Ope_Pat_Partes_Vehiculos + "." + Ope_Pat_Partes_Vehiculos.Campo_Parte_ID;
                        Mi_SQL = Mi_SQL + " IN ( SELECT " + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'PARTE_VEHICULO' AND " + Ope_Pat_Bienes_Resguardos.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " = '" + Parte.P_Resguardante_ID + "')";                            
                    }
                    Ds_Partes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if (Ds_Partes != null) {
                        Dt_Partes = Ds_Partes.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Partes;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Obtener_Diferencia_Resguardos
            ///DESCRIPCIÓN: Saca la diferencia de unos resguardantes a otros.
            ///PARAMETROS:     
            ///             1. Actuales.        Parte como esta actualmente en la Base de Datos.
            ///             2. Actualizados.    Parte como quiere que quede al Actualizarlo.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 09/Marzo/2010 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private static Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Obtener_Diferencia_Resguardos(Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Comparar, Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Base_Comparacion) {
                Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio Resguardos = new Cls_Ope_Pat_Com_Partes_Vehiculos_Negocio();

                DataTable Dt_Tabla = new DataTable();
                Dt_Tabla.Columns.Add("BIEN_RESGUARDO_ID", Type.GetType("System.Int32"));
                Dt_Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Dt_Tabla.Columns.Add("NOMBRE_EMPLEADO", Type.GetType("System.String"));
                Dt_Tabla.Columns.Add("COMENTARIOS", Type.GetType("System.String"));
                for (int Contador_1 = 0; Contador_1 < Comparar.P_Resguardantes.Rows.Count; Contador_1++) {
                    Boolean Eliminar = true;
                    for (int Contador_2 = 0; Contador_2 < Base_Comparacion.P_Resguardantes.Rows.Count; Contador_2++) {
                        if (Comparar.P_Resguardantes.Rows[Contador_1][1].ToString().Equals(Base_Comparacion.P_Resguardantes.Rows[Contador_2][1].ToString())) {
                            Eliminar = false;
                            break;
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
                Resguardos.P_Resguardantes = Dt_Tabla;
                return Resguardos;
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
            public static Int64 Consulta_Consecutivo_Inventario()
            {
                String Mi_SQL = String.Empty; //Variable para las consultas
                Object Consecutivo;
                Int64 No_Consecutivo;

                try
                {
                    // Consulta 
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + "), 0) ";
                    Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;

                    //Ejecutar consulta
                    Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Verificar si no es nulo
                    if (Consecutivo != null && Convert.IsDBNull(Consecutivo) == false)
                        No_Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                    else
                        No_Consecutivo = 1;

                    return No_Consecutivo;
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