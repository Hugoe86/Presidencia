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
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Ayudante_CarlosAG;
using Presidencia.SUA.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Informar_SUA : System.Web.UI.Page
{
    #region (Init/Load)
    /// ************************************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Inicializa la configuración inicial de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) {
                Inicializa_Controles();
            }

            Lbl_Mensaje_Error.Text = String.Empty;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// 
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Consultar_SAP_Unidades_Responsables();//Consulta las unidades responsables que estan registradas en el sistema.
        }
        catch (Exception ex)
        {
            throw new Exception("Error al inicializar los controles de la página. Error: [" + ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Excel
    /// 
    /// DESCRIPCION : Genera el reporte en Excel.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Excel()
    {
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.
        DataTable Dt_Empleados = null;//Variable que almacenara el listado de empleados según los filtros proporcionados.

        try
        {
            //Consultamos los empleados.
            Dt_Empleados = Consultar_Empleados();
            Quitar_Guiones_Bajos_Cabecera(ref Dt_Empleados);

            Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Empleados);
            //Mandamos a imprimir el reporte en excel.
            Mostrar_Excel(Libro, "SUA.xls");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Quitar_Guiones_Bajos_Cabecera
    /// 
    /// DESCRIPCION : Quita los guines bajos de la cabecera de la tabla que se mostrara 
    ///               en el archivo de excel.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Quitar_Guiones_Bajos_Cabecera(ref DataTable Dt_Empleados)
    {
        try
        {
            if (Dt_Empleados is DataTable) {
                if (Dt_Empleados.Rows.Count > 0) {
                    foreach (DataColumn CABECERA in Dt_Empleados.Columns) {
                        CABECERA.ColumnName = CABECERA.ColumnName.Replace("_", " ");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al quitar los guiones bajos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consultas)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llenar_Grid_Busqueda_Empleados
    /// 
    /// DESCRIPCION : Consultar los empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Consultar_Empleados()
    {
        Cls_Rpt_Nom_Generacion_Arch_SUA_Negocio Negocio = new Cls_Rpt_Nom_Generacion_Arch_SUA_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara el listado de empleados según los filtros seleccionados.

        try
        {
            if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim(); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Empleado = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Empleado = Txt_Busqueda_Nombre_Empleado.Text.Trim(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Unidad_Responsable = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            if (Txt_Registro_Patronal.Text.Trim().Length > 0) { Negocio.P_Registro_Patronal = Txt_Registro_Patronal.Text.Trim(); }

            //Se ejecuta la consulta de empleados.
            Dt_Empleados = Negocio.Consultar_Empleados();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar a los empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_SAP_Unidades_Responsables
    /// 
    /// DESCRIPCION : Consulta los unidades responsables que existen actualmente 
    ///               registrados en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
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

    #region (Reporte)
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: Enero/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Empleados
    /// 
    /// DESCRIPCION : Carga el grid de empleados.
    ///               
    /// PARAMETROS  : No Aplica.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Empleados(DataTable Dt_Empleados)
    {
        try
        {
            Grid_Busqueda_Empleados.DataSource = Dt_Empleados;
            Grid_Busqueda_Empleados.DataBind();

            Grid_Busqueda_Empleados.SelectedIndex = (-1);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Grid de Empleados. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)
    /// ************************************************************************************************************************
    /// Nombre: Btn_Generar_Reporte_Excel_Click
    /// 
    /// Descripción: Método que lanza el reporte de para SUA.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    protected void Btn_Generar_Reporte_Excel_Click(object sender, EventArgs e)
    {
        try
        {
            Generar_Reporte_Excel();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// ************************************************************************************************************************
    /// Nombre: Btn_Busqueda_Empleados_Click
    /// 
    /// Descripción: Método que ejecuta la consulta de empleados.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Enero/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ************************************************************************************************************************ 
    protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleados = null;//Variable que almacenara el listado de empleados según los filtros proporcionados.

        try
        {
            Dt_Empleados = Consultar_Empleados();
            Cargar_Grid_Empleados(Dt_Empleados);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion
}
