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
using Presidencia.Reportes_Tramites.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Ventanilla_Lista_Tramites.Negocio;
using System.Text.RegularExpressions;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Ayudante_CarlosAG;
using Presidencia.Sessiones;
using Presidencia.Ayudante_Informacion;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using Presidencia.Catalogo_Ordenamiento_Territorial_Parametros.Negocio;

public partial class paginas_Tramites_Frm_Ope_Tra_Reportes : System.Web.UI.Page
{
    #region Variables
    private Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio = new Cls_Ope_Tra_Reportes_Negocio();

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Reporte_Negocio = new Cls_Ope_Tra_Reportes_Negocio();
                Limpiar_Componentes();
                Llenar_Grid();
                Llenar_Combo_Dependencia();
                LLenar_Combos_Perito();

                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Tramites.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                //Btn_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Filtro_Dependnecia.Attributes.Add("onclick", Ventana_Modal);
                ViewState["SortDirection"] = "DESC";
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos

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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Dependencia
    ///DESCRIPCIÓN: Hace una consulta a la Base de Datos para obtener las dependencias
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO:  03/Julio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************      
    private void Llenar_Combo_Dependencia()
    {
        Cls_Cat_Tramites_Negocio Negocio_Dependencia = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Dependencias = new DataTable();
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = "";
        DataTable Dt_Consulta_Tramites = new DataTable();
        try
        {
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();
            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;

            Negocio_Dependencia.P_Tipo_DataTable = "DEPENDENCIAS";
            Dt_Dependencias = Negocio_Dependencia.Consultar_DataTable();

            //  se ordenara la tabla por fecha
            DataView Dv_Ordenar = new DataView(Dt_Dependencias);
            Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;
            Dt_Dependencias = Dv_Ordenar.ToTable();

            Cmb_Dependencias.DataSource = Dt_Dependencias;
            Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencias.DataTextField = "NOMBRE";
            Cmb_Dependencias.DataBind();
            Cmb_Dependencias.Items.Insert(0, "< SELECCIONE >");
            Cmb_Dependencias.SelectedIndex = 0;

            Cmb_Filtro_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Filtro_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Filtro_Dependencia.DataTextField = "NOMBRE";
            Cmb_Filtro_Dependencia.DataBind();
            Cmb_Filtro_Dependencia.Items.Insert(0, "< SELECCIONE >");
            Cmb_Filtro_Dependencia.SelectedIndex = 0;

            if ((!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ordenamiento)
              || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ambiental)
              || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Inmobiliario)
              || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Urbanistico))
            {
                Cmb_Dependencias.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                Cmb_Dependencias.Enabled = false;
                Cmb_Filtro_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                Cmb_Filtro_Dependencia.Enabled = false;

                Btn_Buscar_Dependencia.Visible = true;
                Btn_Filtro_Dependnecia.Visible = true;
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: Metodo que limpia los componentes del catalogo
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Limpiar_Componentes()
    {
        try
        {
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            //Chk_Todos.Checked = false;
            Chk_Estatus.Checked = false;
            Llenar_Combo();
            //Cmb_Estatus.Enabled = false;
            Cmb_Estatus.SelectedIndex = 0;
            Chk_Avance.Checked = false;
            Txt_Avance.Text = "0";
            Txt_Avance.Enabled = false;
            //NumericUpDownExtender1.Enabled = false;
            Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", false);
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            Chk_Proximo.Enabled = false;
            Chk_Sin_Demora.Enabled = false;
            Chk_Vencido.Enabled = false;

            Chk_Pendientes_Pago.Checked = false;
            //Cmb_Dependencias.Enabled = false;
            Btn_Buscar_Dependencia.Enabled = true;

            Txt_Cuenta_Predial.Enabled = true;
            Txt_Propietario.Enabled = false;

            //  area de filtro dependencia
            Cmb_Filtro_Dependencia.Enabled = true;

            Btn_Filtro_Dependnecia.Visible = true;

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
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
        try
        {
            //Seleccionamos todos los checks
            for (int i = 0; i < MyGrid.Rows.Count; i++)
            {
                GridViewRow row = MyGrid.Rows[i];
                ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked = Estado;
            }//fin del for i
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

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
        try
        {
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
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Numero_Seleccionados;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 30-Junio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
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
    public void Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        try
        {
            //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
            if ((Txt_Fecha_Inicio.Text.Length == 11) && (Txt_Fecha_Fin.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicio.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Fin.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Reporte_Negocio.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text);
                    Reporte_Negocio.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text);

                }
                else
                {
                    Lbl_Mensaje_Error.Text += "+ Fecha no valida (Fecha inicial mayor a la final) <br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += "+ Fecha no valida (Formato incorrecto) <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

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
    public void Verificar_Fecha_Vigencia()
    {
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        try
        {
            //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
            if ((Txt_Vigencia_Inicio.Text.Length == 11) && (Txt_Vigencia_Fin.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Vigencia_Inicio.Text);
                Date2 = DateTime.Parse(Txt_Vigencia_Fin.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Reporte_Negocio.P_Fecha_Vigencia_Inicial = Formato_Fecha(Txt_Vigencia_Inicio.Text);
                    Reporte_Negocio.P_Fecha_Vigencia_Final = Formato_Fecha(Txt_Vigencia_Fin.Text);

                }
                else
                {
                    Lbl_Mensaje_Error.Text += "+ Fecha no valida (Fecha inicial mayor a la final) <br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += "+ Fecha no valida (Formato incorrecto) <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

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
    public void Verificar_Fecha_Vigencia_Documento()
    {
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        try
        {
            //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
            if ((Txt_Vigencia_Documento_Inicio.Text.Length == 11) && (Txt_Vigencia_Documento_Fin.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Vigencia_Documento_Inicio.Text);
                Date2 = DateTime.Parse(Txt_Vigencia_Documento_Fin.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Reporte_Negocio.P_Fecha_Vigencia_Documento_Inicial = Formato_Fecha(Txt_Vigencia_Documento_Inicio.Text);
                    Reporte_Negocio.P_Fecha_Vigencia_Documento_Final = Formato_Fecha(Txt_Vigencia_Documento_Fin.Text);

                }
                else
                {
                    Lbl_Mensaje_Error.Text += "+ Fecha no valida (Fecha inicial mayor a la final) <br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += "+ Fecha no valida (Formato incorrecto) <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
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
        String Fecha_Valida = "";
        //Se le aplica un split a la fecha 
        String[] aux;
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        try
        {
            Fecha_Valida = Fecha;
            aux = Fecha.Split('/');

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
            Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

        return Fecha_Valida;
    }// fin de Formato_Fecha

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Estatus()
    {
        try
        {
            if (Chk_Estatus.Checked == true)
            {
                if (Cmb_Estatus.SelectedIndex != 0)
                {
                    Reporte_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                }
                else
                {
                    Lbl_Mensaje_Error.Text += "+ Debe seleccionar un Estatus <br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Reporte_Negocio.P_Estatus = null;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public Boolean Validar_Cuenta_Predial()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        try
        {
            Lbl_Mensaje_Error.Text = "";
            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";

            if ((Txt_Propietario.Text == "") && (Txt_Cuenta_Predial.Text == ""))
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La cuenta predial o el propietario.<br>";
                Datos_Validos = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        return Datos_Validos;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Avance
    ///DESCRIPCIÓN: Metodo que valida en caso de que seleccione un Avance
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 21/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Avance()
    {
        int Avance = 0;
        try
        {
            if (Chk_Avance.Checked == false)
            {
                Reporte_Negocio.P_Avance = null;
            }
            else
            {
                if (!String.IsNullOrEmpty(Txt_Avance.Text))
                {
                    Avance = Convert.ToInt32(Txt_Avance.Text);
                    if (Avance > 100)
                    {
                        Lbl_Mensaje_Error.Text += "+ El avance no puede ser mayor de 100 <br />";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                    else
                    {
                        Reporte_Negocio.P_Avance = "" + Avance;
                    }
                    //Asignamos el valor a la clase de negocios en caso de no existir errores. 
                    if (Div_Contenedor_Msj_Error.Visible == false)
                        Reporte_Negocio.P_Avance = Txt_Avance.Text;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
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
        String[] Array_aux;
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
                    //Obtiene el nombre del catalogo seleccionados
                    Check_seleccionado = Convert.ToString(row.Cells[2].Text);
                    //llenamos el arreglo con los nombres de los tramites
                    Array_aux[num] = Check_seleccionado;
                    num = num + 1;
                }
            }//fin del for i
            //Validamos en caso de no seleccionar ningun Tramite 
            if (num == 0)
            {
                Lbl_Mensaje_Error.Text += "+ Debe seleccionar por lo menos un Tramite <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            else
            {
                //Generamos la cadena con los Tramites seleccionados para generar la consulta de oracle y 
                Seleccionados = Generar_Cadena(Array_aux, num);
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
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
            throw new Exception(ex.Message.ToString());
        }
        return Cadena;
    }//fin de generar cadena

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Tramites_Sorting
    ///DESCRIPCIÓN          : ordena las columnas
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramirez Aguilera
    /// FECHA_CREO          : 06/Diciembre/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Tramites_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataTable Dt_Consulta = (DataTable)(Session["GRID_TRAMITES"]);

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

        Grid_Tramites.Columns[2].Visible = true;
        Grid_Tramites.DataSource = Dv_Ordenar;
        Grid_Tramites.DataBind();
        Grid_Tramites.Columns[2].Visible = false;
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
        string filePath = "";
        DataRow renglon;

        try
        {
            filePath = Server.MapPath("../Rpt/Tramites/" + nombre_reporte);
            reporte.Load(filePath);

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
            diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Reportes_Tramites.pdf");
            //5
            exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
            //6
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //7
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //8
            reporte.Export(exportOptions);
            //9
            string ruta = "../../Reporte/Rpt_Reportes_Tramites.pdf";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Data_Set.- contiene la Mostrar_Informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene la Ruta del Reporte a mostrar en pantalla
    ///             4.- Nombre_Xls, nombre con el que se geradara en disco el archivo xls
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 7/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Exportar_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Xls)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = "";
        DataRow Renglon;

        try
        {
            File_Path = Server.MapPath("../Rpt/Tramites/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
            {
                Renglon = Data_Set.Tables[0].Rows[i];
                Ds_Reporte.Tables[0].ImportRow(Renglon);
            }
            Reporte.SetDataSource(Ds_Reporte);

            //1
            ExportOptions Export_Options = new ExportOptions();
            //2
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            //3
            //4
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Xls);
            //5
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            //6
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            //7
            Export_Options.ExportFormatType = ExportFormatType.Excel;
            //8
            Reporte.Export(Export_Options);
            //9
            String Ruta = "../../Reporte/" + Nombre_Xls;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(string Ruta_Archivo, string Contenido)
    {
        try
        {
            System.IO.FileInfo ArchivoExcel = new System.IO.FileInfo(Ruta_Archivo);
            if (ArchivoExcel.Exists)
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentType = Contenido;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ArchivoExcel.Name);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                //Response.WriteFile(ArchivoExcel.FullName);
                Response.End();
            }
        }
        catch (Exception Ex)
        {
            //// Response.End(); siempre genera una excepción (http://support.microsoft.com/kb/312629/EN-US/)
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #region Grid

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
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Dependencia_ID_Ordenamiento = "";
        string Dependencia_ID_Ambiental = "";
        string Dependencia_ID_Urbanistico = "";
        string Dependencia_ID_Inmobiliario = "";
        DataTable Dt_Consulta_Tramites = new DataTable();
        try
        {// consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ambiental))
                Dependencia_ID_Ambiental = Obj_Parametros.P_Dependencia_ID_Ambiental;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Urbanistico))
                Dependencia_ID_Urbanistico = Obj_Parametros.P_Dependencia_ID_Urbanistico;

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Inmobiliario))
                Dependencia_ID_Inmobiliario = Obj_Parametros.P_Dependencia_ID_Inmobiliario;


            //  validacion para las areas de ordenamiento territorial
            if ((!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ordenamiento)
               || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ambiental)
               || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Inmobiliario)
               || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Urbanistico))
            {
                Reporte_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;
            }
            Dt_Consulta_Tramites = Reporte_Negocio.Consulta_Tramites();//   se consultan los tramites
            
            //  se carga la tabla en el grid
            if (Dt_Consulta_Tramites != null && Dt_Consulta_Tramites.Rows.Count > 0)
            {
                Grid_Tramites.Columns[2].Visible = true;
                Grid_Tramites.DataSource = Dt_Consulta_Tramites;
                Grid_Tramites.DataBind(); 
                Grid_Tramites.Columns[2].Visible = false;
                Session["GRID_TRAMITES"] = Dt_Consulta_Tramites;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }


     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Pendientes_Pago
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Pendientes_Pago()
    {
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        try
        {

            Reporte_Negocio.P_Dependencia_ID = Cmb_Dependencias.SelectedValue;
            Dt_Pendientes_Pago = Reporte_Negocio.Consulta_Solicitudes_Pendientes_Pago();

            Dt_Pendientes_Pago.TableName = "Dt_Solicitud_Pago";

            DataSet Data_Reporte = new DataSet();
            Data_Reporte.Clear();
            Data_Reporte.Tables.Add(Dt_Pendientes_Pago.Copy());

            //Generar_Reporte(Data_Reporte, Reporte_Solicitud, "Rpt_Ort_Solicitid_Fecha_Entrega.rpt");

            String Nombre_Archivo = "Reporte_Solicitud_Pendientes_Pagar" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath("../Rpt/Tramites/");//Obtiene la ruta en la cual será guardada el archivo

            Reporte.Load(Ruta_Archivo + "Rpt_Tra_Solicitud_Por_Pagar.rpt");
            Reporte.SetDataSource(Data_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            Nombre_Archivo += ".pdf";
            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Opciones_Exportacion);

            Abrir_Ventana(Nombre_Archivo);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Pendientes_Pago_Excel
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Pendientes_Pago_Excel()
    {
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        try
        {
            Reporte_Negocio.P_Dependencia_ID = Cmb_Dependencias.SelectedValue;
            Dt_Pendientes_Pago = Reporte_Negocio.Consulta_Solicitudes_Pendientes_Pago();

            Dt_Pendientes_Pago.TableName = "Dt_Solicitud_Pago";

            DataSet Data_Reporte = new DataSet();
            Data_Reporte.Clear();
            Data_Reporte.Tables.Add(Dt_Pendientes_Pago.Copy());

            //Generar_Reporte(Data_Reporte, Reporte_Solicitud, "Rpt_Ort_Solicitid_Fecha_Entrega.rpt");

            String Nombre_Archivo = "Reporte_Solicitud_Pendientes_Pagar" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            String Ruta_Archivo = @Server.MapPath("../Rpt/Tramites/");//Obtiene la ruta en la cual será guardada el archivo

            Reporte.Load(Ruta_Archivo + "Rpt_Tra_Solicitud_Por_Pagar.rpt");
            Reporte.SetDataSource(Data_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            Nombre_Archivo += ".xls";
            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(Opciones_Exportacion);

            String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Pendientes_Pago_Excel
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Historial_Cuenta_Predial_Excel()
    {
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        try
        {
            //  filtro para la cuenta predial
            if (Txt_Cuenta_Predial.Text != "")
                Reporte_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;

            //  filtro para el nombre del propietario
            if (Txt_Propietario.Text != "")
                Reporte_Negocio.P_Propietario = Txt_Propietario.Text;

            DataTable Dt_Consulta = Reporte_Negocio.Consulta_Cuenta_Predial_Propietario();
            DataSet Data_Reporte = new DataSet();
            Data_Reporte.Tables.Add(Dt_Consulta.Copy());
            Exportar_Excel(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt", "Rpt_Reportes_Tramites.xls");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Historial_Cuenta_Predial
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Historial_Cuenta_Predial()
    {
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        try
        {
            //  filtro para la cuenta predial
            if (Txt_Cuenta_Predial.Text != "")
                Reporte_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;

            //  filtro para el nombre del propietario
            if (Txt_Propietario.Text != "")
                Reporte_Negocio.P_Propietario = Txt_Propietario.Text;

            DataTable Dt_Consulta = Reporte_Negocio.Consulta_Cuenta_Predial_Propietario();
            DataSet Data_Reporte = new DataSet();
            Data_Reporte.Tables.Add(Dt_Consulta.Copy());
            Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Solicitud_Tramtie
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Solicitud_Tramtie()
    {
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument(); 
        DataTable Dt_Solicitud_Demoradas = new DataTable();
        DataTable Dt_Solicitud_Proximas = new DataTable();
        DataTable Dt_Solicitud_Sin_Demora = new DataTable();
        Ds_Rpt_Ort_Solictud_Fecha_Entrega Reporte_Solicitud = new Ds_Rpt_Ort_Solictud_Fecha_Entrega();
        try
        {
             Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");

             if (Div_Contenedor_Msj_Error.Visible == false)
             {
                 if (Chk_Solicitud_Demorada.Checked == false && Chk_Pendientes_Pago.Checked == false)
                 {
                     //Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");
                     Validar_Estatus();
                     Validar_Avance();
                     Verificar_Fecha();

                     if (Chk_Vigencia.Checked == true)
                         Verificar_Fecha();

                     if (Chk_Vigencia_Documento.Checked == true)
                         Verificar_Fecha_Vigencia();

                     //  filtro para la depenencia
                     if (Chk_Filtro_Dependencia.Checked == true)
                         Reporte_Negocio.P_Dependencia_ID = Cmb_Filtro_Dependencia.SelectedValue;

                     if (Chk_Perito.Checked == true)
                         Reporte_Negocio.P_Perito = Cmb_Perito.SelectedValue;

                     if (Chk_Solicitante.Checked == true)
                         Reporte_Negocio.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();

                     if (Chk_Vigencia.Checked)
                     {
                         DataTable Dt_Vigencia = Reporte_Negocio.Consulta_Por_Vigencia();
                         DataSet Data_Reporte = new DataSet();
                         Data_Reporte.Tables.Add(Dt_Vigencia.Copy());
                         Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt");
                     }
                     if (Chk_Vigencia_Documento.Checked)
                     {
                         Verificar_Fecha_Vigencia_Documento();
                         DataTable Dt_Vigencia = Reporte_Negocio.Consulta_Por_Vigencia_Documento();
                         DataSet Data_Reporte = new DataSet();
                         Data_Reporte.Tables.Add(Dt_Vigencia.Copy());
                         Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites_Vigencia.rpt");
                         //Estatus = false;
                     }

                     if (Div_Contenedor_Msj_Error.Visible == false)
                     {
                         DataTable Dt_Consulta = Reporte_Negocio.Consulta_Solicitudes();
                         DataSet Data_Reporte = new DataSet();
                         Data_Reporte.Tables.Add(Dt_Consulta.Copy());
                         Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt");
                     }
                 }
                 else if (Chk_Solicitud_Demorada.Checked == true)
                 {
                     //Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");
                     Reporte_Negocio.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", DateTime.Today);

                     if (Chk_Vencido.Checked == true)
                         Dt_Solicitud_Demoradas = Reporte_Negocio.Consulta_Solicitudes_Demorando();

                     else
                         Dt_Solicitud_Demoradas = Reporte_Solicitud.Dt_Solicitud_Vencida.Clone();

                     if (Chk_Proximo.Checked == true)
                         Dt_Solicitud_Proximas = Reporte_Negocio.Consulta_Solicitudes_Por_Vencer();
                     else
                         Dt_Solicitud_Proximas = Reporte_Solicitud.Dt_Solicitu_Por_Vencerse.Clone();

                     if (Chk_Sin_Demora.Checked == true)
                         Dt_Solicitud_Sin_Demora = Reporte_Negocio.Consulta_Solicitudes_Con_2_dias_Vencer();
                     else
                         Dt_Solicitud_Sin_Demora = Reporte_Solicitud.Dt_Solicitud_Sin_Vencerse.Clone();

                     //  se nombran las tablas
                     Dt_Solicitud_Sin_Demora.TableName = "Dt_Solicitud_Sin_Vencerse";
                     Dt_Solicitud_Demoradas.TableName = "Dt_Solicitud_Vencida";
                     Dt_Solicitud_Proximas.TableName = "Dt_Solicitu_Por_Vencerse";

                     DataSet Data_Reporte = new DataSet();
                     Data_Reporte.Clear();
                     Data_Reporte.Tables.Add(Dt_Solicitud_Sin_Demora.Copy());
                     Data_Reporte.Tables.Add(Dt_Solicitud_Proximas.Copy());
                     Data_Reporte.Tables.Add(Dt_Solicitud_Demoradas.Copy());

                     //Generar_Reporte(Data_Reporte, Reporte_Solicitud, "Rpt_Ort_Solicitid_Fecha_Entrega.rpt");

                     String Nombre_Archivo = "Reporte_Solicitud_Demorados_Pendientes_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
                     String Ruta_Archivo = @Server.MapPath("../Rpt/Tramites/");//Obtiene la ruta en la cual será guardada el archivo

                     Reporte.Load(Ruta_Archivo + "Rpt_Ort_Solicitid_Fecha_Entrega.rpt");
                     Reporte.SetDataSource(Data_Reporte);

                     DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

                     Nombre_Archivo += ".pdf";
                     Ruta_Archivo = @Server.MapPath("../../Reporte/");
                     m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                     ExportOptions Opciones_Exportacion = new ExportOptions();
                     Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                     Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                     Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                     Reporte.Export(Opciones_Exportacion);

                     Abrir_Ventana(Nombre_Archivo);
                 }// fin else if Chk_Solicitud_Demorada
             }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Solicitud_Tramtie
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Solicitud_Tramtie_Excel()
    {
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataTable Dt_Solicitud_Demoradas = new DataTable();
        DataTable Dt_Solicitud_Proximas = new DataTable();
        DataTable Dt_Solicitud_Sin_Demora = new DataTable();
        Ds_Rpt_Ort_Solictud_Fecha_Entrega Reporte_Solicitud = new Ds_Rpt_Ort_Solictud_Fecha_Entrega();
        try
        {
            Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                if (Chk_Solicitud_Demorada.Checked == false && Chk_Pendientes_Pago.Checked == false)
                {
                    //Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");
                    Validar_Estatus();
                    Validar_Avance();
                    Verificar_Fecha();

                    //  filtro para la depenencia
                    if (Chk_Filtro_Dependencia.Checked == true)
                        Reporte_Negocio.P_Dependencia_ID = Cmb_Filtro_Dependencia.SelectedValue;

                    if (Chk_Perito.Checked == true)
                        Reporte_Negocio.P_Perito = Cmb_Perito.SelectedValue;

                    if (Chk_Solicitante.Checked == true)
                        Reporte_Negocio.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();

                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        DataTable Dt_Consulta = Reporte_Negocio.Consulta_Solicitudes();
                        DataSet Data_Reporte = new DataSet();
                        Data_Reporte.Tables.Add(Dt_Consulta.Copy());
                        Exportar_Excel(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt", "Rpt_Reportes_Tramites.xls");
                    }
                }
                else if (Chk_Solicitud_Demorada.Checked == true)
                {
                    //Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");
                    Reporte_Negocio.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", DateTime.Today);

                    if (Chk_Vencido.Checked == true)
                        Dt_Solicitud_Demoradas = Reporte_Negocio.Consulta_Solicitudes_Demorando();

                    else
                        Dt_Solicitud_Demoradas = Reporte_Solicitud.Dt_Solicitud_Vencida.Clone();

                    if (Chk_Proximo.Checked == true)
                        Dt_Solicitud_Proximas = Reporte_Negocio.Consulta_Solicitudes_Por_Vencer();
                    else
                        Dt_Solicitud_Proximas = Reporte_Solicitud.Dt_Solicitu_Por_Vencerse.Clone();

                    if (Chk_Sin_Demora.Checked == true)
                        Dt_Solicitud_Sin_Demora = Reporte_Negocio.Consulta_Solicitudes_Con_2_dias_Vencer();
                    else
                        Dt_Solicitud_Sin_Demora = Reporte_Solicitud.Dt_Solicitud_Sin_Vencerse.Clone();

                    //  se nombran las tablas
                    Dt_Solicitud_Sin_Demora.TableName = "Dt_Solicitud_Sin_Vencerse";
                    Dt_Solicitud_Demoradas.TableName = "Dt_Solicitud_Vencida";
                    Dt_Solicitud_Proximas.TableName = "Dt_Solicitu_Por_Vencerse";

                    DataSet Data_Reporte = new DataSet();
                    Data_Reporte.Clear();
                    Data_Reporte.Tables.Add(Dt_Solicitud_Sin_Demora.Copy());
                    Data_Reporte.Tables.Add(Dt_Solicitud_Proximas.Copy());
                    Data_Reporte.Tables.Add(Dt_Solicitud_Demoradas.Copy());

                    //Generar_Reporte(Data_Reporte, Reporte_Solicitud, "Rpt_Ort_Solicitid_Fecha_Entrega.rpt");

                    String Nombre_Archivo = "Reporte_Solicitud_Demorados_Pendientes_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
                    String Ruta_Archivo = @Server.MapPath("../Rpt/Tramites/");//Obtiene la ruta en la cual será guardada el archivo

                    Reporte.Load(Ruta_Archivo + "Rpt_Ort_Solicitid_Fecha_Entrega.rpt");
                    Reporte.SetDataSource(Data_Reporte);

                    DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

                    Nombre_Archivo += ".xls";
                    Ruta_Archivo = @Server.MapPath("../../Reporte/");
                    m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                    ExportOptions Opciones_Exportacion = new ExportOptions();
                    Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                    Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                    Reporte.Export(Opciones_Exportacion);

                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }// fin else if Chk_Solicitud_Demorada
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Click
    ///DESCRIPCIÓN: Evento del boton limpiar que manda llamar el metodo Limpiar_Formulario()
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Componentes();
            Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", false);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
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
        try
        {
            //if (Chk_Todos.Checked == true)
            //    Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", true);
            //if (Chk_Todos.Checked == false)
            //    Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", false);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
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
        try
        {
            int Seleccionados = Numero_Checks(Grid_Tramites, "Chk_Tramite");
            //if (Seleccionados == Grid_Tramites.Rows.Count)
            //{
            //    Chk_Todos.Checked = true;
            //}
            //else
            //{
            //    Chk_Todos.Checked = false;
            //}
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Dependencias.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencias.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Filtro_Dependnecia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 21/Sep/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Filtro_Dependnecia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Filtro_Dependencia.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Filtro_Dependencia.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Click
    ///DESCRIPCIÓN: Evento del boton Generar Reporte
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Click(object sender, ImageClickEventArgs e)
    {       
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        Ds_Rpt_Ort_Solictud_Fecha_Entrega Reporte_Solicitud = new Ds_Rpt_Ort_Solictud_Fecha_Entrega();
        DataTable Dt_Solicitud_Demoradas = new DataTable();
        DataTable Dt_Solicitud_Proximas = new DataTable();
        DataTable Dt_Solicitud_Sin_Demora = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            //  para obtener los pagos pendientes
            if (Chk_Pendientes_Pago.Checked == true)
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Cmb_Dependencias.SelectedIndex != 0)
                    Reporte_Pendientes_Pago();

                else 
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Selecciones la unidad responsable por favor";
                }
            }

            else if (Chk_Cuenta_Predial.Checked == true)
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Validar_Cuenta_Predial())
                    Reporte_Historial_Cuenta_Predial();
            }
            
            else
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                Reporte_Solicitud_Tramtie();
            }
        }
        catch (Exception ex)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Tramites/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte";
        }

        String Archivo_Reporte = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            String Ruta_Destino = "";
            Ruta_Destino = Server.MapPath("../../Reporte/" + Archivo_Reporte);
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Reporte);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            if (Formato == ExportFormatType.Excel)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-excel");                

            }
            else if (Formato == ExportFormatType.WordForWindows)
            {
                //Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-word");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN: Evento del boton Exportar a Excel
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 21/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        Ds_Rpt_Ort_Solictud_Fecha_Entrega Reporte_Solicitud = new Ds_Rpt_Ort_Solictud_Fecha_Entrega();
        DataTable Dt_Solicitud_Demoradas = new DataTable();
        DataTable Dt_Solicitud_Proximas = new DataTable();
        DataTable Dt_Solicitud_Sin_Demora = new DataTable();
        DataTable Dt_Pendientes_Pago = new DataTable();        
        ReportDocument Reporte = new ReportDocument();
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";


            //  para obtener los pagos pendientes
            if (Chk_Pendientes_Pago.Checked == true)
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Cmb_Dependencias.SelectedIndex != 0)
                {
                    Reporte_Pendientes_Pago_Excel();
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Selecciones la unidad responsable por favor";
                }

            }// fin else if Chk_Pendientes_Pago

            else if (Chk_Cuenta_Predial.Checked == true)
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Validar_Cuenta_Predial())
                    Reporte_Historial_Cuenta_Predial_Excel();
            }
            else
            {
                Reporte_Solicitud_Tramtie_Excel();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
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
                    
                    //  se limpian las sesiones
                    Session.Remove("TRAMITE_ID");
                    Session.Remove("BUSQUEDA_TRAMITES");
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
            else
            {
                Llenar_Grid();
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Perito_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del Perito seleccionado en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 10-jun-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Perito_Click(object sender, ImageClickEventArgs e)
    {
        // limpiar mensaje de error
        Lbl_Mensaje_Error.Visible = false;
        IBtn_Imagen_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_PERITOS"] != null)
        {
            // si el valor de la sesión es igual a true y el valor de la sesiones PERITO_ID no es nulo ni vacío
            if (Convert.ToBoolean(Session["BUSQUEDA_PERITOS"]) == true && Session["PERITO_ID"] != null && Session["PERITO_ID"].ToString().Length > 0)
            {
                try
                {
                    string Perito_ID = Session["PERITO_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Perito.Items.FindByValue(Perito_ID) != null)
                    {
                        Cmb_Perito.SelectedValue = Perito_ID;
                    }
                    else if (Session["NOMBRE_PERITO"] != null && Session["NOMBRE_PERITO"].ToString().Length > 0)
                    {
                        Cmb_Perito.Items.Add(new ListItem(HttpUtility.HtmlDecode(Session["NOMBRE_PERITO"].ToString()), Perito_ID));
                        Cmb_Perito.SelectedValue = Perito_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    IBtn_Imagen_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message;
                }

                // limpiar variables de sesión
                Session.Remove("PERITO_ID");
                Session.Remove("NOMBRE_PERITO");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_PERITOS");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los datos para los combos y los asigna al combo correspondiente
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 10-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos_Perito()
    {
        var Obj_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Peritos;

        Lbl_Mensaje_Error.Visible = false;
        IBtn_Imagen_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // consultar peritos
            Dt_Peritos = Obj_Peritos.Consultar_Inspectores();
            // cargar datos en el combo
            Cmb_Perito.Items.Clear();
            Cmb_Perito.DataSource = Dt_Peritos;
            Cmb_Perito.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
            Cmb_Perito.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
            Cmb_Perito.DataBind();
            Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), ""));
            Cmb_Perito.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: evento del boton Salir
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Componentes();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Solicitud_Demorada_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus en demoras
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO:  30/Junio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Solicitud_Demorada_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Solicitud_Demorada.Checked == true)
            {
                Chk_Estatus.Checked = false;
                Chk_Estatus_CheckedChanged(sender, null);

                Chk_Avance.Checked = false;
                Chk_Avance_CheckedChanged(sender, null);

                Chk_Pendientes_Pago.Checked = false;
                Chk_Pendientes_Pago_CheckedChanged(sender, null);

                Chk_Cuenta_Predial.Checked = false;
                Chk_Cuenta_Predial_CheckedChanged(sender, null);

                Chk_Proximo.Enabled = true;
                Chk_Sin_Demora.Enabled = true ;
                Chk_Vencido.Enabled = true;
            }
            else
            {
                Chk_Proximo.Enabled = false;
                Chk_Sin_Demora.Enabled = false;
                Chk_Vencido.Enabled = false;

                Chk_Proximo.Checked = false;
                Chk_Sin_Demora.Checked = false;
                Chk_Vencido.Checked = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Pendientes_Pago_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus de las solicitudes que se encuentran pendientes de pagos
    ///PARAMETROS:  
    ///CREO:        Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO:  04/Julio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Pendientes_Pago_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Pendientes_Pago.Checked == true)
            {
                Chk_Estatus.Checked = false;
                Chk_Estatus_CheckedChanged(sender, null);

                Chk_Avance.Checked = false;
                Chk_Avance_CheckedChanged(sender, null);

                Chk_Solicitud_Demorada.Checked = false;
                Chk_Solicitud_Demorada_CheckedChanged(sender, null);

                Chk_Cuenta_Predial.Checked = false;
                Chk_Cuenta_Predial_CheckedChanged(sender, null);

                Cmb_Dependencias.Enabled = true;
                Btn_Buscar_Dependencia.Enabled = true;
            }
            else
            {
                Llenar_Combo_Dependencia();
                Cmb_Dependencias.Enabled = false;
                Btn_Buscar_Dependencia.Enabled = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    
        ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Filtro_Dependencia_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus
    ///PARAMETROS:  
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Filtro_Dependencia_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Filtro_Dependencia.Checked == true)
            {
                Cmb_Filtro_Dependencia.Enabled = true;
            }
            else
            {
                Cmb_Filtro_Dependencia.Enabled = false;
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Cuenta_Predial_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Cuenta_Predial_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Cuenta_Predial.Checked == true)
            {
                Chk_Estatus.Checked = false;
                Chk_Estatus_CheckedChanged(sender, null);

                Chk_Avance.Checked = false;
                Chk_Avance_CheckedChanged(sender, null);

                Chk_Solicitud_Demorada.Checked = false;
                Chk_Solicitud_Demorada_CheckedChanged(sender, null);
                
                Chk_Pendientes_Pago.Checked = false;
                Chk_Pendientes_Pago_CheckedChanged(sender, null);

                Txt_Cuenta_Predial.Enabled = true;
                Txt_Cuenta_Predial.Text = "";

                Txt_Propietario.Enabled = true;
                Txt_Propietario.Text = "";
               
            }
            else
            {
                Txt_Cuenta_Predial.Enabled = false;
                Txt_Cuenta_Predial.Text = "";

                Txt_Propietario.Enabled = false;
                Txt_Propietario.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    
    #endregion
}
