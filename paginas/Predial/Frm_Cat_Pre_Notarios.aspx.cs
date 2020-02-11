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
using Presidencia.Catalogo_Notarios.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_predial_Frm_Cat_Pre_Notarios : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Tabla_Notarios(0);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS          : 
    ///                     1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO
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
        Grid_Notarios.Enabled = Estatus;
        Grid_Notarios.SelectedIndex = (-1);
        Cmb_Estatus.Enabled = !Estatus;
        Cmb_Sexo.Enabled = !Estatus;
        Txt_Numero_Notaria.Enabled = !Estatus;
        Txt_Apellido_Materno.Enabled = !Estatus;
        Txt_Apellido_Paterno.Enabled = !Estatus;
        Txt_Calle.Enabled = !Estatus;
        Txt_Celular.Enabled = !Estatus;
        Txt_Ciudad.Enabled = !Estatus;
        Txt_Codigo_Postal.Enabled = !Estatus;
        Txt_Colonia.Enabled = !Estatus;
        Txt_CURP.Enabled = !Estatus;
        Txt_EMail.Enabled = !Estatus;
        Txt_Estado.Enabled = !Estatus;
        Txt_Fax.Enabled = !Estatus;
        Txt_No_Exterior.Enabled = !Estatus;
        Txt_No_Interior.Enabled = !Estatus;
        Txt_Nombre.Enabled = !Estatus;
        Txt_RFC.Enabled = !Estatus;
        Txt_Telefono.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Hdf_Notario.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Sexo.SelectedIndex = 0;
        Txt_Notario_ID.Text = "";
        Txt_Numero_Notaria.Text = "";
        Txt_Apellido_Paterno.Text = "";
        Txt_Apellido_Materno.Text = "";
        Txt_Calle.Text = "";
        Txt_Celular.Text = "";
        Txt_Ciudad.Text = "";
        Txt_Codigo_Postal.Text = "";
        Txt_Colonia.Text = "";
        Txt_CURP.Text = "";
        Txt_EMail.Text = "";
        Txt_Estado.Text = "";
        Txt_Fax.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Nombre.Text = "";
        Txt_RFC.Text = "";
        Txt_Telefono.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Notarios
    ///DESCRIPCIÓN          : Llena la tabla de Notarios con una consulta que puede o no tener Filtros.
    ///PARAMETROS          : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Ocubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Notarios(int Pagina)
    {
        try
        {
            //Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
            //Notario.P_Nombre = Txt_Busqueda.Text.Trim().ToUpper();
            //DataTable Tabla = Notario.Consultar_Notarios();
            //DataView Vista = new DataView(Tabla);
            //String Expresion_De_Busqueda = string.Format("{0} '%{1}%'", Grid_Notarios.SortExpression, Txt_Busqueda.Text.Trim().ToUpper());
            //Vista.RowFilter = "NOMBRE_COMPLETO LIKE " + Expresion_De_Busqueda;
            //Grid_Notarios.Columns[1].Visible = true;
            //Grid_Notarios.DataSource = Vista;
            //Grid_Notarios.PageIndex = Pagina;
            //Grid_Notarios.DataBind();
            //Grid_Notarios.Columns[1].Visible = false;

            Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Notario.P_Filtro_Dinamico = "";
                Notario.P_Filtro_Dinamico += Cat_Pre_Notarios.Campo_Nombre + " LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%' OR ";
                Notario.P_Filtro_Dinamico += Cat_Pre_Notarios.Campo_Numero_Notaria + " LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%'";
            }
            DataTable Tabla = Notario.Consultar_Notarios();
            Grid_Notarios.Columns[1].Visible = true;
            Grid_Notarios.DataSource = Tabla;
            Grid_Notarios.PageIndex = Pagina;
            Grid_Notarios.DataBind();
            Grid_Notarios.Columns[1].Visible = false;

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
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Numero_Notaria.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el número de notaría.";
            Validacion = false;
        }
        //Se comento debido a peticion de usuario
        //else // si hay un numero de notaria, validar que no se repita
        //{
        //    Int32 Notarios_Existentes = 0;
        //    Notarios_Existentes = Validar_Existe_Numero_Notaria(Txt_Numero_Notaria.Text.Trim(), Txt_Notario_ID.Text);
        //    if (Notarios_Existentes > 0)
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Ya existe un notario registrado con el número de notaría " + Txt_Numero_Notaria.Text.Trim() + ".";
        //        Validacion = false;
        //    }
        //}
        if (Txt_Apellido_Paterno.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Apellido Paterno.";
            Validacion = false;
        }
        //if (Txt_Apellido_Materno.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el Apellido Materno.";
        //    Validacion = false;
        //}
        if (Txt_Nombre.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre.";
            Validacion = false;
        }
        //if (Txt_RFC.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el RFC.";
        //    Validacion = false;
        //}
        //if (Txt_CURP.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el CURP.";
        //    Validacion = false;
        //}
        //if (Cmb_Sexo.SelectedIndex == 0)
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Sexo.";
        //    Validacion = false;
        //}
        //if (Txt_Estado.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el Estado.";
        //    Validacion = false;
        //}
        //if (Txt_Ciudad.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir la Ciudad.";
        //    Validacion = false;
        //}
        //if (Txt_Colonia.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir la Colonia.";
        //    Validacion = false;
        //}
        //if (Txt_Calle.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir la Calle.";
        //    Validacion = false;
        //}
        //if (Txt_Codigo_Postal.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el Codigo Postal.";
        //    Validacion = false;
        //}
        //if (Txt_No_Exterior.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el N&uacute;mero Exterior.";
        //    Validacion = false;
        //}
        //if (Txt_Telefono.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el Tel&eacute;fono.";
        //    Validacion = false;
        //}
        //if (Txt_Celular.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el Celular.";
        //    Validacion = false;
        //}
        //if (Txt_EMail.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Introducir el E-Mail.";
        //    Validacion = false;
        //}
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Existe_Numero_Notaria
    /// DESCRIPCIÓN: Buscar en la base de datos el numero de notaria dado y si se especifica un 
    ///             ID de notario, excluir de la búsqueda
    ///             Regresa el número de notarios encontrados
    /// PARÁMETROS:
    /// 		1. Numero_Notaria: Numero de notaria a validar
    /// 		2. Notario_ID: id del notario (por si se esta modificando)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Validar_Existe_Numero_Notaria(String Numero_Notaria, String Notario_ID)
    {
        Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
        DataTable Dt_Notarios;
        Int32 Notarios_Encontrados = 0;
        Int32 No_Notaria = 0;
        String Filtros_SQL = "";

        Int32.TryParse(Numero_Notaria, out No_Notaria);

        // formar filtro dinamico
        Filtros_SQL = "TO_NUMBER(" + Cat_Pre_Notarios.Campo_Numero_Notaria + ") = " + No_Notaria;
        // si se especifico id de notario, excluir este id (validar al modificar un notario)
        if (!String.IsNullOrEmpty(Notario_ID))
        {
            Filtros_SQL += " AND " + Cat_Pre_Notarios.Campo_Notario_ID + " != '" + Notario_ID + "'";
        }

        try
        {
            // consultar la base de datos
            Notario.P_Filtro_Dinamico = Filtros_SQL;
            Dt_Notarios = Notario.Consultar_Notarios();
            // si se obtuvieron resultados
            if (Dt_Notarios != null && Dt_Notarios.Rows.Count > 0)
            {
                Notarios_Encontrados = Dt_Notarios.Rows.Count;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Notarios_Encontrados;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Notarios_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Notarios
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Notarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Notarios.SelectedIndex = (-1);
            Llenar_Tabla_Notarios(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Notarios_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene los datos de un Notario Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO                 : Antonio Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Notarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Notarios.SelectedIndex > (-1))
            {
                String ID_Seleccionado = Grid_Notarios.SelectedRow.Cells[1].Text.Trim();
                Limpiar_Catalogo();
                Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
                Notario.P_Notario_ID = ID_Seleccionado;
                Notario = Notario.Consultar_Datos_Notario();
                Hdf_Notario.Value = Notario.P_Notario_ID;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Notario.P_Estatus));
                Cmb_Sexo.SelectedIndex = Cmb_Sexo.Items.IndexOf(Cmb_Sexo.Items.FindByValue(Notario.P_Sexo));
                Txt_Numero_Notaria.Text = Notario.P_Numero_Notaria;
                Txt_Apellido_Materno.Text = Notario.P_Apellido_Materno;
                Txt_Apellido_Paterno.Text = Notario.P_Apellido_Paterno;
                Txt_Calle.Text = Notario.P_Calle;
                Txt_Celular.Text = Notario.P_Celular;
                Txt_Ciudad.Text = Notario.P_Ciudad;
                Txt_Codigo_Postal.Text = Notario.P_Codigo_Postal;
                Txt_Colonia.Text = Notario.P_Colonia;
                Txt_CURP.Text = Notario.P_CURP;
                Txt_EMail.Text = Notario.P_E_Mail;
                Txt_Estado.Text = Notario.P_Estado;
                Txt_Fax.Text = Notario.P_Fax;
                Txt_Nombre.Text = Notario.P_Nombre;
                Txt_Notario_ID.Text = Notario.P_Notario_ID;
                Txt_No_Exterior.Text = Notario.P_Numero_Exterior;
                Txt_No_Interior.Text = Notario.P_Numero_Interior;
                Txt_RFC.Text = Notario.P_RFC;
                Txt_Telefono.Text = Notario.P_Telefono;
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Notario
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
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
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
                    Notario.P_Apellido_Materno = Txt_Apellido_Materno.Text.Trim().ToUpper();
                    Notario.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.Trim().ToUpper();
                    Notario.P_Numero_Notaria = Txt_Numero_Notaria.Text.Trim();
                    Notario.P_Calle = Txt_Calle.Text.Trim().ToUpper();
                    Notario.P_Celular = Txt_Celular.Text.Trim();
                    Notario.P_Ciudad = Txt_Ciudad.Text.Trim().ToUpper();
                    Notario.P_Codigo_Postal = Txt_Codigo_Postal.Text.Trim();
                    Notario.P_Colonia = Txt_Colonia.Text.Trim().ToUpper();
                    Notario.P_CURP = Txt_CURP.Text.Trim().ToUpper();
                    Notario.P_E_Mail = Txt_EMail.Text.Trim();
                    Notario.P_Estado = Txt_Estado.Text.Trim().ToUpper();
                    Notario.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Notario.P_Fax = Txt_Fax.Text.Trim();
                    Notario.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                    Notario.P_Numero_Exterior = Txt_No_Exterior.Text.Trim();
                    Notario.P_Numero_Interior = Txt_No_Interior.Text.Trim();
                    Notario.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                    Notario.P_Sexo = Cmb_Sexo.SelectedItem.Value;
                    Notario.P_Telefono = Txt_Telefono.Text.Trim();
                    Notario.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Notario.Alta_Notario())
                    {
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Notarios(Grid_Notarios.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Notarios", "alert('Alta de Notario Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Notarios", "alert('Alta de Notario No fue Exitosa');", true);
                    }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Notario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
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
                if (Grid_Notarios.Rows.Count > 0 && Grid_Notarios.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
                    Notario.P_Notario_ID = Hdf_Notario.Value;
                    Notario.P_Apellido_Materno = Txt_Apellido_Materno.Text.Trim().ToUpper();
                    Notario.P_Apellido_Paterno = Txt_Apellido_Paterno.Text.Trim().ToUpper();
                    Notario.P_Numero_Notaria = Txt_Numero_Notaria.Text.Trim();
                    Notario.P_Calle = Txt_Calle.Text.Trim().ToUpper();
                    Notario.P_Celular = Txt_Celular.Text.Trim();
                    Notario.P_Ciudad = Txt_Ciudad.Text.Trim().ToUpper();
                    Notario.P_Codigo_Postal = Txt_Codigo_Postal.Text.Trim();
                    Notario.P_Colonia = Txt_Colonia.Text.Trim().ToUpper();
                    Notario.P_CURP = Txt_CURP.Text.Trim().ToUpper();
                    Notario.P_E_Mail = Txt_EMail.Text.Trim();
                    Notario.P_Estado = Txt_Estado.Text.Trim().ToUpper();
                    Notario.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Notario.P_Fax = Txt_Fax.Text.Trim();
                    Notario.P_Nombre = Txt_Nombre.Text.Trim().ToUpper();
                    Notario.P_Numero_Exterior = Txt_No_Exterior.Text.Trim();
                    Notario.P_Numero_Interior = Txt_No_Interior.Text.Trim();
                    Notario.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                    Notario.P_Sexo = Cmb_Sexo.SelectedItem.Value;
                    Notario.P_Telefono = Txt_Telefono.Text.Trim();
                    Notario.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    if (Notario.Modificar_Notario())
                    {
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Tabla_Notarios(Grid_Notarios.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Notarios", "alert('Actualización Notario Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Notarios", "alert('Actualización Notario No fue Exitosa');", true);
                    }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Notario_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Notarios.SelectedIndex = (-1);
            Llenar_Tabla_Notarios(0);
            if (Grid_Notarios.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con Valor \"" + Txt_Busqueda.Text + "\" no se encotraron coincidencias";
                //Lbl_Mensaje_Error.Text = "(Se cargaron todos los notarios almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                //Txt_Busqueda.Text = "";
                //Llenar_Tabla_Notarios(0);
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    ///DESCRIPCIÓN          : Elimina un Notario de la Base de Datos
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Notarios.Rows.Count > 0 && Grid_Notarios.SelectedIndex > (-1))
            {
                Cls_Cat_Pre_Notarios_Negocio Notario = new Cls_Cat_Pre_Notarios_Negocio();
                Notario.P_Notario_ID = Grid_Notarios.SelectedRow.Cells[1].Text;
                if (Notario.Eliminar_Notario())
                {
                    Grid_Notarios.SelectedIndex = (-1);
                    Llenar_Tabla_Notarios(Grid_Notarios.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Notarios", "alert('El Notario fue Eliminado Exitosamente');", true);
                    Limpiar_Catalogo();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catálogo de Notarios", "alert('El Notario No fue Eliminado');", true);
                }
            }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    #endregion

}
