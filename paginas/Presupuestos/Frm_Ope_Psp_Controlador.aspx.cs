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
using Presidencia.Clasificacion_Gasto.Negocio;
using Presidencia.Ayudante_JQuery;
using Presidencia.Nodo_Atributos;
using Presidencia.Nodo_Arbol;
using Newtonsoft.Json;
using System.Collections.Generic;

public partial class paginas_Presupuestos_Frm_Ope_Psp_Controlador : System.Web.UI.Page
{
    #region PAGE LOAD

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Controlador_Inicio();
            }
        }

    #endregion

    #region METODOS

        #region (Metodos Generales)
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Controlador_Inicio
            ///DESCRIPCIÓN          : Metodo para el inicio de la pagina
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private void Controlador_Inicio()
            {
                String Accion = String.Empty;
                String Json_Cadena = String.Empty;
                Response.Clear();
                try
                {
                    if (this.Request.QueryString["Accion"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Accion"].ToString().Trim()))
                        {
                            Accion = this.Request.QueryString["Accion"].ToString().Trim();
                            switch (Accion)
                            {
                                case "Anios":
                                    Json_Cadena = Obtener_Anios();
                                    break;
                                case "Nivel_Fte_Financiamiento":
                                    Json_Cadena = Obtener_Fuente_Financiamiento();
                                    break;
                                case "Nivel_Area_Funcional":
                                    Json_Cadena = Obtener_Area_Funcional();
                                    break;
                                case "Nivel_Programa":
                                    Json_Cadena = Obtener_Programa();
                                    break;
                                case "Nivel_Dependencias":
                                    Json_Cadena = Obtener_Dependencias();
                                    break;
                                case "Nivel_Partida_Especifica":
                                    Json_Cadena = Obtener_Partida_Especifica();
                                    break;
                                case "Detalles":
                                    Json_Cadena = Obtener_Detalles();
                                    break;
                                case "Nivel_Capitulos":
                                    Json_Cadena = Obtener_Capitulos();
                                    break;
                                case "Nivel_Conceptos":
                                    Json_Cadena = Obtener_Conceptos();
                                    break;
                                case "Nivel_Partida_Generica":
                                    Json_Cadena = Obtener_Partida_Generica();
                                    break;
                                case "Nivel_Partida":
                                    Json_Cadena = Obtener_Partida();
                                    break;
                            }
                        }
                    }
                    Response.ContentType = "application/json";
                    Response.Write(Json_Cadena);
                    Response.Flush();
                    Response.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Controlador_Inicio Error[" + ex.Message + "]");
                }
            }
        #endregion

        #region METODOS JSON
            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Anios
            ///DESCRIPCIÓN          : Metodo para obtener los años presupuestados aprobados
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Anios()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Anios = string.Empty;
                DataTable Dt_Anios = new DataTable();

                try
                {
                    Dt_Anios = Negocio.Consultar_Anios();
                    Dt_Anios.TableName = "anios";
                    if (Dt_Anios != null)
                    {
                        if (Dt_Anios.Rows.Count > 0)
                        {
                            Json_Anios = Ayudante_JQuery.Crear_Tabla_Formato_JSON(Dt_Anios);
                        }
                    }
                    return Json_Anios;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Anios Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Fuente_Financiamiento
            ///DESCRIPCIÓN          : Metodo para obtener las fuentes de financiamiento
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Fuente_Financiamiento()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Fte_Financiamiento = "{[]}"; //aki almacenaremos el json de las fuentes de financiamiento
                DataTable Dt_Fte_Financiamiento = new DataTable(); // dt donde guardaremos las fuentes de financiamiento
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;
                String Json_Total = String.Empty;
                String Json = string.Empty;
                try
                {
                    if (this.Request.QueryString["Anio"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                            Dt_Fte_Financiamiento = Negocio.Consultar_Fuente_Financiamiento();
                            Json_Total = Obtener_Total(Dt_Fte_Financiamiento);

                            if (Dt_Fte_Financiamiento != null)
                            {
                                if (Dt_Fte_Financiamiento.Rows.Count > 0)
                                {
                                    foreach(DataRow Dr in Dt_Fte_Financiamiento.Rows)
                                    {
                                        Nodo_Arbol = new Cls_Nodo_Arbol();

                                        Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                        Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                        Nodo_Arbol.state = "closed";
                                        Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                        Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                        Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                        Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                        Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                        //Agregamos los atributos
                                        Atributos = new Cls_Atributos_TreeGrid();
                                        Atributos.valor1 = "nivel_FteFinanciamiento";
                                        Atributos.valor2 = Dr["id"].ToString().Trim();
                                        Atributos.valor3 = "";
                                        Atributos.valor4 = "";
                                        Atributos.valor5 = "";
                                        Atributos.valor6 = "";
                                        Atributos.valor7 = "";
                                        Atributos.valor8 = "";

                                        Nodo_Arbol.attributes = Atributos;
                                        Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                    }
                                    Json_Fte_Financiamiento = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);

                                    if (!String.IsNullOrEmpty(Json_Total))
                                    {
                                        Json = Json_Fte_Financiamiento.Substring(0, Json_Fte_Financiamiento.Length - 1); ;
                                        Json += "," + Json_Total.Substring(1, Json_Total.Length - 2);
                                        Json += "]";
                                    }
                                    else {
                                        Json = Json_Fte_Financiamiento;
                                    }
                                }
                            }
                        }
                    }
                    return Json;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Fuente_Financiamiento Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Area_Funcional
            ///DESCRIPCIÓN          : Metodo para obtener las areas funcionales
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Area_Funcional()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Areas = string.Empty;
                DataTable Dt_Areas = new DataTable();
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;

                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Fte_Financiamiento"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Fte_Financiamiento"].ToString().Trim()))
                            {
                                Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                Negocio.P_Fuente_Financiamiento_ID = this.Request.QueryString["Fte_Financiamiento"].ToString().Trim();
                                Dt_Areas = Negocio.Consultar_Area_Funcional();

                                if (Dt_Areas != null)
                                {
                                    if (Dt_Areas.Rows.Count > 0)
                                    {
                                        foreach (DataRow Dr in Dt_Areas.Rows)
                                        {
                                            Nodo_Arbol = new Cls_Nodo_Arbol();

                                            Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                            Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                            Nodo_Arbol.state = "closed";
                                            Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                            Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                            Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                            Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                            Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);
                                            
                                            //Agregamos los atributos
                                            Atributos = new Cls_Atributos_TreeGrid();
                                            Atributos.valor1 = "nivel_Areas";
                                            Atributos.valor2 = Dr["Fte_id"].ToString().Trim();
                                            Atributos.valor3 = Dr["Area_id"].ToString().Trim();
                                            Atributos.valor4 = "";
                                            Atributos.valor5 = "";
                                            Atributos.valor6 = "";
                                            Atributos.valor7 = "";
                                            Atributos.valor8 = "";

                                            Nodo_Arbol.attributes = Atributos;
                                            Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                        }
                                        Json_Areas = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                    }
                                }
                            }
                        }
                    }
                    return Json_Areas;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Area_Funcional Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Programa
            ///DESCRIPCIÓN          : Metodo para obtener los programas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Programa()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Programas = string.Empty;
                DataTable Dt_Programas = new DataTable();
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;

                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Fte_Financiamiento"] != null && this.Request.QueryString["Area"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Area"].ToString().Trim()))
                            {
                                if (!String.IsNullOrEmpty(this.Request.QueryString["Fte_Financiamiento"].ToString().Trim()))
                                {
                                    Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                    Negocio.P_Area_Funcional_ID = this.Request.QueryString["Area"].ToString().Trim();
                                    Negocio.P_Fuente_Financiamiento_ID = this.Request.QueryString["Fte_Financiamiento"].ToString().Trim();
                                    Dt_Programas = Negocio.Consultar_Programa();

                                    if (Dt_Programas != null)
                                    {
                                        if (Dt_Programas.Rows.Count > 0)
                                        {
                                            foreach (DataRow Dr in Dt_Programas.Rows)
                                            {
                                                Nodo_Arbol = new Cls_Nodo_Arbol();

                                                Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                                Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                                Nodo_Arbol.state = "closed";
                                                Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                                Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                                Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                                Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                                Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                                //Agregamos los atributos
                                                Atributos = new Cls_Atributos_TreeGrid();
                                                Atributos.valor1 = "nivel_Programas";
                                                Atributos.valor2 = Dr["Fte_id"].ToString().Trim();
                                                Atributos.valor3 = Dr["Area_id"].ToString().Trim();
                                                Atributos.valor4 = Dr["Programa_id"].ToString().Trim();
                                                Atributos.valor5 = "";
                                                Atributos.valor6 = "";
                                                Atributos.valor7 = "";
                                                Atributos.valor8 = "";

                                                Nodo_Arbol.attributes = Atributos;
                                                Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                            }
                                            Json_Programas = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json_Programas;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Programa Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Dependencias
            ///DESCRIPCIÓN          : Metodo para obtener las dependencias
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 30/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Dependencias()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Dependencias = string.Empty;
                DataTable Dt_Dependencias = new DataTable();
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;

                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Fte_Financiamiento"] != null && this.Request.QueryString["Programa"] != null && this.Request.QueryString["Area"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Fte_Financiamiento"].ToString().Trim()))
                            {
                                if (!String.IsNullOrEmpty(this.Request.QueryString["Programa"].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(this.Request.QueryString["Area"].ToString().Trim()))
                                    {
                                        Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                        Negocio.P_Fuente_Financiamiento_ID = this.Request.QueryString["Fte_Financiamiento"].ToString().Trim();
                                        Negocio.P_Programa_ID = this.Request.QueryString["Programa"].ToString().Trim();
                                        Negocio.P_Area_Funcional_ID = this.Request.QueryString["Area"].ToString().Trim();

                                        Dt_Dependencias = Negocio.Consultar_Dependencias();

                                        if (Dt_Dependencias != null)
                                        {
                                            if (Dt_Dependencias.Rows.Count > 0)
                                            {
                                                foreach (DataRow Dr in Dt_Dependencias.Rows)
                                                {
                                                    Nodo_Arbol = new Cls_Nodo_Arbol();

                                                    Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                                    Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                                    Nodo_Arbol.state = "closed";
                                                    Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                                    Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                                    Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                                    Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                                    Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                                    //Agregamos los atributos
                                                    Atributos = new Cls_Atributos_TreeGrid();
                                                    Atributos.valor1 = "nivel_Dependencia";
                                                    Atributos.valor2 = Dr["Fte_id"].ToString().Trim();
                                                    Atributos.valor3 = Dr["Area_id"].ToString().Trim();
                                                    Atributos.valor4 = Dr["Programa_id"].ToString().Trim();
                                                    Atributos.valor5 = Dr["Dependencia_id"].ToString().Trim();
                                                    Atributos.valor6 = "";
                                                    Atributos.valor7 = "";
                                                    Atributos.valor8 = "";

                                                    Nodo_Arbol.attributes = Atributos;
                                                    Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                                }
                                                Json_Dependencias = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json_Dependencias;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Dependencias Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Partida_Especifica
            ///DESCRIPCIÓN          : Metodo para obtener las partidas especificas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 02/Diciembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Partida_Especifica()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Partida_Especifica = string.Empty;
                DataTable Dt_Partida_Especifica = new DataTable();
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;

                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Fte_Financiamiento"] != null && this.Request.QueryString["Programa"] != null && this.Request.QueryString["Area"] != null && this.Request.QueryString["Dependencia"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Fte_Financiamiento"].ToString().Trim()))
                            {
                                if (!String.IsNullOrEmpty(this.Request.QueryString["Programa"].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(this.Request.QueryString["Area"].ToString().Trim()))
                                    {
                                        if (!String.IsNullOrEmpty(this.Request.QueryString["Dependencia"].ToString().Trim()))
                                        {
                                            Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                            Negocio.P_Fuente_Financiamiento_ID = this.Request.QueryString["Fte_Financiamiento"].ToString().Trim();
                                            Negocio.P_Programa_ID = this.Request.QueryString["Programa"].ToString().Trim();
                                            Negocio.P_Area_Funcional_ID = this.Request.QueryString["Area"].ToString().Trim();
                                            Negocio.P_Dependencia_ID = this.Request.QueryString["Dependencia"].ToString().Trim();
                                            Dt_Partida_Especifica = Negocio.Consultar_Partidas_Especificas();

                                            if (Dt_Partida_Especifica != null)
                                            {
                                                if (Dt_Partida_Especifica.Rows.Count > 0)
                                                {
                                                    foreach (DataRow Dr in Dt_Partida_Especifica.Rows)
                                                    {
                                                        Nodo_Arbol = new Cls_Nodo_Arbol();

                                                        Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                                        Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                                        Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                                        Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                                        Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                                        Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                                        Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                                        //Agregamos los atributos
                                                        Atributos = new Cls_Atributos_TreeGrid();
                                                        Atributos.valor1 = "nivel_Partida";
                                                        Atributos.valor2 = Dr["Fte_id"].ToString().Trim();
                                                        Atributos.valor3 = Dr["Area_id"].ToString().Trim();
                                                        Atributos.valor4 = Dr["Programa_id"].ToString().Trim();
                                                        Atributos.valor5 = Dr["Dependencia_id"].ToString().Trim();
                                                        Atributos.valor6 = Dr["Partida_id"].ToString().Trim(); ;
                                                        Atributos.valor7 = "";
                                                        Atributos.valor8 = "";

                                                        Nodo_Arbol.attributes = Atributos;
                                                        Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                                    }
                                                    Json_Partida_Especifica = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json_Partida_Especifica;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Partida_Especifica Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Detalles
            ///DESCRIPCIÓN          : Metodo para obtener los detalles de la clasificacion
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 03/Diciembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Detalles()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Detalles = string.Empty;
                DataTable Dt_Detalles = new DataTable();
               
                try
                {
                    if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                    {
                        Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                    }

                    if (!String.IsNullOrEmpty(this.Request.QueryString["Fte_Financiamiento"].ToString().Trim()))
                    {
                        Negocio.P_Fuente_Financiamiento_ID = this.Request.QueryString["Fte_Financiamiento"].ToString().Trim();
                    }

                    if (!String.IsNullOrEmpty(this.Request.QueryString["Area"].ToString().Trim()))
                    {
                        Negocio.P_Area_Funcional_ID = this.Request.QueryString["Area"].ToString().Trim();
                    }

                    if (!String.IsNullOrEmpty(this.Request.QueryString["Programa"].ToString().Trim()))
                    {
                        Negocio.P_Programa_ID = this.Request.QueryString["Programa"].ToString().Trim();
                    }

                    if (!String.IsNullOrEmpty(this.Request.QueryString["Dependencia"].ToString().Trim()))
                    {
                        Negocio.P_Dependencia_ID = this.Request.QueryString["Dependencia"].ToString().Trim();
                    }
                    if (!String.IsNullOrEmpty(this.Request.QueryString["Partida"].ToString().Trim()))
                    {
                        Negocio.P_Partida_Especifica_ID = this.Request.QueryString["Partida"].ToString().Trim();
                    }
                    if (!String.IsNullOrEmpty(this.Request.QueryString["Tipo_Descripcion"].ToString().Trim()))
                    {
                        Negocio.P_Tipo_Descripcion = this.Request.QueryString["Tipo_Descripcion"].ToString().Trim();
                    }

                    Dt_Detalles = Negocio.Consultar_Detalles();

                    if (Dt_Detalles != null) 
                    {
                        if(Dt_Detalles.Rows.Count > 0)
                        {
                            Dt_Detalles.Columns.Add("SALDO_FORMATEADO");
                            Dt_Detalles.Columns.Add("IMPORTE_FORMATEADO");
                            foreach(DataRow Dr in Dt_Detalles.Rows)
                            {
                                Dr["SALDO_FORMATEADO"] = String.Format("{0:c}", Dr["SALDO"]);
                                Dr["IMPORTE_FORMATEADO"] = String.Format("{0:c}", Convert.ToDouble(String.IsNullOrEmpty(Dr["IMPORTE"].ToString()) ? "0" : Dr["IMPORTE"].ToString().Trim()));
                            }

                            Dt_Detalles.TableName = "rows";
                            Json_Detalles = Ayudante_JQuery.Crear_Tabla_Formato_JSON_DataGrid(Dt_Detalles, Dt_Detalles.Rows.Count);
                        }
                    }
                    

                    return Json_Detalles;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Detalles Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Capitulos
            ///DESCRIPCIÓN          : Metodo para obtener los capitulos
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Capitulos()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Capitulos = "{[]}"; //aki almacenaremos el json de los datos
                DataTable Dt_Capitulos = new DataTable(); // dt donde guardaremos los datos
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;
                String Json_Total = String.Empty;
                String Json = string.Empty;

                try
                {
                    if (this.Request.QueryString["Anio"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                            Dt_Capitulos = Negocio.Consultar_Capitulos();
                            Json_Total = Obtener_Total(Dt_Capitulos);
                            if (Dt_Capitulos != null)
                            {
                                if (Dt_Capitulos.Rows.Count > 0)
                                {
                                    foreach (DataRow Dr in Dt_Capitulos.Rows)
                                    {
                                        Nodo_Arbol = new Cls_Nodo_Arbol();

                                        Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                        Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                        Nodo_Arbol.state = "closed";
                                        Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                        Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                        Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                        Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                        Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                        //Agregamos los atributos
                                        Atributos = new Cls_Atributos_TreeGrid();
                                        Atributos.valor1 = "nivel_Capitulos";
                                        Atributos.valor2 = Dr["id"].ToString().Trim();
                                        Atributos.valor3 = "";
                                        Atributos.valor4 = "";
                                        Atributos.valor5 = "";
                                        Atributos.valor6 = "";
                                        Atributos.valor7 = "";
                                        Atributos.valor8 = "";

                                        Nodo_Arbol.attributes = Atributos;
                                        Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                    }
                                    Json_Capitulos = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);

                                    if (!String.IsNullOrEmpty(Json_Total))
                                    {
                                        Json = Json_Capitulos.Substring(0, Json_Capitulos.Length - 1); ;
                                        Json += "," + Json_Total.Substring(1, Json_Total.Length - 2);
                                        Json += "]";
                                    }
                                    else
                                    {
                                        Json = Json_Capitulos;
                                    }
                                }
                            }
                        }
                    }
                    return Json;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Capitulos Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Conceptos
            ///DESCRIPCIÓN          : Metodo para obtener los conceptos
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Conceptos()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Conceptos = "{[]}"; //aki almacenaremos el json de los datos
                DataTable Dt_Conceptos = new DataTable(); // dt donde guardaremos los datos
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;


                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Capitulo"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Capitulo"].ToString().Trim()))
                            {
                                Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                Negocio.P_Capitulo_ID = this.Request.QueryString["Capitulo"].ToString().Trim();
                                Dt_Conceptos = Negocio.Consultar_Conceptos();

                                if (Dt_Conceptos != null)
                                {
                                    if (Dt_Conceptos.Rows.Count > 0)
                                    {
                                        foreach (DataRow Dr in Dt_Conceptos.Rows)
                                        {
                                            Nodo_Arbol = new Cls_Nodo_Arbol();

                                            Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                            Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                            Nodo_Arbol.state = "closed";
                                            Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                            Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                            Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                            Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                            Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                            //Agregamos los atributos
                                            Atributos = new Cls_Atributos_TreeGrid();
                                            Atributos.valor1 = "nivel_Concepto";
                                            Atributos.valor2 = Dr["Capitulo_id"].ToString().Trim();
                                            Atributos.valor3 = Dr["Concepto_id"].ToString().Trim();
                                            Atributos.valor4 = "";
                                            Atributos.valor5 = "";
                                            Atributos.valor6 = "";
                                            Atributos.valor7 = "";
                                            Atributos.valor8 = "";

                                            Nodo_Arbol.attributes = Atributos;
                                            Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                        }
                                        Json_Conceptos = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                    }
                                }
                            }
                        }
                    }
                    return Json_Conceptos;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Conceptos Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Partida_Generica
            ///DESCRIPCIÓN          : Metodo para obtener las partidas genericas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Diciembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Partida_Generica()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Partida_Generica = "{[]}"; //aki almacenaremos el json de las Partidas Genericas
                DataTable Dt_Partida_Generica = new DataTable(); // dt donde guardaremos las Partidas Genericas
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;


                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Capitulo"] != null && this.Request.QueryString["Concepto"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Capitulo"].ToString().Trim()))
                            {
                                if (!String.IsNullOrEmpty(this.Request.QueryString["Concepto"].ToString().Trim()))
                                {
                                    Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                    Negocio.P_Capitulo_ID = this.Request.QueryString["Capitulo"].ToString().Trim();
                                    Negocio.P_Concepto_ID = this.Request.QueryString["Concepto"].ToString().Trim();

                                    Dt_Partida_Generica = Negocio.Consultar_Partida_Generica();

                                    if (Dt_Partida_Generica != null)
                                    {
                                        if (Dt_Partida_Generica.Rows.Count > 0)
                                        {
                                            foreach (DataRow Dr in Dt_Partida_Generica.Rows)
                                            {
                                                Nodo_Arbol = new Cls_Nodo_Arbol();

                                                Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                                Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                                Nodo_Arbol.state = "closed";
                                                Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                                Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                                Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                                Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                                Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                                //Agregamos los atributos
                                                Atributos = new Cls_Atributos_TreeGrid();
                                                Atributos.valor1 = "nivel_Partida_Generica";
                                                Atributos.valor2 = Dr["Capitulo_id"].ToString().Trim();
                                                Atributos.valor3 = Dr["Concepto_id"].ToString().Trim();
                                                Atributos.valor4 = Dr["Partida_Generica_id"].ToString().Trim();
                                                Atributos.valor5 = "";
                                                Atributos.valor6 = "";
                                                Atributos.valor7 = "";
                                                Atributos.valor8 = "";

                                                Nodo_Arbol.attributes = Atributos;
                                                Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                            }
                                            Json_Partida_Generica = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json_Partida_Generica;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Partida_Generica Error[" + ex.Message + "]");
                }
            }

            ///********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Partida
            ///DESCRIPCIÓN          : Metodo para obtener las partidas
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 05/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Partida()
            {
                Cls_Ope_Psp_Clasificacion_Gasto_Negocio Negocio = new Cls_Ope_Psp_Clasificacion_Gasto_Negocio(); //conexion con la capa de negocios
                String Json_Partida = "{[]}"; //aki almacenaremos el json de las partidas
                DataTable Dt_Partida = new DataTable(); // dt donde guardaremos las partidas
                Cls_Nodo_Arbol Nodo_Arbol = new Cls_Nodo_Arbol(); // Objero de la clase de nodo arbol
                List<Cls_Nodo_Arbol> Lista_Nodo_Arbol = new List<Cls_Nodo_Arbol>();
                Cls_Atributos_TreeGrid Atributos = new Cls_Atributos_TreeGrid();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;


                try
                {
                    if (this.Request.QueryString["Anio"] != null && this.Request.QueryString["Capitulo"] != null && this.Request.QueryString["Concepto"] != null && this.Request.QueryString["Partida_Generica"] != null)
                    {
                        if (!String.IsNullOrEmpty(this.Request.QueryString["Anio"].ToString().Trim()))
                        {
                            if (!String.IsNullOrEmpty(this.Request.QueryString["Capitulo"].ToString().Trim()))
                            {
                                if (!String.IsNullOrEmpty(this.Request.QueryString["Concepto"].ToString().Trim()))
                                {
                                    if (!String.IsNullOrEmpty(this.Request.QueryString["Partida_Generica"].ToString().Trim()))
                                    {
                                        Negocio.P_Anio = this.Request.QueryString["Anio"].ToString().Trim();
                                        Negocio.P_Capitulo_ID = this.Request.QueryString["Capitulo"].ToString().Trim();
                                        Negocio.P_Concepto_ID = this.Request.QueryString["Concepto"].ToString().Trim();
                                        Negocio.P_Partida__Generica_ID = this.Request.QueryString["Partida_Generica"].ToString().Trim();
                                        Dt_Partida = Negocio.Consultar_Partidas();

                                        if (Dt_Partida != null)
                                        {
                                            if (Dt_Partida.Rows.Count > 0)
                                            {
                                                foreach (DataRow Dr in Dt_Partida.Rows)
                                                {
                                                    Nodo_Arbol = new Cls_Nodo_Arbol();

                                                    Nodo_Arbol.id = Dr["id"].ToString().Trim();
                                                    Nodo_Arbol.texto = Dr["nombre"].ToString().Trim();
                                                    Nodo_Arbol.descripcion1 = String.Format("{0:c}", Dr["DEVENGADO"]);
                                                    Nodo_Arbol.descripcion2 = String.Format("{0:c}", Dr["EJERCIDO"]);
                                                    Nodo_Arbol.descripcion3 = String.Format("{0:c}", Dr["PAGADO"]);
                                                    Nodo_Arbol.descripcion4 = String.Format("{0:c}", Dr["COMPROMETIDO"]);
                                                    Nodo_Arbol.descripcion5 = String.Format("{0:c}", Dr["DISPONIBLE"]);

                                                    //Agregamos los atributos
                                                    Atributos = new Cls_Atributos_TreeGrid();
                                                    Atributos.valor1 = "nivel_FteFinanciamiento";
                                                    Atributos.valor2 = Dr["Capitulo_id"].ToString().Trim();
                                                    Atributos.valor3 = Dr["Concepto_id"].ToString().Trim();
                                                    Atributos.valor4 = Dr["Partida_Generica_id"].ToString().Trim();
                                                    Atributos.valor5 = Dr["Partida_id"].ToString().Trim();
                                                    Atributos.valor6 = "";
                                                    Atributos.valor7 = "";
                                                    Atributos.valor8 = "";

                                                    Nodo_Arbol.attributes = Atributos;
                                                    Lista_Nodo_Arbol.Add(Nodo_Arbol);
                                                }
                                                Json_Partida = JsonConvert.SerializeObject(Lista_Nodo_Arbol, Formatting.Indented, Configuracion_Json);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    return Json_Partida;
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al Obtener_Partida Error[" + ex.Message + "]");
                }
            }

            //********************************************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Obtener_Total
            ///DESCRIPCIÓN          : Metodo para obtener el total
            ///PROPIEDADES          :
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 06/Noviembre/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN...:
            ///*********************************************************************************************************
            private String Obtener_Total(DataTable Dt_Datos) 
            {
                Double Devengado = 0.00;
                Double Disponible = 0.00;
                Double Comprometido = 0.00;
                Double Ejercido = 0.00;
                Double Pagado = 0.00;
                String Json_Total = string.Empty;
                Json_Total = "{[]}";
                DataTable Dt_Total = new DataTable();
                JsonSerializerSettings Configuracion_Json = new JsonSerializerSettings();
                Configuracion_Json.NullValueHandling = NullValueHandling.Ignore;
                DataRow Fila;
                
                try 
                { 
                    if(Dt_Datos != null)
                    {
                        if(Dt_Datos.Rows.Count > 0)
                        {
                            Dt_Total.Columns.Add("id");
                            Dt_Total.Columns.Add("texto");
                            Dt_Total.Columns.Add("descripcion1");
                            Dt_Total.Columns.Add("descripcion2");
                            Dt_Total.Columns.Add("descripcion3");
                            Dt_Total.Columns.Add("descripcion4");
                            Dt_Total.Columns.Add("descripcion5");

                            foreach (DataRow Dr in Dt_Datos.Rows)
                            {
                                Devengado += Convert.ToDouble(String.IsNullOrEmpty(Dr["DEVENGADO"].ToString().Trim()) ? "0" : Dr["DEVENGADO"].ToString().Trim());
                                Ejercido += Convert.ToDouble(String.IsNullOrEmpty(Dr["EJERCIDO"].ToString().Trim()) ? "0" : Dr["EJERCIDO"].ToString().Trim());
                                Pagado += Convert.ToDouble(String.IsNullOrEmpty(Dr["PAGADO"].ToString().Trim()) ? "0" : Dr["PAGADO"].ToString().Trim());
                                Comprometido += Convert.ToDouble(String.IsNullOrEmpty(Dr["COMPROMETIDO"].ToString().Trim()) ? "0" : Dr["COMPROMETIDO"].ToString().Trim());
                                Disponible += Convert.ToDouble(String.IsNullOrEmpty(Dr["DISPONIBLE"].ToString().Trim()) ? "0" : Dr["DISPONIBLE"].ToString().Trim());
                            }

                            Fila = Dt_Total.NewRow();
                            Fila["id"] = "footer";
                            Fila["texto"] = "Total";
                            Fila["descripcion1"] = String.Format("{0:c}", Devengado);
                            Fila["descripcion2"] = String.Format("{0:c}", Ejercido);
                            Fila["descripcion3"] = String.Format("{0:c}", Pagado);
                            Fila["descripcion4"] = String.Format("{0:c}", Comprometido);
                            Fila["descripcion5"] = String.Format("{0:c}", Disponible);
                            Dt_Total.Rows.Add(Fila);

                            Json_Total = JsonConvert.SerializeObject(Dt_Total, Formatting.Indented, Configuracion_Json);

                        }
                    }
                    return Json_Total;
                }
                catch(Exception Ex)
                {
                    throw new Exception(" Error al tratar de Obtener_Total Error[" + Ex.Message + "]");
                }
            }
        #endregion

        #endregion
}
