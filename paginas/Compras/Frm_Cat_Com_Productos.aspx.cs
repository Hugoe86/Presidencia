using System;
using System.Configuration;
using System.Data;
using System.Collections;
using System.Data.OracleClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Compras_Productos.Negocio;
using Presidencia.Catalogo_Compras_Familias.Negocio;
using Presidencia.Catalogo_Compras_Subfamilias.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Catalogo_Compras_Modelos.Negocio;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;
using Presidencia.Catalogo_Compras_Giros.Negocio;
using Presidencia.Catalogo_Compras_Impuestos.Negocio;
using Presidencia.Catalogo_Compras_Unidades.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using System.Collections.Generic;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using Presidencia.Bitacora_Eventos;
using CarlosAg.ExcelXmlWriter;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Excel = Microsoft.Office.Interop.Excel;


public partial class paginas_Compras_Frm_Cat_Com_Productos : System.Web.UI.Page
{
    #region Variables Globales
    private static String Descripcion;
    private static DataTable Dt_Productos;
    #endregion

    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            }
            Mensaje_Error();
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region(Metodos)

    #region Metodos Modificaciones
    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String _Texto, String _Valor, String Seleccion)
    {
        String Texto = "";
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                if (_Texto.Contains("+"))
                {
                    String[] Array_Texto = _Texto.Split('+');

                    foreach (String Campo in Array_Texto)
                    {
                        Texto = Texto + row[Campo].ToString();
                        Texto = Texto + "  ";
                    }
                }
                else
                {
                    Texto = row[_Texto].ToString();
                }
                Obj_DropDownList.Items.Add(new ListItem(Texto, row[_Valor].ToString()));
                Texto = "";
            }
            Obj_DropDownList.SelectedValue = Seleccion;

            // Se le agrega un ToolTip a cada elemento del combo, ya que los valores no caben en el combo
            if (Cmb_Capitulo != null)
                foreach (ListItem li in Cmb_Capitulo.Items)
                    li.Attributes.Add("title", li.Text);

            if (Cmb_Conceptos != null)
                foreach (ListItem li in Cmb_Conceptos.Items)
                    li.Attributes.Add("title", li.Text);

            if (Cmb_Partida_General != null)
                foreach (ListItem li in Cmb_Partida_General.Items)
                    li.Attributes.Add("title", li.Text);

            if (Cmb_Partida_Especifica != null)
                foreach (ListItem li in Cmb_Partida_Especifica.Items)
                    li.Attributes.Add("title", li.Text);
            
            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
        Lbl_Mensaje_Error.Visible = true;
    }
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }

    #endregion

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Cargar_Combos(); // Poblar todos los combos con datos de la BD
            Limpiar_Controles(); //Limpia los controles del forma
            //Consulta_Productos(); //Consulta todos los Productos en la BD
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    private void Mostrar_Busqueda(Boolean Estatus)
    {
        Div_Busqueda_Av.Visible = Estatus;
        //Txt_Busqueda_Producto.Visible = Estatus;
        //Btn_Buscar_Producto.Visible = Estatus;
        //Lbl_Busqueda.Visible = Estatus;
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Habilitar_Controles
    /// 	DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera 
    /// 	            para la siguiente operación
    /// 	PARÁMETROS:
    /// 	        1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 		             (inicial, nuevo, modificar)
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 03-Feb-2011 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Buscar_Producto.Focus();
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Cmb_Estatus.Enabled = false;
                    Txt_Clave.ReadOnly = true;
                    Txt_Clave.Enabled = false;
                    //Btn_Descripcion.Visible = false;
                    Grid_Productos.Visible = false;

                    Cmb_Capitulo.Enabled = false;
                    Cmb_Conceptos.Enabled = false;
                    Cmb_Partida_General.Enabled = false;
                    Cmb_Partida_Especifica.Enabled = false;
                    //Cmb_Resguardo.Enabled = false;
                    Cmb_Stock.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    Txt_Costo.ReadOnly = false;

                    Configuracion_Acceso("Frm_Cat_Com_Productos.aspx");
                    Mostrar_Busqueda(true);
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;

                    Txt_Costo.ReadOnly = false;
                    Txt_Clave.Enabled = false;

                    //Cmb_Resguardo.Enabled = true;
                    Cmb_Stock.Enabled = true;
                    Cmb_Capitulo.Enabled = true;
                    Cmb_Conceptos.Enabled = true;
                    Cmb_Partida_General.Enabled = true;
                    Cmb_Partida_Especifica.Enabled = true;

                    Cmb_Estatus.SelectedIndex = 1;
                    Cmb_Estatus.Enabled = false;
                    Mostrar_Busqueda(false);
                    Configuracion_Acceso("Frm_Cat_Com_Productos.aspx");
                    break;

                case "Modificar":
                    Cmb_Estatus.Enabled = true;
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    //Cmb_Resguardo.Enabled = true;
                    Cmb_Stock.Enabled = true;
                    Cmb_Capitulo.Enabled = true;
                    Cmb_Conceptos.Enabled = true;
                    Cmb_Partida_General.Enabled = true;
                    Cmb_Partida_Especifica.Enabled = true;
                    Cmb_Estatus.Enabled = true;
                    Mostrar_Busqueda(false);
                    Configuracion_Acceso("Frm_Cat_Com_Productos.aspx");
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Nombre_Producto.Enabled = Habilitado;
            Txt_Descripcion.Enabled = Habilitado;
            Txt_Costo.Enabled = Habilitado;
            //Txt_Costo_Promedio.Enabled = Habilitado;     //Se calcula a partir del costo            
            Txt_Existencia.Enabled = Habilitado;
            //Txt_Comprometido.Enabled = Habilitado;       //El usuario no debe modificar (es parte de la operación)
            //Txt_Disponible.Enabled = Habilitado;         //El usuario no debe modificar (es parte de la operación)
            Txt_Maximo.Enabled = Habilitado;
            Txt_Minimo.Enabled = Habilitado;
            //Txt_Reorden.Enabled = Habilitado;         //El usuario no debe modificar (es la mitad entre el mínimo y el máximo)
            Txt_Ubicacion.Enabled = Habilitado;
            //Txt_Busqueda_Producto.Enabled = !Habilitado;
            Btn_Buscar_Producto.Enabled = !Habilitado;
            Cmb_Unidad.Enabled = Habilitado;
            Cmb_Impuesto.Enabled = Habilitado;
            Cmb_Impuesto_2.Enabled = Habilitado;
            Grid_Productos.Enabled = !Habilitado;
            //Cmb_Resguardo.Enabled = Habilitado;
            Cmb_Stock.Enabled = Habilitado;
            Cmb_Capitulo.Enabled = Habilitado;
            Cmb_Conceptos.Enabled = Habilitado;
            Cmb_Partida_General.Enabled = Habilitado;
            Cmb_Partida_Especifica.Enabled = Habilitado;
            Cmb_Tipo.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    #region(CARGAR_COMBOS)


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combos
    /// DESCRIPCION: Llamar a todos los métodos de cargar combo
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        Cargar_Combo_Unidades();
        Cargar_Combo_Impuesto();
        Cargar_Combo_Impuesto_2();
        Cargar_Combos_Partidas();
        Cargar_Combo_Tipo();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos_Partidas
    ///DESCRIPCIÓN: metodo usado para cargar la informacion de todos los combos referentes a la partida especifica 
    ///             del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos_Partidas()
    {
        try
        {
            Cls_Cat_Com_Proyectos_Programas_Negocio Proyectos_Programas = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Capitulo, Proyectos_Programas.Consulta_Capitulos(), Cat_SAP_Capitulos.Campo_Clave + "+" + Cat_SAP_Capitulos.Campo_Descripcion, Cat_SAP_Capitulos.Campo_Capitulo_ID, "0");
            Cmb_Estatus.Items.Clear();
            Cmb_Partida_General.Items.Clear();
            Cmb_Partida_Especifica.Items.Clear();
            Cmb_Conceptos.Items.Clear();
            Cmb_Estatus.Items.Clear();
            Cmb_Estatus.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            Cmb_Estatus.Items.Add(new ListItem("ACTIVO", "ACTIVO"));
            Cmb_Estatus.Items.Add(new ListItem("INACTIVO", "INACTIVO"));
            //Cmb_Resguardo.Items.Clear();
            //Cmb_Resguardo.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            //Cmb_Resguardo.Items.Add(new ListItem("SI", "SI"));
            //Cmb_Resguardo.Items.Add(new ListItem("NO", "NO"));
            Cmb_Stock.Items.Clear();
            Cmb_Stock.Items.Add(new ListItem("< SELECCIONAR >", "0"));
            Cmb_Stock.Items.Add(new ListItem("SI", "SI"));
            Cmb_Stock.Items.Add(new ListItem("NO", "NO"));
            Llenar_Combo_ID(Cmb_Conceptos);
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_General);

        }
        catch (Exception Ex)
        {
            throw new Exception("No se pudieron cargar los datos necesarios" + "</br>" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Unidades
    /// DESCRIPCION: Consulta las unidades del catálogo Cat_Com_Unidades
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Unidades()
    {
        DataTable Dt_Unidades; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Unidades_Negocio Rs_Consulta_Cat_Unidades = new Cls_Cat_Com_Unidades_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Unidades = Rs_Consulta_Cat_Unidades.Consulta_Unidades(); //Consulta todas las familias que estan dadas de alta en la BD
            Cmb_Unidad.DataSource = Dt_Unidades;
            Cmb_Unidad.DataValueField = "Unidad_ID";
            Cmb_Unidad.DataTextField = "Nombre";
            Cmb_Unidad.DataBind();
            Cmb_Unidad.Items.Insert(0, "< SELECCIONAR >");
            Cmb_Unidad.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Unidades " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Impuesto
    /// DESCRIPCION: Consulta los Impuesto en la base de datos (Cat_Com_Impuestos)
    ///             Y carga el combo Impuesto
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Impuesto()
    {
        DataTable Dt_Impuesto; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Impuestos_Negocio Rs_Consulta_Cat_Impuestos = new Cls_Cat_Com_Impuestos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Impuesto = Rs_Consulta_Cat_Impuestos.Consulta_Impuestos(); //Consulta todos los Impuestos que estan dadas de alta en la BD
            Cmb_Impuesto.DataSource = Dt_Impuesto;
            Cmb_Impuesto.DataValueField = "Impuesto_ID";
            Cmb_Impuesto.DataTextField = "Nombre";
            Cmb_Impuesto.DataBind();
            Cmb_Impuesto.Items.Insert(0, "< SELECCIONAR >");
            Cmb_Impuesto.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Impuesto " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Combo_Impuesto_2
    /// DESCRIPCION: Consulta los Impuesto en la base de datos (Cat_Com_Impuestos)
    /// 			para poblar el combo Impuesto_2
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Impuesto_2()
    {
        DataTable Dt_Impuesto; //Variable que obtendrá los datos de la consulta        
        Cls_Cat_Com_Impuestos_Negocio Rs_Consulta_Cat_Impuestos = new Cls_Cat_Com_Impuestos_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Impuesto = Rs_Consulta_Cat_Impuestos.Consulta_Impuestos(); //Consulta todos los Impuestos que estan dadas de alta en la BD
            Cmb_Impuesto_2.DataSource = Dt_Impuesto;
            Cmb_Impuesto_2.DataValueField = "Impuesto_ID";
            Cmb_Impuesto_2.DataTextField = "Nombre";
            Cmb_Impuesto_2.DataBind();
            Cmb_Impuesto_2.Items.Insert(0, "< SELECCIONAR >");
            Cmb_Impuesto_2.SelectedIndex = 0;

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Combo_Impuesto_2 " + ex.Message.ToString(), ex);
        }
    }


    #endregion //(CARGAR COMBOS)



    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Limpia los controles que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //Cmb_Resguardo.SelectedIndex = 0;
            Cmb_Stock.SelectedIndex = 0;
            Cmb_Capitulo.SelectedIndex = 0;
            Cmb_Conceptos.SelectedIndex = 0;
            Cmb_Partida_General.SelectedIndex = 0;
            Cmb_Partida_Especifica.SelectedIndex = 0;
            Cmb_Unidad.SelectedIndex = 0;
            Cmb_Impuesto.SelectedIndex = 0;
            Cmb_Impuesto_2.SelectedIndex = 0;
            Cmb_Estatus.SelectedIndex = 1;
            Txt_Producto_ID.Value = "";
            Txt_Clave.Text = "";
            Txt_Nombre_Producto.Text = "";
            Txt_Descripcion.Text = "";
            Txt_Costo.Text = "";
            Txt_Costo_Promedio.Text = "";
            Txt_Existencia.Text = "";
            Txt_Comprometido.Text = "";
            Txt_Disponible.Text = "";
            Txt_Maximo.Text = "";
            Txt_Minimo.Text = "";
            Txt_Reorden.Text = "";
            Txt_Ubicacion.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Productos
    /// DESCRIPCION: Consulta los Productos que estan dados de alta en la BD
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Productos()
    {
        Cls_Cat_Com_Productos_Negocio RS_Consulta_Cat_Com_Productos = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión hacia la capa de Negocios
        //DataTable Dt_Productos; //Variable que obtendrá los datos de la consulta 

        try
        {
            if (Txt_Nombre_Producto_B.Text.Trim() != "")
            {
                RS_Consulta_Cat_Com_Productos.P_Nombre = Txt_Nombre_Producto_B.Text.Trim();
            }
            if (Txt_Descripcion_Producto_B.Text.Trim() != "")
            {
                RS_Consulta_Cat_Com_Productos.P_Descripcion = Txt_Descripcion_Producto_B.Text.Trim();
            }
            if (Txt_Clave_B.Text.Trim() != "")
            {
                RS_Consulta_Cat_Com_Productos.P_Clave = Txt_Clave_B.Text.Trim();
            }
            RS_Consulta_Cat_Com_Productos.P_Estatus = Cmb_Estatus.SelectedValue;
            Dt_Productos = RS_Consulta_Cat_Com_Productos.Consulta_Datos_Producto(); //Consulta los Productos con sus datos generales

            for ( int Indice = 0; Indice <= Dt_Productos.Rows.Count-1; Indice++ )
            {
                if (Dt_Productos.Rows[Indice][Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString() != String.Empty && Dt_Productos.Rows[Indice][Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString() != "" && Dt_Productos.Rows[Indice][Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString() != null)
                {
                    DataTable Detalles_Productos = RS_Consulta_Cat_Com_Productos.Consulta_Indices_Producto(Dt_Productos.Rows[Indice][Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString());
                    Dt_Productos.Rows[Indice]["PARTIDA_GENERICA_ID"] = Detalles_Productos.Rows[0]["PARTIDA_GENERICA_ID"];
                    Dt_Productos.Rows[Indice]["CONCEPTO_ID"] = Detalles_Productos.Rows[0]["CONCEPTO_ID"];
                    Dt_Productos.Rows[Indice]["CAPITULO_ID"] = Detalles_Productos.Rows[0]["CAPITULO_ID"];
                    Dt_Productos.Rows[Indice]["P_ESPECIFICA_DESCRIPCION"] = Detalles_Productos.Rows[0]["P_ESPECIFICA_DESCRIPCION"];
                    Dt_Productos.Rows[Indice]["P_GENERICA_DESCRIPCION"] = Detalles_Productos.Rows[0]["P_GENERICA_DESCRIPCION"];
                    Dt_Productos.Rows[Indice]["CONCEPTO_DESCRIPCION"] = Detalles_Productos.Rows[0]["CONCEPTO_DESCRIPCION"];
                    Dt_Productos.Rows[Indice]["CAPITULO_DESCRIPCION"] = Detalles_Productos.Rows[0]["CAPITULO_DESCRIPCION"];                    
                    //Dt_Productos.ImportRow(Detalles_Productos.Rows[0]);                    
                }
            }
            Session["Consulta_Productos"] = Dt_Productos;
            Llena_Grid_Productos();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Productos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Productos
    /// DESCRIPCION: Llena el grid con los Productos de la base de datos
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-Feb-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Productos()
    {
        //DataTable Dt_Productos; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Productos.DataBind();
            Dt_Productos = (DataTable)Session["Consulta_Productos"];
            Grid_Productos.Columns[1].Visible = true;

            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                String Marca = Dt_Productos.Rows[i]["MARCA_NOMBRE"].ToString().Trim();
                String Descripcion = Dt_Productos.Rows[i]["DESCRIPCION"].ToString().Trim();

                if(Marca != ""){
                    String Descripcion_Completa= Descripcion + ", " + Marca;
                    Dt_Productos.Rows[i].SetField("DESCRIPCION",Descripcion_Completa);
                }
            }

            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
            Grid_Productos.Columns[1].Visible = false;

            Grid_Productos.Visible = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Productos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Alta_Productos
    /// 	DESCRIPCIÓN: Modifica los datos del Producto con lo que introdujo el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 03-Feb-2011 
    /// 	MODIFICÓ: Jesus Toledo Rodriguez
    /// 	FECHA_MODIFICÓ: 05-Abril-2011
    /// 	CAUSA_MODIFICACIÓN: Mofidificaciones de campos en la tabla de la base de datos
    ///*******************************************************************************************************
    private void Alta_Productos()
    {
        Cls_Cat_Com_Productos_Negocio Rs_Alta_Producto = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            String Descripcion = "";
            String Tipo = "";
            if (Cmb_Tipo.SelectedIndex == 0)
                Tipo = "PRODUCTO";

            else if (Cmb_Tipo.SelectedIndex == 1)
                Tipo = "CEMOVIENTE";

            else if (Cmb_Tipo.SelectedIndex == 2)
                Tipo = "BIEN_MUEBLE";

            else if (Cmb_Tipo.SelectedIndex == 3)
                Tipo = "PARTE_VEHICULO";

            else if (Cmb_Tipo.SelectedIndex == 4)
                Tipo = "VEHICULO";

            Rs_Alta_Producto.P_Tipo = Tipo;

            if (Cmb_Unidad.Items.Count > 0)
            {
                if (Cmb_Unidad.SelectedIndex != 0)
                    Rs_Alta_Producto.P_Unidad_ID = Cmb_Unidad.SelectedValue;
            }
            
            if (Cmb_Stock.SelectedIndex == 1)
            {
                //Rs_Alta_Producto.P_Existencia = Convert.ToInt32(Txt_Existencia.Text);
                //Rs_Alta_Producto.P_Comprometido = Convert.ToInt32(Txt_Comprometido.Text);
                //Rs_Alta_Producto.P_Disponible = Convert.ToInt32(Txt_Disponible.Text);
                //Rs_Alta_Producto.P_Maximo = Convert.ToInt32(Txt_Maximo.Text);
                //Rs_Alta_Producto.P_Minimo = Convert.ToInt32(Txt_Minimo.Text);
                //Rs_Alta_Producto.P_Reorden = Convert.ToInt32(Txt_Reorden.Text);
                //Rs_Alta_Producto.P_Ubicacion = Txt_Ubicacion.Text;
            }

            if (Cmb_Impuesto.SelectedIndex != 0)
                Rs_Alta_Producto.P_Impuesto_ID = Cmb_Impuesto.SelectedValue;
            else
                Rs_Alta_Producto.P_Impuesto_ID = "";

            if (Cmb_Impuesto_2.SelectedIndex != 0)
                Rs_Alta_Producto.P_Impuesto_2_ID = Cmb_Impuesto_2.SelectedValue;
            else
                Rs_Alta_Producto.P_Impuesto_2_ID = "";

            Rs_Alta_Producto.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Alta_Producto.P_Clave = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, 4);
            Rs_Alta_Producto.P_Nombre = Txt_Nombre_Producto.Text;
            Rs_Alta_Producto.P_Descripcion = Txt_Descripcion.Text;
            Rs_Alta_Producto.P_Costo = Convert.ToDouble(Txt_Costo.Text);
            Rs_Alta_Producto.P_Costo_Promedio = Convert.ToDouble(Txt_Costo_Promedio.Text);
            Rs_Alta_Producto.P_Stock = Cmb_Stock.SelectedValue;
            Rs_Alta_Producto.P_Capitulo_ID = Cmb_Capitulo.SelectedValue;
            Rs_Alta_Producto.P_Concepto_ID = Cmb_Conceptos.SelectedValue;
            Rs_Alta_Producto.P_Partida_Generica_ID = Cmb_Partida_General.SelectedValue;
            Rs_Alta_Producto.P_Partida_Especifica_ID = Cmb_Partida_Especifica.SelectedValue;

            Rs_Alta_Producto.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            String Producto_ID= Rs_Alta_Producto.Alta_Producto(); //Da de alta los datos del Producto proporcionados por el usuario en la BD
            Session["Producto_ID"] = Producto_ID;
            Descripcion = "Se dio de Alta el Producto " + Producto_ID + " " + Txt_Nombre_Producto.Text + ", STOCK = " + Cmb_Stock.SelectedValue;
            Descripcion = Descripcion + ", Costo:" + Txt_Costo.Text;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Productos ", "alert('El Alta del producto fue exitosa');", true);
            Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Cat_Com_Productos.aspx", "Producto " + Convert.ToInt32(Producto_ID) , Descripcion);
            Inicializa_Controles();
            //Modal_Foto_Producto.Show();
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Productos " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Modificar_Producto
    /// 	DESCRIPCIÓN: Modifica los datos del Producto con lo que introdujo el usuario
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 03-Feb-2011 
    /// 	MODIFICÓ: Jesus Toledo Rdz
    /// 	FECHA_MODIFICÓ: 04-Abril-2011
    /// 	CAUSA_MODIFICACIÓN: Modificacion general de fommulario
    ///*******************************************************************************************************
    private void Modificar_Producto()
    {
        Cls_Cat_Com_Productos_Negocio Rs_Modificar_Cat_Productos = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        String Descripcion = "";
        try
        {
            String Tipo = "";
            if (Cmb_Tipo.SelectedIndex == 0)
                Tipo = "PRODUCTO";

            else if (Cmb_Tipo.SelectedIndex == 1)
                Tipo = "CEMOVIENTE";

            else if (Cmb_Tipo.SelectedIndex == 2)
                Tipo = "BIEN_MUEBLE";

            else if (Cmb_Tipo.SelectedIndex == 3)
                Tipo = "PARTE_VEHICULO";

            else if (Cmb_Tipo.SelectedIndex == 4)
                Tipo = "VEHICULO";

            Rs_Modificar_Cat_Productos.P_Tipo = Tipo;
            Rs_Modificar_Cat_Productos.P_Producto_ID = Txt_Producto_ID.Value;
            Session["Producto_ID_Modificar"] = Txt_Producto_ID.Value;

            Rs_Modificar_Cat_Productos.P_Unidad_ID = Cmb_Unidad.SelectedValue;
            Rs_Modificar_Cat_Productos.P_Estatus = Cmb_Estatus.SelectedValue;
            //Rs_Modificar_Cat_Productos.P_Clave = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, 4);
            Rs_Modificar_Cat_Productos.P_Clave = Txt_Clave.Text.Trim();

            Rs_Modificar_Cat_Productos.P_Nombre = Txt_Nombre_Producto.Text;
            Rs_Modificar_Cat_Productos.P_Descripcion = Txt_Descripcion.Text;
            Rs_Modificar_Cat_Productos.P_Costo = Convert.ToDouble(Txt_Costo.Text);
            Rs_Modificar_Cat_Productos.P_Costo_Promedio = Convert.ToDouble(Txt_Costo.Text);

            if (Cmb_Impuesto.SelectedIndex != 0)
                Rs_Modificar_Cat_Productos.P_Impuesto_ID = Cmb_Impuesto.SelectedValue;
            else
                Rs_Modificar_Cat_Productos.P_Impuesto_ID = "";

            if (Cmb_Impuesto_2.SelectedIndex != 0)
                Rs_Modificar_Cat_Productos.P_Impuesto_2_ID = Cmb_Impuesto_2.SelectedValue;
            else
                Rs_Modificar_Cat_Productos.P_Impuesto_2_ID = "";

            if (Cmb_Stock.SelectedIndex == 1)
            {
                //Rs_Modificar_Cat_Productos.P_Existencia = Convert.ToInt32(Txt_Existencia.Text.Trim());

                //if (Txt_Comprometido.Text.Trim() !="")
                //Rs_Modificar_Cat_Productos.P_Comprometido = Convert.ToInt32(Txt_Comprometido.Text.Trim());

                //Rs_Modificar_Cat_Productos.P_Disponible = Convert.ToInt32(Txt_Disponible.Text.Trim());
                //Rs_Modificar_Cat_Productos.P_Maximo = Convert.ToInt32(Txt_Maximo.Text.Trim());
                //Rs_Modificar_Cat_Productos.P_Minimo = Convert.ToInt32(Txt_Minimo.Text.Trim());
                //Rs_Modificar_Cat_Productos.P_Reorden = Convert.ToInt32(Txt_Reorden.Text.Trim());

                Rs_Modificar_Cat_Productos.P_Ubicacion = Txt_Ubicacion.Text.Trim();
            }
            else
            {
                Rs_Modificar_Cat_Productos.P_Existencia = 0;
                Rs_Modificar_Cat_Productos.P_Comprometido = 0;
                Rs_Modificar_Cat_Productos.P_Disponible = 0;
                Rs_Modificar_Cat_Productos.P_Maximo = 0;
                Rs_Modificar_Cat_Productos.P_Minimo = 0;
                Rs_Modificar_Cat_Productos.P_Reorden = 0;
                Rs_Modificar_Cat_Productos.P_Ubicacion = "";
            }            

            Rs_Modificar_Cat_Productos.P_Stock = Cmb_Stock.SelectedValue;

            //if (Cmb_Resguardo.SelectedIndex != 0)
            //    Rs_Modificar_Cat_Productos.P_Resguardo = Cmb_Resguardo.SelectedValue;
            //else
            //    Rs_Modificar_Cat_Productos.P_Resguardo = "";

            Rs_Modificar_Cat_Productos.P_Partida_Especifica_ID = Cmb_Partida_Especifica.SelectedValue;
            Rs_Modificar_Cat_Productos.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;

            //Rs_Modificar_Cat_Productos.Alta_Producto();
            Rs_Modificar_Cat_Productos.Modificar_Producto(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Descripcion = " Se modifico el producto " + Txt_Clave.Text + " con los datos : Tipo " + Tipo +
                ", Nombre: " + Txt_Nombre_Producto.Text + ", Descripcion:" + Txt_Descripcion.Text.Trim() + ", Costo:" + Txt_Costo.Text; 
            Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Cat_Com_Productos.aspx", "Producto " + Txt_Clave.Text.Trim(), Descripcion);
          

            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Productos", "alert('La Modificación del Producto fue Exitosa');", true);
  
            //Modal_Modificar_Foto.Show();
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Productos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Validar_Campos
    /// 	DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    /// 	            correspondiente.
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 10-feb-2011
    /// 	MODIFICÓ: Jesus Toledo Rdz
    /// 	FECHA_MODIFICÓ: 04-Abril-2011
    /// 	CAUSA_MODIFICACIÓN: Modificacion general de formulario
    ///*******************************************************************************************************
    private bool Validar_Campos()
    {
        bool Resultado = true;
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 

        if (Cmb_Unidad.Items.Count > 0)
        {
            if (Cmb_Unidad.SelectedIndex <= 0)  //Validar combo Unidad
            {
                Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar una Unidad para el Producto");
                Resultado = false;
            }
        }

        if (Txt_Nombre_Producto.Text == "")  //Validar campo NOMBRE de producto (no vacío)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Descripcion Corta del Producto");
            Resultado = false;
        }
        else if (Txt_Nombre_Producto.Text.Length > 100)  //Validar campo NOMBRE de producto (longitud menor a 100)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Que el nombre del producto no contenga más de 100 caracteres");
            Resultado = false;
        }
        if (Txt_Descripcion.Text == "")  //Validar campo DESCRIPCION de producto (no vacío)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Descipción larga del Producto");
            Resultado = false;
        }
        else if (Txt_Descripcion.Text.Length > 3600)  //Validar campo DESCRIPCION de producto (longitud menor a 100)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Que la Descripión del producto no contenga más de 3600 caracteres");
            Resultado = false;
        }
        if (Txt_Costo.Text == "")  //Validar campo COSTO de producto (no vacío)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el Costo del Producto");
            Resultado = false;
        }
        else if (Convert.ToDouble(Extraer_Numero(Txt_Costo.Text)) <= 0)  //Validar campo COSTO de producto (longitud menor a 100)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + El Costro del producto debe ser mayor a 0");
            Resultado = false;
        }

        if (Cmb_Impuesto.SelectedIndex <= 0)  //Validar combo IMPUESTO
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Impuesto para el Producto");
            Resultado = false;
        }
        if (Cmb_Impuesto_2.SelectedIndex <= 0)  //Validar combo IMPUESTO 2
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar el segundo Impuesto para el Producto");
            Resultado = false;
        }
        Txt_Existencia.Text = "0";
        Txt_Minimo.Text = "0";
        Txt_Maximo.Text = "0";
            
        if (Cmb_Partida_Especifica.SelectedIndex <= 0)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar una Partida Especifica para el Producto");
            Resultado = false;
        }
        if (Cmb_Stock.SelectedIndex <= 0)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar el Stock para el Producto");
            Resultado = false;
        }

        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar el Estatus para el Producto");
            Resultado = false;
        }
        return Resultado;
    }


    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Extraer_Numero
    /// 	DESCRIPCIÓN: Mediante una expresión regular encuentra números en el texto
    /// 	PARÁMETROS:
    /// 		1. Texto: Texto en el que se va a buscar un número
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Extraer_Numero(String Texto)
    {
        Regex Rge_Decimal = new Regex(@"(?<entero>[0-9]{1,12})(?:\.[0-9]{0,4})?");
        Match Numero_Encontrado = Rge_Decimal.Match(Texto);

        return Numero_Encontrado.Value;
    }

    #endregion (Metodos)

    #region Eventos Grid
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Productos_PageIndexChanging
    /// 	DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Productos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Productos(); //Carga los productos que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Grid_Productos_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Consulta los datos del producto que selecciono el usuario y los muestra en los campos
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Com_Productos_Negocio Rs_Consulta_Cat_Productos = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Producto
        DataRow[] Registros; //Variable que obtendrá los datos de la consulta
        DataRow Registro;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Productos.P_Producto_ID = Grid_Productos.SelectedRow.Cells[1].Text;
            Registros = Dt_Productos.Select("PRODUCTO_ID = " + Grid_Productos.SelectedRow.Cells[1].Text);
            Registro = Registros[0];
                
                //Rs_Consulta_Cat_Productos.Consulta_Datos_Producto(); //Consulta los datos del Producto que fue seleccionada por el usuario
            if (Registro != null)
            {
                //Agrega los valores de los campos a los controles correspondientes de la forma
                //foreach (DataRow Registro in Dt_Productos.Rows)
                //{
                    Txt_Producto_ID.Value = Registro[Cat_Com_Productos.Campo_Producto_ID].ToString();
                    /// Combos, llaves foráneas
                    Cmb_Capitulo.SelectedValue = Registro[Cat_SAP_Capitulos.Campo_Capitulo_ID].ToString();
                    Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
                    //Cmb_Capitulo.ToolTip = Cmb_Capitulo.SelectedItem.Text;

                    ///Se llena el combo de conceptos y se selecciona el registado en el producto
                    Llenar_Combo_ID(Cmb_Partida_Especifica);
                    Llenar_Combo_ID(Cmb_Partida_General);
                    Llenar_Combo_ID(Cmb_Conceptos);
                    Llenar_Combo_ID(Cmb_Conceptos, Programas_Negocio.Consulta_Conceptos(Cmb_Capitulo.SelectedValue.ToString()), Cat_Sap_Concepto.Campo_Clave + "+" + Cat_Sap_Concepto.Campo_Descripcion, Cat_Sap_Concepto.Campo_Concepto_ID, "0");
                    Cmb_Conceptos.SelectedValue = Registro[Cat_Sap_Concepto.Campo_Concepto_ID].ToString();
                    ///Se llena el combo de partidas genericas y se selecciona el registado en el producto
                    Cmb_Conceptos.ToolTip = Cmb_Conceptos.SelectedItem.Text;
                    Llenar_Combo_ID(Cmb_Partida_Especifica);
                    Llenar_Combo_ID(Cmb_Partida_General);
                    Llenar_Combo_ID(Cmb_Partida_General, Programas_Negocio.Consulta_Partidas_Genericas(Cmb_Conceptos.SelectedValue), Cat_SAP_Partida_Generica.Campo_Clave + "+" + Cat_SAP_Partida_Generica.Campo_Descripcion, Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID, "0");
                    Cmb_Partida_General.SelectedValue = Registro[Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID].ToString();
                    ///Se llena el combo de partidas especificas y se selecciona el registado en el producto
                    Cmb_Partida_General.ToolTip = Cmb_Partida_General.SelectedItem.Text;
                    Llenar_Combo_ID(Cmb_Partida_Especifica);
                    Llenar_Combo_ID(Cmb_Partida_Especifica, Programas_Negocio.Consulta_Partidas_Especificas(Cmb_Partida_General.SelectedValue), Cat_Sap_Partidas_Especificas.Campo_Clave + "+" + Cat_Sap_Partidas_Especificas.Campo_Nombre, Cat_Sap_Partidas_Especificas.Campo_Partida_ID, "0");
                    Cmb_Partida_Especifica.SelectedValue = Registro[Cat_Sap_Partidas_Especificas.Campo_Partida_ID].ToString();

                    if (Registro[Cat_Com_Productos.Campo_Unidad_ID].ToString().Trim() !="")   
                        Cmb_Unidad.SelectedValue = Registro[Cat_Com_Productos.Campo_Unidad_ID].ToString();

                    if (Registro[Cat_Com_Productos.Campo_Tipo].ToString()=="PRODUCTO")
                        Cmb_Tipo.SelectedIndex = 0;
                    else if (Registro[Cat_Com_Productos.Campo_Tipo].ToString() == "CEMOVIENTE")
                        Cmb_Tipo.SelectedIndex = 1;
                    else if (Registro[Cat_Com_Productos.Campo_Tipo].ToString() == "BIEN_MUEBLE")
                        Cmb_Tipo.SelectedIndex = 2;
                    else if (Registro[Cat_Com_Productos.Campo_Tipo].ToString() == "PARTE_VEHICULO")
                        Cmb_Tipo.SelectedIndex = 3;
                    else if (Registro[Cat_Com_Productos.Campo_Tipo].ToString() == "VEHICULO")
                        Cmb_Tipo.SelectedIndex = 4;

                    if (Registro[Cat_Com_Productos.Campo_Impuesto_ID].ToString() !="")
                    Cmb_Impuesto.SelectedValue = Registro[Cat_Com_Productos.Campo_Impuesto_ID].ToString();

                    if (Registro[Cat_Com_Productos.Campo_Impuesto_2_ID].ToString() != "")
                    Cmb_Impuesto_2.SelectedValue = Registro[Cat_Com_Productos.Campo_Impuesto_2_ID].ToString();

                    Cmb_Estatus.SelectedValue = Registro[Cat_Com_Productos.Campo_Estatus].ToString() == "ACTIVO" ? "ACTIVO" : "INACTIVO";

                    ///Campos de texto
                    Txt_Clave.Text = Registro[Cat_Com_Productos.Campo_Clave].ToString();
                    Txt_Nombre_Producto.Text = Registro[Cat_Com_Productos.Campo_Nombre].ToString();
                    Txt_Descripcion.Text = Registro[Cat_Com_Productos.Campo_Descripcion].ToString();
                    Txt_Costo.Text = Registro[Cat_Com_Productos.Campo_Costo].ToString();
                    Txt_Costo_Promedio.Text = Registro[Cat_Com_Productos.Campo_Costo_Promedio].ToString();
                    Txt_Existencia.Text = Registro[Cat_Com_Productos.Campo_Existencia].ToString();
                    Txt_Comprometido.Text = Registro[Cat_Com_Productos.Campo_Comprometido].ToString();
                    Txt_Disponible.Text = Registro[Cat_Com_Productos.Campo_Disponible].ToString();
                    Txt_Maximo.Text = Registro[Cat_Com_Productos.Campo_Maximo].ToString();
                    Txt_Minimo.Text = Registro[Cat_Com_Productos.Campo_Minimo].ToString();
                    Txt_Reorden.Text = Registro[Cat_Com_Productos.Campo_Reorden].ToString();
                    Txt_Ubicacion.Text = Registro[Cat_Com_Productos.Campo_Ubicacion].ToString();

                    if (Registro[Cat_Com_Productos.Campo_Stock].ToString() != "")
                    {
                        Cmb_Stock.SelectedValue = Registro[Cat_Com_Productos.Campo_Stock].ToString();
                    }
                    else
                    {
                        //Cmb_Resguardo.SelectedIndex = 0;
                    }
                    //if (Registro[Cat_Com_Productos.Campo_Resguardo].ToString() != "")
                    //{
                    //    Cmb_Resguardo.SelectedValue = Registro[Cat_Com_Productos.Campo_Resguardo].ToString();
                    //}
                    //else
                    //{
                    //    Cmb_Resguardo.SelectedIndex = 0;
                    //}
                //}
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

    #region (EVENTOS)

    #region Eventos_ABC

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// 	DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    /// 	            en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    /// 	            error
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-feb-2011
    /// 	MODIFICÓ: Jesus Toledo Rdz
    /// 	FECHA_MODIFICÓ: 04-Abril-2011
    /// 	CAUSA_MODIFICACIÓN: Modificacion general del formulario
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Txt_Nombre_Producto.Focus();
            }
            else
            {
                if (Validar_Campos())
                {
                    Alta_Productos(); //Da de alta los datos proporcionados por el usuario                        
                }
                else
                {
                    String Mensaje = Lbl_Mensaje_Error.Text;
                    Mensaje_Error();
                    Mensaje_Error("Es necesario: </br>" + Mensaje);
                    Mensaje = "";
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Modificar_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Modificar. Validar los datos en los campos 
    /// 	        antes de enviar
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            // Si se dio clic en el botón Modificar, revisar que haya un producto seleccionado, si no mostrar mensaje

            if (Btn_Modificar.ToolTip == "Modificar")
            {
                
                if (Txt_Producto_ID.Value != "")
                {
                    //Verificamos si el Producto esta en Estatus diferente de CERRADA, CANCELADA,COMPLETA
                    
                    Cls_Cat_Com_Productos_Negocio Negocio = new Cls_Cat_Com_Productos_Negocio();
                    Negocio.P_Producto_ID = Txt_Producto_ID.Value.Trim();
                    DataTable Dt_Req = Negocio.Consultar_Productos_Ocupados();

                    if (Dt_Req.Rows.Count == 0)
                    {
                        Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                        Txt_Nombre_Producto.Focus();
                        Txt_Clave.Enabled = false;              // no se debe modificar la clave
                        //Txt_Costo.Enabled = false;
                        //Txt_Costo_Promedio.Enabled = false;
                        Habilitar_Componentes_Stock();
                    }
                    else
                    {
                        Mensaje_Error("El producto esta en Proceso en una Requisicion. No se puede Modificar");
                    }
                }
                else
                {
                    Mensaje_Error("Seleccione el Producto cuyos datos desea modificar");
                }
            }
            /// Si se da clic en el botón y el tooltip  es Actualizar, verificar la validez de los campos y enviar 
            /// los cambios o los mensajes de error correspondientes
            else
            {
                if (Validar_Campos())
                {
                    Modificar_Producto(); //Actualizar los datos del producto
                }
                else
                {
                    String Mensaje = Lbl_Mensaje_Error.Text;
                    Mensaje_Error();
                    Mensaje_Error("Es necesario: </br>" + Mensaje);
                    Mensaje = "";
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// 	DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    /// 	        inicializar controles 
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Consulta_Productos");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Btn_Buscar_Producto_Click
    /// 	DESCRIPCIÓN: Buscar Productos en la base de datos por nombre
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 04-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Producto_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
                Consulta_Productos(); //Consultar los productos que coincidan con el nombre porporcionado por el usuario
                Limpiar_Controles(); //Limpia los controles de la forma
                //Si no se encontraron Productos con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
                Btn_Salir.ToolTip = "Regresar";
                if (Grid_Productos.Rows.Count <= 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron Productos con el nombre proporcionado <br />";
                    Grid_Productos.Visible = false;
                }
            
        }

        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }

    }


    #endregion //Eventos ABC

    #region Eventos_Formulario

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Impuesto_SelectedIndexChanged
    /// 	DESCRIPCIÓN: Cuando cambie el índice seleccionado del combo, volver a poblar el combo Cmb_Impuesto_2
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 09-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Impuesto_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Volver a cargar el combo Impuesto_2
        Cargar_Combo_Impuesto_2();
        // Si hay un valor seleccionado en el combo Cmb_Impuesto, qutarlo del combo Cmb_Impuesto_2
        if (Cmb_Impuesto.SelectedIndex > 0 && Cmb_Impuesto.SelectedItem.ToString() != "TASA 0")
        {
            Cmb_Impuesto_2.Items.RemoveAt(Cmb_Impuesto.SelectedIndex);
            Cmb_Impuesto_2.Focus();
        }
    }
    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Costo_TextChanged
    /// 	DESCRIPCIÓN: Al cambiar el texto de Txt_Costo, igualarlo en el costo promedio si es un nuevo registro
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-feb-2011 
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Costo_TextChanged(object sender, EventArgs e)
    {
        // Si es un nuevo registro inicializar Comprometido a partir de 
        if (Btn_Nuevo.ToolTip == "Dar de Alta")
        {
            Txt_Costo_Promedio.Text = Extraer_Numero(Txt_Costo.Text);
        }
        Cmb_Estatus.Focus();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Existencia_TextChanged
    /// 	DESCRIPCIÓN: Asigna valores a los campos Comprometido y Disponible, de 0 e igual a Existencia 
    /// 	respectivamente, cuando un poducto se da de alta. Y cuando un producto se modifica, sólo permite
    /// 	aumentar la existencia (el nuevo valor no puede ser menor)
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Existencia_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Com_Productos_Negocio Rs_Consulta_Cat_Productos = new Cls_Cat_Com_Productos_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del Producto
        DataTable Dt_Productos; //Variable que obtendrá los datos de la consulta
        int Existencia_Anterior = 0;
        int Existencia = 0;
        int Comprometido = 0;
        int Disponible = 0;

        // Si es un nuevo registro y la caja de texto no está vacía inicializar Comprometido en 0
        if (Txt_Existencia.Text.Length > 0 && Btn_Nuevo.ToolTip == "Dar de Alta")
        {
            Txt_Comprometido.Text = "0";
            Txt_Disponible.Text = Extraer_Numero(Txt_Existencia.Text);
        }
        // Si es un nuevo registro y la caja de texto está vacía limpiar los campos Comprometido y disponible
        else if (Txt_Existencia.Text.Length <= 0 && Btn_Nuevo.ToolTip == "Dar de Alta")
        {
            Txt_Comprometido.Text = "";
            Txt_Disponible.Text = "";
        }

        // Si se dio clic en Modificar y el campo Existencia no está vacío, verificar que la existencia no sea menor que la existencia anterior
        if (Btn_Modificar.ToolTip == "Actualizar")
        {
            // Si el campo Existencia no está vacío, Almacenar el valor numérico en la variable Existencia
            if (Txt_Existencia.Text.Length > 0)
                Existencia = Convert.ToInt32(Extraer_Numero(Txt_Existencia.Text));
            if (Txt_Comprometido.Text.Length > 0)   //Aegurarnos de que el campo de texto no está vacío
                Comprometido = Convert.ToInt32(Extraer_Numero(Txt_Comprometido.Text));  //Asignar a la variable local el número en Txt_Comprometido
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            try
            {               // Obtener el valor anterior de existencia en la base de datos
                Rs_Consulta_Cat_Productos.P_Producto_ID = Txt_Producto_ID.Value;
                Dt_Productos = Rs_Consulta_Cat_Productos.Consulta_Datos_Producto(); //Consulta los datos del Producto seleccionado
                if (Dt_Productos.Rows.Count > 0)
                {
                    //Obtiene el valor del campo Existencia
                    foreach (DataRow Registro in Dt_Productos.Rows)
                    {
                        Existencia_Anterior = Convert.ToInt32(Registro[Cat_Com_Productos.Campo_Existencia].ToString());
                    }
                }
                // Si el valor anterior de existencia es mayor que el valor introducido, volver a poner el valor anterior y mostrar mensaje
                if (Existencia_Anterior > Existencia)
                {
                    Txt_Existencia.Text = Existencia_Anterior.ToString();
                    Existencia = Existencia_Anterior;
                    Mensaje_Error("No es posible reducir el valor de la Existencia.");
                }

                // Calcular el disponible (Existencia - Comprometido)
                Disponible = Existencia - Comprometido;
                Txt_Disponible.Text = Disponible.ToString();

                Txt_Minimo.Focus();
            }
            catch (Exception ex)
            {
                Mensaje_Error(ex.Message.ToString());
            }
        }
        Txt_Minimo.Focus();

    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Minimo_TextChanged
    /// 	DESCRIPCIÓN: Cuando cambie el valor del campo, se revisa que máximo sea mayor que mínimo, por lo menos en una unidad
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Minimo_TextChanged(object sender, EventArgs e)
    {
        int Minimo;
        int Maximo;

        // Si las cajas de texto minimo y Maximo contienen valores, comprobar que máximo es mayor que mínimo
        if (Txt_Minimo.Text.Length > 0 && Txt_Maximo.Text.Length > 0)
        {
            Minimo = Convert.ToInt32(Extraer_Numero(Txt_Minimo.Text));
            Maximo = Convert.ToInt32(Extraer_Numero(Txt_Maximo.Text));

            //Si máximo es menor o igual a mínimo, asignar a máximo el valor de mínimo más uno y reorden igual a mínimo
            if (Maximo <= Minimo)
            {
                Txt_Maximo.Text = (Minimo + 1).ToString();
                Txt_Reorden.Text = Minimo.ToString();
            }
            else if (Maximo > Minimo)       //Si máximo es mayor que mínimo, reorden es igual a la suma de estos entre dos
            {
                Txt_Reorden.Text = ((Minimo + Maximo) / 2).ToString();
            }
        }
        Txt_Maximo.Focus();
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Txt_Maximo_TextChanged
    /// 	DESCRIPCIÓN: Cuando cambie el valor del campo, se revisa que máximo sea mayor que mínimo, por lo menos en una unidad
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 11-feb-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Maximo_TextChanged(object sender, EventArgs e)
    {
        int Minimo;
        int Maximo;

        // Si las cajas de texto minimo y Maximo contienen valores, comprobar que máximo es mayor que mínimo
        if (Txt_Minimo.Text.Length > 0 && Txt_Maximo.Text.Length > 0)
        {
            Minimo = Convert.ToInt32(Extraer_Numero(Txt_Minimo.Text));
            Maximo = Convert.ToInt32(Extraer_Numero(Txt_Maximo.Text));

            //Si máximo es menor o igual a mínimo, asignar a máximo el valor de mínimo más uno y reorden igual a mínimo
            if (Maximo <= Minimo)
            {
                if (Maximo == 0)
                {
                    Txt_Minimo.Text = "0";
                    Txt_Reorden.Text = "0";
                    Txt_Maximo.Text = "1";
                }
                else
                {
                    Txt_Minimo.Text = (Maximo - 1).ToString();
                    Txt_Reorden.Text = (Maximo - 1).ToString();
                }
            }
            else if (Maximo > Minimo)       //Si máximo es mayor que mínimo, reorden es igual a la suma de estos entre dos
            {
                Txt_Reorden.Text = ((Minimo + Maximo) / 2).ToString();
            }
        }
        Txt_Ubicacion.Focus();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Descripcion_Click
    ///DESCRIPCIÓN: evento del combo partidas especificas muestra la descripcion de la partida seleccionada
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/08/2011 10:34:35 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************        
    protected void Btn_Descripcion_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Partida_Especifica.SelectedIndex > 0)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Descripción ", "alert('" + Hdn_Txt_Descripcion.Value + "');", true);
        }
    }

    #endregion

    #region Combos_Partidas
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Capitulo_Selectedindexchanged
    ///DESCRIPCIÓN: evento del combo capitulos para llenar el combo de conceptos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/08/2011 10:32:04 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************        
    protected void Cmb_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Capitulo.SelectedIndex > 0)
        {
            //Btn_Descripcion.Visible = false;
            Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            //Cmb_Capitulo.ToolTip = Cmb_Capitulo.SelectedItem.Text;
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_General);
            Llenar_Combo_ID(Cmb_Conceptos);
            Llenar_Combo_ID(Cmb_Conceptos, Programas_Negocio.Consulta_Conceptos(Cmb_Capitulo.SelectedValue.ToString()), Cat_Sap_Concepto.Campo_Clave + "+" + Cat_Sap_Concepto.Campo_Descripcion, Cat_Sap_Concepto.Campo_Concepto_ID, "0");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Conceptos_Selectedindexchanged
    ///DESCRIPCIÓN: evento del combo conceptos para llenar el combo de patidas genericas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/08/2011 10:32:39 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************        
    protected void Cmb_Conceptos_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Capitulo.SelectedIndex != 0)
            Cmb_Conceptos.Enabled = true;
        else
            Cmb_Conceptos.Enabled = false;

        if (Cmb_Conceptos.SelectedIndex > 0)
        {
            //Btn_Descripcion.Visible = false;
            //Cmb_Conceptos.ToolTip = Cmb_Conceptos.SelectedItem.Text;
            Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_General);
            Llenar_Combo_ID(Cmb_Partida_General, Programas_Negocio.Consulta_Partidas_Genericas(Cmb_Conceptos.SelectedValue), Cat_SAP_Partida_Generica.Campo_Clave + "+" + Cat_SAP_Partida_Generica.Campo_Descripcion, Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID, "0");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Partida_General_Selectedindexchanged
    ///DESCRIPCIÓN: evento del combo partidas genericas para llenar el combo de patidas especificas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/08/2011 10:33:14 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************        
    protected void Cmb_Partida_General_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Partida_General.SelectedIndex > 0)
        {
            //Btn_Descripcion.Visible = false;
            //Cmb_Partida_General.ToolTip = Cmb_Partida_General.SelectedItem.Text;
            Cls_Cat_Com_Proyectos_Programas_Negocio Programas_Negocio = new Cls_Cat_Com_Proyectos_Programas_Negocio();
            Llenar_Combo_ID(Cmb_Partida_Especifica);
            Llenar_Combo_ID(Cmb_Partida_Especifica, Programas_Negocio.Consulta_Partidas_Especificas(Cmb_Partida_General.SelectedValue), Cat_Sap_Partidas_Especificas.Campo_Clave + "+" + Cat_Sap_Partidas_Especificas.Campo_Nombre, Cat_Sap_Partidas_Especificas.Campo_Partida_ID, "0");

        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Partida_General_Selectedindexchanged
    ///DESCRIPCIÓN: evento del combo partidas especificas muestra la descripcion de la partida
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 04/08/2011 10:34:02 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************        
    protected void Cmb_Partida_Especifica_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Partida_Especifica.SelectedIndex > 0)
        {
            Cls_Cat_Com_Productos_Negocio Productos_Negocio = new Cls_Cat_Com_Productos_Negocio();
            //Cmb_Partida_Especifica.ToolTip = Cmb_Partida_Especifica.SelectedItem.Text;
            Hdn_Txt_Descripcion.Value = Productos_Negocio.Consulta_Descripcion(Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, 4));
            //if (Cmb_Partida_Especifica.SelectedIndex >= 1)
            //    Btn_Descripcion.Visible = true;
        }
        else
        {
            //Btn_Descripcion.Visible = false;
        }
    }
    #endregion

    #endregion // (EVENTOS)

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
            Botones.Add(Btn_Buscar_Producto);

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


    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Stock_SelectedIndexChanged
    /// 	DESCRIPCIÓN:      Se habilitan y deshabilitan los controles del Div "Div_Datos_Especificos"
    /// 	PARÁMETROS:
    /// 	CREO:            Salvador Hernández Ramírez
    /// 	FECHA_CREO:      21-Junio-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Stock_SelectedIndexChanged(object sender, EventArgs e)
    {
        Habilitar_Componentes_Stock();
    }


    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Cmb_Stock_SelectedIndexChanged
    /// 	DESCRIPCIÓN:    Habilitar y Deshabilitar los controles del Div "Div_Datos_Especificos"
    /// 	PARÁMETROS:
    /// 	CREO:            Salvador Hernández Ramírez
    /// 	FECHA_CREO:      20-Junio-2011
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Habilitar_Componentes_Stock()
    {
        Boolean Estatus = false;

        if (Cmb_Stock.SelectedIndex == 1)
        {
            Estatus = true;
            Txt_Existencia.BackColor = System.Drawing.Color.White;
            Txt_Minimo.BackColor = System.Drawing.Color.White;
           
            Txt_Maximo.BackColor = System.Drawing.Color.White;
            Txt_Ubicacion.BackColor = System.Drawing.Color.White;

            //Txt_Comprometido.BackColor = System.Drawing.Color.White;
            //Txt_Disponible.BackColor = System.Drawing.Color.White;
            //Txt_Reorden.BackColor = System.Drawing.Color.White;
        }
        else
        {
            Txt_Existencia.Text = "";
            Txt_Minimo.Text = "";
            Txt_Comprometido.Text = "";
            Txt_Maximo.Text = "";
            Txt_Disponible.Text = "";
            Txt_Reorden.Text = "";
            Txt_Ubicacion.Text = "";
            Estatus = false;
            Txt_Existencia.BackColor = System.Drawing.Color.WhiteSmoke;
            Txt_Minimo.BackColor = System.Drawing.Color.WhiteSmoke;
            Txt_Maximo.BackColor = System.Drawing.Color.WhiteSmoke;
            Txt_Ubicacion.BackColor = System.Drawing.Color.WhiteSmoke;

            //Txt_Comprometido.BackColor = System.Drawing.Color.WhiteSmoke;
            //Txt_Disponible.BackColor = System.Drawing.Color.WhiteSmoke;
            //Txt_Reorden.BackColor = System.Drawing.Color.WhiteSmoke;

        } 
        Cmb_Impuesto.Enabled = true;
        Cmb_Impuesto_2.Enabled = true;
        Txt_Existencia.Enabled = Estatus;
        Txt_Minimo.Enabled = Estatus;
        //Txt_Comprometido.Enabled = Estatus;
        Txt_Maximo.Enabled = Estatus;
        //Txt_Disponible.Enabled = Estatus;
        //Txt_Reorden.Enabled = Estatus;
        Txt_Ubicacion.Enabled = Estatus;

        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar__Combo_Tipo
    ///DESCRIPCIÓN:          Metodo que llena el combo Cmb_Tipo con los tipos de productos
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Tipo()
    {
        if (Cmb_Tipo.Items.Count == 0)
        {
            Cmb_Tipo.Items.Add("PRODUCTO");
            Cmb_Tipo.Items.Add("ANIMAL");
            Cmb_Tipo.Items.Add("BIEN MUEBLE");
            Cmb_Tipo.Items.Add("PARTE DE VEHICULO");
            Cmb_Tipo.Items.Add("VEHICULO");
            Cmb_Tipo.Items[0].Value = "0";
            Cmb_Tipo.Items[0].Selected = true;
        }
    }

    
    protected void Btn_Aceptar_Guardar_Foto_Click(object sender, EventArgs e)
    {
        // Hace referencia a la página utilizada para guardar el producto
        String Producto_ID = Session["Producto_ID"].ToString();
        String Modificar = "false";
        String Paginas = Request.QueryString["PAGINA"].Trim();
        Response.Redirect("Frm_Cat_Com_Foto_Productos.aspx?Producto_ID=" + HttpUtility.HtmlEncode(Producto_ID) + "&Modificar=" + HttpUtility.HtmlEncode(Modificar) + "&Pagina_P=" + Paginas);        
    }

    // Evento utilizado para  ocultar el panel
    protected void Btn_Cancelar_Guardar_Foto_Click(object sender, EventArgs e)
    {
        Modal_Foto_Producto.Hide();
    }

    protected void Btn_Aceptar_Modificar_Foto_Click(object sender, EventArgs e)
    {
        // Hace referencia a la página utilizada para Modificar la foto del producto
        String Producto_ID = Session["Producto_ID_Modificar"].ToString();
        String Modificar = "true";
        String Paginas = Request.QueryString["PAGINA"].Trim();
        Response.Redirect("Frm_Cat_Com_Foto_Productos.aspx?Producto_ID=" + HttpUtility.HtmlEncode(Producto_ID) + "&Modificar=" + HttpUtility.HtmlEncode(Modificar) + "&Pagina_P=" + Paginas);
    }

    protected void Btn_Cancelar_Modificar_Foto_Click(object sender, EventArgs e)
    {
        Modal_Modificar_Foto.Hide();
    }

    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Clave_B.Text = "";
        Txt_Nombre_Producto_B.Text = "";
        Txt_Descripcion_Producto_B.Text = "";
        Lbl_Mensaje_Error.Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel_Registros_Grid
    ///DESCRIPCIÓN: Consulta los prdocutos y los envia a excel
    ///PARAMETROS: 
    ///CREO: Susana Trigueros
    ///FECHA_CREO: 2/FEB/13 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Exportar_Excel_Registros()
    {
        try
        {
            Cls_Cat_Com_Productos_Negocio Negocio = new Cls_Cat_Com_Productos_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Productos = new DataTable();
            //Tabla con los datos de las cuentas principales
            Dt_Productos = Negocio.Consulta_Datos_A_Exportar_Excel();
            if (Dt_Productos != null && Dt_Productos.Rows.Count > 0)
            {
                string Ruta_Archivo;

                //Se determina el nombre del archivo
                string filename = "Rpt_Productos_Registrados" + String.Format("{0:MM-dd-yyyy}", DateTime.Now) + ".xls";
                //Se establece la ruta donde se guardara el archivo
                Ruta_Archivo = Server.MapPath("~") + "\\" + "Archivos" + "\\" + filename;
                Workbook book = new Workbook();
                // Especificar qué hoja debe ser abierto y el tamaño de la ventana por defecto
                book.ExcelWorkbook.ActiveSheetIndex = 0;
                book.ExcelWorkbook.WindowTopX = 100;
                book.ExcelWorkbook.WindowTopY = 200;
                book.ExcelWorkbook.WindowHeight = 7000;
                book.ExcelWorkbook.WindowWidth = 8000;
                // Propiedades del documento
                book.Properties.Author = "CONTEL";
                book.Properties.Title = "PRODUCTOS_REGISTRADOS";
                book.Properties.Created = DateTime.Now;

                // Se agrega estilo al libro
                WorksheetStyle style = book.Styles.Add("HeaderStyle");
                style.Font.FontName = "Courier New";
                style.Font.Size = 9;
                style.Font.Bold = true;
                style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                style.Font.Color = "White";
                //style.Interior.Color = "Black";
                style.Interior.Color = "Blue";
                style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");


                style.Interior.Pattern = StyleInteriorPattern.Solid;

                // Estilo
                WorksheetStyle style_Filas_Primarias = book.Styles.Add("FilaPrincipalStyle");
                style_Filas_Primarias.Font.FontName = "Courier New";
                style_Filas_Primarias.Font.Size = 8;
                style_Filas_Primarias.Font.Bold = false;
                style_Filas_Primarias.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                style_Filas_Primarias.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                style_Filas_Primarias.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                style_Filas_Primarias.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                style_Filas_Primarias.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                // Se Crea el estilo a usar
                style = book.Styles.Add("Default");
                style.Font.FontName = "Courier New";
                style.Font.Size = 8;
                // Se le da nombre a la hoja
                Worksheet sheet = book.Worksheets.Add("Hoja1");
                // Se ajustan las columnas
                sheet.Table.Columns.Add(new WorksheetColumn(40));//CLAVE
                sheet.Table.Columns.Add(new WorksheetColumn(120));//NOMBRE
                sheet.Table.Columns.Add(new WorksheetColumn(150));//DESCRIPCION
                sheet.Table.Columns.Add(new WorksheetColumn(55));//TIPO
                sheet.Table.Columns.Add(new WorksheetColumn(55));//COSTO
                sheet.Table.Columns.Add(new WorksheetColumn(60));//COSTO_PROMEDIO
                sheet.Table.Columns.Add(new WorksheetColumn(55));//UNIDAD
  //              sheet.Table.Columns.Add(new WorksheetColumn(60));
                sheet.Table.Columns.Add(new WorksheetColumn(50));//ESTATUS
                sheet.Table.Columns.Add(new WorksheetColumn(60));//EXISTENCIA
                sheet.Table.Columns.Add(new WorksheetColumn(60));//COMPROMETIDO
                sheet.Table.Columns.Add(new WorksheetColumn(60));//DISPONIBLE
                sheet.Table.Columns.Add(new WorksheetColumn(50));//REORDEN
                sheet.Table.Columns.Add(new WorksheetColumn(74));//IMPUESTO
                sheet.Table.Columns.Add(new WorksheetColumn(150));//PARTIDA
                sheet.Table.Columns.Add(new WorksheetColumn(50));//STOCK
                
    //            sheet.Table.Columns.Add(new WorksheetColumn(52));



                WorksheetRow row = sheet.Table.Rows.Add();
                row.Index = 0;//Para saltarse Filas

                row.Cells.Add(new WorksheetCell("CLAVE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("NOMBRE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("DESCRIPCION", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("TIPO", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("COSTO", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("COSTO PROM", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("UNIDAD", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("ESTATUS", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("EXISTENCIA", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("COMPROMETIDO", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("DISPONIBLE", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("REORDEN", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("IMPUESTO", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("PARTIDA", "HeaderStyle"));
                row.Cells.Add(new WorksheetCell("STOCK", "HeaderStyle"));
//                row.Cells.Add(new WorksheetCell("REF_JAPAMI", "HeaderStyle"));

                // Se recorre el grid 
                for (int Cont_Filas = 0; Cont_Filas < Dt_Productos.Rows.Count; Cont_Filas++)
                {
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["CLAVE"].ToString().Trim(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["NOMBRE"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["DESCRIPCION"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["TIPO"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["COSTO"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["COSTO_PROMEDIO"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["UNIDAD"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["ESTATUS"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["EXISTENCIA"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["COMPROMETIDO"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["DISPONIBLE"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["REORDEN"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["IMPUESTO"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["PARTIDA"].ToString(), "FilaPrincipalStyle"));
                    row.Cells.Add(new WorksheetCell(Dt_Productos.Rows[Cont_Filas]["STOCK"].ToString(), "FilaPrincipalStyle"));
                }
                ////GUARDAR Y MOSTRAR REPORTE 
                //book.Save(Ruta_Archivo);

                //Response.Clear();
                //Response.Buffer = true;
                //Response.ContentType = "application/vnd.ms-excel";
                //Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                //Response.Charset = "UTF-8";
                //Response.ContentEncoding = System.Text.Encoding.Default;
                //book.Save(Response.OutputStream);
                //Response.End();
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + filename);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = System.Text.Encoding.Default;
                book.Save(Response.OutputStream);
                Response.End();

                //Visualiza el archivo
                //Response.WriteFile(Ruta_Archivo);
                //Response.Flush();
                //Response.Close();

//                string script = @"<script type='text/javascript'>                           
//                alert('Registros Exportados a Excel');                        
//                </script>";
//                ScriptManager.RegisterStartupScript(this, typeof(Page), "alerta", script, false);
            }
            else
            {
                Mensaje_Error("&nbsp; &nbsp; &nbsp; &nbsp; + No se encontraron registros");
            }

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message, Ex);
        }

    }
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Exportar_Excel_Registros();
    }
}