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
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using System.Text;
using Presidencia.Ayudante_JQuery;
using Presidencia.Prestamos.Negocio;
using Presidencia.Recibo_Pago.Negocio;
using System.Collections.Specialized;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Numalet;
using System.Globalization;
using Presidencia.Periodos_Vacacionales.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Ver_Documentos_Empl_Ctrl : System.Web.UI.Page
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
        String Nomina_ID = String.Empty;
        String No_Nomina = String.Empty;
        String Tipo_Nomina = String.Empty;
        String No_Empleado = String.Empty;
        String RFC = String.Empty;
        String CURP = String.Empty;
        String Unidad_Responsable = String.Empty;
        String Pagina = String.Empty;
        String Filas = String.Empty;
        String No_Recibo = String.Empty;
        String Banco_ID = String.Empty;

        try
        {
            //Obtenemos los valores de la petición.
            Obtener_Parametros(ref Opcion, ref Tabla, ref Nomina_ID, ref No_Nomina, ref Tipo_Nomina, ref No_Empleado, ref RFC, ref CURP, ref Unidad_Responsable, ref Filas, ref Pagina, ref No_Recibo, ref Banco_ID);

            //Limpiamos el objeto response.
            Response.Clear();

            switch (Opcion)
            {
                case "consultar_calendario_nomina":
                    Str_Respuesta_JSON = JSON_Consultar_Calendario_Nominas(Tabla);
                    break;
                case "consultar_periodos_nominales":
                    Str_Respuesta_JSON = JSON_Consultar_Periodos_Calendario_Nomina(Tabla, Nomina_ID);
                    break;
                case "consultar_tipos_nominas":
                    Str_Respuesta_JSON = JSON_Consultar_Tipos_Nomina(Tabla);
                    break;
                case "consultar_unidad_responsable":
                    Str_Respuesta_JSON = JSON_Consultar_UR(Tabla);
                    break;
                case "consultar_tipos_banco":
                    Str_Respuesta_JSON = JSON_Consultar_Bancos(Tabla);
                    break;
                case "consultar_empleados":
                    Str_Respuesta_JSON = JSON_Consultar_Empleados(Tabla, No_Empleado, RFC, CURP, Unidad_Responsable, Tipo_Nomina, Pagina, Filas);
                    break;
                case "consultar_documentos_empleados":
                    Str_Respuesta_JSON = JSON_Consultar_Requisitos_Empleado(Tabla, No_Empleado, Pagina, Filas);
                    break;
                default:
                    break;
            }

            Response.ContentType = "application/json";
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

    #region (Métodos Consulta)
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Calendario_Nominas
    ///DESCRIPCIÓN          : Metodo para generar el JSON para llenar el combo de calendario de nominas
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Calendario_Nominas(String Tabla)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Calendarios_Nomina = null;

        try
        {
            Dt_Calendarios_Nomina = Obj_Calendario_Nomina_Negocio.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nomina.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Calendarios_Nomina);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Periodos_Calendario_Nomina
    ///DESCRIPCIÓN          : Metodo para generar el JSON para llenar el combo de periodos de calendario de nominas
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Periodos_Calendario_Nomina(String Tabla, String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            Dt_Periodos_Catorcenales.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos del calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Periodos_Catorcenales);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Tipos_Nomina
    ///DESCRIPCIÓN          : Metodo para generar el JSON para llenar el combo de tipos de nominas
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Tipos_Nomina(String Tabla)
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina_Negocio = new Cls_Cat_Tipos_Nominas_Negocio();
        DataTable Dt_Tipos_Nomina = null;

        try
        {
            Dt_Tipos_Nomina = Obj_Tipos_Nomina_Negocio.Consulta_Tipos_Nominas();
            Dt_Tipos_Nomina.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nominas. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Tipos_Nomina);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_UR
    ///DESCRIPCIÓN          : Metodo para generar el JSON para llenar el combo de unidades responsables
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_UR(String Tabla)
    {
        Cls_Cat_Dependencias_Negocio Obj_UR_Negocio = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_UR = null;

        try
        {
            Dt_UR = Obj_UR_Negocio.Consulta_Dependencias();
            Dt_UR.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_UR);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Bancos
    ///DESCRIPCIÓN          : Metodo para generar el JSON de los bancos 
    ///PROPIEDADES          :
    ///CREO                 : Leslie Gonzalez Vazquez
    ///FECHA_CREO           : 12/Diciembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Bancos(String Tabla)
    {
        Cls_Cat_Nom_Bancos_Negocio Obj_Bancos = new Cls_Cat_Nom_Bancos_Negocio();

        DataTable Dt_Bancos = null;

        try
        {
            Dt_Bancos = Obj_Bancos.Consulta_Bancos();
            Dt_Bancos.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los bancos. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Bancos);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Empleados
    ///DESCRIPCIÓN          : Metodo para generar el JSON de los Empleados
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Empleados(String Tabla, String No_Empleado, String RFC, String CURP, String UR, String Tipo_Nomina_ID, String Pagina, String Filas)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que listara los empleados.
        DataTable Dt_Informacion_Mostrar_Empleado = new DataTable();//Datos a mostrar en la tabla de empleados.
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);
        Int32 Total_No_Registros = 0;

        try
        {
            //Creamos la estructura de la tabla que almacenara la informacion del empleado.
            Dt_Informacion_Mostrar_Empleado.Columns.Add(Cat_Empleados.Campo_No_Empleado, typeof(String));
            Dt_Informacion_Mostrar_Empleado.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(String));
            Dt_Informacion_Mostrar_Empleado.Columns.Add(Cat_Empleados.Campo_Ruta_Foto, typeof(String));

            if (!String.IsNullOrEmpty(No_Empleado))
                Obj_Empleados.P_No_Empleado = No_Empleado;

            if (!String.IsNullOrEmpty(RFC))
                Obj_Empleados.P_RFC = RFC;

            if (!String.IsNullOrEmpty(CURP))
                Obj_Empleados.P_CURP = CURP;

            if (!String.IsNullOrEmpty(UR))
                Obj_Empleados.P_Dependencia_ID = UR;

            if (!String.IsNullOrEmpty(Tipo_Nomina_ID))
                Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Empleado in Dt_Empleados.Rows)
                    {
                        if (Dr_Empleado is DataRow)
                        {
                            DataRow Renglon_Empleado = Dt_Informacion_Mostrar_Empleado.NewRow();

                            if (!String.IsNullOrEmpty(Dr_Empleado[Cat_Empleados.Campo_No_Empleado].ToString()))
                                Renglon_Empleado[Cat_Empleados.Campo_No_Empleado] = Dr_Empleado[Cat_Empleados.Campo_No_Empleado].ToString();

                            if (!String.IsNullOrEmpty(Dr_Empleado["EMPLEADO"].ToString()))
                                Renglon_Empleado[Cat_Empleados.Campo_Nombre] = Dr_Empleado["EMPLEADO"].ToString();

                            if (!String.IsNullOrEmpty(Dr_Empleado[Cat_Empleados.Campo_Ruta_Foto].ToString()))
                                Renglon_Empleado[Cat_Empleados.Campo_Ruta_Foto] = Dr_Empleado[Cat_Empleados.Campo_Ruta_Foto].ToString();

                            Dt_Informacion_Mostrar_Empleado.Rows.Add(Renglon_Empleado);
                        }
                    }
                }
            }

            Total_No_Registros = Dt_Informacion_Mostrar_Empleado.Rows.Count;
            Dt_Informacion_Mostrar_Empleado = Obtener_Registros_Mostrar_Tabla(Dt_Informacion_Mostrar_Empleado, _Filas, _Pagina);
            Dt_Informacion_Mostrar_Empleado.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los empleados. Error: [" + Ex.Message + "]");
        }
        return Presidencia.Ayudante_JQuery.Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Informacion_Mostrar_Empleado, Total_No_Registros);
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Requisitos_Empleado
    ///DESCRIPCIÓN          : Metodo para generar el JSON de los los requisitos del Empleado.
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Enero/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Requisitos_Empleado(String Tabla, String No_Empleado, String Pagina, String Filas) {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Requisitos = null;
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);
        Int32 Total_No_Registros = 0;

        try
        {
            INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(No_Empleado);

            Obj_Empleados.P_Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
            Dt_Requisitos = Obj_Empleados.Consultar_Documentos();

            Total_No_Registros = Dt_Requisitos.Rows.Count;
            Dt_Requisitos = Obtener_Registros_Mostrar_Tabla(Dt_Requisitos, _Filas, _Pagina);
            Dt_Requisitos.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los requisitos del empleado. Error: [" + Ex.Message + "]");
        }
        return Presidencia.Ayudante_JQuery.Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Requisitos, Total_No_Registros);
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
        ref String Nomina_ID,
        ref String No_Nomina,
        ref String Tipo_Nomina,
        ref String No_Empleado,
        ref String RFC,
        ref String CURP,
        ref String Unidad_Responsable,
        ref String Filas,
        ref String Pagina,
        ref String No_Recibo,
        ref String Banco_ID
    )
    {
        NameValueCollection nvc = Request.Form;

        if (!String.IsNullOrEmpty(Request.QueryString["opcion"]))
            Opcion = Request.QueryString["opcion"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tabla"]))
            Tabla = Request.QueryString["tabla"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["nomina_id"]))
            Nomina_ID = Request.QueryString["nomina_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["no_nomina"]))
            No_Nomina = Request.QueryString["no_nomina"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["tipo_nomina_id"]))
            Tipo_Nomina = Request.QueryString["tipo_nomina_id"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["no_empleado"]))
            No_Empleado = Request.QueryString["no_empleado"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["rfc"]))
            RFC = Request.QueryString["rfc"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["curp"]))
            CURP = Request.QueryString["curp"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["ur"]))
            Unidad_Responsable = Request.QueryString["ur"].ToString().Trim();

        if (!String.IsNullOrEmpty(nvc["page"]))
            Pagina = nvc["page"].ToString().Trim();

        if (!String.IsNullOrEmpty(nvc["rows"]))
            Filas = nvc["rows"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["no_recibo"]))
            No_Recibo = Request.QueryString["no_recibo"].ToString().Trim();

        if (!String.IsNullOrEmpty(Request.QueryString["banco_id"]))
            Banco_ID = Request.QueryString["banco_id"].ToString().Trim();
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Registros_Mostrar_Tabla
    ///DESCRIPCIÓN          : 
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private DataTable Obtener_Registros_Mostrar_Tabla(DataTable Dt_Tabla, Int32 Filas, Int32 Pagina)
    {
        Int32 Contador_Registros_Mostrar = 0;
        DataTable Dt_Segmento_Recibos_Registros_Mostrar = null;
        int Tope_Segmento_Registros = Filas * Pagina;

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
}
