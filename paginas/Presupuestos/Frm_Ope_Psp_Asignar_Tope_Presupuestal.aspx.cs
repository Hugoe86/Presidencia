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
using Presidencia.Sessiones;
using Presidencia.Capitulos.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Limite_Presupuestal.Negocio;
using Presidencia.Paramentros_Presupuestos.Negocio;


public partial class paginas_presupuestos_Frm_Ope_Psp_Asignar_Tope_Presupuestal : System.Web.UI.Page
{
    #region VARIABLES GLOBALES

        private static String P_Dt_Capitulos = "Dt_Capitulos_Presupuesto";
        private const String Operacion_Quitar_Renglon = "QUITAR";
        private const String Operacion_Agregar_Renglon_Nuevo = "AGREGAR_NUEVO";
        private const String Operacion_Agregar_Renglon_Copia = "AGREGAR_COPIA";
        private const String P_Dt_Unidades_Asignadas = "DT_UNIDADES_ASIGNADAS";
        private const String MODO_MODIFICAR = "Modificar";
        private const String MODO_NUEVO = "Nuevo";
        private const String MODO_INICIAL = "Inicial";

    #endregion
    
    #region PAGE LOAD
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                Cls_Sessiones.Mostrar_Menu = true;
                Asignar_Tope_Presupuestal_Inicio();
            }
            Mostrar_Informacion("", false);
        }
    #endregion

    #region METODOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Unidades_Asignadas
        ///DESCRIPCIÓN:
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18/Oct /2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Llenar_Grid_Unidades_Asignadas()
        {
            Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio();
            Negocio.P_Anio_Presupuestal = Txt_Anio_Presupuestal.Text.Trim();
            DataTable Dt_UR_Asiganadas = Negocio.Consultar_Unidades_Asignadas();
            Grid_Unidades_Responsables.DataBind();
            if (Dt_UR_Asiganadas != null && Dt_UR_Asiganadas.Rows.Count > 0)
            {
                Session[P_Dt_Unidades_Asignadas] = Dt_UR_Asiganadas;
                Grid_Unidades_Responsables.Columns[3].Visible = true;
                Grid_Unidades_Responsables.Columns[6].Visible = true;
                Grid_Unidades_Responsables.Columns[7].Visible = true;
                Grid_Unidades_Responsables.DataSource = Session[P_Dt_Unidades_Asignadas] as DataTable;
                Grid_Unidades_Responsables.DataBind();
                Grid_Unidades_Responsables.Columns[3].Visible = false;
                Grid_Unidades_Responsables.Columns[6].Visible = false;
                Grid_Unidades_Responsables.Columns[7].Visible = false;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Unidades
        ///DESCRIPCIÓN:
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18/Oct /2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataTable Crear_Tabla_Unidades()
        {
            DataTable Dt_Unidades_Asiganadas = new DataTable("UNIDADES");
            DataColumn Dc_Temporal = null;
            Dc_Temporal = new DataColumn("DEPENDENCIA_ID", System.Type.GetType("System.String"));
            Dt_Unidades_Asiganadas.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("CLAVE", System.Type.GetType("System.String"));
            Dt_Unidades_Asiganadas.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("CLAVE_NOMBRE", System.Type.GetType("System.String"));
            Dt_Unidades_Asiganadas.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("LIMITE_PRESUPUESTAL", System.Type.GetType("System.String"));
            Dt_Unidades_Asiganadas.Columns.Add(Dc_Temporal);
            Dc_Temporal = new DataColumn("ANIO_PRESUPUESTAL", System.Type.GetType("System.String"));
            Dt_Unidades_Asiganadas.Columns.Add(Dc_Temporal);
            return Dt_Unidades_Asiganadas;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Agregar_Quitar_Renglones_A_DataTable
        ///DESCRIPCIÓN: 
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18 OCT 2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataTable Agregar_Quitar_Renglones_A_DataTable(DataTable _DataTable, DataRow _DataRow, String Operacion)
        {
            if (Operacion == Operacion_Agregar_Renglon_Nuevo)
            {
                _DataTable.Rows.Add(_DataRow);
            }
            else if (Operacion == Operacion_Agregar_Renglon_Copia)
            {
                _DataTable.ImportRow(_DataRow);
                _DataTable.AcceptChanges();
            }
            else if (Operacion == Operacion_Quitar_Renglon)
            {
                //((DataTable)Session[P_Dt_Unidades_Asignadas]).Rows.Remove(_DataRow);
                _DataTable.Rows.Remove(_DataRow);
            }
            return _DataTable;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Form
        ///DESCRIPCIÓN: 
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18 OCT 2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Limpiar_Form()
        {
            Txt_Tope_Presupuestal.Text = "";
            Cmb_Unidad_Responsable.SelectedIndex = -1;
            Cmb_Programa.SelectedIndex = -1;
            Grid_Capitulos.SelectedIndex = -1;
            Limpiar_Grid_Capitulos();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Form
        ///DESCRIPCIÓN: 
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18 OCT 2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Limpiar_Formulario()
        {
            Txt_Tope_Presupuestal.Text = "";
            Txt_Fecha_Limite.Text = "";
            Txt_Anio_Presupuestal.Text = "";
            Cmb_Unidad_Responsable.SelectedIndex = -1;
            Cmb_Fuente_Financiamiento.SelectedIndex = -1;
            Cmb_Programa.SelectedIndex = -1;
            Grid_Unidades_Responsables.DataBind();
            Limpiar_Grid_Capitulos();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidades_Responsables
        ///DESCRIPCIÓN: 
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18 OCT 2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private void Llenar_Combo_Unidades_Responsables(String Accion)
        {
            Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio();
            Cmb_Unidad_Responsable.Items.Clear();
            try 
            {
                Negocio.P_Anio_Presupuestal = Txt_Anio_Presupuestal.Text.Trim();
                Negocio.P_Accion = Accion;
                DataTable Dt_Unidades = Negocio.Consultar_Unidades_Responsables_Sin_Asignar();
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable, Dt_Unidades, "NOMBRE",Cat_Dependencias.Campo_Dependencia_ID);
            }  
            catch(Exception ex)
            {
                throw new Exception("Error al tratar de llenar el combo de unidades responsables Error[" + ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Capitulos
        ///DESCRIPCIÓN: 
        ///CREO: Gustavo Angeles
        ///FECHA_CREO: 18 OCT 2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
         private void Llenar_Grid_Capitulos()
        {
            Cls_Cat_SAP_Capitulos_Negocio Negocio_Capitulos = new Cls_Cat_SAP_Capitulos_Negocio();
            DataTable Dt_Capitulos = Negocio_Capitulos.Consulta_Datos_Capitulos();   
            try
            {
                Grid_Capitulos.Columns[1].Visible = true;
                Grid_Capitulos.DataSource = Dt_Capitulos;
                Grid_Capitulos.DataBind();
                Grid_Capitulos.Columns[1].Visible = false;
                Session[P_Dt_Capitulos] = Dt_Capitulos;
            }
            catch(Exception Ex)
            {
                Ex.ToString();
                Grid_Capitulos.DataSource = null;
                Grid_Capitulos.DataBind();
                Session[P_Dt_Capitulos] = null;
            }
        }

         ///*******************************************************************************
         // NOMBRE DE LA FUNCIÓN: Guardar
         // DESCRIPCIÓN:    
         // RETORNA: 
         // CREO: Gustavo Angeles Cruz
         // FECHA_CREO: 30/Agosto/2010 
         // MODIFICO:
         // FECHA_MODIFICO
         // CAUSA_MODIFICACIÓN   
         // *******************************************************************************/
         private String Guardar()
         {
             bool Respuesta = true;
             int indice = -1;
             bool Seleccionado = false;
             Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio();
             DataTable Dt_Capitulos = new DataTable("CAPITULOS");
             DataColumn Dc_Temporal = null;
             Dc_Temporal = new DataColumn("CAPITULO_ID", System.Type.GetType("System.String"));
             Dt_Capitulos.Columns.Add(Dc_Temporal);
             DataRow _DataRow = null;
             foreach (GridViewRow Renglon_Grid in Grid_Capitulos.Rows)
             {
                 indice++;
                 Grid_Capitulos.SelectedIndex = indice;
                 Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Capitulo")).Checked;
                 if (Seleccionado)
                 {
                     _DataRow = Dt_Capitulos.NewRow();
                     _DataRow["CAPITULO_ID"] = Grid_Capitulos.SelectedDataKey["CAPITULO_ID"].ToString().Trim();
                     Dt_Capitulos.Rows.Add(_DataRow);
                 }
             }
             Negocio.P_Dt_Capitulos = Dt_Capitulos;
             Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
             Negocio.P_Anio_Presupuestal = Txt_Anio_Presupuestal.Text.Trim();
             Negocio.P_Limite_Presupuestal = Txt_Tope_Presupuestal.Text.Trim();
             Negocio.P_Fuente_Financiamiento_ID = Cmb_Fuente_Financiamiento.SelectedValue.Trim();
             Negocio.P_Programa_ID = Cmb_Programa.SelectedValue.Trim();

             return Negocio.Guardar_Limites();
         }

         ///*******************************************************************************
         // NOMBRE DE LA FUNCIÓN: Validar
         // DESCRIPCIÓN:    
         // RETORNA: 
         // CREO: Gustavo Angeles Cruz
         // FECHA_CREO: 30/Agosto/2010 
         // MODIFICO:
         // FECHA_MODIFICO
         // CAUSA_MODIFICACIÓN   
         // *******************************************************************************/
         private bool Validar()
         {
             bool Respuesta = true;
             int indice = -1;
             bool Seleccionado = false;

             foreach (GridViewRow Renglon_Grid in Grid_Capitulos.Rows)
             {
                 indice++;
                 Grid_Capitulos.SelectedIndex = indice;
                 Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Capitulo")).Checked;
                 if (Seleccionado)
                 {
                     break;
                 }
             }
             if (!Seleccionado)
             {
                 Grid_Capitulos.SelectedIndex = -1;
                 return false;
             }
             if (Txt_Tope_Presupuestal.Text.Trim().Length == 0)
             {
                 Grid_Capitulos.SelectedIndex = -1;
                 return false;
             }
             if (Cmb_Unidad_Responsable.SelectedIndex == 0)
             {
                 Grid_Capitulos.SelectedIndex = -1;
                 return false;
             }
             if (Cmb_Programa.SelectedIndex == 0)
             {
                 Grid_Capitulos.SelectedIndex = -1;
                 return false;
             }
             Grid_Capitulos.SelectedIndex = -1;
             return Respuesta;
         }

         ///*******************************************************************************
         // NOMBRE DE LA FUNCIÓN: Registro_Duplicado
         // DESCRIPCIÓN:
         // RETORNA: 
         // CREO: Gustavo Angeles Cruz
         // FECHA_CREO: 30/Agosto/2010 
         // MODIFICO:
         // FECHA_MODIFICO
         // CAUSA_MODIFICACIÓN   
         // *******************************************************************************/
         private bool Registro_Duplicado(String Dependencia_ID)
         {
             bool Respuesta = false;
             DataTable Dt_Tabla = Session[P_Dt_Unidades_Asignadas] as DataTable;
             DataRow[] _DataRow = Dt_Tabla.Select("DEPENDENCIA_ID ='" + Dependencia_ID + "'");
             if (_DataRow != null && _DataRow.Length > 0)
             {
                 Respuesta = true;
             }
             return Respuesta;
         }

         ///*******************************************************************************
         // NOMBRE DE LA FUNCIÓN: Habilitar_Controles
         // DESCRIPCIÓN: Habilita la configuracion de acuerdo a la operacion     
         // RETORNA: 
         // CREO: Gustavo Angeles Cruz
         // FECHA_CREO: 30/Agosto/2010 
         // MODIFICO:
         // FECHA_MODIFICO
         // CAUSA_MODIFICACIÓN   
         // *******************************************************************************/
         private void Habilitar_Controles(String Modo)
         {
             try
             {
                 switch (Modo)
                 {
                     case MODO_INICIAL:
                         Btn_Nuevo.Visible = true;
                         Btn_Modificar.Visible = true;
                         Btn_Salir.Visible = true;

                         Btn_Nuevo.ToolTip = "Nuevo";
                         Btn_Modificar.ToolTip = "Modificar";

                         if (Div_Listado_Parametros.Visible)
                         {
                             Btn_Salir.ToolTip = "Inicio";
                         }
                         else {
                             Btn_Salir.ToolTip = "Regresar";
                         }

                         Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                         Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                         Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                         Cmb_Unidad_Responsable.Enabled = false;
                         Txt_Tope_Presupuestal.Enabled = false;
                         Txt_Anio_Presupuestal.Enabled = false;
                         Txt_Fecha_Limite.Enabled = false;
                         Txt_Busqueda.Enabled = false;
                         Btn_Buscar.Enabled = false;
                         Grid_Capitulos.Enabled = false;
                         Cmb_Programa.Enabled = false;
                         break;
                     //Estado de Nuevo
                     case MODO_NUEVO:
                         Btn_Nuevo.Visible = true;
                         Btn_Modificar.Visible = false;

                         Btn_Nuevo.ToolTip = "Guardar";
                         Btn_Salir.ToolTip = "Cancelar";

                         Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                         Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                         Cmb_Unidad_Responsable.Enabled = true;
                         Txt_Tope_Presupuestal.Enabled = true;
                         Txt_Busqueda.Enabled = false;
                         Cmb_Programa.Enabled = true;
                         Btn_Buscar.Enabled = false;
                         Grid_Capitulos.Enabled = true;
                         break;
                     //Estado de Modificar
                     case MODO_MODIFICAR:
                         Btn_Nuevo.Visible = false;
                         Btn_Modificar.Visible = true;

                         Btn_Modificar.ToolTip = "Actualizar";
                         Btn_Salir.ToolTip = "Cancelar";

                         Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                         Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                         Cmb_Unidad_Responsable.Enabled = false;
                         Txt_Tope_Presupuestal.Enabled = true;
                         Txt_Busqueda.Enabled = false;
                         Btn_Buscar.Enabled = false;
                         Grid_Capitulos.Enabled = true;
                         Cmb_Programa.Enabled = true;
                         Div_Listado_Parametros.Visible = false;
                         Div_Listado_Requisiciones.Visible = true;
                         break;
                     default: break;
                 }
             }
             catch (Exception ex)
             {
                 Mostrar_Informacion(ex.ToString(), true);
             }
         }

         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Actualizar
         ///DESCRIPCIÓN          : metodo para actualizar los datos de las unidades responsables
         ///PARAMETROS           : 
         ///CREO                 : Leslie González Vázquez
         ///FECHA_CREO           : 08/Noviembre/2011
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         private Boolean Actualizar()
         {
             Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio(); //conexion con la capa de negocio
             Boolean Actualizacion_Valida = false;
             int indice = -1;
             bool Seleccionado = false;
             DataTable Dt_Capitulos = new DataTable("CAPITULOS");
             DataColumn Dc_Temporal = null;
             Dc_Temporal = new DataColumn("CAPITULO_ID", System.Type.GetType("System.String"));
             Dt_Capitulos.Columns.Add(Dc_Temporal);
             DataRow _DataRow = null;

             try
             {
                 Negocio.P_Anio_Presupuestal = Txt_Anio_Presupuestal.Text.Trim();
                 Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
                 Negocio.P_Limite_Presupuestal = Txt_Tope_Presupuestal.Text.Trim().Replace(",", "");
                 Negocio.P_Programa_ID = Cmb_Programa.SelectedValue.Trim();

                 foreach (GridViewRow Renglon_Grid in Grid_Capitulos.Rows)
                 {
                     indice++;
                     Grid_Capitulos.SelectedIndex = indice;
                     Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Capitulo")).Checked;
                     if (Seleccionado)
                     {
                         _DataRow = Dt_Capitulos.NewRow();
                         _DataRow["CAPITULO_ID"] = Grid_Capitulos.SelectedDataKey["CAPITULO_ID"].ToString().Trim();
                         Dt_Capitulos.Rows.Add(_DataRow);
                     }
                 }
                 Negocio.P_Dt_Capitulos = Dt_Capitulos;

                 Actualizacion_Valida = Negocio.Actualizar_Limites();
             }
             catch(Exception ex)
             {
                 throw new Exception("Error al tratar de actalizar los datos Error[" + ex.Message  + "]");
             }
             return Actualizacion_Valida;
         }

         ///*******************************************************************************
         //NOMBRE DE LA FUNCIÓN: Mostrar_Información
         //DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
         //RETORNA: 
         //CREO: Gustavo Angeles Cruz
         //FECHA_CREO: 24/Agosto/2010 
         //MODIFICO:
         //FECHA_MODIFICO:
         //CAUSA_MODIFICACIÓN:
         //********************************************************************************/
         private void Mostrar_Informacion(String txt, Boolean mostrar)
         {
             Lbl_Informacion.Style.Add("color", "#990000");
             Lbl_Informacion.Visible = mostrar;
             Img_Warning.Visible = mostrar;
             Lbl_Informacion.Text = txt;
         }

         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Listado_Parametros
         ///DESCRIPCIÓN          : Llenar el listado de parametros.
         ///PARAMETROS           : 
         ///CREO                 : Francisco Antonio Gallardo Castañeda
         ///FECHA_CREO           : 19/Octubre/2011
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         private void Llenar_Grid_Listado_Parametros()
         {
             Grid_Listado_Parametros.SelectedIndex = (-1);
             Grid_Listado_Parametros.Columns[1].Visible = true;
             Grid_Listado_Parametros.Columns[5].Visible = true;
             Grid_Listado_Parametros.Columns[6].Visible = true;
             Cls_Cat_Psp_Parametros_Negocio Parametros = new Cls_Cat_Psp_Parametros_Negocio();
             if (Txt_Busqueda.Text.Trim().Length > 0)
             {
                 Parametros.P_Anio_Presupuestar = Convert.ToInt32(Txt_Busqueda.Text.Trim());
             }
             Grid_Listado_Parametros.DataSource = Parametros.Consultar_Parametros();
             Grid_Listado_Parametros.DataBind();
             Grid_Listado_Parametros.Columns[1].Visible = false;
             Grid_Listado_Parametros.Columns[5].Visible = true;
             Grid_Listado_Parametros.Columns[6].Visible = false;
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
         ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Capitulos
         ///DESCRIPCIÓN          : Llena el combo de Capitulos
         ///PARAMETROS           : 
         ///CREO                 : Leslie González Vázquez
         ///FECHA_CREO           : 07/Noviembre/2011
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         private void Asignar_Tope_Presupuestal_Inicio() 
         {
             try
             {
                 Llenar_Grid_Listado_Parametros();
                 Habilitar_Controles(MODO_INICIAL);
                 Div_Listado_Parametros.Visible = true;
                 Div_Listado_Requisiciones.Visible = false;
                 Txt_Busqueda.Enabled = true;
                 Btn_Buscar.Enabled = true;
                 Llenar_Grid_Capitulos();
                 Session[P_Dt_Unidades_Asignadas] = Crear_Tabla_Unidades();
                 Limpiar_Formulario();
                 Llenar_Combo_Fuente_Financiamiento();
             }
             catch (Exception ex)
             {
                 throw new Exception("Error al inicio de asiganr tope presupuestal inicio Error[" + ex.Message + "]");
             }
         }

         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Limpiar_Grid_Capitulos
         ///DESCRIPCIÓN          : Metodo para deseleccionar los checkboxs del grid de capitulos
         ///PARAMETROS           : 
         ///CREO                 : Leslie González Vázquez
         ///FECHA_CREO           : 08/Noviembre/2011
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         private void Limpiar_Grid_Capitulos() {
             try {
                if (Grid_Capitulos is GridView)
                     {
                         if (Grid_Capitulos.Rows.Count > 0)
                         {
                             foreach (GridViewRow GV_CAPITULO in Grid_Capitulos.Rows)
                             {
                                 if (GV_CAPITULO is GridViewRow)
                                 {
                                     if (!String.IsNullOrEmpty(GV_CAPITULO.Cells[1].Text))
                                     {
                                         ((CheckBox)GV_CAPITULO.Cells[0].FindControl("Chk_Capitulo")).Checked = false;
                                     }
                                 }
                             }
                         }
                     }
             }
             catch(Exception ex){
                 throw new Exception("Error al limpiar el grid de capitulos Error[" + ex.Message + "]");
             }
         }

         ///*******************************************************************************
         ///NOMBRE DE LA FUNCIÓN : Llenar_Combo_Programas
         ///DESCRIPCIÓN          : Llena el combo de programas
         ///PARAMETROS           : 
         ///CREO                 : Leslie González Vázquez
         ///FECHA_CREO           : 14/Noviembre/2011
         ///MODIFICO             :
         ///FECHA_MODIFICO       :
         ///CAUSA_MODIFICACIÓN   :
         ///*******************************************************************************
         private void Llenar_Combo_Programas()
         {
             Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio();//Instancia con la clase de negocios
             
             Cmb_Programa.Items.Clear(); //limpiamos el combo
             if (Cmb_Unidad_Responsable.SelectedIndex > 0) {
                 Negocio.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value.Trim();
                 Cmb_Programa.DataSource = Negocio.Consultar_Programa_Unidades_Responsables();
                 Cmb_Programa.DataTextField = "NOMBRE";
                 Cmb_Programa.DataValueField = Cat_Sap_Proyectos_Programas.Campo_Proyecto_Programa_Id;
                 Cmb_Programa.DataBind();
                 Cmb_Programa.Items.Insert(0, new ListItem("<-- SELECCIONE -->", ""));
             }
         }
    #endregion

    #region EVENTOS
        #region GENERALES
             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
             ///DESCRIPCIÓN: 
             ///CREO: Gustavo Angeles
             ///FECHA_CREO: 18 OCT 2011
             ///MODIFICO:
             ///FECHA_MODIFICO:
             ///CAUSA_MODIFICACIÓN:
             ///*******************************************************************************
             protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
            {
                if (Btn_Salir.ToolTip == "Inicio")
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    if (Btn_Salir.ToolTip == "Cancelar")
                    {
                        Asignar_Tope_Presupuestal_Inicio();
                        Div_Listado_Parametros.Visible = false;
                        Div_Listado_Requisiciones.Visible = true;
                        Txt_Busqueda.Enabled = false;
                        Btn_Buscar.Enabled = false;
                    }
                    else {
                        Asignar_Tope_Presupuestal_Inicio();
                    }
                }
            }

             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
             ///DESCRIPCIÓN: 
             ///CREO: Gustavo Angeles
             ///FECHA_CREO: 18 OCT 2011
             ///MODIFICO:
             ///FECHA_MODIFICO:
             ///CAUSA_MODIFICACIÓN:
             ///*******************************************************************************
             protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
             {
                 if (Btn_Nuevo.ToolTip == "Nuevo")
                 {
                     if(!string.IsNullOrEmpty(Txt_Anio_Presupuestal.Text.Trim()))
                     {
                         Cmb_Unidad_Responsable.SelectedIndex = -1;
                         Limpiar_Form();
                         Habilitar_Controles(MODO_NUEVO);
                         Llenar_Combo_Unidades_Responsables("Sin_Asignar");
                     }else
                     {
                        Mostrar_Informacion("Seleccione Año Presupuestal", true);
                     }
                     
                 }
                 else if (Btn_Nuevo.ToolTip == "Guardar")
                 {
                     if (Validar())
                     {
                         if (!Registro_Duplicado(Cmb_Unidad_Responsable.SelectedValue))
                         {
                             if (Guardar() == "EXITO")
                             {
                                 //agregar unidad creada o asignada al grid correspondiente
                                 DataTable Dt_Unidades = ((DataTable)Session[P_Dt_Unidades_Asignadas]);
                                 DataRow _DataRow = Dt_Unidades.NewRow();
                                 String Select_Cmb_Unidades = Cmb_Unidad_Responsable.SelectedItem.Text.Trim();
                                 char[] ch = { ' ' };
                                 String[] Split = Select_Cmb_Unidades.Split(ch);
                                 _DataRow["CLAVE"] = Split[0];
                                 String Descripcion = "";
                                 for (int i = 1; i < Split.Length; i++)
                                 {
                                     Descripcion += Split[i];
                                 }
                                 _DataRow["CLAVE_NOMBRE"] = Descripcion;
                                 _DataRow["DEPENDENCIA_ID"] = Cmb_Unidad_Responsable.SelectedValue;
                                 _DataRow["ANIO_PRESUPUESTAL"] = Txt_Anio_Presupuestal.Text.Trim();
                                 _DataRow["LIMITE_PRESUPUESTAL"] = Txt_Tope_Presupuestal.Text.Trim();
                                 Dt_Unidades.Rows.Add(_DataRow);
                                 Llenar_Grid_Unidades_Asignadas();
                                 Llenar_Combo_Unidades_Responsables("Sin_Asignar");
                                 Limpiar_Form();
                                 Habilitar_Controles(MODO_INICIAL);
                                 Grid_Unidades_Responsables.SelectedIndex = -1;
                                 ScriptManager.RegisterStartupScript(
                                     this, this.GetType(), "Requisiciones", "alert('Registro guardado');", true);
                             }
                             else
                             {
                                 ScriptManager.RegisterStartupScript(
                                     this, this.GetType(), "Requisiciones", "alert('No se guardó el registro, consulte al adimistrador del sistema');", true);
                             }
                             Habilitar_Controles(MODO_INICIAL);
                         }
                         else
                         {
                             Mostrar_Informacion("La Unidad Responsable ya fué registrada", true);
                         }
                     }
                     else
                     {
                         Mostrar_Informacion("Seleccione o llene los campos marcados con *", true);
                     }
                 }
             }

             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
             ///DESCRIPCIÓN: 
             ///CREO: Gustavo Angeles
             ///FECHA_CREO: 18 OCT 2011
             ///MODIFICO:
             ///FECHA_MODIFICO:
             ///CAUSA_MODIFICACIÓN:
             ///*******************************************************************************
             protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
             {
                 if (Btn_Modificar.ToolTip == "Modificar")
                 {
                     if (Grid_Unidades_Responsables.SelectedIndex > (-1))
                     {
                         Habilitar_Controles(MODO_MODIFICAR);
                     }
                     else
                     {
                         if (string.IsNullOrEmpty(Txt_Anio_Presupuestal.Text.Trim()))
                         {
                             Mostrar_Informacion("Seleccione Año Presupuestal", true);
                         }
                         else {
                             Mostrar_Informacion("Seleccione Unidad Responsable", true);
                         }
                     }
                 }
                 else
                 {
                     if (Validar())
                     {
                         if (Actualizar())
                         {
                             //Actualizar unidad creada 
                             Llenar_Grid_Unidades_Asignadas();
                             
                             Limpiar_Form();
                             Grid_Unidades_Responsables.SelectedIndex = -1;
                             Habilitar_Controles(MODO_INICIAL);
                             ScriptManager.RegisterStartupScript(
                                 this, this.GetType(), "Requisiciones", "alert('Registro modificado');", true);
                         }
                         else
                         {
                             ScriptManager.RegisterStartupScript(
                                 this, this.GetType(), "Requisiciones", "alert('No se guardó el registro, consulte al adimistrador del sistema');", true);
                         }
                         Habilitar_Controles(MODO_INICIAL);
                     }
                     else
                     {
                         Mostrar_Informacion("Seleccione o llene los campos marcados con *", true);
                     }

                 }
             }

             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Unidad_Click
             ///DESCRIPCIÓN          : Ejecuta la operacion de click en el boton de eliminar una unidad responsable.
             ///PARAMETROS           : 
             ///CREO                 : Leslie González Vázquez
             ///FECHA_CREO           : 09/Noviembre/2011
             ///MODIFICO             :
             ///FECHA_MODIFICO       :
             ///CAUSA_MODIFICACIÓN   :
             ///*******************************************************************************
             protected void Btn_Eliminar_Unidad_Click(object sender, ImageClickEventArgs e)
             {
                 Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio(); //instancia con la capa de negocios
                 String Dependencia_ID = ((ImageButton)sender).CommandArgument.Trim();
                 try
                 {
                     Negocio.P_Anio_Presupuestal = Txt_Anio_Presupuestal.Text.Trim();
                     Negocio.P_Dependencia_ID = Dependencia_ID.Trim();
                     if (Negocio.Eliminar_Limites()) 
                     {
                         Llenar_Grid_Unidades_Asignadas();
                         Llenar_Combo_Unidades_Responsables("Todas");
                         Limpiar_Form();
                         Grid_Unidades_Responsables.SelectedIndex = -1;
                         Habilitar_Controles(MODO_INICIAL);
                         ScriptManager.RegisterStartupScript(
                             this, this.GetType(), "Requisiciones", "alert('Registro eliminado');", true);
                     }
                 }
                 catch (Exception Ex)
                 {
                     throw new Exception("Error al quitar la unidad responsable Error:[" + Ex.Message + "]");
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
             protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
             {
                 Llenar_Grid_Listado_Parametros();
             }


             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN : Cmb_Programas_SelectedIndexChanged
             ///DESCRIPCIÓN          : Evento del combo de programas
             ///PROPIEDADES          :
             ///CREO                 : Leslie González Vázquez
             ///FECHA_CREO           : 14/Noviembre/2011 
             ///MODIFICO             :
             ///FECHA_MODIFICO       :
             ///CAUSA_MODIFICACIÓN...:
             ///******************************************************************************* 
             protected void Cmb_Programas_SelectedIndexChanged(object sender, EventArgs e)
             {
                 try
                 {
                     Llenar_Combo_Programas();
                     Cmb_Programa.Enabled = true;
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("Error en el evento del combo de programas Error[" + ex.Message + "]");
                 }
             }
        #endregion

             #region EVENTOS GRID
             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN : Grid_Listado_Parametros_SelectedIndexChanged
             ///DESCRIPCIÓN          : Ejecuta la operacion de cambio de seleccion en listado 
             ///                       de parametros.
             ///PARAMETROS           : 
             ///CREO                 : Leslie Gonzalez Vázquez
             ///FECHA_CREO           : 07/Noviembre/2011
             ///MODIFICO             :
             ///FECHA_MODIFICO       :
             ///CAUSA_MODIFICACIÓN   :
             ///*******************************************************************************
             protected void Grid_Listado_Parametros_SelectedIndexChanged(object sender, EventArgs e)
             {
                 Cls_Cat_Psp_Parametros_Negocio Parametro = new Cls_Cat_Psp_Parametros_Negocio();
                 try 
                 {
                     if (Grid_Listado_Parametros.SelectedIndex > (-1))
                     {
                         Hdf_Parametro_ID.Value = Grid_Listado_Parametros.SelectedRow.Cells[1].Text.Trim();
                         Parametro.P_Parametro_ID = Hdf_Parametro_ID.Value.Trim();
                         Parametro = Parametro.Consultar_Detalles_Parametro();
                         Limpiar_Form();
                         Hdf_Parametro_ID.Value = Parametro.P_Parametro_ID;
                         Txt_Anio_Presupuestal.Text = Parametro.P_Anio_Presupuestar.ToString();
                         Txt_Fecha_Limite.Text = String.Format("{0:dd/MMM/yyyy}", Parametro.P_Fecha_Cierre);
                         if (String.IsNullOrEmpty(Parametro.P_Fuente_Financiamiento_ID))
                         {
                             Cmb_Fuente_Financiamiento.SelectedIndex = -1;
                         }
                         else
                         {
                             Cmb_Fuente_Financiamiento.SelectedIndex = Cmb_Fuente_Financiamiento.Items.IndexOf(Cmb_Fuente_Financiamiento.Items.FindByValue(Parametro.P_Fuente_Financiamiento_ID));
                         }
                         Llenar_Grid_Unidades_Asignadas();
                         Llenar_Combo_Unidades_Responsables("Todas");
                         Div_Listado_Parametros.Visible = false;
                         Div_Listado_Requisiciones.Visible = true;
                         Txt_Busqueda.Enabled = false;
                         Btn_Buscar.Enabled = false;
                         
                         if (Grid_Listado_Parametros.SelectedRow.Cells[5].Text.Trim() == "INACTIVO")
                         {
                             Btn_Nuevo.Visible = false;
                             Btn_Modificar.Visible = false;
                             Grid_Unidades_Responsables.Columns[5].Visible = false;
                         }
                         else
                         {
                             Habilitar_Controles(MODO_INICIAL);
                             Grid_Unidades_Responsables.Columns[5].Visible = true;
                         }
                     }
                 }
                 catch(Exception ex)
                 {
                     throw new Exception("Error al querer seleccionar un registro del grid de parametros Error[" + ex.Message + "]");
                 }
             }

             ///*******************************************************************************
             ///NOMBRE DE LA FUNCIÓN : Grid_Unidades_Responsables_SelectedIndexChanged
             ///DESCRIPCIÓN          : Ejecuta la operacion de cambio de seleccion en listado 
             ///                       de unidades responsables.
             ///PARAMETROS           : 
             ///CREO                 : Leslie Gonzalez Vázquez
             ///FECHA_CREO           : 08/Noviembre/2011
             ///MODIFICO             :
             ///FECHA_MODIFICO       :
             ///CAUSA_MODIFICACIÓN   :
             ///*******************************************************************************
             protected void Grid_Unidades_Responsables_SelectedIndexChanged(object sender, EventArgs e)
             {
                 Cls_Ope_Psp_Limite_Presupuestal_Negocio Negocio = new Cls_Ope_Psp_Limite_Presupuestal_Negocio(); //instancia con la clase de negocio
                 String Dependencia_ID = String.Empty; ;
                 DataTable Dt_Capitulos = new DataTable();
                 try
                 {
                     Limpiar_Form();
                     Llenar_Combo_Unidades_Responsables("Todas");
                     if (Grid_Unidades_Responsables.SelectedIndex > (-1))
                     {
                         
                         Negocio.P_Anio_Presupuestal = Txt_Anio_Presupuestal.Text.Trim();
                         Negocio.P_Dependencia_ID = HttpUtility.HtmlDecode(Grid_Unidades_Responsables.SelectedRow.Cells[3].Text.Trim()).ToString();
                         Dt_Capitulos = Negocio.Consultar_Capitulos_Asignados_A_Unidad_Asignada();
                         
                         Cmb_Unidad_Responsable.SelectedIndex = Cmb_Unidad_Responsable.Items.IndexOf(Cmb_Unidad_Responsable.Items.FindByValue(Negocio.P_Dependencia_ID));
                         Cmb_Fuente_Financiamiento.SelectedIndex = Cmb_Fuente_Financiamiento.Items.IndexOf(Cmb_Fuente_Financiamiento.Items.FindByValue(Grid_Unidades_Responsables.SelectedRow.Cells[6].Text.Trim()));
                         Llenar_Combo_Programas();
                         Cmb_Programa.SelectedIndex = Cmb_Programa.Items.IndexOf(Cmb_Programa.Items.FindByValue(Grid_Unidades_Responsables.SelectedRow.Cells[7].Text.Trim()));
                         Txt_Tope_Presupuestal.Text = HttpUtility.HtmlDecode(Grid_Unidades_Responsables.SelectedRow.Cells[4].Text.Trim()).ToString();

                         if (Dt_Capitulos is DataTable) {
                             if (Dt_Capitulos.Rows.Count > 0) {
                                 foreach (DataRow CAPITULO in Dt_Capitulos.Rows) {
                                     if (CAPITULO is DataRow) {
                                         if (!String.IsNullOrEmpty(CAPITULO[Ope_Psp_Detalle_Lim_Presup.Campo_Capitulo_ID].ToString())) {
                                             if (Grid_Capitulos is GridView) {
                                                 if (Grid_Capitulos.Rows.Count > 0) {
                                                     foreach (GridViewRow GV_CAPITULO in Grid_Capitulos.Rows) {
                                                         if (GV_CAPITULO is GridViewRow) {
                                                             if (!String.IsNullOrEmpty(GV_CAPITULO.Cells[1].Text)) {
                                                                 if (CAPITULO[Ope_Psp_Detalle_Lim_Presup.Campo_Capitulo_ID].ToString().Trim().Equals(GV_CAPITULO.Cells[1].Text.Trim()))
                                                                 {
                                                                     ((CheckBox)GV_CAPITULO.Cells[0].FindControl("Chk_Capitulo")).Checked = true;
                                                                 }
                                                             }
                                                         }
                                                     }
                                                 }
                                             }
                                         }
                                     }
                                 }
                             }
                         }
                     }
                 }
                 catch (Exception ex)
                 {
                     throw new Exception("Error al querer seleccionar un registro del grid de unidades Responsables Error[" + ex.Message + "]");
                 }
             }
        #endregion
    #endregion
}
