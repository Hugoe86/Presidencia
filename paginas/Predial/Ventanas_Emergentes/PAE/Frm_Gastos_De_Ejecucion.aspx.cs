using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Gastos_Ejecucion.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Predial_Pae_Notificaciones.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Gastos_De_Ejecucion : System.Web.UI.Page
{
    string Cuenta_Predial;
    string Proceso;
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    { 
        if (!IsPostBack)
        {
            Cuenta_Predial = Request.QueryString["Cuenta_Predial"].ToString();
            Proceso = Request.QueryString["Proceso"].ToString();
            Session["AGREGA_GASTOS"] = false;
            Cargar_Combo_Estatus_Ejecucion();
            Cargar_Combo_Gastos_Ejecucion();
            Cargar_Notificacion();
        }
        Frm_Motivo_Omision.Page.Title = "Gastos de Ejecución";
        Lbl_Title.Text = "Gastos de Ejecución";
        Mensaje_Error();
    }
    #endregion
    #region Metodos   
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Estatus_Ejecucion
    ///DESCRIPCIÓN: Metodo usado para cargar el listado de los estatus de ejecucion
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 06/03/2012 12:22:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Estatus_Ejecucion()
    {
        try
        {
            Cmb_Estatus_Ejecucion.Items.Add(new ListItem("<-- SELECCIONE -->", "0"));
            Cmb_Estatus_Ejecucion.Items.Add(new ListItem("NOTIFICACION", "1"));
            Cmb_Estatus_Ejecucion.Items.Add(new ListItem("NO DILIGENCIADO", "2"));
            Cmb_Estatus_Ejecucion.Items.Add(new ListItem("ILOCALIZABLE", "3"));
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar el costo del gasto de ejecucion en un TextBox
    ///PARAMETROS: 
    ///CREO: Angel Antonio Escamilla Trejo 
    ///FECHA_CREO: 23/03/2012 04:53:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt_Gastos = (DataTable)Session["Gastos"];
            String Valor = Cmb_Gastos_Ejecucion.SelectedItem.Text;
            for (int Cont_Gastos = 0; Cont_Gastos < Dt_Gastos.Rows.Count; Cont_Gastos++)
            {
                if (Valor == Dt_Gastos.Rows[Cont_Gastos]["NOMBRE"].ToString() && Dt_Gastos.Rows[Cont_Gastos]["COSTO"].ToString() != null)
                {
                    Txt_Costo.Text = Dt_Gastos.Rows[Cont_Gastos]["COSTO"].ToString();
                    Txt_Costo.ReadOnly = true;
                    break;
                }
                else
                {
                    Txt_Costo.ReadOnly = false;
                    Txt_Costo.Text = "";
                    break;
                }

            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Gastos_Ejecucion
    ///DESCRIPCIÓN: Metodo usado para cargar el listado de los gastos de ejecucion
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 06/03/2012 12:22:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Gastos_Ejecucion()
    {
        DataTable Dt_Gastos = new DataTable();
        try
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Negocio Rs_Gastos = new Cls_Cat_Pre_Gastos_Ejecucion_Negocio();
            Rs_Gastos.P_Filtro = "";
            Cmb_Gastos_Ejecucion.DataTextField = Cat_Pre_Gastos_Ejecucion.Campo_Descripcion;
            Cmb_Gastos_Ejecucion.DataValueField = Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;

            Dt_Gastos = Rs_Gastos.Consultar_Gastos_Ejecucion();
            Session["Gastos"] = Dt_Gastos;
            foreach (DataRow Dr_Fila in Dt_Gastos.Rows)
            {
                if (Dr_Fila[Cat_Pre_Gastos_Ejecucion.Campo_Estatus].ToString() != "VIGENTE")//Busca el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Gastos_Ejecucion.DataSource = Dt_Gastos;
            Cmb_Gastos_Ejecucion.DataBind();
            Cmb_Gastos_Ejecucion.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Armando Zavala Moreno.
    ///FECHA_CREO  : 17-Abril-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Ecabezado_Mensaje.Text += P_Mensaje;// +"</br>";
        Div_Contenedor_Msj_Error.Visible = true;

    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Ecabezado_Mensaje.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Honorarios
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Honorarios()
    {
        DataTable Dt_Honorarios = new DataTable();
        Dt_Honorarios.Columns.Add(new DataColumn("GASTO_EJECUCION_ID", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("TIPO_DE_GASTO", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("IMPORTE", typeof(Decimal)));
        //Dt_Honorarios.Columns.Add(new DataColumn("FECHA_HONORARIO", typeof(String)));
        //Dt_Honorarios.Columns.Add(new DataColumn("PROCESO", typeof(String)));
        //Dt_Honorarios.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        return Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Notiticaciones
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las Notiticaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Notiticaciones()
    {
        DataTable Dt_Notificaciones = new DataTable();
        Dt_Notificaciones.Columns.Add(new DataColumn("FECHA_HORA", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("NOTIFICADOR", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("RECIBIO", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("ACUSE_RECIBIO", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("FOLIO", typeof(String)));
        //Dt_Notificaciones.Columns.Add(new DataColumn("PROCESO", typeof(String)));
        //Dt_Notificaciones.Columns.Add(new DataColumn("MEDIO_NOTIFICACION", typeof(String)));        
        return Dt_Notificaciones;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Publicaciones
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las Publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Publicaciones()
    {
        DataTable Dt_Publicaciones = new DataTable();
        Dt_Publicaciones.Columns.Add(new DataColumn("FECHA_PUBLICACION", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("MEDIO_PUBLICACION", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("PAGINA", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("TOMO", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("PARTE", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("FOJA", typeof(String)));        
        Dt_Publicaciones.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        //Dt_Publicaciones.Columns.Add(new DataColumn("PROCESO", typeof(String)));
        return Dt_Publicaciones;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Honorarios
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Honorarios(DataTable Dt_Honorarios)
    {
        DataRow Dr_Honorario;
        Dr_Honorario = Dt_Honorarios.NewRow();
        Dr_Honorario["GASTO_EJECUCION_ID"] = Cmb_Gastos_Ejecucion.SelectedValue;
        Dr_Honorario["TIPO_DE_GASTO"] = Cmb_Gastos_Ejecucion.SelectedItem.Text;        
        Dr_Honorario["IMPORTE"] = Txt_Costo.Text;
        Dt_Honorarios.Rows.Add(Dr_Honorario);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Publicaciones
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla Publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Publicaciones(DataTable Dt_Publicaciones)
    {
        DataRow Dr_Publicaciones;
        Dr_Publicaciones = Dt_Publicaciones.NewRow();
        Dr_Publicaciones["MEDIO_PUBLICACION"] = Txt_Publicacion.Text.ToUpper();
        Dr_Publicaciones["FECHA_PUBLICACION"] = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fecha_Publicacion.Text));
        Dr_Publicaciones["PAGINA"] = Txt_Pagina.Text.ToUpper();
        Dr_Publicaciones["TOMO"] = Txt_Tomo.Text.ToUpper();
        Dr_Publicaciones["PARTE"] = Txt_Parte.Text.ToUpper();
        Dr_Publicaciones["FOJA"] = Txt_Foja.Text.ToUpper();
        Dt_Publicaciones.Rows.Add(Dr_Publicaciones);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Notificaciones
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de notificaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Notificaciones(DataTable Dt_Notificaciones)
    {
        DataRow Dr_Notificaciones;
        DateTime Hora;
        DateTime.TryParse(Txt_Fecha.Text + " " + Txt_Hora.Text, out Hora);

        Dr_Notificaciones = Dt_Notificaciones.NewRow();
        Dr_Notificaciones["FECHA_HORA"] = Hora.ToString("dd/MM/yyyy HH:mm:ss");
        Dr_Notificaciones["ESTATUS"] = Cmb_Estatus_Ejecucion.SelectedItem.Text;
        Dr_Notificaciones["NOTIFICADOR"] = Txt_Notificador.Text.ToUpper();
        Dr_Notificaciones["RECIBIO"] = Txt_Recibio.Text.ToUpper();
        Dr_Notificaciones["ACUSE_RECIBIO"] = Txt_Acuse.Text.ToUpper();
        Dr_Notificaciones["FOLIO"] = Txt_Folio.Text.ToUpper();
        Dt_Notificaciones.Rows.Add(Dr_Notificaciones);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validadar_Campos
    ///DESCRIPCIÓN          : Valida los campos para agregar un gasto de ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 26/03/2012 05:18:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Validadar_Campos()
    {        
        Boolean Validacion = true;
        if (Txt_Fecha.Text.Length > 1 || Txt_Hora.Text.Length > 1 || Cmb_Estatus_Ejecucion.SelectedIndex > 1 || Txt_Notificador.Text.Length > 1
            || Txt_Recibio.Text.Length > 1 || Txt_Acuse.Text.Length > 1 || Txt_Folio.Text.Length > 1)
        { 
            if (Txt_Fecha.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce La Fecha de notificacion <br>";
                Validacion = false;
            }
            if (Txt_Hora.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce La Hora de notificacion <br>";
                Validacion = false;
            }
            if (Txt_Notificador.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce el notificador <br>";
                Validacion = false;
            }
            if (Cmb_Estatus_Ejecucion.SelectedIndex < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce el notificador <br>";
                Validacion = false;
            }
            if (Txt_Recibio.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce el nombre de quien recibio <br>";
                Validacion = false;
            }
            if (Txt_Acuse.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce el acuse de recibo <br>";
                Validacion = false;
            }
            if (Txt_Folio.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce el folio <br>";
                Validacion = false;
            }
        }
        if (Txt_Publicacion.Text.Length > 1 || Txt_Fecha_Publicacion.Text.Length > 1 || Txt_Tomo.Text.Length > 1 ||
            Txt_Parte.Text.Length > 1 || Txt_Foja.Text.Length > 1 || Txt_Pagina.Text.Length > 1)
        {
            if (Txt_Publicacion.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce una publicación <br>";
                Validacion = false;
            }
             if (Txt_Fecha_Publicacion.Text.Length < 1)
             {
                 Lbl_Ecabezado_Mensaje.Text += "Introduce una fecha de publicación <br>";
                 Validacion = false;
             }
             if (Txt_Tomo.Text.Length < 1)
             {
                 Lbl_Ecabezado_Mensaje.Text += "Introduce un tomo de publicacion <br>";
                 Validacion = false;
             }
             if (Txt_Parte.Text.Length < 1)
             {
                 Lbl_Ecabezado_Mensaje.Text += "Introduce la parte de publicacion <br>";
                 Validacion = false;
             }
             if (Txt_Foja.Text.Length < 1)
             {
                 Lbl_Ecabezado_Mensaje.Text += "Introduce la foja de publicacion <br>";
                 Validacion = false;
             }
             if (Txt_Pagina.Text.Length < 1)
             {
                 Lbl_Ecabezado_Mensaje.Text += "Introduce la pagina de publicacion <br>";
                 Validacion = false;
             }
        }
        if (Cmb_Gastos_Ejecucion.SelectedIndex > 1 || Txt_Costo.Text.Length > 1)
        {
            if (Cmb_Gastos_Ejecucion.SelectedIndex < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Selecciona un gasto de ejecucion <br>";
                Validacion = false;
            }
            if (Txt_Costo.Text.Length < 1)
            {
                Lbl_Ecabezado_Mensaje.Text += "Introduce un costo de ejecucion <br>";
                Validacion = false;
            }
        }
        if (Validacion != true)
            Div_Contenedor_Msj_Error.Visible = true;
        //return Validacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar_y_Guardar_Estatus
    ///DESCRIPCIÓN          : Valida los campos para agregar un estatus
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 26/03/2012 05:18:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private bool Validar_y_Guardar_Estatus()
    {
        Boolean Validacion = true;
        DataTable Dt_Notificaciones = null;
        DateTime Hora;
        if (Txt_Fecha.Enabled == true)
        {
            if (Txt_Fecha.Text.Length > 1 || Txt_Hora.Text.Length > 1 || Txt_Notificador.Text.Length > 1 || Txt_Recibio.Text.Length > 1 || Txt_Acuse.Text.Length > 1 || Txt_Folio.Text.Length > 1 || Cmb_Estatus_Ejecucion.SelectedIndex > 0)
            {
                if (Txt_Fecha.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un la fecha de notificación <br>";
                    Validacion = false;
                }
                if (Txt_Hora.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un la hora de notificación <br>";
                    Validacion = false;
                }
                else
                {
                    if (DateTime.TryParse(Txt_Fecha.Text + " " + Txt_Hora.Text, out Hora))
                    {

                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text += "Hora invalida <br>";
                        Validacion = false;
                    }
                }
                if (Txt_Notificador.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un notificador <br>";
                    Validacion = false;
                }
                //if (Txt_Recibio.Text.Length < 1)
                //{
                //    Lbl_Ecabezado_Mensaje.Text += "Introduce quien recibió <br>";
                //    Validacion = false;
                //}
                //if (Txt_Acuse.Text.Length < 1)
                //{
                //    Lbl_Ecabezado_Mensaje.Text += "Introduce el acuse de recibo <br>";
                //    Validacion = false;
                //}
                //if (Txt_Folio.Text.Length < 1)
                //{
                //    Lbl_Ecabezado_Mensaje.Text += "Introduce el folio";
                //    Validacion = false;
                //}

                if (Cmb_Estatus_Ejecucion.SelectedIndex < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Selecciona un estatus";
                    Validacion = false;
                }
                if (Validacion != false)
                {
                    if (Txt_Fecha.Text.Length > 1 && Txt_Hora.Text.Length > 1 && Txt_Notificador.Text.Length > 1)
                    {
                        Dt_Notificaciones = Crear_Tabla_Notiticaciones();
                        Llenar_DataRow_Notificaciones(Dt_Notificaciones);
                        Session["Notificaciones"] = Dt_Notificaciones;
                    }
                }
            }
        }
        return Validacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Notificacion
    ///DESCRIPCIÓN          : Se carga la configuracion del combo Notificaciones, para que
    ///                       no existan dos registros de notificacion,
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 17/04/2012 04:40:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Notificacion()
    {
        Cls_Ope_Pre_Pae_Notificaciones_Negocio Notificacion = new Cls_Ope_Pre_Pae_Notificaciones_Negocio();
        DataTable Dt_Notificaciones=new DataTable();
        Notificacion.P_Cuenta_predial = Cuenta_Predial;
        Notificacion.P_Proceso = Proceso;
        Dt_Notificaciones = Notificacion.Consulta_Notificacion_Cuenta_Predial();

        foreach (DataRow Dr_Fila in Dt_Notificaciones.Rows)
        {
            if (Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Estatus].ToString() == "NOTIFICACION")
            {
                Txt_Fecha.Enabled = false;
                Txt_Hora.Enabled = false;                
                Txt_Notificador.Enabled = false;
                Txt_Recibio.Enabled = false;
                Txt_Acuse.Enabled = false;
                Txt_Folio.Enabled = false;
                Cmb_Estatus_Ejecucion.Enabled = false;
                Txt_Fecha.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora].ToString().Substring(0, 10);
                Txt_Hora.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora].ToString().Substring(11, 13);
                Txt_Notificador.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Notificador].ToString();
                Txt_Recibio.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Recibio].ToString();
                Txt_Acuse.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Acuse_Recibo].ToString();
                Txt_Folio.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Folio].ToString();
                Cmb_Estatus_Ejecucion.SelectedIndex = 1;
            }
        }
    }
    #endregion
    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Gasto_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Agregar_Gasto_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Honorarios;
        Boolean Validacion=true;
        String Mensaje="";
        Mensaje_Error();
        if (Session["Honorarios"] != null)
        {
            Dt_Honorarios = (DataTable)Session["Honorarios"];
        }
        else
        {
            Dt_Honorarios = Crear_Tabla_Honorarios();
        }
        
        if (Cmb_Gastos_Ejecucion.SelectedIndex < 1)
        {
            Mensaje += "Selecciona un gasto de ejecucion <br>";
            Validacion = false;
        }
        if (Txt_Costo.Text.Length < 1)
        {
            Mensaje += "Introduce un costo de ejecucion";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Mensaje_Error(Mensaje);
        }
        else
        {
            Llenar_DataRow_Honorarios(Dt_Honorarios);
            Grid_Gastos_Ejecucion.DataSource = Dt_Honorarios;
            Grid_Gastos_Ejecucion.DataBind();
            Cargar_Combo_Gastos_Ejecucion();
            Txt_Costo.Text = "";
            Session["Honorarios"] = Dt_Honorarios;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Publicacion_Click
    ///DESCRIPCIÓN          : Llena el Grid de Las Publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 06/03/2012 05:25:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_Costo_Publicacion_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Publicaciones;
        Boolean Validacion=true;
        Mensaje_Error();

        if (Session["Publicaciones"] != null)
        {
            Dt_Publicaciones = (DataTable)Session["Publicaciones"];
        }
        else
        {
            Dt_Publicaciones = Crear_Tabla_Publicaciones();
        }
        if (Txt_Publicacion.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce una publicación <br>";
            Validacion = false;
        }
        if (Txt_Fecha_Publicacion.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce una fecha de publicación <br>";
            Validacion = false;
        }
        if (Txt_Tomo.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce un tomo de publicacion <br>";
            Validacion = false;
        }
        if (Txt_Parte.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce la parte de publicacion <br>";
            Validacion = false;
        }
        if (Txt_Foja.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce la foja de publicacion <br>";
            Validacion = false;
        }
        if (Txt_Pagina.Text.Length < 1)
        {
            Lbl_Ecabezado_Mensaje.Text += "Introduce la pagina de publicacion";
            Validacion = false;
        }
        if (Validacion == false)
        {
            IBtn_Imagen_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            Llenar_DataRow_Publicaciones(Dt_Publicaciones);
            Grid_Publicacion.DataSource = Dt_Publicaciones;
            Grid_Publicacion.DataBind();
            Session["Publicaciones"] = Dt_Publicaciones;
            Txt_Publicacion.Text = "";
            Txt_Fecha_Publicacion.Text = "";
            Txt_Parte.Text = "";
            Txt_Tomo.Text = "";
            Txt_Foja.Text = "";
            Txt_Pagina.Text = "";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Acepta el motivo de omision
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Honorarios = (DataTable)Session["Honorarios"];
        DataTable Dt_Publicaciones = (DataTable)Session["Publicaciones"];
        DataTable Dt_Notificaciones = null;
        Mensaje_Error();
        if (Validar_y_Guardar_Estatus())
        {
            Dt_Notificaciones = (DataTable)Session["Notificaciones"];
            //Si inserta y despues son borrados la tabla ya no es nula pero no tiene datos y marca error
            if (Dt_Honorarios != null && Dt_Honorarios.Rows.Count < 1) { Dt_Honorarios = null; }
            if (Dt_Publicaciones != null && Dt_Publicaciones.Rows.Count < 1) { Dt_Publicaciones = null; }
            if (Dt_Notificaciones != null && Dt_Notificaciones.Rows.Count < 1) { Dt_Notificaciones = null; }

            if (Dt_Honorarios != null || Dt_Publicaciones != null || Dt_Notificaciones != null)
            {
                Session["AGREGA_GASTOS"] = true;
                //Cierra la ventana
                string Pagina = "<script language='JavaScript'>";
                Pagina += "window.close();";
                Pagina += "</script>";
                ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
            }
            else
            {
                IBtn_Imagen_Error.Visible = true;
                Lbl_Ecabezado_Mensaje.Text = "No se introdujo ningun dato para guardar";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Hora_TextChanged
    ///DESCRIPCIÓN          : Valida la hora para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Hora_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Hora.Text != "")
        {
            if (DateTime.TryParse(Txt_Hora.Text, out Fecha_valida))
            {
                Txt_Hora.Text = Fecha_valida.ToString("hh:mm tt");
            }
            else
            {
                Txt_Hora.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha.Text, out Fecha_valida))
            {
                Txt_Fecha.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Publicacion_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Publicacion_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Publicacion.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Publicacion.Text, out Fecha_valida))
            {
                Txt_Fecha_Publicacion.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Publicacion.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["AGREGA_GASTOS"] = false;
        Session.Remove("Honorarios");
        Session.Remove("Notificaciones");
        Session.Remove("Publicaciones");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }    
    #endregion
    #region Grids
    
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Publicacion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Borra un Registro del Grid Publicacion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Publicacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Publicacion = (DataTable)Session["Publicaciones"];
        Dt_Publicacion.Rows.RemoveAt(Grid_Publicacion.SelectedIndex);
        Grid_Publicacion.DataSource = Dt_Publicacion;
        Grid_Publicacion.DataBind();
        Session["Publicaciones"] = Dt_Publicacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Publicacion_PageIndexChanging
    ///DESCRIPCIÓN          : Cambia de pagina en el Grid Publicacion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Publicacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Publicacion.PageIndex = e.NewPageIndex;
            Grid_Publicacion.DataSource = Session["Publicaciones"];
            Grid_Publicacion.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Borra un Registro del Grid Gastos Ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Honorarios = (DataTable)Session["Honorarios"];
        Dt_Honorarios.Rows.RemoveAt(Grid_Gastos_Ejecucion.SelectedIndex);
        Grid_Gastos_Ejecucion.DataSource = Dt_Honorarios;
        Grid_Gastos_Ejecucion.DataBind();
        Session["Honorarios"] = Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Gastos_Ejecucion_PageIndexChanging
    ///DESCRIPCIÓN          : Cambia de pagina en el Grid Gastos de ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Gastos_Ejecucion.PageIndex = e.NewPageIndex;
            Grid_Gastos_Ejecucion.DataSource = Session["Honorarios"];
            Grid_Gastos_Ejecucion.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion
}
