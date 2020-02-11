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
using Presidencia.Incidencias_Checadas.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Calendario_Reloj_Checador.Negocio;
using Presidencia.Prestamos.Negocio;
using Presidencia.Tipos_Nominas;
using Presidencia.Generacion_Asistencias.Negocio;
using Presidencia.Generar_Faltas_Retardos_Empleados.Negocio;
using Presidencia.Empleados.Negocios;

public partial class paginas_Nomina_Frm_Ope_Nom_Incidencias_Checadas : System.Web.UI.Page
{
    #region(Load/Init)
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
                    Session["Activa"] = true;//Variable para mantener la session activa.
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
        /// FECHA_CREO  : 04-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializa_Controles()
        {
            try
            {
                Limpia_Controles();             //Limpia los controles del forma
                Consultar_Calendarios_Nomina();
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
        /// FECHA_CREO  : 04-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Txt_Total_Incidencias_Importadas.Text = "";
                Txt_Total_Empleados_Nuevos.Text = "";
                Txt_Fecha_Termino_Incidencias_Reloj_Checador.Text = "";
                Txt_Fecha_Inicio_Incidencias_Reloj_Checador.Text = "";
                
                Cmb_Calendario_Nomina.SelectedIndex = -1;
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
                Cmb_Tipo_Nomina.SelectedIndex = -1;

                Grid_Reloj_Checador.DataSource = new DataTable();
                Grid_Reloj_Checador.DataBind();
                Grid_Lista_Asistencias.DataSource = new DataTable();
                Grid_Lista_Asistencias.DataBind();

                Session.Remove("Consulta_Checadas");
                Session.Remove("Consulta_Lista_Asistencia");
            }
            catch (Exception ex)
            {
                throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Llena_Grid_Checadas
        /// DESCRIPCION : Llena el grid con las checadas de los Empleados que pertenecen
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 04-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Llena_Grid_Checadas()
        {
            DataTable Dt_Checadas; //Variable que obtendra los datos de la consulta 
            try
            {

                Grid_Reloj_Checador.Columns[0].Visible = true;
                Grid_Reloj_Checador.Columns[1].Visible = true;
                Grid_Reloj_Checador.Columns[7].Visible = true;
                Dt_Checadas = (DataTable)Session["Consulta_Checadas"];
                Grid_Reloj_Checador.DataSource = Dt_Checadas;
                Grid_Reloj_Checador.DataBind();
                Grid_Reloj_Checador.Columns[0].Visible = false;
                Grid_Reloj_Checador.Columns[1].Visible = false;
                Grid_Reloj_Checador.Columns[7].Visible = false;
                Grid_Reloj_Checador.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Asistencias " + ex.Message.ToString(), ex);
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

                Grid_Lista_Asistencias.Columns[0].Visible = true;
                Grid_Lista_Asistencias.Columns[1].Visible = true;
                Dt_Lista_Asistencia = (DataTable)Session["Consulta_Lista_Asistencia"];
                Grid_Lista_Asistencias.DataSource = Dt_Lista_Asistencia;
                Grid_Lista_Asistencias.DataBind();
                Grid_Lista_Asistencias.Columns[0].Visible = false;
                Grid_Lista_Asistencias.Columns[1].Visible = false;
                Grid_Lista_Asistencias.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Llena_Grid_Asistencias " + ex.Message.ToString(), ex);
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
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos_Asistencias_Empleado
        /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 05-Octubre-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos_Asistencias_Empleado()
        {
            Int32 index =0;
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

            index = Cmb_Calendario_Nomina.SelectedIndex;
            if (index <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione El año de la Nómina consultar los periodos nominales validos. <br>";
                Datos_Validos = false;
            }
            index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
            if (index <=0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el periodo nóminal para consultar la fecha de inicio y termino para la generación de las asistencias. <br>";
                Datos_Validos = false;
            }
            return Datos_Validos;
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
        #region (Consulta Combos)
            ///******************************************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN: LLenar_Combos
            ///
            ///DESCRIPCIÓN: Carga el combo que es pasado como parámetro con la tabla de datos que tambien es pasada como parámetro.
            ///
            ///PARÁMETROS: Combo: [DropDownList] Control donde se cargaran los datos.
            ///            Dt_Datos: Tabla que contiene el listado a mostrar en el combo.
            ///            Valor: Valor del elemento al seleccionar una opcion del combo.
            ///            Texto_Mostrar: Texto que se mostrara al usuario.
            /// 
            ///CREO: Juan Alberto Hernández Negrete
            ///FECHA_CREO: 06/Abril/2011 11:40 am 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///******************************************************************************************************************************
            private void LLenar_Combos(DropDownList Combo, DataTable Dt_Datos, String Valor, String Texto_Mostrar)
            {
                try
                {
                    if (Dt_Datos is DataTable)
                    {
                        if (Dt_Datos.Rows.Count > 0)
                        {
                            Combo.DataSource = Dt_Datos;
                            Combo.DataValueField = Valor;
                            Combo.DataTextField = Texto_Mostrar;
                            Combo.DataBind();
                            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                            Combo.SelectedIndex = -1;
                        }
                        else
                        {
                            Combo.DataSource = new DataTable();
                            Combo.DataBind();
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al cargar el combo. Error: [" + Ex.Message + "]");
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Consultar_Tipos_Nomina
            /// DESCRIPCION : Consulta los tipos de Nomina que existen
            /// CREO        : Juan Alberto Hernandez Negrete
            /// FECHA_CREO  : 05/Nov/2010
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Consultar_Tipos_Nomina_Busqueda()
            {
                DataTable Dt_Tipos_Nomina;
                Cls_Cat_Empleados_Negocios Cat_Empleados = new Cls_Cat_Empleados_Negocios();

                try
                {
                    Dt_Tipos_Nomina = Cat_Empleados.Consultar_Tipos_Nomina();
                    Cmb_Tipo_Nomina.DataSource = Dt_Tipos_Nomina;
                    Cmb_Tipo_Nomina.DataValueField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
                    Cmb_Tipo_Nomina.DataTextField = Presidencia.Constantes.Cat_Nom_Tipos_Nominas.Campo_Nomina;
                    Cmb_Tipo_Nomina.DataBind();
                    Cmb_Tipo_Nomina.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                    Cmb_Tipo_Nomina.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
                }
            }
        #endregion
        #region (Calendario Nomina)
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
                    Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_No_Nomina =Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
                    Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.Consulta_Fechas_Calendario_Reloj_Checador(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

                    //Asigna los valores obtenidos de la consulta a los controles correspondientes
                    foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                        {
                            Txt_Fecha_Inicio_Incidencias_Reloj_Checador.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()));
                            Txt_Fecha_Termino_Incidencias_Reloj_Checador.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString()));
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
                }
            }
            ///*******************************************************************************
            /// NOMBRE DE LA FUNCION: Registrar_Asistencias
            /// DESCRIPCION : Consulta en base a las fechas de inicio y termino obtenidas del
            ///               periodo nominal las asistencias que se pudieron generar de todos
            ///               los empleados que checan asistencia
            ///               Si se obtuvieron datos de registros de asistencias de la consulta
            ///               entonces estos registros los da de alta en la base de datos
            /// PARAMETROS  :
            /// CREO        : Yazmin A Delgado Gómez
            /// FECHA_CREO  : 05/Octubre/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************
            private void Registrar_Asistencias(DataTable Dt_Empleados)
            {
                Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio(); //Variable de conexión a la capa de negocios
                Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio Rs_Alta_Ope_Nom_Asistencias = new Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio(); //Vairble de conexión a la capa de Negocios
                DataTable Dt_Asistencias_Empleados; //Variable que obtendra los datos de la consulta 
                String Fecha_Hora;                  //Obtiene la fecha y la hora de la entrada del empleado

                try
                {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    if (Validar_Datos_Asistencias_Empleado())
                    {
                        //Consulta las asistencias de los empleados en base a las fechas el periodo nominal que selecciono el usuario
                        Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Inicio_Incidencias_Reloj_Checador.Text.ToString()) + " 00:00:00";
                        Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Hora_Entrada = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                        Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Termino_Incidencias_Reloj_Checador.Text.ToString()) + " 23:59:59";
                        Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Hora_Salida = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));

                        Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Dt_Empleados = Dt_Empleados;//Linea Agregada.

                        //Dt_Asistencias_Empleados = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Asistencias_Empleados(); //Consulta los datos generales de las asistencias de los empleados
                        Dt_Asistencias_Empleados = Rs_Consulta_Ope_Nom_Incidencias_Checadas.queryListAsistencia();

                        Session["Consulta_Lista_Asistencia"] = Dt_Asistencias_Empleados;
                        Llena_Grid_Lista_Asistencias(); //Agrega las asistencias obtenidas de la consulta anterior

                        if (Grid_Lista_Asistencias.Rows.Count > 0)
                        {
                            //Registra las Asistencias que se obtuvieron de acuerdo a la consulta realizada para la generación de las asistencias
                            Rs_Alta_Ope_Nom_Asistencias.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Rs_Alta_Ope_Nom_Asistencias.P_Dt_Lista_Asistencia = (DataTable)Session["Consulta_Lista_Asistencia"];
                            Rs_Alta_Ope_Nom_Asistencias.Alta_Asistencias(); //Da de Alta las asistencias de los empleados en la base de datos
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "No se obtuvieron registros de asistencias de empleados";
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Registrar_Asistencias " + ex.Message.ToString());
                }
            }
        #endregion
    #endregion
    #region (Grid)
        protected void Grid_Reloj_Checador_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Grid_Reloj_Checador.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Checadas();                          //Carga las checadas que estan asignados a la página seleccionada
                Grid_Reloj_Checador.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
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
        #region (Eventos Combos)
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
                    Txt_Fecha_Termino_Incidencias_Reloj_Checador.Text = "";
                    Txt_Fecha_Inicio_Incidencias_Reloj_Checador.Text = "";
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
        #endregion
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Btn_Sincronizar_Asistencias_Empleados_Click
        /// DESCRIPCION : Realiza la importación de las checadas de los empleados de presi-
        ///               dencia mostrando esta información al usuario, así como da de alta
        ///               cada una de las insidencias en la base de datos
        /// PARAMETROS  :
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 04/Agosto/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        protected void Btn_Sincronizar_Asistencias_Empleados_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Nom_Incidencias_Checadas_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Incidencias_Checadas_Negocio(); //Variable de conexión a la capa de negocios
            Cls_Ope_Nom_Incidencias_Checadas_Negocio Rs_Alta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Incidencias_Checadas_Negocio();     //Vairble de conexión a la capa de Negocios
            DataTable Dt_Checadas; //Variable que obtendra los datos de la consulta 
            DataTable Dt_Empleados = null;

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Dt_Checadas = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Checadas_Empleados_SQL(); //Consulta los datos generales de las checadas generales de los empleados

                //Obtenemos un listado de empleados sin numeros de empleado duplicados.
                Dt_Empleados = Obtener_Listado_Empleados_Reloj_Checador_Sincronizado(Dt_Checadas);

                Session["Consulta_Checadas"] = Dt_Checadas;
                Llena_Grid_Checadas(); //Agrega las checadas obtenidas de la consulta anterior
                Txt_Total_Incidencias_Importadas.Text = String.Format("{0:#,###,##0}", Convert.ToInt32(Grid_Reloj_Checador.Rows.Count));

                if (Grid_Reloj_Checador.Rows.Count > 0)
                {
                    Txt_Total_Incidencias_Importadas.Text = Grid_Reloj_Checador.Rows.Count.ToString();
                    Rs_Alta_Ope_Nom_Incidencias_Checadas.P_Dt_Checadas = Dt_Checadas;
                    Rs_Alta_Ope_Nom_Incidencias_Checadas.Alta_Incidencias_Checadas(); //Da de Alta las checadas de los empleados en la base de datos
                    Registrar_Asistencias(Dt_Empleados); //Da de Alta las asistencias de los empleados en la base de datos
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pasar Registros de Reloj Checador", "alert('La Importacion de los registros de Checada fue Exitosa');", true);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No hay incidencias registradas";
                }
            }
            catch (Exception ex)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
        protected void Btn_Reporte_Faltas_Retardos_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio(); //Variable de conexión a la capa de negocios
            DataTable Dt_Faltas_Empleados; //Variable que obtendra los datos de la consulta 
            String Fecha_Hora;             //Obtiene la fecha y la hora de la entrada del empleado
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Valida que todos los campos requeridos los haya proporcionado el usuario si es así consulta las faltas que serán considerarán
                //para registrarlas dentro del sistema
                if (Validar_Datos_Asistencias_Empleado())
                {
                    //if (Cmb_Tipo_Nomina.SelectedIndex > 1) Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Inicio_Incidencias_Reloj_Checador.Text.ToString());
                    Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Inicio = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy HH:mm:ss}", Fecha_Hora));
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Termino_Incidencias_Reloj_Checador.Text.ToString());
                    Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Termino = Convert.ToDateTime(String.Format("{0:dd/MM/yyyy HH:mm:ss}", Fecha_Hora));
                    Dt_Faltas_Empleados = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Faltas_Retardos(); //Consulta los datos generales de las faltas de los empleados
                }
                //Si faltaron datos por propocionar muestra al usuario que campos fueron
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
            try
            {
                Session.Remove("Consulta_Checadas");
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


        private DataTable Obtener_Listado_Empleados_Reloj_Checador_Sincronizado(DataTable Dt_Checadas)
        {
            DataTable Dt_Empleados = null;

            try
            {
                Dt_Empleados = Dt_Checadas.Clone();

                if (Dt_Checadas is DataTable) {
                    if (Dt_Checadas.Rows.Count > 0) {
                        foreach (DataRow item in Dt_Checadas.Rows) {
                            if (item is DataRow) {
                                if (!String.IsNullOrEmpty(item["No_Empleado"].ToString())) {
                                    DataRow[] registros = Dt_Empleados.Select("No_Empleado='" + item["No_Empleado"].ToString() + "'");

                                    if (registros.Length <= 0) {
                                        Dt_Empleados.ImportRow(item);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al obtener el listado del empleados sin registro duplicados. Error:[" + Ex.Message + "]");
            }
            return Dt_Empleados;
        }
}
