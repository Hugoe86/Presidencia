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
using Presidencia.Control_Patrimonial_Catalogo_Funciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Adiestramiento.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Combustible.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using Presidencia.Catalogo_Compras_Modelos.Negocio;
using Presidencia.Catalogo_Compras_Productos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Detalles_Vehiculos.Negocio;


public partial class paginas_Compras_Frm_Ope_Pat_Migrar_Bienes : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
    }

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
    public DataSet Leer_Excel(String sqlExcel, String Archivo)
    {
        //Para empezar definimos la conexión OleDb a nuestro fichero Excel.
        String Rta = @MapPath("../../Archivos/" + Archivo);
        String sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                           "Data Source=" + Rta + ";" +
                           "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";

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
        oledbConn.Close();
        return DS;
    }

    protected void Btn_Cargar_Actualizacion_Click(object sender, EventArgs e) {
        //try {
            DataSet Ds_Bienes_Muebles = new DataSet();
            String SqlExcel = "Select * From [ACTUALIZAR$]";
            Ds_Bienes_Muebles = Leer_Excel(SqlExcel, "ACTUALIZAR.xlsx");
            DataTable Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
            Int32 Cnt_Encontrados = 0;
            Int32 Cnt_No_Encontrados = 0;
            Int32 Cnt_Registros = 0;
            DataTable Dt_Encontrados = new DataTable();
            Cargar_Estructura_(ref Dt_Encontrados);
            DataTable Dt_No_Encontrados = new DataTable();
            Cargar_Estructura_(ref Dt_No_Encontrados);
            for (Int32 Contador = 0; Contador < Bienes_Muebles.Rows.Count; Contador++) {
                Cnt_Registros = Cnt_Registros + 1;
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Negocio.P_Bien_Mueble_ID = Convertir_A_Formato_ID(Convert.ToInt32(Bienes_Muebles.Rows[Contador]["CLAVE_ID"].ToString().Trim()),10);
                Negocio = Negocio.Consultar_Detalles_Bien_Mueble();
                if (Negocio.P_Bien_Mueble_ID != null && Negocio.P_Bien_Mueble_ID.Trim().Length > 0) {
                    if (Bienes_Muebles.Rows[Contador]["NUMERO_INVENTARIO"].ToString().Trim().Equals(Negocio.P_Numero_Inventario_Anterior.Trim())) {
                        Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Actualizar = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                        Actualizar.P_Bien_Mueble_ID = Convertir_A_Formato_ID(Convert.ToInt32(Bienes_Muebles.Rows[Contador]["CLAVE_ID"].ToString().Trim()), 10);
                        Actualizar.P_Observaciones = Bienes_Muebles.Rows[Contador]["OBSERVACIONES"].ToString().Trim();
                        Actualizar.P_Usuario_Nombre = Bienes_Muebles.Rows[Contador]["USUARIO"].ToString().Trim();
                        Actualizar.P_Fecha_Creo = Convert.ToDateTime(Bienes_Muebles.Rows[Contador]["FECHA"].ToString().Trim());
                        Actualizar.Actualizar_Bienes_Migracion();
                    }
                }

            }//Compras/Frm_Ope_Pat_Migrar_Bienes.aspx
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Registros: " + Cnt_Registros + "... Encontradas: " + Cnt_Encontrados + "... NO Encontradas: " + Cnt_No_Encontrados + "...');", true);
            Dt_Encontrados.DefaultView.Sort = "BIEN_ID";
            Grid_Encontrados.DataSource = Dt_Encontrados;
            Grid_Encontrados.DataBind();
            Grid_No_Encontrados.DataSource = Dt_No_Encontrados;
            Grid_No_Encontrados.DataBind();
        //} catch (Exception Ex) {
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXCEPCION:" + Ex.Message.Trim() + "');", true);
        //}
    }

    private void Cargar_Estructura_(ref DataTable Dt_Cargar) {
        Dt_Cargar.Columns.Add("BIEN_ID", Type.GetType("System.String"));
        Dt_Cargar.Columns.Add("NO_INVENTARIO_ANTERIOR", Type.GetType("System.String"));
        Dt_Cargar.Columns.Add("NO_INVENTARIO_SIAS", Type.GetType("System.String"));
        Dt_Cargar.Columns.Add("NOMBRE", Type.GetType("System.String"));
        Dt_Cargar.Columns.Add("ESTATUS", Type.GetType("System.String"));
        Dt_Cargar.Columns.Add("FECHA", Type.GetType("System.String"));
    }

    private void Cargar_Fila_(ref DataTable Dt_Datos, String Bien_ID, String No_Inventario_SIAS, String No_Inventario, String Nombre, String Estatus, String Fecha)
    {
        DataRow Fila = Dt_Datos.NewRow();
        Fila["BIEN_ID"] = Bien_ID;
        Fila["NO_INVENTARIO_ANTERIOR"] = No_Inventario;
        Fila["NO_INVENTARIO_SIAS"] = No_Inventario_SIAS;
        Fila["NOMBRE"] = Nombre;
        Fila["ESTATUS"] = Estatus;
        if (Fecha.Trim().Equals("[]")) { Fecha = "-"; }
        else { Fecha = Fecha.Substring(Fecha.Trim().IndexOf('[')); Fecha = Fecha.Replace("[", ""); Fecha = Fecha.Replace("]", ""); }
        Fila["FECHA"] = Fecha;
        Dt_Datos.Rows.Add(Fila);
    }

    private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
    {
        String Retornar = "";
        String Dato = "" + Dato_ID;
        for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
            Retornar = Retornar + "0";
        }
        Retornar = Retornar + Dato;
        return Retornar;
    }

}