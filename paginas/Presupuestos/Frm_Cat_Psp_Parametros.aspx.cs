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
using Presidencia.Paramentros_Presupuestos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Partidas.Negocio;

public partial class paginas_presupuestos_Frm_Cat_Psp_Parametros : System.Web.UI.Page {

    #region "Page_Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Load de la pagina.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Grid_Listado_Parametros.PageIndex = 0;
                Llenar_Grid_Listado_Parametros();
                Configuracion_Formulario(false, "");
                Llenar_Combo_Capitulos();
                Llenar_Combo_Fuente_Financiamiento();
                Habilitar_Formulario(false);
            }
        }

    #endregion

    #region "Metodos"
        #region "Generales"
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Limpiar_Componentes_Formulario
            ///DESCRIPCIÓN          : Limpiar componentes del Formulario.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Limpiar_Componentes_Formulario() {
                Hdf_Parametro_ID.Value = "";
                Txt_Anio.Text = "";
                Txt_Fecha_Apertura.Text = "";
                Txt_Fecha_Cierre.Text = "";
                Grid_Partidas_Stock.DataSource = new DataTable();
                Grid_Partidas_Stock.DataBind();
                Session.Remove("Dt_Partidas_Stock");
                Cmb_Capitulo.SelectedIndex = -1;
                Cmb_Partida_Especifica.SelectedIndex = -1;
                Cmb_Estatus.SelectedIndex = -1;
                Cmb_Fuente_Financiamiento.SelectedIndex = -1;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Validar_Movimiento
            ///DESCRIPCIÓN          : Valida los datos del formulario antes de hacer algun 
            ///                       movimiento.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private Boolean Validar_Movimiento() {
                String Mensaje = "";
                Boolean Pasa_Validacion = true;
                if (Txt_Anio.Text.Trim().Length == 0) {
                    if (!Pasa_Validacion) {
                        Mensaje = Mensaje + " <br/> ";
                    }
                    Mensaje = Mensaje + "* Introducir el Año.";
                    Pasa_Validacion = false;
                }
                if (Txt_Fecha_Apertura.Text.Trim().Length == 0) {
                    if (!Pasa_Validacion) {
                        Mensaje = Mensaje + " <br/> ";
                    }
                    Mensaje = Mensaje + "* Introducir la Fecha de Apertura.";
                    Pasa_Validacion = false;
                 }
                if (Txt_Fecha_Cierre.Text.Trim().Length == 0) {
                    if (!Pasa_Validacion) {
                        Mensaje = Mensaje + " <br/> ";
                    }
                    Mensaje = Mensaje + "* Introducir la Fecha de Cierre.";
                    Pasa_Validacion = false;
                 }
                if (Cmb_Estatus.SelectedIndex <= 0)
                {
                    if (!Pasa_Validacion)
                    {
                        Mensaje = Mensaje + " <br/> ";
                    }
                    Mensaje = Mensaje + "* Seleccionar un Estatus.";
                    Pasa_Validacion = false;
                }
                if (Cmb_Fuente_Financiamiento.SelectedIndex  <= 0)
                {
                    if (!Pasa_Validacion)
                    {
                        Mensaje = Mensaje + " <br/> ";
                    }
                    Mensaje = Mensaje + "* Seleccionar una fuente de financiamiento.";
                    Pasa_Validacion = false;
                }
                if(!Pasa_Validacion){
                    Lbl_Ecabezado_Mensaje.Text = "Verificar:";
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Pasa_Validacion;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
            ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
            ///PARAMETROS :     
            ///             1. Estatus.    Estatus en el que se cargara la configuración de los
            ///                             controles.
            ///CREO       : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO : 19/Octubre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN 
            ///*******************************************************************************
            private void Configuracion_Formulario(Boolean Estatus, String Operacion) {
                if (Operacion.Trim().Equals("NUEVO")) {
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Div_Busqueda.Visible = false;
                    Div_Listado_Parametros.Visible = false;
                    Div_Campos.Visible = true;
                } else if (Operacion.Trim().Equals("MODIFICAR")) {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Div_Busqueda.Visible = false;
                    //Div_Listado_Parametros.Visible = false;
                    //Div_Campos.Visible = true;
                } else {
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png"; 
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Div_Busqueda.Visible = true;
                    Div_Listado_Parametros.Visible = true;
                    Div_Campos.Visible = false;
                    Btn_Buscar.Enabled  = true;
                   Txt_Busqueda.Enabled = true;
                }
                
            }
    
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Buscar_Clave_DataTable
            ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
            ///             en caso contrario 'false'.
            ///PROPIEDADES:  
            ///             1.  Clave.  Clave que se buscara en el DataTable
            ///             2.  Tabla.  Datatable donde se va a buscar la clave.
            ///             3.  Columna.Columna del DataTable donde se va a buscar la clave.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 24/Enero/2011
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Anio_No_Repetido
            ///DESCRIPCIÓN: Busca una Clave en un DataTable, si la encuentra Retorna 'true'
            ///             en caso contrario 'false'.
            ///PROPIEDADES:  
            ///FECHA_CREO : 19/Octubre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private Boolean Validar_Anio_No_Repetido() {
                Boolean Pasa_Validacion = true;
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                Parametros.P_Anio_Presupuestar = Convert.ToInt32(Txt_Anio.Text.Trim());
                DataTable Dt_Parametros = Parametros.Consultar_Parametros();
                if (Dt_Parametros.Rows.Count > 0) {
                    Pasa_Validacion = false;
                    if (Hdf_Parametro_ID.Value.Trim().Length>0) {
                        if (Dt_Parametros.Rows[0][Cat_Psp_Parametros.Campo_Parametro_ID].ToString().Trim().Equals(Hdf_Parametro_ID.Value.Trim())) {
                            Pasa_Validacion = true;
                        }
                    }
                }
                return Pasa_Validacion;
            }

        #endregion

        #region "Interacción con Base de Datos"

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Parametros
            ///DESCRIPCIÓN          : Llenar el listado de partidas.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Parametros() {
                Grid_Listado_Parametros.SelectedIndex = (-1);
                Grid_Listado_Parametros.Columns[1].Visible = true;
                Grid_Listado_Parametros.Columns[6].Visible = true;
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                if (Txt_Busqueda.Text.Trim().Length > 0) {
                    Parametros.P_Anio_Presupuestar = Convert.ToInt32(Txt_Busqueda.Text.Trim());
                }
                Grid_Listado_Parametros.DataSource = Parametros.Consultar_Parametros(); 
                Grid_Listado_Parametros.DataBind();
                Grid_Listado_Parametros.Columns[1].Visible = false;
                Grid_Listado_Parametros.Columns[6].Visible = false;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Partidas_Stock
            ///DESCRIPCIÓN          : Llenar partidas de Stock.
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Grid_Partidas_Stock(DataTable Dt_Datos) {
                Grid_Partidas_Stock.Columns[0].Visible = true;
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                Grid_Partidas_Stock.DataSource = Dt_Datos;
                Grid_Partidas_Stock.DataBind();
                Grid_Partidas_Stock.Columns[0].Visible = false;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Alta_Parametro
            ///DESCRIPCIÓN          : Alta de un parametro
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Alta_Parametro() {
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                Parametros.P_Anio_Presupuestar = Convert.ToInt32(Txt_Anio.Text.Trim());
                Parametros.P_Fecha_Apertura = Convert.ToDateTime(Txt_Fecha_Apertura.Text.Trim());
                Parametros.P_Fecha_Cierre = Convert.ToDateTime(Txt_Fecha_Cierre.Text.Trim());
                Parametros.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
                Parametros.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedItem.Value.Trim();

                if (Session["Dt_Partidas_Stock"] != null) {
                    Parametros.P_Dt_Partidas_Stock = (DataTable)Session["Dt_Partidas_Stock"];
                }
                Parametros.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Parametros.Alta_Parametro();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Modifica_Parametro
            ///DESCRIPCIÓN          : Modifica un Parametro
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Modifica_Parametro() {
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                Parametros.P_Parametro_ID = Hdf_Parametro_ID.Value.Trim();
                Parametros.P_Anio_Presupuestar = Convert.ToInt32(Txt_Anio.Text.Trim());
                Parametros.P_Fecha_Apertura = Convert.ToDateTime(Txt_Fecha_Apertura.Text.Trim());
                Parametros.P_Fecha_Cierre = Convert.ToDateTime(Txt_Fecha_Cierre.Text.Trim());
                Parametros.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedItem.Value.Trim();

                if (Session["Dt_Partidas_Stock"] != null) {
                    Parametros.P_Dt_Partidas_Stock = (DataTable)Session["Dt_Partidas_Stock"];
                }
                Parametros.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Parametros.Modificar_Parametro();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Elimina_Parametro
            ///DESCRIPCIÓN          : Elimina un Parametro
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Elimina_Parametro() {
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                Parametros.P_Parametro_ID = Hdf_Parametro_ID.Value.Trim();
                Parametros.Eliminar_Parametro();
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consulta_Parametro
            ///DESCRIPCIÓN          : Consulta y carga los detalles de un Parametro
            ///PARAMETROS           : 
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Consulta_Parametro() {
                Cls_Cat_Psp_Parametros_Negocio Parametro = new Cls_Cat_Psp_Parametros_Negocio();
                Parametro.P_Parametro_ID = Hdf_Parametro_ID.Value.Trim();
                Parametro = Parametro.Consultar_Detalles_Parametro();
                Limpiar_Componentes_Formulario();
                Hdf_Parametro_ID.Value = Parametro.P_Parametro_ID;
                Txt_Anio.Text = Parametro.P_Anio_Presupuestar.ToString();
                Txt_Fecha_Apertura.Text = String.Format("{0:dd/MMM/yyyy}", Parametro.P_Fecha_Apertura);
                Txt_Fecha_Cierre.Text = String.Format("{0:dd/MMM/yyyy}", Parametro.P_Fecha_Cierre);
                if (String.IsNullOrEmpty(Parametro.P_Estatus))
                {
                    Cmb_Estatus.SelectedIndex = -1;
                }
                else {
                    Cmb_Estatus.SelectedValue = Parametro.P_Estatus;
                }
                if (String.IsNullOrEmpty(Parametro.P_Fuente_Financiamiento_ID))
                {
                    Cmb_Fuente_Financiamiento.SelectedIndex = -1;
                }
                else
                {
                    Cmb_Fuente_Financiamiento.SelectedIndex = Cmb_Fuente_Financiamiento.Items.IndexOf(Cmb_Fuente_Financiamiento.Items.FindByValue(Parametro.P_Fuente_Financiamiento_ID));
                }
                
                if (Parametro.P_Dt_Partidas_Stock != null && Parametro.P_Dt_Partidas_Stock.Rows.Count > 0) {
                    Session["Dt_Partidas_Stock"] = Parametro.P_Dt_Partidas_Stock;
                    Llenar_Grid_Partidas_Stock(Parametro.P_Dt_Partidas_Stock);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Partidas_Especificas
            ///DESCRIPCIÓN          : Llena el combo de Partidas Especificas
            ///PARAMETROS           1 Capitulo_Id Filtro por el que llenaremos las partidas especificas
            ///CREO                 : Francisco Antonio Gallardo Castañeda
            ///FECHA_CREO           : 19/Octubre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combo_Partidas_Especificas(String Capitulo_Id) {
                Cls_Cat_Psp_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Psp_Parametros_Negocio();//Instancia con la clase de negocios
                Cmb_Partida_Especifica.Items.Clear(); //limpiamos el combo
                Parametros_Negocio.P_Capitulo_Id = Capitulo_Id.Trim();
                Cmb_Partida_Especifica.DataSource = Parametros_Negocio.Consultar_Partida_Especifica();
                Cmb_Partida_Especifica.DataTextField = Cat_Com_Partidas.Campo_Nombre;
                Cmb_Partida_Especifica.DataValueField = Cat_Com_Partidas.Campo_Partida_ID;
                Cmb_Partida_Especifica.DataBind();
                Cmb_Partida_Especifica.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
            }


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Capitulos
            ///DESCRIPCIÓN          : Llena el combo de Capitulos
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 07/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combo_Capitulos()
            {
                Cls_Cat_Psp_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Psp_Parametros_Negocio();//Instancia con la clase de negocios
                Cmb_Capitulo.Items.Clear(); //limpiamos el combo
                Cmb_Capitulo.DataSource = Parametros_Negocio.Consultar_Capitulos();
                Cmb_Capitulo.DataTextField = "NOMBRE";
                Cmb_Capitulo.DataValueField = Cat_SAP_Capitulos.Campo_Capitulo_ID;
                Cmb_Capitulo.DataBind();
                Cmb_Capitulo.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
                
                Cmb_Partida_Especifica.Enabled = false;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Fuente_Financiamiento
            ///DESCRIPCIÓN          : Llena el combo de fuente de financiamiento
            ///PARAMETROS           : 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 11/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Llenar_Combo_Fuente_Financiamiento()
            {
                Cls_Cat_Psp_Parametros_Negocio Parametros_Negocio = new Cls_Cat_Psp_Parametros_Negocio();//Instancia con la clase de negocios
                Cmb_Fuente_Financiamiento.Items.Clear(); //limpiamos el combo
                Cmb_Fuente_Financiamiento.DataSource = Parametros_Negocio.Consultar_Fuente_Financiamiento();
                Cmb_Fuente_Financiamiento.DataTextField = "NOMBRE";
                Cmb_Fuente_Financiamiento.DataValueField = Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
                Cmb_Fuente_Financiamiento.DataBind();
                Cmb_Fuente_Financiamiento.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Habilitar_Formulario
            ///DESCRIPCIÓN          : Metodo para habilitar o deshabilitar los controles del formulario
            ///PARAMETROS           1 Estatus para manejar si se habilita o deshabilitan los controles 
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 07/Noviembre/2011
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            private void Habilitar_Formulario(Boolean Estatus)
            {
                Txt_Anio.Enabled = Estatus;
                Cmb_Capitulo.Enabled = Estatus;
                Cmb_Estatus.Enabled = Estatus;
                Btn_Agregar_Partida.Enabled = Estatus; 
                Grid_Partidas_Stock.Columns[3].Visible = Estatus;
                Cmb_Fuente_Financiamiento.Enabled = Estatus;
            }
        #endregion
    #endregion

    #region "Grids"
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Grid_Listado_Parametros_SelectedIndexChanged
        ///DESCRIPCIÓN          : Ejecuta la operacion de cambio de seleccion en listado 
        ///                       de parametros.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Grid_Listado_Parametros_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Parametros.SelectedIndex > (-1)) {
                Hdf_Parametro_ID.Value = Grid_Listado_Parametros.SelectedRow.Cells[1].Text.Trim();
                Consulta_Parametro();
                Div_Listado_Parametros.Visible = false;
                Div_Campos.Visible = true;
                Btn_Salir.AlternateText = "Regresar";
                Txt_Busqueda.Enabled = false;
                Btn_Buscar.Enabled = false;
                if (Cmb_Estatus.SelectedItem.Text.Trim() == "INACTIVO")
                {
                    Grid_Partidas_Stock.Columns[3].Visible = false;
                    Cmb_Partida_Especifica.Visible = false;
                    Cmb_Capitulo.Visible = false;
                    Lbl_Capitulo.Visible = false;
                    Lbl_Partida_Especifica.Visible = false;
                    Btn_Agregar_Partida.Visible = false;
                    Btn_Txt_Fecha_Apertura.Enabled = false;
                    Btn_Txt_Fecha_Cierre.Enabled = false;
                }
                else
                {
                    Grid_Partidas_Stock.Columns[3].Visible = true;
                    Cmb_Partida_Especifica.Visible = true;
                    Cmb_Capitulo.Visible = true;
                    Lbl_Capitulo.Visible = true;
                    Lbl_Partida_Especifica.Visible = true;
                    Btn_Agregar_Partida.Visible = true;
                    Btn_Txt_Fecha_Apertura.Enabled = true;
                    Btn_Txt_Fecha_Cierre.Enabled = true;
                }
            }
            Habilitar_Formulario(false);
        }
    #endregion

    #region "Eventos"
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de nuevo.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Nuevo.AlternateText.Trim().Equals("Nuevo")) {
                
                Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
                DataTable Dt_Parametros = Parametros.Consultar_Parametros();
                Boolean Valido = true;
                foreach(DataRow Dr in Dt_Parametros.Rows){
                    if (Dr["ESTATUS"].ToString().Trim().Equals("ACTIVO")) {
                        Valido = false;
                    }
                }
                if (Valido)
                {
                    Limpiar_Componentes_Formulario();
                    Configuracion_Formulario(true, "NUEVO");
                    Habilitar_Formulario(true);
                    Txt_Anio.Text = (DateTime.Today.Year).ToString();
                    Txt_Fecha_Apertura.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    Txt_Fecha_Cierre.Text = "31/dic/" + (DateTime.Today.Year).ToString();
                    Cmb_Estatus.SelectedIndex = 1;
                }
                else {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "Ya existen un Año activo.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                if (Validar_Movimiento()) {
                    if (Validar_Anio_No_Repetido()) {
                        Alta_Parametro();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Alta Exitosa');", true);
                        Llenar_Grid_Listado_Parametros();
                        Configuracion_Formulario(false, "");
                        Habilitar_Formulario(false);
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                        Lbl_Mensaje_Error.Text = "Ya existen parametros para el Año '" + Txt_Anio.Text.Trim() + "'.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de modificar.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)  {
            if (Btn_Modificar.AlternateText.Trim().Equals("Modificar")) {
                if (Hdf_Parametro_ID.Value.Trim().Length > 0) { 
                    Configuracion_Formulario(true, "MODIFICAR");
                    Habilitar_Formulario(true);
                    Cmb_Estatus.Enabled = false;
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "Es necesario que se seleccione el elemento a Modificar";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                if (Validar_Movimiento()) {
                    if (Validar_Anio_No_Repetido()) {
                        Modifica_Parametro();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Actualización Exitosa');", true);
                        Llenar_Grid_Listado_Parametros();
                        Configuracion_Formulario(false, "");
                        Habilitar_Formulario(false);
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                        Lbl_Mensaje_Error.Text = "Ya existen parametros para el Año '" + Txt_Anio.Text.Trim() + "'.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de cancelar.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            if (Btn_Salir.AlternateText.Trim().Equals("Salir")) {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            } else {
                Grid_Listado_Parametros.SelectedIndex = (-1);
                Limpiar_Componentes_Formulario();
                Configuracion_Formulario(false, "");
                Limpiar_Componentes_Formulario();
                Habilitar_Formulario(false);
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de Buscar.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e) {
            Llenar_Grid_Listado_Parametros();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Quitar_Partida_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de Quitar Partida.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Quitar_Partida_Click(object sender, ImageClickEventArgs e) {
            try {
                ImageButton Btn_Quitar_Partida = (ImageButton)sender;
                String Partida_ID = Btn_Quitar_Partida.CommandArgument.Trim();
                if (Session["Dt_Partidas_Stock"] != null) {
                    DataTable Dt_Partidas_Stock = (DataTable)Session["Dt_Partidas_Stock"];
                    Int32 Fila = (-1);
                    for (Int32 Contador = 0; Contador < Dt_Partidas_Stock.Rows.Count; Contador++) {
                        if (Dt_Partidas_Stock.Rows[Contador]["PARTIDA_ID"].ToString().Equals(Partida_ID)) {
                            Fila = Contador;
                            break;
                        }
                    }
                    if (Fila > (-1)) {
                        Dt_Partidas_Stock.Rows.RemoveAt(Fila);
                        Session["Dt_Partidas_Stock"] = Dt_Partidas_Stock;
                        Llenar_Grid_Partidas_Stock(Dt_Partidas_Stock);
                    }
                }
            } catch (Exception Ex) {
                throw new Exception("Error al quitar la partida Error:["+Ex.Message+"]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Partida_Click
        ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de Agregar Partida.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Agregar_Partida_Click(object sender, ImageClickEventArgs e) {
            if (Cmb_Partida_Especifica.SelectedIndex>0) {
                DataTable Tabla = (DataTable)Grid_Partidas_Stock.DataSource;
                if (Tabla == null) {
                    if (Session["Dt_Partidas_Stock"] == null) {
                        Tabla = new DataTable("Resguardos");
                        Tabla.Columns.Add("PARTIDA_ID", Type.GetType("System.String"));
                        Tabla.Columns.Add("CLAVE", Type.GetType("System.String"));
                        Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    } else {
                        Tabla = (DataTable)Session["Dt_Partidas_Stock"];
                    }
                }
                if (!Buscar_Clave_DataTable(Cmb_Partida_Especifica.SelectedItem.Value, Tabla, 0)) {
                    DataRow Fila = Tabla.NewRow();
                    Fila["PARTIDA_ID"] = Cmb_Partida_Especifica.SelectedItem.Value;
                    Fila["CLAVE"] = Cmb_Partida_Especifica.SelectedItem.Text.Substring(0, Cmb_Partida_Especifica.SelectedItem.Text.IndexOf(" ")).Trim();
                    Fila["NOMBRE"] = Cmb_Partida_Especifica.SelectedItem.Text.Substring(Cmb_Partida_Especifica.SelectedItem.Text.IndexOf(" ")+1).Trim();
                    Tabla.Rows.Add(Fila);
                    Session["Dt_Partidas_Stock"] = Tabla;
                    Llenar_Grid_Partidas_Stock(Tabla);
                    Cmb_Partida_Especifica.SelectedIndex = -1;
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "La Partida ya esta Agregada";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "Seleccione la Partida a Agregar";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Txt_Anio_TextChanged
        ///DESCRIPCIÓN          : Evento de Cambio de Año.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 19/Octubre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Txt_Anio_TextChanged(object sender, EventArgs e) {
            if (Txt_Anio.Text.Trim().Length > 0) {
                Int32 Anio = Convert.ToInt32(Txt_Anio.Text.Trim());
                if (Txt_Fecha_Apertura.Text.Trim().Length > 0) {
                    DateTime Temporal = Convert.ToDateTime(Txt_Fecha_Apertura.Text.Trim());
                    Txt_Fecha_Apertura.Text = String.Format("{0:dd/MMM}", Temporal) + "/" + Anio.ToString();
                }
                if (Txt_Fecha_Cierre.Text.Trim().Length > 0) {
                    DateTime Temporal = Convert.ToDateTime(Txt_Fecha_Cierre.Text.Trim());
                    Txt_Fecha_Cierre.Text = String.Format("{0:dd/MMM}", Temporal) + "/" + Anio.ToString();
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cmb_Capitulo_SelectedIndexChanged
        ///DESCRIPCIÓN          : Evento del combo de Capitulos
        ///PROPIEDADES          :
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 07/Noviembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///******************************************************************************* 
        protected void Cmb_Capitulo_SelectedIndexChanged(object sender, EventArgs e)
        {
            String Capitulo_Id = String.Empty;
            try
            {
                Cmb_Partida_Especifica.Enabled = true;
                if(Cmb_Capitulo.SelectedIndex  > 0){
                    Capitulo_Id = Cmb_Capitulo.SelectedValue.Trim();
                    Llenar_Combo_Partidas_Especificas(Capitulo_Id);
                }
            }
            catch(Exception ex) 
            {
                throw new Exception("Error en el evento del combo de capitulos Error[" + ex.Message + "]");
            }
        }
    #endregion
}

