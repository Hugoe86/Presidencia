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
using Presidencia.Estadisticas_Tramites.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Ventanilla_Lista_Tramites.Negocio;

public partial class paginas_Tramites_Frm_Tra_Estadisticas : System.Web.UI.Page
{
         
    #region Variables

    private Cls_Tra_Estadisticas_Negocio Estadistica_Negocio = new Cls_Tra_Estadisticas_Negocio();


    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Limpiar_Componentes();
            Llenar_Grid();
            Llenar_Combo();
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Tramites.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
        }
    }
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid
    ///DESCRIPCIÓN: Metodo que llena el grid view con el metodo de Consulta_tramites
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Llenar_Grid()
    {
        Estadistica_Negocio = new Cls_Tra_Estadisticas_Negocio();
        DataSet Data_Set = Estadistica_Negocio.Consulta_Tramites();
        if (Data_Set != null)
        {
            Grid_Tramites.DataSource = Data_Set;
            Grid_Tramites.DataBind();
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Seleccionar_Cheks
    ///DESCRIPCIÓN: Metodo que selecciona los checkbox dentro de un Grid view de acuerdo al parametro estado
    ///PARAMETROS:  1.- GridView MyGrid grid que se va a recorrer
    ///             2.- String Nombre_check nombre del checkbox dentro del grid
    ///             3.- bool Estado estado al que se desea cambiar los check box del grid
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Seleccionar_Cheks(GridView MyGrid, String Nombre_Check, bool Estado)
    {

        //Seleccionamos todos los checks
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked = Estado;
        }//fin del for i


    }//fin de Seleccionar Cheks

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Arreglo_Tramites
    ///DESCRIPCIÓN: Metodo que regresa un string con los catalogos seleccionados
    ///PARAMETROS:    1.- GridView que se recorre
    ///               2.- Nombre_Check del cual se evalua el estado Checked
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String[] Generar_Arreglo_Tramites(GridView MyGrid, String Nombre_Check)
    {

        //GridViewRow selectedRow = MyGrid.Rows[MyGrid.SelectedIndex];
        String Check_seleccionado = "";
        int num = 0;
        String[] Array_aux = new String[Numero_Checks(MyGrid, Nombre_Check)];

        //Obtenemos el numero de Checkbox seleccionados
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked;

            if (isChecked)
            {
                //Definir el numero de check box seleccionados
                Check_seleccionado = Convert.ToString(row.Cells[1].Text);
                //llenamos el arreglo con los nombres de los catalogos
                Array_aux[num] = Check_seleccionado;
                //Variable auxiliar para contar el numero de check seleccionados. 
                num = num + 1;
            }
        }//fin del for i

        if (num == 0)
        {
            Lbl_Mensaje_Error.Text += "+ Debe seleccionar por lo menos un tramite <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Array_aux;
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo()
    {
        try
        {
            if (Cmb_Estatus.Items.Count == 0)
            {
                Cmb_Estatus.Items.Add("< SELECCIONAR >");
                Cmb_Estatus.Items.Add("PENDIENTE");
                Cmb_Estatus.Items.Add("PROCESO");
                Cmb_Estatus.Items.Add("TERMINADO");
                Cmb_Estatus.Items.Add("DETENIDO");
                Cmb_Estatus.Items.Add("CANCELADO");

                Cmb_Estatus.Items[0].Value = "0";
                Cmb_Estatus.Items[0].Selected = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: Metodo que limpia los controles de la pagina, regresandolos a su estado inicial
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta    
    ///FECHA_CREO: 16/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Limpiar_Componentes()
    {
        try
        {
            Chk_Todos.Checked = false;
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", false);

            Txt_Avance.Enabled = false;
            Cmb_Estatus.Enabled = false;
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia de posicion los dias y meses para acoplarlo al formato fecha de oracle
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///CREO: Silvia Morales
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String fecha_inicial)
    {
        char[] ch = { ' ' };
        String[] str = fecha_inicial.Split(ch);
        String fecha = str[0];
        String[] fecha_nueva = fecha.Split('/');
        String Fecha_Valida = "";
        Fecha_Valida = fecha_nueva[1] + "/" + fecha_nueva[0] + "/" + fecha_nueva[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Verificar_Fecha()
    {
        //creamos dos variables de tipo fecha para usarlas como auxiliar
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        //se convierte el texto de fecha de los textbox a datetime
        Date1 = DateTime.Parse(Formato_Fecha(Txt_Fecha_Inicio.Text));
        Date2 = DateTime.Parse(Formato_Fecha(Txt_Fecha_Fin.Text));
        //Validamos que las fechas sean iguales o mayor la fecha final que la fecha inicial
        if ((Date1 < Date2) | (Date1 == Date2))
        {
            //damos el valor de las fechas a la clase de Negocio 
            Estadistica_Negocio.P_Fecha_Inicial = Txt_Fecha_Inicio.Text;
            Estadistica_Negocio.P_Fecha_Final = Txt_Fecha_Fin.Text;
        }
        else
        {
            //Si la fecha no es valida se mostrara el mensaje de error 
            Lbl_Mensaje_Error.Text += "* Fecha no valida (Fecha inicial mayor a la final) <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }//fin de Verificar_Fecha

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el reporte especificado
    ///PARAMETROS:  1.- data_set.- contiene la informacion de la consulta a la base de datos
    ///             2.-ds_reporte, objeto que contiene la instancia del Data set fisico del reporte a generar
    ///             3.-nombre_reporte, contiene la ruta del reporte a mostrar en pantalla
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataSet Data_Set, DataSet Ds_Reporte, string Nombre_Reporte, String Nombre)
    {

        ReportDocument reporte = new ReportDocument();
        string filePath = Server.MapPath("../Rpt/Tramites/" + Nombre_Reporte);

        reporte.Load(filePath);
        DataRow renglon;

        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(renglon);
        }
        reporte.SetDataSource(Ds_Reporte);

        //1
        ExportOptions exportOptions = new ExportOptions();
        //2
        DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
        //3
        //4
        diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/" + Nombre);
        //5
        exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
        //6
        exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //8
        reporte.Export(exportOptions);
        //9
        String ruta = "../../Reporte/" + Nombre;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    #endregion
        
    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Todos_CheckedChanged
    ///DESCRIPCIÓN: evento del check seleccionar todos los tramites
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Chk_Todos_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Todos.Checked == true)
            Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", true);
        if (Chk_Todos.Checked == false)
            Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", false);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Tramite_CheckedChanged
    ///DESCRIPCIÓN: evento del check "Chk_Tramite" que se encuentra dentro del GridView
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Tramite_CheckedChanged(object sender, EventArgs e)
    {
        int Seleccionados = Numero_Checks(Grid_Tramites, "Chk_Tramite");
        if (Seleccionados == Grid_Tramites.Rows.Count)
        {
            Chk_Todos.Checked = true;
        }
        else
        {
            Chk_Todos.Checked = false;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Estatus.Checked == true)
            {
                Cmb_Estatus.Enabled = true;
                Cmb_Estatus.SelectedIndex = 0;
            }
            else
            {
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Avance_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Chk_Avance_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Avance.Checked == true)
            {
                Txt_Avance.Text = "0";
                Txt_Avance.Enabled = true;
                //NumericUpDownExtender1.Enabled = true;
            }
            else
            {
                Txt_Avance.Text = "0";
                Txt_Avance.Enabled = false;
                //NumericUpDownExtender1.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Graficar_Click
    ///DESCRIPCIÓN: evento del boton Graficar, que genera la grafica 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Graficar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        String Espacios_Blanco = "";
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario: <br>";

        Estadistica_Negocio.P_Tramites = Generar_Arreglo_Tramites(Grid_Tramites, "Chk_Tramite");
        //Asignamos las fechas a la clase de negocio con el metodo sig. 
        Verificar_Fecha();

        if (Chk_Estatus.Checked == true)
        {
            if (Cmb_Estatus.SelectedIndex != 0)
            {
                Estadistica_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text += "* Seleccione el tipo de estatus. <br />";
            }
        }
        if (Chk_Avance.Checked == true)
        {
            if (Txt_Avance.Text != "")
            {
                if (Convert.ToDouble(Txt_Avance.Text) <= 100)
                {
                    Estadistica_Negocio.P_Porcentaje = Txt_Avance.Text;
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "* El porcentaje no debe exceder el 100%.<br />";
                }
            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text += "* Ingrese el porcentaje.<br />";
            }
        }

        //Si los datos son validos  se genera la grafica global 
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Obtenemos el data_set para generar las graficas

            DataSet Data_Estadistica = Estadistica_Negocio.Consulta_Solicitudes();
            Ds_Grafica_Solicitudes_Acumulados Ds_Grafica = new Ds_Grafica_Solicitudes_Acumulados();
            Generar_Reporte(Data_Estadistica, Ds_Grafica, "Rpt_Grafica_Solicitudes_Acumulados.rpt", "Rpt_Grafica_Solicitudes_Acumulados.pdf");
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Negocio_Cargar_Grid = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        DataTable Dt_Tramite = new DataTable();
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Session["BUSQUEDA_TRAMITES"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_TRAMITES"].ToString());

                if (Estado != false)
                {
                    Negocio_Cargar_Grid.P_Tramite_ID = Session["TRAMITE_ID"].ToString();
                    Dt_Tramite = Negocio_Cargar_Grid.Consultar_Tramites();

                    if (Dt_Tramite is DataTable)
                    {
                        if (Dt_Tramite.Rows.Count > 0)
                        {
                            Grid_Tramites.DataSource = Dt_Tramite;
                            Grid_Tramites.DataBind();
                        }
                    }

                }
                else
                {
                    Llenar_Grid();
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN: evento del boton  de limpiar, el cual ejecuta el metodo de Limpiar_Componentes() 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Componentes();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: evento del boton Salir, que regresa a la pagina principal 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 18/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Componentes();
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    #endregion
    
}
