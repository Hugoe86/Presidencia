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
using System.Collections.Generic;
using System.Globalization;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Generacion_Asistencias.Negocio;

public partial class paginas_Nomina_Frm_Ope_Nom_Generacion_Asistencias_Empleados : System.Web.UI.Page
{
    #region(Load/Init)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
        ///               diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializa_Controles()
        {
            try
            {
                Limpia_Controles(); //Limpia los controles del forma
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
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Txt_Fecha_Entrada_Asistencia.Text = "";
                Txt_Fecha_Salida_Asistencia.Text = "";
                Grid_Lista_Asistencias.DataSource = new DataTable();
                Grid_Lista_Asistencias.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Llena_Grid_Lista_Asistencias
        /// DESCRIPCION : Llena el grid con las asistencias de los Empleados que pertenecen
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Llena_Grid_Lista_Asistencias()
        {
            DataTable Dt_Lista_Asistencia; //Variable que obtendra los datos de la consulta 
            try
            {

                Grid_Lista_Asistencias.Columns[1].Visible = true;
                Grid_Lista_Asistencias.Columns[2].Visible = true;
                Dt_Lista_Asistencia = (DataTable)Session["Consulta_Lista_Asistencia"];
                Grid_Lista_Asistencias.DataSource = Dt_Lista_Asistencia;
                Grid_Lista_Asistencias.DataBind();
                Grid_Lista_Asistencias.Columns[1].Visible = false;
                Grid_Lista_Asistencias.Columns[2].Visible = false;
                Grid_Lista_Asistencias.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Asistencias " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos_Asistencias_Empleado
        /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 08-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos_Asistencias_Empleado()
        {
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

            if (string.IsNullOrEmpty(Txt_Fecha_Entrada_Asistencia.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Entrada es un dato requerido por el sistema. <br>";
                Datos_Validos = false;
            }
            if (string.IsNullOrEmpty(Txt_Fecha_Salida_Asistencia.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha de Salida es un dato requerido por el sistema. <br>";
                Datos_Validos = false;
            }
            return Datos_Validos;
        }
    #endregion
    #region (Grid)
        protected void Grid_Lista_Asistencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Grid_Lista_Asistencias.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Lista_Asistencias();                    //Carga las Asistencias que estan asignados a la página seleccionada
                Grid_Lista_Asistencias.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion
    #region (Eventos)
        protected void Btn_Generar_Asistencias_Empleados_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio(); //Variable de conexión a la capa de negocios
            DataTable Dt_Asistencias_Empleados; //Variable que obtendra los datos de la consulta 
            String Fecha_Hora; //Obtiene la fecha y la hora de la entrada del empleado

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                if (Validar_Datos_Asistencias_Empleado())
                {
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Entrada_Asistencia.Text.ToString()) + " 00:00:00";
                    Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Hora_Entrada = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Salida_Asistencia.Text.ToString()) + " 23:59:59";
                    Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Hora_Salida = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                    Dt_Asistencias_Empleados = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Asistencias_Empleados(); //Consulta los datos generales de las asistencias de los empleados
                    Session["Consulta_Lista_Asistencia"] = Dt_Asistencias_Empleados;
                    Llena_Grid_Lista_Asistencias(); //Agrega las asistencias obtenidas de la consulta anterior
                    if (Grid_Lista_Asistencias.Rows.Count > 0) Btn_Generar_Asistencias_Empleados.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Registrar_Asistencias_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Rs_Alta_Ope_Nom_Asistencias= new Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio(); //Vairble de conexión a la capa de Negocios

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Valida que existan datos que importar en la base de datos
                if (Grid_Lista_Asistencias.Rows.Count > 0)
                {
                    Rs_Alta_Ope_Nom_Asistencias.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Rs_Alta_Ope_Nom_Asistencias.P_Dt_Lista_Asistencia = (DataTable)Session["Consulta_Lista_Asistencia"];
                    Rs_Alta_Ope_Nom_Asistencias.Alta_Asistencias(); //Da de Alta las asistencias de los empleados en la base de datos
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pasar Registros de Reloj Checador", "alert('El Alta de los registros fue Exitosa');", true);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No hay asistencias generadas en el periodo que consulto <br>";
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
            try
            {
                Session.Remove("Consulta_Lista_Asistencia");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion
}
