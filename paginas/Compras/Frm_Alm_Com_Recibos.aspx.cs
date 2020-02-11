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
using Presidencia.Almacen_Recibos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Compras_Frm_Alm_Com_Recibos : System.Web.UI.Page
{
    #region (variables)

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
        ///DESCRIPCION:             Colocar la pagina en un estado inicial de navegacion
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              29/Noviembre/2010 17:50
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Estado_Inicial()
        {
            //Declaracion de variables
            long No_Requisicion = 0; //Variable para el numero de requisicion

            try
            {
                Eliminar_Sesiones();
                
                //Colocar el numero de requisicion
                No_Requisicion = Convert.ToInt64(HttpUtility.HtmlDecode(Request.QueryString["No_Requisicion"]).Trim());

                //Colocarlo en variable de sesion
                Session["No_Requisicion"] = No_Requisicion;

                //Llenar el grid con los datos de la requisicion
                Llena_Grid_Requisicion_Detalles(No_Requisicion, -1);

                //Llenar los datos de la requisicion
                Llena_Datos(No_Requisicion);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Eliminar_Sesiones
        ///DESCRIPCION:             Eliminar las sesiones utilizadad en la pagina
        ///PARAMETROS:              
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              24/Noviembre/2010 11:12
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Eliminar_Sesiones()
        {
            try
            {
                Session.Remove("No_Remision");
                Session.Remove("Dt_Requisicion_Detalles");
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Llena_Grid_Requisicion_Detalles
        ///DESCRIPCION:             Llenar el grid con los detalles de la requisicion
        ///PARAMETROS:              1. No_Requisicion: Numero de la requisicion
        ///                         2. Pagina: Entero que contiene el numero de pagina en el grid
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              29/Noviembre/2010 17:45
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Llena_Grid_Requisicion_Detalles(long No_Requisicion, int Pagina)
        {
            //Declaracion de variables
            Cls_Alm_Com_Recibos_Negocio Recibo_Negocios = new Cls_Alm_Com_Recibos_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Requisicion_Detalles = new DataTable(); //Tabla para el llenado del grid

            try
            {
                //Verificar si existe la variable de sesion
                if (Session["Dt_Requisicion_Detalles"] != null)
                    Dt_Requisicion_Detalles = (DataTable)Session["Dt_Requisicion_Detalles"];
                else
                {
                    //Realizar la consulta
                    Recibo_Negocios.P_No_Requisicion = No_Requisicion;
                    Dt_Requisicion_Detalles = Recibo_Negocios.Consulta_Requisiciones_Detalles();
                }

                //Llenar el grid
                Grid_Requisiciones_Detalles.DataSource = Dt_Requisicion_Detalles;
                
                //Verificar si esta paginado
                if (Pagina > -1)
                    Grid_Requisiciones_Detalles.PageIndex = Pagina;

                Grid_Requisiciones_Detalles.DataBind();

                //Colocar tabla en variable de sesion
                Session["Dt_Requisicion_Detalles"] = Dt_Requisicion_Detalles;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Alta_Recibo
        ///DESCRIPCION:             Dar de alta el recibo de material
        ///PARAMETROS:              No_Requisicion: Numero de la requisicion
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              29/Noviembre/2010 18:45
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Alta_Recibo(long No_Requisicion)
        {
            //Declaracion de variables
            Cls_Alm_Com_Recibos_Negocio Recibo_Negocios = new Cls_Alm_Com_Recibos_Negocio(); //Variable para la capa de negocios
            String No_Recibo = String.Empty; //Variable para el numero del recibo

            try
            {
                //Asignar propiedades
                Recibo_Negocios.P_No_Requisicion = No_Requisicion;
                Recibo_Negocios.P_Usuario = Cls_Sessiones.Nombre_Empleado; 

                //Verificar la extension de los comentarios
                if (Txt_Comentarios.Text.Trim().Length > 250)
                    Recibo_Negocios.P_Comentarios = Txt_Comentarios.Text.Trim().Substring(0, 250);
                else
                    Recibo_Negocios.P_Comentarios = Txt_Comentarios.Text.Trim();

                //Dar de alta el recibo y obtener su numero
                No_Recibo = Recibo_Negocios.Alta_Recibo();

               //Imprimir el recibo
                Imprime_Recibo(No_Recibo); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCION:    Imprime_Recibo
        ///DESCRIPCION:             Imprimer el recibo de material
        ///PARAMETROS:              No_Recibo: Numero del recibo
        ///CREO:                    Noe Mosqueda Valadez
        ///FECHA_CREO:              29/Noviembre/2010 19:00
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACION:      
        ///*******************************************************************************
        private void Imprime_Recibo(String No_Recibo)
        {
            //Declaracion de variables
            Cls_Alm_Com_Recibos_Negocio Recibo_Negocio = new Cls_Alm_Com_Recibos_Negocio(); //Variable para la capa de negocios
            DataSet Ds_Recibo_Normal = new DataSet(); //Dataset normal para la consulta de los datos
            Ds_Ope_Com_Recibos Ds_Ope_Com_Recibos_src = new Ds_Ope_Com_Recibos(); //Dataset archivo para el llenado del reporte
            DataRow Renglon; //Renglon para el llenado de la tabla
            ReportDocument Rpt_Ope_Com_Recibos_src = new ReportDocument(); //Variable para el reporte de crystal
            String Ruta_Reporte = String.Empty; //Variable que contiene la ruta del reporte
            String Ruta_pdf = String.Empty; //Variable para la ruta del pdf

            try
            {
                //Realizar la consulta del recibo
                Recibo_Negocio.P_No_Recibo = No_Recibo;
                Ds_Recibo_Normal = Recibo_Negocio.Imprime_Recibo();

                //Verificar si el dataset tiene registros
                if (Ds_Recibo_Normal.Tables.Count == 2)
                {
                    //Importar datos al dataset archivo
                    for (int Cont_Elementos = 0; Cont_Elementos < Ds_Recibo_Normal.Tables[0].Rows.Count; Cont_Elementos++)
                    {
                        //Instanciar renglon e importarlo
                        Renglon = Ds_Recibo_Normal.Tables[0].Rows[Cont_Elementos];
                        Ds_Ope_Com_Recibos_src.Tables[0].ImportRow(Renglon);
                    }

                    for (int Cont_Elementos = 0; Cont_Elementos < Ds_Recibo_Normal.Tables[1].Rows.Count; Cont_Elementos++)
                    {
                        //Instanciar renglon e importarlo
                        Renglon = Ds_Recibo_Normal.Tables[1].Rows[Cont_Elementos];
                        Ds_Ope_Com_Recibos_src.Tables[1].ImportRow(Renglon);
                    }

                    //Colocar la ruta del reporte
                    Ruta_Reporte = Server.MapPath("../Rpt/Compras/Rpt_Ope_Com_Recibos.rpt");

                    //Instanciar reporte
                    Rpt_Ope_Com_Recibos_src.Load(Ruta_Reporte);

                    //Colocar fuente de datos
                    Rpt_Ope_Com_Recibos_src.SetDataSource(Ds_Ope_Com_Recibos_src);

                    //Asignar propiedades para la exportacion
                    ExportOptions exportOptions = new ExportOptions();
                    DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
                    diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Ope_Com_Recibos.pdf");
                    exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
                    exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
                    exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
                    Rpt_Ope_Com_Recibos_src.Export(exportOptions);
                    Ruta_pdf = "../../Reporte/Rpt_Ope_Com_Recibos.pdf";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_pdf + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                }
                else
                {
                    throw new Exception("La consulta no arrojo resultados");
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }

        private void Llena_Datos(long No_Requisicion)
        {
            //Declaracion de variables
            Cls_Alm_Com_Recibos_Negocio Recibo_Negocio = new Cls_Alm_Com_Recibos_Negocio(); //Variable para la capa de negocios
            DataTable Dt_Requisicion = new DataTable(); //Tabla para los datos de la requisicion

            try
            {
                //Realizar la consulta de los datos
                Recibo_Negocio.P_No_Requisicion = No_Requisicion;
                Dt_Requisicion = Recibo_Negocio.Consulta_Datos_Requisicion();

                //Verificar si la consulta arrojo datos
                if (Dt_Requisicion.Rows.Count > 0)
                {
                    //Colocar los datos en la pagina
                    Txt_No_Requisicion.Text = Dt_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
                    Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    Txt_Dependencia.Text = Dt_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
                    Txt_Area.Text = Dt_Requisicion.Rows[0]["AREA"].ToString().Trim();                   
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
        }
    #endregion

    #region (Grid)
        protected void Grid_Requisiciones_Detalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                //Llenar el grid con la pagina correspondiente
                Llena_Grid_Requisicion_Detalles(0, e.NewPageIndex);
            }
            catch (Exception ex)
            {
                Lbl_Informacion.Text = "Error: (Grid_Requisiciones_Detalles_PageIndexChanging)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion

    #region (Eventos)
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
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
                if (Txt_No_Requisicion.Text.Trim() != null && Txt_No_Requisicion.Text.Trim() != "" && Txt_No_Requisicion.Text.Trim() != String.Empty)
                {
                    //Verificar si hay comentarios
                    if (Txt_Comentarios.Text.Trim() != null && Txt_Comentarios.Text.Trim() != "" && Txt_Comentarios.Text.Trim() != String.Empty)
                        Alta_Recibo((long)Session["No_Requisicion"]);
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
                Lbl_Informacion.Text = "Error: (Btn_Nuevo_Click)" + ex.ToString();
                Mostrar_Informacion(1);
            }
        }
    #endregion
}
