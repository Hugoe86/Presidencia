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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Cuentas_Contables.Negocio;
using Presidencia.Reporte_Basicos_Contabilidad.Negocio;

public partial class paginas_Contabilidad_Frm_Rpt_Balanza_Mensual : System.Web.UI.Page
{
    #region (Load/Init)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Refresca la session del usuario lagueado al sistema.
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                //Valida que exista algun usuario logueado al sistema.
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
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
        #region (Métodos Generales)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Inicializa_Controles
            /// DESCRIPCION : Inicializa los controles de la forma para prepararla para el
            ///               reporte
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Inicializa_Controles()
            {
                try
                {
                    Cargar_Cuentas_Contables_Combos(); //Carga los cmbos con la descripci{on de las cuentas contables que se tienen dadas de alta0
                    Limpia_Controles(); //limpia los campos de la forma
                }
                catch (Exception ex)
                {
                    throw new Exception("Inicializa_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Limpiar_Controles
            /// DESCRIPCION : Limpia los controles que se encuentran en la forma
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Limpia_Controles()
            {
                try
                {  
                    Cmb_Anio.SelectedIndex = -1;
                    Cmb_Meses.SelectedIndex = -1;
                    Cmb_Cuenta_Inicial.SelectedIndex = -1;
                    Cmb_Cuenta_Contable_Final.SelectedIndex = -1;
                    Chk_Movimientos_Saldo_No_Cero.Checked = false;
                    Txt_Cuenta_Inicial.Text = "";
                    Txt_Cuenta_Final.Text = "";
                }
                catch (Exception ex)
                {
                    throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Validar_Datos_Reporte
            /// DESCRIPCION : Validar que se hallan proporcionado todos los datos para la
            ///               generación del reporte
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 18-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private Boolean Validar_Datos_Reporte()
            {
                Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
                Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

                if (Cmb_Tipo_Balance.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Tipo de Balance. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Meses.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El mes a consultar. <br>";
                    Datos_Validos = false;
                }
                if (Cmb_Anio.SelectedIndex <= 0)
                {
                    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Año a consultar. <br>";
                    Datos_Validos = false;
                }
                return Datos_Validos;
            }
        #endregion
        #region (Consultas)
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Cargar_Cuentas_Contables_Combos
            /// DESCRIPCION : Consulta las cuentas contables estan dadas de alta
            /// PARAMETROS  :                           
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Cargar_Cuentas_Contables_Combos()
            {
                DataTable Dt_Cuenta_Contable = new DataTable(); //Variable a contener los valores de la consulta
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión hacia la capa de negocios
                try
                {
                    Dt_Cuenta_Contable = Rs_Consulta_Cat_Con_Cuentas_Contables.Consulta_Cuentas_Contables(); //Consulta las cuentas contables que se tienen dadas de alta para cargar el combo
                    Cmb_Cuenta_Inicial.DataSource = Dt_Cuenta_Contable;
                    Cmb_Cuenta_Inicial.DataValueField = Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                    Cmb_Cuenta_Inicial.DataTextField = Cat_Con_Cuentas_Contables.Campo_Descripcion;
                    Cmb_Cuenta_Inicial.DataBind();

                    Cmb_Cuenta_Inicial.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                    Cmb_Cuenta_Inicial.SelectedIndex = 0;

                    Cmb_Cuenta_Contable_Final.DataSource = Dt_Cuenta_Contable;
                    Cmb_Cuenta_Contable_Final.DataValueField = Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
                    Cmb_Cuenta_Contable_Final.DataTextField = Cat_Con_Cuentas_Contables.Campo_Descripcion;
                    Cmb_Cuenta_Contable_Final.DataBind();

                    Cmb_Cuenta_Contable_Final.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                    Cmb_Cuenta_Contable_Final.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Cargar_Cuentas_Contables_Combos " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Cuenta_Contable
            /// DESCRIPCION : Consulta que la cuenta contable que proporciono el usuario este
            ///               dada de alta si es así consulta el ID o Cuenta a la cual pertenece
            /// PARAMETROS  : Tipo_Busqueda: Indica si la consulta se realiza desde una caja
            ///                              de Texto o Combo y de acuerdo a esto realiza el
            ///                              filtro que debera llevar
            ///               Cuenta_Incia : Indica si se esta consultando la cuenta desde donde
            ///                              se iniciara la consulta de la cuentas contables                              
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Cuenta_Contable(String Tipo_Busqueda, String Cuenta_Inicial)
            {
                DataTable Dt_Cuenta_Contable = new DataTable(); //Variable a contener los valores de la consulta
                Cls_Cat_Con_Cuentas_Contables_Negocio Rs_Consulta_Cat_Con_Cuentas_Contables = new Cls_Cat_Con_Cuentas_Contables_Negocio(); //Variable de conexión hacia la capa de negocios

                try
                {
                    //Si se pretende consultar la cuenta contable del combo de la cuenta contable
                    if (Tipo_Busqueda == "COMBO")
                    {
                        if (Cuenta_Inicial == "SI")
                        {
                            Rs_Consulta_Cat_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Cmb_Cuenta_Inicial.SelectedValue;
                            Txt_Cuenta_Inicial.Text = "";
                        }
                        else
                        {
                            Rs_Consulta_Cat_Con_Cuentas_Contables.P_Cuenta_Contable_ID = Cmb_Cuenta_Contable_Final.SelectedValue;
                            Txt_Cuenta_Final.Text = "";
                        }
                    }
                    //Si se pretende consultar el ID de la cuenta contable proporcionada por el usuario
                    else
                    {
                        if (Cuenta_Inicial == "SI")
                        {
                            Rs_Consulta_Cat_Con_Cuentas_Contables.P_Cuenta = Txt_Cuenta_Inicial.Text.ToString();
                            Cmb_Cuenta_Inicial.SelectedIndex = -1;
                        }
                        else
                        {
                            Rs_Consulta_Cat_Con_Cuentas_Contables.P_Cuenta = Txt_Cuenta_Final.Text.ToString();
                            Cmb_Cuenta_Contable_Final.SelectedIndex = -1;
                        }
                    }
                    Dt_Cuenta_Contable = Rs_Consulta_Cat_Con_Cuentas_Contables.Consulta_Cuentas_Contables(); //Consulta el ID y número de cuenta de la cuenta que se pretende buscar

                    //Agrega los datos de la consulta en los controles correspondientes
                    foreach (DataRow Registro in Dt_Cuenta_Contable.Rows)
                    {
                        //Agrega la cuenta contable que tiene asignado ID seleccionado del combo por el usuario
                        if (Tipo_Busqueda == "COMBO")
                        {
                            if (Cuenta_Inicial == "SI")
                            {
                                Txt_Cuenta_Inicial.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString();
                            }
                            else
                            {
                                Txt_Cuenta_Final.Text = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta].ToString();
                            }
                        }
                        //Selecciona el ID de la cuenta contable que fue proporcionada por el usuario
                        else
                        {
                            if (Cuenta_Inicial == "SI")
                            {
                                Cmb_Cuenta_Inicial.SelectedValue = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                            }
                            else
                            {
                                Cmb_Cuenta_Contable_Final.SelectedValue = Registro[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Cuenta_Contable " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consulta_Balance_Mensual
            /// DESCRIPCION : Consulta los datos a Generar para poder mostrar el reporte al
            ///               usuario
            /// PARAMETROS  : 
            /// CREO        : Yazmin Abigail Delgado Gómez
            /// FECHA_CREO  : 18-Octubre-2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consulta_Balance_Mensual()
            {
                String Mes; //Obtiene el mes para la genración del reporte
                String Anio; //Obtiene el año para la genración del reporte
                String Ruta_Archivo = @Server.MapPath("../Rpt/Contabilidad/");//Obtiene la ruta en la cual será guardada el archivo
                String Nombre_Archivo = "Balance_Mensual" + Session.SessionID + Convert.ToString(String.Format("{0:ddMMMyyyHHmmss}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
                DataRow Registro; //Obtiene los valores de la consulta realizada para la impresión del recibo
                Ds_Rpt_Con_Balanza_Mensual Ds_Balance_Mensual = new Ds_Rpt_Con_Balanza_Mensual();
                DataTable Dt_Balance_Mensual = new DataTable(); //Va a conter los valores a pas-ar al reporte                
                Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Rs_Consulta_Ope_Con_Cierre_Mensual = new Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio(); //Conexion hacia la capa de negocios

                try
                {
                    Anio = Cmb_Anio.SelectedItem.Text.ToString();
                    Mes = Cmb_Meses.SelectedItem.Text.Substring(0, 2).ToString();
                    Anio = Anio.Substring(2, 2).ToString();
                    Rs_Consulta_Ope_Con_Cierre_Mensual.P_Montos_Cero="NO";
                    if (Cmb_Cuenta_Inicial.SelectedIndex > 0) Rs_Consulta_Ope_Con_Cierre_Mensual.P_Cuenta_Inicial = Txt_Cuenta_Inicial.Text.ToString();
                    if (Cmb_Cuenta_Contable_Final.SelectedIndex > 0) Rs_Consulta_Ope_Con_Cierre_Mensual.P_Cuenta_Final = Txt_Cuenta_Final.Text.ToString();
                    if (Chk_Movimientos_Saldo_No_Cero.Checked == true) Rs_Consulta_Ope_Con_Cierre_Mensual.P_Montos_Cero = "SI";
                    Rs_Consulta_Ope_Con_Cierre_Mensual.P_Mes_Anio = Mes + Anio;
                    Dt_Balance_Mensual = Rs_Consulta_Ope_Con_Cierre_Mensual.Consulta_Balanza_Mensual(); //Consulta los valores del mes y el año que selecciono el usuario

                    if (Dt_Balance_Mensual.Rows.Count > 0)
                    {
                        // Se llena la cabecera del DataSet                
                        //Registro = Dt_Balance_Mensual.Rows[0];
                        //Ds_Balance_Mensual.Tables["Balanza_Mensual"].ImportRow(Registro);
                        Dt_Balance_Mensual.TableName = "Balanza_Mensual";

                        Ds_Balance_Mensual.Clear();
                        Ds_Balance_Mensual.Tables.Clear();
                        Ds_Balance_Mensual.Tables.Add(Dt_Balance_Mensual.Copy());
                    }
                    ReportDocument Reporte = new ReportDocument();
                    Reporte.Load(Ruta_Archivo + "Rpt_Balanza_Mensual.rpt");
                    Reporte.SetDataSource(Ds_Balance_Mensual);

                    ParameterFieldDefinitions Cr_Parametros;
                    ParameterFieldDefinition Cr_Parametro;
                    ParameterValues Cr_Valor_Parametro = new ParameterValues();
                    ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                    Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                    Cr_Parametro = Cr_Parametros["Mes"];
                    Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                    Cr_Valor_Parametro.Clear();

                    Cr_Valor.Value = Cmb_Meses.SelectedValue;
                    Cr_Valor_Parametro.Add(Cr_Valor);
                    Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                    Cr_Parametro = Cr_Parametros["Anio"];
                    Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                    Cr_Valor_Parametro.Clear();

                    Cr_Valor.Value = Cmb_Anio.SelectedValue;
                    Cr_Valor_Parametro.Add(Cr_Valor);
                    Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                    if (Dt_Balance_Mensual.Rows.Count > 0)
                    {
                        Dt_Balance_Mensual = new DataTable();

                        Dt_Balance_Mensual = Rs_Consulta_Ope_Con_Cierre_Mensual.Consulta_Balanza_Mensual_Debe_Haber(); //Consulta los totales del debe y haber de las cuenntas contable seleccionadas por el usuario
                        //Obtiene los valores de la consulta anterior
                        foreach (DataRow Balance in Dt_Balance_Mensual.Rows)
                        {
                            Cr_Parametro = Cr_Parametros["Total_Debe"];
                            Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                            Cr_Valor_Parametro.Clear();

                            Cr_Valor.Value = Convert.ToDecimal(Balance["Total_Debe"].ToString());
                            Cr_Valor_Parametro.Add(Cr_Valor);
                            Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                            Cr_Parametro = Cr_Parametros["Total_Haber"];
                            Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                            Cr_Valor_Parametro.Clear();

                            Cr_Valor.Value =Convert.ToDecimal(Balance["Total_Haber"].ToString());
                            Cr_Valor_Parametro.Add(Cr_Valor);
                            Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                        }
                    }
                    else
                    {
                        Cr_Parametro = Cr_Parametros["Total_Debe"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = 0;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                        Cr_Parametro = Cr_Parametros["Total_Haber"];
                        Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                        Cr_Valor_Parametro.Clear();

                        Cr_Valor.Value = 0;
                        Cr_Valor_Parametro.Add(Cr_Valor);
                        Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                    }

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
                    throw new Exception("Consulta_Balance_Mensual " + ex.Message.ToString(), ex);
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
            ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
            ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
            ///                             para mostrar los datos al usuario
            ///CREO       : Yazmin A Delgado Gómez
            ///FECHA_CREO : 12-Octubre-2011
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
    #region (Eventos)
        protected void Cmb_Cuenta_Inicial_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_Cuenta_Inicial.Text = "";
            if (Cmb_Cuenta_Inicial.SelectedIndex > 0) Consulta_Cuenta_Contable("COMBO", "SI"); //Consulta el No Cuenta Contable que tiene seleccionado la descripcion de la cuenta proporcionada
        }
        protected void Cmb_Cuenta_Contable_Final_SelectedIndexChanged(object sender, EventArgs e)
        {
            Txt_Cuenta_Final.Text = "";
            if (Cmb_Cuenta_Contable_Final.SelectedIndex > 0) Consulta_Cuenta_Contable("COMBO", "NO"); //Consulta el No Cuenta Contable que tiene seleccionado la descripcion de la cuenta proporcionada
        }
        protected void Txt_Cuenta_Inicial_TextChanged(object sender, EventArgs e)
        {
            Cmb_Cuenta_Inicial.SelectedIndex = -1;
            if (!String.IsNullOrEmpty(Txt_Cuenta_Inicial.Text)) Consulta_Cuenta_Contable("TEXTO", "SI"); //Consulta el ID de la cuenta contable que tiene asignado el n{umero de cuenta que fue proporcionada por el usuario
        }
        protected void Txt_Cuenta_Final_TextChanged(object sender, EventArgs e)
        {
            Cmb_Cuenta_Contable_Final.SelectedIndex = -1;
            if (!String.IsNullOrEmpty(Txt_Cuenta_Final.Text)) Consulta_Cuenta_Contable("TEXTO", "NO"); //Consulta el ID de la cuenta contable que tiene asignado el n{umero de cuenta que fue proporcionada por el usuario
        }
        protected void Btn_Reporte_Balance_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Validar_Datos_Reporte())
                {
                    Consulta_Balance_Mensual(); //Consulta el balance mensual
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    #endregion
}
