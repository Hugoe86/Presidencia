using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls.Adapters;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Roles.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Requisitos_Empleados.Negocios;
using AjaxControlToolkit;
using System.IO;
using System.Globalization;
using Presidencia.Programas.Negocios;
using Presidencia.Tipos_Contratos.Negocios;
using Presidencia.Puestos.Negocios;
using Presidencia.Escolaridad.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Turnos.Negocios;
using Presidencia.Zona_Economica.Negocios;
using Presidencia.Tipo_Trabajador.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Utilidades_Nomina;
using Presidencia.Bancos_Nomina.Negocio;
using Presidencia.Catalogo_SAP_Fuente_Financiamiento.Negocio;
using Presidencia.Catalogo_Compras_Proyectos_Programas.Negocio;
using Presidencia.Catalogo_Compras_Partidas.Negocio;
using Presidencia.Indemnizacion.Negocio;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Nomina_Tipos_Pago.Negocio;
using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Cat_Empleados : System.Web.UI.Page
{
    #region (Page Load)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializa_Controles();
                ViewState["SortDirection"] = "ASC";
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            Btn_Eliminar_Mostrar_Popup.Visible = true;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpia_Controles();
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realiza
            Cargar_Fecha();
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
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            ///Datos Generales del Empleado
            Txt_Empleado_ID.Text = "";
            Txt_No_Empleado.Text = "";
            Cmb_Estatus_Empleado.SelectedIndex = 1;
            Txt_Nombre_Empleado.Text = "";
            Txt_Apellido_Paterno_Empleado.Text = "";
            Txt_Apellido_Materno_Empleado.Text = "";
            Txt_Comentarios_Empleado.Text = "";
            Cmb_Tipo_Baja.SelectedIndex = -1;
            Txt_Fecha_Inicio_Licencia.Text = String.Empty;
            Txt_Termino_Licencia.Text = String.Empty;
            Txt_Fecha_Baja_IMSS.Text = String.Empty;
            Chk_Aplica_Licencia.Checked = false;
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
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10-Septiembre-2010
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
                    Btn_Salir.ToolTip = "Salir";
                    Cmb_Estatus_Empleado.Enabled = Habilitado;
                    Btn_Eliminar.Visible = true;
                    Btn_Eliminar_Mostrar_Popup.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                    Img_Foto_Empleado.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
                    Img_Foto_Empleado.DataBind();
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Eliminar.Visible = false;
                    Btn_Eliminar_Mostrar_Popup.Visible = false;
                    Cmb_Estatus_Empleado.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                    Img_Foto_Empleado.ImageUrl = "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG";
                    Img_Foto_Empleado.DataBind();
                    Cmb_Estatus_Empleado.SelectedIndex = 1;
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Salir.ToolTip = "Cancelar";
                    Cmb_Estatus_Empleado.Enabled = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Eliminar_Mostrar_Popup.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    break;
            }
            ///Datos Personales
            Txt_No_Empleado.Enabled = Habilitado;
            Txt_Nombre_Empleado.Enabled = Habilitado;
            Txt_Apellido_Paterno_Empleado.Enabled = Habilitado;
            Txt_Apellido_Materno_Empleado.Enabled = Habilitado;
            Txt_Comentarios_Empleado.Enabled = true;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cmb_Estatus_Empleado.Enabled = false;
            Btn_Eliminar_Mostrar_Popup.Visible = true;
            Cmb_Puestos.Enabled = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    /// ********************************************************************************
    /// Nombre: Crear_Tabla_Mostrar_Errores_Pagina
    /// Descripción: Crea la tabla que almacenara que datos son requeridos 
    /// por el sistema
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private String Crear_Tabla_Mostrar_Errores_Pagina( String Errores ) {
        String Tabla_Inicio = "<table style='width:100%px;font-size:10px;color:red;text-align:left;'>";
        String Tabla_Cierra = "</table>";
        String Fila_Inicia = "<tr>";
        String Fila_Cierra = "</tr>";
        String Celda_Inicia = "<td style='width:25%;text-align:left;vertical-align:top;font-size:10px;' " +
                                "onmouseover=this.style.background='#DFE8F6';this.style.color='#000000'"+
                                " onmouseout=this.style.background='#ffffff';this.style.color='red'>";
        String Celda_Cierra = "</td>";
        char[] Separador = {'+'};
        String[] _Errores_Temp = Errores.Replace("<br>", "").Split(Separador);
        String[] _Errores = new String[(_Errores_Temp.Length - 1)];
        String Tabla;
        String Filas = "";
        String Celdas = "";
        int Contador_Celdas = 1;
        for (int i = 0; i < _Errores.Length; i++) _Errores[i] = _Errores_Temp[i+1];

        Tabla = Tabla_Inicio;
        for (int i = 0; i < _Errores.Length; i++)
        {
            if (Contador_Celdas == 5 )
            {
                Filas += Fila_Inicia;
                Filas += Celdas;
                Filas += Fila_Cierra;
                Celdas = "";
                Contador_Celdas = 0;
                i = i - 1;
            }
            else
            {
                Celdas += Celda_Inicia;
                Celdas += "<b style='font-size:12px;'>+</b>" + _Errores[i];
                Celdas += Celda_Cierra;                
            }
            Contador_Celdas = Contador_Celdas + 1;            
        }
        if (_Errores.Length < 5 || Contador_Celdas > 0) {
            Filas += Fila_Inicia;
            Filas += Celdas;
            Filas += Fila_Cierra;
        }
        Tabla += Filas;
        Tabla += Tabla_Cierra;
        return Tabla;
    }
    /// ******************************************************************************************************
    /// NOMBRE: Cargar_Fecha
    /// DESCRIPCIÓN: Cargara la fecha actual dentro de la caja de texto de Txt_Fecha_Baja
    /// PARÁMETROS: 
    /// USUARIO CREÓ: Hugo Enrique Ramírez Aguilera
    /// FECHA CREÓ:     2/Abril/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************************
    protected void Cargar_Fecha()
    {
        DateTime Dte_Fecha_Actual = DateTime.Now;
        try
        {
            Txt_Fecha_Baja.Text = String.Format("{0:dd/MMM/yyyy}", Dte_Fecha_Actual);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el código programático en los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
    
    #region (Operacion [Baja])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Empleado
    /// DESCRIPCION : Elimina los datos del Empleado que fue seleccionado por el Usuario
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 13-Septiembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Empleado()
    {
        Cls_Cat_Empleados_Negocios Rs_Eliminar_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        Cls_Cat_Empleados_Negocios INF_EMPLEADOS = null;

        try
        {
            INF_EMPLEADOS = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_Empleado_ID.Text);

            Rs_Eliminar_Cat_Empleados.P_Empleado_ID = INF_EMPLEADOS.P_Empleado_ID;
            Rs_Eliminar_Cat_Empleados.P_Dependencia_ID = INF_EMPLEADOS.P_Dependencia_ID;
            Rs_Eliminar_Cat_Empleados.P_Puesto_ID = INF_EMPLEADOS.P_Puesto_ID;
            Rs_Eliminar_Cat_Empleados.P_Tipo_Nomina_ID= INF_EMPLEADOS.P_Tipo_Nomina_ID;
            Rs_Eliminar_Cat_Empleados.P_Tipo_Movimiento = "BAJA";
            Rs_Eliminar_Cat_Empleados.P_Motivo_Movimiento = Txt_Motivo_Baja.Text.Trim();
            Rs_Eliminar_Cat_Empleados.P_Sueldo_Actual = (INF_EMPLEADOS.P_Salario_Diario * (new Cls_Utlidades_Nomina(DateTime.Now).P_Dias_Promedio_Mes_Anio));
            Rs_Eliminar_Cat_Empleados.P_Estatus = "INACTIVO";
            if (!string.IsNullOrEmpty(Txt_Fecha_Baja.Text)) Rs_Eliminar_Cat_Empleados.P_Fecha_Baja = Convert.ToDateTime(Txt_Fecha_Baja.Text.Trim());

            if (!string.IsNullOrEmpty(Txt_Fecha_Inicio_Licencia.Text)) Rs_Eliminar_Cat_Empleados.P_Fecha_Inicio_Licencia = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Licencia.Text.Trim()));
            if (!string.IsNullOrEmpty(Txt_Termino_Licencia.Text)) Rs_Eliminar_Cat_Empleados.P_Fecha_Termino_Licencia = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Termino_Licencia.Text.Trim()));
            if (!string.IsNullOrEmpty(Txt_Fecha_Baja_IMSS.Text)) Rs_Eliminar_Cat_Empleados.P_Fecha_Baja_IMSS = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Baja_IMSS.Text.Trim()));
            Rs_Eliminar_Cat_Empleados.P_Tipo_Baja = Cmb_Tipo_Baja.SelectedItem.Text;

            if (Chk_Aplica_Licencia.Checked)
            {
                Rs_Eliminar_Cat_Empleados.P_Aplica_Baja_Licencia = "SI";
            }
            else
            {
                Rs_Eliminar_Cat_Empleados.P_Aplica_Baja_Licencia = "NO";
            }

            if (Cmb_Puestos.SelectedIndex > 0)
            {
                if (Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' }).Length > 1)
                    Rs_Eliminar_Cat_Empleados.P_Clave = Cmb_Puestos.SelectedItem.Text.Trim().Split(new Char[] { '*' })[0];
            }

            Rs_Eliminar_Cat_Empleados.Eliminar_Empleado(); //Elimina el Empleado que selecciono el usuario de la BD
            Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones

            Mpe_Baja_Empleado.Hide();
            Upd_Panel.Update();
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Empleado " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Combos Pagina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Puestos
    /// DESCRIPCION : Consulta los puestos
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Nov/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Puestos(String Dependencia_ID, String Estatus, String Puesto_ID, String Empleado_ID)
    {
        DataTable Dt_Puestos;
        Cls_Cat_Puestos_Negocio Cat_Nom_Puestos = new Cls_Cat_Puestos_Negocio();
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        String DataTextField = String.Empty;
        String DataValueField = String.Empty;

        try
        {
            if (!String.IsNullOrEmpty(Estatus)) Cat_Nom_Puestos.P_Estatus = Estatus;
            Cat_Nom_Puestos.P_Dependencia_ID = Dependencia_ID;

            Dt_Puestos = Cat_Nom_Puestos.Consultar_Puestos_Disponibles_Dependencia();

            Cmb_Puestos.DataSource = Dt_Puestos;
            Cmb_Puestos.DataValueField = Presidencia.Constantes.Cat_Puestos.Campo_Puesto_ID;
            Cmb_Puestos.DataTextField = "PUESTO";
            Cmb_Puestos.DataBind();
            Cmb_Puestos.Items.Insert(0, new ListItem("<- Seleccione ->", ""));

            if (!String.IsNullOrEmpty(Txt_Empleado_ID.Text.Trim()))
            {
                Cat_Nom_Puestos.P_Empleado_ID = Txt_Empleado_ID.Text.Trim();
                Cat_Nom_Puestos.P_Puesto_ID = Obj_Empleados.Obtener_Puesto_ID(Txt_Empleado_ID.Text.Trim());
            }


            if (!String.IsNullOrEmpty(Cat_Nom_Puestos.P_Puesto_ID) && !String.IsNullOrEmpty(Cat_Nom_Puestos.P_Empleado_ID))
            {
                Dt_Puestos = Cat_Nom_Puestos.Consultar_Puestos();
                if (Dt_Puestos is DataTable)
                {
                    if (Dt_Puestos.Rows.Count > 0)
                    {
                        foreach (DataRow PUESTO in Dt_Puestos.Rows)
                        {
                            if (PUESTO is DataRow)
                            {
                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim()))
                                {
                                    DataTextField = PUESTO[Cat_Puestos.Campo_Nombre].ToString().Trim();
                                }

                                if (!String.IsNullOrEmpty(PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim()))
                                {
                                    DataValueField = PUESTO[Cat_Puestos.Campo_Puesto_ID].ToString().Trim();
                                }

                                Cmb_Puestos.Items.Insert(1, new ListItem(DataTextField, DataValueField));
                            }
                        }
                    }
                }

            }

            Cmb_Puestos.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Roles " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region (Metodos Validacion)
    /// ********************************************************************************
    /// Nombre: Validar_Datos
    /// Descripción: Validar Campos
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "";

        ///-------------------------------  Datos Generales  --------------------------------------------
        if (Txt_No_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El No. de Empleado <br>";
            Datos_Validos = false;
        }
        if (Cmb_Estatus_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Estatus del Empleado <br>";
            Datos_Validos = false;
        }
        if (Txt_Nombre_Empleado.Text == "")
        {
            Lbl_Mensaje_Error.Text += "+ El Nombre del Empleado <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Comentarios_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "+ Comentarios <br>";
            Datos_Validos = false;
        }
        if (Txt_Comentarios_Empleado.Text.Length > 250)
        {
            Lbl_Mensaje_Error.Text += "+ Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
            Datos_Validos = false;
        }

        if (Chk_Aplica_Licencia.Checked)
        {
            if (String.IsNullOrEmpty(Txt_Fecha_Inicio_Licencia.Text) || (Txt_Fecha_Inicio_Licencia.Text.Trim().Equals("__/___/____")))
            {
                Lbl_Mensaje_Error.Text += "+ La fecha de inicio de licencia es un dato obligatorio. <br>";
                Datos_Validos = false;
            }
            else if (!Validar_Formato_Fecha(Txt_Fecha_Inicio_Licencia.Text))
            {
                Lbl_Mensaje_Error.Text += "+ El formato de la fecha de inicio de licencia es incorrecto. <br>";
                Datos_Validos = false;
            }

            if (String.IsNullOrEmpty(Txt_Termino_Licencia.Text) || (Txt_Termino_Licencia.Text.Trim().Equals("__/___/____")))
            {
                Lbl_Mensaje_Error.Text += "+ La fecha de termino de licencia es un dato obligatorio. <br>";
                Datos_Validos = false;
            }
            else if (!Validar_Formato_Fecha(Txt_Termino_Licencia.Text))
            {
                Lbl_Mensaje_Error.Text += "+ El formato de la fecha de termino de licencia es incorrecto. <br>";
                Datos_Validos = false;
            }
        }
        else {
            Txt_Fecha_Inicio_Licencia.Text = String.Empty;
            Txt_Termino_Licencia.Text = String.Empty;
        }

        if (Cmb_Tipo_Baja.SelectedIndex <= 0) {
            Lbl_Mensaje_Error.Text += "+ El tipo de baja es un dato obligatorio. <br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Fecha_Baja.Text) || (Txt_Fecha_Baja.Text.Trim().Equals("__/___/____")))
        {
            Lbl_Mensaje_Error.Text += "+ La fecha de baja es un dato obligatorio. <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Baja.Text))
        {
            Lbl_Mensaje_Error.Text += "+ El formato de la fecha de baja es incorrecto. <br>";
            Datos_Validos = false;
        }

        if (String.IsNullOrEmpty(Txt_Fecha_Baja_IMSS.Text) || (Txt_Fecha_Baja_IMSS.Text.Trim().Equals("__/___/____")))
        {
            Txt_Fecha_Baja_IMSS.Text = String.Empty;
        }
        else if (Validar_Formato_Fecha(Txt_Fecha_Baja_IMSS.Text))
        {
            Lbl_Mensaje_Error.Text += "+ El formato de la fecha de baja IMSS es incorrecto. <br>";
            Datos_Validos = false;
        }

        Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode("Es necesario Introducir: <br>" + Crear_Tabla_Mostrar_Errores_Pagina(Lbl_Mensaje_Error.Text));
        return Datos_Validos;
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
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
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
    #endregion

    #region (Consultas)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_SAP_Unidades_Responsables
    /// 
    /// DESCRIPCION : Consulta los unidades responsables que existen actualmente 
    ///               registrados en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 15/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_SAP_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que lista las unidades responsables registrdas en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Unidades_Responsables;
            Cmb_Busqueda_Dependencia.DataTextField = "CLAVE_NOMBRE";
            Cmb_Busqueda_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (GridView)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Grid_Busqueda_Empleados
    /// 
    /// DESCRIPCION : Consulta y carga el grid de los empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 15/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Grid_Busqueda_Empleados()
    {
        Cls_Cat_Empleados_Negocios Negocio = new Cls_Cat_Empleados_Negocios();

        Grid_Busqueda_Empleados.SelectedIndex = (-1);
        Grid_Busqueda_Empleados.Columns[1].Visible = true;

        if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim(); }
        if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC = Txt_Busqueda_RFC.Text.Trim(); }
        if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
        if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }

        Grid_Busqueda_Empleados.DataSource = Negocio.Consultar_Empleados_Resguardos();
        Grid_Busqueda_Empleados.DataBind();
        Grid_Busqueda_Empleados.Columns[1].Visible = false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Busqueda_Empleados_PageIndexChanging
    /// 
    /// DESCRIPCION : Cambia la pagina del grid del empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 15/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados.PageIndex = e.NewPageIndex;
            Llenar_Grid_Busqueda_Empleados();
            MPE_Empleados.Show();
        }
        catch (Exception Ex)
        {
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Busqueda_Empleados_SelectedIndexChanged
    /// 
    /// DESCRIPCION : Carga la información del registro seleccionado.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 15/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            if (Grid_Busqueda_Empleados.SelectedIndex > (-1))
            {
                INF_EMPLEADO = Cls_Ayudante_Nom_Informacion._Informacion_Empleado(HttpUtility.HtmlDecode(Grid_Busqueda_Empleados.Rows[Grid_Busqueda_Empleados.SelectedIndex].Cells[2].Text));

                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Ruta_Foto))
                {
                    Img_Foto_Empleado.ImageUrl = (@INF_EMPLEADO.P_Ruta_Foto.Equals("")) ? "~/paginas/imagenes/paginas/Sias_No_Disponible.JPG" : @INF_EMPLEADO.P_Ruta_Foto;
                    Txt_Ruta_Foto.Value = INF_EMPLEADO.P_Ruta_Foto;
                }

                Txt_Empleado_ID.Text = INF_EMPLEADO.P_Empleado_ID;
                Txt_No_Empleado.Text = INF_EMPLEADO.P_No_Empleado;
                Txt_Nombre_Empleado.Text = INF_EMPLEADO.P_Nombre;
                Txt_Apellido_Paterno_Empleado.Text = INF_EMPLEADO.P_Apellido_Paterno;
                Txt_Apellido_Materno_Empleado.Text = INF_EMPLEADO.P_Apelldo_Materno;

                if (!string.IsNullOrEmpty(INF_EMPLEADO.P_Dependencia_ID))
                {
                    Consultar_Puestos(INF_EMPLEADO.P_Dependencia_ID,
                        "DISPONIBLE", INF_EMPLEADO.P_Puesto_ID,
                        INF_EMPLEADO.P_Empleado_ID);

                    Cmb_Puestos.SelectedIndex = Cmb_Puestos.Items.IndexOf(Cmb_Puestos.Items.FindByValue(INF_EMPLEADO.P_Puesto_ID));
                }

                MPE_Empleados.Hide();
                Upd_Panel.Update();
            }
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Operacion [Eliminar - Consultar])
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Click
    /// DESCRIPCION : Eliminar al Empleado Seleccionado
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!Cmb_Estatus_Empleado.SelectedItem.Text.Trim().Equals("INACTIVO"))
            {

                if (Validar_Datos())
                {
                    Eliminar_Empleado(); //Elimina el Empleado que fue seleccionado por el usuario
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "El Empleado que desea dar de baja ya se encuentra inactivo <br>";
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
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : Salir o Cancelar la Operacion Actual
    /// PARAMETROS  : 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25-Agosto-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Empleados");
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
    ///DESCRIPCIÓN: Ejecuta la busqueda de empleados.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 08/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados.PageIndex = 0;
            Llenar_Grid_Busqueda_Empleados();
            MPE_Empleados.Show();
        }
        catch (Exception Ex)
        {
        }
    }
    #endregion

    #endregion
}
