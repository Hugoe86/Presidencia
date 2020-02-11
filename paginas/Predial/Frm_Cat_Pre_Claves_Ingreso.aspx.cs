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
using Presidencia.Catalogo_Claves_Ingreso.Negocio;

public partial class paginas_Predial_Frm_Cat_Pre_Claves_Ingreso : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
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
                Configuracion_Acceso("Frm_Cat_Pre_Claves_Ingreso.aspx");
                Configuracion_Formulario(true);
                Llenar_Combo();
                Llenar_Combo_Documentos();
                Llenar_Combo_Gastos_Ejecucion();
                //Llenar_Combo_Movimientos();
                Llenar_Combo_Ramas();
                Llenar_Combo_Otros_Pagos();
                Llenar_Combo_Cuentas_Contables();
                Llenar_Combo_Unidad_Responsable();
                Llenar_Tabla_Claves_Ingreso(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Eliminar.Visible = Estatus;
        Txt_Descripcion.Enabled = !Estatus;
        Txt_Fundamento.Enabled = !Estatus;
        Txt_Clave.Enabled = !Estatus;
        Cmb_Estatus.Enabled = !Estatus;
        Cmb_Grupo.Enabled = !Estatus;
        Cmb_Rama.Enabled = !Estatus;
        Grid_Claves_Ingreso.Enabled = Estatus;
        Grid_Claves_Ingreso.SelectedIndex = (-1);
        Btn_Buscar.Enabled = Estatus;
        Txt_Busqueda.Enabled = Estatus;
        Tab_Claves_Ingreso.Enabled = !Estatus;
        //Tab_Grupos_Movimientos.Enabled = !Estatus;
        //Tab_Otros_Pagos.Enabled = !Estatus;
        //Tab_Documentos.Enabled = !Estatus;
        //Tab_Gastos_Ejecucion.Enabled = !Estatus;
        //Tab_Predial_Traslado.Enabled = !Estatus;
        Cmb_Documentos.Enabled = !Estatus;
        Cmb_Gastos_Ejecucion.Enabled = !Estatus;
        Cmb_Otros_Pagos.Enabled = !Estatus;
        //Cmb_Movimiento.Enabled = !Estatus;
        Cmb_Cuenta_Contable.Enabled = !Estatus;
        Cmb_Unidad_Responsable.Enabled = !Estatus;
        Cmb_Tipo.Enabled = !Estatus;
        Cmb_Tipo_Predial_Traslado.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.    Numero de Empleado que se verificara.
    ///             2. Clave.      Nombre del Modulo que se verificara.
    ///             3. Tabla.        Numero de la Caja que se verificara.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle(String Clave_Ingreso, String Clave, DataTable Tabla)
    {
        Boolean Encontrada = false;
        if (Tabla != null && Tabla.Rows.Count > 0)
        {
            for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
            {
                if (Tabla.Rows[cnt][0].ToString().Trim().Equals(Clave_Ingreso) &&
                    Tabla.Rows[cnt][1].ToString().Trim().Equals(Clave))
                {
                    Encontrada = true;
                    break;
                }
            }
        }
        return Encontrada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.    Numero de Empleado que se verificara.
    ///             2. Clave.      Nombre del Modulo que se verificara.
    ///             3. Tabla.        Numero de la Caja que se verificara.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Costo(String Anio, DataTable Tabla)
    {
        Boolean Encontrada = false;
        if (Tabla != null && Tabla.Rows.Count > 0)
        {
            for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
            {
                if (Tabla.Rows[cnt][2].ToString().Trim()==Txt_Anio.Text)
                {
                    Encontrada = true;
                    break;
                }
            }
        }
        return Encontrada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.    Numero de Empleado que se verificara.
    ///             2. Clave.      Nombre del Modulo que se verificara.
    ///             3. Tabla.        Numero de la Caja que se verificara.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Costo_Modificando(String Anio, DataTable Tabla)
    {
        Boolean Encontrada = false;
        if (Tabla != null && Tabla.Rows.Count > 0)
        {
            for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
            {
                if (Tabla.Rows[cnt][1].ToString().Trim() == Txt_Anio.Text)
                {
                    Encontrada = true;
                    break;
                }
            }
        }
        return Encontrada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.    Numero de Empleado que se verificara.
    ///             2. Clave.      Nombre del Modulo que se verificara.
    ///             3. Tabla.        Numero de la Caja que se verificara.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Predial(String Clave_Ingreso, String Tipo, String Tipo_Predial, DataTable Tabla)
    {
        Boolean Encontrada = false;
        if (Tabla != null && Tabla.Rows.Count > 0)
        {
            for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
            {
                if (Tabla.Rows[cnt][0].ToString().Trim().Equals(Clave_Ingreso) &&
                    Tabla.Rows[cnt][1].ToString().Trim().Equals(Tipo) &&
                    Tabla.Rows[cnt][2].ToString().Trim().Equals(Tipo_Predial))
                {
                    Encontrada = true;
                    break;
                }
            }
        }
        return Encontrada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle_Campo_Movimiento
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.   Clave Ingreso a buscar.
    ///             2. Clave.   Clave del Detalle a buscar.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Campo_Movimiento(String Clave_Ingreso, String Clave)
    {
        Cls_Cat_Pre_Claves_Ingreso_Negocio Tabla = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Tabla.P_Clave_Ingreso_ID = Clave_Ingreso;
        Tabla.P_Clave = Clave;
        DataTable DT_Tabla = Tabla.Buscar_Campo_Movimiento();
        if (DT_Tabla.Rows.Count > 0)
        {
            return false;
        }
        else 
        {
            return true;
        }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle_Campo_Otro_Pago
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.   Clave Ingreso a buscar.
    ///             2. Clave.   Clave del Detalle a buscar.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Campo_Otro_Pago(String Clave_Ingreso, String Clave)
    {
        Cls_Cat_Pre_Claves_Ingreso_Negocio Tabla = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Tabla.P_Clave_Ingreso_ID = Clave_Ingreso;
        Tabla.P_Clave = Clave;
        DataTable DT_Tabla = Tabla.Buscar_Campo_Otro_Pago();
        if (DT_Tabla.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle_Campo_Documento
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.   Clave Ingreso a buscar.
    ///             2. Clave.   Clave del Detalle a buscar.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Campo_Documento(String Clave_Ingreso, String Clave)
    {
        Cls_Cat_Pre_Claves_Ingreso_Negocio Tabla = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Tabla.P_Clave_Ingreso_ID = Clave_Ingreso;
        Tabla.P_Clave = Clave;
        DataTable DT_Tabla = Tabla.Buscar_Campo_Documento();
        if (DT_Tabla.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle_Campo_Gasto
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.   Clave Ingreso a buscar.
    ///             2. Clave.   Clave del Detalle a buscar.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Campo_Gasto(String Clave_Ingreso, String Clave)
    {
        Cls_Cat_Pre_Claves_Ingreso_Negocio Tabla = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Tabla.P_Clave_Ingreso_ID = Clave_Ingreso;
        Tabla.P_Clave = Clave;
        DataTable DT_Tabla = Tabla.Buscar_Campo_Predial_Traslado();
        if (DT_Tabla.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Detalle_Campo_Gasto
    ///DESCRIPCIÓN: Permite verificar que una Asignacion de Detalle de Claves de ingreso 
    ///             no sea repetida.
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.   Clave Ingreso a buscar.
    ///             2. Clave.   Clave del Detalle a buscar.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 11/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Detalle_Campo_Predial_Traslado(String Clave_Ingreso, String Tipo, String Tipo_Predial_Traslado)
    {
        Cls_Cat_Pre_Claves_Ingreso_Negocio Tabla = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Tabla.P_Clave_Ingreso_ID = Clave_Ingreso;
        Tabla.P_Tipo = Tipo;
        Tabla.P_Tipo_Predial_Traslado = Tipo_Predial_Traslado;
        DataTable DT_Tabla = Tabla.Buscar_Campo_Predial_Traslado();
        if (DT_Tabla.Rows.Count > 0)
        {
            return false;
        }
        else
        {
            return true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Encontrar_Clave_Ingreso
    ///DESCRIPCIÓN: Permite verificar que una Clave de Ingreso no se Duplique
    ///PROPIEDADES:   
    ///             1. Clave_Ingreso.   Clave Ingreso a buscar.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Encontrar_Clave_Ingreso(String Clave_Ingreso, String Clave_Ingreso_ID)
    {
        Cls_Cat_Pre_Claves_Ingreso_Negocio Tabla = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Tabla.P_Clave = Clave_Ingreso;
        Boolean Variable = true;
        DataSet DT_Tabla = Tabla.Buscar_Clave_Ingreso();
        if (Btn_Nuevo.AlternateText.Equals("Dar de Alta"))
        {
            if (DT_Tabla.Tables[0].Rows.Count > 0)
            {
                Variable = false;
            }
            else
            {
                Variable = true;
            }
        }
        else if (Btn_Modificar.AlternateText.Equals("Actualizar") && DT_Tabla.Tables[0].Rows.Count > 0)
        {
            if (DT_Tabla.Tables[0].Rows[0]["CLAVE"].ToString().Equals(Clave_Ingreso) &&
                DT_Tabla.Tables[0].Rows[0]["CLAVE_INGRESO_ID"].ToString().Equals(Clave_Ingreso_ID))
            {
                Variable = true;
            }
            else if (DT_Tabla.Tables[0].Rows[0]["CLAVE"].ToString().Equals(Clave_Ingreso))
            {
                Variable = false;
            }
        }
        return Variable;
    }

    #region Llenar Campos de Detalles

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Llenar_Campos_Movimiento
    /////DESCRIPCIÓN: Llena los campos de la pestaña de Movimientos
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Llenar_Campos_Movimiento(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
    //        Clave.P_Movimiento_ID = Cmb_Movimiento.SelectedItem.Value;
    //        Txt_Clave_Movimiento.Text = Cmb_Movimiento.SelectedItem.Value;
    //        DataSet Claves = Clave.Consultar_Clave_Movimiento();
    //        Txt_Descripcion_Mov.Text = Claves.Tables[0].Rows[0]["DESCRIPCION"].ToString();
    //        Txt_Descripcion_Movimiento.Text = Claves.Tables[0].Rows[0]["NOMBRE"].ToString();


    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Campos_Documento
    ///DESCRIPCIÓN: Llena los campos de la pestaña de Documentos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Campos_Documento(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Clave.P_Documento_ID = Cmb_Documentos.SelectedItem.Value;
            Txt_Clave_Documento.Text = Cmb_Documentos.SelectedItem.Value;
            DataSet Claves = Clave.Consultar_Clave_Documento();
            Txt_Descripcion_Doc.Text = Claves.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            Txt_Descripcion_Documento.Text = Claves.Tables[0].Rows[0]["NOMBRE"].ToString();


        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Campos_Otro_Pago
    ///DESCRIPCIÓN: Llena los campos de la pestaña de Otros Pagos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Campos_Otro_Pago(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Clave.P_Pago_ID = Cmb_Otros_Pagos.SelectedItem.Value;
            Txt_Clave_Otro_Pago.Text = Cmb_Otros_Pagos.SelectedItem.Value;
            DataSet Claves = Clave.Consultar_Clave_Otro_Pago();
            Txt_Descripcion_OP.Text = Claves.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            Txt_Descripcion_Otros_Pagos.Text = Claves.Tables[0].Rows[0]["NOMBRE"].ToString();


        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Campos_Gasto_Ejecucion
    ///DESCRIPCIÓN: Llena los campos de la pestaña de Gastos de Ejecucion.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Campos_Gasto_Ejecucion(object sender, EventArgs e)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Clave.P_Gasto_ID = Cmb_Gastos_Ejecucion.SelectedItem.Value;
            Txt_Clave_Gastos.Text = Cmb_Gastos_Ejecucion.SelectedItem.Value;
            DataSet Claves = Clave.Consultar_Clave_Gasto_Ejecucion();
            Txt_Descripcion_Gas.Text = Claves.Tables[0].Rows[0]["DESCRIPCION"].ToString();
            Txt_Descripcion_Gastos.Text = Claves.Tables[0].Rows[0]["NOMBRE"].ToString();


        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    //#region Pestañas

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Tab_Claves_Ingreso_Click
    /////DESCRIPCIÓN: Cambia la configuracion de los controles si la pestaña de 
    /////             Claves de Ingreso fue seleccionada
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Tab_Claves_Ingreso_Click()
    //{
    //    try
    //    {
    //        Btn_Nuevo.AlternateText = "Nuevo";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.AlternateText = "Modificar";
    //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
    //        Btn_Eliminar.AlternateText = "Eliminar";
    //        Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
    //        Btn_Salir.AlternateText = "Salir";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Tab_Movimientos_Click
    /////DESCRIPCIÓN: Cambia la configuracion de los controles si la pestaña de 
    /////             Movimientos fue seleccionada
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Tab_Movimientos_Click()
    //{
    //    try
    //    {
    //        Btn_Nuevo.Enabled = true;
    //        Btn_Salir.Enabled = true;
    //        Btn_Nuevo.AlternateText = "Nuevo_Movimiento";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.Visible = false;
    //        Btn_Eliminar.Visible = false;
    //        Btn_Salir.AlternateText = "Cancelar";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Tab_Documentos_Click
    /////DESCRIPCIÓN: Cambia la configuracion de los controles si la pestaña de 
    /////             Documentos fue seleccionada
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Tab_Documentos_Click()
    //{
    //    try
    //    {
    //        Btn_Nuevo.Enabled = true;
    //        Btn_Salir.Enabled = true;
    //        Btn_Nuevo.AlternateText = "Nuevo_Movimiento";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.Visible = false;
    //        Btn_Eliminar.Visible = false;
    //        Btn_Salir.AlternateText = "Cancelar";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Tab_Gastos_Ejecucion_Click
    /////DESCRIPCIÓN: Cambia la configuracion de los controles si la pestaña de 
    /////             Gastos de Ejecucion fue seleccionada
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Tab_Gastos_Ejecucion_Click()
    //{
    //    try
    //    {
    //        Btn_Nuevo.Enabled = true;
    //        Btn_Salir.Enabled = true;
    //        Btn_Nuevo.AlternateText = "Nuevo_Movimiento";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.Visible = false;
    //        Btn_Eliminar.Visible = false;
    //        Btn_Salir.AlternateText = "Cancelar";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Tab_Otros_Pagos_Click
    /////DESCRIPCIÓN: Cambia la configuracion de los controles si la pestaña de 
    /////             Otros Pagos fue seleccionada
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Tab_Otros_Pagos_Click()
    //{
    //    try
    //    {
    //        Btn_Nuevo.Enabled = true;
    //        Btn_Salir.Enabled = true;
    //        Btn_Nuevo.AlternateText = "Nuevo_Movimiento";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.Visible = false;
    //        Btn_Eliminar.Visible = false;
    //        Btn_Salir.AlternateText = "Cancelar";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Tab_Predial_Ingresos_Click
    /////DESCRIPCIÓN: Cambia la configuracion de los controles si la pestaña de 
    /////             Predial Traslado fue seleccionada
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 25/Agosto/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Tab_Predial_Traslado_Click()
    //{
    //    try
    //    {
    //        Btn_Nuevo.Enabled = true;
    //        Btn_Salir.Enabled = true;
    //        Btn_Nuevo.AlternateText = "Nuevo_Predial";
    //        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
    //        Btn_Modificar.Visible = false;
    //        Btn_Eliminar.Visible = false;
    //        Btn_Salir.AlternateText = "Cancelar";
    //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    //#endregion

    #region Crear La Clave De Ingreso

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Rama_SelectedIndexChanged
    ///DESCRIPCIÓN: Llena el combo de Grupos de acuerdo a la Rama seleccionada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Rama_SelectedIndexChanged(object sender, EventArgs e)
    {
        if(Cmb_Grupo.SelectedIndex != 0){
            if (Cmb_Rama.SelectedIndex != 0)
            {
                Llenar_Combo_Grupos();
                Cmb_Grupo.Enabled = true;
                if (Cmb_Grupo.SelectedIndex != 0)
                {

                    Llenar_Clave();
                }
            }
        }
        else {
            if (Cmb_Rama.SelectedIndex != 0)
             {
                    Llenar_Combo_Grupos();
                    Cmb_Grupo.Enabled = true;
                    if (Cmb_Grupo.SelectedIndex != 0)
                    {

                        Llenar_Clave();
                    }
                }
            }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Grupo_SelectedIndexChanged
    ///DESCRIPCIÓN: Llena la caja de texto de la Clave de Ingreso
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Grupo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Grupo.SelectedIndex != 0)
        {
            Llenar_Clave();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Clave
    ///DESCRIPCIÓN: Crea la Clave de Ingreso de acuerdo a la Rama y Grupo seleccionado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Clave()
    {
        Txt_Clave.Text = Cmb_Grupo.SelectedItem.Text.Substring(0, 4);
    }

    #endregion

    #region Combos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN: Metodo que llena el Combo de Grupos con los sectores existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Grupo = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Grupos = Grupo.Llenar_Combo();
            DataRow fila = Grupos.NewRow();
            fila[Cat_Pre_Grupos.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Grupos.Campo_Grupo_ID] = "SELECCIONE";
            Grupos.Rows.InsertAt(fila, 0);
            Cmb_Grupo.DataTextField = Cat_Pre_Grupos.Campo_Nombre;
            Cmb_Grupo.DataValueField = Cat_Pre_Grupos.Campo_Grupo_ID;
            Cmb_Grupo.DataSource = Grupos;
            Cmb_Grupo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Grupos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Grupos con los sectores existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Grupos()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Grupo = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Grupo.P_Rama_ID = Cmb_Rama.SelectedItem.Value;
            DataTable Grupos = Grupo.Llenar_Combo_Grupos();
            DataRow fila = Grupos.NewRow();
            fila[Cat_Pre_Grupos.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Grupos.Campo_Grupo_ID] = "SELECCIONE";
            Grupos.Rows.InsertAt(fila, 0);
            Cmb_Grupo.DataTextField = Cat_Pre_Grupos.Campo_Nombre;
            Cmb_Grupo.DataValueField = Cat_Pre_Grupos.Campo_Grupo_ID;
            Cmb_Grupo.DataSource = Grupos;
            Cmb_Grupo.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Ramas
    ///DESCRIPCIÓN: Metodo que llena el Combo de Ramas con las ramas existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Ramas()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Rama = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Ramas = Rama.Llenar_Combo_Ramas();
            DataRow fila = Ramas.NewRow();
            fila[Cat_Pre_Ramas.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Ramas.Campo_Rama_ID] = "SELECCIONE";
            Ramas.Rows.InsertAt(fila, 0);
            Cmb_Rama.DataTextField = Cat_Pre_Ramas.Campo_Nombre;
            Cmb_Rama.DataValueField = Cat_Pre_Ramas.Campo_Rama_ID;
            Cmb_Rama.DataSource = Ramas;
            Cmb_Rama.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Documentos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Documentos con los documentos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Documentos()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Documento = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Documentos = Documento.Llenar_Combo_Documentos();
            DataRow fila = Documentos.NewRow();
            fila[Cat_Pre_Tipos_Constancias.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID] = "SELECCIONE";
            Documentos.Rows.InsertAt(fila, 0);
            Cmb_Documentos.DataTextField = Cat_Pre_Tipos_Constancias.Campo_Descripcion;
            Cmb_Documentos.DataValueField = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID;
            Cmb_Documentos.DataSource = Documentos;
            Cmb_Documentos.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Llenar_Combo_Movimientos
    /////DESCRIPCIÓN: Metodo que llena el Combo de Movimientos con los tipos de movimiento existentes.
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 23/Julio/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Llenar_Combo_Movimientos()
    //{
    //    try
    //    {
    //        Cls_Cat_Pre_Claves_Ingreso_Negocio Movimiento = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
    //        DataTable Movimientos = Movimiento.Llenar_Combo_Movimientos();
    //        DataRow fila = Movimientos.NewRow();
    //        fila[Cat_Pre_Movimientos.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        fila[Cat_Pre_Movimientos.Campo_Movimiento_ID] = "SELECCIONE";
    //        Movimientos.Rows.InsertAt(fila, 0);
    //        Cmb_Movimiento.DataTextField = Cat_Pre_Movimientos.Campo_Descripcion;
    //        Cmb_Movimiento.DataValueField = Cat_Pre_Movimientos.Campo_Movimiento_ID;
    //        Cmb_Movimiento.DataSource = Movimientos;
    //        Cmb_Movimiento.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Gastos_Ejecucion
    ///DESCRIPCIÓN: Metodo que llena el Combo de Gastos de Ejecucion 
    ///             con los gastos de ejecucion existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Gastos_Ejecucion()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Gasto = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Gastos = Gasto.Llenar_Combo_Gastos_Ejecucion();
            DataRow fila = Gastos.NewRow();
            fila[Cat_Pre_Gastos_Ejecucion.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID] = "SELECCIONE";
            Gastos.Rows.InsertAt(fila, 0);
            Cmb_Gastos_Ejecucion.DataTextField = Cat_Pre_Gastos_Ejecucion.Campo_Nombre;
            Cmb_Gastos_Ejecucion.DataValueField = Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;
            Cmb_Gastos_Ejecucion.DataSource = Gastos;
            Cmb_Gastos_Ejecucion.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Otros_Pagos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Otros Pagos con los otros pagos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Otros_Pagos()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Pago = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Pagos = Pago.Llenar_Combo_Otros_Pagos();
            DataRow fila = Pagos.NewRow();
            fila[Cat_Pre_Otros_Pagos.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Pre_Otros_Pagos.Campo_Pago_ID] = "SELECCIONE";
            Pagos.Rows.InsertAt(fila, 0);
            Cmb_Otros_Pagos.DataTextField = Cat_Pre_Otros_Pagos.Campo_Descripcion;
            Cmb_Otros_Pagos.DataValueField = Cat_Pre_Otros_Pagos.Campo_Pago_ID;
            Cmb_Otros_Pagos.DataSource = Pagos;
            Cmb_Otros_Pagos.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Otros_Pagos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Otros Pagos con los otros pagos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Cuentas_Contables()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Cuenta = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Cuentas = Cuenta.Llenar_Combo_Cuentas_Contables();
            DataRow fila = Cuentas.NewRow();
            fila[Cat_Con_Cuentas_Contables.Campo_Descripcion] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID] = "SELECCIONE";
            Cuentas.Rows.InsertAt(fila, 0);
            Cmb_Cuenta_Contable.DataTextField = Cat_Con_Cuentas_Contables.Campo_Descripcion;
            Cmb_Cuenta_Contable.DataValueField = Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID;
            Cmb_Cuenta_Contable.DataSource = Cuentas;
            Cmb_Cuenta_Contable.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Otros_Pagos
    ///DESCRIPCIÓN: Metodo que llena el Combo de Otros Pagos con los otros pagos existentes.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Unidad_Responsable()
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Dependencia = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            DataTable Dependencias = Dependencia.Llenar_Combo_Unidad_Responsable();
            DataRow fila = Dependencias.NewRow();
            fila[Cat_Dependencias.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            fila[Cat_Dependencias.Campo_Dependencia_ID] = "SELECCIONE";
            Dependencias.Rows.InsertAt(fila, 0);
            Cmb_Unidad_Responsable.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Unidad_Responsable.DataSource = Dependencias;
            Cmb_Unidad_Responsable.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion  

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;

        if (Btn_Modificar.AlternateText.Equals("Actualizar") || Btn_Nuevo.AlternateText.Equals("Dar de Alta"))
        {
            if (!Encontrar_Clave_Ingreso(Txt_Clave.Text.Trim(), Txt_Clave_Ingreso_ID.Text.Trim()))
            {
                Mensaje_Error = Mensaje_Error + "+ Esa Clave de Ingreso ya existe.";
                Validacion = false;
            }
        }

        if (Txt_Clave.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave de Ingreso.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Cmb_Rama.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Ramas.";
            Validacion = false;
        }
        if (Cmb_Grupo.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Grupos.";
            Validacion = false;
        }
        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir La Descripcion.";
            Validacion = false;
        }
        if (Cmb_Unidad_Responsable.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Selecciona una opción en el Combo de Unidad Responsable.";
            Validacion = false;
        }
        if (Cmb_Cuenta_Contable.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Selecciona una opción en el Combo de Cuentas Contables.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #region Llenar Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Claves_Ingreso
    ///DESCRIPCIÓN: Llena el Grid de Claves de Ingreso
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Claves_Ingreso(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Grid_Claves_Ingreso.DataSource = Claves.Llenar_Tabla_Claves_Ingreso();
            Grid_Claves_Ingreso.PageIndex = Pagina;
            Grid_Claves_Ingreso.Columns[1].Visible = true;
            Grid_Claves_Ingreso.Columns[4].Visible = true;
            Grid_Claves_Ingreso.Columns[5].Visible = true;
            Grid_Claves_Ingreso.Columns[7].Visible = true;
            Grid_Claves_Ingreso.Columns[8].Visible = true;
            Grid_Claves_Ingreso.Columns[11].Visible = true;
            Grid_Claves_Ingreso.Columns[12].Visible = true;
            Grid_Claves_Ingreso.Columns[13].Visible = true;
            Grid_Claves_Ingreso.DataBind();
            Grid_Claves_Ingreso.Columns[1].Visible = false;
            Grid_Claves_Ingreso.Columns[4].Visible = false;
            Grid_Claves_Ingreso.Columns[5].Visible = false;
            Grid_Claves_Ingreso.Columns[7].Visible = false;
            Grid_Claves_Ingreso.Columns[8].Visible = false;
            Grid_Claves_Ingreso.Columns[11].Visible = false;
            Grid_Claves_Ingreso.Columns[12].Visible = false;
            Grid_Claves_Ingreso.Columns[13].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Movimientos
    ///DESCRIPCIÓN: Llena la tabla de Costos de claves.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO: Miguel Angel Bedolla Moreno
    ///FECHA_MODIFICO: 17/Noviembre/2011
    ///CAUSA_MODIFICACIÓN: Se cambio de ser de movimientos a ser de costos de claves.
    ///*******************************************************************************
    private void Llenar_Tabla_Movimientos(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
            DataTable Dt_Movimientos = Claves.Consultar_Costos_Claves();
            Grid_Movimientos.DataSource = Dt_Movimientos;
            Grid_Movimientos.PageIndex = Pagina;
            Grid_Movimientos.Columns[1].Visible = true;
            Grid_Movimientos.Columns[0].Visible = true;
            Grid_Movimientos.DataBind();
            Grid_Movimientos.Columns[1].Visible = false;
            Grid_Movimientos.Columns[0].Visible = false;
            Grid_Movimientos.Enabled = true;
            Session["Dt_Movimientos"] = Dt_Movimientos;
            Session["Asignacion_Movimiento"] = Dt_Movimientos;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Otros_Pagos
    ///DESCRIPCIÓN: Llena la tabla de Otros Pagos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Otros_Pagos(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
            DataTable Dt_Otros_Pagos = Claves.Llenar_Tabla_Otros_Pagos();
            Grid_Otros_Pagos.DataSource = Dt_Otros_Pagos;
            Grid_Otros_Pagos.PageIndex = Pagina;
            Grid_Otros_Pagos.Columns[1].Visible = true;
            Grid_Otros_Pagos.Columns[4].Visible = true;
            Grid_Otros_Pagos.DataBind();
            Grid_Otros_Pagos.Columns[1].Visible = false;
            Grid_Otros_Pagos.Columns[4].Visible = false;
            Grid_Otros_Pagos.Enabled = true;
            Session["Dt_Otros_Pagos"] = Dt_Otros_Pagos;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Documentos
    ///DESCRIPCIÓN: Llena la tabla de Documentos.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Documentos(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
            DataTable Dt_Documentos = Claves.Llenar_Tabla_Documentos();
            Grid_Documentos.DataSource = Dt_Documentos;
            Grid_Documentos.PageIndex = Pagina;
            Grid_Documentos.Columns[1].Visible = true;
            Grid_Documentos.Columns[4].Visible = true;
            Grid_Documentos.DataBind();
            Grid_Documentos.Columns[1].Visible = false;
            Grid_Documentos.Columns[4].Visible = false;
            Grid_Documentos.Enabled = true;
            Session["Dt_Documentos"] = Dt_Documentos;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Gastos_Ejecucion
    ///DESCRIPCIÓN: Llena la tabla de Gastos de Ejecucion.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Gastos_Ejecucion(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
            DataTable Dt_Gastos = Claves.Llenar_Tabla_Gastos_Ejecucion();
            Grid_Gastos_Ejecucion.DataSource = Dt_Gastos;
            Grid_Gastos_Ejecucion.PageIndex = Pagina;
            Grid_Gastos_Ejecucion.Columns[1].Visible = true;
            Grid_Gastos_Ejecucion.Columns[2].Visible = true;
            Grid_Gastos_Ejecucion.Columns[5].Visible = true;
            Grid_Gastos_Ejecucion.DataBind();
            Grid_Gastos_Ejecucion.Columns[1].Visible = false;
            Grid_Gastos_Ejecucion.Columns[2].Visible = false;
            Grid_Gastos_Ejecucion.Columns[5].Visible = false;
            Grid_Gastos_Ejecucion.Enabled = true;
            Session["Dt_Gastos"] = Dt_Gastos;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Gastos_Ejecucion
    ///DESCRIPCIÓN: Llena la tabla de Gastos de Ejecucion.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Predial_Traslado(int Pagina)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
            DataTable Dt_Predial = Claves.Llenar_Tabla_Predial_Traslado();
            Grid_Predial_Traslado.DataSource = Dt_Predial;
            Grid_Predial_Traslado.PageIndex = Pagina;
            Grid_Predial_Traslado.Columns[1].Visible = true;
            //Grid_Gastos_Ejecucion.Columns[2].Visible = true;
            //Grid_Gastos_Ejecucion.Columns[5].Visible = true;
            Grid_Predial_Traslado.DataBind();
            Grid_Predial_Traslado.Columns[1].Visible = false;
            //Grid_Gastos_Ejecucion.Columns[2].Visible = false;
            //Grid_Gastos_Ejecucion.Columns[5].Visible = false;
            Grid_Predial_Traslado.Enabled = true;
            Session["Dt_Predial"] = Dt_Predial;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Claves_Ingreso_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Claves de Ingreso de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Claves_Ingreso_Busqueda(int Pagina) 
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Claves.P_Clave = Txt_Busqueda.Text.Trim().ToUpper(); 
            Grid_Claves_Ingreso.DataSource = Claves.Llenar_Tabla_Claves_Ingreso_Busqueda();
            Grid_Claves_Ingreso.PageIndex = Pagina;
            Grid_Claves_Ingreso.Columns[1].Visible = true;
            Grid_Claves_Ingreso.Columns[4].Visible = true;
            Grid_Claves_Ingreso.Columns[5].Visible = true;
            Grid_Claves_Ingreso.Columns[7].Visible = true;
            Grid_Claves_Ingreso.Columns[8].Visible = true;
            Grid_Claves_Ingreso.Columns[11].Visible = true;
            Grid_Claves_Ingreso.DataBind();
            Grid_Claves_Ingreso.Columns[1].Visible = false;
            Grid_Claves_Ingreso.Columns[4].Visible = false;
            Grid_Claves_Ingreso.Columns[5].Visible = false;
            Grid_Claves_Ingreso.Columns[7].Visible = false;
            Grid_Claves_Ingreso.Columns[8].Visible = false;
            Grid_Claves_Ingreso.Columns[11].Visible = false;
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

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Claves_Ingreso_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Claves de Ingreso
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Claves_Ingreso_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Claves_Ingreso.SelectedIndex = (-1);
            Llenar_Tabla_Claves_Ingreso(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Movimientos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Movimientos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Movimientos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Movimientos.SelectedIndex = (-1);
            Llenar_Tabla_Movimientos(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Otros_Pagos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Otros Pagos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Otros_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Otros_Pagos.SelectedIndex = (-1);
            Llenar_Tabla_Otros_Pagos(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Documentos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Documentos.SelectedIndex = (-1);
            Llenar_Tabla_Documentos(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Gastos_Ejecucion_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Gastos de Ejecucion
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Gastos_Ejecucion.SelectedIndex = (-1);
            Llenar_Tabla_Gastos_Ejecucion(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Gastos_Ejecucion_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de Gastos de Ejecucion
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Predial_Traslado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Predial_Traslado.SelectedIndex = (-1);
            Llenar_Tabla_Predial_Traslado(e.NewPageIndex);
            //Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Claves_Ingreso_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Clave de Ingreso Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Claves_Ingreso_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Claves_Ingreso.SelectedIndex > (-1))
            {
                Txt_Clave_Ingreso_ID.Text = HttpUtility.HtmlDecode(Grid_Claves_Ingreso.SelectedRow.Cells[1].Text);
                Txt_Clave.Text = HttpUtility.HtmlDecode(Grid_Claves_Ingreso.SelectedRow.Cells[2].Text);
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Grid_Claves_Ingreso.SelectedRow.Cells[3].Text));
                Cmb_Rama.SelectedIndex = Cmb_Rama.Items.IndexOf(Cmb_Rama.Items.FindByText(Grid_Claves_Ingreso.SelectedRow.Cells[4].Text));
                Cmb_Rama.SelectedIndex = Cmb_Rama.Items.IndexOf(Cmb_Rama.Items.FindByValue(Grid_Claves_Ingreso.SelectedRow.Cells[5].Text));
                Llenar_Combo_Grupos();
                Cmb_Grupo.SelectedIndex = Cmb_Grupo.Items.IndexOf(Cmb_Grupo.Items.FindByText(Grid_Claves_Ingreso.SelectedRow.Cells[7].Text));
                Cmb_Grupo.SelectedIndex = Cmb_Grupo.Items.IndexOf(Cmb_Grupo.Items.FindByValue(Grid_Claves_Ingreso.SelectedRow.Cells[8].Text));
                Cmb_Unidad_Responsable.SelectedIndex = Cmb_Unidad_Responsable.Items.IndexOf(Cmb_Unidad_Responsable.Items.FindByValue(Grid_Claves_Ingreso.SelectedRow.Cells[13].Text));
                Cmb_Cuenta_Contable.SelectedIndex = Cmb_Cuenta_Contable.Items.IndexOf(Cmb_Cuenta_Contable.Items.FindByValue(Grid_Claves_Ingreso.SelectedRow.Cells[12].Text));
                Txt_Descripcion.Text = HttpUtility.HtmlDecode(Grid_Claves_Ingreso.SelectedRow.Cells[10].Text);
                Txt_Fundamento.Text = HttpUtility.HtmlDecode(Grid_Claves_Ingreso.SelectedRow.Cells[11].Text);
                System.Threading.Thread.Sleep(1000);
                //Tab_Documentos.Enabled = false;
                //Tab_Otros_Pagos.Enabled = false;
                //Tab_Grupos_Movimientos.Enabled = false;
                //Tab_Gastos_Ejecucion.Enabled = false;
                //Tab_Predial_Traslado.Enabled = false;
                Llenar_Tabla_Documentos(0);
                Llenar_Tabla_Gastos_Ejecucion(0);
                Llenar_Tabla_Movimientos(0);
                Llenar_Tabla_Otros_Pagos(0);
                Llenar_Tabla_Predial_Traslado(0);
                Session["Asignacion_Predial_Traslado"] = null;
                Session["Asignacion_Gastos_Ejecucion"] = null;
                Session["Asignacion_Documentos"] = null;
                Session["Asignacion_Otros_Pagos"] = null;
                Session["Asignacion_Movimiento"] = null;

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
    ///NOMBRE DE LA FUNCIÓN: Grid_Movimientos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Movimiento Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Movimientos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Movimientos.SelectedIndex > (-1))
            {
                //Cmb_Movimiento.SelectedIndex = Cmb_Movimiento.Items.IndexOf(Cmb_Movimiento.Items.FindByText(Grid_Movimientos.SelectedRow.Cells[4].Text));
                //Cmb_Movimiento.SelectedIndex = Cmb_Movimiento.Items.IndexOf(Cmb_Movimiento.Items.FindByValue(Grid_Movimientos.SelectedRow.Cells[2].Text)); 
                System.Threading.Thread.Sleep(1000);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Otros_Pagos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de Otro Pago Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Otros_Pagos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Otros_Pagos.SelectedIndex > (-1))
            {
                Cmb_Otros_Pagos.SelectedIndex = Cmb_Otros_Pagos.Items.IndexOf(Cmb_Otros_Pagos.Items.FindByText(Grid_Otros_Pagos.SelectedRow.Cells[4].Text));
                Cmb_Otros_Pagos.SelectedIndex = Cmb_Otros_Pagos.Items.IndexOf(Cmb_Otros_Pagos.Items.FindByValue(Grid_Otros_Pagos.SelectedRow.Cells[2].Text));
                System.Threading.Thread.Sleep(1000);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Documento Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Documentos.SelectedIndex > (-1))
            {
                Cmb_Documentos.SelectedIndex = Cmb_Documentos.Items.IndexOf(Cmb_Documentos.Items.FindByText(Grid_Documentos.SelectedRow.Cells[4].Text));
                Cmb_Documentos.SelectedIndex = Cmb_Documentos.Items.IndexOf(Cmb_Documentos.Items.FindByValue(Grid_Documentos.SelectedRow.Cells[2].Text));
                System.Threading.Thread.Sleep(1000);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Gasto de Ejecucion Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Gastos_Ejecucion.SelectedIndex > (-1))
            {
                Cmb_Gastos_Ejecucion.SelectedIndex = Cmb_Gastos_Ejecucion.Items.IndexOf(Cmb_Gastos_Ejecucion.Items.FindByText(Grid_Gastos_Ejecucion.SelectedRow.Cells[5].Text));
                Cmb_Gastos_Ejecucion.SelectedIndex = Cmb_Gastos_Ejecucion.Items.IndexOf(Cmb_Gastos_Ejecucion.Items.FindByValue(Grid_Gastos_Ejecucion.SelectedRow.Cells[2].Text));
                System.Threading.Thread.Sleep(1000);
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos del Gasto de Ejecucion Seleccionado para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Predial_Traslado_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Predial_Traslado.SelectedIndex > (-1))
            {
                Cmb_Tipo.SelectedIndex = Cmb_Tipo.Items.IndexOf(Cmb_Tipo.Items.FindByText(Grid_Predial_Traslado.SelectedRow.Cells[2].Text));
                Cmb_Tipo_Predial_Traslado.SelectedIndex = Cmb_Tipo_Predial_Traslado.Items.IndexOf(Cmb_Tipo_Predial_Traslado.Items.FindByText(Grid_Predial_Traslado.SelectedRow.Cells[3].Text));
                System.Threading.Thread.Sleep(1000);
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

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Movimiento_Click
    ///DESCRIPCIÓN: Agrega el Movimiento seleccionado al Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Movimiento_Click(object sender, EventArgs e)
    {
        if (Btn_Nuevo.AlternateText == "Dar de Alta")
        {
            try
            {
                Convert.ToDouble(Txt_Costo.Text);
            }
            catch (Exception E)
            {
                Txt_Costo.Text = "";
                return;
            }

            try
            {
                Session["Dt_Movimientos"] = null;
                DataTable tabla;
                //Tab_Movimientos_Click();
                if (Session["Asignacion_Movimiento"] == null)
                {
                    //tabla = (DataTable)Grid_Movimientos.DataSource;
                    //Session["Asignacion_Movimiento"] = tabla;
                    tabla = new DataTable("Asignacion_Movimiento");
                    tabla.Columns.Add("COSTO_CLAVE_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE_INGRESO_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("ANIO", Type.GetType("System.Int32"));
                    tabla.Columns.Add("COSTO", Type.GetType("System.Double"));
                }
                else
                {
                    tabla = (DataTable)Session["Asignacion_Movimiento"];
                }
                if (!Encontrar_Detalle_Costo(Txt_Anio.Text.Trim(), tabla) && Txt_Anio.Text.Length == 4 && Txt_Costo.Text != "")
                {
                    DataRow fila = tabla.NewRow();
                    fila["COSTO_CLAVE_ID"] = "0";
                    fila["CLAVE_INGRESO_ID"] = "0";
                    fila["ANIO"] = Convert.ToInt32(Txt_Anio.Text);
                    fila["COSTO"] = Convert.ToDouble(Txt_Costo.Text);
                    //fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Mov.Text.ToUpper());
                    //fila["CLAVE_MOVIMIENTO"] = HttpUtility.HtmlDecode(Txt_Descripcion_Movimiento.Text.ToUpper());
                    Grid_Movimientos.Columns[1].Visible = true;
                    Grid_Movimientos.Columns[0].Visible = true;
                    tabla.Rows.Add(fila);
                    Session["Asignacion_Movimiento"] = tabla;
                    Grid_Movimientos.DataSource = tabla;
                    Grid_Movimientos.DataBind();
                    Grid_Movimientos.Columns[1].Visible = false;
                    Grid_Movimientos.Columns[0].Visible = false;
                    Txt_Anio.Text = "";
                    Txt_Costo.Text = "";
                    //Limpiar_Catalogo();
                    //Grid_Movimientos.Enabled = false;
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            try
            {
                Convert.ToDouble(Txt_Costo.Text);
            }
            catch (Exception E)
            {
                Txt_Costo.Text = "";
                return;
            }
            DataTable tabla;
            if (Session["Dt_Movimientos"] != null)
            {
                tabla = (DataTable)Session["Dt_Movimientos"];
            }
            else if (Session["Asignacion_Movimiento"] == null)
            {
                //tabla = (DataTable)Grid_Movimientos.DataSource;
                //Session["Asignacion_Movimiento"] = tabla;
                tabla = new DataTable("Asignacion_Movimiento");
                tabla.Columns.Add("COSTO_CLAVE_ID", Type.GetType("System.String"));
                tabla.Columns.Add("CLAVE_INGRESO_ID", Type.GetType("System.String"));
                tabla.Columns.Add("ANIO", Type.GetType("System.Int32"));
                tabla.Columns.Add("COSTO", Type.GetType("System.Double"));
            }
            else
            {
                tabla = (DataTable)Session["Asignacion_Movimiento"];
            }
            if (!Encontrar_Detalle_Costo_Modificando(Txt_Anio.Text.Trim(), tabla) && Txt_Anio.Text.Length == 4 && Txt_Costo.Text != "")
            {
                Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
                Clave.P_Anio = Txt_Anio.Text;
                Clave.P_Costo = Convert.ToDouble(Txt_Costo.Text).ToString();
                Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Clave.Alta_Costo_Clave();
                Clave.P_Anio = null;
                tabla = Clave.Consultar_Costos_Claves();
                Grid_Movimientos.Columns[1].Visible = true;
                Grid_Movimientos.Columns[0].Visible = true;
                Session["Asignacion_Movimiento"] = tabla;
                Session["Dt_Movimientos"] = tabla;
                Grid_Movimientos.DataSource = tabla;
                if (Grid_Movimientos.PageIndex > 0)
                {
                    Grid_Movimientos.PageIndex = Grid_Movimientos.PageCount - 1;
                }
                else
                {
                    Grid_Movimientos.PageIndex = 0;
                }
                Grid_Movimientos.DataBind();
                Grid_Movimientos.Columns[1].Visible = false;
                Grid_Movimientos.Columns[0].Visible = false;
                Txt_Anio.Text = "";
                Txt_Costo.Text = "";
                //Limpiar_Catalogo();
                //Grid_Movimientos.Enabled = false;
            }
        }
     }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Otros_Pagos_Click
    ///DESCRIPCIÓN: Agrega el Otro Pago seleccionado al Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Otros_Pagos_Click(object sender, EventArgs e)
    {
        if (Cmb_Otros_Pagos.SelectedItem.Value != null && Cmb_Otros_Pagos.SelectedIndex != 0)
        {
            try
            {
                Session["Dt_Otros_Pagos"] = null;
                DataTable tabla;
                //Tab_Otros_Pagos_Click();
                if (Session["Asignacion_Otros_Pagos"] == null)
                {
                    //tabla = (DataTable)Grid_Otros_Pagos.DataSource;
                    //Session["Asignacion_Otros_Pagos"] = tabla;
                    tabla = new DataTable("Asignacion_Otros_Pagos");
                    tabla.Columns.Add("CLAVE_INGRESO_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE", Type.GetType("System.String"));
                    tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE_OTRO_PAGO", Type.GetType("System.String"));
                }
                else
                {
                    tabla = (DataTable)Session["Asignacion_Otros_Pagos"];
                }
                if (!Encontrar_Detalle(Txt_Clave_Ingreso_ID.Text.Trim(),
                    Cmb_Otros_Pagos.SelectedItem.Value, tabla) &&
                    Encontrar_Detalle_Campo_Otro_Pago(Txt_Clave_Ingreso_ID.Text,
                    Cmb_Otros_Pagos.SelectedItem.Value))
                {
                    DataRow fila = tabla.NewRow();
                    fila["CLAVE_INGRESO_ID"] = HttpUtility.HtmlDecode(Txt_Clave_Ingreso_ID.Text.Trim());
                    fila["CLAVE"] = HttpUtility.HtmlDecode(Cmb_Otros_Pagos.SelectedItem.Value);
                    fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_OP.Text.ToUpper());
                    fila["CLAVE_OTRO_PAGO"] = HttpUtility.HtmlDecode(Txt_Descripcion_Otros_Pagos.Text.ToUpper());
                    Grid_Otros_Pagos.Columns[1].Visible = true;
                    Grid_Otros_Pagos.Columns[4].Visible = true;
                    tabla.Rows.Add(fila);
                    Session["Asignacion_Otros_Pagos"] = tabla;
                    Grid_Otros_Pagos.DataSource = tabla;
                    Grid_Otros_Pagos.DataBind();
                    Grid_Otros_Pagos.Columns[1].Visible = false;
                    Grid_Otros_Pagos.Columns[4].Visible = false;
                    //Grid_Otros_Pagos.Enabled = false;
                    //Limpiar_Catalogo();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Documentos_Click
    ///DESCRIPCIÓN: Agrega el Tipo de Documento seleccionado al Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Documentos_Click(object sender, EventArgs e)
    {
        if (Cmb_Documentos.SelectedItem.Value != null && Cmb_Documentos.SelectedIndex != 0)
        {
            try
            {
                Session["Dt_Documentos"] = null;
                DataTable tabla;
                //Tab_Documentos_Click();
                if (Session["Asignacion_Documentos"] == null)
                {
                    //tabla = (DataTable)Grid_Documentos.DataSource;
                    //Session["Asignacion_Documentos"] = tabla;
                    tabla = new DataTable("Asignacion_Documentos");
                    tabla.Columns.Add("CLAVE_INGRESO_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE", Type.GetType("System.String"));
                    tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE_DOCUMENTO", Type.GetType("System.String"));
                }
                else
                {
                    tabla = (DataTable)Session["Asignacion_Documentos"];
                }
                if (!Encontrar_Detalle(Txt_Clave_Ingreso_ID.Text.Trim(),
                    Cmb_Documentos.SelectedItem.Value, tabla) &&
                    Encontrar_Detalle_Campo_Documento(Txt_Clave_Ingreso_ID.Text,
                    Cmb_Documentos.SelectedItem.Value))
                {
                    DataRow fila = tabla.NewRow();
                    fila["CLAVE_INGRESO_ID"] = HttpUtility.HtmlDecode(Txt_Clave_Ingreso_ID.Text.Trim());
                    fila["CLAVE"] = HttpUtility.HtmlDecode(Cmb_Documentos.SelectedItem.Value);
                    fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Doc.Text.ToUpper());
                    fila["CLAVE_DOCUMENTO"] = HttpUtility.HtmlDecode(Txt_Descripcion_Documento.Text.ToUpper());
                    Grid_Documentos.Columns[1].Visible = true;
                    Grid_Documentos.Columns[4].Visible = true;
                    tabla.Rows.Add(fila);
                    Session["Asignacion_Documentos"] = tabla;
                    Grid_Documentos.DataSource = tabla;
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[1].Visible = false;
                    Grid_Documentos.Columns[4].Visible = false;
                    //Grid_Documentos.Enabled = false;
                    //Limpiar_Catalogo();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Gastos_Ejecucion
    ///DESCRIPCIÓN: Agrega el Gasto de Ejecucion seleccionado al Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Gastos_Ejecucion(object sender, EventArgs e)
    {
        if (Cmb_Gastos_Ejecucion.SelectedItem.Value != null && Cmb_Gastos_Ejecucion.SelectedIndex != 0)
        {
            try
            {
                Session["Dt_Gastos"] = null;
                DataTable tabla;
                //Tab_Gastos_Ejecucion_Click();
                if (Session["Asignacion_Gastos_Ejecucion"] == null)
                {
                    //tabla = (DataTable)Grid_Gastos_Ejecucion.DataSource;
                    //Session["Asignacion_Gastos_Ejecucion"] = tabla;
                    tabla = new DataTable("Asignacion_Gastos_Ejecucion");
                    tabla.Columns.Add("CLAVE_INGRESO_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("GASTO_EJECUCION_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE", Type.GetType("System.String"));
                    tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                    tabla.Columns.Add("CLAVE_GASTOS", Type.GetType("System.String"));
                }
                else
                {
                    tabla = (DataTable)Session["Asignacion_Gastos_Ejecucion"];
                }
                if (!Encontrar_Detalle(Txt_Clave_Ingreso_ID.Text.Trim(),
                    Cmb_Gastos_Ejecucion.SelectedItem.Value, tabla) &&
                    Encontrar_Detalle_Campo_Gasto(Txt_Clave_Ingreso_ID.Text,
                    Cmb_Gastos_Ejecucion.SelectedItem.Value))
                {
                    DataRow fila = tabla.NewRow();
                    fila["CLAVE_INGRESO_ID"] = HttpUtility.HtmlDecode(Txt_Clave_Ingreso_ID.Text.Trim());
                    fila["GASTO_EJECUCION_ID"] = HttpUtility.HtmlDecode(Cmb_Gastos_Ejecucion.SelectedItem.Value);
                    fila["CLAVE"] = HttpUtility.HtmlDecode(Txt_Clave_Gastos.Text.Trim());
                    fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Gas.Text.ToUpper());
                    fila["CLAVE_GASTOS"] = HttpUtility.HtmlDecode(Txt_Descripcion_Gastos.Text.ToUpper());
                    Grid_Gastos_Ejecucion.Columns[1].Visible = true;
                    Grid_Gastos_Ejecucion.Columns[2].Visible = true;
                    Grid_Gastos_Ejecucion.Columns[5].Visible = true;
                    tabla.Rows.Add(fila);
                    Session["Asignacion_Gastos_Ejecucion"] = tabla;
                    Grid_Gastos_Ejecucion.DataSource = tabla;
                    Grid_Gastos_Ejecucion.DataBind();
                    Grid_Gastos_Ejecucion.Columns[1].Visible = false;
                    Grid_Gastos_Ejecucion.Columns[2].Visible = false;
                    Grid_Gastos_Ejecucion.Columns[5].Visible = false;
                    //Grid_Gastos_Ejecucion.Enabled = false;
                    //Limpiar_Catalogo();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Predial_Traslado
    ///DESCRIPCIÓN: Agrega el Tipo seleccionado al Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Predial_Traslado(object sender, EventArgs e)
    {
        if (Cmb_Tipo.SelectedItem.Value != null && Cmb_Tipo.SelectedIndex != 0
            && Cmb_Tipo_Predial_Traslado.SelectedItem.Value != null && Cmb_Tipo_Predial_Traslado.SelectedIndex != 0)
        {
            try
            {
                Session["Dt_Predial"] = null;
                DataTable tabla;
                //Tab_Predial_Traslado_Click();
                if (Session["Asignacion_Predial_Traslado"] == null)
                {
                    //tabla = (DataTable)Grid_Gastos_Ejecucion.DataSource;
                    //Session["Asignacion_Gastos_Ejecucion"] = tabla;
                    tabla = new DataTable("Asignacion_Predial_Traslado");
                    tabla.Columns.Add("CLAVE_INGRESO_ID", Type.GetType("System.String"));
                    tabla.Columns.Add("TIPO", Type.GetType("System.String"));
                    tabla.Columns.Add("TIPO_PREDIAL_TRASLADO", Type.GetType("System.String"));
                }
                else
                {
                    tabla = (DataTable)Session["Asignacion_Predial_Traslado"];
                }
                if (!Encontrar_Detalle_Predial(Txt_Clave_Ingreso_ID.Text.Trim(),
                    Cmb_Tipo.SelectedItem.Text, Cmb_Tipo_Predial_Traslado.SelectedItem.Text, tabla) &&
                    Encontrar_Detalle_Campo_Predial_Traslado(Txt_Clave_Ingreso_ID.Text, 
                    Cmb_Tipo.SelectedItem.Text, Cmb_Tipo_Predial_Traslado.SelectedItem.Text))
                {
                    DataRow fila = tabla.NewRow();
                    fila["CLAVE_INGRESO_ID"] = HttpUtility.HtmlDecode(Txt_Clave_Ingreso_ID.Text.Trim());
                    fila["TIPO"] = HttpUtility.HtmlDecode(Cmb_Tipo.SelectedItem.Text);
                    fila["TIPO_PREDIAL_TRASLADO"] = HttpUtility.HtmlDecode(Cmb_Tipo_Predial_Traslado.SelectedItem.Text);
                    Grid_Predial_Traslado.Columns[1].Visible = true;
                    //Grid_Gastos_Ejecucion.Columns[2].Visible = true;
                    //Grid_Gastos_Ejecucion.Columns[5].Visible = true;
                    tabla.Rows.Add(fila);
                    Session["Asignacion_Predial_Traslado"] = tabla;
                    Grid_Predial_Traslado.DataSource = tabla;
                    Grid_Predial_Traslado.DataBind();
                    Grid_Predial_Traslado.Columns[1].Visible = false;
                    //Grid_Gastos_Ejecucion.Columns[2].Visible = false;
                    //Grid_Gastos_Ejecucion.Columns[5].Visible = false;
                    //Grid_Predial_Traslado.Enabled = false;
                    //Limpiar_Catalogo();
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una Clave de Ingreso
    ///             o alguno de sus detalles.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {

            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Cmb_Grupo.Enabled = false;
                //Tab_Grupos_Movimientos.Enabled = false;
                //Tab_Otros_Pagos.Enabled = false;
                //Tab_Documentos.Enabled = false;
                //Tab_Gastos_Ejecucion.Enabled = false;
                //Tab_Predial_Traslado.Enabled = false;
                //Limpiar_Catalogo();
                Cmb_Cuenta_Contable.SelectedIndex = 0;
                Cmb_Unidad_Responsable.SelectedIndex = 0;
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Grupo.SelectedIndex = 0;
                Cmb_Rama.SelectedIndex = 0;
                Txt_Clave_Ingreso_ID.Text = "";
                Txt_Clave.Text = "";
                Txt_Descripcion.Text = "";
                Txt_Fundamento.Text = "";
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;

                Session["Dt_Otros_Pagos"] = null;
                Session["Asignacion_Otros_Pagos"] = null;
                Session["Dt_Documentos"] = null;
                Session["Asignacion_Documentos"] = null;
                Session["Dt_Movimientos"] = null;
                Session["Asignacion_Movimiento"] = null;
                Session["Dt_Gastos"] = null;
                Session["Asignacion_Gastos"] = null;
                Session["Dt_Predial"] = null;
                Session["Asignacion_Predial_Traslado"] = null;

                //Grid_Otros_Pagos.Visible = false;
                //Grid_Movimientos.Visible = false;
                //Grid_Gastos_Ejecucion.Visible = false;
                //Grid_Documentos.Visible = false;
                //Grid_Predial_Traslado.Visible = false;
                return;
            }
            else
            {
                if (Btn_Nuevo.AlternateText.Equals("Dar de Alta") && Validar_Componentes_Generales())
                {
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Claves = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Claves.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
                    Claves.P_Grupo_ID = Cmb_Grupo.SelectedItem.Value;
                    Claves.P_Clave = Txt_Clave.Text.Trim().ToUpper();
                    Claves.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                    Claves.P_Cuenta_Contable_ID = Cmb_Cuenta_Contable.SelectedItem.Value;
                    Claves.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value;
                    Claves.P_Descripcion = Txt_Descripcion.Text.ToUpper();
                    Claves.P_Fundamento = Txt_Fundamento.Text.ToUpper();
                    //Limpiar_Catalogo();
                    Claves.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Grid_Claves_Ingreso.Columns[1].Visible = true;
                    Grid_Claves_Ingreso.Columns[4].Visible = true;
                    Grid_Claves_Ingreso.Columns[5].Visible = true;
                    Grid_Claves_Ingreso.Columns[7].Visible = true;
                    Grid_Claves_Ingreso.Columns[8].Visible = true;
                    Grid_Claves_Ingreso.Columns[11].Visible = true;
                    Grid_Claves_Ingreso.Columns[12].Visible = true;
                    Grid_Claves_Ingreso.Columns[13].Visible = true;
                    Claves.Alta_Clave_Ingreso();
                    Grid_Claves_Ingreso.Columns[1].Visible = false;
                    Grid_Claves_Ingreso.Columns[4].Visible = false;
                    Grid_Claves_Ingreso.Columns[5].Visible = false;
                    Grid_Claves_Ingreso.Columns[7].Visible = false;
                    Grid_Claves_Ingreso.Columns[8].Visible = false;
                    Grid_Claves_Ingreso.Columns[11].Visible = false;
                    Grid_Claves_Ingreso.Columns[12].Visible = false;
                    Grid_Claves_Ingreso.Columns[13].Visible = false;
                    Txt_Clave_Ingreso_ID.Text = Claves.P_Clave_Ingreso_ID;
                    //Aqui van a ir
                    if (Session["Asignacion_Movimiento"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave.P_Movimientos = (DataTable)Session["Asignacion_Movimiento"];
                        Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
                        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        //Clave.Eliminar_Detalle_Movimientos();
                        Clave.Llenar_Tabla_Movimientos_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        //Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        //Session["Asignacion_Documentos"] = null;
                        Session["Asignacion_Movimiento"] = null;
                        //Session["Asignacion_Predial_Traslado"] = null;
                    }

                    if (Session["Asignacion_Documentos"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave.P_Documentos = (DataTable)Session["Asignacion_Documentos"];
                        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave.Llenar_Tabla_Documentos_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        //Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        Session["Asignacion_Documentos"] = null;
                        //Session["Asignacion_Movimiento"] = null;
                        //Session["Asignacion_Predial_Traslado"] = null;
                    }

                    if (Session["Asignacion_Otros_Pagos"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave.P_Otros_Pagos = (DataTable)Session["Asignacion_Otros_Pagos"];
                        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave.Llenar_Tabla_Otros_Pagos_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        Session["Asignacion_Otros_Pagos"] = null;
                    }

                    if (Session["Asignacion_Gastos_Ejecucion"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave.P_Gastos_Ejecucion = (DataTable)Session["Asignacion_Gastos_Ejecucion"];
                        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave.Llenar_Tabla_Gastos_Ejecucion_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        //Session["Asignacion_Documentos"] = null;
                        //Session["Asignacion_Movimiento"] = null;
                        //Session["Asignacion_Predial_Traslado"] = null;
                    }

                    if (Session["Asignacion_Predial_Traslado"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave.P_Predial_Traslado = (DataTable)Session["Asignacion_Predial_Traslado"];
                        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave.Llenar_Tabla_Predial_Traslado_Detalles();
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        //Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        //Session["Asignacion_Documentos"] = null;
                        //Session["Asignacion_Movimiento"] = null;
                        Session["Asignacion_Predial_Traslado"] = null;
                    }   
                    Configuracion_Formulario(true);
                    //Limpiar_Catalogo();
                    Txt_Clave_Ingreso_ID.Text = "";
                    Txt_Busqueda.Text = "";
                    Cmb_Estatus.SelectedIndex = 0;
                    Cmb_Rama.SelectedIndex = 0;
                    Cmb_Grupo.SelectedIndex = 0;
                    Txt_Clave.Text = "";
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Cmb_Cuenta_Contable.SelectedIndex = 0;
                    Txt_Descripcion.Text = "";
                    Txt_Fundamento.Text = "";
                    Configuracion_Formulario(true);
                    //cerrar todas las sesiones

                    Cmb_Documentos.SelectedIndex = 0;
                    Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                    Cmb_Otros_Pagos.SelectedIndex = 0;
                    //Cmb_Movimiento.SelectedIndex = 0;
                    Cmb_Tipo.SelectedIndex = 0;
                    Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                    //Costos
                    Txt_Anio.Text = "";
                    Txt_Costo.Text = "";
                    Llenar_Tabla_Claves_Ingreso(Grid_Claves_Ingreso.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Alta de Clave de Ingreso Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Grid_Claves_Ingreso.Enabled = true;
                    //Tab_Documentos.Enabled = false;
                    //Tab_Gastos_Ejecucion.Enabled = false;
                    //Tab_Grupos_Movimientos.Enabled = false;
                    //Tab_Otros_Pagos.Enabled = false;
                    //Tab_Predial_Traslado.Enabled = false;
                }
            }
            Tab_Claves_Ingreso.ActiveTabIndex = 0;
            //Configuracion_Formulario(true);
            //Btn_Nuevo.AlternateText = "Nuevo";
            //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            //Btn_Modificar.Visible = true;
            //Btn_Eliminar.Visible = true;
            //Btn_Salir.AlternateText = "Salir";
            //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //Tab_Claves_Ingreso.ActiveTabIndex = 1;
            //Cmb_Movimiento.SelectedIndex = 0;
            //Cmb_Gastos_Ejecucion.SelectedIndex = 0;
            //Cmb_Documentos.SelectedIndex = 0;
            //Cmb_Otros_Pagos.SelectedIndex = 0;
            //Session["Asignacion_Gastos_Ejecucion"] = null;
            //Session["Asignacion_Otros_Pagos"] = null;
            //Session["Asignacion_Documentos"] = null;
            //Session["Asignacion_Movimiento"] = null;
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
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Clave de ingreso
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    //Tab_Grupos_Movimientos.Enabled = false;
                    //Tab_Otros_Pagos.Enabled = false;
                    //Tab_Documentos.Enabled = false;
                    //Tab_Gastos_Ejecucion.Enabled = false;
                    //Tab_Predial_Traslado.Enabled = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                DataTable Dt_Asignacion_Movimiento = (DataTable)Session["Dt_Asignacion_Movimiento"];
                DataTable Dt_Asignacion_Otros_Pagos = (DataTable)Session["Dt_Asignacion_Otros_Pagos"];
                DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
                DataTable Dt_Gastos_Ejecucion = (DataTable)Session["Dt_Gastos_Ejecucion"];
                DataTable Dt_Predial_Traslado = (DataTable)Session["Dt_Predial_Traslado"];
                
                if (Validar_Componentes_Generales() && Btn_Modificar.AlternateText.Equals("Actualizar"))
                {

                    Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.Trim();
                    Clave.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Clave.P_Grupo_ID = Cmb_Grupo.SelectedItem.Value;
                    Clave.P_Clave = Txt_Clave.Text.ToUpper().Trim();
                    Clave.P_Cuenta_Contable_ID = Cmb_Cuenta_Contable.SelectedItem.Value;
                    Clave.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedItem.Value;
                    Clave.P_Descripcion = Txt_Descripcion.Text.ToUpper().Trim();
                    Clave.P_Fundamento = Txt_Fundamento.Text.ToUpper().Trim();
                    Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Grid_Claves_Ingreso.Columns[1].Visible = true;
                    Grid_Claves_Ingreso.Columns[4].Visible = true;
                    Grid_Claves_Ingreso.Columns[5].Visible = true;
                    Grid_Claves_Ingreso.Columns[7].Visible = true;
                    Grid_Claves_Ingreso.Columns[8].Visible = true;
                    Grid_Claves_Ingreso.Columns[11].Visible = true;
                    Grid_Claves_Ingreso.Columns[12].Visible = true;
                    Grid_Claves_Ingreso.Columns[13].Visible = true;
                    Clave.Modificar_Clave_Ingreso();
                    Grid_Claves_Ingreso.Columns[1].Visible = false;
                    Grid_Claves_Ingreso.Columns[4].Visible = false;
                    Grid_Claves_Ingreso.Columns[5].Visible = false;
                    Grid_Claves_Ingreso.Columns[7].Visible = false;
                    Grid_Claves_Ingreso.Columns[8].Visible = false;
                    Grid_Claves_Ingreso.Columns[11].Visible = false;
                    Grid_Claves_Ingreso.Columns[12].Visible = false;
                    Grid_Claves_Ingreso.Columns[13].Visible = false;

                    //Aqui Mero

                    if (Session["Asignacion_Otros_Pagos"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Pago = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave_Pago.P_Otros_Pagos = (DataTable)Session["Asignacion_Otros_Pagos"];
                        Clave_Pago.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave_Pago.Llenar_Tabla_Otros_Pagos_Detalles();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        Session["Asignacion_Otros_Pagos"] = null;
                    }

                    if (Session["Asignacion_Movimiento"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Movimiento = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave_Movimiento.P_Movimientos = (DataTable)Session["Asignacion_Movimiento"];
                        Clave_Movimiento.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave_Movimiento.P_Clave_Ingreso_ID = Clave.P_Clave_Ingreso_ID;
                        //Clave.Eliminar_Detalle_Movimientos();
                        //Clave_Movimiento.Llenar_Tabla_Movimientos_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        //Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        //Session["Asignacion_Documentos"] = null;
                        Session["Asignacion_Movimiento"] = null;
                        //Session["Asignacion_Predial_Traslado"] = null;
                    }

                    if (Session["Asignacion_Documentos"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Documento = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave_Documento.P_Documentos = (DataTable)Session["Asignacion_Documentos"];
                        Clave_Documento.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave_Documento.Llenar_Tabla_Documentos_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        //Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        Session["Asignacion_Documentos"] = null;
                        //Session["Asignacion_Movimiento"] = null;
                        //Session["Asignacion_Predial_Traslado"] = null;
                    }

                    if (Session["Asignacion_Gastos_Ejecucion"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Gasto = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave_Gasto.P_Gastos_Ejecucion = (DataTable)Session["Asignacion_Gastos_Ejecucion"];
                        Clave_Gasto.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave_Gasto.Llenar_Tabla_Gastos_Ejecucion_Detalles();
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Detalle de Clave de Ingreso Exitosa');", true);
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        //Session["Asignacion_Documentos"] = null;
                        //Session["Asignacion_Movimiento"] = null;
                        //Session["Asignacion_Predial_Traslado"] = null;
                    }

                    if (Session["Asignacion_Predial_Traslado"] != null)
                    {
                        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave_Predial = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                        Clave_Predial.P_Predial_Traslado = (DataTable)Session["Asignacion_Predial_Traslado"];
                        Clave_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                        Clave_Predial.Llenar_Tabla_Predial_Traslado_Detalles();
                        //Configuracion_Formulario(true);
                        //Btn_Nuevo.AlternateText = "Nuevo";
                        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        //Btn_Modificar.Visible = true;
                        //Btn_Eliminar.Visible = true;
                        //Btn_Salir.AlternateText = "Salir";
                        //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Tab_Claves_Ingreso.ActiveTabIndex = 1;
                        //Cmb_Movimiento.SelectedIndex = 0;
                        Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                        Cmb_Documentos.SelectedIndex = 0;
                        Cmb_Otros_Pagos.SelectedIndex = 0;
                        Cmb_Tipo.SelectedIndex = 0;
                        Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                        //Session["Asignacion_Gastos_Ejecucion"] = null;
                        //Session["Asignacion_Otros_Pagos"] = null;
                        //Session["Asignacion_Documentos"] = null;
                        //Session["Asignacion_Movimiento"] = null;
                        Session["Asignacion_Predial_Traslado"] = null;
                    }  


                    Configuracion_Formulario(true);
                    //Limpiar_Catalogo();
                    Llenar_Tabla_Claves_Ingreso(Grid_Claves_Ingreso.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('Actualización de Clave de Ingreso Exitosa');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Claves_Ingreso.Enabled = true;

                    Txt_Clave_Ingreso_ID.Text = "";
                    Txt_Busqueda.Text = "";
                    Cmb_Estatus.SelectedIndex = 0;
                    Cmb_Rama.SelectedIndex = 0;
                    Cmb_Grupo.SelectedIndex = 0;
                    Txt_Clave.Text = "";
                    Cmb_Unidad_Responsable.SelectedIndex = 0;
                    Cmb_Cuenta_Contable.SelectedIndex = 0;
                    Txt_Descripcion.Text = "";
                    Txt_Fundamento.Text = "";
                    Configuracion_Formulario(true);
                    //cerrar todas las sesiones

                    Cmb_Documentos.SelectedIndex = 0;
                    Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                    Cmb_Otros_Pagos.SelectedIndex = 0;
                    //Cmb_Movimiento.SelectedIndex = 0;
                    Cmb_Tipo.SelectedIndex = 0;
                    Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                    //Costos
                    Txt_Anio.Text = "";
                    Txt_Costo.Text = ""; 
                    
                }

            //}
            //if (Btn_Modificar.AlternateText.Equals("Modificar_Movimiento"))
            //{
            //    if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
            //    {
                    
            //        Btn_Modificar.AlternateText = "Actualizar_Movimiento";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            //        Btn_Salir.AlternateText = "Cancelar";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            //        Btn_Nuevo.Visible = false;
            //    }
            //    else
            //    {
            //        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
            //        Lbl_Mensaje_Error.Text = "";
            //        Div_Contenedor_Msj_Error.Visible = true;
            //    }
            //}
            //else
            //{
            //    if (Btn_Modificar.AlternateText.Equals("Actualizar_Movimiento"))
            //    {
            //        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //        Clave.P_Detalle_ID = Txt_Detalle_ID.Text.Trim();
            //        Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.ToUpper().Trim();
            //        Clave.P_Movimiento_ID = Txt_Clave_Movimiento.Text.ToUpper().Trim();
            //        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //        Grid_Claves_Ingreso.Columns[1].Visible = true;
            //        Grid_Claves_Ingreso.Columns[2].Visible = true;
            //        Grid_Claves_Ingreso.Columns[5].Visible = true;
            //        Clave.Modificar_Movimiento();
            //        Grid_Claves_Ingreso.Columns[1].Visible = false;
            //        Grid_Claves_Ingreso.Columns[2].Visible = false;
            //        Grid_Claves_Ingreso.Columns[5].Visible = false;
            //        Configuracion_Formulario(true);
            //        //Limpiar_Catalogo();
            //        Llenar_Tabla_Movimientos(Grid_Otros_Pagos.PageIndex);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Detalles de Claves de Ingreso", "alert('Actualización de Movimiento Exitosa');", true);
            //        Btn_Modificar.AlternateText = "Modificar_Movimiento";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            //        Btn_Salir.AlternateText = "Salir";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //        Grid_Movimientos.Enabled = true;
            //    }
            //}

            //if (Btn_Modificar.AlternateText.Equals("Modificar_Pago"))
            //{
            //    if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
            //    {
                    
            //        Btn_Modificar.AlternateText = "Actualizar_Otro_Pago";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            //        Btn_Salir.AlternateText = "Cancelar";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            //        Btn_Nuevo.Visible = false;
            //    }
            //    else
            //    {
            //        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
            //        Lbl_Mensaje_Error.Text = "";
            //        Div_Contenedor_Msj_Error.Visible = true;
            //    }
            //}
            //else
            //{
            //    if (Btn_Modificar.AlternateText.Equals("Actualizar_Otro_Pago"))
            //    {
            //        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //        Clave.P_Detalle_ID = Txt_Detalle_ID_2.Text.Trim();
            //        Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.ToUpper().Trim();
            //        Clave.P_Pago_ID = Txt_Clave_Otro_Pago.Text.ToUpper().Trim();
            //        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //        Grid_Otros_Pagos.Columns[1].Visible = true;
            //        Grid_Otros_Pagos.Columns[2].Visible = true;
            //        Grid_Otros_Pagos.Columns[5].Visible = true;
            //        Clave.Modificar_Otro_Pago();
            //        Grid_Otros_Pagos.Columns[1].Visible = false;
            //        Grid_Otros_Pagos.Columns[2].Visible = false;
            //        Grid_Otros_Pagos.Columns[5].Visible = false;
            //        Configuracion_Formulario(true);
            //        //Limpiar_Catalogo();
            //        Llenar_Tabla_Otros_Pagos(Grid_Movimientos.PageIndex);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Detalles de Claves de Ingreso", "alert('Actualización de Otro Pago Exitoso');", true);
            //        Btn_Modificar.AlternateText = "Modificar_Pago";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            //        Btn_Salir.AlternateText = "Salir";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //        Grid_Otros_Pagos.Enabled = true;
            //    }
            //}

            //if (Btn_Modificar.AlternateText.Equals("Modificar_Documento"))
            //{
            //    if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
            //    {
                  
            //        Btn_Modificar.AlternateText = "Actualizar_Documento";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            //        Btn_Salir.AlternateText = "Cancelar";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            //        Btn_Nuevo.Visible = false;
            //    }
            //    else
            //    {
            //        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
            //        Lbl_Mensaje_Error.Text = "";
            //        Div_Contenedor_Msj_Error.Visible = true;
            //    }
            //}
            //else
            //{
            //    if (Btn_Modificar.AlternateText.Equals("Actualizar_Documento"))
            //    {
            //        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //        Clave.P_Detalle_ID = Txt_Detalle_ID_3.Text.Trim();
            //        Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.ToUpper().Trim();
            //        Clave.P_Documento_ID = Txt_Clave_Documento.Text.ToUpper().Trim();
            //        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //        Grid_Documentos.Columns[1].Visible = true;
            //        Grid_Documentos.Columns[2].Visible = true;
            //        Grid_Documentos.Columns[5].Visible = true;
            //        Clave.Modificar_Documento();
            //        Grid_Documentos.Columns[1].Visible = false;
            //        Grid_Documentos.Columns[2].Visible = false;
            //        Grid_Documentos.Columns[5].Visible = false;
            //        Configuracion_Formulario(true);
            //        //Limpiar_Catalogo();
            //        Llenar_Tabla_Documentos(Grid_Documentos.PageIndex);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Detalles de Claves de Ingreso", "alert('Actualización de Documento Exitosa');", true);
            //        Btn_Modificar.AlternateText = "Modificar_Documento";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            //        Btn_Salir.AlternateText = "Salir";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //        Grid_Documentos.Enabled = true;
            //    }
            //}

            //if (Btn_Modificar.AlternateText.Equals("Modificar_Gasto"))
            //{
            //    if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
            //    {
                    
            //        Btn_Modificar.AlternateText = "Actualizar_Gasto";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
            //        Btn_Salir.AlternateText = "Cancelar";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            //        Btn_Nuevo.Visible = false;
            //    }
            //    else
            //    {
            //        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
            //        Lbl_Mensaje_Error.Text = "";
            //        Div_Contenedor_Msj_Error.Visible = true;
            //    }
            //}
            //else
            //{
            //    if (Btn_Modificar.AlternateText.Equals("Actualizar_Gasto"))
            //    {
            //        Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //        Clave.P_Detalle_ID = Txt_Detalle_ID_4.Text.Trim();
            //        Clave.P_Clave_Ingreso_ID = Txt_Clave_Ingreso_ID.Text.ToUpper().Trim();
            //        Clave.P_Gasto_ID = Txt_Clave_Gastos.Text.ToUpper().Trim();
            //        Clave.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //        Grid_Documentos.Columns[1].Visible = true;
            //        Grid_Documentos.Columns[2].Visible = true;
            //        Grid_Documentos.Columns[3].Visible = true;
            //        Grid_Documentos.Columns[6].Visible = true;
            //        Clave.Modificar_Gasto();
            //        Grid_Documentos.Columns[1].Visible = false;
            //        Grid_Documentos.Columns[2].Visible = false;
            //        Grid_Documentos.Columns[3].Visible = false;
            //        Grid_Documentos.Columns[6].Visible = true;
            //        Configuracion_Formulario(true);
            //        //Limpiar_Catalogo();
            //        Llenar_Tabla_Gastos_Ejecucion(Grid_Gastos_Ejecucion.PageIndex);
            //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Detalles de Claves de Ingreso", "alert('Actualización de Gasto Exitosa');", true);
            //        Btn_Modificar.AlternateText = "Modificar_Gasto";
            //        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            //        Btn_Salir.AlternateText = "Salir";
            //        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            //        Grid_Gastos_Ejecucion.Enabled = true;
            //    }
            }
            Tab_Claves_Ingreso.ActiveTabIndex = 0;      
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
    ///DESCRIPCIÓN: Llena la Tabla de Claves de Ingreso con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Limpiar_Catalogo();
            Llenar_Tabla_Claves_Ingreso_Busqueda(0);
            if (Grid_Claves_Ingreso.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todas las Colonias almacenadas)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Claves_Ingreso(0);
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina una Clave de Ingreso de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            
            if (Grid_Claves_Ingreso.Rows.Count > 0 && Grid_Claves_Ingreso.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                Clave.P_Clave_Ingreso_ID = Grid_Claves_Ingreso.SelectedRow.Cells[1].Text;
                Clave.Eliminar_Clave_Ingreso();
                Grid_Claves_Ingreso.SelectedIndex = 0;
                Llenar_Tabla_Claves_Ingreso(Grid_Claves_Ingreso.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('La Clave de Ingreso fue eliminada exitosamente');", true);
                Tab_Claves_Ingreso.Enabled = false;

                Session["Asignacion_Movimiento"] = null;
                Session["Asignacion_Otros_Pagos"] = null;
                Session["Asignacion_Documentos"] = null;
                Session["Asignacion_Gastos_Ejecucion"] = null;
                Session["Asignacion_Predial_Traslado"] = null;

                Grid_Movimientos.DataBind();
                Grid_Gastos_Ejecucion.DataBind();
                Grid_Otros_Pagos.DataBind();
                Grid_Predial_Traslado.DataBind();
                Grid_Documentos.DataBind();

                Txt_Clave_Ingreso_ID.Text = "";
                Txt_Busqueda.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Rama.SelectedIndex = 0;
                Cmb_Grupo.SelectedIndex = 0;
                Txt_Clave.Text = "";
                Cmb_Unidad_Responsable.SelectedIndex = 0;
                Cmb_Cuenta_Contable.SelectedIndex = 0;
                Txt_Descripcion.Text = "";
                Txt_Fundamento.Text = "";
                Configuracion_Formulario(true);
                //cerrar todas las sesiones

                Cmb_Documentos.SelectedIndex = 0;
                Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                Cmb_Otros_Pagos.SelectedIndex = 0;
                //Cmb_Movimiento.SelectedIndex = 0;
                Cmb_Tipo.SelectedIndex = 0;
                Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;

                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Claves_Ingreso.Enabled = true;
                Tab_Claves_Ingreso.ActiveTabIndex = 0;

                //Limpiar_Catalogo();
            }
            //if (Grid_Movimientos.Rows.Count > 0 && Btn_Eliminar.AlternateText.Equals("Eliminar_Movimiento")) 
            //{
            //    Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //    Clave.P_Detalle_ID = Grid_Movimientos.SelectedRow.Cells[1].Text;
            //    Clave.Eliminar_Detalle_Clave_Ingreso();
            //    Grid_Movimientos.SelectedIndex = 0;
            //    Llenar_Tabla_Movimientos(Grid_Movimientos.PageIndex);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('El Detalle de la Clave de Ingreso fue eliminada exitosamente');", true);
            //    //Limpiar_Catalogo();
            //}
            //if (Grid_Otros_Pagos.Rows.Count > 0 && Btn_Eliminar.AlternateText.Equals("Eliminar_Otro_Pago"))
            //{
            //    Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //    Clave.P_Detalle_ID = Grid_Otros_Pagos.SelectedRow.Cells[1].Text;
            //    Clave.Eliminar_Detalle_Clave_Ingreso();
            //    Grid_Otros_Pagos.SelectedIndex = 0;
            //    Llenar_Tabla_Otros_Pagos(Grid_Otros_Pagos.PageIndex);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('El Detalle de la Clave de Ingreso fue eliminada exitosamente');", true);
            //    //Limpiar_Catalogo();
            //}
            //if (Grid_Documentos.Rows.Count > 0 && Btn_Eliminar.AlternateText.Equals("Eliminar_Documento"))
            //{
            //    Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //    Clave.P_Detalle_ID = Grid_Documentos.SelectedRow.Cells[1].Text;
            //    Clave.Eliminar_Detalle_Clave_Ingreso();
            //    Grid_Documentos.SelectedIndex = 0;
            //    Llenar_Tabla_Documentos(Grid_Documentos.PageIndex);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('El Detalle de la Clave de Ingreso fue eliminada exitosamente');", true);
            //    //Limpiar_Catalogo();
            //}
            //if (Grid_Gastos_Ejecucion.Rows.Count > 0 && Btn_Eliminar.AlternateText.Equals("Eliminar_Gastos"))
            //{
            //    Cls_Cat_Pre_Claves_Ingreso_Negocio Clave = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            //    Clave.P_Detalle_ID = Grid_Gastos_Ejecucion.SelectedRow.Cells[1].Text;
            //    Clave.Eliminar_Detalle_Clave_Ingreso();
            //    Grid_Gastos_Ejecucion.SelectedIndex = 0;
            //    Llenar_Tabla_Gastos_Ejecucion(Grid_Gastos_Ejecucion.PageIndex);
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Claves de Ingreso", "alert('El Detalle de la Clave de Ingreso fue eliminada exitosamente');", true);
            //    //Limpiar_Catalogo();
            //}
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
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
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Session["Asignacion_Movimiento"] = null;
                Session["Asignacion_Otros_Pagos"] = null;
                Session["Asignacion_Documentos"] = null;
                Session["Asignacion_Gastos_Ejecucion"] = null;
                Session["Asignacion_Predial_Traslado"] = null;
                
                Grid_Movimientos.DataBind();
                Grid_Gastos_Ejecucion.DataBind();
                Grid_Otros_Pagos.DataBind();
                Grid_Predial_Traslado.DataBind();
                Grid_Documentos.DataBind();
                
                //Ocultar

                //Tab_Grupos_Movimientos.Enabled = false;
                //Tab_Otros_Pagos.Enabled = false;
                //Tab_Documentos.Enabled = false;
                //Tab_Gastos_Ejecucion.Enabled = false;
                //Tab_Predial_Traslado.Enabled = false;
                //Limpiar_Catalogo();
                Txt_Clave_Ingreso_ID.Text = "";
                Txt_Busqueda.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Cmb_Rama.SelectedIndex = 0;
                Cmb_Grupo.SelectedIndex = 0;
                Txt_Clave.Text = "";
                Cmb_Unidad_Responsable.SelectedIndex = 0;
                Cmb_Cuenta_Contable.SelectedIndex = 0;
                Txt_Descripcion.Text = "";
                Txt_Fundamento.Text = "";
                Configuracion_Formulario(true);
                //cerrar todas las sesiones

                Cmb_Documentos.SelectedIndex = 0;
                Cmb_Gastos_Ejecucion.SelectedIndex = 0;
                Cmb_Otros_Pagos.SelectedIndex = 0;
                //Cmb_Movimiento.SelectedIndex = 0;
                Cmb_Tipo.SelectedIndex = 0;
                Cmb_Tipo_Predial_Traslado.SelectedIndex = 0;
                //Costos
                Txt_Anio.Text = "";
                Txt_Costo.Text = "";

                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Claves_Ingreso.Enabled = true;
                Tab_Claves_Ingreso.ActiveTabIndex = 0;
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
    ///NOMBRE DE LA FUNCIÓN: Borrar_Registro
    ///DESCRIPCIÓN: Permite borrar un registro del grid y elimina el registro en la base de datos
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 06/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Borrar_Registro(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName.Equals("Erase_Otro_Pago"))
        {
            DataTable Dt_Otros_Pagos = (DataTable)Session["Dt_Otros_Pagos"];
            DataTable Dt_Asignacion_Otros_Pagos = (DataTable)Session["Asignacion_Otros_Pagos"];
            if (Session["Dt_Otros_Pagos"] != null)
            {
                Int32 Registro = ((Grid_Otros_Pagos.PageIndex) *
                Grid_Otros_Pagos.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Otros_Pagos.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Otros_Pagos"];
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Eliminar = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Eliminar.P_Clave_Ingreso_ID = Grid_Otros_Pagos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                    Eliminar.P_Pago_ID = Grid_Otros_Pagos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text;
                    Tabla.Rows.RemoveAt(Registro);
                    Eliminar.Eliminar_Detalle_Otro_Pago();
                    //Cmb_Colonia.SelectedIndex = 0;
                    //Llenar_Combo_Colonias();
                    //Cmb_Colonia.DataBind();
                    Session["Dt_Otros_Pagos"] = Tabla;
                    //Grid_Colonias_Sectores.Columns[0].Visible = true;
                    Grid_Otros_Pagos.Columns[1].Visible = true;
                    Grid_Otros_Pagos.Columns[2].Visible = true;
                    Grid_Otros_Pagos.PageIndex = 0;
                    Grid_Otros_Pagos.DataSource = Tabla;
                    Grid_Otros_Pagos.DataBind();
                    //Grid_Colonias_Sectores.Columns[0].Visible = false;
                    Grid_Otros_Pagos.Columns[1].Visible = false;
                    Grid_Otros_Pagos.Columns[2].Visible = false;
                }
            }
            else if (Session["Asignacion_Otros_Pagos"] != null)
            {
                Int32 Registro = ((Grid_Otros_Pagos.PageIndex) *
                Grid_Otros_Pagos.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Asignacion_Otros_Pagos.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Asignacion_Otros_Pagos"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Asignacion_Otros_Pagos"] = Tabla;
                    //Grid_Colonias_Sectores.Columns[0].Visible = true;
                    Grid_Otros_Pagos.Columns[1].Visible = true;
                    Grid_Otros_Pagos.Columns[2].Visible = true;
                    Grid_Otros_Pagos.PageIndex = 0;
                    Grid_Otros_Pagos.DataSource = Tabla;
                    Grid_Otros_Pagos.DataBind();
                    //Grid_Colonias_Sectores.Columns[0].Visible = false;
                    Grid_Otros_Pagos.Columns[1].Visible = false;
                    Grid_Otros_Pagos.Columns[2].Visible = true;
                }
            }
        }
        if (e.CommandName.Equals("Erase_Documento"))
        {
            DataTable Dt_Documentos = (DataTable)Session["Dt_Documentos"];
            DataTable Dt_Asignacion_Documentos = (DataTable)Session["Asignacion_Documentos"];
            if (Session["Dt_Documentos"] != null)
            {
                Int32 Registro = ((Grid_Documentos.PageIndex) *
                Grid_Documentos.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Documentos.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Documentos"];
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Eliminar = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Eliminar.P_Clave_Ingreso_ID = Grid_Documentos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                    Eliminar.P_Documento_ID = Grid_Documentos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text;
                    Tabla.Rows.RemoveAt(Registro);
                    Eliminar.Eliminar_Detalle_Documento();
                    //Cmb_Colonia.SelectedIndex = 0;
                    //Llenar_Combo_Colonias();
                    //Cmb_Colonia.DataBind();
                    Session["Dt_Documentos"] = Tabla;
                    Grid_Documentos.Columns[1].Visible = true;
                    Grid_Documentos.Columns[4].Visible = true;
                    Grid_Documentos.PageIndex = 0;
                    Grid_Documentos.DataSource = Tabla;
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[1].Visible = false;
                    Grid_Documentos.Columns[4].Visible = false;
                }
            }
            else if (Session["Asignacion_Documentos"] != null)
            {
                Int32 Registro = ((Grid_Documentos.PageIndex) *
                Grid_Documentos.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Asignacion_Documentos.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Asignacion_Documentos"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Asignacion_Documentos"] = Tabla;
                    Grid_Documentos.Columns[1].Visible = true;
                    Grid_Documentos.Columns[4].Visible = true;
                    Grid_Documentos.PageIndex = 0;
                    Grid_Documentos.DataSource = Tabla;
                    Grid_Documentos.DataBind();
                    Grid_Documentos.Columns[1].Visible = false;
                    Grid_Documentos.Columns[4].Visible = false;
                }
            }
        }
        if (e.CommandName.Equals("Erase_Movimiento"))
        {
            DataTable Dt_Movimientos = (DataTable)Session["Dt_Movimientos"];
            DataTable Dt_Asignacion_Movimiento = (DataTable)Session["Asignacion_Movimiento"];
            if (Session["Dt_Movimientos"] != null)
            {
                Int32 Registro = ((Grid_Movimientos.PageIndex) *
                Grid_Movimientos.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Movimientos.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Movimientos"];
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Eliminar = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Eliminar.P_Costo_Clave_ID = Grid_Movimientos.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                    Tabla.Rows.RemoveAt(Registro);
                    Eliminar.Eliminar_Costo_Clave();
                    //Cmb_Colonia.SelectedIndex = 0;
                    //Llenar_Combo_Colonias();
                    //Cmb_Colonia.DataBind();
                    Session["Dt_Movimientos"] = Tabla;
                    Grid_Movimientos.Columns[1].Visible = true;
                    Grid_Movimientos.Columns[0].Visible = true;
                    ////Grid_Movimientos.Columns[2].Visible = true;
                    Grid_Movimientos.PageIndex = 0;
                    Grid_Movimientos.DataSource = Tabla;
                    Grid_Movimientos.DataBind();
                    //Grid_Movimientos.Columns[2].Visible = false;
                    Grid_Movimientos.Columns[1].Visible = false;
                    Grid_Movimientos.Columns[0].Visible = false;
                }
            }
            else if (Session["Asignacion_Movimiento"] != null)
            {
                Int32 Registro = ((Grid_Movimientos.PageIndex) *
                Grid_Movimientos.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Asignacion_Movimiento.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Asignacion_Movimiento"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Asignacion_Movimiento"] = Tabla;
                    //Grid_Movimientos.Columns[2].Visible = true;
                    Grid_Movimientos.Columns[1].Visible = true;
                    Grid_Movimientos.Columns[0].Visible = true;
                    Grid_Movimientos.PageIndex = 0;
                    Grid_Movimientos.DataSource = Tabla;
                    Grid_Movimientos.DataBind();
                    //Grid_Movimientos.Columns[2].Visible = false;
                    Grid_Movimientos.Columns[1].Visible = false;
                    Grid_Movimientos.Columns[0].Visible = false;
                }
            }
        }
        if (e.CommandName.Equals("Erase_Gasto"))
        {
            DataTable Dt_Gastos = (DataTable)Session["Dt_Gastos"];
            DataTable Dt_Asignacion_Gastos_Ejecucion = (DataTable)Session["Asignacion_Gastos_Ejecucion"];
            if (Session["Dt_Gastos"] != null)
            {
                Int32 Registro = ((Grid_Gastos_Ejecucion.PageIndex) *
                Grid_Gastos_Ejecucion.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Gastos.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Gastos"];
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Eliminar = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Eliminar.P_Clave_Ingreso_ID = Grid_Gastos_Ejecucion.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                    Eliminar.P_Gasto_ID = Grid_Gastos_Ejecucion.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text;
                    Tabla.Rows.RemoveAt(Registro);
                    Eliminar.Eliminar_Detalle_Gasto();
                    //Cmb_Colonia.SelectedIndex = 0;
                    //Llenar_Combo_Colonias();
                    //Cmb_Colonia.DataBind();
                    Session["Dt_Gastos"] = Tabla;
                    Grid_Gastos_Ejecucion.Columns[2].Visible = true;
                    Grid_Gastos_Ejecucion.Columns[1].Visible = true;
                    Grid_Gastos_Ejecucion.PageIndex = 0;
                    Grid_Gastos_Ejecucion.DataSource = Tabla;
                    Grid_Gastos_Ejecucion.DataBind();
                    Grid_Gastos_Ejecucion.Columns[2].Visible = false;
                    Grid_Gastos_Ejecucion.Columns[1].Visible = false;
                }
            }
            else if (Session["Asignacion_Gastos_Ejecucion"] != null)
            {
                Int32 Registro = ((Grid_Gastos_Ejecucion.PageIndex) *
                Grid_Gastos_Ejecucion.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Asignacion_Gastos_Ejecucion.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Asignacion_Gastos_Ejecucion"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Asignacion_Gastos_Ejecucion"] = Tabla;
                    Grid_Gastos_Ejecucion.Columns[2].Visible = true;
                    Grid_Gastos_Ejecucion.Columns[1].Visible = true;
                    Grid_Gastos_Ejecucion.PageIndex = 0;
                    Grid_Gastos_Ejecucion.DataSource = Tabla;
                    Grid_Gastos_Ejecucion.DataBind();
                    Grid_Gastos_Ejecucion.Columns[2].Visible = false;
                    Grid_Gastos_Ejecucion.Columns[1].Visible = false;
                }
            }
        }
        if (e.CommandName.Equals("Erase_Predial"))
        {
            DataTable Dt_Predial = (DataTable)Session["Dt_Predial"];
            DataTable Dt_Asignacion_Predial = (DataTable)Session["Asignacion_Predial"];
            if (Session["Dt_Predial"] != null)
            {
                Int32 Registro = ((Grid_Predial_Traslado.PageIndex) *
                Grid_Predial_Traslado.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Predial.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Predial"];
                    Cls_Cat_Pre_Claves_Ingreso_Negocio Eliminar = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
                    Eliminar.P_Clave_Ingreso_ID = Grid_Predial_Traslado.Rows[Convert.ToInt32(e.CommandArgument)].Cells[1].Text;
                    Eliminar.P_Tipo = Grid_Predial_Traslado.Rows[Convert.ToInt32(e.CommandArgument)].Cells[2].Text;
                    Eliminar.P_Tipo_Predial_Traslado = Grid_Predial_Traslado.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text;
                    Tabla.Rows.RemoveAt(Registro);
                    Eliminar.Eliminar_Detalle_Predial_Traslado();
                    //Cmb_Colonia.SelectedIndex = 0;
                    //Llenar_Combo_Colonias();
                    //Cmb_Colonia.DataBind();
                    Session["Dt_Predial"] = Tabla;
                    Grid_Predial_Traslado.Columns[1].Visible = true;
                    Grid_Predial_Traslado.PageIndex = 0;
                    Grid_Predial_Traslado.DataSource = Tabla;
                    Grid_Predial_Traslado.DataBind();
                    Grid_Predial_Traslado.Columns[1].Visible = false;
                }
            }
            else if (Session["Asignacion_Predial"] != null)
            {
                Int32 Registro = ((Grid_Predial_Traslado.PageIndex) *
                Grid_Predial_Traslado.PageSize) + (Convert.ToInt32(e.CommandArgument));

                if (Dt_Asignacion_Predial.Rows.Count > 0)
                {
                    DataTable Tabla = (DataTable)Session["Asignacion_Predial"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Asignacion_Predial"] = Tabla;
                    Grid_Predial_Traslado.Columns[1].Visible = true;
                    Grid_Predial_Traslado.PageIndex = 0;
                    Grid_Predial_Traslado.DataSource = Tabla;
                    Grid_Predial_Traslado.DataBind();
                    Grid_Predial_Traslado.Columns[1].Visible = false;
                }
            }
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
            Botones.Add(Btn_Buscar);

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