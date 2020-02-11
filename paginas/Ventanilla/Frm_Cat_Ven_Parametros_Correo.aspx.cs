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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;

public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ven_Parametros_Correo : System.Web.UI.Page
{
    #region (Page_Load)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Metodo de inicio de la pagina
        ///PARAMETROS           :
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 03/Mayo/2012
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            try
            {
                if (!IsPostBack)
                {
                    Mostar_Limpiar_Error(String.Empty, String.Empty, false);
                    Parametros_Inicio();
                    ViewState["SortDirection"] = "DESC";
                }
            }
            catch (Exception Ex)
            {
                Mostar_Limpiar_Error(String.Empty, "Error al cargar la pagina de clases. Error[" + Ex.Message + "]", true);
            }
        }
    #endregion

    #region (Metodos)
        #region (Generales)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Parametros_Inicio
            ///DESCRIPCIÓN          : Metodo para la configuracion inicial de la pagina
            ///PARAMETROS           :
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Parametros_Inicio()
            {
                try
                {
                    Mostar_Limpiar_Error(String.Empty, String.Empty, false);
                    Limpiar_Forma();
                    Habilitar_Forma(false);
                    Consultar_Parametros();
                    Estado_Botones("Inicial");
                }
                catch (Exception Ex)
                {
                    Mostar_Limpiar_Error(String.Empty, "Error al cargar la pagina de clases. Error[" + Ex.Message + "]", true);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Habilitar_Forma
            ///DESCRIPCIÓN          : Metodo para habilitar o deshabilitar los controles
            ///PARAMETROS           1: Estatus: true o false para habilitar los controles
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Habilitar_Forma(Boolean Estatus)
            {
                Txt_Servidor.Enabled = Estatus;
                Txt_Correo.Enabled = Estatus;
                Txt_Password.Enabled = Estatus;
                Txt_Puerto.Enabled = Estatus;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Forma
            ///DESCRIPCIÓN          : Metodo para limpiar los controles
            ///PARAMETROS           :
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Limpiar_Forma()
            {
                Txt_Correo.Text = String.Empty;
                Txt_Servidor.Text = String.Empty;
                Txt_Password.Text = String.Empty;
                Txt_Puerto.Text = String.Empty;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Forma
            ///DESCRIPCIÓN          : Metodo para limpiar los controles
            ///PARAMETROS           :
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Mostar_Limpiar_Error(String Encabezado_Error, String Mensaje_Error, Boolean Mostrar)
            {
                Lbl_Encabezado_Error.Text = Encabezado_Error;
                Lbl_Mensaje_Error.Text = Mensaje_Error;
                Lbl_Mensaje_Error.Visible = Mostrar;
                Td_Error.Visible = Mostrar;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Estado_Botones
            ///DESCRIPCIÓN          : metodo que muestra los botones de acuerdo al estado en el que se encuentre
            ///PARAMETROS           1: String Estado: El estado de los botones solo puede tomar 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public void Estado_Botones(String Estado)
            {
                switch (Estado)
                {
                    case "Inicial":

                        if (String.IsNullOrEmpty(Txt_Puerto.Text.Trim()))
                        {
                            //Boton Nuevo
                            Btn_Nuevo.ToolTip = "Nuevo";
                            Btn_Nuevo.Enabled = true;
                            Btn_Nuevo.Visible = true;
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            //Boton Modificar
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Modificar.Enabled = false;
                            Btn_Modificar.Visible = false;
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        }
                        else 
                        {
                            //Boton Nuevo
                            Btn_Nuevo.ToolTip = "Nuevo";
                            Btn_Nuevo.Enabled = false;
                            Btn_Nuevo.Visible = false;
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            //Boton Modificar
                            Btn_Modificar.ToolTip = "Modificar";
                            Btn_Modificar.Enabled = true;
                            Btn_Modificar.Visible = true;
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        }

                        //Boton Salir
                        Btn_Salir.ToolTip = "Inicio";
                        Btn_Salir.Enabled = true;
                        Btn_Salir.Visible = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        break;
                    case "Nuevo":
                        //Boton Nuevo
                        Btn_Nuevo.ToolTip = "Guardar";
                        Btn_Nuevo.Enabled = true;
                        Btn_Nuevo.Visible = true;
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        //Boton Modificar
                        Btn_Modificar.Visible = false;
                        //Boton Salir
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Salir.Enabled = true;
                        Btn_Salir.Visible = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        break;
                    case "Modificar":
                        //Boton Nuevo
                        Btn_Nuevo.Visible = false;
                        //Boton Modificar
                        Btn_Modificar.ToolTip = "Actualizar";
                        Btn_Modificar.Enabled = true;
                        Btn_Modificar.Visible = true;
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        //Boton Salir
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Salir.Enabled = true;
                        Btn_Salir.Visible = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        break;
                }//fin del switch
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Validar_Datos
            ///DESCRIPCIÓN          : metodo para validar los datos del formulario
            ///PARAMETROS           :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public Boolean Validar_Datos()
            {
                String Mensaje_Encabezado = "Es necesario: ";
                String Mensaje_Error = String.Empty;
                Boolean Datos_Validos = true;

                try
                {
                    Mostar_Limpiar_Error(String.Empty, String.Empty, false);

                    if (String.IsNullOrEmpty(Txt_Puerto.Text.Trim()))
                    {
                        Mensaje_Error += "&nbsp;&nbsp;* Instroducir un puerto. <br />";
                        Datos_Validos = false;
                    }

                    if (String.IsNullOrEmpty(Txt_Servidor.Text.Trim()))
                    {
                        Mensaje_Error += "&nbsp;&nbsp;* Instroducir un servidor. <br />";
                        Datos_Validos = false;
                    }

                    if (String.IsNullOrEmpty(Txt_Password.Text.Trim()))
                    {
                        Mensaje_Error += "&nbsp;&nbsp;* Instroducir un password del correo. <br />";
                        Datos_Validos = false;
                    }

                    if (String.IsNullOrEmpty(Txt_Correo.Text.Trim()))
                    {
                        Mensaje_Error += "&nbsp;&nbsp;* Instroducir un correo. <br />";
                        Datos_Validos = false;
                    }

                    if (!Datos_Validos)
                    {
                        Mostar_Limpiar_Error(Mensaje_Encabezado, Mensaje_Error, true);
                    }
                }
                catch (Exception Ex)
                {
                    Mostar_Limpiar_Error(String.Empty, "Error al validar los datos. Error[" + Ex.Message + "]", true);
                }
                return Datos_Validos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Parametros
            ///DESCRIPCIÓN          : Metodo para obtener los parametros
            ///PARAMETROS           :
            ///CREO                 : Leslie Gonzalez Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Consultar_Parametros()
            {
                Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
                DataTable Dt_Parametros = new DataTable();

                try
                {
                    Dt_Parametros = Negocio.Consultar_Parametros();
                    if (Dt_Parametros != null && Dt_Parametros.Rows.Count > 0)
                    {
                        Txt_Correo.Text = Dt_Parametros.Rows[0][Cat_Ven_Param_Correo.Campo_Correo_Notificador].ToString().Trim();
                        Txt_Servidor.Text = Dt_Parametros.Rows[0][Cat_Ven_Param_Correo.Campo_Correo_Servidor].ToString().Trim();
                        Txt_Password.Text = Dt_Parametros.Rows[0][Cat_Ven_Param_Correo.Campo_Password_Correo_Notificador].ToString().Trim();
                        Txt_Puerto.Text = Dt_Parametros.Rows[0][Cat_Ven_Param_Correo.Campo_Correo_Puerto].ToString().Trim();
                    }
                }
                catch (Exception ex)
                {
                    Mostar_Limpiar_Error("Error al consultar los parametros, Error:[" + ex.Message + "]", "", true);
                }
            }
        #endregion
    #endregion

    #region (Eventos)
        #region (Botones)
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
            ///DESCRIPCIÓN          : Evento del boton Salir 
            ///PARAMETROS           :    
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            protected void Btn_Salir_Click(object sender, EventArgs e)
            {
                Mostar_Limpiar_Error(String.Empty, String.Empty, false);
                switch (Btn_Salir.ToolTip)
                {
                    case "Cancelar":
                        Parametros_Inicio();
                        break;

                    case "Inicio":
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        break;
                }//fin del switch
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
            ///DESCRIPCIÓN          : Evento del boton Guardar
            ///PARAMETROS           :    
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 03/Mayo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            protected void Btn_Nuevo_Click(object sender, EventArgs e)
            {
                Mostar_Limpiar_Error(String.Empty, String.Empty, false);
                Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();//Variable de conexion con la capa de negocios.
                try
                {
                    switch (Btn_Nuevo.ToolTip)
                    {
                        case "Nuevo":
                            Estado_Botones("Nuevo");
                            Limpiar_Forma();
                            Habilitar_Forma(true);
                            break;
                        case "Guardar":
                            if (Validar_Datos())
                            {
                                Negocio.P_Correo_Notificador = Txt_Correo.Text.Trim();
                                Negocio.P_Correo_Puerto = Txt_Puerto.Text.Trim();
                                Negocio.P_Correo_Servidor = Txt_Servidor.Text.Trim();
                                Negocio.P_Password_Correo_Not = Txt_Password.Text.Trim();

                                if (Negocio.Guardar_Parametros())
                                {
                                    Parametros_Inicio();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Alta", "alert('Operacion Completa');", true);
                                }
                            }
                            break;
                    }//fin del swirch
                }
                catch (Exception Ex)
                {
                    Mostar_Limpiar_Error(String.Empty, "Error al guardar los parametros. Error[" + Ex.Message + "]", true);
                }
            }//fin del boton Nuevo

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
            ///DESCRIPCIÓN          : Evento del boton Modificar
            ///PARAMETROS           :    
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 20/Marzo/2012
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            protected void Btn_Modificar_Click(object sender, EventArgs e)
            {
                Mostar_Limpiar_Error(String.Empty, String.Empty, false);
                Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio = new Cls_Cat_Ven_Registro_Usuarios_Negocio();//Variable de conexion con la capa de negocios.
                try
                {
                    switch (Btn_Modificar.ToolTip)
                    {
                        //Validacion para actualizar un registro y para habilitar los controles que se requieran
                        case "Modificar":
                            Estado_Botones("Modificar");
                            Habilitar_Forma(true);
                            break;
                        case "Actualizar":
                            if (Validar_Datos())
                            {
                                Negocio.P_Correo_Notificador = Txt_Correo.Text.Trim();
                                Negocio.P_Correo_Puerto = Txt_Puerto.Text.Trim();
                                Negocio.P_Correo_Servidor = Txt_Servidor.Text.Trim();
                                Negocio.P_Password_Correo_Not = Txt_Password.Text.Trim();

                                if (Negocio.Actualizar_Parametros())
                                {
                                    Parametros_Inicio();
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Actualizar", "alert('Operacion Completa');", true);
                                }
                            }
                            break;
                    }//fin del switch
                }
                catch (Exception Ex)
                {
                    Mostar_Limpiar_Error(String.Empty, "Error al modificar los parametros. Error[" + Ex.Message + "]", true);
                }
            }//fin de Modificar
        #endregion
    #endregion
}
