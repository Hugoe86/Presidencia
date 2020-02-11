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

public partial class paginas_Nomina_Frm_Ope_Nom_Controlador_Gen_Recibos : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) {
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
                case "consultar_recibos_nomina":
                    Str_Respuesta_JSON = JSON_Consulta_Recibo_Nomina_Empleados(Tabla, Nomina_ID, No_Nomina, Tipo_Nomina, No_Empleado, RFC, CURP, Unidad_Responsable, Pagina, Filas, Banco_ID);
                    break;
                case "consultar_percepciones_recibo_nomina":
                    Str_Respuesta_JSON = JSON_Consultar_Percepciones_Recibo_Nomina(Tabla, No_Recibo, Pagina, Filas);
                    break;
                case "consultar_deducciones_recibo_nomina":
                    Str_Respuesta_JSON = JSON_Consultar_Deducciones_Recibo_Nomina(Tabla, No_Recibo, Pagina, Filas);
                    break;
                case "obtener_recibo_nomina":
                    Str_Respuesta_JSON = JSON_obtener_recibo_nomina(Tabla, Nomina_ID, No_Nomina, Tipo_Nomina, No_Empleado, RFC, CURP, Unidad_Responsable, Banco_ID);
                    break;
                case "obtener_detalles_recibo_nomina":
                    Str_Respuesta_JSON = JSON_Consultar_Detalles_Recibo_Nomina(Tabla, No_Recibo);
                    break;
                case "consultar_tipos_banco":
                    Str_Respuesta_JSON = JSON_Consultar_Bancos(Tabla);
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
    ///NOMBRE DE LA FUNCIÓN : JSON_Consulta_Recibo_Nomina_Empleados
    ///DESCRIPCIÓN          : Metodo para generar el JSON de los recibos
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consulta_Recibo_Nomina_Empleados(String Tabla, String Nomina_ID, String No_Nomina, String Tipo_Nomina_ID, String No_Empleado,
        String RFC, String CURP, String Dependencia_ID, String Pagina, String Filas, String Banco_ID)
    {
        Cls_Ope_Nom_Recibo_Pago_Negocio Obj_Recibos_Nomina_Negocios = new Cls_Ope_Nom_Recibo_Pago_Negocio();
        DataTable Dt_Recibos_Nomina = null;
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);
        Int32 Total_No_Registros = 0;

        try
        {
            Obj_Recibos_Nomina_Negocios.P_Nomina_ID = Nomina_ID;
            Obj_Recibos_Nomina_Negocios.P_Periodo = No_Nomina;
            Obj_Recibos_Nomina_Negocios.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Obj_Recibos_Nomina_Negocios.P_Rfc = RFC;
            Obj_Recibos_Nomina_Negocios.P_Curp = CURP;
            Obj_Recibos_Nomina_Negocios.P_Departamento = Dependencia_ID;
            Obj_Recibos_Nomina_Negocios.P_Empleado_ID = No_Empleado;
            Obj_Recibos_Nomina_Negocios.P_Banco_ID = Banco_ID;

            //Dt_Recibos_Nomina = Obj_Recibos_Nomina_Negocios.Generar_Recibo();
            Dt_Recibos_Nomina = Obj_Recibos_Nomina_Negocios.Consultar_Recibos_Empleados();
            Total_No_Registros = Dt_Recibos_Nomina.Rows.Count;
            Dt_Recibos_Nomina = Obtener_Registros_Mostrar_Tabla(Dt_Recibos_Nomina, _Filas, _Pagina);
            Dt_Recibos_Nomina.TableName = Tabla;

            Dt_Recibos_Nomina.Columns.Add("TOTAL_NOMINA_FOR");
            Dt_Recibos_Nomina.Columns.Add("TOTAL_PERCEPCIONES_FOR");
            Dt_Recibos_Nomina.Columns.Add("TOTAL_DEDUCCIONES_FOR");
            foreach (DataRow Dr in Dt_Recibos_Nomina.Rows)
            {
                Dr["TOTAL_NOMINA_FOR"] = String.Format("{0:c}", Dr["TOTAL_NOMINA"]);
                Dr["TOTAL_PERCEPCIONES_FOR"] = String.Format("{0:c}", Dr["TOTAL_PERCEPCIONES"]);
                Dr["TOTAL_DEDUCCIONES_FOR"] = String.Format("{0:c}", Dr["TOTAL_DEDUCCIONES"]);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los recibos de nomina de los empleados. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Recibos_Nomina, Total_No_Registros);
    }

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Percepciones_Recibo_Nomina
    ///DESCRIPCIÓN          : Metodo para generar el JSON de las percepciones de los recibos
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Percepciones_Recibo_Nomina(String Tabla, String No_Recibo, String Pagina, String Filas)
    {
        Cls_Ope_Nom_Recibo_Pago_Negocio Obj_Recibos_Nomina_Negocios = new Cls_Ope_Nom_Recibo_Pago_Negocio();
        DataTable Dt_Percepciones_Recibo_Nomina = null;
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);
        Int32 Total_No_Registros = 0;

        try
        {
            Obj_Recibos_Nomina_Negocios.P_No_Recibo = No_Recibo;
            Dt_Percepciones_Recibo_Nomina = Obj_Recibos_Nomina_Negocios.Consultar_Percepciones_Recibo_Empleado();
            Total_No_Registros = Dt_Percepciones_Recibo_Nomina.Rows.Count;
            Dt_Percepciones_Recibo_Nomina = Obtener_Registros_Mostrar_Tabla(Dt_Percepciones_Recibo_Nomina, _Filas, _Pagina);
            Dt_Percepciones_Recibo_Nomina.TableName = Tabla;

            Dt_Percepciones_Recibo_Nomina.Columns.Add("MONTO_FOR");
            foreach (DataRow Dr in Dt_Percepciones_Recibo_Nomina.Rows)
            {
                Dr["MONTO_FOR"] = String.Format("{0:c}", Dr["MONTO"]);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepiones del recibo de nomina. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Percepciones_Recibo_Nomina, Total_No_Registros);
    }

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Deducciones_Recibo_Nomina
    ///DESCRIPCIÓN          : Metodo para generar el JSON de las deducciones de los recibos
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : Noviembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Deducciones_Recibo_Nomina(String Tabla, String No_Recibo, String Pagina, String Filas)
    {
        Cls_Ope_Nom_Recibo_Pago_Negocio Obj_Recibos_Nomina_Negocios = new Cls_Ope_Nom_Recibo_Pago_Negocio();
        DataTable Dt_Deducciones_Recibo_Nomina = null;
        Int32 _Pagina = Convert.ToInt32((String.IsNullOrEmpty(Pagina)) ? "0" : Pagina);
        Int32 _Filas = Convert.ToInt32((String.IsNullOrEmpty(Filas)) ? "0" : Filas);
        Int32 Total_No_Registros = 0;

        try
        {
            Obj_Recibos_Nomina_Negocios.P_No_Recibo = No_Recibo;
            Dt_Deducciones_Recibo_Nomina = Obj_Recibos_Nomina_Negocios.Consultar_Deducciones_Recibo_Empleado();
            Total_No_Registros = Dt_Deducciones_Recibo_Nomina.Rows.Count;
            Dt_Deducciones_Recibo_Nomina = Obtener_Registros_Mostrar_Tabla(Dt_Deducciones_Recibo_Nomina, _Filas, _Pagina);
            Dt_Deducciones_Recibo_Nomina.TableName = Tabla;

            Dt_Deducciones_Recibo_Nomina.Columns.Add("MONTO_FOR");
            foreach(DataRow Dr in Dt_Deducciones_Recibo_Nomina.Rows)
            {
                Dr["MONTO_FOR"] = String.Format("{0:c}", Dr["MONTO"]);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepiones del recibo de nomina. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Deducciones_Recibo_Nomina, Total_No_Registros);
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

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_obtener_recibo_nomina
    ///DESCRIPCIÓN          : Metodo para generar el JSON de los recibos
    ///PROPIEDADES          :
    ///CREO                 : Leslie Gonzalez Vazquez
    ///FECHA_CREO           : 09/Diciembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_obtener_recibo_nomina(String Tabla, String Nomina_ID, String No_Nomina, String Tipo_Nomina_ID, String No_Empleado,
        String RFC, String CURP, String Dependencia_ID, String Banco_ID)
    {
        Cls_Ope_Nom_Recibo_Pago_Negocio Obj_Recibos_Nomina_Negocios = new Cls_Ope_Nom_Recibo_Pago_Negocio();
        
        DataTable Dt_Recibos_Nomina = null;
        Int32 Total_No_Registros = 0;

        try
        {
            Obj_Recibos_Nomina_Negocios.P_Nomina_ID = Nomina_ID;
            Obj_Recibos_Nomina_Negocios.P_Periodo = No_Nomina;
            Obj_Recibos_Nomina_Negocios.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Obj_Recibos_Nomina_Negocios.P_Rfc = RFC;
            Obj_Recibos_Nomina_Negocios.P_Curp = CURP;
            Obj_Recibos_Nomina_Negocios.P_Departamento = Dependencia_ID;
            Obj_Recibos_Nomina_Negocios.P_Empleado_ID = No_Empleado;
            Obj_Recibos_Nomina_Negocios.P_Banco_ID = Banco_ID;

            Dt_Recibos_Nomina = Obj_Recibos_Nomina_Negocios.Consultar_Recibos_Empleados();

            Dt_Recibos_Nomina.Columns.Add("TOTAL_PERCEPCIONES_FOR");
            Dt_Recibos_Nomina.Columns.Add("TOTAL_DEDUCCIONES_FOR");
            Dt_Recibos_Nomina.Columns.Add("TOTAL_NOMINA_FOR");
            Dt_Recibos_Nomina.Columns.Add("FECHA_FIN_FOR");
            Dt_Recibos_Nomina.Columns.Add("NO_RECIBO_FOR");
            Dt_Recibos_Nomina.Columns.Add("HORA");

            if (Tipo_Nomina_ID.Equals("00003"))
            {
                Dt_Recibos_Nomina.Columns.Add("FECHA");
                Dt_Recibos_Nomina.Columns.Add("CANTIDAD_LETRA");
                Dt_Recibos_Nomina.Columns.Add("FECHA_INI");
                Dt_Recibos_Nomina.Columns.Add("FECHA_FINAL");

                foreach(DataRow Dr in Dt_Recibos_Nomina.Rows)
                {
                    Dr["FECHA"] = "IRAPUATO, GTO., A " + Crear_Fecha(String.Format("{0:MM/dd/yyyy}", DateTime.Now));
                    Dr["CANTIDAD_LETRA"] = "(" + Convertir_Cantidad_Letras(Convert.ToDouble(String.IsNullOrEmpty(Dr["TOTAL_NOMINA"].ToString()) ? "0" : Dr["TOTAL_NOMINA"].ToString().Trim())) + ")";
                    Dr["FECHA_INI"] = Crear_Fecha(String.Format("{0:MM/dd/yyyy}", Dr["FECHA_INICIO"]));
                    Dr["FECHA_FINAL"] = Crear_Fecha(String.Format("{0:MM/dd/yyyy}", Dr["FECHA_FIN"]));
                }
            }

            foreach (DataRow Dr in Dt_Recibos_Nomina.Rows)
            {
                Dr["CODIGO_PROGRAMATICO"] = Dr["CODIGO_PROGRAMATICO"].ToString().Trim().Replace(" ", "");
                Dr["TOTAL_PERCEPCIONES_FOR"] = String.Format("{0:c}", Dr["TOTAL_PERCEPCIONES"]);
                Dr["TOTAL_DEDUCCIONES_FOR"] = String.Format("{0:c}", Dr["TOTAL_DEDUCCIONES"]);
                Dr["TOTAL_NOMINA_FOR"] = String.Format("{0:c}", Dr["TOTAL_NOMINA"]);
                Dr["FECHA_FIN_FOR"] = String.Format("{0:dd-MMM-yyyy}", Dr["FECHA_FIN"]).ToUpper();
                Dr["NO_RECIBO_FOR"] = Convert.ToInt32(Dr["NO_RECIBO"].ToString().Trim());
                Dr["HORA"] = DateTime.Now.ToString("HH:mm:ss");
            }
            Total_No_Registros = Dt_Recibos_Nomina.Rows.Count;
            Dt_Recibos_Nomina.TableName = Tabla;

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los recibos de nomina de los empleados. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Recibos_Nomina, Total_No_Registros);
    }

    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : JSON_Consultar_Detalles_Recibo_Nomina
    ///DESCRIPCIÓN          : Metodo para generar el JSON de los detalles de los recibos
    ///PROPIEDADES          :
    ///CREO                 : Leslie Gonzalez Vazquez
    ///FECHA_CREO           : 10/Diciembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    private String JSON_Consultar_Detalles_Recibo_Nomina(String Tabla, String No_Recibo)
    {
        Cls_Ope_Nom_Recibo_Pago_Negocio Obj_Recibos_Nomina_Negocios = new Cls_Ope_Nom_Recibo_Pago_Negocio(); //conexion con la capa de negocios
        DataTable Dt_Percepciones_Recibo_Nomina = new DataTable();
        DataTable Dt_Deducciones_Recibo_Nomina = new DataTable();
        Int32 Total_No_Registros = 0;
        DataTable Dt_Detalles = new DataTable();
        DataRow Fila;
        Int32 Contador_Detalles;
        Int32 Contador_Fila;

        try
        {
            Obj_Recibos_Nomina_Negocios.P_No_Recibo = No_Recibo;
            Dt_Percepciones_Recibo_Nomina = Obj_Recibos_Nomina_Negocios.Consultar_Percepciones_Recibo_Empleado();
            Dt_Deducciones_Recibo_Nomina = Obj_Recibos_Nomina_Negocios.Consultar_Deducciones_Recibo_Empleado();

            Dt_Detalles.Columns.Add("Clave_Percepcion");
            Dt_Detalles.Columns.Add("Concepto_Percepcion");
            Dt_Detalles.Columns.Add("Importe_Percepcion");
            Dt_Detalles.Columns.Add("Clave_Deduccion");
            Dt_Detalles.Columns.Add("Concepto_Deduccion");
            Dt_Detalles.Columns.Add("Importe_Deduccion");

            if (Dt_Percepciones_Recibo_Nomina != null)
            {
                if (Dt_Percepciones_Recibo_Nomina.Rows.Count > 0)
                {
                    foreach (DataRow Dr_Percepcion in Dt_Percepciones_Recibo_Nomina.Rows)
                    {
                        Fila = Dt_Detalles.NewRow();
                        Fila["Clave_Percepcion"] = Dr_Percepcion["CLAVE"].ToString().Trim();
                        Fila["Concepto_Percepcion"] = Dr_Percepcion["NOMBRE"].ToString().Trim();
                        Fila["Importe_Percepcion"] = String.Format("{0:c}", Dr_Percepcion["MONTO"]);
                        Fila["Clave_Deduccion"] = " ";
                        Fila["Concepto_Deduccion"] = " ";
                        Fila["Importe_Deduccion"] = " ";
                        Dt_Detalles.Rows.Add(Fila);
                    }
                }
            }

            if (Dt_Deducciones_Recibo_Nomina != null)
            {
                if (Dt_Deducciones_Recibo_Nomina.Rows.Count > 0)
                {
                    if (Dt_Detalles != null)
                    {
                        if (Dt_Detalles.Rows.Count > 0)
                        {
                            Contador_Detalles = Dt_Detalles.Rows.Count;
                            Contador_Fila = -1;

                            foreach (DataRow Dr_Deduccion in Dt_Deducciones_Recibo_Nomina.Rows)
                            {                                
                                if (Contador_Detalles > 0)
                                {
                                    Contador_Detalles--;
                                    Contador_Fila++;
                                    Dt_Detalles.Rows[Contador_Fila]["Clave_Deduccion"] = Dr_Deduccion["CLAVE"].ToString().Trim();
                                    Dt_Detalles.Rows[Contador_Fila]["Concepto_Deduccion"] = Dr_Deduccion["NOMBRE"].ToString().Trim();
                                    Dt_Detalles.Rows[Contador_Fila]["Importe_Deduccion"] = String.Format("{0:c}", Dr_Deduccion["MONTO"]);
                                }
                                else
                                {
                                    Fila = Dt_Detalles.NewRow();
                                    Fila["Clave_Percepcion"] = " ";
                                    Fila["Concepto_Percepcion"] = " ";
                                    Fila["Importe_Percepcion"] = " ";
                                    Fila["Clave_Deduccion"] = Dr_Deduccion["CLAVE"].ToString().Trim();
                                    Fila["Concepto_Deduccion"] = Dr_Deduccion["NOMBRE"].ToString().Trim();
                                    Fila["Importe_Deduccion"] = String.Format("{0:c}", Dr_Deduccion["MONTO"]);
                                    Dt_Detalles.Rows.Add(Fila);
                                }
                            }
                        }
                        else
                        {
                            foreach (DataRow Dr_Deduccion in Dt_Deducciones_Recibo_Nomina.Rows)
                            {
                                Fila = Dt_Detalles.NewRow();
                                Fila["Clave_Percepcion"] = " ";
                                Fila["Concepto_Percepcion"] = " ";
                                Fila["Importe_Percepcion"] = " ";
                                Fila["Clave_Deduccion"] = Dr_Deduccion["CLAVE"].ToString().Trim();
                                Fila["Concepto_Deduccion"] = Dr_Deduccion["NOMBRE"].ToString().Trim();
                                Fila["Importe_Deduccion"] = String.Format("{0:c}", Dr_Deduccion["MONTO"]);
                                Dt_Detalles.Rows.Add(Fila);
                            }
                        }
                    }
                }
            }

            Total_No_Registros = Dt_Detalles.Rows.Count;
            Dt_Detalles.TableName = Tabla;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los detalles del recibo de nomina. Error: [" + Ex.Message + "]");
        }
        return Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Detalles, Total_No_Registros);
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
    ) {
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


    //****************************************************************************************************
    //NOMBRE DE LA FUNCIÓN : Crear_Fecha
    //DESCRIPCIÓN          : Metodo para obtener la fecha en letras
    //PARAMETROS           1 Fecha: fecha a la cual le daremos formato
    //CREO                 : Leslie González Vázquez
    //FECHA_CREO           : 13/Diciembre/2011 
    //MODIFICO             :
    //FECHA_MODIFICO       :
    //CAUSA_MODIFICACIÓN   :
    //****************************************************************************************************
    internal static String Crear_Fecha(String Fecha)
    {
        String Fecha_Formateada = String.Empty;
        String Mes = String.Empty;
        String[] Fechas;

        try
        {
            Fechas = Fecha.Split('/');
            Mes = Fechas[0].ToString();
            switch (Mes)
            {
                case "01":
                    Mes = "ENERO";
                    break;
                case "02":
                    Mes = "FEBRERO";
                    break;
                case "03":
                    Mes = "MARZO";
                    break;
                case "04":
                    Mes = "ABRIL";
                    break;
                case "05":
                    Mes = "MAYO";
                    break;
                case "06":
                    Mes = "JUNIO";
                    break;
                case "07":
                    Mes = "JULIO";
                    break;
                case "08":
                    Mes = "AGOSTO";
                    break;
                case "09":
                    Mes = "SEPTIEMBRE";
                    break;
                case "10":
                    Mes = "OCTUBRE";
                    break;
                case "11":
                    Mes = "NOVIEMBRE";
                    break;
                default:
                    Mes = "DICIEMBRE";
                    break;
            }
            Fecha_Formateada = Fechas[1].ToString() + " de " + Mes + " de " + Fechas[2].ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear el fromato de fecha. Error: [" + Ex.Message + "]");
        }
        return Fecha_Formateada;
    }

    //********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Convertir_Cantidad_Letras
    ///DESCRIPCIÓN          : Metodo para convertir una cantidad a letras
    ///PROPIEDADES          :
    ///CREO                 : Juan Alberto Hernandez Negrete
    ///FECHA_CREO           : 2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected String Convertir_Cantidad_Letras(Double Cantidad_Numero)
    {
        Numalet Obj_Numale = new Numalet();
        String Cantidad_Letra = String.Empty;

        try
        {
            Obj_Numale.MascaraSalidaDecimal = "00/100 M.N.";
            Obj_Numale.SeparadorDecimalSalida = "pesos";
            Obj_Numale.ApocoparUnoParteEntera = true;
            Cantidad_Letra = Obj_Numale.ToCustomCardinal(Cantidad_Numero).Trim().ToUpper();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al convertir la cantidad a letras. Error:[" + Ex.Message + "]");
        }
        return Cantidad_Letra;
    }
    #endregion
}
