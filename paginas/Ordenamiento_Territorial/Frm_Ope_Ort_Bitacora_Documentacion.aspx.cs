using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using System.Drawing;
using System.Drawing.Drawing2D;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Ordenamiento_Territorial_Areas_Publicas.Negocio;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Bitacora_Documentacion : System.Web.UI.Page
{
    #region Load
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializar_Controles();
                ViewState["SortDirection"] = "DESC";
            }
            string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Solicitud_Tramite.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Solicitud.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Documentos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Documento.Attributes.Add("onclick", Ventana_Modal);
            
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Metodos generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles("Inicial");
            Cargar_Combo_Documentos();
            Cargar_Combo_Solicitud();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Hdf_Elemento_ID.Value = "";

            Grid_Bitacoras.DataSource = new DataTable();
            Grid_Bitacoras.DataBind();

            Session.Remove("Documentos");
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE:         Habilitar_Controles
    /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
    ///                 para a siguiente operación
    /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    //Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    //Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    //Btn_Eliminar.Visible = true;
                    //Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    //Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    //Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    //Btn_Eliminar.Visible = false;
                    //Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    //Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    //Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    //Btn_Eliminar.Visible = false;
                    //Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            //  mensajes de error
            Mostrar_Mensaje_Error(false);

            Cmb_Documentacion.Enabled = Habilitado;
            Cmb_Solicitud.Enabled = false;
            Btn_Buscar_Documento.Enabled = Habilitado;
            Btn_Agregar_Documento.Enabled = Habilitado;
            Cmb_Estatus.Enabled = !Habilitado;
            Grid_Bitacoras.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
    ///DESCRIPCIÓN          : se habilitan los mensajes de error
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Mensaje_Error(Boolean Accion)
    {
        try
        {
            Img_Error.Visible = Accion;
            Lbl_Mensaje_Error.Visible = Accion;
        }
        catch (Exception ex)
        {
            throw new Exception("Mostrar_Mensaje_Error " + ex.Message.ToString());
        }
    }
    #endregion

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Mostrar_Mensaje_Error(true);

        if (Cmb_Solicitud.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la solcitidud de tramite.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Documentacion.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el documento.<br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Modificar
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Modificar()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Mostrar_Mensaje_Error(true);

        if (Grid_Bitacoras.Rows.Count == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el documento.<br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Solicitud
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Combo_Solicitud()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Solicitudes();
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Solicitud.DataSource = Dt_Consulta;
                Cmb_Solicitud.DataValueField = "SOLICITUD_ID";
                Cmb_Solicitud.DataTextField = "CLAVE_SOLICITUD";
                Cmb_Solicitud.DataBind();
                Cmb_Solicitud.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {
                Cmb_Solicitud.DataSource = new DataTable();
                Cmb_Solicitud.DataBind();

                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "No se encuentran solicitudes de tramites";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Documentos
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Combo_Documentos()
    {
        Cls_Cat_Documentos_Negocio Negocio_Consulta = new Cls_Cat_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consulta.Consultar_Todo().Tables[0];

            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Documentacion.DataSource = Dt_Consulta;
                Cmb_Documentacion.DataValueField = "DOCUMENTO_ID";
                Cmb_Documentacion.DataTextField = "NOMBRE";
                Cmb_Documentacion.DataBind();
                Cmb_Documentacion.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {
                Cmb_Documentacion.DataSource = new DataTable();
                Cmb_Documentacion.DataBind();

                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "No se encuentran documentos";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Bicatora
    ///DESCRIPCIÓN          : se cargara el grid
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Grid_Bicatora(DataTable Dt_Consulta)
    {
        try
        {
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Bitacoras.Columns[0].Visible = true;
                Grid_Bitacoras.Columns[1].Visible = true;
                Grid_Bitacoras.Columns[2].Visible = true;
                Grid_Bitacoras.Columns[3].Visible = true;
                Grid_Bitacoras.Columns[9].Visible = true;
                Grid_Bitacoras.DataSource = Dt_Consulta;
                Grid_Bitacoras.DataBind();
                Grid_Bitacoras.Columns[0].Visible = false;
                Grid_Bitacoras.Columns[1].Visible = false;
                Grid_Bitacoras.Columns[2].Visible = false;
                Grid_Bitacoras.Columns[3].Visible = false;
                Grid_Bitacoras.Columns[9].Visible = false;
                Session["Documentos"] = Dt_Consulta;
            }
            else
            {
                Grid_Bitacoras.Columns[1].Visible = true;
                Grid_Bitacoras.Columns[2].Visible = true;
                Grid_Bitacoras.Columns[3].Visible = true;
                Grid_Bitacoras.DataSource = new DataTable();
                Grid_Bitacoras.DataBind();
                Grid_Bitacoras.Columns[0].Visible = false;
                Grid_Bitacoras.Columns[1].Visible = false;
                Grid_Bitacoras.Columns[2].Visible = false;
                Grid_Bitacoras.Columns[3].Visible = false;
                Session.Remove("Documentos");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Modificar()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Modificar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Documentos = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false);
            Dt_Documentos = (DataTable)(Session["Documentos"]);
            Negocio_Modificar.P_Dt_Bitacora = Dt_Documentos;
            Negocio_Modificar.P_Estatus = Check_Box_Seleccionados(Grid_Bitacoras, "Chck_Activar");
            Negocio_Modificar.Alta();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Numero_Checks
    ///DESCRIPCIÓN: Metodo que cuenta el numero de checks seleccionados dentro del GridView 
    ///PROPIEDADES:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public int Numero_Checks(GridView MyGrid, String nombre_check)
    {
        int Numero_Seleccionados = 0;
        //Obtenemos el numero de Checkbox seleccionados
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(nombre_check)).Checked;

            if (isChecked)
            {
                //Variable auxiliar para contar el numero de check seleccionados. 
                Numero_Seleccionados = Numero_Seleccionados + 1;
            }
        }//fin del for i

        return Numero_Seleccionados;
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Check_Box_Seleccionados
    ///DESCRIPCIÓN: Metodo que debuelve un string con los catalogos seleccionados
    ///PARAMETROS:    1.- GridView que se recorre
    ///               2.- nombre_check del cual se evalua el estado Checked
    ///               3.- Nombre_ope nombre de la operacion ya sea (Catalogo u operaciones)
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String[] Check_Box_Seleccionados(GridView MyGrid, String nombre_check)
    {

        //Variable que guarda el nombre del catalogo seleccionado. Ejem: (Frm_Cat_Ate_Colonias)
        String Check_seleccionado = "";
        //auxiliar para contar el numero de check seleccionados dentro del grid. 
        int num = 0;
        //Arreglo donde se almacenaran los catalogos seleccionados 
        String[] Array_aux = new String[Numero_Checks(MyGrid, nombre_check)]; ;
        String Seleccionados = "";
        try
        {
            Array_aux = new String[MyGrid.Rows.Count];
            //Obtenemos el numero de Checkbox seleccionados
            for (int i = 0; i < MyGrid.Rows.Count; i++)
            {
                GridViewRow row = MyGrid.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(nombre_check)).Checked;

                if (isChecked)
                {
                    //llenamos el arreglo con los nombres de los tramites
                    Array_aux[num] = "ENTREGADO";
                    num = num + 1;
                }
                else
                {
                    //llenamos el arreglo con los nombres de los tramites
                    Array_aux[num] = "FALTANTE";
                    num = num + 1;
                }
            }

            //Generamos la cadena con los Tramites seleccionados para generar la consulta de oracle y 
            Seleccionados = Generar_Cadena(Array_aux, num);

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return Array_aux;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Cadena
    ///DESCRIPCIÓN: Metodo que genera una cadena a partir de un arreglo 
    ///PARAMETROS: 1.- String []Arreglo: Arreglo en el que a el listado de los catalogos seleccionados 
    ///            2.- String []Pagina: arreglo con los titulos del catalogo seleccionados
    ///            3.- int Longitud: Numero de check seleccionados 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String Generar_Cadena(String[] Arreglo, int Longitud)
    {
        //Variable que almacenara los catalogos seleccionados 
        String Cadena = "";
        try
        {
            for (int y = 0; y < Longitud; y++)
            {
                if (y == 0)
                {
                    Cadena += "'" + Arreglo[y] + "'";
                }
                else
                {
                    Cadena += ",'" + Arreglo[y] + "'";
                }

            }//fin del for y
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return Cadena;
    }//fin de generar cadena

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Buscar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Buscar()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Buscar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Negocio_Buscar.P_Estatus_Busqueda = Cmb_Estatus.SelectedValue;
            Negocio_Buscar.P_Solicitud_ID = Cmb_Solicitud.SelectedValue;
            Dt_Consulta = Negocio_Buscar.Consultar_Bitacora();
            Session["Sesion_Solicitud"] = Dt_Consulta;
            Cargar_Grid_Bicatora(Dt_Consulta);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Botones
    
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : realiza la modificacion del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Modificar.ToolTip == "Modificar")
            {
                Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos_Modificar())
                {
                    Modificar();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Agrega un nuevo Documento a la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera.
    ///FECHA_CREO:  26/Junio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Documento = new DataTable();
        DataTable Dt_Consultar = new DataTable();
        String Subproceso_ID = "";
        String Clave_Solicitud = "";
        String Nombre_Actividad = "";
        Boolean Estado = false;
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Validar_Datos())
            {
                Mostrar_Mensaje_Error(false);
                if (Session["Documentos"] == null)
                {
                    Dt_Documento = new DataTable("Documentos");
                    Dt_Documento.Columns.Add("BITACORA_ID", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("SOLICITUD_ID", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("SUBPROCESO_ID", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("DOCUMENTO_ID", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("CLAVE_SOLICITUD", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("NOMBRE_ACTIVIDAD", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("NOMBRE_DOCUMENTO", Type.GetType("System.String"));
                    Dt_Documento.Columns.Add("FECHA_ENTREGA_DOC", typeof(System.DateTime));
                    Dt_Documento.Columns.Add("ESTATUS", Type.GetType("System.String"));
                }
                else
                {
                    Dt_Documento = (DataTable)Session["Documentos"];
                }

                //  para saber si se encuentra dentro del grid
                if (Dt_Documento.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Documento.Rows)
                    {
                        if ((Cmb_Documentacion.SelectedValue == Registro["DOCUMENTO_ID"].ToString()) &&
                            (Cmb_Solicitud.SelectedValue == Registro["SOLICITUD_ID"].ToString()))
                        {
                            Estado = true;
                            break;
                        }
                    }
                }
                Negocio_Consultar.P_Solicitud_ID = Cmb_Solicitud.SelectedItem.Value;
                Negocio_Consultar.P_Documento_ID = Cmb_Documentacion.SelectedValue;
                Dt_Consultar = Negocio_Consultar.Consultar_Documentos_Repetidos();
                if (Dt_Consultar != null && Dt_Consultar.Rows.Count > 0)
                {
                    Estado = true;
                }

                //Consultar_Documentos_Repetidos

                if (Estado == false)
                {
                    DataRow Fila = Dt_Documento.NewRow();

                    Fila["BITACORA_ID"] = "";
                    Fila["SOLICITUD_ID"] = HttpUtility.HtmlDecode(Cmb_Solicitud.SelectedItem.Value);

                    Negocio_Consultar.P_Solicitud_ID = Cmb_Solicitud.SelectedItem.Value;
                    Dt_Consultar = Negocio_Consultar.Consultar_Solicitudes();
                    //  para el subproceso
                    if (Dt_Consultar != null && Dt_Consultar.Rows.Count > 0)
                    {
                        Subproceso_ID = Dt_Consultar.Rows[0][Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString();
                        Clave_Solicitud = Dt_Consultar.Rows[0][Ope_Tra_Solicitud.Campo_Clave_Solicitud].ToString();
                        Nombre_Actividad = Dt_Consultar.Rows[0]["NOMBRE_ACTIVIDAD"].ToString();
                    }
                    Fila["SUBPROCESO_ID"] = Subproceso_ID;
                    Fila["DOCUMENTO_ID"] = HttpUtility.HtmlDecode(Cmb_Documentacion.SelectedValue);
                    Fila["CLAVE_SOLICITUD"] = Clave_Solicitud;
                    Fila["NOMBRE_ACTIVIDAD"] = Nombre_Actividad;
                    Fila["NOMBRE_DOCUMENTO"] = HttpUtility.HtmlDecode(Cmb_Documentacion.SelectedItem.ToString());
                    Fila["FECHA_ENTREGA_DOC"] = DateTime.Now;
                    Fila["ESTATUS"] = "";
                    Dt_Documento.Rows.Add(Fila);
                    Session["Documentos"] = Dt_Documento;

                    Cargar_Grid_Bicatora(Dt_Documento);
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                    Lbl_Mensaje_Error.Text = "El Documento ya se encuentra seleccionado";
                }
            }
            else 
            {
                Mostrar_Mensaje_Error(true);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Cancelar")
            {
                Inicializar_Controles();
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);
            if (Cmb_Solicitud.SelectedIndex != 0)
            {
                Buscar();
            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccion la solicitud a buscar";
                //Cargar_Grid_Bicatora();
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString(); ;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Solicitud_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Solicitud_Click(object sender, EventArgs e)
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Buscar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Tramite = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false);
            if (Session["BUSQUEDA_SOLICITUD"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_SOLICITUD"].ToString());

                if (Estado != false)
                {
                    String Solicitud_id = Session["SOLICITUD_ID"].ToString();
                    Negocio_Buscar.P_Solicitud_ID = Solicitud_id;
                    Dt_Consulta = Negocio_Buscar.Consultar_Bitacora();

                    if (Cmb_Solicitud.Items.FindByValue(Solicitud_id) != null)
                    {
                        Cmb_Solicitud.SelectedValue = Solicitud_id;
                    }
                    Session["Sesion_Solicitud"] = Dt_Consulta;
                    Cargar_Grid_Bicatora(Dt_Consulta);

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Documento_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Documento_Click(object sender, EventArgs e)
    {
        
        DataTable Dt_Tramite = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Session["BUSQUEDA_DOCUMENTO"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_DOCUMENTO"].ToString());

                if (Estado != false)
                {
                    String Documento_id = Session["DOCUMENTO_ID"].ToString();

                    if (Cmb_Documentacion.Items.FindByValue(Documento_id) != null)
                    {
                        Cmb_Documentacion.SelectedValue = Documento_id;
                    }  

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion

    #region Grid
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Bitacoras_RowDataBound
    ///DESCRIPCIÓN          : cargara los botones
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 08/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Bitacoras_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        String Estatus = "";
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.CheckBox Chk_Estatus = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Chck_Activar");

                Estatus = e.Row.Cells[9].Text.Trim();

                if (Estatus != "")
                {
                    if (Estatus == "ENTREGADO")
                    {
                        Chk_Estatus.Checked = true;
                        Chk_Estatus.Enabled = false;
                    }
                    else if (Estatus == "FALTANTE")
                    {
                        Chk_Estatus.Checked = false;
                        Chk_Estatus.Enabled = true;
                    }
                }               
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Bitacoras_Sorting
    ///DESCRIPCIÓN          : ordena las columnas
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 29/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Bitacoras_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable Dt_Consulta = (DataTable)(Session["Sesion_Solicitud"]);

        DataView Dv_Ordenar = new DataView(Dt_Consulta);
        String Orden = ViewState["SortDirection"].ToString();

        if (Orden.Equals("ASC"))
        {
            Dv_Ordenar.Sort = e.SortExpression + " " + "DESC";
            ViewState["SortDirection"] = "DESC";
        }
        else
        {
            Dv_Ordenar.Sort = e.SortExpression + " " + "ASC";
            ViewState["SortDirection"] = "ASC";
        }
        Grid_Bitacoras.Columns[1].Visible = true;
        Grid_Bitacoras.DataSource = Dv_Ordenar;
        Grid_Bitacoras.DataBind();
        Grid_Bitacoras.Columns[1].Visible = false;
    }
    #endregion

    #region Combos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Solicitud_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga la cuenta contable id de la cuenta de ingresos seleccionada
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Solicitud_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
        
    #endregion
}
