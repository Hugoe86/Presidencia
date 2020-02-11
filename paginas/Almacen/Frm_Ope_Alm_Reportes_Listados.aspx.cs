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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Reportes_Listados_Productos.Negocio;
using Presidencia.Reportes;


public partial class paginas_Almacen_Frm_Ope_Alm_Reportes_Listados : System.Web.UI.Page
{

    #region Variables
    Cls_Ope_Com_Alm_Rpts_Listados_Productos_Negocio Listados_Productos  = new Cls_Ope_Com_Alm_Rpts_Listados_Productos_Negocio();
    #endregion

    #region Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Llenar_Combo_Estatus();
        }
    }
    
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Checkbox
    ///DESCRIPCIÓN:          Método utilizado habilitar o deshabilitar los checkbox        
    ///PARAMETROS:           Estatus: Variable que contiene un True o False
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           07/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Checkbox(bool Estatus)
    {
        Ckb_Fecha_Genero.Enabled = Estatus;
        Ckb_Estatus.Enabled = Estatus;
        Ckb_Empleado_Genero.Enabled = Estatus;

        Ckb_Fecha_Genero.Checked = false;
        Ckb_Empleado_Genero.Checked = false;
        Ckb_Estatus.Checked = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Inhbilitar_Combos
    ///DESCRIPCIÓN:          Método utilizado para inhabilitar los criterios de generación de inventarios                
    ///PARAMETROS:           Estatus: Variable que contiene un True o False
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           07/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Inhbilitar_Combos()
    {
        if (Cmb_Estatus.Items.Count > 0)
            Cmb_Estatus.SelectedIndex = 0;

        Cmb_Estatus.Enabled = false;

        Btn_Calendar_Fecha_Final.Enabled = false;
        Btn_Calendar_Fecha_Inicial.Enabled = false;

        Txt_Fecha_Final_B.Text = "";
        Txt_Fecha_Inicial_B.Text = "";

        if (Cmb_Empleado_Genero.Items.Count > 0)
            Cmb_Empleado_Genero.SelectedIndex = 0;

        Cmb_Empleado_Genero.Enabled = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Boton_Imprimir
    ///DESCRIPCIÓN:          Método  utilizado para poner visible u ocultar el botón Btn_Imprimir                
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           07/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Boton_Imprimir()
    {
        if ((Ckb_Reporte_Listados.Checked == true) | (Ckb_Reporte_Ajustes.Checked == true))
        {
            Btn_Imprimir.Visible = true;
            Btn_Imprimir_Excel.Visible = true;
        }
        else
        {
            Btn_Imprimir.Visible = false;
            Btn_Imprimir_Excel.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus()
    {
        try
        {
            if (Cmb_Estatus.Items.Count == 0)
            {
                Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
                Cmb_Estatus.Items.Add("EN CONSTRUCCION");
                Cmb_Estatus.Items.Add("GENERADA");
                Cmb_Estatus.Items.Add("AUTORIZADA");
                Cmb_Estatus.Items.Add("CANCELADA");
                Cmb_Estatus.Items[0].Value = "0";
                Cmb_Estatus.Items[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Empleados()
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Listados_Productos.Consultar_Empleados();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Empleado_Genero.DataSource = Dt_Consulta;
                Cmb_Empleado_Genero.DataTextField = "EMPLEADO";
                Cmb_Empleado_Genero.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                Cmb_Empleado_Genero.DataBind();
                Cmb_Empleado_Genero.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Empleado_Genero.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Listados
    ///DESCRIPCIÓN:          Método utilizado para llenar el combo Cmb_Ajustes
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Listados()
    {
        DataTable Dt_Consulta = new DataTable();

        try
        {
            Dt_Consulta = Listados_Productos.Consultar_Numeros_Listados();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Ajustes_Listado.DataSource = Dt_Consulta;
                Cmb_Ajustes_Listado.DataTextField =  Ope_Com_Listado.Campo_Folio;
                Cmb_Ajustes_Listado.DataValueField = Ope_Com_Listado.Campo_Listado_ID;
                Cmb_Ajustes_Listado.DataBind();
                Cmb_Ajustes_Listado.Items.Insert(0, new ListItem("<SELECCIONE>", ""));
                Cmb_Ajustes_Listado.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial_Componentes
    ///DESCRIPCIÓN:          Método utilizado para asignar propiedades a los componentes
    ///                      Iniciales
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Inicial_Componentes()
    {
        Txt_Fecha_Inicial_B.Text = "";
        Txt_Fecha_Final_B.Text = "";
        Btn_Calendar_Fecha_Final.Enabled = false;
        Btn_Calendar_Fecha_Inicial.Enabled = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Inventarios
    ///DESCRIPCIÓN:          Metodo utilizado para llenar el Dataset e instanciar el método Generar_Reporte
    ///PARAMETROS:           1.- DataTable Dt_Consulta, Esta tabla contiene los datos de la 
    ///                      consulta que se realizó a la base de datos
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Listados(DataTable Dt_Consulta, DataSet Ds_Listados_Productos, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        String Usuario = Cls_Sessiones.Nombre_Empleado;
        DataRow Renglon;

        try
        {   // Se llenan los detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Consulta.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Consulta.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Listados_Productos.Tables[0].ImportRow(Renglon);
                Ds_Listados_Productos.Tables[0].Rows[Cont_Elementos].SetField("PERSONA_IMPRIMIO", Usuario);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Rep_Listados_Productos.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Listados_Productos_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Listados_Productos, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Ajustes_Inventarios
    ///DESCRIPCIÓN:          Metodo utilizado para llenar el Dataset e instanciar el método Generar_Reporte
    ///PARAMETROS:           1.- DataTable Dt_Consulta, Esta tabla contiene los datos de la 
    ///                      consulta que se realizó a la base de datos
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte_Ajustes_Listado(DataTable Dt_Consulta, DataSet Ds_Ajustes_Listados_Productos, String Formato)
    {
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        String Usuario = Cls_Sessiones.Nombre_Empleado;
        DataRow Renglon;

        try
        {
            // Se llena la cabecera del DataSet
            Renglon = Dt_Consulta.Rows[0];
            Ds_Ajustes_Listados_Productos.Tables[0].ImportRow(Renglon); // Llena la cabecera
            Ds_Ajustes_Listados_Productos.Tables[0].Rows[0].SetField("PERSONA_IMPRIMIO", Usuario);

            // Se llenan los detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Consulta.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Consulta.Rows[Cont_Elementos];  // Instanciar renglon e importarlo
                Ds_Ajustes_Listados_Productos.Tables[1].ImportRow(Renglon); // Llena los detalles
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Almacen/Rpt_Alm_Com_Rep_Ajustes_Listado_Productos.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Ajustes_Listados_Produ_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar


            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Ajustes_Listados_Productos, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
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


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para validar que el usuario seleccione los
    ///                      combos, una vez que ha seleccionado los CheckBoxt
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Validar_Opciones_Consulta()
    {
        Boolean Validacion = true;
        Lbl_Informacion.Text = "Es necesario.";
        String Mensaje_Error = "";

        if (Ckb_Reporte_Listados.Checked)
        {
            if (Ckb_Empleado_Genero.Checked)
            {
                if (Cmb_Empleado_Genero.SelectedIndex == 0)
                {
                    if (!Validacion)
                    {
                        Mensaje_Error = Mensaje_Error + "<br>";
                    }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opción del combo Empleado Generó.";
                    Validacion = false;
                }
            }

            if ((Ckb_Empleado_Genero.Checked == false) && (Ckb_Estatus.Checked == false) && (Ckb_Fecha_Genero.Checked == false))
            {

                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar un criterio de búsqueda.";
                Validacion = false;
            }
        }
        else if (Ckb_Reporte_Ajustes.Checked)
        {
            if (Cmb_Ajustes_Listado.SelectedIndex == 0)
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ Seleccionar un número de listado.";
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
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Fecha_Valida = true;

        try
        {
            if (Ckb_Fecha_Genero.Checked)
            {
                if ((Txt_Fecha_Inicial_B.Text.Length != 0))
                {
                    // Convertimos el Texto de los TextBox fecha a dateTime
                    Date1 = DateTime.Parse(Txt_Fecha_Inicial_B.Text);
                    Date2 = DateTime.Parse(Txt_Fecha_Final_B.Text);

                    //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                    if ((Date1 < Date2) | (Date1 == Date2))
                    {
                        //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                        Listados_Productos.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial_B.Text.Trim());
                        Listados_Productos.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final_B.Text.Trim());
                        Fecha_Valida = true;
                    }
                    else
                    {
                        Lbl_Informacion.Text = " La fecha inicial no pude ser mayor que la fecha final <br />";
                        Div_Contenedor_Msj_Error.Visible = true;
                        Fecha_Valida = false;
                    }
                }
            }
            return Fecha_Valida;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           07/Mayo/2011 
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

    #endregion

    #region Eventos

    protected void Ckb_Reporte_Listados_CheckedChanged(object sender, EventArgs e)
    {
        if (Ckb_Reporte_Listados.Checked)
        {
            Ckb_Reporte_Ajustes.Checked = false;
            Habilitar_Checkbox(true);
        }
        else
        {
            Habilitar_Checkbox(false);
        }
        Inhbilitar_Combos();
        Cmb_Ajustes_Listado.Enabled = false;
        Lbl_Listado.Enabled = false;
        Boton_Imprimir();
    }

    protected void Ckb_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Estatus.Checked)
            {
                Cmb_Estatus.Enabled = true;
                Cmb_Estatus.SelectedIndex = 0;
            }
            else
            {
                Cmb_Estatus.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    protected void Ckb_Empleado_Genero_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Empleado_Genero.Checked)
            {
                Cmb_Empleado_Genero.Enabled = true;
                Llenar_Combo_Empleados();
                Cmb_Empleado_Genero.SelectedIndex = 0;
            }
            else
                Cmb_Empleado_Genero.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    protected void Ckb_Fecha_Genero_CheckedChanged(object sender, EventArgs e)
    {
        if (Ckb_Fecha_Genero.Checked)
        {
            Btn_Calendar_Fecha_Final.Enabled = true;
            Btn_Calendar_Fecha_Inicial.Enabled = true;
            Txt_Fecha_Inicial_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final_B.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Btn_Calendar_Fecha_Final.Enabled = false;
            Btn_Calendar_Fecha_Inicial.Enabled = false;
            Txt_Fecha_Inicial_B.Text = "";
            Txt_Fecha_Final_B.Text = "";
        }
    }
    protected void Ckb_Reporte_Ajustes_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Ckb_Reporte_Ajustes.Checked)
            {
                Ckb_Reporte_Listados.Checked = false;
                Cmb_Ajustes_Listado.Enabled = true;
                Lbl_Listado.Enabled = true;
                Habilitar_Checkbox(false);
                Llenar_Combo_Listados();
            }
            else
            {
                Cmb_Ajustes_Listado.Enabled = false;
                Lbl_Listado.Enabled = false;
            }
            Estatus_Inicial_Componentes();
            Inhbilitar_Combos();
            Boton_Imprimir();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "PDF";
        Consultar_Listados_Productos(Formato);
    }


    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        Consultar_Listados_Productos(Formato);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Listados_Productos
    ///DESCRIPCIÓN:          Método utilizado para instanciar al los métodos: "Validar_Opciones_Consulta", 
    ///                      "Consultar_Listados_Productos", Consultar_Ajustes_Listado  y Generar_Reporte
    ///PARAMETROS:            
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Consultar_Listados_Productos( String Formato)
    {
        Boolean consulta = false;
        DataTable Dt_Consulta = new DataTable();

        try
        {
            if (Validar_Opciones_Consulta())
            {
                if (Ckb_Reporte_Listados.Checked)
                {
                    if ((Cmb_Estatus.SelectedIndex != 0) && (Ckb_Estatus.Checked))
                    {
                        Listados_Productos.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
                        consulta = true;
                    }
                    else if ((Cmb_Estatus.SelectedIndex == 0) && (Ckb_Estatus.Checked))
                    {
                        Listados_Productos.P_Estatus = null;
                        consulta = true;
                    }

                    if ((Cmb_Empleado_Genero.SelectedIndex != 0) && (Ckb_Empleado_Genero.Checked))
                    {
                        Listados_Productos.P_Empleado_Creo = Cmb_Empleado_Genero.SelectedValue.Trim();
                        consulta = true;
                    }
                    else
                    {
                        Listados_Productos.P_Empleado_Creo = null;
                    }

                    if (Ckb_Fecha_Genero.Checked)
                    {
                        if (!Verificar_Fecha())
                        {
                            Div_Contenedor_Msj_Error.Visible = true;
                            Listados_Productos.P_Fecha_Inicial = null;
                            Listados_Productos.P_Fecha_Final = null;
                            consulta = false;
                        }
                        else
                        {
                            consulta = true;
                        }
                    }

                    if (consulta)
                    {
                        Dt_Consulta = Listados_Productos.Consultar_Listados_Productos();

                        if (Dt_Consulta.Rows.Count > 0)
                        {
                            Ds_Alm_Com_Rep_Listados_Productos Ds_Listados_Productos = new Ds_Alm_Com_Rep_Listados_Productos();
                            Generar_Reporte_Listados(Dt_Consulta, Ds_Listados_Productos,  Formato); // Se instancia el método que llena el DataSet
                            Div_Contenedor_Msj_Error.Visible = false;
                        }
                        else
                        {
                            Lbl_Informacion.Text = "No se encontraron listados";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }

                }
                else if (Ckb_Reporte_Ajustes.Checked)
                {
                    if ((Cmb_Ajustes_Listado.SelectedIndex != 0))
                    {
                        Listados_Productos.P_No_Listado = Cmb_Ajustes_Listado.SelectedValue.Trim();
                        consulta = true;
                    }

                    if (consulta)
                    {
                        Dt_Consulta = Listados_Productos.Consultar_Ajustes_Listado();

                        if (Dt_Consulta.Rows.Count > 0)
                        {
                            Ds_Alm_Com_Reporte_Ajustes_Listado_Productos Ds_Ajustes_Listados_Productos = new Ds_Alm_Com_Reporte_Ajustes_Listado_Productos();
                            Generar_Reporte_Ajustes_Listado(Dt_Consulta, Ds_Ajustes_Listados_Productos, Formato); // Se instancia el método que llena el DataSet
                            Div_Contenedor_Msj_Error.Visible = false;
                        }
                        else
                        {
                            Lbl_Informacion.Text = "No se encontraron ajustes del listado de productos seleccionado";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }


    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

#endregion Eventos
    
}
