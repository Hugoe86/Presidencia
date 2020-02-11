using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Ayudante_JQuery;
using System.Collections.Specialized;
using System.Data;
using Presidencia.Puestos.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Creacion_Plazas.Negocio;

public partial class paginas_Nomina_Frm_Cat_Nom_Creacion_Plazas_Ctrl : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Controlador_Peticiones_Cliente();
        }
    }
    #endregion

    #region (Controlador Peticiones)
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Controlador_Peticiones_Cliente
    ///DESCRIPCIÓN          : Metodo para tener el control de las peticiones del cliente
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private void Controlador_Peticiones_Cliente()
    {
        String Str_Respuesta_JSON = String.Empty;
        String Str_Respuesta_Texto_Plano = String.Empty;
        String Opcion = String.Empty;
        String Tabla = String.Empty;
        String Pagina = String.Empty;
        String Filas = String.Empty;
        String ur_q = String.Empty;

        String Fte_Financiamiento_ID = String.Empty;
        String Area_Funcional_ID = String.Empty;
        String Proyecto_Programa_ID = String.Empty;
        String Unidad_Responsable_ID = String.Empty;
        String Partida_ID = String.Empty;
        String Puesto_ID = String.Empty;

        String Tipo_Plaza = String.Empty;
        String Estatus = String.Empty;
        String Empleado_ID = String.Empty;
        String Clave = String.Empty;

        String S_Fte_Financiamiento_ID = String.Empty;
        String S_Area_Funcional_ID = String.Empty;
        String S_Proyecto_Programa_ID = String.Empty;
        String S_Unidad_Responsable_ID = String.Empty;
        String S_Partida_ID = String.Empty;

        String PSM_Fte_Financiamiento_ID = String.Empty;
        String PSM_Area_Funcional_ID = String.Empty;
        String PSM_Proyecto_Programa_ID = String.Empty;
        String PSM_Unidad_Responsable_ID = String.Empty;
        String PSM_Partida_ID = String.Empty;

        try
        {
            //Obtenemos los valores de la petición.
            Obtener_Parametros(
                ref Opcion,
                ref Tabla,
                ref Filas,
                ref Pagina,
                ref Fte_Financiamiento_ID,
                ref Area_Funcional_ID,
                ref Proyecto_Programa_ID,
                ref Unidad_Responsable_ID,
                ref Partida_ID,
                ref Puesto_ID,
                ref Tipo_Plaza,
                ref Estatus,
                ref Empleado_ID,
                ref Clave,
                ref S_Fte_Financiamiento_ID,
                ref S_Area_Funcional_ID,
                ref S_Proyecto_Programa_ID,
                ref S_Unidad_Responsable_ID,
                ref S_Partida_ID,
                ref PSM_Fte_Financiamiento_ID,
                ref PSM_Area_Funcional_ID,
                ref PSM_Proyecto_Programa_ID,
                ref PSM_Unidad_Responsable_ID,
                ref PSM_Partida_ID,
                ref ur_q
                );

            //Limpiamos el objeto response.
            Response.Clear();

            switch (Opcion)
            {
                case "cmb_puestos":
                    Str_Respuesta_JSON = JSON_Puestos(Puesto_ID, Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_fte_financiamiento":
                    Str_Respuesta_JSON = Consultar_SAP_Fuentes_Financiamiento(Unidad_Responsable_ID, Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_area_funcional":
                    Str_Respuesta_JSON = Consultar_SAP_Areas_Funcionales(Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_proyecto_programa":
                    Str_Respuesta_JSON = Consulta_SAP_Programas(Unidad_Responsable_ID, Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_partida":
                    Str_Respuesta_JSON = Consultar_SAP_Partidas(Proyecto_Programa_ID, Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_unidades_responsables":
                    Str_Respuesta_JSON = Consultar_SAP_Unidades_Responsables(Tabla, ur_q);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_unidades_responsables_2":
                    Str_Respuesta_JSON = Consultar_SAP_Unidades_Responsables(Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "buscar_area_funcional":
                    Str_Respuesta_JSON = Text_Consultar_Area_Funcional(Unidad_Responsable_ID);
                    Response.ContentType = "application/text";
                    break;
                case "consultar_puesto":
                    Str_Respuesta_JSON = JSON_Puestos(Puesto_ID, Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "consultar_plazas_x_dependencia":
                    Str_Respuesta_JSON = JSON_Consultar_Puestos_X_Dependencia(Tabla, Unidad_Responsable_ID, Pagina, Filas);
                    Response.ContentType = "application/json";
                    break;
                case "consultar_disponible_codigo":
                    Str_Respuesta_JSON = Text_Consultar_Disponible_CP(Fte_Financiamiento_ID, Area_Funcional_ID, Proyecto_Programa_ID, Unidad_Responsable_ID, Partida_ID);
                    Response.ContentType = "application/text";
                    break;
                case "alta_plaza":
                    Str_Respuesta_JSON = Alta_Plaza(
                                 Unidad_Responsable_ID,
                                 Puesto_ID,
                                 Tipo_Plaza,
                                 Estatus,
                                 Empleado_ID,
                                 Clave,
                                 S_Fte_Financiamiento_ID,
                                 S_Area_Funcional_ID,
                                 S_Proyecto_Programa_ID,
                                 S_Unidad_Responsable_ID,
                                 S_Partida_ID,
                                 PSM_Fte_Financiamiento_ID,
                                 PSM_Area_Funcional_ID,
                                 PSM_Proyecto_Programa_ID,
                                 PSM_Unidad_Responsable_ID,
                                 PSM_Partida_ID
                        );
                    Response.ContentType = "application/text";
                    break;
                case "baja_plaza":
                    Str_Respuesta_JSON = Baja_Plaza(
                                 Unidad_Responsable_ID,
                                 Puesto_ID,
                                 Clave
                        );
                    Response.ContentType = "application/text";
                    break;
                case "busqueda_plazas":
                    Str_Respuesta_JSON = JSON_Consultar_Plazas_Dependencia(Tabla, Unidad_Responsable_ID);
                    Response.ContentType = "application/json";
                    break;


                default:
                    break;
            }

            Response.Write(Str_Respuesta_JSON);
            Response.Flush();
            Response.Close();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error lanzado en el controlador de peticiones del cliente. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Operaciones)
    /// ************************************************************************************************************************************************************
    /// Nombre: Agregar
    /// 
    /// Descripción: Metodo que agrega un registro a la tabla.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Clave de la percepción y/o deducción.
    ///             Cuenta_Contable_ID.- Identificador de las cuentas contables.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String Agregar(String Percepcion_Deduccion_ID, String Cuenta_Contable_ID)
    {
        String Estatus = "NO";//Variable que informa al usuario si la operacion se realizo de forma correcta. 

        try
        {
            Estatus = "SI";
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar un un registro a la tabla. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Eliminar
    /// 
    /// Descripción: Metodo que elimina un registro a la tabla.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Clave de la percepción y/o deducción.
    ///             Cuenta_Contable_ID.- Identificador de las cuentas contables.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String Eliminar(String Percepcion_Deduccion_ID, String Cuenta_Contable_ID)
    {
        String Estatus = "NO";//Variable que informa al usuario si la operacion se realizo de forma correcta. 

        try
        {
            Estatus = "SI";
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar un un registro a la tabla. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #region (Métodos Consulta)
    private String JSON_Puestos(String Puesto_ID, String Tabla)
    {
        Cls_Cat_Puestos_Negocio Obj_Puestos = new Cls_Cat_Puestos_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Resultado = new DataTable();//Variable que almacenara los puestos del sistema.

        try
        {
            Obj_Puestos.P_Puesto_ID = Puesto_ID;
            Dt_Resultado = Obj_Puestos.Consultar_Puestos();
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los puestos en el sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Resultado);
    }

    private String JSON_Consultar_Puestos_X_Dependencia(String Tabla, String Dependencia_ID, String Pagina, String Filas)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Dependencias = new Cls_Cat_Nom_Creacion_Plazas_Negocio();
        DataTable Dt_Resultado = null;
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);//No de paginas.
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);//No de filas.
        Int32 Total_No_Registros = 0;//Total de registros de la tabla completa.

        try
        {
            Obj_Dependencias.P_Unidad_Responsable_ID= Dependencia_ID;
            Dt_Resultado = Obj_Dependencias.Consultar_Puestos_UR();
            Total_No_Registros = Dt_Resultado.Rows.Count;
            Dt_Resultado = Obtener_Registros_Mostrar_Tabla(Dt_Resultado, _Filas, _Pagina);
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las plazas de la unidad responsable seleccionada. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Resultado, Total_No_Registros);
    }

    private String JSON_Consultar_Plazas_Dependencia(String Tabla, String Dependencia_ID)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Dependencias = new Cls_Cat_Nom_Creacion_Plazas_Negocio();
        DataTable Dt_Resultado = null;

        try
        {
            Obj_Dependencias.P_Unidad_Responsable_ID = Dependencia_ID;
            Dt_Resultado = Obj_Dependencias.Consultar_Puestos_UR();
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las plazas de la unidad responsable seleccionada. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Resultado);
    }

    private String Text_Consultar_Disponible_CP(
         String Fte_Financiamiento_ID,
         String Area_Funcional,
         String Proyecto_Programa_ID,
         String Unidad_Responsable_ID,
         String Partida_ID
        )
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Creacion_Plazas = new Cls_Cat_Nom_Creacion_Plazas_Negocio();
        String Disponible = String.Empty;

        try
        {
            Obj_Creacion_Plazas.P_Fte_Financiamiento_ID = Fte_Financiamiento_ID;
            Obj_Creacion_Plazas.P_Area_Funcional_ID = Area_Funcional;
            Obj_Creacion_Plazas.P_Proyecto_Programa_ID = Proyecto_Programa_ID;
            Obj_Creacion_Plazas.P_Unidad_Responsable_ID = Unidad_Responsable_ID;
            Obj_Creacion_Plazas.P_Partida_ID = Partida_ID;

            Disponible = String.Format("{0:c}", Obj_Creacion_Plazas.Consultar_Comprometido_Sueldos());
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el disponible. Error: [" + Ex.Message + "]");
        }
        return Disponible;
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: Obtener_Registros_Mostrar_Tabla
    /// 
    /// Descripción: Metodo que obtiene el no de registros a mostrar en la tabla por página.
    /// 
    /// Parámetros: Dt_Tabla.- Total de registros devueltos por la consulta.
    ///             Pagina.- No de paginas de la tabla.
    ///             Filas.- No de filas por pagina.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private DataTable Obtener_Registros_Mostrar_Tabla(DataTable Dt_Tabla, Int32 Filas, Int32 Pagina)
    {
        Int32 Contador_Registros_Mostrar = 0;//Contador de registros a mostrar al usuario por pagina.
        DataTable Dt_Segmento_Recibos_Registros_Mostrar = null;//Segmento de registros a mostrar.
        int Tope_Segmento_Registros = Filas * Pagina;//Obtenemos el numero de registros a mostrar por pagina al usuario.

        try
        {
            Dt_Segmento_Recibos_Registros_Mostrar = Dt_Tabla.Clone();

            if (Dt_Tabla is DataTable)
            {
                if (Dt_Tabla.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Tabla.Rows)
                    {
                        if (Registro is DataRow)
                        {
                            ++Contador_Registros_Mostrar;
                            if (Contador_Registros_Mostrar > (Tope_Segmento_Registros - Filas) && Contador_Registros_Mostrar <= Tope_Segmento_Registros)
                            {
                                Dt_Segmento_Recibos_Registros_Mostrar.ImportRow(Registro);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al paginar. Error: [" + Ex.Message + "]");
        }
        return Dt_Segmento_Recibos_Registros_Mostrar;
    }
    #endregion

    #region (Métodos Generales)
    //********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Parametros
    ///DESCRIPCIÓN          : Metodo para obtener los parametros del las peticiones del cliente
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private void Obtener_Parametros(
        ref String Opcion,
        ref String Tabla,
        ref String Filas,
        ref String Pagina,
        ref String Fte_Financiamiento_ID,
        ref String Area_Funcional,
        ref String Proyecto_Programa_ID,
        ref String Unidad_Responsable_ID,
        ref String Partida_ID,
        ref String Puesto_ID,
        ref String Tipo_Plaza,
        ref String Estatus,
        ref String Empleado_ID,
        ref String Clave,
        ref String S_Fte_Financiamiento_ID,
        ref String S_Area_Funcional_ID,
        ref String S_Proyecto_Programa_ID,
        ref String S_Unidad_Responsable_ID,
        ref String S_Partida_ID,
        ref String PSM_Fte_Financiamiento_ID,
        ref String PSM_Area_Funcional_ID,
        ref String PSM_Proyecto_Programa_ID,
        ref String PSM_Unidad_Responsable_ID,
        ref String PSM_Partida_ID,
        ref String ur_q
    )
    {
        NameValueCollection nvc = Request.Form;

        if (!String.IsNullOrEmpty(Request.QueryString["opcion"]))
            Opcion = Request.QueryString["opcion"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tabla"]))
            Tabla = Request.QueryString["tabla"].ToString().Trim();

        if (!String.IsNullOrEmpty(nvc["page"]))
            Pagina = nvc["page"].ToString().Trim();

        if (!String.IsNullOrEmpty(nvc["rows"]))
            Filas = nvc["rows"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["q"]))
            ur_q = Request.QueryString["q"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["fte_financiamiento"]))
            Fte_Financiamiento_ID = Request.QueryString["fte_financiamiento"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["area_funcional"]))
            Area_Funcional = Request.QueryString["area_funcional"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["proyecto_programa_id"]))
            Proyecto_Programa_ID = Request.QueryString["proyecto_programa_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["unidad_responsable_id"]))
            Unidad_Responsable_ID = Request.QueryString["unidad_responsable_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["partida_id"]))
            Partida_ID = Request.QueryString["partida_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["puesto_id"]))
            Puesto_ID = Request.QueryString["puesto_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tipo_plaza"]))
            Tipo_Plaza = Request.QueryString["tipo_plaza"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["estatus"]))
            Estatus = Request.QueryString["estatus"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["empleado_id"]))
            Empleado_ID = Request.QueryString["empleado_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["clave"]))
            Clave = Request.QueryString["clave"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["s_fte_financiamiento_id"]))
            S_Fte_Financiamiento_ID = Request.QueryString["s_fte_financiamiento_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["s_area_funcional_id"]))
            S_Area_Funcional_ID = Request.QueryString["s_area_funcional_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["s_proyecto_programa_id"]))
            S_Proyecto_Programa_ID = Request.QueryString["s_proyecto_programa_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["s_unidad_responsable_id"]))
            S_Unidad_Responsable_ID = Request.QueryString["s_unidad_responsable_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["s_partida_id"]))
            S_Partida_ID = Request.QueryString["s_partida_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["psm_fte_financiamiento_id"]))
            PSM_Fte_Financiamiento_ID = Request.QueryString["psm_fte_financiamiento_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["psm_area_funcional_id"]))
            PSM_Area_Funcional_ID = Request.QueryString["psm_area_funcional_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["psm_proyecto_programa_id"]))
            PSM_Proyecto_Programa_ID = Request.QueryString["psm_proyecto_programa_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["psm_unidad_responsable_id"]))
            PSM_Unidad_Responsable_ID = Request.QueryString["psm_unidad_responsable_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["psm_partida_id"]))
            PSM_Partida_ID = Request.QueryString["psm_partida_id"].ToString().Trim();

    }
    #endregion

    #region (Codigo Programatico)
    private String Consultar_SAP_Unidades_Responsables(String Tabla, String ur_q)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Unidades_Responsables = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Obj_Unidades_Responsables.P_ur_q = ur_q;
            Dt_Resultado = Obj_Unidades_Responsables.Consulta_Dependencias();
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_ComboBox(Dt_Resultado);
    }

    private String Consultar_SAP_Unidades_Responsables(String Tabla)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Unidades_Responsables = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Dt_Resultado = Obj_Unidades_Responsables.Consulta_Dependencias();
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Resultado);
    }

    private String Consultar_SAP_Fuentes_Financiamiento(String Dependencia_ID, String Tabla)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Fte_Financiamiento = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Fte_Financiamiento = null;//Variable que almacenara los resultados de la busqueda realizada.

        try
        {
            Obj_Fte_Financiamiento.P_Unidad_Responsable_ID = Dependencia_ID;
            Dt_Fte_Financiamiento = Obj_Fte_Financiamiento.Consultar_Sap_Det_Fte_Dependencia();
            Dt_Fte_Financiamiento.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las fuentes de financiamento registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Fte_Financiamiento);
    }

    private String Consulta_SAP_Programas(String Dependencia_ID, String Tabla)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Programas = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Programas = null;//Variable que alamacenara los resultados obtenidos de la busqueda realizada.

        try
        {
            Obj_Programas.P_Unidad_Responsable_ID = Dependencia_ID;
            Dt_Programas = Obj_Programas.Consultar_Sap_Det_Prog_Dependencia();
            Dt_Programas.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los programas registrados actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Programas);
    }

    private String Consultar_SAP_Partidas(String Programa_ID, String Tabla)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Partidas = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Partidas = null;

        try
        {
            Dt_Partidas = Obj_Partidas.Consultar_Partidas(Programa_ID);
            Dt_Partidas.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las partidas registrdas en el sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Partidas);
    }

    private String Consultar_SAP_Areas_Funcionales(String Tabla)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Dependencias = new Cls_Cat_Nom_Creacion_Plazas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Areas_Funcionales = null;                                             //Variable que almacena un listado de areas funcionales registradas actualmente en el sistema.

        try
        {
            Dt_Areas_Funcionales = Obj_Dependencias.Consulta_Area_Funcional();
            Dt_Areas_Funcionales.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las áreas funcionales registradas actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Areas_Funcionales);
    }

    private String Text_Consultar_Area_Funcional(String Dependencia_ID)
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_Dependencias = new Cls_Cat_Nom_Creacion_Plazas_Negocio();
        DataTable Dt_Depedencias = null;
        String Area_Funcional_ID = String.Empty;

        try
        {
            Obj_Dependencias.P_Unidad_Responsable_ID = Dependencia_ID;
            Dt_Depedencias = Obj_Dependencias.Consulta_Dependencias();

            if (Dt_Depedencias is DataTable)
            {
                var items_dependencias = from item_dependencia in Dt_Depedencias.AsEnumerable()
                                         select new
                                         {
                                             Area_Funcional = ((item_dependencia.IsNull(Cat_Dependencias.Campo_Area_Funcional_ID)) ?
                                             String.Empty : item_dependencia.Field<String>(Cat_Dependencias.Campo_Area_Funcional_ID))
                                         };

                if (items_dependencias != null)
                {
                    foreach (var item in items_dependencias)
                    {
                        if (item.Area_Funcional != null)
                        {
                            Area_Funcional_ID = item.Area_Funcional;
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el area funcional de la unidad responsable seleccionada. Error: [" + Ex.Message + "]");
        }
        return Area_Funcional_ID;
    }
    #endregion


    private String Alta_Plaza(
         String Unidad_Responsable_ID,
         String Puesto_ID,
         String Tipo_Plaza,
         String Estatus,
         String Empleado_ID,
         String Clave,
         String S_Fte_Financiamiento_ID,
         String S_Area_Funcional_ID,
         String S_Proyecto_Programa_ID,
         String S_Unidad_Responsable_ID,
         String S_Partida_ID,
         String PSM_Fte_Financiamiento_ID,
         String PSM_Area_Funcional_ID,
         String PSM_Proyecto_Programa_ID,
         String PSM_Unidad_Responsable_ID,
         String PSM_Partida_ID
        )
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_creacion_Plaza = new Cls_Cat_Nom_Creacion_Plazas_Negocio();
        String Operacion_Completa = "NO";

        try
        {
            Obj_creacion_Plaza.P_Unidad_Responsable_ID = Unidad_Responsable_ID;
            Obj_creacion_Plaza.P_Puesto_ID = Puesto_ID;
            Obj_creacion_Plaza.P_Tipo_Plaza = Tipo_Plaza;
            Obj_creacion_Plaza.P_Estatus = Estatus;
            Obj_creacion_Plaza.P_Empleado_ID = Empleado_ID;
            Obj_creacion_Plaza.P_Clave = Clave;

            Obj_creacion_Plaza.P_S_Fte_Financiamiento_ID = S_Fte_Financiamiento_ID;
            Obj_creacion_Plaza.P_S_Area_Funcional_ID = S_Area_Funcional_ID;
            Obj_creacion_Plaza.P_S_Proyecto_Programa_ID = S_Proyecto_Programa_ID;
            Obj_creacion_Plaza.P_S_Unidad_Responsable_ID = S_Unidad_Responsable_ID;
            Obj_creacion_Plaza.P_S_Partida_ID = S_Partida_ID;

            Obj_creacion_Plaza.P_PSM_Fte_Financiamiento_ID = PSM_Fte_Financiamiento_ID;
            Obj_creacion_Plaza.P_PSM_Area_Funcional_ID = PSM_Area_Funcional_ID;
            Obj_creacion_Plaza.P_PSM_Proyecto_Programa_ID = PSM_Proyecto_Programa_ID;
            Obj_creacion_Plaza.P_PSM_Unidad_Responsable_ID = PSM_Unidad_Responsable_ID;
            Obj_creacion_Plaza.P_PSM_Partida_ID = PSM_Partida_ID;

            if (Obj_creacion_Plaza.Alta_Plaza())
            {
                Operacion_Completa = "SI";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al dar el alta de la plaza. Error: [" + Ex.Message + "]");
        }
        return Operacion_Completa;
    }

    private String Baja_Plaza(
         String Unidad_Responsable_ID,
         String Puesto_ID,
         String Clave
        )
    {
        Cls_Cat_Nom_Creacion_Plazas_Negocio Obj_creacion_Plaza = new Cls_Cat_Nom_Creacion_Plazas_Negocio();
        String Operacion_Completa = "NO";

        try
        {
            Obj_creacion_Plaza.P_Unidad_Responsable_ID = Unidad_Responsable_ID;
            Obj_creacion_Plaza.P_Puesto_ID = Puesto_ID;
            Obj_creacion_Plaza.P_Clave = Clave;

            if (Obj_creacion_Plaza.Eliminar_Plaza())
            {
                Operacion_Completa = "SI";
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al eliminar la plaza. Error: [" + Ex.Message + "]");
        }
        return Operacion_Completa;
    }
}
