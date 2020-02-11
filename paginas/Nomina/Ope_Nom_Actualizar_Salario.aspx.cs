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
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Nomina_Actualizar_Salario.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;

public partial class paginas_Nomina_Ope_Nom_Actualizar_Salario : System.Web.UI.Page {

    #region Page Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se carga al cargar la página de Inicio.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            try {
                //Actualizamos el tiempo de la session de la página.
                Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
                //Validamos que exista un usuario logueado al sistema, con una session activa actualmente.
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                Lbl_Ecabezado_Mensaje.Text = "";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                if (!IsPostBack) {
                    Llenar_Combo_Tipos_Nomina();
                    Consultar_Calendarios_Nomina();
                }
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }  
        }

    #endregion

    #region Metodos

        #region Combos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Nomina
            ///DESCRIPCIÓN: Llena el Combo de Tipos de Nomina.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Abril/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Tipos_Nomina(){
                try {
                    Cls_Cat_Tipos_Nominas_Negocio Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
                    DataTable Dt_Tipos_Nominas = null;//Variable que almacenara una lista de las nominas que existe actualmente en el sistema.
                    Tipos_Nominas.P_Actualizar_Salario = "PUESTO";
                    //Consultamos los tipo de nómina que existen actualmente en el sistema.
                    Dt_Tipos_Nominas = Tipos_Nominas.Consulta_Datos_Tipo_Nomina();
                    //Cargamos el combo que corresponde a los tipo de nómina en el sistema.
                    Cmb_Tipo_Nomina.DataSource = Dt_Tipos_Nominas;
                    Cmb_Tipo_Nomina.DataTextField = Cat_Nom_Tipos_Nominas.Campo_Nomina;
                    Cmb_Tipo_Nomina.DataValueField = Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
                    Cmb_Tipo_Nomina.DataBind();
                    Cmb_Tipo_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "NINGUNO"));
                    Cmb_Tipo_Nomina.SelectedIndex = 0;
                } catch(Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = "Error:";
                    Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                    Div_Contenedor_Msj_Error.Visible = true;
                }                
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Sindicatos
            ///DESCRIPCIÓN: Llena el Combo de Sindicatos.
            ///PARAMETROS:     
            ///             1. Tipo_Nomina_ID. 
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 05/Abril/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Sindicatos() {
                
                try {
                    if (Cmb_Tipo_Nomina.SelectedIndex > 0 && Cmb_Calendario_Nomina.SelectedIndex > 0) {
                        DataTable Dt_Sindicatos;
                        Cls_Cat_Nom_Sindicatos_Negocio Cat_Nom_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();
                        Dt_Sindicatos = Cat_Nom_Sindicatos.Consulta_Sindicato();
                        Cmb_Sindicato.DataSource = Dt_Sindicatos;
                        Cmb_Sindicato.DataValueField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Sindicato_ID;
                        Cmb_Sindicato.DataTextField = Presidencia.Constantes.Cat_Nom_Sindicatos.Campo_Nombre;
                        Cmb_Sindicato.DataBind();
                        Cmb_Sindicato.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                        Cmb_Sindicato.Items.Insert(Cmb_Sindicato.Items.Count, new ListItem(">>> TODOS <<< (Aplica a Todos los Sindicatos)", "TODOS"));
                        Cmb_Sindicato.SelectedIndex = 0;
                        Cmb_Sindicato.Enabled = true;



                    } else {
                        Cmb_Sindicato.DataSource = new DataTable();
                        Cmb_Sindicato.DataBind();
                        Cmb_Sindicato.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                        Cmb_Sindicato.Enabled = false;
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = "Error:";
                    Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            #region Calendario Nomina

                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
                /// DESCRIPCION : 
                /// 
                /// PARAMETROS:
                /// 
                /// CREO        : Juan Alberto Hernandez Negrete
                /// FECHA_CREO  : 06/Abril/2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                private void Consultar_Calendarios_Nomina() {
                    Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
                    DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
                    try {
                        Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
                        Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

                        if (Dt_Calendarios_Nominales is DataTable) {
                            Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                            Cmb_Calendario_Nomina.DataTextField = "Nomina";
                            Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                            Cmb_Calendario_Nomina.DataBind();
                            Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
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
                            if (Dt_Periodos_Catorcenales.Rows.Count > 0) {
                                Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                                Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                                Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                                Cmb_Periodos_Catorcenales_Nomina.DataBind();
                                Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                                Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                            } else {
                                Lbl_Ecabezado_Mensaje.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                                Lbl_Mensaje_Error.Text = "";
                                Div_Contenedor_Msj_Error.Visible = true;
                            }
                        }
                    } catch (Exception Ex) {
                        throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
                    }
                }

                ///*******************************************************************************
                /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
                /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
                /// sistema.
                /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
                ///             en el sistema.
                /// 
                /// CREO        : Juan Alberto Hernandez Negrete
                /// FECHA_CREO  : 06/Abril/2011
                /// MODIFICO          :
                /// FECHA_MODIFICO    :
                /// CAUSA_MODIFICACION:
                ///*******************************************************************************
                private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas) {
                    DataTable Dt_Nominas = new DataTable();
                    DataRow Renglon_Dt_Clon = null;
                    Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
                    Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

                    if (Dt_Calendario_Nominas is DataTable) {
                        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows) {
                            if (Renglon is DataRow) {
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
                ///a partir del periodo actual.
                ///CREO: Juan alberto Hernández Negrete
                ///FECHA_CREO: 06/Abril/2011
                ///MODIFICO:
                ///FECHA_MODIFICO
                ///CAUSA_MODIFICACIÓN
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

                                    if (Fecha_Fin >= Fecha_Actual) {
                                        Elemento.Enabled = true;
                                    } else {
                                        Elemento.Enabled = false;
                                    }
                                }
                            }
                        }
                    }
                }

            #endregion

        #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Listado_Sindicatos
        ///DESCRIPCIÓN: Llena el Grid de Sindicatos con la fuente de datos pasada como 
        ///             parametro.
        ///PARAMETROS:  
        ///             1.  Tabla.  Fuente de Datos para llenar el Grid.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void LLenar_Grid_Listado_Sindicatos(DataTable Tabla) {
            try {
                Grid_Listado_Sindicatos.DataSource = Tabla;
                Grid_Listado_Sindicatos.DataBind();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Validar_Sindicatos_PreActualizacion
        ///DESCRIPCIÓN: Llena el Grid de Sindicatos con la fuente de datos pasada como 
        ///             parametro.
        ///PARAMETROS:  
        ///             1.  Tabla.  Fuente de Datos para llenar el Grid.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private DataTable Validar_Sindicatos_PreActualizacion(DataTable Tabla_Sindicatos) { 
            DataTable Filtrado = new DataTable();
            String Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedItem.Value;
            String Nomina_ID = Cmb_Calendario_Nomina.SelectedItem.Value;
            String Periodo = Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Value;
            try {
                for (Int32 Contador = 0; Contador < Tabla_Sindicatos.Rows.Count; Contador++) { 
                    
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
            return Filtrado;
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_DataTable
        ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
        ///             en caso contrario 'false'.
        ///PARAMETROS:  
        ///             1.  Clave.  Clave que se buscara en el DataTable
        ///             2.  Tabla.  Datatable donde se va a buscar la clave.
        ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Boolean Buscar_Clave_DataTable(String Clave, DataTable Tabla, Int32 Columna) {
            Boolean Resultado_Busqueda = false;
            if (Tabla != null && Tabla.Rows.Count > 0 && Tabla.Columns.Count > 0) {
                if (Tabla.Columns.Count > Columna) {
                    for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++) {
                        if (Tabla.Rows[Contador][Columna].ToString().Trim().Equals(Clave.Trim())) {
                            Resultado_Busqueda = true;
                            break;
                        }
                    }
                }
            }
            return Resultado_Busqueda;
        }

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 04/Abril/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private Boolean Validar_Actualizar_Salario() {
                Lbl_Ecabezado_Mensaje.Text = "Para hacer la Actualización es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Cmb_Tipo_Nomina.SelectedIndex == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Tipo de Nomina.";
                    Validacion = false;
                }
                if (Cmb_Calendario_Nomina.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br />"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Año de la Nomina.";
                    Validacion = false;
                }
                if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br />"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Periodo la Nomina a partir de donde se hará la actualización.";
                    Validacion = false;
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
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

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Sindicatos_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el evento de cambio de página en el Grid de Sindicatos.
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Sindicatos_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                DataTable Tabla = new DataTable();
                if (Session["Dt_Sindicatos"] != null) {
                    Tabla = (DataTable)Session["Dt_Sindicatos"];
                }
                Grid_Listado_Sindicatos.PageIndex = e.NewPageIndex;
                LLenar_Grid_Listado_Sindicatos(Tabla);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Sindicatos_RowDataBound
        ///DESCRIPCIÓN: Maneja el evento de RowDataBound en el Grid de Sindicatos.
        ///PARAMETROS:  
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************            
        protected void Grid_Listado_Sindicatos_RowDataBound(object sender, GridViewRowEventArgs e) {
            try {
                if (e.Row.RowType == DataControlRowType.DataRow) {
                    ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Quitar_Sindicato");
                    Boton.CommandArgument = e.Row.Cells[0].Text.Trim();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Sindicato_Click
        ///DESCRIPCIÓN: Maneja el evento de Click del Botón de Agregar Sindicatos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Agregar_Sindicato_Click(object sender, EventArgs e) {
            try {
                if (Cmb_Sindicato.SelectedIndex > 0) {
                    DataTable Tabla = (Session["Dt_Sindicatos"] != null) ? ((DataTable)Session["Dt_Sindicatos"]) : new DataTable();
                    if (Tabla.Columns.Count == 0) {
                        Tabla.Columns.Add("SINDICATO_ID", Type.GetType("System.String"));
                        Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    }
                    if (Cmb_Sindicato.SelectedIndex == (Cmb_Sindicato.Items.Count-1)) {
                        Session.Remove("Dt_Sindicatos");
                        Tabla = new DataTable();
                        Tabla.Columns.Add("SINDICATO_ID", Type.GetType("System.String"));
                        Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                        Grid_Listado_Sindicatos.DataSource = new DataTable();
                        Grid_Listado_Sindicatos.DataBind();
                        foreach (ListItem Item in Cmb_Sindicato.Items) {
                            if (!Item.Value.Equals("NINGUNO") && !Item.Value.Equals("TODOS")) {
                                DataRow Fila = Tabla.NewRow();
                                Fila["SINDICATO_ID"] = Item.Value;
                                Fila["NOMBRE"] = Item.Text;
                                Tabla.Rows.Add(Fila);
                            }
                        }
                    } else {
                        if (!Buscar_Clave_DataTable(Cmb_Sindicato.SelectedItem.Value, Tabla, 0)) {
                            DataRow Fila = Tabla.NewRow();
                            Fila["SINDICATO_ID"] = Cmb_Sindicato.SelectedItem.Value;
                            Fila["NOMBRE"] = Cmb_Sindicato.SelectedItem.Text;
                            Tabla.Rows.Add(Fila);
                        } else {
                            Lbl_Ecabezado_Mensaje.Text = "El Sindicato ya se Encuentra en el Listado";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    Session["Dt_Sindicatos"] = Tabla;
                    Grid_Listado_Sindicatos.PageIndex = 0;
                    Grid_Listado_Sindicatos.SelectedIndex = (-1);
                    LLenar_Grid_Listado_Sindicatos(Tabla);
                }
            } catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Sindicatos_Click
        ///DESCRIPCIÓN: Maneja el evento de Click del Botón de Eliminar Listado de Sindicatos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************        
        protected void Btn_Limpiar_Sindicatos_Click(object sender, EventArgs e) {
            try {
                Session.Remove("Dt_Sindicatos");
                Grid_Listado_Sindicatos.DataSource = new DataTable();
                Grid_Listado_Sindicatos.DataBind();
            } catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
        ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
        ///CREO: Juan Alberto Hernández Negrete
        ///FECHA_CREO: 06/Abril/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e) {
            Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
            if (index > 0) {
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                Cmb_Periodos_Catorcenales_Nomina.Enabled = true;
            } else {
                Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
                Cmb_Periodos_Catorcenales_Nomina.DataBind();
                Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                Cmb_Periodos_Catorcenales_Nomina.Enabled = false;
            }
            Llenar_Combo_Sindicatos();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Sindicato_Click
        ///DESCRIPCIÓN: Maneja el evento de Click del Botón de Quitar de Listado de Sindicatos.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        protected void Btn_Quitar_Sindicato_Click(object sender, ImageClickEventArgs e) {
            try {
                if (sender != null) {
                    ImageButton Boton = (ImageButton)sender;
                    if (Boton != null) {
                        String Sindicato_ID = Boton.CommandArgument;
                        if (Session["Dt_Sindicatos"] != null) {
                            DataTable Tabla = (DataTable)Session["Dt_Sindicatos"];
                            for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++) {
                                if (Tabla.Rows[Contador][0].ToString().Trim().Equals(Sindicato_ID)) {
                                    Tabla.Rows.RemoveAt(Contador);
                                    break;
                                }
                            }
                            if (Tabla.Rows.Count == 0) { Session.Remove("Dt_Sindicatos"); }
                            LLenar_Grid_Listado_Sindicatos(Tabla);
                        }
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Maneja el evento de Click del Botón de Salir.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************           
        protected void Btn_Salir_Click(object sender, EventArgs e) {
            try {
                Session.Remove("Dt_Sindicatos");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Actualizar_Salario_Click
        ///DESCRIPCIÓN: Maneja el evento de Click del Botón de Actualizar Salario. Ejecuta
        ///             la Actualización con las previas validaciones.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 06/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************                  
        protected void Btn_Actualizar_Salario_Click(object sender, EventArgs e) {
            try {
                if (Validar_Actualizar_Salario()) {
                    Cls_Ope_Nom_Actualizar_Salario_Negocio Negocio = new Cls_Ope_Nom_Actualizar_Salario_Negocio();
                    Negocio.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedItem.Value;
                    Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedItem.Value;
                    Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Value);
                    if (Session["Dt_Sindicatos"] != null) {
                        DataTable Tabla = (DataTable)Session["Dt_Sindicatos"];
                        List<Cls_Cat_Nom_Sindicatos_Negocio> Listado_Sindicatos = new List<Cls_Cat_Nom_Sindicatos_Negocio>();
                        for (Int32 Contador = 0; Contador < Tabla.Rows.Count; Contador++) {
                            Cls_Cat_Nom_Sindicatos_Negocio Sindicato = new Cls_Cat_Nom_Sindicatos_Negocio();
                            Sindicato.P_Sindicato_ID = Tabla.Rows[Contador][0].ToString();
                            Sindicato.P_Nombre = Tabla.Rows[Contador][1].ToString();
                            Listado_Sindicatos.Add(Sindicato);
                        }
                        Negocio.P_Listado_Sindicatos = Listado_Sindicatos;
                    }
                    Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Negocio.Registrar_Actualizacion();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Actualización de Salario", "alert('Actualización de Salario Correcta');", true);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Nomina_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de Cambio de Seleccion del Combo de Cmb_Tipo_Nomina.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Abril/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Cmb_Tipo_Nomina_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                Llenar_Combo_Sindicatos();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

}
