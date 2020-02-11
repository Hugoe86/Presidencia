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
using Presidencia.Relacion_Perc_Dedu_Cuentas_Contables.Negocio;
using Presidencia.Constantes;
using Presidencia.Ayudante_JQuery;
using System.Collections.Specialized;

public partial class paginas_Nomina_Frm_Cat_Nom_Perc_Dedu_CC_Deta_Ctrl : System.Web.UI.Page
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
        String Percepcion_Deduccion_ID = String.Empty;
        String Cuenta_Contable_ID = String.Empty;

        try
        {
            //Obtenemos los valores de la petición.
            Obtener_Parametros(ref Opcion, ref Tabla, ref Filas, ref Pagina, ref Percepcion_Deduccion_ID, ref Cuenta_Contable_ID);

            //Limpiamos el objeto response.
            Response.Clear();

            switch (Opcion)
            {
                case "cmb_cuentas_contables":
                    Str_Respuesta_JSON = JSON_Cuentas_Contables(Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "cmb_percepciones_deducciones":
                    Str_Respuesta_JSON = JSON_Conceptos(Tabla);
                    Response.ContentType = "application/json";
                    break;
                case "consultar_conceptos_x_cuenta_contable":
                    Str_Respuesta_JSON = JSON_Conceptos_Cuentas_Contables(Tabla, Percepcion_Deduccion_ID, Cuenta_Contable_ID, Pagina, Filas);
                    Response.ContentType = "application/json";
                    break;
                case "agregar":
                    Str_Respuesta_JSON = Agregar(Percepcion_Deduccion_ID, Cuenta_Contable_ID);
                    Response.ContentType = "application/text";
                    break;
                case "eliminar":
                    Str_Respuesta_JSON = Eliminar(Percepcion_Deduccion_ID, Cuenta_Contable_ID);
                    Response.ContentType = "application/text";
                    break;
                case "buscar":
                    Str_Respuesta_JSON = JSON_Consultar_X_Clave(Percepcion_Deduccion_ID);
                    Response.ContentType = "application/text";
                    break;
                case "buscar_cuenta":
                    Str_Respuesta_JSON = JSON_Consultar_X_Cuenta(Cuenta_Contable_ID);
                    Response.ContentType = "application/text";
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
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexion con la capa de negocios.
        String Estatus = "NO";//Variable que informa al usuario si la operacion se realizo de forma correcta. 

        try
        {
            Obj_Perc_Dedu_CC.P_Percepcion_Deduccion_ID = Percepcion_Deduccion_ID;
            Obj_Perc_Dedu_CC.P_Cuenta_Contable_ID = Cuenta_Contable_ID;

            Obj_Perc_Dedu_CC.Alta_Individual();
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
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexion a la capa de negocios.
        String Estatus = "NO";//Variable que informa al usuario si la operacion se realizo de forma correcta. 

        try
        {
            Obj_Perc_Dedu_CC.P_Percepcion_Deduccion_ID = Percepcion_Deduccion_ID;
            Obj_Perc_Dedu_CC.P_Cuenta_Contable_ID = Cuenta_Contable_ID;

            Obj_Perc_Dedu_CC.Delete();
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
    /// ************************************************************************************************************************************************************
    /// Nombre: JSON_Consultar_X_Cuenta
    /// 
    /// Descripción: Metodo que consulta las cuentas contables por la clave.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Clave de la percepción y/o deducción.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String JSON_Consultar_X_Cuenta(String Cuenta_Contable_ID)
    {
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Rel_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado de la consulta.
        String ID = String.Empty;//Variable que almacena el identificador del concepto.

        try
        {
            Obj_Rel_Perc_Dedu_CC.P_Cuenta = Cuenta_Contable_ID;
            Dt_Resultado = Obj_Rel_Perc_Dedu_CC.Consultar_Cuenta_Contable_X_Cuenta();

            var Cuenta = from c in Dt_Resultado.AsEnumerable()
                           select new { ID = c.Field<String>(Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID) };

            foreach (var account in Cuenta)
            {
                ID = account.ID;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return ID;
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: JSON_Consultar_X_Clave
    /// 
    /// Descripción: Metodo que consulta las percepciones y/o deducciones por la clave.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Clave de la percepción y/o deducción.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String JSON_Consultar_X_Clave(String Percepcion_Deduccion_ID)
    {
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Rel_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado de la consulta.
        String ID = String.Empty;//Variable que almacena el identificador del concepto.

        try
        {
            Obj_Rel_Perc_Dedu_CC.P_CLave = Percepcion_Deduccion_ID;
            Dt_Resultado = Obj_Rel_Perc_Dedu_CC.Consultar_Conceptos_X_Clave();

            var Concepto = from c in Dt_Resultado.AsEnumerable()
                           select new { ID = c.Field<String>(Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID) };

            foreach (var percepcion_deduccion in Concepto)
            {
                ID = percepcion_deduccion.ID;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return ID;
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: JSON_Cuentas_Contables
    /// 
    /// Descripción: Metodo que consulta las cuentas contables.
    /// 
    /// Parámetros: Tabla.- Nombre de la tabla.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String JSON_Cuentas_Contables(String Tabla)
    {
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Rel_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Resultado = null ;//Variable que almacena el resultado.

        try
        {
            Dt_Resultado = Obj_Rel_Perc_Dedu_CC.Consultar_Cuentas_Contables();
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Resultado);
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: JSON_Conceptos
    /// 
    /// Descripción: Metodo que consulta las percepciones y/o deducciones que existen actualmente en el sistema.
    /// 
    /// Parámetros: Tabla.- Nombre de la tabla
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String JSON_Conceptos(String Tabla)
    {
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Rel_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado. 

        try
        {
            Dt_Resultado = Obj_Rel_Perc_Dedu_CC.Consultar_Conceptos();
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Resultado);
    }
    /// ************************************************************************************************************************************************************
    /// Nombre: JSON_Conceptos_Cuentas_Contables
    /// 
    /// Descripción: Metodo que consulta las cuentas contables por concepto.
    /// 
    /// Parámetros: Percepcion_Deduccion_ID.- Identificador del concepto al que se buscaran las cuentas contables.
    ///             Pagina.- No de paginas de la tabla.
    ///             Filas.- No de filas por pagina.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creo: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico.
    /// ************************************************************************************************************************************************************
    private String JSON_Conceptos_Cuentas_Contables(String Tabla, String Percepcion_Deduccion_ID, String Cuenta_Contable_ID, String Pagina, String Filas)
    {
        Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio Obj_Rel_Perc_Dedu_CC = new Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Resultado = null;//Variable que almacena el resultado.
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);//No de paginas.
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);//No de filas.
        Int32 Total_No_Registros = 0;//Total de registros de la tabla completa.

        try
        {
            Obj_Rel_Perc_Dedu_CC.P_Percepcion_Deduccion_ID = Percepcion_Deduccion_ID;
            Obj_Rel_Perc_Dedu_CC.P_Cuenta_Contable_ID = Cuenta_Contable_ID;

            Dt_Resultado = Obj_Rel_Perc_Dedu_CC.Consultar_Cuentas_Contables_X_Concepto();
            Total_No_Registros = Dt_Resultado.Rows.Count;
            Dt_Resultado = Obtener_Registros_Mostrar_Tabla(Dt_Resultado, _Filas, _Pagina);
            Dt_Resultado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Resultado, Total_No_Registros);
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
        ref String Percepcion_Deduccion_ID,
        ref String Cuenta_Contable_ID
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

        if (!String.IsNullOrEmpty(Request.QueryString["percepcion_deduccion_id"]))
            Percepcion_Deduccion_ID = Request.QueryString["percepcion_deduccion_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["cuenta_contable_id"]))
            Cuenta_Contable_ID = Request.QueryString["cuenta_contable_id"].ToString().Trim();

    }
    #endregion
}
