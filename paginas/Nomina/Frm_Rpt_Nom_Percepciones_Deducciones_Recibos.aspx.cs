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
using Presidencia.Reportes_nomina_Fijos.Negocio;
using Presidencia.Incidencias_Checadas.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Calendario_Reloj_Checador.Negocio;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;
using Presidencia.Prestamos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Reportes_Nomina.Percepciones_Deducciones.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Percepciones_Deducciones_Recibos : System.Web.UI.Page
{
    #region Page_Load
        protected void Page_Load(object sender, EventArgs e) {
             if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            try {
                if (!IsPostBack) {
                    Inicializacion();
                    Cmb_Tipo_Reporte_SelectedIndexChanged(Cmb_Tipo_Reporte, null);
                }
            } catch (Exception ex) {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = ex.Message.ToString();
            }
        }
    #endregion

    #region Metodos
        #region Metodos Generales

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
            private void Inicializacion() {
                try {
                    Limpiar_Controles();//Limpia los controles de la forma
                    Consultar_Unidades_Responsables();
                    Consultar_Tipos_Nominas();
                    Consultar_Calendarios_Nomina();
                } catch (Exception ex) {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
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
            private void Limpiar_Controles() {
                try {
                    Hdf_Empleado_ID.Value = "";
                    Txt_No_Empleado.Text = "";
                    Txt_Nombre_Empleado.Text = "";

                    if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                        Cmb_Unidad_Responsable.SelectedIndex = 0;

                    if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
                        Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = 0;

                    if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                        Cmb_Calendario_Nomina.SelectedIndex = 0;

                    if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                        Cmb_Tipo_Nomina.SelectedIndex = 0;

                    Txt_Fecha_Inicial.Text = "";
                    Txt_Fecha_Final.Text = "";
                } catch (Exception ex) {
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
            private Boolean IsNumeric(String Cadena) {
                Boolean Resultado = true;
                Char[] Array = Cadena.ToCharArray();
                try {
                    for (int index = 0; index < Array.Length; index++) {
                        if (!Char.IsDigit(Array[index])) return false;
                    }
                } catch (Exception Ex) {
                    throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
                }
                return Resultado;
            }

        #endregion
        #region Validaciones
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
        private void Validar_Periodos_Pago(DropDownList Combo) {
            Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
            DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
            DateTime Fecha_Actual = DateTime.Now;
            DateTime Fecha_Inicio = new DateTime();
            DateTime Fecha_Fin = new DateTime();

            Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

            foreach (ListItem Elemento in Combo.Items) {
                if (IsNumeric(Elemento.Text.Trim())) {
                    Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                    Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                    if (Dt_Detalles_Nomina != null) {
                        if (Dt_Detalles_Nomina.Rows.Count > 0) {
                            Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                            Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());
                        }
                    }
                }
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
        private Boolean Validar_Reporte() {
            String Espacios_Blanco;
            Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
            Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario: <br>";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;

            //  validacion para cuando se selecciona algun numero de nomina o periodo
            if ( ((Txt_Fecha_Inicial.Text == "") && (Txt_Fecha_Final.Text == ""))) {
                if (Cmb_Calendario_Nomina.SelectedIndex == 0) {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecciona la nomina.<br>";
                    Datos_Validos = false;
                }
                if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex == 0) {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecciona el periodo de la nomina.<br>";
                    Datos_Validos = false;
                }
            } else {
                if (Txt_Fecha_Inicial.Text == "") {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha inicial.<br>";
                    Datos_Validos = false;
                } else if (Txt_Fecha_Inicial.Text.Length != 8) {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud de la fecha inicial debe de ser de 8 caracteres.<br>";
                    Datos_Validos = false;
                }

                if (Txt_Fecha_Final.Text == "") {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha final.<br>";
                    Datos_Validos = false;
                } else if (Txt_Fecha_Final.Text.Length != 8) {
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud de la fecha final debe de ser de 8 caracteres.<br>";
                    Datos_Validos = false;
                }

                //  para validar la fecha introducida
                if ((Txt_Fecha_Inicial.Text != "") && (Txt_Fecha_Final.Text != "")) {
                    if (Validar_Fechas()) {
                        Datos_Validos = false;
                    }
                }
            }

           
            
            return Datos_Validos;
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
        private Boolean Validar_Fechas() {
            Boolean Estatus_Final = false;
            Boolean Estatus_Fecha_Inicial = false;
            Boolean Estatus_Fecha_final = false;
            DateTime Fecha_Inicial = new DateTime() ;
            DateTime Fecha_Final = new DateTime();
            String Fecha = "";
            String Fecha1 = "";
            String Fecha2 = "";
            String Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
            try {
                Fecha_Inicial = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text.Trim());
                Fecha_Final = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text.Trim());

                if (Fecha_Inicial.CompareTo(Fecha_Final) == 1) {
                    Estatus_Final = true;
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La fecha final debe ser mayor que la inicial.<br>";
                }

                return Estatus_Final;
            } catch (Exception Ex) {
                throw new Exception("Error al Validar las Fechas . Error: [" + Ex.Message + "]");
            }
        }
        #endregion
        #region Consultas
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
        private void Consultar_Calendarios_Nomina() {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión con la capa de negocios.
            DataTable Dt_Calendarios_Nominales = null; //Variable que almacena los calendarios nominales que existén actualmente en el sistema.
            try {
                Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
                Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

                if (Dt_Calendarios_Nominales is DataTable) {
                    Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                    Cmb_Calendario_Nomina.DataTextField = "Nomina";
                    Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                    Cmb_Calendario_Nomina.DataBind();
                    Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("< - TODAS - >", ""));
                    Cmb_Calendario_Nomina.SelectedIndex = -1;
                }
            } catch (Exception Ex) {
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
        private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID) {
            Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
            DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

            try {
                Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
                Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
                if (Dt_Periodos_Catorcenales != null) {
                    if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                    {
                        Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                        Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                        Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                        Cmb_Periodos_Catorcenales_Nomina.DataBind();
                        Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< - TODOS - >", ""));
                        Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                        Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                    } else {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                    }
                }
            } catch (Exception Ex) {
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
            Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
            DataTable Dt_Calendario_Nomina = null;

            try
            {
                Obj_Calendario.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Obj_Calendario.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text);

                Dt_Calendario_Nomina = Obj_Calendario.Consulta_Periodos_Nomina();

                var periodos = from periodo in Dt_Calendario_Nomina.AsEnumerable()
                               select new
                               {
                                   Fecha_Inicia = periodo.Field<DateTime>(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio),
                                   Fecha_Fin = periodo.Field<DateTime>(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin)
                               };

                foreach (var fecha in periodos)
                {
                    Txt_Fecha_Inicial.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(fecha.Fecha_Inicia));
                    Txt_Fecha_Final.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(fecha.Fecha_Fin));
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
            }
        }

        protected void Consultar_Percepciones_Fijas() {
            Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepciones_Fijas = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
            DataTable Dt_Percepciones_Fijas = null;

            try
            {
                Obj_Percepciones_Fijas.P_TIPO = Cmb_Tipo_Reporte.SelectedItem.Value.Trim();
                Obj_Percepciones_Fijas.P_ESTATUS = "ACTIVO";
                Dt_Percepciones_Fijas = Obj_Percepciones_Fijas.Consultar_Percepciones_Deducciones_General();

                Dt_Percepciones_Fijas = Juntar_Clave_Percepcion_Deduccion(Dt_Percepciones_Fijas);
                Cmb_Percepciones.DataSource = Dt_Percepciones_Fijas;
                Cmb_Percepciones.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
                Cmb_Percepciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
                Cmb_Percepciones.DataBind();

                Cmb_Percepciones.Items.Insert(0, new ListItem("< - TODAS - >", ""));

                Cmb_Percepciones.SelectedIndex = -1;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las percepciones fijas del empleado. Error: [" + Ex.Message + "]");
            }
        }

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
                Cargar_Combos(Cmb_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
                Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al cosnultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
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
                Cargar_Combos(Cmb_Busqueda_Dependencia, Dt_Unidades_Responsables, "CLAVE_NOMBRE", Cat_Dependencias.Campo_Dependencia_ID, 0);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
            }
        }
        #endregion

    #region Formato y carga
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
            Combo.Items.Insert(0, new ListItem("<- TODAS ->", ""));
            Combo.SelectedIndex = Index;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Ctlr de Tipo DropDownList. Error: [" + Ex.Message + "]");
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
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Percepcion_Deduccion
    /// 
    /// DESCRIPCION : Junta la clave de la percepcion y deduccion con el nombre.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 07/Julio/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Percepcion_Deduccion(DataTable Dt_Percepciones_Deducciones)
    {
        try
        {
            if (Dt_Percepciones_Deducciones is DataTable)
            {
                if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepciones_Deducciones.Rows)
                    {
                        PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                            PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave] + " -- " +
                            PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre];
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al juntar el nombre de la percepcion deduccion con la clave. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
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
    ///FECHA_CREO:  26/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Pdf_Click(object sender, ImageClickEventArgs e) {
        try {
            Crear_Tablas_Reporte("PDF");
        } catch (Exception Ex) {
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
    ///FECHA_CREO:  26/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Excel_Click(object sender, ImageClickEventArgs e) {
        try{
            Crear_Tablas_Reporte("EXCEL");
        } catch (Exception Ex) {
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
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 24/Marzo/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e) {
        MPE_Resguardante.Show();
    }

    #endregion

    #region Combos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
            ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
            ///CREO       : Juan Alberto Hernández Negrete
            ///FECHA_CREO : 06/Abril/2011
            ///MODIFICO          :
            ///FECHA_MODIFICO    :
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e) {
                Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
                if (index > 0) {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim()); //Consulta los periodos nominales validos
                } else {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged
            ///DESCRIPCIÓN: .
            ///CREO       : Juan Alberto Hernández Negrete
            ///FECHA_CREO : 06/Abril/2011
            ///MODIFICO          :
            ///FECHA_MODIFICO    :
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e) {
                try {
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
                    Txt_Fecha_Inicial.Text = "";
                    Txt_Fecha_Final.Text = "";
                    if (index > 0) {
                        Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado
                    }
                } catch (Exception ex) {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = ex.Message.ToString();
                }
            }
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
            ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
            ///CREO: Juan alberto Hernández Negrete
            ///FECHA_CREO: 01/Diciembre/2010
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            protected void Cmb_Tipo_Reporte_SelectedIndexChanged(object sender, EventArgs e) {
                Consultar_Percepciones_Fijas();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "javascript:refresh_tabla_empleados();", true);
            }

        #endregion

    #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Tablas_Reporte
        ///DESCRIPCIÓN: Crea las Tablas con las que se generara el Reporte
        ///PARAMETROS:  1.- Tipo.- Tipo de Reporte.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Crear_Tablas_Reporte(String Tipo) {
            Ds_Rpt_Nom_Percepciones_Deducciones Ds_Reporte = new Ds_Rpt_Nom_Percepciones_Deducciones();
            //Se hace la consulta del reporte.
            Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Negocio = new Cls_Rpt_Nom_Percepciones_Deducciones_Negocio();
            if (Cmb_Tipo_Reporte.SelectedIndex > (-1)) { Negocio.P_Tipo_Percepcion_Deduccion = Cmb_Tipo_Reporte.SelectedItem.Value; }
            if (Cmb_Calendario_Nomina.SelectedIndex > 0) { Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedItem.Value; }
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0) { Negocio.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Value; }
            if (Cmb_Percepciones.SelectedIndex > 0) { Negocio.P_Percepcion_Deduccion_ID = Cmb_Percepciones.SelectedItem.Value; }
            if (Cmb_Unidad_Responsable.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value; }
            if (Cmb_Tipo_Nomina.SelectedIndex > 0) { Negocio.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedItem.Value; }
            if (!String.IsNullOrEmpty(Hdf_Empleado_ID.Value)) { Negocio.P_Empleado_ID = Hdf_Empleado_ID.Value.Trim(); }
            DataTable Dt_Consulta = Negocio.Consultar_Datos_Percepciones_Deducciones();
            Dt_Consulta.TableName = "DT_DATOS";
            if (Tipo.Trim().Equals("PDF")) { 
                DataSet Ds_Consulta = new DataSet();
                Ds_Consulta.Tables.Add(Dt_Consulta.Copy());
                Ds_Consulta.Tables.Add(Obtener_Datos_Generales_Reporte());
                Generar_Reporte(Ds_Consulta, Ds_Reporte, "Cr_Rpt_Nom_Percepciones_Deducciones.rpt", "Cr_Rpt_Nom_Percepciones_Deducciones.pdf");            
            } else if (Tipo.Trim().Equals("EXCEL")) {
                Cambiar_Nombre_Cabeceras(ref Dt_Consulta);
                Pasar_DataTable_A_Excel(Dt_Consulta);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cambiar_Nombre_Cabeceras
        ///DESCRIPCIÓN: Cambia el Nombre de las Cabeceras
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Cambiar_Nombre_Cabeceras(ref DataTable Dt_Datos_Empleado) {
            Dt_Datos_Empleado.Columns["NO_RECIBO"].ColumnName = "No. Recibo";
            Dt_Datos_Empleado.Columns["NOMBRE_DEPENDENCIA"].ColumnName = "Unidad Responsable";
            Dt_Datos_Empleado.Columns["NO_EMPLEADO"].ColumnName = "No. Empleado";
            Dt_Datos_Empleado.Columns["NOMBRE_EMPLEADO"].ColumnName = "Nombre del Empleado";
            Dt_Datos_Empleado.Columns["ANIO"].ColumnName = "Año";
            Dt_Datos_Empleado.Columns["PERIODO"].ColumnName = "Periodo";
            Dt_Datos_Empleado.Columns["PERCEPCION_DEDUCCION"].ColumnName = "Percepcion - Deduccion";
            Dt_Datos_Empleado.Columns["IMPORTE"].ColumnName = "Importe";
            Dt_Datos_Empleado.Columns.Remove("EMPLEADO_ID");
        }
    
        /// *************************************************************************************************************************
        /// Nombre: Pasar_DataTable_A_Excel
        /// 
        /// Descripción: Pasa DataTable a Excel.
        /// 
        /// Parámetros: Dt_Reporte.- DataTable que se pasara a excel.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 18/Octubre/2011.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public void Pasar_DataTable_A_Excel(System.Data.DataTable Dt_Reporte) {
            String Ruta = "Reporte de Percepciones y Deducciones.xls";//Variable que almacenara el nombre del archivo. 

            try {
                //Creamos el libro de Excel.
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

                Libro.Properties.Title = "RH";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "RH";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
                //Creamos el estilo titulo para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Titulo = Libro.Styles.Add("TitleStyle");

                Estilo_Titulo.Font.FontName = "Tahoma";
                Estilo_Titulo.Font.Size = 9;
                Estilo_Titulo.Font.Bold = true;
                Estilo_Titulo.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Titulo.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Titulo.Font.Color = "#808080";
                Estilo_Titulo.Interior.Color = "#FFFFFF";
                Estilo_Titulo.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Titulo.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 0, "Black");
                Estilo_Titulo.Alignment.WrapText = true;

                Estilo_Cabecera.Font.FontName = "Tahoma";
                Estilo_Cabecera.Font.Size = 10;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Cabecera.Font.Color = "#FFFFFF";
                Estilo_Cabecera.Interior.Color = "#193d61";
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Alignment.WrapText = true;

                Estilo_Contenido.Font.FontName = "Tahoma";
                Estilo_Contenido.Font.Size = 8;
                Estilo_Contenido.Font.Bold = true;
                Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Contenido.Font.Color = "#000000";
                Estilo_Contenido.Interior.Color = "White";
                Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Alignment.WrapText = true;

                //SE CARGA LA CABECERA PRINCIPAL DEL ARCHIVO
                WorksheetCell cell = Renglon.Cells.Add("MUNICIPIO DE IRAPUATO, GUANAJUATO");
                cell.MergeAcross = Dt_Reporte.Columns.Count - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add("COORDINACION GENERAL DE ADMINISTRACION - DIRECCION DE RECURSOS HUMANOS");
                cell.MergeAcross = Dt_Reporte.Columns.Count - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add("Reporte de Percepciones y Deducciones");
                cell.MergeAcross = Dt_Reporte.Columns.Count - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add("");
                cell.MergeAcross = Dt_Reporte.Columns.Count - 1;
                cell.StyleID = "TitleStyle";
                Renglon.Height = 20;
                Renglon = Hoja.Table.Rows.Add();

                //Agregamos las columnas que tendrá la hoja de excel.
                for (Int32 Columna=0; Columna<Dt_Reporte.Columns.Count; Columna++){
                        Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(180));
                }
                if (Dt_Reporte is System.Data.DataTable) {
                    if (Dt_Reporte.Rows.Count > 0) {
                        for (Int32 Columna=0; Columna<Dt_Reporte.Columns.Count; Columna++){
                            if (Columna <= Dt_Reporte.Columns.Count) {
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Dt_Reporte.Columns[Columna].ColumnName, "HeaderStyle"));
                                Renglon.Height = 20;
                            }
                        }
                        foreach (System.Data.DataRow FILA in Dt_Reporte.Rows) {
                            if (FILA is System.Data.DataRow) {
                                Renglon = Hoja.Table.Rows.Add();
                                for (Int32 Columna=0; Columna<Dt_Reporte.Columns.Count; Columna++){
                                    if (Columna <= Dt_Reporte.Columns.Count) {
                                        if (!String.IsNullOrEmpty(FILA[Columna].ToString())) { 
                                            if (Columna == Dt_Reporte.Columns.Count-1) {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(String.Format("{0:c}", Convert.ToDouble(FILA[Columna])), DataType.String, "BodyStyle"));
                                            } else {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[Columna].ToString(), DataType.String, "BodyStyle"));
                                            }
                                        } else {
                                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[Columna].ToString(), DataType.String, "BodyStyle"));
                                        }
                                    }
                                }
                                Renglon.Height = 35;
                                Renglon.AutoFitHeight = true;
                            }
                        }
                    }
                }

                //Abre el archivo de excel
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Libro.Save(Response.OutputStream);
                Response.End();
            } catch (Exception Ex) {
                throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Datos_Generales_Reporte
        ///DESCRIPCIÓN: Carga los Datos Generales del Reporte
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataTable Obtener_Datos_Generales_Reporte() {
            DataTable Dt_Datos = new DataTable("DT_GENERALES_REPORTE");
            Dt_Datos.Columns.Add("ELABORO", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("NOMBRE_REPORTE", Type.GetType("System.String"));

            DataRow Fila = Dt_Datos.NewRow();
            Fila["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            Fila["NOMBRE_REPORTE"] = Cmb_Tipo_Reporte.SelectedItem.Text;
            Dt_Datos.Rows.Add(Fila);

            return Dt_Datos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
        ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
        ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
        ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
        ///CREO: Susana Trigueros Armenta.
        ///FECHA_CREO: 01/Mayo/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo) {
            String Ruta = "../../Reporte/ " + Nombre_Archivo;
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Concepto_TextChanged
        ///DESCRIPCIÓN: Evento de Cambio de Texto
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 04/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Txt_Concepto_TextChanged(object sender, EventArgs e) {
            if (!String.IsNullOrEmpty(Txt_Concepto.Text.Trim())) {
                String Clave = Txt_Concepto.Text.ToUpper().Trim();
                Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Negocio = new Cls_Rpt_Nom_Percepciones_Deducciones_Negocio();
                Negocio.P_Percepcion_Deduccion_Clave = Clave;
                Negocio = Negocio.Consultar_PD_Clave();
                if (!String.IsNullOrEmpty(Negocio.P_Tipo_Percepcion_Deduccion)) { Cmb_Tipo_Reporte.SelectedIndex = Cmb_Tipo_Reporte.Items.IndexOf(Cmb_Tipo_Reporte.Items.FindByValue(Negocio.P_Tipo_Percepcion_Deduccion)); Cmb_Tipo_Reporte_SelectedIndexChanged(Cmb_Tipo_Reporte, null); }
                if (!String.IsNullOrEmpty(Negocio.P_Percepcion_Deduccion_ID)) { Cmb_Percepciones.SelectedIndex = Cmb_Percepciones.Items.IndexOf(Cmb_Percepciones.Items.FindByValue(Negocio.P_Percepcion_Deduccion_ID)); }
                if (String.IsNullOrEmpty(Negocio.P_Tipo_Percepcion_Deduccion) || Negocio.P_Tipo_Percepcion_Deduccion.Length==0)
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "", "alert('No se encontro la Percepcion y/ó Deducción');", true);
                }
            } else {
                Cmb_Percepciones.SelectedIndex = 0;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e) {
            try {
                Grid_Busqueda_Empleados.PageIndex = 0;
                Llenar_Grid_Busqueda_Empleados();
                MPE_Resguardante.Show();
            }  catch (Exception Ex) {
                //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                //Lbl_Mensaje_Error.Text = "";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
        ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Busqueda_Empleados() {
            Grid_Busqueda_Empleados.SelectedIndex = (-1);
            Grid_Busqueda_Empleados.Columns[1].Visible = true;
            Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Negocio = new Cls_Rpt_Nom_Percepciones_Deducciones_Negocio();
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = String.Format("{0:000000}",Convert.ToInt32(Txt_Busqueda_No_Empleado.Text.Trim())); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Empleado = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Empleado = Txt_Busqueda_Nombre_Empleado.Text.Trim().ToUpper(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            Grid_Busqueda_Empleados.DataSource = Negocio.Consultar_Empleados();
            Grid_Busqueda_Empleados.DataBind();
            Grid_Busqueda_Empleados.Columns[1].Visible = false;
        }

               
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el evento de cambio de Página del GridView de Busqueda
        ///             de empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Busqueda_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Busqueda_Empleados.PageIndex = e.NewPageIndex;
                Llenar_Grid_Busqueda_Empleados();
                MPE_Resguardante.Show();
            } catch (Exception Ex) {
                //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                //Lbl_Mensaje_Error.Text = "";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del GridView de Busqueda
        ///             de empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Busqueda_Empleados_SelectedIndexChanged(object sender, EventArgs e) { 
            try {
                if (Grid_Busqueda_Empleados.SelectedIndex > (-1)) {
                    Hdf_Empleado_ID.Value = "";
                    Txt_Nombre_Empleado.Text = "";
                    Txt_No_Empleado.Text = "";
                    String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados.SelectedRow.Cells[1].Text.Trim();
                    Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                    Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                    DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                    String Dependencia_ID = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim() : null;
                    Int32 Index_Combo = (-1);
                    Cmb_Unidad_Responsable.SelectedIndex = Cmb_Unidad_Responsable.Items.IndexOf(Cmb_Unidad_Responsable.Items.FindByValue(Dependencia_ID));
                    Hdf_Empleado_ID.Value = ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim() : null);
                    Txt_No_Empleado.Text = ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim() : null);
                    Txt_Nombre_Empleado.Text =  ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString().Trim() : null);
                    Txt_Nombre_Empleado.Text = Txt_Nombre_Empleado.Text.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString().Trim() : null);
                    Txt_Nombre_Empleado.Text = Txt_Nombre_Empleado.Text.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString().Trim() : null);
                    MPE_Resguardante.Hide();
                    Grid_Busqueda_Empleados.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                //Lbl_Mensaje_Error.Text = "";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }

}
