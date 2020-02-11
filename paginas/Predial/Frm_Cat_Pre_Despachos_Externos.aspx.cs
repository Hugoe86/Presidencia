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
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Catalogo_Colonias.Negocio;
using Presidencia.Catalogo_Calles.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Despachos_Externos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Cat_Pre_Despachos_Externos.aspx");
                Llenar_Combo_Colonias();
                Llenar_Combo_Calles();
                Configuracion_Formulario(true);
                Llenar_Despachos_Externos(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                            controles.
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        if (Btn_Modificar.AlternateText.Equals("Actualizar"))
        {
        }
        else
        {
            Cmb_Estatus.SelectedIndex = (0);
            Cmb_Calles.SelectedIndex = (0);
            Cmb_Colonias.SelectedIndex = (0);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Txt_Contacto.Enabled = !estatus;
        Txt_Correo.Enabled = !estatus;
        Txt_Despacho.Enabled = !estatus;
        Txt_Fax.Enabled = !estatus;
        Txt_No_Exterior.Enabled = !estatus;
        Txt_No_interior.Enabled = !estatus;
        Txt_Telefono.Enabled = !estatus;
        Cmb_Calles.Enabled = !estatus;
        Cmb_Colonias.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Grid_Despachos.Enabled = estatus;
        Grid_Despachos.SelectedIndex = (-1);
        Btn_Buscar.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
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
        Txt_Contacto.Text = "";
        Txt_Correo.Text = "";
        Txt_Despacho.Text = "";
        Txt_Fax.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_interior.Text = "";
        Txt_Telefono.Text = "";
        Txt_Id.Text = "";
        Cmb_Estatus.SelectedIndex = (0);
        Cmb_Calles.SelectedIndex = (0);
        Cmb_Colonias.SelectedIndex = (0);
        Txt_Busqueda.Text = "";
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Despachos_Externos
    ///DESCRIPCIÓN: Llena la tabla de Despachos Externos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Despachos_Externos(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Despachos_Externos_Negocio Despachos_Externos = new Cls_Cat_Pre_Despachos_Externos_Negocio();
            Despachos_Externos.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Grid_Despachos.Columns[1].Visible = true;
            Grid_Despachos.DataSource = Despachos_Externos.Consultar_Despachos_Externos();
            Grid_Despachos.PageIndex = Pagina;
            Grid_Despachos.DataBind();
            Grid_Despachos.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Colonias
    ///DESCRIPCIÓN: Llena el combo de Colonias
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 29/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Colonias()
    {
        try
        {
            Cls_Cat_Pre_Colonias_Negocio Colonias = new Cls_Cat_Pre_Colonias_Negocio();
            DataTable tabla = Colonias.Consultar_Nombre_Id_Colonias();
            DataRow fila = tabla.NewRow();
            fila[Cat_Ate_Colonias.Campo_Colonia_ID] = "SELECCIONE";
            fila[Cat_Ate_Colonias.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Colonias.DataSource = tabla;
            Cmb_Colonias.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
            Cmb_Colonias.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
            Cmb_Colonias.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Calles
    ///DESCRIPCIÓN: Llena el combo de Calles
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 29/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Calles()
    {
            try
            {
                Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                Calle.P_Colonia_ID = Cmb_Colonias.SelectedValue.ToUpper();
                DataTable tabla = Calle.Consultar_Nombre_Id_Calles();
                DataRow fila = tabla.NewRow();
                fila[Cat_Pre_Calles.Campo_Calle_ID] = "SELECCIONE";
                fila[Cat_Pre_Calles.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                tabla.Rows.InsertAt(fila, 0);
                Cmb_Calles.DataSource = tabla;
                Cmb_Calles.DataValueField = Cat_Pre_Calles.Campo_Calle_ID;
                Cmb_Calles.DataTextField = Cat_Pre_Calles.Campo_Nombre;
                Cmb_Calles.DataBind();
                if (Cmb_Colonias.SelectedIndex == 0)
                {
                    Cmb_Calles.Enabled = false;
                }
                else
                {
                    Cmb_Calles.Enabled = true;
                }
            }
            catch (Exception Exc)
            {
                Lbl_Mensaje_Error.Text = Exc.Message;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Visible = true;
            }
    }

    protected void Cmb_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Colonias.SelectedIndex != 0)
        {
            try
            {
                Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                Calle.P_Colonia_ID = Cmb_Colonias.SelectedValue.ToUpper();
                DataTable tabla = Calle.Consultar_Nombre_Id_Calles();
                DataRow fila = tabla.NewRow();
                fila[Cat_Pre_Calles.Campo_Calle_ID] = "SELECCIONE";
                fila[Cat_Pre_Calles.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                tabla.Rows.InsertAt(fila, 0);
                Cmb_Calles.DataSource = tabla;
                Cmb_Calles.DataValueField = Cat_Pre_Calles.Campo_Calle_ID;
                Cmb_Calles.DataTextField = Cat_Pre_Calles.Campo_Nombre;
                Cmb_Calles.DataBind();
                Cmb_Calles.Enabled = true;
            }
            catch( Exception Exc)
            {
                Lbl_Mensaje_Error.Text = Exc.Message;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Visible = true;
            }
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
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Cmb_Calles.SelectedIndex==0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione una calle.";
            Validacion = false;
        }
        if (Cmb_Colonias.SelectedIndex==0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione una colonia.";
            Validacion = false;
        }
        if (Txt_Contacto.Text.Trim().Length == 0 || Txt_Contacto.Text.Trim().Length > 100)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un contacto.";
            Validacion = false;
        }
        if (Txt_Correo.Text.Trim().Length == 0 || Txt_Correo.Text.Trim().Length > 100)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un correo.";
            Validacion = false;
        }
        if (Txt_Despacho.Text.Trim().Length == 0 || Txt_Despacho.Text.Trim().Length > 100)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un despacho.";
            Validacion = false;
        }
        if (Txt_Telefono.Text.Trim().Length == 0 || Txt_Telefono.Text.Trim().Length > 25)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un teléfono.";
            Validacion = false;
        }
        if (Txt_No_Exterior.Text.Trim().Length == 0 || Txt_No_Exterior.Text.Trim().Length > 5)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un número exterior.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Despachos_Externos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de las Instituciones 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Despachos_Externos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Despachos.SelectedIndex = (-1);
        Llenar_Despachos_Externos(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Despachos_Externos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Institucion seleccionada para mostrarla a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 15/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Despachos_Externos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Despachos.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Despachos.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Despachos_Externos_Negocio Despacho = new Cls_Cat_Pre_Despachos_Externos_Negocio();
                Despacho.P_Despacho_Id = ID_Seleccionado;
                Despacho = Despacho.Consultar_Datos_Despacho_Externo();
                Txt_Id.Text = Despacho.P_Despacho_Id;
                Cmb_Colonias.SelectedIndex = Cmb_Colonias.Items.IndexOf(Cmb_Colonias.Items.FindByValue(Despacho.P_Colonia));
                try
                {
                    Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                    Calle.P_Colonia_ID = Cmb_Colonias.SelectedValue.ToUpper();
                    DataTable tabla = Calle.Consultar_Nombre_Id_Calles();
                    DataRow fila = tabla.NewRow();
                    fila[Cat_Pre_Calles.Campo_Calle_ID] = "SELECCIONE";
                    fila[Cat_Pre_Calles.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                    tabla.Rows.InsertAt(fila, 0);
                    Cmb_Calles.DataSource = tabla;
                    Cmb_Calles.DataValueField = Cat_Pre_Calles.Campo_Calle_ID;
                    Cmb_Calles.DataTextField = Cat_Pre_Calles.Campo_Nombre;
                    Cmb_Calles.DataBind();
                }
                catch (Exception Exc)
                {
                    Lbl_Mensaje_Error.Text = Exc.Message;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Visible = true;
                }
                Cmb_Calles.SelectedIndex = Cmb_Calles.Items.IndexOf(Cmb_Calles.Items.FindByValue(Despacho.P_Calle));
                Txt_Contacto.Text = Despacho.P_Contacto;
                Txt_Correo.Text = Despacho.P_Correo_Electronico;
                Txt_Despacho.Text = Despacho.P_Despacho;
                Txt_Fax.Text = Despacho.P_Fax;
                Txt_No_Exterior.Text = Despacho.P_No_Exterior;
                Txt_No_interior.Text = Despacho.P_No_Interior;
                Txt_Telefono.Text = Despacho.P_Telefono;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Despacho.P_Estatus));
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
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Despacho Externo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 18/Julio/2011 
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
                try
                {
                    Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                    Calle.P_Colonia_ID = "";
                    DataTable tabla = Calle.Consultar_Nombre_Id_Calles();
                    DataRow fila = tabla.NewRow();
                    fila[Cat_Pre_Calles.Campo_Calle_ID] = "SELECCIONE";
                    fila[Cat_Pre_Calles.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                    tabla.Rows.InsertAt(fila, 0);
                    Cmb_Calles.DataSource = tabla;
                    Cmb_Calles.DataValueField = Cat_Pre_Calles.Campo_Calle_ID;
                    Cmb_Calles.DataTextField = Cat_Pre_Calles.Campo_Nombre;
                    Cmb_Calles.DataBind();
                }
                catch (Exception Exc)
                {
                    Lbl_Mensaje_Error.Text = Exc.Message;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Despachos_Externos_Negocio Despacho = new Cls_Cat_Pre_Despachos_Externos_Negocio();
                    Despacho.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Despacho.P_Calle = Cmb_Calles.SelectedItem.Value.ToUpper();
                    Despacho.P_Colonia = Cmb_Colonias.SelectedItem.Value.ToUpper();
                    Despacho.P_Contacto = Txt_Contacto.Text.ToUpper();
                    Despacho.P_Correo_Electronico = Txt_Correo.Text.ToUpper();
                    Despacho.P_Despacho = Txt_Despacho.Text.ToUpper();
                    Despacho.P_Fax = Txt_Fax.Text.ToUpper();
                    Despacho.P_No_Exterior = Txt_No_Exterior.Text.ToUpper();
                    Despacho.P_No_Interior = Txt_No_interior.Text.ToUpper();
                    Despacho.P_Telefono = Txt_Telefono.Text.ToUpper();
                    Despacho.Alta_Despacho_Externo();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Despachos_Externos(Grid_Despachos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Despachos Externos", "alert('Alta de Deschapo Externo Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
                if (Grid_Despachos.Rows.Count > 0 && Grid_Despachos.SelectedIndex > (-1))
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
                    Cls_Cat_Pre_Despachos_Externos_Negocio Despacho = new Cls_Cat_Pre_Despachos_Externos_Negocio();
                    Despacho.P_Despacho_Id = Txt_Id.Text;
                    Despacho.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    Despacho.P_Calle = Cmb_Calles.SelectedItem.Value.ToUpper();
                    Despacho.P_Colonia = Cmb_Colonias.SelectedItem.Value.ToUpper();
                    Despacho.P_Contacto = Txt_Contacto.Text.ToUpper();
                    Despacho.P_Correo_Electronico = Txt_Correo.Text.ToUpper();
                    Despacho.P_Despacho = Txt_Despacho.Text.ToUpper();
                    Despacho.P_Fax = Txt_Fax.Text.ToUpper();
                    Despacho.P_No_Exterior = Txt_No_Exterior.Text.ToUpper();
                    Despacho.P_No_Interior = Txt_No_interior.Text.ToUpper();
                    Despacho.P_Telefono = Txt_Telefono.Text.ToUpper();
                    Despacho.Modificar_Despacho_Externo();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Despachos_Externos(Grid_Despachos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Despachos Externos", "alert('Actualización de Despacho Externo Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
            if (Grid_Despachos.Rows.Count > 0 && Grid_Despachos.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Despachos_Externos_Negocio Despacho = new Cls_Cat_Pre_Despachos_Externos_Negocio();
                Despacho.P_Despacho_Id = Grid_Despachos.SelectedRow.Cells[1].Text;
                Despacho.Eliminar_Despacho_Externo();
                Grid_Despachos.SelectedIndex = (-1);
                Llenar_Despachos_Externos(Grid_Despachos.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Despachos Externos", "alert('El Despacho Externo fue eliminado exitosamente');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Despachos_Externos_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Despachos_Externos_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Despachos.SelectedIndex = (-1);
        Llenar_Despachos_Externos(0);
        Limpiar_Catalogo();
        if (Grid_Despachos.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Despachos_Externos(0);
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
            Botones.Add(Btn_Buscar);
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
