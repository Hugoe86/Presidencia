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
using Presidencia.Constantes;
using System.Text;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Catalogo_Tramites.Negocio;

/// <summary>
/// Summary description for Cls_Cat_Tramites_Datos
/// </summary>

namespace Presidencia.Catalogo_Tramites.Datos{

    public class Cls_Cat_Tramites_Datos{

        #region Metodos

        ///****************************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Tramite
        /// DESCRIPCION : Da de Alta un Tramite en la Base de Datos.
        /// PARAMETROS  : 
        ///               1.Tramite.    Objeto de la Clase de Negocio de Tramite con las propiedades
        ///                             del Tramite que va a ser dado de Alta.
        /// CREO        : Francisco Antonio Gallardo Castañeda.
        /// FECHA_CREO  : 28-Septiembre-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///****************************************************************************************
        public static void Alta_Tramite(Cls_Cat_Tramites_Negocio Tramite)
        {
            String Mensaje = "";
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;
           
            String Condicion_Si = "";
            String Condicion_No = "";
            String Tramite_ID = "";
            String Mi_SQL = "";
            String Detalle_Autorizacion_ID = "";
            String Dato_ID = "";
            String Detalle_Documento_ID = "";
            String Subproceso_ID = "";
            String Detalle_Plantilla_ID = "";
            String Detalle_Formato_ID = "";
            String Matriz_ID = "";
            try
            {
                Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                Tramite_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando ,Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites, Cat_Tra_Tramites.Campo_Tramite_ID, 5);
                
                Mi_SQL = "INSERT INTO " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " (";
                Mi_SQL = Mi_SQL + Cat_Tra_Tramites.Campo_Tramite_ID;//01
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Dependencia_ID;//02
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Cuenta_ID;//03
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Clave_Tramite;//04
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Nombre;//05
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Tipo;//06
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Descripcion;//07
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Tiempo_Estimado;//08
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Costo;//09
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Estatus_Tramite;//10
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Usuario_Creo;//11
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Clave_Ingreso_ID;//12
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Area_Dependencia;//13
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Fecha_Creo;//14
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Parametro1;//15
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Parametro2;//16
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Parametro3;//17
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Operador1;//18
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Operador2;//19
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Operador3;//20
                Mi_SQL = Mi_SQL + ") VALUES (";
                Mi_SQL = Mi_SQL + "'" + Tramite_ID;//01
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Dependencia_ID;//02
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Cuenta_ID;//03
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Clave_Tramite;//04
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Nombre;//05
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Tipo;//06
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Descripcion;//07
                Mi_SQL = Mi_SQL + "',  " + Tramite.P_Tiempo_Estimado;//08
                Mi_SQL = Mi_SQL + " , '" + Tramite.P_Costo;//09
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Estatus_Tramite;//10
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Usuario;//11
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Cuenta_Contable_Clave;//12
                Mi_SQL = Mi_SQL + "', '" + Tramite.P_Area_Dependencia;//13
                Mi_SQL = Mi_SQL + "', SYSDATE";//14
                Mi_SQL += ", '" + Tramite.P_Parametro1 + "'";//15
                Mi_SQL += ", '" + Tramite.P_Parametro2 + "'";//16
                Mi_SQL += ", '" + Tramite.P_Parametro3 + "'";//17
                Mi_SQL += ", '" + Tramite.P_Operador1 + "'";//18
                Mi_SQL += ", '" + Tramite.P_Operador2 + "'";//19
                Mi_SQL += ", '" + Tramite.P_Operador3 + "'";//20
                Mi_SQL += ")";
                Comando.CommandText = Mi_SQL;
                Comando.ExecuteNonQuery();


                if (Tramite.P_Perfiles_Autorizar != null && Tramite.P_Perfiles_Autorizar.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Tramite.P_Perfiles_Autorizar.Rows.Count; cnt++)
                    {
                        Detalle_Autorizacion_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando ,Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones, Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID, 5);
                        
                        Mi_SQL = "INSERT INTO " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + " (";
                        Mi_SQL += Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID;//01
                        Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Tramite_ID;//02
                        Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Perfil_ID;//03
                        Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Usuario_Creo;//04
                        Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Fecha_Creo;//05

                        Mi_SQL += ") VALUES (";

                        Mi_SQL += "'" + Detalle_Autorizacion_ID + "'";//01
                        Mi_SQL += ", '" + Tramite_ID + "'";//02
                        Mi_SQL += ", '" + Tramite.P_Perfiles_Autorizar.Rows[cnt][1] + "'";//03
                        Mi_SQL += ", '" + Tramite.P_Usuario + "'";//04
                        Mi_SQL += ", SYSDATE ";//05
                        Mi_SQL += ")";
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                }
                
                //  para los datos del tramite
                if (Tramite.P_Datos_Tramite != null && Tramite.P_Datos_Tramite.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Tramite.P_Datos_Tramite.Rows.Count; cnt++)
                    {
                        Dato_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando ,Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite, Cat_Tra_Datos_Tramite.Campo_Dato_ID, 10);
                        
                        Mi_SQL = "INSERT INTO " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " (";
                        Mi_SQL += Cat_Tra_Datos_Tramite.Campo_Dato_ID;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Tramite_ID;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Nombre;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Descripcion;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Dato_Requerido;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Fecha_Creo;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato;
                        Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Orden;

                        Mi_SQL += ") VALUES (";

                        Mi_SQL += "'" + Dato_ID + "'";
                        Mi_SQL += ", '" + Tramite_ID + "'";
                        Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][1].ToString().Trim() + "'";
                        Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][2].ToString().Trim() + "'";
                        Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][3].ToString().Trim() + "'";
                        Mi_SQL += ", '" + Tramite.P_Usuario + "'";
                        Mi_SQL += ", SYSDATE";
                        Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][4].ToString().Trim() + "'";
                        Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][5].ToString().Trim() + "'";
                        Mi_SQL += ")";
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                }
                //  para los documentos del tramite
                if (Tramite.P_Documentacion_Tramite != null && Tramite.P_Documentacion_Tramite.Rows.Count > 0)
                {
                    
                    for (int Cnt = 0; Cnt < Tramite.P_Documentacion_Tramite.Rows.Count; Cnt++)
                    {
                        Detalle_Documento_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando ,Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos, Tra_Detalle_Documentos.Campo_Detalle_Documento_ID, 10);

                        Mi_SQL = "INSERT INTO " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + " (";
                        Mi_SQL += Tra_Detalle_Documentos.Campo_Detalle_Documento_ID;
                        Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Tramite_ID;
                        Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Documento_ID;
                        Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Fecha_Creo;
                        Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Documento_Requerido;

                        Mi_SQL += ") VALUES (";

                        Mi_SQL += "'" + Detalle_Documento_ID + "'";
                        Mi_SQL += ", '" + Tramite_ID + "'";
                        Mi_SQL += ", '" + Tramite.P_Documentacion_Tramite.Rows[Cnt][1] + "'";
                        Mi_SQL += ", '" + Tramite.P_Usuario + "'";
                        Mi_SQL += ", SYSDATE";
                        Mi_SQL += ", '" + Tramite.P_Documentacion_Tramite.Rows[Cnt][2] + "'";
                        Mi_SQL += ")";
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                }
                //  para el subproceso
                if (Tramite.P_SubProcesos_Tramite != null && Tramite.P_SubProcesos_Tramite.Rows.Count > 0)
                {
                   
                    for (int cnt = 0; cnt < Tramite.P_SubProcesos_Tramite.Rows.Count; cnt++)
                    {
                        Subproceso_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos, Cat_Tra_Subprocesos.Campo_Subproceso_ID, 5);

                        //  se valida que contenga informacion
                        if (Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString() != "" && Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString() != "0")
                            Condicion_Si = Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString().Trim();
                        else
                            Condicion_Si = "null";

                        if (Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString() != "" && Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString() != "0")
                            Condicion_No = Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString().Trim();
                        else
                            Condicion_No = "null";

                        Mi_SQL = "INSERT INTO " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " (";
                        Mi_SQL += Cat_Tra_Subprocesos.Campo_Subproceso_ID;
                        Mi_SQL += "," + Cat_Tra_Subprocesos.Campo_Tramite_ID;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Nombre;
                        Mi_SQL += "," + Cat_Tra_Subprocesos.Campo_Descripcion;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Valor;
                        Mi_SQL += "," + Cat_Tra_Subprocesos.Campo_Orden;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Plantilla;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Fecha_Creo;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Tipo_Actividad;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Condicion_Si;
                        Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Condicion_No;

                        Mi_SQL += ") VALUES (";

                        Mi_SQL += "'" + Subproceso_ID + "'";
                        Mi_SQL += ", '" + Tramite_ID + "'";
                        Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][1] + "' ";
                        Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][2] + "' ";
                        Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][3] + "' ";
                        Mi_SQL += ", " + Tramite.P_SubProcesos_Tramite.Rows[cnt][4] + " ";//   numerico
                        Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][5] + "' ";
                        Mi_SQL += ", '" + Tramite.P_Usuario + "' ";
                        Mi_SQL += ", SYSDATE";
                        Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][6] + "' ";
                        Mi_SQL += ", " + Condicion_Si;//   numerico
                        Mi_SQL += ", " + Condicion_No;//   numerico
                        Mi_SQL += ")";
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();


                        //  se ingresaran los detalles de la plantilla
                        if (Tramite.P_Dt_Detalle_Plantilla != null)
                        {
                            if (Tramite.P_Dt_Detalle_Plantilla.Rows.Count > 0)
                            {
                                foreach (DataRow Registro in Tramite.P_Dt_Detalle_Plantilla.Rows)
                                {
                                    if (Tramite.P_SubProcesos_Tramite.Rows[cnt]["ORDEN"].ToString() == Registro["ORDEN"].ToString())
                                    {
                                        Detalle_Plantilla_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla, Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID, 5);
                                      
                                        Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + " (";
                                        Mi_SQL += Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Usuario_Creo;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Fecha_Creo;
                                        Mi_SQL += ") VALUES (";
                                        Mi_SQL += "'" + Subproceso_ID + "'";
                                        Mi_SQL += ", '" + Tramite_ID + "'";
                                        Mi_SQL += ", '" + Registro["PLANTILLA_ID"].ToString() + "'";
                                        Mi_SQL += ", '" + Detalle_Plantilla_ID + "'";
                                        Mi_SQL += ", '" + Tramite.P_Usuario + "'";
                                        Mi_SQL += ", SYSDATE";
                                        Mi_SQL += ")";
                                        Comando.CommandText = Mi_SQL;
                                        Comando.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        //  se ingresaran los detalles de la formato
                        if (Tramite.P_Dt_Detalle_Formato != null)
                        {
                            if (Tramite.P_Dt_Detalle_Formato.Rows.Count > 0)
                            {
                                foreach (DataRow Registro in Tramite.P_Dt_Detalle_Formato.Rows)
                                {
                                    if (Tramite.P_SubProcesos_Tramite.Rows[cnt]["ORDEN"].ToString() == Registro["ORDEN"].ToString())
                                    {
                                        Detalle_Formato_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato, Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID, 5);
                                        
                                        Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + " (";
                                        Mi_SQL += Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Usuario_Creo;
                                        Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Fecha_Creo;
                                        Mi_SQL += ") VALUES (";
                                        Mi_SQL += "'" + Subproceso_ID + "'";
                                        Mi_SQL += ",'" + Tramite_ID + "'";
                                        Mi_SQL += ",'" + Registro["PLANTILLA_ID"].ToString() + "'";
                                        Mi_SQL += ",'" + Detalle_Formato_ID + "'";
                                        Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                                        Comando.CommandText = Mi_SQL;
                                        Comando.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                }

                //  se ingresaran los detalles de la formato
                if (Tramite.P_Matriz_Costo != null)
                {
                    if (Tramite.P_Matriz_Costo.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Tramite.P_Matriz_Costo.Rows)
                        {
                            Matriz_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo, Ope_Tra_Matriz_Costo.Campo_Matriz_ID, 10);

                            Mi_SQL = "INSERT INTO " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + " (";
                            Mi_SQL += Ope_Tra_Matriz_Costo.Campo_Matriz_ID;
                            Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Tramite_ID;
                            Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Tipo;
                            Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Costo_Base;
                            Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Usuario_Creo;
                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Fecha_Creo;
                            
                            Mi_SQL += ") VALUES (";
                            
                            Mi_SQL += "'" + Matriz_ID + "'";
                            Mi_SQL += ",'" + Tramite_ID + "'";
                            Mi_SQL += ",'" + Registro["TIPO"].ToString() + "'";
                            Mi_SQL += ",'" + Registro["COSTO_BASE"].ToString() + "'";
                            Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                            Comando.CommandText = Mi_SQL;
                            Comando.ExecuteNonQuery();
                        }
                    }
                }

                Transaccion.Commit();
            }

            catch (OracleException Ex)
            {
                if (Comando == null)
                {
                    Transaccion.Rollback();
                }
                //variable para el mensaje 
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Error al intentar dar de Alta una P_Clave de Ingreso. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            catch (Exception Ex)
            {
                Transaccion.Rollback();
                throw new Exception(Ex.ToString());
            }

            finally
            {
                Conexion.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Tramite
        ///DESCRIPCIÓN: Obtiene todos los Detalles de un Tramite y se almacenan en una variable
        ///             de tipo de Tramite, basandose en el ID pasado como propiedad
        ///PARAMETROS:     
        ///             1.P_Tramite.    Tramite que contiene el ID del cual se extraeran los datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Tramites_Negocio Consultar_Datos_Tramite(Cls_Cat_Tramites_Negocio P_Tramite) 
        {
            Cls_Cat_Tramites_Negocio R_Tramite = new Cls_Cat_Tramites_Negocio();
            OracleDataReader Data_Reader;
            StringBuilder Mi_SQL2 = new StringBuilder();
            try 
            {
                String Mi_SQL = "SELECT " + Cat_Tra_Tramites.Campo_Dependencia_ID;//0
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Clave_Tramite + //1
                                  ", " + Cat_Tra_Tramites.Campo_Nombre;//2
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Tipo + //3
                                  ", " + Cat_Tra_Tramites.Campo_Descripcion;//4
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Tiempo_Estimado;// 5
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Estatus_Tramite;//6
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Costo + //7
                                  ", " + Cat_Tra_Tramites.Campo_Cuenta_ID;    //8            
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Clave_Ingreso_ID + //9
                                  ", " + Cat_Tra_Tramites.Campo_Area_Dependencia;//10
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Parametro1;//11
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Parametro2;//12
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Operador1;//13
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Operador2;//14
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Parametro3;//15
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Operador3;//16
                Mi_SQL = Mi_SQL + "  FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID + " = '" + P_Tramite.P_Tramite_ID + "'";
                Data_Reader = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                R_Tramite.P_Tramite_ID = P_Tramite.P_Tramite_ID;
                //  se carga la clase de negocio con la informacion del tramite
                while (Data_Reader.Read()) 
                {
                    R_Tramite.P_Dependencia_ID = Data_Reader.GetString(0).ToString();
                    R_Tramite.P_Clave_Tramite = Data_Reader.GetString(1).ToString();
                    R_Tramite.P_Nombre = Data_Reader.GetString(2).ToString();
                    R_Tramite.P_Tipo = Data_Reader.GetString(3).ToString();
                    R_Tramite.P_Descripcion = Data_Reader.GetString(4).ToString();
                    R_Tramite.P_Tiempo_Estimado = Data_Reader.GetInt32(5);
                    R_Tramite.P_Estatus_Tramite = Data_Reader.GetString(6);
                    R_Tramite.P_Costo = Data_Reader.GetDouble(7);
                    R_Tramite.P_Cuenta_Contable_Clave = !Data_Reader.IsDBNull(9) ? Data_Reader.GetString(9) : "";
                    R_Tramite.P_Cuenta_ID = !Data_Reader.IsDBNull(8) ? Data_Reader.GetString(8).ToString() : "";
                    R_Tramite.P_Parametro1 = !Data_Reader.IsDBNull(11) ? Data_Reader.GetString(11).ToString() : "";
                    R_Tramite.P_Parametro2 = !Data_Reader.IsDBNull(12) ? Data_Reader.GetString(12).ToString() : "";
                    R_Tramite.P_Operador1= !Data_Reader.IsDBNull(13) ? Data_Reader.GetString(13).ToString() : "";
                    R_Tramite.P_Operador2 = !Data_Reader.IsDBNull(14) ? Data_Reader.GetString(14).ToString() : "";
                    R_Tramite.P_Parametro3 = !Data_Reader.IsDBNull(15) ? Data_Reader.GetString(15).ToString() : "";
                    R_Tramite.P_Operador3 = !Data_Reader.IsDBNull(16) ? Data_Reader.GetString(16).ToString() : "";
                }
                Data_Reader.Close();

                Mi_SQL = "SELECT " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID + " AS DETALLE_DOCUMENTO_ID";
                Mi_SQL += ", " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Documento_ID + " AS DOCUMENTO_ID";
                Mi_SQL += ", " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Documento_Requerido + " AS DOCUMENTO_REQUERIDO";
                Mi_SQL += ", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Nombre + " AS NOMBRE";
                Mi_SQL += ", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Descripcion + " AS DESCRIPCION";
                Mi_SQL += " FROM " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + ", " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos;
                Mi_SQL += " WHERE " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Documento_ID;
                Mi_SQL += " = " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos + "." + Cat_Tra_Documentos.Campo_Documento_ID;
                Mi_SQL += " AND " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + "." + Tra_Detalle_Documentos.Campo_Tramite_ID;
                Mi_SQL += " = '" + P_Tramite.P_Tramite_ID + "'";
                DataSet dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                
                if (dataSet == null) 
                {
                    R_Tramite.P_Documentacion_Tramite = new DataTable();
                } 
                else 
                {
                    R_Tramite.P_Documentacion_Tramite = dataSet.Tables[0];
                }
                dataSet = null;
                Mi_SQL = "SELECT " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + "." + Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID + " AS DETALLE_AUTORIZACION_ID";
                Mi_SQL = Mi_SQL + ", " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + "." + Tra_Detalle_Autorizaciones.Campo_Perfil_ID + " AS PERFIL_ID";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + ", " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
                Mi_SQL = Mi_SQL + " WHERE " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + "." + Tra_Detalle_Autorizaciones.Campo_Perfil_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles + "." + Cat_Tra_Perfiles.Campo_Perfil_ID;
                Mi_SQL = Mi_SQL + " AND " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + "." + Tra_Detalle_Autorizaciones.Campo_Tramite_ID;
                Mi_SQL = Mi_SQL + " = '" + P_Tramite.P_Tramite_ID + "'";
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (dataSet == null) 
                {
                    R_Tramite.P_Perfiles_Autorizar = new DataTable();
                } 
                else 
                {
                    R_Tramite.P_Perfiles_Autorizar = dataSet.Tables[0];
                }
                dataSet = null;

                Mi_SQL = "SELECT " + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " AS DATO_ID ";
                Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Nombre + " AS NOMBRE ";
                Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Descripcion + " AS DESCRIPCION ";
                Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Dato_Requerido + " AS DATO_REQUERIDO ";
                Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato + " AS TIPO_DATO ";
                Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Orden+ " AS ORDEN ";
                Mi_SQL += " FROM " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite;
                Mi_SQL += " WHERE " + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + " = '" + P_Tramite.P_Tramite_ID + "'";
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet == null) 
                {
                    R_Tramite.P_Datos_Tramite = new DataTable();
                } 
                else 
                {
                    R_Tramite.P_Datos_Tramite = dataSet.Tables[0];
                }
                dataSet = null;

                Mi_SQL = "SELECT " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " AS SUBPROCESO_ID, " + Cat_Tra_Subprocesos.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Descripcion + " AS DESCRIPCION, " + Cat_Tra_Subprocesos.Campo_Valor + " AS VALOR";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Orden + " AS ORDEN, " + Cat_Tra_Subprocesos.Campo_Plantilla + " AS PLANTILLA";
                Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Tipo_Actividad + " AS TIPO_ACTIVIDAD ";
                Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Condicion_Si;
                Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Condicion_No;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Subprocesos.Campo_Tramite_ID + " = '" + P_Tramite.P_Tramite_ID + "'";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Tra_Subprocesos.Campo_Orden;
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (dataSet == null) 
                {
                    R_Tramite.P_SubProcesos_Tramite = new DataTable();
                } 
                else 
                {
                    R_Tramite.P_SubProcesos_Tramite = dataSet.Tables[0];
                }
                //  para el detalle de plantillas
                Mi_SQL2.Append("SELECT ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID);
                Mi_SQL2.Append(", " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID);
                Mi_SQL2.Append(", " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID);
                Mi_SQL2.Append(", " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID);
                Mi_SQL2.Append(", " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Nombre);
                Mi_SQL2.Append(", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

                Mi_SQL2.Append(" From ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla);

                Mi_SQL2.Append(" left outer join ");
                Mi_SQL2.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL2.Append(" on ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + "=");
                Mi_SQL2.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_SQL2.Append(" left outer join ");
                Mi_SQL2.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas);
                Mi_SQL2.Append(" on ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID + "=");
                Mi_SQL2.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Plantilla_ID);

                Mi_SQL2.Append(" left outer join ");
                Mi_SQL2.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL2.Append(" on ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID + "=");
                Mi_SQL2.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL2.Append(" where ");
                Mi_SQL2.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID + "='" + P_Tramite.P_Tramite_ID + "'");

                Mi_SQL2.Append(" order by " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

                DataTable Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL2.ToString()).Tables[0];
                if (dataSet == null)
                {
                    R_Tramite.P_Dt_Detalle_Plantilla = new DataTable();
                }
                else
                {
                    R_Tramite.P_Dt_Detalle_Plantilla = Dt_Temporal;
                }

                //   para el detalle de los formatos
                Mi_SQL2 = new StringBuilder();
                Mi_SQL2.Append("SELECT ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID);
                Mi_SQL2.Append(", " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID);
                Mi_SQL2.Append(", " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID);
                Mi_SQL2.Append(", " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID);
                Mi_SQL2.Append(", " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Nombre);
                Mi_SQL2.Append(", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

                Mi_SQL2.Append(" From ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato);

                Mi_SQL2.Append(" left outer join ");
                Mi_SQL2.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL2.Append(" on ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + "=");
                Mi_SQL2.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_SQL2.Append(" left outer join ");
                Mi_SQL2.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas);
                Mi_SQL2.Append(" on ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID + "=");
                Mi_SQL2.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Plantilla_ID);

                Mi_SQL2.Append(" left outer join ");
                Mi_SQL2.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL2.Append(" on ");
                Mi_SQL2.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID + "=");
                Mi_SQL2.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL2.Append(" where ");
                Mi_SQL2.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID + "='" + P_Tramite.P_Tramite_ID + "'");

                Mi_SQL2.Append(" order by " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);
                Dt_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL2.ToString()).Tables[0];

                if (dataSet == null)
                {
                    R_Tramite.P_Dt_Detalle_Plantilla = new DataTable();
                }
                else
                {
                    R_Tramite.P_Dt_Detalle_Plantilla = Dt_Temporal;
                }


                Mi_SQL = "SELECT " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + ".*";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + "." + Ope_Tra_Matriz_Costo.Campo_Tramite_ID;
                Mi_SQL = Mi_SQL + " = '" + P_Tramite.P_Tramite_ID + "'";
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (dataSet == null)
                {
                    R_Tramite.P_Matriz_Costo = new DataTable();
                }
                else
                {
                    R_Tramite.P_Matriz_Costo = dataSet.Tables[0];
                }

                dataSet = null;
            } 
            catch (OracleException ex) 
            {
                new Exception(ex.Message);
            }
            return R_Tramite;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Tramite
        ///DESCRIPCIÓN: Obtiene todos los Detalles de un Tramite y se almacenan en una variable
        ///             de tipo de Tramite, basandose en el ID pasado como propiedad
        ///PARAMETROS:     
        ///             1.P_Tramite.    Tramite que contiene el ID del cual se extraeran los datos.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_Tabla_Tramite(Cls_Cat_Tramites_Negocio Parametros)
        {
            String Mi_SQL = null;
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = new DataTable();
            Boolean Entro_Where = false;
            try
            {
                Mi_SQL = "SELECT * FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITES ";
                if (!String.IsNullOrEmpty(Parametros.P_Tramite_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " TRAMITES." + Cat_Tra_Tramites.Campo_Tramite_ID + " = '" + Parametros.P_Tramite_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Dependencia_ID))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " TRAMITES." + Cat_Tra_Tramites.Campo_Dependencia_ID + " = '" + Parametros.P_Dependencia_ID + "'";
                }
                if (!String.IsNullOrEmpty(Parametros.P_Clave_Tramite))
                {
                    if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                    Mi_SQL += " TRAMITES." + Cat_Tra_Tramites.Campo_Clave_Tramite + " = '" + Parametros.P_Clave_Tramite + "'";
                }
                Mi_SQL += " ORDER BY " + Cat_Tra_Tramites.Campo_Tramite_ID + " ASC";
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Datos != null)
                {
                    Dt_Datos = Ds_Datos.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Datos;
        }

        /// *******************************************************************************
        /// NOMBRE:         Consultar_Avance
        /// COMENTARIOS:    consultara el avance del flujo del tramite
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     09/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Avance(Cls_Cat_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append("sum(" + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Valor + ") as Suma_Valor");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Datos.P_Tramite_ID + "'");

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
        /// NOMBRE:         Consultar_Tipo_Actividad
        /// COMENTARIOS:    consultara el tipo de actividad del subproceso
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     21/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Tipo_Actividad(Cls_Cat_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT *");                
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID + "='" + Datos.P_Sub_Proceso_ID + "'");

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
        /// NOMBRE:         Consultar_Clave_Repetida
        /// COMENTARIOS:    consultara la clave esto para saber si se repite la clave del tramite
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     09/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Clave_Repetida(Cls_Cat_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + ".*");
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);

                if (!String.IsNullOrEmpty(Datos.P_Clave_Tramite))
                {
                    Mi_SQL.Append(" WHERE ");
                    Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Clave_Tramite + "='" + Datos.P_Clave_Tramite + "'");
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Eliminar_Tramite
        ///DESCRIPCIÓN: Elimina una Trmaite de la Base de Datos, ademas las relaciones con
        ///             otras tablas que se manejan Internamente en el Catalogo (Subprocesos,
        ///             Tramites_Autorizadores, Tramites_Datos, Tramites_Documentos, Subprocesos).
        ///PROPIEDADES:   
        ///             1. Tramite.   Tramite que se va eliminar.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Tramite(Cls_Cat_Tramites_Negocio Tramite)
        {
            String Mensaje = "";
            String Mi_SQL = "";

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;

            try
            {
                Cn.ConnectionString = Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;

                Mi_SQL = "DELETE FROM " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones;
                Mi_SQL = Mi_SQL + " WHERE " + Tra_Detalle_Autorizaciones.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "DELETE FROM " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "DELETE FROM " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos;
                Mi_SQL = Mi_SQL + " WHERE " + Tra_Detalle_Documentos.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS
                Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //  SE ELIMINAN LOS DETALLES DE LOS FORMATOS
                Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "DELETE FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Subprocesos.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Mi_SQL = "DELETE FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Trans != null)
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
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE:      Eliminar_Detalles_Plantillas_Formato
        ///DESCRIPCIÓN: Elimina los detalles de las plantilla y formatos
        ///PROPIEDADES: 1. Tramite.   Tramite que se va eliminar.
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  19/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Eliminar_Detalles_Plantillas_Formato(Cls_Cat_Tramites_Negocio Datos)
        {
            String Mensaje = "";
            String Mi_SQL = "";

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;

            try
            {
                Cn.ConnectionString = Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;

                //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS
                Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID + " = '" + Datos.P_Sub_Proceso_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                //  SE ELIMINAN LOS DETALLES DE LOS FORMATOS
                Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID + " = '" + Datos.P_Sub_Proceso_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();

                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                if (Trans != null)
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
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static string Obtener_ID_Consecutivo(string tabla, string campo, int longitud_id) 
        {
            String id = Convertir_A_Formato_ID(1, longitud_id); ;
            try 
            {
                String Mi_SQL = "SELECT MAX(" + campo + ") FROM " + tabla;
                Object tmp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(tmp is Nullable) && !tmp.ToString().Equals("")) 
                {
                    id = Convertir_A_Formato_ID((Convert.ToInt32(tmp) + 1), longitud_id);
                }
            } 
            catch (OracleException Ex) 
            {
                new Exception(Ex.Message);
            }
            return id;
        }

        ///*******************************************************************************
        ///NOMBRE               : Obtener_ID_Consecutivo_Transaccion
        ///DESCRIPCIÓN          : Obtiene el ID Consecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS           :     
        ///CREO                 : Hugo Enrique Ramírez Aguilera.
        ///FECHA_CREO           : 19/Diciembre/20102
        ///MODIFICO             :
        ///FECHA_MODIFICO       :  
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static string Obtener_ID_Consecutivo_Transaccion(ref  OracleCommand Comando, String tabla, String campo, int longitud_id)
        {
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            String Id = "";
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            String Mi_SQL = "";
            try
            {
                if (Comando == null)
                {
                    // crear transaccion
                    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Cn.Open();
                    Trans = Cn.BeginTransaction();
                    Cmd.Connection = Cn;
                    Cmd.Transaction = Trans;
                }
                else
                {
                    Cmd = Comando;
                }

                Mi_SQL = "SELECT NVL(MAX (" + campo + "), '" + Convertir_A_Formato_ID(0,longitud_id) + " ' ) FROM " + tabla;
                
                Cmd.CommandText = Mi_SQL;
                Obj = Cmd.ExecuteScalar();

                if (!(Obj is Nullable) && !Obj.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj) + 1), longitud_id);
                }

                else
                {
                    Id = Convertir_A_Formato_ID(1, longitud_id);
                }
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cmd == null)
                {
                    Cn.Close();
                }
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Plantilla_Formato
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARAMETROS:     
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 18/Mayo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static string Obtener_ID_Plantilla_Formato(ref OracleCommand Comando, string tabla, string campo, int longitud_id, int Repetidos)
        {
            String Id = Convertir_A_Formato_ID(1, longitud_id);

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans = null;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            String Mi_SQL = "";

            try
            {
                if (Comando == null)
                {
                    // crear transaccion
                    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                    Cn.Open();
                    Trans = Cn.BeginTransaction();
                    Cmd.Connection = Cn;
                    Cmd.Transaction = Trans;
                }
                else
                {
                    Cmd = Comando;
                }

                Mi_SQL = "SELECT NVL(MAX (" + campo + "), '" + Convertir_A_Formato_ID(0, longitud_id) + "' ) FROM " + tabla;
                Cmd.CommandText = Mi_SQL;
                Obj = Cmd.ExecuteScalar();

                if (Convert.IsDBNull(Obj))
                {
                    Id = Convertir_A_Formato_ID(1, longitud_id);
                }
                else
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj) + 1), longitud_id);
                }
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                throw new Exception("Error: " + Ex.Message);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
            finally
            {
                if (Cmd == null)
                {
                    Cn.Close();
                }
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
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private static string Convertir_A_Formato_ID(int Dato_ID, int Longitud_ID) 
        {
            String retornar = "";
            String Dato = "" + Dato_ID;
            for (int tmp = Dato.Length; tmp < Longitud_ID; tmp++) 
            {
                retornar = retornar + "0";
            }
            retornar = retornar + Dato;
            return retornar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN: Hace una consulta dependiendo del parametro que tenga y genera un 
        ///             DataTable de esa consulta
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 29/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Cat_Tramites_Negocio Tramite)
        {
            DataTable tabla = null;
            String Mi_SQL = null;
            DataSet dataSet = null;
            if (Tramite.P_Tipo_DataTable.Equals("DEPENDENCIAS"))
            {
                Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Dependencias.Campo_Estatus + " = 'ACTIVO'";
            }
            else if (Tramite.P_Tipo_DataTable.Equals("PERFILES"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Perfiles.Campo_Perfil_ID + " AS PERFIL_ID, " + Cat_Tra_Perfiles.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Perfiles.Tabla_Cat_Tra_Perfiles;
            }
            else if (Tramite.P_Tipo_DataTable.Equals("TRAMITES"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Tramites.Campo_Tramite_ID + " AS TRAMITE_ID, " + Cat_Tra_Tramites.Campo_Clave_Tramite + " AS CLAVE_TRAMITE";
                Mi_SQL = Mi_SQL + ", " + Cat_Tra_Tramites.Campo_Nombre + " AS NOMBRE  FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Campo_Nombre + " LIKE '%" + Tramite.P_Nombre + "%'";
            }
            else if (Tramite.P_Tipo_DataTable.Equals("CUENTAS"))
            {
                Mi_SQL = "SELECT " + Cat_Cuentas.Campo_Cuenta_ID + " AS CUENTA_ID, " + Cat_Cuentas.Campo_No_Cuenta + " AS NUMERO_CUENTA";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Cuentas.Tabla_Cat_Cuentas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Cuentas.Campo_Dependencia_ID + " = '" + Tramite.P_Dependencia_ID + "'";
            }
            else if (Tramite.P_Tipo_DataTable.Equals("DOCUMENTOS"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Documentos.Campo_Documento_ID + " AS DOCUMENTO_ID, " + Cat_Tra_Documentos.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos;
            }
            else if (Tramite.P_Tipo_DataTable.Equals("PLANTILLAS"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Plantillas.Campo_Plantilla_ID + " AS PLANTILLA_ID, " + Cat_Tra_Plantillas.Campo_Nombre + " AS NOMBRE";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas;
            }

            else if (Tramite.P_Tipo_DataTable.Equals("FORMATOS"))
            {
                Mi_SQL = "SELECT " + Cat_Tra_Formato_Predefinido.Campo_Formato_ID + " as FORMATO_ID," + Cat_Tra_Formato_Predefinido.Campo_Nombre + " AS NOMBRE ";
                Mi_SQL += " FROM " + Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido;
            }
            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
            {
                dataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            if (dataSet == null)
            {
                tabla = new DataTable();
            }
            else
            {
                tabla = dataSet.Tables[0];
            }
            return tabla;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tramite
        ///DESCRIPCIÓN: Actualiza los datos de un tramite en la Base de Datos
        ///PARAMETROS:     
        ///             1. Tramite. Instancia de la Clase de Negocio de Tramites con los 
        ///                         datos del Tramite que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Tramite(Cls_Cat_Tramites_Negocio Tramite)
        {
            Cls_Cat_Tramites_Negocio Tramite_Tmp = Consultar_Datos_Tramite(Tramite);
            String Mensaje = "";
            OracleConnection Conexion = new OracleConnection();
            OracleCommand Comando = new OracleCommand();
            OracleTransaction Transaccion = null;           
            Boolean Estado_Detalles = false;
            String Mi_SQL = "";
            String Detalle_Plantilla_ID = ""; 
            String Detalle_Documento_ID = "";
            String Detalle_Formato_ID = ""; 
            String MATRIZ_ID = "";
            String Dato_ID = "";
            String Subproceso_ID = "";
            String Plantilla_ID = "";
            String Tramite_ID = "";
            DataTable Dt_Consulta_Plantilla_Formato = new DataTable();
            DataTable Dt_Actividades_Tramite = new DataTable();
            int Contador = 0;
            Boolean Id_Repetido = false;
            String Elemento_Eliminar = "";
            Boolean ID_Eliminar = false;
            String Condicion_Si = "";
            String Condicion_No = "";
            String Detalle_Autorizacion_ID = "";
            try
            {
                Conexion.ConnectionString = Cls_Constantes.Str_Conexion;
                Conexion.Open();
                Transaccion = Conexion.BeginTransaction();
                Comando.Connection = Conexion;
                Comando.Transaction = Transaccion;

                //SE ACTULIZAN LOS DATOS GENERALES DEL TRAMITE 
                Mi_SQL = "UPDATE " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " SET ";
                Mi_SQL += Cat_Tra_Tramites.Campo_Dependencia_ID + " = '" + Tramite.P_Dependencia_ID + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Clave_Ingreso_ID + " = '" + Tramite.P_Cuenta_Contable_Clave + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Cuenta_ID + " = '" + Tramite.P_Cuenta_ID + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Clave_Tramite + " = '" + Tramite.P_Clave_Tramite + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Nombre + " = '" + Tramite.P_Nombre + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Tipo + " = '" + Tramite.P_Tipo + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Estatus_Tramite + " = '" + Tramite.P_Estatus_Tramite + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Descripcion + " = '" + Tramite.P_Descripcion + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Tiempo_Estimado + " = " + Tramite.P_Tiempo_Estimado + "";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Costo + " = '" + Tramite.P_Costo + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Area_Dependencia + " = '" + Tramite.P_Area_Dependencia + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Parametro1 + " = '" + Tramite.P_Parametro1 + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Parametro2 + " = '" + Tramite.P_Parametro2 + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Parametro3 + " = '" + Tramite.P_Parametro3 + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Operador1 + " = '" + Tramite.P_Operador1 + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Operador2 + " = '" + Tramite.P_Operador2 + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Operador3 + " = '" + Tramite.P_Operador3 + "'";
                Mi_SQL += ", " + Cat_Tra_Tramites.Campo_Fecha_Modifico + " = SYSDATE";
                Mi_SQL += " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID + "'";
                Comando.CommandText = Mi_SQL;
                Comando.ExecuteNonQuery();

                Tramite_Tmp = Obtener_Registros_SubCatalogos_Eliminados(Tramite_Tmp, Tramite); // SE OBTIENEN LOS REGISTROS ELIMINADOS

                //SE ELIMINAN DE LA BASE DE DATOS, LOS REGISTROS DE LOS AUTORIZADORES PARA ESTE TRAMITE
                for (int cnt = 0; cnt < Tramite_Tmp.P_Perfiles_Autorizar.Rows.Count; cnt++)
                {
                    Mi_SQL = "DELETE FROM " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones;
                    Mi_SQL += " WHERE " + Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID + " = '" + Tramite_Tmp.P_Perfiles_Autorizar.Rows[cnt][0].ToString() + "'";
                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();
                }

                //SE ELIMINAN DE LA BASE DE DATOS, LOS REGISTROS DE LOS DATOS PARA ESTE TRAMITE
                for (int cnt = 0; cnt < Tramite_Tmp.P_Datos_Tramite.Rows.Count; cnt++)
                {
                    Mi_SQL = "DELETE FROM " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite;
                    Mi_SQL += " WHERE " + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = '" + Tramite_Tmp.P_Datos_Tramite.Rows[cnt][0].ToString() + "'";
                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();
                }

                //SE ELIMINAN DE LA BASE DE DATOS, LOS REGISTROS DE LOS DOCUMENTOS PARA ESTE TRAMITE
                for (int cnt = 0; cnt < Tramite_Tmp.P_Documentacion_Tramite.Rows.Count; cnt++)
                {
                    Mi_SQL = "DELETE FROM " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos;
                    Mi_SQL += " WHERE " + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID + " = '" + Tramite_Tmp.P_Documentacion_Tramite.Rows[cnt][0].ToString() + "'";
                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();
                }

                //SE ELIMINAN DE LA BASE DE DATOS, LOS REGISTROS DE LOS SUBPROCESOS PARA ESTE TRAMITE
                for (int cnt = 0; cnt < Tramite_Tmp.P_SubProcesos_Tramite.Rows.Count; cnt++)
                {
                    Mi_SQL = "DELETE FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                    Mi_SQL += " WHERE " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " = '" + Tramite_Tmp.P_SubProcesos_Tramite.Rows[cnt][0].ToString() + "'";
                    Comando.CommandText = Mi_SQL;
                    Comando.ExecuteNonQuery();
                }

                //SE ACTUALIZAN E INSERTAN LAS ACTUALIZACIONES DE LOS PERFILES AUTORIZADORES PARA EL TRAMITE EN LA BASE DE DATOS
                if (Tramite.P_Perfiles_Autorizar != null && Tramite.P_Perfiles_Autorizar.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Tramite.P_Perfiles_Autorizar.Rows.Count; cnt++)
                    {
                        if (Tramite.P_Perfiles_Autorizar.Rows[cnt][0].ToString().Trim().Equals(""))
                        {
                            Detalle_Autorizacion_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones, Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID, 5);
                            Mi_SQL = "INSERT INTO " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + " (";
                            Mi_SQL += Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID;//01
                            Mi_SQL += "," + Tra_Detalle_Autorizaciones.Campo_Tramite_ID;//02
                            Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Perfil_ID;//03
                            Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Usuario_Creo;//04
                            Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Fecha_Creo;//05
                            Mi_SQL += ") VALUES (";
                            Mi_SQL += "'" + Detalle_Autorizacion_ID + "' ";//01
                            Mi_SQL += ",'" + Tramite.P_Tramite_ID + "' ";//02
                            Mi_SQL += ", '" + Tramite.P_Perfiles_Autorizar.Rows[cnt][1] + "'";//03
                            Mi_SQL += ", '" + Tramite.P_Usuario + "'";//04
                            Mi_SQL += ", SYSDATE ";//05
                            Mi_SQL += ")";
                        }
                        else
                        {
                            Mi_SQL = "UPDATE " + Tra_Detalle_Autorizaciones.Tabla_Tra_Detalle_Autorizaciones + " SET ";
                            Mi_SQL += Tra_Detalle_Autorizaciones.Campo_Perfil_ID + " = '" + Tramite.P_Perfiles_Autorizar.Rows[cnt][1].ToString().Trim() + "'";
                            Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Usuario_Modifico + " = '" + Tramite.P_Usuario + "'";
                            Mi_SQL += ", " + Tra_Detalle_Autorizaciones.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Tra_Detalle_Autorizaciones.Campo_Detalle_Autorizacion_ID + " = '" + Tramite.P_Perfiles_Autorizar.Rows[cnt][0].ToString().Trim() + "'";
                        }
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                }

                //SE ACTUALIZAN E INSERTAN LAS ACTUALIZACIONES DE LOS DATOS PARA EL TRAMITE EN LA BASE DE DATOS
                if (Tramite.P_Datos_Tramite != null && Tramite.P_Datos_Tramite.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Tramite.P_Datos_Tramite.Rows.Count; cnt++)
                    {
                        if (Tramite.P_Datos_Tramite.Rows[cnt][0].ToString().Trim().Equals(""))
                        {
                            Dato_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite, Cat_Tra_Datos_Tramite.Campo_Dato_ID, 10);

                            Mi_SQL = "INSERT INTO " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " (";
                            Mi_SQL += Cat_Tra_Datos_Tramite.Campo_Dato_ID;
                            Mi_SQL += "," + Cat_Tra_Datos_Tramite.Campo_Tramite_ID;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Nombre;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Descripcion;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Dato_Requerido;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Usuario_Creo;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Fecha_Creo;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato;
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Orden;
                            Mi_SQL += ") VALUES (";
                            Mi_SQL += "'" + Dato_ID + "','" + Tramite.P_Tramite_ID + "'";
                            Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][1].ToString().Trim() + "'";
                            Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][2].ToString().Trim() + "'";
                            Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][3].ToString().Trim() + "'";
                            Mi_SQL += ", '" + Tramite.P_Usuario + "'";
                            Mi_SQL += ", SYSDATE ";
                            Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][4].ToString().Trim() + "' ";
                            Mi_SQL += ", '" + Tramite.P_Datos_Tramite.Rows[cnt][5].ToString().Trim() + "' ";
                            Mi_SQL += ")";
                        }
                        else
                        {
                            Mi_SQL = "UPDATE " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite + " SET ";
                            Mi_SQL += Cat_Tra_Datos_Tramite.Campo_Nombre + " = '" + Tramite.P_Datos_Tramite.Rows[cnt][1].ToString().Trim() + "'";
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Descripcion + " = '" + Tramite.P_Datos_Tramite.Rows[cnt][2].ToString().Trim() + "'";
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Dato_Requerido + " = '" + Tramite.P_Datos_Tramite.Rows[cnt][3].ToString().Trim() + "'";
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Usuario_Modifico + " = '" + Tramite.P_Usuario + "'";
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato + " = '" + Tramite.P_Datos_Tramite.Rows[cnt][4].ToString().Trim() + "'";
                            Mi_SQL += ", " + Cat_Tra_Datos_Tramite.Campo_Orden + " = '" + Tramite.P_Datos_Tramite.Rows[cnt][5].ToString().Trim() + "'";
                            Mi_SQL += " WHERE " + Cat_Tra_Datos_Tramite.Campo_Dato_ID + " = '" + Tramite.P_Datos_Tramite.Rows[cnt][0].ToString().Trim() + "'";
                        }
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                }

                //SE ACTUALIZAN E INSERTAN LAS ACTUALIZACIONES DE LOS DOCUMENTOS PARA EL TRAMITE EN LA BASE DE DATOS
                if (Tramite.P_Documentacion_Tramite != null && Tramite.P_Documentacion_Tramite.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < Tramite.P_Documentacion_Tramite.Rows.Count; cnt++)
                    {
                        if (Tramite.P_Documentacion_Tramite.Rows[cnt][0].ToString().Trim().Equals(""))
                        {
                            Detalle_Documento_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos, Tra_Detalle_Documentos.Campo_Detalle_Documento_ID, 10);

                            Mi_SQL = "INSERT INTO " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + " (";
                            Mi_SQL += Tra_Detalle_Documentos.Campo_Detalle_Documento_ID;//01
                            Mi_SQL += "," + Tra_Detalle_Documentos.Campo_Tramite_ID;//02 
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Documento_ID;//03
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Usuario_Creo;//04
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Fecha_Creo;//05
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Documento_Requerido; //06                            
                            Mi_SQL += ") VALUES (";
                            Mi_SQL += "'" + Detalle_Documento_ID + "'";//01
                            Mi_SQL += ",'" + Tramite.P_Tramite_ID + "'";//02
                            Mi_SQL += ", '" + Tramite.P_Documentacion_Tramite.Rows[cnt][1] + "'";//03
                            Mi_SQL += ", '" + Tramite.P_Usuario + "'";//04
                            Mi_SQL += ", SYSDATE";//05
                            Mi_SQL += ", '" + Tramite.P_Documentacion_Tramite.Rows[cnt][2] + "'";//06
                            Mi_SQL += ")";
                        }
                        else
                        {
                            Mi_SQL = "UPDATE " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos + " SET ";
                            Mi_SQL += Tra_Detalle_Documentos.Campo_Documento_ID + " = '" + Tramite.P_Documentacion_Tramite.Rows[cnt][1].ToString().Trim() + "'";
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Documento_Requerido + " = '" + Tramite.P_Documentacion_Tramite.Rows[cnt][2].ToString().Trim() + "'";
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Usuario_Modifico + " = '" + Tramite.P_Usuario + "'";
                            Mi_SQL += ", " + Tra_Detalle_Documentos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL += " WHERE " + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID + " = '" + Tramite.P_Documentacion_Tramite.Rows[cnt][0].ToString().Trim() + "'";
                        }
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                }

                //  clase de negocios con la informacion registrada sin ningun cambio
                //  se comparan los elementos eliminados < Tramite_Tmp.P_SubProcesos_Tramite >
                //  los elementos nuevos Tramite ( Tramite.P_SubProcesos_Tramite )
                //  si no existen en la nueva tabla se procede a eliminar el registro con sus detalles

                
                Dt_Actividades_Tramite = Tramite.Consultar_Subprocesos_Tramite();

                if (Dt_Actividades_Tramite != null && Dt_Actividades_Tramite.Rows.Count > 0)
                {
                    if (Tramite.P_SubProcesos_Tramite == null || Tramite.P_SubProcesos_Tramite.Rows.Count == 0)
                    {
                        foreach (DataRow Registro in Dt_Actividades_Tramite.Rows)
                        {
                            //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS
                            Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla;
                            Mi_SQL += " WHERE " + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + " = '" + Registro[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim() + "'";
                            Comando.CommandText = Mi_SQL;
                            Comando.ExecuteNonQuery();

                            //  SE ELIMINAN LOS DETALLES DE LOS FORMATOS
                            Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato;
                            Mi_SQL += " WHERE " + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + " = '" + Registro[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim() + "'";
                            Comando.CommandText = Mi_SQL;
                            Comando.ExecuteNonQuery();
                        }


                        //  SE ELIMINAN los subprocesos por medio del tramite id
                        Mi_SQL = "DELETE FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                        Mi_SQL += " WHERE " + Cat_Tra_Subprocesos.Campo_Tramite_ID + " = '" + Tramite.P_Tramite_ID.ToString() + "'";
                        Comando.CommandText = Mi_SQL;
                        Comando.ExecuteNonQuery();
                    }
                    else
                    {
                        //  se eliminaran los registros que ya no esten en la nueva tabla
                        foreach (DataRow Registro_Eliminado in Dt_Actividades_Tramite.Rows)
                        {
                            //  elemento id anterior para ver si ya no existe
                            Elemento_Eliminar = Registro_Eliminado[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim();

                            //  se compara con la nueva tabla
                            if (Tramite.P_SubProcesos_Tramite != null && Tramite.P_SubProcesos_Tramite.Rows.Count > 0)
                            {
                                foreach (DataRow Registro_Nuevo in Tramite.P_SubProcesos_Tramite.Rows)
                                {
                                    //  si no se encuentra se cambia el ID_Eliminar a verdadero para entrar a realizar el delete
                                    if (Elemento_Eliminar == Registro_Nuevo[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim())
                                    {
                                        ID_Eliminar = true;
                                        break;
                                    }
                                }
                                //  se eliminara el registro
                                if (ID_Eliminar == false)
                                {
                                    //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS
                                    Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla;
                                    Mi_SQL += " WHERE " + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + " = '" + Registro_Eliminado[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim() + "'";
                                    Comando.CommandText = Mi_SQL;
                                    Comando.ExecuteNonQuery();

                                    //  SE ELIMINAN LOS DETALLES DE LOS FORMATOS
                                    Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato;
                                    Mi_SQL += " WHERE " + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + " = '" + Registro_Eliminado[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim() + "'";
                                    Comando.CommandText = Mi_SQL;
                                    Comando.ExecuteNonQuery();


                                    //  SE ELIMINAN LOS DETALLES DE LOS FORMATOS
                                    Mi_SQL = "DELETE FROM " + Tra_Subprocesos_Perfiles.Tabla_Tra_Subprocesos_Perfiles;
                                    Mi_SQL += " WHERE " + Tra_Subprocesos_Perfiles.Campo_Subproceso_ID + " = '" + Registro_Eliminado[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim() + "'";
                                    Comando.CommandText = Mi_SQL;
                                    Comando.ExecuteNonQuery();

                                    //  SE ELIMINAN los subprocesos por medio del tramite id
                                    Mi_SQL = "DELETE FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                                    Mi_SQL += " WHERE " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " = '" + Registro_Eliminado[Cat_Tra_Subprocesos.Campo_Subproceso_ID].ToString().Trim() + "'";
                                    Comando.CommandText = Mi_SQL;
                                    Comando.ExecuteNonQuery();
                                }

                                //  se reinicia el falso
                                ID_Eliminar = false;
                            }
                        }
                    }

                }

                //  se actualizaran los elementos de la tabla nueva 
                //  tambien se insertaran los nuevos elementos con sus detalles
                Contador = 0;

                if (Tramite.P_SubProcesos_Tramite != null && Tramite.P_SubProcesos_Tramite.Rows.Count > 0)
                {
                    //   nota: se utilizara el tramite_id de la clase de negocio
                    for (int cnt = 0; cnt < Tramite.P_SubProcesos_Tramite.Rows.Count; cnt++)
                    {
                        //  se insertara el nuevo subproceso
                        if (Tramite.P_SubProcesos_Tramite.Rows[cnt]["SUBPROCESO_ID"].ToString().Trim() == "")
                        {
                            if (Subproceso_ID == "")
                                Subproceso_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos, Cat_Tra_Subprocesos.Campo_Subproceso_ID, 5);

                            //  se valida que contenga infromacion
                            if (Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString() != "" && Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString() != "0")
                                Condicion_Si = Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString().Trim();
                            else
                                Condicion_Si = "null";

                            if (Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString() != "" && Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString() != "0")
                                Condicion_No = Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString().Trim();
                            else
                                Condicion_No = "null";


                            Mi_SQL = "INSERT INTO " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " (";
                            Mi_SQL += Cat_Tra_Subprocesos.Campo_Subproceso_ID;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Tramite_ID;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Nombre;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Descripcion;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Valor;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Orden;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Plantilla;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Usuario_Creo;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Fecha_Creo;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Tipo_Actividad;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Condicion_Si;
                            Mi_SQL += ", " + Cat_Tra_Subprocesos.Campo_Condicion_No;
                            Mi_SQL += ") VALUES (";
                            Mi_SQL += "'" + Subproceso_ID + "'";
                            Mi_SQL += ", '" + Tramite.P_Tramite_ID + "'";
                            Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][1] + "'";
                            Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][2] + "'";
                            Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][3] + "'";
                            Mi_SQL += ",  " + Tramite.P_SubProcesos_Tramite.Rows[cnt][4] + " ";//   numerico
                            Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][5] + "'";
                            Mi_SQL += ", '" + Tramite.P_Usuario + "'";
                            Mi_SQL += ", SYSDATE ";
                            Mi_SQL += ", '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][6] + "' ";
                            Mi_SQL += ",  " + Condicion_Si + " ";//   numerico
                            Mi_SQL += ",  " + Condicion_No + " ";//   numerico
                            Mi_SQL += ")";
                            Comando.CommandText = Mi_SQL;
                            Comando.ExecuteNonQuery();

                            //  se ingresaran los detalles de la plantilla
                            if (Tramite.P_Dt_Detalle_Plantilla != null)
                            {
                                if (Tramite.P_Dt_Detalle_Plantilla == null)
                                {
                                    foreach (DataRow Registro in Tramite.P_Dt_Detalle_Plantilla.Rows)
                                    {
                                        if (Tramite.P_SubProcesos_Tramite.Rows[cnt]["ORDEN"].ToString() == Registro["ORDEN"].ToString())
                                        {
                                            Detalle_Plantilla_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla, Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID, 5);
                                         
                                            Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + " (";
                                            Mi_SQL += Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Usuario_Creo;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Fecha_Creo;
                                            Mi_SQL += ") VALUES (";
                                            Mi_SQL += "'" + Subproceso_ID + "'";
                                            Mi_SQL += ",'" + Tramite.P_Tramite_ID + "'";
                                            Mi_SQL += ",'" + Registro["PLANTILLA_ID"].ToString() + "'";
                                            Mi_SQL += ",'" + Detalle_Plantilla_ID + "'";
                                            Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                                            Comando.CommandText = Mi_SQL;
                                            Comando.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }

                            //  se ingresaran los detalles de la formato
                            if (Tramite.P_Dt_Detalle_Formato != null)
                            {
                                if (Tramite.P_Dt_Detalle_Formato.Rows.Count > 0)
                                {
                                    foreach (DataRow Registro in Tramite.P_Dt_Detalle_Formato.Rows)
                                    {
                                        if (Tramite.P_SubProcesos_Tramite.Rows[cnt]["ORDEN"].ToString() == Registro["ORDEN"].ToString())
                                        {
                                            Detalle_Formato_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato, Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID, 5);
                                          
                                            Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + " (";
                                            Mi_SQL += Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Usuario_Creo;
                                            Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Fecha_Creo;
                                            Mi_SQL += ") VALUES (";
                                            Mi_SQL += "'" + Subproceso_ID + "'";
                                            Mi_SQL += ",'" + Tramite.P_Tramite_ID + "'";
                                            Mi_SQL += ",'" + Registro["PLANTILLA_ID"].ToString() + "'";
                                            Mi_SQL += ",'" + Detalle_Formato_ID + "'";
                                            Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                                            Comando.CommandText = Mi_SQL;
                                            Comando.ExecuteNonQuery();
                                        }
                                    }
                                }
                            }
                        }


                        //  se actualiza el registro con la nueva informacion
                        else
                        {
                            //  se valida que conten
                            if (Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString() != "" && Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString() != "0")
                                Condicion_Si = Tramite.P_SubProcesos_Tramite.Rows[cnt][7].ToString().Trim();

                            else
                                Condicion_Si = "null";

                            if (Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString() != "" && Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString() != "0")
                                Condicion_No = Tramite.P_SubProcesos_Tramite.Rows[cnt][8].ToString().Trim();

                            else 
                                Condicion_No = "null";

                            Mi_SQL = "UPDATE " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + " SET ";
                            Mi_SQL = Mi_SQL + Cat_Tra_Subprocesos.Campo_Nombre + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][1].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Descripcion + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][2].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Valor + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][3].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Orden + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][4].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Plantilla + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][5].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Condicion_Si + " = " + Condicion_Si + "";//  numerico
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Condicion_No + " = " + Condicion_No + "";//  numerico
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Usuario_Modifico + " = '" + Tramite.P_Usuario + "'";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Fecha_Modifico + " = SYSDATE";
                            Mi_SQL = Mi_SQL + ", " + Cat_Tra_Subprocesos.Campo_Tipo_Actividad + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][6].ToString().Trim() + "'";
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + " = '" + Tramite.P_SubProcesos_Tramite.Rows[cnt][0].ToString().Trim() + "'";
                            Comando.CommandText = Mi_SQL;
                            Comando.ExecuteNonQuery();
                        }
                    }
                }

                //  se ingresaran los detalles de la formato
                if (Tramite.P_Matriz_Costo != null)
                {
                    if (Tramite.P_Matriz_Costo.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Tramite.P_Matriz_Costo.Rows)
                        {
                            if (!String.IsNullOrEmpty(Registro["MATRIZ_ID"].ToString()))
                            {
                                Mi_SQL = "UPDATE " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + " SET ";
                                Mi_SQL += Ope_Tra_Matriz_Costo.Campo_Tipo + " = '" + Registro["TIPO"].ToString() + "'";
                                Mi_SQL += ", " + Ope_Tra_Matriz_Costo.Campo_Costo_Base + " = '" + Registro["COSTO_BASE"].ToString() + "'";
                                Mi_SQL += ", " + Ope_Tra_Matriz_Costo.Campo_Usuario_Modifico + " = '" + Tramite.P_Usuario + "'";
                                Mi_SQL += ", " + Ope_Tra_Matriz_Costo.Campo_Fecha_Modifico + " = SYSDATE";
                                Mi_SQL += " Where " + Ope_Tra_Matriz_Costo.Campo_Matriz_ID + "='" + Registro["MATRIZ_ID"].ToString() + "'";
                                Comando.CommandText = Mi_SQL;
                                Comando.ExecuteNonQuery();
                            }
                            else
                            {
                                MATRIZ_ID = Obtener_ID_Consecutivo_Transaccion(ref Comando, Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo, Ope_Tra_Matriz_Costo.Campo_Matriz_ID, 10);
                                
                                Mi_SQL = "INSERT INTO " + Ope_Tra_Matriz_Costo.Tabla_Ope_Tra_Matriz_Costo + " (";
                                Mi_SQL += Ope_Tra_Matriz_Costo.Campo_Matriz_ID;
                                Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Tramite_ID;
                                Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Tipo;
                                Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Costo_Base;
                                Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Usuario_Creo;
                                Mi_SQL += "," + Ope_Tra_Matriz_Costo.Campo_Fecha_Creo;
                                Mi_SQL += ") VALUES (";
                                Mi_SQL += "'" + MATRIZ_ID + "'";
                                Mi_SQL += ",'" + Tramite.P_Tramite_ID + "'";
                                Mi_SQL += ",'" + Registro["TIPO"].ToString() + "'";
                                Mi_SQL += ",'" + Registro["COSTO_BASE"].ToString() + "'";
                                Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                                Comando.CommandText = Mi_SQL;
                                Comando.ExecuteNonQuery();
                            }
                        }

                    }
                }
                Transaccion.Commit();

            }// fin del try

            catch (OracleException Ex)
            {
                Transaccion.Rollback();
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
                Transaccion.Rollback();

                throw new Exception(Ex.ToString());
            }
            finally
            {
                Conexion.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Tramite
        ///DESCRIPCIÓN: Actualiza los datos de un tramite en la Base de Datos
        ///PARAMETROS:     
        ///             1. Tramite. Instancia de la Clase de Negocio de Tramites con los 
        ///                         datos del Tramite que va a ser Actualizado.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Dar_Baja_Tramite(String Tramite_ID)
        {
            String Mensaje = "";
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
                String Mi_SQL = "UPDATE " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " SET ";
                Mi_SQL = Mi_SQL + Cat_Tra_Tramites.Campo_Estatus_Tramite + " = 'BAJA'";
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID + " = '" + Tramite_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                if (Trans != null)
                {
                    Trans.Rollback();
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        ///NOMBRE:      Modificar_Detalles_Plantillas_Formato
        ///DESCRIPCIÓN: Modificara los detalles de las plantillas y formatos de la actividad del tramite 
        ///PROPIEDADES: 1. Tramite.   Tramite que se va eliminar.
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  19/Mayo/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static void Modificar_Detalles_Plantillas_Formato(Cls_Cat_Tramites_Negocio Tramite)
        {
            String Mensaje = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            Boolean Estado_Detalles = false;
            String Mi_SQL = "";
            Int32 Plantillas = 1;
            Int32 Formatos = 1;
            String Detalle_Plantilla_ID = "";
            String Detalle_Formato_ID = "";
            String Subproceso_ID = "";
            String Plantilla_ID = "";
            String Tramite_ID = "";
            DataTable Dt_Consulta_Plantilla_Formato = new DataTable();
            int Contador = 0;
            Boolean Id_Repetido = false;
            String Elemento_Eliminar = "";
            Boolean ID_Eliminar = false;
            try
            {
                //  se realiza una consulta para la obtener los campos anteriores
                // dato requerido para la consulta subproceo_id
                Dt_Consulta_Plantilla_Formato = Consultar_Detalles_Plantilla(Tramite);

                //  para eliminar los id
                if (Dt_Consulta_Plantilla_Formato != null)
                {
                    if (Dt_Consulta_Plantilla_Formato.Rows.Count > 0)
                    {
                        if (Tramite.P_Dt_Detalle_Plantilla == null)
                        {
                            //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS CON EL ID DE DETALLE
                            Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + " = '" + Tramite.P_Sub_Proceso_ID + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }

                        else
                        {
                            //  se eliminaran los registros que ya no esten en la nueva tabla
                            foreach (DataRow Registro_Eliminado in Dt_Consulta_Plantilla_Formato.Rows)
                            {
                                //  elemento id anterior para ver si ya no existe
                                Elemento_Eliminar = Registro_Eliminado[Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID].ToString();


                                //  se ejecuta la sentencia delete
                                if (Tramite.P_Dt_Detalle_Plantilla != null && Tramite.P_Dt_Detalle_Plantilla.Rows.Count > 0)
                                {
                                    foreach (DataRow Registro_Nuevo in Tramite.P_Dt_Detalle_Plantilla.Rows)
                                    {
                                        if (Elemento_Eliminar == Registro_Nuevo[Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID].ToString())
                                        {
                                            ID_Eliminar = true;
                                            break;
                                        }
                                    }
                                    //  se eliminara el registro
                                    if (ID_Eliminar == false)
                                    {
                                        //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS CON EL ID DE DETALLE
                                        Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla;
                                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID + " = '" + Elemento_Eliminar + "'";
                                        Cmd.CommandText = Mi_SQL;
                                        Cmd.ExecuteNonQuery();
                                    }

                                    //  se reinicia el falso
                                    ID_Eliminar = false;
                                }
                            }
                        }
                    }
                }


                //  se ingresaran los detalles de la plantilla nuevos
                if (Tramite.P_Dt_Detalle_Plantilla != null)
                {
                    if (Tramite.P_Dt_Detalle_Plantilla.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Tramite.P_Dt_Detalle_Plantilla.Rows)
                        {
                            Contador++;
                            //  se realizara una consulta para obtener los id ya que los registros nuevos no tiene id
                            if (Contador == 1)
                            {
                                //  se obtienen los id 
                                if (Dt_Consulta_Plantilla_Formato != null && Dt_Consulta_Plantilla_Formato.Rows.Count > 0)
                                {
                                    Subproceso_ID = Dt_Consulta_Plantilla_Formato.Rows[0][Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID].ToString();
                                    Tramite_ID = Dt_Consulta_Plantilla_Formato.Rows[0][Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID].ToString();
                                }
                                else
                                {
                                    Subproceso_ID = Tramite.P_Sub_Proceso_ID;
                                    Tramite_ID = Tramite.P_Tramite_ID;
                                }
                            }

                            //  se buscara que el id ya este repetido
                            if (Dt_Consulta_Plantilla_Formato != null && Dt_Consulta_Plantilla_Formato.Rows.Count > 0)
                            {
                                foreach (DataRow Registro_Plantilla in Dt_Consulta_Plantilla_Formato.Rows)
                                {
                                    Detalle_Plantilla_ID = Registro[Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID].ToString();

                                    //  elemento id anterior
                                    Elemento_Eliminar = Registro_Plantilla[Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID].ToString();

                                    //  para buscar que el id se repeta lo cual no provocara ninguna accion
                                    if (Detalle_Plantilla_ID == Registro_Plantilla[Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID].ToString())
                                    {
                                        Id_Repetido = true;
                                        break;
                                    }
                                }
                            }

                            //  se se encuentra repetido el id no se realizara ninguna accion
                            //  si no se encuentra se insertara la nueva plantilla
                            if (Id_Repetido != true)
                            {
                                //  se obtiene el id del detalle
                                Detalle_Plantilla_ID = Obtener_ID_Plantilla_Formato(ref Cmd, Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla, Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID, 5, Plantillas);
                                Plantillas++;

                                Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + " (";
                                Mi_SQL += Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Usuario_Creo;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Plantilla.Campo_Fecha_Creo;
                                Mi_SQL += ") VALUES (";
                                Mi_SQL += "'" + Subproceso_ID + "'";
                                Mi_SQL += ",'" + Tramite_ID + "'";
                                Mi_SQL += ",'" + Registro["PLANTILLA_ID"].ToString() + "'";
                                Mi_SQL += ",'" + Detalle_Plantilla_ID + "'";
                                Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                            }
                            //  se inicializa para otra busqueda
                            Id_Repetido = false;
                        }
                    }
                }


                //  para formatos*****************************************

                //  se realiza una consulta para la obtener los campos anteriores
                // dato requerido para la consulta subproceo_id
                Dt_Consulta_Plantilla_Formato = Consultar_Detalles_Formato(Tramite);

                //  para eliminar los id
                if (Dt_Consulta_Plantilla_Formato != null)
                {
                    if (Dt_Consulta_Plantilla_Formato.Rows.Count > 0)
                    {
                        if (Tramite.P_Dt_Detalle_Formato == null)
                        {
                            //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS CON EL ID DE DETALLE
                            Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + " = '" + Tramite.P_Sub_Proceso_ID + "'";
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                        }
                        else
                        {
                            //  se eliminaran los registros que ya no esten en la nueva tabla
                            foreach (DataRow Registro_Eliminado in Dt_Consulta_Plantilla_Formato.Rows)
                            {
                                //  elemento id anterior para ver si ya no existe
                                Elemento_Eliminar = Registro_Eliminado[Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID].ToString().Trim();


                                //  se ejecuta la sentencia delete
                                if (Tramite.P_Dt_Detalle_Formato != null && Tramite.P_Dt_Detalle_Formato.Rows.Count > 0)
                                {
                                    foreach (DataRow Registro_Nuevo in Tramite.P_Dt_Detalle_Formato.Rows)
                                    {
                                        if (Elemento_Eliminar.Trim() == Registro_Nuevo["DETALLE_FORMATO_ID"].ToString().Trim())
                                        {
                                            ID_Eliminar = true;
                                            break;
                                        }
                                    }
                                    //  se eliminara el registro
                                    if (ID_Eliminar == false)
                                    {
                                        //  SE ELIMINAN LOS DETALLES DE LAS PLANTILLAS CON EL ID DE DETALLE
                                        Mi_SQL = "DELETE FROM " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato;
                                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID + " = '" + Elemento_Eliminar + "'";
                                        Cmd.CommandText = Mi_SQL;
                                        Cmd.ExecuteNonQuery();
                                    }

                                    //  se reinicia el falso
                                    ID_Eliminar = false;
                                }
                            }
                        }
                    }
                }

                Contador = 0;
                //  se ingresaran los detalles de la plantilla nuevos
                if (Tramite.P_Dt_Detalle_Formato != null)
                {
                    if (Tramite.P_Dt_Detalle_Formato.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Tramite.P_Dt_Detalle_Formato.Rows)
                        {
                            Contador++;
                            //  se realizara una consulta para obtener los id ya que los registros nuevos no tiene id
                            if (Contador == 1)
                            {

                                //  se obtienen los id 
                                if (Dt_Consulta_Plantilla_Formato != null && Dt_Consulta_Plantilla_Formato.Rows.Count > 0)
                                {
                                    Subproceso_ID = Dt_Consulta_Plantilla_Formato.Rows[0][Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID].ToString();
                                    Tramite_ID = Dt_Consulta_Plantilla_Formato.Rows[0][Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID].ToString();
                                }
                                else
                                {
                                    Subproceso_ID = Tramite.P_Sub_Proceso_ID;
                                    Tramite_ID = Tramite.P_Tramite_ID;
                                }
                            }


                            //  se buscara que el id ya este repetido
                            if (Dt_Consulta_Plantilla_Formato != null && Dt_Consulta_Plantilla_Formato.Rows.Count > 0)
                            {
                                foreach (DataRow Registro_Plantilla in Dt_Consulta_Plantilla_Formato.Rows)
                                {
                                    Detalle_Plantilla_ID = Registro["DETALLE_FORMATO_ID"].ToString();

                                    //  elemento id anterior
                                    Elemento_Eliminar = Registro_Plantilla[Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID].ToString().Trim();

                                    //  para buscar que el id se repeta lo cual no provocara ninguna accion
                                    if (Detalle_Plantilla_ID.Trim() == Registro_Plantilla[Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID].ToString().Trim())
                                    {
                                        Id_Repetido = true;
                                        break;
                                    }
                                }
                            }

                            //  se se encuentra repetido el id no se realizara ninguna accion
                            //  si no se encuentra se insertara la nueva plantilla
                            if (Id_Repetido != true)
                            {
                                //  se obtiene el id del detalle
                                Detalle_Formato_ID = Obtener_ID_Plantilla_Formato(ref Cmd,  Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato, Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID, 5, Formatos);
                                Formatos++;

                                Mi_SQL = "INSERT INTO " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + " (";
                                Mi_SQL += Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Usuario_Creo;
                                Mi_SQL += "," + Ope_Tra_Det_Sproc_Formato.Campo_Fecha_Creo;
                                Mi_SQL += ") VALUES (";
                                Mi_SQL += "'" + Subproceso_ID + "'";
                                Mi_SQL += ",'" + Tramite_ID + "'";
                                Mi_SQL += ",'" + Registro["PLANTILLA_ID"].ToString() + "'";
                                Mi_SQL += ",'" + Detalle_Formato_ID + "'";
                                Mi_SQL += ",'" + Tramite.P_Usuario + "', SYSDATE )";
                                Cmd.CommandText = Mi_SQL;
                                Cmd.ExecuteNonQuery();
                            }
                            //  se reinicia para otra comparacion
                            Id_Repetido = false;
                        }
                    }
                }

                Trans.Commit();
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
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
                Trans.Rollback();

                throw new Exception(Ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Registros_SubCatalogos_Eliminados
        ///DESCRIPCIÓN: Obtiene la lista de los Datos_Tramite, Subprocesos, 
        ///             Documentacion_Tramite y Autorizadores_Tramite eliminados.
        ///             
        ///PARAMETROS:     
        ///             1. Actuales.        Objeto de la Clase de Tramite con los tramites 
        ///                                 actuales y que se van a comparar con los que ha
        ///                                 actualizado el usuario para sacar los eliminados.
        ///             2. Actualizados.    Objeto de la Clase de Tramite con los tramites
        ///                                 actualizados y que se van a comparar con los que
        ///                                 estan guardados en la Base de Datos para sacar los
        ///                                 eliminados.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static Cls_Cat_Tramites_Negocio Obtener_Registros_SubCatalogos_Eliminados(Cls_Cat_Tramites_Negocio Actuales, Cls_Cat_Tramites_Negocio Actualizados)
        {
            Cls_Cat_Tramites_Negocio Tramite = new Cls_Cat_Tramites_Negocio();

            //SACAR LOS AUTORIZADORES ELIMINADOS PARA TRAMITE
            DataTable Tabla_Perfiles = new DataTable();
            Tabla_Perfiles.Columns.Add("ID_ELIMINAR", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Perfiles_Autorizar.Rows.Count; cnt1++)
            {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Perfiles_Autorizar.Rows.Count; cnt2++)
                {
                    if (!Actualizados.P_Perfiles_Autorizar.Rows[cnt2][0].ToString().Equals(""))
                    {
                        if (Actuales.P_Perfiles_Autorizar.Rows[cnt1][0].ToString().Equals(Actualizados.P_Perfiles_Autorizar.Rows[cnt2][0].ToString()))
                        {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar)
                {
                    DataRow Fila = Tabla_Perfiles.NewRow();
                    Fila["ID_ELIMINAR"] = Actuales.P_Perfiles_Autorizar.Rows[cnt1][0].ToString();
                    Tabla_Perfiles.Rows.Add(Fila);
                }
            }
            Tramite.P_Perfiles_Autorizar = Tabla_Perfiles;

            //SACAR LOS DATOS ELIMINADOS PARA EL TRAMITE
            DataTable Tabla_Datos = new DataTable();
            Tabla_Datos.Columns.Add("ID_ELIMINAR", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Datos_Tramite.Rows.Count; cnt1++)
            {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Datos_Tramite.Rows.Count; cnt2++)
                {
                    if (!Actualizados.P_Datos_Tramite.Rows[cnt2][0].ToString().Equals(""))
                    {
                        if (Actuales.P_Datos_Tramite.Rows[cnt1][0].ToString().Equals(Actualizados.P_Datos_Tramite.Rows[cnt2][0].ToString()))
                        {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar)
                {
                    DataRow Fila = Tabla_Datos.NewRow();
                    Fila["ID_ELIMINAR"] = Actuales.P_Datos_Tramite.Rows[cnt1][0].ToString();
                    Tabla_Datos.Rows.Add(Fila);
                }
            }
            Tramite.P_Datos_Tramite = Tabla_Datos;

            //SACAR LOS DOCUMENTOS ELIMINADOS PARA TRAMITE
            DataTable Tabla_Documentos = new DataTable();
            Tabla_Documentos.Columns.Add("ID_ELIMINAR", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_Documentacion_Tramite.Rows.Count; cnt1++)
            {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_Documentacion_Tramite.Rows.Count; cnt2++)
                {
                    if (!Actualizados.P_Documentacion_Tramite.Rows[cnt2][0].ToString().Equals(""))
                    {
                        if (Actuales.P_Documentacion_Tramite.Rows[cnt1][0].ToString().Equals(Actualizados.P_Documentacion_Tramite.Rows[cnt2][0].ToString()))
                        {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar)
                {
                    DataRow Fila = Tabla_Documentos.NewRow();
                    Fila["ID_ELIMINAR"] = Actuales.P_Documentacion_Tramite.Rows[cnt1][0].ToString();
                    Tabla_Documentos.Rows.Add(Fila);
                }
            }
            Tramite.P_Documentacion_Tramite = Tabla_Documentos;

            //SACAR LOS SUBPROCESOS ELIMINADOS PARA TRAMITE
            DataTable Tabla_Subprocesos = new DataTable();
            Tabla_Subprocesos.Columns.Add("ID_ELIMINAR", Type.GetType("System.String"));
            for (int cnt1 = 0; cnt1 < Actuales.P_SubProcesos_Tramite.Rows.Count; cnt1++)
            {
                bool eliminar = true;
                for (int cnt2 = 0; cnt2 < Actualizados.P_SubProcesos_Tramite.Rows.Count; cnt2++)
                {
                    if (!Actualizados.P_SubProcesos_Tramite.Rows[cnt2][0].ToString().Equals(""))
                    {
                        if (Actuales.P_SubProcesos_Tramite.Rows[cnt1][0].ToString().Equals(Actualizados.P_SubProcesos_Tramite.Rows[cnt2][0].ToString()))
                        {
                            eliminar = false;
                            break;
                        }
                    }
                }
                if (eliminar)
                {
                    DataRow Fila = Tabla_Subprocesos.NewRow();
                    Fila["ID_ELIMINAR"] = Actuales.P_SubProcesos_Tramite.Rows[cnt1][0].ToString();
                    Tabla_Subprocesos.Rows.Add(Fila);
                }
            }
            Tramite.P_SubProcesos_Tramite = Tabla_Subprocesos;


            return Tramite;
        }

        #endregion

        #region Consultas
        /// *******************************************************************************
        /// NOMBRE:         Consultar_Subprocesos_Tramite
        /// COMENTARIOS:    consultara las actividades del tramite
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     21/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Subprocesos_Tramite(Cls_Cat_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT *");
                Mi_SQL.Append(" From " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                // filtrar por id del trámite
                if (!string.IsNullOrEmpty(Datos.P_Tramite_ID))
                {
                    Mi_SQL.Append(" Where " + Cat_Tra_Subprocesos.Campo_Tramite_ID + "='" + Datos.P_Tramite_ID + "' ");
                }
                else if (!string.IsNullOrEmpty(Datos.P_Sub_Proceso_ID))
                {
                    Mi_SQL.Append(" Where " + Cat_Tra_Subprocesos.Campo_Subproceso_ID + "='" + Datos.P_Sub_Proceso_ID + "' ");
                }

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
        /// NOMBRE:         Consultar_Detalles_Plantilla
        /// COMENTARIOS:    consultara los detalles de las plantillas
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     18/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Detalles_Plantilla(Cls_Cat_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID);
                Mi_SQL.Append(", " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID);
                Mi_SQL.Append(", " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID);
                Mi_SQL.Append(", " + Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Detalle_Plantilla_ID);
                Mi_SQL.Append(", " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + "=");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Plantilla_ID + "=");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Plantilla_ID);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Tramite_ID + "=");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Plantilla.Tabla_Ope_Tra_Det_Sproc_Plantilla + "." + Ope_Tra_Det_Sproc_Plantilla.Campo_Subproceso_ID + "='" + Datos.P_Tipo_Actividad + "'");

                Mi_SQL.Append(" order by " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

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
        /// NOMBRE:         Consultar_Detalles_Formato
        /// COMENTARIOS:    consultara los detalles de los formatos
        /// PARÁMETROS:     Datos.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     18/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Detalles_Formato(Cls_Cat_Tramites_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID);
                Mi_SQL.Append(", " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID);
                Mi_SQL.Append(", " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID);
                Mi_SQL.Append(", " + Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Detalle_Formato_ID);
                Mi_SQL.Append(", " + Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Nombre);
                Mi_SQL.Append(", " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

                Mi_SQL.Append(" From ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + "=");
                Mi_SQL.Append(Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Subproceso_ID);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Plantilla_ID + "=");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Formato_ID);

                Mi_SQL.Append(" left outer join ");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites);
                Mi_SQL.Append(" on ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato+ "." + Ope_Tra_Det_Sproc_Formato.Campo_Tramite_ID + "=");
                Mi_SQL.Append(Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + "." + Cat_Tra_Tramites.Campo_Tramite_ID);

                Mi_SQL.Append(" where ");
                Mi_SQL.Append(Ope_Tra_Det_Sproc_Formato.Tabla_Ope_Tra_Det_Sproc_Formato + "." + Ope_Tra_Det_Sproc_Formato.Campo_Subproceso_ID + "='" + Datos.P_Tipo_Actividad + "'");

                Mi_SQL.Append(" order by " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos + "." + Cat_Tra_Subprocesos.Campo_Orden);

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
        /// NOMBRE:         Consultar_Cuenta
        /// COMENTARIOS:    Metodo para consultar los cuentas de ingresos
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     31/Mayo/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Cuenta(Cls_Cat_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(", " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);
                Mi_SQL.Append(", " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion);
                Mi_SQL.Append(", " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Importe + " as IMPORTE" );
                Mi_SQL.Append(", " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);
                Mi_SQL.Append("|| '' ||" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion + " as Clave_Nombre");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing);

                Mi_SQL.Append("  left outer join " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + " on ");
                Mi_SQL.Append(Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Concepto_Ing_ID);
                Mi_SQL.Append(" = " + Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Concepto_Ing_ID);

                Mi_SQL.Append("  left outer join " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " on ");
                Mi_SQL.Append(Cat_Psp_Concepto_Ing.Tabla_Cat_Psp_Concepto_Ing + "." + Cat_Psp_Concepto_Ing.Campo_Cuenta_Contable_ID);
                Mi_SQL.Append(" = " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID);

                String Texto_Auxiliar = "";

                if (!String.IsNullOrEmpty(Negocio.P_Cuenta))
                {
                    Texto_Auxiliar = Mi_SQL.ToString();
                    if (Texto_Auxiliar.Contains("where"))
                    {
                        Mi_SQL.Append(" and upper(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);
                        Mi_SQL.Append(") like upper('%" + Negocio.P_Cuenta + "%')");
                    }
                    else
                    {
                        Mi_SQL.Append(" where upper(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);
                        Mi_SQL.Append(") like upper('%" + Negocio.P_Cuenta + "%')");
                    }
                }
                if (!String.IsNullOrEmpty(Negocio.P_Nombre_Cuenta))
                {
                    Texto_Auxiliar = Mi_SQL.ToString();
                    if (Texto_Auxiliar.Contains("where"))
                    {
                        Mi_SQL.Append(" and upper(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion);
                        Mi_SQL.Append(") like upper('%" + Negocio.P_Nombre_Cuenta + "%')");
                    }
                    else
                    {
                        Mi_SQL.Append(" where upper(" + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Descripcion);
                        Mi_SQL.Append(") like upper('%" + Negocio.P_Nombre_Cuenta + "%')");
                    }
                }
                if (!String.IsNullOrEmpty(Negocio.P_Cuenta_Contable_Clave))
                {
                    Texto_Auxiliar = Mi_SQL.ToString();
                    if (Texto_Auxiliar.Contains("where"))
                    {
                        Mi_SQL.Append(" and " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);
                        Mi_SQL.Append("= '" + Negocio.P_Cuenta_Contable_Clave + "'");
                    }
                    else
                    {
                        Mi_SQL.Append(" where " + Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);
                        Mi_SQL.Append("= '" + Negocio.P_Cuenta_Contable_Clave + "'");
                    }
                }
                //where CAT_PSP_SUBCONCEPTO_ING.CLAVE='1201010001'

                Mi_SQL.Append(" ORDER BY ");
                Mi_SQL.Append(Cat_Psp_SubConcepto_Ing.Tabla_Cat_Psp_SubConcepto_Ing + "." + Cat_Psp_SubConcepto_Ing.Campo_Clave);

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
        /// NOMBRE:         Consultar_Plantillas
        /// COMENTARIOS:    Metodo para consultar las plantillas
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     19/Junio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Plantillas(Cls_Cat_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + ".*");
                Mi_SQL.Append(" FROM " + Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas);

                if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                {
                    Mi_SQL.Append(" WHERE ");
                    Mi_SQL.Append(" upper(" + Cat_Tra_Plantillas.Campo_Nombre + ") like( upper('%" + Negocio.P_Nombre + "%') )");
                }

                Mi_SQL.Append(" ORDER BY ");
                Mi_SQL.Append(Cat_Tra_Plantillas.Tabla_Cat_Tra_Plantillas + "." + Cat_Tra_Plantillas.Campo_Nombre);

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
        /// NOMBRE:         Consultar_Formatos
        /// COMENTARIOS:    Metodo para consultar los formatos
        /// PARÁMETROS:     Negocio.- Valor de los campos a consultar 
        /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ:     19/Junio/2012 
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA DE LA MODIFICACIÓN:
        /// ******************************************************************************
        public static DataTable Consultar_Formatos(Cls_Cat_Tramites_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para las consultas
            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + ".*");
                Mi_SQL.Append(" FROM " + Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido);

                if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                {
                    Mi_SQL.Append(" WHERE ");
                    Mi_SQL.Append(" upper(" + Cat_Tra_Formato_Predefinido.Campo_Nombre + ") like( upper('%" + Negocio.P_Nombre + "%') )");
                }
                Mi_SQL.Append(" ORDER BY ");
                Mi_SQL.Append(Cat_Tra_Formato_Predefinido.Tabla_Cat_Tra_Formato_Predefinido + "." + Cat_Tra_Formato_Predefinido.Campo_Nombre);

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