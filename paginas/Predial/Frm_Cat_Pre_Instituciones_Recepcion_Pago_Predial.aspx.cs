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
using Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Instituciones_Recepcion_Pago_Predial : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Llenar_Combo_Cajas();
                Llenar_Instituciones(0);
                Configuracion_Formulario(true);
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

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                            controles.
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
       // Btn_Nuevo.Visible = true;
        if (Btn_Modificar.AlternateText.Equals("Actualizar"))
        {
            Btn_Nuevo.Visible = true;
        }
        else
        {
            Cmb_Estatus.SelectedIndex = (0);
            Cmb_Caja.SelectedIndex = (-1);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Txt_Institucion.Enabled = !estatus;
        Txt_Convenio.Enabled = !estatus;
        //Txt_Texto.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        //Cmb_Campos.Enabled = !estatus;
        Cmb_Caja.Enabled = !estatus;
        //Cmb_Mes.Enabled = !estatus;
        Grid_Instituciones.Enabled = estatus;
        Grid_Instituciones.SelectedIndex = (-1);
        Btn_Buscar.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
        Cmb_Lineas_Captura_Enero.Enabled = !estatus;
        Cmb_Lineas_Captura_Febrero.Enabled = !estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Busqueda.Text = "";
        Txt_Id.Text = "";
        Txt_Convenio.Text = "";
        Txt_Institucion.Text = "";
        //Txt_Texto.Text = "";
        Cmb_Caja.SelectedIndex = (-1);
        Cmb_Estatus.SelectedIndex = (0);
        //Cmb_Campos.SelectedIndex = (0);
        //Cmb_Mes.SelectedIndex = (0);
        Cmb_Lineas_Captura_Enero.SelectedIndex = (0);
        Cmb_Lineas_Captura_Febrero.SelectedIndex = (0);

        //limpiar mensajes de error
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Instituciones
    ///DESCRIPCIÓN: Llena la tabla de Instituciones
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Instituciones(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
            Institucion.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Instituciones.Columns[1].Visible = true;
            Grid_Instituciones.Columns[3].Visible = true;
            Grid_Instituciones.DataSource = Institucion.Consultar_Institucion();
            Grid_Instituciones.PageIndex = Pagina;
            Grid_Instituciones.DataBind();
            Grid_Instituciones.Columns[1].Visible = false;
            Grid_Instituciones.Columns[3].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cajas
    ///DESCRIPCIÓN: Llena el combo de Cajas
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Cajas()
    {
        try
        {
            Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
            DataTable tabla = Institucion.Consultar_Cajas_Nombre_Id();
            //DataRow fila = tabla.NewRow();
            //fila[Cat_Pre_Cajas.Campo_Caja_Id] = "SELECCIONE";
            //fila[Cat_Pre_Cajas.Campo_Clave] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            //tabla.Rows.InsertAt(fila, 0);
            Cmb_Caja.DataSource = tabla;
            Cmb_Caja.DataValueField = Cat_Pre_Cajas.Campo_Caja_Id;
            Cmb_Caja.DataTextField = Cat_Pre_Cajas.Campo_Clave;
            Cmb_Caja.DataBind();
            Cmb_Caja.Items.Insert(0, new ListItem("<SELECCIONE>", "SELECCIONE"));
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        String Lineas_Captura = "";

        // poner las lineas de captura seleccionadas en un string
        Lineas_Captura = Cmb_Lineas_Captura_Enero.SelectedValue + "/" + Cmb_Lineas_Captura_Febrero.SelectedValue;
        // verificar si las lineas de captura seleccionadas requieren convenio
        if (Lineas_Captura.Contains("BANAMEX") || Lineas_Captura.Contains("BANCOMER") || Lineas_Captura.Contains("BANORTE"))
        {
            // validar que no este vacio el convenio, de ser asi, mostrar mensaje
            if (Txt_Convenio.Text == "")
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introducir el n&uacute;mero de convenio con la instituci&oacute;n.";
                Validacion = false;
            }
        }

        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Institucion.Text.Trim().Length == 0 || Txt_Institucion.Text.Trim().Length > 50)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Institución.";
            Validacion = false;
        }
        if (Cmb_Caja.SelectedIndex==0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Cajas.";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Instituciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de las Instituciones 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Instituciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Instituciones.SelectedIndex = (-1);
        Llenar_Instituciones(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Instituciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Institucion seleccionada para mostrarla a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Instituciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Instituciones.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Instituciones.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
                Institucion.P_Institucion_Id = ID_Seleccionado;
                Institucion = Institucion.Consultar_Datos_Institucion();
                Txt_Id.Text = Institucion.P_Institucion_Id;
                Txt_Institucion.Text = Institucion.P_Institucion;
                Txt_Convenio.Text = Institucion.P_Convenio;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Institucion.P_Estatus));
                Cmb_Caja.SelectedIndex = Cmb_Caja.Items.IndexOf(Cmb_Caja.Items.FindByValue(Institucion.P_Caja_Id));
                Cmb_Lineas_Captura_Enero.SelectedIndex = Cmb_Lineas_Captura_Enero.Items.IndexOf(Cmb_Lineas_Captura_Enero.Items.FindByValue(Institucion.P_Linea_Captura_Enero));
                Cmb_Lineas_Captura_Febrero.SelectedIndex = Cmb_Lineas_Captura_Febrero.Items.IndexOf(Cmb_Lineas_Captura_Febrero.Items.FindByValue(Institucion.P_Linea_Captura_Febrero));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Institucion
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
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
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
                    Institucion.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Institucion.P_Institucion = Txt_Institucion.Text.ToUpper();
                    Institucion.P_Convenio = Txt_Convenio.Text.Trim();
                    Institucion.P_Caja_Id = Cmb_Caja.SelectedItem.Value.ToUpper();
                    // evitar que si no se selecciona linea de captura se inserte el texto SELECCIONE
                    if (Cmb_Lineas_Captura_Enero.SelectedItem.Value != "SELECCIONE")
                    {
                        Institucion.P_Linea_Captura_Enero = Cmb_Lineas_Captura_Enero.SelectedItem.Value.ToUpper();
                    }
                    else
                    {
                        Institucion.P_Linea_Captura_Enero = "";
                    }
                    if (Cmb_Lineas_Captura_Febrero.SelectedItem.Value != "SELECCIONE")
                    {
                        Institucion.P_Linea_Captura_Febrero = Cmb_Lineas_Captura_Febrero.SelectedItem.Value.ToUpper();
                    }
                    else
                    {
                        Institucion.P_Linea_Captura_Febrero = "";
                    }
                    if (Institucion.Consultar_No_Repetir_Caja().Rows.Count == 0)
                    {
                        Institucion.Alta_Institucion();

                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Instituciones(Grid_Instituciones.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Instituciones", "alert('Alta de Institucion Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Instituciones", "alert('Caja asignada a otra institución, seleccione otra caja por favor.');", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Institucion
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Instituciones.Rows.Count > 0 && Grid_Instituciones.SelectedIndex > (-1))
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el registro que quiere modificar.";
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
                    Institucion.P_Institucion_Id = Txt_Id.Text;
                    Institucion.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Institucion.P_Institucion = Txt_Institucion.Text.ToUpper();
                    Institucion.P_Convenio = Txt_Convenio.Text.Trim();
                    Institucion.P_Caja_Id = Cmb_Caja.SelectedItem.Value.ToUpper();
                    // evitar que si no se selecciona linea de captura se inserte el texto SELECCIONE
                    if (Cmb_Lineas_Captura_Enero.SelectedItem.Value != "SELECCIONE")
                    {
                        Institucion.P_Linea_Captura_Enero = Cmb_Lineas_Captura_Enero.SelectedItem.Value.ToUpper();
                    }
                    else
                    {
                        Institucion.P_Linea_Captura_Enero = "";
                    }
                    if (Cmb_Lineas_Captura_Febrero.SelectedItem.Value != "SELECCIONE")
                    {
                        Institucion.P_Linea_Captura_Febrero = Cmb_Lineas_Captura_Febrero.SelectedItem.Value.ToUpper();
                    }
                    else
                    {
                        Institucion.P_Linea_Captura_Febrero = "";
                    }
                    if (Institucion.Consultar_No_Repetir_Caja().Rows.Count == 0)
                    {
                        Institucion.Modificar_Institucion();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Instituciones(Grid_Instituciones.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Instituciones", "alert('Actualización de Institucion Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Instituciones", "alert('Caja asignada a otra institución, seleccione otra caja por favor.');", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina una Institución de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Instituciones.Rows.Count > 0 && Grid_Instituciones.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Institucion = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
                Institucion.P_Institucion_Id = Grid_Instituciones.SelectedRow.Cells[1].Text;
                Institucion.Eliminar_Institucion();
                Grid_Instituciones.SelectedIndex = (-1);
                Llenar_Instituciones(Grid_Instituciones.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cat&acute;logo de Instituciones", "alert('La Institución fue actualizada exitosamente');", true);
                Limpiar_Catalogo();
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Institucion_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Institucion_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Instituciones.SelectedIndex = (-1);
        Llenar_Instituciones(0);
        Limpiar_Catalogo();
        if (Grid_Instituciones.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Instituciones(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    #endregion

}
