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
using System.Text;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using System.Net.Mail;

namespace Presidencia.Cls_Cat_Ven_Registro_Usuarios.Datos
{
    public class Cls_Cat_Ven_Registro_Usuarios_Datos
    {
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Usuario
        ///DESCRIPCIÓN          : consulta para guardar los datos de las clases
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Alta_Usuario(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.
            String Id = Consecutivo_ID(Cat_Pre_Contribuyentes.Campo_Contribuyente_ID, Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes , "10");

            try
            {
                if (String.IsNullOrEmpty(Negocio.P_Password))
                {
                    Negocio.P_Password = Generar_Password(Negocio.P_Nombre);
                }

                Mi_SQL.Append("INSERT INTO " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + "(");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Nombre + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Nombre_Completo + ", ");

                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Email + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Password + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Estatus + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Tipo_Pesona + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Calle_Ubicacion + ", ");

                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Colonia_Ubicacion + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Calle_ID + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Colonia_ID + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Codigo_Postal + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Ciudad_Ubicacion + ", ");

                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Estado_Ubicacion + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Telefono_Casa + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Celular + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Fecha_Nacimiento + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Edad + ", ");

                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Sexo + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_RFC + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_CURP + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Fecha_Registro + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Comentarios + ", ");

                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Pregunta_Secreta + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Respuesta_Secreta + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Usuario_Creo + ", ");
                Mi_SQL.Append(Cat_Pre_Contribuyentes.Campo_Fecha_Creo);

                Mi_SQL.Append(") VALUES( ");

                Mi_SQL.Append("'" + Id + "', ");
                Mi_SQL.Append("'" + Negocio.P_Nombre + "', ");
                Mi_SQL.Append("'" + Negocio.P_Apellido_Paterno + "', ");
                Mi_SQL.Append("'" + Negocio.P_Apellido_Materno + "', ");
                Mi_SQL.Append("'" + Negocio.P_Nombre_Completo + "', ");

                Mi_SQL.Append("'" + Negocio.P_Email + "', ");
                Mi_SQL.Append("'" + Negocio.P_Password + "', ");
                Mi_SQL.Append("'VIGENTE', ");//   estatus
                Mi_SQL.Append("'FISICA', ");//  TIPO DE PERSONA
                Mi_SQL.Append("'" + Negocio.P_Calle + "', ");

                Mi_SQL.Append("'" + Negocio.P_Colonia + "', ");
                Mi_SQL.Append("'" + Negocio.P_Calle_ID + "', ");
                Mi_SQL.Append("'" + Negocio.P_Colonia_ID + "', ");
                Mi_SQL.Append("'" + Negocio.P_Codigo_Postal + "', ");
                Mi_SQL.Append("'" + Negocio.P_Ciudad + "', ");

                Mi_SQL.Append("'" + Negocio.P_Estado + "', ");
                Mi_SQL.Append("'" + Negocio.P_Telefono_Casa + "', ");
                Mi_SQL.Append("'" + Negocio.P_Celular + "', ");
                Mi_SQL.Append("'" + Negocio.P_Fecha_Nacimiento + "', ");
                Mi_SQL.Append("'" + Negocio.P_Edad + "', ");

                Mi_SQL.Append("'" + Negocio.P_Sexo + "', ");
                Mi_SQL.Append("'" + Negocio.P_Rfc + "', ");
                Mi_SQL.Append("'" + Negocio.P_Curp + "', ");
                Mi_SQL.Append("SYSDATE, ");
                Mi_SQL.Append("'" + Negocio.P_Comentarios + "', ");

                Mi_SQL.Append("'" + Negocio.P_Pregunta_Secreta + "', ");
                Mi_SQL.Append("'" + Negocio.P_Respuesta_Secreta + "', ");
                Mi_SQL.Append("'" + Negocio.P_Nombre_Completo + "', ");
                Mi_SQL.Append("SYSDATE) ");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                if (!String.IsNullOrEmpty(Negocio.P_Email))
                {
                    Enviar_Correo(Negocio.P_Email, Negocio.P_Password, Negocio.P_Nombre_Completo);
                }
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta de usuarios. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consecutivo_ID
        ///DESCRIPCIÓN          : consulta para obtener el consecutivo de una tabla
        ///PARAMETROS           1 Campo_Id: campo del que se obtendra el consecutivo
        ///                     2 Tabla: tabla del que se obtendra el consecutivo
        ///                     3 Tamaño: longitud del campo 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 15/Marzo/2012
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
        ///NOMBRE DE LA FUNCIÓN : Consultar_Usuario
        ///DESCRIPCIÓN          : consulta para obtener los datos de los usuarios
        ///PARAMETROS           1 Negocio:. conexion con la capa de negocios 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Usuario(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre_Completo + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Email + ", ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Password);
                Mi_Sql.Append(" FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);

                if (!String.IsNullOrEmpty(Negocio.P_Email))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_Email + " = '" + Negocio.P_Email.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Email + " = '" + Negocio.P_Email.Trim() + "'");
                    }
                }
                if (!String.IsNullOrEmpty(Negocio.P_Rfc))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_RFC.Trim() + " = '" + Negocio.P_Rfc.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_RFC.Trim() + " = '" + Negocio.P_Rfc.Trim() + "'");
                    }
                }
                if (!String.IsNullOrEmpty(Negocio.P_Curp))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_CURP.Trim() + " = '" + Negocio.P_Curp.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_CURP.Trim() + " = '" + Negocio.P_Curp.Trim() + "'");
                    }
                }
                //  filtro pregunta secreta
                if (!String.IsNullOrEmpty(Negocio.P_Pregunta_Secreta))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_Pregunta_Secreta + " = '" + Negocio.P_Pregunta_Secreta.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Pregunta_Secreta + " = '" + Negocio.P_Pregunta_Secreta.Trim() + "'");
                    }
                }
                if (!String.IsNullOrEmpty(Negocio.P_Pregunta_Secreta))
                {
                    if (Mi_Sql.ToString().Trim().Contains("WHERE"))
                    {
                        Mi_Sql.Append(" AND " + Cat_Pre_Contribuyentes.Campo_Respuesta_Secreta + " = '" + Negocio.P_Respuesta_Secreta.Trim() + "'");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Respuesta_Secreta + " = '" + Negocio.P_Pregunta_Secreta.Trim() + "'");
                    }
                }

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Usuario_Soliucitante
        ///DESCRIPCIÓN          : consulta la informacion del usuario que desea realizar un tramite
        ///PARAMETROS           1 Negocio:. conexion con la capa de negocios 
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 18/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Usuario_Soliucitante(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            String Comparacion = "";
            try
            {
                Mi_Sql.Append("SELECT ");
                Mi_Sql.Append(Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " ||' '|| " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "||' '||" + Cat_Pre_Contribuyentes.Campo_Nombre + " as NOMBRE_COMPLETO");
                Mi_Sql.Append(", " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + ".*");
                Mi_Sql.Append(" FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes);

                if (!String.IsNullOrEmpty(Negocio.P_Nombre))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and upper(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") like upper('%" + Negocio.P_Nombre + "'%) ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE upper(" + Cat_Pre_Contribuyentes.Campo_Nombre + ") like upper('%" + Negocio.P_Nombre + "%') ");
                    }
                }

                if (!String.IsNullOrEmpty(Negocio.P_Apellido_Paterno))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and upper(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") like upper('%" + Negocio.P_Apellido_Paterno + "%') ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE upper(" + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + ") like upper('%" + Negocio.P_Apellido_Paterno + "%') ");
                    }
                }

                if (!String.IsNullOrEmpty(Negocio.P_Apellido_Materno))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and upper(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") like upper('%" + Negocio.P_Apellido_Materno + "%') ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE upper(" + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + ") like upper('%" + Negocio.P_Apellido_Materno + "%') ");
                    }
                }


                if (!String.IsNullOrEmpty(Negocio.P_Rfc))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and upper(" + Cat_Pre_Contribuyentes.Campo_RFC + ") like upper('%" + Negocio.P_Rfc + "%') ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE upper(" + Cat_Pre_Contribuyentes.Campo_RFC + ") like upper('%" + Negocio.P_Rfc + "%') ");
                    }
                }


                if (!String.IsNullOrEmpty(Negocio.P_Curp))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and upper(" + Cat_Pre_Contribuyentes.Campo_CURP + ") like upper('%" + Negocio.P_Curp + "%') ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE upper(" + Cat_Pre_Contribuyentes.Campo_CURP + ") like upper('%" + Negocio.P_Curp + "%') ");
                    }
                }


                if (!String.IsNullOrEmpty(Negocio.P_Email))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and upper(" + Cat_Pre_Contribuyentes.Campo_Email + ") like upper('%" + Negocio.P_Email + "%') ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE upper(" + Cat_Pre_Contribuyentes.Campo_Email + ") like upper('%" + Negocio.P_Email + "%') ");
                    }
                }


                if (!String.IsNullOrEmpty(Negocio.P_Telefono_Casa))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and (" + Cat_Pre_Contribuyentes.Campo_Telefono_Casa + "= '" + Negocio.P_Telefono_Casa + "' ");
                        Mi_Sql.Append(" or " + Cat_Pre_Contribuyentes.Campo_Celular + "= '" + Negocio.P_Celular + "') ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE (" + Cat_Pre_Contribuyentes.Campo_Telefono_Casa + "= '" + Negocio.P_Telefono_Casa + "' ");
                        Mi_Sql.Append(" or " + Cat_Pre_Contribuyentes.Campo_Celular + "= '" + Negocio.P_Celular + "') ");
                    }
                }

                if (!String.IsNullOrEmpty(Negocio.P_Ciudadano_ID))
                {
                    Comparacion = Mi_Sql.ToString();
                    if (Comparacion.Contains("WHERE"))
                    {
                        Mi_Sql.Append(" and " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "= '" + Negocio.P_Ciudadano_ID + "' ");
                    }
                    else
                    {
                        Mi_Sql.Append(" WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + "= '" + Negocio.P_Ciudadano_ID + "' ");
                    }
                }


                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Enviar_Correo
        ///DESCRIPCIÓN          : Envia un correo a un usuario
        ///PROPIEDADES          : 
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static void Enviar_Correo(String Email, String Password, String Nombre)
        {
            try
            {
                MailMessage Correo = new MailMessage();
                String Para = Email;
                //String De = "leslie.gonzalez@contel.com.mx";
                //String Puerto = "25";
                //String Servidor = "correo.contel.com.mx";
                String De = Consultar_Correo(Cat_Ven_Param_Correo.Campo_Correo_Notificador);
                String Puerto = Consultar_Correo(Cat_Ven_Param_Correo.Campo_Correo_Puerto);
                String Servidor = Consultar_Correo(Cat_Ven_Param_Correo.Campo_Correo_Servidor);
                String Contraseña = Consultar_Correo(Cat_Ven_Param_Correo.Campo_Password_Correo_Notificador);

                Correo.To.Add(Para);

                Correo.From = new MailAddress(De, "Atención Ciudadana SIAG");
                Correo.Subject = " Registro Exitoso Atención Ciudadana";

                Correo.SubjectEncoding = System.Text.Encoding.UTF8;

                if ((!Correo.From.Equals("") || Correo.From != null) && (!Correo.To.Equals("") || Correo.To != null))
                {

                    Correo.Body =
                    "<html>" +
                         "<body> EN ATENCIÓN CIUDADANA, TE DAMOS LA BIENVENIDA : <b>" + Nombre + "</b><br>" +
                                "TUS DATOS PARA EL REGISTRO SON: <br><br>" +
                                "USUARIO: <b>" + Email + "</b><br>" +
                                "PASSWORD: <b>" + Password + "</b><br><br>" +
                                "<b>GRACIAS POR REGISTRARTE CON NOSOTROS. </b> <br>" +
                           "</body>" +
                     "</html>";

                    Correo.BodyEncoding = System.Text.Encoding.UTF8;
                    Correo.IsBodyHtml = true;

                    SmtpClient cliente_correo = new SmtpClient();
                    cliente_correo.Port = int.Parse(Puerto);
                    cliente_correo.UseDefaultCredentials = true;
                    cliente_correo.Credentials = new System.Net.NetworkCredential(De, Contraseña);
                    //cliente_correo.Credentials = new System.Net.NetworkCredential("leslie.gonzalez@contel.com.mx", "lgonzalez");
                    cliente_correo.Host = Servidor;
                    cliente_correo.Send(Correo);
                    Correo = null;
                }
                else
                {
                    throw new Exception("No se tiene configurada una cuenta de correo, favor de notificar");
                }
            }
            catch (SmtpException ex)
            {
                Console.WriteLine("{0}", ex.Message.ToString());
                throw new Exception("Envio de Correo " + ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Correo
        ///DESCRIPCIÓN          : consulta para obtener los datos del correo
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static String Consultar_Correo(String Campo)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Campo);
                Mi_Sql.Append(" FROM " + Cat_Ven_Param_Correo.Tabla_Cat_Ven_Param_Correo);

                return (String)Convert.ToString(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()));
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Generar_Password
        ///DESCRIPCIÓN          : metodo para generar el password el usuario
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static String Generar_Password(String Nombre)
        {
            String Password;
            try
            {
                if (Nombre.Trim().Contains(" "))
                {
                    Nombre.Substring(0, Nombre.IndexOf(" ")).Trim();
                }

                Password = Nombre;
                Password += String.Format("{0:ddMMyy}", DateTime.Now);
                Password += "%";


                if (Password.Length > 20)
                {
                    Password = Password.Substring(0, 20);
                    Password = Password.Trim();
                }
                return Password;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el password del usuario. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Alta_Parametros
        ///DESCRIPCIÓN          : consulta para guardar los datos de los parametros del correo
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 03/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Alta_Parametros(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            try
            {
                Mi_SQL.Append("INSERT INTO " + Cat_Ven_Param_Correo.Tabla_Cat_Ven_Param_Correo + "(");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Correo_Servidor + ", ");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Correo_Puerto + ", ");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Correo_Notificador + ", ");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Password_Correo_Notificador + ") VALUES( ");
                Mi_SQL.Append("'" + Negocio.P_Correo_Servidor + "', ");
                Mi_SQL.Append("'" + Negocio.P_Correo_Puerto + "', ");
                Mi_SQL.Append("'" + Negocio.P_Correo_Notificador + "', ");
                Mi_SQL.Append("'" + Negocio.P_Password_Correo_Not + "') ");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el alta de los parametros. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Actualizar_Parametros
        ///DESCRIPCIÓN          : consulta para actualizar los datos de los parametros del correo
        ///PARAMETROS           1 Negocio: conexion con la capa de negocios
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 03/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static Boolean Actualizar_Parametros(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            try
            {
                Mi_SQL.Append("UPDATE " + Cat_Ven_Param_Correo.Tabla_Cat_Ven_Param_Correo);
                Mi_SQL.Append(" SET " + Cat_Ven_Param_Correo.Campo_Correo_Servidor + " = '" + Negocio.P_Correo_Servidor + "', ");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Correo_Puerto + " = '" + Negocio.P_Correo_Puerto + "', ");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Correo_Notificador + " = '" + Negocio.P_Correo_Notificador + "', ");
                Mi_SQL.Append(Cat_Ven_Param_Correo.Campo_Password_Correo_Notificador + " = '" + Negocio.P_Password_Correo_Not + "' ");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar el actualizar de los parametros. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Parametros
        ///DESCRIPCIÓN          : consulta para obtener los datos de los parametros del correo
        ///PARAMETROS           1 Negocio:. conexion con la capa de negocios 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 03/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Parametros()
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT * ");
                Mi_Sql.Append(" FROM " + Cat_Ven_Param_Correo.Tabla_Cat_Ven_Param_Correo);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Colonia
        ///DESCRIPCIÓN          : consulta para obtener los datos de las colonias
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 23/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Colonia(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias);
                Mi_Sql.Append(" ORDER BY " + Cat_Ate_Colonias.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Calles
        ///DESCRIPCIÓN          : consulta para obtener los datos de las calles
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 23/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Calles(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT * " + " FROM " + Cat_Pre_Calles.Tabla_Cat_Pre_Calles);
                Mi_Sql.Append(" where " + Cat_Pre_Calles.Campo_Colonia_ID + "='" + Negocio.P_Colonia_ID + "'");
                Mi_Sql.Append(" ORDER BY " + Cat_Pre_Calles.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Estados
        ///DESCRIPCIÓN          : Metodo para consultar los datos de los estados
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Estados(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT *  FROM " + Cat_Pre_Estados.Tabla_Cat_Pre_Estados);
                Mi_Sql.Append(" ORDER BY " + Cat_Pre_Estados.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Ciudades
        ///DESCRIPCIÓN          : Metodo para consultar los datos de las ciudades
        ///PARAMETROS           :
        ///CREO                 : Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO           : 31/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Ciudades(Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT *  FROM " + Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades);
                Mi_Sql.Append(" WHERE " + Cat_Pre_Ciudades.Campo_Estado_ID + "='" + Negocio.P_Estado_ID + "' ");
                Mi_Sql.Append(" ORDER BY " + Cat_Pre_Ciudades.Campo_Nombre);

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
