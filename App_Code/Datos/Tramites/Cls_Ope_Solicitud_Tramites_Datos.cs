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
using System.Text;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Catalogo_Tramites.Negocio;

namespace Presidencia.Solicitud_Tramites.Datos
{
    public class Cls_Ope_Solicitud_Tramites_Datos
    {
        #region Métodos

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

                try
                {
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
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                } 
                return Consecutivo;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud
            ///DESCRIPCIÓN: crea una sentencia sql para insertar un Solicitud en la base de datos
            ///PARAMETROS: 1.-Solicitud, objeto de la calse de negocio que contiene los datos para realizar la consulta
            ///            2.-Usuario_Creo, Nombre del usuario logueado actualmente en el sistema
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static String Alta_Solicitud(Cls_Ope_Solicitud_Tramites_Negocio Solicitud, String Usuario_Creo)
            {
                String Consecutivo = "";
                String Mi_SQL = "";
                OracleConnection Cn = new OracleConnection();
                OracleCommand Cmd = new OracleCommand();
                OracleTransaction Trans;
                Object Aux;

                Cn.ConnectionString = Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;
                
                try
                {
                    //INSERTAR LA SOLICITUD
                    Solicitud.P_Solicitud_ID = Obtener_Id_Consecutivo(Ope_Tra_Solicitud.Campo_Solicitud_ID, Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);
                    Consecutivo = Solicitud.P_Solicitud_ID;

                    Mi_SQL = "INSERT INTO " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " (";
                    Mi_SQL += Ope_Tra_Solicitud.Campo_Solicitud_ID;//1  
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Tramite_ID;//2
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Estatus;//3
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Clave_Solicitud;//4
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Porcentaje_Avance;//5
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Usuario_Creo;//6
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Creo;//7
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Entrega;//8
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Nombre_Solicitante;//9
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Apellido_Paterno;//10
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Apellido_Materno;//11
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Subproceso_ID;//12
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Correo_Electronico;//13
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Cuenta_Predial;//14
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Inspector_ID;//15
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Zona_ID;//16
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Empleado_ID;//17
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Folio;//18
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Costo_Base;//19
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Cantidad;//20
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Costo_Total;//21
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Contribuyente_Id;//22
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Direccion_Predio;//23
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Propietario_Predio;//24
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Calle_Predio;//25
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Numero_Predio;//26
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Manzana_Predio;//27
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Lote_Predio;//28
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Otros_Predio;//29
                    Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Complemento;//30

                    if (!String.IsNullOrEmpty(Solicitud.P_Consecutivo))
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Consecutivo;//31

                    Mi_SQL += ") VALUES ('";

                    Mi_SQL += Solicitud.P_Solicitud_ID + "'";//1
                    Mi_SQL += ", '" + Solicitud.P_Tramite_ID + "'";//2
                    Mi_SQL += ", '" + Solicitud.P_Estatus + "'";//3
                    Mi_SQL += ", '" + Solicitud.P_Clave_Solicitud + "'";//4
                    Mi_SQL += ", '" + Solicitud.P_Porcentaje + "'";//5
                    Mi_SQL += ", '" + Usuario_Creo + "'";//6
                    Mi_SQL += ", SYSDATE";//7
                    Mi_SQL += ", '" + String.Format("{0:dd/MM/yyyy}", Solicitud.P_Fecha_Entrega) + "' ";//8
                    Mi_SQL += ", '" + Solicitud.P_Nombre_Solicitante + "' ";//9
                    Mi_SQL += ", '" + Solicitud.P_Apellido_Paterno + "'";//10
                    Mi_SQL += ", '" + Solicitud.P_Apellido_Materno + "'";//11
                    Mi_SQL += ", '" + Solicitud.P_Subproceso_ID + "'";//12
                    Mi_SQL += ", '" + Solicitud.P_E_Mail + "'";//13
                    Mi_SQL += ", '" + Solicitud.P_Cuenta_Predial + "'";//14
                    Mi_SQL += ", '" + Solicitud.P_Perito_ID + "'";//15
                    Mi_SQL += ", '" + Solicitud.P_Zona_ID + "'";//16
                    Mi_SQL += ", '" + Solicitud.P_Empleado_ID + "'";//17
                    Mi_SQL += ", '" + Solicitud.P_Folio + "/" + Convert.ToInt64(Solicitud.P_Solicitud_ID).ToString() + "/" + DateTime.Now.Year.ToString() + "'";//18
                    Mi_SQL += ", '" + Solicitud.P_Costo_Base + "'";//19
                    Mi_SQL += ", '" + Solicitud.P_Cantidad + "'";//20
                    Mi_SQL += ", '" + Solicitud.P_Costo_Total + "' ";//21
                    Mi_SQL += ", '" + Solicitud.P_Contribuyente_ID + "'";//22
                    Mi_SQL += ", '" + Solicitud.P_Direccion_Predio + "'";//23
                    Mi_SQL += ", '" + Solicitud.P_Propietario_Predio + "'";//24
                    Mi_SQL += ", '" + Solicitud.P_Calle_Predio + "'";//25
                    Mi_SQL += ", '" + Solicitud.P_Nuemro_Predio + "'";//26
                    Mi_SQL += ", '" + Solicitud.P_Manzana_Predio + "'";//27
                    Mi_SQL += ", '" + Solicitud.P_Lote_Predio + "'";//28
                    Mi_SQL += ", '" + Solicitud.P_Otros_Predio + "'";//29
                    Mi_SQL += ", '" + Solicitud.P_Complemento + "'";//30

                    if (!String.IsNullOrEmpty(Solicitud.P_Consecutivo))
                        Mi_SQL += ", '" + Solicitud.P_Consecutivo + "'";//31

                    Mi_SQL += ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();

                    Mi_SQL = "SELECT NVL(MAX (" + Ope_Tra_Datos.Campo_Ope_Dato_ID + "),'0000000000') ";
                    Mi_SQL += "FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos;
                    //Ejecutar consulta del consecutivo
                    Cmd.CommandText = Mi_SQL;
                    Aux = Cmd.ExecuteScalar();

                    //INSERTAR  LOS DATOS PROPORCIONADOS PAR LA SOLICITUD EN LA BASE DE DATOS
                    int Columnas = Solicitud.P_Datos.Length / 2;
                    for (int Cnt_Datos = 0; Cnt_Datos < Columnas; Cnt_Datos++)
                    {
                        if (Convert.IsDBNull(Aux))
                        {
                            Consecutivo = "0000000001";
                        }
                        else
                        {
                            Consecutivo = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                        }

                        Mi_SQL = "INSERT INTO " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " (";
                        Mi_SQL += Ope_Tra_Datos.Campo_Ope_Dato_ID;
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Tramite_ID;
                        Mi_SQL += ", " + Ope_Tra_Datos.Campo_Solicitud_ID;
                        Mi_SQL += ", " + Ope_Tra_Datos.Campo_Dato_ID;
                        Mi_SQL += ", " + Ope_Tra_Datos.Campo_Valor;
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Usuario_Creo;
                        Mi_SQL += ", " + Ope_Tra_Solicitud.Campo_Fecha_Creo;
                        Mi_SQL += ", " + Ope_Tra_Datos.Campo_Tipo_Dato;

                        Mi_SQL += ") VALUES ('";

                        Mi_SQL += Consecutivo + "'";
                        Mi_SQL += ", '" + Solicitud.P_Tramite_ID + "'";
                        Mi_SQL += ", '" + Solicitud.P_Solicitud_ID + "'";
                        Mi_SQL += ", '" + Solicitud.P_Datos[Cnt_Datos, 0] + "'";
                        Mi_SQL += ", '" + Solicitud.P_Datos[Cnt_Datos, 1] + "'";
                        Mi_SQL += ", '" + Usuario_Creo + "'";
                        Mi_SQL += ", SYSDATE";

                        if (!String.IsNullOrEmpty(Solicitud.P_Consecutivo))
                            Mi_SQL += ", 'FINAL'";
                        else
                            Mi_SQL += ", 'INICIAL'";

                        Mi_SQL += ")";
                        Mi_SQL += " RETURNING " + Ope_Tra_Datos.Campo_Ope_Dato_ID + " INTO :ID";

                        OracleParameter Ultimo_Id = new OracleParameter(":ID", Aux);
                        Ultimo_Id.Direction = ParameterDirection.Output;
                        Cmd.Parameters.Add(Ultimo_Id);
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Aux = Ultimo_Id.Value.ToString();
                    }

                    Mi_SQL = "SELECT NVL(MAX (" + Ope_Tra_Documentos.Campo_Ope_Documento_ID + "),'0000000000') ";
                    Mi_SQL += "FROM " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos;
                    //Ejecutar consulta del consecutivo
                    Cmd.CommandText = Mi_SQL;
                    Cmd.Parameters.Clear();
                    Aux = Cmd.ExecuteScalar();

                    //INSERTAR LOS DATOS DE LOS DOCUMENTOS DE LA SOLICITUD EN LA BASE DE DATOS
                    Columnas = Solicitud.P_Documentos.Length / 2;
                    for (int Cnt_Documentos = 0; Cnt_Documentos < Columnas; Cnt_Documentos++)
                    {
                        if (Solicitud.P_Documentos[Cnt_Documentos, 0] != null)
                        {
                            if (Convert.IsDBNull(Aux))
                            {
                                Consecutivo = "0000000001";
                            }
                            else
                            {
                                Consecutivo = String.Format("{0:0000000000}", Convert.ToInt32(Aux) + 1);
                            }
                            Mi_SQL = "INSERT INTO " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos + " (";
                            Mi_SQL += Ope_Tra_Documentos.Campo_Ope_Documento_ID;
                            Mi_SQL += ", " + Ope_Tra_Documentos.Campo_Detalle_Documento_ID;
                            Mi_SQL += ", " + Ope_Tra_Documentos.Campo_Solicitud_ID;
                            Mi_SQL += ", " + Ope_Tra_Documentos.Campo_URL;
                            Mi_SQL += ", " + Ope_Tra_Documentos.Campo_Usuario_Creo;
                            Mi_SQL += ", " + Ope_Tra_Documentos.Campo_Fecha_Creo;

                            Mi_SQL += ") VALUES ('";

                            Mi_SQL += Consecutivo + "'";
                            Mi_SQL += ", '" + Solicitud.P_Documentos[Cnt_Documentos, 0] + "'";
                            Mi_SQL += ", '" + Solicitud.P_Solicitud_ID + "'";
                            Mi_SQL += ", '" + Solicitud.P_Documentos[Cnt_Documentos, 1] + "'";
                            Mi_SQL += ", '" + Usuario_Creo + "'";
                            Mi_SQL += ", SYSDATE ";
                            Mi_SQL += ")";
                            Mi_SQL += " RETURNING " + Ope_Tra_Documentos.Campo_Ope_Documento_ID + " INTO :ID";

                            OracleParameter Ultimo_Id = new OracleParameter(":ID", Aux);
                            Ultimo_Id.Direction = ParameterDirection.Output;
                            Cmd.Parameters.Add(Ultimo_Id);
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery();
                            Aux = Ultimo_Id.Value.ToString();
                        }
                    }
                    Trans.Commit();
                }
                catch (Exception Ex)
                {
                    Trans.Rollback();
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                finally
                {
                    Cn.Close();
                    Cmd = null;
                    Cn = null;
                    Trans = null;
                }
                return Solicitud.P_Solicitud_ID;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Solicitud
            ///DESCRIPCIÓN: crea una sentencia sql para insertar un Solicitud en la base de datos
            ///PARAMETROS: 1.-Solicitud, objeto de la calse de negocio que contiene los datos para realizar la consulta
            ///            2.-Usuario_Creo, Nombre del usuario logueado actualmente en el sistema
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static void Modificar_Solicitud(Cls_Ope_Solicitud_Tramites_Negocio Solicitud, String Usuario_Modifico)
            {
                //Char[] Ch = { ' ' };
                //String[] Str = DateTime.Now.ToString().Split(Ch);
                //String Fecha_Modifico = Str[0];
                //String[] Fecha = Fecha_Modifico.Split('/');
                //Fecha_Modifico = "";
                //Fecha_Modifico = Fecha[1] + "/" + Fecha[0] + "/" + Fecha[2];
                //String Mi_SQL = "UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud +
                //    " SET " + Ope_Tra_Solicitud.Campo_Apellido_Materno +
                //    " = '" + Solicitud.P_Apellido_Materno +
                //    "', " + Ope_Tra_Solicitud.Campo_Apellido_Paterno +
                //    " = '" + Solicitud.P_Apellido_Paterno +
                //    "', " + Ope_Tra_Solicitud.Campo_Estatus +
                //    " = '" + Solicitud.P_Estatus +
                //    "', " + Ope_Tra_Solicitud.Campo_Nombre_Solicitante +
                //    " = '" + Solicitud.P_Nombre_Solicitante +
                //    "', " + Ope_Tra_Datos.Campo_Usuario_Creo +
                //    " = '" + Usuario_Modifico +
                //    "', " + Ope_Tra_Datos.Campo_Fecha_Creo +
                //    "', " + Ope_Tra_Solicitud.Campo_Costo_Base + "='" + Solicitud.P_Costo_Base + "'" +
                //    "', " + Ope_Tra_Solicitud.Campo_Cantidad + "='" + Solicitud.P_Cantidad + "'" +
                //    "', " + Ope_Tra_Solicitud.Campo_Costo_Total + "='" + Solicitud.P_Costo_Total + "'" +
                //    " = SYSDATE" +
                //    " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID +
                //    " = '" + Solicitud.P_Solicitud_ID + "'";
                //int row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


                ////MODIFICAR LOS DATOS DE LA SOLICITUD
                //Mi_SQL = "";
                //int Columnas = Solicitud.P_Datos.Length / 2;
                //for (int i = 0; i < Columnas; i++)
                //{
                //    Mi_SQL = "UPDATE " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos +
                //    " SET " + Ope_Tra_Datos.Campo_Valor + " = '" + Solicitud.P_Datos[i, 1] +
                //    "', " + Ope_Tra_Datos.Campo_Usuario_Modifico +
                //    " = '" + Usuario_Modifico +
                //    "', " + Ope_Tra_Datos.Campo_Fecha_Modifico +
                //    " = SYSDATE" +
                //    " WHERE " + Ope_Tra_Solicitud.Campo_Solicitud_ID +
                //    " = '" + Solicitud.P_Solicitud_ID +
                //    "' AND " + Ope_Tra_Datos.Campo_Dato_ID + " = '" +
                //    Solicitud.P_Datos[i, 0] + "'";
                    
                //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //}

             }

            ///****************************************************************************************
            /// NOMBRE DE LA FUNCION: Modificar_Solicitud_Estatus_Pendiente
            /// DESCRIPCION : modifica una soliciutd en la Base de Datos.
            /// PARAMETROS  : 
            ///               1.Negocio.    Objeto de la Clase de Negocio de Tramite con las propiedades
            ///                             del Tramite que va a ser modificado.
            /// CREO        : Hugo Enrique Ramírez Aguilera
            /// FECHA_CREO  : 16-Julio-2012
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///****************************************************************************************
            public static Boolean Modificar_Solicitud_Estatus_Pendiente(Cls_Ope_Solicitud_Tramites_Negocio Negocio)
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

                Boolean Operacion_Completa = false;//Estado de la operacion.
                StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
                try
                {
                    //SE ACTULIZAN LOS DATOS de la solicitud
                     int Columnas = Negocio.P_Datos.Length / 2;
                     int Contador_Datos = 0;

                     Mi_SQL = new StringBuilder();
                     Mi_SQL.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " set ");

                     Mi_SQL.Append(Ope_Tra_Solicitud.Campo_Cuenta_Predial + "='" + Negocio.P_Cuenta_Predial + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Direccion_Predio + "='" + Negocio.P_Direccion_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Propietario_Predio + "='" + Negocio.P_Propietario_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Calle_Predio + "='" + Negocio.P_Calle_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Numero_Predio + "='" + Negocio.P_Nuemro_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Manzana_Predio + "='" + Negocio.P_Manzana_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Lote_Predio + "='" + Negocio.P_Lote_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Otros_Predio + "='" + Negocio.P_Otros_Predio + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Inspector_ID + "='" + Negocio.P_Inspector_ID + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Usuario_Modifico + "='" + Negocio.P_Usuario + "' ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Fecha_Modifico + "= SYSDATE ");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Costo_Base + "='" + Negocio.P_Costo_Base + "'");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Cantidad + "='" + Negocio.P_Cantidad + "'");
                     Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Costo_Total + "='" + Negocio.P_Costo_Total + "'");

                     Mi_SQL.Append(" Where " + Ope_Tra_Solicitud.Campo_Solicitud_ID + "='" + Negocio.P_Solicitud_ID + "'");
                     Cmd.CommandText = Mi_SQL.ToString();
                     Cmd.ExecuteNonQuery();

                    foreach (DataRow Registro in Negocio.P_Dt_Datos.Rows)
                    {
                        Mi_SQL = new StringBuilder();
                        Mi_SQL.Append("UPDATE " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos + " set ");

                        Mi_SQL.Append(Ope_Tra_Datos.Campo_Valor + "='" + Negocio.P_Datos[Contador_Datos, 1] + "' ");
                        Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Usuario_Modifico + "='" + Negocio.P_Usuario + "' ");
                        Mi_SQL.Append(", " + Ope_Tra_Datos.Campo_Fecha_Modifico + "= SYSDATE ");

                        Mi_SQL.Append(" Where " + Ope_Tra_Datos.Campo_Ope_Dato_ID + "='" + Registro[Ope_Tra_Datos.Campo_Ope_Dato_ID].ToString() + "'");
                        
                        Cmd.CommandText = Mi_SQL.ToString();
                        Cmd.ExecuteNonQuery();

                        Contador_Datos++;
                    }
                    Trans.Commit();
                    Operacion_Completa = true;
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
                finally
                {
                    if (Cn.State == ConnectionState.Open)
                    {
                        Cn.Close();
                    }
                }
                return Operacion_Completa;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Tramite
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar 
            ///los datos que se requieres para realizzar un tramite
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Datos_Tramite(Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio)
            {
                String Mi_SQL = "";
                DataSet Data_Set = new DataSet();
                try
                {
                    Mi_SQL = "SELECT * FROM " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite;
                    Mi_SQL += " WHERE " + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + " = '" + Solicitud_Negocio.P_Tramite_ID + "'";
                    if (!String.IsNullOrEmpty(Solicitud_Negocio.P_Tipo_Dato))
                    {
                        Mi_SQL += " AND " + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato + " = '" + Solicitud_Negocio.P_Tipo_Dato + "'";
                    }
                    Mi_SQL += " order by " + Cat_Tra_Datos_Tramite.Campo_Nombre;
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                } 
                return Data_Set;
            }
            
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Tramite
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar los dicumentos requeridos
            ///por un tramite
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Documentos_Tramite(Cls_Ope_Solicitud_Tramites_Negocio Solicitud_Negocio)
            {
                String Mi_SQL = "";
                DataSet Data_Set = new DataSet();
                try
                {
                    Mi_SQL = "SELECT " +
                        "  DETALLE." + Tra_Detalle_Documentos.Campo_Detalle_Documento_ID +
                        ", DETALLE." + Tra_Detalle_Documentos.Campo_Documento_Requerido +
                        ", DOCUMENTO." + Cat_Tra_Documentos.Campo_Nombre + " AS DOCUMENTO" +
                        ", DOCUMENTO." + Cat_Tra_Documentos.Campo_Documento_ID +
                        ", DOCUMENTO." + Cat_Tra_Documentos.Campo_Descripcion +
                        " FROM " + Tra_Detalle_Documentos.Tabla_Tra_Detalle_Documentos +
                        " DETALLE JOIN " + Cat_Tra_Documentos.Tabla_Cat_Tra_Documentos +
                        " DOCUMENTO ON DETALLE." + Tra_Detalle_Documentos.Campo_Documento_ID +
                        " = DOCUMENTO." + Cat_Tra_Documentos.Campo_Documento_ID +
                        " WHERE DETALLE." + Tra_Detalle_Documentos.Campo_Tramite_ID +
                        " = '" + Solicitud_Negocio.P_Tramite_ID + "'";

                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Data_Set;
            }
            
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tramites
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar los tramites existentes
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Tramites(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                String Mi_SQL = "";
                DataSet Data_Set = new DataSet();
                try
                {
                    Mi_SQL = "SELECT *FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites;
                    if (Solicitud.P_Tramite_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Cat_Tra_Tramites.Campo_Tramite_ID +
                            " = '" + Solicitud.P_Tramite_ID + "'";
                    }
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Data_Set;
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cosultar_Solicitud
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar los datos generales 
            ///de una solicitud
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Cosultar_Solicitud(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                Boolean Entro_Where = false;
                String Mi_SQL = "";
                DataSet Data_Set = new DataSet();
                try
                {
                    Mi_SQL = "SELECT * FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    if (Solicitud.P_Clave_Solicitud != null)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL += Ope_Tra_Solicitud.Campo_Clave_Solicitud + " = '" + Solicitud.P_Clave_Solicitud + "'";
                    }
                    if (Solicitud.P_Solicitud_ID != null)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL += Ope_Tra_Solicitud.Campo_Solicitud_ID + " = '" + Solicitud.P_Solicitud_ID + "'";
                    }
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Data_Set;
            }
            
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Solicitud
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar los 
            ///datos pertenecientes al tramite de la solicitud
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Datos_Solicitud(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                String Mi_SQL = "";
                DataSet Data_Set = new DataSet();
                try
                {
                    Mi_SQL = "SELECT *FROM " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos +
                       " WHERE " + Ope_Tra_Datos.Campo_Solicitud_ID +
                       " = '" + Solicitud.P_Solicitud_ID + "'";
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Data_Set;
            }

            //*******************************************************************************
            ///NOMBRE       : Modificar_Actividad_Solicitud_Hija
            ///DESCRIPCIÓN  : Modifica la activadad de las solicitudes hijas 
            ///PARAMETROS   : 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///                 los datos para realizar la consulta
            ///CREO         : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO   : 07/Noviembre/2012 
            ///MODIFICO     :
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static Boolean Modificar_Actividad_Solicitud_Hija(Cls_Ope_Solicitud_Tramites_Negocio Negocio)
            {
                StringBuilder Mi_SQL = new StringBuilder();
                Boolean Estatus = false;
                try
                {

                    Mi_SQL.Append("UPDATE " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SET ");
                    Mi_SQL.Append(Ope_Tra_Solicitud.Campo_Subproceso_ID + "='" + Negocio.P_Subproceso_ID + "' ");
                    Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Estatus + "='" + Negocio.P_Estatus + "' ");
                    Mi_SQL.Append(", " + Ope_Tra_Solicitud.Campo_Consecutivo + "='" + Negocio.P_Consecutivo + "' ");
                    Mi_SQL.Append(" WHERE " + Ope_Tra_Solicitud.Campo_Complemento + "='" + Negocio.P_Solicitud_ID + "' ");
                    
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

                    Estatus = true;
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }

                return Estatus;
            }
            
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Documentos_Solicitud
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar los documentos 
            ///de un tramite pértenecientes la solicitud
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Documentos_Solicitud(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                DataSet Data_Set = new DataSet();
                String Mi_SQL = "";
                try
                {
                    Mi_SQL = "SELECT *FROM " + Ope_Tra_Documentos.Tabla_Ope_Tra_Documentos +
                       " WHERE " + Ope_Tra_Documentos.Campo_Solicitud_ID +
                       " = '" + Solicitud.P_Solicitud_ID + "'";
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Data_Set;
            }
            
            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Subproceso
            ///DESCRIPCIÓN: Crea una sentencia sql para consultar el id del suprimer subproceso del tramite
            ///PARAMETROS: 1.-Solicitud_Negocio, objeto de la calse de negocio que contiene 
            ///            los datos para realizar la consulta
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 16/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public static DataSet Consultar_Subproceso(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                DataSet Data_Set = new DataSet();
                String Mi_SQL = "";
                try
                {
                    Mi_SQL = "SELECT " + Cat_Tra_Subprocesos.Campo_Subproceso_ID +
                       " FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos +
                       " WHERE " + Cat_Tra_Subprocesos.Campo_Tramite_ID + " = '" +
                       Solicitud.P_Tramite_ID + "' AND " + Cat_Tra_Subprocesos.Campo_Orden + " = 1";
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Data_Set;

            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Alta_Solicitud
            ///DESCRIPCIÓN: crea una sentencia sql para insertar un Solicitud en la base de datos
            ///PARAMETROS: 1.-Solicitud, objeto de la calse de negocio que contiene los datos para realizar la consulta
            ///            2.-Usuario_Creo, Nombre del usuario logueado actualmente en el sistema
            ///CREO: Silvia Morales Portuhondo
            ///FECHA_CREO: 12/Octubre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            public static int Alta_Solicitud_Empleado(Cls_Ope_Solicitud_Tramites_Negocio Solicitud, String Usuario_Creo)
            {
                int Row = -1;
                try
                {
                    //INSERTAR LA SOLICITUD
                    Solicitud.P_Solicitud_ID = Obtener_Id_Consecutivo(Ope_Tra_Solicitud.Campo_Solicitud_ID, Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud);
                    //Solicitud.P_Clave_Solicitud = Generar_Folio();//"TR-" + //Solicitud.P_Solicitud_ID;
                    String Mi_SQL = "INSERT INTO " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud +
                         " (" + Ope_Tra_Solicitud.Campo_Solicitud_ID + ", " +
                         Ope_Tra_Solicitud.Campo_Tramite_ID + ", " +
                         Ope_Tra_Solicitud.Campo_Estatus + ", " +
                         Ope_Tra_Solicitud.Campo_Clave_Solicitud + ", " +
                         Ope_Tra_Solicitud.Campo_Porcentaje_Avance + ", " +
                         Ope_Tra_Solicitud.Campo_Usuario_Creo + ", " +
                         Ope_Tra_Solicitud.Campo_Fecha_Creo + ", " +
                         Ope_Tra_Solicitud.Campo_Nombre_Solicitante + ", " +
                         Ope_Tra_Solicitud.Campo_Apellido_Paterno + ", " +
                         Ope_Tra_Solicitud.Campo_Apellido_Materno + ", " +
                         Ope_Tra_Solicitud.Campo_Subproceso_ID + ", " +
                         Ope_Tra_Solicitud.Campo_Correo_Electronico + ", " +
                         Ope_Tra_Solicitud.Campo_Folio + ") VALUES ('"
                         + Solicitud.P_Solicitud_ID + "', '" +
                         Solicitud.P_Tramite_ID + "', '" + Solicitud.P_Estatus + "', '" +
                         Solicitud.P_Clave_Solicitud + "', '" + Solicitud.P_Porcentaje + "', '" +
                         Usuario_Creo + "', SYSDATE, '" +
                         Solicitud.P_Nombre_Solicitante + "', '" + Solicitud.P_Apellido_Paterno +
                         "', '" + Solicitud.P_Apellido_Materno +
                         "', '" + Solicitud.P_Subproceso_ID + "', '" + Solicitud.P_E_Mail + "', '" + Solicitud.P_Folio + Solicitud.P_Solicitud_ID + "')";
                    Row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


                    //INSERTAR  LOS DATOS PROPORCIONADOS PAR LA SOLICITUD EN LA BASE DE DATOS
                    Mi_SQL = "";
                    int Columnas = Solicitud.P_Datos.Length / 2;
                    for (int i = 0; i < Columnas; i++)
                    {
                        Mi_SQL = "INSERT INTO " + Ope_Tra_Datos.Tabla_Ope_Tra_Datos +
                          " (" + Ope_Tra_Datos.Campo_Ope_Dato_ID + ", " +
                          Ope_Tra_Solicitud.Campo_Tramite_ID + ", " +
                          Ope_Tra_Datos.Campo_Solicitud_ID +
                          ", " + Ope_Tra_Datos.Campo_Dato_ID + ", " +
                          Ope_Tra_Datos.Campo_Valor + ", " +
                          Ope_Tra_Solicitud.Campo_Usuario_Creo + ", " +
                          Ope_Tra_Solicitud.Campo_Fecha_Creo + ") VALUES ('" +
                          Obtener_Id_Consecutivo(Ope_Tra_Datos.Campo_Ope_Dato_ID, Ope_Tra_Datos.Tabla_Ope_Tra_Datos) + "', '" +
                          Solicitud.P_Tramite_ID + "', '" + Solicitud.P_Solicitud_ID + "', '" +
                          Solicitud.P_Datos[i, 0] + "', '" + Solicitud.P_Datos[i, 1] + "', '" +
                          Usuario_Creo + "',SYSDATE)";
                        Row = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Row;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Subproceso
            ///DESCRIPCIÓN         : Consulta los Subprocesos
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 10/Agosto/2012
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public static DataTable Consultar_Tabla_Subproceso(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                String Mi_SQL = null;
                DataTable Tabla = null;
                DataSet Data_Set = null;
                Boolean Entro_Where = false;

                try
                {
                    Mi_SQL = "SELECT * FROM " + Cat_Tra_Subprocesos.Tabla_Cat_Tra_Subprocesos;
                    if (Solicitud.P_Subproceso_ID != null)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL += Cat_Tra_Subprocesos.Campo_Subproceso_ID + " = '" + Solicitud.P_Subproceso_ID + "'";
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
            ///NOMBRE DE LA FUNCIÓN: Consultar_Datos_Finales_Tramite
            ///DESCRIPCIÓN         : Consulta los Subprocesos
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO          : 10/Noviembre/2012 
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Finales_Tramite(Cls_Ope_Solicitud_Tramites_Negocio Negocio)
            {
                DataTable Dt_Consulta = new DataTable();
                StringBuilder Mi_Sql = new StringBuilder();

                try
                {
                    Mi_Sql.Append("Select * from " + Cat_Tra_Datos_Tramite.Tabla_Cat_Tra_Datos_Tramite);
                    Mi_Sql.Append(" Where " + Cat_Tra_Datos_Tramite.Campo_Tramite_ID + "='" + Negocio.P_Tramite_ID + "'");
                    Mi_Sql.Append(" And " + Cat_Tra_Datos_Tramite.Campo_Tipo_Dato + "='FINAL'");

                     return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
                return Dt_Consulta;
            }
            
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consultar_Tabla_Tramites
            ///DESCRIPCIÓN         : Consulta los Subprocesos
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 10/Agosto/2012
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public static DataTable Consultar_Tabla_Tramites(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                String Mi_SQL = null;
                DataTable Tabla = null;
                DataSet Data_Set = null;
                Boolean Entro_Where = false;

                try
                {
                    Mi_SQL = "SELECT * FROM " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud;
                    if (Solicitud.P_Tramite_ID != null)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL += Ope_Tra_Solicitud.Campo_Tramite_ID + " = '" + Solicitud.P_Tramite_ID + "'";
                    }
                    if (Solicitud.P_Estatus != null)
                    {
                        if (Entro_Where) { Mi_SQL += " AND "; } else { Mi_SQL += " WHERE "; Entro_Where = true; }
                        Mi_SQL += Ope_Tra_Solicitud.Campo_Estatus + " = '" + Solicitud.P_Estatus + "'";
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
            ///NOMBRE DE LA FUNCIÓN: Consultar_Cuenta_Predial
            ///DESCRIPCIÓN         : Consulta los Datos de la cuenta predial.
            ///PARAMETROS          : 1.- Solicitud. Filtros para realizar la busqueda
            ///CREO                : Salvador Vazquez Camacho
            ///FECHA_CREO          : 10/Agosto/2012
            ///MODIFICO            :
            ///FECHA_MODIFICO      :
            ///CAUSA_MODIFICACIÓN  :
            ///*******************************************************************************
            public static DataTable Consultar_Cuenta_Predial(Cls_Ope_Solicitud_Tramites_Negocio Solicitud)
            {
                String Mi_SQL = null;
                DataTable Tabla = null;
                DataSet Data_Set = null;

                try
                {
                    Mi_SQL = "SELECT CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Mi_SQL += ", CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID;
                    Mi_SQL += ", CAT_CALLES." + Cat_Pre_Calles.Campo_Nombre + " AS CALLE";
                    Mi_SQL += ", CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior;
                    Mi_SQL += ", CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID;
                    Mi_SQL += ", CAT_COLONIAS." + Cat_Ate_Colonias.Campo_Nombre + " AS COLONIA";
                    Mi_SQL += ", CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Manzana;
                    Mi_SQL += ", CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Lote;
                    Mi_SQL += ", CAT_PROPIETARIOS." + Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                    Mi_SQL += ", CAT_CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || ";
                    Mi_SQL += "  CAT_CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' ||";
                    Mi_SQL += "  CAT_CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " AS PROPIETARIO";
                    Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CAT_CUENTAS";
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " CAT_CALLES";
                    Mi_SQL += " ON CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = CAT_CALLES." + Cat_Pre_Calles.Campo_Calle_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " CAT_COLONIAS";
                    Mi_SQL += " ON CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = CAT_COLONIAS." + Cat_Ate_Colonias.Campo_Colonia_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " CAT_PROPIETARIOS";
                    Mi_SQL += " ON CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = CAT_PROPIETARIOS." + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID;
                    Mi_SQL += " LEFT OUTER JOIN " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " CAT_CONTRIBUYENTES";
                    Mi_SQL += " ON CAT_PROPIETARIOS." + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " = CAT_CONTRIBUYENTES." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID;
                    Mi_SQL += " WHERE CAT_CUENTAS." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Solicitud.P_Cuenta_Predial + "'";
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

        #endregion 

        #region Consultas

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Obra
            ///DESCRIPCIÓN          : Metodo que consultara la informacion de la obra
            ///PARAMETROS           :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 10/Julio/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Datos_Obra(Cls_Ope_Solicitud_Tramites_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT ");
                    Mi_Sql.Append(Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Nombre + " as Calle");
                    Mi_Sql.Append(", " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Nombre + " as Colonia");
                    Mi_Sql.Append(", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ".LOTE");
                    Mi_Sql.Append(", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + ".MANZANA");
                    Mi_Sql.Append(", " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + " as Numero");

                    Mi_Sql.Append(" From ");
                    Mi_Sql.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);

                    Mi_Sql.Append(" left outer join " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " ON ");
                    Mi_Sql.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Colonia_ID + " = ");
                    Mi_Sql.Append(Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + "." + Cat_Ate_Colonias.Campo_Colonia_ID);

                    Mi_Sql.Append(" left outer join " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles + " ON ");
                    Mi_Sql.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = ");
                    Mi_Sql.Append(Cat_Pre_Calles.Tabla_Cat_Pre_Calles + "." + Cat_Pre_Calles.Campo_Calle_ID);

                    Mi_Sql.Append(" Where ");
                    Mi_Sql.Append(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial);
                    Mi_Sql.Append(" ='" + Negocio.P_Cuenta_Predial + "'");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Inspectores
            ///DESCRIPCIÓN          : Metodo que consultara la informacion del inspector
            ///PARAMETROS           :
            ///CREO                 : Hugo Enrique Ramírez Aguilera
            ///FECHA_CREO           : 10/Julio/2012 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public static DataTable Consultar_Inspectores(Cls_Ope_Solicitud_Tramites_Negocio Negocio)
            {
                StringBuilder Mi_Sql = new StringBuilder();
                try
                {
                    Mi_Sql.Append("SELECT * ");
                    Mi_Sql.Append(" From ");
                    Mi_Sql.Append(Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores);
                    Mi_Sql.Append(" Where ");
                    Mi_Sql.Append(Cat_Ort_Inspectores.Tabla_Cat_Ort_Inspectores + "." + Cat_Ort_Inspectores.Campo_Inspector_ID);
                    Mi_Sql.Append(" ='" + Negocio.P_Perito_ID + "'");

                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
                }
            }

        #endregion
    }


}
