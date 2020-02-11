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
using Presidencia.Almacen_Orden_Salida.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;

public partial class paginas_Compras_Frm_Alm_Com_Orden_Salida : System.Web.UI.Page
{
    #region (Variables)

    #endregion

    #region (Page Load)
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Estado_Inicial();
                }
                else
                {
                    Lbl_Informacion.Enabled = false;
                    Img_Warning.Visible = false;
                    Lbl_Informacion.Text = "";
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Page_Load)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion

    #region (Metodos)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Mostrar_Informacion
        ///DESCRIPCION:             Habilita o deshabilita la muestra en pantalle del mensaje 
        ///                         de Mostrar_Informacion para el usuario
        ///PARAMETROS:              1.- Condicion, entero para saber si es 1 habilita para que se muestre mensaje si es cero
        ///                         deshabilita para que no se muestre el mensaje
        ///CREO:                    Silvia Morales Portuhondo
        ///FECHA_CREO:              23/Septiembre/2010 
        ///MODIFICO:                Noe Mosqueda Valadez
        ///FECHA_MODIFICO:          22/Octubre/2010 11:38
        ///CAUSA_MODIFICACION:      Agregar try-catch para el manejo de errores  
        ///*******************************************************************************
        private void Mostrar_Informacion(int Condicion)
        {
            try
            {
                if (Condicion == 1)
                {
                    Lbl_Informacion.Enabled = true;
                    Img_Warning.Visible = true;
                }
                else
                {
                    Lbl_Informacion.Text = "";
                    Lbl_Informacion.Enabled = false;
                    Img_Warning.Visible = false;
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Enabled = true;
                Img_Warning.Visible = true;
                Lbl_Informacion.Text = "Error: " + ex.ToString();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Estado_Inicial
        ///DESCRIPCION:             Colocar la pagina en un estaod inicial para su navegacion
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              24/Noviembre/2010 17:05
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Estado_Inicial()
        {
            //Declaracion de variables
            long No_Requisicion = 0; //Variable para el numero de la requisicion

            try
            {
                Elimina_Sesiones();
                Limpiar_Controles();
                Grid_Requisiciones_Detalles.SelectedIndex = -1;
                //No_Requisicion = (long)HttpUtility.HtmlDecode(Request.QueryString["No_Requisicion"]);
                No_Requisicion = Convert.ToInt64(HttpUtility.HtmlDecode(Request.QueryString["No_Requisicion"]));
                Session["No_Requisicion"] = No_Requisicion;

                String Pagina = HttpUtility.HtmlDecode(Request.QueryString["Pagina"].Trim());
                Session["Pagina"] = Pagina;

                Llena_Datos(No_Requisicion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Llena_Grid_Requisiciones_Detalles
        ///DESCRIPCION:             Llenar el grid de los detalles de una requisicion
        ///PARAMETROS:              1. No_Requisicion: Numero que contiene el No de la requisicion
        ///                         2. Pagina: Entero que indica la pagina del grid a visualizar
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              12/Noviembre/2010 11:47
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Llena_Grid_Requisiciones_Detalles(long No_Requisicion, int Pagina)
        {
            //Declaracion de variables
            DataTable Dt_Requisiciones_Detalles = new DataTable(); //Tabla para los detalles de la requisicion
            Cls_Alm_Com_Orden_Salida_Negocio Orden_Salida_Negocio = new Cls_Alm_Com_Orden_Salida_Negocio(); //Variable para la capa de negocios

            try
            {
                //Relizar la consulta de los detalles de la requisicion
                Orden_Salida_Negocio.P_No_Requisicion = No_Requisicion;
                Dt_Requisiciones_Detalles = Orden_Salida_Negocio.Consulta_Requisicion_Detalles();

                if (Dt_Requisiciones_Detalles.Rows.Count > 0)
                {
                    //LLenar el grid
                    Grid_Requisiciones_Detalles.DataSource = Dt_Requisiciones_Detalles;

                    //Verificar si hay pagina
                    if (Pagina > -1)
                        Grid_Requisiciones_Detalles.PageIndex = Pagina;
                    Grid_Requisiciones_Detalles.Columns[2].Visible=true;
                    Grid_Requisiciones_Detalles.Columns[3].Visible = true;
                    Grid_Requisiciones_Detalles.DataBind();
                    Grid_Requisiciones_Detalles.Columns[2].Visible = false;
                    Grid_Requisiciones_Detalles.Columns[3].Visible = false;

                    //Colocar la tabla en la variable de sesion
                    Session["Dt_Requisiciones_Detalles"] = Dt_Requisiciones_Detalles;
                    Btn_Nuevo.Visible = true;
                }
                else
                {
                    Btn_Nuevo.Visible = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Limpiar_Controles
        ///DESCRIPCION:             Limpiar los controles del formulario
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              12/Noviembre/2010 11:51
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Limpiar_Controles()
        {
            try
            {
                Txt_Comentarios.Text = "";
                Txt_Dependencia.Text = "";
                Txt_Fecha.Text = "";
                Txt_Requisicion_ID.Text = "";
                Grid_Requisiciones_Detalles.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Elimina_Sesiones
        ///DESCRIPCION:             Eliminar las sesiones utilizadas en la pagina
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              12/Noviembre/2010 11:53
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Elimina_Sesiones()
        {
            try
            {
                Session.Remove("Dt_Requisiciones_Detalles");
                Session.Remove("No_Requisicion");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Alta_Orden_Salida
        ///DESCRIPCION:             Dar de alta la orden de salida
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              24/Noviembre/2010 16:29
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Alta_Orden_Salida()
        {
            //Declaracion de variables
            Cls_Alm_Com_Orden_Salida_Negocio Orden_Salida_Negocio = new Cls_Alm_Com_Orden_Salida_Negocio(); //Variable para la capa de negocios
            Ds_Alm_Com_Orden_Salida Ds_Alm_Com_Orden_Salida_src = new Ds_Alm_Com_Orden_Salida(); //Dataset archivo para el llenado del reporte
            String Formato = "PDF";
            DataTable Dt_Datos_Requisicion = new DataTable(); //Tabla que contiene los datos de la requisicion
            long No_Salida = 0;

            try
            {
                //Colocar variable de sesione n la tabla
                Dt_Datos_Requisicion = (DataTable)Session["Dt_Datos_Requisicion"];
               
                //Asignar propiedades
                Orden_Salida_Negocio.P_No_Requisicion = (long)Session["No_Requisicion"];
                
                //Verificar si son 250 caracteres
                if (Txt_Comentarios.Text.Trim().Length > 250)
                    Orden_Salida_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim().Substring(0, 250);
                else
                    Orden_Salida_Negocio.P_Comentarios = Txt_Comentarios.Text.Trim();

                Orden_Salida_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Orden_Salida_Negocio.P_Empleado_Surtido_ID = Cls_Sessiones.Empleado_ID;

                //realizar orden de salida
                No_Salida = Orden_Salida_Negocio.Alta_Orden_Salida();

                //Imprimir orden de salida
                Imprime_Orden_Salida(No_Salida, Ds_Alm_Com_Orden_Salida_src, Formato);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Llena_Datos
        ///DESCRIPCION:             Llenar los controles con los datos de la requisicion
        ///PARAMETROS:              No_Requisicion: numero que contiene el 
        ///                         numero de requisicion
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              23/Noviembre/2010 19:00
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Llena_Datos(long No_Requisicion)
        {
            //Declaracion de variables
            Cls_Alm_Com_Orden_Salida_Negocio Orden_Salida_Negocio = new Cls_Alm_Com_Orden_Salida_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Datos_Requisicion = new DataTable(); //Tabla para los datos de la requisicion

            try
            {
                //Asignar consulta
                Orden_Salida_Negocio.P_No_Requisicion = No_Requisicion;
                Dt_Datos_Requisicion = Orden_Salida_Negocio.Consulta_Requisiciones();

                //Verificar si hay datos
                if (Dt_Datos_Requisicion.Rows.Count > 0)
                {
                    //Colocar datos en los controles
                    Txt_Requisicion_ID.Text = Dt_Datos_Requisicion.Rows[0]["NO_REQUISICION"].ToString().Trim();
                    Txt_Dependencia.Text = Dt_Datos_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
                    Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);

                    //Llenar el grid con los datos de la requisicion
                    Llena_Grid_Requisiciones_Detalles(No_Requisicion, -1);
                    Div_Datos_Generales.Visible = true;
                   
                }
                else
                {
                    Lbl_Informacion.Text = "La requisición no contiene datos";
                    Mostrar_Informacion(1);
                    Div_Datos_Generales.Visible = false;
                    Btn_Nuevo.Visible = false;

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Imprime_Orden_Salida
        ///DESCRIPCION:             Generar el archivo pdf de la orden de salida
        ///PARAMETROS:              No_Salida: Cadena de texto que contiene el numero de salida
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              23/Diciembre/2010 19:00
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Imprime_Orden_Salida(long No_Salida, DataSet Ds_Alm_Com_Orden_Salida_src, String Formato)
        {
            String Ruta_Reporte_Crystal = ""; 
            String Nombre_Reporte_Generar = "";
            DataRow Renglon;                        //Renglon para el llenado de la tabla

            //Declaracion de variables
            Cls_Alm_Com_Orden_Salida_Negocio Orden_Salida_Negocio = new Cls_Alm_Com_Orden_Salida_Negocio(); //Variable para la capa de negocios
            DataSet Ds_Orden_Salida_Normal = new DataSet(); //Dataset normal para la consulta de los datos
          
            try
            {
                //Realizar la consulta de la orden de salida
                Orden_Salida_Negocio.P_No_Salida = No_Salida;
                Ds_Orden_Salida_Normal = Orden_Salida_Negocio.Imprime_Orden_Salida();

                //Importar datos al dataset archivo
                for (int Cont_Elementos = 0; Cont_Elementos < Ds_Orden_Salida_Normal.Tables[0].Rows.Count; Cont_Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Ds_Orden_Salida_Normal.Tables[0].Rows[Cont_Elementos];
                    Ds_Alm_Com_Orden_Salida_src.Tables[0].ImportRow(Renglon);
                }

                for (int Cont_Elementos = 0; Cont_Elementos < Ds_Orden_Salida_Normal.Tables[1].Rows.Count; Cont_Elementos++)
                {
                    // Instanciar renglon e importarlo
                    Renglon = Ds_Orden_Salida_Normal.Tables[1].Rows[Cont_Elementos];
                    Ds_Alm_Com_Orden_Salida_src.Tables[1].ImportRow(Renglon);
                }

                // Ruta donde se encuentra el Reporte Crystal
                Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Alm_Com_Orden_Salida.rpt";

                // Se crea el nombre del reporte
                String Nombre_Reporte = "Rpt_Orden_Salida_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

                // Se da el nombre del reporte que se va generar
                if (Formato == "PDF")
                    Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                else if (Formato == "Excel")
                    Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar



                Cls_Reportes Reportes = new Cls_Reportes();
                Reportes.Generar_Reporte(ref Ds_Alm_Com_Orden_Salida_src, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
                Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        /// *************************************************************************************
        /// NOMBRE:              Mostrar_Reporte
        /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
        /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
        ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
        /// USUARIO CREO:        Juan Alberto Hernández Negrete.
        /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
        /// USUARIO MODIFICO:    Salvador Hernández Ramírez
        /// FECHA MODIFICO:      23-Mayo-2011
        /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
        /// *************************************************************************************
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
    #endregion

    #region (Grid)
        protected void Grid_Requisiciones_Detalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Llenar el grid con la pagina
                Llena_Grid_Requisiciones_Detalles((long)Session["Requisicion_ID"], e.NewPageIndex);
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Grid_Requisiciones_Detalles_PageIndexChanging)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion

    #region (Eventos)
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Buscar_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }

        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                String ruta = "../Compras/Frm_Ope_Com_Requisiciones_Pendientes.aspx?Tipo_Requisicion=Stock&?PAGINA=" + Session["Pagina"].ToString().Trim();
                Response.Redirect(ruta);
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Salir_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }

        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                //Verificar si hay una requisicion
                if (Txt_Requisicion_ID.Text.Trim() != null && Txt_Requisicion_ID.Text.Trim() != "" && Txt_Requisicion_ID.Text.Trim() != String.Empty)
                {
                    //Verificar que existan los comentarios
                    if (Txt_Comentarios.Text.Trim() != null && Txt_Comentarios.Text.Trim() != "" && Txt_Comentarios.Text.Trim() != String.Empty)
                    {
                        Alta_Orden_Salida();
                        Limpiar_Controles();

                        Div_Datos_Generales.Visible = false;
                        Btn_Nuevo.Visible = false;
                    }
                    else
                    {
                        Mostrar_Informacion(1);
                        Lbl_Informacion.Text = "Favor de proporcionar los comentarios correspondientes";
                    }                    
                }
                else
                {
                    Mostrar_Informacion(1);
                    Lbl_Informacion.Text = "No se puede generar orden de salida si la requisicion no contiene datos";                
                }
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Btn_Salir_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion
}
