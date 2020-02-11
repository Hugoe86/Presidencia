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
using Presidencia.Cat_Terceros.Negocio;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Cat_Nom_Terceros : System.Web.UI.Page
{

    #region (Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();
                ViewState["SortDirection"] = "ASC";
            }
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex) {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (GridView)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Terceros_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Terceros_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Terceros.PageIndex = e.NewPageIndex;
            Consultar_Terceros();
            Grid_Terceros.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Terceros_SelectedIndexChanged
    ///DESCRIPCIÓN: Realiza la seleccion de un elemento de la tabla
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 5/Noviembre/2010  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Terceros_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            int index = ((Grid_Terceros.PageIndex) * Grid_Terceros.PageSize) + (Grid_Terceros.SelectedIndex);
            if (index != -1)
            {
                Txt_Tercero_ID.Text = Grid_Terceros.Rows[index].Cells[1].Text;
                Txt_Nombre_Tercero.Text = Grid_Terceros.Rows[index].Cells[2].Text;
                Txt_Contacto_Tercero.Text = Grid_Terceros.Rows[index].Cells[4].Text;
                Txt_Telefono.Text = Grid_Terceros.Rows[index].Cells[3].Text;
                Txt_Porcentaje_Retencion.Text = Grid_Terceros.Rows[index].Cells[5].Text;
                Txt_Comentarios.Text = Grid_Terceros.Rows[index].Cells[6].Text;
                Cmb_Deducciones_Calculadas.SelectedIndex = Cmb_Deducciones_Calculadas.Items.IndexOf(Cmb_Deducciones_Calculadas.Items.FindByValue(HttpUtility.HtmlDecode(Grid_Terceros.Rows[index].Cells[7].Text.Trim())));
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error seleccionar un elemento de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Terceros_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Terceros_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consultar_Terceros();
        DataTable Dt_Terceros = (Grid_Terceros.DataSource as DataTable);

        if (Dt_Terceros != null)
        {
            DataView Dv_Terceros = new DataView(Dt_Terceros);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Terceros.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Terceros.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Terceros.DataSource = Dv_Terceros;
            Grid_Terceros.DataBind();
        }
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Terceros
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Ctlrs();
        Consultar_Terceros();
        Consultar_Deducciones();
        Habilitar_Controles("Inicial");
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Ctlrs()
    {
        Txt_Tercero_ID.Text = "";
        Txt_Nombre_Tercero.Text = "";
        Txt_Contacto_Tercero.Text = "";        
        Txt_Telefono.Text = "";
        Txt_Porcentaje_Retencion.Text = "";
        Txt_Comentarios.Text = "";
        Cmb_Deducciones_Calculadas.SelectedIndex = -1;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Txt_Busqueda_Tercero.Enabled = true;
                    Btn_Buscar_Tercero.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Configuracion_Acceso("Frm_Cat_Nom_Terceros.aspx");
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    Txt_Busqueda_Tercero.Enabled = false;
                    Btn_Buscar_Tercero.Enabled = false;
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
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";

                    Txt_Busqueda_Tercero.Enabled = false;
                    Btn_Buscar_Tercero.Enabled = false;
                    break;
            }
            Txt_Tercero_ID.Enabled = false;
            Cmb_Deducciones_Calculadas.Enabled = Habilitado;
            Txt_Nombre_Tercero.Enabled = Habilitado;
            Txt_Contacto_Tercero.Enabled = Habilitado;            
            Txt_Telefono.Enabled = Habilitado;
            Txt_Porcentaje_Retencion.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Grid_Terceros.Enabled = !Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Tercero
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Tercero()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Deducciones_Calculadas.SelectedIndex <= 0) {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione la Deducción <br>";
            Datos_Validos = false;        
        }

        if (string.IsNullOrEmpty(Txt_Nombre_Tercero.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Nombre <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Contacto_Tercero.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Contacto <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Telefono.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Telefono <br>";
            Datos_Validos = false;
        }
        else if(!Validar_Telefono()){
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de Teléfono Incorrecto <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Porcentaje_Retencion.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Porcentaje de Retencion <br>";
            Datos_Validos = false;
        }
        else if (Convert.ToDouble(Txt_Porcentaje_Retencion.Text.Trim()) > 100 ||
            Convert.ToDouble(Txt_Porcentaje_Retencion.Text.Trim()) < 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Porcentaje de Retencion no puede ser mayor a 1<br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Comentarios.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comentarios <br>";
            Datos_Validos = false;
        }        
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Telefono
    /// DESCRIPCION : Valida el Telefono Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Telefono()
    {
        string MatchEmailPattern = @"^[0-9]{2,3}-? ?[0-9]{7,10}$";

        if (Txt_Telefono.Text != null) return Regex.IsMatch(Txt_Telefono.Text, MatchEmailPattern);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean IsNumeric(String Cadena)
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Nombre
    /// DESCRIPCION : Junta el nombre del concepto con la clave.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Nombre(DataTable Dt_Conceptos)
    {
        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                                "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                    CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir el nombre con la clave del concepto. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    #endregion

    #region (Metodos de Operacion [Alta - Modificar- Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Tercero
    /// DESCRIPCION : Ejecuta la Alta de un Tercero
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Tercero()
    {
        Cls_Cat_Nom_Terceros_Negocio _Cat_Tercero = new Cls_Cat_Nom_Terceros_Negocio();
        try
        {
            _Cat_Tercero.P_Percepcion_Deduccion_ID = Cmb_Deducciones_Calculadas.SelectedValue.Trim();
            _Cat_Tercero.P_Nombre = Txt_Nombre_Tercero.Text;
            _Cat_Tercero.P_Contacto = Txt_Contacto_Tercero.Text;
            _Cat_Tercero.P_Telefono = Txt_Telefono.Text;
            _Cat_Tercero.P_Porcentaje_Retencion = (Txt_Porcentaje_Retencion.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Retencion.Text));
            _Cat_Tercero.P_Comentarios = Txt_Comentarios.Text;
            _Cat_Tercero.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            if (_Cat_Tercero.Alta_Terceros())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Tercero]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar a un Tercero. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Tercero
    /// DESCRIPCION : Ejecuta la Actualizacion de un Tercero
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Tercero()
    {
        Cls_Cat_Nom_Terceros_Negocio _Cat_Tercero = new Cls_Cat_Nom_Terceros_Negocio();
        try
        {
            _Cat_Tercero.P_Tercero_ID= Txt_Tercero_ID.Text;
            _Cat_Tercero.P_Percepcion_Deduccion_ID = Cmb_Deducciones_Calculadas.SelectedValue.Trim();
            _Cat_Tercero.P_Nombre = Txt_Nombre_Tercero.Text;
            _Cat_Tercero.P_Contacto = Txt_Contacto_Tercero.Text;            
            _Cat_Tercero.P_Telefono = Txt_Telefono.Text;
            _Cat_Tercero.P_Porcentaje_Retencion = (Txt_Porcentaje_Retencion.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Porcentaje_Retencion.Text));
            _Cat_Tercero.P_Comentarios = Txt_Comentarios.Text;
            _Cat_Tercero.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            if (_Cat_Tercero.Modificar_Terceros())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Actualizar Tercero]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Actualizar a un Tercero. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Tercero
    /// DESCRIPCION : Ejecuta la Baja de un Tercero
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Tercero()
    {
        Cls_Cat_Nom_Terceros_Negocio _Cat_Tercero = new Cls_Cat_Nom_Terceros_Negocio();
        try
        {
            _Cat_Tercero.P_Tercero_ID = Txt_Tercero_ID.Text;

            if (_Cat_Tercero.Eliminar_Terceros())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Tercero]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar Combo de Percepciones Deducciones
    /// DESCRIPCION : Carga las Percepciones Deducciones Fijas o Variables que no son calculadas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones()
    {
        DataTable Dt_Deducciones = null;
        Cls_Cat_Nom_Terceros_Negocio Cat_Deducciones = new Cls_Cat_Nom_Terceros_Negocio();
        try
        {
            Dt_Deducciones = Cat_Deducciones.Consulta_Percepciones_Deducciones();
            Dt_Deducciones = Juntar_Clave_Nombre(Dt_Deducciones);

            Session["Dt_Deducciones_Combo"] = Dt_Deducciones;
            Cmb_Deducciones_Calculadas.DataSource = Dt_Deducciones;
            Cmb_Deducciones_Calculadas.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Cmb_Deducciones_Calculadas.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Deducciones_Calculadas.DataBind();
            Cmb_Deducciones_Calculadas.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Deducciones_Calculadas.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Deducciones. Error: [" + Ex.Message + "]");
        }

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Terceros
    /// DESCRIPCION : Consultar la tabla de Terceros
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 5/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Terceros()
    {
        DataTable Dt_Terceros;
        Cls_Cat_Nom_Terceros_Negocio Cat_Terceros = new Cls_Cat_Nom_Terceros_Negocio();

        try
        {
            Grid_Terceros.Columns[4].Visible = true;
            Grid_Terceros.Columns[5].Visible = true;
            Grid_Terceros.Columns[6].Visible = true;
            Grid_Terceros.Columns[7].Visible = true;
            if (string.IsNullOrEmpty(Txt_Busqueda_Tercero.Text))
            {
                Dt_Terceros = Cat_Terceros.Consultar_Terceros();
            }
            else
            {
                Cat_Terceros.P_Nombre = (IsNumeric(Txt_Busqueda_Tercero.Text) ? "" : Txt_Busqueda_Tercero.Text);
                Cat_Terceros.P_Tercero_ID = (IsNumeric(Txt_Busqueda_Tercero.Text) ? Txt_Busqueda_Tercero.Text : "");
                Dt_Terceros = Cat_Terceros.Consultar_Terceros_Nombre();
            }
            Grid_Terceros.DataSource = Dt_Terceros;
            Grid_Terceros.DataBind();

            Grid_Terceros.Columns[4].Visible = false;
            Grid_Terceros.Columns[5].Visible = false;
            Grid_Terceros.Columns[6].Visible = false;
            Grid_Terceros.Columns[7].Visible = false;

            Grid_Terceros.SelectedIndex = -1;

            if (Grid_Terceros.Rows.Count <= 0) {
                Lbl_Mensaje_Error.Text = "No se encontraron resultados para la busqueda realizada";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al consultar la tabla de terceros. Error [" + Ex.Message + "]");
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
            Botones.Add(Btn_Buscar_Tercero);

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

    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Tercero
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Ctlrs();
            }
            else
            {
                if (Validar_Datos_Tercero())
                {
                    Alta_Tercero();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    ///DESCRIPCIÓN: Modificar un Tercero
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Terceros.SelectedIndex != -1 & !Txt_Tercero_ID.Text.Equals(""))
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos_Tercero())
                {
                    Modificar_Tercero();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar un Tercero
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 5/Noviembre/2010  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (Grid_Terceros.SelectedIndex != -1 & !Txt_Tercero_ID.Text.Equals(""))
                {
                    Eliminar_Tercero();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 5/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Click
    ///DESCRIPCIÓN: Busqueda de Terceros
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 6/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Terceros();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
}
