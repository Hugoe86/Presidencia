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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Estadisticas.Negocios;


public partial class paginas_Atencion_Ciudadana_Frm_Estadisticas : System.Web.UI.Page
{

    /********************************************************************************************************
    * Seccion de Variables
    *********************************************************************************************************/
    #region Variables

    private Cls_Estadisticas_Negocio Estadistica_Negocio;

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {


    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Estadistica_Negocio = new Cls_Estadisticas_Negocio();
        Limpiar_Formulario();
    }

    #endregion
    /********************************************************************************************************
    * Seccion de Metodos
    *********************************************************************************************************/
    #region Metodos

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
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Limpia los componentes checkbox y gridviews del formulario
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Formulario()
    {
        //Componentes de la pestaña Graficas por Estatus

        Chk_Areas.Checked = false;
        Div_Areas.Visible = false;
        Chk_Todos_Areas.Checked = false;
        Chk_Todos_Areas.Visible = false;

        //Componentes de la pestaña Graficas por Tiempos

        Chk_Asuntos_Tiempos.Checked = false;

        //Componentes de la pestaña Graficas Globales
        Chk_Estatus_Global.Checked = true;
        //Componente Generales 
        Txt_Fecha_Inicia.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");

        Grid_Dependencias_Tiempos.Enabled = true;
        Grid_Dependencias_Estatus.Enabled = true;
        Llenar_Grid(Grid_Dependencias_Estatus,"Dependencias");
        Llenar_Grid(Grid_Dependencias_Tiempos,"Dependencias");

        TabContainer1.ActiveTabIndex = 0;
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Check_Box_Seleccionados
    ///DESCRIPCIÓN: Metodo que regresa un string con los catalogos seleccionados
    ///PARAMETROS:    1.- GridView que se recorre
    ///               2.- Nombre_Check del cual se evalua el estado Checked
    ///               3.- Nombre_ope nombre de la operacion ya sea (Catalogo u operaciones)
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String[] Check_Box_Seleccionados(GridView MyGrid, String Nombre_Check, String Nombre_Ope)
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
            Lbl_Mensaje_Error.Text += "+ Debe seleccionar por lo menos un" + Nombre_Ope + "<br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }


        return Array_aux;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Cadena
    ///DESCRIPCIÓN: Metodo que genera una cadena a partir de un arreglo 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String Generar_Cadena(String[] Arreglo, int Longitud)
    {
        String Cadena = "";
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
        return Cadena;
    }//fin de generar cadena

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Numero_Checks
    ///DESCRIPCIÓN: Metodo que cuenta el numero de checks seleccionados dentro del GridView 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public int Numero_Checks(GridView MyGrid, String nombre_check)
    {
        int Numero_Seleccionados = 0;
        //Obtenemos el numero de Checkbox seleccionados dentro del GridView
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
        Date1 = DateTime.Parse(Formato_Fecha(Txt_Fecha_Inicia.Text));
        Date2 = DateTime.Parse(Formato_Fecha(Txt_Fecha_Final.Text));
        //Validamos que las fechas sean iguales o mayor la fecha final que la fecha inicial
        if ((Date1 < Date2) | (Date1 == Date2))
        {
            //damos el valor de las fechas a la clase de Negocio 
            Estadistica_Negocio.P_Fechas_Inicio = Txt_Fecha_Inicia.Text;
            Estadistica_Negocio.P_Fecha_Fin = Txt_Fecha_Final.Text;
        }
        else
        {
            //Si la fecha no es valida se mostrara el mensaje de error 
            Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }//fin de Verificar_Fecha

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Convertir_DataSet_a_Array
    ///DESCRIPCIÓN: Metodo que generar un arreglo apartir del Dataset enviado 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String[] Convertir_DataSet_a_Array(DataSet Datos)
    {
        //Creamos un arreglo del tamaño de los renglones del Dataset
        String[] Arreglo = new String[Datos.Tables[0].Rows.Count];
        //Recorremos el dataset para generar el arreglo
        for (int i = 0; i < Datos.Tables[0].Rows.Count; i++)
        {
            Arreglo[i] = Datos.Tables[0].Rows[i].ItemArray[0].ToString();
        }//fin for

        return Arreglo;
    }


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
        string filePath = Server.MapPath("../paginas/Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

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

   
    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid
    ///DESCRIPCIÓN: Metodo que clasifica los dos tipos de data_set y manda llamar la clase datos 
    ///             dependiendo del caso en el que se encuentre 
    ///PARAMETROS:  1.- Grid_View es el Grid que se llenara
    ///             2.- Opcion es el caso que se ejecutara solo puede ser 1 0 2
    ///                 1 es para obtener el dataset de las dependencias
    ///                 2 es para obtener el dataset del area
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 08/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Llenar_Grid(GridView Grid_View, String Tipo)
    {

        DataSet Data_Set = new DataSet();
        switch (Tipo)
        {
            case "Dependencias":
                Data_Set = Estadistica_Negocio.Consulta_Dependencias();
                break;
            case "Areas":
                Data_Set = Estadistica_Negocio.Consulta_Areas();
                break;
        }
        Grid_View.DataSource = Data_Set;
        Grid_View.DataBind();

    }//fin de llenar_Grid
    
    #endregion
    
    /********************************************************************************************************
    * Seccion de eventos (Controles)
    *********************************************************************************************************/
    #region Eventos



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN: Evento del boton Limpiar Formulario, que permite llamar el metodo 
    ///             Limpiar_Formulario()
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 09/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Formulario();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del boton Salir, que permite enlasar a la ventana principal
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        
        Limpiar_Formulario();
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Graficar_Click
    ///DESCRIPCIÓN: Evento del boton Generar Grafica, que permite generar la grafica de acuerdo a la 
    ///             pestaña que este seleccionada. 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 09/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************


    protected void Btn_Graficar_Click(object sender, ImageClickEventArgs e)
    {

        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        DataSet Data_Set = new DataSet();
        switch (TabContainer1.ActiveTabIndex)
        {
            //En caso de Seleccionar la pestaña 1
            case 0:
                
                Verificar_Fecha();
                //Si los datos son validos  se genera la grafica global 
                if ( Div_Contenedor_Msj_Error.Visible == false)
                {

                    Data_Set = Estadistica_Negocio.Generar_Grafica_Pastel();
                    Ds_Grraficas_Acumulados Ds_Grafica = new Ds_Grraficas_Acumulados();
                    Generar_Reporte(Data_Set,Ds_Grafica,"Rpt_Grafica_Acumulados_Todos.rpt","Rpt_Acumulado_Global.pdf");
                }
                break;
            //En caso de Seleccionar la pestaña 2
            case 1:
                //Para generar graficas de Areas por dependencia     
                if (Chk_Areas.Checked == true)
                {
                    Estadistica_Negocio.P_Dependencias = Check_Box_Seleccionados(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus", "a Dependencia");
                    Estadistica_Negocio.P_Areas = Check_Box_Seleccionados(Grid_Areas, "Chk_Areas_Estatus", " Area");
                    Verificar_Fecha();
                    //si los datos son correctos se genera el dataset
                    if (Div_Contenedor_Msj_Error.Visible == false) 
                    {
                        Data_Set = Estadistica_Negocio.Generar_Grafica_Areas_Acumulados();
                        Ds_Grraficas_Acumulados ds_grafica = new Ds_Grraficas_Acumulados();
                        Generar_Reporte(Data_Set, ds_grafica, "Rpt_Grafica_Acumulado_Areas.rpt", "Rpt_Acumulado_Areas.pdf");

                    }

                }
                //Generar graficas de dependencias ya que no selecciono areas
                else
                {
                    Estadistica_Negocio.P_Dependencias = Check_Box_Seleccionados(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus", "a Dependencia");
                    Verificar_Fecha();
                    //si los datos son correctos se genera el dataset 
                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        Data_Set = Estadistica_Negocio.Generar_Grafica_Dependencias_Acumulados();
                        Ds_Grraficas_Acumulados ds_grafica = new Ds_Grraficas_Acumulados();
                        Generar_Reporte(Data_Set, ds_grafica, "Rpt_Grafica_Acumulados_Dependencias.rpt", "Rpt_Acumulado_Dependencias.pdf");
                    }

                }

                break;
            //En caso de Seleccionar la pestaña 3
            case 2:
                //En caso de que seleccione Grafica de tiempos por asuntos
                if (Chk_Asuntos_Tiempos.Checked == true)
                {
                    Estadistica_Negocio.P_Dependencias = Check_Box_Seleccionados(Grid_Dependencias_Tiempos, "Chk_Dependencias_Tiempos", "a Dependencia");
                    DataSet Asuntos = Estadistica_Negocio.Consultar_Asuntos();
                    Estadistica_Negocio.P_Asuntos = Convertir_DataSet_a_Array(Asuntos);
                    Verificar_Fecha();
                    //si los datos son correctos se genera el dataset
                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        Data_Set = Estadistica_Negocio.Generar_Grafica_Tiempos_Asuntos();
                        Ds_Graficas_Tiempos ds_grafica = new Ds_Graficas_Tiempos();
                        Generar_Reporte(Data_Set, ds_grafica, "Rpt_Grafica_Tiempos_Asuntos.rpt", "Rpt_Tiempos_Asuntos.pdf");
                    }
                }
                // En caso de que seleccione solo Grafica de tiempos por dependencias
                else
                {
                    Estadistica_Negocio.P_Dependencias = Check_Box_Seleccionados(Grid_Dependencias_Tiempos, "Chk_Dependencias_Tiempos", "a Dependencia");
                    Verificar_Fecha();
                    //si los datos son correctos se genera el dataset
                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        Data_Set = Estadistica_Negocio.Generar_Grafica_Tiempos_Dependencias();
                        Ds_Graficas_Tiempos ds_grafica = new Ds_Graficas_Tiempos();
                        Generar_Reporte(Data_Set, ds_grafica, "Rpt_Grafica_Tiempos_Dependencias.rpt", "Rpt_Tiempos_Dependencias.pdf");
                    }
                }
                break;

        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Areas_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged de areas, que permite validar las dependencias seleccionadas
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Areas_CheckedChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        //obtenemos el numero de check seleccionados
        int Num_Seleccionados = Numero_Checks(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus");
        //Validamos que si no esta seleccionada habilite el grid de dependencias
        if (Chk_Areas.Checked == false)
        {
            Grid_Dependencias_Estatus.Enabled = true;
            Div_Areas.Visible = false;
            Chk_Todos_Areas.Checked = false;
        }
        else
        {
            if (Num_Seleccionados == 1)
            {
                //aqui entrara cuando solo seleccione una dependencia 
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Div_Areas.Visible = true;
                Chk_Todos_Areas.Visible = true;
                Grid_Dependencias_Estatus.Enabled = false;
                String []Dependencia = Check_Box_Seleccionados(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus", "");
                //llenamos el grid de Areas 
                try
                {
                    
                    Estadistica_Negocio.P_Dependencia_Area = Dependencia[0];
                    Llenar_Grid(Grid_Areas, "Areas");
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Text = "" + Ex;
                }
            }
            if (Num_Seleccionados == 0)
            {
                //Se mostrara un mensaje de error si no selecciona ninguna dependencia 
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Debe seleccionar una Dependencia <br />";
                Div_Areas.Visible = false;
                Chk_Areas.Checked = false;
                Grid_Dependencias_Estatus.Enabled = true;

            }
            if (Num_Seleccionados > 1)
            {
                //Si selecciono mas de una dependencia 
                Div_Contenedor_Msj_Error.Visible = true;
                Grid_Dependencias_Estatus.Enabled = true;
                Lbl_Mensaje_Error.Text = "+ Solo puede seleccionar una Dependencia <br />";
                Div_Areas.Visible = false;
                Chk_Areas.Checked = false;
            }
        }//fin de else

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Asuntos_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged de asuntos, que permite validar las dependencias seleccionadas
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Asuntos_CheckedChanged(object sender, EventArgs e)
    {
        //Primero Validamos que solo este seleccionada una dependencia
        //obtenemos el numero de check seleccionados
        int Num_Seleccionados = Numero_Checks(Grid_Dependencias_Tiempos, "Chk_Dependencias_Tiempos");
        //Validamos que si no esta seleccionada habilite el grid de dependencias
        if (Chk_Asuntos_Tiempos.Checked == false)
            Grid_Dependencias_Tiempos.Enabled = true;
        else
        {
            if (Num_Seleccionados == 1)
            {
                //Si solo selecciono una dependencia es valido y se deshabilitara el grid  Grid_Dependencias_Tiempos
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Grid_Dependencias_Tiempos.Enabled = false;
                

            }
            if (Num_Seleccionados == 0)
            {
                //Si no selecciono dependencias
                Div_Contenedor_Msj_Error.Visible = true;
                Grid_Dependencias_Tiempos.Enabled = true;
                Lbl_Mensaje_Error.Text = "+ Debe seleccionar una Dependencia <br />";
                Chk_Asuntos_Tiempos.Checked = false;

            }
            if (Num_Seleccionados > 1)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Grid_Dependencias_Tiempos.Enabled = true;
                Lbl_Mensaje_Error.Text = "+ Solo puede seleccionar una Dependencia <br />";
                Chk_Asuntos_Tiempos.Checked = false;
            }
        }//fin de else
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Todos_Tiempo_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged de todas las dependencias de la péstaña de tiempos
    /// que permite validar la seleccion de checkBox's  dentro del gridview
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Todos_Tiempo_CheckedChanged(object sender, EventArgs e)
    {
        //Seleccionamos todos los checks del Grid_Dependencias_Tiempos si selecciono el check de todos y aparte 
        //el grid esta habilitado. 
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if ((Chk_Todos_Tiempo.Checked == true) && (Grid_Dependencias_Tiempos.Enabled == true)) 
        {
            Seleccionar_Cheks(Grid_Dependencias_Tiempos, "Chk_Dependencias_Tiempos", true);
        }

        //Quitamos la seleccion a todos los checks del Grid_Dependencias_Tiempos si selecciono el check de todos y aparte 
        //el grid esta habilitado. 
        if ((Chk_Todos_Tiempo.Checked == false) && (Grid_Dependencias_Tiempos.Enabled == true))
        {
            Seleccionar_Cheks(Grid_Dependencias_Tiempos, "Chk_Dependencias_Tiempos", false);
        }

        //validamos aue no se seleccione el check todos cuando esta deshabilitado el grid de dependencias
        if (Grid_Dependencias_Tiempos.Enabled == false)
            Chk_Todos_Tiempo.Checked = false;

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Todos_Areas_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged de la la péstaña de Estatus
    /// que permite validar la seleccion de checkBox's  dentro del gridview
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Chk_Todos_Areas_CheckedChanged(object sender, EventArgs e)
    {
        //Se seleccionan todos los checkbox dentro del gridview en caso de seleccionar el de todos 
        
        if (Chk_Todos_Areas.Checked == true)
        {
            //Seleccionamos todos los check dentro del grid de areas 
            Seleccionar_Cheks(Grid_Areas, "Chk_Areas_Estatus", true);
        }
        if (Chk_Todos_Areas.Checked == false)
        {
            Seleccionar_Cheks(Grid_Areas, "Chk_Areas_Estatus", false);
        }
         
    }//fin de Chk_Todos_Areas_CheckedChanged

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Todas_Dependencias_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged de la la péstaña de Estatus
    /// que permite validar la seleccion de checkBox's  dentro del gridview
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Chk_Todas_Dependencias_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        //En caso de seleccionar el check Todos se seleccionaran todos los checks del grid view
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if ((Chk_Todas_Dependencias_Estatus.Checked == true) && (Grid_Dependencias_Estatus.Enabled == true))
        {
            Seleccionar_Cheks(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus", true);
        }
        // En caso de quitar la seleccion al checkbox todos se limpiaran los check del box 
        if ((Chk_Todas_Dependencias_Estatus.Checked == false) && (Grid_Dependencias_Estatus.Enabled == true))
        {
            Seleccionar_Cheks(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus", false);
        }

        if (Grid_Dependencias_Estatus.Enabled == false)
            Chk_Todas_Dependencias_Estatus.Checked = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencias_Tiempos_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged del check dentro del grid view
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    protected void Chk_Dependencias_Tiempos_CheckedChanged(object sender, EventArgs e)
    {
        int seleccionados = Numero_Checks(Grid_Dependencias_Tiempos, "Chk_Dependencias_Tiempos");
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        //validamos que dependiendo de lo seleccionado dentro del gridview se seleccione o no el check Chk_Todos_Tiempo
        if (seleccionados == Grid_Dependencias_Tiempos.Rows.Count)
        {
            Chk_Todos_Tiempo.Checked = true;
        }
        else
        {
            Chk_Todos_Tiempo.Checked = false;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencias_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged del check dentro del grid view
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Dependencias_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        int seleccionados = Numero_Checks(Grid_Dependencias_Estatus, "Chk_Dependencias_Estatus");
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        //validamos que dependiendo de lo seleccionado dentro del gridview se seleccione o no el Chk_Todas_Dependencias_Estatus
        if (seleccionados == Grid_Dependencias_Estatus.Rows.Count)
        {
            Chk_Todas_Dependencias_Estatus.Checked = true;
        }
        else
        {
            Chk_Todas_Dependencias_Estatus.Checked = false;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Areas_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento CheckedChanged del check dentro del grid view de Areas
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 07/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Chk_Areas_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        int seleccionados = Numero_Checks(Grid_Areas, "Chk_Areas_Estatus");
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        //validamos que dependiendo de lo seleccionado dentro del gridview se seleccione o no el Chk_Todos_Areas
        if (seleccionados == Grid_Areas.Rows.Count)
        {
            Chk_Todos_Areas.Checked = true;
        }
        else
        {
            Chk_Todos_Areas.Checked = false;
        }

    }
    #endregion

}