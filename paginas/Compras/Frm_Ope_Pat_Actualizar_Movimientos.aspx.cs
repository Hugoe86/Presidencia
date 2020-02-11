using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Control_Patrimonial.Cargar_Tipo_Movimiento.Negocio;
using Presidencia.Sessiones;
using System.Data.OleDb;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;

public partial class paginas_Compras_Frm_Ope_Pat_Actualizar_Movimientos : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (Cls_Sessiones.No_Empleado == null || Cls_Sessiones.Nombre_Empleado == null) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
    }
    protected void Btn_Actualizar_Vehiculos_Click(object sender, EventArgs e) {
        try {
            Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Negocio = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
            Negocio.Cargar_Movimientos_Vehiculos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Proceso Concluido con Exito!!');", true);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar";
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Btn_Actualizar_Animales_Click(object sender, EventArgs e) {
        try {
            Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Negocio = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
            Negocio.Cargar_Movimientos_Animales();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Proceso Concluido con Exito!!');", true);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar";
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Btn_Actualizar_Bienes_Muebles_Click(object sender, EventArgs e) {
        try {
            Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Negocio = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
            Negocio.Cargar_Movimientos_BM_Resguardos();
            Negocio.Cargar_Movimientos_BM_Recibos(); 
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Proceso Concluido con Exito!!');", true);
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar";
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Btn_Actualizar_Obs_Est_Dep_Click(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Negocio = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
            Negocio.Actualizacion_Dependencias();
            Negocio.Actualizacion_Observaciones();
            Negocio.Actualizacion_Estados();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Proceso Concluido con Exito!!');", true);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar";
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Btn_Actualizar_Dep_Archivo_Click(object sender, EventArgs e)
    {
        try
        {
            Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio Negocio = new Cls_Ope_Pat_Cargar_Tipo_Movimiento_Negocio();
            DataSet Ds_Bienes_Muebles = new DataSet();
            String SqlExcel = "Select * From [ACTUALIZAR$]";
            Ds_Bienes_Muebles = Leer_Excel(SqlExcel, "ACTUALIZACION_CUENTA_PUBLICA.xlsx");
            DataTable Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
            Bienes_Muebles.Columns.Add("DEPENDENCIA_ID", Type.GetType("System.String"));
            Bienes_Muebles.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            foreach (DataRow Fila_Actualizar in Bienes_Muebles.Rows)
            {
                String Clave_Dependencia = ((Fila_Actualizar["CLAVE_ACTUAL_DEPENDENCIA"].ToString().Trim().Length == 3) ? String.Format("{0:0000}", Convert.ToInt32(Fila_Actualizar["CLAVE_ACTUAL_DEPENDENCIA"].ToString().Trim())) : Fila_Actualizar["CLAVE_ACTUAL_DEPENDENCIA"].ToString().Trim());
                if (String.IsNullOrEmpty(Clave_Dependencia)) Clave_Dependencia = Fila_Actualizar["CLAVE_ANTERIOR_DEPENDENCIA"].ToString().Trim();
                Fila_Actualizar.SetField("DEPENDENCIA_ID", Obtener_ID_Dependencia(Clave_Dependencia));
                Fila_Actualizar.SetField("EMPLEADO_ID", Obtener_ID_Empleado(Fila_Actualizar["NO_EMPLEADO"].ToString().Trim()));
            }
            Negocio.P_Movimientos_Archivo = Bienes_Muebles;
            Negocio.Actualizacion_Empleados_Antiguos();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('Proceso Concluido con Exito!!');", true);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Verificar";
            Lbl_Mensaje_Error.Text = Ex.Message;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
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
    private String Obtener_ID_Dependencia(String Dependencia)
    {
        String Dependencia_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " WHERE " + Cat_Dependencias.Campo_Clave + "='" + Dependencia.Trim() + "'";
        DataSet Ds_Aux_Dependencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Dependencia != null && Ds_Aux_Dependencia.Tables.Count > 0)
        {
            DataTable Dt_Aux_Dependencia = Ds_Aux_Dependencia.Tables[0];
            if (Dt_Aux_Dependencia != null && Dt_Aux_Dependencia.Rows.Count > 0)
            {
                Dependencia_ID = Dt_Aux_Dependencia.Rows[0][Cat_Dependencias.Campo_Dependencia_ID].ToString().Trim();
            }
        }
        return Dependencia_ID;
    }
    private String Obtener_ID_Empleado(String Empleado)
    {
        String Empleado_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + Empleado.Trim() + "'";
        DataSet Ds_Aux_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Empleado != null && Ds_Aux_Empleado.Tables.Count > 0)
        {
            DataTable Dt_Aux_Empleado = Ds_Aux_Empleado.Tables[0];
            if (Dt_Aux_Empleado != null && Dt_Aux_Empleado.Rows.Count > 0)
            {
                Empleado_ID = Dt_Aux_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
            }
        }
        return Empleado_ID;
    }
}
