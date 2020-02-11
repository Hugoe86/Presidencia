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

public partial class paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Bitacora_Prestamo_Expediente : System.Web.UI.Page
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
            }
            string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Solicitud_Tramite.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Solicitud.Attributes.Add("onclick", Ventana_Modal);
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
            Cargar_Combo_Solicitud();
            Cargar_Combo_Nombres();
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
            Hdf_Accion.Value = "";

            Grid_Bitacoras.DataSource = new DataTable();
            Grid_Bitacoras.DataBind();

            Txt_Ubicacion.Text = "";
            Txt_Solicitud.Text = "";
            Txt_Obsevaciones.Text = "";
            //Txt_Nombre_Persona.Text = "";
            Txt_Nombre_Documento.Text = "";
            Txt_Fecha_Prestamo.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
            Txt_Fecha_Devolucion.Text = "";

            RbtL_Satisfaccion.SelectedIndex = -1;
            Txt_Tiempo_Espera.Text = "";
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
    public void Habilitar_Controles(String Operacion)
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
                    Btn_Modificar.Visible = false;
                    //Btn_Eliminar.Visible = true;
                    //Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Div_Solicitud.Style.Value = "display:block";
                    Pnl_Prestamo.Style.Value = "display:none";
                    Pnl_Encuesta.Style.Value = "display:none";

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
                    Div_Solicitud.Style.Value = "display:none";
                    Pnl_Prestamo.Style.Value = "display:block";
                    Pnl_Encuesta.Style.Value = "display:block";
                    break;
            }
            //  mensajes de error
            Mostrar_Mensaje_Error(false);

            Cmb_Solicitud.Enabled = true;         
            Grid_Bitacoras.Enabled = true;
            Txt_Solicitud.Enabled = false;
            Txt_Nombre_Documento.Enabled = false;
            Txt_Fecha_Prestamo.Enabled = false;
            Txt_Fecha_Devolucion.Enabled = false;

            RbtL_Satisfaccion.Enabled = true;
            Txt_Tiempo_Espera.Enabled = true;
            Btn_Fecha.Enabled = false;

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


        if (Txt_Fecha_Prestamo.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha del prestamo.<br>";
            Datos_Validos = false;
        }
        if (Txt_Fecha_Devolucion.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha del devolucion.<br>";
            Datos_Validos = false;
        }

        DateTime Fecha = (DateTime.Now);
        if (Txt_Fecha_Devolucion.Text != "")
        {
            if (Convert.ToDateTime(Txt_Fecha_Devolucion.Text) < Fecha)
            {
                if ((Txt_Fecha_Devolucion.Text) == String.Format("{0:dd/MMM/yyyy}", Fecha))
                {
                }
                else
                {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La Fecha de devolucion debe ser mayor que la fecha del prestamo.<br>";
                    Datos_Validos = false;
                }
            }
        }

        //if (Txt_Nombre_Persona.Text == "")
        //{
        //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el nombre de la persona a la que se le presta el expediente.<br>";
        //    Datos_Validos = false;
        //}
        if (Cmb_Nombre.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*seleccione el nombre de la persona a la que se le presta el expediente.<br>";
            Datos_Validos = false;
        }
        if (Txt_Ubicacion.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la ubicacion del expediente.<br>";
            Datos_Validos = false;
        }
        if (Txt_Obsevaciones.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese las observaciones.<br>";
            Datos_Validos = false;
        }
        //  para la encuesta ********************************************************************************************
        if (RbtL_Satisfaccion.SelectedIndex == -1)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la primera pregunta de la encuesta .<br>";
            Datos_Validos = false;
        }
        if (Txt_Tiempo_Espera.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la segunda pregunta de la encuesta .<br>";
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
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Nombres
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Combo_Nombres()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            //Negocio_Consultar_Solicitud.P_Nombre_Usuario = "ordenamiento territorial";
            Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Personal();
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Nombre.DataSource = Dt_Consulta;
                Cmb_Nombre.DataValueField = "Nombre_Usuario";
                Cmb_Nombre.DataTextField = "Nombre_Usuario";
                Cmb_Nombre.DataBind();
                Cmb_Nombre.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {
                Cmb_Nombre.DataSource = new DataTable();
                Cmb_Nombre.DataBind();

                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "No se encuentran personal";
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
                int Contador = 0;
                Session["Contador"] = Contador;

                Grid_Bitacoras.Columns[1].Visible = true;
                Grid_Bitacoras.Columns[2].Visible = true;
                Grid_Bitacoras.Columns[3].Visible = true;
                Grid_Bitacoras.Columns[4].Visible = true;
                Grid_Bitacoras.Columns[9].Visible = true;
                Grid_Bitacoras.DataSource = Dt_Consulta;
                Grid_Bitacoras.DataBind();
                Grid_Bitacoras.Columns[1].Visible = false;
                Grid_Bitacoras.Columns[2].Visible = false;
                Grid_Bitacoras.Columns[3].Visible = false;
                Grid_Bitacoras.Columns[4].Visible = false;
                Grid_Bitacoras.Columns[9].Visible = false;
                Session["Documentos"] = Dt_Consulta;
                Grid_Bitacoras.SelectedIndex = -1;

               

            }
            else
            {
                Grid_Bitacoras.Columns[1].Visible = true;
                Grid_Bitacoras.Columns[2].Visible = true;
                Grid_Bitacoras.Columns[3].Visible = true;
                Grid_Bitacoras.Columns[4].Visible = true;
                Grid_Bitacoras.Columns[9].Visible = true;
                Grid_Bitacoras.DataSource = new DataTable();
                Grid_Bitacoras.DataBind();
                Grid_Bitacoras.Columns[1].Visible = false;
                Grid_Bitacoras.Columns[2].Visible = false;
                Grid_Bitacoras.Columns[3].Visible = false;
                Grid_Bitacoras.Columns[4].Visible = false;
                Grid_Bitacoras.Columns[9].Visible = false;
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
    private String Modificar()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Modificar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Documentos = new DataTable();
        String Bitacora_id = "";
        try
        {
            Mostrar_Mensaje_Error(false);
            Negocio_Modificar.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Negocio_Modificar.P_Documento_ID= Hdf_Documento_ID.Value;
            Negocio_Modificar.P_Dtime_Fecha_Prestamo = Convert.ToDateTime(Txt_Fecha_Prestamo.Text);
            Negocio_Modificar.P_Dtime_Fecha_Devolucion = Convert.ToDateTime(Txt_Fecha_Devolucion.Text);
            Negocio_Modificar.P_Usuario =Cmb_Nombre.SelectedValue;
            Negocio_Modificar.P_Observaciones = Txt_Obsevaciones.Text;
            Negocio_Modificar.P_Ubicacion = Txt_Ubicacion.Text;
            Negocio_Modificar.P_Estatus_Prestamo = "PRESTADO";
            Negocio_Modificar.P_Pregunta_Satisfaccion = RbtL_Satisfaccion.SelectedValue;
            Negocio_Modificar.P_Tiempo_Espera = Txt_Tiempo_Espera.Text;
            Bitacora_id = Negocio_Modificar.Alta_Detalle();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return Bitacora_id;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar_Estatus
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 28/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Modificar_Estatus()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Modificar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Documentos = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false);
            Negocio_Modificar.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Negocio_Modificar.P_Documento_ID = Hdf_Documento_ID.Value;
            Negocio_Modificar.P_Estatus_Prestamo = "DEVOLUCION";
            Negocio_Modificar.Modificar_Estatus_Prestamo();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_ID
    ///DESCRIPCIÓN          : obtendra el id de la bitacora
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 01/Agosto/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private String Obtener_ID()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Modificar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Documentos = new DataTable();
        String ID = "";
        try
        {
            Mostrar_Mensaje_Error(false);
            Negocio_Modificar.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Negocio_Modificar.P_Documento_ID = Hdf_Documento_ID.Value;
            ID = Negocio_Modificar.Consultar_Bitacora_Consecutivo_ID();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        return ID;
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
    ///NOMBRE DE LA FUNCIÓN : Modificar_Estatus
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 28/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Generar_Reporte_Tabla(String Detalle_Bitacora_ID)
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Informacion = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        //Ds_Rpt_Ort_Bitacora_Prestamo_Expediente Ds_Reporte = new Ds_Rpt_Ort_Bitacora_Prestamo_Expediente();
        DataSet Ds_Reporte = new DataSet();
        DataTable Dt_Informacion = new DataTable();
        try
        {

            //Dt_Informacion = Ds_Reporte.Dt_Bitacora_Expediente.Clone();

            Negocio_Informacion.P_Detalle_Bitacora_ID = Detalle_Bitacora_ID;
            Dt_Informacion = Negocio_Informacion.Consultar_Informacion_Reporte_Bitacora();

            Dt_Informacion.TableName = "Dt_Bitacora_Expediente";
            Ds_Reporte.Tables.Add(Dt_Informacion.Copy());

            Generar_Reporte(Ds_Reporte, "PDF", "Bitacora_Prestamos", "../Rpt/Ordenamiento_Territorial/Rpt_Ort_Bitacora_Prestamos.rpt");

        }
        catch (Exception ex)
        {
            throw new Exception("Generar_Reporte_Folio_Solicitud: " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: genera el reporte de pdf
    ///PARÁMETROS : 	
    ///         1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo, String Tipo, string Ruta_Archivo_Rpt)
    {
        String Nombre_Archivo = "Reporte_Bitacora_Prestamos" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        String Ruta_Archivo = @Server.MapPath(Ruta_Archivo_Rpt);//Obtiene la ruta en la cual será guardada el archivo
        ReportDocument Reporte = new ReportDocument();

        try
        {
            Reporte.Load(Ruta_Archivo);
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Nombre_Archivo += ".pdf";
            else if (Extension_Archivo == "EXCEL")
                Nombre_Archivo += ".xls";

            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            else if (Extension_Archivo == "EXCEL")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

            Reporte.Export(Opciones_Exportacion);

            if (Extension_Archivo == "PDF")
                Abrir_Ventana(Nombre_Archivo, Tipo);

            else if (Extension_Archivo == "EXCEL")
            {
                String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', '" + Tipo + "','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Tipo,
            "window.open('" + Pagina + "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
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
                    //  se modifica 
                    String Bitacora_ID = Modificar();

                    //  se genera el reporte
                    Generar_Reporte_Tabla(Bitacora_ID);

                    Limpiar_Controles();
                    Habilitar_Controles("Inicial");
                    Cmb_Solicitud_SelectedIndexChanged(sender, null);
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
    /// NOMBRE DE LA FUNCION: Btn_Imprimir_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, EventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);
            System.Web.UI.WebControls.ImageButton Boton_Imprimir = (System.Web.UI.WebControls.ImageButton)sender;
            
            Generar_Reporte_Tabla(Boton_Imprimir.ToolTip.ToString());

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Accion_Realizar_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Accion_Realizar_Click(object sender, EventArgs e)
    {
        String Bitacora_ID = "";
        try
        {
            System.Web.UI.WebControls.Button ImageButton = (System.Web.UI.WebControls.Button)sender;
            TableCell TableCell = (TableCell)ImageButton.Parent;
            GridViewRow Row = (GridViewRow)TableCell.Parent;
            Grid_Bitacoras.SelectedIndex = Row.RowIndex;
            int Fila = Row.RowIndex;

            Grid_Bitacoras.SelectedIndex = Fila;
            GridViewRow selectedRow = Grid_Bitacoras.Rows[Grid_Bitacoras.SelectedIndex];

            System.Web.UI.WebControls.Button Btn_Accion_Realizar = (System.Web.UI.WebControls.Button)Row.FindControl("Btn_Accion_Realizar");
            
            Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            Hdf_Documento_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[4].Text).ToString();

            if (Btn_Accion_Realizar.Text == "PRESTAMO")
            {
                Pnl_Prestamo.Style.Value = "display:block";
                Habilitar_Controles("Modificar");
                Txt_Solicitud.Text = HttpUtility.HtmlDecode(selectedRow.Cells[6].Text).ToString();
                Txt_Nombre_Documento.Text = HttpUtility.HtmlDecode(selectedRow.Cells[7].Text).ToString();
            }

            else
            {
                Modificar_Estatus();
                //Bitacora_ID = Obtener_ID();
                //Generar_Reporte_Tabla(Bitacora_ID);
                Limpiar_Controles();
                Habilitar_Controles("Inicial");
                Cmb_Solicitud_SelectedIndexChanged(sender, null);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Documento Devuelto');", true);
            }   
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
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
                    Negocio_Buscar.P_Estatus_Busqueda = "ENTREGADO";
                    Dt_Consulta = Negocio_Buscar.Consultar_Bitacora();

                    if (Cmb_Solicitud.Items.FindByValue(Solicitud_id) != null)
                    {
                        Cmb_Solicitud.SelectedValue = Solicitud_id;
                    }

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

    
    #endregion

    #region Grid

    
        ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Detalles_Bitacora_RowDataBound
    ///DESCRIPCIÓN          : cargara los botones
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 27/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Detalles_Bitacora_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        String Estatus = "";
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.ImageButton Boton_Imprimir = (System.Web.UI.WebControls.ImageButton)e.Row.FindControl("Btn_Imprimir");

                Boton_Imprimir.ToolTip = e.Row.Cells[0].Text.Trim();
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
       ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Detalles_Bitacora_RowCommand
    ///DESCRIPCIÓN          : cargara los botones
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 27/Junio/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Detalles_Bitacora_RowCommand(object sender, GridViewCommandEventArgs  e)
    {
        try
        {
           

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    


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
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        String Estatus = "";
        String Solicitud_Id = "";
        String Documento_Id = "";
        DateTime Fecha_Actual = DateTime.Today;
        DateTime Fecha_Devolucion = DateTime.Today; 
        TimeSpan Diferencia_Dias;
        int Contador;
        int Dias;
        GridView Gv_Detalles = new GridView();
        System.Web.UI.WebControls.Image Img = new System.Web.UI.WebControls.Image();
        Img = (System.Web.UI.WebControls.Image)e.Row.FindControl("Img_Btn_Expandir");
        Literal Lit = new Literal();
        Lit = (Literal)e.Row.FindControl("Ltr_Inicio");
        System.Web.UI.WebControls.Label Lbl_facturas = new System.Web.UI.WebControls.Label();
        Lbl_facturas = (System.Web.UI.WebControls.Label)e.Row.FindControl("Lbl_Actividades");
        Color Color = new Color();
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Contador = (int)(Session["Contador"]);
                Lit.Text = Lit.Text.Trim().Replace("Renglon_Grid", ("Renglon_Grid" + Lbl_facturas.Text));
                Img.Attributes.Add("OnClick", ("Mostrar_Tabla(\'Renglon_Grid"
                                + (Lbl_facturas.Text + ("\',\'"
                                + (Img.ClientID + "\')")))));

                System.Web.UI.WebControls.CheckBox Chk_Prestamo = (System.Web.UI.WebControls.CheckBox)e.Row.FindControl("Chck_Prestamo");
                System.Web.UI.WebControls.Button Btn_Accion_Realizar = (System.Web.UI.WebControls.Button)e.Row.FindControl("Btn_Accion_Realizar");

                Solicitud_Id = e.Row.Cells[2].Text.Trim();
                Documento_Id = e.Row.Cells[4].Text.Trim(); 
                Estatus = e.Row.Cells[9].Text.Trim();

                Negocio_Consultar_Solicitud.P_Solicitud_ID = Solicitud_Id;
                Negocio_Consultar_Solicitud.P_Documento_ID = Documento_Id;
                Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Documentos_Prestados_Devueltos();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Fecha_Devolucion = Convert.ToDateTime(Dt_Consulta.Rows[0][Ope_Ort_Det_Bitacora.Campo_Fecha_Devolucion].ToString());
                }

                Diferencia_Dias = (Fecha_Devolucion - Fecha_Actual);
                Dias = Diferencia_Dias.Days;

                if (Estatus != "")
                {
                    if (Estatus == "PRESTADO")
                    {
                        //  para cuando se debe de devolver
                        Chk_Prestamo.Checked = true;
                        Chk_Prestamo.Enabled = false;
                        Btn_Accion_Realizar.Text = "DEVOLUCION";

                        if (Dias == 0 || Dias > 0)
                        {
                            Btn_Accion_Realizar.Visible = true;
                            Btn_Accion_Realizar.BackColor = Color.Yellow;
                        }
                        else
                        {
                            Btn_Accion_Realizar.Visible = true;
                            Btn_Accion_Realizar.BackColor = Color.Tomato;
                        }
                        

                    }
                    else
                    {
                        Chk_Prestamo.Checked = false;
                        Chk_Prestamo.Enabled = false;
                        Btn_Accion_Realizar.Text = "PRESTAMO";
                        Btn_Accion_Realizar.Visible = true;
                        Btn_Accion_Realizar.BackColor = Color.LightGreen;
                    }
                }

                Session["Contador_Detalle"] = 0;
                Gv_Detalles = (GridView)e.Row.Cells[10].FindControl("Grid_Detalles_Bitacora");
                Gv_Detalles.Columns[0].Visible = true;
                Gv_Detalles.DataSource = Dt_Consulta;
                Gv_Detalles.DataBind();
                Gv_Detalles.Columns[0].Visible = false;
                Session["Contador"] = Contador + 1;

            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
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
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Buscar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Negocio_Buscar.P_Solicitud_ID = Cmb_Solicitud.SelectedValue;
            Negocio_Buscar.P_Estatus_Busqueda = "ENTREGADO";
            Dt_Consulta = Negocio_Buscar.Consultar_Bitacora();
            Cargar_Grid_Bicatora(Dt_Consulta);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion
}