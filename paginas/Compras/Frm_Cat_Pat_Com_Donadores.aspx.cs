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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes;
using Presidencia.Control_Patrimonial_Catalogo_Donadores.Negocio;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Cat_Pat_Com_Donadores : System.Web.UI.Page
{
    #region Variables
    Cls_Cat_Pat_Com_Donadores_Negocio Donadores = new Cls_Cat_Pat_Com_Donadores_Negocio();
    # endregion

    #region  Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            Consultar_Donadores();
            //Configuracion_Acceso("Frm_Cat_Pat_Com_Donadores.aspx");
        }
    }
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Donadores
    ///DESCRIPCIÓN:          Método utilizado para consultar los donadores que se encuentran guardados en la Base da Datos y mostrarlso en un DataGrid
    ///CREO:                 Salvador Hernándz Ramírez
    ///FECHA_CREO:           14/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Consultar_Donadores()
    {
        DataTable Table_Donadores = new DataTable();

        try
        {
            Table_Donadores = Donadores.Consultar_Donadores();

            if (Table_Donadores.Rows.Count > 0)
            {
                Grid_Donadores.DataSource = Table_Donadores;
                Grid_Donadores.DataBind();
                Session["Tabla_Donadores"] = Table_Donadores;
                Grid_Donadores.Visible = true;
                Lbl_Donadores.Visible = true;
                Div_Productos_Donados.Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
                Lbl_Mensaje_Error.Text = "No se encontraron donadores";
                Div_Contenedor_Msj_Error.Visible = true;
                Grid_Donadores.Visible = false;
                Lbl_Donadores.Visible = false;
            }

        } catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Datos_Donador
    ///DESCRIPCIÓN: Método utilizado para mostrar los productos donados por el donador ( Bienes Muebles, Vehiculos y Animales)
    ///PROPIEDADES: 1. Bienes_Muebles. Es la tabla que contiene los bienes muebles donados por el donador
    ///             2. Vehiculos.      Es la tabla que contiene los vehiculos donados por el donador
    ///             3. Animales        Es la tabla que contiene los animales donados por el donador
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 14/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Datos_Donador(DataTable Bienes_Muebles, DataTable Vehiculos, DataTable Animales)
    {
         try
         {
            DataTable DataTable_Productos_Donados = new DataTable();
            DataTable_Productos_Donados.Columns.Add("NOMBRE");
            DataTable_Productos_Donados.Columns.Add("CANTIDAD");
            DataTable_Productos_Donados.Columns.Add("FECHA_ADQUISICION");
            DataTable_Productos_Donados.Columns.Add("NUMERO_INVENTARIO");
            DataTable_Productos_Donados.Columns.Add("DEPENDENCIA");

            String Descripcion = "";
            String Cantidad = "";
            String Fecha_Adquisicion = "";
            String No_Inventario = "";
            String Dependencia = "";
            int j = 0;


            if (Bienes_Muebles.Rows.Count > 0)
            {
                for (int i = 0; i < Bienes_Muebles.Rows.Count; i++)
                {
                    Descripcion = Bienes_Muebles.Rows[i]["NOMBRE"].ToString().Trim();
                    Cantidad = Bienes_Muebles.Rows[i]["CANTIDAD"].ToString().Trim();
                    Fecha_Adquisicion = Bienes_Muebles.Rows[i]["FECHA_ADQUISICION"].ToString().Trim();
                    No_Inventario = Bienes_Muebles.Rows[i]["NUMERO_INVENTARIO"].ToString().Trim();
                    Dependencia = Bienes_Muebles.Rows[i]["DEPENDENCIA"].ToString().Trim();

                    DataRow Registro = DataTable_Productos_Donados.NewRow();
                    Registro["NOMBRE"] = Descripcion;
                    Registro["CANTIDAD"] = Cantidad;
                    Registro["FECHA_ADQUISICION"] = Fecha_Adquisicion;
                    Registro["NUMERO_INVENTARIO"] = No_Inventario;
                    Registro["DEPENDENCIA"] = Dependencia;

                    DataTable_Productos_Donados.Rows.InsertAt(Registro, j);
                    j++;
                }
            }

            if (Vehiculos.Rows.Count > 0)
            {
                for (int i = 0; i < Vehiculos.Rows.Count; i++)
                {
                    Descripcion = Vehiculos.Rows[i]["NOMBRE"].ToString().Trim();
                    Cantidad = Vehiculos.Rows[i]["CANTIDAD"].ToString().Trim();
                    Fecha_Adquisicion = Vehiculos.Rows[i]["FECHA_ADQUISICION"].ToString().Trim();
                    No_Inventario = Vehiculos.Rows[i]["NUMERO_INVENTARIO"].ToString().Trim();
                    Dependencia = Vehiculos.Rows[i]["DEPENDENCIA"].ToString().Trim();

                    DataRow Registro = DataTable_Productos_Donados.NewRow();
                    Registro["NOMBRE"] = Descripcion;
                    Registro["CANTIDAD"] = Cantidad;
                    Registro["FECHA_ADQUISICION"] = Fecha_Adquisicion;
                    Registro["NUMERO_INVENTARIO"] = No_Inventario;
                    Registro["DEPENDENCIA"] = Dependencia;

                    DataTable_Productos_Donados.Rows.InsertAt(Registro, j);
                    j++;
                }
            }

            if (Animales.Rows.Count > 0)
            {
                for (int i = 0; i < Animales.Rows.Count; i++)
                {
                    Descripcion = Animales.Rows[i]["NOMBRE"].ToString().Trim();
                    Cantidad = Animales.Rows[i]["CANTIDAD"].ToString().Trim();
                    Fecha_Adquisicion = Animales.Rows[i]["FECHA_ADQUISICION"].ToString().Trim();
                    No_Inventario = Animales.Rows[i]["NUMERO_INVENTARIO"].ToString().Trim();
                    Dependencia = Animales.Rows[i]["DEPENDENCIA"].ToString().Trim();

                    DataRow Registro = DataTable_Productos_Donados.NewRow();
                    Registro["NOMBRE"] = Descripcion;
                    Registro["CANTIDAD"] = Cantidad;
                    Registro["FECHA_ADQUISICION"] = Fecha_Adquisicion;
                    Registro["NUMERO_INVENTARIO"] = No_Inventario;
                    Registro["DEPENDENCIA"] = Dependencia;

                    DataTable_Productos_Donados.Rows.InsertAt(Registro, j);
                    j++;
                }
            }

            if (DataTable_Productos_Donados.Rows.Count > 0)
            {
                Grid_Productos_Donados.DataSource = DataTable_Productos_Donados;
                Grid_Productos_Donados.DataBind();
                Session["Tabla_Productos_Donados"] = DataTable_Productos_Donados;
                Div_Productos_Donados.Visible = true;
                Grid_Donadores.Visible = false;
                Lbl_Donadores.Visible = false;
                Div_Contenedor_Msj_Error.Visible = false;

                Donadores.P_Donador_ID = Session["Id_Donador"].ToString();
                Donadores = Donadores.Consultar_Datos_Donador();
                Txt_Nombre_Donador.Text = (Donadores.P_Nombre + " " + Donadores.P_Apellido_Paterno + " " + Donadores.P_Apellido_Materno);

                Txt_Direccion.Text = Donadores.P_Direccion.ToString();
                Txt_Curp.Text = Donadores.P_CURP.ToString();
                Txt_Estado.Text = Donadores.P_Estado.ToString();
                Txt_RFC.Text = Donadores.P_RFC.ToString();
                Txt_Celular.Text = Donadores.P_Celular.ToString();
                Txt_Telefono.Text = Donadores.P_Telefono.ToString();
                Cambiar_Boton(true);
            }
            else
            {
                Lbl_Mensaje_Error.Text = "No se encontraron productos del donador";
                Div_Contenedor_Msj_Error.Visible = true;
                Div_Productos_Donados.Visible = false;
                Grid_Donadores.Visible = true;
                Lbl_Donadores.Visible = true;
            }


        }catch (Exception Ex)
         {
             Lbl_Ecabezado_Mensaje.Text = Ex.Message;
             Lbl_Mensaje_Error.Text = "";
             Div_Contenedor_Msj_Error.Visible = true;
         }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cambiar_Boton
    ///DESCRIPCIÓN: Método utilizado para configurar los botones 
    ///PROPIEDADES: 1. Estatus. Variable que contiene un "True o false"
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 14/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Cambiar_Boton(Boolean Estatus)
    {
        if (Estatus)
        {
            Btn_Salir.AlternateText = "Atras";
            Btn_Imprimir.Visible = true;
            Btn_Imprimir_Excel.Visible = true;
            Mostrar_Busqueda(false);
        }
        else
        {
            Btn_Salir.AlternateText = "Salir";
            Btn_Imprimir.Visible = false;
            Btn_Imprimir_Excel.Visible = false;
            Mostrar_Busqueda(true);
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tablas
    ///DESCRIPCIÓN: Método utilizado para llenar las tablas que se van utilizar para mostrar el reporte en pantalla
    ///CREO: Salvador Hernándz Ramírez
    ///FECHA_CREO: 14/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Llenar_Tablas(String Formato)
    {
        // Se crea la tabla "Cabecera"
        DataTable DataTable_Cabecera = new DataTable();
        DataTable_Cabecera.Columns.Add("DONADOR_ID");
        DataTable_Cabecera.Columns.Add("DONADOR");
        DataTable_Cabecera.Columns.Add("DIRECCION");
        DataTable_Cabecera.Columns.Add("CIUDAD");
        DataTable_Cabecera.Columns.Add("ESTADO");
        DataTable_Cabecera.Columns.Add("CURP");
        DataTable_Cabecera.Columns.Add("RFC");
        DataTable_Cabecera.Columns.Add("TELEFONO");
        DataTable_Cabecera.Columns.Add("CELULAR");
        DataTable_Cabecera.Columns.Add("USUARIO_CREO");

        // Se crea la tabla "Detalles"
        DataTable DataTable_Detalles = new DataTable();
        DataTable_Detalles.Columns.Add("DONADOR_ID");
        DataTable_Detalles.Columns.Add("DESCRIPCION");
        DataTable_Detalles.Columns.Add("CANTIDAD");
        DataTable_Detalles.Columns.Add("FECHA_ADQUISICION");
        DataTable_Detalles.Columns.Add("NO_INVENTARIO");
        DataTable_Detalles.Columns.Add("DEPENDENCIA");

        try
        {
            if (Grid_Productos_Donados.Rows.Count > 0)
            {
                // Se llena la tabla Cabecera
                String Donador_Id = Session["Id_Donador"].ToString();
                String Donador = Txt_Nombre_Donador.Text.Trim();
                String Direccion = Txt_Direccion.Text.Trim();
                String Ciudad = Txt_Ciudad.Text.Trim();
                String Estado = Txt_Estado.Text.Trim();
                String Curp = Txt_Curp.Text.Trim();
                String Rfc = Txt_RFC.Text.Trim();
                String Telefono = Txt_Telefono.Text.Trim();
                String Celular = Txt_Celular.Text.Trim();
                String Usuario_Creo = Cls_Sessiones.Nombre_Empleado;

                DataRow Registro = DataTable_Cabecera.NewRow();
                Registro["DONADOR_ID"] = Donador_Id;
                Registro["DONADOR"] = Donador;
                Registro["DIRECCION"] = Direccion;
                Registro["CIUDAD"] = Ciudad;
                Registro["ESTADO"] = Estado;
                Registro["CURP"] = Curp;
                Registro["RFC"] = Rfc;
                Registro["TELEFONO"] = Telefono;
                Registro["CELULAR"] = Celular;
                Registro["USUARIO_CREO"] = Usuario_Creo;

                DataTable_Cabecera.Rows.InsertAt(Registro, 0);

                for (int i = 0; i < Grid_Productos_Donados.Rows.Count; i++)
                {
                    // Se llena la tabla de Detalles
                    String Donador_Id_Detalles = Session["Id_Donador"].ToString();
                    String Descripcion = HttpUtility.HtmlDecode(Grid_Productos_Donados.Rows[i].Cells[2].Text.ToString()); 
                    //String Cantidad = "" + Grid_Productos_Donados.Rows[i].Cells[1].Text.ToString();
                    String Fecha_Adquisicion = "" + Grid_Productos_Donados.Rows[i].Cells[1].Text.ToString();
                    String No_Inventario = "" + Grid_Productos_Donados.Rows[i].Cells[0].Text.ToString();
                    //String Dependencia = HttpUtility.HtmlDecode(Grid_Productos_Donados.Rows[i].Cells[3].Text.ToString());
                    DataRow Registro_Detalles = DataTable_Detalles.NewRow();
                    Registro_Detalles["DONADOR_ID"] = Donador_Id_Detalles.Trim();
                    Registro_Detalles["DESCRIPCION"] = Descripcion.Trim();
                    //Registro_Detalles["CANTIDAD"] = Cantidad.Trim();
                    Registro_Detalles["FECHA_ADQUISICION"] = Fecha_Adquisicion.Trim();
                    Registro_Detalles["NO_INVENTARIO"] = No_Inventario.Trim();
                    //Registro_Detalles["DEPENDENCIA"] = Dependencia.Trim();

                    DataTable_Detalles.Rows.InsertAt(Registro_Detalles, i);
                }
            }

            if ((DataTable_Detalles.Rows.Count > 0) && (DataTable_Cabecera.Rows.Count > 0))
            {
                Ds_Ope_Com_Donadores Ds_Donadores_Report = new Ds_Ope_Com_Donadores();
                Generar_Reporte(DataTable_Cabecera, DataTable_Detalles, Ds_Donadores_Report, Formato);
                Div_Contenedor_Msj_Error.Visible = false;
            }
            else
            {
                Lbl_Mensaje_Error.Text = "No se puede imprimir el reporte, inténtelo nuevamente";
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
    ///NOMBRE DE LA FUNCIÓN:Generar_Reporte
    ///DESCRIPCIÓN:         Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:          1.- Tabla_Cabecera.- Contiene la informacion de la cabecera del DataSet
    ///                     2.- Tabla_Detalles.- Contiene la informacion de los detalles del DataSet
    ///                     3.- Ds_Donadores_Report.- Es el DataSet que contiene los campos utilizados
    ///                     para mostrar el reporte en pantalla
    ///                     4.- Nombre_Reporte, contiene el nombre del reporte a mostrar en pantalla
    ///CREO:                Salvador Hernández Ramírez
    ///FECHA_CREO:          14/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Tabla_Cabecera, DataTable Tabla_Detalles, DataSet Ds_Donadores_Report, String Formato)
    {
        try
        {
            DataRow Renglon;
            String Ruta_Reporte_Crystal = "";
            String Nombre_Reporte_Generar = "";

            // Llenar la tabla "Cabecera" del Dataset
            Renglon = Tabla_Cabecera.Rows[0];
            Ds_Donadores_Report.Tables[1].ImportRow(Renglon); // Se llena la tabla del DataSet, con la información del reporte

            // Llenar los detalles del DataSet
            for (int Cont_Elementos = 0; Cont_Elementos < Tabla_Detalles.Rows.Count; Cont_Elementos++)
            {
                Renglon = Tabla_Detalles.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Donadores_Report.Tables[0].ImportRow(Renglon);
            }

            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Ope_Com_Donadores.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_Productos_Donados_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Donadores_Report, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Busqueda
    ///DESCRIPCIÓN:          Método utilizado para mostrar y ocultar los controles
    ///                      utilizados para realizar la búsqueda simple y abanzada
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           12/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Mostrar_Busqueda(Boolean Estatus)
    {
        Txt_Busqueda.Visible = Estatus;
        Btn_Buscar.Visible = Estatus;
        Lbl_Busqueda.Visible = Estatus;
    }

    #endregion


    #region Eventos

     

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento utilizado para redireccionar esta página a la página principal o para consultar donadores (Estado Inicial)
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 14/Febrero/2011 
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
            Consultar_Donadores();
            Cambiar_Boton(false);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:          Evento utilizado para mostrar el reporte en PDF de los donadores en pantalla
    ///PROPIEDADES:     
    ///CREO:                 Salvador Hernández Ramírez.
    ///FECHA_CREO:           14/Febrero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "PDF";
        Llenar_Tablas( Formato);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN: Evento utilizado para mostrar el reporte en excel de los donadores en pantalla
    ///PROPIEDADES:     
    ///CREO: Salvador Hernández Ramírez.
    ///FECHA_CREO: 18/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Formato = "Excel";
        Llenar_Tablas(Formato);
    }
        #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Productos_Donados_PageIndexChanged
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los productos donados
        ///PROPIEDADES:     
        ///CREO: Salvador Hernández Ramírez.
        ///FECHA_CREO: 14/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Productos_Donados_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_Productos_Donados.PageIndex = e.NewPageIndex;
            Grid_Productos_Donados.DataSource = (DataTable)Session["Tabla_Productos_Donados"];
            Grid_Productos_Donados.DataBind();
        }
        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Donadores_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el evento para llenar las siguientes páginas del grid con la informaciçon de la consulta
        ///PROPIEDADES:     
        ///CREO: Salvador Hernández Ramírez
        ///FECHA_CREO: 14/Febrero/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        protected void Grid_Donadores_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            Grid_Donadores.PageIndex = e.NewPageIndex;
            Grid_Donadores.DataSource = (DataTable)Session["Tabla_Donadores"];
            Grid_Donadores.DataBind();
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Donadores_SelectedIndexChanged
        ///DESCRIPCIÓN: Evento utilizado para obtener el Identificador del donador seleccionado por el usuario
        ///PROPIEDADES:     
        ///CREO: Salvador Hernández Ramírez.
        ///FECHA_CREO: 14/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Donadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow SelectedRow = Grid_Donadores.Rows[Grid_Donadores.SelectedIndex];//GridViewRow representa una fila individual de un control gridview
                String Id_Donador = Convert.ToString(SelectedRow.Cells[1].Text);
                Session["Id_Donador"] = Id_Donador;

                DataTable Bienes_Muebles = new DataTable();
                DataTable Vehiculos = new DataTable();
                DataTable Animales = new DataTable();

                Donadores.P_Tipo_DataTable = "BIENES_MUEBLES";
                Donadores.P_Donador_ID = Session["Id_Donador"].ToString();
                Bienes_Muebles = Donadores.Consultar_Productos_Donador();

                Donadores.P_Tipo_DataTable = "VEHICULOS";
                Donadores.P_Donador_ID = Session["Id_Donador"].ToString();
                Vehiculos = Donadores.Consultar_Productos_Donador();

                Donadores.P_Tipo_DataTable = "ANIMALES";
                Donadores.P_Donador_ID = Session["Id_Donador"].ToString();
                Animales = Donadores.Consultar_Productos_Donador();

                Mostrar_Datos_Donador(Bienes_Muebles, Vehiculos, Animales);

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

        // Evento utilizado para consultar los donadores en base a su nombre
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Donadores.P_Nombre = Txt_Busqueda.Text.Trim();
            }
            else
            {
                Donadores.P_Nombre = null;
            }
            Consultar_Donadores();
        }

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
