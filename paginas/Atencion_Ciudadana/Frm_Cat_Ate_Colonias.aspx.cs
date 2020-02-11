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
using Presidencia.Colonias.Negocios;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Constantes;


public partial class paginas_Atencion_Ciudadana_Frm_Cat_Ate_Colonias : System.Web.UI.Page {

    #region Variables

    private Cls_Cat_Ate_Colonias_Negocio Colonia_Negocio;
    
    #endregion

    #region Page_Load

    protected void Page_Init(object sender, EventArgs e)
    {
        Colonia_Negocio = new Cls_Cat_Ate_Colonias_Negocio();
        Cargar_Datos_Negocio(false);
        Llenar_Grid_Colonias();
        Habilitar_Cajas(false);
        Estado_Botones("inicial");
        Limpiar_Formulario();
        Llenar_Combo_Tipos_Colonias();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Btn_Eliminar.Enabled = false;
        //Grid_Colonias.Columns[4].Visible = false;
    }
    #endregion
    
    #region Metodos 
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Cajas
    ///DESCRIPCIÓN: MEtodo que habilita las cajas
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Cajas(Boolean estatus)
    {
        txt_Nombre.Enabled = estatus;
        txt_Descripcion.Enabled = estatus;
        Cmb_Tipo_Colonia.Enabled = estatus;

    }// fin de Habilitar_Cajas

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia los componentes textbox
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Limpiar_Formulario()
    {
        Txt_Busqueda_colonias.Text = "";
        txt_Colonia_ID.Text = "";
        txt_Nombre.Text = "";
        txt_Descripcion.Text = "";
        Cmb_Tipo_Colonia.SelectedIndex = 0;

    }//fin de limpiar formulario

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Negocio
    ///DESCRIPCIÓN: Metodo que asigna los valores de textbox a la clase cat_ate_colonias_negocio
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Datos_Negocio(bool Estado)
    {
        if (Estado == true)
        {
            Colonia_Negocio.P_Colonia_Id = txt_Colonia_ID.Text;
            Colonia_Negocio.P_Nombre = txt_Nombre.Text;
            Colonia_Negocio.P_Descripcion = txt_Descripcion.Text;
            Colonia_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Colonia_Negocio.P_Tipo_Colonia = Cmb_Tipo_Colonia.SelectedItem.Value;
        }
        else
        {
            Colonia_Negocio.P_Colonia_Id = null;
            Colonia_Negocio.P_Nombre = null;
            Colonia_Negocio.P_Descripcion = null;
            Colonia_Negocio.P_Tipo_Colonia = null;
        }   
    }//fin de cargar_datos_negocio

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: metodo que muestra los botones de acuerdo al estado en el que se encuentre
    ///PARAMETROS:   1.- String Estado: El estado de los botones solo puede tomar 
    ///                 + inicial
    ///                 + nuevo
    ///                 + modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Botones(String Estado)
    {

        switch (Estado){
            case "inicial":
                //Boton Nuevo
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.Enabled = false;
                Btn_Modificar.Visible = true;
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Eliminar
                Btn_Eliminar.Enabled = false;
                Btn_Eliminar.Visible = true; 
                //Boton Salir
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                break;
            case "nuevo":
                //Boton Nuevo
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
                //Boton Eliminar
                Btn_Eliminar.Visible = false;
                //Boton Salir
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                break;
            case "modificar":
                //Boton Nuevo
                Btn_Nuevo.Visible = false;
                //Boton Modificar
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.Enabled = true;
                Btn_Modificar.Visible = true;
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Eliminar
                Btn_Eliminar.Visible = false;
                //Boton Salir
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                break;
                
        }//fin del switch
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Colonias
    ///DESCRIPCIÓN: Metodo que llena el GridView
    ///PARAMETROS: GridView que se llenara
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************            
    public void Llenar_Grid_Colonias()
    {
        DataSet Data_Set = new DataSet();
        Data_Set = Colonia_Negocio.Consulta_Datos();
        if (Data_Set != null)
        {
            //Grid_Colonias.Columns[4].Visible = true;
            Grid_Colonias.DataSource = Data_Set;
            Grid_Colonias.DataBind();
            //Grid_Colonias.Columns[4].Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Tipos_Colonias
    ///DESCRIPCIÓN          : Llena el Combo de Tipos de Colonias con los existentes en la Base de Datos.
    ///PROPIEDADES          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO           : 04/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Combo_Tipos_Colonias(){
        if (Colonia_Negocio == null) { Colonia_Negocio = new Cls_Cat_Ate_Colonias_Negocio(); }
        DataTable Tipos_Colonias = Colonia_Negocio.Consulta_Tipos_Colonias();
        DataRow Fila_Tipo_Colonia = Tipos_Colonias.NewRow();
        Fila_Tipo_Colonia["TIPO_COLONIA_ID"] = HttpUtility.HtmlDecode("00000");
        Fila_Tipo_Colonia["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
        Tipos_Colonias.Rows.InsertAt(Fila_Tipo_Colonia, 0);
        Cmb_Tipo_Colonia.DataSource = Tipos_Colonias;
        Cmb_Tipo_Colonia.DataValueField = "TIPO_COLONIA_ID";
        Cmb_Tipo_Colonia.DataTextField = "DESCRIPCION";
        Cmb_Tipo_Colonia.DataBind();
    }

    #endregion

    #region Grid
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colonias_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar los datos del elemento seleccionado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 25/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Grid_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Colonias.SelectedIndex > (-1)) {
            Limpiar_Formulario();
            //GridViewRow representa una fila individual de un control gridview
            GridViewRow selectedRow = Grid_Colonias.Rows[Grid_Colonias.SelectedIndex];

            String colonia_seleccionada = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            String colonia_nombre = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            String colonia_descripcion = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
            String tipo_colonia = HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString();
            txt_Colonia_ID.Text = colonia_seleccionada;
            txt_Nombre.Text = colonia_nombre;
            txt_Descripcion.Text = colonia_descripcion;
            Cmb_Tipo_Colonia.SelectedIndex = Cmb_Tipo_Colonia.Items.IndexOf(Cmb_Tipo_Colonia.Items.FindByValue(tipo_colonia));
            //Validaciones de los botones
            Estado_Botones("inicial");
            Btn_Eliminar.Enabled = true;
            Btn_Modificar.Enabled = true;
            Habilitar_Cajas(false);
            Cargar_Datos_Negocio(false);
            Llenar_Grid_Colonias();
            System.Threading.Thread.Sleep(1000);           
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colonias_PageIndexChanging
    ///DESCRIPCIÓN: Metodo para manejar la paginacion del Grid_Colonias
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 25/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Grid_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Colonias.PageIndex = e.NewPageIndex;
        Grid_Colonias.DataBind();

    }
    #endregion

    #region Eventos


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Boton que tiene la funcion de insertar un elemento en la tabla cat_ate_colonias
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        //Validacion para crear un nuevo registro y para habilitar los controles que se requieran
        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Estado_Botones("nuevo");
                Limpiar_Formulario();
                Habilitar_Cajas(true);
                break;
            case "Dar de Alta":
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                if (((txt_Nombre.Text.Length < 100) && (txt_Nombre.Text.Length > 0)))
                {
                    if ((txt_Descripcion.Text.Length < 100) && (txt_Descripcion.Text.Length > 0))
                    {
                        Estado_Botones("inicial");
                        //Generamos el ID de la colonia
                        txt_Colonia_ID.Text = Colonia_Negocio.Generar_ID();
                        //cargamos los datos a la clase de negocio
                        Cargar_Datos_Negocio(true);
                        Colonia_Negocio.Alta_Colonia();
                        //Registramos la accion en la bitacora
                        Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Cat_Ate_Colonias.aspx",Colonia_Negocio.P_Nombre, "");
                        Cargar_Datos_Negocio(false);
                        Llenar_Grid_Colonias();
                        Habilitar_Cajas(false);
                    }
                }

                if ((txt_Nombre.Text == "") | (txt_Descripcion.Text == "") | (Cmb_Tipo_Colonia.SelectedIndex == 0))
                {
                    if ((txt_Nombre.Text == ""))
                    {
                        Lbl_Mensaje_Error.Text += "+ El nombre es un campo obligatorio <br />";
                    }
                    if (txt_Descripcion.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "+ La descripcion es un campo obligatorio <br />";
                    }
                    if (Cmb_Tipo_Colonia.SelectedIndex == 0) {
                        Lbl_Mensaje_Error.Text += "+ Es necesario elegir un Tipo de Colonia del Combo <br />";
                    }
                    //Se hace visible el mensaje de error
                    Div_Contenedor_Msj_Error.Visible = true;
                }

                if ((txt_Nombre.Text.Length > 100) | (txt_Descripcion.Text.Length > 100))
                {

                    if (txt_Nombre.Text.Length > 100)
                    {
                        Lbl_Mensaje_Error.Text += "+ El Nombre sobrepasa el numero de caracteres <br />";
                    }

                    if (txt_Descripcion.Text.Length > 100)
                    {
                        Lbl_Mensaje_Error.Text += "+ La Descripcion sobrepasa el numero de caracteres <br />";
                    }
                    //Se hace visible el mensaje de error
                    Div_Contenedor_Msj_Error.Visible = true;
                }


                break;
        }//fin del swirch

    }//fin del boton Nuevo


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton salir 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        switch (Btn_Salir.ToolTip)
        {
            case "Cancelar":
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                Estado_Botones("inicial");
                Limpiar_Formulario();
                txt_Colonia_ID.Text = "";
                Habilitar_Cajas(false);
                Llenar_Grid_Colonias();
                break;

            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
        }//fin del switch
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del boton de modificar
    ///PARAMETROS:    
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {

        switch (Btn_Modificar.ToolTip)
        {
            //Validacion para actualizar un registro y para habilitar los controles que se requieran
            case "Modificar":

                Estado_Botones("modificar");
                Habilitar_Cajas(true);
                Llenar_Grid_Colonias();
                break;
            case "Actualizar":
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                if (((txt_Nombre.Text.Length < 100) && (txt_Nombre.Text.Length > 0)))
                {
                    if ((txt_Descripcion.Text.Length < 100) && (txt_Descripcion.Text.Length > 0))
                    {
                        Cargar_Datos_Negocio(true);
                        //Obtengo el dataset antes de modificar
                        DataSet Datos_No_Modificados = Colonia_Negocio.Consulta_Datos();
                        //Modifico la colonia
                        Colonia_Negocio.Modificar_Colonia();
                        //Obtengo otro dataset con los datos ya modificados 
                        DataSet Datos_Modificados = Colonia_Negocio.Consulta_Datos();
                        //Genero la descripcion de las modificaciones realizadas 
                        String Descripcion_Bitacora = Cls_Bitacora.Revisar_Actualizaciones(Datos_No_Modificados, Datos_Modificados);
                        Habilitar_Cajas(false);
                        Estado_Botones("inicial");
                        //Registro la accion de modificar en la bitacora 
                        Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Cat_Ate_Colonias.aspx", Colonia_Negocio.P_Nombre, Descripcion_Bitacora);
                        Cargar_Datos_Negocio(false);
                        Llenar_Grid_Colonias();
                    }
                }
                //Se valida que los campos nombre y descripcion no esten vacios 
                if ((txt_Nombre.Text == "") | (txt_Descripcion.Text == "") | (Cmb_Tipo_Colonia.SelectedIndex == 0))
                {
                    if ((txt_Nombre.Text == ""))
                    {
                        Lbl_Mensaje_Error.Text += "+ El nombre es un campo obligatorio <br />";
                    }
                    if (txt_Descripcion.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "+ La descripcion es un campo obligatorio <br />";
                    }
                    if (Cmb_Tipo_Colonia.SelectedIndex == 0)
                    {
                        Lbl_Mensaje_Error.Text += "+ Es necesario elegir un Tipo de Colonia del Combo <br />";
                    }
                    //Se hace visible el mensaje de error
                    Div_Contenedor_Msj_Error.Visible = true;
                }

                if ((txt_Nombre.Text.Length > 100) | (txt_Descripcion.Text.Length > 100))
                {

                    if (txt_Nombre.Text.Length > 100)
                    {
                        Lbl_Mensaje_Error.Text += "+ El Nombre sobrepasa el numero de caracteres <br />";
                    }

                    if (txt_Descripcion.Text.Length > 100)
                    {
                        Lbl_Mensaje_Error.Text += "+ La Descripcion sobrepasa el numero de caracteres <br />";
                    }
                    //Se hace visible el mensaje de error
                    Div_Contenedor_Msj_Error.Visible = true;
                }

                break;

        }//fin del switch
    }//fin de Modificar

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Evento del boton Eliminar 
    ///PARAMETROS:    
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        Cargar_Datos_Negocio(true);
        Colonia_Negocio.Eliminar_Colonia();
        Estado_Botones("inicial");
        Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Baja, "Frm_Cat_Ate_Colonias.aspx", Colonia_Negocio.P_Nombre, "");
        Cargar_Datos_Negocio(false);
        Llenar_Grid_Colonias();
        Habilitar_Cajas(false);
        Limpiar_Formulario();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_colonia_Click
    ///DESCRIPCIÓN: Evento del boton Buscar 
    ///PARAMETROS:    
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_colonia_Click(object sender, ImageClickEventArgs e)
    {
        Cargar_Datos_Negocio(false);
        Colonia_Negocio.P_Nombre = Txt_Busqueda_colonias.Text;
        Llenar_Grid_Colonias();
        Estado_Botones("inicial");
        Habilitar_Cajas(false);
        Limpiar_Formulario();
    }
    #endregion

}