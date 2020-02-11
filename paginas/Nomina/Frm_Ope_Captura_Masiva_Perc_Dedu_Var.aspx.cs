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
using Presidencia.Constantes;
using Presidencia.Deducciones_Variables.Negocio;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Empleados.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Dependencias.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using System.IO;
using AjaxControlToolkit;
using System.Reflection;
using System.Text;

public partial class paginas_Nomina_Frm_Ope_Captura_Masiva_Perc_Dedu_Var : System.Web.UI.Page
{
    #region (Page_Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        //Estilo para agregar al boton que cuando el cursor este sobre de el.
        String Estilo_Raton_Sobre = "this.style.backgroundColor='#DFE8F6';this.style.cursor='hand';this.style.color='DarkBlue';" +
            "this.style.borderStyle='none';this.style.borderColor='Silver';";

        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
            }
            Btn_Busqueda_Deducciones.Attributes.Add("onmouseover", Estilo_Raton_Sobre);//Se agrega el estilo que tendra boton al estar el mouse sobrede el.
            //Se agrega el estilo que tendra el boton cuando el mouse salga fuera de el.
            Btn_Busqueda_Deducciones.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();
        Habilitar_Controles("Inicial");

        Consultar_Deducciones_Variables_Asignadas();
        Consultar_Deducciones_Variables_Opcionales();
        Consultar_Calendarios_Nomina();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Deduccion.Text = "";
            Cmb_Deducciones.SelectedIndex = -1;
            Cmb_Estatus.SelectedIndex = -1;
            Txt_Comentarios.Text = "";
            Txt_Busqueda_No_Deduccion.Text = "";
            Cmb_Busqueda_Estatus.SelectedIndex = -1;
            Grid_Deducciones_Variables.SelectedIndex = -1;

            Grid_Empleados.Columns[0].Visible = true;
            Grid_Empleados.SelectedIndex = -1;
            Grid_Empleados.DataSource = new DataTable();
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[0].Visible = false;

            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles del formulario. Error: [" + Ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Busqueda_No_Deduccion_Variable.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    TPnl_Contenedor.ActiveTabIndex = 0;

                    Cmb_Busqueda_Estatus.SelectedIndex = Cmb_Busqueda_Estatus.Items.IndexOf(Cmb_Busqueda_Estatus.Items.FindByText("Pendiente"));

                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    Btn_Busqueda_No_Deduccion_Variable.Enabled = false;
                    if (Session["Dt_Empleados"] != null) Session.Remove("Dt_Empleados");

                    TPnl_Contenedor.ActiveTabIndex = 1;
                    Cmb_Estatus.SelectedIndex = 1;
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";

                    Btn_Busqueda_No_Deduccion_Variable.Enabled = false;
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    break;
            }

            Txt_No_Deduccion.Enabled = false;
            Cmb_Deducciones.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Grid_Deducciones_Variables.Enabled = !Habilitado;
            Grid_Empleados.Enabled = Habilitado;

            Cmb_Estatus.Enabled = false;
            Cmb_Estatus.SelectedIndex = 2;
            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;

            AFU_Cargar_Archivo_Variables.Enabled = Habilitado;
            Btn_Cargar_Empleados.Enabled = Habilitado;
            Btn_Limpiar_Empleados.Enabled = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al Habilitar los Controles del formulario. Error:[" + ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Asignacion_Deducciones_Variables
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Asignacion_Deducciones_Variables()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Deducciones.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione alguna deduccion variable <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Estatus <br>";
            Datos_Validos = false;
        }

        //if (string.IsNullOrEmpty(Txt_Comentarios.Text))
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comentarios <br>";
        //    Datos_Validos = false;
        //}

        if (Grid_Empleados.Rows.Count <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se ha asignado a ningún empleado<br>";
            Datos_Validos = false;
        }

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se a seleccionado ninguna nómina. <br />";
            Datos_Validos = false;
        }

        if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")))
        {
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se a seleccionado ningún periodo nominal. <br />";
                Datos_Validos = false;
            }
        }


        return Datos_Validos;
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
    private Boolean IsNumeric(String Cadena)
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 21/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
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
    /// FECHA_CREO  : 01/Diciembre/2010
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
        }
    }
    ///********************************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Leer_Archivo_Obtener_Historial_Nomina_Generada
    ///
    ///DESCRIPCIÓN: Lee los empleados del archivo para crear una tabla con la información del mismo.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*********************************************************************************************************************************************************
    protected DataTable Leer_Archivo_Canceptos_Variables(String Ruta, String Nombre_Archivo, String Extencion)
    {
        StreamReader Lector = null;//Variable que leerá el archivo que almacena los registros que fueron afectados al Generar la Nómina que se desea Regenerar.
        String Linea_Leida = "";//Variable que leerá cada línea del archivo. para su tratamiento como cadena.
        String[] Columnas;//Variable que contendrá todos lo campos que se guardaron por cada registro que se almaceno en cada línea del archivo.
        DataRow Renglon_Insertar = null;//Variable que almacenrá cada renglon contruido, y que se insertara en una respectiva tabla
        DataTable Dt_Empleados = new DataTable("VARIABLES");//Tabla [PRESTAMOS] con los registros que fueron afectados al Generar la Nómina.

        try
        {


            //Se válida que exista un archivo.
            if (File.Exists(@"" + (Ruta + Nombre_Archivo + Extencion)))
            {
                //Si existe el archivo el siguiente paso, es crear el objeto que nos ayudará a leer el archivo.
                Lector = new StreamReader(@"" + (Ruta + Nombre_Archivo + Extencion));

                //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(String));
                Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(String));
                Dt_Empleados.Columns.Add("CANTIDAD", typeof(String));
                Dt_Empleados.Columns.Add(Cat_Dependencias.Campo_Dependencia_ID, typeof(String));

                //Leer el archivo, hasta llegar al final del mismo.
                while ((Linea_Leida = Lector.ReadLine()) != null)
                {
                    Renglon_Insertar = null;//Limpiamos el renglon, para que este disponible para cuando se realize el siguiente registro.
                    Linea_Leida = Linea_Leida.Replace("[", "");//Eliminamos el carácter [ de la cadena leida.
                    Linea_Leida = Linea_Leida.Replace("]", "");//Eliminamos el carácter ] de la cadena leida.
                    Columnas = Linea_Leida.Split(new Char[] { ',' });//Obtenemos un arreglo con un número de elementos igual, al número de

                    if (Columnas.Length == 4)
                    {

                        Renglon_Insertar = Dt_Empleados.NewRow();
                        Renglon_Insertar[Cat_Empleados.Campo_Empleado_ID] = Consultar_Empleado_ID(Columnas[0]);
                        Renglon_Insertar[Cat_Empleados.Campo_Nombre] = Columnas[1];
                        Renglon_Insertar["CANTIDAD"] = Columnas[2];
                        Renglon_Insertar[Cat_Dependencias.Campo_Dependencia_ID] = Columnas[3];
                        Dt_Empleados.Rows.Add(Renglon_Insertar);
                    }
                }
                Lector.Close();//Cerramos el lector del archivo.
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al escribir el Archivo " + Nombre_Archivo + ". Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
    }
    /// ****************************************************************************************************************
    /// Nombre: Consultar_Empleado_ID
    /// 
    /// Descripción: Consulta la información del empleado y obtiene el identifacador del empleado.
    /// 
    /// Parámetros: No_Empleado.- Es el identificador del empleado para uso de recursos humanos.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete
    /// Fecha Creo: 6/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ****************************************************************************************************************
    protected String Consultar_Empleado_ID(String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empelado = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa d enegocios.
        DataTable Dt_Empleado = null;//Variable que guarda la lista de empleados que se consulto del archivo.
        String Empleado_ID = String.Empty;//Variable que almacenara el identificador del empleado.

        try
        {
            Obj_Empelado.P_No_Empleado = No_Empleado;
            Dt_Empleado = Obj_Empelado.Consulta_Empleados_General();

            if (Dt_Empleado is DataTable)
            {
                if (Dt_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los ");
        }
        return Empleado_ID;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Remover_Sesiones_Control_Carga_Archivos
    /// DESCRIPCION : Remueve la sesion del Ctlr AsyncFileUpload que mantiene al archivo
    /// en memoria.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 27/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Remover_Sesiones_Control_Carga_Archivos(String Client_ID)
    {
        HttpContext currentContext;
        if (HttpContext.Current != null && HttpContext.Current.Session != null)
        {
            currentContext = HttpContext.Current;
        }
        else
        {
            currentContext = null;
        }

        if (currentContext != null)
        {
            foreach (String key in currentContext.Session.Keys)
            {
                if (key.Contains(Client_ID))
                {
                    currentContext.Session.Remove(key);
                    break;
                }
            }
        }
    }
    /// *****************************************************************************************************
    /// Nombre: Leer_Archivo_Excel
    /// 
    /// Descripción: Lee un archivo de excel y retorna una tabla con las columnas definidas en el doc.
    /// 
    /// Parámetros: Ruta.- Parámetro que almacena la ruta del archivo.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 06/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    public DataTable Leer_Archivo_Excel(String Ruta)
    {
        Int32 Contador_Columnas = 1;
        Int32 Contador_Filas = 0;
        DataTable Dt_Empleados = new DataTable();
        DataRow[] Empleados = new DataRow[] { };

        //Declaro las variables necesarias
        Microsoft.Office.Interop.Excel._Application xlApp;
        Microsoft.Office.Interop.Excel._Workbook xlLibro;
        Microsoft.Office.Interop.Excel._Worksheet xlHoja1;
        Microsoft.Office.Interop.Excel.Sheets xlHojas;
        //asigno la ruta dónde se encuentra el archivo
        String Ruta_Archivo = Ruta;
        //inicializo la variable xlApp (referente a la aplicación)
        xlApp = new Microsoft.Office.Interop.Excel.Application();
        //Muestra la aplicación Excel si está en true
        xlApp.Visible = false;
        //Abrimos el libro a leer (documento excel)
        xlLibro = xlApp.Workbooks.Open(Ruta_Archivo, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
            Missing.Value, Missing.Value);

        try
        {
            //Asignamos las hojas
            xlHojas = xlLibro.Sheets;
            //Asignamos la hoja con la que queremos trabajar: 
            xlHoja1 = (Microsoft.Office.Interop.Excel._Worksheet)xlHojas["Hoja1"];

            //recorremos las celdas que queremos y sacamos los datos 
            while (!((string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text).Trim().Equals(""))
            {
                if (Contador_Filas == 1)
                {
                    HTxt_Referencia.Value =(string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text;
                }

                if (Contador_Filas == 2)
                {
                    //Se crea la estructura de la tabla que alamcenará los registros que fueron afectados 
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(String));
                    Dt_Empleados.Columns.Add("NOMBRE", typeof(String));
                    Dt_Empleados.Columns.Add("CANTIDAD", typeof(String));
                }
                else if (Contador_Filas > 2)
                {
                    string No_Empleado = Consultar_Empleado_ID(((string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text));
                    string Nombre = "[" + String.Format("{0:000000}", Convert.ToInt32(((string)xlHoja1.get_Range("A" + Contador_Columnas, Missing.Value).Text))) + "] -- "+
                        (string)xlHoja1.get_Range("B" + Contador_Columnas, Missing.Value).Text;
                    string Cantidad = (string)xlHoja1.get_Range("C" + Contador_Columnas, Missing.Value).Text;

                    if (!String.IsNullOrEmpty(No_Empleado))
                    {
                        if (Dt_Empleados.Rows.Count > 0)
                            Empleados = Dt_Empleados.Select(Cat_Empleados.Campo_Empleado_ID + "=" + No_Empleado);

                        if (Empleados.Length <= 0)
                        {
                            DataRow Dr_Insertar = Dt_Empleados.NewRow();
                            Dr_Insertar[0] = No_Empleado;
                            Dr_Insertar[1] = Nombre;
                            Dr_Insertar[2] = String.Format("{0:c}", Convert.ToDouble(String.IsNullOrEmpty(Cantidad) ? "0" : Cantidad));
                            Dt_Empleados.Rows.Add(Dr_Insertar);
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text = "Hay empleados duplicados.";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;

                            Dt_Empleados = new DataTable();
                            break;
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "Hay empleados que no existen en el sistema.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;

                        Dt_Empleados = new DataTable();
                        break;
                    }
                }
                Contador_Columnas++;
                Contador_Filas++;
            }

            return Dt_Empleados;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Hay información en el archivo que no viene de forma correcta.";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        finally
        {
            //Cerrar el Libro
            xlLibro.Close(false, Missing.Value, Missing.Value);
            //Cerrar la Aplicación
            xlApp.Quit();
        }
        return Dt_Empleados;
    }
    /// ***************************************************************************************************
    /// Nombre: Quitar_Caracteres_Cantidad
    /// 
    /// Descripción: Recorrecorre todas las celdas de la tabla y revisa si en alguna celda hay algun 
    ///              caracter de simbolo de pesos y lo elimina.
    /// 
    /// Parámetros: Dt_Perc_Deduc_Empl.- Tabla la cuál se recorrera en para eliminar los caracteres
    ///             de simbolo de pesos.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: 14/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected DataTable Quitar_Caracteres_Cantidad(DataTable Dt_Perc_Deduc_Empl)
    {
        try
        {
            if (Dt_Perc_Deduc_Empl is DataTable)
            {
                if (Dt_Perc_Deduc_Empl.Rows.Count > 0)
                {
                    foreach (DataRow FILA in Dt_Perc_Deduc_Empl.Rows)
                    {
                        if (FILA is DataRow)
                        {
                            if (Dt_Perc_Deduc_Empl.Columns.Count > 0)
                            {
                                foreach (DataColumn COLUMNA in Dt_Perc_Deduc_Empl.Columns)
                                {
                                    if (COLUMNA is DataColumn)
                                    {
                                        FILA[COLUMNA.ColumnName.Trim()] = FILA[COLUMNA.ColumnName.Trim()].ToString().Trim().Replace("$", "");
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
            throw new Exception("Error al quitar los signos de pesos. Error: [" + Ex.Message + "]");
        }
        return Dt_Perc_Deduc_Empl;
    }
    /// *************************************************************************************
    /// Nombre: Copiar_Archivo_Servidor
    /// 
    /// Descripción: Elimina y vuelve a copiar el archivo.
    /// 
    /// Parámetros: Ctrl que se uso para cargar el archivo.
    /// 
    /// Usuario Creo: Juan Alberto Hernandez Negrete.
    /// Fecha Creo: 06/Diciembre/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// *************************************************************************************
    private String Copiar_Archivo_Servidor(AsyncFileUpload Ctlr_AFU_Archivo)
    {
        String Extension = String.Empty;
        String Ruta = String.Empty;
        String Full_Path = String.Empty;

        try
        {
            if (Ctlr_AFU_Archivo is AsyncFileUpload)
            {
                if (Ctlr_AFU_Archivo.HasFile)
                {
                    Extension = Path.GetExtension(Ctlr_AFU_Archivo.PostedFile.FileName);//Obtenemos la extensión del archivo.
                    Ruta = Server.MapPath("Archivos");
                    Full_Path = Ruta + "/Archivo" + Extension;

                    if (File.Exists(Full_Path))
                    {
                        File.Delete(Full_Path);
                    }

                    Ctlr_AFU_Archivo.SaveAs(Full_Path);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al copiar los archivos al servidor. Error: [" + Ex.Message + "]");
        }
        return Full_Path;
    }
    #endregion

    #region (Metodos Alta - Modificar - Actualizar)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Asignacion_Deducciones_Variables
    /// DESCRIPCION : Ejecuta el alta de una Asiganacion de la Deduccion Variable
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Asignacion_Deducciones_Variables()
    {
        Cls_Ope_Nom_Deducciones_Var_Negocio Alta_Asiganacion_Deduccion_Variable = new Cls_Ope_Nom_Deducciones_Var_Negocio();//Variable de conexion con la capa de negocios
        try
        {
            Alta_Asiganacion_Deduccion_Variable.P_Dependencia_ID = String.Empty; ;
            Alta_Asiganacion_Deduccion_Variable.P_Percepcion_Deduccion_ID = Cmb_Deducciones.SelectedValue.Trim();
            Alta_Asiganacion_Deduccion_Variable.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Alta_Asiganacion_Deduccion_Variable.P_Comentarios = Txt_Comentarios.Text;
            Alta_Asiganacion_Deduccion_Variable.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Alta_Asiganacion_Deduccion_Variable.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Alta_Asiganacion_Deduccion_Variable.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Alta_Asiganacion_Deduccion_Variable.P_Referencia = HTxt_Referencia.Value.Trim();

            if (Session["Dt_Empleados"] != null)
            {
                Alta_Asiganacion_Deduccion_Variable.P_Dt_Empleados = Quitar_Caracteres_Cantidad((DataTable)Session["Dt_Empleados"]);
            }
            if (Session["Dt_Empleados"] != null)
            {
                Session.Remove("Dt_Empleados");
            }
            //Alta Asigancion de Percepcion Variable
            if (Alta_Asiganacion_Deduccion_Variable.Alta_Deduccion_Variable())
            {
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
                Limpiar_Controles();//limpia los controles de la pagina.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Asignacion Deduccion Variable]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta Asignacion Percepcion Variable. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Asignacion_Deducciones_Variables
    /// DESCRIPCION : Modificar Asigancion de Deduccion Variable.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Asignacion_Deducciones_Variables()
    {
        Cls_Ope_Nom_Deducciones_Var_Negocio Modificar_Asignacion_Deduccion_Variable = new Cls_Ope_Nom_Deducciones_Var_Negocio();//Variable de conexion con la capa de negocios
        try
        {
            Modificar_Asignacion_Deduccion_Variable.P_No_Deduccion = Txt_No_Deduccion.Text.Trim();
            Modificar_Asignacion_Deduccion_Variable.P_Dependencia_ID = String.Empty;
            Modificar_Asignacion_Deduccion_Variable.P_Percepcion_Deduccion_ID = Cmb_Deducciones.SelectedValue.Trim();
            Modificar_Asignacion_Deduccion_Variable.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Modificar_Asignacion_Deduccion_Variable.P_Comentarios = Txt_Comentarios.Text;
            Modificar_Asignacion_Deduccion_Variable.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Modificar_Asignacion_Deduccion_Variable.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Modificar_Asignacion_Deduccion_Variable.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Modificar_Asignacion_Deduccion_Variable.P_Referencia = HTxt_Referencia.Value.Trim();

            if (Session["Dt_Empleados"] != null)
            {
                Modificar_Asignacion_Deduccion_Variable.P_Dt_Empleados = Quitar_Caracteres_Cantidad((DataTable)Session["Dt_Empleados"]);
            }
            if (Session["Dt_Empleados"] != null)
            {
                Session.Remove("Dt_Empleados");
            }
            //Modificar Asigancion de Percepcion Variable.
            if (Modificar_Asignacion_Deduccion_Variable.Modificar_Deduccion_Empleado())
            {
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
                Limpiar_Controles();//limpia los controles de la pagina.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Modificar Asigancion de Deduccion Variable]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Modificar Asigancion de Percepcion Variable. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Asignacion_Percepciones_Variables
    /// DESCRIPCION : Eliminar Asignacion Deduccion Variable
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Asignacion_Deduccion_Variables()
    {
        Cls_Ope_Nom_Deducciones_Var_Negocio Baja_Asignacion_Deduccion_Variable = new Cls_Ope_Nom_Deducciones_Var_Negocio();//Variable de conexion con la capa de negocios
        try
        {
            Baja_Asignacion_Deduccion_Variable.P_No_Deduccion = Txt_No_Deduccion.Text.Trim();
            //Eliminar Asignacion Percepcion Variable
            if (Baja_Asignacion_Deduccion_Variable.Eliminar_Deduccion_Variable())
            {
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina
                Limpiar_Controles();//limpia los controles de la pagina.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Asignacion Deduccion Variable]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Eliminar Asignacion Percepcion Variable. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Cargar Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias
    /// DESCRIPCION : Consulta las dependencia que existen actualmente. Y carga el 
    /// Combo de Dependencias.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias(DropDownList _DropDownList)
    {
        Cls_Cat_Dependencias_Negocio _Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Dependencias = null;//Variable que almacenara una lista de dependencias.        

        try
        {
            Dt_Dependencias = _Cat_Dependencias.Consulta_Dependencias();//consulta las dependencias.
            _DropDownList.DataSource = Dt_Dependencias;
            _DropDownList.DataTextField = Cat_Dependencias.Campo_Nombre;
            _DropDownList.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            _DropDownList.DataBind();
            _DropDownList.Items.Insert(0, new ListItem("< Seleccione >", ""));
            //Solo podra realizar busquedas de su de la dependencia a la que pertence el usuario logueado.
            if (Cls_Sessiones.Datos_Empleado != null)
            {
                if (Cls_Sessiones.Datos_Empleado.Rows.Count > 0)
                {
                    _DropDownList.SelectedIndex = _DropDownList.Items.IndexOf(_DropDownList.Items.FindByValue(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Deducciones_Variables_Opcionales
    /// DESCRIPCION : Consulta las Deducciones Variables Opcionales en el sistema que existen actualmente.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Deducciones_Variables_Opcionales()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones_Consulta = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios
        DataTable Dt_Deducciones_Variables_Opcionales = null;//Variable que almacenara una lista de las percepciones variables asignadas

        try
        {
            Cat_Percepciones_Deducciones_Consulta.P_Concepto = "TIPO_NOMINA";
            Cat_Percepciones_Deducciones_Consulta.P_TIPO_ASIGNACION = "VARIABLE";
            Cat_Percepciones_Deducciones_Consulta.P_TIPO = "DEDUCCION";

            Dt_Deducciones_Variables_Opcionales = Cat_Percepciones_Deducciones_Consulta.Consulta_Avanzada_Percepciones_Deducciones();
            Dt_Deducciones_Variables_Opcionales = Juntar_Clave_Percepcion_Deduccion(Dt_Deducciones_Variables_Opcionales);
            Cmb_Deducciones.DataSource = Dt_Deducciones_Variables_Opcionales;
            Cmb_Deducciones.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Cmb_Deducciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Deducciones.DataBind();
            Cmb_Deducciones.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Deducciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las deduccciones Variables Opcionales en el sistema. Error: [" + Ex.Message + "]");
        }
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
                            "[" + PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave] + "] -- " +
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
            Botones.Add(Btn_Busqueda_No_Deduccion_Variable);

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

    #region (Grid_Empleados)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Empleados"] != null)
            {
                LLenar_Grid_Empleados(e.NewPageIndex, (DataTable)Session["Dt_Empleados"]);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Empleados
    ///DESCRIPCIÓN: LLena el grid de Empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Empleados(Int32 Pagina, DataTable Tabla)
    {
        Grid_Empleados.Columns[0].Visible = true;
        Grid_Empleados.SelectedIndex = (-1);
        Grid_Empleados.DataSource = Tabla;
        Grid_Empleados.PageIndex = Pagina;
        Grid_Empleados.DataBind();
        Session["Dt_Empleados"] = Tabla;
        Grid_Empleados.Columns[0].Visible = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 28/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[3].FindControl("Btn_Eliminar_Empleado")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[3].FindControl("Btn_Eliminar_Empleado")).ToolTip = "Quitar al Empleado " + HttpUtility.HtmlDecode(e.Row.Cells[1].Text);
            }
            if (e.Row.RowType.Equals(DataControlRowType.Footer))
            {
                e.Row.Cells[1].Text = HttpUtility.HtmlDecode("<p Style='font-family=Courier New; font-size:11px' >Total de Registros Cargados&nbsp;=&nbsp;" + ((GridView)sender).Rows.Count.ToString() + "</p>");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Total
    ///DESCRIPCIÓN: Obtiene el total de la columna de Cantidad de la tabla de Empleados. 00000001458
    /// 
    ///PARAMETROS: No Aplica.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public double Obtener_Total()
    {
        double Total = 0.0;//Almacenara el total de la columna de cantidad.

        try
        {
            foreach (GridViewRow Fila in Grid_Empleados.Rows)
                Total += Convert.ToDouble(String.IsNullOrEmpty(((Label)Fila.Cells[2].FindControl("Lbl_Cantidad")).Text) ? "0" :
                    ((Label)Fila.Cells[2].FindControl("Lbl_Cantidad")).Text.Replace("$", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el total de la columna de CANTIDAD. Error: [" + Ex.Message + "]");
        }
        return Total;
    }
    #endregion

    #region (Grid_Deducciones_Variables)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Deducciones_Variables_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la Pagina del Grid Deducciones Variables
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Deducciones_Variables_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Deducciones_Variables.PageIndex = e.NewPageIndex;
            Consultar_Deducciones_Variables_Asignadas();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar la de pagina del Grid de Percepciones Variables. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Deducciones_Variables_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemento de la tabla de Deducciones Variables
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Deducciones_Variables_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Deducciones_Var_Negocio Cls_Ope_Nom_Deducciones_Var_Consulta = new Cls_Ope_Nom_Deducciones_Var_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        Cls_Ope_Nom_Deducciones_Var_Negocio Cls_Ope_Nom_Deduccion_Variable_Consultada = null;//Variable de tipo clase que almacenara todos los atributos de capa de negocio.
        try
        {
            Cls_Ope_Nom_Deducciones_Var_Consulta.P_No_Deduccion = Grid_Deducciones_Variables.Rows[Grid_Deducciones_Variables.SelectedIndex].Cells[1].Text.Trim();
            Cls_Ope_Nom_Deduccion_Variable_Consultada = Cls_Ope_Nom_Deducciones_Var_Consulta.Consulta_Deducciones_Variables();

            Txt_No_Deduccion.Text = Cls_Ope_Nom_Deduccion_Variable_Consultada.P_No_Deduccion;
            Cmb_Deducciones.SelectedIndex = Cmb_Deducciones.Items.IndexOf(Cmb_Deducciones.Items.FindByValue(Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Percepcion_Deduccion_ID));
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Estatus));
            Txt_Comentarios.Text = Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Comentarios;

            if (!string.IsNullOrEmpty(Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Nomina_ID))
            {
                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Nomina_ID));
                Consultar_Periodos_Catorcenales_Nomina(Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Nomina_ID);
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Cls_Ope_Nom_Deduccion_Variable_Consultada.P_No_Nomina.ToString()));
            }


            LLenar_Grid_Empleados(0, Cls_Ope_Nom_Deduccion_Variable_Consultada.P_Dt_Empleados);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Deducciones_Variables_Asignadas
    ///DESCRIPCIÓN: Consulta las Deducciones Variables Asignadas que existen en la Base de Datos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO:29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Deducciones_Variables_Asignadas()
    {
        Cls_Ope_Nom_Deducciones_Var_Negocio Ope_Nom_Deducciones_Var_Consulta = new Cls_Ope_Nom_Deducciones_Var_Negocio();//Variable de conexion con la capa de negocio.
        try
        {
            Grid_Deducciones_Variables.Columns[5].Visible = true;
            Grid_Deducciones_Variables.Columns[3].Visible = true;

            //Consulta de los Perciones Variables que existen actualmente.
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                Ope_Nom_Deducciones_Var_Consulta.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();//"Pendiente";
            else Ope_Nom_Deducciones_Var_Consulta.P_Estatus = String.Empty;

            Grid_Deducciones_Variables.DataSource = Ope_Nom_Deducciones_Var_Consulta.Consulta_Deducciones_Variables().P_Dt_Ope_Nom_Deducciones_Var;
            Grid_Deducciones_Variables.DataBind();
            Grid_Deducciones_Variables.Columns[3].Visible = false;
            Grid_Deducciones_Variables.Columns[5].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Deducciones Variables Asignadas existentes. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Alta- Baja - Modificar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Asignacion de una Deduccion Variable
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Limpiar_Controles();//limpia los controles de la pagina.
                Habilitar_Controles("Nuevo");//Habilita la configuracion de para ejecutar el alta.              
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Asignacion_Deducciones_Variables())
                {
                    Alta_Asignacion_Deducciones_Variables();//ejecuta el alta.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificacion de un Asignacion de una Deduccion Variable
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 29/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Deducciones_Variables.SelectedIndex != -1 & !Txt_No_Deduccion.Text.Equals(""))
                {
                    if (Cmb_Estatus.SelectedItem.Text.Trim().ToUpper().Equals("ACEPTADO"))
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El registro ya fea aceptado, ya no es posible realizar ninguna modificacion <br>";
                    }
                    else
                    {
                        Habilitar_Controles("Modificar");//Habilita la configuracion de los controles para ejecutar la operacion de modificar.
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Asignacion_Deducciones_Variables())
                {
                    Modificar_Asignacion_Deducciones_Variables();//Ejecuta la modificacion.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar un Percepcion Variable Asignada
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                //Valida que se halla seleccionado la percepcion variable a eliminar.
                if (Grid_Deducciones_Variables.SelectedIndex != -1 & !Txt_No_Deduccion.Text.Equals(""))
                {
                    Eliminar_Asignacion_Deduccion_Variables();//Ejecuta la baja.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("Dt_Empleados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
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
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Deducciones_Click
    /// DESCRIPCION : Ejecuta la Busqueda de Percepciones Variables Asignadas Dados de Alta en el Sistema
    /// por diferentes filtros. [No_Deduccion, Estatus, Dependencia]
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Deducciones_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Deducciones_Var_Negocio Cls_Ope_Nom_Deducciones_Variables_Consulta = new Cls_Ope_Nom_Deducciones_Var_Negocio();//Variable de conexion con la capa de negocio.
        Cls_Ope_Nom_Deducciones_Var_Negocio Cls_Ope_Nom_Deducciones_Variables_Consultadas = null;//Variable que alamcenara todas las propiedades de la clase de Cls_Ope_Nom_Percepciones_Var_Negocio

        try
        {
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Deduccion.Text.Trim())) Txt_Busqueda_No_Deduccion.Text = String.Format("{0:0000000000}", Convert.ToInt64(Txt_Busqueda_No_Deduccion.Text.Trim()));
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Deduccion.Text.Trim())) Cls_Ope_Nom_Deducciones_Variables_Consulta.P_No_Deduccion = Txt_Busqueda_No_Deduccion.Text.Trim();
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0) Cls_Ope_Nom_Deducciones_Variables_Consulta.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();

            Cls_Ope_Nom_Deducciones_Variables_Consultadas = Cls_Ope_Nom_Deducciones_Variables_Consulta.Consulta_Deducciones_Variables();

            Grid_Deducciones_Variables.Columns[3].Visible = true;
            Grid_Deducciones_Variables.Columns[5].Visible = true;
            Grid_Deducciones_Variables.DataSource = Cls_Ope_Nom_Deducciones_Variables_Consultadas.P_Dt_Ope_Nom_Deducciones_Var;
            Grid_Deducciones_Variables.DataBind();
            Grid_Deducciones_Variables.Columns[3].Visible = false;
            Grid_Deducciones_Variables.Columns[5].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error producido al realizar la Busqueda. Error: [" + Ex.Message + "]";
        }
    }
    /// ***********************************************************************************************************
    /// Nombre: Btn_Cargar_Empleados_Click
    /// 
    /// Descripción: Carga los empleados del archivo de conceptos variables.
    /// 
    /// Parámetros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 06/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***********************************************************************************************************
    protected void Btn_Cargar_Empleados_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleados = null;//Variable que almacenara la informacion del empleado.

        try
        {
            if (AFU_Cargar_Archivo_Variables is AsyncFileUpload)
            {
                if (AFU_Cargar_Archivo_Variables.HasFile)
                {
                    String Full_Path = Copiar_Archivo_Servidor(AFU_Cargar_Archivo_Variables);

                    String Ruta = Path.GetPathRoot(AFU_Cargar_Archivo_Variables.PostedFile.FileName);//Obtenemos la ruta.
                    String Nombre = Path.GetFileNameWithoutExtension(AFU_Cargar_Archivo_Variables.PostedFile.FileName);//Obtenemos el nombre del archivo.
                    String Extension = Path.GetExtension(AFU_Cargar_Archivo_Variables.PostedFile.FileName);//Obtenemos la extensión del archivo.

                    if (Extension.Equals(".xlsx") || Extension.Equals(".xls"))
                    {
                        //Dt_Empleados = Leer_Archivo_Canceptos_Variables(Ruta, Nombre, Extension);//Leemos los empleados a lo que le aplicara el concepto variable.
                       // Dt_Empleados = Leer_Archivo_Excel(AFU_Cargar_Archivo_Variables.PostedFile.FileName);
                        HTxt_Referencia.Value = Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Obtener_Referencia(
                            Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Leer_Tabla_Excel(Full_Path, "REFERENCIA"));

                        Dt_Empleados = Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Leer_Tabla_Excel(Full_Path, "DEDUCCIONES_VARIABLES");
                        Dt_Empleados = Presidencia.Ayudante_Excel.Cls_Ayudante_Leer_Excel.Crear_Estructura_Carga_Masiva(Dt_Empleados);
                        Session["Dt_Empleados"] = Dt_Empleados;
                        Grid_Empleados.Columns[0].Visible = true;
                        Grid_Empleados.DataSource = (DataTable)Session["Dt_Empleados"];
                        Grid_Empleados.DataBind();
                        Grid_Empleados.Columns[0].Visible = false;
                    }
                    else {
                        Lbl_Mensaje_Error.Text = "Archivo Invalido";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }

                    Remover_Sesiones_Control_Carga_Archivos(AFU_Cargar_Archivo_Variables.ClientID);
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// ***********************************************************************************************************
    /// Nombre: Btn_Limpiar_Empleados_Click
    /// 
    /// Descripción: Limpia los empleados leidos del archivo de conceptos variables.
    /// 
    /// Parámetros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 06/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// ***********************************************************************************************************
    protected void Btn_Limpiar_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Session["Dt_Empleados"] = new DataTable();
            Grid_Empleados.Columns[0].Visible = true;
            Grid_Empleados.DataSource = (DataTable)Session["Dt_Empleados"];
            Grid_Empleados.DataBind();
            Grid_Empleados.SelectedIndex = -1;
            Grid_Empleados.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al leer los empleados del archivo de conceptos variables. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region(Eventos Agregar y Quitar Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Empleado_Click
    /// DESCRIPCION : Evento que genera la peticion para Quitar el Empleado seleccionado
    /// de la tabla de Empelados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Empleado_Click(object sender, EventArgs e)
    {
        DataRow[] Renglones;//Variable que almacena una lista de DataRows del  Grid_Empleados
        DataRow Renglon;//Variable que almacenara un Renglon del Grid_Empleados
        ImageButton Btn_Eliminar_Empleado = (ImageButton)sender;//Variable que almacenra el control Btn_Eliminar_Empleado

        if (Session["Dt_Empleados"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Empleados"]).Select(Cat_Empleados.Campo_Empleado_ID + "='" + Btn_Eliminar_Empleado.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Empleados"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Empleados"] = Tabla;
                Grid_Empleados.SelectedIndex = (-1);
                LLenar_Grid_Empleados(Grid_Empleados.PageIndex, Tabla);
            }            
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Empleados a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
    }
    #endregion

    #endregion
}







