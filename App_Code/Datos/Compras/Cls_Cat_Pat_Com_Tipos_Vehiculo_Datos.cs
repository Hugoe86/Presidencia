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
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Datos
{

    public class Cls_Cat_Pat_Com_Tipos_Vehiculo_Datos {

        #region Metodos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Tipo_Vehiculo
            ///DESCRIPCIÓN          : Da de alta en la Base de Datos una nuevo Tipo de Vehiculo
            ///PARAMETROS           : 
            ///                         1.  Tipo_Vehiculo.  Contiene los parametros que se van a dar de
            ///                                             Alta en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             : Francisco Antonio Gallardo Castañeda
            ///FECHA_MODIFICO       : 11/Marzo/2011 
            ///CAUSA_MODIFICACIÓN   : Se le agrego la parte de Aseguradora
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Alta_Tipo_Vehiculo(Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo) {
                String Mensaje = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                String Tipo_Vehiculo_ID = Obtener_ID_Consecutivo(Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo, Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID, 5);
                try {
                    String Mi_SQL = "INSERT INTO " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " (" + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Estatus + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Usuario_Creo + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Fecha_Creo;
                    Mi_SQL = Mi_SQL + ") VALUES ('" + Tipo_Vehiculo_ID + "', '" + Tipo_Vehiculo.P_Descripcion + "','" + Tipo_Vehiculo.P_Estatus + "','" + Tipo_Vehiculo.P_Aseguradora_ID + "'";
                    Mi_SQL = Mi_SQL + ",'" + Tipo_Vehiculo.P_Usuario + "', SYSDATE)"; 
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    if (!String.IsNullOrEmpty(Tipo_Vehiculo.P_Aseguradora_ID)) {
                        String Vehiculo_Aseguradora_ID = Obtener_ID_Consecutivo(Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora, Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID, 50);
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                        Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza;
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Creo + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Creo;
                        Mi_SQL = Mi_SQL + ") VALUES (" + Convert.ToInt32(Vehiculo_Aseguradora_ID) + ", '" + Tipo_Vehiculo.P_Aseguradora_ID + "', '" + Tipo_Vehiculo_ID + "'";
                        Mi_SQL = Mi_SQL + ", '" + Tipo_Vehiculo.P_Descripcion_Seguro + "', '" + Tipo_Vehiculo.P_Cobertura_Seguro + "', '" + Tipo_Vehiculo.P_No_Poliza_Seguro + "'";
                        Mi_SQL = Mi_SQL + ", 'VIGENTE', '" + Tipo_Vehiculo.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Tipo_Vehiculo.P_Nombre_Fisico_Archivo != null && Tipo_Vehiculo.P_Nombre_Fisico_Archivo.Trim().Length > 0){
                        Int32 Archivo_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pat_Tipo_Vehiculo_Archivo.Tabla_Cat_Pat_Tipo_Vehiculo_Archivo, Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Archivo_ID, 50));
                        Tipo_Vehiculo.P_Nombre_Fisico_Archivo = Archivo_ID.ToString() + Tipo_Vehiculo.P_Nombre_Fisico_Archivo;
                        Mi_SQL = "INSERT INTO " + Cat_Pat_Tipo_Vehiculo_Archivo.Tabla_Cat_Pat_Tipo_Vehiculo_Archivo;
                        Mi_SQL = Mi_SQL + " ( " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Archivo_ID + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Tipo_Vehiculo_ID + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Nombre;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Comentarios + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Fecha + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Nombre_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Usuario_Creo + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Fecha_Creo + " ) VALUES";
                        Mi_SQL = Mi_SQL + " ( '" + Archivo_ID + "', '" + Tipo_Vehiculo_ID + "', '" + Tipo_Vehiculo.P_Descripcion_Archivo + "', '" + Tipo_Vehiculo.P_Comentarios_Archivo + "'";
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Tipo_Vehiculo.P_Fecha) + "', '" + Tipo_Vehiculo.P_Nombre_Fisico_Archivo + "',  '" + Tipo_Vehiculo.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Tipo_Vehiculo.P_Dt_Detalles != null && Tipo_Vehiculo.P_Dt_Detalles.Rows.Count > 0) {
                        Int32 Registro_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles, Cat_Pat_Tipo_Veh_Detalles.Campo_Registro_ID, 50));
                        for (Int32 Contador = 0; Contador < (Tipo_Vehiculo.P_Dt_Detalles.Rows.Count); Contador++) {
                            Mi_SQL = "INSERT INTO " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + " ( " + Cat_Pat_Tipo_Veh_Detalles.Campo_Registro_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Veh_Detalles.Campo_Detalle_Vehiculo_ID + ", " + Cat_Pat_Tipo_Veh_Detalles.Campo_Tipo_Vehiculo_ID + ") ";
                            Mi_SQL = Mi_SQL + " VALUES ( '" + Registro_ID + "','" + Tipo_Vehiculo.P_Dt_Detalles.Rows[Contador]["DETALLE_ID"].ToString() + "','" + Tipo_Vehiculo_ID + "')";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            Registro_ID = Registro_ID + 1;
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
                        Mensaje = "Error al intentar dar de Alta un Tipo de Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
                return Tipo_Vehiculo;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modificar_Tipo_Vehiculo
            ///DESCRIPCIÓN          : Modifica en la Base de Datos el registro indicado del Tipo de Vehiculo
            ///PARAMETROS           : 
            ///                         1.  Tipo_Vehiculo.  Contiene los parametros que se van hacer la
            ///                                             Modificación en la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             : Francisco Antonio Gallardo Castañeda
            ///FECHA_MODIFICO       : 11/Marzo/2011 
            ///CAUSA_MODIFICACIÓN   : Se le agrego la parte de Aseguradora
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Modificar_Tipo_Vehiculo(Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo)
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
                    String Mi_SQL = "UPDATE " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                    Mi_SQL = Mi_SQL + " SET " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " = '" + Tipo_Vehiculo.P_Descripcion + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Estatus + " = '" + Tipo_Vehiculo.P_Estatus + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " = '" + Tipo_Vehiculo.P_Aseguradora_ID + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Usuario_Modifico + " = '" + Tipo_Vehiculo.P_Usuario + "'";
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Fecha_Modifico + " = SYSDATE";
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    if (!String.IsNullOrEmpty(Tipo_Vehiculo.P_Aseguradora_ID)) {  
                        Boolean Aseguradora_Nueva = Nueva_Aseguradora(Tipo_Vehiculo);
                        if (Aseguradora_Nueva) {
                            Mi_SQL = "UPDATE " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'BAJA'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Modifico + " = '" + Tipo_Vehiculo.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + "=" + Tipo_Vehiculo.P_Vehiculo_Aseguradora_ID + "";
                            Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + "= 'VIGENTE'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Convert.ToString(Vehiculo.P_Vehiculo_Aseguradora_ID), Mi_SQL);  // Se da de alta el update en la tabla "APL_BITACORA" de la BD

                            String Vehiculo_Aseguradora_ID = Obtener_ID_Consecutivo(Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora, Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID, 50);
                            Mi_SQL = "INSERT INTO " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                            Mi_SQL = Mi_SQL + " (" + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza;
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Creo + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Creo;
                            Mi_SQL = Mi_SQL + ") VALUES (" + Convert.ToInt32(Vehiculo_Aseguradora_ID) + ", '" + Tipo_Vehiculo.P_Aseguradora_ID + "', '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                            Mi_SQL = Mi_SQL + ", '" + Tipo_Vehiculo.P_Descripcion_Seguro + "', '" + Tipo_Vehiculo.P_Cobertura_Seguro + "', '" + Tipo_Vehiculo.P_No_Poliza_Seguro + "'";
                            Mi_SQL = Mi_SQL + ", 'VIGENTE', '" + Tipo_Vehiculo.P_Usuario + "', SYSDATE)";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Vehiculo_Aseguradora_ID, Mi_SQL);  // Se da de alta el Insert en la tabla "APL_BITACORA" de la BD
                        } else {
                            Mi_SQL = "UPDATE " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                            Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza + " = '" + Tipo_Vehiculo.P_No_Poliza_Seguro + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + " = '" + Tipo_Vehiculo.P_Descripcion_Seguro + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + " = '" + Tipo_Vehiculo.P_Cobertura_Seguro + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Modifico + " = '" + Tipo_Vehiculo.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + "=" + Tipo_Vehiculo.P_Vehiculo_Aseguradora_ID + "";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Pat_Com_Actualizacion_Vehiculos.aspx", Convert.ToString(Vehiculo.P_Vehiculo_Aseguradora_ID), Mi_SQL);  // Se da de alta el update en la tabla "APL_BITACORA" de la BD
                        }
                    } else {
                        Mi_SQL = "UPDATE " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                        Mi_SQL = Mi_SQL + " SET " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'BAJA'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Usuario_Modifico + " = '" + Tipo_Vehiculo.P_Usuario + "'";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Fecha_Modifico + " = SYSDATE";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + "=" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + "= 'VIGENTE'";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }
                    if (Tipo_Vehiculo.P_Nombre_Fisico_Archivo != null && Tipo_Vehiculo.P_Nombre_Fisico_Archivo.Trim().Length > 0) {
                        Int32 Archivo_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pat_Tipo_Vehiculo_Archivo.Tabla_Cat_Pat_Tipo_Vehiculo_Archivo, Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Archivo_ID, 50));
                        Tipo_Vehiculo.P_Nombre_Fisico_Archivo = Archivo_ID.ToString() + Tipo_Vehiculo.P_Nombre_Fisico_Archivo;
                        Mi_SQL = "INSERT INTO " + Cat_Pat_Tipo_Vehiculo_Archivo.Tabla_Cat_Pat_Tipo_Vehiculo_Archivo;
                        Mi_SQL = Mi_SQL + " ( " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Archivo_ID + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Tipo_Vehiculo_ID + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Nombre;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Comentarios + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Fecha + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Nombre_Archivo;
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Usuario_Creo + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Fecha_Creo + " ) VALUES";
                        Mi_SQL = Mi_SQL + " ( '" + Archivo_ID + "', '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "', '" + Tipo_Vehiculo.P_Descripcion_Archivo + "', '" + Tipo_Vehiculo.P_Comentarios_Archivo + "'";
                        Mi_SQL = Mi_SQL + ", '" + String.Format("{0:dd/MM/yyyy}", Tipo_Vehiculo.P_Fecha) + "', '" + Tipo_Vehiculo.P_Nombre_Fisico_Archivo + "',  '" + Tipo_Vehiculo.P_Usuario + "', SYSDATE)";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                    }

                    Mi_SQL = "DELETE FROM " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + " WHERE " + Cat_Pat_Tipo_Veh_Detalles.Campo_Tipo_Vehiculo_ID + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    if (Tipo_Vehiculo.P_Dt_Detalles != null && Tipo_Vehiculo.P_Dt_Detalles.Rows.Count >0) {
                        Int32 Registro_ID = Convert.ToInt32(Obtener_ID_Consecutivo(Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles, Cat_Pat_Tipo_Veh_Detalles.Campo_Registro_ID, 50));
                        for (Int32 Contador = 0; Contador < (Tipo_Vehiculo.P_Dt_Detalles.Rows.Count); Contador++) {
                            Mi_SQL = "INSERT INTO " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + " ( " + Cat_Pat_Tipo_Veh_Detalles.Campo_Registro_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Veh_Detalles.Campo_Detalle_Vehiculo_ID + ", " + Cat_Pat_Tipo_Veh_Detalles.Campo_Tipo_Vehiculo_ID + ") ";
                            Mi_SQL = Mi_SQL + " VALUES ( '" + Registro_ID + "','" + Tipo_Vehiculo.P_Dt_Detalles.Rows[Contador]["DETALLE_ID"].ToString() + "','" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "')";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            Registro_ID = Registro_ID + 1;
                        }
                    }

                    Trans.Commit();
                } catch (OracleException Ex) {
                    Trans.Rollback();
                    //variable para el mensaje 
                    //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                    if (Ex.Code == 8152) {
                        Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar";
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
                        Mensaje = "Error al intentar Modificar un Tipo de Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    }
                    //Indicamos el mensaje 
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
                return Tipo_Vehiculo;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
            ///DESCRIPCIÓN          : Obtiene datos de la Base de Datos y los regresa en un DataTable.
            ///PARAMETROS           : 
            ///                         1.  Tipo_Vehiculo.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la consulta de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             : Francisco Antonio Gallardo Castañeda
            ///FECHA_MODIFICO       : 11/Marzo/2011 
            ///CAUSA_MODIFICACIÓN   : Se le agrego la parte de Aseguradora
            ///*******************************************************************************
            public static DataTable Consultar_DataTable(Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo)  {
                String Mi_SQL = null;
                DataSet Ds_Tipos_Vehiculo = null;
                DataTable Dt_Tipos_Vehiculo = new DataTable();
                try {
                    if (Tipo_Vehiculo.P_Tipo_DataTable.Equals("TIPOS_VEHICULOS")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " AS TIPO_VEHICULO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " AS DESCRIPCION";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Estatus + " AS ESTATUS";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipos_Vehiculo.Campo_Aseguradora_ID + " AS ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + " LIKE '%" + Tipo_Vehiculo.P_Descripcion + "%'";
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID;
                    }  else if (Tipo_Vehiculo.P_Tipo_DataTable.Equals("ASEGURADORAS")) {
                        Mi_SQL = "SELECT " + Cat_Pat_Aseguradora.Campo_Aseguradora_ID + " AS ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Aseguradora.Campo_Nombre_Fiscal + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Aseguradora.Tabla_Cat_Pat_Aseguradora;
                        Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Aseguradora.Campo_Nombre;
                    }
                    if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) {
                        Ds_Tipos_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                    if (Ds_Tipos_Vehiculo != null) {
                        Dt_Tipos_Vehiculo = Ds_Tipos_Vehiculo.Tables[0];
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Dt_Tipos_Vehiculo;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Tipos_Vehiculo
            ///DESCRIPCIÓN: Obtiene los Datos a Detalle de un Vehiculo en Especifico.
            ///PARAMETROS:   
            ///             1. Parametros.   Vehiculo que se va a ver a Detalle.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 14/Marzo/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Consultar_Datos_Tipos_Vehiculo(Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Parametros) {
                String Mi_SQL = "SELECT * FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Tipo_Vehiculo_ID + "'";
                Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
                DataSet Ds_Temporal = null;
                try {
                    OracleDataReader Data_Reader;
                    Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if(Data_Reader != null){
                        while (Data_Reader.Read()) {
                            Tipo_Vehiculo.P_Tipo_Vehiculo_ID = (Data_Reader[Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID] != null) ? Data_Reader[Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID].ToString() : "";
                            Tipo_Vehiculo.P_Descripcion = (Data_Reader[Cat_Pat_Tipos_Vehiculo.Campo_Descripcion] != null) ? Data_Reader[Cat_Pat_Tipos_Vehiculo.Campo_Descripcion].ToString() : "";
                            Tipo_Vehiculo.P_Estatus = (Data_Reader[Cat_Pat_Tipos_Vehiculo.Campo_Estatus] != null) ? Data_Reader[Cat_Pat_Tipos_Vehiculo.Campo_Estatus].ToString() : "";
                        }
                    }
                    if (Tipo_Vehiculo.P_Tipo_Vehiculo_ID != null && Tipo_Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                        Mi_SQL = "SELECT " + Ope_Pat_Vehiculo_Aseguradora.Campo_Vehiculo_Aseguradora_ID + " AS VEHICULO_ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + " AS ASEGURADORA_ID";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Descripcion_Seguro + " AS DESCRIPCION_SEGURO";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_Cobertura + " AS COBERTURA";
                        Mi_SQL = Mi_SQL + ", " + Ope_Pat_Vehiculo_Aseguradora.Campo_No_Poliza + " AS NO_POLIZA"; ;
                        Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'VIGENTE'";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                        Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if(Data_Reader != null) {
                            while (Data_Reader.Read()) {
                                Tipo_Vehiculo.P_Vehiculo_Aseguradora_ID = (Data_Reader["VEHICULO_ASEGURADORA_ID"] != null) ? Convert.ToInt32(Data_Reader["VEHICULO_ASEGURADORA_ID"]) : 0;
                                Tipo_Vehiculo.P_Aseguradora_ID = (Data_Reader["ASEGURADORA_ID"] != null) ? Data_Reader["ASEGURADORA_ID"].ToString() : "";
                                Tipo_Vehiculo.P_Descripcion_Seguro = (Data_Reader["DESCRIPCION_SEGURO"] != null) ? Data_Reader["DESCRIPCION_SEGURO"].ToString() : "";
                                Tipo_Vehiculo.P_Cobertura_Seguro = (Data_Reader["COBERTURA"] != null) ? Data_Reader["COBERTURA"].ToString() : "";
                                Tipo_Vehiculo.P_No_Poliza_Seguro = (Data_Reader["NO_POLIZA"] != null) ? Data_Reader["NO_POLIZA"].ToString() : "";
                            }
                        }
                        Data_Reader.Close();                        
                    }
                    if (Tipo_Vehiculo.P_Tipo_Vehiculo_ID != null && Tipo_Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0)  {
                        Mi_SQL = "SELECT " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Archivo_ID + " AS ARCHIVO_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Nombre_Archivo + " AS NOMBRE_ARCHIVO";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Fecha + " AS FECHA";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Comentarios + " AS COMENTARIOS";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Tipo_Vehiculo_Archivo.Tabla_Cat_Pat_Tipo_Vehiculo_Archivo;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipo_Vehiculo_Archivo.Campo_Tipo_Vehiculo_ID + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                        Ds_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Temporal != null) {
                            Tipo_Vehiculo.P_Dt_Archivos = Ds_Temporal.Tables[0];
                        }
                    }
                    Ds_Temporal = null;
                    if (Tipo_Vehiculo.P_Tipo_Vehiculo_ID != null && Tipo_Vehiculo.P_Tipo_Vehiculo_ID.Trim().Length > 0) {
                        Mi_SQL = "SELECT " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + "." + Cat_Pat_Tipo_Veh_Detalles.Campo_Detalle_Vehiculo_ID + " AS DETALLE_ID";
                        Mi_SQL = Mi_SQL + ", " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + "." + Cat_Pat_Det_Vehiculos.Campo_Nombre + " AS NOMBRE";
                        Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + ", " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles;
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Det_Vehiculos.Tabla_Cat_Pat_Det_Vehiculos + "." + Cat_Pat_Det_Vehiculos.Campo_Detalle_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + "." + Cat_Pat_Tipo_Veh_Detalles.Campo_Detalle_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " AND " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + "." + Cat_Pat_Tipo_Veh_Detalles.Campo_Tipo_Vehiculo_ID;
                        Mi_SQL = Mi_SQL + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                        Ds_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        if (Ds_Temporal != null) {
                            Tipo_Vehiculo.P_Dt_Detalles = Ds_Temporal.Tables[0];
                        }
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error al intentar consultar los datos del Tipo de Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                }
                return Tipo_Vehiculo;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Eliminar_Tipo_Vehiculo
            ///DESCRIPCIÓN          : Elimina un Registro de un Tipo de Vehiculo
            ///PARAMETROS           : 
            ///                         1.  Tipo_Vehiculo.  Contiene los parametros que se van a utilizar para
            ///                                             hacer la eliminacion de la Base de Datos.
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 23/Noviembre/2010 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static void Eliminar_Tipo_Vehiculo(Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Vehiculo)
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
                    String Mi_SQL = "DELETE FROM " + Cat_Pat_Tipo_Veh_Detalles.Tabla_Cat_Pat_Tipo_Veh_Detalles + " WHERE " + Cat_Pat_Tipo_Veh_Detalles.Campo_Tipo_Vehiculo_ID + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "DELETE FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID + " = '" + Tipo_Vehiculo.P_Tipo_Vehiculo_ID + "'";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Trans.Commit();
                } catch (OracleException Ex) {
                    if (Ex.Code == 547) {
                        Mensaje = "No se puede eliminar el registro, ya que está relacionado con datos. Error: [" + Ex.Message + "]";
                    } else {
                        Mensaje = "Error al intentar eliminar el Tipo de Vehiculo. Error: [" + Ex.Message + "]";
                    }
                    throw new Exception(Mensaje);
                } catch (Exception Ex) {
                    Mensaje = "Error al intentar eliminar el Tipo de Vehiculo. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje);
                } finally {
                    Cn.Close();
                }
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
                for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                    Retornar = Retornar + "0";
                }
                Retornar = Retornar + Dato;
                return Retornar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Nueva_Aseguradora
            ///DESCRIPCIÓN: Verifica si la Aseguradora es nueva para el Tipo de Vehiculo o es 
            ///             la misma.
            ///PARAMETROS:     
            ///             1. Parametros.  Datos del Vehiculo.
            ///FECHA_CREO: 14/Marzo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private static Boolean Nueva_Aseguradora(Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Parametros){
                Boolean Nueva = true;
                try {
                    String Mi_SQL = "SELECT " + Ope_Pat_Vehiculo_Aseguradora.Campo_Aseguradora_ID + " FROM " + Ope_Pat_Vehiculo_Aseguradora.Tabla_Ope_Pat_Vehiculo_Aseguradora;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Pat_Vehiculo_Aseguradora.Campo_Tipo_Vehiculo_ID + " = '" + Parametros.P_Tipo_Vehiculo_ID + "'";
                    Mi_SQL = Mi_SQL + " AND " + Ope_Pat_Vehiculo_Aseguradora.Campo_Estatus + " = 'VIGENTE'";
                    Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    if ((Obj_Temp != null) && !(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals("")) {
                        if (Parametros.P_Aseguradora_ID.Equals(Obj_Temp.ToString().Trim())) {
                            Nueva = false;
                        }
                    }
                } catch (Exception Ex) {
                    String Mensaje = "Error: [" + Ex.Message + "]";
                    throw new Exception(Mensaje);
                }
                return Nueva;
            }

        #endregion

    }

}