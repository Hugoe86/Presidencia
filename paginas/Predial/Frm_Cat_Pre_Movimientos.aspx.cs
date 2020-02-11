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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Movimientos.Negocio;
using Presidencia.Catalogo_Claves_Grupos_Movimiento.Negocio;
using Presidencia.Catalogo_Grupos.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Movimientos : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Combo_Grupos();
            Llenar_Movimientos(0);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Crear_Tabla_Modulos
    ///DESCRIPCIÓN              : Crea un DataTable con los Modulos a cargar
    ///PROPIEDADES: 
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 10/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Crear_Tabla_Modulos()
    {
        DataTable Dt_Modulos = new DataTable("DataTable");

        DataRow Dr_Modulos = null;
        Dt_Modulos.Columns.AddRange(new DataColumn[] { new DataColumn("Modulo_ID"), new DataColumn("Descripcion_Modulo") });

        Dr_Modulos = Dt_Modulos.NewRow();

        Dr_Modulos["Modulo_ID"] = "Bajas_Directas";
        Dr_Modulos["Descripcion_Modulo"] = "Bajas Directas";
        Dt_Modulos.Rows.Add(Dr_Modulos);

        Dr_Modulos = Dt_Modulos.NewRow();
        Dr_Modulos["Modulo_ID"] = "Cancelacion_Cuentas";
        Dr_Modulos["Descripcion_Modulo"] = "Cancelación de Cuentas";
        Dt_Modulos.Rows.Add(Dr_Modulos);

        Dr_Modulos = Dt_Modulos.NewRow();
        Dr_Modulos["Modulo_ID"] = "Reactivacion_Cuentas";
        Dr_Modulos["Descripcion_Modulo"] = "Reactivación de Cuentas";
        Dt_Modulos.Rows.Add(Dr_Modulos);

        return Dt_Modulos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                             controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = estatus;
        Cmb_Aplica.Enabled = !estatus;
        Txt_Identificador.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Cmb_Grupo.Enabled = !estatus;
        Chk_Lst_Cargar_Modulos.Enabled = !estatus;
        Txt_Descripcion.Enabled = !estatus;
        Grid_Movimientos.Enabled = estatus;
        Grid_Movimientos.SelectedIndex = (-1);
        Btn_Buscar_Movimiento.Enabled = estatus;
        Txt_Busqueda_Movimiento.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Identificador.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Grupo.SelectedIndex = 0;
        Txt_Descripcion.Text = "";
        Txt_ID_Movimiento.Text = "";
        Hdf_Movimiento_ID.Value = "";
        Cmb_Aplica.SelectedIndex = 0;
        Chk_Lst_Cargar_Modulos.SelectedIndex = -1;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Movimientos
    ///DESCRIPCIÓN: Llena la tabla de Movimientos con una consulta que puede o no
    ///             tener Filtros.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Movimientos(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
            Movimiento.P_Identificador = Txt_Busqueda_Movimiento.Text.Trim().ToUpper();
            Movimiento.P_Ordenar_Dinamico = Cat_Pre_Movimientos.Campo_Identificador;
            Grid_Movimientos.Columns[1].Visible = true;
            Grid_Movimientos.DataSource = Movimiento.Consultar_Movimientos();
            Grid_Movimientos.PageIndex = Pagina;
            Grid_Movimientos.DataBind();
            Grid_Movimientos.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Grupos
    ///DESCRIPCIÓN: Llena el combo de Grupos
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 10/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Grupos()
    {
        try
        {
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupos = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
            Grupos.P_Tipo_DataTable = "GRUPOS_MOVIMIENTO";
            DataTable tabla = Grupos.Consultar_DataTable();
            DataRow fila = tabla.NewRow();
            fila[Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID] = "SELECCIONE";
            fila[Cat_Pre_Grupos_Movimiento.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            tabla.Rows.InsertAt(fila, 0);
            Cmb_Grupo.DataSource = tabla;
            Cmb_Grupo.DataValueField = Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID;
            Cmb_Grupo.DataTextField = Cat_Pre_Grupos.Campo_Nombre;
            Cmb_Grupo.DataBind();
        }
        catch (Exception Ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 03/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Identificador.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Identificador.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Cmb_Grupo.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Grupos.";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n.";
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Movimientos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Movimientos 
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Movimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Movimientos.SelectedIndex = (-1);
            Llenar_Movimientos(e.NewPageIndex);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Movimientos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un Movimiento Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Movimientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Movimientos.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Movimientos.SelectedRow.Cells[1].Text;
                Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
                Movimiento.P_Movimiento_ID = ID_Seleccionado;
                Movimiento = Movimiento.Consultar_Datos_Movimiento();
                Hdf_Movimiento_ID.Value = Movimiento.P_Movimiento_ID;
                Txt_ID_Movimiento.Text = Movimiento.P_Movimiento_ID;
                Txt_Identificador.Text = Movimiento.P_Identificador;
                Cmb_Aplica.SelectedIndex = Cmb_Aplica.Items.IndexOf(Cmb_Aplica.Items.FindByValue(Movimiento.P_Aplica));
                Txt_Descripcion.Text = Movimiento.P_Descripcion;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Movimiento.P_Estatus));
                Cmb_Grupo.SelectedIndex = Cmb_Grupo.Items.IndexOf(Cmb_Grupo.Items.FindByValue(Movimiento.P_Grupo_ID));
                Seleccionar_Items_Check_List(Chk_Lst_Cargar_Modulos, Grid_Movimientos.DataKeys[Grid_Movimientos.SelectedIndex].Values[0].ToString());
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Seleccionar_Items_Check_List
    ///DESCRIPCIÓN              : Selecciona en el ChekBoxList los elementos que coincidan con la Cadena de Datos dada
    ///PROPIEDADES: 
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 11/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Seleccionar_Items_Check_List(CheckBoxList Chk_List, String Datos)
    {
        char[] Separador = { ',' };

        if (Datos.Trim().EndsWith(","))
        {
            Datos = Datos.Substring(0, Datos.Length - 1);
        }

        foreach (ListItem Item_Chk_List in Chk_List.Items)
        {
            foreach (String Dato in Datos.Split(Separador))
            {
                if (Dato.Trim().ToUpper() == Item_Chk_List.Value.Trim().ToUpper())
                {
                    Item_Chk_List.Selected = true;
                    break;
                }
            }
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Movimiento
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
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
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
                    Movimiento.P_Identificador = Txt_Identificador.Text.ToUpper();
                    Movimiento.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    if (Cmb_Aplica.SelectedIndex > 0)
                    {
                        Movimiento.P_Aplica = Cmb_Aplica.SelectedItem.Value;
                    }
                    Movimiento.P_Cargar_Modulos = Obtener_Valores_Items_Seleccionados_Chek_List(Chk_Lst_Cargar_Modulos);
                    Movimiento.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Movimiento.P_Grupo_ID = Cmb_Grupo.SelectedValue;
                    Movimiento.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    if (!Movimiento.Validar_Existe())
                    {
                        Movimiento.Alta_Movimiento();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Movimientos(Grid_Movimientos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Movimientos", "alert('Alta de Movimiento Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Movimientos", "alert('El Movimiento que intenta Agregar ya existe');", true);
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
    ///NOMBRE DE LA FUNCIÓN     : Obtener_Valores_Items_Seleccionados_Chek_List
    ///DESCRIPCIÓN              : Obtiene una cadena con los Valores seleccionado del CheckList
    ///PROPIEDADES: 
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 11/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Valores_Items_Seleccionados_Chek_List(CheckBoxList Chek_List)
    {
        String Valores_Seleccionados = "";
        foreach (ListItem Item_Check_List in Chek_List.Items)
        {
            if (Item_Check_List.Selected)
            {
                Valores_Seleccionados += Item_Check_List.Value + ",";
            }
        }
        if (Valores_Seleccionados.Trim().EndsWith(","))
        {
            Valores_Seleccionados = Valores_Seleccionados.Substring(0, Valores_Seleccionados.Length - 1);
        }
        return Valores_Seleccionados;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Movimiento
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
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
                if (Grid_Movimientos.Rows.Count > 0 && Grid_Movimientos.SelectedIndex > (-1))
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
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
                    Movimiento.P_Movimiento_ID = Hdf_Movimiento_ID.Value.ToUpper();
                    Movimiento.P_Identificador = Txt_Identificador.Text.ToUpper();
                    Movimiento.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                    if (Cmb_Aplica.SelectedIndex > 0)
                    {
                        Movimiento.P_Aplica = Cmb_Aplica.SelectedItem.Value;
                    }
                    Movimiento.P_Cargar_Modulos = Obtener_Valores_Items_Seleccionados_Chek_List(Chk_Lst_Cargar_Modulos);
                    Movimiento.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Movimiento.P_Grupo_ID = Cmb_Grupo.SelectedItem.Value.ToUpper();
                    Movimiento.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    if (!Movimiento.Validar_Existe())
                    {
                        Movimiento.Modificar_Movimiento();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Movimientos(Grid_Movimientos.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Movimientos", "alert('Actualización Movimiento Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Movimientos", "alert('El Movimiento que intenta Modificar ya existe con el mismo Identificador y mismo Aplica');", true);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Movimiento_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Movimiento_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Movimientos.SelectedIndex = (-1);
            Llenar_Movimientos(0);
            if (Grid_Movimientos.Rows.Count == 0 && Txt_Busqueda_Movimiento.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Búsqueda con el Identificador \"" + Txt_Busqueda_Movimiento.Text + "\" no se encotraron coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los movimientos almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda_Movimiento.Text = "";
                Llenar_Movimientos(0);
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
    ///DESCRIPCIÓN: Elimina un Movimiento de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Movimientos.Rows.Count > 0 && Grid_Movimientos.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
                Movimiento.P_Movimiento_ID = Grid_Movimientos.SelectedRow.Cells[1].Text;
                Movimiento.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Movimiento.Eliminar_Movimiento();
                Grid_Movimientos.SelectedIndex = (-1);
                Llenar_Movimientos(Grid_Movimientos.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Movimiento", "alert('El Movimiento fue eliminado exitosamente');", true);
                Limpiar_Catalogo();
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
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
}