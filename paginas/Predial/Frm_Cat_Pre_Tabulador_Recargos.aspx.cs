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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;

public partial class paginas_predial_Frm_Cat_Pre_Tabulador_Recargos : System.Web.UI.Page
{
    private bool _Grid_Recargos_Editable = false;
    protected bool Grid_Recargos_Editable
    {
        get { return this._Grid_Recargos_Editable; }
        set { this._Grid_Recargos_Editable = value; }
    }


    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Cat_Pre_Tabulador_Recargos.aspx");
                Habilitar_Controles("Inicial");
                Llenar_Tabla_Recargos(0);
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

    #region Metodos

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN:  Habilita o Deshabilita los controles de la forma para según se 
    ///             requiera para la siguiente operación
    /// PARÁMETROS:
    /// 		1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial, nuevo, modificar)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Cmb_Anio_Tabulador.Enabled = true;
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Cmb_Anio_Tabulador.Visible = true;
                    Lbl_Anio_Tabulador_Combo.Visible = true;
                    Tbl_Campos_Nuevo_Recargo.Visible = false;
                    // establecer propiedades de los controles plantilla en el grid
                    Grid_Recargos_Editable = false;
                    Llenar_Tabla_Recargos(0);
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Cmb_Anio_Tabulador.Visible = false;
                    Lbl_Anio_Tabulador_Combo.Visible = false;
                    Tbl_Campos_Nuevo_Recargo.Visible = true;
                    // establecer propiedades de los controles plantilla en el grid
                    Grid_Recargos_Editable = false;
                    Llenar_Tabla_Recargos(0);
                    Lbl_Anio.Visible = true;
                    Txt_Anio.Visible = true;
                    Lbl_Bimestre.Visible = true;
                    Txt_Bimestre.Visible = true;
                    Txt_Anio_Tabulador.Enabled = true;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Cmb_Anio_Tabulador.Visible = false;
                    Lbl_Anio_Tabulador_Combo.Visible = true;
                    Tbl_Campos_Nuevo_Recargo.Visible = false;
                    // establecer propiedades de los controles plantilla en el grid
                    Grid_Recargos_Editable = true;
                    Llenar_Tabla_Recargos(0);
                    Lbl_Anio.Visible = false;
                    Txt_Anio.Visible = false;
                    Lbl_Bimestre.Visible = false;
                    Txt_Bimestre.Visible = false;
                    Txt_Anio_Tabulador.Enabled = false;
                    if (Cmb_Anio_Tabulador.SelectedIndex > 0)
                    {
                        Txt_Anio_Tabulador.Text = Cmb_Anio_Tabulador.SelectedValue;
                    }
                    break;
            }


            Txt_Anio.Enabled = Habilitado;
            Txt_Enero.Enabled = Habilitado;
            Txt_Febrero.Enabled = Habilitado;
            Txt_Marzo.Enabled = Habilitado;
            Txt_Abril.Enabled = Habilitado;
            Txt_Mayo.Enabled = Habilitado;
            Txt_Junio.Enabled = Habilitado;
            Txt_Julio.Enabled = Habilitado;
            Txt_Agosto.Enabled = Habilitado;
            Txt_Septiembre.Enabled = Habilitado;
            Txt_Noviembre.Enabled = Habilitado;
            Txt_Diciembre.Enabled = Habilitado;
            Txt_Octubre.Enabled = Habilitado;
            Txt_Bimestre.Enabled = Habilitado;
            Grid_Recargos.Enabled = true;
            Grid_Recargos.SelectedIndex = (-1);
            Btn_Buscar_Recargos.Enabled = !Habilitado;
            Txt_Busqueda_Recargos.Enabled = !Habilitado;

            Txt_Anio_Tabulador.Visible = Habilitado;
            Cmb_Anio_Tabulador.Visible = !Habilitado;
            Tbl_Anio_Tabulador_Combo.Visible = !Habilitado;
            Lbl_Anio_Tabulador_Combo.Visible = !Habilitado;
            Lbl_Anio_Tabulador_Texto.Visible = Habilitado;
            Tbl_Nuevo_Tabulador_Anio_bimestre.Visible = Habilitado;

        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Enero.Text = "";
        Txt_Febrero.Text = "";
        Txt_Marzo.Text = "";
        Txt_Abril.Text = "";
        Txt_Mayo.Text = "";
        Txt_Junio.Text = "";
        Txt_Julio.Text = "";
        Txt_Agosto.Text = "";
        Txt_Septiembre.Text = "";
        Txt_Noviembre.Text = "";
        Txt_Diciembre.Text = "";
        Txt_Octubre.Text = "";
        Txt_Anio.Text = "";
        Txt_Bimestre.Text = "";
        Txt_Anio_Tabulador.Text = "";
        Llenar_Tabla_Recargos(0);
        //Grid_Recargos.DataSource = new DataTable();
        //Grid_Recargos.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Anios_Tabulador
    ///DESCRIPCIÓN: Metodo que llena el Combo de Años Tabulador con los años existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 13/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Anios_Tabulador()
    {
        try
        {
            Cls_Cat_Pre_Tabulador_Recargos_Negocio Anio = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
            DataTable Dt_Anios = Anio.Consultar_Anios();

            string Valor_Seleccionado = Cmb_Anio_Tabulador.SelectedValue;
            Cmb_Anio_Tabulador.DataTextField = Cat_Pre_Recargos.Campo_Anio_Tabulador;
            Cmb_Anio_Tabulador.DataValueField = Cat_Pre_Recargos.Campo_Anio_Tabulador;
            Cmb_Anio_Tabulador.DataSource = Dt_Anios;
            Cmb_Anio_Tabulador.DataBind();
            Cmb_Anio_Tabulador.Items.Insert(0, new ListItem("<SELECCIONE>", "SELECCIONE"));
            // si el combo contiene el valor seleccioando anterior, volver a seleccionar
            if (Cmb_Anio_Tabulador.Items.Contains(new ListItem(Valor_Seleccionado, Valor_Seleccionado)))
            {
                Cmb_Anio_Tabulador.SelectedValue = Valor_Seleccionado;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        String Mensaje_Error = "";
        Boolean Validacion = true;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        int Valor_Entero;
        decimal Valor_Decimal;

        if (!int.TryParse(Txt_Anio.Text.Trim(), out Valor_Entero))
        {
            Mensaje_Error += "+ Introducir el Año.<br />";
            Validacion = false;
        }
        if (!int.TryParse(Txt_Bimestre.Text.Trim(), out Valor_Entero))
        {
            Mensaje_Error += "+ Introducir el Bimestre.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Enero.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Enero.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Febrero.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Febrero.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Marzo.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Marzo.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Abril.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Abril.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Mayo.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Mayo.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Junio.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Junio.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Julio.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Julio.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Agosto.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Agosto.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Septiembre.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Septiembre.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Octubre.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Octubre.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Noviembre.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Noviembre.<br />";
            Validacion = false;
        }
        if (!decimal.TryParse(Txt_Diciembre.Text.Trim(), out Valor_Decimal))
        {
            Mensaje_Error += "+ Introducir el Recargo del mes de Diciembre.<br />";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario:";
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Existe_Registro
    /// DESCRIPCIÓN: Valida mediante una consulta a la base de datos que no exista ya un 
    ///             registro para el año, bimestre y año tabulador ingresados
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Validar_Existe_Registro()
    {
        String Mensaje_Error = "";
        Boolean Validacion = true;
        int Anio;
        int Anio_Tabulador;
        int Bimestre;
        DataTable Dt_Recargos;
        var Consulta_Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();

        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        int.TryParse(Txt_Anio_Tabulador.Text.Trim(), out Anio_Tabulador);
        int.TryParse(Txt_Anio.Text.Trim(), out Anio);
        int.TryParse(Txt_Bimestre.Text.Trim(), out Bimestre);
        if (Anio > 0 && Anio_Tabulador > 0 && Bimestre > 0)
        {
            Consulta_Recargos.P_Anio = Anio.ToString();
            Consulta_Recargos.P_Anio_Tabulador = Anio_Tabulador.ToString();
            Consulta_Recargos.P_Bimestre = Bimestre.ToString();
            Dt_Recargos = Consulta_Recargos.Consultar_Recargos();
            if (Dt_Recargos != null && Dt_Recargos.Rows.Count > 0)
            {
                Mensaje_Error += "+ Ya existe el año y bimestre para el tabulador ingresado.<br />";
                Validacion = false;
            }
        }
        else // generar mensaje de error para valor menor o igual a cero
        {
            if (Anio > 0)
            {
                Mensaje_Error += "+ Introducir un número de año válido.<br />";
            }
            if (Anio_Tabulador > 0)
            {
                Mensaje_Error += "+ Introducir un número de Año tabulador válido.<br />";
            }
            if (Bimestre > 0)
            {
                Mensaje_Error += "+ Introducir un número de Bimestre válido.<br />";
            }
            Validacion = false;
        }

        if (!Validacion)
        {
            Lbl_Ecabezado_Mensaje.Text = "Es necesario:";
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_DataTable_Recargos
    /// DESCRIPCIÓN: Forma un datatable con el tabulador de recargos en el grid y valida que no haya
    ///             registros repetidos de año y bimestre
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Obtener_DataTable_Recargos()
    {
        String Mensaje_Error = "";
        Boolean Validacion = true;
        List<string> Periodo = new List<string>();
        DataTable Dt_Tabulador;
        int Valor_Entero;
        decimal Valor_Decimal;

        Dt_Tabulador = Formar_Tabla_Tabulador_Recargos();
        // recorrer el grid
        foreach (GridViewRow Recargo in Grid_Recargos.Rows)
        {
            DataRow Fila_Recargo;

            TextBox Txt_Anio_Grid = (TextBox)Recargo.Cells[1].FindControl("Txt_Grid_Anio");
            TextBox Txt_Bimestre_Grid = (TextBox)Recargo.Cells[2].FindControl("Txt_Grid_Bimestre");
            TextBox Txt_Enero_Grid = (TextBox)Recargo.Cells[3].FindControl("Txt_Grid_Enero");
            TextBox Txt_Febrero_Grid = (TextBox)Recargo.Cells[4].FindControl("Txt_Grid_Febrero");
            TextBox Txt_Marzo_Grid = (TextBox)Recargo.Cells[5].FindControl("Txt_Grid_Marzo");
            TextBox Txt_Abril_Grid = (TextBox)Recargo.Cells[6].FindControl("Txt_Grid_Abril");
            TextBox Txt_Mayo_Grid = (TextBox)Recargo.Cells[7].FindControl("Txt_Grid_Mayo");
            TextBox Txt_Junio_Grid = (TextBox)Recargo.Cells[8].FindControl("Txt_Grid_Junio");
            TextBox Txt_Julio_Grid = (TextBox)Recargo.Cells[9].FindControl("Txt_Grid_Julio");
            TextBox Txt_Agosto_Grid = (TextBox)Recargo.Cells[10].FindControl("Txt_Grid_Agosto");
            TextBox Txt_Septiembre_Grid = (TextBox)Recargo.Cells[11].FindControl("Txt_Grid_Septiembre");
            TextBox Txt_Octubre_Grid = (TextBox)Recargo.Cells[12].FindControl("Txt_Grid_Octubre");
            TextBox Txt_Noviembre_Grid = (TextBox)Recargo.Cells[13].FindControl("Txt_Grid_Noviembre");
            TextBox Txt_Diciembre_Grid = (TextBox)Recargo.Cells[14].FindControl("Txt_Grid_Diciembre");

            // recuperar valores de las cajas de texto y agregar a una nueva fila
            Fila_Recargo = Dt_Tabulador.NewRow();
            Fila_Recargo[Cat_Pre_Recargos.Campo_Recargo_ID] = Recargo.Cells[0].Text;

            if (int.TryParse(Txt_Anio_Grid.Text, out Valor_Entero) && Valor_Entero > 0)
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Anio] = Valor_Entero;
            }
            else
            {
                // no repetir el mismo mensaje varias veces
                if (!Mensaje_Error.Contains("válido para el año"))
                {
                    Mensaje_Error += "+ Introducir un valor válido para el año.<br />";
                }
                Validacion = false;
            }
            string Str_Anio = Fila_Recargo[Cat_Pre_Recargos.Campo_Anio].ToString();
            if (int.TryParse(Txt_Bimestre_Grid.Text, out Valor_Entero) && Valor_Entero > 0 && Valor_Entero < 7)
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Bimestre] = Valor_Entero;
            }
            else
            {
                // no repetir el mismo mensaje varias veces
                if (!Mensaje_Error.Contains("válido para el bimestre en " + Str_Anio))
                {
                    Mensaje_Error += "+ Introducir un valor válido para el bimestre en " + Str_Anio + ".<br />";
                }
                Validacion = false;
            }

            string Str_Bimestre = Fila_Recargo[Cat_Pre_Recargos.Campo_Bimestre].ToString();
            // validar que no haya duplicados mediante una lista del año y bimestre
            if (Str_Bimestre.Trim() != "" && Str_Anio.Trim() != "" )
            {
                if (!Periodo.Contains((Str_Anio + Str_Bimestre)))
                {
                    Periodo.Add((Str_Anio + Str_Bimestre));
                }
                else // si el valor ya esta en la lista, mostrar mensaje
                {
                    // no repetir el mismo mensaje varias veces
                    if (!Mensaje_Error.Contains("duplicado " + Str_Bimestre + "/" + Str_Anio))
                    {
                        Mensaje_Error += "+ Bimestre duplicado " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                    }
                    Validacion = false;
                }
            }

            // agregar valores de los meses o mostrar mensaje de error si el valor es incorrecto
            if (decimal.TryParse(Txt_Enero_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Enero] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Enero " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Febrero_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Febrero] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Febrero " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Marzo_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Marzo] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Marzo " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Abril_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Abril] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Abril " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Mayo_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Mayo] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Mayo " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Junio_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Junio] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Junio " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Julio_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Julio] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Julio " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Agosto_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Agosto] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Agosto " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Septiembre_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Septiembre] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Septiembre " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Octubre_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Octubre] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Octubre " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Noviembre_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Noviembre] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Noviembre " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            if (decimal.TryParse(Txt_Diciembre_Grid.Text, out Valor_Decimal))
            {
                Fila_Recargo[Cat_Pre_Recargos.Campo_Diciembre] = Valor_Decimal;
            }
            else
            {
                Mensaje_Error += "+ Introducir un valor válido para el recargo de Diciembre " + Str_Bimestre + "/" + Str_Anio + ".<br />";
                Validacion = false;
            }

            Dt_Tabulador.Rows.Add(Fila_Recargo);
        }

        // validar que haya registros
        if (Dt_Tabulador.Rows.Count <= 0)
        {
            Mensaje_Error += "+ No hay registros para guardar.<br />";
            Validacion = false;
        }

        if (!Validacion)
        {
            Lbl_Ecabezado_Mensaje.Text = "es necesario corregir:";
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Dt_Tabulador;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Formar_Tabla_Tabulador_Recargos
    /// DESCRIPCIÓN: Crear tabla con columnas para almacenar el tabulador de recargos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Formar_Tabla_Tabulador_Recargos()
    {
        // tabla y columnas
        DataTable Dt_Recargos = new DataTable();

        // agregar columnas a la tabla
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Recargo_ID, System.Type.GetType("System.String"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Anio, System.Type.GetType("System.Int32"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Bimestre, System.Type.GetType("System.Int32"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Enero, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Febrero, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Marzo, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Abril, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Mayo, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Junio, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Julio, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Agosto, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Septiembre, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Octubre, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Noviembre, System.Type.GetType("System.Decimal"));
        Dt_Recargos.Columns.Add(Cat_Pre_Recargos.Campo_Diciembre, System.Type.GetType("System.Decimal"));
        // regresar tabla adeudos 
        return Dt_Recargos;
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recargos
    ///DESCRIPCIÓN: Llena la tabla de Recargos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Recargos(int Pagina)
    {
        var Recargo = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
        DataTable DT_Recargos = null;
        try
        {

            Llenar_Combo_Anios_Tabulador();

            // si hay un valor seleccionado en el combo año tabulador filtrar
            if (Cmb_Anio_Tabulador.SelectedIndex > 0)
            {
                Recargo.P_Anio_Tabulador = Cmb_Anio_Tabulador.SelectedValue;
                DT_Recargos = Recargo.Consultar_Recargos();
            }
            else // si no hay seleccion, intentar año actual
            {
                ListItem Anio_Actual = new ListItem(DateTime.Now.Year.ToString(), DateTime.Now.Year.ToString());
                if (Cmb_Anio_Tabulador.Items.Contains(Anio_Actual))
                {
                    Cmb_Anio_Tabulador.SelectedValue = DateTime.Now.Year.ToString();
                    Recargo.P_Anio_Tabulador = Cmb_Anio_Tabulador.SelectedValue;
                    DT_Recargos = Recargo.Consultar_Recargos();
                }
            }

            Session["Recargos"] = DT_Recargos;

            Grid_Recargos.Columns[0].Visible = true;
            Grid_Recargos.DataSource = DT_Recargos;
            Grid_Recargos.DataBind();
            Grid_Recargos.Columns[0].Visible = false;

            // ocultar columna con boton eliminar si se est[a editando
            if (Btn_Nuevo.AlternateText== "Dar de Alta"  || Btn_Modificar.AlternateText=="Actualizar")
            {
                Grid_Recargos.Columns[15].Visible = false;
            }
            else
            {
                Grid_Recargos.Columns[15].Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Recargos_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Recargos de acuerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Recargos_Busqueda(int Pagina)
    {
        try
        {
            if (Txt_Busqueda_Recargos.Text.Length > 0)
            {
                Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
                Recargos.P_Anio = Txt_Busqueda_Recargos.Text.ToUpper().Trim();
                Grid_Recargos.DataSource = Recargos.Consultar_Anio();
                Grid_Recargos.Columns[0].Visible = true;
                Session["Recargos"] = (DataTable)Grid_Recargos.DataSource;
                DataTable DT_Recargos = (DataTable)Session["Recargos"];
                Grid_Recargos.DataSource = DT_Recargos;
                Grid_Recargos.DataBind();
                Grid_Recargos.Columns[0].Visible = false;
            }
        }

        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #endregion


    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Recargo
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Habilitar_Controles("Nuevo");
                Limpiar_Catalogo();
                // inicializar valores para nuevo registro
                Txt_Anio_Tabulador.Text = Cmb_Anio_Tabulador.SelectedIndex > 0 ? Cmb_Anio_Tabulador.SelectedValue : DateTime.Now.Year.ToString();
                Txt_Enero.Text = "0";
                Txt_Febrero.Text = "0";
                Txt_Marzo.Text = "0";
                Txt_Abril.Text = "0";
                Txt_Mayo.Text = "0";
                Txt_Junio.Text = "0";
                Txt_Julio.Text = "0";
                Txt_Agosto.Text = "0";
                Txt_Septiembre.Text = "0";
                Txt_Octubre.Text = "0";
                Txt_Noviembre.Text = "0";
                Txt_Diciembre.Text = "0";
            }
            else
            {
                if (Validar_Componentes_Generales() && Validar_Existe_Registro())
                {

                    Cls_Cat_Pre_Tabulador_Recargos_Negocio Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
                    Recargos.P_Anio_Tabulador = Txt_Anio_Tabulador.Text.Trim();
                    Recargos.P_Anio = Txt_Anio.Text.Trim();
                    Recargos.P_Enero = Txt_Enero.Text.Trim();
                    Recargos.P_Febrero = Txt_Febrero.Text.Trim();
                    Recargos.P_Marzo = Txt_Marzo.Text.Trim();
                    Recargos.P_Abril = Txt_Abril.Text.Trim();
                    Recargos.P_Mayo = Txt_Mayo.Text.Trim();
                    Recargos.P_Junio = Txt_Junio.Text.Trim();
                    Recargos.P_Julio = Txt_Julio.Text.Trim();
                    Recargos.P_Agosto = Txt_Agosto.Text.Trim();
                    Recargos.P_Septiembre = Txt_Septiembre.Text.Trim();
                    Recargos.P_Octubre = Txt_Octubre.Text.Trim();
                    Recargos.P_Noviembre = Txt_Noviembre.Text.Trim();
                    Recargos.P_Diciembre = Txt_Diciembre.Text.Trim();
                    Recargos.P_Bimestre = Txt_Bimestre.Text.Trim();
                    Recargos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Recargos.Alta_Recargo();

                    Limpiar_Catalogo();
                    Habilitar_Controles("Inicial");
                    Llenar_Tabla_Recargos(Grid_Recargos.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tabulador de Recargos", "alert('Alta de Recargo Exitosa');", true);
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Recargo
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {

                if (Grid_Recargos.Rows.Count > 0 && Cmb_Anio_Tabulador.SelectedIndex != 0)
                {
                    Habilitar_Controles("Modificar");
                }
                else
                {

                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar un elemento del Combo Año tabulador";
                    Lbl_Ecabezado_Mensaje.Visible = true;
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                var Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
                DataTable Dt_Tabulador = Obtener_DataTable_Recargos();
                // si hay mensajes de error salir
                if (Lbl_Mensaje_Error.Text != "" || Lbl_Ecabezado_Mensaje.Text != "")
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    return;
                }

                Recargos.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Recargos.DT_Recargos = Dt_Tabulador;
                Recargos.Modificar_Recargo();
                Habilitar_Controles("Inicial");
                Llenar_Tabla_Recargos(0);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tabulador de Recargos", "alert('Modificación de Recargo Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            // llamar metodo que realiza la búsqueda, sólo si se especifica criterio de busqueda
            if (Txt_Busqueda_Recargos.Text.Trim().Length > 0)
            {
                Limpiar_Catalogo();
                // llamar método de búsqueda
                Llenar_Tabla_Recargos_Busqueda(0);
                // si la búsqueda no regresa valores, mostrar mensaje
                if (Grid_Recargos.Rows.Count == 0)
                {
                    Lbl_Ecabezado_Mensaje.Text = "Para la Búsqueda con el Concepto: \"" + Txt_Busqueda_Recargos.Text + "\" no se encotraron coincidencias";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                    Llenar_Tabla_Recargos(0);
                }
                else // si la búsqueda regresa valores, seleccionar elemento en combo año tabulador
                {
                    ListItem Criterio_Busqueda = new ListItem(Txt_Busqueda_Recargos.Text.Trim(), Txt_Busqueda_Recargos.Text.Trim());
                    if (Cmb_Anio_Tabulador.Items.Contains(Criterio_Busqueda))
                    {
                        Cmb_Anio_Tabulador.SelectedValue = Criterio_Busqueda.Value;
                    }
                    else
                    {
                        Cmb_Anio_Tabulador.SelectedIndex = -1;
                    }
                }
                Txt_Busqueda_Recargos.Text = "";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;

        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Habilitar_Controles("Inicial");
                Limpiar_Catalogo();
                Llenar_Tabla_Recargos(0);
                Session["Recargos"] = null;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_Tabulador_SelectedIndexChanged
    ///DESCRIPCIÓN: Mostrar el tabulador de recagos del año seleccionado
    ///PROPIEDADES:     
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 07/oct/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Anio_Tabulador_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Llenar_Tabla_Recargos(0);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region TextChanged

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Borrar_Recargo
    ///DESCRIPCIÓN: Elimina el registro en la base de datos
    ///PROPIEDADES:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 01-mar-2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Borrar_Recargo(object sender, GridViewCommandEventArgs e)
    {
        int Afectaciones = 0;
        if (e.CommandName.Equals("Erase"))
        {
            if (Grid_Recargos.Rows.Count > 0)
            {
                var Eliminar_Recargo = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
                Eliminar_Recargo.P_Recargo_ID = Grid_Recargos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[0].Text;
                Afectaciones = Eliminar_Recargo.Eliminar_Recargo();

                if (Afectaciones > 0)
                {
                    Llenar_Tabla_Recargos(0);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tabulador de Recargos", "alert('Modificación de Recargo Exitosa');", true);
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo Tabulador de Recargos", "alert('Ocurrió un error y el recargo no pudo ser eliminado.');", true);
                }
            }
        }
    }

    #endregion

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

            Botones.Add(Btn_Buscar_Recargos);

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

}