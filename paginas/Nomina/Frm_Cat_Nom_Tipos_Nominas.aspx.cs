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
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Nomina_Frm_Cat_Nom_Tipos_Nominas : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Carga inicial de la pagina, habilita la configuracion inicial
    ///               de los controles de la pagina.
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :Juan Alberto Hernández Negrete
    /// FECHA_MODIFICO    :07/Enero/2011
    /// CAUSA_MODIFICACION: Validaciones
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ViewState["SortDirection"] = "ASC";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 08-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpia_Controles();             //Limpia los controles del forma
            Consulta_Tipos_Nomina();       //Consulta todas los Tipos de Nómina que fueron dadas de alta en la BD     
            Consultar_Percepciones();    //Carga las percepciones que se encuentran actualmente en el sistema
            Consultar_Deducciones();     //Carga las deducciones que se encuentran actualmente en el sistema                    
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
    /// FECHA_CREO  : 08-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Txt_Tipo_Nomina_ID.Text = "";
            Txt_Nomina.Text = "";
            Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text = "";
            Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text = "";
            Txt_Dias_Aguinaldo_Tipo_Nomina.Text = "";
            Txt_Despensa_Tipo_Nomina.Text = "";
            Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text = "";
            Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text = "";
            Txt_Comentarios_Tipo_Nomina.Text = "";
            Cmb_Aplica_ISR.SelectedIndex = -1;
            Txt_Busqueda_Tipo_Nomina.Text = "";
            Cmb_Deducciones.SelectedIndex = 0;
            Cmb_Percepciones.SelectedIndex = 0;
            if (Session["Dt_Percepciones_Grid"] != null) Session.Remove("Dt_Percepciones_Grid");
            if (Session["Dt_Deducciones_Grid"] != null) Session.Remove("Dt_Deducciones_Grid");
            Grid_Deducciones.DataBind();
            Grid_Percepciones.DataBind();

            Grid_Tipos_Nominas.SelectedIndex = -1;

            Cmb_Actualizar_Salario.SelectedIndex = -1;
            Txt_Dias_Prima_Antiguedad.Text = String.Empty;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 08-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Configuracion_Acceso("Frm_Cat_Nom_Tipos_Nominas.aspx");
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    break;
            }
            Txt_Nomina.Enabled = Habilitado;
            Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Enabled = Habilitado;
            Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Enabled = Habilitado;
            Txt_Dias_Aguinaldo_Tipo_Nomina.Enabled = Habilitado;
            Txt_Despensa_Tipo_Nomina.Enabled = Habilitado;
            Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Enabled = Habilitado;
            Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Enabled = Habilitado;
            Txt_Comentarios_Tipo_Nomina.Enabled = Habilitado;
            Cmb_Aplica_ISR.Enabled = Habilitado;
            Txt_Busqueda_Tipo_Nomina.Enabled = !Habilitado;
            Cmb_Deducciones.Enabled = Habilitado;
            Cmb_Percepciones.Enabled = Habilitado;
            Btn_Agregar_Percepciones.Enabled = Habilitado;
            Btn_Agregar_Deducciones.Enabled = Habilitado;
            Btn_Buscar_Tipo_Nomina.Enabled = !Habilitado;
            Grid_Tipos_Nominas.Enabled = !Habilitado;
            Grid_Percepciones.Enabled = Habilitado;
            Grid_Deducciones.Enabled = Habilitado;

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Chk_Aplica_Todos_Empleados_Percepcion.Enabled = false;
            Chk_Aplica_Todos_Empleados_Deduccion.Enabled = false;

            Cmb_Actualizar_Salario.Enabled = Habilitado;
            Txt_Dias_Prima_Antiguedad.Enabled = Habilitado;

            Btn_Agregar_Todo_Percepciones.Enabled = Habilitado;
            Btn_Agregar_Todo_Deducciones.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Construir_Tabla_Conceptos
    /// DESCRIPCION : Construye la tabla que contendra todos los conceptos y que le
    ///               aplicaran a todos los empleados.
    /// 
    /// PARAMETROS  : Dt_Conceptos_Disponibles: Son todos los conceptos de percepcion que se
    ///               encuentran actualmente disponibles.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete.
    /// FECHA_CREO  : 01/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Construir_Tabla_Conceptos(DataTable Dt_Conceptos_Disponibles)
    {
        DataTable Dt_Conceptos = new DataTable();

        try
        {
            Dt_Conceptos.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
            Dt_Conceptos.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(System.String));
            Dt_Conceptos.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(System.String));
            Dt_Conceptos.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));
            Dt_Conceptos.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos, typeof(System.String));

            if (Dt_Conceptos_Disponibles is DataTable)
            {
                if (Dt_Conceptos_Disponibles.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos_Disponibles.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            DataRow Dr_Fila_Agregar = Dt_Conceptos.NewRow();

                            if (!String.IsNullOrEmpty(CONCEPTO[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString().Trim()))
                                Dr_Fila_Agregar[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID] = CONCEPTO[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim()))
                                Dr_Fila_Agregar[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();

                            if (!String.IsNullOrEmpty(CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString().Trim()))
                                Dr_Fila_Agregar[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion].ToString().Trim();

                            Dr_Fila_Agregar[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = "0.00";
                            Dr_Fila_Agregar[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos] = "SI";

                            Dt_Conceptos.Rows.Add(Dr_Fila_Agregar);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al construir la tabla de conceptos cuando aplican todos por tipo de nómina. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Nombre
    /// DESCRIPCION : Junta el nombre del concepto con la clave.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Nombre(DataTable Dt_Conceptos)
    {
        try
        {
            if (Dt_Conceptos is DataTable)
            {
                if (Dt_Conceptos.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos.Rows)
                    {
                        if (CONCEPTO is DataRow)
                        {
                            CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                                "[" + CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Trim() + "] -- " +
                                    CONCEPTO[Cat_Nom_Percepcion_Deduccion.Campo_Nombre].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir el nombre con la clave del concepto. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    #endregion

    #region (Metodos Validacion Percepcion por Concepto Despensa por Tipo Nómina)
    ///***************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Validar_Percepcion_Agregar_Despensa
    /// DESCRIPCION : Consulta y Valida que la percepcion no corresponda a la percepcion otorgada como despensa al
    ///               empleado.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************************************************
    private DataTable Consultar_Validar_Percepcion_Agregar_Despensa(String Identificador_Percepcion_Agregar)
    {
        Cls_Cat_Nom_Parametros_Negocio Parámetros_Nómina = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión copn la capa de negocios.
        DataTable Dt_Parámetros_Nómina = null;//Variable que almacenará el parámetro de la nómina. 
        String Identificador_Parametro_Percepcion_Despensa = "";//Variable que almacenará el identificador que le corresponde a la despensa otorgada por
        //el tipo de nómina a la que pertence el empleado.
        Double Cantidad = 0.0;//Variable que almacenará la cantidad que otorga el tipo de nómina al empleado por concepto de despensa.
        DataTable Dt_Resultado_Es_Despensa = new DataTable();//Variable que almacenará el resultado si la percepción corresponde al concepto por despensa.
        DataRow Registro_Cantidad_Despensa_Otorgar = null;//Almacena el regitro de la cantidad de despensa a otorgar al empleado.

        try
        {
            Dt_Resultado_Es_Despensa.Columns.Add("Es_Despensa", typeof(Boolean));
            Dt_Resultado_Es_Despensa.Columns.Add("Cantidad_Despensa", typeof(Double));

            //Paso I.- Consultamos el parametron de la nomina que le corresponde a Percepcion para la despensa.
            Dt_Parámetros_Nómina = Parámetros_Nómina.Consulta_Parametros();
            if (Dt_Parámetros_Nómina != null)
                if (Dt_Parámetros_Nómina.Rows.Count > 0)
                    if (!string.IsNullOrEmpty(Dt_Parámetros_Nómina.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString()))
                        Identificador_Parametro_Percepcion_Despensa = Dt_Parámetros_Nómina.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString();


            //Paso II.- Validamos si el identificador de la percepción corresponde al identificador de la percepción en la tabla de parámetros
            //          que le corresponde a la percepcion otorgada por el tipo de nómina a la que pertenecé el empleado.  
            if (!string.IsNullOrEmpty(Identificador_Parametro_Percepcion_Despensa))
            {
                if (Identificador_Percepcion_Agregar.Equals(Identificador_Parametro_Percepcion_Despensa))
                {
                    Cantidad = Convert.ToDouble((String.IsNullOrEmpty(Txt_Despensa_Tipo_Nomina.Text.Trim())) ? "0" : Txt_Despensa_Tipo_Nomina.Text.Trim());

                    Registro_Cantidad_Despensa_Otorgar = Dt_Resultado_Es_Despensa.NewRow();
                    Registro_Cantidad_Despensa_Otorgar["Es_Despensa"] = true;
                    Registro_Cantidad_Despensa_Otorgar["Cantidad_Despensa"] = Cantidad;
                    Dt_Resultado_Es_Despensa.Rows.Add(Registro_Cantidad_Despensa_Otorgar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar y validar si la percepcion corresponde a la despensa otorgada por Tipo Nómina. Error: [" + Ex.Message + "]");
        }
        return Dt_Resultado_Es_Despensa;
    }
    ///***************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Cantidad_Despensa_Dentro_Grid_Percepciones
    /// DESCRIPCION : Ejecuta la búsqueda y cambio de cantidad si se encontro agregada la percepcion que corresponde a la percepcion
    ///               otorgada por concepto despensa. Otorgada por el tipo de nómina al que pertence el empleado. 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************************************************************
    private void Modificar_Cantidad_Despensa_Dentro_Grid_Percepciones()
    {
        Cls_Cat_Nom_Parametros_Negocio Parámetros_Nómina = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión copn la capa de negocios.
        DataTable Dt_Parámetros_Nómina = null;//Variable que almacenará el parámetro de la nómina.
        String Identificador_Parametro_Percepcion_Despensa = "";
        String Identificador_Percepcion = "";//Variable que identicará la percepcion que corresponde por concepto de despensa otorgada por el
        //por el tipo de nómina al que pertence el empleado.
        DataTable Dt_Percepciones = (DataTable)Session["Dt_Percepciones_Grid"];//Variable que almacena los datos del grid de percepciones.

        try
        {
            if (Dt_Percepciones is DataTable)
            {
                //Paso I.- Consultamos el parametron de la nomina que le corresponde a Percepcion para la despensa.
                Dt_Parámetros_Nómina = Parámetros_Nómina.Consulta_Parametros();
                if (Dt_Parámetros_Nómina != null)
                {
                    if (Dt_Parámetros_Nómina.Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(Dt_Parámetros_Nómina.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString()))
                        {
                            Identificador_Parametro_Percepcion_Despensa = Dt_Parámetros_Nómina.Rows[0][Cat_Nom_Parametros.Campo_Percepcion_Despensa].ToString();

                            foreach (DataRow Percepcion in Dt_Percepciones.Rows)
                            {
                                //Obtenemos los identificadores de las percepciones para posteriormente realizar la validación.
                                Identificador_Percepcion = Percepcion[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID].ToString();

                                if (!string.IsNullOrEmpty(Identificador_Percepcion))
                                {
                                    //Se realiza la comparativa de los identicadores, y si se encontro alguna coincidencia 
                                    //se realiza la actuañlizacion de la información. En la cantidad que respecta al concepto  de despensa. 
                                    //Otorgtado por el tipo de nomina al que pertence el empleado.
                                    if (Identificador_Percepcion.Equals(Identificador_Parametro_Percepcion_Despensa))
                                    {
                                        Percepcion[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = Convert.ToDouble(Txt_Despensa_Tipo_Nomina.Text.Trim());
                                        Dt_Percepciones.AcceptChanges();
                                        Session["Dt_Percepciones_Grid"] = Dt_Percepciones;
                                        Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al consultar el parámetro de la nómina que corresponde al concepto de Despensa otorgado, por el tipo de nómina del empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Manejo Cantidades Grids)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_DataTable_Percepciones_Deducciones
    /// DESCRIPCION : Crea un datatable con la informacion de del id de la percepcion
    ///               y la cantidad asignada para la percepcion.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Crear_DataTable_Percepciones_Deducciones(GridView _GridView, String TextBox_ID)
    {
        DataTable Dt_Percepciones_Deducciones = new DataTable();
        Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
        Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));
        Dt_Percepciones_Deducciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos, typeof(System.String));
        DataRow Renglon;

        try
        {
            for (int index = 0; index < _GridView.Rows.Count; index++)
            {
                Renglon = Dt_Percepciones_Deducciones.NewRow();
                Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID] = _GridView.Rows[index].Cells[0].Text;
                Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = ((TextBox)_GridView.Rows[index].Cells[3].FindControl(TextBox_ID)).Text;
                Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos] = _GridView.Rows[index].Cells[5].Text;
                Dt_Percepciones_Deducciones.Rows.Add(Renglon);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Dt_Percepciones_Deducciones;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Cantidad_Grid_Percepciones_Deducciones
    /// DESCRIPCION : Carga la cantidad correspodiente a la percepcion o deduccion 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Cantidad_Grid_Percepciones_Deducciones(GridView Grid_Percepcion_Deduccion, DataTable Dt_Datos_Consultados, String TextBox_ID)
    {
        int index = 0;
        try
        {
            foreach (DataRow Renglon in Dt_Datos_Consultados.Rows)
            {
                TextBox Txt_Cantidad = (TextBox)Grid_Percepcion_Deduccion.Rows[index].Cells[3].FindControl(TextBox_ID);
                Txt_Cantidad.Text = String.Format("{0:#,###,##0.00}", Convert.ToDouble((string.IsNullOrEmpty(Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()) || Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString().Equals("$  _,___,___.__")) ? "0" : Renglon[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad].ToString()));
                index = index + 1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar Combo de Percepciones Deducciones
    /// DESCRIPCION : Carga las Percepciones Deducciones Fijas o Variables que no son calculadas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Percepciones()
    {
        DataTable Dt_Percepciones = null;
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        try
        {
            Cat_Percepciones.P_TIPO = "PERCEPCION";
            Cat_Percepciones.P_Concepto = "TIPO_NOMINA";
            Cat_Percepciones.P_ESTATUS = "ACTIVO";
            Dt_Percepciones = Cat_Percepciones.Consultar_Percepciones_Deducciones_General();
            Session["Dt_Percepciones_Combo"] = Dt_Percepciones;
            Cmb_Percepciones.DataSource = Dt_Percepciones;
            Cmb_Percepciones.DataTextField = "NOMBRE_CONCEPTO";
            Cmb_Percepciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Percepciones.DataBind();
            Cmb_Percepciones.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Percepciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Percepciones Deducciones. Error: [" + Ex.Message + "]");
        }

    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar Combo de Percepciones Deducciones
    /// DESCRIPCION : Carga las Percepciones Deducciones Fijas o Variables que no son calculadas
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones()
    {
        DataTable Dt_Deducciones = null;
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        try
        {
            Cat_Deducciones.P_TIPO = "DEDUCCION";
            Cat_Deducciones.P_Concepto = "TIPO_NOMINA";
            Cat_Deducciones.P_ESTATUS = "ACTIVO";
            Dt_Deducciones = Cat_Deducciones.Consultar_Percepciones_Deducciones_General();
            Session["Dt_Deducciones_Combo"] = Dt_Deducciones;
            Cmb_Deducciones.DataSource = Dt_Deducciones;
            Cmb_Deducciones.DataTextField = "NOMBRE_CONCEPTO";
            Cmb_Deducciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Deducciones.DataBind();
            Cmb_Deducciones.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Deducciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al consultar las Deducciones. Error: [" + Ex.Message + "]");
        }

    }
    #endregion

    #region (Metodos de Operacion [Alta - Modificar - Eliminar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Tipo_Nomina
    /// DESCRIPCION : Da de Alta el Tipo de Nómina con los datos proporcionados por el 
    ///               usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 08-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Tipo_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Rs_Alta_Cat_Nom_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Nomina = Txt_Nomina.Text;
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Dias_Prima_Vacacional_1 = Convert.ToDouble(Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Dias_Prima_Vacacional_2 = Convert.ToDouble(Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Dias_Aguinaldo = Convert.ToDouble(Txt_Dias_Aguinaldo_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Dias_Exenta_Prima_Vacacional = Convert.ToDouble(Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Dias_Exenta_Aguinaldo = Convert.ToDouble(Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Despensa = Convert.ToDouble(Txt_Despensa_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Comentarios = Convert.ToString(Txt_Comentarios_Tipo_Nomina.Text);
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Aplica_ISR = Cmb_Aplica_ISR.SelectedItem.Text.Trim();
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Actualizar_Salario = Cmb_Actualizar_Salario.SelectedItem.Text.Trim();
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Dias_Prima_Antiguedad = Txt_Dias_Prima_Antiguedad.Text.Trim();

            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Percepciones_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion");
            Rs_Alta_Cat_Nom_Tipos_Nominas.P_Deducciones_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion");

            Rs_Alta_Cat_Nom_Tipos_Nominas.Alta_Tipo_Nomina(); //Da de alta los datos del Tipo de Nómina proporcionados por el usuario en la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Nóminas", "alert('El Alta del Tipo de Nomina fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Tipo_Nomina " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Tipo_Nomina
    /// DESCRIPCION : Modifica los datos del Tipo de Nómina con los proporcionados 
    ///               por el usuario en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 08-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Tipo_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Rs_Modificar_Cat_Nom_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Tipo_Nomina_ID = Convert.ToString(Txt_Tipo_Nomina_ID.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Nomina = Convert.ToString(Txt_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Dias_Prima_Vacacional_1 = Convert.ToDouble(Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Dias_Prima_Vacacional_2 = Convert.ToDouble(Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Dias_Aguinaldo = Convert.ToDouble(Txt_Dias_Aguinaldo_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Dias_Exenta_Prima_Vacacional = Convert.ToDouble(Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Dias_Exenta_Aguinaldo = Convert.ToDouble(Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Despensa = Convert.ToDouble(Txt_Despensa_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Comentarios = Convert.ToString(Txt_Comentarios_Tipo_Nomina.Text);
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Aplica_ISR = Cmb_Aplica_ISR.SelectedItem.Text.Trim();
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Actualizar_Salario = Cmb_Actualizar_Salario.SelectedItem.Text.Trim();
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Dias_Prima_Antiguedad = Txt_Dias_Prima_Antiguedad.Text.Trim();

            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Percepciones_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion");
            Rs_Modificar_Cat_Nom_Tipos_Nominas.P_Deducciones_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion");

            Rs_Modificar_Cat_Nom_Tipos_Nominas.Modificar_Tipo_Nomina(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Nóminas", "alert('La Modificación del Tipo de Nómina fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Tipo_Nomina " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Tipo_Nomina
    /// DESCRIPCION : Elimina los datos del Tipo de Nómina que fue seleccionada por el
    ///               Usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 08-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Tipo_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Rs_Eliminar_Cat_Nom_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Cat_Nom_Tipos_Nominas.P_Tipo_Nomina_ID = Txt_Tipo_Nomina_ID.Text;
            //Rs_Eliminar_Cat_Nom_Tipos_Nominas.P_Percepciones_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Percepciones, "Txt_Cantidad_Percepcion");
            //Rs_Eliminar_Cat_Nom_Tipos_Nominas.P_Deducciones_Nomina = Crear_DataTable_Percepciones_Deducciones(Grid_Deducciones, "Txt_Cantidad_Deduccion");

            Rs_Eliminar_Cat_Nom_Tipos_Nominas.Eliminar_Tipo_Nomina(); //Elimina el tipo de nómina que selecciono el usuario de la BD
            Inicializa_Controles();                                   //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tipos de Nóminas", "alert('La Eliminación del Tipo de Nómina fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Tipo_Nomina " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar_Tipo_Nomina);

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
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
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

    #endregion

    #region (Grid)

    #region (GridView Tipos Nomina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Tipos_Nominas_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del Tipo de Nomina que selecciono el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Tipos_Nominas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Tipos_Nominas_Negocio Rs_Consulta_Cat_Nom_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_Tipos_Nominas; //Variable que obtendra los datos de la consulta

        try
        {
            Consultar_Percepciones_Tipos_Nominas();
            Consultar_Deducciones_Tipos_Nomina();

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Rs_Consulta_Cat_Nom_Tipos_Nomina.P_Tipo_Nomina_ID = Grid_Tipos_Nominas.SelectedRow.Cells[1].Text;
            Dt_Tipos_Nominas = Rs_Consulta_Cat_Nom_Tipos_Nomina.Consulta_Datos_Tipo_Nomina(); //Consulta los datos del Tipo de Nomina que fue seleccionado por el usuario

            if (Dt_Tipos_Nominas is DataTable)
            {
                if (Dt_Tipos_Nominas.Rows.Count > 0)
                {
                    //Agrega los valores de los campos a los controles correspondientes de la forma
                    foreach (DataRow TIPO_NOMINA in Dt_Tipos_Nominas.Rows)
                    {
                        if (TIPO_NOMINA is DataRow)
                        {
                            Txt_Tipo_Nomina_ID.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString();
                            Txt_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();
                            Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString();
                            Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString();
                            Txt_Dias_Aguinaldo_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Aguinaldo].ToString();
                            Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Prima_Vacacional].ToString();
                            Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Exenta_Aguinaldo].ToString();
                            Txt_Despensa_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Despensa].ToString();
                            Txt_Comentarios_Tipo_Nomina.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Comentarios].ToString();
                            Cmb_Aplica_ISR.SelectedIndex = Cmb_Aplica_ISR.Items.IndexOf(Cmb_Aplica_ISR.Items.FindByText(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Aplica_ISR].ToString()));
                            Cmb_Actualizar_Salario.SelectedIndex = Cmb_Actualizar_Salario.Items.IndexOf(Cmb_Actualizar_Salario.Items.FindByText(TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Actualizar_Salario].ToString()));
                            Txt_Dias_Prima_Antiguedad.Text = TIPO_NOMINA[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Antiguedad].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Tipos_Nominas_PageIndexChanging
    /// DESCRIPCION : Cambia la pagina del Grid de los tipos de nomina.
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Tipos_Nominas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpia_Controles();                        //Limpia todos los controles de la forma
            Grid_Tipos_Nominas.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Tipos_Nominas();                   //Carga los Tipos de Nomina que estan asignadas a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Tipos_Nomina
    /// DESCRIPCION : Consulta los Tipos de Nomina que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Tipos_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Rs_Consulta_Cat_Nom_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Nominas; //Variable que obtendra los datos de la consulta 

        try
        {
            if (Txt_Busqueda_Tipo_Nomina.Text != "")
            {
                Rs_Consulta_Cat_Nom_Tipos_Nomina.P_Nomina = Txt_Busqueda_Tipo_Nomina.Text;
            }
            Dt_Nominas = Rs_Consulta_Cat_Nom_Tipos_Nomina.Consulta_Tipos_Nominas(); //Consulta todos los Tipos de Nomina con sus datos generales
            Session["Consulta_Nominas"] = Dt_Nominas;
            Llena_Grid_Tipos_Nominas();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Tipos_Nomina " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Tipos_Nominas
    /// DESCRIPCION : Llena el grid con los Tipos de Nomina que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Tipos_Nominas()
    {
        DataTable Dt_Nominas; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Tipos_Nominas.DataBind();
            Dt_Nominas = (DataTable)Session["Consulta_Nominas"];
            Grid_Tipos_Nominas.DataSource = Dt_Nominas;
            Grid_Tipos_Nominas.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Tipos_Nominas " + ex.Message.ToString(), ex);
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Tipos_Nominas_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Tipos_Nominas_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consulta_Tipos_Nomina();
        DataTable Dt_Tipos_Nominas = (Grid_Tipos_Nominas.DataSource as DataTable);

        if (Dt_Tipos_Nominas != null)
        {
            DataView Dv_Calendario_Nominas = new DataView(Dt_Tipos_Nominas);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Calendario_Nominas.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Tipos_Nominas.DataSource = Dv_Calendario_Nominas;
            Grid_Tipos_Nominas.DataBind();
        }
    }
    #endregion

    #region (GridView Percepciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Percepciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Percepciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Percepcion")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Percepcion")).ToolTip = "Quitar la Percepcion " + e.Row.Cells[1].Text + " al Tipo de Nomina";
                ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Percepcion")).ToolTip = "" + e.Row.RowIndex;
                if (e.Row.Cells[2].Text.Equals("OPERACION") ||
                    e.Row.Cells[2].Text.Equals("VARIABLE"))
                {
                    e.Row.Cells[3].Enabled = false;
                    ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Percepcion")).Style.Add("display", "none");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Tipos_Nominas
    /// DESCRIPCION : Consultar las Percepciones del Tipo de Nomina Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Percepciones_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Rs_Consulta_Cat_Nom_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();
        Int32 Tipo_Nomina_Seleccionada = Grid_Tipos_Nominas.SelectedIndex;
        DataTable Dt_Percepciones_Nomina;

        if (Tipo_Nomina_Seleccionada != -1)
        {
            Rs_Consulta_Cat_Nom_Tipos_Nominas.P_Tipo_Nomina_ID = Grid_Tipos_Nominas.Rows[Tipo_Nomina_Seleccionada].Cells[1].Text;
            Rs_Consulta_Cat_Nom_Tipos_Nominas.P_Tipo = "PERCEPCION";
            Dt_Percepciones_Nomina = Rs_Consulta_Cat_Nom_Tipos_Nominas.Consulta_Percepciones_Deducciones_Nomina();
            Dt_Percepciones_Nomina = Juntar_Clave_Nombre(Dt_Percepciones_Nomina);
            Session["Dt_Percepciones_Grid"] = Dt_Percepciones_Nomina;

            Grid_Percepciones.Columns[0].Visible = true;
            Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
            Grid_Percepciones.DataBind();
            Grid_Percepciones.Columns[0].Visible = false;

            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
        }
    }
    #endregion

    #region (GridView Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Deducciones_RowDataBound
    /// DESCRIPCION : Agrega un identificador al boton de cancelar de la tabla
    /// para identicar la fila seleccionada de tabla.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Deducciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Deduccion")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[4].FindControl("Btn_Delete_Deduccion")).ToolTip = "Quitar la Deduccion " + e.Row.Cells[1].Text + " al Tipo de Nomina";
                ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Deduccion")).ToolTip = "" + e.Row.RowIndex;
                if (e.Row.Cells[2].Text.Equals("OPERACION") ||
                    e.Row.Cells[2].Text.Equals("VARIABLE"))
                {
                    e.Row.Cells[3].Enabled = false;
                    ((TextBox)e.Row.Cells[3].FindControl("Txt_Cantidad_Deduccion")).Style.Add("display", "none");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Tipos_Nomina
    /// DESCRIPCION : Consultar las Deducciones del Tipo Nomina Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones_Tipos_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Cat_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();
        Int32 Tipo_Nomina_Seleccionada = Grid_Tipos_Nominas.SelectedIndex;
        DataTable Dt_Deducciones_Nomina;

        if (Tipo_Nomina_Seleccionada != -1)
        {
            Cat_Tipos_Nominas.P_Tipo_Nomina_ID = Grid_Tipos_Nominas.Rows[Tipo_Nomina_Seleccionada].Cells[1].Text;
            Cat_Tipos_Nominas.P_Tipo = "DEDUCCION";
            Dt_Deducciones_Nomina = Cat_Tipos_Nominas.Consulta_Percepciones_Deducciones_Nomina();
            Dt_Deducciones_Nomina = Juntar_Clave_Nombre(Dt_Deducciones_Nomina);
            Session["Dt_Deducciones_Grid"] = Dt_Deducciones_Nomina;

            Grid_Deducciones.Columns[0].Visible = true;
            Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
            Grid_Deducciones.DataBind();
            Grid_Deducciones.Columns[0].Visible = false;
            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos [Agregar- Elimnar Percepciones al Grid Percepciones])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Percepcion
    /// DESCRIPCION : Agrega una nueva percepcion a la tabla de percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Percepcion(object sender, EventArgs e)
    {
        if (Cmb_Percepciones.SelectedIndex > 0)
        {
            if (Session["Dt_Percepciones_Grid"] != null)
            {
                Agregar_Percepcion((DataTable)Session["Dt_Percepciones_Grid"], Grid_Percepciones, Cmb_Percepciones);
            }
            else
            {
                DataTable Dt_Percepciones = new DataTable();
                Dt_Percepciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));
                Dt_Percepciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos, typeof(System.String));

                Session["Dt_Percepciones_Grid"] = Dt_Percepciones;

                Grid_Percepciones.Columns[0].Visible = true;
                Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
                Grid_Percepciones.DataBind();
                Grid_Percepciones.Columns[0].Visible = false;

                Agregar_Percepcion(Dt_Percepciones, Grid_Percepciones, Cmb_Percepciones);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Percepcion
    /// DESCRIPCION : Agrega una nueva percepcion a la tabla de percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Percepcion(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Percepciones_Grid"];
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        Double Cantidad_Despensa = 0.0;//Variable que almacenará la cantidad de despensa otorgada por el tipo de nómina al que pertencé el empleado.
        DataTable Dt_Cantidad_Despensa_Otorgar_TN = null;//Estructura que almacenará la cantidad que recibira el empleado por concepto de Despensa por tipo de nómina.

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la Percepción, ya que esta ya se ha agregado');", true);
                    Cmb_Percepciones.SelectedIndex = 0;
                }
                else
                {
                    DataTable Dt_Temporal = Cat_Percepciones_Deducciones.Busqueda_Percepcion_Deduccion_Por_ID(_DropDownList.SelectedValue.Trim());
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow row = Dt.NewRow();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Dt_Temporal.Rows[0][0].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" + Dt_Temporal.Rows[0][15].ToString() + "] - " + Dt_Temporal.Rows[0][1].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = Dt_Temporal.Rows[0][5].ToString();
                            row[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos] = (Chk_Aplica_Todos_Empleados_Percepcion.Checked) ? "SI" : "NO";

                            //Este proceso consulta y valida que la percepión agregada no corresponda a la percepción otorgada por el tipo de nómina
                            //a la que pertence el empleado.
                            Dt_Cantidad_Despensa_Otorgar_TN = Consultar_Validar_Percepcion_Agregar_Despensa(Dt_Temporal.Rows[0][0].ToString());
                            if (Dt_Cantidad_Despensa_Otorgar_TN != null)
                            {
                                if (Dt_Cantidad_Despensa_Otorgar_TN.Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(Dt_Cantidad_Despensa_Otorgar_TN.Rows[0]["Es_Despensa"].ToString()))
                                    {
                                        if (Convert.ToBoolean(Dt_Cantidad_Despensa_Otorgar_TN.Rows[0]["Es_Despensa"].ToString()))
                                        {
                                            if (!string.IsNullOrEmpty(Dt_Cantidad_Despensa_Otorgar_TN.Rows[0]["Cantidad_Despensa"].ToString()))
                                            {
                                                Cantidad_Despensa = Convert.ToDouble(Dt_Cantidad_Despensa_Otorgar_TN.Rows[0]["Cantidad_Despensa"].ToString());
                                                row[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] = Cantidad_Despensa;
                                            }
                                        }
                                    }
                                }
                            }

                            Dt.Rows.Add(row);
                            Dt.AcceptChanges();
                            Session["Dt_Percepciones_Grid"] = Dt;
                            _GridView.Columns[0].Visible = true;
                            _GridView.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
                            _GridView.DataBind();
                            _GridView.Columns[0].Visible = false;
                            Cmb_Percepciones.SelectedIndex = 0;

                            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna percepcion a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar la percepcion al Grid de Percepciones" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Delete_Percepcion
    /// DESCRIPCION : Elimina la fila seleccionada del Grid de Percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Delete_Percepcion(object sender, EventArgs e)
    {
        ImageButton Btn_Eliminar_Percepcion = (ImageButton)sender;
        DataTable Dt_Percepciones = (DataTable)Session["Dt_Percepciones_Grid"];
        DataRow[] Filas = Dt_Percepciones.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                "='" + Btn_Eliminar_Percepcion.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Percepciones.Rows.Remove(Filas[0]);
                Session["Dt_Percepciones_Grid"] = Dt_Percepciones;
                Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];

                Grid_Percepciones.Columns[0].Visible = true;
                Grid_Percepciones.DataBind();
                Cmb_Percepciones.SelectedIndex = 0;
                Grid_Percepciones.Columns[0].Visible = false;

                Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
            }
        }
    }
    #endregion

    #region (Eventos [Agregar - Eliminar Deducciones al Grid Deducciones])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Deduccion
    /// DESCRIPCION : Agrega una nueva deduccion a la tabla de deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Deduccion(object sender, EventArgs e)
    {
        if (Cmb_Deducciones.SelectedIndex > 0)
        {
            if (Session["Dt_Deducciones_Grid"] != null)
            {
                Agregar_Deduccion((DataTable)Session["Dt_Deducciones_Grid"], Grid_Deducciones, Cmb_Deducciones);

            }
            else
            {
                DataTable Dt_Deducciones = new DataTable();
                Dt_Deducciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Nombre, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad, typeof(System.String));
                Dt_Deducciones.Columns.Add(Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos, typeof(System.String));

                Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
                Grid_Deducciones.Columns[0].Visible = true;
                Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                Grid_Deducciones.DataBind();
                Grid_Deducciones.Columns[0].Visible = false;

                Agregar_Deduccion(Dt_Deducciones, Grid_Deducciones, Cmb_Deducciones);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Deduccion
    /// DESCRIPCION : Agrega una nueva deduccion a la tabla de deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Deduccion(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Deducciones_Grid"];
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones = new Cls_Cat_Nom_Percepciones_Deducciones_Business();

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar la Deduccion, ya que esta ya se ha agregado');", true);
                    Cmb_Deducciones.SelectedIndex = 0;
                }
                else
                {
                    DataTable Dt_Temporal = Cat_Percepciones_Deducciones.Busqueda_Percepcion_Deduccion_Por_ID(_DropDownList.SelectedValue.Trim());
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            DataRow row = Dt.NewRow();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID] = Dt_Temporal.Rows[0][0].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] = "[" + Dt_Temporal.Rows[0][15].ToString() + "] -- " + Dt_Temporal.Rows[0][1].ToString();
                            row[Cat_Nom_Percepcion_Deduccion.Campo_Tipo_Asignacion] = Dt_Temporal.Rows[0][5].ToString();
                            row[Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Aplica_Todos] = (Chk_Aplica_Todos_Empleados_Deduccion.Checked) ? "SI" : "NO";

                            Dt.Rows.Add(row);
                            Dt.AcceptChanges();
                            Session["Dt_Deducciones_Grid"] = Dt;
                            _GridView.Columns[0].Visible = true;

                            _GridView.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                            _GridView.DataBind();
                            Cmb_Deducciones.SelectedIndex = 0;
                            _GridView.Columns[0].Visible = false;

                            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ninguna deduccion a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar la deduccion al Grid de Deducciones" + Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Delete_Deduccion
    /// DESCRIPCION : Elimina la fila seleccionada del Grid de Deducciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Delete_Deduccion(object sender, EventArgs e)
    {
        ImageButton Btn_Eliminar_Deduccion = (ImageButton)sender;
        DataTable Dt_Deducciones = (DataTable)Session["Dt_Deducciones_Grid"];
        DataRow[] Filas = Dt_Deducciones.Select(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID +
                "='" + Btn_Eliminar_Deduccion.CommandArgument + "'");

        if (!(Filas == null))
        {
            if (Filas.Length > 0)
            {
                Dt_Deducciones.Rows.Remove(Filas[0]);
                Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
                Grid_Deducciones.Columns[0].Visible = true;
                Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
                Grid_Deducciones.DataBind();
                Cmb_Deducciones.SelectedIndex = 0;
                Grid_Deducciones.Columns[0].Visible = false;

                Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
            }
        }
    }
    #endregion

    #region (Eventos TextBox Cantidad Inner GridView Percepciones y Deducciones)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cantidad_Percepcion_TextChanged
    /// DESCRIPCION : Actualiza la informacion del DataTable de Percepciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cantidad_Percepcion_TextChanged(object sender, EventArgs e)
    {
        TextBox _Txt_Cantidad_Percepcion = (TextBox)sender;
        if (Session["Dt_Percepciones_Grid"] != null)
        {
            DataTable Dt_Percepciones = ((DataTable)Session["Dt_Percepciones_Grid"]);
            Dt_Percepciones.DefaultView.AllowEdit = true;
            Dt_Percepciones.Rows[Convert.ToInt32(_Txt_Cantidad_Percepcion.ToolTip)].BeginEdit();
            Dt_Percepciones.Rows[Convert.ToInt32(_Txt_Cantidad_Percepcion.ToolTip)][Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] =
                (_Txt_Cantidad_Percepcion.Text.Equals("") || _Txt_Cantidad_Percepcion.Text.Equals("$  _,___,___.__") || _Txt_Cantidad_Percepcion.Text.Contains("$")) ? "0" : _Txt_Cantidad_Percepcion.Text;
            Dt_Percepciones.Rows[Convert.ToInt32(_Txt_Cantidad_Percepcion.ToolTip)].EndEdit();

            Session["Dt_Percepciones_Grid"] = Dt_Percepciones;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Cantidad_Deduccion_TextChanged
    /// DESCRIPCION : Actualiza la informacion del DataTable de Deducciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 11/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cantidad_Deduccion_TextChanged(object sender, EventArgs e)
    {
        TextBox _Txt_Cantidad_Deduccion = (TextBox)sender;
        if (Session["Dt_Deducciones_Grid"] != null)
        {
            DataTable Dt_Deducciones = ((DataTable)Session["Dt_Deducciones_Grid"]);
            Dt_Deducciones.DefaultView.AllowEdit = true;
            Dt_Deducciones.Rows[Convert.ToInt32(_Txt_Cantidad_Deduccion.ToolTip)].BeginEdit();
            Dt_Deducciones.Rows[Convert.ToInt32(_Txt_Cantidad_Deduccion.ToolTip)][Cat_Nom_Tipos_Nomina_Percepciones_Deducciones_Detalles.Campo_Cantidad] =
                (_Txt_Cantidad_Deduccion.Text.Equals("") || _Txt_Cantidad_Deduccion.Text.Equals("$  _,___,___.__") || _Txt_Cantidad_Deduccion.Text.Contains("$")) ? "0" : _Txt_Cantidad_Deduccion.Text;
            Dt_Deducciones.Rows[Convert.ToInt32(_Txt_Cantidad_Deduccion.ToolTip)].EndEdit();

            Session["Dt_Deducciones_Grid"] = Dt_Deducciones;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Despensa_Tipo_Nomina_TextChanged
    /// DESCRIPCION : Si se hace algun cambio en la cantidad destinada para la percepcion que otorga el tipo de nómina al empleado.
    ///               este cambio debe realizarse tambien el la percepcion que se encuentra agregada al grid de percepciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Enero/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Despensa_Tipo_Nomina_TextChanged(object sender, EventArgs e)
    {
        try
        {
            //Si se hace algun cambio en la cantidad destinada para la percepcion que otorga el tipo de nómina al empleado.
            //este cambio debe realizarse tambien el la percepcion que se encuentra agregada al grid de percepciones.
            Modificar_Cantidad_Despensa_Dentro_Grid_Percepciones();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Eventos Operacion [Alta - Modificar - Eliminar])
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        String Espacios = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"; //Variable que contiene la cantidad de espacios a considerar para dejar de sangria a los mensajes
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Limpia_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Nomina.Text != "" && Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text != "" &&
                    Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text != "" &&
                    Txt_Dias_Aguinaldo_Tipo_Nomina.Text != "" && Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text != "" &&
                    Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text != "" && Txt_Despensa_Tipo_Nomina.Text != "" &&
                    Txt_Comentarios_Tipo_Nomina.Text.Length <= 250 &&
                    Cmb_Aplica_ISR.SelectedIndex > 0)
                {
                    Alta_Tipo_Nomina(); //Da de alta los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Nombre del Tipo de Nómina <br>";
                    }
                    if (Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días de la Prima Vacacional para el Primer Periodo <br>";
                    }
                    if (Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días de la Prima Vacacional para el Segundo Periodo <br>";
                    }
                    if (Txt_Dias_Aguinaldo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días de Aguinaldo <br>";
                    }
                    if (Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Dias que Exenta la Prima Vacacional <br>";
                    }
                    if (Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días que Exenta el Aguinaldo <br>";
                    }
                    if (Txt_Despensa_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Cantidad en Despesa a Otorgar <br>";
                    }
                    if (Txt_Comentarios_Tipo_Nomina.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
                    }
                    if (Cmb_Aplica_ISR.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Aplica ISR <br>";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        String Espacios = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"; //Variable que contiene la cantidad de espacios a considerar para dejar de sangria a los mensajes
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_Tipo_Nomina_ID.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Nómina que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                if (Txt_Nomina.Text != "" && Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text != "" &&
                    Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text != "" &&
                    Txt_Dias_Aguinaldo_Tipo_Nomina.Text != "" && Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text != "" &&
                    Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text != "" && Txt_Despensa_Tipo_Nomina.Text != "" &&
                    Txt_Comentarios_Tipo_Nomina.Text.Length <= 250 &&
                    Cmb_Aplica_ISR.SelectedIndex > 0)
                {
                    Modificar_Tipo_Nomina(); //Modifica los datos del Tipo de Nómina con los datos proporcionados por el usuario
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
                    if (Txt_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Nombre del Tipo de Nómina <br>";
                    }
                    if (Txt_Dias_Primera_Vacacional_Primer_Periodo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días de la Prima Vacacional para el Primer Periodo <br>";
                    }
                    if (Txt_Dias_Primera_Vacacional_Segundo_Periodo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días de la Prima Vacacional para el Segundo Periodo <br>";
                    }
                    if (Txt_Dias_Aguinaldo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días de Aguinaldo <br>";
                    }
                    if (Txt_Dias_Exenta_Primera_Vacacional_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Dias que Exenta la Prima Vacacional <br>";
                    }
                    if (Txt_Dias_Exenta_Aguinaldo_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Días que Exenta el Aguinaldo <br>";
                    }
                    if (Txt_Despensa_Tipo_Nomina.Text == "")
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Cantidad en Despesa a Otorgar <br>";
                    }
                    if (Txt_Comentarios_Tipo_Nomina.Text.Length > 250)
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
                    }
                    if (Cmb_Aplica_ISR.SelectedIndex <= 0)
                    {
                        Lbl_Mensaje_Error.Text += Espacios + " + Aplica ISR <br>";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Turno entonces la elimina de la base de datos
            if (Txt_Tipo_Nomina_ID.Text != "")
            {
                Eliminar_Tipo_Nomina(); //Elimina el Tipo de Nómina que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algun Tipo de Nómina manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Nómina que desea eliminar <br>";
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
            Session.Remove("Percepciones_Nomina");
            Session.Remove("Deducciones_Nomina");
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Nominas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Buscar_Tipo_Nomina_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Tipos_Nomina(); //Consulta los tipos de nómina que coincidan con la descipción porporcionada por el usuario
            Limpia_Controles(); //Limpia los controles de la forma
            //Si no se encontraron Tipos de Nómina con el Nombre similar proporcionado por el usuario entonces manda un mensaje al usuario
            if (Grid_Tipos_Nominas.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Tipos de Nómina con el Nombre proporcionado <br>";
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

    #region (Agregar Todos los conceptos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Todo_Percepciones_Click
    /// DESCRIPCION : agrega todas las percepciones disponibles al tipo de nomina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Julio/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Todo_Percepciones_Click(object sender, EventArgs e)
    {
        DataTable Dt_Percepciones = null;

        try
        {
            Session["Dt_Percepciones_Grid"] = null;
            Dt_Percepciones = (DataTable)Session["Dt_Percepciones_Combo"];
            Dt_Percepciones = Construir_Tabla_Conceptos(Dt_Percepciones);
            Session["Dt_Percepciones_Grid"] = Dt_Percepciones;

            Grid_Percepciones.Columns[0].Visible = true;
            Grid_Percepciones.DataSource = (DataTable)Session["Dt_Percepciones_Grid"];
            Grid_Percepciones.DataBind();
            Grid_Percepciones.SelectedIndex = -1;
            Grid_Percepciones.Columns[0].Visible = false; 

            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Percepciones, (DataTable)Session["Dt_Percepciones_Grid"], "Txt_Cantidad_Percepcion");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }


    protected void Btn_Agregar_Todo_Deducciones_Click(object sender, EventArgs e) {
        DataTable Dt_Deducciones = null;

        try
        {
            Session["Dt_Deducciones_Grid"] = null;
            Dt_Deducciones = (DataTable)Session["Dt_Deducciones_Combo"];
            Dt_Deducciones = Construir_Tabla_Conceptos(Dt_Deducciones);
            Session["Dt_Deducciones_Grid"] = Dt_Deducciones;

            Grid_Deducciones.Columns[0].Visible = true;
            Grid_Deducciones.DataSource = (DataTable)Session["Dt_Deducciones_Grid"];
            Grid_Deducciones.DataBind();
            Grid_Deducciones.SelectedIndex = -1;
            Grid_Deducciones.Columns[0].Visible = false;

            Cargar_Cantidad_Grid_Percepciones_Deducciones(Grid_Deducciones, (DataTable)Session["Dt_Deducciones_Grid"], "Txt_Cantidad_Deduccion");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #endregion

}
