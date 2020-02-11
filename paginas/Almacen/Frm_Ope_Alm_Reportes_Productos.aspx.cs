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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Xml.Linq;
using Presidencia.Reportes_Productos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes;


public partial class paginas_Almacen_Frm_Ope_Alm_Reportes_Productos : System.Web.UI.Page
{
    #region Variables
    Cls_Ope_Com_Alm_Rpts_Productos_Negocio Consulta_Productos = new Cls_Ope_Com_Alm_Rpts_Productos_Negocio();
    int No_Reporte = 0;
    #endregion

    #region Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Llenar_Combos();
            Estatus_Inicial();
        }
    }

    #endregion

    #region Metodos

     public void Estatus_Inicial(){
         Session["No_Reporte"] = null;
     }




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN:          Método utilizado para instanciar los métodos que llenan los combos
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combos()
    {
        Llenar_Combo_Partidas_Especificas();
        Llenar_Combo_Marcas();
        Llenar_Combo_Modelos();
        Llenar_Combo_Proveedores();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Partidas_Especificas
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con las partidas especificas
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Partidas_Especificas()
    {
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Partidas_Especificas =  new DataTable();
        
        // Se crean las columnas
        Dt_Partidas_Especificas.Columns.Add("PARTIDA_ID");
        Dt_Partidas_Especificas.Columns.Add("PARTIDA_ESPECIFICA");
           
        try
        {   // Se consultan las partidas especificas
            Consulta_Productos.P_Nombre_Tabla ="PARTIDAS_ESPECIFICAS";
            Dt_Consulta = Consulta_Productos.Consultar_Tablas();

            for (int i = 0; i < Dt_Consulta.Rows.Count; i++)
            {
                String Partida_Especifica = Dt_Consulta.Rows[i]["CLAVE"].ToString().Trim() + " " + Dt_Consulta.Rows[i]["PARTIDA_ESPECIFICA"].ToString().Trim();
                String Partida_ID = Dt_Consulta.Rows[i]["PARTIDA_ID"].ToString().Trim();

                DataRow Fila = Dt_Partidas_Especificas.NewRow();
                Fila["PARTIDA_ID"] = Partida_ID;
                Fila["PARTIDA_ESPECIFICA"] = Partida_Especifica;

                Dt_Partidas_Especificas.Rows.InsertAt(Fila, i);
            }

            if (Dt_Partidas_Especificas.Rows.Count > 0)
            {
                Cmb_Partidas_Especificas.DataSource = Dt_Partidas_Especificas;
                Cmb_Partidas_Especificas.DataTextField = "PARTIDA_ESPECIFICA";
                Cmb_Partidas_Especificas.DataValueField = Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Cmb_Partidas_Especificas.DataBind();
                Cmb_Partidas_Especificas.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Partidas_Especificas.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Modelos
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los modelos
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Modelos()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Consulta_Productos.P_Nombre_Tabla = "MODELOS";
            Dt_Consulta = Consulta_Productos.Consultar_Tablas();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Modelos.DataSource = Dt_Consulta;
                Cmb_Modelos.DataTextField = "MODELO";
                Cmb_Modelos.DataValueField = Cat_Com_Modelos.Campo_Modelo_ID;
                Cmb_Modelos.DataBind();
                Cmb_Modelos.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Modelos.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Marcas
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con las marcas
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Marcas()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Consulta_Productos.P_Nombre_Tabla = "MARCAS";
            Dt_Consulta = Consulta_Productos.Consultar_Tablas();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Marcas.DataSource = Dt_Consulta;
                Cmb_Marcas.DataTextField = "MARCA";
                Cmb_Marcas.DataValueField = Cat_Com_Marcas.Campo_Marca_ID;
                Cmb_Marcas.DataBind();
                Cmb_Marcas.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Marcas.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo con los proveedores
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Proveedores()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Consulta_Productos.P_Nombre_Tabla = "PROVEEDORES";
            Dt_Consulta = Consulta_Productos.Consultar_Tablas();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Proveedores.DataSource = Dt_Consulta;
                Cmb_Proveedores.DataTextField = "PROVEEDOR";
                Cmb_Proveedores.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
                Cmb_Proveedores.DataBind();
                Cmb_Proveedores.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Proveedores.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Opciones_Consulta
    ///DESCRIPCIÓN:          Método utilizado para validar que el usuario seleccione los
    ///                      combos, una vez que ha seleccionado los CheckBox
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Validar_Opciones_Consulta()
    {
        Boolean Validacion = true;
        Lbl_Informacion.Text = "Es necesario.";
        String Mensaje_Error = "";

        if (Ckb_Partida_Especifica.Checked)
        {
            if (Cmb_Partidas_Especificas.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del combo Partida Especifica.";
                Validacion = false;
            }
        }

        if (Ckb_Marca.Checked)
        {
            if (Cmb_Marcas.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del combo Marca.";
                Validacion = false;
            }
        }

        if (Ckb_Modelo.Checked)
        {
            if (Cmb_Modelos.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del combo Modelo.";
                Validacion = false;
            }
        }

        if (Ckb_Proveedor.Checked)
        {
            if (Cmb_Proveedores.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del combo Proveedor.";
                Validacion = false;
            }
        }

        if (Ckb_Descripcion_A_Z.Checked)
        {
            if (Txt_Letra_Inicial.Text == "")
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Asignar la letra inicial para la búsqueda.";
                Validacion = false;
            }
        }

        if (!Validacion)
        {
            Lbl_Informacion.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Validacion;
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consulta_Productos
    ///DESCRIPCIÓN:          Evento utilizado para instanciar al los métodos: "Validar_Opciones_Consulta", 
    ///                      "Consultar_Productos" y Generar_Reporte
    ///PARAMETROS:            
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Consulta_Productos_Almacen(String Formato)
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            if (Validar_Opciones_Consulta())
            {
                if ((Ckb_Partida_Especifica.Checked) && (Cmb_Partidas_Especificas.SelectedIndex != 0))
                    Consulta_Productos.P_Partida_Especifica_ID = Cmb_Partidas_Especificas.SelectedValue.Trim();

                else
                    Consulta_Productos.P_Partida_Especifica_ID = null;


                if ((Ckb_Modelo.Checked) && (Cmb_Modelos.SelectedIndex != 0))
                    Consulta_Productos.P_Modelo_ID = Cmb_Modelos.SelectedValue.Trim();
                else
                    Consulta_Productos.P_Modelo_ID = null;


                if ((Ckb_Marca.Checked) && (Cmb_Marcas.SelectedIndex != 0))
                    Consulta_Productos.P_Marca_ID = Cmb_Marcas.SelectedValue.Trim();
                else
                    Consulta_Productos.P_Marca_ID = null;


                if ((Ckb_Proveedor.Checked) && (Cmb_Proveedores.SelectedIndex != 0))
                    Consulta_Productos.P_Proveedor_ID = Cmb_Proveedores.SelectedValue.Trim();
                else
                    Consulta_Productos.P_Proveedor_ID = null;


                if ((Ckb_Descripcion_A_Z.Checked) && (Txt_Letra_Inicial.Text.Trim() != ""))
                {
                    Consulta_Productos.P_Letra_Inicial = Txt_Letra_Inicial.Text.Trim();

                    if ((Txt_Letra_Final.Text.Trim() != ""))
                        Consulta_Productos.P_Letra_Final = Txt_Letra_Final.Text.Trim();
                    else
                        Consulta_Productos.P_Letra_Final = Txt_Letra_Inicial.Text.Trim();
                }
                else
                {
                    Consulta_Productos.P_Letra_Inicial = null;
                    Consulta_Productos.P_Letra_Final = null;
                }


                if (Ckb_Productos_Stock.Checked)
                {
                    if (Cmb_Stock_Transitorios.SelectedIndex == 0)
                        Consulta_Productos.P_Tipo_Producto = null;
                    else if (Cmb_Stock_Transitorios.SelectedIndex == 1)
                        Consulta_Productos.P_Tipo_Producto = "TRANSITORIOS";
                    else if (Cmb_Stock_Transitorios.SelectedIndex == 2)
                        Consulta_Productos.P_Tipo_Producto = "STOCK";
                    else
                        Consulta_Productos.P_Tipo_Producto = null;
                }
                else
                    Consulta_Productos.P_Tipo_Producto = null;
                

                // Se realiza la consulta
                Dt_Consulta = Consulta_Productos.Consultar_Productos();

                if (Dt_Consulta.Rows.Count > 0)
                {
                    Ds_Alm_Com_Reporte_Productos Ds_Productos = new Ds_Alm_Com_Reporte_Productos();
                    Generar_Reporte_Productos(Dt_Consulta, Ds_Productos, Formato); // Se instancia el método que llena el DataSet
                    Div_Contenedor_Msj_Error.Visible = false;
                }
                else
                {
                    Lbl_Informacion.Text = "No se encontraron productos";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Inventarios
    ///DESCRIPCIÓN:          Metodo utilizado para llenar el Dataset e instanciar el método Generar_Reporte
    ///PARAMETROS:           1.- DataTable Dt_Consulta, Esta tabla contiene los datos de la 
    ///                          consulta que se realizó a la base de datos
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Productos(DataTable Dt_Consulta, DataSet Ds_Productos, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;
        String Usuario = Cls_Sessiones.Nombre_Empleado;
        String Cantidad_Productos = "";

        int Cont_Elementos=0;
        
        try
        {
            // Agregar detalles al DataSet
            for (Cont_Elementos = 0; Cont_Elementos < Dt_Consulta.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Consulta.Rows[Cont_Elementos]; // Instanciar renglon e importarlo
                Ds_Productos.Tables[0].ImportRow(Renglon);

                String Marca = Ds_Productos.Tables[0].Rows[Cont_Elementos]["MARCA"].ToString().Trim();
                String Descripcion = Ds_Productos.Tables[0].Rows[Cont_Elementos]["DESCRIPCION"].ToString().Trim();

                if (Marca != "")
                {
                    String Descripcion_Completa = Descripcion + ", " + Marca;
                    Ds_Productos.Tables[0].Rows[Cont_Elementos].SetField("DESCRIPCION", Descripcion_Completa);
                }
            }

            // Se asigna si es producto o son productos
            if (Dt_Consulta.Rows.Count == 1)
                Cantidad_Productos = "" + Dt_Consulta.Rows.Count.ToString().Trim() + " Producto ";
            else if (Dt_Consulta.Rows.Count > 1)
                Cantidad_Productos = "" + Dt_Consulta.Rows.Count.ToString().Trim() + " Productos ";

            Ds_Productos.Tables[0].Rows[Cont_Elementos - 1].SetField("PRODUCTOS_TOTALES", Cantidad_Productos);   // Se asigna la el numero total de productos


            Ds_Productos.Tables[0].Rows[Cont_Elementos - 1].SetField("PERSONA_GENERO", Usuario); // Se asigna la persona que generó el Reporte

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Reporte_Productos.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Productos_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Productos, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }



    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      17-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }


    
    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:          Evento utilizado por el botón  "Btn_Imprimir"
    ///PARAMETROS:                       
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           06/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "PDF";
        Consulta_Productos_Almacen(Formato);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel
    ///DESCRIPCIÓN:          Evento utilizado por el botón  "Btn_Imprimir_Excel"
    ///PARAMETROS:                       
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        Consulta_Productos_Almacen(Formato);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento  utilizadao para ir a la página principal
    ///                      de la cplicación
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Partida_Especifica
    ///DESCRIPCIÓN:          Evento  utilizadao habilitar o deshabilitar los el combo
    ///                      
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Partida_Especifica_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Partida_Especifica.Checked)
            {
                Cmb_Partidas_Especificas.Enabled = true;
            }
            else
                Cmb_Partidas_Especificas.Enabled = false;

            if (Cmb_Partidas_Especificas.Items.Count > 0)
                Cmb_Partidas_Especificas.SelectedIndex = 0;
           
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Modelo_CheckedChanged
    ///DESCRIPCIÓN:          Evento  utilizadao habilitar o deshabilitar los el combo
    ///                      
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Modelo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Modelo.Checked)
            {
                Cmb_Modelos.Enabled = true;
            }
            else
                Cmb_Modelos.Enabled = false;

            if (Cmb_Modelos.Items.Count > 0)
                Cmb_Modelos.SelectedIndex = 0;
           
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Marca_CheckedChanged
    ///DESCRIPCIÓN:          Evento  utilizadao habilitar o deshabilitar los el combo
    ///                      
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Marca_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Marca.Checked)
            {
                Cmb_Marcas.Enabled = true;
            }
            else
                Cmb_Marcas.Enabled = false;

            if (Cmb_Marcas.Items.Count > 0)
                Cmb_Marcas.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Proveedor_CheckedChanged
    ///DESCRIPCIÓN:          Evento  utilizadao habilitar o deshabilitar los el combo
    ///                      
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Proveedor_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Proveedor.Checked)
            {
                Cmb_Proveedores.Enabled = true;
            }
            else
                Cmb_Proveedores.Enabled = false;

            if (Cmb_Proveedores.Items.Count > 0)
                Cmb_Proveedores.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Descripcion_A_Z_CheckedChanged
    ///DESCRIPCIÓN:          Evento  utilizadao habilitar o deshabilitar TextBox de búsqueda
    ///                      
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           05/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Descripcion_A_Z_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Descripcion_A_Z.Checked)
            {
                Txt_Letra_Inicial.Enabled = true;
                Txt_Letra_Final.Enabled = true;
            }
            else
            {
                Txt_Letra_Inicial.Enabled = false;
                Txt_Letra_Final.Enabled = false;
            }

            Txt_Letra_Inicial.Text = "";
            Txt_Letra_Final.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    #endregion

    protected void Ckb_Productos_Stock_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Productos_Stock.Checked)
            {
                Cmb_Stock_Transitorios.Enabled = true;
            }
            else
                Cmb_Stock_Transitorios.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    protected void Cmb_Proveedores_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
