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
using CrystalDecisions.Web;
using CrystalDecisions.ReportSource;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Reflection;
using Presidencia.Nomina_Reporte_Retardos_Faltas.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;
using Presidencia.Incidencias_Checadas.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Calendario_Reloj_Checador.Negocio;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;
using Presidencia.Prestamos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Generar_Faltas_Retardos_Empleados.Negocio;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Rpt_Nom_Retardos_Faltas : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.

                Limpiar_Controles();//Limpia los controles de la forma
                Consultar_Unidades_Responsables();
                Consultar_Tipos_Nominas();
                Consultar_Calendarios_Nomina();
                Habilitar_Controles(1);
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

    #region (Metodos)
    #region Validacion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///CREO       : Juan alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fechas
    /// DESCRIPCION : Validar el rango de fechas introducidas por el usuario
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Fechas()
    {
        Boolean Estatus_Final = false;
        Boolean Estatus_Fecha_Inicial = false;
        Boolean Estatus_Fecha_final = false;
        DateTime Fecha_Inicial = new DateTime();
        DateTime Fecha_Final = new DateTime();
        String Fecha = "";
        String Fecha1 = "";
        String Fecha2 = "";
        String Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        try
        {
            Fecha_Inicial = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text.Trim());
            Fecha_Final = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text.Trim());

            if (Fecha_Inicial.CompareTo(Fecha_Final) == 1)
            {
                Estatus_Final = true;
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La fecha final debe ser mayor que la inicial.<br>";
            }

            return Estatus_Final;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar las Fechas . Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 14/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Reporte()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario: <br>";
        Lbl_Mensaje_Error.Visible = true;
        Img_Error.Visible = true;

        if (Cmb_Tipo_Reporte.SelectedValue != "REPORTE PERSONAL QUE CHECA")
        {
            if (Txt_Fecha_Inicial.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha inicial.<br>";
                Datos_Validos = false;
            }
            else if (Txt_Fecha_Inicial.Text.Length != 8)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud de la fecha inicial debe de ser de 8 caracteres.<br>";
                Datos_Validos = false;
            }

            if (Txt_Fecha_Final.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha final.<br>";
                Datos_Validos = false;
            }
            else if (Txt_Fecha_Final.Text.Length != 8)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud de la fecha final debe de ser de 8 caracteres.<br>";
                Datos_Validos = false;
            }

            if ((Txt_Fecha_Inicial.Text != "") && (Txt_Fecha_Final.Text != ""))
            {
                if (Validar_Fechas())
                {
                    Datos_Validos = false;
                }
            }
        }

        ////  para el id del empleado
        //if (Txt_No_Empleado.Text != "")
        //{
        //    if (Txt_No_Empleado.Text.Length != 6)
        //    {
        //        Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud del empleado id debe de ser de 10 caracteres.<br>";
        //        Datos_Validos = false;
        //    }
        //}
        
        

        return Datos_Validos;
    }
    #endregion
    #region (Calendario Nomina)
    
    #endregion
    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : 1.- Operacion: Indica la operación que se desea realizar por parte del usuario
    ///               si es una alta, modificacion
    ///                           
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 07/Noviembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(Int32 Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = true;
            switch (Operacion)
            {
                case 1:
                    //RETARDOS Y FALTAS RELOJ CHECADOR
                    Limpiar_Controles();
                    Txt_No_Empleado.Enabled = Habilitado;
                    Txt_Nombre_Empleado.Enabled = Habilitado;
                    Btn_Buscar_Empleado.Enabled = Habilitado;
                    Cmb_Nombre_Empleado.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Txt_Fecha_Inicial.Enabled = Habilitado;
                    Txt_Fecha_Final.Enabled = Habilitado;
                    Cmb_Checa.Enabled = !Habilitado;
                    Cmb_Calendario_Nomina.Enabled = Habilitado;
                    Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
                    break;

                case 2:
                    //FALTAS RELOJ CHECADOR
                    Limpiar_Controles();
                    Txt_No_Empleado.Enabled = Habilitado;
                    Txt_Nombre_Empleado.Enabled = Habilitado;
                    Btn_Buscar_Empleado.Enabled = Habilitado;
                    Cmb_Nombre_Empleado.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Txt_Fecha_Inicial.Enabled = Habilitado;
                    Txt_Fecha_Final.Enabled = Habilitado;
                    Cmb_Checa.Enabled = !Habilitado;
                    Cmb_Calendario_Nomina.Enabled = Habilitado;
                    Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
                    break;

                case 3:
                    //SALIDAS ANTICIPADAS RELOJ CHECADOR
                    Limpiar_Controles();
                    Txt_No_Empleado.Enabled = Habilitado;
                    Txt_Nombre_Empleado.Enabled = Habilitado;
                    Btn_Buscar_Empleado.Enabled = Habilitado;
                    Cmb_Nombre_Empleado.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.Enabled = !Habilitado;
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Cmb_Checa.Enabled = !Habilitado;
                    Cmb_Calendario_Nomina.Enabled = Habilitado;
                    Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
                    Txt_Fecha_Inicial.Enabled = Habilitado;
                    Txt_Fecha_Final.Enabled = Habilitado;
                    break;

                case 4:
                    //REPORTE HISTORICO RELOJ CHECADOR
                    Limpiar_Controles();
                    Txt_No_Empleado.Enabled = Habilitado;
                    Txt_Nombre_Empleado.Enabled = Habilitado;
                    Btn_Buscar_Empleado.Enabled = Habilitado;
                    Cmb_Nombre_Empleado.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.Enabled = !Habilitado;
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Cmb_Checa.Enabled = !Habilitado;
                    Cmb_Calendario_Nomina.Enabled = Habilitado;
                    Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
                    Txt_Fecha_Inicial.Enabled = Habilitado;
                    Txt_Fecha_Final.Enabled = Habilitado;
                    break;

                case 5:
                    //REPORTE PERSONAL QUE CHECA
                    Limpiar_Controles();
                    Txt_No_Empleado.Enabled = Habilitado;
                    Txt_Nombre_Empleado.Enabled = Habilitado;
                    Btn_Buscar_Empleado.Enabled = Habilitado;
                    Cmb_Nombre_Empleado.Enabled = Habilitado;
                    Cmb_Unidad_Responsable.Enabled = !Habilitado;
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Cmb_Checa.Enabled = Habilitado;
                    Cmb_Calendario_Nomina.Enabled = !Habilitado;
                    Cmb_Periodos_Catorcenales_Nomina.Enabled = !Habilitado;
                    Txt_Fecha_Inicial.Enabled = !Habilitado;
                    Txt_Fecha_Final.Enabled = !Habilitado;
                    break;
            }
            
        }

        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 22/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";

            if (Cmb_Nombre_Empleado.SelectedIndex > 0)
                Cmb_Nombre_Empleado.SelectedIndex = 0;

            if (Cmb_Unidad_Responsable.SelectedIndex > 0) 
                Cmb_Unidad_Responsable.SelectedIndex = 0;

            Cmb_Checa.SelectedIndex = 0;

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0) 
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = 0;

            if (Cmb_Calendario_Nomina.SelectedIndex > 0) 
                Cmb_Calendario_Nomina.SelectedIndex = 0;

            Txt_Fecha_Inicial.Text = "";
            Txt_Fecha_Final.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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

    #region Consultas
    /// *************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuantran dadas de alta 
    ///              actualmente en sistema.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 10:52 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacena la lista de tipos de nominas. 
        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();//Consulta los tipos de nominas.
            //Cargar_Combos(Cmb_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
                //Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Cargar_Combos
    /// 
    /// DESCRIPCIÓN: Carga cualquier ctlr DropDownList que se le pase como parámetro.
    ///              
    /// PARÁMETROS: Combo.- Ctlr que se va a cargar.
    ///             Dt_Datos.- Informacion que se cargara en el combo.
    ///             Text.- Texto que será la parte visible de la lista de tipos de nómina.
    ///             Value.- Valor que será el que almacenará el elemnto seleccionado.
    ///             Index.- Indice el cuál será el que se mostrara inicialmente. 
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Cargar_Combos(DropDownList Combo, DataTable Dt_Datos, String Text, String Value, Int32 Index)
    {
        try
        {
            Combo.DataSource = Dt_Datos;
            Combo.DataTextField = Text;
            Combo.DataValueField = Value;
            Combo.DataBind();
            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Combo.SelectedIndex = Index;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Ctlr de Tipo DropDownList. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Unidades_Responsables
    /// 
    /// DESCRIPCIÓN: Consulta las Unidades responsables que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplicá.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que almacena una lista de las unidades resposables en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();//Consulta las unidades responsables registradas en  sistema.
            Cargar_Combos(Cmb_Unidad_Responsable, Dt_Unidades_Responsables, "CLAVE_NOMBRE",
                Cat_Dependencias.Campo_Dependencia_ID, 0);//Se carga el control que almacena las unidades responsables.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Estado_Salida_Anticipada
    /// DESCRIPCION : Consulta los salidas anticipadas que tenga en el periodo a reportar
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21/Febrero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Estado_Salida_Anticipada(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador Ds_Reporte = new Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador();
        DataTable Dt_Faltas = new DataTable();
        DataTable Dt_Informacion_Empleado = new DataTable();
        DataTable Dt_Inasistencias_Reloj = new DataTable();
        DataTable Dt_Horario_Especial = new DataTable();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataRow Dt_Row;
        String Fecha = "";
        String Fecha2 = "";
        String Empleado_ID = "";
        TimeSpan Extraer;
        String Auxiliar = "";
        Double Minutos = 0;
        Double PSM = 0.0;
        Boolean Estatus = false;
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Salidas_Anticipadas_Reloj_Checador_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        String Hora_Salida = "";
        String Hora_Falta = "";
        DateTime Dtt_Falta ;
        DateTime Dtt_Salida ;

        Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio();

        try
        {
            //  para obtener el psm
            Cls_Cat_Nom_Parametros_Negocio INF_NOMINA = null;

            INF_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();
            if (INF_NOMINA is Cls_Cat_Nom_Parametros_Negocio)
                PSM = Convert.ToDouble(INF_NOMINA.P_ISSEG_Porcentaje_Prevision_Social_Multiple);

            DataTable Dt_Auxiliar = new DataTable();

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Dt_Periodo.Columns.Add("FECHA_INICIO", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Periodo.Columns.Add("TIPO_REPORTE", typeof(System.String));
            Dt_Periodo.Columns.Add("ELABORO", typeof(System.String));
            Dt_Periodo.Columns.Add("TIPO_RETARDO", typeof(System.String));
            Dt_Periodo.TableName = "Dt_Periodo";

            Dt_Reporte.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("EMPLEADO_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("TURNO_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("TURNO_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("RELOJ_CHECADOR", typeof(System.String));
            Dt_Reporte.Columns.Add("RETARDOR", typeof(System.Double));
            Dt_Reporte.Columns.Add("FALTAS", typeof(System.Double));
            Dt_Reporte.Columns.Add("SALARIO_DIARIO", typeof(System.Double));
            Dt_Reporte.Columns.Add("PSM", typeof(System.Double));
            Dt_Reporte.TableName = "Dt_Reporte";

            // se aplica el formato a la fecha inicial
            Fecha = Txt_Fecha_Inicial.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);

            Rs_Faltas.P_Fecha_Inicial = Fecha; // se pasa la fecha inicial a la capa de negocio

            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIO"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);

            //  se aplica el formato a la fecha final
            Fecha = Txt_Fecha_Final.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);

            //  para la consulta de las faltas  
            Rs_Faltas.P_Fecha_Final = Fecha; // se pasa la fecha final a la capa de negocio

            //  se llena el renglon y se carga a la tabla de periodo
            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row["FECHA_FINAL"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            //Dt_Row["TIPO_NOMINA"] = Cmb_Tipo_Nomina.SelectedItem.Text;
            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            Dt_Row["TIPO_REPORTE"] = Cmb_Tipo_Reporte.SelectedValue;
            Dt_Row["TIPO_RETARDO"] = "ANTICIPADA";
            Dt_Periodo.Rows.Add(Dt_Row);

            //  para la consulta de la informacion del empleado por medio del numero del empleado
            if (Txt_No_Empleado.Text != "")
            {
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }

            //  se aplican los filtros para realizar otros reportes
            Rs_Faltas.P_Empleado_ID = Empleado_ID;

            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
            {
                Rs_Faltas.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            }

            ///::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Inicio = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);
            Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Termino = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            Dt_Faltas = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Faltas_Retardos();

            if (Dt_Faltas != null)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                    {
                        IEnumerable<DataRow> query_ur = from item_faltas in Dt_Faltas.AsEnumerable()
                                                        where item_faltas.Field<String>(Cat_Empleados.Campo_Dependencia_ID) == Cmb_Unidad_Responsable.SelectedValue
                                                        select item_faltas;
                        if (query_ur != null)
                        {
                            if (query_ur.Count() > 0)
                                Dt_Faltas = query_ur.CopyToDataTable();
                            else
                                Dt_Faltas = null;
                        }
                    }
                }
            }

            if (Dt_Faltas != null)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(Empleado_ID))
                    {
                        IEnumerable<DataRow> query = from item_faltas in Dt_Faltas.AsEnumerable()
                                                     where item_faltas.Field<String>(Cat_Empleados.Campo_Empleado_ID) == Empleado_ID
                                                     select item_faltas;
                        if (query != null)
                        {
                            if (query.Count() > 0)
                                Dt_Faltas = query.CopyToDataTable();
                            else
                                Dt_Faltas = null;
                        }
                    }
                }
            }
            ///::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            //  se realiza la consulta
            //Dt_Faltas = Rs_Faltas.Consultar_Faltas_Reporte();

            //  se llena la tabla con la informacion del reporte
            if (Dt_Faltas is DataTable)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Faltas in Dt_Faltas.Rows)
                    {
                        if (Dt_Row_Faltas is DataRow)
                        {
                            //  para la consulta de la informacion del empleado
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Dt_Informacion_Empleado = Rs_Faltas.Consultar_Informacion_Empleado();

                            //  para la consulta de horario especial
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Rs_Faltas.P_Fecha = String.Format("{0:dd/MM/yyyy}", Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha]);
                            Dt_Horario_Especial = Rs_Faltas.Consultar_Informacion_Horario_Empleado();

                            //  para la consulta de la informacion del horario de la falta y el numero de reloj checador
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Rs_Faltas.P_Fecha = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                            Dt_Inasistencias_Reloj = Rs_Faltas.Consultar_Informacion_Asistencia();

                            if (Dt_Informacion_Empleado is DataTable)
                            {
                                if (Dt_Informacion_Empleado.Rows.Count > 0)
                                {
                                    //  para las salidas anticipadas
                                    if (Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString() == "RETARDO")
                                    {
                                        //  para el horario especial o el horario normal del empleado
                                        if (Dt_Horario_Especial is DataTable)
                                        {
                                            if (Dt_Horario_Especial.Rows.Count > 0)
                                                Hora_Salida = Dt_Horario_Especial.Rows[0][Ope_Nom_Horarios_Empleados.Campo_Fecha_Termino].ToString();
                                          
                                            else
                                                Hora_Salida = Dt_Informacion_Empleado.Rows[0][Cat_Turnos.Campo_Hora_Salida].ToString();
                                        }
                                        
                                        Fecha = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                                        
                                        if(Fecha.Length>10)
                                            Fecha = Fecha.Substring(0, 10);

                                        //  para el horario de la inasistencia y el numero del reloj checador
                                        if (Dt_Inasistencias_Reloj is DataTable)
                                        {
                                            if (Dt_Inasistencias_Reloj.Rows.Count > 0)
                                            {
                                                foreach (DataRow Registro in Dt_Inasistencias_Reloj.Rows)
                                                {
                                                    if (Registro is DataRow)
                                                    {
                                                        Fecha2 = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();
                                                        if (Fecha2.Length > 10)
                                                            Fecha2 = Fecha2.Substring(0, 10);

                                                        if (Fecha == Fecha2)
                                                        {
                                                            if (!String.IsNullOrEmpty(Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString()))
                                                                Hora_Falta =   Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (Hora_Falta.Length > 10)
                                    {
                                        //  par sacar la fecha y extraer los minutos
                                        Auxiliar = Hora_Falta.Substring(0, 10);//   se saca el dia de la falta
                                        Auxiliar += " " + Hora_Salida.Substring(11, 13);//  se saca la hora en que sale el empleado
                                        Dtt_Falta = Convert.ToDateTime(Hora_Falta);
                                        Dtt_Salida = Convert.ToDateTime(Auxiliar);
                                        //  se extrae la diferencia de minutos de la salida
                                        Extraer = Dtt_Salida.Subtract(Dtt_Falta);
                                        //  para sacar los minutos de la salida anticipada
                                        if (Extraer.Minutes > 0 || Extraer.Hours > 0 || Extraer.Seconds > 0)
                                        {
                                            Estatus = true;
                                            Auxiliar = Extraer.Hours.ToString();
                                            Minutos = Convert.ToDouble(Auxiliar) * 60; // para las horas
                                            Auxiliar = Extraer.Minutes.ToString();
                                            Minutos += Convert.ToDouble(Auxiliar); // para los minutos
                                            Auxiliar = Extraer.Seconds.ToString();
                                            Minutos += Convert.ToDouble(Auxiliar) / 60; // para los segundos
                                        }
                                    }

                                    //  para llenar la tabla que se reportara
                                    if ((Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString() == "RETARDO" && Estatus == true))
                                    {
                                        Dt_Row = Dt_Reporte.NewRow();
                                        Dt_Row["DEPENDENCIA_ID"] = Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Nombre].ToString() ;

                                        Dt_Row["EMPLEADO_ID"] = Dt_Informacion_Empleado.Rows[0]["NOMBRE_EMPLEADO"].ToString() ;

                                        Dt_Row["NOMBRE_EMPLEADO"] = Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                                        Dt_Row["NOMBRE_DEPENDENCIA"] = Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Clave].ToString());


                                        Dt_Row["TIPO_NOMINA"] = "" + Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()) + " " +
                                                                Dt_Informacion_Empleado.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();

                                        //  para el horario del empleado
                                        if (Dt_Horario_Especial is DataTable)
                                        {
                                            if (Dt_Horario_Especial.Rows.Count > 0)
                                            {
                                                Dt_Row["TURNO_1"] = Dt_Horario_Especial.Rows[0][Ope_Nom_Horarios_Empleados.Campo_Fecha_Inicio].ToString();
                                                Dt_Row["TURNO_2"] = Dt_Horario_Especial.Rows[0][Ope_Nom_Horarios_Empleados.Campo_Fecha_Termino].ToString();
                                            }
                                            else
                                            {
                                                Dt_Row["TURNO_1"] = Dt_Informacion_Empleado.Rows[0][Cat_Turnos.Campo_Hora_Entrada].ToString();
                                                Dt_Row["TURNO_2"] = Dt_Informacion_Empleado.Rows[0][Cat_Turnos.Campo_Hora_Salida].ToString();
                                            }
                                        }


                                        //  para la fecha de la inasistencia
                                        Dt_Row["FECHA"] = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                                        Fecha = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                                        if (Fecha.Length > 10)
                                            Fecha = Fecha.Substring(0, 10);

                                        //  para el horario de la inasistencia y el numero del reloj checador
                                        if (Dt_Inasistencias_Reloj is DataTable)
                                        {
                                            if (Dt_Inasistencias_Reloj.Rows.Count > 0)
                                            {
                                                foreach (DataRow Registro in Dt_Inasistencias_Reloj.Rows)
                                                {
                                                    if (Registro is DataRow)
                                                    {
                                                        Fecha2 = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();
                                                        
                                                        if (Fecha2.Length > 10)
                                                            Fecha2 = Fecha2.Substring(0, 10);

                                                        if (Fecha == Fecha2)
                                                        {
                                                            //if (!String.IsNullOrEmpty(Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString()))
                                                            //    Dt_Row["HORARIO_FALTA_1"] = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();

                                                            if (!String.IsNullOrEmpty(Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString()))
                                                                Dt_Row["HORARIO_FALTA_2"] = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString();

                                                            if (!String.IsNullOrEmpty(Registro[Cat_Nom_Reloj_Checador.Campo_Clave].ToString()))
                                                                Dt_Row["RELOJ_CHECADOR"] = Registro[Cat_Nom_Reloj_Checador.Campo_Clave].ToString();
                                                            break;
                                                        }
                                                    }
                                                }
                                            }
                                        }

                                        //  para saber si es retardo 
                                        if (Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Retardo].ToString() == "SI")
                                        {
                                            if (Estatus == true)
                                            {
                                                Dt_Row["RETARDOR"] = Minutos;
                                                Dt_Row["FALTAS"] = 0.0;
                                            }

                                           
                                        }
                                        
                                        //  para el salario
                                        if (!String.IsNullOrEmpty(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString()))
                                        {
                                            Dt_Row["SALARIO_DIARIO"] = Convert.ToDouble(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString());
                                        }
                                        else
                                        {
                                            Dt_Row["SALARIO_DIARIO"] = 0.0;
                                        }
                                        Dt_Row["PSM"] = PSM;
                                        //  para ingresar la informacion al DataTable
                                        Dt_Reporte.Rows.Add(Dt_Row);
                                    }
                                    Estatus = false;
                                }
                            }
                        }
                    }
                }
            }

            //  se llena el dataset
            Dt_Periodo.TableName = "Dt_Periodo";
            Dt_Reporte.TableName = "Dt_Reporte";
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Retardos_Faltas_Reloj_Checador.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
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
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Estado
    /// DESCRIPCION : Consulta los retardos y faltas del periodo a reportar
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21/Febrero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Estado_Retardo_Faltas(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador Ds_Reporte = new Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador();
        DataTable Dt_Faltas = new DataTable();
        DataTable Dt_Informacion_Empleado = new DataTable();
        DataTable Dt_Inasistencias_Reloj = new DataTable();
        DataTable Dt_Horario_Especial = new DataTable();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataRow Dt_Row;
        String Fecha = "";
        String Fecha2 = "";
        String Empleado_ID = "";
        Double PSM = 0.0;
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Retardos_Faltas_Reloj_Checador_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento

        Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio();

        try
        {
            //  para obtener el psm
            Cls_Cat_Nom_Parametros_Negocio INF_NOMINA = null;

            INF_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();
            if (INF_NOMINA is Cls_Cat_Nom_Parametros_Negocio)
                PSM = Convert.ToDouble(INF_NOMINA.P_ISSEG_Porcentaje_Prevision_Social_Multiple);

            DataTable Dt_Auxiliar = new DataTable();

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Dt_Periodo.Columns.Add("FECHA_INICIO", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Periodo.Columns.Add("ELABORO", typeof(System.String));
            Dt_Periodo.Columns.Add("TIPO_REPORTE", typeof(System.String));
            Dt_Periodo.Columns.Add("TIPO_RETARDO", typeof(System.String));
            Dt_Periodo.TableName = "Dt_Periodo";

            Dt_Reporte.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("EMPLEADO_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("TURNO_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("TURNO_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("RELOJ_CHECADOR", typeof(System.String));
            Dt_Reporte.Columns.Add("RETARDOR", typeof(System.Double));
            Dt_Reporte.Columns.Add("FALTAS", typeof(System.Double));
            Dt_Reporte.Columns.Add("SALARIO_DIARIO", typeof(System.Double));
            Dt_Reporte.Columns.Add("PSM", typeof(System.Double));
            Dt_Reporte.TableName = "Dt_Reporte";

            // se aplica el formato a la fecha inicial
            Fecha = Txt_Fecha_Inicial.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);

            Rs_Faltas.P_Fecha_Inicial = Fecha; // se pasa la fecha inicial a la capa de negocio

            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIO"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);

            //  se aplica el formato a la fecha final
            Fecha = Txt_Fecha_Final.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);

            //  para la consulta de las faltas  
            Rs_Faltas.P_Fecha_Final = Fecha; // se pasa la fecha final a la capa de negocio

            //  se llena el renglon y se carga a la tabla
            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row["FECHA_FINAL"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            Dt_Row["TIPO_REPORTE"] = Cmb_Tipo_Reporte.SelectedValue;
            Dt_Row["TIPO_RETARDO"] = "RETARDO";
            Dt_Periodo.Rows.Add(Dt_Row);

            //  para la consulta de la informacion del empleado por medio de la cuenta del empleado
            if (Txt_No_Empleado.Text != "")
            {
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }
            //  se aplican los filtros para realizar otros reportes
            Rs_Faltas.P_Empleado_ID = Empleado_ID;

            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
            {
                Rs_Faltas.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            }


            ///::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Inicio = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);
            Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Termino = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            Dt_Faltas = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Faltas_Retardos();

            if (Dt_Faltas != null)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                    {
                        IEnumerable<DataRow> query_ur = from item_faltas in Dt_Faltas.AsEnumerable()
                                                        where item_faltas.Field<String>(Cat_Empleados.Campo_Dependencia_ID) == Cmb_Unidad_Responsable.SelectedValue
                                                        select item_faltas;
                        if (query_ur != null)
                        {
                            if (query_ur.Count() > 0)
                                Dt_Faltas = query_ur.CopyToDataTable();
                            else
                                Dt_Faltas = null;
                        }
                    }
                }
            }

            if (Dt_Faltas != null)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(Empleado_ID))
                    {
                        IEnumerable<DataRow> query = from item_faltas in Dt_Faltas.AsEnumerable()
                                                     where item_faltas.Field<String>(Cat_Empleados.Campo_Empleado_ID) == Empleado_ID
                                                     select item_faltas;
                        if (query != null)
                        {
                            if (query.Count() > 0)
                                Dt_Faltas = query.CopyToDataTable();
                            else
                                Dt_Faltas = null;
                        }
                    }
                }
            }
            ///::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::

            //  se realiza la consulta
            //Dt_Faltas = Rs_Faltas.Consultar_Faltas_Reporte();

            //  se llena la tabla con la informacion del reporte
            if (Dt_Faltas is DataTable)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Faltas in Dt_Faltas.Rows)
                    {
                        if (Dt_Row_Faltas is DataRow)
                        {
                            //  para la consulta de la informacion del empleado
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Dt_Informacion_Empleado = Rs_Faltas.Consultar_Informacion_Empleado();

                            //  para la consulta de horario especial
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Rs_Faltas.P_Fecha = String.Format("{0:dd/MM/yyyy}", Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha]);
                            Dt_Horario_Especial = Rs_Faltas.Consultar_Informacion_Horario_Empleado();

                            //  para la consulta de la informacion del horario de la falta y el numero de reloj checador
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Rs_Faltas.P_Fecha =  Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                            Dt_Inasistencias_Reloj = Rs_Faltas.Consultar_Informacion_Asistencia();

                            if (Dt_Informacion_Empleado is DataTable)
                            {
                                if (Dt_Informacion_Empleado.Rows.Count > 0)
                                {
                                    Dt_Row = Dt_Reporte.NewRow();
                                    Dt_Row["DEPENDENCIA_ID"] = Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();

                                    Dt_Row["EMPLEADO_ID"] = Dt_Informacion_Empleado.Rows[0]["NOMBRE_EMPLEADO"].ToString();

                                    Dt_Row["NOMBRE_EMPLEADO"] = Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                                    Dt_Row["NOMBRE_DEPENDENCIA"] = Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Clave].ToString());


                                    Dt_Row["TIPO_NOMINA"] = "" + Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()) + " " +
                                                            Dt_Informacion_Empleado.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();

                                    //  para el horario del empleado
                                    if (Dt_Horario_Especial is DataTable)
                                    {
                                        if (Dt_Horario_Especial.Rows.Count > 0)
                                        {
                                            Dt_Row["TURNO_1"] = Dt_Horario_Especial.Rows[0][Ope_Nom_Horarios_Empleados.Campo_Fecha_Inicio].ToString();
                                            Dt_Row["TURNO_2"] = Dt_Horario_Especial.Rows[0][Ope_Nom_Horarios_Empleados.Campo_Fecha_Termino].ToString();
                                        }
                                        else
                                        {
                                            Dt_Row["TURNO_1"] = Dt_Informacion_Empleado.Rows[0][Cat_Turnos.Campo_Hora_Entrada].ToString();
                                            Dt_Row["TURNO_2"] = Dt_Informacion_Empleado.Rows[0][Cat_Turnos.Campo_Hora_Salida].ToString();
                                        }
                                    }


                                    //  para la fecha de la inasistencia
                                    Dt_Row["FECHA"] = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                                    Fecha = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                                    Fecha = Fecha.Substring(0, 10);
                                    
                                    //  para el horario de la inasistencia y el numero del reloj checador
                                    if (Dt_Inasistencias_Reloj is DataTable)
                                    {
                                        if (Dt_Inasistencias_Reloj.Rows.Count > 0)
                                        {
                                            foreach (DataRow Registro in Dt_Inasistencias_Reloj.Rows)
                                            {
                                                if (Registro is DataRow)
                                                {
                                                    Fecha2 = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();
                                                    Fecha2 = Fecha2.Substring(0, 10);

                                                    if (Fecha == Fecha2)
                                                    {
                                                        if (!String.IsNullOrEmpty(Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString()))
                                                            Dt_Row["HORARIO_FALTA_1"] = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();

                                                        if (!String.IsNullOrEmpty(Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString()))
                                                            Dt_Row["HORARIO_FALTA_2"] = Registro[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString();

                                                        if (!String.IsNullOrEmpty(Registro[Cat_Nom_Reloj_Checador.Campo_Clave].ToString()))
                                                            Dt_Row["RELOJ_CHECADOR"] = Registro[Cat_Nom_Reloj_Checador.Campo_Clave].ToString();
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    //  para saber si es retardo 
                                    if (Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Retardo].ToString() == "SI")
                                    {
                                        Dt_Row["RETARDOR"] = Convert.ToDouble(Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Cantidad].ToString());
                                        Dt_Row["FALTAS"] = 0.0;
                                    }
                                    //  para saber si es falta
                                    else
                                    {
                                        Dt_Row["RETARDOR"] = 0.0;
                                        Dt_Row["FALTAS"] = 1.0;
                                    }

                                    //  para el salario
                                    if (!String.IsNullOrEmpty(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString()))
                                    {
                                        Dt_Row["SALARIO_DIARIO"] = Convert.ToDouble(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString());
                                    }
                                    else
                                    {
                                        Dt_Row["SALARIO_DIARIO"] = 0.0;
                                    }
                                    Dt_Row["PSM"] = PSM;

                                    //  para ingresar la informacion al DataTable
                                    Dt_Reporte.Rows.Add(Dt_Row);
                                }
                            }
                        }
                    }
                }
            }

            //  se llena el dataset
            Dt_Periodo.TableName = "Dt_Periodo";
            Dt_Reporte.TableName = "Dt_Reporte";
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
           
            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Retardos_Faltas_Reloj_Checador.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
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
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Personal_Checa
    /// DESCRIPCION : Consulta la informacion del personal que checa en el reloj checador
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Personal_Checa(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador Ds_Reporte = new Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador();
        DataTable Dt_Personal = new DataTable();
        DataTable Dt_Reporte = new DataTable();
        DataRow Dt_Row;
        String Formato_Salario = "";
        String Reloj_Checador = "";
        Double Salario = 0.0;
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Personal_Checa_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Dt_Reporte.Columns.Add("NO_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("PUESTO", typeof(System.String));
            Dt_Reporte.Columns.Add("FECHA_ALTA", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("RELOJ", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_COLUMNA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_TITULO", typeof(System.String));
            Dt_Reporte.Columns.Add("ELABORO", typeof(System.String));
            Dt_Reporte.Columns.Add("CLAVE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.TableName = "Dt_Personal_checa";

            //  se realiza la consulta
            if (Txt_No_Empleado.Text != "")
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
            
            Rs_Faltas.P_Reloj_checador = Cmb_Checa.SelectedValue;
            Dt_Personal = Rs_Faltas.Consultar_Personal_Checa();

            //  se llena la tabla con la informacion del reporte
            if (Dt_Personal is DataTable)
            {
                if (Dt_Personal.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Checan in Dt_Personal.Rows)
                    {
                        if (Dt_Row_Checan is DataRow)
                        {
                            Dt_Row = Dt_Reporte.NewRow();

                            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;

                            Dt_Row["NO_EMPLEADO"] = Dt_Row_Checan[Cat_Empleados.Campo_No_Empleado].ToString();

                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Checan[Cat_Empleados.Campo_Apellido_Paterno].ToString() + " " +
                                    Dt_Row_Checan[Cat_Empleados.Campo_Apellido_Materno].ToString() + " " +
                                    Dt_Row_Checan[Cat_Empleados.Campo_Nombre].ToString();

                            //  para saber si contiene algo el campo de la dependencia 
                            if (!String.IsNullOrEmpty(Dt_Row_Checan["Nombre_Dependencia"].ToString()))
                            {
                                //  para saber si pertenece a alguna dependencia
                                if (Dt_Row_Checan["Nombre_Dependencia"].ToString() != "")
                                    Dt_Row["DEPENDENCIA"] = Dt_Row_Checan["Nombre_Dependencia"].ToString();

                                //  si no pertenece se deja vacio
                                else
                                    Dt_Row["DEPENDENCIA"] = "";
                            }

                            if (!String.IsNullOrEmpty(Dt_Row_Checan[Cat_Dependencias.Campo_Clave].ToString()))
                            {
                                if (Dt_Row_Checan[Cat_Dependencias.Campo_Clave].ToString() != "")
                                    Dt_Row["CLAVE_DEPENDENCIA"] = Dt_Row_Checan[Cat_Dependencias.Campo_Clave].ToString();

                               //  si no pertenece se deja vacio
                                
                            }
                            else
                                Dt_Row["CLAVE_DEPENDENCIA"] = "";
                            //  para saber si contiene informacion sobre el puesto o el salario mensual 
                            if (Dt_Row_Checan["Nombre_Puesto"].ToString() != "" /*|| Dt_Row_Checan[Cat_Puestos.Campo_Salario_Mensual].ToString() != ""*/)
                            {
                                //if (!String.IsNullOrEmpty(Dt_Row_Checan[Cat_Puestos.Campo_Salario_Mensual].ToString()))
                                //    Salario = Convert.ToDouble(Dt_Row_Checan[Cat_Puestos.Campo_Salario_Mensual].ToString());

                                //Formato_Salario = String.Format("{0:n}", Salario);
                                Dt_Row["PUESTO"] = Dt_Row_Checan["Nombre_Puesto"].ToString();
                            }
                            //  si no se deja vacio la celda del puesto
                            else
                                Dt_Row["PUESTO"] = "";

                            //  para saber si pertenece el estatus del reloj checador
                            Reloj_Checador = Dt_Row_Checan[Cat_Empleados.Campo_Reloj_Checador].ToString();
                            
                            //  para saber el tipo de reporte al que pertenece
                            if (Reloj_Checador == "NO")
                            {
                                Dt_Row["FECHA_ALTA"] = Dt_Row_Checan[Cat_Empleados.Campo_Fecha_Inicio].ToString();
                                Dt_Row["TIPO_TITULO"] = "PERSONAL QUE NO CHECA";
                                Dt_Row["TIPO_COLUMNA"] = "FECHA ALTA";
                            }
                            else if (Reloj_Checador == "SI")
                            {
                                Dt_Row["RELOJ"] = Dt_Row_Checan["Clave_Reloj"].ToString();
                                Dt_Row["TIPO_TITULO"] = "PERSONAL QUE CHECA";
                                Dt_Row["TIPO_COLUMNA"] = "RELOJ";
                            }

                            //  para ingresar la informacion al DataTable
                            Dt_Reporte.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  para ordenar la tabla por nombre
            DataView Dv_Ordenar = new DataView(Dt_Reporte);
            DataTable Dt_Auxiliar = new DataTable();
            Dv_Ordenar.Sort = "DEPENDENCIA, CLAVE_DEPENDENCIA, PUESTO, NOMBRE_EMPLEADO";
            Dt_Reporte = Dv_Ordenar.ToTable();

            //  se llena el dataset
            Dt_Reporte.TableName = "Dt_Personal_checa";
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Personal_Checa_Reloj_Checador.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
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
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Estado_historico_Reloj
    /// DESCRIPCION : Consulta las faltas del periodo a reportar
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 20/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Estado_historico_Reloj(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador Ds_Reporte = new Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador();
        DataTable Dt_Historico = new DataTable();
        DataTable Dt_Informacion_Empleado = new DataTable();
        DataTable Dt_Inasistencias_Reloj = new DataTable();
        DataTable Dt_Horario_Especial = new DataTable();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataTable Dt_Auxiliar = new DataTable();
        DataRow Dt_Row;
        String Fecha = "";
        String Fecha_Inicio = "";
        String Fecha_Fin = "";
        String Empleado_ID = "";
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Historial_Reloj_Checador_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        Double PSM = 0.0;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  para obtener el psm
            Cls_Cat_Nom_Parametros_Negocio INF_NOMINA = null;

            INF_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();
            if (INF_NOMINA is Cls_Cat_Nom_Parametros_Negocio)
                PSM= Convert.ToDouble(INF_NOMINA.P_ISSEG_Porcentaje_Prevision_Social_Multiple);

           
            Dt_Periodo.Columns.Add("FECHA_INICIO", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Periodo.Columns.Add("ELABORO", typeof(System.String));
            Dt_Periodo.TableName = "Dt_Periodo";

            Dt_Reporte.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("EMPLEADO_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("TURNO_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("TURNO_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("RELOJ_CHECADOR", typeof(System.String));
            Dt_Reporte.Columns.Add("RETARDOR", typeof(System.Double));
            Dt_Reporte.Columns.Add("FALTAS", typeof(System.Double));
            Dt_Reporte.Columns.Add("SALARIO_DIARIO", typeof(System.Double));
            Dt_Reporte.Columns.Add("PSM", typeof(System.Double));
            Dt_Reporte.TableName = "Dt_Reporte";

            // se aplica el formato a la fecha inicial
            Fecha = Txt_Fecha_Inicial.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);
            Fecha_Inicio = Fecha;

            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIO"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);

            //  se aplica el formato a la fecha final
            Fecha = Txt_Fecha_Final.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);
            Fecha_Fin = Fecha;
           
            //  se llena el renglon y se carga a la tabla
            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row["FECHA_FINAL"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            //Dt_Row["TIPO_NOMINA"] = Cmb_Tipo_Nomina.SelectedItem.Text;
            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            //RETARDOS Y FALTAS DEL RELOJ CHECADOR
            Dt_Periodo.Rows.Add(Dt_Row);

            //  para la consulta de la informacion del empleado por medio de la cuenta del empleado
            if (Txt_No_Empleado.Text != "")
            {
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }

            //  se realiza la consulta
            if (Empleado_ID != "")
                Rs_Faltas.P_Empleado_ID = Empleado_ID;

            Rs_Faltas.P_Fecha_Inicial = Fecha_Inicio; // se pasa la fecha inicial a la capa de negocio
            Rs_Faltas.P_Fecha_Final = Fecha_Fin; // se pasa la fecha final a la capa de negocio
            Dt_Historico = Rs_Faltas.Consultar_Historico_Reloj();

            //  se llena la tabla con la informacion del reporte
            if (Dt_Historico is DataTable)
            {
                if (Dt_Historico.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Historico in Dt_Historico.Rows)
                    {
                        if (Dt_Row_Historico is DataRow)
                        {
                            //  para la consulta de la informacion del empleado
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Empleado_ID].ToString();
                            Dt_Informacion_Empleado = Rs_Faltas.Consultar_Informacion_Empleado();

                            if (Dt_Informacion_Empleado is DataTable)
                            {
                                if (Dt_Informacion_Empleado.Rows.Count > 0)
                                {
                                    Dt_Row = Dt_Reporte.NewRow();
                                    //  para el nombre de la dependencia
                                    Dt_Row["DEPENDENCIA_ID"] = Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                                    //  para el nombre del empleado
                                    Dt_Row["EMPLEADO_ID"] = Dt_Informacion_Empleado.Rows[0]["NOMBRE_EMPLEADO"].ToString();

                                    //  para las claves de la dependencia y el empleado
                                    Dt_Row["NOMBRE_EMPLEADO"] = Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                                    Dt_Row["NOMBRE_DEPENDENCIA"] = Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Clave].ToString());

                                    //  para el tipo de nomina + el nombre de la nomina
                                    Dt_Row["TIPO_NOMINA"] = "" + Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()) + " " +
                                                            Dt_Informacion_Empleado.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();

                                    //  para la fecha
                                    if (!String.IsNullOrEmpty(Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString()))
                                        Dt_Row["FECHA"] = Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();

                                    else if (!String.IsNullOrEmpty(Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString()))
                                        Dt_Row["FECHA"] = Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString();

                                    //  para la hora de entrada
                                    if (!String.IsNullOrEmpty(Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString()))
                                        Dt_Row["HORARIO_FALTA_1"] = Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Entrada].ToString();
                                    
                                    //  para la hora de salida
                                    if (!String.IsNullOrEmpty(Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString()))
                                        Dt_Row["HORARIO_FALTA_2"] = Dt_Row_Historico[Ope_Nom_Asistencias.Campo_Fecha_Hora_Salida].ToString();
                                    
                                    //  para la clave del reloj checador
                                    if (!String.IsNullOrEmpty(Dt_Row_Historico[Cat_Nom_Reloj_Checador.Campo_Clave].ToString()))
                                        Dt_Row["RELOJ_CHECADOR"] = Dt_Row_Historico[Cat_Nom_Reloj_Checador.Campo_Clave].ToString();

                                    ////  para ingresar la informacion al DataTable
                                    Dt_Reporte.Rows.Add(Dt_Row);
                                }
                            }
                        }
                    }
                }
            }

            //  se llena el dataset
            Dt_Periodo.TableName = "Dt_Periodo";
            Dt_Reporte.TableName = "Dt_Reporte";
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Historial_Reloj_Checador.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
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
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Estado_Faltas
    /// DESCRIPCION : Consulta las faltas del periodo a reportar
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 16/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Estado_Faltas(String Formato)
    {
        Cls_Rpt_Nom_Retardos_Faltas_Negocio Rs_Faltas = new Cls_Rpt_Nom_Retardos_Faltas_Negocio();
        Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador Ds_Reporte = new Ds_Rpt_Nom_Retardos_Faltas_Reloj_Checador();
        DataTable Dt_Faltas = new DataTable();
        DataTable Dt_Informacion_Empleado = new DataTable();
        DataTable Dt_Inasistencias_Reloj = new DataTable();
        DataTable Dt_Horario_Especial = new DataTable();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Periodo = new DataTable();
        DataRow Dt_Row;
        String Fecha = "";
        String Empleado_ID = "";
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Retardos_Faltas_Reloj_Checador_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        Double PSM = 0.0;

        Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio();

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  para obtener el psm
            Cls_Cat_Nom_Parametros_Negocio INF_NOMINA = null;

            INF_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();
            if (INF_NOMINA is Cls_Cat_Nom_Parametros_Negocio)
                PSM= Convert.ToDouble(INF_NOMINA.P_ISSEG_Porcentaje_Prevision_Social_Multiple);

            //  para la consulta de la informacion del empleado por medio de la cuenta del empleado
            if (Txt_No_Empleado.Text != "")
            {
                DataTable Dt_Auxiliar = new DataTable();
                Rs_Faltas.P_No_Empleado = Txt_No_Empleado.Text;
                Dt_Auxiliar = Rs_Faltas.Consultar_Informacion_Empleado();
                Rs_Faltas.P_No_Empleado = null;

                if (Dt_Auxiliar is DataTable)
                {
                    if (Dt_Auxiliar.Rows.Count > 0)
                    {
                        foreach (DataRow Dt_Row_Empleado in Dt_Auxiliar.Rows)
                        {
                            if (Dt_Row_Empleado is DataRow)
                            {
                                Empleado_ID = Dt_Row_Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }
            Dt_Periodo.Columns.Add("FECHA_INICIO", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("FECHA_FINAL", typeof(System.DateTime));
            Dt_Periodo.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Periodo.Columns.Add("ELABORO", typeof(System.String));
            Dt_Periodo.TableName = "Dt_Periodo";

            Dt_Reporte.Columns.Add("DEPENDENCIA_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("EMPLEADO_ID", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("TURNO_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("TURNO_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_1", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("HORARIO_FALTA_2", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("RELOJ_CHECADOR", typeof(System.String));
            Dt_Reporte.Columns.Add("RETARDOR", typeof(System.Double));
            Dt_Reporte.Columns.Add("FALTAS", typeof(System.Double));
            Dt_Reporte.Columns.Add("SALARIO_DIARIO", typeof(System.Double));
            Dt_Reporte.Columns.Add("PSM", typeof(System.Double));
            Dt_Reporte.TableName = "Dt_Reporte";

            // se aplica el formato a la fecha inicial
            Fecha = Txt_Fecha_Inicial.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);

            Rs_Faltas.P_Fecha_Inicial = Fecha; // se pasa la fecha inicial a la capa de negocio

            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row = Dt_Periodo.NewRow();
            Dt_Row["FECHA_INICIO"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);

            //  se aplica el formato a la fecha final
            Fecha = Txt_Fecha_Final.Text;
            Fecha = Fecha.Insert(2, "/");
            Fecha = Fecha.Insert(5, "/");
            Fecha = Fecha.Substring(0, 6) + Fecha.Substring(8, 2);

            //  para la consulta de las faltas  
            Rs_Faltas.P_Fecha_Final = Fecha; // se pasa la fecha final a la capa de negocio

            //  se llena el renglon y se carga a la tabla
            Fecha = Fecha.Substring(3, 2) + "/" + Fecha.Substring(0, 2) + "/" + Fecha.Substring(6, 2);
            Dt_Row["FECHA_FINAL"] = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            Dt_Periodo.Rows.Add(Dt_Row);

            //  se aplican los filtros para realizar otros reportes
            Rs_Faltas.P_Empleado_ID = Empleado_ID;

            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
            {
                Rs_Faltas.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;
            }

            ///::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Inicio = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text);
            Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Termino = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text);
            Dt_Faltas = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Faltas_Retardos();

            if (Dt_Faltas != null)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                    {
                        IEnumerable<DataRow> query_ur = from item_faltas in Dt_Faltas.AsEnumerable()
                                                        where item_faltas.Field<String>(Cat_Empleados.Campo_Dependencia_ID) == Cmb_Unidad_Responsable.SelectedValue
                                                        select item_faltas;
                        if (query_ur != null)
                        {
                            if (query_ur.Count() > 0)
                                Dt_Faltas = query_ur.CopyToDataTable();
                            else
                                Dt_Faltas = null;
                        }
                    }
                }
            }

            if (Dt_Faltas != null)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    if (!String.IsNullOrEmpty(Empleado_ID))
                    {
                        IEnumerable<DataRow> query = from item_faltas in Dt_Faltas.AsEnumerable()
                                                     where item_faltas.Field<String>(Cat_Empleados.Campo_Empleado_ID) == Empleado_ID
                                                     select item_faltas;
                        if (query != null)
                        {
                            if (query.Count() > 0)
                                Dt_Faltas = query.CopyToDataTable();
                            else
                                Dt_Faltas = null;
                        }
                    }
                }
            }
            ///::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::::
            //  se realiza la consulta
            //Dt_Faltas = Rs_Faltas.Consultar_Faltas_Reporte();

            //  se llena la tabla con la informacion del reporte
            if (Dt_Faltas is DataTable)
            {
                if (Dt_Faltas.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Faltas in Dt_Faltas.Rows)
                    {
                        if (Dt_Row_Faltas is DataRow)
                        {
                            //  para la consulta de la informacion del empleado
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Dt_Informacion_Empleado = Rs_Faltas.Consultar_Informacion_Empleado();

                            //  para la consulta de horario especial
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Rs_Faltas.P_Fecha = String.Format("{0:dd/MM/yyyy}", Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha]);
                            Dt_Horario_Especial = Rs_Faltas.Consultar_Informacion_Horario_Empleado();

                            //  para la consulta de la informacion del horario de la falta y el numero de reloj checador
                            Rs_Faltas.P_Empleado_ID = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Empleado_ID].ToString();
                            Rs_Faltas.P_Fecha = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();
                            Dt_Inasistencias_Reloj = Rs_Faltas.Consultar_Informacion_Asistencia();

                            if (Dt_Informacion_Empleado is DataTable)
                            {
                                if (Dt_Informacion_Empleado.Rows.Count > 0)
                                {
                                    if (Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString() == "INASISTENCIA" ||
                                        Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Tipo_Falta].ToString() == "JUSTIFICADA")
                                    {

                                        Dt_Row = Dt_Reporte.NewRow();
                                        Dt_Row["DEPENDENCIA_ID"] = Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Nombre].ToString() + " - " +
                                                                Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Clave].ToString());

                                        Dt_Row["EMPLEADO_ID"] =  Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();

                                        Dt_Row["NOMBRE_EMPLEADO"] = Dt_Informacion_Empleado.Rows[0]["NOMBRE_EMPLEADO"].ToString() ;

                                        Dt_Row["NOMBRE_DEPENDENCIA"] = Dt_Informacion_Empleado.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();


                                        Dt_Row["TIPO_NOMINA"] = "" + Convert.ToInt32(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()) + " " +
                                                                Dt_Informacion_Empleado.Rows[0][Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();

                                        //  para la fecha de la inasistencia
                                        Dt_Row["FECHA"] = Dt_Row_Faltas[Ope_Nom_Faltas_Empleado.Campo_Fecha].ToString();

                                        //  para la falta
                                        Dt_Row["RETARDOR"] = 0.0;
                                        Dt_Row["FALTAS"] = 1.0;


                                        //  para el salario
                                        if (!String.IsNullOrEmpty(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString()))
                                        {
                                            Dt_Row["SALARIO_DIARIO"] = Convert.ToDouble(Dt_Informacion_Empleado.Rows[0][Cat_Empleados.Campo_Salario_Diario].ToString());
                                        }
                                        else
                                        {
                                            Dt_Row["SALARIO_DIARIO"] = 0.0;
                                        }
                                        //  para ingresar la informacion al DataTable
                                        Dt_Row["PSM"] = PSM;
                                        Dt_Reporte.Rows.Add(Dt_Row);
                                    }
                                }
                            }
                        }
                    }
                }
            }

            //  se llena el dataset
            Dt_Periodo.TableName = "Dt_Periodo";
            Dt_Reporte.TableName = "Dt_Reporte";
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Periodo.Copy());
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Faltas_Reloj_Checador.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

            if (Formato == "PDF")
            {
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
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
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
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
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
    ///NOMBRE DE LA FUNCIÓN: Consulta_Fechas_Periodo_Nominal
    ///DESCRIPCIÓN: Consulta las fecchas del periodo nominal que fue seleccionado por
    ///             el usuario para poder realizar las asistencias de los empleados
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 05-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Fechas_Periodo_Nominal()
    {
        Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador = new Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de negocios
        DataTable Dt_Periodo_Nominal; //Obtiene los valores de la consulta y servira para poder asignar estos a los controles correspondientes
        try
        {
            Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.ToString();
            Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
            Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.Consulta_Fechas_Calendario_Reloj_Checador(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

            //Asigna los valores obtenidos de la consulta a los controles correspondientes
            foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                {
                    Txt_Fecha_Inicial.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()));
                    Txt_Fecha_Final.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 21-Febrero-2012
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
    #endregion
    #endregion

    #region Eventos
    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Pdf_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Pdf_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Reporte())
            {
                if (Cmb_Tipo_Reporte.SelectedValue == "RETARDOS Y FALTAS RELOJ CHECADOR")
                    Consulta_Estado_Retardo_Faltas("PDF");

                else if (Cmb_Tipo_Reporte.SelectedValue == "FALTAS RELOJ CHECADOR")
                    Consulta_Estado_Faltas("PDF");

                else if (Cmb_Tipo_Reporte.SelectedValue == "SALIDAS ANTICIPADAS RELOJ CHECADOR")
                    Consulta_Estado_Salida_Anticipada("PDF");

                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE HISTORICO RELOJ CHECADOR")
                    Consulta_Estado_historico_Reloj("PDF");

                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE PERSONAL QUE CHECA")
                    Consulta_Personal_Checa("PDF");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Excel_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Reporte())
            {
                if (Cmb_Tipo_Reporte.SelectedValue == "RETARDOS Y FALTAS RELOJ CHECADOR" )
                    Consulta_Estado_Retardo_Faltas("EXCEL");
                 
                else if (Cmb_Tipo_Reporte.SelectedValue == "FALTAS RELOJ CHECADOR")
                    Consulta_Estado_Faltas("EXCEL");

                else if (Cmb_Tipo_Reporte.SelectedValue == "SALIDAS ANTICIPADAS RELOJ CHECADOR")
                    Consulta_Estado_Salida_Anticipada("EXCEL");

                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE HISTORICO RELOJ CHECADOR")
                    Consulta_Estado_historico_Reloj("EXCEL");

                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE PERSONAL QUE CHECA")
                    Consulta_Personal_Checa("EXCEL");
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN          : Evento del boton de busqueda de empleados
    ///PROPIEDADES          :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 21/Diciembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text))
            {
                if (!string.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                {
                    Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Nombre_Empleado.Text.Trim();
                }
                Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";
                Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General();
                Cmb_Nombre_Empleado.DataSource = new DataTable();
                Cmb_Nombre_Empleado.DataBind();
                Cmb_Nombre_Empleado.DataSource = Dt_Empleados;
                Cmb_Nombre_Empleado.DataTextField = "Empleado";
                Cmb_Nombre_Empleado.DataValueField = Cat_Empleados.Campo_No_Empleado;
                Cmb_Nombre_Empleado.DataBind();
                Cmb_Nombre_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                Cmb_Nombre_Empleado.SelectedIndex = -1;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    #endregion 

    #region combos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Reporte_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Habilitara las cajas de texto correspondientes al reporte
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Tipo_Reporte_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Tipo_Reporte.SelectedValue == "RETARDOS Y FALTAS RELOJ CHECADOR")
                Habilitar_Controles(1);

            else if (Cmb_Tipo_Reporte.SelectedValue == "FALTAS RELOJ CHECADOR")
                Habilitar_Controles(2);

            else if (Cmb_Tipo_Reporte.SelectedValue == "SALIDAS ANTICIPADAS RELOJ CHECADOR")
                Habilitar_Controles(3);

            else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE HISTORICO RELOJ CHECADOR")
                Habilitar_Controles(4);

            else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE PERSONAL QUE CHECA")
                Habilitar_Controles(5);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Nombre_Empleado_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Habilitara las cajas de texto correspondientes al reporte
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Nombre_Empleado_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Nombre_Empleado.SelectedIndex > 0)
            {
                Txt_No_Empleado.Text = Cmb_Nombre_Empleado.SelectedValue;
            }
            else
            {
                Txt_No_Empleado.Text = "";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim()); //Consulta los periodos nominales validos
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
    }
    protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
            Txt_Fecha_Inicial.Text = "";
            Txt_Fecha_Final.Text = "";
            if (index > 0)
            {
                Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION :
    /// PARAMETROS  :
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null; //Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<Seleccione>", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION: Crea el DataTable con la consulta de las nomina vigentes en el 
    ///              sistema.
    /// PARAMETROS : Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///              en el sistema.
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    #endregion
    #endregion
}
