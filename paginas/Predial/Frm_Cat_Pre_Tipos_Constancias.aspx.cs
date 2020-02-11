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
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_predial_Frm_Cat_Pre_Tipos_Constancias : System.Web.UI.Page
{

    #region Pago_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Tabla_Tipos_Constancias(0);
            Cargar_Combo_Años(Cmb_Años, 2000, DateTime.Now.Year, "<SELECCIONE>");
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO
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
        Grid_Tipos_Constancias.Enabled = Estatus;
        Grid_Tipos_Constancias.SelectedIndex = (-1);
        Cmb_Estatus.Enabled = !Estatus;
        Cmb_Años.Enabled = !Estatus;
        Txt_Clave.Enabled = !Estatus;
        Txt_Nombre.Enabled = !Estatus;
        Txt_Costo.Enabled = !Estatus;
        Ckb_Certificacion.Enabled = !Estatus;
        Txt_Descripcion.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Años
    ///DESCRIPCIÓN          : Carga un combo con la lista de años indicando el comienzo y fin de esta.
    ///PARAMETROS           : Combo.    Objeto de tipo DropDownList a ser cargado
    ///                       Primer_Año, dato de tipo Int32 que indica el inicio de la lista
    ///                       Ultimo_Año, dato de tipo Int32 que indica el término de la lista
    ///                       Encabezado, dato de tipo String que indica el Texto a mostrar como primer elemento de la listas
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Combo_Años(DropDownList Combo, Int32 Primer_Año, Int32 Ultimo_Año, String Encabezado)
    {
        int Cont_Años = 0;

        Combo.Items.Clear();
        if (Ultimo_Año < 2000)
        {
            if (Primer_Año < DateTime.Now.Year)
            {
                for (Cont_Años = Primer_Año; Cont_Años <= DateTime.Now.Year; Cont_Años++)
                {
                    Combo.Items.Add(Cont_Años.ToString());
                }
            }
            else
            {
                if (Primer_Año > DateTime.Now.Year)
                {
                    for (Cont_Años = Primer_Año; Cont_Años >= DateTime.Now.Year; Cont_Años--)
                    {
                        Combo.Items.Add(Cont_Años.ToString());
                    }
                }
                else
                {
                    Combo.Items.Add(DateTime.Now.Year.ToString());
                }
            }
        }
        else
        {
            if (Primer_Año < Ultimo_Año)
            {
                for (Cont_Años = Primer_Año; Cont_Años <= Ultimo_Año; Cont_Años++)
                {
                    Combo.Items.Add(Cont_Años.ToString());
                }
            }
            else
            {
                if (Primer_Año > Ultimo_Año)
                {
                    for (Cont_Años = Primer_Año; Cont_Años >= Ultimo_Año; Cont_Años--)
                    {
                        Combo.Items.Add(Cont_Años.ToString());
                    }
                }
                else
                {
                    Combo.Items.Add(Primer_Año.ToString());
                }
            }
        }
        if (Encabezado != "")
        {
            Combo.Items.Insert(0, Encabezado);
        }
        Combo.SelectedIndex = Combo.Items.Count - 1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Hdf_Tipo_Constancia_ID.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        //Cmb_Años.SelectedIndex = 0;
        Txt_Clave.Text = "";
        Txt_Nombre.Text = "";
        Txt_Costo.Text = "";
        Txt_Descripcion.Text = "";
        Ckb_Certificacion.Checked = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Tipos_Constancias
    ///DESCRIPCIÓN          : Llena la tabla de Tipos de Constancias con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Tipos_Constancias(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
            Tipo_Constancia.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", ";
            Tipo_Constancia.P_Campos_Dinamicos += Cat_Pre_Tipos_Constancias.Campo_Clave + ", ";
            Tipo_Constancia.P_Campos_Dinamicos += Cat_Pre_Tipos_Constancias.Campo_Nombre + ", ";
            Tipo_Constancia.P_Campos_Dinamicos += Cat_Pre_Tipos_Constancias.Campo_Costo + ", ";
            Tipo_Constancia.P_Campos_Dinamicos += Cat_Pre_Tipos_Constancias.Campo_Año + ", ";
            Tipo_Constancia.P_Campos_Dinamicos += Cat_Pre_Tipos_Constancias.Campo_Estatus + ", ";
            Tipo_Constancia.P_Campos_Dinamicos += Cat_Pre_Tipos_Constancias.Campo_Certificacion;
            //Tipo_Constancia.P_Nombre = Txt_Busqueda.Text.Trim();
            //Tipo_Constancia.P_Descripcion = Txt_Busqueda.Text.Trim();
            DataTable Tabla = Tipo_Constancia.Consultar_Tipos_Constancias();
            DataView Vista = new DataView(Tabla);
            String Expresion_De_Busqueda = string.Format("{0} '%{1}%'", Grid_Tipos_Constancias.SortExpression, Txt_Busqueda.Text.Trim().ToUpper());
            Vista.RowFilter = Cat_Pre_Tipos_Constancias.Campo_Nombre + " LIKE " + Expresion_De_Busqueda;
            Vista.RowFilter += " OR " + Cat_Pre_Tipos_Constancias.Campo_Nombre + " LIKE " + Expresion_De_Busqueda;
            Grid_Tipos_Constancias.DataSource = Vista;
            Grid_Tipos_Constancias.PageIndex = Pagina;
            Grid_Tipos_Constancias.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        //Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Clave.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave del Tipo de Constancia.";
            Validacion = false;
        }
        if (Txt_Nombre.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Tipo de Constancia.";
            Validacion = false;
        }
        if (Cmb_Años.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Selecciona un Año.";
            Validacion = false;
        }
        if (Txt_Costo.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir un Costo.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir una Descripción.";
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

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Tipos_Constancias_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Constancias
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tipos_Constancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Tipos_Constancias.SelectedIndex = (-1);
            Llenar_Tabla_Tipos_Constancias(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Tipos_Constancias_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene los datos de un Tipo_Constancia Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO                 : Antonio Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tipos_Constancias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Tipos_Constancias.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                DataTable Dt_Tipo_Constancia;

                Tipo_Constancia.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + " = '" + Grid_Tipos_Constancias.SelectedRow.Cells[2].Text.Trim() + "'";
                Dt_Tipo_Constancia = Tipo_Constancia.Consultar_Tipos_Constancias();

                if (Dt_Tipo_Constancia != null)
                {
                    Hdf_Tipo_Constancia_ID.Value = Dt_Tipo_Constancia.Rows[0]["TIPO_CONSTANCIA_ID"].ToString();
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Tipo_Constancia.Rows[0]["ESTATUS"].ToString()));
                    Cmb_Años.SelectedValue = Dt_Tipo_Constancia.Rows[0]["AÑO"].ToString();
                    //Cmb_Años.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Tipo_Constancia.Rows[0]["AÑO"].ToString()));
                    Txt_Clave.Text = Dt_Tipo_Constancia.Rows[0]["CLAVE"].ToString();
                    Txt_Nombre.Text = Dt_Tipo_Constancia.Rows[0]["NOMBRE"].ToString();
                    Txt_Costo.Text = Dt_Tipo_Constancia.Rows[0]["COSTO"].ToString();
                    Txt_Descripcion.Text = Dt_Tipo_Constancia.Rows[0]["DESCRIPCION"].ToString();
                    String Certificado = Dt_Tipo_Constancia.Rows[0]["CERTIFICADO"].ToString();
                    if (Certificado.Equals("SI"))
                    {
                        Ckb_Certificacion.Checked = true;
                    }
                    else
                    {
                        Ckb_Certificacion.Checked = false;
                    }
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

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Tipo_Constancia
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
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
                Cmb_Estatus.SelectedValue = "VIGENTE";
                Cmb_Estatus.Enabled = false;
                Cmb_Años.SelectedValue = DateTime.Now.Year.ToString();
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                    Tipo_Constancia.P_Clave = Txt_Clave.Text.Trim().ToUpper();
                    Tipo_Constancia.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                    Tipo_Constancia.P_Año = Convert.ToInt32(Cmb_Años.SelectedValue);
                    Tipo_Constancia.P_Costo = Convert.ToDouble(Txt_Costo.Text);
                    Tipo_Constancia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    if (Ckb_Certificacion.Checked == true)
                    {
                        Tipo_Constancia.P_Certificacion = "SI";
                    }
                    else 
                    {
                        Tipo_Constancia.P_Certificacion = "NO";
                    }
                    Tipo_Constancia.P_Descripcion = Txt_Descripcion.Text.Trim().ToUpper();
                    Tipo_Constancia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Tipo_Constancia.Alta_Tipo_Constancia())
                    {
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Tipos_Constancias(Grid_Tipos_Constancias.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos de Constancias", "alert('Alta de Tipo de Constancia Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos de Constancias", "alert('Alta de Tipo de Constancia No fue Exitosa');", true);
                    }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Tipo_Constancia.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
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
                if (Grid_Tipos_Constancias.Rows.Count > 0 && Grid_Tipos_Constancias.SelectedIndex > (-1))
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
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                    Tipo_Constancia.P_Tipo_Constancia_ID = Hdf_Tipo_Constancia_ID.Value;
                    Tipo_Constancia.P_Clave = Txt_Clave.Text.Trim().ToUpper();
                    Tipo_Constancia.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                    Tipo_Constancia.P_Año = Convert.ToInt32(Cmb_Años.SelectedValue);
                    Tipo_Constancia.P_Costo = Convert.ToDouble(Txt_Costo.Text);
                    Tipo_Constancia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Tipo_Constancia.P_Descripcion = Txt_Descripcion.Text.Trim().ToUpper();
                    Tipo_Constancia.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Ckb_Certificacion.Checked == true)
                    {
                        Tipo_Constancia.P_Certificacion = "SI";
                    }
                    else
                    {
                        Tipo_Constancia.P_Certificacion = "NO";
                    }
                    if (Tipo_Constancia.Modificar_Tipo_Constancia())
                    {
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Tipos_Constancias(Grid_Tipos_Constancias.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos de Constancias", "alert('Actualización Tipo de Constancia Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos de Constancias", "alert('Actualización Tipo de Constancia No fue Exitosa');", true);
                    }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Tipo_Constancia_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Tipos_Constancias.SelectedIndex = (-1);
            Llenar_Tabla_Tipos_Constancias(0);
            if (Grid_Tipos_Constancias.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los Tipos de Constancias encontrados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Tipos_Constancias(0);
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    ///DESCRIPCIÓN          : Elimina un Tipo_Constancia de la Base de Datos
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Tipos_Constancias.Rows.Count > 0 && Grid_Tipos_Constancias.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Tipos_Constancias_Negocio Tipo_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                Tipo_Constancia.P_Tipo_Constancia_ID = Grid_Tipos_Constancias.SelectedRow.Cells[1].Text;
                if (Tipo_Constancia.Eliminar_Tipo_Constancia())
                {
                    Grid_Tipos_Constancias.SelectedIndex = (-1);
                    Llenar_Tabla_Tipos_Constancias(Grid_Tipos_Constancias.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos de Constancias", "alert('El Tipo de Constancia fue Eliminado Exitosamente');", true);
                    Limpiar_Catalogo();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos de Constancias", "alert('El Tipo de Constancia No fue Eliminado');", true);
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
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


    //////////#region Page_Load

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN: Page_Load
    //////////    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    //////////    ///PROPIEDADES:     
    //////////    ///CREO: 
    //////////    ///FECHA_CREO: 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************        
    //////////    protected void Page_Load(object sender, EventArgs e){
    //////////        if (!IsPostBack) {
    //////////            Configuracion_Formulario(true);
    //////////            Llenar_Tabla_Tipos_Colonias(0);
    //////////        }
    //////////        Div_Contenedor_Msj_Error.Visible = false;
    //////////    }

    //////////#endregion

    //////////#region Metodos

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    //////////    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    //////////    ///PROPIEDADES          : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    private void Configuracion_Formulario( Boolean Estatus ) {
    //////////        Btn_Nuevo.Visible = true;
    //////////        Btn_Nuevo.AlternateText = "Nuevo";
    //////////        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
    //////////        Btn_Modificar.Visible = true;
    //////////        Btn_Modificar.AlternateText = "Modificar";
    //////////        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
    //////////        Grid_Tipos_Constancias.Enabled = Estatus;
    //////////        Grid_Tipos_Constancias.SelectedIndex = (-1);
    //////////        Cmb_Estatus.Enabled = !Estatus;
    //////////        Txt_Busqueda.Enabled = Estatus;
    //////////        Txt_Descripcion.Enabled = !Estatus;
    //////////    }

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    //////////    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    //////////    ///PROPIEDADES          :     
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    private void Limpiar_Catalogo() {
    //////////        Hdf_Tipo_Constancia.Value = "";
    //////////        Cmb_Estatus.SelectedIndex = 0;
    //////////        Txt_Nombre.Text = "";
    //////////        Txt_Descripcion.Text = "";
    //////////    }

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Tipos_Colonias
    //////////    ///DESCRIPCIÓN          : Llena la tabla de Tipos_Colonias con una consulta que puede o no tener Filtros.
    //////////    ///PROPIEDADES          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Ocubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    private void Llenar_Tabla_Tipos_Colonias(int Pagina) {
    //////////        try{
    //////////            Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
    //////////            Tipo_Colonia.P_Descripcion = Txt_Busqueda.Text.Trim();
    //////////            Grid_Tipos_Constancias.DataSource = Tipo_Colonia.Consultar_Tipos_Colonias();
    //////////            Grid_Tipos_Constancias.PageIndex = Pagina;
    //////////            Grid_Tipos_Constancias.DataBind();
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }
    
    //////////    #region Validaciones

    //////////        ///*******************************************************************************
    //////////        ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    //////////        ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    //////////        ///PROPIEDADES:     
    //////////        ///CREO                 : Antonio Salvador Benvides Guardado
    //////////        ///FECHA_CREO           : 26/Octubre/2010 
    //////////        ///MODIFICO:
    //////////        ///FECHA_MODIFICO
    //////////        ///CAUSA_MODIFICACIÓN
    //////////        ///*******************************************************************************
    //////////        private bool Validar_Componentes() {
    //////////            Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
    //////////            String Mensaje_Error = "";
    //////////            Boolean Validacion = true;
    //////////            if (Cmb_Estatus.SelectedIndex == 0) {
    //////////                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
    //////////                Validacion = false;
    //////////            }
    //////////            if (Txt_Descripcion.Text.Trim().Equals("")) {
    //////////                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
    //////////                Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción.";
    //////////                Validacion = false;
    //////////            }
    //////////            if (Txt_Descripcion.Text.Trim().Length > 50) {
    //////////                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
    //////////                Mensaje_Error = Mensaje_Error + "+ Que la Descripción tenga un máximo de 50 Carácteres (Sobrepasa por " + (Txt_Descripcion.Text.Trim().Length - 50) + ").";
    //////////                Validacion = false;
    //////////            }
    //////////            if (!Validacion) {
    //////////                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
    //////////                Div_Contenedor_Msj_Error.Visible = true;
    //////////            }
    //////////            return Validacion;
    //////////        }

    //////////    #endregion

    //////////#endregion

    //////////#region Grids

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Grid_Tipos_Colonias_PageIndexChanging
    //////////    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Colonias
    //////////    ///PROPIEDADES:
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Grid_Tipos_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e){
    //////////        try{
    //////////            Grid_Tipos_Constancias.SelectedIndex = (-1);
    //////////            Llenar_Tabla_Tipos_Colonias(e.NewPageIndex);
    //////////            Limpiar_Catalogo();
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Grid_Tipos_Colonias_SelectedIndexChanged
    //////////    ///DESCRIPCIÓN          : Obtiene los datos de un Tipo_Colonia Seleccionado para mostrarlos a detalle
    //////////    ///PROPIEDADES:     
    //////////    ///CREO                 : Antonio Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Grid_Tipos_Colonias_SelectedIndexChanged(object sender, EventArgs e) {
    //////////        try{
    //////////            if (Grid_Tipos_Constancias.SelectedIndex > (-1)) {
    //////////                Limpiar_Catalogo();
    //////////                String ID_Seleccionado = Grid_Tipos_Constancias.SelectedRow.Cells[1].Text;
    //////////                Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
    //////////                Tipo_Colonia.P_Tipo_Colonia_ID = ID_Seleccionado;
    //////////                Tipo_Colonia = Tipo_Colonia.Consultar_Datos_Tipo_Colonia();
    //////////                Hdf_Tipo_Constancia.Value = Tipo_Colonia.P_Tipo_Colonia_ID;
    //////////                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Tipo_Colonia.P_Estatus));
    //////////                Txt_Descripcion.Text = Tipo_Colonia.P_Descripcion;
    //////////                Txt_Nombre.Text = Tipo_Colonia.P_Tipo_Colonia_ID;
    //////////                System.Threading.Thread.Sleep(1000);          
    //////////            }
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }
    
    //////////#endregion

    //////////#region Eventos

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    //////////    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Tipo_Colonia
    //////////    ///PROPIEDADES:     
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Btn_Nuevo_Click(object sender, EventArgs e){
    //////////        try{
    //////////            if (Btn_Nuevo.AlternateText.Equals("Nuevo")){
    //////////                Configuracion_Formulario(false);
    //////////                Limpiar_Catalogo();
    //////////                Btn_Nuevo.AlternateText = "Dar de Alta";
    //////////                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //////////                Btn_Salir.AlternateText = "Cancelar";
    //////////                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //////////                Btn_Modificar.Visible = false;
    //////////            } else {
    //////////                if (Validar_Componentes()) {
    //////////                    Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
    //////////                    Tipo_Colonia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
    //////////                    Tipo_Colonia.P_Descripcion = Txt_Descripcion.Text.Trim();
    //////////                    Tipo_Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado;
    //////////                    if (Tipo_Colonia.Alta_Tipo_Colonia()) {
    //////////                        Configuracion_Formulario(true);
    //////////                        Limpiar_Catalogo();
    //////////                        Llenar_Tabla_Tipos_Colonias(Grid_Tipos_Constancias.PageIndex);
    //////////                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Alta de Tipo Colonia Exitosa');", true);
    //////////                        Btn_Nuevo.AlternateText = "Nuevo";
    //////////                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
    //////////                        Btn_Salir.AlternateText = "Salir";
    //////////                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
    //////////                    }
    //////////                    else {
    //////////                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Alta de Tipo Colonia No fue Exitosa');", true);
    //////////                    }
    //////////                }
    //////////            }
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }    
    
    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    //////////    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Tipo_Colonia.
    //////////    ///PROPIEDADES:     
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Btn_Modificar_Click(object sender, EventArgs e){
    //////////        try{
    //////////            if (Btn_Modificar.AlternateText.Equals("Modificar")){
    //////////                if (Grid_Tipos_Constancias.Rows.Count > 0 && Grid_Tipos_Constancias.SelectedIndex > (-1)){
    //////////                    Configuracion_Formulario(false);
    //////////                    Btn_Modificar.AlternateText = "Actualizar";
    //////////                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
    //////////                    Btn_Salir.AlternateText = "Cancelar";
    //////////                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //////////                    Btn_Nuevo.Visible = false;
    //////////                }else{
    //////////                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
    //////////                    Lbl_Mensaje_Error.Text = "";
    //////////                    Div_Contenedor_Msj_Error.Visible = true;
    //////////                }
    //////////            } else {
    //////////                if (Validar_Componentes()){
    //////////                    Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
    //////////                    Tipo_Colonia.P_Tipo_Colonia_ID = Hdf_Tipo_Constancia.Value;
    //////////                    Tipo_Colonia.P_Estatus = Cmb_Estatus.SelectedItem.Value;
    //////////                    Tipo_Colonia.P_Descripcion = Txt_Descripcion.Text.Trim();
    //////////                    Tipo_Colonia.P_Usuario = Cls_Sessiones.Nombre_Empleado;
    //////////                    if (Tipo_Colonia.Modificar_Tipo_Colonia()){
    //////////                        Configuracion_Formulario(true);
    //////////                        Limpiar_Catalogo();
    //////////                        Llenar_Tabla_Tipos_Colonias(Grid_Tipos_Constancias.PageIndex);
    //////////                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Actualización Tipo Colonia Exitosa');", true);
    //////////                        Btn_Modificar.AlternateText = "Modificar";
    //////////                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
    //////////                        Btn_Salir.AlternateText = "Salir";
    //////////                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
    //////////                    }
    //////////                    else{
    //////////                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('Actualización Tipo Colonia No fue Exitosa');", true);
    //////////                    }
    //////////                }
    //////////            }
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Tipo_Colonia_Click
    //////////    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    //////////    ///PROPIEDADES          :     
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e){
    //////////        try{
    //////////            Limpiar_Catalogo();
    //////////            Grid_Tipos_Constancias.SelectedIndex = (-1);
    //////////            Llenar_Tabla_Tipos_Colonias(0);
    //////////            if (Grid_Tipos_Constancias.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0) {
    //////////                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con la descripción \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
    //////////                Lbl_Mensaje_Error.Text = "(Se cargaron todos los tipos de colonia almacenados)";
    //////////                Div_Contenedor_Msj_Error.Visible = true;
    //////////                Txt_Busqueda.Text = "";
    //////////                Llenar_Tabla_Tipos_Colonias(0);
    //////////            }
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    //////////    ///DESCRIPCIÓN          : Elimina un Tipo_Colonia de la Base de Datos
    //////////    ///PROPIEDADES          :     
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Btn_Eliminar_Click(object sender, EventArgs e){
    //////////        try{
    //////////            if (Grid_Tipos_Constancias.Rows.Count > 0 && Grid_Tipos_Constancias.SelectedIndex > (-1)){
    //////////                Cls_Cat_Pre_Tipos_Colonias_Negocio Tipo_Colonia = new Cls_Cat_Pre_Tipos_Colonias_Negocio();
    //////////                Tipo_Colonia.P_Tipo_Colonia_ID = Grid_Tipos_Constancias.SelectedRow.Cells[1].Text;
    //////////                if (Tipo_Colonia.Eliminar_Tipo_Colonia()) {
    //////////                    Grid_Tipos_Constancias.SelectedIndex = (-1);
    //////////                    Llenar_Tabla_Tipos_Colonias(Grid_Tipos_Constancias.PageIndex);
    //////////                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('El Tipo Colonia fue Eliminado Exitosamente');", true);
    //////////                    Limpiar_Catalogo();
    //////////                }else{
    //////////                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Tipos Colonias", "alert('El Tipo Colonia No fue Eliminado');", true);
    //////////                }
    //////////            }else{
    //////////                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
    //////////                Lbl_Mensaje_Error.Text = "";
    //////////                Div_Contenedor_Msj_Error.Visible = true;
    //////////            }
    //////////        }catch(Exception Ex){
    //////////            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //////////            Lbl_Mensaje_Error.Text = "";
    //////////            Div_Contenedor_Msj_Error.Visible = true;                
    //////////        }

    //////////    }

    //////////    ///*******************************************************************************
    //////////    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    //////////    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    //////////    ///PROPIEDADES:     
    //////////    ///CREO                 : Antonio Salvador Benavides Guardado
    //////////    ///FECHA_CREO           : 26/Octubre/2010 
    //////////    ///MODIFICO:
    //////////    ///FECHA_MODIFICO
    //////////    ///CAUSA_MODIFICACIÓN
    //////////    ///*******************************************************************************
    //////////    protected void Btn_Salir_Click(object sender, EventArgs e){
    //////////        if (Btn_Salir.AlternateText.Equals("Salir")){
    //////////            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    //////////        }else {
    //////////            Configuracion_Formulario(true);
    //////////            Limpiar_Catalogo();
    //////////            Btn_Salir.AlternateText = "Salir";
    //////////            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
    //////////        }
    //////////    }

    //////////#endregion
}
