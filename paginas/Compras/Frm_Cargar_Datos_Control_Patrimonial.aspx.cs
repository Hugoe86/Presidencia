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
using System.Data.Common;
using System.Windows.Forms;
using System.Data.OleDb;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using Presidencia.Control_Patrimonial_Catalogo_Zonas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Vehiculo.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Siniestros.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Bajas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Alimentacion.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Colores.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Materiales.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Ascendencia.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Razas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Vacunas.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Veterinarios.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Adiestramiento.Negocio;

using Presidencia.Control_Patrimonial_Catalogo_Tipos_Combustible.Negocio;
public partial class paginas_Compras_Frm_Cargar_Datos_Control_Patrimonial : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }

    }

    #region "Metodos"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Leer_Excel
    ///DESCRIPCIÓN:          Metodo que consulta un archivo EXCEL
    ///PARAMETROS:           String sqlExcel.- string que contiene el select
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           23/Mayo/2011 
    ///MODIFICO:             Salvador Hernández Ramírez
    ///FECHA_MODIFICO:       26/Mayo/2011 
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataSet Leer_Excel(String sqlExcel)
    {
        //Para empezar definimos la conexión OleDb a nuestro fichero Excel.
        String Rta = @MapPath("../../Archivos/Datos_Control_Patrimonial.xls");
        string sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Rta + ";Extended Properties=Excel 8.0;";

        //Definimos el DataSet donde insertaremos los datos que leemos del excel
        DataSet DS = new DataSet();

        //Definimos la conexión OleDb al fichero Excel y la abrimos
        OleDbConnection oledbConn = new OleDbConnection(sConnectionString);
        oledbConn.Open();

        //Creamos un comand para ejecutar la sentencia SELECT.
        OleDbCommand oledbCmd = new OleDbCommand(sqlExcel, oledbConn);

        //Creamos un dataAdapter para leer los datos y asocialor al DataSet.
        OleDbDataAdapter da = new OleDbDataAdapter(oledbCmd);
        da.Fill(DS);
        return DS;
    }

    #endregion


    #region "Eventos"

    protected void Btn_Aseguradoras_Click(object sender, EventArgs e)
    {
        DataSet Ds_Aseguradoras = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Aseguradoras$]";
        Ds_Aseguradoras = Leer_Excel(SqlExcel);
        DataTable Aseguradoras = Ds_Aseguradoras.Tables[0];
        for (int cnt = 0; cnt < Aseguradoras.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
            Aseguradora.P_Nombre = Aseguradoras.Rows[cnt]["NOMBRE"].ToString();
            Aseguradora.P_Nombre_Fiscal = Aseguradoras.Rows[cnt]["NOMBRE_FISCAL"].ToString();
            Aseguradora.P_Nombre_Comercial = Aseguradoras.Rows[cnt]["NOMBRE_COMERCIAL"].ToString();
            Aseguradora.P_RFC = Aseguradoras.Rows[cnt]["RFC"].ToString();
            Aseguradora.P_Cuenta_Contable = Aseguradoras.Rows[cnt]["Cuenta Contable"].ToString();
            Aseguradora.P_Estatus = Aseguradoras.Rows[cnt]["ESTATUS"].ToString();
            Aseguradora.P_Usuario = "CONTEL";
            Aseguradora.Alta_Aseguradora();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Procedencias_Click(object sender, EventArgs e)
    {
        DataSet Ds_Procedencias = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Procedencias$]";
        Ds_Procedencias = Leer_Excel(SqlExcel);
        DataTable Procedencias = Ds_Procedencias.Tables[0];
        for (int cnt = 0; cnt < Procedencias.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Procedencias_Negocio Procedencia = new Cls_Cat_Pat_Com_Procedencias_Negocio();
            Procedencia.P_Nombre = Procedencias.Rows[cnt]["NOMBRE"].ToString();
            Procedencia.P_Estatus = Procedencias.Rows[cnt]["ESTATUS"].ToString();
            Procedencia.P_Usuario = "CONTEL";
            Procedencia.Alta_Procedencia();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Materiales_Click(object sender, EventArgs e)
    {
        DataSet Ds_Materiales = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Materiales$]";
        Ds_Materiales = Leer_Excel(SqlExcel);
        DataTable Materiales = Ds_Materiales.Tables[0];
        for (int cnt = 0; cnt < Materiales.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Materiales_Negocio Material = new Cls_Cat_Pat_Com_Materiales_Negocio();
            Material.P_Descripcion = Materiales.Rows[cnt]["DESCRIPCION"].ToString();
            Material.P_Estatus = Materiales.Rows[cnt]["ESTATUS"].ToString();
            Material.P_Usuario = "CONTEL";
            Material.Alta_Material();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Colores_Click(object sender, EventArgs e)
    {
        DataSet Ds_Colores = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Colores$]";
        Ds_Colores = Leer_Excel(SqlExcel);
        DataTable Colores = Ds_Colores.Tables[0];
        for (int cnt = 0; cnt < Colores.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Colores_Negocio Color = new Cls_Cat_Pat_Com_Colores_Negocio();
            Color.P_Descripcion = Colores.Rows[cnt]["DESCRIPCION"].ToString();
            Color.P_Estatus = Colores.Rows[cnt]["ESTATUS"].ToString();
            Color.P_Usuario = "CONTEL";
            Color.Alta_Color();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Tipos_Alimentacion_Click(object sender, EventArgs e)
    {
        DataSet Tipos_Alimentacion = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [TiposAlimentación$]";
        Tipos_Alimentacion = Leer_Excel(SqlExcel);
        DataTable Tipos = Tipos_Alimentacion.Tables[0];
        for (int cnt = 0; cnt < Tipos.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
            Tipo.P_Nombre = Tipos.Rows[cnt]["NOMBRE"].ToString();
            Tipo.P_Estatus = Tipos.Rows[cnt]["ESTATUS"].ToString();
            Tipo.P_Usuario = "CONTEL";
            Tipo.Alta_Tipo_Alimentacion();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Tipos_Bajas_Click(object sender, EventArgs e)
    {
        DataSet Tipos_Bajas = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [TiposBajas$]";
        Tipos_Bajas = Leer_Excel(SqlExcel);
        DataTable Bajas = Tipos_Bajas.Tables[0];
        for (int cnt = 0; cnt < Bajas.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Tipos_Bajas_Negocio Baja = new Cls_Cat_Pat_Com_Tipos_Bajas_Negocio();
            Baja.P_Descripcion = Bajas.Rows[cnt]["DESCRIPCION"].ToString();
            Baja.P_Estatus = Bajas.Rows[cnt]["ESTATUS"].ToString();
            Baja.P_Usuario = "CONTEL";
            Baja.Alta_Tipo_Baja();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Tipos_Animales_Click(object sender, EventArgs e)
    {
        DataSet Tipos_Animales = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [TiposAnimales$]";
        Tipos_Animales = Leer_Excel(SqlExcel);
        DataTable Animales = Tipos_Animales.Tables[0];
        for (int cnt = 0; cnt < Animales.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Tipos_Cemovientes_Negocio Animal = new Cls_Cat_Pat_Com_Tipos_Cemovientes_Negocio();
            Animal.P_Nombre = Animales.Rows[cnt]["NOMBRE"].ToString();
            Animal.P_Estatus = Animales.Rows[cnt]["ESTATUS"].ToString();
            Animal.P_Usuario = "CONTEL";
            Animal.Alta_Tipo_Cemoviente();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Tipos_Combustibles_Click(object sender, EventArgs e)
    {
        DataSet Tipos_Combustibles = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [TiposCombustibles$]";
        Tipos_Combustibles = Leer_Excel(SqlExcel);
        DataTable Tipos = Tipos_Combustibles.Tables[0];
        for (int cnt = 0; cnt < Tipos.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Tipos_Combustible_Negocio Tipo = new Cls_Cat_Pat_Com_Tipos_Combustible_Negocio();
            Tipo.P_Descripcion = Tipos.Rows[cnt]["DESCRIPCION"].ToString();
            Tipo.P_Estatus = Tipos.Rows[cnt]["ESTATUS"].ToString();
            Tipo.P_Usuario = "CONTEL";
            Tipo.Alta_Tipo_Combustible();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Tipos_Siniestros_Click(object sender, EventArgs e)
    {
        DataSet Tipos_Siniestros = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [TiposSiniestros$]";
        Tipos_Siniestros = Leer_Excel(SqlExcel);
        DataTable Tipos = Tipos_Siniestros.Tables[0];
        for (int cnt = 0; cnt < Tipos.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Tipos_Siniestros_Negocio Tipo = new Cls_Cat_Pat_Com_Tipos_Siniestros_Negocio();
            Tipo.P_Descripcion = Tipos.Rows[cnt]["DESCRIPCION"].ToString();
            Tipo.P_Estatus = Tipos.Rows[cnt]["ESTATUS"].ToString();
            Tipo.P_Usuario = "CONTEL";
            Tipo.Alta_Tipo_Siniestro();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Tipos_Vehiculo_Click(object sender, EventArgs e)
    {
        DataSet Tipos_Vehiculo = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [TipoVehiculo$]";
        Tipos_Vehiculo = Leer_Excel(SqlExcel);
        DataTable Tipos = Tipos_Vehiculo.Tables[0];
        for (int cnt = 0; cnt < Tipos.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
            Tipo.P_Descripcion = Tipos.Rows[cnt]["DESCRIPCION"].ToString();
            Tipo.P_Estatus = Tipos.Rows[cnt]["ESTATUS"].ToString();
            Tipo.P_Aseguradora_ID = "00001";
            Tipo.P_Usuario = "CONTEL";
            Tipo.Alta_Tipo_Vehiculo();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    protected void Btn_Zonas_Click(object sender, EventArgs e)
    {
        DataSet Ds_Zonas = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Zonas$]";
        Ds_Zonas = Leer_Excel(SqlExcel);
        DataTable Zonas = Ds_Zonas.Tables[0];
        for (int cnt = 0; cnt < Zonas.Rows.Count; cnt++)
        {
            Cls_Cat_Pat_Com_Zonas_Negocio Zona = new Cls_Cat_Pat_Com_Zonas_Negocio();
            Zona.P_Descripcion = Zonas.Rows[cnt]["DESCRIPCION"].ToString();
            Zona.P_Estatus = Zonas.Rows[cnt]["ESTATUS"].ToString();
            Zona.P_Usuario = "CONTEL";
            Zona.Alta_Zona();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('exito!');", true);
    }

    #endregion
}
