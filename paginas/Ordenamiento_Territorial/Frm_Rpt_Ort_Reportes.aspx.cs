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
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;
using Presidencia.Bandeja_Solicitudes_Tramites.Negocio;
using CarlosAg.ExcelXmlWriter;
using System.Drawing;

public partial class paginas_Ordenamiento_Terrirotial_Frm_Rpt_Ort_Reportes : System.Web.UI.Page
{
    #region Variables
    private Cls_Ope_Tra_Reportes_Negocio Reporte_Negocio = new Cls_Ope_Tra_Reportes_Negocio();

    #endregion

    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Session["Activa"] = true;//Variable para mantener la session activa.

            if (!IsPostBack)
            {
                Reporte_Negocio = new Cls_Ope_Tra_Reportes_Negocio();
                Limpiar_Componentes();
                Llenar_Grid(); 
                Habilitar_Reporte_Modulo();
                Llenar_Combo_Dependencia();
                LLenar_Combos_Perito();
                ViewState["SortDirection"] = "DESC";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
                Cmb_Estatus.Items.Add("< SELECCIONE >");
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
            Mostrar_Mensaje_Error(true, "Llenar_Combo: " + ex.Message.ToString());
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
        StringBuilder Expresion_Sql = new StringBuilder();
        try
        {
            //Btn_Buscar_Dependencia.Visible = true;
            //Btn_Filtro_Dependnecia.Visible = true;

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


            //  filtro para obtener las areas de los parametros de ordenamiento
            Expresion_Sql.Append(Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Ordenamiento + "'");
            Expresion_Sql.Append(" or " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Ambiental + "'");
            Expresion_Sql.Append(" or " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Urbanistico + "'");
            Expresion_Sql.Append(" or " + Cat_Dependencias.Campo_Dependencia_ID + "='" + Dependencia_ID_Inmobiliario + "'");

            Negocio_Dependencia.P_Tipo_DataTable = "DEPENDENCIAS";
            Dt_Dependencias = Negocio_Dependencia.Consultar_DataTable();

            DataRow[] Drow_Dependencias_Ordenamiento = Dt_Dependencias.Select(Expresion_Sql.ToString());

            Dt_Dependencias = (DataTable)(Drow_Dependencias_Ordenamiento.CopyToDataTable());

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

            //if ((!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ambiental)
            //  || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Inmobiliario)
            //  || (!string.IsNullOrEmpty(Cls_Sessiones.Dependencia_ID_Empleado) && Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Urbanistico))
            //{
            //    Cmb_Dependencias.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            //    Cmb_Dependencias.Enabled = false;
            //    Cmb_Filtro_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            //    Cmb_Filtro_Dependencia.Enabled = false;

            //    Btn_Buscar_Dependencia.Visible = false;
            //    Btn_Filtro_Dependnecia.Visible = false;
            //}

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(false, "");

            Llenar_Combo();

            Cmb_Estatus.SelectedIndex = 0;
            Cmb_Filtro_Dependencia.Enabled = true;
            Cmb_Perito.SelectedIndex = 0;
            Cmb_Tipo_Estatus.SelectedIndex = 0;
            Cmb_Dependencias.SelectedIndex = 0;
            Cmb_Filtro_Dependencia.SelectedIndex = 0;

            Seleccionar_Cheks(Grid_Tramites, "Chk_Tramite", false);

            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Avance.Text = "0";
            Txt_Avance.Enabled = false;
            Txt_Cuenta_Predial.Enabled = true;
            Txt_Calle.Text = "";
            Txt_Colonia.Text = "";
            Txt_Cuenta_Predial.Text = "";
            Txt_Folio.Text = "";
            Txt_Propietario.Text = "";
            Txt_Solicitante.Text = "";
            Txt_Vigencia_Documento_Fin.Text = "";
            Txt_Vigencia_Documento_Inicio.Text = "";
            Txt_Vigencia_Fin.Text = "";
            Txt_Vigencia_Inicio.Text = "";

            Chk_Avance.Checked = false;
            Chk_Proximo.Enabled = false;
            Chk_Estatus.Checked = false;
            Chk_Sin_Demora.Enabled = false;
            Chk_Vencido.Enabled = false;
            Chk_Pendientes_Pago.Checked = false;
            Chk_Calle.Checked = false;
            Chk_Colonia.Checked = false;
            Chk_Filtro_Dependencia.Checked = false;
            Chk_Cuenta_Predial.Checked = false;
            Chk_Folio.Checked = false;
            Chk_Modulo.Checked = false;
            Chk_Perito.Checked = false;
            Chk_Propietario_Predio.Checked = false;
            Chk_Reporte_Archivo.Checked = false;
            Chk_Reporte_Demorados.Checked = false;
            Chk_Reporte_Solicitud.Checked = false;
            Chk_Sin_Demora.Checked = false;
            Chk_Solicitante.Checked = false;
            Chk_Solicitud_Demorada.Checked = false;
            Chk_Solicitudes_Pagadas.Checked = false;
            Chk_Solicitudes_Pendientes_Pago.Checked = false;
            Chk_Vencido.Checked = false;
            Chk_Vigencia.Checked = false;
            Chk_Vigencia_Documento.Checked = false;

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
                    Div_Contenedor_Msj_Error.Visible = true;
                    Mostrar_Mensaje_Error(true, "* Fecha no valida (Fecha inicial mayor a la final) <br />");
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text += "* Fecha no valida (Formato incorrecto) <br />";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            }
            else
            {
                Reporte_Negocio.P_Estatus = null;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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

    public void Validar_Calle()
    {
        try
        {
            if (Chk_Calle.Checked == true)
            {
                if (Txt_Calle.Text != "")
                {
                    Reporte_Negocio.P_Calle = Txt_Calle.Text;
                }
            }
            else
            {
                Reporte_Negocio.P_Estatus = null;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
                Mostrar_Mensaje_Error(true, "+ Debe seleccionar por lo menos un Tramite <br />");
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        return Cadena;
    }//fin de generar cadena

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
    public void Generar_Reporte(DataSet data_set, DataSet ds_reporte, string Nombre_Reporte, String Formato, String Carpeta)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte = "";
        DataRow Renglon;
        String Ruta = "";

        try
        {
            Ruta_Reporte = Server.MapPath("../Rpt/" + Carpeta + "/" + Nombre_Reporte);
            Reporte.Load(Ruta_Reporte);

            for (int Cnt_Tabla = 0; Cnt_Tabla < data_set.Tables[0].Rows.Count; Cnt_Tabla++)
            {
                Renglon = data_set.Tables[0].Rows[Cnt_Tabla];
                ds_reporte.Tables[0].ImportRow(Renglon);
            }

            Reporte.SetDataSource(ds_reporte);

            //1
            ExportOptions exportOptions = new ExportOptions();
            //2
            DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();           
            //4
            if (Formato == "PDF")
            {
                if (Nombre_Reporte == "Rpt_Ort_Demorados.rpt")
                    diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Ort_Demorados.pdf");

                else
                    diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Reportes_Tramites.pdf");
            }
            else
            {
                if (Nombre_Reporte == "Rpt_Ort_Demorados.rpt")
                    diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Ort_Demorados.xls");

                else
                    diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Reportes_Tramites.xls");
            }
            //5
            exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
            //6
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //7

            if (Formato == "PDF")
                exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            else
                exportOptions.ExportFormatType = ExportFormatType.Excel;

            //8
            Reporte.Export(exportOptions);
            //9
            if (Formato == "PDF")
            {
                if (Nombre_Reporte == "Rpt_Ort_Demorados.rpt")
                {
                    Ruta = "../../Reporte/Rpt_Ort_Demorados.pdf";
                }
                else
                {
                    Ruta = "../../Reporte/Rpt_Reportes_Tramites.pdf";
                }
            }
            else
            {
                if (Nombre_Reporte == "Rpt_Ort_Demorados.rpt")
                {
                    Ruta = "../../Reporte/Rpt_Ort_Demorados.xls";
                }
                else
                {
                    Ruta = "../../Reporte/Rpt_Reportes_Tramites.xls";
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
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
    public void Generar_Reporte_Ordenamiento(DataSet data_set, DataSet ds_reporte, string nombre_reporte)
    {
        ReportDocument reporte = new ReportDocument();
        string filePath = "";

        try
        {
            filePath = Server.MapPath("../Rpt/Ordenamiento_Territorial/" + nombre_reporte);
            reporte.Load(filePath);

            reporte.SetDataSource(ds_reporte);

            //1
            ExportOptions exportOptions = new ExportOptions();
            //2
            DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
            //3
            //4
            diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Reportes_Ordenamiento_Pagos.pdf");
            //5
            exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
            //6
            exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
            //7
            exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
            //8
            reporte.Export(exportOptions);
            //9
            string ruta = "../../Reporte/Rpt_Reportes_Ordenamiento_Pagos.pdf";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, "Error al mostrar el reporte en excel. Error: [" + Ex.Message.ToString() + "]");
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }
     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Mensaje_Error
    ///DESCRIPCIÓN: Metodo que llena el grid view con el metodo de Consulta_tramites
    ///PARAMETROS:   
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  23/Octubre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Mensaje_Error(Boolean Estatus, String Mensaje)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = Estatus;
            IBtn_Imagen_Error.Visible = Estatus;
            Lbl_Mensaje_Error.Visible = Estatus;
            Lbl_Mensaje_Error.Text = Mensaje;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(true, Ex.Message.ToString());
            Div_Contenedor_Msj_Error.Visible = Estatus;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]"; 
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }
   
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Reporte_Modulo
    ///DESCRIPCIÓN: Metodo que llena el grid view con el metodo de Consulta_tramites
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 24/Noviembre/20102
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Reporte_Modulo()
    {
        Cls_Cat_Ort_Parametros_Negocio Obj_Parametros = new Cls_Cat_Ort_Parametros_Negocio();
        string Dependencia_ID_Ordenamiento = "";
        try
        {
            // consultar parámetros
            Obj_Parametros.Consultar_Parametros();

            // validar que la consulta haya regresado valor
            if (!string.IsNullOrEmpty(Obj_Parametros.P_Dependencia_ID_Ordenamiento))
                Dependencia_ID_Ordenamiento = Obj_Parametros.P_Dependencia_ID_Ordenamiento;

            if (Cls_Sessiones.Dependencia_ID_Empleado == Dependencia_ID_Ordenamiento)
            {
                Pnl_Modulo.Style.Value = "Display: block";
                Chk_Reporte_Archivo.Enabled = true;
                Pnl_Pagos.Style.Value = "Display: block";
            }

            else
            {
                Pnl_Modulo.Style.Value = "Display: none";
                Pnl_Pagos.Style.Value = "Display: none";
                Chk_Reporte_Archivo.Enabled = false;
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Filtros
    ///DESCRIPCIÓN: Metodo que llena el grid view con el metodo de Consulta_tramites
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 24/Noviembre/20102
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Filtros(String Reporte,Boolean Estatus)
    {
        try
        {
            if (Reporte == "Solicitud")
            {
                Chk_Avance.Enabled = Estatus;
                Chk_Estatus.Enabled = Estatus;
                Chk_Filtro_Dependencia.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;
                Chk_Solicitante.Enabled = Estatus;
                Chk_Folio.Enabled = Estatus;

                Chk_Perito.Enabled = !Estatus;
                Chk_Calle.Enabled = !Estatus;
                Chk_Colonia.Enabled = !Estatus;
                Chk_Propietario_Predio.Enabled = !Estatus;
            }
            else if (Reporte == "Demorados")
            {
                Chk_Avance.Enabled = Estatus;
                Chk_Estatus.Enabled = Estatus;
                Chk_Filtro_Dependencia.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;
                Chk_Solicitante.Enabled = Estatus;
                Chk_Folio.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;

                Chk_Calle.Enabled = !Estatus;
                Chk_Colonia.Enabled = !Estatus;
                Chk_Propietario_Predio.Enabled = !Estatus;
            }
            else if (Reporte == "Archivo")
            {
                Chk_Avance.Enabled = Estatus;
                Chk_Estatus.Enabled = Estatus;
                Chk_Filtro_Dependencia.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;
                Chk_Solicitante.Enabled = Estatus;
                Chk_Folio.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;
                Chk_Calle.Enabled = Estatus;
                Chk_Colonia.Enabled = Estatus;
                Chk_Propietario_Predio.Enabled = Estatus;
            }
            else
            {
                Chk_Avance.Enabled = Estatus;
                Chk_Estatus.Enabled = Estatus;
                Chk_Filtro_Dependencia.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;
                Chk_Solicitante.Enabled = Estatus;
                Chk_Folio.Enabled = Estatus;
                Chk_Perito.Enabled = Estatus;
                Chk_Calle.Enabled = Estatus;
                Chk_Colonia.Enabled = Estatus;
                Chk_Propietario_Predio.Enabled = Estatus;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Demorados
    ///DESCRIPCIÓN: Metodo que genera el reporte del expediente de archivos
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Octubre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Demorados(String Tipo_Archivo)
    {
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        StringBuilder Expresion_Sql = new StringBuilder();
        String Responsable = "";
        Cls_Ope_Bandeja_Tramites_Negocio Negocio_Consulta_Solicitud = new Cls_Ope_Bandeja_Tramites_Negocio();
        try
        {
            Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");

            Validar_Estatus();
            Validar_Avance();
            Verificar_Fecha();

            Reporte_Negocio.P_Demorados = "Demorados";

            if (Cmb_Filtro_Dependencia.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Dependencia_ID = Cmb_Filtro_Dependencia.SelectedValue;
            }

            if (Cmb_Perito.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Perito = Cmb_Perito.SelectedValue;
            }

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                DataTable Dt_Consulta = Reporte_Negocio.Consultar_Solicitud_Ordenamiento();


                //  se ordenara la tabla por fecha
                DataView Dv_Ordenar = new DataView(Dt_Consulta);
                Dv_Ordenar.Sort = "CLAVE_SOLICITUD asc, CONSECUTIVO";
                Dt_Consulta = Dv_Ordenar.ToTable();

                Dt_Consulta.Columns.Add("NOMBRE_EMPLEADO");

                //NOMBRE_EMPLEADO
                Expresion_Sql.Append("");
                DataTable Dt_Responsable = new DataTable();
                int Cnt_For = 0 ;
                
                //  se llenara la columna del responsable de la demora
                if (Dt_Consulta != null)
                {
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        foreach (DataRow Registro in Dt_Consulta.Rows)
                        {
                            if (!String.IsNullOrEmpty(Registro[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString()))
                            {
                                Responsable = "";
                                Reporte_Negocio.P_Actividad_ID = Registro[Ope_Tra_Solicitud.Campo_Subproceso_ID].ToString();
                                Dt_Responsable = Reporte_Negocio.Consultar_Responsable_Demora();

                                if (Dt_Responsable is DataTable)
                                {
                                    if (Dt_Responsable != null)
                                    {
                                        if (Dt_Responsable.Rows.Count > 0)
                                        {
                                            foreach (DataRow Registro_Responsable in Dt_Responsable.Rows)
                                            {
                                                if (Registro_Responsable is DataRow)
                                                {
                                                    if (!String.IsNullOrEmpty(Registro_Responsable["NOMBRE_RESPONSABLE"].ToString()))
                                                    {
                                                        Responsable += Registro_Responsable["NOMBRE_RESPONSABLE"].ToString() + ", ";
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                //  se actualiza el campo del responsable de la demora
                                DataRow Dr_Registro = (DataRow)Dt_Consulta.Rows[Cnt_For];
                                Dr_Registro.BeginEdit();
                                Dr_Registro["NOMBRE_EMPLEADO"] = Responsable;
                                Dr_Registro.EndEdit();

                                Cnt_For++;

                            }// fin del if

                        }
                    }
                }
                Dt_Consulta.AcceptChanges();

                DataSet Data_Reporte = new DataSet();
                Data_Reporte.Tables.Add(Dt_Consulta.Copy());
                Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Ort_Demorados.rpt", Tipo_Archivo, "Ordenamiento_Territorial");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Archivo
    ///DESCRIPCIÓN: Metodo que genera el reporte del expediente de archivos
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Octubre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Archivo(String Tipo_Archivo)
    {
        Ds_Reportes_Tramites Reporte_Tramites = new Ds_Reportes_Tramites();
        DataTable Dt_Pendientes_Pago = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        try
        {
            Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                if (Chk_Solicitud_Demorada.Checked == false && Chk_Pendientes_Pago.Checked == false)
                {
                    Validar_Estatus();
                    Validar_Avance();
                    Verificar_Fecha();

                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        //  filtro para la depenencia
                        if (Chk_Filtro_Dependencia.Checked == true)
                            Reporte_Negocio.P_Dependencia_ID = Cmb_Filtro_Dependencia.SelectedValue;

                        if (Chk_Perito.Checked == true)
                            Reporte_Negocio.P_Perito = Cmb_Perito.SelectedValue;

                        if (Chk_Solicitante.Checked == true)
                            Reporte_Negocio.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();

                        if (Chk_Propietario_Predio.Checked == true)
                            Reporte_Negocio.P_Propietario = Txt_Propietario.Text.Trim().ToUpper();
                         
                        //  filtro calle
                        if (Chk_Calle.Checked == true)
                            Reporte_Negocio.P_Calle = Txt_Calle.Text.Trim().ToUpper();

                        //  filtro colonia
                        if (Chk_Colonia.Checked == true)
                            Reporte_Negocio.P_Colonia = Txt_Colonia.Text.Trim().ToUpper();

                        if (Chk_Folio.Checked == true)
                            Reporte_Negocio.P_Folio = Txt_Folio.Text.Trim().ToUpper();

                        Reporte_Negocio.P_Formato = "Archivo";

                        if (Div_Contenedor_Msj_Error.Visible == false)
                        {
                            DataTable Dt_Consulta = Reporte_Negocio.Consultar_Solicitud_Ordenamiento();
                            DataSet Data_Reporte = new DataSet();
                            Data_Reporte.Tables.Add(Dt_Consulta.Copy());
                            Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Ort_Archivo.rpt", Tipo_Archivo, "Ordenamiento_Territorial");
                        }
                        else
                        {
                           
                        }
                    }
                    else
                    {
                        Mostrar_Mensaje_Error(true, "Seleccione el tramite");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt", "PDF", "Tramites");
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Vigencia
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Vigencia(String Formato)
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
            //Reporte_Negocio.P_Tramites = Check_Box_Seleccionados(Grid_Tramites, "Chk_Tramite");

            if (Chk_Vigencia.Checked)
            {
                Verificar_Fecha_Vigencia();
            }
            if (Chk_Vigencia_Documento.Checked)
            {
                Verificar_Fecha_Vigencia_Documento();
            }

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                DataTable Dt_Vigencia = Reporte_Negocio.Consulta_Por_Vigencia();
                DataSet Data_Reporte = new DataSet();
                Data_Reporte.Tables.Add(Dt_Vigencia.Copy());
                Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites_Vigencia.rpt", Formato, "Tramites");
            }
            else
            {
                Mostrar_Mensaje_Error(true, "Seleccione el tramite");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());

        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Tabla_Responsable
    ///DESCRIPCIÓN: Metodo que genera el reporte de modulo
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 22/Noviembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Generar_Tabla_Responsable(DataTable  Dt_Reporte_Responsable)
    {
        DataTable Dt_Nueva_Tabla = new DataTable();
        Ds_Rpt_Ort_Modulo Ds_Reporte = new Ds_Rpt_Ort_Modulo();
        String Consecutivo = "";
        String Valor_Comparacion = "";
        DataRow Renglon;
        try
        {
            Dt_Nueva_Tabla = Ds_Reporte.Dt_Rpt_Responsable.Copy();

            if (Dt_Reporte_Responsable is DataTable)
            {
                if (Dt_Reporte_Responsable != null && Dt_Reporte_Responsable.Rows.Count > 0) ;
                {
                    foreach (DataRow Registro in Dt_Reporte_Responsable.Rows)
                    {
                        Valor_Comparacion = Registro["CONSECUTIVO"].ToString();

                        if (Consecutivo != Valor_Comparacion)
                        {
                            Consecutivo = Valor_Comparacion;

                            Renglon = Dt_Nueva_Tabla.NewRow();
                            Renglon["CONSECUTIVO"] = Registro["CONSECUTIVO"].ToString();
                            Renglon["NOMBRE_EMPLEADO"] = Registro["NOMBRE_EMPLEADO"].ToString();
                            Dt_Nueva_Tabla.Rows.Add(Renglon);
                        }
                    }
                }
            }
            Dt_Nueva_Tabla.AcceptChanges();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());

        }
        return Dt_Nueva_Tabla;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Tabla_Reporte
    ///DESCRIPCIÓN: Metodo que genera el reporte de modulo
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 22/Noviembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Generar_Tabla_Reporte(DataTable Dt_Reporte_General, DataTable Dt_Reporte_M2, DataTable Dt_Reporte_Responsable)
    {
        Ds_Rpt_Ort_Modulo Ds_Reporte = new Ds_Rpt_Ort_Modulo();
        String Consecutivo = "";
        String Valor_Comparacion = "";
        DataRow Renglon;
        int Cnt_Filas = 0;
        try
        {

            Dt_Reporte_General.Columns.Add("VALOR");
            Dt_Reporte_General.Columns.Add("NOMBRE_EMPLEADO");
            Dt_Reporte_General.AcceptChanges();

            if (Dt_Reporte_General is DataTable)
            {
                if (Dt_Reporte_General != null && Dt_Reporte_General.Rows.Count > 0) ;
                {
                    foreach (DataRow Registro in Dt_Reporte_General.Rows)
                    {
                        Consecutivo = Registro["CONSECUTIVO"].ToString();

                        //  para el valor de M2
                        foreach (DataRow Registro_M2 in Dt_Reporte_M2.Rows)
                        {
                            Valor_Comparacion = Registro_M2["CONSECUTIVO"].ToString();

                            if (Consecutivo == Valor_Comparacion)
                            {
                                Dt_Reporte_General.Rows[Cnt_Filas]["VALOR"] = Registro_M2["VALOR"].ToString();
                                break;
                            }
                        }

                        //  para el responsable
                        foreach (DataRow Registro_Responsable in Dt_Reporte_Responsable.Rows)
                        {
                            Valor_Comparacion = Registro_Responsable["CONSECUTIVO"].ToString();

                            if (Consecutivo == Valor_Comparacion)
                            {
                                Dt_Reporte_General.Rows[Cnt_Filas]["NOMBRE_EMPLEADO"] = Registro_Responsable["NOMBRE_EMPLEADO"].ToString();
                                break;
                            }
                        }

                        Cnt_Filas++;
                    }
                }
            }
            Dt_Reporte_General.AcceptChanges();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());

        }
        return Dt_Reporte_General;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Modulo
    ///DESCRIPCIÓN: Metodo que genera el reporte de modulo
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 22/Noviembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Modulo(String Formato)
    {
        Ds_Rpt_Ort_Modulo Ds_Modulo = new Ds_Rpt_Ort_Modulo(); 
        DataSet Ds_Reporte_Modulo = new DataSet();
        DataTable Dt_Modulo_General = new DataTable();
        DataTable Dt_Modulo_Metros = new DataTable();
        DataTable Dt_Modulo_Responsable = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        Cls_Ope_Tra_Reportes_Negocio Negocio_Reporte_Modulo = new Cls_Ope_Tra_Reportes_Negocio();
        try
        {
            Verificar_Fecha();

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                Negocio_Reporte_Modulo.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text);
                Negocio_Reporte_Modulo.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text);

                Dt_Modulo_General = Negocio_Reporte_Modulo.Consulta_Reporte_Modulo_Principal();
                Dt_Modulo_Metros = Negocio_Reporte_Modulo.Consulta_Reporte_Modulo_M2();
                Dt_Modulo_Responsable = Negocio_Reporte_Modulo.Consulta_Reporte_Modulo_Responsable();

                Dt_Modulo_Responsable = Generar_Tabla_Responsable(Dt_Modulo_Responsable);
                Dt_Modulo_General = Generar_Tabla_Reporte(Dt_Modulo_General, Dt_Modulo_Metros, Dt_Modulo_Responsable);

                Dt_Modulo_General.TableName = "Dt_Rpt_Ordenamiento_Modulo";
                Dt_Modulo_Metros.TableName = "Dt_Rpt_Valor_Dictamen";
                Dt_Modulo_Responsable.TableName = "Dt_Rpt_Responsable";

                Ds_Reporte_Modulo.Tables.Add(Dt_Modulo_General.Copy());
                Ds_Reporte_Modulo.Tables.Add(Dt_Modulo_Metros.Copy());
                Ds_Reporte_Modulo.Tables.Add(Dt_Modulo_Responsable.Copy());
                //Generar_Reporte_Modulo(Ds_Reporte_Modulo, Ds_Modulo, "Rpt_Ort_Modulo.rpt", Formato, "Tramites");


                String Nombre_Archivo = "Reporte_Modulo_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today));
                String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo

                Reporte.Load(Ruta_Archivo + "Rpt_Ort_Modulo.rpt");
                Reporte.SetDataSource(Ds_Reporte_Modulo);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

                if (Formato == "PDF")
                    Nombre_Archivo += ".pdf";
                else
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                if (Formato == "PDF")
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;

                else
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Formato == "PDF")
                    Abrir_Ventana(Nombre_Archivo);

                else
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }

            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());

        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Modulo_Excel
    ///DESCRIPCIÓN: Metodo que genera el reporte de modulo
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 22/Noviembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Modulo_Excel()
    {
        WorksheetCell Celda = new WorksheetCell();
        String Nombre_Archivo = "";
        String Ruta_Archivo = "";
        Double Suma_Costo = 0.0;
        TimeSpan Dias = new TimeSpan();
        Ds_Rpt_Ort_Modulo Ds_Modulo = new Ds_Rpt_Ort_Modulo();
        DataSet Ds_Reporte_Modulo = new DataSet();
        DataTable Dt_Modulo_General = new DataTable();
        DataTable Dt_Modulo_Metros = new DataTable();
        DataTable Dt_Modulo_Responsable = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        Cls_Ope_Tra_Reportes_Negocio Negocio_Reporte_Modulo = new Cls_Ope_Tra_Reportes_Negocio();
        Color Color_Celda = Color.Yellow;
        Color Color_Encabezado = Color.White;
        Color Color_Encabezado_Especial = Color.HotPink; 
        try
        {

            Verificar_Fecha();

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                Negocio_Reporte_Modulo.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicio.Text);
                Negocio_Reporte_Modulo.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Fin.Text);

                Dt_Modulo_General = Negocio_Reporte_Modulo.Consulta_Reporte_Modulo_Principal();
                Dt_Modulo_Metros = Negocio_Reporte_Modulo.Consulta_Reporte_Modulo_M2();
                Dt_Modulo_Responsable = Negocio_Reporte_Modulo.Consulta_Reporte_Modulo_Responsable();

                Dt_Modulo_Responsable = Generar_Tabla_Responsable(Dt_Modulo_Responsable);
                Dt_Modulo_General = Generar_Tabla_Reporte(Dt_Modulo_General, Dt_Modulo_Metros, Dt_Modulo_Responsable);


                #region Estructura_Principal_Documento
                //  Creamos el libro de Excel.
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

                Libro.Properties.Title = "Reporte modulo";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "Presidencia_Irapuato";

                //  Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("Encabezado");
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera_Especial_Rosa = Libro.Styles.Add("Encabezado_Rosa");
                //  Creamos el estilo cabecera 2 para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido_Nombre = Libro.Styles.Add("Contenido_Nombre");
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido_Nombre_Sin_Bordes = Libro.Styles.Add("Contenido_Nombre_Sin_Bordes");
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido_Total = Libro.Styles.Add("Contenido_Total");
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido_Numerico = Libro.Styles.Add("Contenido_Numerico");
                #endregion

                #region Estilo
                //  estilo para la cabecera    Encabezado
                Estilo_Cabecera.Font.FontName = "Arial";
                Estilo_Cabecera.Font.Size = 10;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Cabecera.Alignment.Rotate = 0;
                Estilo_Cabecera.Font.Color = "#000000";
                Estilo_Cabecera.Interior.Color = Color_Encabezado.Name.ToString();
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //  estilo para la cabecera    Encabezado
                Estilo_Cabecera_Especial_Rosa.Font.FontName = "Arial";
                Estilo_Cabecera_Especial_Rosa.Font.Size = 10;
                Estilo_Cabecera_Especial_Rosa.Font.Bold = true;
                Estilo_Cabecera_Especial_Rosa.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera_Especial_Rosa.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Cabecera_Especial_Rosa.Alignment.Rotate = 0;
                Estilo_Cabecera_Especial_Rosa.Font.Color = "#000000";
                Estilo_Cabecera_Especial_Rosa.Interior.Color = Color_Encabezado_Especial.Name.ToString();
                Estilo_Cabecera_Especial_Rosa.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera_Especial_Rosa.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera_Especial_Rosa.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera_Especial_Rosa.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera_Especial_Rosa.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");


                //estilo para el    Contenido_Nombre
                Estilo_Contenido_Nombre.Font.FontName = "Arial";
                Estilo_Contenido_Nombre.Font.Size = 8;
                Estilo_Contenido_Nombre.Font.Bold = false;
                Estilo_Contenido_Nombre.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                Estilo_Contenido_Nombre.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Contenido_Nombre.Alignment.Rotate = 0;
                Estilo_Contenido_Nombre.Font.Color = "#000000";
                Estilo_Contenido_Nombre.Interior.Color = "White";
                Estilo_Contenido_Nombre.Interior.Pattern = StyleInteriorPattern.None;
                Estilo_Contenido_Nombre.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Nombre.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Nombre.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Nombre.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");



                //estilo para el    Estilo_Contenido_Nombre_Sin_Bordes
                Estilo_Contenido_Nombre_Sin_Bordes.Font.FontName = "Arial";
                Estilo_Contenido_Nombre_Sin_Bordes.Font.Size = 8;
                Estilo_Contenido_Nombre_Sin_Bordes.Font.Bold = false;
                Estilo_Contenido_Nombre_Sin_Bordes.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                Estilo_Contenido_Nombre_Sin_Bordes.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Contenido_Nombre_Sin_Bordes.Alignment.Rotate = 0;
                Estilo_Contenido_Nombre_Sin_Bordes.Font.Color = "#000000";
                Estilo_Contenido_Nombre_Sin_Bordes.Interior.Color = "White";
                Estilo_Contenido_Nombre_Sin_Bordes.Interior.Pattern = StyleInteriorPattern.None;
                //Estilo_Contenido_Nombre.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                //Estilo_Contenido_Nombre.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                //Estilo_Contenido_Nombre.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                //Estilo_Contenido_Nombre.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para el    Estilo_Contenido_Nombre_Sin_Bordes
                Estilo_Contenido_Total.Font.FontName = "Arial";
                Estilo_Contenido_Total.Font.Size = 10;
                Estilo_Contenido_Total.Font.Bold = true;
                Estilo_Contenido_Total.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                Estilo_Contenido_Total.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Contenido_Total.Alignment.Rotate = 0;
                Estilo_Contenido_Total.Font.Color = "#000000";
                Estilo_Contenido_Total.Interior.Color = Color_Celda.Name.ToString();
                Estilo_Contenido_Total.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido_Total.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Total.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Total.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Total.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para el    Estilo_Contenido_Nombre_Sin_Bordes
                Estilo_Contenido_Numerico.Font.FontName = "Arial";
                Estilo_Contenido_Numerico.Font.Size = 8;
                Estilo_Contenido_Numerico.Font.Bold = false;
                Estilo_Contenido_Numerico.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                Estilo_Contenido_Numerico.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Contenido_Numerico.Alignment.Rotate = 0;
                Estilo_Contenido_Numerico.Font.Color = "#000000";
                Estilo_Contenido_Numerico.Interior.Color = "White";
                Estilo_Contenido_Numerico.Interior.Pattern = StyleInteriorPattern.None;
                Estilo_Contenido_Numerico.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Numerico.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Numerico.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido_Numerico.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                #endregion

                #region Contenido_Documento
                Nombre_Archivo = "Reporte_Modulo_" + DateTime.Now.Year.ToString() + "_" + DateTime.Now.Month.ToString() + "_" + DateTime.Now.Day.ToString() + ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/" + Nombre_Archivo);

                #region Hojas
                //  Creamos una hoja
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Modulo");
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                #endregion

                #region Columnas
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));   //  1 Consecutivo.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));   //  2 Situacion
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));   //  3 fecha_recepcion
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));   //  4 ingreso por
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));   //  5 datos del solicitante
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));   //  6 datos del propietario por
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));   //  7 Asunto
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));   //  8 Rubro
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  9 Giro
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  10 CUENTA PREDIAL
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  11 Calle
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  12 numero
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  13 lote
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  14 manzana
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  15 Colonia
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  16 CORREO ELECTRONICO
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  17 m2
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  18 ml
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  19 DRO
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  20 Nombre dro
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  21 Colegio
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  22 Dependencia
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  23 Zona
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  24 TURNADO A
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  25 observaciones
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  26 FECHA PROMESA DE ENTREGA
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  27 RESOLUCIÓN  OFICIO / LICENCIA                 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  28 TIPO DE RESOL / OBSERVACIONES               
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  29 FECHA AUTORIZACION               
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  30 FECHA  VENC             
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  31 TOTAL              
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  32 FECHA ENTREGA AL MODULO
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  33 VENTANILLA/MENSAJERO
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  34 FECHA DE PAGADO/ENTREGADO
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  35 NO. BIT
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  36 NO. LET
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  37 COPIAS MENSAJERIA 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  38 FECHA INGRESO ARCHIVO 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  39 LOCALIZACION ARCHIVO
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  40 OBSERVACIONES
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  41 DIAS REALES RESPUESTA
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  42 DEMORADO/ANTICIPADO/PUNTUAL
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));   //  43 OBSERVACIONES PERITOS
                #endregion

                #region Encabezado
                //  se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CONSECUTIVO", "Encabezado"));//1
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("SITUACCION", "Encabezado"));//2
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA RECEPCION", "Encabezado"));//3
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("INGRESO POR", "Encabezado"));//4
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DATOS DEL SOLICITANTE", "Encabezado"));//5
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DATOS DEL PROPIETARIO", "Encabezado"));//6
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("ASUNTO", "Encabezado"));//7
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("RUBRO", "Encabezado"));//8
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("GIRO", "Encabezado"));//9
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CUENTA PREDIAL", "Encabezado"));//10
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CALLE", "Encabezado"));//11
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("No. OFICIAL", "Encabezado"));//12
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("LOTE", "Encabezado"));//13
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("MANZANA", "Encabezado"));//14
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("COLONIA", "Encabezado"));//15
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CORREO ELECTRONICO", "Encabezado"));//16
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("M2", "Encabezado"));//17
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Ml", "Encabezado"));//18
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DRO ", "Encabezado"));//19
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("NOMBRE DRO ", "Encabezado"));//20
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("COLEGIO", "Encabezado"));//21
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PARA SU ATENCION (DIRECCION)", "Encabezado"));//22
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("ZONA", "Encabezado"));//23
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TURNADO A", "Encabezado"));//24
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("OBSERVACIONES", "Encabezado"));//25
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA PROMESA DE ENTREGA", "Encabezado"));//26
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("RESOLUCIÓN  OFICIO / LICENCIA", "Encabezado"));//27
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TIPO DE RESOL / OBSERVACIONES", "Encabezado"));//28
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA AUTORIZACION", "Encabezado"));//29
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA  VENC", "Encabezado"));//30
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL", "Encabezado"));//31
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA ENTREGA AL MODULO", "Encabezado"));//32
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("VENTANILLA/MENSAJERO", "Encabezado"));//33
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA DE PAGADO/ENTREGADO", "Encabezado"));//34
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("NO. BIT", "Encabezado"));//35
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("NO. LET", "Encabezado"));//36
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("COPIAS MENSAJERIA", "Encabezado"));//37
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA INGRESO ARCHIVO ", "Encabezado"));//38
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("LOCALIZACION ARCHIVO", "Encabezado"));//39
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("OBSERVACIONES", "Encabezado"));//40
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DIAS REALES RESPUESTA", "Encabezado"));//41
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DEMORADO/ANTICIPADO/PUNTUAL", "Encabezado"));//42
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("OBSERVACIONES PERITOS", "Encabezado"));//43
                #endregion

                int Cnt_For = 0;
                foreach (DataRow Registro in Dt_Modulo_General.Rows)
                {
                    Renglon = Hoja.Table.Rows.Add();
                    String Consecutivo_Tabla = Registro["CONSECUTIVO"].ToString();

                    if (Registro["CONSECUTIVO"].ToString() != "")//1
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Convert.ToDouble(Registro["CONSECUTIVO"].ToString()), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["SITUACION"].ToString(), "Contenido_Nombre"));//2

                    if (Registro["FECHA_RECEPCION"].ToString() != "")//3
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_RECEPCION"].ToString())), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));


                    if (Registro["USUARIO_CREO"].ToString() != "" && Registro["USUARIO_CREO"].ToString().Contains("@"))//4
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PAGINA WEB", "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("MODULO", "Contenido_Nombre"));

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["DATO_SOLICITANTE"].ToString(), "Contenido_Nombre"));//5
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["DATO_PROPIETARIO"].ToString(), "Contenido_Nombre"));//6
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["ASUNTO"].ToString(), "Contenido_Nombre"));//7
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["ASUNTO"].ToString(), "Contenido_Nombre"));//8
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//9
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["CUENTA_PREDIAL"].ToString(), "Contenido_Nombre"));//10
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["CALLE"].ToString(), "Contenido_Nombre"));//11
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["NUMERO"].ToString(), "Contenido_Nombre"));//12
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["LOTE"].ToString(), "Contenido_Nombre"));//13
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["MANZANA"].ToString(), "Contenido_Nombre"));//14
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["COLONIA"].ToString(), "Contenido_Nombre"));//15
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["CORREO_ELECTRONICO"].ToString(), "Contenido_Nombre"));//16
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["VALOR"].ToString(), "Contenido_Nombre"));//17 M2
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//18 Ml
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["DRO"].ToString(), "Contenido_Nombre"));//19
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["NOMBRE_DRO"].ToString(), "Contenido_Nombre"));//20
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["COLEGIO"].ToString(), "Contenido_Nombre"));//21
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["DEPENDENCIA"].ToString(), "Contenido_Nombre"));//22
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["ZONA"].ToString(), "Contenido_Nombre"));//23
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["NOMBRE_EMPLEADO"].ToString(), "Contenido_Nombre"));//24
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//25

                    if (Registro["FECHA_PROMESA"].ToString() != "")//26
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_PROMESA"].ToString())), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["FOLIO"].ToString(), "Contenido_Nombre"));//27
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//28


                    if (Registro["FECHA_AUTORIZACION"].ToString() != "")//29
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_AUTORIZACION"].ToString())), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    if (Registro["FECHA_VENCIMIENTO"].ToString() != "")//30
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_VENCIMIENTO"].ToString())), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    if (Registro["TOTAL"].ToString() != "")//31
                    {
                        Double Costo = Convert.ToDouble(Registro["TOTAL"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Costo, "Contenido_Numerico"));
                        Suma_Costo += Costo;
                    }
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    if (Registro["FECHA_INGRESO_MODULO"].ToString() != "")//32
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_INGRESO_MODULO"].ToString())), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//33

                    if (Registro["FECHA_PAGO"].ToString() != "")//34
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_PAGO"].ToString())), "Contenido_Nombre"));
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//35
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//36
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//37

                    if (Registro["FECHA_INGRESO_MODULO"].ToString() != "")//38
                    {
                        if (Registro["ESTATUS"].ToString() != "TERMINADO")
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro["FECHA_INGRESO_MODULO"].ToString())), "Contenido_Nombre"));

                        else
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));
                    }
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Registro["LOCALIZACION_ARCHIVO"].ToString(), "Contenido_Nombre"));//39

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//40

                    if (Registro["ESTATUS"].ToString() == "TERMINADO")//41
                    {
                        Dias = Convert.ToDateTime(Registro["FECHA_PROMESA"].ToString()) - Convert.ToDateTime(Registro["FECHA_INGRESO_MODULO"].ToString());
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Dias.Days, "Contenido_Nombre"));
                    }
                    else
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));


                    if (Dias.TotalDays > 0)//42
                    {
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PUNTUAL", "Contenido_Nombre"));
                    }
                    else if (Dias.TotalDays < 0)
                    {
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DEMORADO", "Contenido_Nombre"));
                    }
                    else
                    {
                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));
                    }

                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre"));//43


                    Dias = new TimeSpan();
                    Cnt_For++;
                }


                // PARA EL TOTAL DE LA SUMA DEL COSTO
                Renglon = Hoja.Table.Rows.Add();
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//1
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//2
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//3
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//4
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//5
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//6
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//7
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//8
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//9

                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//10
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//11
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//12
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//13
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//14
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//15
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//16
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//17
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//18
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//19

                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//20
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//21
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//22
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//23
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//24
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//25
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//26
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//27
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//28
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", "Contenido_Nombre_Sin_Bordes"));//29


                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL", "Contenido_Total"));//3
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("" + Suma_Costo, "Contenido_Total"));//31
                #endregion

                Libro.Save(Ruta_Archivo);
                Mostrar_Reporte(Nombre_Archivo);

            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());

        }
    }
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    /// USUARIO CREO:        Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:          18/Enero/2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar)
    {
        String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
        try
        {
            Mostrar_Mensaje_Error(false, "");

            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

            //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=300,height=100')", true);
        }
        catch (Exception Ex)
        {
           
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Solicitud_Tramite
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 24/Septiembre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Solicitud_Tramite()
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

                     if (Div_Contenedor_Msj_Error.Visible == false)
                     {
                         //  filtro para la depenencia
                         if (Chk_Filtro_Dependencia.Checked == true)
                             Reporte_Negocio.P_Dependencia_ID = Cmb_Filtro_Dependencia.SelectedValue;

                         if (Chk_Perito.Checked == true)
                             Reporte_Negocio.P_Perito = Cmb_Perito.SelectedValue;

                         if (Chk_Solicitante.Checked == true)
                             Reporte_Negocio.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();
                         
                         if (Chk_Folio.Checked == true)
                             Reporte_Negocio.P_Propietario = Txt_Folio.Text.Trim().ToUpper();

                         if (Div_Contenedor_Msj_Error.Visible == false)
                         {
                             DataTable Dt_Consulta = Reporte_Negocio.Consultar_Solicitud_Ordenamiento();
                             DataSet Data_Reporte = new DataSet();
                             Data_Reporte.Tables.Add(Dt_Consulta.Copy());
                             Generar_Reporte(Data_Reporte, Reporte_Tramites, "Rpt_Reportes_Tramites.rpt", "PDF", "Tramites");
                         }
                         else
                         {
                             Mostrar_Mensaje_Error(true, "Error");
                         }
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
             else
             {
                 Mostrar_Mensaje_Error(true, "Seleccion el tramites por el cual se realizara la busqueda");
             }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
            
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Reporte_Solicitud_Tramite
    ///DESCRIPCIÓN: Metodo que genera el reporte de pendientes de pago
    ///PARAMETROS:   Parametro_Reporte= El nombre del reporte
    ///CREO: Hugo   Enrique Ramirez Aguilera
    ///FECHA_CREO:  23/Octubre/2012 11:42 a.m.
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Reporte_Pagos(String Parametro_Reporte, String Formato)
    {
        Ds_Rpt_Ort_Pagos Ds_Reporte_Tramites = new Ds_Rpt_Ort_Pagos();
        DataTable Dt_Pagos = new DataTable("Dt_Ort_Estatus_Pagos");
        DataTable Dt_Parametro = new DataTable("Dt_Ort_Pagos_Parametros");
        ReportDocument Reporte = new ReportDocument();
        try
        {
            Verificar_Fecha();
            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                if (Parametro_Reporte == "PENDIENTES DE PAGO")
                    Dt_Pagos = Reporte_Negocio.Consultar_Pendientes_Pago_Ordenamiento();
                
                else
                    Dt_Pagos = Reporte_Negocio.Consultar_Pagados_Ordenamiento();
                
                
                Dt_Pagos.TableName = "Dt_Ort_Estatus_Pagos";

                Dt_Parametro.Columns.Add("TIPO", Type.GetType("System.String"));
                DataRow Fila = Dt_Parametro.NewRow();
                Fila["TIPO"] = Parametro_Reporte;
                Dt_Parametro.Rows.Add(Fila);

                DataSet Ds_Reporte = new DataSet();
                Ds_Reporte.Tables.Add(Dt_Pagos.Copy());
                Ds_Reporte.Tables.Add(Dt_Parametro.Copy());

                String Nombre_Archivo = "Reporte_Pagos_Realizados_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); 
                String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo

                Reporte.Load(Ruta_Archivo + "Rpt_Ort_Pagos.rpt");
                Reporte.SetDataSource(Ds_Reporte);

                DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

                if (Formato == "PDF") 
                    Nombre_Archivo += ".pdf";
                else
                    Nombre_Archivo += ".xls";

                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

                if (Formato == "PDF") 
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;

                else
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

                Reporte.Export(Opciones_Exportacion);

                if (Formato == "PDF")
                    Abrir_Ventana(Nombre_Archivo);

                else
                {
                    String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }
                
            }
            else
            {

            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
                    
                    if (Chk_Folio.Checked == true)
                        Reporte_Negocio.P_Propietario = Txt_Folio.Text.Trim().ToUpper();

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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
        try
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
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(false, "");

            //  filtro para modulo
            if (Chk_Modulo.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Modulo("PDF");
            }
            //  para obtener los del historico de cuenta predial
            else if (Chk_Cuenta_Predial.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                if (Validar_Cuenta_Predial())
                    Reporte_Historial_Cuenta_Predial();
            }
            //  filtro para las solicitudes pendientes de pago
            else if (Chk_Solicitudes_Pendientes_Pago.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Pagos("PENDIENTES DE PAGO", "PDF");
            }
            //  filtro para las solicitudes que ya han pagado
            else if (Chk_Solicitudes_Pagadas.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Pagos("PAGADOS", "PDF");
            }
            //  filtro de vigencia
            else if (Chk_Vigencia.Checked || Chk_Vigencia_Documento.Checked)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Vigencia("PDF");
            }
            // filtro para buscar en archivo
            else if (Chk_Reporte_Archivo.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Archivo("PDF");
            }
            else if (Chk_Reporte_Demorados.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Demorados("PDF");
            }
            // filtro para solicitudes entrantes
            else if (Chk_Reporte_Solicitud.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Solicitud_Tramite();
            }
            else
            {
                Mostrar_Mensaje_Error(false, "");
                Mostrar_Mensaje_Error(true, "Seleccione un tipo de filtro");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, Ex.Message.ToString());
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
            Mostrar_Mensaje_Error(false, "");

            //  filtro para modulo
            if (Chk_Modulo.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");
                Reporte_Modulo_Excel();
            }
            //  filtro pagos
            else if (Chk_Solicitudes_Pendientes_Pago.Checked == true)
            {
                 Reporte_Pagos("PENDIENTES DE PAGO", "");
            }
            else if (Chk_Solicitudes_Pagadas.Checked == true)
            {
                Reporte_Pagos("PAGADOS", "");
            }
            //  filtro por historico de la cuenta predial
            else if (Chk_Cuenta_Predial.Checked == true)
            {
                Mostrar_Mensaje_Error(false, "");

                if (Validar_Cuenta_Predial())
                    Reporte_Historial_Cuenta_Predial_Excel();

                else
                    Mostrar_Mensaje_Error(true, "Cuenta predial invalida");
            }
            //  filtro para la vigencia de los documentos, condicionales
            else if (Chk_Vigencia.Checked || Chk_Vigencia_Documento.Checked)
            {
                Reporte_Vigencia("");
            }
            else if (Chk_Reporte_Demorados.Checked == true)
            {
                Reporte_Demorados("EXCEL");
            }
            // filtro para buscar en archivo
            else if (Chk_Reporte_Archivo.Checked == true)
            {
                Reporte_Archivo("EXCEL");
            }
            // filtro para solicitudes entrantes
            else if (Chk_Reporte_Solicitud.Checked == true)
            {
                
                Reporte_Solicitud_Tramtie_Excel();
            }
            else
            {
                Mostrar_Mensaje_Error(true, "Seleccione un tipo de filtro");
            }
           
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
                    Mostrar_Mensaje_Error(true, Ex.Message.ToString());
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

        var Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Inspecores = new DataTable();


        Lbl_Mensaje_Error.Visible = false;
        IBtn_Imagen_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // consultar peritos
            //  Dt_Peritos = Obj_Peritos.Consultar_Inspectores();
            Dt_Inspecores = Negocio_Consultar_Solicitud.Consultar_Personal();

            // cargar datos en el combo
            Cmb_Perito.Items.Clear();
            Cmb_Perito.DataSource = Dt_Inspecores;
            Cmb_Perito.DataTextField = "Nombre_Usuario";
            Cmb_Perito.DataValueField = Cat_Empleados.Campo_Empleado_ID ;
            Cmb_Perito.DataBind();
            Cmb_Perito.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("< SELECCIONE >"), ""));
            Cmb_Perito.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(true, Ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
                //Btn_Buscar_Dependencia.Enabled = true;
            }
            else
            {
                Llenar_Combo_Dependencia();
                Cmb_Dependencias.Enabled = false;
                //Btn_Buscar_Dependencia.Enabled = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Reporte_Archivo_CheckedChanged
    ///DESCRIPCIÓN: limpia las cajas de texto
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Reporte_Archivo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Reporte_Archivo.Checked == true)
            {
                Chk_Reporte_Demorados.Checked = false;
                Chk_Reporte_Solicitud.Checked = false;
                Chk_Solicitudes_Pendientes_Pago.Checked = false;
                Chk_Solicitudes_Pagadas.Checked = false;
                Chk_Modulo.Checked = false;

                Habilitar_Filtros("Archivo", true);
            }
            else
            {
                Habilitar_Filtros("Todos", true);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Reporte_Demorados_CheckedChanged
    ///DESCRIPCIÓN: limpia las cajas de texto
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Reporte_Demorados_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Reporte_Demorados.Checked == true)
            {
                Chk_Reporte_Archivo.Checked = false;
                Chk_Reporte_Solicitud.Checked = false;
                Chk_Solicitudes_Pendientes_Pago.Checked = false;
                Chk_Solicitudes_Pagadas.Checked = false; 
                Chk_Modulo.Checked = false;

                Habilitar_Filtros("Demorados", true);
            }
            else
            {
                Habilitar_Filtros("Todo", true);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Reporte_Solicitud_CheckedChanged
    ///DESCRIPCIÓN: limpia las cajas de texto
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Reporte_Solicitud_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Reporte_Solicitud.Checked == true)
            {
                Chk_Reporte_Demorados.Checked = false;
                Chk_Reporte_Archivo.Checked = false;
                Chk_Solicitudes_Pendientes_Pago.Checked = false;
                Chk_Solicitudes_Pagadas.Checked = false;
                Chk_Modulo.Checked = false;

                Habilitar_Filtros("Solicitud", true);

            }
            else
            {
                Habilitar_Filtros("Todo", true);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Modulo_CheckedChanged
    ///DESCRIPCIÓN: limpia las cajas de texto
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 20/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Modulo_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Modulo.Checked == true)
            {
                Chk_Reporte_Demorados.Checked = false;
                Chk_Reporte_Archivo.Checked = false;
                Chk_Reporte_Solicitud.Checked = false;
                Chk_Solicitudes_Pendientes_Pago.Checked = false;
                Chk_Solicitudes_Pagadas.Checked = false;
            }
            else
            {

            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Solicitudes_Pendientes_Pago_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus
    ///PARAMETROS:  
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 23/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Solicitudes_Pendientes_Pago_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Solicitudes_Pendientes_Pago.Checked == true)
            {
                Chk_Solicitudes_Pagadas.Checked = false; 
                Chk_Reporte_Demorados.Checked = false;
                Chk_Reporte_Archivo.Checked = false;
                Chk_Reporte_Solicitud.Checked = false;
                Chk_Modulo.Checked = false;
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Solicitudes_Pagadas_CheckedChanged
    ///DESCRIPCIÓN: evento del Chk estatus
    ///PARAMETROS:  
    ///CREO: Hugo Enrique Ramirez Aguilera
    ///FECHA_CREO: 23/Octubre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Solicitudes_Pagadas_CheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (Chk_Solicitudes_Pagadas.Checked == true)
            {
                Chk_Solicitudes_Pendientes_Pago.Checked = false;
                Chk_Reporte_Demorados.Checked = false;
                Chk_Reporte_Archivo.Checked = false;
                Chk_Reporte_Solicitud.Checked = false;
                Chk_Modulo.Checked = false;
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
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
            }
            else
            {
                Txt_Cuenta_Predial.Enabled = false;
                Txt_Cuenta_Predial.Text = "";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    
    #endregion

    #region Grid
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
    #endregion 
}
