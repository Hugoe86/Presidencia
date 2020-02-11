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
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Bitacora.Negocios;
using Presidencia.Bitacora_Eventos;
using Presidencia.Sessiones;

public partial class paginas_Atencion_Ciudadana_Frm_Apl_Bitacora : System.Web.UI.Page
{
    #region Variables
    //Variable que sirve para hacer referencia a la clase de negocio 
    private Cls_Apl_Bitacora_Negocio Bitacora_Negocio;
    //Variable que sirve para construir la descripcion de la accion que se realiza y posteriormente 
    //insertar este dato en la tabla de bitacora con todos sus datos correspondientes. 
    private String Descripcion;

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {

    }

    protected void Page_Init(object sender, EventArgs e)
    {
        Limpiar_Formulario();
        Cargar_Tabla_Catalogos();

    }
    #endregion

    #region Metodos
    
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Evento que limpia el formulario
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Limpiar_Formulario()
    {

        Chk_Altas.Checked = false;
        Chk_Modificar.Checked = false;
        Chk_Usuario.Checked = false;
        Chk_Impresiones.Checked = false;
        Chk_Accesos.Checked = false;
        Chk_Bajas.Checked = false;
        Chk_Consultas.Checked = false;
        Chk_Todos_Catalogos.Checked = false;
        Txt_Usuario.Text = "";
        Txt_Usuario.Enabled = false;
        //asignamos la fecha actual a las cajas de fecha
        Txt_Fecha_inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Chk_Ordenar_Accion.Checked = false;
        Chk_Ordenar_Fecha.Checked = false;
        Chk_Ordenar_Usuario.Checked = false;
   
    }//fin de limpiar_Formulario
       
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
    
    public String Check_Box_Seleccionados(GridView MyGrid, String nombre_check,String Nombre_ope)
    {

        //Variable que guarda el nombre del catalogo seleccionado. Ejem: (Frm_Cat_Ate_Colonias)
        String Check_seleccionado = "";
        //variable que guarda el nombre de la pagina. Ejem: (Colonias)
        String Nombre_Pagina = "";
        //auxiliar para contar el numero de check seleccionados dentro del grid. 
        int num = 0;
        //Arreglo donde se almacenaran los catalogos seleccionados 
        String[] Array_aux = new String[MyGrid.Rows.Count];
        //Arreglo donde se almacenara los nombres de las paginas que seleccionaron para formar la descripcion de la accion 
        String[] Arreglo_Pagina = new String[MyGrid.Rows.Count];
        String Seleccionados = "";
        //Validamos que no este seleccionado el CheckBox de todos los catalogos
        if (Chk_Todos_Catalogos.Checked == false)
        {
            //Obtenemos el numero de Checkbox seleccionados
            for (int i = 0; i < MyGrid.Rows.Count; i++)
            {
                GridViewRow row = MyGrid.Rows[i];
                bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(nombre_check)).Checked;

                if (isChecked)
                {
                    //Obtiene el nombre del catalogo seleccionados
                    Check_seleccionado = Convert.ToString(row.Cells[3].Text);
                    //Obtiene el Titulo del catalogo seleccionado 
                    Nombre_Pagina = Convert.ToString(row.Cells[1].Text);
                    //llenamos el arreglo con los nombres de los catalogos
                    Array_aux[num] = Check_seleccionado;
                    //llenamos el arreglo con los titulos de la pagina que servira para la variable Descripcion
                    Arreglo_Pagina[num] = Nombre_Pagina;
                    num = num + 1;
                   
                }
            }//fin del for i
            //Validamos en caso de no seleccionar ningun catalogo, esto en base a la variable num, que contiene el numero decheck seleccionados 
            if (num == 0)
            {
                Lbl_Mensaje_Error.Text += "+ Debe seleccionar por lo menos un" + Nombre_ope + "<br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            else
            {
                //Generamos la cadena con los Catalogos seleccionados para generar la consulta de oracle y 
                //la descripcion detallada
                Seleccionados = Generar_Cadena(Array_aux, Arreglo_Pagina, num);
            }
        }//fin del if principal
        else
        {
            Descripcion += "- Todos los catalogos ";
        }
        
        return Seleccionados;
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

    public String Generar_Cadena(String [] Arreglo, String [] Pagina, int Longitud)
    {
        //Variable que almacenara los catalogos seleccionados 
        String Cadena = "";
        Descripcion += " - Los catalogos de ";
        for (int y = 0; y < Longitud; y++)
        {
            if (y == 0)
            {
                Cadena += "'" + Arreglo[y] + "'";
                Descripcion += Pagina[y];
            }
            else
            {
                Cadena += ",'" + Arreglo[y] + "'";
                Descripcion += ", "+Pagina[y];
            }

        }//fin del for y
        
        return Cadena;
    }//fin de generar cadena
        
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas ingresadas por el usuario
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Verificar_Fecha()
    {
        //variable que almacenara los datos de la fecha, ya como sentencia construida
        String Sentencia = "";
        String Fecha_inicio = "";
        String Fecha_fin = "";
        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        
        if ((Txt_Fecha_inicial.Text.Length == 11) && (Txt_Fecha_final.Text.Length == 11))
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Txt_Fecha_inicial.Text);
            Date2 = DateTime.Parse(Txt_Fecha_final.Text);
            //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
            if ((Date1 < Date2) | (Date1 == Date2))
            {
                //Variable que guarda la descripcion de la accion con respecto a la fecha
                Descripcion += " de las fechas del " + Txt_Fecha_inicial.Text + " al " + Txt_Fecha_final.Text; 
                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                Fecha_inicio = Formato_Fecha(Txt_Fecha_inicial.Text);
                Fecha_fin = Formato_Fecha(Txt_Fecha_final.Text);
                Sentencia = " AND TO_DATE(TO_CHAR(BITACORA." +Apl_Bitacora.Campo_Fecha_Hora +",'DD/MM/YY')) BETWEEN '" + Fecha_inicio + "'" + " AND '" + Fecha_fin + "'";
                
            }
            else
            {
                Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
           
        return Sentencia;
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Usuario
    ///DESCRIPCIÓN: Metodo que genera la cadena del reporte teniendo un usuario
    ///PARAMETROS:    
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 1/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Verificar_Usuario()
    {
        String Sentencia = "";
        
        if (Chk_Usuario.Checked == true)
        {
            if (Txt_Usuario.Text != "")
            {
                //Se genera la sentencia de acuerdo al nombre del usuario 
                Sentencia = "AND USUARIO."+ Cat_Empleados.Campo_Nombre+" || ' ' || USUARIO."+Cat_Empleados.Campo_Apellido_Paterno+" || ' ' || USUARIO." + Cat_Empleados.Campo_Apellido_Materno + " LIKE '%" + Txt_Usuario.Text + "%'";
                //Se crea la descripcion de la accion con respecto al usuario consultado 
                Descripcion += " - Usuario : " + Txt_Usuario.Text;
            }
            else
            {
                //Se valida que no este vacia la caja de usuario cuando seleccione el CheckBox de usuario
                Lbl_Mensaje_Error.Text += "+ Usuario no valido <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            if (Txt_Usuario.Text.Length > 20)
            {
                Lbl_Mensaje_Error.Text += "+ Usuario no valido <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }

        }//fin del if 
        
        return Sentencia;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: generar_reporte
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
    public void Generar_Reporte(DataSet data_set, DataSet ds_reporte, string nombre_reporte)
    {

        
        ReportDocument reporte = new ReportDocument();
        string filePath = Server.MapPath("../paginas/rpt/Atencion_Ciudadana/" + nombre_reporte);

        reporte.Load(filePath);
        DataRow renglon;

        for (int i = 0; i < data_set.Tables[0].Rows.Count; i++)
        {
            renglon = data_set.Tables[0].Rows[i];
            ds_reporte.Tables[0].ImportRow(renglon);
        }
        reporte.SetDataSource(ds_reporte);

        //1
        ExportOptions exportOptions = new ExportOptions();
        //2
        DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
        //3
        //4
        diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Bitacora_Eventos.pdf");
        //5
        exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
        //6
        exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //8
        reporte.Export(exportOptions);
        //9
        string ruta = "../../Reporte/Rpt_Bitacora_Eventos.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    
   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        
        String Fecha_Valida= Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "ene":
                aux[1] = "ENE";
                break;
            case "feb":
                aux[1] = "FEB";
                break;
            case "mar":
                aux[1] = "MAR";
                break;
            case "abr":
                aux[1] = "ABR";
                break;
            case "may":
                aux[1] = "MAY";
                break;
            case "jun":
                aux[1] = "JUN";
                break;
            case "jul":
                aux[1] = "JUL";
                break;
            case "ago":
                aux[1] = "AGO";
                break;
            case "sep":
                aux[1] = "SEP";
                break;
            case "oct":
                aux[1] = "OCT";
                break;
            case "nov":
                aux[1] = "NOV";
                break;
            case "dic":
                aux[1] = "DEC";
                break;
        }
            //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
            Fecha_Valida = aux[0] +"-"+ aux[1]+"-"+aux[2];
       

        return Fecha_Valida;
    }// fin de Formato_Fecha
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Check_Box_Otros
    ///DESCRIPCIÓN: Metodo que revisa los check seleccionados de la pestaña otros
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String CheckBox_Otros()
    {
        String Cadena_Accion = "";

        String[] Acciones = new String[4];

        int Posicion = 0;
            if (Chk_Impresiones.Checked == true)
            {
                Acciones[Posicion] = "Imprimir";
                Posicion++;
            }
            
            if (Chk_Accesos.Checked == true)
            {
                Acciones[Posicion] = "Acceso";
                Posicion++;
            }
        
        //Validamos que el usuario seleccionara uno de los check de la pestaña otros
            if (Posicion == 0)
            {
                Lbl_Mensaje_Error.Text += "Debe seleccionar una opcion:<br /> + Impresion <br /> + Accesos al Sistema";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            else
            {
                Descripcion += " - La Accion de ";
                for (int i = 0; i < Posicion; i++)
                {
                    if (i == 0)
                    {
                        Cadena_Accion += "'" + Acciones[i] + "'";
                        Descripcion += Acciones[i];
                    }
                    else
                    {
                        Cadena_Accion += ", '" + Acciones[i] + "'";
                        Descripcion += ", " + Acciones[i];
                    }
                }//fin del for
            }
        return Cadena_Accion;
    }// fin de CheckBox
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: CheckAcciones()
    ///DESCRIPCIÓN: Metodo que revisa los check seleccionados de la pestaña Paginas
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public String Check_Acciones()
    {
        String Cadena_Accion = "";
        //arreglo que almacenara el nombre de las acciones seleccionadas por el usuario
        String[] Acciones = new String[4];
        //Variable que indica la posicion en la que se guardara a accion seleccionada
        int Posicion = 0;
        //Se verifican los 4 checks de las acciones, en caso de ser seleccionados se guarda en una posicion del arreglo
        if (Chk_Altas.Checked == true)
        {
            Acciones[Posicion] = Ope_Bitacora.Accion_Alta;
            Posicion++;
        }

        if (Chk_Bajas.Checked == true)
        {
            Acciones[Posicion] = Ope_Bitacora.Accion_Baja;
            Posicion++;
        }
        if (Chk_Modificar.Checked == true)
        {
            Acciones[Posicion] = Ope_Bitacora.Accion_Modificar;
            Posicion++;
        }
        if (Chk_Consultas.Checked == true)
        {
            Acciones[Posicion] = Ope_Bitacora.Accion_Consultar;
            Posicion++;
        }
        //Validamos que el usuario seleccionara uno de los check de la pestaña otros y formamos la sentencia de acciones para asignarla a la clase de negocio
        if (Posicion == 0)
        {
            Lbl_Mensaje_Error.Text += "Debe seleccionar una opcion:<br /> + Altas <br /> + Modificaciones <br /> + Bajas<br /> + Consultas <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            //Se agrega a la descripcion las acciones que selecciono el usuario para generar el reporte
            Descripcion += " - La Accion de ";
            Cadena_Accion += "BITACORA." + Apl_Bitacora.Campo_Accion + " IN ("; 
            for (int i = 0; i < Posicion; i++)
            {
                if (i == 0)
                {
                    Cadena_Accion += "'" + Acciones[i] + "'";
                    Descripcion += Acciones[i];
                }
                else
                {
                    Cadena_Accion += ", '" + Acciones[i] + "'";
                    Descripcion += ", " + Acciones[i];
                }
            }//fin del for
            Cadena_Accion += ")";
        }
        return Cadena_Accion;
    }// fin de CheckBox

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Orden()
    ///DESCRIPCIÓN: Metodo que revisa los check de orden para indicar el Order By al reporte generado 
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Verificar_Orden()
    {
        int Num_Aux = 0;
        //Variable que indica el Order By que se aplicara al reporte 
        String Ordenar_Por = "";
        //dependiendo del Check que seleccione el usuario se asigna el valos a Ordenar_Por
                if (Chk_Ordenar_Usuario.Checked == true)
                {
                    Num_Aux = Num_Aux + 1;
                    Ordenar_Por = "USUARIO." + Cat_Empleados.Campo_Nombre + " ASC";
                }
                if (Chk_Ordenar_Fecha.Checked == true)
                {
                    Num_Aux = Num_Aux + 1;
                    Ordenar_Por = "BITACORA." + Apl_Bitacora.Campo_Fecha_Hora + " DESC";
                }
                if (Chk_Ordenar_Accion.Checked == true)
                {
                    Num_Aux = Num_Aux + 1;
                    Ordenar_Por = "BITACORA." + Apl_Bitacora.Campo_Accion + " ASC";
                }
        //Validamos que solo seleccione el usuario un orden 
                if (Num_Aux > 1)
                {
                    Lbl_Mensaje_Error.Text += "Solo puede seleccionar un orden:<br /> + Usuario <br /> + Fecha <br /> + Accion<br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                if (Num_Aux == 0)
                {
                    Lbl_Mensaje_Error.Text += "Debe indicar la opción por la que se ordenara el reporte:<br /> + Usuario <br /> + Fecha <br /> + Accion<br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                
        
        return Ordenar_Por;
    }//fin Verificar_Orden 

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
    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Tabla_Catalogos
    ///DESCRIPCIÓN: Evento que carga el grid de catalogos
    ///PROPIEDADES:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Tabla_Catalogos()
    {
        Bitacora_Negocio = new Cls_Apl_Bitacora_Negocio();
        DataSet Data_set = Bitacora_Negocio.Consultar_Pagina();
        Grid_Catalogos.DataSource = Data_set;
        Grid_Catalogos.DataBind();
        

    }
    
    #endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN: Evento del boton limpiar que manda llamar el metodo Limpiar_Formulario()
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 25/Agosto/2010 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Formulario();
        Cargar_Tabla_Catalogos();
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Click
    ///DESCRIPCIÓN: Evento del boton Generar Reporte
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Click(object sender, ImageClickEventArgs e)
    {

        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        Bitacora_Negocio = new Cls_Apl_Bitacora_Negocio();
        //Inicializamos los atributos de clase para evitar problemas 
        Bitacora_Negocio.P_Accion = "";
        Bitacora_Negocio.P_Catalogos = "";
        Bitacora_Negocio.P_Usuario = "";
        Bitacora_Negocio.P_Fecha = "";
        //Esta variable contendra la descripcion del reporte que se genero en la bitacora para posteriormente hacer su registro
        Descripcion = "Se genero un reporte con: ";
        DataSet Data_Set = new DataSet();
        Ds_Bitacora_Catalogos Reporte_Catalogo = new Ds_Bitacora_Catalogos();

        switch (Tab_Container.ActiveTabIndex)
        {
            case 0:
                //Esto se ejecutara en caso de seleccionar la pestaña de Catalogos 
                Bitacora_Negocio.P_Catalogos = Check_Box_Seleccionados(Grid_Catalogos, "Chk_catalogo", "a Pagina");
                //Verifica si selecciono una accion 
                Bitacora_Negocio.P_Accion = Check_Acciones();
                //Verifica si selecciono un usuario 
                Bitacora_Negocio.P_Usuario = Verificar_Usuario();
                //Verifica la fecha seleccionada
                Bitacora_Negocio.P_Fecha = Verificar_Fecha();
                //Asignamos el Orden que mostrara el reporte 
                Bitacora_Negocio.P_Orden = Verificar_Orden();
                //Si no falta ningun dato y estan correctos se ejecutara el query                             
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    // en caso de seleccionar todos los catalogos
                    if (Chk_Todos_Catalogos.Checked == true)
                    {
                        Data_Set = Bitacora_Negocio.Consultar_Bitacora(2);
                        Generar_Reporte(Data_Set, Reporte_Catalogo, "Rpt_Bitacora_Catalogos.rpt");
                        Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Consultar, "Frm_Apl_Bitacora.aspx", "Reporte de eventos de todos los catalogos", Descripcion);
                    }
                    else
                    {
                        //Se agrega el AND a las sentencia de Oracle
                        Bitacora_Negocio.P_Accion = " AND " + Bitacora_Negocio.P_Accion;
                        Data_Set = Bitacora_Negocio.Consultar_Bitacora(0);
                        Generar_Reporte(Data_Set, Reporte_Catalogo, "Rpt_Bitacora_Catalogos.rpt");
                        Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Consultar, "Frm_Apl_Bitacora.aspx", "Reporte de eventos de catalogos", Descripcion);
                    }
                }
                break;

            case 1:
                //Esto se ejecutara en caso de seleccionar la pestaña de Otros
                Bitacora_Negocio.P_Accion = CheckBox_Otros();
                Bitacora_Negocio.P_Usuario = Verificar_Usuario();
                Bitacora_Negocio.P_Fecha = Verificar_Fecha();
                Bitacora_Negocio.P_Catalogos = "";
                
                //Asignamos el Orden que mostrara el reporte 
                Bitacora_Negocio.P_Orden = Verificar_Orden();
                //Si no falta ningun dato se ejecutara el query 
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Data_Set = Bitacora_Negocio.Consultar_Bitacora(1);
                    Generar_Reporte(Data_Set, Reporte_Catalogo, "Rpt_Bitacora_Catalogos.rpt");
                    Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Consultar, "Frm_Apl_Bitacora.aspx", "Reporte de Eventos de Impresion y/o Accesos al sistema", Descripcion);
                }//fin del if
                break;

        }//fin del switch

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Usuario_CheckedChanged
    ///DESCRIPCIÓN: evento del Check usuario que valida este componente
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Usuario_CheckedChanged(object sender, EventArgs e)
    {

        if (Chk_Usuario.Checked == true)
        {
            Txt_Usuario.Text = "";
            Txt_Usuario.Enabled = true;
        }
        else
        {
            Txt_Usuario.Text = "";
            Txt_Usuario.Enabled = false;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: evento del boton Salir
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 12/Septiembre/2010 
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Todos_Catalogos_CheckedChanged
    ///DESCRIPCIÓN: evento del check seleccionar todos los catalogos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 12/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Todos_Catalogos_CheckedChanged(object sender, EventArgs e)
    {

        //En caso de seleccionar el check Todos se seleccionaran todos los checks del grid view
        if (Chk_Todos_Catalogos.Checked == true )
        {
            Seleccionar_Cheks(Grid_Catalogos, "Chk_catalogo", true);
        }
        // En caso de quitar la seleccion al checkbox todos se limpiaran 
        if ((Chk_Todos_Catalogos.Checked == false))
        {
            Seleccionar_Cheks(Grid_Catalogos, "Chk_catalogo", false);
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_catalogo_CheckedChanged
    ///DESCRIPCIÓN: evento del check "Chk_catalogo" que se encuentra dentro del GridView
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 12/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Chk_catalogo_CheckedChanged(object sender, EventArgs e)
    {
        int Seleccionados = Numero_Checks(Grid_Catalogos, "Chk_catalogo");
        if (Seleccionados == Grid_Catalogos.Rows.Count)
        {
            Chk_Todos_Catalogos.Checked = true;
        }
        else
        {
            Chk_Todos_Catalogos.Checked = false;
        }

    }
    #endregion   
    
}
