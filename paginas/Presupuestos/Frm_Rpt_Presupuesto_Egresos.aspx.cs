using System;
using System.Diagnostics;
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
using System.IO;
using System.Text;
using Presidencia.DateDiff;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Grupos_Dependencias.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Reporte_Presupuesto_Egresos.Negocio;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using Excel = Microsoft.Office.Interop.Excel;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Movimiento_Presupuestal.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;

public partial class paginas_Paginas_Presupuestos_Frm_Rpt_Presupuesto_Egresos : System.Web.UI.Page
{
    #region(Variables Globales)
    String Global_Ruta_Archivo = "";
    
    #endregion

    #region(Load)
    ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo Inicial de la página.
        ///PARAMETROS:  
        ///CREO: Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO: 03/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //Refresca la sesion del usuario logeado en el sistema
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                //Valida que existe un usuario logueado en el sistema
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

                if (!IsPostBack)
                {
                    Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                    //ViewState["SortDirection"] = "ASC";
                    Configuracion_Acceso("Frm_Rpt_Presupuesto_Egresos.aspx");
                    Session["Session_Reporte_Grupo_DEpendencia"] = null;

                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Inicializa_Controles " + Ex.Message.ToString());
            }

        }
    #endregion

    #region(Metodos)
        #region(Metodos Generales)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Inicializa_Controles
        /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
        ///               realizar diferentes operaciones
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializa_Controles()
        {
            try
            {
                Cargar_Combo_Año();
                Limpiar_Controles(); //Limpia los controles del formulario
            }
            catch (Exception ex)
            {
                throw new Exception("Inicializa_Controles " + ex.Message.ToString());
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Limpiar_Controles
        /// DESCRIPCION : Limpia los controles que se encuentran en la forma
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpiar_Controles()
        {
            try
            {
                DateTime Tiempo_Ahora = DateTime.Now;
                String Año ="" +Tiempo_Ahora.Year;
                //Cmb_Anio.SelectedIndex=Cmb_Anio.Items.IndexOf(Cmb_Anio.Items.FindByValue(Año));
                Cmb_Anio.SelectedIndex = 0;
                Lbl_Mensaje_Error.Text = "";
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
            }
            catch (Exception ex)
            {
                throw new Exception("Limpia_Controles " + ex.Message.ToString());
            }
        }
        #endregion

        #region(Control Acceso Pagina)
        /// ******************************************************************************
        /// NOMBRE: Configuracion_Acceso
        /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
        /// PARÁMETROS  :
        /// USUARIO CREÓ: Hugo Enrique Ramírez Aguilera
        /// FECHA CREÓ  : 03/Diciembre/2011 
        /// USUARIO MODIFICO  :
        /// FECHA MODIFICO    :
        /// CAUSA MODIFICACIÓN:
        /// ******************************************************************************
        protected void Configuracion_Acceso(String URL_Pagina)
        {
            List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.
                
                Botones.Add(Btn_Salir);
                //Botones.Add(Btn_Reporte_Grupo_Dependencia);

                if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
                {
                    if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                    {
                        //Consultamos el menu de la página.
                        Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                        if (Dr_Menus.Length > 0)
                        {
                            //Validamos que el menu consultado corresponda a la página a validar.
                            if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                            {
                                Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                            }
                            else
                            {
                                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                            }
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Es_Numero
        /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
        /// PARÁMETROS  : Cadena.- El dato a evaluar si es numerico.
        /// CREO        : Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 03/Diciembre/2011 
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

        #region(Consultas)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Fuente_Financiamiento
        /// DESCRIPCION : Consulta la clave de fuente de financiamiento
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 08-Diciembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private String Consultar_Fuente_Financiamiento(String Clave_Fuente_Financiamiento)
        {
            Cls_Cat_SAP_Fuente_Financiamiento_Negocio Rs_Consulta = new Cls_Cat_SAP_Fuente_Financiamiento_Negocio();
            DataTable Dt_Consulta = new DataTable();
            String Calve_Id = "";
            try
            {
                Rs_Consulta.P_Fuente_Financiamiento_ID = Clave_Fuente_Financiamiento;
                Dt_Consulta = Rs_Consulta.Consulta_Fuente_Financiamiento();//se llena el datatable con la consulta   
                foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                {
                    Calve_Id = Registro[Cat_SAP_Fuente_Financiamiento.Campo_Clave].ToString().ToUpper();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
            }
            return Calve_Id;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Programa
        /// DESCRIPCION : Consulta la clave del programa
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 08-Diciembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private String Consultar_Programa(String Clave_Programa)
        {
            Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Consulta = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
            DataTable Dt_Consulta = new DataTable();
            String Calve_Id = "";
            try
            {
                Rs_Consulta.P_Programa = Clave_Programa;
                Dt_Consulta = Rs_Consulta.Consultar_Programa();//se llena el datatable con la consulta   
                foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                {
                    Calve_Id = Registro[Cat_Sap_Proyectos_Programas.Campo_Clave].ToString().ToUpper();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
            }
            return Calve_Id;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Partida
        /// DESCRIPCION : Consulta la clave de la partida especifica
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 08-Diciembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private String Consultar_Partida(String Clave_Partida)
        {
            Cls_Cat_Com_Partidas_Negocio Rs_Consulta = new Cls_Cat_Com_Partidas_Negocio();
            DataTable Dt_Consulta = new DataTable();
            String Calve_Id = "";
            try
            {
                Rs_Consulta.P_Partida_ID = Clave_Partida;
                Dt_Consulta = Rs_Consulta.Consulta_Datos_Partidas();//se llena el datatable con la consulta   
                foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                {
                    Calve_Id = Registro[Cat_Com_Partidas.Campo_Clave].ToString().ToUpper();

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
            }
            return Calve_Id;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Partida_Nombre
        /// DESCRIPCION : Consulta el nombre de la partida especifica
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 08-Diciembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private String Consultar_Partida_Nombre(String Clave_Partida)
        {
            Cls_Cat_Com_Partidas_Negocio Rs_Consulta = new Cls_Cat_Com_Partidas_Negocio();
            DataTable Dt_Consulta = new DataTable();
            String Calve_Id = "";
            try
            {
                Rs_Consulta.P_Partida_ID = Clave_Partida;
                Dt_Consulta = Rs_Consulta.Consulta_Datos_Partidas();//se llena el datatable con la consulta   
                foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                {
                    Calve_Id = Registro[Cat_Com_Partidas.Campo_Nombre].ToString().ToUpper();

                }

            }
            catch (Exception ex)
            {
                throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
            }
            return Calve_Id;
        }

        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Dependencia
        /// DESCRIPCION : Consulta el id de fuente de financiamiento
        /// PARAMETROS  : 
        /// CREO        : Hugo Enrique Ramirez Aguilera
        /// FECHA_CREO  : 17-Noviembre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private String Consultar_Dependencia(String Clave_Dependencia)
        {
            Cls_Cat_Dependencias_Negocio Rs_Consulta = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Consulta = new DataTable();
            String Calve_Id = "";
            try
            {
                Rs_Consulta.P_Dependencia_ID = Clave_Dependencia;
                Dt_Consulta = Rs_Consulta.Consulta_Dependencias();//se llena el datatable con la consulta   
                foreach (DataRow Registro in Dt_Consulta.Rows)//se llena el registro para pasar el valor a la caja de texto
                {
                    Calve_Id = Registro[Cat_Dependencias.Campo_Clave].ToString().ToUpper();
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Movimiento Presupuestal" + ex.Message.ToString(), ex);
            }
            return Calve_Id;
        }
        #endregion

        #region(Reporte)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Generar_Rpt_Presupuesto_Excel
        /// DESCRIPCION :   Se encarga de generar el archivo de excel pasndole los paramentros
        ///                 al documento
        /// PARAMETROS  :   Dt_Reporte.- Es la consulta con la informacion de los grupos de 
        ///                 dependencias que esten relacionados con las dependencias y 
        ///                 su presupuesto
        /// CREO        :   Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  :   05/Diciembre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public void Generar_Rpt_Presupuesto_Excel(System.Data.DataTable Dt_Reporte)
        {
            Cls_Rpt_Presupuesto_Egresos_Negocio Rs_Individual_Dependencia = new Cls_Rpt_Presupuesto_Egresos_Negocio();//instancia con capa de datos 
            Cls_Cat_Dependencias_Negocio Rs_Dependencias = new Cls_Cat_Dependencias_Negocio();
            Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Programa = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
            Cls_Cat_Com_Partidas_Negocio Rs_Capitulos = new Cls_Cat_Com_Partidas_Negocio(); //Variable de conexión hacia la capa de Negocios

            String Consulta = "";
            String Grupo_Dependencia_ID = "";
            String Dependencia_Concepto = "";
            String Dependencia_Clave = "";
            String Monto_Aprobado = "";
            String Formato_Importe = "";
            String Capitulo_Concepto = "";
            String Fuente_Financiamiento = "";
            String Programa = "";
            String Partida = "";
            String Partida_Nombre = "";
            double Importe = 0;
            double Suma_Importe = 0;
            double Suma_Partidas = 0;
            
            String Nombre_Archivo = "Rpt_Presupuestos_Egreso_" +Cmb_Anio.SelectedValue + ".xls";
            String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
            Global_Ruta_Archivo = Ruta_Archivo;
            try
            {
                //Creamos el libro de Excel.
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
                //propiedades del libro
                Libro.Properties.Title = "Reporte_Presupuesto";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "Presidencia_RH";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Dependencia");
                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja2 = Libro.Worksheets.Add("Unidad Responsable");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja3 = Libro.Worksheets.Add("Programa");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja4 = Libro.Worksheets.Add("Partida");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja5 = Libro.Worksheets.Add("Analitico");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo cabecera 2 para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera2 = Libro.Styles.Add("HeaderStyle2");
                //Creamos el estilo cabecera 3 para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera3 = Libro.Styles.Add("HeaderStyle3"); 
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
                //Creamos el estilo contenido del presupuesto para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Presupuesto = Libro.Styles.Add("Presupuesto");
                //Creamos el estilo contenido del presupuesto total para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Presupuesto_Total = Libro.Styles.Add("Presupuesto_Total");
                //Creamos el estilo contenido del concepto para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Concepto = Libro.Styles.Add("Concepto");
                //Creamos el estilo contenido del concepto para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Partida_Total = Libro.Styles.Add("Partida");

                //***************************************inicio de los estilos***********************************************************
                //estilo para la cabecera
                Estilo_Cabecera.Font.FontName = "Tahoma";
                Estilo_Cabecera.Font.Size = 12;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Cabecera.Font.Color = "#FFFFFF";
                Estilo_Cabecera.Interior.Color = "Gray";
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para la cabecera2
                Estilo_Cabecera2.Font.FontName = "Tahoma";
                Estilo_Cabecera2.Font.Size = 10;
                Estilo_Cabecera2.Font.Bold = true;
                Estilo_Cabecera2.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera2.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Cabecera2.Font.Color = "#FFFFFF";
                Estilo_Cabecera2.Interior.Color = "DarkGray";
                Estilo_Cabecera2.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para la cabecera3
                Estilo_Cabecera3.Font.FontName = "Tahoma";
                Estilo_Cabecera3.Font.Size = 10;
                Estilo_Cabecera3.Font.Bold = true;
                Estilo_Cabecera3.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera3.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Cabecera3.Font.Color = "#000000";
                Estilo_Cabecera3.Interior.Color = "white";
                Estilo_Cabecera3.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para el contenido
                Estilo_Contenido.Font.FontName = "Tahoma";
                Estilo_Contenido.Font.Size = 9;
                Estilo_Contenido.Font.Bold = false;
                Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Contenido.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Contenido.Font.Color = "#000000";
                Estilo_Contenido.Interior.Color = "White";
                Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para el Concepto
                Estilo_Concepto.Font.FontName = "Tahoma";
                Estilo_Concepto.Font.Size = 9;
                Estilo_Concepto.Font.Bold = false;
                Estilo_Concepto.Alignment.Horizontal = StyleHorizontalAlignment.Left;
                Estilo_Concepto.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
                Estilo_Concepto.Font.Color = "#000000";
                Estilo_Concepto.Interior.Color = "White";
                Estilo_Concepto.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Concepto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Concepto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Concepto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Concepto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para el presupuesto (importe)
                Estilo_Presupuesto.Font.FontName = "Tahoma";
                Estilo_Presupuesto.Font.Size = 9;
                Estilo_Presupuesto.Font.Bold = false;
                Estilo_Presupuesto.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                Estilo_Presupuesto.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Presupuesto.Font.Color = "#000000";
                Estilo_Presupuesto.Interior.Color = "White";
                Estilo_Presupuesto.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Presupuesto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Presupuesto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Presupuesto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Presupuesto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //estilo para el presupuesto (Total del importe)
                Estilo_Presupuesto_Total.Font.FontName = "Tahoma";
                Estilo_Presupuesto_Total.Font.Size = 9;
                Estilo_Presupuesto_Total.Font.Bold = true;
                Estilo_Presupuesto_Total.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                Estilo_Presupuesto_Total.Font.Color = "#000000";
                Estilo_Presupuesto_Total.Interior.Color = "Yellow";
                Estilo_Presupuesto_Total.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Presupuesto_Total.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Presupuesto_Total.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Presupuesto_Total.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Presupuesto_Total.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //para partida totales de todos los capitulos
                Estilo_Partida_Total.Font.FontName = "Tahoma";
                Estilo_Partida_Total.Font.Size = 9;
                Estilo_Partida_Total.Font.Bold = true;
                Estilo_Partida_Total.Alignment.Horizontal = StyleHorizontalAlignment.Right;
                Estilo_Partida_Total.Font.Color = "#000000";
                Estilo_Partida_Total.Interior.Color = "LightGreen";
                Estilo_Partida_Total.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Partida_Total.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Partida_Total.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Partida_Total.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Partida_Total.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

                //***************************************fin de los estilos*************************************************************
                
                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(90));//Dependencia.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Concepto.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Presupuesto.

                //se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                WorksheetCell Celda = Renglon.Cells.Add("GOBIERNO MUNICIPAL DE IRAPUATO, GUANAJUATO");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("TESORERIA MUNICIPAL");
                Celda.MergeAcross = 2; // Merge two cells together
                
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("COORDINACION GENERAL DE FINANZAS ");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //encabezado3
                Renglon = Hoja.Table.Rows.Add();//espacio entre el encabezado y el contenido
                Renglon = Hoja.Table.Rows.Add();
                String Texto_Año = "PRESUPUESTO DE EGRESOS EJERCICIO FISCAL " + Cmb_Anio.SelectedValue + " CLASIFICADO POR DEPENDENCIA";
                Celda = Renglon.Cells.Add(Texto_Año);
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle3";
                //(uso del encabezado2) para los titulos del detalle o cuerpo principal 
                Renglon = Hoja.Table.Rows.Add();
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("DEPENDENCIA", "HeaderStyle2"));
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CONCEPTO", "HeaderStyle2"));
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PRESUPUESTO  DE EGRESOS  " + Cmb_Anio.SelectedValue, "HeaderStyle2"));

                /*pasos a realizar: se buscan los grupos de dependencia,
                se continua buscando las dependencias a las que pertenecen al grupo de dependendcia,
                las dependencias se les busca si tienen algun presupuesto y se le suma hasta tener 
                el presupuesto de egresos del grupo de dependencias al que pertenecen las dependencias buscadas*/
                foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                {
                    Renglon = Hoja.Table.Rows.Add();
                    Grupo_Dependencia_ID = (Renglon_Reporte[Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID].ToString());

                    //realizo la busqueda individual de los que contengan el grupo dependencia
                    Rs_Individual_Dependencia.P_Grupo_Dependencia_ID = Grupo_Dependencia_ID;
                    System.Data.DataTable Dt_Dependencias = Rs_Individual_Dependencia.Consultar_Dependencias();
                    foreach (DataRow Renglon_Individual_Dependancia in Dt_Dependencias.Rows)
                    {
                        //se realiza la consulta del presupuesto que contengan el id dependencia buscado
                        Rs_Individual_Dependencia.P_Dependencia_ID = (Renglon_Individual_Dependancia[Cat_Dependencias.Campo_Dependencia_ID].ToString());
                        Rs_Individual_Dependencia.P_Anio = Cmb_Anio.SelectedValue;
                        System.Data.DataTable Dt_Presupuesto = Rs_Individual_Dependencia.Consultar_Presupuesto_Dependencias();
                        foreach (DataRow Renglon_Presupuesto in Dt_Presupuesto.Rows)
                        {
                            //se realiza la suma de lo aprobado de cada dependencia
                            Monto_Aprobado = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado].ToString());
                            Importe += Convert.ToDouble(Monto_Aprobado);
                        }
                    } 

                    //para la informacion del cuerpo principal
                    Dependencia_Concepto = (Renglon_Reporte[Cat_Grupos_Dependencias.Campo_Nombre].ToString().ToUpper());
                    Dependencia_Clave = (Renglon_Reporte[Cat_Grupos_Dependencias.Campo_Clave].ToString().ToUpper());
                    //para agragar la clave del grupo dependencia
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dependencia_Clave, "BodyStyle"));
                    //para agregar el nombre de la clave en la celda
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dependencia_Concepto, "Concepto"));
                    //para el importe se le cambia el formato a un numerico
                    Formato_Importe = string.Format("{0:n}", Importe);
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                    Suma_Importe += Importe;
                    Importe = 0;
                }

                //(Totales) para los resultados finales 
                Renglon = Hoja.Table.Rows.Add();
                Celda = Renglon.Cells.Add("TOTAL PRESUPUESTO DE EGRESOS " + Cmb_Anio.SelectedValue + " ");
                Celda.MergeAcross = 1; // Merge two cells together
                Celda.StyleID = "Presupuesto_Total";
                Formato_Importe = string.Format("{0:n}", Suma_Importe);
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto_Total"));//aqui va el monto total

                //*************************************** unidad responsable hoja 2*************************************************************
                //inicializan los valores
                Suma_Importe = 0;
                Importe = 0;

                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon2 = Hoja2.Table.Rows.Add();

                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja2.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(90));//Dependencia.
                Hoja2.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Concepto.
                Hoja2.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Presupuesto.

               //se llena el encabezado principal
                Renglon2 = Hoja2.Table.Rows.Add();
                Celda = Renglon2.Cells.Add("GOBIERNO MUNICIPAL DE IRAPUATO, GUANAJUATO");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon2 = Hoja2.Table.Rows.Add();
                Celda = Renglon2.Cells.Add("TESORERIA MUNICIPAL");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon2 = Hoja2.Table.Rows.Add();
                Celda = Renglon2.Cells.Add("COORDINACION GENERAL DE FINANZAS");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //encabezado3
                Renglon2 = Hoja2.Table.Rows.Add();//espacio entre el encabezado y el contenido
                Renglon2 = Hoja2.Table.Rows.Add();
                Texto_Año = "PRESUPUESTO DE EGRESOS EJERCICIO FISCAL " + Cmb_Anio.SelectedValue + " CLASIFICADO POR UNIDAD RESPONSABLE";
                Celda = Renglon2.Cells.Add(Texto_Año);
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle3";
                //(uso del encabezado2) para los titulos del detalle o cuerpo principal 
                Renglon2 = Hoja2.Table.Rows.Add();
                Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("UNIDAD RESPONSABLE", "HeaderStyle2"));
                Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CONCEPTO", "HeaderStyle2"));
                Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PRESUPUESTO  DE EGRESOS  " + Cmb_Anio.SelectedValue, "HeaderStyle2"));


                System.Data.DataTable Dt_Unidad_Responsable = Rs_Dependencias.Consulta_Dependencias();

                foreach (DataRow Renglon_Reporte in Dt_Unidad_Responsable.Rows)
                {
                    Renglon2 = Hoja2.Table.Rows.Add();
                    //para la informacion del cuerpo principal
                    Dependencia_Concepto = (Renglon_Reporte[Cat_Dependencias.Campo_Nombre].ToString().ToUpper());
                    Dependencia_Clave = (Renglon_Reporte[Cat_Dependencias.Campo_Clave].ToString().ToUpper());
                    //para agragar la clave del grupo dependencia
                    Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dependencia_Clave, "BodyStyle"));
                    //para agregar el nombre de la clave en la celda
                    Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dependencia_Concepto, "Concepto"));

                    Rs_Individual_Dependencia.P_Dependencia_ID = (Renglon_Reporte[Cat_Dependencias.Campo_Dependencia_ID].ToString());
                    Rs_Individual_Dependencia.P_Anio = Cmb_Anio.SelectedValue;
                    System.Data.DataTable Dt_Presupuesto = Rs_Individual_Dependencia.Consultar_Presupuesto_Dependencias();

                    if (Dt_Presupuesto.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon_Presupuesto in Dt_Presupuesto.Rows)
                        {
                            //se realiza la suma de lo aprobado de cada dependencia
                            Monto_Aprobado = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado].ToString());
                            Importe += Convert.ToDouble(Monto_Aprobado);
                        }
                    }
                    if (Importe == 0)
                    {
                        //para el importe se le cambia el formato a un numerico
                        Formato_Importe = string.Format("{0:n}", Importe);
                        Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                        Suma_Importe += Importe;
                        Importe = 0;
                    }
                    else
                    {
                        //para el importe se le cambia el formato a un numerico
                        Formato_Importe = string.Format("{0:n}", Importe);
                        Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                        Suma_Importe += Importe;
                        Importe = 0;
                    }
                }

                //(Totales) para los resultados finales 
                Renglon2 = Hoja2.Table.Rows.Add();
                Celda = Renglon2.Cells.Add("TOTAL PRESUPUESTO DE EGRESOS " + Cmb_Anio.SelectedValue + " ");
                Celda.MergeAcross = 1; // Merge two cells together
                Celda.StyleID = "Presupuesto_Total";
                Formato_Importe = string.Format("{0:n}", Suma_Importe);
                Renglon2.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto_Total"));//aqui va el monto total
                //********************************** fin de unidad responsable hoja 2*************************************************

                //********************************** para  Programa hoja 3************************************************************
                //inicializan los valores
                Suma_Importe = 0;
                Importe = 0;
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon3 = Hoja3.Table.Rows.Add();
                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja3.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(90));//Dependencia.
                Hoja3.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Concepto.
                Hoja3.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Presupuesto.

                //se llena el encabezado principal
                Renglon3 = Hoja3.Table.Rows.Add();
                Celda = Renglon3.Cells.Add("GOBIERNO MUNICIPAL DE IRAPUATO, GUANAJUATO");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon3 = Hoja3.Table.Rows.Add();
                Celda = Renglon3.Cells.Add("TESORERIA MUNICIPAL");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon3 = Hoja3.Table.Rows.Add();
                Celda = Renglon3.Cells.Add("COORDINACION GENERAL DE FINANZAS");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //encabezado3
                Renglon3 = Hoja3.Table.Rows.Add();//espacio entre el encabezado y el contenido
                Renglon3 = Hoja3.Table.Rows.Add();
                Texto_Año = "PRESUPUESTO DE EGRESOS EJERCICIO FISCAL " + Cmb_Anio.SelectedValue + " CLASIFICADO POR PROGRAMA";
                Celda = Renglon3.Cells.Add(Texto_Año);
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle3";
                //(uso del encabezado2) para los titulos del detalle o cuerpo principal 
                Renglon3 = Hoja3.Table.Rows.Add();
                Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PROGRAMA", "HeaderStyle2"));
                Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CONCEPTO", "HeaderStyle2"));
                Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PRESUPUESTO  DE EGRESOS  " + Cmb_Anio.SelectedValue, "HeaderStyle2"));

                System.Data.DataTable Dt_Programa = Rs_Programa.Consultar_Programa();

                foreach (DataRow Renglon_Reporte in Dt_Programa.Rows)
                {
                    Renglon3 = Hoja3.Table.Rows.Add();
                    //para la informacion del cuerpo principal
                    String Programa_Concepto = (Renglon_Reporte[Cat_Sap_Proyectos_Programas.Campo_Descripcion].ToString().ToUpper());
                    String Programa_Clave = (Renglon_Reporte[Cat_Dependencias.Campo_Clave].ToString().ToUpper());
                    //para agragar la clave del grupo dependencia
                    Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Programa_Clave, "BodyStyle"));
                    //para agregar el nombre de la clave en la celda
                    Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Programa_Concepto, "Concepto"));

                    Rs_Individual_Dependencia.P_Programa_ID = (Renglon_Reporte[Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id].ToString());
                    Rs_Individual_Dependencia.P_Anio = Cmb_Anio.SelectedValue;
                    System.Data.DataTable Dt_Presupuesto = Rs_Individual_Dependencia.Consultar_Presupuesto_Programa();


                    if (Dt_Presupuesto.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon_Presupuesto in Dt_Presupuesto.Rows)
                        {
                            //se realiza la suma de lo aprobado de cada dependencia
                            Monto_Aprobado = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado].ToString());
                            Importe += Convert.ToDouble(Monto_Aprobado);
                        }
                    }
                    if (Importe == 0)
                    {
                        //para el importe se le cambia el formato a un numerico
                        Formato_Importe = string.Format("{0:n}", Importe);
                        Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                        Suma_Importe += Importe;
                        Importe = 0;
                    }
                    else
                    {
                        //para el importe se le cambia el formato a un numerico
                        Formato_Importe = string.Format("{0:n}", Importe);
                        Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                        Suma_Importe += Importe;
                        Importe = 0;
                    }

                }
                //(Totales) para los resultados finales 
                Renglon3 = Hoja3.Table.Rows.Add();
                Celda = Renglon3.Cells.Add("TOTAL PRESUPUESTO DE EGRESOS " + Cmb_Anio.SelectedValue + " ");
                Celda.MergeAcross = 1; // Merge two cells together
                Celda.StyleID = "Presupuesto_Total";
                Formato_Importe = string.Format("{0:n}", Suma_Importe);
                Renglon3.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe , "Presupuesto_Total"));
                //********************************** fin de Programa hoja 3*************************************************


                //********************************** inicio de partida hoja 4*************************************************
                //inicializan los valores
                Suma_Importe = 0;
                Importe = 0;
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon4 = Hoja4.Table.Rows.Add();
                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja4.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(90));//Dependencia.
                Hoja4.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(350));//Concepto.
                Hoja4.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Presupuesto.

                //se llena el encabezado principal
                Renglon4 = Hoja4.Table.Rows.Add();
                Celda = Renglon4.Cells.Add("GOBIERNO MUNICIPAL DE IRAPUATO, GUANAJUATO");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon4 = Hoja4.Table.Rows.Add();
                Celda = Renglon4.Cells.Add("TESORERIA MUNICIPAL");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon4 = Hoja4.Table.Rows.Add();
                Celda = Renglon4.Cells.Add("COORDINACION GENERAL DE FINANZAS");
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //encabezado3
                Renglon4 = Hoja4.Table.Rows.Add();//espacio entre el encabezado y el contenido
                Renglon4 = Hoja4.Table.Rows.Add();
                Texto_Año = "PRESUPUESTO DE EGRESOS EJERCICIO FISCAL " + Cmb_Anio.SelectedValue + " CLASIFICADO POR PARTIDA";
                Celda = Renglon4.Cells.Add(Texto_Año);
                Celda.MergeAcross = 2; // Merge two cells together
                Celda.StyleID = "HeaderStyle3";
                //(uso del encabezado2) para los titulos del detalle o cuerpo principal 
                Renglon4 = Hoja4.Table.Rows.Add();
                Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PARTIDA", "HeaderStyle2"));
                Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CONCEPTO", "HeaderStyle2"));
                Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PRESUPUESTO  DE EGRESOS  " + Cmb_Anio.SelectedValue, "HeaderStyle2"));

                //CARGAR LOS CAPITULOS
                System.Data.DataTable Dt_Capitulos = Rs_Capitulos.Consulta_Capitulos();
                Cls_Ope_Psp_Movimiento_Presupuestal_Negocio Rs_Partida = new Cls_Ope_Psp_Movimiento_Presupuestal_Negocio();
                //capitulos
                Renglon4 = Hoja4.Table.Rows.Add();
                int Contador_Renglon = 0;
                foreach (DataRow Renglon_Reporte in Dt_Capitulos.Rows)
                {
                    Contador_Renglon++;
                    //se buscan las partidas y se obtiene el concepto para asignarlo al encabezado
                    String Capitulo_Clave = (Renglon_Reporte[Cat_SAP_Capitulos.Campo_Clave].ToString().ToUpper());
                    Capitulo_Concepto = (Renglon_Reporte[Cat_SAP_Capitulos.Campo_Descripcion].ToString().ToUpper());
                    Capitulo_Clave = Capitulo_Clave.Substring(0, 1);
                    Rs_Partida.P_Partida = Capitulo_Clave;
                    System.Data.DataTable Dt_Partida = Rs_Partida.Consulta_Datos_Partidas();
                   
                    //partida
                    if (Dt_Partida.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon_Partida in Dt_Partida.Rows)
                        {
                            String Partida_Concepto = (Renglon_Partida[Cat_Com_Partidas.Campo_Nombre].ToString().ToUpper());
                            String Partida_Clave = (Renglon_Partida[Cat_Com_Partidas.Campo_Clave].ToString().ToUpper());
                            //para agragar la clave del grupo dependencia
                            Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Partida_Clave, "BodyStyle"));
                            //para agregar el nombre de la clave en la celda
                            Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Partida_Concepto, "Concepto"));

                            Rs_Individual_Dependencia.P_Partida_ID = (Renglon_Partida[Cat_Com_Partidas.Campo_Partida_ID].ToString().ToUpper());
                            Rs_Individual_Dependencia.P_Anio = Cmb_Anio.SelectedValue;
                            System.Data.DataTable Dt_Presupuesto = Rs_Individual_Dependencia.Consultar_Presupuesto_Partida();

                            //presupuesto
                            if (Dt_Presupuesto.Rows.Count > 0)
                            {
                                foreach (DataRow Renglon_Presupuesto in Dt_Presupuesto.Rows)
                                {
                                    //se realiza la suma de lo aprobado de cada dependencia
                                    Monto_Aprobado = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado].ToString());
                                    Importe += Convert.ToDouble(Monto_Aprobado);
                                }
                            }
                            if (Importe == 0)
                            {
                                //para el importe se le cambia el formato a un numerico
                                Formato_Importe = string.Format("{0:n}", Importe);
                                Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                                Suma_Importe += Importe;//se usa para el total de un capitulo por individual
                                Importe = 0;
                                Renglon4 = Hoja4.Table.Rows.Add();
                            }
                            else
                            {
                                //para el importe se le cambia el formato a un numerico
                                Formato_Importe = string.Format("{0:n}", Importe);
                                //para el presupuesto
                                Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                                Suma_Importe += Importe;//se usa para el total de un capitulo por individual
                                Suma_Partidas += Suma_Importe;//esta variable se usa para sumar los totales del total de cada capitulo
                                Importe = 0;
                                Renglon4 = Hoja4.Table.Rows.Add();
                            }
                        }

                    }

                    
                    //(Totales) para los resultados finales 
                    Celda = Renglon4.Cells.Add("TOTAL " + Capitulo_Concepto +" ");
                    Celda.MergeAcross = 1; // Merge two cells together
                    Celda.StyleID = "Presupuesto_Total";
                    Formato_Importe = string.Format("{0:n}", Suma_Importe);
                    Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto_Total"));//aqui va el monto total

                    if (Contador_Renglon == Dt_Capitulos.Rows.Count)
                    {
                        Renglon4 = Hoja4.Table.Rows.Add();
                    }
                    else
                    {
                        Renglon4 = Hoja4.Table.Rows.Add(); 
                        Renglon4 = Hoja4.Table.Rows.Add();
                    }
                    Suma_Importe = 0;
                    Importe = 0;
                    
                }
                Celda = Renglon4.Cells.Add("TOTAL PRESUPUESTO DE EGRESOS " + Cmb_Anio.SelectedValue + " ");
                Celda.MergeAcross = 1; // Merge two cells together
                Celda.StyleID = "Partida";
                Formato_Importe = string.Format("{0:n}", Suma_Partidas);
                Renglon4.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Partida"));
                //********************************** fin de Partida hoja 4*************************************************

                //********************************** iniciio de Analitico hoja 5*************************************************
                //inicializan los valores
                Suma_Importe = 0;
                Importe = 0;
                Suma_Partidas = 0;
                Contador_Renglon = 0;

                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon5 = Hoja5.Table.Rows.Add();
                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja5.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//fuente de financiamiento 1
                Hoja5.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//Programa 2.
                Hoja5.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//Unidad Responsable 3.
                Hoja5.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));//Partida 4.
                Hoja5.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(350));//Concepto 5.
                Hoja5.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Presupuesto 6.

                //se llena el encabezado principal
                Renglon5 = Hoja5.Table.Rows.Add();
                Celda = Renglon5.Cells.Add("GOBIERNO MUNICIPAL DE IRAPUATO, GUANAJUATO");
                Celda.MergeAcross = 5; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon5 = Hoja5.Table.Rows.Add();
                Celda = Renglon5.Cells.Add("TESORERIA MUNICIPAL");
                Celda.MergeAcross = 5; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //se llena el encabezado principal
                Renglon5 = Hoja5.Table.Rows.Add();
                Celda = Renglon5.Cells.Add("COORDINACION GENERAL DE FINANZAS");
                Celda.MergeAcross = 5; // Merge two cells together
                Celda.StyleID = "HeaderStyle";
                //encabezado3
                Renglon5 = Hoja5.Table.Rows.Add();//espacio entre el encabezado y el contenido
                Renglon5 = Hoja5.Table.Rows.Add();
                Texto_Año = "PRESUPUESTO DE EGRESOS EJERCICIO FISCAL " + Cmb_Anio.SelectedValue + " CLASIFICADO POR ANALITICA";
                Celda = Renglon5.Cells.Add(Texto_Año);
                Celda.MergeAcross = 5; // Merge two cells together
                Celda.StyleID = "HeaderStyle3";
                //para el codigo programatico
                Renglon5 = Hoja5.Table.Rows.Add();
                Celda = Renglon5.Cells.Add("CODIGO PROGRAMATICO");
                Celda.MergeAcross = 3; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";
                //para concepto
                Celda = Renglon5.Cells.Add("CONCEPTO");
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";
                //para el presupuesto
                Celda = Renglon5.Cells.Add("PRESUPUESTO  DE EGRESOS  " + Cmb_Anio.SelectedValue);
                Celda.MergeDown = 1; // Merge two cells together
                Celda.StyleID = "HeaderStyle2";

                //(uso del encabezado2) para los titulos del detalle o cuerpo principal 
                Renglon5 = Hoja5.Table.Rows.Add();
                Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("F.F.", "HeaderStyle2"));
                Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PROGR", "HeaderStyle2"));
                Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("U.R", "HeaderStyle2"));
                Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PARTIDA", "HeaderStyle2"));


                //busco las unidades responsables
                Dt_Programa = Rs_Programa.Consultar_Programa();
                
                foreach (DataRow Renglon_Reporte in Dt_Programa.Rows)
                {
                    //para programa
                    String Programa_Nombre = (Renglon_Reporte[Cat_Sap_Proyectos_Programas.Campo_Descripcion].ToString().ToUpper());
                    Programa = (Renglon_Reporte[Cat_Sap_Proyectos_Programas.Campo_Clave].ToString().ToUpper());
                    //para el presupuesto
                    Rs_Individual_Dependencia.P_Programa_ID = (Renglon_Reporte[Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id].ToString());
                    Rs_Individual_Dependencia.P_Anio = Cmb_Anio.SelectedValue;
                    System.Data.DataTable Dt_Presupuesto = Rs_Individual_Dependencia.Consultar_Presupuesto_Programa();

                    if (Dt_Presupuesto.Rows.Count > 0)
                    {
                        foreach (DataRow Renglon_Presupuesto in Dt_Presupuesto.Rows)
                        {
                            Contador_Renglon++;
                            Renglon5 = Hoja5.Table.Rows.Add();
                            //para la fuente de financiamiento  
                            Consulta = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Fte_Financiamiento_ID].ToString());
                            Fuente_Financiamiento = Consultar_Fuente_Financiamiento(Consulta);
                            //para dependencia
                            Consulta = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Dependencia_ID].ToString());
                            Dependencia_Clave = Consultar_Dependencia(Consulta);
                            //para la partida
                            Consulta = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Partida_ID].ToString());
                            Partida = Consultar_Partida(Consulta);
                            Partida_Nombre = Consultar_Partida_Nombre(Consulta);
                            //se realiza la suma de lo aprobado de cada dependencia
                            Monto_Aprobado = (Renglon_Presupuesto[Ope_Psp_Presupuesto_Aprobado.Campo_Aprobado].ToString());
                            Importe = Convert.ToDouble(Monto_Aprobado);

                            //para agragar la clave de la fuente de financiamiento
                            Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fuente_Financiamiento, "BodyStyle"));
                            //para agragar la clave del programa
                            Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Programa, "BodyStyle"));
                            //para agragar la clave de la unidad responsable
                            Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dependencia_Clave, "BodyStyle"));
                            //para agragar la clave de la partida
                            Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Partida, "BodyStyle"));
                            //para agregar el nombre de la clave de la partida 
                            Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Partida_Nombre, "Concepto"));
                            //para el formato
                            Formato_Importe = string.Format("{0:n}", Importe);
                            Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto"));
                            Suma_Importe += Importe;//se usa para el total de un capitulo por individual
                            Importe = 0;
                        }
                        //(Totales) para los resultados finales 
                        Renglon5 = Hoja5.Table.Rows.Add();
                        Celda = Renglon5.Cells.Add("TOTAL " + Programa_Nombre +" ");
                        Celda.MergeAcross = 4; // Merge two cells together
                        Celda.StyleID = "Presupuesto_Total";
                        Formato_Importe = string.Format("{0:n}", Suma_Importe);
                        Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Presupuesto_Total"));//aqui va el monto total
                        Suma_Partidas += Suma_Importe;
                        Suma_Importe = 0;
                        Renglon5 = Hoja5.Table.Rows.Add();
                    }

                }
                if (Contador_Renglon == 0)
                {
                    Renglon5 = Hoja5.Table.Rows.Add();
                    Renglon5 = Hoja5.Table.Rows.Add();
                }
                Celda = Renglon5.Cells.Add("TOTAL PRESUPUESTO " + Cmb_Anio.SelectedValue + " ");
                Celda.MergeAcross = 4; // Merge two cells together
                Celda.StyleID = "Partida";
                Formato_Importe = string.Format("{0:n}", Suma_Partidas);
                Renglon5.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Formato_Importe, "Partida"));
                //********************************** fin de Analitico hoja 5*************************************************
               
               //se guarda el documento
                Libro.Save(Ruta_Archivo);
                //Process.Start(Ruta_Archivo);

                //Abre el archivo de excel
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta_Archivo);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Libro.Save(Response.OutputStream);
                Response.End();
            }
            catch (Exception Ex)
            {
                
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                //throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
            }
        }

       
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Abrir_Archivo
        ///DESCRIPCIÓN: Abre el archivo de excel
        ///PARAMETROS: String Ruta_Archivo.- Es la ruta donde se encuentra el archivo
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  06/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Abrir_Archivo(String Ruta_Archivo)
        {
            try
            {
                Response.Clear();
                Response.ClearContent();
                Response.ClearHeaders();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta_Archivo);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                
                Response.End();
            }
            catch (Exception Ex)
            {

                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                //throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
            }
        }

        #endregion
    #endregion

    #region(Eventos)
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operacion actual qye se este realizando
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  07/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
           
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
        ///DESCRIPCIÓN: Manda llamar los datos para cargarlos en el reporte de excel
        ///PARAMETROS:  
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  05/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e) 
        {
            Cls_Cat_Grupos_Dependencias_Negocio Rs_Consultar_Dependencia = new Cls_Cat_Grupos_Dependencias_Negocio();
           //limpia los mendajes mostados
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            try 
            {
                if (Cmb_Anio.SelectedIndex > 0)
                {
                    System.Data.DataTable Dt_Grupo_Dependencia = Rs_Consultar_Dependencia.Consultar_Grupos_Dependencias();
                    Generar_Rpt_Presupuesto_Excel(Dt_Grupo_Dependencia);
                 }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Año";
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                //throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Año
        ///DESCRIPCIÓN: Cargara los años de los registros que se encuentran en el presupuesto
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  07/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Cargar_Combo_Año()
        {
            Cls_Rpt_Presupuesto_Egresos_Negocio Rs_Año = new Cls_Rpt_Presupuesto_Egresos_Negocio();
            
            try
            {
                System.Data.DataTable Dt_Año = Rs_Año.Consultar_Presupuesto_Año();
                Cmb_Anio.Items.Clear();
                Cmb_Anio.DataSource = Dt_Año;
                Cmb_Anio.DataValueField = Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
                Cmb_Anio.DataTextField = Ope_Psp_Presupuesto_Aprobado.Campo_Anio;
                Cmb_Anio.DataBind();
                Cmb_Anio.Items.Insert(0, "< SELECCIONE >");
                Cmb_Anio.SelectedIndex = 0;
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                //throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
            }
        }
       
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_OnSelectedIndexChanged
        ///DESCRIPCIÓN: Sirve para mandar avisar si se a seleccionado algun año, si no
        ///             manda un aviso para indicarle al usuario que requiere escoger alguno
        ///PARAMETROS: 
        ///CREO:        Hugo Enrique Ramírez Aguilera
        ///FECHA_CREO:  05/Diciembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Cmb_Anio_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //limpia los mendajes mostados
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";

                if (Cmb_Anio.SelectedIndex > 0)
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Lbl_Mensaje_Error.Text = "";
                    //Btn_Reporte_Grupo_Dependencia.Visible = true;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione lagun elemento del combo de año";
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al generar el reporte de las Dependencias. Error: [" + Ex.Message + "]");
            }
        }

    
    #endregion

}
