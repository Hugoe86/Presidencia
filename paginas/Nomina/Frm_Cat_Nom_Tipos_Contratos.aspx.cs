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
using Presidencia.Tipos_Contratos.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Nom_Tipos_Contratos : System.Web.UI.Page
{
    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    ViewState["SortDirection"] = "ASC";
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion

    #region (Metodos)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
        ///               diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializa_Controles()
        {
            try
            {
                Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
                Limpia_Controles();             //Limpia los controles del forma
                Consulta_Tipos_Contratos();     //Consulta todos los tipos de contratos que fueron dadas de alta en la BD
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Limpiar_Controles
        /// DESCRIPCION : Limpia los controles que se encuentran en la forma
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Txt_Tipo_Contrato_ID.Text = "";
                Txt_Descripcion_Tipo_Contrato.Text = "";
                Txt_Comentarios_Tipo_Contrato.Text = "";
                Txt_Busqueda_Tipos_Contratos.Text = "";
                Cmb_Estatus_Tipo_Contrato.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Habilitar_Controles
        /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
        ///               para a siguiente operación
        /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
        ///                          si es una alta, modificacion
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Habilitar_Controles(String Operacion)
        {
            Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

            try
            {
                Habilitado = false;
                switch (Operacion)
                {
                    case "Inicial":
                        Habilitado = false;
                        Cmb_Estatus_Tipo_Contrato.SelectedIndex = 0;
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Modificar.ToolTip = "Modificar";
                        Btn_Salir.ToolTip = "Salir";
                        Btn_Nuevo.Visible = true;
                        Btn_Modificar.Visible = true;
                        Btn_Eliminar.Visible = true;                    
                        Btn_Nuevo.CausesValidation = false;
                        Btn_Modificar.CausesValidation = false;                    
                        Cmb_Estatus_Tipo_Contrato.Enabled = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                        Configuracion_Acceso("Frm_Cat_Nom_Tipos_Contratos.aspx");
                        break;

                    case "Nuevo":
                        Habilitado = true;
                        Cmb_Estatus_Tipo_Contrato.SelectedIndex = 0;
                        Btn_Nuevo.ToolTip = "Dar de Alta";
                        Btn_Modificar.ToolTip = "Modificar";
                        Btn_Salir.ToolTip = "Cancelar";
                        Btn_Nuevo.Visible = true;
                        Btn_Modificar.Visible = false;
                        Btn_Eliminar.Visible = false;                    
                        Btn_Nuevo.CausesValidation = true;
                        Btn_Modificar.CausesValidation = true;                    
                        Cmb_Estatus_Tipo_Contrato.Enabled = false;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                        break;

                    case "Modificar":
                        Habilitado = true;
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Modificar.ToolTip = "Actualizar";
                        Btn_Salir.ToolTip = "Cancelar";                    
                        Btn_Nuevo.Visible = false;
                        Btn_Modificar.Visible = true;
                        Btn_Eliminar.Visible = false;                    
                        Btn_Nuevo.CausesValidation = true;
                        Btn_Modificar.CausesValidation = true;
                        Cmb_Estatus_Tipo_Contrato.Enabled = true;
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        break;
                }
                Txt_Tipo_Contrato_ID.Enabled = false;
                Txt_Descripcion_Tipo_Contrato.Enabled = Habilitado;
                Txt_Comentarios_Tipo_Contrato.Enabled = Habilitado;
                Txt_Busqueda_Tipos_Contratos.Enabled = !Habilitado;
                Btn_Buscar_Tipos_Contratos.Enabled = !Habilitado;
                Grid_Tipos_Contratos.Enabled = !Habilitado;
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Tipos_Contratos
        /// DESCRIPCION : Consulta los tipos de contratos que estan dadas de alta en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Consulta_Tipos_Contratos()
        {
            Cls_Cat_Nom_Tipos_Contratos_Negocio Rs_Consulta_Cat_Nom_Tipos_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio(); //Variable de conexión hacia la capa de Negocios
            DataTable Dt_Tipos_Contratos; //Variable que obtendra los datos de la consulta 

            try
            {
                if (Txt_Busqueda_Tipos_Contratos.Text != "")
                {
                    Rs_Consulta_Cat_Nom_Tipos_Contratos.P_Descripcion = Txt_Busqueda_Tipos_Contratos.Text;
                }
                Dt_Tipos_Contratos = Rs_Consulta_Cat_Nom_Tipos_Contratos.Consulta_Datos_Tipo_Contrato(); //Consulta todos los tipos de contratos con sus datos generales
                Session["Consulta_Tipos_Contratos"] = Dt_Tipos_Contratos;
                Llena_Grid_Tipos_Contratos();
            }
            catch (Exception ex)
            {
                throw new Exception("Consulta_Tipos_Contratos " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Llena_Grid_Tipos_Contratos
        /// DESCRIPCION : Llena el grid con los tipos de contratos que se encuentran en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Llena_Grid_Tipos_Contratos()
        {
            DataTable Dt_Tipos_Contratos; //Variable que obtendra los datos de la consulta 
            try
            {
                Grid_Tipos_Contratos.DataBind();
                Dt_Tipos_Contratos = (DataTable)Session["Consulta_Tipos_Contratos"];
                Grid_Tipos_Contratos.DataSource = Dt_Tipos_Contratos;
                Grid_Tipos_Contratos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Tipos_Contratos " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Tipo_Contrato
        /// DESCRIPCION : Da de Alta el Tipo de Contrato con los datos proporcionados por el usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Alta_Tipo_Contrato()
        {
            Cls_Cat_Nom_Tipos_Contratos_Negocio Rs_Alta_Cat_Nom_Tipos_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
            try
            {
                Rs_Alta_Cat_Nom_Tipos_Contratos.P_Descripcion = Txt_Descripcion_Tipo_Contrato.Text;
                Rs_Alta_Cat_Nom_Tipos_Contratos.P_Estatus = Cmb_Estatus_Tipo_Contrato.SelectedValue;
                Rs_Alta_Cat_Nom_Tipos_Contratos.P_Comentarios = Txt_Comentarios_Tipo_Contrato.Text;
                Rs_Alta_Cat_Nom_Tipos_Contratos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                Rs_Alta_Cat_Nom_Tipos_Contratos.Alta_Tipo_Contrato(); //Da de alta los datos del tipo de contrato proporcionados por el usuario en la BD
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Contratos", "alert('El Alta del Tipo de Contrato fue Exitosa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Alta_Tipo_Contrato " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Modificar_Tipo_Contrato
        /// DESCRIPCION : Modifica los datos del Tipo de Contrato con los proporcionados por el usuario en la BD
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Modificar_Tipo_Contrato()
        {
            Cls_Cat_Nom_Tipos_Contratos_Negocio Rs_Modificar_Cat_Nom_Tipos_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
            try
            {
                Rs_Modificar_Cat_Nom_Tipos_Contratos.P_Tipo_Contrato_ID = Txt_Tipo_Contrato_ID.Text;
                Rs_Modificar_Cat_Nom_Tipos_Contratos.P_Descripcion = Txt_Descripcion_Tipo_Contrato.Text;
                Rs_Modificar_Cat_Nom_Tipos_Contratos.P_Estatus = Cmb_Estatus_Tipo_Contrato.SelectedValue;
                Rs_Modificar_Cat_Nom_Tipos_Contratos.P_Comentarios = Txt_Comentarios_Tipo_Contrato.Text;
                Rs_Modificar_Cat_Nom_Tipos_Contratos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;

                Rs_Modificar_Cat_Nom_Tipos_Contratos.Modificar_Tipo_Contrato(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Contratos", "alert('La Modificación del Tipo de Contrato fue Exitosa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Modificar_Tipo_Contrato " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Contrato
        /// DESCRIPCION : Elimina los datos del tipo de contrato que fue seleccionado por el Usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Eliminar_Tipo_Contrato()
        {
            Cls_Cat_Nom_Tipos_Contratos_Negocio Rs_Eliminar_Cat_Nom_Tipos_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
            try
            {
                Rs_Eliminar_Cat_Nom_Tipos_Contratos.P_Tipo_Contrato_ID = Txt_Tipo_Contrato_ID.Text;
                Rs_Eliminar_Cat_Nom_Tipos_Contratos.Eliminar_Tipo_Contrato(); //Elimina el tipo de contrato que selecciono el usuario de la BD
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Programas", "alert('La Eliminación del Tipo de Contrato fue Exitosa');", true);
            }
            catch (Exception ex)
            {
                throw new Exception("Eliminar_Tipo_Contrato " + ex.Message.ToString(), ex);
            }
        }
    #endregion

    #region (Grid)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Grid_Tipos_Contratos_SelectedIndexChanged
        /// DESCRIPCION : Consulta los datos del Tipo de Contrato que selecciono el usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 27-Agosto-2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Grid_Tipos_Contratos_SelectedIndexChanged(object sender, EventArgs e)
        {
            Cls_Cat_Nom_Tipos_Contratos_Negocio Rs_Consulta_Cat_Nom_Tipos_Contratos = new Cls_Cat_Nom_Tipos_Contratos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
            DataTable Dt_Tipo_Contrato; //Variable que obtendra los datos de la consulta

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Rs_Consulta_Cat_Nom_Tipos_Contratos.P_Tipo_Contrato_ID = Grid_Tipos_Contratos.SelectedRow.Cells[1].Text;
                Dt_Tipo_Contrato = Rs_Consulta_Cat_Nom_Tipos_Contratos.Consulta_Datos_Tipo_Contrato(); //Consulta los datos del tipo de contrato que fue seleccionado por el usuario
                if (Dt_Tipo_Contrato.Rows.Count > 0)
                {
                    //Agrega los valores de los campos a los controles correspondientes de la forma
                    foreach (DataRow Registro in Dt_Tipo_Contrato.Rows)
                    {
                        Txt_Tipo_Contrato_ID.Text = Registro[Cat_Nom_Tipos_Contratos.Campo_Tipo_Contrato_ID].ToString();
                        Txt_Descripcion_Tipo_Contrato.Text = Registro[Cat_Nom_Tipos_Contratos.Campo_Descripcion].ToString();
                        Txt_Comentarios_Tipo_Contrato.Text = Registro[Cat_Nom_Tipos_Contratos.Campo_Comentarios].ToString();
                        Cmb_Estatus_Tipo_Contrato.SelectedValue = Registro[Cat_Nom_Tipos_Contratos.Campo_Estatus].ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Grid_Tipos_Contratos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpia_Controles();                       //Limpia todos los controles de la forma
                Grid_Tipos_Contratos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Tipos_Contratos();                   //Carga los tipos de contratos que estan asignadas a la página seleccionada
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }

        /// **************************************************************************************************************************************
        /// NOMBRE: Grid_Tipos_Contratos_Sorting
        /// 
        /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
        /// 
        /// CREÓ:   Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
        /// MODIFICÓ:
        /// FECHA MODIFICÓ:
        /// CAUSA MODIFICACIÓN:
        /// **************************************************************************************************************************************
        protected void Grid_Tipos_Contratos_Sorting(object sender, GridViewSortEventArgs e)
        {
            Consulta_Tipos_Contratos();
            DataTable Dt_Tipo_Contrato = (Grid_Tipos_Contratos.DataSource as DataTable);

            if (Dt_Tipo_Contrato != null)
            {
                DataView Dv_Tipo_Contrato = new DataView(Dt_Tipo_Contrato);
                String Orden = ViewState["SortDirection"].ToString();

                if (Orden.Equals("ASC"))
                {
                    Dv_Tipo_Contrato.Sort = e.SortExpression + " " + "DESC";
                    ViewState["SortDirection"] = "DESC";
                }
                else
                {
                    Dv_Tipo_Contrato.Sort = e.SortExpression + " " + "ASC";
                    ViewState["SortDirection"] = "ASC";
                }

                Grid_Tipos_Contratos.DataSource = Dv_Tipo_Contrato;
                Grid_Tipos_Contratos.DataBind();
            }
        }
    #endregion

    #region (Eventos)
        protected void Btn_Buscar_Tipos_Contratos_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Consulta_Tipos_Contratos(); //Consulta los Tipos de Contratos que coincidan con la /// DESCRIPCION porporcionada por el usuario
                Limpia_Controles();   //Limpia los controles de la forma
                //Si no se encontraron Tipos de Contratos con una /// DESCRIPCION similar al proporcionado por el usuario entonces manda un mensaje al usuario
                if (Grid_Tipos_Contratos.Rows.Count <= 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Tipos de Contratos con la /// DESCRIPCION proporcionada <br>";
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                    Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                }
                else
                {   
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                    if (Txt_Descripcion_Tipo_Contrato.Text != "" & Txt_Comentarios_Tipo_Contrato.Text.Length <= 250)
                    {
                        Alta_Tipo_Contrato(); //Da de alta los datos proporcionados por el usuario
                    }
                    //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                        if (Txt_Descripcion_Tipo_Contrato.Text == "")
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + /// DESCRIPCION del Tipo de Contrato <br>";
                        }                    
                        if (Txt_Comentarios_Tipo_Contrato.Text.Length > 250)
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Btn_Modificar.ToolTip == "Modificar")
                {
                    if (Txt_Tipo_Contrato_ID.Text != "")
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Contrato que desea modificar sus datos <br>";
                    }
                }
                else
                {
                    //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos en la BD
                    if (Txt_Descripcion_Tipo_Contrato.Text != "" & Txt_Comentarios_Tipo_Contrato.Text.Length <= 250)
                    {
                        Modificar_Tipo_Contrato(); //Modifica los datos del Tipo de Contrato con los datos proporcionados por el usuario
                    }
                    //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                        if (Txt_Descripcion_Tipo_Contrato.Text == "")
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + /// DESCRIPCION del Tipo de Contrato <br>";
                        }
                        if (Txt_Comentarios_Tipo_Contrato.Text.Length > 250)
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si el usuario selecciono un Tipo de Contrato entonces la elimina de la base de datos
                if (Txt_Tipo_Contrato_ID.Text != "")
                {
                    Eliminar_Tipo_Contrato(); //Elimina el Tipo de Contrato que fue seleccionada por el usuario
                }
                //Si el usuario no selecciono algun Tipo de Contrato manda un mensaje indicando que es necesario que seleccione alguna para
                //poder eliminar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Contrato que desea eliminar <br>";
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Salir.ToolTip == "Salir")
                {
                    Session.Remove("Consulta_Tipos_Contratos");
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion

    #region (Control Acceso Pagina)
        /// *****************************************************************************************************************************
        /// NOMBRE: Configuracion_Acceso
        /// 
        /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
        /// 
        /// PARÁMETROS: No Áplica.
        /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
        /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
        /// USUARIO MODIFICO:
        /// FECHA MODIFICO:
        /// CAUSA MODIFICACIÓN:
        /// *****************************************************************************************************************************
        protected void Configuracion_Acceso(String URL_Pagina)
        {
            List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.
                Botones.Add(Btn_Nuevo);
                Botones.Add(Btn_Modificar);
                Botones.Add(Btn_Eliminar);
                Botones.Add(Btn_Buscar_Tipos_Contratos);

                if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
                {
                    if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                    {
                        //Consultamos el menu de la página.
                        Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                        if (Dr_Menus.Length > 0)
                        {
                            //Validamos que el menu consultado corresponda a la página a validar.
                            if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                            {
                                Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                            }
                            else
                            {
                                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: IsNumeric
        /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
        /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
        /// CREO        : Juan Alberto Hernandez Negrete
        /// FECHA_CREO  : 29/Noviembre/2010
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Es_Numero(String Cadena)
        {
            Boolean Resultado = true;
            Char[] Array = Cadena.ToCharArray();
            try
            {
                for (int index = 0; index < Array.Length; index++)
                {
                    if (!Char.IsDigit(Array[index])) return false;
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
            }
            return Resultado;
        }
        #endregion
}
