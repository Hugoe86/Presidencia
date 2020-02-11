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
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using System.Text.RegularExpressions;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using System.Collections.Generic;
using Presidencia.Orden_Territorial_Administracion_Urbana.Negocio;
using Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Negocio;
using Presidencia.Catalogo_Calles.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Cat_Ort_Inspectores : System.Web.UI.Page
{
    #region Load
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
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
            string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Calle_Particular.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Colonia_Particular.Attributes.Add("onclick", Ventana_Modal);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
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
    /// FECHA_CREO  : 11/Junio/2012
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
            Cargar_Inspectores();

            Cargar_Combo_Colonias();
            Cargar_Combo_Calles(new DataTable());

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
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Hdf_Elemento_ID.Value = "";
            Txt_Busqueda.Text = "";
            Txt_Nombre.Text = "";

            Txt_Afiliado.Text = "";
            Txt_Cedula_Profesional.Text = "";
            Txt_Codigo_Postal.Text = "";
            Txt_Email.Text = "";
            Txt_Especialidad.Text = "";
            //Txt_Nextel.Text = "";
            Txt_Numero_Casa.Text = "";
            Txt_Numero_Oficina.Text = "";
            //Txt_Numero_Registro.Text = "";
            Txt_Telefono_Oficina.Text = "";
            Txt_Telefono_Particular.Text = "";
            Txt_Titulo.Text = "";
            Txt_Celular.Text = "";
            Txt_Comentarios.Text = "";

            Chk_Acreditacion.Checked = false;
            Chk_Cedula_Profesional.Checked = false;
            Chk_Constancia.Checked = false;
            Chk_Curriculum.Checked = false;
            Chk_Refrendo.Checked = false;
            Chk_Titulo_Profesional.Checked = false;
            Chk_Curso.Checked = false;
            Chk_Conformidad_Finaza.Checked = false;
    
            Cargar_Combo_Colonias();
            Cargar_Combo_Calles(new DataTable());
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE:         Habilitar_Controles
    /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
    ///                 para a siguiente operación
    /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     11/Junio/2012
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
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
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
                    break;
            }
            //  mensajes de error
            Mostrar_Mensaje_Error(false);

            Txt_Nombre.Enabled = Habilitado;
            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Inspector.Enabled = !Habilitado;

            Txt_Afiliado.Enabled = Habilitado;
            Txt_Cedula_Profesional.Enabled = Habilitado;
            Txt_Codigo_Postal.Enabled = Habilitado;
            Txt_Email.Enabled = Habilitado;
            Txt_Especialidad.Enabled = Habilitado;
            //Txt_Nextel.Enabled = Habilitado;
            Txt_Numero_Casa.Enabled = Habilitado;
            Txt_Numero_Oficina.Enabled = Habilitado;
            //Txt_Numero_Registro.Enabled = Habilitado;
            Txt_Telefono_Oficina.Enabled = Habilitado;
            Txt_Telefono_Particular.Enabled = Habilitado;
            Txt_Titulo.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Txt_Celular.Enabled = Habilitado;

            Chk_Acreditacion.Enabled = Habilitado;
            Chk_Cedula_Profesional.Enabled = Habilitado;
            Chk_Constancia.Enabled = Habilitado;
            Chk_Curriculum.Enabled = Habilitado;
            Chk_Refrendo.Enabled = Habilitado;
            Chk_Titulo_Profesional.Enabled = Habilitado;
            Chk_Conformidad_Finaza.Enabled = Habilitado;
            Chk_Curso.Enabled = Habilitado;

            Cmb_Calle.Enabled = Habilitado;
            Cmb_Calle_Particular.Enabled = Habilitado;
            Cmb_Colonias.Enabled = Habilitado;
            Cmb_Colonias_Particular.Enabled = Habilitado;
            Cmb_Tipo_Perito.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
    ///DESCRIPCIÓN          : se habilitan los mensajes de error
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mostrar_Mensaje_Error(Boolean Accion)
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
    /// FECHA_CREO  : 05/Junio/2012
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

        if (Txt_Nombre.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El nombre del inspector.<br>";
            Datos_Validos = false;
        }
        if (Txt_Celular.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero de telefono celular.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Tipo_Perito.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione el tipo de perito.<br>";
            Datos_Validos = false;
        }
        if (Txt_Cedula_Profesional.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El la cedula profesional.<br>";
            Datos_Validos = false;
        } 
        if (Txt_Titulo.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El titulo.<br>";
            Datos_Validos = false;
        }
        if (Txt_Afiliado.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La afiliacion que tiene.<br>";
            Datos_Validos = false;
        }

        if (Cmb_Colonias.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La colonia de la oficina.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Colonias_Particular.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La colonia de la vivienda particular.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Calle.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La calle de la oficina.<br>";
            Datos_Validos = false;
        }
        if (Cmb_Calle_Particular.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La calle de la vivienda particular.<br>";
            Datos_Validos = false;
        }

        if (Txt_Numero_Oficina.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El numero de la locacion de la oficina.<br>";
            Datos_Validos = false;
        }
        if (Txt_Numero_Casa.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El numero de la vivienda particular.<br>";
            Datos_Validos = false;
        }

        if (Txt_Telefono_Oficina.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El numero del telefono de la oficina.<br>";
            Datos_Validos = false;
        }
        if (Txt_Telefono_Particular.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El numero del telefono de la vivienda particular.<br>";
            Datos_Validos = false;
        }

        if (Txt_Email.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El email.<br>";
            Datos_Validos = false;
        }
        if (Txt_Codigo_Postal.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*El codigo postal de la vivienda particular.<br>";
            Datos_Validos = false;
        }
        if (Txt_Especialidad.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La especialidad de la persona.<br>";
            Datos_Validos = false;
        }
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Modificar
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
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

        if (Hdf_Elemento_ID.Value == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione algun registro.<br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Inspectores
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Inspectores()
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Consulta = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consulta.Consultar_Inspectores();
            Cargar_Grid_Inspectores(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Area " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Inspectores
    ///DESCRIPCIÓN          : se cargara el grid
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Grid_Inspectores(DataTable Dt_Consulta)
    {
        try
        {
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Inspector.Columns[1].Visible = true;
                Grid_Inspector.DataSource = Dt_Consulta;
                Grid_Inspector.DataBind();
                Grid_Inspector.Columns[1].Visible = false;
                Grid_Inspector.SelectedIndex = -1;
            }
            else
            {
                Grid_Inspector.Columns[1].Visible = true;
                Grid_Inspector.DataSource = new DataTable();
                Grid_Inspector.DataBind();
                Grid_Inspector.Columns[1].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Gridr_Area " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Colonias
    ///DESCRIPCIÓN: cargara la informacion de las calles y colonias
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  23/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Colonias()
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Colonias = new DataTable();
        try
        {
            //  para las colonias
            Dt_Colonias = Negocio_Consulta.Consultar_Colonia();

            Cmb_Colonias.DataSource = Dt_Colonias;
            Cmb_Colonias.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
            Cmb_Colonias.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
            Cmb_Colonias.DataBind();
            Cmb_Colonias.Items.Insert(0, "< SELECCIONE >");

            Cmb_Colonias_Particular.DataSource = Dt_Colonias;
            Cmb_Colonias_Particular.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
            Cmb_Colonias_Particular.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
            Cmb_Colonias_Particular.DataBind();
            Cmb_Colonias_Particular.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles
    ///DESCRIPCIÓN: cargara la informacion de las calles pertenecientes a una colonia
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Calles(DataTable Dt_Calles)
    {
        try
        {
            Cmb_Calle.DataSource = Dt_Calles;
            Cmb_Calle.DataValueField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle.DataTextField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle.DataBind();
            Cmb_Calle.Items.Insert(0, "< SELECCIONE >");

            Cmb_Calle_Particular.DataSource = Dt_Calles;
            Cmb_Calle_Particular.DataValueField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle_Particular.DataTextField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle_Particular.DataBind();
            Cmb_Calle_Particular.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Alta
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Alta()
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Alta = new Cls_Cat_Ort_Inspectores_Negocio();
        try
        {
            Negocio_Alta.P_Nombre = Txt_Nombre.Text;
            Negocio_Alta.P_Usuario = Cls_Sessiones.Nombre_Empleado;

            Negocio_Alta.P_Cedula_Profesional = Txt_Cedula_Profesional.Text;
            Negocio_Alta.P_Titulo = Txt_Titulo.Text;
            Negocio_Alta.P_Afiliado = Txt_Afiliado.Text;

            Negocio_Alta.P_Calle_Oficina = Cmb_Calle.SelectedItem.ToString();
            Negocio_Alta.P_Colonia_Oficina = Cmb_Colonias.SelectedItem.ToString();
            Negocio_Alta.P_Numero_Oficina = Txt_Numero_Oficina.Text;
            Negocio_Alta.P_Telefono_Oficina = Txt_Telefono_Oficina.Text;
            Negocio_Alta.P_Email = Txt_Email.Text;

            Negocio_Alta.P_Calle_Particular = Cmb_Calle_Particular.SelectedItem.ToString();
            Negocio_Alta.P_Colonia_Particular = Cmb_Colonias_Particular.SelectedItem.ToString();
            Negocio_Alta.P_Numero_Particular = Txt_Numero_Casa.Text;
            Negocio_Alta.P_Codigo_Postal = Txt_Codigo_Postal.Text;
            Negocio_Alta.P_Telefono_Particular = Txt_Telefono_Particular.Text;
            Negocio_Alta.P_Especialidad = Txt_Especialidad.Text;

            if (Chk_Titulo_Profesional.Checked == true)
            {
                Negocio_Alta.P_Documento_Titulo = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Titulo = "NO";
            }

            if (Chk_Cedula_Profesional.Checked == true)
            {
                Negocio_Alta.P_Documento_Cedula = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Cedula = "NO";
            }

            if (Chk_Curriculum.Checked == true)
            {
                Negocio_Alta.P_Documento_Curriculum = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Curriculum = "NO";
            }

            if (Chk_Constancia.Checked == true)
            {
                Negocio_Alta.P_Documento_Constancia = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Constancia = "NO";
            }

            if (Chk_Refrendo.Checked == true)
            {
                Negocio_Alta.P_Documento_Refrendo = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Refrendo = "NO";
            }
            if (Chk_Acreditacion.Checked == true)
            {
                Negocio_Alta.P_Documento_Especialidad = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Especialidad = "NO";
            }
            if (Chk_Conformidad_Finaza.Checked == true)
            {
                Negocio_Alta.P_Documento_Conformidad = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Conformidad = "NO";
            }
            if (Chk_Curso.Checked == true)
            {
                Negocio_Alta.P_Documento_Curso = "SI";
            }
            else
            {
                Negocio_Alta.P_Documento_Curso = "NO";
            }

            Negocio_Alta.P_Tipo_Perito = Cmb_Tipo_Perito.SelectedValue;
            Negocio_Alta.P_Telefono_Celular = Txt_Celular.Text;
            Negocio_Alta.P_Comentario = Txt_Comentarios.Text;

            Negocio_Alta.Alta();
        }
        catch (Exception ex)
        {
            throw new Exception("Alta " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Modificar()
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Modificar = new Cls_Cat_Ort_Inspectores_Negocio();
        try
        {
            Negocio_Modificar.P_Inspector_ID = Hdf_Elemento_ID.Value;
            Negocio_Modificar.P_Nombre = Txt_Nombre.Text;
            Negocio_Modificar.P_Usuario = Cls_Sessiones.Nombre_Empleado;


            Negocio_Modificar.P_Cedula_Profesional = Txt_Cedula_Profesional.Text;
            Negocio_Modificar.P_Titulo = Txt_Titulo.Text;
            Negocio_Modificar.P_Afiliado = Txt_Afiliado.Text;

            Negocio_Modificar.P_Calle_Oficina = Cmb_Calle.SelectedItem.ToString();
            Negocio_Modificar.P_Colonia_Oficina = Cmb_Colonias.SelectedItem.ToString();
            Negocio_Modificar.P_Numero_Oficina = Txt_Numero_Oficina.Text;
            Negocio_Modificar.P_Telefono_Oficina = Txt_Telefono_Oficina.Text;
            Negocio_Modificar.P_Email = Txt_Email.Text;

            Negocio_Modificar.P_Calle_Particular = Cmb_Calle_Particular.SelectedItem.ToString();
            Negocio_Modificar.P_Colonia_Particular = Cmb_Colonias_Particular.SelectedItem.ToString();
            Negocio_Modificar.P_Numero_Particular = Txt_Numero_Casa.Text;
            Negocio_Modificar.P_Codigo_Postal = Txt_Codigo_Postal.Text;
            Negocio_Modificar.P_Telefono_Particular = Txt_Telefono_Particular.Text;
            Negocio_Modificar.P_Especialidad = Txt_Especialidad.Text;

            if (Chk_Titulo_Profesional.Checked == true)
            {
                Negocio_Modificar.P_Documento_Titulo = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Titulo = "NO";
            }

            if (Chk_Cedula_Profesional.Checked == true)
            {
                Negocio_Modificar.P_Documento_Cedula = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Cedula = "NO";
            }

            if (Chk_Curriculum.Checked == true)
            {
                Negocio_Modificar.P_Documento_Curriculum = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Curriculum = "NO";
            }

            if (Chk_Constancia.Checked == true)
            {
                Negocio_Modificar.P_Documento_Constancia = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Constancia = "NO";
            }

            if (Chk_Refrendo.Checked == true)
            {
                Negocio_Modificar.P_Documento_Refrendo = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Refrendo = "NO";
            }
            if (Chk_Acreditacion.Checked == true)
            {
                Negocio_Modificar.P_Documento_Especialidad = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Especialidad = "NO";
            }
            if (Chk_Conformidad_Finaza.Checked == true)
            {
                Negocio_Modificar.P_Documento_Conformidad = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Conformidad = "NO";
            }
            if (Chk_Curso.Checked == true)
            {
                Negocio_Modificar.P_Documento_Curso = "SI";
            }
            else
            {
                Negocio_Modificar.P_Documento_Curso = "NO";
            }
            Negocio_Modificar.P_Tipo_Perito = Cmb_Tipo_Perito.SelectedValue;
            Negocio_Modificar.P_Telefono_Celular = Txt_Celular.Text;
            Negocio_Modificar.P_Comentario = Txt_Comentarios.Text;

            Negocio_Modificar.Modificar();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Eliminar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Eliminar()
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Eliminar = new Cls_Cat_Ort_Inspectores_Negocio();
        try
        {
            Negocio_Eliminar.P_Inspector_ID = Hdf_Elemento_ID.Value;
            Negocio_Eliminar.Eliminar();
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Buscar
    ///DESCRIPCIÓN          : se pasaran los elementos a la capa de negocio
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 11/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Buscar()
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Buscar = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Negocio_Buscar.P_Nombre = Txt_Busqueda.Text;
            Dt_Consulta = Negocio_Buscar.Consultar_Inspectores();
            Cargar_Grid_Inspectores(Dt_Consulta);
        }
        catch (Exception ex)
        {
            throw new Exception("Buscar " + ex.Message.ToString());
        }
    }
    #endregion

    #region Botones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : realiza la alta del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos())
                {
                    Alta();
                    Inicializar_Controles();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Nuevo_Click", "alert('Alta Exitosa');", true);
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
    /// NOMBRE DE LA FUNCION: Btn_Modificar_Click
    /// DESCRIPCION : realiza la modificacion del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Modificar.ToolTip == "Modificar" && Hdf_Elemento_ID.Value != "")
            {
                Habilitar_Controles("Modificar"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos_Modificar())
                {
                    if (Validar_Datos())
                    {
                        Modificar();
                        Inicializar_Controles();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Modificar_Click", "alert('Modificacion Exitosa');", true);
                    }
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
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : realiza la baja
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (!string.IsNullOrEmpty(Hdf_Elemento_ID.Value))
            {
                Eliminar();
                Inicializar_Controles();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Btn_Eliminar_Click", "alert('Baja Exitosa');", true);
            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccione el registro que se eliminara <br>";
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
    /// FECHA_CREO  : 11/Junio/2012
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
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);
            if (!String.IsNullOrEmpty(Txt_Busqueda.Text))
            {
                Buscar();
            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Ingrese el nombre a buscar";
                Cargar_Inspectores();
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
            {
                try
                {
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonias.SelectedValue = Colonia_ID;
                        Cmb_Colonias_OnSelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                    //Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                try
                {
                    string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // consultar las calles de la colonia a la que pertenece la calle seleccionada
                    Cmb_Calle.Items.Clear();
                    Obj_Calles.P_Colonia_ID = Colonia_ID;
                    Llenar_Combo_Con_DataTable(Cmb_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonias.SelectedValue = Colonia_ID;
                    }
                    // si el combo calles contiene un elemento con el ID, seleccionar
                    if (Cmb_Calle.Items.FindByValue(Calle_ID) != null)
                    {
                        Cmb_Calle.SelectedValue = Calle_ID;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("CLAVE_COLONIA");
                Session.Remove("CALLE_ID");
                Session.Remove("CLAVE_CALLE");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS_CALLES");
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Particular_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonia_Particular_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
            {
                try
                {
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonias_Particular.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonias_Particular.SelectedValue = Colonia_ID;
                        Cmb_Colonias_Particular_SelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                    //Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calle_Particular_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Calle_Particular_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                try
                {
                    string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // consultar las calles de la colonia a la que pertenece la calle seleccionada
                    Cmb_Calle_Particular.Items.Clear();
                    Obj_Calles.P_Colonia_ID = Colonia_ID;
                    Llenar_Combo_Con_DataTable(Cmb_Calle_Particular, Obj_Calles.Consultar_Calles(), 0, 5);
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonias_Particular.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonias_Particular.SelectedValue = Colonia_ID;
                    }
                    // si el combo calles contiene un elemento con el ID, seleccionar
                    if (Cmb_Calle_Particular.Items.FindByValue(Calle_ID) != null)
                    {
                        Cmb_Calle_Particular.SelectedValue = Calle_ID;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("CLAVE_COLONIA");
                Session.Remove("CALLE_ID");
                Session.Remove("CLAVE_CALLE");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS_CALLES");
        }

    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    /// 		3. Indice_Campo_Valor: entero con el número de columna de la tabla con el valor para el combo
    /// 		4. Indice_Campo_Texto: entero con el número de columna de la tabla con el texto para el combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal, int Indice_Campo_Valor, int Indice_Campo_Texto)
    {
        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataTextField = Dt_Temporal.Columns[Indice_Campo_Texto].ToString();
        Obj_Combo.DataValueField = Dt_Temporal.Columns[Indice_Campo_Valor].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }
    #endregion

    #region Combos
    ///*******************************************************************************
    ///NOMBRE:      Cmb_Colonias_OnSelectedIndexChanged
    ///DESCRIPCIÓN: se cargara la colonia 
    ///PARAMETROS:
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  23/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Colonias_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Calles = new DataTable();
        DataTable Dt_Colonias = new DataTable();
        try
        {
            Negocio_Consulta.P_Colonia_ID = Cmb_Colonias.SelectedValue;
            Dt_Calles = Negocio_Consulta.Consultar_Calles();

            Cargar_Combo_Calles_Oficina(Dt_Calles);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:      Cmb_Colonias_Particular_SelectedIndexChanged
    ///DESCRIPCIÓN: se cargara la colonia 
    ///PARAMETROS:
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  23/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Colonias_Particular_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Calles = new DataTable();
        DataTable Dt_Colonias = new DataTable();
        try
        {
            Negocio_Consulta.P_Colonia_ID = Cmb_Colonias_Particular.SelectedValue;
            Dt_Calles = Negocio_Consulta.Consultar_Calles();

            Cargar_Combo_Calles_Particular(Dt_Calles);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles_Particular
    ///DESCRIPCIÓN: cargara la informacion de las calles pertenecientes a una colonia
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Calles_Particular(DataTable Dt_Calles)
    {
        try
        {
            Cmb_Calle_Particular.DataSource = Dt_Calles;
            Cmb_Calle_Particular.DataValueField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle_Particular.DataTextField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle_Particular.DataBind();
            Cmb_Calle_Particular.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles_Oficina
    ///DESCRIPCIÓN: cargara la informacion de las calles pertenecientes a una colonia
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Calles_Oficina(DataTable Dt_Calles)
    {
        try
        {
            Cmb_Calle.DataSource = Dt_Calles;
            Cmb_Calle.DataValueField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle.DataTextField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Calle.DataBind();
            Cmb_Calle.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Grid
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Areas_SelectedIndexChanged
    /// DESCRIPCION : se cargara la informacion del grid en las cajas de texto
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 11/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Inspector_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Estado_Documento = "";
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Consulta = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Limpiar_Controles();
            GridViewRow selectedRow = Grid_Inspector.Rows[Grid_Inspector.SelectedIndex];

            Hdf_Elemento_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            Txt_Nombre.Text = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            
            Negocio_Consulta.P_Inspector_ID = Hdf_Elemento_ID.Value;
            Dt_Consulta = Negocio_Consulta.Consultar_Inspectores();

            if(Dt_Consulta != null && Dt_Consulta.Rows.Count>0)
            {
                foreach (DataRow Registro in Dt_Consulta.Rows)
                {
                    Cmb_Tipo_Perito.SelectedIndex = Cmb_Tipo_Perito.Items.IndexOf(Cmb_Tipo_Perito.Items.FindByText(Registro[Cat_Ort_Inspectores.Campo_Tipo_Perito].ToString()));
                    Txt_Cedula_Profesional.Text = Registro[Cat_Ort_Inspectores.Campo_Cedula_Profesional].ToString();
                    Txt_Titulo.Text = Registro[Cat_Ort_Inspectores.Campo_Titulo].ToString();
                    Txt_Afiliado.Text = Registro[Cat_Ort_Inspectores.Campo_Afiliado].ToString();

                    Cmb_Colonias.SelectedIndex = Cmb_Colonias.Items.IndexOf(Cmb_Colonias.Items.FindByText(Registro[Cat_Ort_Inspectores.Campo_Colonia_Oficina].ToString()));
                    Cmb_Colonias_OnSelectedIndexChanged(sender, null);
                    Cmb_Calle.SelectedIndex = Cmb_Calle.Items.IndexOf(Cmb_Calle.Items.FindByText(Registro[Cat_Ort_Inspectores.Campo_Calle_Oficina].ToString()));
                    Txt_Numero_Oficina.Text = Registro[Cat_Ort_Inspectores.Campo_Numero_Oficina].ToString();
                    Txt_Telefono_Oficina.Text = Registro[Cat_Ort_Inspectores.Campo_Telefono_Oficina].ToString();
                    Txt_Email.Text = Registro[Cat_Ort_Inspectores.Campo_Email].ToString();
                    Txt_Celular.Text = Registro[Cat_Ort_Inspectores.Campo_Telefono_Celular].ToString();
                    Txt_Comentarios.Text = Registro[Cat_Ort_Inspectores.Campo_Comentario].ToString();


                    Cmb_Colonias_Particular.SelectedIndex = Cmb_Colonias_Particular.Items.IndexOf(Cmb_Colonias_Particular.Items.FindByText(Registro[Cat_Ort_Inspectores.Campo_Colonia_Particular].ToString()));
                    Cmb_Colonias_Particular_SelectedIndexChanged(sender, null);
                    Cmb_Calle_Particular.SelectedIndex = Cmb_Calle_Particular.Items.IndexOf(Cmb_Calle_Particular.Items.FindByText(Registro[Cat_Ort_Inspectores.Campo_Calle_Particular].ToString()));
                    Txt_Numero_Casa.Text = Registro[Cat_Ort_Inspectores.Campo_Numero_Particular].ToString();
                    Txt_Codigo_Postal.Text = Registro[Cat_Ort_Inspectores.Campo_Codigo_Postal].ToString();
                    Txt_Telefono_Particular.Text = Registro[Cat_Ort_Inspectores.Campo_Telefono_Particular].ToString();
                    Txt_Especialidad.Text = Registro[Cat_Ort_Inspectores.Campo_Especialidad].ToString();
                    
                    //  para el titulo
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Titulo].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Titulo_Profesional.Checked = true;
                    }
                    else
                    {
                        Chk_Titulo_Profesional.Checked = false;
                    }
                    //  para la cedula
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Cedula].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Cedula_Profesional.Checked = true;
                    }
                    else
                    {
                        Chk_Cedula_Profesional.Checked = false;
                    }
                    //  para la curriculum
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Curriculum].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Curriculum.Checked = true;
                    }
                    else
                    {
                        Chk_Curriculum.Checked = false;
                    }
                    //  para la cosntancia
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Constancia].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Constancia.Checked = true;
                    }
                    else
                    {
                        Chk_Constancia.Checked = false;
                    }
                    //  para la refrendo
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Refrendo].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Refrendo.Checked = true;
                    }
                    else
                    {
                        Chk_Refrendo.Checked = false;
                    }
                    //  para la especialidad
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Especialidad].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Acreditacion.Checked = true;
                    }
                    else
                    {
                        Chk_Acreditacion.Checked = false;
                    }
                    //  para la conformidad de la finanza
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Conformidad].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Conformidad_Finaza.Checked = true;
                    }
                    else
                    {
                        Chk_Conformidad_Finaza.Checked = false;
                    }
                    //  para el curso
                    Estado_Documento = Registro[Cat_Ort_Inspectores.Campo_Documento_Curso].ToString();
                    if (Estado_Documento == "SI")
                    {
                        Chk_Curso.Checked = true;
                    }
                    else
                    {
                        Chk_Curso.Checked = false;
                    }
                }
            }
           
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion
}
