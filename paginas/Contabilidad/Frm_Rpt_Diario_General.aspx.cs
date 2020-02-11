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
using Presidencia.Reporte_Basicos_Contabilidad.Negocio;
using Presidencia.DateDiff;

public partial class paginas_Contabilidad_Frm_Rpt_Diario_General : System.Web.UI.Page
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
                    Limpia_Controles();//Limpia los controles de la forma
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Limpiar_Controles
        /// DESCRIPCION : Limpia los controles que se encuentran en la forma
        /// PARAMETROS  : 
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 19-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Cmb_Libro_Diario.SelectedIndex = 0;
                Txt_Fecha_Inicio.Text = "";
                Txt_Fecha_Final.Text = "";                
            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Diario_General
        /// DESCRIPCION : Consulta todos los movimientos con sus detalles de las pólizas
        ///               realizadas en el rango de fechas proporcionadas por el usuario
        /// PARAMETROS  : 
        /// CREO        : Yazmin Abigail Delgado Gómez
        /// FECHA_CREO  : 19-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Consulta_Diario_General()
        {
            String Ruta_Archivo = @Server.MapPath("../Rpt/Contabilidad/");//Obtiene la ruta en la cual será guardada el archivo
            String Nombre_Archivo = "Diario_General" + Session.SessionID + Convert.ToString(String.Format("{0:ddMMMyyyHHmmss}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
            DataTable Dt_Diario_General = new DataTable(); //Variable a conter los valores a pasar al reporte
            Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio Rs_Consulta_Ope_Con_Polizas = new Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio(); //Conexion hacia la capa de negocios
            ReportDocument Reporte = new ReportDocument();

            try
            {
                Rs_Consulta_Ope_Con_Polizas.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text));
                Rs_Consulta_Ope_Con_Polizas.P_Fecha_Final = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text));
                if (Cmb_Libro_Diario.SelectedValue == "GENERAL")
                {
                    Ds_Rpt_Con_Diario_General Ds_Diario_General = new Ds_Rpt_Con_Diario_General();
                    DataTable Dt_Diario_General_Detalles = new DataTable(); //Variable a conter los valores a pasar al reporte

                    Dt_Diario_General = Rs_Consulta_Ope_Con_Polizas.Consulta_Diario_General();                  //Consulta las pólizas realizadas por rango de fechas
                    Dt_Diario_General_Detalles = Rs_Consulta_Ope_Con_Polizas.Consulta_Diario_General_Detalles();//Consulta los datos generales del detalle de las pólizas

                    Dt_Diario_General.TableName = "Diario_General";
                    Dt_Diario_General_Detalles.TableName = "Detalles_Diario_General";

                    Ds_Diario_General.Clear();
                    Ds_Diario_General.Tables.Clear();

                    Ds_Diario_General.Tables.Add(Dt_Diario_General.Copy());
                    Ds_Diario_General.Tables.Add(Dt_Diario_General_Detalles.Copy());

                    
                    Reporte.Load(Ruta_Archivo + "Rpt_Con_Diario_General.rpt");
                    Reporte.SetDataSource(Ds_Diario_General);
                }
                else
                {
                    Ds_Rpt_Con_Libro_Diario_CONAC Ds_Libro_Diario_CONAC = new Ds_Rpt_Con_Libro_Diario_CONAC();

                    Dt_Diario_General = Rs_Consulta_Ope_Con_Polizas.Consulta_Libro_Diario();       //Consulta las pólizas realizadas por rango de fechas
                    Dt_Diario_General.TableName = "Ds_Libro_Diario_CONAC";

                    Ds_Libro_Diario_CONAC.Clear();
                    Ds_Libro_Diario_CONAC.Tables.Clear();

                    Ds_Libro_Diario_CONAC.Tables.Add(Dt_Diario_General.Copy());

                    Reporte.Load(Ruta_Archivo + "Rpt_Con_Libro_Diario_CONAC.rpt");
                    Reporte.SetDataSource(Ds_Libro_Diario_CONAC);
                }                

                ParameterFieldDefinitions Cr_Parametros;
                ParameterFieldDefinition Cr_Parametro;
                ParameterValues Cr_Valor_Parametro = new ParameterValues();
                ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                Cr_Parametro = Cr_Parametros["Fecha_Inicial"];
                Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                Cr_Valor_Parametro.Clear();

                Cr_Valor.Value = Txt_Fecha_Inicio.Text.ToString();
                Cr_Valor_Parametro.Add(Cr_Valor);
                Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                Cr_Parametro = Cr_Parametros["Fecha_Final"];
                Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                Cr_Valor_Parametro.Clear();

                Cr_Valor.Value = Txt_Fecha_Final.Text.ToString();
                Cr_Valor_Parametro.Add(Cr_Valor);
                Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);

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
                throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos_Reporte
        /// DESCRIPCION : Validar que se hallan proporcionado todos los datos para la
        ///               generación del reporte
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 19-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos_Reporte()
        {
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

            if (Cmb_Libro_Diario.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Tipo de libro Diario a Generar. <br>";
                Datos_Validos = false;
            }
            if (String.IsNullOrEmpty(Txt_Fecha_Inicio.Text))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La fecha en que va a iniciar la consulta. <br>";
                Datos_Validos = false;
            }
            if (String.IsNullOrEmpty(Txt_Fecha_Final.Text))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La fecha en que va a finalizar la consulta. <br>";
                Datos_Validos = false;
            }
            return Datos_Validos;
        }
    #endregion
    protected void Btn_Reporte_Balance_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Validar_Datos_Reporte())
            {
                
                Consulta_Diario_General(); //Consulta el diario general

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
}
