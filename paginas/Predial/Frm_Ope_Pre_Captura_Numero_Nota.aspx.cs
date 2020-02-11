using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Bitacora_Eventos;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Claves_Grupos_Movimiento.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
//using System.Threading;

public partial class paginas_Predial_Frm_Ope_Pre_Captura_Numero_Nota : System.Web.UI.Page
{

    #region Variables y Propieades
    private enum Tipos_Eventos
    {
        Default, Nuevo, Modificar, Eliminar, Cancelar, Salir
    }

    private enum Busqueda_Por
    {
        Orden_Variacion,
        Cuenta_Predial
    }

    DataTable P_Dt_Ordenes_Variacion
    {
        get
        {
            DataTable Dt_Ordenes_Variacion = null;
            if (Session["Dt_Ordenes_Variacion"] != null)
            {
                Dt_Ordenes_Variacion = (DataTable)Session["Dt_Ordenes_Variacion"];
            }
            return Dt_Ordenes_Variacion;
        }
        set
        {
            Session["Dt_Ordenes_Variacion"] = null;
            if (value != null)
            {
                Session["Dt_Ordenes_Variacion"] = value.Copy();
            }
        }
    }
    #endregion

    #region Load/Init
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Evento de Carga de la Página
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            }
            if (!IsPostBack)
            {
                String Ventana_Modal;
                String Propiedades;

                Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','PENDIENTE')";

                Llenar_Combo_Grupos_Movimientos();

                Configurar_Formulario(Tipos_Eventos.Default);
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de cuentas predial
                Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:on');";
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx";
                Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "'" + Propiedades);
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Métodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configurar_Formulario
    ///DESCRIPCIÓN          : Establece la vista de los controles del Formulario
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configurar_Formulario(Tipos_Eventos Evento)
    {
        //Btn_Nuevo.Visible = false;
        Btn_Modificar.Visible = false;
        //Btn_Eliminar.Visible = false;
        Btn_Salir.Visible = false;
        Btn_Buscar.Enabled = false;
        Txt_Busqueda.Enabled = false;
        switch (Evento)
        {
            //case Tipos_Eventos.Nuevo:
            //    Btn_Nuevo.Visible = true;
            //    Btn_Nuevo.AlternateText = "Dar de Alta";
            //    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
            //    Btn_Salir.Visible = true;
            //    Btn_Salir.AlternateText = "Cancelar";
            //    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            //    Configurar_Controles(true);
            //    break;
            case Tipos_Eventos.Modificar:
                Btn_Modificar.Visible = true;
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                Btn_Salir.Visible = true;
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Configurar_Controles(true);
                break;
            default:
                //Btn_Nuevo.Visible = true;
                //Btn_Nuevo.AlternateText = "Nuevo";
                //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.Visible = true;
                if (Txt_Cuenta_Predial.Text.Trim() != "")
                {
                    Btn_Modificar.Enabled = true;
                }
                else
                {
                    Btn_Modificar.Enabled = false;
                }
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Btn_Eliminar.Visible = true;
                //Btn_Eliminar.AlternateText = "Eliminar";
                //Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                Btn_Salir.Visible = true;
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Buscar.Enabled = true;
                Txt_Busqueda.Enabled = true;
                Configurar_Controles(false);
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Datos_Obligatorios
    ///DESCRIPCIÓN          : Valida que se hallan introducidon datos en los campos obligatorios
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Datos_Obligatorios()
    {
        Lbl_Encabezado_Error.Text = "";
        Lbl_Mensaje_Error.Text = "";
        String Mensaje_Error = "";
        Boolean Validacion = true;

        if (Cmb_Grupos_Movimientos.SelectedItem.Text == "SELECCIONE")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione un Grupo de Movimiento.";
            Validacion = false;
        }
        if (Txt_No_Nota.Text.Trim() == "")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca un Número de Nota.";
            Validacion = false;
        }
        else
        {
            if (Txt_No_Nota.Text.Trim() == "0")
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introduzca un Número de Nota diferente de Cero.";
                Validacion = false;
            }
            else
            {
                //if (Lbl_Ultimo_No_Nota_Grupo.Text.Trim() != "")
                //{
                //    if (Convert.ToInt16(Txt_No_Nota.Text.Trim()) <= Convert.ToInt16(Lbl_Ultimo_No_Nota_Grupo.Text))
                //    {
                //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                //        Mensaje_Error = Mensaje_Error + "+ El Número de Nota NO debe ser menor a igual al Actual registrado.";
                //        Validacion = false;
                //    }
                //}
                String No_Nota_Asignado = Obtener_Dato_Consulta("NVL(" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", '')", Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion, Ope_Pre_Ordenes_Variacion.Campo_Anio + " || " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " || " + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " || " + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + " = '" + Lbl_Año_Movimiento.Text + Cmb_Grupos_Movimientos.SelectedItem.Value + Hdn_Tipo_Predio_ID.Value + Txt_No_Nota.Text.Trim() + "' AND " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " <> '" + Txt_No_Movimiento.Text.Trim() + "'", "").Trim();
                if (No_Nota_Asignado == Txt_No_Nota.Text.Trim())
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ El un Número de Nota que indicó ya se encuentra asignado.";
                    Validacion = false;
                }
            }
        }

        if (!Validacion)
        {
            Lbl_Encabezado_Error.Text = "Es necesario.";
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configurar_Controles
    ///DESCRIPCIÓN          : Habilita/Deshbailita los controles del Formulario
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configurar_Controles(Boolean Estatus)
    {
        Txt_Cuenta_Predial.Enabled = false;
        Txt_No_Movimiento.Enabled = false;
        Txt_Tipo_Movimiento.Enabled = false;
        Cmb_Grupos_Movimientos.Enabled = Estatus;
        Txt_Tipo_Predio.Enabled = false;
        Txt_No_Nota.Enabled = Estatus;
        Grid_Ordenes_Variacion.Enabled = !Estatus;

        Txt_Busqueda.Enabled = !Estatus;
        Btn_Buscar.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Datos_Controles
    ///DESCRIPCIÓN          : Limpia los controles del contenido que tengan
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Datos_Controles()
    {
        Txt_Cuenta_Predial.Text = "";
        Lbl_Año_Movimiento.Text = "";
        Txt_No_Movimiento.Text = "";
        Txt_Tipo_Movimiento.Text = "";
        Cmb_Grupos_Movimientos.SelectedValue = "SELECCIONE";
        Txt_Tipo_Predio.Text = "";
        Txt_No_Nota.Text = "";
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION : Cargar_Grid_Ordenes_Variacion
    ///DESCRIPCION          : Método que invoca al método Cargar_Grid_Ordenes_Variacion con los parámetros de Pagina y Tipo_Busqueda
    ///PARAMETROS           : Pagina, valor que indica en qué página de Grid se mostrará la información
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Cargar_Grid_Ordenes_Variacion(Int32 Pagina)
    {
        Cargar_Grid_Ordenes_Variacion(Pagina, Busqueda_Por.Orden_Variacion);
    }

    private void Cargar_Grid_Ordenes_Variacion(Int32 Pagina, Busqueda_Por Tipo_Busqueda)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

        try
        {
            Orden_Variacion.P_Campos_Dinamicos = "";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Anio + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + ", ";
            Orden_Variacion.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ") AS MOVIMIENTO, ";
            Orden_Variacion.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Grupos_Movimiento.Campo_Clave + " || ' - ' ||" + Cat_Pre_Grupos_Movimiento.Campo_Nombre + " FROM " + Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + " WHERE " + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + ") AS GRUPO_MOVIMIENTO, ";
            Orden_Variacion.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Tipos_Predio.Campo_Descripcion + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " WHERE " + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + ") AS TIPO_PREDIO, ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_No_Nota + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Creo + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Fecha_Valido + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Usuario_Valido + ", ";
            Orden_Variacion.P_Campos_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + ", ";
            if (Orden_Variacion.P_Campos_Dinamicos.EndsWith(", "))
            {
                Orden_Variacion.P_Campos_Dinamicos = Orden_Variacion.P_Campos_Dinamicos.Substring(0, Orden_Variacion.P_Campos_Dinamicos.Length - 2);
            }
            //if (Cmb_Busqueda_Estatus_Ordenes.SelectedValue == "POR VALIDAR")
            //{
            //    Orden_Variacion.P_Ordenar_Dinamico = Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + ", (SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ")";
            //}
            //else
            //{
            Orden_Variacion.P_Fecha_Creo = "NOT NULL";
            Orden_Variacion.P_Ordenar_Dinamico = Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC, " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC, (SELECT " + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ")";
            //}
            //if (Txt_Busqueda.Text.Trim() != ""
            //    && Tipo_Busqueda == Busqueda_Por.Orden_Variacion)
            //{
            //    Orden_Variacion.P_Cuenta_Predial = "LIKE UPPER('%" + Txt_Busqueda.Text + "%')";
            //    Orden_Variacion.P_Orden_Variacion_ID = "LIKE '%" + Txt_Busqueda.Text + "%'";
            //}
            //else
            //{
            //    if (Hdn_Cuenta_Predial.Value.Trim() != ""
            //        && Tipo_Busqueda == Busqueda_Por.Cuenta_Predial)
            //    {
            //        Orden_Variacion.P_Cuenta_Predial = "LIKE UPPER('%" + Hdn_Cuenta_Predial.Value + "%')";
            //    }
            //}
            if (Hdn_Cuenta_Predial.Value != ""
                && Tipo_Busqueda == Busqueda_Por.Cuenta_Predial)
            {
                Orden_Variacion.P_Cuenta_Predial = Hdn_Cuenta_Predial.Value;
            }
            else
            {
                Orden_Variacion.P_Filtros_Dinamicos = "";
                Orden_Variacion.P_Filtros_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + " = '" + Txt_Busqueda.Text.ToUpper() + "' OR ";
                Orden_Variacion.P_Filtros_Dinamicos += Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " LIKE '%" + Txt_Busqueda.Text.ToUpper() + "'";
            }
            Orden_Variacion.P_Maximo_Registros = 50001;
            P_Dt_Ordenes_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion();
            Grid_Ordenes_Variacion.DataSource = P_Dt_Ordenes_Variacion;// Dt_Limitado_Ordenes_Variacion;
            Grid_Ordenes_Variacion.PageIndex = Pagina;
            Grid_Ordenes_Variacion.DataBind();

            if (P_Dt_Ordenes_Variacion.Rows.Count == 1)
            {
                Grid_Ordenes_Variacion.SelectedIndex = 0;
                Grid_Ordenes_Variacion_SelectedIndexChanged(null, null);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION : Mensaje_Error
    ///DESCRIPCION          : Muestra el mensaje de error
    ///PARAMETROS           : Mensaje, texto del mensaje
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += Mensaje + "</br>";
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION : Llenar_Combo_Grupos_Movimientos
    ///DESCRIPCION          : Consulta los datos de Grupos Movimientos y los carga en el Combo indicado
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Llenar_Combo_Grupos_Movimientos()
    {
        Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio Grupos_Movimientos = new Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio();
        Grupos_Movimientos.P_Campos_Dinamicos = Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + ", (" + Cat_Pre_Grupos_Movimiento.Campo_Clave + " || ' - ' || " + Cat_Pre_Grupos_Movimiento.Campo_Nombre + ") AS " + Cat_Pre_Grupos_Movimiento.Campo_Clave;
        DataTable Dt_Grupos_Movimientos = Grupos_Movimientos.Consultar_Grupos_Movimientos();
        DataRow Dr_Grupos_Movimientos = Dt_Grupos_Movimientos.NewRow();
        Dr_Grupos_Movimientos[Cat_Pre_Grupos_Movimiento.Campo_Clave] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
        Dr_Grupos_Movimientos[Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID] = "SELECCIONE";
        Dt_Grupos_Movimientos.Rows.InsertAt(Dr_Grupos_Movimientos, 0);
        Cmb_Grupos_Movimientos.DataTextField = Cat_Pre_Grupos_Movimiento.Campo_Clave;
        Cmb_Grupos_Movimientos.DataValueField = Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID;
        Cmb_Grupos_Movimientos.DataSource = Dt_Grupos_Movimientos;
        Cmb_Grupos_Movimientos.DataBind();
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION : Consultar_Ultimo_No_Nota
    ///DESCRIPCION          : Consulta los Grupos y Obtiene el último Número de Nota Registrado
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Consultar_Ultimo_No_Nota()
    {
        if (Cmb_Grupos_Movimientos.SelectedItem.Value != "SELECCIONE")
        {
            //Arma el No de Nota
            Int32 No_Nota_Consecutivo = Convert.ToInt32(Obtener_Dato_Consulta("NVL(MAX(" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + "), 0)", Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion, Ope_Pre_Ordenes_Variacion.Campo_Anio + " || " + Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID + " || " + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID + " = '" + Lbl_Año_Movimiento.Text + Cmb_Grupos_Movimientos.SelectedItem.Value + Hdn_Tipo_Predio_ID.Value + "' ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_No_Nota, "0").Trim());
            //Int32 No_Nota_Inicial = Convert.ToInt32(Obtener_Dato_Consulta("NVL(" + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial + ", 0)", Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles, Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " || " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " || " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " = '" + Lbl_Año_Movimiento.Text + Cmb_Grupos_Movimientos.SelectedItem.Value + Cmb_Tipos_Predio.SelectedItem.Value + "'", "0").Trim());

            Lbl_Ultimo_No_Nota_Grupo.Text = No_Nota_Consecutivo.ToString();// (No_Nota_Consecutivo > No_Nota_Inicial ? No_Nota_Consecutivo : No_Nota_Inicial).ToString();
        }
        else
        {
            Lbl_Ultimo_No_Nota_Grupo.Text = "";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 19/Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
    {
        return Obtener_Dato_Consulta(Campo, Tabla, Condiciones, "");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Sobrecarga para consultar el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones, String Dato_Salida_Default)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Campo;
            if (Tabla != "")
            {
                Mi_SQL += " FROM " + Tabla;
            }
            if (Condiciones != "")
            {
                Mi_SQL += " WHERE " + Condiciones;
            }

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            if (Dr_Dato != null)
            {
                Dr_Dato.Close();
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        if (Dato_Consulta == "")
        {
            Dato_Consulta = Dato_Salida_Default;
        }

        return Dato_Consulta;
    }
    #endregion

    #region Eventos Controles
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Evento Click de control ImageButton
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Btn_Modificar.AlternateText == "Modificar")
        {
            Configurar_Formulario(Tipos_Eventos.Modificar);
        }
        else
        {
            if (Validar_Datos_Obligatorios())
            {
                Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Ordenes_Variacion.P_Grupo_Movimiento_ID = Cmb_Grupos_Movimientos.SelectedItem.Value;
                Ordenes_Variacion.P_No_Nota = Convert.ToInt16(Txt_No_Nota.Text);
                Ordenes_Variacion.P_Fecha_Nota = DateTime.Now;
                Ordenes_Variacion.P_No_Nota_Impreso = "NO";
                Ordenes_Variacion.P_Orden_Variacion_ID = Txt_No_Movimiento.Text;
                Ordenes_Variacion.P_Año = Convert.ToInt16(Lbl_Año_Movimiento.Text);
                Ordenes_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
                Ordenes_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                if (Ordenes_Variacion.Modificar_Orden_Variacion())
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Captura de Número de Nota", "alert('El Número de Nota fue asigando correctamente');", true);
                    Configurar_Formulario(Tipos_Eventos.Default);
                    Cargar_Grid_Ordenes_Variacion(0);
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Evento Click de control ImageButton
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText == "Salir")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configurar_Formulario(Tipos_Eventos.Cancelar);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Click
    ///DESCRIPCIÓN          : Evento Click de control ImageButton
    ///PROPIEDADES          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if (Txt_Busqueda.Text.Trim() != "")
        {
            Cargar_Grid_Ordenes_Variacion(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Boolean Busqueda_Ubicaciones;
        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);

        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Hdn_Cuenta_Predial_ID.Value = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdn_Cuenta_Predial.Value = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Cargar_Grid_Ordenes_Variacion(0, Busqueda_Por.Cuenta_Predial);
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("CUENTA_PREDIAL");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Ordenes_Variacion_PageIndexChanging
    ///DESCRIPCIÓN          : Manejo de la paginación del GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Ordenes_Variacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Ordenes_Variacion.SelectedIndex = (-1);
            Grid_Ordenes_Variacion.DataSource = P_Dt_Ordenes_Variacion;
            Grid_Ordenes_Variacion.PageIndex = e.NewPageIndex;
            Grid_Ordenes_Variacion.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Ordenes_Variacion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Manejo de la seleccion de la fila del GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Ordenes_Variacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            //String Grupo_Movimiento__Tipos_Predio = "";
            //String Grupo_Movimiento = "";
            //String Tipos_Predio = "";

            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            Limpiar_Datos_Controles();

            Hdn_Cuenta_Predial_ID.Value = Grid_Ordenes_Variacion.SelectedDataKey["CUENTA_PREDIAL_ID"].ToString();
            Txt_Cuenta_Predial.Text = HttpUtility.HtmlDecode(Grid_Ordenes_Variacion.SelectedRow.Cells[6].Text).Trim();
            Lbl_Año_Movimiento.Text = HttpUtility.HtmlDecode(Grid_Ordenes_Variacion.SelectedDataKey["ANIO"].ToString()).Trim();
            Txt_No_Movimiento.Text = HttpUtility.HtmlDecode(Grid_Ordenes_Variacion.SelectedRow.Cells[7].Text).Trim();
            Txt_Tipo_Movimiento.Text = HttpUtility.HtmlDecode(Grid_Ordenes_Variacion.SelectedRow.Cells[8].Text).Trim();
            if (Cmb_Grupos_Movimientos.Items.Count > 1)
            {
                Cmb_Grupos_Movimientos.SelectedValue = Grid_Ordenes_Variacion.SelectedDataKey["GRUPO_MOVIMIENTO_ID"].ToString();
            }
            Hdn_Tipo_Predio_ID.Value = Grid_Ordenes_Variacion.SelectedDataKey["TIPO_PREDIO_ID"].ToString();
            Txt_Tipo_Predio.Text = Grid_Ordenes_Variacion.SelectedDataKey["TIPO_PREDIO"].ToString();
            Txt_No_Nota.Text = HttpUtility.HtmlDecode(Grid_Ordenes_Variacion.SelectedRow.Cells[11].Text).Trim();
            Consultar_Ultimo_No_Nota();

            Btn_Modificar.Enabled = true;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Grupos_Movimientos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento de control DropDawnList
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Mayo/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Cmb_Grupos_Movimientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Consultar_Ultimo_No_Nota();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Busqueda_TextChanged
    ///DESCRIPCIÓN          : Evento de control TextBox
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 31/Mayo/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Txt_Busqueda_TextChanged(object sender, EventArgs e)
    {
        Btn_Buscar_Click(sender, null);
    }
    #endregion
}
