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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Impuestos_Traslado_Dominio : System.Web.UI.Page
{

        #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Configuracion_Acceso("Frm_Cat_Pre_Impuestos_Traslado_Dominio.aspx");
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Impuestos(0);                   
                }
                Div_Contenedor_Msj_Error.Visible = false;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }

    #endregion

        #region Metodos

        ///****************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PROPIEDADES:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Configuracion_Formulario(Boolean Estatus)
        {
             Btn_Nuevo.Visible = true;
             Btn_Nuevo.AlternateText = "Nuevo";
             Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
             Btn_Modificar.Visible = true;
             Btn_Modificar.AlternateText = "Modificar";
             Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
             Btn_Eliminar.Visible = Estatus;
             Txt_Anio.Enabled = !Estatus;
             Cmb_Estatus.Enabled = !Estatus;
             Txt_Valor_Inicial.Enabled = !Estatus;
             Txt_Valor_Final.Enabled = !Estatus;
             Txt_Tasa.Enabled = !Estatus;
             Txt_Deducible_Uno.Enabled = !Estatus;
             Txt_Deducible_Dos.Enabled = !Estatus;
             Txt_Deducible_Tres.Enabled = !Estatus;
             Txt_Comentarios.Enabled = !Estatus;
             Grid_Impuestos.Enabled = Estatus;
             Grid_Impuestos.SelectedIndex = (-1);
             Btn_Buscar_Impuestos.Enabled = Estatus;
             Txt_Busqueda_Impuestos_Traslado_Dominio.Enabled = Estatus; 
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo()
        {
            Txt_Tasa_ID.Text = "";
            Txt_Tasa.Text = "";
            Txt_Valor_Inicial.Text = "";
            Txt_Valor_Final.Text = "";
            Txt_Deducible_Uno.Text = "";
            Txt_Deducible_Dos.Text = "";
            Txt_Deducible_Tres.Text = "";
            Txt_Anio.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Comentarios.Text = "";
            Grid_Impuestos.DataSource = new DataTable();
            Grid_Impuestos.DataBind();

            // limpiar mensaje de error
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
        }

        #region Validaciones

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
        ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
        ///             una operación que se vea afectada en la basae de datos.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private bool Validar_Componentes_Generales()
        {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
            String Mensaje_Error = "";
            Boolean Validacion = true;
            if (Txt_Tasa.Text.Trim().Length == 0)
            {
                Mensaje_Error = Mensaje_Error + "+ Introducir la Tasa.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedIndex == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
                Validacion = false;
            }
            if (Txt_Valor_Inicial.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Valor fiscal inicial.";
                Validacion = false;
            }
            if (Txt_Valor_Final.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Valor fiscal final.";
                Validacion = false;
            }
            if (Txt_Deducible_Uno.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Deducible 1.";
                Validacion = false;
            }
            if (Txt_Deducible_Dos.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Deducible 2.";
                Validacion = false;
            }
            if (Txt_Deducible_Tres.Text.Trim().Length == 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el Deducible 3.";
                Validacion = false;
            }
            if (!Validacion)
            {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Div_Contenedor_Msj_Error.Visible = true;
            }
            return Validacion;
        }

        #endregion

        #region Grid

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Impuestos
        ///DESCRIPCIÓN: Llena la tabla de Impuestos de Traslado de Dominio
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Impuestos(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuesto = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
                Grid_Impuestos.DataSource = Impuesto.Consultar_Impuestos_Traslado_Dominio();
                Grid_Impuestos.PageIndex = Pagina;
                Grid_Impuestos.Columns[1].Visible = true;
                Grid_Impuestos.Columns[10].Visible = true;
                Grid_Impuestos.DataBind();
                Grid_Impuestos.Columns[1].Visible = false;
                Grid_Impuestos.Columns[10].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Impuestos_Busqueda
        ///DESCRIPCIÓN: Llena la tabla de Impuestos de acuerdo a la busqueda introducida.
        ///PROPIEDADES:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Tabla_Impuestos_Busqueda(int Pagina)
        {
            try
            {
                Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuestos = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
                Impuestos.P_Tasa_ID = Txt_Tasa_ID.Text.Trim();
                Impuestos.P_Anio= Txt_Busqueda_Impuestos_Traslado_Dominio.Text.ToUpper().Trim();
                Grid_Impuestos.DataSource = Impuestos.Consultar_Anio();
                Grid_Impuestos.PageIndex = Pagina;
                Grid_Impuestos.Columns[10].Visible = true;
                Grid_Impuestos.Columns[1].Visible = true;
                Grid_Impuestos.DataBind();
                Grid_Impuestos.Columns[1].Visible = false;
                Grid_Impuestos.Columns[10].Visible = false;
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        #endregion

        #endregion

        #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Impuestos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView General de Impuestos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Impuestos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Grid_Impuestos.SelectedIndex = (-1);
                Llenar_Tabla_Impuestos(e.NewPageIndex);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Impuestos_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos del Impuesto seleccionado para mostrarlos a detalle
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Impuestos_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Impuestos.SelectedIndex > (-1))
                {
                    Txt_Tasa_ID.Text = Grid_Impuestos.SelectedRow.Cells[1].Text;
                    Txt_Anio.Text = Grid_Impuestos.SelectedRow.Cells[2].Text;

                    String Valor_Inicial = Grid_Impuestos.SelectedRow.Cells[3].Text.ToString();
                    Valor_Inicial = Valor_Inicial.Replace('$', ' ');
                    Txt_Valor_Inicial.Text = Valor_Inicial.Trim();

                    String Valor_Final = Grid_Impuestos.SelectedRow.Cells[4].Text.ToString();
                    Valor_Final = Valor_Final.Replace('$', ' ');
                    Txt_Valor_Final.Text = Valor_Final.Trim();

                    Txt_Tasa.Text = Grid_Impuestos.SelectedRow.Cells[5].Text;

                    String Deducible_Uno = Grid_Impuestos.SelectedRow.Cells[6].Text.ToString();
                    Deducible_Uno = Deducible_Uno.Replace('$', ' ');
                    Txt_Deducible_Uno.Text = HttpUtility.HtmlDecode(Deducible_Uno.Trim());

                    String Deducible_Dos = Grid_Impuestos.SelectedRow.Cells[7].Text.ToString();
                    Deducible_Dos = Deducible_Dos.Replace('$', ' ');
                    Txt_Deducible_Dos.Text =  HttpUtility.HtmlDecode(Deducible_Dos.Trim());
                    
                    String Deducible_Tres = Grid_Impuestos.SelectedRow.Cells[8].Text.ToString();
                    Deducible_Tres = Deducible_Tres.Replace('$', ' ');
                    Txt_Deducible_Tres.Text = HttpUtility.HtmlDecode(Deducible_Tres.Trim());
                    
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Grid_Impuestos.SelectedRow.Cells[9].Text));
                    Txt_Comentarios.Text = HttpUtility.HtmlDecode(Grid_Impuestos.SelectedRow.Cells[10].Text);
                    System.Threading.Thread.Sleep(1000);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        #endregion

        #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
        ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Impuesto
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    Configuracion_Formulario(false);
                    Limpiar_Catalogo();
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                }
                else
                {
                    if (Validar_Componentes_Generales())
                    {
                        Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuestos = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
                        Impuestos.P_Anio = Txt_Anio.Text.Trim();
                        Impuestos.P_Valor_Inicial = Convert.ToDouble(Txt_Valor_Inicial.Text.Trim()).ToString();
                        Impuestos.P_Valor_Final = Convert.ToDouble(Txt_Valor_Final.Text.Trim()).ToString();
                        Impuestos.P_Tasa = Convert.ToDouble(Txt_Tasa.Text.Trim()).ToString();
                        Impuestos.P_Deducible_Uno = Convert.ToDouble(Txt_Deducible_Uno.Text.Trim()).ToString();
                        Impuestos.P_Deducible_Dos = Convert.ToDouble(Txt_Deducible_Dos.Text.Trim()).ToString();
                        Impuestos.P_Deducible_Tres = Convert.ToDouble(Txt_Deducible_Tres.Text.Trim()).ToString();
                        Impuestos.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                        Impuestos.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                        Limpiar_Catalogo();
                        Impuestos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Impuestos.Columns[1].Visible = true;
                        Grid_Impuestos.Columns[10].Visible = true;
                        Impuestos.Alta_Impuesto_Traslado_Dominio();
                        Grid_Impuestos.Columns[1].Visible = false;
                        Grid_Impuestos.Columns[10].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Impuestos(Grid_Impuestos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Impuestos de Traslado de Dominio", "alert('Impuesto de Traslado de Dominio Exitoso');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Impuestos.Enabled = true;
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Impuesto
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    if (Grid_Impuestos.Rows.Count > 0 && Grid_Impuestos.SelectedIndex > (-1))
                    {
                        Configuracion_Formulario(false);
                        Btn_Modificar.AlternateText = "Actualizar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                        Btn_Salir.AlternateText = "Cancelar";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                        Btn_Nuevo.Visible = false;
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes_Generales())
                    {
                        Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuestos = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
                        Impuestos.P_Tasa_ID = Txt_Tasa_ID.Text;
                        Impuestos.P_Anio = Txt_Anio.Text.Trim();
                        Impuestos.P_Valor_Inicial = Convert.ToDouble(Txt_Valor_Inicial.Text.Trim().Replace(",", "")).ToString();
                        Impuestos.P_Valor_Final = Convert.ToDouble(Txt_Valor_Final.Text.Trim().Replace(",", "")).ToString();
                        Impuestos.P_Tasa = Convert.ToDouble(Txt_Tasa.Text.Trim().Replace(",", "")).ToString();
                        Impuestos.P_Deducible_Uno = Convert.ToDouble(Txt_Deducible_Uno.Text.Trim().Replace(",", "")).ToString();
                        Impuestos.P_Deducible_Dos = Convert.ToDouble(Txt_Deducible_Dos.Text.Trim().Replace(",", "")).ToString();
                        Impuestos.P_Deducible_Tres = Convert.ToDouble(Txt_Deducible_Tres.Text.Trim().Replace(",", "")).ToString();
                        Impuestos.P_Comentarios = Txt_Comentarios.Text.ToUpper().Trim();
                        Impuestos.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                        Impuestos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Grid_Impuestos.Columns[1].Visible = true;
                        Grid_Impuestos.Columns[10].Visible = true;
                        Impuestos.Modificar_Impuesto_Traslado_Dominio();
                        Grid_Impuestos.Columns[1].Visible = false;
                        Grid_Impuestos.Columns[10].Visible = false;
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Impuestos(Grid_Impuestos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Impuestos de Traslado de Dominio", "alert('Impuesto de Traslado de Dominio Exitoso');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Grid_Impuestos.Enabled = true;
                    }

                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Limpiar_Catalogo();
                Llenar_Tabla_Impuestos_Busqueda(0);
                if (Grid_Impuestos.Rows.Count == 0 && Txt_Busqueda_Impuestos_Traslado_Dominio.Text.Trim().Length > 0)
                {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda_Impuestos_Traslado_Dominio.Text + "\" no se encotrarón coincidencias";
                    Lbl_Mensaje_Error.Text = "(Se cargaron  todos los Impuestos de Traslado de Dominio almacenados)";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Txt_Busqueda_Impuestos_Traslado_Dominio.Text = "";
                    Llenar_Tabla_Impuestos_Busqueda(0);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
        ///DESCRIPCIÓN: Elimina un Impuesto de la Base de Datos
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Eliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (Grid_Impuestos.Rows.Count > 0 && Grid_Impuestos.SelectedIndex > (-1))
                {
                    Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                    if (Convert.ToInt32(Txt_Anio.Text) > Parametros.Consultar_Anio_Corriente())
                    {
                        Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Impuestos = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
                        Impuestos.P_Tasa_ID = Grid_Impuestos.SelectedRow.Cells[1].Text;
                        Impuestos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Grid_Impuestos.Columns[1].Visible = true;
                        Grid_Impuestos.Columns[10].Visible = true;
                        Impuestos.Eliminar_Impuesto_Traslado_Dominio();
                        Grid_Impuestos.Columns[1].Visible = false;
                        Grid_Impuestos.Columns[10].Visible = false;
                        Grid_Impuestos.SelectedIndex = -1;
                        Llenar_Tabla_Impuestos(Grid_Impuestos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Impuestos de Traslado de Dominio", "alert('El Impuesto de Traslado de Dominio fue eliminado exitosamente');", true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Impuestos(0);
                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text = "No se puede eliminar el impuesto de traslado de dominio ya que es de años anteriores.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el registro que desea Eliminar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = "No se puede eliminar el impuesto de traslado de dominio ya que se encuentra en uso.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: José Alfredo García Pichardo.
        ///FECHA_CREO: 21/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e)
        {
            try
            {
                if (Btn_Salir.AlternateText.Equals("Salir"))
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Impuestos(0);
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Impuestos.Enabled = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
                Botones.Add(Btn_Buscar_Impuestos);

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

        protected void Txt_Deducible_Uno_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Deducible_Uno.Text.Trim() == "")
            {
                Txt_Deducible_Uno.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Deducible_Uno.Text = Convert.ToDouble(Txt_Deducible_Uno.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                }
                catch
                {
                    Txt_Deducible_Uno.Text = "0.00";
                }
            }
        }
        protected void Txt_Deducible_Tres_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Deducible_Tres.Text.Trim() == "")
            {
                Txt_Deducible_Tres.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Deducible_Tres.Text = Convert.ToDouble(Txt_Deducible_Tres.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                }
                catch
                {
                    Txt_Deducible_Tres.Text = "0.00";
                }
            }
        }
        protected void Txt_Deducible_Dos_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Deducible_Dos.Text.Trim() == "")
            {
                Txt_Deducible_Dos.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Deducible_Dos.Text = Convert.ToDouble(Txt_Deducible_Dos.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                }
                catch
                {
                    Txt_Deducible_Dos.Text = "0.00";
                }
            }
        }
        protected void Txt_Tasa_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Tasa.Text.Trim() == "")
            {
                Txt_Tasa.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Tasa.Text = Convert.ToDouble(Txt_Tasa.Text).ToString("##0.00");
                }
                catch
                {
                    Txt_Tasa.Text = "0.00";
                }
            }
        }
        protected void Txt_Valor_Final_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Valor_Final.Text.Trim() == "")
            {
                Txt_Valor_Final.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Valor_Final.Text = Convert.ToDouble(Txt_Valor_Final.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                }
                catch
                {
                    Txt_Valor_Final.Text = "0.00";
                }
            }
        }
        protected void Txt_Valor_Inicial_TextChanged(object sender, EventArgs e)
        {
            if (Txt_Valor_Inicial.Text.Trim() == "")
            {
                Txt_Valor_Inicial.Text = "0.00";
            }
            else
            {
                try
                {
                    Txt_Valor_Inicial.Text = Convert.ToDouble(Txt_Valor_Inicial.Text).ToString("#,###,###,###,###,###,###,###,##0.00");
                }
                catch
                {
                    Txt_Valor_Inicial.Text = "0.00";
                }
            }
        }
}