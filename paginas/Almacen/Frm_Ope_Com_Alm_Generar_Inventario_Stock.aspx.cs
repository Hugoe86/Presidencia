using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Administrar_Stock.Negocios;
using Presidencia.Sessiones;
using Presidencia.Reportes;
using Presidencia.Constantes;
using System.Collections.Generic;


public partial class paginas_Compras_Frm_Ope_Com_Alm_Generar_Inventario_Stock : System.Web.UI.Page
{

    #region Variables
    DataSet Data_Set_Consulta = new DataSet(); // Objeto creado para guardar los datos provenientes de alguna consulta
    Cls_Ope_Com_Alm_Administrar_Stock_Negocios Stock_Negocios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios(); // Objeto de la capa de Negocios
    #endregion

    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Mostrar_Inventarios_Datagrid();
            Llenar_Combo(); // Se llena el combo con los estatus de los inventarios

            // Se verifica si esta visible la búsqueda avanzada, en caso de que si este visible, se verifica si debe estar visible
            if (Btn_Busqueda_Avanzada.Visible)                                   
            {
               // Configuracion_Acceso_LinkButton("Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx");

                if (Btn_Busqueda_Avanzada.Visible)
                {
                    Cmb_Estatus.Visible = true;
                    Btn_Buscar.Visible = true;
                    Btn_Busqueda_Avanzada.Visible = true;
                }
                else
                {
                    Cmb_Estatus.Visible = false;
                    Btn_Buscar.Visible = false;
                    Btn_Busqueda_Avanzada.Visible = false;
                }
            }

            if (Btn_Nuevo.Visible)
            {
               // Configuracion_Acceso("Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx");
            }
        }
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Inventarios_Datagrid
    ///DESCRIPCIÓN:          Metodo utilizado para llenar el dataGrid
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Inventarios_Datagrid()
    {
        DataTable Table_Inventarios = null;
       
        try
        { 
            Stock_Negocios.P_Tipo_DataTable = "TODOS_INVENTARIOS";
            Table_Inventarios = Stock_Negocios.Consultar_DataTable();

            if (Table_Inventarios.Rows.Count > 0)
            {
                Session["Table_Inventarios"] = Table_Inventarios;
                Grid_Inventario_Stock.DataSource = Table_Inventarios;
                Grid_Inventario_Stock.DataBind();
                Grid_Inventario_Stock.Visible = true;
                Lbl_Inventarios.Visible = true;
            }
            else
            {
                Lbl_Mensaje_Error.Text = "No se encontraron inventarios";
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Inventarios.Visible = false;
            }
         }
         catch (Exception Ex)
         {
             throw new Exception("Error al mostrar los inventarios. Error: [" + Ex.Message + "]");
         }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN:          Metodo que llena el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo()
    {
        if (Cmb_Estatus.Items.Count == 0)
        {
            Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
            Cmb_Estatus.Items.Add("PENDIENTE");
            Cmb_Estatus.Items.Add("CAPTURADO");
            Cmb_Estatus.Items.Add("APLICADO");
            Cmb_Estatus.Items.Add("CANCELADO");
            Cmb_Estatus.Items[0].Value = "0";
            Cmb_Estatus.Items[0].Selected = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Activar_Inventario_Selectivo
    ///DESCRIPCIÓN:          Metodo utilizado para activar y desactivar los CheckBox para generar el inventario selectivo
    ///PARAMETROS:           Estatus: valor que indica si se activan o desactivan los CheckBox
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Activar_Inventario_Selectivo(Boolean Estatus)
    {
        if (Estatus) 
        {
            Ckb_Familia.Enabled = true;
            Ckb_Subfamilia.Enabled = true;
            Ckb_Marca.Enabled = true;
            Ckb_A_Z.Enabled = true;

            Ckb_Familia.Checked= false;
            Ckb_Subfamilia.Checked = false;
            Ckb_Marca.Checked = false;
            Ckb_A_Z.Checked = false;
        }
        else
        {
            Ckb_Inventario_Fisico.Enabled = true;
            Ckb_Inventario_Selectivo.Enabled= true;
            Ckb_Inventario_Fisico.Checked = false;
            Ckb_Inventario_Selectivo.Checked = false;

            Ckb_Familia.Enabled = false;
            Ckb_Subfamilia.Enabled = false;
            Ckb_Marca.Enabled = false;
            Ckb_A_Z.Enabled = false;

            Cmb_Familia.Enabled = false;
            Cmb_Subfamilia.Enabled = false;
            Cmb_Marca.Enabled = false;

            Txt_Letra_Inicial.Text = "";
            Txt_Letra_Inicial.Enabled = false;
            Txt_Letra_Final.Text = "";
            Txt_Letra_Final.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Componentes
    ///DESCRIPCIÓN:          Metodo utilizado para poner visible u ocultar el datagri y el lbl_No_Inventario
    ///PARAMETROS:           Estatus Es un valor que indica si se activan o desactivan los CheckBox
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Componentes(Boolean Estado)
    {
        if (Estado)
        {
            Grid_Productos_Inventario.Visible = true;
            Lbl_No_Inventario.Visible = true;
            Grid_Productos_Inventario.Visible = true;
            
        }
        else
        {
            Grid_Productos_Inventario.Visible = false;
            Lbl_No_Inventario.Visible = false;
            Grid_Productos_Inventario.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validacion_Inventario_Selectivo
    ///DESCRIPCIÓN:          Hace una validacion de que se hayan seleccionado los combos correspondientes al inventario selectivo       
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           18/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public bool Validacion_Inventario_Selectivo()
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        String Mensaje_Error = "";
        Boolean Validacion = true;

        if (Ckb_Inventario_Selectivo.Checked)
        {

            Boolean Inv_Selectivo = true;

            if (Ckb_Familia.Checked)
            {
                if (Cmb_Familia.SelectedIndex == 0)
                {
                    if (!Validacion)
                    {
                        Mensaje_Error = Mensaje_Error + "<br>";
                    }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del combo Partida Genéral.";
                    Validacion = false;
                }
                Inv_Selectivo = false;
            }

            if (Ckb_Subfamilia.Checked)
            {
                if (Cmb_Subfamilia.SelectedIndex == 0)
                {
                    if (!Validacion)
                    {
                        Mensaje_Error = Mensaje_Error + "<br>";
                    }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del combo Partida Específica.";
                    Validacion = false;
                }
                Inv_Selectivo = false;
            }

            if (Ckb_Marca.Checked)
            {
                if (Cmb_Marca.SelectedIndex == 0)
                {
                    if (!Validacion)
                    {
                        Mensaje_Error = Mensaje_Error + "<br>";
                    }
                    Mensaje_Error = Mensaje_Error + "+ Debe seleccionar una opción del combo de Marca.";
                    Validacion = false;
                }
                Inv_Selectivo = false;
            }

            if (Ckb_A_Z.Checked)
            {
                if ((Txt_Letra_Inicial.Text == "") | (Txt_Letra_Final.Text == ""))
                {
                    if (!Validacion)
                    {
                        Mensaje_Error = Mensaje_Error + "<br>";
                    }
                    Mensaje_Error = Mensaje_Error + "+ Debe asignar una Letra Inicial y una Letra Final.";
                    Validacion = false;
                }
                Inv_Selectivo = false;
            }
            if (Inv_Selectivo == true)
            {
                Mensaje_Error = Mensaje_Error + "+ Debe seleccionar por lo menos un combo.";
                Validacion = false;
            }
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Validacion;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validacion_Productos_Inventario
    ///DESCRIPCIÓN:          Hace una validacion de que los productos que se vallan agregar al inventario 
    ///                      no formen parte de un inventario en estado "Pendiente o CAPTURADO"       
    ///PROPIEDADES:          Tabla_DataGrid: Es el DataGrid que contiene los productos que se van a guardardar 
    ///                      en el inventario
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           18/Enero/2011 
    ///MODIFICO:             Salvador Hernández Ramírez
    ///FECHA_MODIFICO        11/Mayo/2011 
    ///CAUSA_MODIFICACIÓN    Se mostró la clave  del producto y el inventario, estos datos son mostrados 
    ///                      cuando un un producto forma parte de un inventario ya que esta pendiente o capturado
    ///*******************************************************************************
    public bool Validacion_Productos_Inventario(DataTable Tabla_DataGrid)
    {
        String Mensaje_Error = "";
        Boolean Validacion = true;

      try
        { 
            for (int i = 0; i < Tabla_DataGrid.Rows.Count; i++)
            {
                DataTable Data_Table_Temporal = new DataTable(); // En esta parte se consultan los inventarios que hay en esa tabla

                Stock_Negocios.P_Producto_ID = Tabla_DataGrid.Rows[i]["Producto_ID"].ToString();
                Data_Table_Temporal = Stock_Negocios.Consulta_Productos_En_Inventarios();
                for (int j = 0; j < Data_Table_Temporal.Rows.Count; j++)
                {
                    if ((Tabla_DataGrid.Rows[i]["Producto_ID"].ToString() == Data_Table_Temporal.Rows[j]["Producto_ID"].ToString()) && ((Data_Table_Temporal.Rows[j]["ESTATUS"].ToString() == "PENDIENTE") | (Data_Table_Temporal.Rows[j]["ESTATUS"].ToString() == "CAPTURADO")))
                    {
                        Mensaje_Error = " El Producto " + Data_Table_Temporal.Rows[j]["CLAVE"].ToString() + " forma parte del inventario  " + Data_Table_Temporal.Rows[j]["NO_INVENTARIO"] + " con estatus " + Data_Table_Temporal.Rows[j]["ESTATUS"];
                        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                        Validacion = false;
                        return Validacion;
                    }
                }
            }
            return Validacion;
          }
          catch (Exception Ex)
          {
              throw new Exception("Error en la validación de los productos. Error: [" + Ex.Message + "]");
          }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_DataSet_Inventarios_Stock
    ///DESCRIPCIÓN:          Llena el dataSet "Data_Set_Consulta_Inventario" con los productos
    ///                      que pertenecen al inventario seleccionado por el usuario
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_DataSet_Inventarios_Stock()
    {
      try
        {
            String Formato = "PDF";

            Stock_Negocios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();

            if (Session["No_Inventario"] != null)
            Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();

            DataSet Data_Set_Consulta_Inventario;
            Data_Set_Consulta_Inventario = Stock_Negocios.Consulta_Inventarios_General();
            Ds_Alm_Com_Inventario_Stock Ds_Reporte_Stock = new Ds_Alm_Com_Inventario_Stock();
            Generar_Reporte(Data_Set_Consulta_Inventario, Ds_Reporte_Stock, "Rpt_Alm_Com_Rep_Generacion_Stock.rpt", Formato);
        }
         catch (Exception Ex)
         {
              throw new Exception("Error aa. Error: [" + Ex.Message + "]");
         }
     }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_Inventario.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte_Stock.- Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte_Crystal.- contiene el nombre del Reporte Crustal
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           14/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_Inventario, DataSet Ds_Reporte_Stock, String Nombre_Reporte_Crystal, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataRow Renglon;

        try
        {
            // Se llena la tabla Cabecera del DataSet
            Renglon = Data_Set_Consulta_Inventario.Tables[0].Rows[0];
            Ds_Reporte_Stock.Tables[1].ImportRow(Renglon);

            // Se llena la tabla Detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Data_Set_Consulta_Inventario.Tables[0].Rows.Count; Cont_Elementos++)
            {
                Renglon = Data_Set_Consulta_Inventario.Tables[0].Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte_Stock.Tables[0].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/" + Nombre_Reporte_Crystal;


            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Inventario_Stock_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Stock, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte( Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al llenar el DataSet. Error: [" + Ex.Message + "]");
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
    /// FECHA MODIFICO:      16-Mayo-2011
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



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN:          Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///                      1.-Estatus. Estatus en el que se cargara la configuración de los 
    ///                         controles.
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           13/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Nuevo.AlternateText = "Consultar";
            Btn_Nuevo.ToolTip = "Consultar";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_consultar.png";
            Btn_Salir.ToolTip = "Salir";
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            lbl_Observaciones.Visible = false;
            Txt_Observaciones.Visible = false;
            Mostrar_Busqueda(true);
        }
        else
        {
            Btn_Salir.ToolTip = "Cancelar";
            Btn_Salir.AlternateText = "Cancelar";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

            lbl_Observaciones.Visible = true;
            Txt_Observaciones.Visible = true;
            Txt_Observaciones.Enabled = true;
            Mostrar_Busqueda(false);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simble y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Cmb_Estatus.Visible = Estatus;
        Btn_Buscar.Visible = Estatus;
        Btn_Busqueda_Avanzada.Visible = Estatus;


        // Se verifica si esta visible la búsqueda avanzada, en caso de que si este visible, se verifica si debe estar visible
        if (Btn_Busqueda_Avanzada.Visible)
        {
            //Configuracion_Acceso_LinkButton("Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx");

            if (Btn_Busqueda_Avanzada.Visible)
            {
                Cmb_Estatus.Visible = true;
                Btn_Buscar.Visible = true;
                Btn_Busqueda_Avanzada.Visible = true;
            }
            else
            {
                Cmb_Estatus.Visible = false;
                Btn_Buscar.Visible = false;
                Btn_Busqueda_Avanzada.Visible = false;
            }
        }

        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Botones_Estado_Inicial
    ///DESCRIPCIÓN:          Metodo utilizado asignarles la Imagen  y el AlternativeText a los botones igual que su estado inicial
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Botones_Estado_Inicial()
    {
        Btn_Nuevo.AlternateText = "NUEVO";
        Btn_Nuevo.ToolTip = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Salir.ToolTip = "Salir";
        Btn_Salir.AlternateText = "Salir";
        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        Btn_Nuevo.Visible = true;
        Btn_Guardar.Visible = false;
        Mostrar_Busqueda(true);

        if (Btn_Nuevo.Visible)
        {
           // Configuracion_Acceso("Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: ChekBox_Estado_Inicial
    ///DESCRIPCIÓN:          Metodo utilizado asignarles  los CheckBox a su estado inicial
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void ChekBox_Estado_Inicial()
    {
        Ckb_Inventario_Fisico.Enabled = true;
        Ckb_Inventario_Selectivo.Enabled = true;
        Ckb_Inventario_Fisico.Checked = false;
        Ckb_Inventario_Selectivo.Checked = false;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus_Busqueda
    ///DESCRIPCIÓN:          Metodo que valida que seleccione un estatus dentro del modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Estatus_Busqueda()
    {
        if (Chk_Estatus_B.Checked == true)
        {
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            {
                Stock_Negocios.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue;
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Debe seleccionar el Estatus <br />";
            }
        }
        else
        {
            Stock_Negocios.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue;
        }

    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();

        if (Chk_Fecha_B.Checked == true)
        {
            if ((Txt_Fecha_Inicial_B.Text.Length == 11) && (Txt_Fecha_Final_B.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial_B.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final_B.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Stock_Negocios.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial_B.Text);
                    Stock_Negocios.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final_B.Text);
                }
                else
                {
                    Img_Error_Busqueda.Visible = true;
                    Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
                }
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Diciembra/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Carga_Componentes_Busqueda
    ///DESCRIPCIÓN:          Metodo que carga e inicializa los componentes del ModalPopUp
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Carga_Componentes_Busqueda()
    {
        Llenar_Combo_Estatus_Busqueda();
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";

        Chk_Estatus_B.Checked = false;
        Cmb_Estatus_Busqueda.Enabled = false;
        Cmb_Estatus_Busqueda.SelectedIndex = 0;

        Chk_Fecha_B.Checked = false;
        Btn_Calendar_Fecha_Inicial.Enabled = false;
        Btn_Calendar_Fecha_Final.Enabled = false;
        Modal_Busqueda.Show();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus_Busqueda
    ///DESCRIPCIÓN:          Metodo que llena el combo Cmb_Estatus_Busqueda
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus_Busqueda()
    {
        if (Cmb_Estatus_Busqueda.Items.Count == 0)
        {
            Cmb_Estatus_Busqueda.Items.Add("<<SELECCIONAR>>");
            Cmb_Estatus_Busqueda.Items.Add("PENDIENTE");
            Cmb_Estatus_Busqueda.Items.Add("CAPTURADO");
            Cmb_Estatus_Busqueda.Items.Add("APLICADO");
            Cmb_Estatus_Busqueda.Items.Add("CANCELADO");
            Cmb_Estatus_Busqueda.Items[0].Value = "0";
            Cmb_Estatus_Busqueda.Items[0].Selected = true;
        }
    }


    public void Estatus_Componentes_Generar_Inventario()
    {
        Ckb_Inventario_Fisico.Enabled=false;
        Ckb_Inventario_Selectivo.Enabled=false;
        Ckb_Familia.Enabled=false;   
        Ckb_Subfamilia.Enabled=false;
        Ckb_Marca.Enabled=false;
        Ckb_A_Z.Enabled=false;
        Cmb_Familia.Enabled = false;
        Cmb_Subfamilia.Enabled = false;
        Cmb_Marca.Enabled = false;
        Txt_Letra_Inicial.Enabled = false;
        Txt_Letra_Final.Enabled = false;
    }
    #endregion 

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Consultar_Click
    ///DESCRIPCIÓN:          Maneja el evento del Botón para Consultar, guardar o Cancelar los inventarios del stock de almacen     
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           07/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        String No_Inventario="";
        String Tipo_Inventario="";
        Boolean MostrarGrid = false;
        Stock_Negocios = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios(); // Objeto de la capa de Negocios

        try
        {
            if (Btn_Nuevo.AlternateText == "NUEVO")
            {
                Div_Generar_Inventario.Visible = true;
                Lbl_No_Inventario.Visible = false;
                Grid_Productos_Inventario.Visible = false;
                Grid_Inventario_Stock.Visible = false;
                Configuracion_Formulario(true);
                Btn_Salir.ToolTip = "Atras";
                Btn_Salir.AlternateText = "Atras";
                Lbl_Inventarios.Visible = false;
                Txt_Observaciones.Text = "";
                Mostrar_Busqueda(false);
            }
            else if (Btn_Nuevo.AlternateText == "Consultar")
            {
                 if (Ckb_Inventario_Fisico.Checked == true){
                    Data_Set_Consulta = Stock_Negocios.Consulta_Inventario_Fisico();
                    if (Data_Set_Consulta.Tables[0].Rows.Count > 0){
                        Estado_Componentes(true);
                        No_Inventario = Stock_Negocios.Obtener_Id_Consecutivo("NO_INVENTARIO", "OPE_COM_CAP_INV_STOCK");
                        Lbl_No_Inventario.Text = "No. Inventario Aproximado." + " " + No_Inventario;
                        Tipo_Inventario = "Fisico";
                        Lbl_No_Inventario.Visible = true;
                        Configuracion_Formulario(false);
                        Stock_Negocios.P_No_Inventario = No_Inventario;
                        MostrarGrid = true;
                        Div_Contenedor_Msj_Error.Visible = false;
                        Txt_Observaciones.Text = "";

                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = " No se encontraron productos";
                        Div_Contenedor_Msj_Error.Visible = true;
                        Configuracion_Formulario(true);
                        Ckb_Inventario_Fisico.Enabled = true;
                        Ckb_Inventario_Selectivo.Enabled = true;
                        MostrarGrid = false;
                        Btn_Nuevo.Visible = false;
                    }
                }else if (Ckb_Inventario_Selectivo.Checked == true){
                    
                    if (Validacion_Inventario_Selectivo())
                    {
                        if ((Cmb_Familia.SelectedIndex > 0) && (Ckb_Familia.Checked))
                        {
                            Stock_Negocios.P_Familia_ID = Cmb_Familia.SelectedItem.Value;
                            //Tipo_Inventario = "Familia:" + " " + Cmb_Familia.SelectedItem.Text.Trim() + "       ";
                            Tipo_Inventario = "Partida Genérica   " + "       ";
                            //Tipo_Inventario = " Partida Genérica" + "\n";
                        }

                        if ((Cmb_Subfamilia.SelectedIndex > 0) && (Ckb_Subfamilia.Checked))
                        {
                            Stock_Negocios.P_Subfamilia_ID = Cmb_Subfamilia.SelectedItem.Value;
                            //Tipo_Inventario = Tipo_Inventario + "  Subfamilia:" + " " + Cmb_Subfamilia.SelectedItem.Text.Trim() + "   ";
                            Tipo_Inventario = Tipo_Inventario + "Partida Específica " + "       ";
                            //Tipo_Inventario = Tipo_Inventario + " Partida Específica " + "\n";
                        }

                        if ((Txt_Letra_Inicial.Text.Trim() != "") && (Txt_Letra_Final.Text.Trim() != "") && (Ckb_A_Z.Checked))
                        {
                            Stock_Negocios.P_Letra_Inicio = Txt_Letra_Inicial.Text.Trim();
                            Stock_Negocios.P_Letra_Fin = Txt_Letra_Final.Text.Trim();
                            Tipo_Inventario = Tipo_Inventario + "De la: " + " " + Txt_Letra_Inicial.Text + " a la: " + " " + Txt_Letra_Final.Text + "      " + "";
                            //Tipo_Inventario = Tipo_Inventario + "  De la: " + " " + Txt_Letra_Inicial.Text + " a la: " + " " + Txt_Letra_Final.Text + "\n";
                        }
                        if ((Cmb_Marca.SelectedIndex > 0) && (Ckb_Marca.Checked))
                        {
                            Stock_Negocios.P_Marca_ID = Cmb_Marca.SelectedItem.Value;
                            Tipo_Inventario = Tipo_Inventario + "Marca:" + " " + Cmb_Marca.SelectedItem.Text.Trim()+ " ";
                        }
                        Data_Set_Consulta = Stock_Negocios.Consulta_Inventario_Selectivo();

                        if (Data_Set_Consulta.Tables[0].Rows.Count > 0)
                        {
                            No_Inventario = Stock_Negocios.Obtener_Id_Consecutivo("NO_INVENTARIO", "OPE_COM_CAP_INV_STOCK");

                            Lbl_No_Inventario.Text = "No. Inventario Aproximado" + " " + No_Inventario;
                            Lbl_No_Inventario.Visible = true;
                            Configuracion_Formulario(false);
                            Stock_Negocios.P_No_Inventario = No_Inventario;
                            Div_Contenedor_Msj_Error.Visible = false;
                            MostrarGrid = true;
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text= "No se encontraron productos";
                            Div_Contenedor_Msj_Error.Visible = true;
                            Configuracion_Formulario(true);
                            MostrarGrid = false;
                        }
                   }
                }

                if (MostrarGrid == true)
                {
                    Grid_Productos_Inventario.DataSource = Data_Set_Consulta;
                    Grid_Productos_Inventario.DataBind();
                    Estado_Componentes(true);
                    Session["Productos_Inventario"] = null;
                    Session["Productos_Inventario"] = Data_Set_Consulta.Tables[0];
                    Session["No_Inventario"] = No_Inventario;
                    Session["Tipo_Inventario"] = Tipo_Inventario.Trim();
                    Estatus_Componentes_Generar_Inventario();
                    Grid_Productos_Inventario.Visible = true;
                    Btn_Guardar.Visible=true;
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                }
             }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el inventario. Error: [" + Ex.Message + "]");
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Maneja el evento del Botón para redireccionar a la página principal o a esta misma página
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           07/Enero/2011 
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
        else if (Btn_Salir.AlternateText.Equals("Atras") | Btn_Salir.AlternateText.Equals("Cancelar"))
        {
            Div_Generar_Inventario.Visible = false;

            Grid_Productos_Inventario.Visible = false;
            Div_Contenedor_Msj_Error.Visible = false;
            Configuracion_Formulario(true);
            Btn_Nuevo.Visible = true;
            Btn_Guardar.Visible = false;
            Activar_Inventario_Selectivo(false);
            Lbl_No_Inventario.Visible = false;
            Botones_Estado_Inicial();
            ChekBox_Estado_Inicial();
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_A_Z_CheckedChanged
    ///DESCRIPCIÓN:          Maneja el evento para habilitar o deshabilitar los TexBox  " Txt_Letra_Inicial y Txt_Letra_Final "
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Ckb_A_Z_CheckedChanged(object sender, EventArgs e)
    {
        if (Ckb_A_Z.Checked)
        {
            Txt_Letra_Inicial.Enabled = true;
            Txt_Letra_Final.Enabled = true;
        }
        else
        {
            Txt_Letra_Inicial.Enabled = false;
            Txt_Letra_Final.Enabled = false;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Inventario_Fisico_CheckedChanged
    ///DESCRIPCIÓN:          Maneja el evento para activar el inventario selectivo y/o para configuracion del formulario     
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Ckb_Inventario_Fisico_CheckedChanged(object sender, EventArgs e)
    {
        if (Ckb_Inventario_Fisico.Checked)
        {
            Ckb_Inventario_Selectivo.Checked = false;

            Ckb_Familia.Enabled = false;
            Ckb_Subfamilia.Enabled = false;
            Ckb_Marca.Enabled = false;
            Ckb_A_Z.Enabled = false;

            Cmb_Familia.Enabled = false;
            Cmb_Subfamilia.Enabled = false;
            Cmb_Marca.Enabled = false;

            Txt_Letra_Inicial.Text = "";
            Txt_Letra_Inicial.Enabled = false;
            Txt_Letra_Final.Text = "";
            Txt_Letra_Final.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Inventario_Stock_SelectedIndexChanged
    ///DESCRIPCIÓN:          Maneja el evento para obtener del datagrid el registro seleccionado y activar el boton para la posible impresion
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inventario_Stock_SelectedIndexChanged(object sender, EventArgs e)
    {
            GridViewRow selectedRow = Grid_Inventario_Stock.Rows[Grid_Inventario_Stock.SelectedIndex];//GridViewRow representa una fila individual de un control gridview
            String No_Inventario = Convert.ToString(selectedRow.Cells[1].Text);
            String Estatus = Convert.ToString(selectedRow.Cells[4].Text);
            String Mostrar_Botones = "true";

        if (!Btn_Nuevo.Visible)
            Mostrar_Botones = "false";

        String Paginas = Request.QueryString["PAGINA"].Trim();
        Response.Redirect("Frm_Ope_Com_Alm_Mostrar_Productos_Inv_Stock.aspx?No_Inventario=" + HttpUtility.HtmlEncode(No_Inventario) + "&Estatus=" + HttpUtility.HtmlEncode(Estatus) + "&Mostrar_Botones=" + HttpUtility.HtmlEncode(Mostrar_Botones) + "&Pagina_GI=" + HttpUtility.HtmlEncode(Paginas)); // Nota: Al poner un TryCatch en este método marca error al redireccionar
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Inventario_Stock_PageIndexChanging
    ///DESCRIPCIÓN:          Maneja el evento para llenar las siguientes páginas del grid con la informaciçon de la consulta
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Inventario_Stock_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Inventario_Stock.PageIndex = e.NewPageIndex;
        if (Session["Table_Inventarios"] != null)
        Grid_Inventario_Stock.DataSource = (DataTable)Session["Table_Inventarios"];
        Grid_Inventario_Stock.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_Inventario_PageIndexChanging
    ///DESCRIPCIÓN:          Maneja el evento para llenar las siguientes páginas del grid con la información de la consulta
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Productos_Inventario_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos_Inventario.PageIndex = e.NewPageIndex;

        if (Session["Productos_Inventario"] != null)
        Grid_Productos_Inventario.DataSource = (DataTable)Session["Productos_Inventario"];

        Grid_Productos_Inventario.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click1
    ///DESCRIPCIÓN:          Maneja el evento para realizar una busqueda simple en base al Estatus del Inventario
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click1(object sender, ImageClickEventArgs e)
    {
        Botones_Estado_Inicial();
        ChekBox_Estado_Inicial();
        Div_Generar_Inventario.Visible = false;
        Grid_Productos_Inventario.Visible = false;
        Div_Contenedor_Msj_Error.Visible = false;
        DataTable Table_Inventarios = null;

         try
         { 

            Stock_Negocios.P_Estatus = Cmb_Estatus.SelectedValue.ToString().Trim();
            Table_Inventarios = Stock_Negocios.Busqueda_Simple();
            if (Table_Inventarios.Rows.Count != 0)
            {
                Grid_Inventario_Stock.DataSource = Table_Inventarios;
                Session["Table_Inventarios"] = Table_Inventarios;
                Grid_Inventario_Stock.DataBind();
                Lbl_Inventarios.Text = "Inventarios";
                Lbl_Inventarios.Visible = true;
                Grid_Inventario_Stock.Visible = true;
                Lbl_No_Inventario.Text = "";
            }
            else
            {
                Lbl_Mensaje_Error.Text = " No se encontraron inventarios";
                Lbl_No_Inventario.Text = "";
                Lbl_Inventarios.Visible = false;
                Div_Contenedor_Msj_Error.Visible = true;
                Grid_Inventario_Stock.DataBind();
                Grid_Inventario_Stock.Visible = false;
            }
         }
         catch (Exception Ex)
         {
             throw new Exception("Error al realizar la búsqueda simple. Error: [" + Ex.Message + "]");
         }
    }


    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Click
    ///DESCRIPCIÓN:          Evento del boton Aceptar del ModalPopUp, utilizado para mostrar los inventarios en base a la busqueda abanzada
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        Botones_Estado_Inicial();
        ChekBox_Estado_Inicial();
        Div_Generar_Inventario.Visible = false;
        Grid_Productos_Inventario.Visible = false;
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
       
      try
        { 
            Validar_Estatus_Busqueda();
            Verificar_Fecha();

            if (Img_Error_Busqueda.Visible == false)
            {
                DataTable Tabla_Inventarios = null;
                Tabla_Inventarios = Stock_Negocios.Busqueda_Avanzada_Inventarios();

                if (Tabla_Inventarios.Rows.Count != 0)
                {
                    Grid_Inventario_Stock.DataSource = Tabla_Inventarios;
                    Session["Table_Inventarios"] = Tabla_Inventarios;
                    Grid_Inventario_Stock.DataBind();
                    Grid_Inventario_Stock.Visible = true;
                    Modal_Busqueda.Hide();
                    Lbl_No_Inventario.Text = "";
                }
                else
                {
                    if ((Chk_Estatus_B.Checked == true) | (Chk_Fecha_B.Checked == true))
                    {
                        Lbl_Error_Busqueda.Text = "+ No se encontraron datos <br />";
                    }
                    else
                    {
                        Lbl_Error_Busqueda.Text = "+ Seleccionar como mínimo un criterio de búsqueda <br />";
                    }
                    Stock_Negocios.P_Fecha_Inicial = null;
                    Stock_Negocios.P_Fecha_Final = null;
                    Modal_Busqueda.Show();
                }
            }
            else
            {
            }
        }
        catch (Exception Ex)
        {
              throw new Exception("Error al realizar la búsqueda avanzada. Error: [" + Ex.Message + "]");
        }
    }

    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN:          Evento.- Mostrar el panel que contiene los componentes para realizar la busqueda
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           20/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Modal_Busqueda.Show();
        Carga_Componentes_Busqueda();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_subfamilia_CheckedChanged
    ///DESCRIPCIÓN:          Valida que cuando se selecciona el filtro para generar el inventario 
    ///                      por subfamilia,se habilite el "Cmb_Subfamilia"
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_subfamilia_CheckedChanged(object sender, EventArgs e)
    {
        Cls_Ope_Com_Alm_Administrar_Stock_Negocios Subfamilia = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();
        DataTable Data_Table_Combo = new DataTable();
        if (Ckb_Subfamilia.Checked)
        {
            Cmb_Subfamilia.Enabled = true;
            Subfamilia.P_Tipo_DataTable = "SUBFAMILIA";
            Data_Table_Combo = Subfamilia.Consultar_DataTable();
           
            DataRow Fila_Subfamilia = Data_Table_Combo.NewRow();
            Fila_Subfamilia["SUBFAMILIA_ID"] = "SELECCIONE";
            Fila_Subfamilia["SUBFAMILIA"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            
            Data_Table_Combo.Rows.InsertAt(Fila_Subfamilia, 0);
            Cmb_Subfamilia.DataSource = Data_Table_Combo;
            Cmb_Subfamilia.DataValueField = "SUBFAMILIA_ID";
            Cmb_Subfamilia.DataTextField = "SUBFAMILIA";
            Cmb_Subfamilia.DataBind();

            // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
            if (Cmb_Subfamilia != null)
            foreach (ListItem li in Cmb_Subfamilia.Items)
            li.Attributes.Add("title", li.Text); 
        }
        else
        {
            Cmb_Subfamilia.SelectedIndex = 0;
            Cmb_Subfamilia.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Marca_CheckedChanged
    ///DESCRIPCIÓN:          Valida que cuando se selecciona el filtro para generar el inventario 
    ///                      por marca,se habilite el "Cmb_Marca"
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Marca_CheckedChanged(object sender, EventArgs e)
    {
        Cls_Ope_Com_Alm_Administrar_Stock_Negocios Marca = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();
        DataTable Data_Table_Combo = new DataTable();

        if (Ckb_Marca.Checked)
        {
            Cmb_Marca.Enabled = true;
            Marca.P_Tipo_DataTable = "MARCAS";
            Data_Table_Combo = Marca.Consultar_DataTable();
            DataRow Fila_Marca = Data_Table_Combo.NewRow();
            Fila_Marca["MARCA_ID"] = "SELECCIONE";
            Fila_Marca["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Data_Table_Combo.Rows.InsertAt(Fila_Marca, 0);
            
            Cmb_Marca.DataSource = Data_Table_Combo;
            Cmb_Marca.DataValueField = "MARCA_ID";
            Cmb_Marca.DataTextField = "NOMBRE";
            Cmb_Marca.DataBind();

                // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
                if (Cmb_Marca != null)
                foreach (ListItem li in Cmb_Marca.Items)
                li.Attributes.Add("title", li.Text); 
        }
        else
        {
            Cmb_Marca.SelectedIndex = 0;
            Cmb_Marca.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Inventario_Selectivo_CheckedChanged
    ///DESCRIPCIÓN:          Valida que cuando se selecciona el inventario selectivo,
    ///                      se habiliten o se deshabiliten sus criterios para generar el invenario.
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Inventario_Selectivo_CheckedChanged(object sender, EventArgs e)
    {
        if (Ckb_Inventario_Selectivo.Checked)
        {
            Activar_Inventario_Selectivo(true);
            Ckb_Inventario_Fisico.Checked = false;
        }
        else
        {
            Activar_Inventario_Selectivo(false);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Guardar_Click
    ///DESCRIPCIÓN:          Evento click del boton "Btn_Guardar_Click", el cual es utilizado para guardar el inventario generado
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Validacion_Productos_Inventario((DataTable)Session["Productos_Inventario"]))
        {
            Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();
            Stock_Negocios.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            Stock_Negocios.P_Estatus = "PENDIENTE";
            Stock_Negocios.P_Tipo = Session["Tipo_Inventario"].ToString();
            Stock_Negocios.P_Observaciones = Txt_Observaciones.Text.Trim();
            Stock_Negocios.P_Inventario_Stock = (DataTable)Session["Productos_Inventario"];
            String Resultado = Stock_Negocios.Guardar_Inventario();

            while (Resultado == "00001") // Se realiza el While para obtener el No. De inventario Consecutivo, estos e realiza en caso de que el No. Invetnario que se esta agregando ya exite en la tabla
            {
                Session["No_Inventario"] = "" + Stock_Negocios.Obtener_Id_Consecutivo("NO_INVENTARIO", "OPE_COM_CAP_INV_STOCK");
                Stock_Negocios.P_No_Inventario = Session["No_Inventario"].ToString();
                Stock_Negocios.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                Stock_Negocios.P_Estatus = "PENDIENTE";
                Stock_Negocios.P_Tipo = Session["Tipo_Inventario"].ToString();
                Stock_Negocios.P_Observaciones = Txt_Observaciones.Text.Trim();
                Stock_Negocios.P_Inventario_Stock = (DataTable)Session["Productos_Inventario"];
                Resultado = Stock_Negocios.Guardar_Inventario();
            }

            Lbl_No_Inventario.Text = " No. Inventario  " + Session["No_Inventario"].ToString();
            Botones_Estado_Inicial();
            ChekBox_Estado_Inicial();
            lbl_Observaciones.Visible = false;
            Txt_Observaciones.Visible = false;
            Div_Generar_Inventario.Visible = false;
            Llenar_DataSet_Inventarios_Stock();
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Txt_Observaciones.Text = "";
        } 
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Panel_Click
    ///DESCRIPCIÓN:          Evento click del boton "Btn_Cancelar_Panel", el cual es utilizado para cancelar el proceso de busqueda abanzada
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cancelar_Panel_Click(object sender, EventArgs e)
    {
           Modal_Busqueda.Hide();
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_B_CheckedChanged
    ///DESCRIPCIÓN:          Evento click del boton "Chk_Fecha_B", el cual habilitar  o deshabilitar los controles del panel de busqueda abanzada
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_B_CheckedChanged(object sender, EventArgs e)
    {
         if (Chk_Fecha_B.Checked == true)
        {
            Txt_Fecha_Inicial_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Btn_Calendar_Fecha_Inicial.Enabled = true;
            Btn_Calendar_Fecha_Final.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicial_B.Text = "";
            Txt_Fecha_Final_B.Text = "";
            Btn_Calendar_Fecha_Inicial.Enabled = false;
            Btn_Calendar_Fecha_Final.Enabled = false;
        }
        Modal_Busqueda.Show();
    }

    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_B_CheckedChanged
    ///DESCRIPCIÓN:          Evento click del boton "Chk_Estatus_B", es utilizado para habilitar  o deshabilitar los controles del panel de busqueda abanzada
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Estatus_B_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Estatus_B.Checked == true)
        {
            Cmb_Estatus_Busqueda.Enabled = true;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
        }
        else
        {
            Cmb_Estatus_Busqueda.Enabled = false;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
        }
        Modal_Busqueda.Show();
    }


    //*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ckb_Familia_CheckedChanged1
    ///DESCRIPCIÓN:          Evento click del boton "Ckb_Familia", es utilizado para habilitar  o deshabilitar los controles del panel de busqueda abanzada
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernàndez Ramírez
    ///FECHA_CREO:           15/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Ckb_Familia_CheckedChanged1(object sender, EventArgs e)
    {
        Cls_Ope_Com_Alm_Administrar_Stock_Negocios Familia = new Cls_Ope_Com_Alm_Administrar_Stock_Negocios();
        DataTable Data_Table_Combo = new DataTable();
        if (Ckb_Familia.Checked)
        {
            Cmb_Familia.Enabled = true;
            Familia.P_Tipo_DataTable = "FAMILIA";
            Data_Table_Combo = Familia.Consultar_DataTable();
            
            DataRow Fila_Familia = Data_Table_Combo.NewRow();
            Fila_Familia["FAMILIA_ID"] = "SELECCIONE";
            Fila_Familia["FAMILIA"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Data_Table_Combo.Rows.InsertAt(Fila_Familia, 0);
            
            Cmb_Familia.DataSource = Data_Table_Combo;
            Cmb_Familia.DataValueField = "FAMILIA_ID";
            Cmb_Familia.DataTextField = "FAMILIA";
            Cmb_Familia.DataBind();

            // Se le agrega un ToolTip a cada elemento del combo, ya que los valores estan muy grandes.
            if (Cmb_Familia != null)
            foreach (ListItem li in Cmb_Familia.Items)
            li.Attributes.Add("title",li.Text); 
        }
        else
        {
            Cmb_Familia.SelectedIndex = 0;
            Cmb_Familia.Enabled = false;
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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]); // Habilitamos la configuracón de los botones.
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

    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
    {
        List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Busqueda_Avanzada);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menú de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        // Validamos que el menu consultado corresponda a la página a validar.
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
            throw new Exception("Error al validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

}

