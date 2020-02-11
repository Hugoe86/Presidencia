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
using Presidencia.Recibos_Empleados.Negocio;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Constantes;
using Presidencia.Generacion_Poliza_Nomina.Negocio;
using Presidencia.Polizas.Negocios;
using Presidencia.Sessiones;
using Presidencia.Cuentas_Contables.Negocio;
using Presidencia.DateDiff;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;

public partial class paginas_Nomina_Frm_Ope_Nom_Generacion_Poliza : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();
            }

            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Métodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///
    ///DESCRIPCIÓN: Habilita la configuración inicial de los controles
    ///             página de generación de la nómina.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Consultar_Calendario_Nominas();//Se consultan los calendarios de nómina que existen actualmente en el sistema.
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : limpia los constroles de la página para realizar la siguiente operación.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 1/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodo.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina de generacion de la nómina. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos_Negocio = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;//Variable que almacenará la fecha actual.
        DateTime Fecha_Inicio = new DateTime();//Variable que almacenará la fecha de inicio de la catorcena.
        DateTime Fecha_Fin = new DateTime();//Variable que almacenará la fecha final de la catorcena.
        Fecha_Actual = Fecha_Actual.AddDays(-14);//Obtenemos la fecha inicial dela catorcena anterior.

        Prestamos_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();//Obtenemos la nomina seleccionada.

        foreach (ListItem Elemento in Combo.Items)
        {
            if (Es_Numerico(Elemento.Text.Trim()))
            {
                Prestamos_Negocio.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos_Negocio.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                    }
                }
            }
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();//Variable que almacenara los calendarios de nóminas.
        DataRow Renglon_Dt_Clon = null;//Variable que almacenará un renglón del calendario de la nómina.

        //Creamos las columnas.
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
        {
            Renglon_Dt_Clon = Dt_Nominas.NewRow();
            Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
            Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
            Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Es_Numerico
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numerico(String Cadena)
    {
        Boolean Resultado = true;//Almacena el resultado true si es numerica o false si no lo es.
        Char[] Array = Cadena.ToCharArray();//Obtenemos un arreglo de carácteres a partir de la cadena que se recibio como parámetro el método.

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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: 
    /// DESCRIPCION : 
    /// PARÁMETROS: 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Año de la Nómina <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Periodo de la Nómina  <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();

            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
                    Cmb_Periodo.DataBind();
                    Cmb_Periodo.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodo.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodo);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Calendario_Nominas()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
        try
        {
            Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();
            if (Dt_Calendario_Nominas != null)
            {
                if (Dt_Calendario_Nominas.Rows.Count > 0)
                {
                    Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                    Cmb_Calendario_Nomina.DataSource = Dt_Calendario_Nominas;
                    Cmb_Calendario_Nomina.DataTextField = "Nomina";
                    Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                    Cmb_Calendario_Nomina.DataBind();
                    Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Calendario_Nomina.SelectedIndex = -1;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron nominas vigentes";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    protected void Btn_Generar_Poliza_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Percepciones_Deducciones = null;//Variable que almacena un listado de empleados.
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.
        DataTable Dt_Detalles_Nomina = null;
        DataTable Dt_Poliza = null;

        try
        {
            if (Validar_Datos())
            {
                Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                var Fechas = from fecha in Dt_Detalles_Nomina.AsEnumerable()
                             select new
                             {
                                 Fecha_Inicio = fecha.Field<DateTime>(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio),
                                 Fecha_Fin = fecha.Field<DateTime>(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin)
                             };


                foreach (var Periodo in Fechas)
                {
                    Dt_Poliza = Cls_Ayudante_Poliza_Nomina.Generar_Poliza(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text, DateTime.Now.Year.ToString(),
                        Periodo.Fecha_Inicio, Periodo.Fecha_Fin);
                }

                //Obtenemos el libro.
                Libro = Cls_Ayudante_Crear_Excel.Generar_Excel_Poliza(Dt_Poliza);
                //Mandamos a imprimir el reporte en excel.
                Mostrar_Excel(Libro, "");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Nomina_Seleccionada = 0;//Variable que almacena la nómina seleccionada del combo.

        try
        {
            //Obtenemos elemento seleccionado del combo.
            Nomina_Seleccionada = Cmb_Calendario_Nomina.SelectedIndex;

            if (Nomina_Seleccionada > 0)
            {
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
            }
            else
            {
                Cmb_Periodo.DataSource = new DataTable();
                Cmb_Periodo.DataBind();
                Configuracion_Inicial();
                Limpiar_Controles();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #endregion
}
