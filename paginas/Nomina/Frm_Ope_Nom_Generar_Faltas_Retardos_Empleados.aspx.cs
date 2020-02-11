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
using System.Text.RegularExpressions;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Reloj_Checador.Negocio;
using Presidencia.Generar_Faltas_Retardos_Empleados.Negocio;

public partial class paginas_Nomina_Frm_Nom_Ope_Generar_Faltas_Retardos_Empleados : System.Web.UI.Page
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
        /// FECHA_CREO  : 09-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Inicializa_Controles()
        {
            try
            {
                Limpia_Controles(); //Limpia los controles del forma
                Consultar_Calendarios_Nomina();
                Btn_Generar_Faltas_Retardos.Enabled = false;
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
        /// FECHA_CREO  : 09-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Limpia_Controles()
        {
            try
            {
                Cmb_Calendario_Nomina.SelectedIndex = -1;
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
                Txt_Fecha_Inicio_Falta_Retardo.Text = "";
                Txt_Fecha_Termino_Falta_Retardo.Text = "";
                Grid_Lista_Faltas_Retardos.DataSource = new DataTable();
                Grid_Lista_Faltas_Retardos.DataBind();
            }
            catch (Exception ex)
            {
                throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
            }
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Validar_Datos
        /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 09-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private Boolean Validar_Datos()
        {
            Int32 index = 0;
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

            index = Cmb_Calendario_Nomina.SelectedIndex;
            if (index <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione El año de la Nómina consultar los periodos nominales validos. <br>";
                Datos_Validos = false;
            }
            index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
            if (index <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el periodo nóminal para consultar la fecha de inicio y termino para la generación de las asistencias. <br>";
                Datos_Validos = false;
            }
            return Datos_Validos;
        }
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Dt_Lista_Faltas_Retardos
        /// DESCRIPCION : Llena el grid con los retarod y faltas de los Empleados que pertenecen
        /// PARAMETROS  : 
        /// CREO        : Yazmin A Delgado Gómez
        /// FECHA_CREO  : 09-Agosto-2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        private void Llena_Grid_Lista_Faltas_Retardos()
        {
            DataTable Dt_Lista_Faltas_Retardos; //Variable que obtendra los datos de la consulta 
            try
            {

                Grid_Lista_Faltas_Retardos.Columns[0].Visible = true;
                Grid_Lista_Faltas_Retardos.Columns[1].Visible = true;
                Grid_Lista_Faltas_Retardos.Columns[6].Visible=true;
                Dt_Lista_Faltas_Retardos = (DataTable)Session["Consulta_Lista_Faltas_Retardos"];
                Grid_Lista_Faltas_Retardos.DataSource = Dt_Lista_Faltas_Retardos;
                Grid_Lista_Faltas_Retardos.DataBind();
                Grid_Lista_Faltas_Retardos.Columns[0].Visible = false;
                Grid_Lista_Faltas_Retardos.Columns[1].Visible = false;
                Grid_Lista_Faltas_Retardos.Columns[6].Visible = false;
                Grid_Lista_Faltas_Retardos.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                throw new Exception("Dt_Lista_Faltas_Retardos " + ex.Message.ToString(), ex);
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
        #region (Consulta Combos)
        ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: LLenar_Combos
            ///DESCRIPCIÓN: Carga el combo que es pasado como parámetro con la tabla de 
            ///             datos que tambien es pasada como parámetro.
            ///PARÁMETROS : Combo: [DropDownList] Control donde se cargaran los datos.
            ///             Dt_Datos: Tabla que contiene el listado a mostrar en el combo.
            ///             Valor: Valor del elemento al seleccionar una opcion del combo.
            ///             Texto_Mostrar: Texto que se mostrara al usuario.
            ///CREO       : Juan Alberto Hernández Negrete
            ///FECHA_CREO : 06/Abril/2011 11:40 am 
            ///MODIFICO          :
            ///FECHA_MODIFICO    :
            ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
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
                    Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
                    Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.Consulta_Fechas_Calendario_Reloj_Checador(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

                    //Asigna los valores obtenidos de la consulta a los controles correspondientes
                    foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
                    {
                        if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                        {
                            Txt_Fecha_Inicio_Falta_Retardo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()));
                            Txt_Fecha_Termino_Falta_Retardo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString()));
                            Btn_Generar_Faltas_Retardos.Enabled = true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
                }
            }
        #endregion
    #endregion
    #region (Grid)
        protected void Grid_Lista_Faltas_Retardos_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Grid_Lista_Faltas_Retardos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
                Llena_Grid_Lista_Faltas_Retardos();                    //Carga las faltas y retados que estan asignados a la página seleccionada
                Grid_Lista_Faltas_Retardos.SelectedIndex = -1;
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
                Btn_Generar_Faltas_Retardos.Enabled = false;
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
                    Btn_Generar_Faltas_Retardos.Enabled = false;

                    Txt_Fecha_Inicio_Falta_Retardo.Text = "";
                    Txt_Fecha_Termino_Falta_Retardo.Text = "";

                    Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
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
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Faltas_Retardos_Click
        ///DESCRIPCIÓN: Consulta las faltas que serán generadas y dadas de alta a los
        ///             empleados de la presidencia de acuerdo a las asistencias que fueron
        ///             registradas
        ///CREO       : Yazmin A Delgado Gómez
        ///FECHA_CREO : 09-Agosto-2011
        ///MODIFICO          :
        ///FECHA_MODIFICO    :
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Generar_Faltas_Retardos_Click(object sender, ImageClickEventArgs e)
        {
            Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Rs_Consulta_Ope_Nom_Incidencias_Checadas = new Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio(); //Variable de conexión a la capa de negocios
            Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio Rs_Alta_Ope_Nom_Faltas_Empleado = new Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio();              //Vairble de conexión a la capa de Negocios
            DataTable Dt_Faltas_Empleados; //Variable que obtendra los datos de la consulta 
            String Fecha_Hora;             //Obtiene la fecha y la hora de la entrada del empleado

            try
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Valida que todos los campos requeridos los haya proporcionado el usuario si es así consulta las faltas que serán considerarán
                //para registrarlas dentro del sistema
                if (Validar_Datos())
                {
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Inicio_Falta_Retardo.Text.ToString());
                    Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Inicio = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                    Fecha_Hora = String.Format("{0:dd/MMM/yyyy}", Txt_Fecha_Termino_Falta_Retardo.Text.ToString());
                    Rs_Consulta_Ope_Nom_Incidencias_Checadas.P_Fecha_Termino = Convert.ToDateTime(String.Format("{0:dd/MMM/yyyy HH:mm:ss}", Fecha_Hora));
                    Dt_Faltas_Empleados = Rs_Consulta_Ope_Nom_Incidencias_Checadas.Consulta_Lista_Faltas_Retardos(); //Consulta los datos generales de las faltas de los empleados
                    Session["Consulta_Lista_Faltas_Retardos"] = Dt_Faltas_Empleados;
                    Llena_Grid_Lista_Faltas_Retardos(); //Agrega las faltas obtenidas de la consulta anterior
                    if (Grid_Lista_Faltas_Retardos.Rows.Count > 0)
                    {
                        Rs_Alta_Ope_Nom_Faltas_Empleado.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Rs_Alta_Ope_Nom_Faltas_Empleado.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                        Rs_Alta_Ope_Nom_Faltas_Empleado.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
                        Rs_Alta_Ope_Nom_Faltas_Empleado.P_Dt_Lista_Faltas_Retardos = (DataTable)Session["Consulta_Lista_Faltas_Retardos"];
                        Rs_Alta_Ope_Nom_Faltas_Empleado.Alta_Faltas_Retardos(); //Da de Alta las faltas y retardos de los empleados en la base de datos
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Generar Faltas y Retardos", "alert('El Alta de los registros fue Exitosa');", true);
                    }
                    else 
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se generaron faltas o retardos de empleados";
                    }
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
                Session.Remove("Consulta_Lista_Faltas_Retardos");
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
