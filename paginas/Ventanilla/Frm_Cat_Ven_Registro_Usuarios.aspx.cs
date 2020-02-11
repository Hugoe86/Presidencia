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
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Datos;
using Presidencia.Constantes;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ven_Registro_Usuarios : System.Web.UI.Page
{
    #region PAGE LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Controlador_Inicio();
            }
        }
    #endregion

    #region METODOS

    #region (Metodos Generales)
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Controlador_Inicio
        ///DESCRIPCIÓN          : Metodo para el inicio de la pagina
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private void Controlador_Inicio()
        {
            Response.Clear();
            String Cadena = String.Empty;
            String Accion = String.Empty;

            try
            {
                if (this.Request.QueryString["Accion"] != null)
                {
                    if (!String.IsNullOrEmpty(this.Request.QueryString["Accion"].ToString().Trim()))
                    {
                        Accion = this.Request.QueryString["Accion"].ToString().Trim();
                        switch (Accion)
                        {
                            case "Registrarse":
                                Cadena = Registrar_Usuarios();
                                break;
                            case "Consultar_Password":
                                Cadena = Consultar_Password_Usuarios();
                                break;
                       
                        }
                    }
                }
                //Response.ContentType = "application/json";
                Response.Write(Cadena);
                Response.Flush();
                Response.Close();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al Controlador_Inicio Error[" + ex.Message + "]");
            }
        }
    #endregion

    #region (Metodos Generales)
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Registrar_Usuarios
        ///DESCRIPCIÓN          : Metodo para registrar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 02/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private String Registrar_Usuarios()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            String Cadena = String.Empty;
            DataTable Dt_Usuarios = new DataTable();
            String Password = String.Empty;
            String Email = String.Empty;
            String Nombre = String.Empty;

            try
            {
                if (this.Request.QueryString["Nombre"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Nombre"].ToString().Trim()))
                {
                    Negocio.P_Nombre = this.Request.QueryString["Nombre"].ToString().Trim().ToUpper();
                    Negocio.P_Nombre_Completo = this.Request.QueryString["Nombre"].ToString().Trim().ToUpper() + " ";
                }
                else 
                {
                     Negocio.P_Nombre = String.Empty;
                }

                if (this.Request.QueryString["Paterno"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Paterno"].ToString().Trim()))
                {
                    Negocio.P_Apellido_Paterno = this.Request.QueryString["Paterno"].ToString().Trim().ToUpper();
                    Negocio.P_Nombre_Completo += this.Request.QueryString["Paterno"].ToString().Trim().ToUpper() + " ";
                }
                else
                {
                    Negocio.P_Apellido_Paterno = String.Empty;
                }

                if (this.Request.QueryString["Materno"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Materno"].ToString().Trim()))
                {
                    Negocio.P_Apellido_Materno = this.Request.QueryString["Materno"].ToString().Trim().ToUpper();
                    Negocio.P_Nombre_Completo += this.Request.QueryString["Materno"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Apellido_Materno = String.Empty;
                }

                if (this.Request.QueryString["Curp"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Curp"].ToString().Trim()))
                {
                    Negocio.P_Curp = this.Request.QueryString["Curp"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Curp = String.Empty;
                }

                if (this.Request.QueryString["Rfc"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Rfc"].ToString().Trim()))
                {
                    Negocio.P_Rfc = this.Request.QueryString["Rfc"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Rfc = String.Empty;
                }

                if (this.Request.QueryString["F_Nacimiento"] != null && !String.IsNullOrEmpty(this.Request.QueryString["F_Nacimiento"].ToString().Trim()))
                {
                    if (this.Request.QueryString["F_Nacimiento"].ToString().Trim().ToUpper().Contains("EJ"))
                    {
                        Negocio.P_Fecha_Nacimiento = String.Empty;
                    }
                    else 
                    {
                        Negocio.P_Fecha_Nacimiento = this.Request.QueryString["F_Nacimiento"].ToString().Trim().ToUpper();
                    }
                }
                else
                {
                    Negocio.P_Fecha_Nacimiento = String.Empty;
                }

                if (this.Request.QueryString["Edad"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Edad"].ToString().Trim()))
                {
                    Negocio.P_Edad = this.Request.QueryString["Edad"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Edad = String.Empty;
                }

                if (this.Request.QueryString["Sexo"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Sexo"].ToString().Trim()))
                {
                    Negocio.P_Sexo = this.Request.QueryString["Sexo"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Sexo = String.Empty;
                }

                if (this.Request.QueryString["Email"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Email"].ToString().Trim()))
                {
                    Negocio.P_Email = this.Request.QueryString["Email"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Email = String.Empty;
                }

                if (this.Request.QueryString["Calle"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Calle"].ToString().Trim()))
                {
                    Negocio.P_Calle = this.Request.QueryString["Calle"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Calle = String.Empty;
                }

                if (this.Request.QueryString["Colonia"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Colonia"].ToString().Trim()))
                {
                    Negocio.P_Colonia = this.Request.QueryString["Colonia"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Colonia = String.Empty;
                }

                if (this.Request.QueryString["Cp"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Cp"].ToString().Trim()))
                {
                    Negocio.P_Codigo_Postal = this.Request.QueryString["Cp"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Codigo_Postal = String.Empty;
                }

                if (this.Request.QueryString["Ciudad"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Ciudad"].ToString().Trim()))
                {
                    Negocio.P_Ciudad = this.Request.QueryString["Ciudad"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Ciudad = String.Empty;
                }

                if (this.Request.QueryString["Estado"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Estado"].ToString().Trim()))
                {
                    Negocio.P_Estado = this.Request.QueryString["Estado"].ToString().Trim().ToUpper();
                }
                else
                {
                    Negocio.P_Estado = String.Empty;
                }

                if (this.Request.QueryString["T_Casa"] != null && !String.IsNullOrEmpty(this.Request.QueryString["T_Casa"].ToString().Trim()))
                {
                    Negocio.P_Telefono_Casa = this.Request.QueryString["T_Casa"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Telefono_Casa = String.Empty;
                }

                if (this.Request.QueryString["T_Celular"] != null && !String.IsNullOrEmpty(this.Request.QueryString["T_Celular"].ToString().Trim()))
                {
                    Negocio.P_Celular = this.Request.QueryString["T_Celular"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Celular = String.Empty;
                }
                //  si obtiene la informacion de la respuesta secreta
                if (this.Request.QueryString["Pregunta_Secreta"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Pregunta_Secreta"].ToString().Trim()))
                {
                    Negocio.P_Pregunta_Secreta = this.Request.QueryString["Pregunta_Secreta"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Pregunta_Secreta = String.Empty;
                }

                //  si contiene informacion se llena la respuesta secreta
                if (Negocio.P_Pregunta_Secreta != "")
                {
                    if (this.Request.QueryString["Respuesta_Secreta"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Respuesta_Secreta"].ToString().Trim()))
                    {
                        Negocio.P_Respuesta_Secreta = this.Request.QueryString["Respuesta_Secreta"].ToString().Trim();
                    }
                    else
                    {
                        Negocio.P_Respuesta_Secreta = String.Empty;
                    }
                }
                else
                {
                    Negocio.P_Respuesta_Secreta = String.Empty;
                }

                //  para la contraseña esto si el usario la ingresa

                if (this.Request.QueryString["Password"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Password"].ToString().Trim()))
                {
                    Negocio.P_Password = this.Request.QueryString["Password"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Password = String.Empty;
                }

                Negocio.P_Estatus = "ACTIVO";
                Negocio.P_Comentarios = String.Empty;

                Dt_Usuarios = Negocio.Consultar_Usuarios();

                if (Dt_Usuarios != null && Dt_Usuarios.Rows.Count > 0)
                {
                    Cadena = "Iguales";
                    Nombre = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Nombre_Completo].ToString().Trim();
                    Email = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString().Trim();
                    Password = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Password].ToString().Trim();
                    Negocio.Enviar_Correo(Email, Password, Nombre);
                }
                else 
                {
                    if (Negocio.Guardar_Usuario())
                    {
                        Cadena = "Registrado";
                    }
                    else
                    {
                        Cadena = "No";
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar los datos del usuario Error[" + ex.Message + "]");
            }
            return Cadena;
        }

        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Password_Usuarios
        ///DESCRIPCIÓN          : Metodo para registrar los datos del usuario
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 03/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        private String Consultar_Password_Usuarios()
        {
            Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
            String Cadena = String.Empty;
            DataTable Dt_Usuarios = new DataTable();
            String Password = String.Empty;
            String Email = String.Empty;
            String Nombre = String.Empty;

            try
            {
                //  Email
                if (this.Request.QueryString["Email"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Email"].ToString().Trim()))
                {
                    Negocio.P_Email = this.Request.QueryString["Email"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Email = String.Empty;
                }

                //  Pregunta_Secreta
                if (this.Request.QueryString["Pregunta_Secreta"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Pregunta_Secreta"].ToString().Trim()))
                {
                    Negocio.P_Pregunta_Secreta = this.Request.QueryString["Pregunta_Secreta"].ToString().Trim();

                    if (Negocio.P_Pregunta_Secreta.Contains("Selecciona"))
                    {
                        Negocio.P_Pregunta_Secreta = String.Empty;
                    }
                }
                else
                {
                    Negocio.P_Pregunta_Secreta = String.Empty;
                }

                //  Respuesta_Secreta
                if (this.Request.QueryString["Respuesta_Secreta"] != null && !String.IsNullOrEmpty(this.Request.QueryString["Respuesta_Secreta"].ToString().Trim()))
                {
                    Negocio.P_Respuesta_Secreta = this.Request.QueryString["Respuesta_Secreta"].ToString().Trim();
                }
                else
                {
                    Negocio.P_Respuesta_Secreta = String.Empty;
                }

                Dt_Usuarios = Negocio.Consultar_Usuarios();

                if (Dt_Usuarios != null && Dt_Usuarios.Rows.Count > 0)
                {
                    Nombre = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Nombre_Completo].ToString().Trim();
                    Email = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Email].ToString().Trim();
                    Password = Dt_Usuarios.Rows[0][Cat_Ven_Usuarios.Campo_Password].ToString().Trim();
                    Negocio.Enviar_Correo(Email, Password, Nombre);
                    Cadena = "Listo";
                }
                else
                {
                    Cadena = "No";
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al registrar los datos del usuario Error[" + ex.Message + "]");
            }
            return Cadena;
        }
    #endregion

    #endregion
}
