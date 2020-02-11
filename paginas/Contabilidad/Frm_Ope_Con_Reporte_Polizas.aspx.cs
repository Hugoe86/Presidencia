using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Tipo_Polizas.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Reporte_Basicos_Contabilidad.Negocio;

public partial class paginas_contabilidad_Frm_Ope_Con_Reporte_Polizas : System.Web.UI.Page
{
    #region (Load/Init)
        protected void Page_Load(object sender, EventArgs e)
        {
            //Refresca la session del usuario lagueado al sistema.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que exista algun usuario logueado al sistema.
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }
        }
    #endregion
    #region (Metodos)
        #region (Métodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
            ///               diferentes operaciones
            /// PARAMETROS  : 
            /// CREO        : José Antonio López Hernández
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Limpia_Controles();     //Limpia los controles del forma
                    Consulta_Tipo_Poliza(); //Consulta todas los Tipos de Polizas que fueron dadas de alta en la BD
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpiar_Controles
            /// DESCRIPCION : Limpia los controles que se encuentran en la forma
            /// PARAMETROS  : 
            /// CREO        : José Antonio López Hernández
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {
                    Txt_Fecha_Final.Text = "";
                    Txt_Fecha_Inicio.Text = "";
                    Txt_No_Poliza_Inicio.Text = "";
                    Txt_No_Poliza_Termino.Text = "";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Control Acceso Pagina)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: IsNumeric
            /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
            /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
            /// CREO        : José Antonio López Hernández
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
                    throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
                }
                return Resultado;
            }
        #endregion
        #region (Método Consulta)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Tipo_Poliza
            /// DESCRIPCION : Consulta los Tipos de Poliza que estan dadas de alta en la BD
            /// PARAMETROS  : 
            /// CREO        : José Antonio López Hernández
            /// FECHA_CREO  : 09-Junio-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Negocio Rs_Consulta_Cat_Con_Tipo_Polizas = new Cls_Cat_Con_Tipo_Polizas_Negocio(); //Variable de conexión hacia la capa de Negocios
                DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 

                try
                {
                    Rs_Consulta_Cat_Con_Tipo_Polizas.P_Descripcion = "";
                    Dt_Tipo_Poliza = Rs_Consulta_Cat_Con_Tipo_Polizas.Consulta_Datos_Tipo_Poliza(); //Consulta los datos generales de los Tipos de Poliza dados de alta en la BD
                    Session["Consulta_Tipo_Poliza"] = Dt_Tipo_Poliza;
                    Llena_Check_Tipo_Poliza(); //Agrega los tipos de Poliza obtenidas de la consulta anterior
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Tipo_Poliza " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Llena_Check_Tipo_Poliza
            /// DESCRIPCION : Llena el grid con los Tipos de Poliza que se encuentran en la 
            ///               base de datos
            /// PARAMETROS  : 
            /// CREO        : José Antonio López Hernández
            /// FECHA_CREO  : 11/Julio/2011 13:18
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Llena_Check_Tipo_Poliza()
            {
                DataTable Dt_Tipo_Poliza; //Variable que obtendra los datos de la consulta 
                try
                {
                    Chk_Tipos_Poliza.DataBind();
                    Dt_Tipo_Poliza = (DataTable)Session["Consulta_Tipo_Poliza"];
                    Chk_Tipos_Poliza.DataSource = Dt_Tipo_Poliza;
                    Chk_Tipos_Poliza.DataTextField = "Descripcion";
                    Chk_Tipos_Poliza.DataValueField = "Tipo_Poliza_ID";
                    Chk_Tipos_Poliza.DataBind();
                }
                catch (Exception ex)
                {
                    throw new Exception("Llena_Check_Tipo_Poliza " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
            ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
            ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
            ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
            ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
            ///CREO: Susana Trigueros Armenta
            ///FECHA_CREO: 01/Mayo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
            {
                ReportDocument Reporte = new ReportDocument();
                String File_Path = Server.MapPath("../Rpt/Contabilidad/" + Nombre_Reporte);
                Reporte.Load(File_Path);
                Ds_Reporte = Data_Set_Consulta_DB;
                Reporte.SetDataSource(Ds_Reporte);
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);
                String Ruta = "../../Reporte/" + Nombre_PDF;
                Mostrar_Reporte(Nombre_PDF, "PDF");
            }
            ///*******************************************************************************
            ///NOMBRE:              Mostrar_Reporte
            ///DESCRIPCIÓN:         Muestra el reporte en pantalla.
            ///PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
            ///                     Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
            ///USUARIO CREO:        Juan Alberto Hernández Negrete.
            ///FECHA CREO:          3/Mayo/2011 18:20 p.m.
            ///USUARIO MODIFICO:    Salvador Hernández Ramírez
            ///FECHA MODIFICO:      16-Mayo-2011
            ///CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
            ///*******************************************************************************
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
                String Fecha_Valida = Fecha;
                //Se le aplica un split a la fecha 
                String[] aux = Fecha.Split('/');
                //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
                switch (aux[1])
                {
                    case "dic":
                        aux[1] = "DEC";
                        break;
                }
                //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
                Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
                return Fecha_Valida;
            }// fin de Formato_Fecha
        #endregion
        #region (Validaciones)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Operacion
            /// DESCRIPCION : Validar datos requeridos para realizar la operación.
            /// CREO        : Juan Alberto Hernandez Negrete
            /// FECHA_CREO  : 18/Mayo/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                try
                {
                    if (Txt_No_Poliza_Inicio.Text != "" || Txt_No_Poliza_Termino.Text != "")
                    {
                        if (Txt_No_Poliza_Inicio.Text == "")
                        {
                            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario proporcionar el limite inicial del rango de números de póliza. <br>";
                            Datos_Validos = false;
                        }
                        else
                        {
                            if (Txt_No_Poliza_Termino.Text == "")
                            {
                                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario proporcionar el limite final del rango de números de póliza. <br>";
                                Datos_Validos = false;
                            }
                        }
                    }

                    if (Txt_Fecha_Inicio.Text == "" || Txt_Fecha_Final.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario seleccionar el periodo de fechas para generar el reporte. <br>";
                        Datos_Validos = false;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al validar los datos para el reporte. Error: [" + Ex.Message + "]");
                }

                return Datos_Validos;
            }
        #endregion
    #endregion
    #region (Eventos)
        protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
        {
            String Mi_SQL = ""; //Arma el query de consulta de las polizas por tipo
            Boolean Selecciono_Tipos_Poliza = false; //Anexa el filtro de los tipos de poliza
            Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Rs_Consulta_Ope_Con_Poliza_Detalles = new Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio(); //Conexion hacia la capa de negocioss

            try
            {
                if (Validar_Datos())
                {
                    //Creamos el objeto del dataset perteneciente al reporte
                    Ds_Rpt_Con_Tipos_Polizas Ds_Obj_Rpt_Tipos_Polizas = new Ds_Rpt_Con_Tipos_Polizas();                    

                    Mi_SQL = " AND (";
                    for (int Cont_Tipos = 0; Cont_Tipos < Chk_Tipos_Poliza.Items.Count; Cont_Tipos++)
                    {
                        if (Chk_Tipos_Poliza.Items[Cont_Tipos].Selected == true)
                        {
                            Mi_SQL += Ope_Con_Polizas.Tabla_Ope_Con_Polizas + "." + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + " = '" + Chk_Tipos_Poliza.Items[Cont_Tipos].Value + "' OR ";
                            Selecciono_Tipos_Poliza = true;
                        }
                    }
                    //Si hubo tipos seleccionados quita el ultimo OR y agrega el parentesis del cierre
                    if (Selecciono_Tipos_Poliza == true)
                    {
                        Mi_SQL = Mi_SQL.Substring(0, Mi_SQL.Length - 3);
                        Mi_SQL += ")";
                    }
                    //Si no se selecciono ningun tipo se quita el primer parentesis y el AND
                    else
                    {
                        Mi_SQL = "";
                    }

                    if (!String.IsNullOrEmpty(Mi_SQL)) Rs_Consulta_Ope_Con_Poliza_Detalles.P_Tipo_Polizas = Mi_SQL;
                    if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text)) Rs_Consulta_Ope_Con_Poliza_Detalles.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text));
                    if (!String.IsNullOrEmpty(Txt_Fecha_Final.Text)) Rs_Consulta_Ope_Con_Poliza_Detalles.P_Fecha_Final = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text));
                    DataSet Ds_Rpt_Tipos_Polizas_Datos = Rs_Consulta_Ope_Con_Poliza_Detalles.Consulta_Tipo_Poliza();

                    Ds_Rpt_Tipos_Polizas_Datos.Tables[0].TableName = "Ope_Polizas";
                    Generar_Reporte(Ds_Rpt_Tipos_Polizas_Datos, Ds_Obj_Rpt_Tipos_Polizas, "Rpt_Con_Tipos_Polizas.rpt", "Rpt_Con_Tipos_Polizas.pdf");
                }
                else
                {
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Visible = true;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message;
            }
        }
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    #endregion
}