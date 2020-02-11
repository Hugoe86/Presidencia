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

public partial class paginas_Compras_Frm_Ope_Migrar_Bienes_Control_Patrimonial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

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

    protected void Btn_Subir_Vehiculos_Click(object sender, EventArgs e)
    {
        DataSet Ds_Vehiculos = new DataSet();
        String SqlExcel = "Select * From [VEHICULOS$]";
        Ds_Vehiculos = Leer_Excel(SqlExcel, "VEHICULOS_A_MIGRAR.xls");
        DataTable Vehiculos = Ds_Vehiculos.Tables[0];
        for (int cnt = 0; cnt < Vehiculos.Rows.Count; cnt++)
        {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo_Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            Vehiculo_Negocio.P_Producto_ID = Obtener_ID_Producto(Vehiculos.Rows[cnt]["PRODUCTO"].ToString().Trim());
            Vehiculo_Negocio.P_Tipo_Vehiculo_ID = Obtener_ID_Tipo_Vehiculo(Vehiculos.Rows[cnt]["TIPO_VEHICULO"].ToString().Trim());
            Vehiculo_Negocio.P_Tipo_Combustible_ID = Obtener_ID_Tipo_Combustible("SIN ASIGNAR");
            Vehiculo_Negocio.P_Color_ID = Obtener_ID_Color("SIN ASIGNAR");
            Vehiculo_Negocio.P_Zona_ID = Obtener_ID_Zona("SIN ASIGNAR");
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["NUMERO_INVENTARIO"].ToString().Trim()))
            {
                Vehiculo_Negocio.P_Numero_Inventario = Convert.ToInt64(Vehiculos.Rows[cnt]["NUMERO_INVENTARIO"].ToString().Trim());
            }
            Vehiculo_Negocio.P_Numero_Economico_ = Vehiculos.Rows[cnt]["NUMERO_ECONOMICO"].ToString().Trim();
            Vehiculo_Negocio.P_Placas = Vehiculos.Rows[cnt]["PLACAS"].ToString().Trim();
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["ANIO_FABRICACION"].ToString().Trim()))
            {
                Vehiculo_Negocio.P_Anio_Fabricacion = Convert.ToInt32(Vehiculos.Rows[cnt]["ANIO_FABRICACION"].ToString().Trim());
            }
            Vehiculo_Negocio.P_Serie_Carroceria = Vehiculos.Rows[cnt]["SERIE_CARROCERIA"].ToString().Trim();
            Vehiculo_Negocio.P_Serie_Motor = "S/N";
            Vehiculo_Negocio.P_Odometro = "FUNCIONA";
            Vehiculo_Negocio.P_Estatus = Vehiculos.Rows[cnt]["ESTATUS"].ToString().Trim();
            Vehiculo_Negocio.P_Nombre_Producto = Vehiculos.Rows[cnt]["NOMBRE"].ToString().Trim();
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["MARCA"].ToString().Trim()))
            {
                Vehiculo_Negocio.P_Marca_ID = Obtener_ID_Marca(Vehiculos.Rows[cnt]["MARCA"].ToString().Trim());
            }
            Vehiculo_Negocio.P_Modelo_ID = Vehiculos.Rows[cnt]["MODELO"].ToString().Trim();
            Vehiculo_Negocio.P_Procedencia = Obtener_ID_Procedencia("SIN ASIGNAR");
            Vehiculo_Negocio.P_Usuario_Nombre = "CARGA INICIAL";
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["DEPENDENCIA"].ToString().Trim()))
            {
                String Dependencia_ID = Obtener_ID_Dependencia(Vehiculos.Rows[cnt]["DEPENDENCIA"].ToString().Trim());
                if (Dependencia_ID != null)
                {
                    Vehiculo_Negocio.P_Dependencia_ID = Dependencia_ID;
                }
            }
            //RESGUARDOS
            DataTable Dt_Resguardos = new DataTable();
            Dt_Resguardos.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["NO_EMPLEADO"].ToString().Trim()))
            {
                String Empleado_ID = Obtener_ID_Empleado(Vehiculos.Rows[cnt]["NO_EMPLEADO"].ToString().Trim());
                if (Empleado_ID != null)
                {
                    DataRow Fila_Empleado = Dt_Resguardos.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = Empleado_ID;
                    Dt_Resguardos.Rows.Add(Fila_Empleado);
                }
            }
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["NO_EMPLEADO_DOS"].ToString().Trim()))
            {
                String Empleado_ID = Obtener_ID_Empleado(Vehiculos.Rows[cnt]["NO_EMPLEADO_DOS"].ToString().Trim());
                if (Empleado_ID != null)
                {
                    DataRow Fila_Empleado = Dt_Resguardos.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = Empleado_ID;
                    Dt_Resguardos.Rows.Add(Fila_Empleado);
                }
            }
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["NO_EMPLEADO_TRES"].ToString().Trim()))
            {
                String Empleado_ID = Obtener_ID_Empleado(Vehiculos.Rows[cnt]["NO_EMPLEADO_TRES"].ToString().Trim());
                if (Empleado_ID != null)
                {
                    DataRow Fila_Empleado = Dt_Resguardos.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = Empleado_ID;
                    Dt_Resguardos.Rows.Add(Fila_Empleado);
                }
            }
            if (!string.IsNullOrEmpty(Vehiculos.Rows[cnt]["NO_EMPLEADO_CUATRO"].ToString().Trim()))
            {
                String Empleado_ID = Obtener_ID_Empleado(Vehiculos.Rows[cnt]["NO_EMPLEADO_CUATRO"].ToString().Trim());
                if (Empleado_ID != null)
                {
                    DataRow Fila_Empleado = Dt_Resguardos.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = Empleado_ID;
                    Dt_Resguardos.Rows.Add(Fila_Empleado);
                }
            }
            Vehiculo_Negocio.P_Resguardantes = Dt_Resguardos;
            Vehiculo_Negocio.P_Observaciones = Vehiculos.Rows[cnt]["OBSERVACIONES"].ToString().Trim();
            Vehiculo_Negocio.Alta_Migrar_Vehiculo();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXITO!');", true);
    }

    private String Obtener_ID_Producto(String Producto)
    {
        String Producto_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Nombre + " = '" + Producto.Trim() + "'";
        DataSet Ds_Aux_Producto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Producto != null && Ds_Aux_Producto.Tables.Count > 0)
        {
            DataTable Dt_Aux_Producto = Ds_Aux_Producto.Tables[0];
            if (Dt_Aux_Producto != null && Dt_Aux_Producto.Rows.Count > 0)
            {
                Producto_ID = Dt_Aux_Producto.Rows[0][Cat_Com_Productos.Campo_Producto_ID].ToString();
            }
        }
        if (Producto_ID == null)
        {
            Cls_Cat_Com_Productos_Negocio Producto_Negocio = new Cls_Cat_Com_Productos_Negocio();
            Producto_Negocio.P_Nombre = Producto;
            Producto_Negocio.P_Estatus = "INICIAL";
            Producto_Negocio.P_Usuario_Creo = "CARGA INICIAL";
            Producto_Negocio.P_Tipo = "VEHICULO";
            Producto_Negocio.Alta_Producto();
            Producto_ID = Obtener_ID_Producto(Producto);
        }
        return Producto_ID;
    }
    private String Obtener_ID_Tipo_Vehiculo(String Tipo_Vehiculo)
    {
        String Tipo_Vehiculo_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Tipos_Vehiculo.Tabla_Cat_Pat_Tipos_Vehiculo + " WHERE " + Cat_Pat_Tipos_Vehiculo.Campo_Descripcion + "='" + Tipo_Vehiculo.Trim() + "'";
        DataSet Ds_Aux_Tipo_Vehiculo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Tipo_Vehiculo != null && Ds_Aux_Tipo_Vehiculo.Tables.Count > 0)
        {
            DataTable Dt_Aux_Tipo_Vehiculo = Ds_Aux_Tipo_Vehiculo.Tables[0];
            if (Dt_Aux_Tipo_Vehiculo != null && Dt_Aux_Tipo_Vehiculo.Rows.Count > 0)
            {
                Tipo_Vehiculo_ID = Dt_Aux_Tipo_Vehiculo.Rows[0][Cat_Pat_Tipos_Vehiculo.Campo_Tipo_Vehiculo_ID].ToString().Trim();
            }
        }
        if (Tipo_Vehiculo_ID == null)
        {
            Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Vehiculo_Negocio();
            Tipo_Negocio.P_Descripcion = Tipo_Vehiculo.Trim();
            Tipo_Negocio.P_Estatus = "VIGENTE";
            Tipo_Negocio.P_Usuario = "CARGA INICIAL";
            Tipo_Negocio.Alta_Tipo_Vehiculo();
            Tipo_Vehiculo_ID = Obtener_ID_Tipo_Vehiculo(Tipo_Vehiculo);
        }
        return Tipo_Vehiculo_ID;
    }
    private String Obtener_ID_Tipo_Combustible(String Tipo_Combustible)
    {
        String Tipo_Combustible_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Tipos_Combustible.Tabla_Cat_Pat_Tipos_Combustible + " WHERE " + Cat_Pat_Tipos_Combustible.Campo_Descripcion + "='" + Tipo_Combustible.Trim() + "'";
        DataSet Ds_Aux_Tipo_Combustible = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Tipo_Combustible != null && Ds_Aux_Tipo_Combustible.Tables.Count > 0)
        {
            DataTable Dt_Aux_Tipo_Combustible = Ds_Aux_Tipo_Combustible.Tables[0];
            if (Dt_Aux_Tipo_Combustible != null && Dt_Aux_Tipo_Combustible.Rows.Count > 0)
            {
                Tipo_Combustible_ID = Dt_Aux_Tipo_Combustible.Rows[0][Cat_Pat_Tipos_Combustible.Campo_Tipo_Combustible_ID].ToString().Trim();
            }
        }
        if (Tipo_Combustible_ID == null)
        {
            Cls_Cat_Pat_Com_Tipos_Combustible_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Combustible_Negocio();
            Tipo_Negocio.P_Descripcion = Tipo_Combustible.Trim();
            Tipo_Negocio.P_Estatus = "VIGENTE";
            Tipo_Negocio.P_Usuario = "INICIAL";
            Tipo_Negocio.Alta_Tipo_Combustible();
            Tipo_Combustible_ID = Obtener_ID_Tipo_Combustible(Tipo_Combustible);
        }
        return Tipo_Combustible_ID;
    }
    private String Obtener_ID_Color(String Color)
    {
        String Color_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " WHERE " + Cat_Pat_Colores.Campo_Descripcion + "='" + Color.Trim() + "'";
        DataSet Ds_Aux_Color = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Color != null && Ds_Aux_Color.Tables.Count > 0)
        {
            DataTable Dt_Aux_Color = Ds_Aux_Color.Tables[0];
            if (Dt_Aux_Color != null && Dt_Aux_Color.Rows.Count > 0)
            {
                Color_ID = Dt_Aux_Color.Rows[0][Cat_Pat_Colores.Campo_Color_ID].ToString().Trim();
            }
        }
        if (Color_ID == null)
        {
            Cls_Cat_Pat_Com_Colores_Negocio Color_Negocio = new Cls_Cat_Pat_Com_Colores_Negocio();
            Color_Negocio.P_Descripcion = Color.Trim();
            Color_Negocio.P_Estatus = "VIGENTE";
            Color_Negocio.P_Usuario = "INICIAL";
            Color_Negocio.Alta_Color();
            Color_ID = Obtener_ID_Color(Color);
        }
        return Color_ID;
    }
    private String Obtener_ID_Zona(String Zona)
    {
        String Zona_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Zonas.Tabla_Cat_Pat_Zonas + " WHERE " + Cat_Pat_Zonas.Campo_Descripcion + "='" + Zona.Trim() + "'";
        DataSet Ds_Aux_Zona = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Zona != null && Ds_Aux_Zona.Tables.Count > 0)
        {
            DataTable Dt_Aux_Zona = Ds_Aux_Zona.Tables[0];
            if (Dt_Aux_Zona != null && Dt_Aux_Zona.Rows.Count > 0)
            {
                Zona_ID = Dt_Aux_Zona.Rows[0][Cat_Pat_Zonas.Campo_Zona_ID].ToString().Trim();
            }
        }
        if (Zona_ID == null)
        {
            Cls_Cat_Pat_Com_Zonas_Negocio Zona_Negocio = new Cls_Cat_Pat_Com_Zonas_Negocio();
            Zona_Negocio.P_Descripcion = Zona.Trim();
            Zona_Negocio.P_Estatus = "VIGENTE";
            Zona_Negocio.P_Usuario = "INICIAL";
            Zona_Negocio.Alta_Zona();
            Zona_ID = Obtener_ID_Zona(Zona);
        }
        return Zona_ID;
    }
    private String Obtener_ID_Procedencia(String Procedencia)
    {
        String Procedencia_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Procedencias.Tabla_Cat_Pat_Procedencias + " WHERE " + Cat_Pat_Procedencias.Campo_Nombre + "='" + Procedencia.Trim() + "'";
        DataSet Ds_Aux_Procedencia = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Procedencia != null && Ds_Aux_Procedencia.Tables.Count > 0)
        {
            DataTable Dt_Aux_Procedencia = Ds_Aux_Procedencia.Tables[0];
            if (Dt_Aux_Procedencia != null && Dt_Aux_Procedencia.Rows.Count > 0)
            {
                Procedencia_ID = Dt_Aux_Procedencia.Rows[0][Cat_Pat_Procedencias.Campo_Procedencia_ID].ToString().Trim();
            }
        }
        if (Procedencia_ID == null)
        {
            Cls_Cat_Pat_Com_Procedencias_Negocio Procedencia_Negocio = new Cls_Cat_Pat_Com_Procedencias_Negocio();
            Procedencia_Negocio.P_Nombre = Procedencia.Trim();
            Procedencia_Negocio.P_Estatus = "VIGENTE";
            Procedencia_Negocio.P_Usuario = "INICIAL";
            Procedencia_Negocio.Alta_Procedencia();
            Procedencia_ID = Obtener_ID_Procedencia(Procedencia);
        }
        return Procedencia_ID;
    }
    private String Obtener_ID_Marca(String Marca)
    {
        String Marca_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " WHERE " + Cat_Com_Marcas.Campo_Nombre + "='" + Marca.Trim() + "'";
        DataSet Ds_Aux_Marca = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Marca != null && Ds_Aux_Marca.Tables.Count > 0)
        {
            DataTable Dt_Aux_Marca = Ds_Aux_Marca.Tables[0];
            if (Dt_Aux_Marca != null && Dt_Aux_Marca.Rows.Count > 0)
            {
                Marca_ID = Dt_Aux_Marca.Rows[0][Cat_Com_Marcas.Campo_Marca_ID].ToString().Trim();
            }
        }
        if (Marca_ID == null)
        {
            Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
            Marca_Negocio.P_Nombre = Marca.Trim();
            Marca_Negocio.P_Estatus = "INICIAL";
            Marca_Negocio.P_Usuario = "INICIAL";
            Marca_Negocio.Alta_Marcas();
            Marca_ID = Obtener_ID_Marca(Marca);
        }
        return Marca_ID;
    }
    private String Obtener_ID_Modelo(String Modelo)
    {
        String Modelo_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE " + Cat_Com_Modelos.Campo_Nombre + "='" + Modelo.Trim() + "'";
        DataSet Ds_Aux_Modelo = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Modelo != null && Ds_Aux_Modelo.Tables.Count > 0)
        {
            DataTable Dt_Aux_Modelo = Ds_Aux_Modelo.Tables[0];
            if (Dt_Aux_Modelo != null && Dt_Aux_Modelo.Rows.Count > 0)
            {
                Modelo_ID = Dt_Aux_Modelo.Rows[0][Cat_Com_Modelos.Campo_Modelo_ID].ToString().Trim();
            }
        }
        if (Modelo_ID == null)
        {
            Cls_Cat_Com_Modelos_Negocio Modelo_Negocio = new Cls_Cat_Com_Modelos_Negocio();
            Modelo_Negocio.P_Nombre = Modelo.Trim();
            Modelo_Negocio.P_Estatus = "INICIAL";
            Modelo_Negocio.Alta_Modelos();
            Modelo_ID = Obtener_ID_Modelo(Modelo);
        }
        return Modelo_ID;
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
    private String Obtener_ID_Empleado(String No_Empleado)
    {
        String Empleado_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + No_Empleado.Trim() + "'";
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



    protected void Btn_Subir_Animales_Click(object sender, EventArgs e)
    {
        DataSet Ds_Animales = new DataSet();
        String SqlExcel = "Select * From [ANIMALES$]";
        Ds_Animales = Leer_Excel(SqlExcel, "ANIMALES_A_MIGRAR.xls");
        DataTable Animales = Ds_Animales.Tables[0];
        for (int cnt = 0; cnt < Animales.Rows.Count; cnt++)//for (int cnt = 0; cnt < Animales.Rows.Count; cnt++)
        {
            Cls_Ope_Pat_Com_Cemovientes_Negocio Animal_Negocio = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
            Animal_Negocio.P_Producto_ID = Obtener_ID_Producto(Animales.Rows[cnt]["PRODUCTO"].ToString().Trim());
            Animal_Negocio.P_Dependencia_ID = Obtener_ID_Dependencia_Empleado(Animales.Rows[cnt]["NO_EMPLEADO"].ToString().Trim());
            Animal_Negocio.P_Tipo_Alimentacion_ID = Obtener_ID_Tipo_Alimentacion(Animales.Rows[cnt]["TIPO_ALIMENTACION"].ToString().Trim());
            Animal_Negocio.P_Tipo_Adiestramiento_ID = Obtener_ID_Tipo_Adiestramiento(Animales.Rows[cnt]["TIPO_ADIESTRAMIENTO"].ToString().Trim());
            Animal_Negocio.P_Raza_ID = Obtener_ID_Raza(Animales.Rows[cnt]["RAZA"].ToString().Trim());
            Animal_Negocio.P_Funcion_ID = Obtener_ID_Funcion(Animales.Rows[cnt]["FUNCION"].ToString().Trim());
            Animal_Negocio.P_Nombre = Animales.Rows[cnt]["NOMBRE"].ToString().Trim();
            Animal_Negocio.P_Numero_Inventario = Convert.ToInt64(Animales.Rows[cnt]["NUMERO_INVENTARIO"].ToString().Trim());
            Animal_Negocio.P_Fecha_Adquisicion = Convert.ToDateTime(Animales.Rows[cnt]["FECHA_ADQUISICION"].ToString().Trim());
            Animal_Negocio.P_Estatus = Animales.Rows[cnt]["ESTATUS"].ToString().Trim();
            Animal_Negocio.P_Observaciones = Animales.Rows[cnt]["OBSERVACIONES"].ToString().Trim();
            Animal_Negocio.P_Color_ID = Obtener_ID_Color(Animales.Rows[cnt]["COLOR"].ToString().Trim());
            Animal_Negocio.P_Costo_Actual = Convert.ToDouble(Animales.Rows[cnt]["COSTO_ACTUAL"].ToString().Trim());
            Animal_Negocio.P_Tipo_Ascendencia = Animales.Rows[cnt]["TIPO_ASCENDENCIA"].ToString().Trim();
            Animal_Negocio.P_Sexo = Animales.Rows[cnt]["SEXO"].ToString().Trim();
            Animal_Negocio.P_Tipo_Cemoviente_ID = Obtener_ID_Tipo_Cemoviente(Animales.Rows[cnt]["TIPO_CEMOVIENTE"].ToString().Trim());
            Animal_Negocio.P_Procedencia = Obtener_ID_Procedencia(Animales.Rows[cnt]["PROCEDENCIA"].ToString());
            if (!string.IsNullOrEmpty(Animales.Rows[cnt]["FACTURA"].ToString().Trim()))
            {
                Animal_Negocio.P_No_Factura = Animales.Rows[cnt]["FACTURA"].ToString().Trim();
            }
            //RESGUARDOS
            DataTable Dt_Resguardos = new DataTable();
            Dt_Resguardos.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
            if (!string.IsNullOrEmpty(Animales.Rows[cnt]["NO_EMPLEADO"].ToString().Trim()))
            {
                String Empleado_ID = Obtener_ID_Empleado(Animales.Rows[cnt]["NO_EMPLEADO"].ToString().Trim());
                if (Empleado_ID != null)
                {
                    DataRow Fila_Empleado = Dt_Resguardos.NewRow();
                    Fila_Empleado["EMPLEADO_ID"] = Empleado_ID;
                    Dt_Resguardos.Rows.Add(Fila_Empleado);
                }
            }
            Animal_Negocio.P_Resguardantes = Dt_Resguardos;
            Animal_Negocio.Alta_Migrar_Cemoviente();
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXITO!');", true);
    }
    private String Obtener_ID_Tipo_Alimentacion(String Tipo_Alimentacion)
    {
        String Tipo_Alimentacion_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Tipos_Alimentacion.Tabla_Cat_Pat_Tipos_Alimentacion + " WHERE " + Cat_Pat_Tipos_Alimentacion.Campo_Nombre + "='" + Tipo_Alimentacion.Trim() + "'";
        DataSet Ds_Aux_Tipo_Alimentacion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Tipo_Alimentacion != null && Ds_Aux_Tipo_Alimentacion.Tables.Count > 0)
        {
            DataTable Dt_Aux_Tipo_Alimentacion = Ds_Aux_Tipo_Alimentacion.Tables[0];
            if (Dt_Aux_Tipo_Alimentacion != null && Dt_Aux_Tipo_Alimentacion.Rows.Count > 0)
            {
                Tipo_Alimentacion_ID = Dt_Aux_Tipo_Alimentacion.Rows[0][Cat_Pat_Tipos_Alimentacion.Campo_Tipo_Alimentacion_ID].ToString().Trim();
            }
        }
        if (Tipo_Alimentacion_ID == null)
        {
            Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Alimentacion_Negocio();
            Tipo_Negocio.P_Nombre = Tipo_Alimentacion.Trim();
            Tipo_Negocio.P_Estatus = "VIGENTE";
            Tipo_Negocio.P_Usuario = "INICIAL";
            Tipo_Negocio.Alta_Tipo_Alimentacion();
            Tipo_Alimentacion_ID = Obtener_ID_Tipo_Alimentacion(Tipo_Alimentacion);
        }
        return Tipo_Alimentacion_ID;
    }
    private String Obtener_ID_Tipo_Adiestramiento(String Tipo_Adiestramiento)
    {
        String Tipo_Adiestramiento_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Tipos_Adiestramiento.Tabla_Cat_Pat_Tipos_Adiestramiento + " WHERE " + Cat_Pat_Tipos_Adiestramiento.Campo_Nombre + "='" + Tipo_Adiestramiento.Trim() + "'";
        DataSet Ds_Aux_Tipo_Adiestramiento = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Tipo_Adiestramiento != null && Ds_Aux_Tipo_Adiestramiento.Tables.Count > 0)
        {
            DataTable Dt_Aux_Tipo_Adiestramiento = Ds_Aux_Tipo_Adiestramiento.Tables[0];
            if (Dt_Aux_Tipo_Adiestramiento != null && Dt_Aux_Tipo_Adiestramiento.Rows.Count > 0)
            {
                Tipo_Adiestramiento_ID = Dt_Aux_Tipo_Adiestramiento.Rows[0][Cat_Pat_Tipos_Adiestramiento.Campo_Tipo_Adiestramiento_ID].ToString().Trim();
            }
        }
        if (Tipo_Adiestramiento_ID == null)
        {
            Cls_Cat_Pat_Com_Tipos_Adiestramiento_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Adiestramiento_Negocio();
            Tipo_Negocio.P_Nombre = Tipo_Adiestramiento.Trim();
            Tipo_Negocio.P_Estatus = "VIGENTE";
            Tipo_Negocio.P_Usuario = "INICIAL";
            Tipo_Negocio.Alta_Tipo_Adiestramiento();
            Tipo_Adiestramiento_ID = Obtener_ID_Tipo_Adiestramiento(Tipo_Adiestramiento);
        }
        return Tipo_Adiestramiento_ID;
    }
    private String Obtener_ID_Raza(String Raza)
    {
        String Raza_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Razas.Tabla_Cat_Pat_Razas + " WHERE " + Cat_Pat_Razas.Campo_Nombre + "='" + Raza.Trim() + "'";
        DataSet Ds_Aux_Raza = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Raza != null && Ds_Aux_Raza.Tables.Count > 0)
        {
            DataTable Dt_Aux_Raza = Ds_Aux_Raza.Tables[0];
            if (Dt_Aux_Raza != null && Dt_Aux_Raza.Rows.Count > 0)
            {
                Raza_ID = Dt_Aux_Raza.Rows[0][Cat_Pat_Razas.Campo_Raza_ID].ToString().Trim();
            }
        }
        if (Raza_ID == null)
        {
            Cls_Cat_Pat_Com_Razas_Negocio Raza_Negocio = new Cls_Cat_Pat_Com_Razas_Negocio();
            Raza_Negocio.P_Nombre = Raza.Trim();
            Raza_Negocio.P_Estatus = "VIGENTE";
            Raza_Negocio.P_Usuario = "INICIAL";
            Raza_Negocio.Alta_Raza();
            Raza_ID = Obtener_ID_Raza(Raza);
        }
        return Raza_ID;
    }
    private String Obtener_ID_Funcion(String Funcion)
    {
        String Funcion_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Funciones.Tabla_Cat_Pat_Funciones + " WHERE " + Cat_Pat_Funciones.Campo_Nombre + "='" + Funcion.Trim() + "'";
        DataSet Ds_Aux_Funcion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Funcion != null && Ds_Aux_Funcion.Tables.Count > 0)
        {
            DataTable Dt_Aux_Funcion = Ds_Aux_Funcion.Tables[0];
            if (Dt_Aux_Funcion != null && Dt_Aux_Funcion.Rows.Count > 0)
            {
                Funcion_ID = Dt_Aux_Funcion.Rows[0][Cat_Pat_Funciones.Campo_Funcion_ID].ToString().Trim();
            }
        }
        if (Funcion_ID == null)
        {
            Cls_Cat_Pat_Com_Funciones_Negocio Funcion_Negocio = new Cls_Cat_Pat_Com_Funciones_Negocio();
            Funcion_Negocio.P_Nombre = Funcion.Trim();
            Funcion_Negocio.P_Estatus = "VIGENTE";
            Funcion_Negocio.P_Usuario = "INICIAL";
            Funcion_Negocio.Alta_Funcion();
            Funcion_ID = Obtener_ID_Funcion(Funcion);
        }
        return Funcion_ID;
    }
    private String Obtener_ID_Tipo_Cemoviente(String Tipo_Cemoviente)
    {
        String Tipo_Cemoviente_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Tipos_Cemovientes.Tabla_Cat_Pat_Tipos_Cemovientes + " WHERE " + Cat_Pat_Tipos_Cemovientes.Campo_Nombre + "='" + Tipo_Cemoviente.Trim() + "'";
        DataSet Ds_Aux_Tipo_Cemoviente = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Tipo_Cemoviente != null && Ds_Aux_Tipo_Cemoviente.Tables.Count > 0)
        {
            DataTable Dt_Aux_Tipo_Cemoviente = Ds_Aux_Tipo_Cemoviente.Tables[0];
            if (Dt_Aux_Tipo_Cemoviente != null && Dt_Aux_Tipo_Cemoviente.Rows.Count > 0)
            {
                Tipo_Cemoviente_ID = Dt_Aux_Tipo_Cemoviente.Rows[0][Cat_Pat_Tipos_Cemovientes.Campo_Tipo_Cemoviente_ID].ToString().Trim();
            }
        }
        if (Tipo_Cemoviente_ID == null)
        {
            Cls_Cat_Pat_Com_Tipos_Cemovientes_Negocio Tipo_Negocio = new Cls_Cat_Pat_Com_Tipos_Cemovientes_Negocio();
            Tipo_Negocio.P_Nombre = Tipo_Cemoviente.Trim();
            Tipo_Negocio.P_Estatus = "VIGENTE";
            Tipo_Negocio.P_Usuario = "INICIAL";
            Tipo_Negocio.Alta_Tipo_Cemoviente();
            Tipo_Cemoviente_ID = Obtener_ID_Tipo_Cemoviente(Tipo_Cemoviente);
        }
        return Tipo_Cemoviente_ID;
    }
    private String Obtener_ID_Dependencia_Empleado(String No_Empleado)
    {
        String Dependencia_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_No_Empleado + "='" + No_Empleado.Trim() + "'";
        DataSet Ds_Aux_Empleado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Empleado != null && Ds_Aux_Empleado.Tables.Count > 0)
        {
            DataTable Dt_Aux_Empleado = Ds_Aux_Empleado.Tables[0];
            if (Dt_Aux_Empleado != null && Dt_Aux_Empleado.Rows.Count > 0)
            {
                Dependencia_ID = Dt_Aux_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
            }
        }
        return Dependencia_ID;
    }



    protected void Btn_Subir_Bienes_Muebles_Click(object sender, EventArgs e)
    {
    }
    private String Obtener_ID_Material(String Material)
    {
        String Material_ID = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " WHERE " + Cat_Pat_Materiales.Campo_Descripcion + "='" + Material.Trim() + "'";
        DataSet Ds_Aux_Material = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Material != null && Ds_Aux_Material.Tables.Count > 0)
        {
            DataTable Dt_Aux_Material = Ds_Aux_Material.Tables[0];
            if (Dt_Aux_Material != null && Dt_Aux_Material.Rows.Count > 0)
            {
                Material_ID = Dt_Aux_Material.Rows[0][Cat_Pat_Materiales.Campo_Material_ID].ToString().Trim();
            }
        }
        if (Material_ID == null)
        {
            Cls_Cat_Pat_Com_Materiales_Negocio Material_Negocio = new Cls_Cat_Pat_Com_Materiales_Negocio();
            Material_Negocio.P_Descripcion = Material.Trim();
            Material_Negocio.P_Estatus = "VIGENTE";
            Material_Negocio.P_Usuario = "INICIAL";
            Material_Negocio.Alta_Material();
            Material_ID = Obtener_ID_Material(Material);
        }
        return Material_ID;
    }

    private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
    {
        String Retornar = "";
        String Dato = "" + Dato_ID;
        for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
        {
            Retornar = Retornar + "0";
        }
        Retornar = Retornar + Dato;
        return Retornar;
    }

    private Boolean Validar_ID_Proveedor(String Proveedor_ID)
    {
        Boolean Existe_Proveedor = false;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + "='" + Proveedor_ID.Trim() + "'";
        DataSet Ds_Aux_Proveedor = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Proveedor != null && Ds_Aux_Proveedor.Tables.Count > 0)
        {
            DataTable Dt_Aux_Material = Ds_Aux_Proveedor.Tables[0];
            if (Dt_Aux_Material != null && Dt_Aux_Material.Rows.Count > 0)
            {
                Existe_Proveedor = true;
            }
        }
        return Existe_Proveedor;
    }

    private Boolean Validar_ID_Bien_Mueble_Creado(String Bien_Mueble_ID)
    {
        Boolean Existe_Bien_Mueble = false;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "='" + Bien_Mueble_ID.Trim() + "'";
        DataSet Ds_Aux_Bienes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        {
            DataTable Dt_Aux_Bienes = Ds_Aux_Bienes.Tables[0];
            if (Dt_Aux_Bienes != null && Dt_Aux_Bienes.Rows.Count > 0)
            {
                Existe_Bien_Mueble = true;
            }
        }
        return Existe_Bien_Mueble;
    }

    protected void Btn_Subir_Bienes_Muebles_2_Click(object sender, EventArgs e)
    {
        DataSet Ds_Bienes_Muebles = new DataSet();
        String SqlExcel = "Select * From [MUEBLES$]";
        Ds_Bienes_Muebles = Leer_Excel(SqlExcel, "MUEBLES_A_MIGRAR_2.xls");
        DataTable Bienes_Muebles = Ds_Bienes_Muebles.Tables[0];
        for (int cnt = 0; cnt < 40827; cnt++)
        {
            String Bien_Mueble_ID = Convertir_A_Formato_ID(Convert.ToInt32(Bienes_Muebles.Rows[cnt]["CLAVE_ID"].ToString().Trim()), 10);
            if (!Validar_ID_Bien_Mueble_Creado(Bien_Mueble_ID))
            {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Mueble_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                Mueble_Negocio.P_Bien_Mueble_ID = Bien_Mueble_ID;
                Mueble_Negocio.P_Producto_ID = Obtener_ID_Producto(Bienes_Muebles.Rows[cnt]["PRODUCTO"].ToString().Trim());
                Mueble_Negocio.P_Material_ID = Obtener_ID_Material(Bienes_Muebles.Rows[cnt]["MATERIAL"].ToString().Trim());
                Mueble_Negocio.P_Color_ID = Obtener_ID_Color(Bienes_Muebles.Rows[cnt]["COLOR"].ToString().Trim());
                Mueble_Negocio.P_Numero_Inventario_Anterior = Bienes_Muebles.Rows[cnt]["NUMERO_INVENTARIO"].ToString().Trim();
                Mueble_Negocio.P_Factura = Bienes_Muebles.Rows[cnt]["FACTURA"].ToString().Trim();
                Mueble_Negocio.P_Numero_Serie = Bienes_Muebles.Rows[cnt]["NUMERO_SERIE"].ToString().Trim();
                Mueble_Negocio.P_Costo_Actual = Convert.ToDouble(Bienes_Muebles.Rows[cnt]["COSTO_ACTUAL"].ToString().Trim());
                Mueble_Negocio.P_Estatus = Bienes_Muebles.Rows[cnt]["ESTATUS"].ToString().Trim();
                if (Bienes_Muebles.Rows[cnt]["ESTADO"].ToString().Trim().Length > 0)
                {
                    if (Bienes_Muebles.Rows[cnt]["ESTADO"].ToString().Trim().Equals("B")) { Mueble_Negocio.P_Estado = "BUENO"; }
                    if (Bienes_Muebles.Rows[cnt]["ESTADO"].ToString().Trim().Equals("R")) { Mueble_Negocio.P_Estado = "REGULAR"; }
                    if (Bienes_Muebles.Rows[cnt]["ESTADO"].ToString().Trim().Equals("M")) { Mueble_Negocio.P_Estado = "MALO"; }
                }
                else
                {
                    Mueble_Negocio.P_Estado = "REGULAR";
                }
                Mueble_Negocio.P_Observaciones = Bienes_Muebles.Rows[cnt]["OBSERVACIONES"].ToString().Trim();
                if (!string.IsNullOrEmpty(Bienes_Muebles.Rows[cnt]["FECHA_INVENTARIO"].ToString().Trim()))
                {
                    Mueble_Negocio.P_Fecha_Inventario_ = Convert.ToDateTime(Bienes_Muebles.Rows[cnt]["FECHA_INVENTARIO"].ToString().Trim());
                }
                Mueble_Negocio.P_Procedencia = Obtener_ID_Procedencia(Bienes_Muebles.Rows[cnt]["PROCEDENCIA"].ToString().Trim());
                if (!string.IsNullOrEmpty(Bienes_Muebles.Rows[cnt]["FECHA_ADQUISICION"].ToString().Trim()))
                {
                    Mueble_Negocio.P_Fecha_Adquisicion_ = Convert.ToDateTime(Bienes_Muebles.Rows[cnt]["FECHA_ADQUISICION"].ToString().Trim());
                }
                Mueble_Negocio.P_Cantidad = 1;
                Mueble_Negocio.P_Nombre_Producto = Bienes_Muebles.Rows[cnt]["NOMBRE"].ToString().Trim();
                Mueble_Negocio.P_Marca_ID = Obtener_ID_Marca(Bienes_Muebles.Rows[cnt]["MARCA"].ToString().Trim());
                if (Bienes_Muebles.Rows[cnt]["OPERACION"].ToString().Trim().Length > 0)
                {
                    if (Bienes_Muebles.Rows[cnt]["OPERACION"].ToString().Trim().Equals("RE")) { Mueble_Negocio.P_Operacion = "RESGUARDO"; }
                    if (Bienes_Muebles.Rows[cnt]["OPERACION"].ToString().Trim().Equals("RS")) { Mueble_Negocio.P_Operacion = "RECIBO"; }
                }
                Mueble_Negocio.P_Modelo = Bienes_Muebles.Rows[cnt]["MODELO"].ToString().Trim();
                Mueble_Negocio.P_Dependencia_ID = Obtener_ID_Dependencia_Empleado(Bienes_Muebles.Rows[cnt]["NO_EMPLEADO"].ToString().Trim());

                Mueble_Negocio.P_Proveedor_ID = (Validar_ID_Proveedor(Convertir_A_Formato_ID(Convert.ToInt32(Bienes_Muebles.Rows[cnt]["PADRON_PROVEEDOR"].ToString().Trim()), 10))) ? Convertir_A_Formato_ID(Convert.ToInt32(Bienes_Muebles.Rows[cnt]["PADRON_PROVEEDOR"].ToString().Trim()), 10) : null;
                Mueble_Negocio.P_Usuario_Nombre = Bienes_Muebles.Rows[cnt]["USUARIO_CREO"].ToString().Trim();
                if (!string.IsNullOrEmpty(Bienes_Muebles.Rows[cnt]["FECHA_CREO"].ToString().Trim()))
                {
                    Mueble_Negocio.P_Fecha_Creo = Convert.ToDateTime(Bienes_Muebles.Rows[cnt]["FECHA_CREO"].ToString());
                }
                Mueble_Negocio.Alta_Migrar_Bien_Mueble();
            }
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXITO!');", true);
    }

    protected void Btn_Subir_Resguardos_Bienes_Muebles_Click(object sender, EventArgs e)
    {
        DataTable Dt_Historial = new DataTable();
        //Se carga la Primera Parte del Historial de los Resguardos
        Dt_Historial = Obtener_Historial_Bienes("HISTORIAL_1");
        for (Int32 Contador = 0; Contador < 64999; Contador++)
        {
            String Bien_Mueble_ID = Convertir_A_Formato_ID(Convert.ToInt32(Dt_Historial.Rows[Contador]["BIEN_ID"].ToString().Trim()), 10);
            if (Validar_ID_Bien_Mueble_Creado(Bien_Mueble_ID))
            {
                String Empleado_ID = Obtener_ID_Empleado(Convertir_A_Formato_ID(Convert.ToInt32(Dt_Historial.Rows[Contador]["EMPLEADO_RESGUARDO_ID"].ToString().Trim()), 6));
                if (Empleado_ID != null && Empleado_ID.Trim().Length > 0)
                {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble_Resguardo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble_Resguardo.P_Bien_Mueble_ID = Bien_Mueble_ID;
                    Bien_Mueble_Resguardo.P_Operacion = Obtener_Operacion_Bien(Bien_Mueble_ID);
                    Bien_Mueble_Resguardo.P_Tipo = Dt_Historial.Rows[Contador]["TIPO"].ToString().Trim();
                    Bien_Mueble_Resguardo.P_Usuario_ID = Empleado_ID;
                    if (Dt_Historial.Rows[Contador]["FECHA_INICIAL"].ToString().Trim().Length > 0)
                    {
                        Bien_Mueble_Resguardo.P_Fecha_Adquisicion_ = Convert.ToDateTime(Dt_Historial.Rows[Contador]["FECHA_INICIAL"].ToString().Trim());
                    }
                    if (Dt_Historial.Rows[Contador]["FECHA_FINAL"].ToString().Trim().Length > 0)
                    {
                        Bien_Mueble_Resguardo.P_Fecha_Creo = Convert.ToDateTime(Dt_Historial.Rows[Contador]["FECHA_FINAL"].ToString().Trim());
                    }
                    Bien_Mueble_Resguardo.P_Observaciones = Dt_Historial.Rows[Contador]["COMENTARIOS"].ToString().Trim();
                    Bien_Mueble_Resguardo.P_Estatus = Dt_Historial.Rows[Contador]["ESTATUS"].ToString().Trim();
                    Bien_Mueble_Resguardo.Alta_Migrar_Resguardos_Bien_Mueble();
                }
            }
        }
        //Se carga la Segunda Parte del Historial de los Resguardos
        Dt_Historial = Obtener_Historial_Bienes("HISTORIAL_2");
        for (Int32 Contador = 0; Contador < 25193; Contador++)
        {
            String Bien_Mueble_ID = Convertir_A_Formato_ID(Convert.ToInt32(Dt_Historial.Rows[Contador]["BIEN_ID"].ToString().Trim()), 10);
            if (Validar_ID_Bien_Mueble_Creado(Bien_Mueble_ID))
            {
                String Empleado_ID = Obtener_ID_Empleado(Convertir_A_Formato_ID(Convert.ToInt32(Dt_Historial.Rows[Contador]["EMPLEADO_RESGUARDO_ID"].ToString().Trim()), 6));
                if (Empleado_ID != null && Empleado_ID.Trim().Length > 0)
                {
                    Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Bien_Mueble_Resguardo = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                    Bien_Mueble_Resguardo.P_Bien_Mueble_ID = Bien_Mueble_ID;
                    Bien_Mueble_Resguardo.P_Operacion = Obtener_Operacion_Bien(Bien_Mueble_ID);
                    Bien_Mueble_Resguardo.P_Tipo = Dt_Historial.Rows[Contador]["TIPO"].ToString().Trim();
                    Bien_Mueble_Resguardo.P_Usuario_ID = Empleado_ID;
                    if (Dt_Historial.Rows[Contador]["FECHA_INICIAL"].ToString().Trim().Length > 0)
                    {
                        Bien_Mueble_Resguardo.P_Fecha_Adquisicion_ = Convert.ToDateTime(Dt_Historial.Rows[Contador]["FECHA_INICIAL"].ToString().Trim());
                    }
                    if (Dt_Historial.Rows[Contador]["FECHA_FINAL"].ToString().Trim().Length > 0)
                    {
                        Bien_Mueble_Resguardo.P_Fecha_Creo = Convert.ToDateTime(Dt_Historial.Rows[Contador]["FECHA_FINAL"].ToString().Trim());
                    }
                    Bien_Mueble_Resguardo.P_Observaciones = Dt_Historial.Rows[Contador]["COMENTARIOS"].ToString().Trim();
                    Bien_Mueble_Resguardo.P_Estatus = Dt_Historial.Rows[Contador]["ESTATUS"].ToString().Trim();
                    Bien_Mueble_Resguardo.Alta_Migrar_Resguardos_Bien_Mueble();
                }
            }
        }
    }

    private DataTable Obtener_Historial_Bienes(String Hoja)
    {
        DataSet Ds_Historial = new DataSet();
        String SqlExcel = "Select * From [" + Hoja + "$]";
        Ds_Historial = Leer_Excel(SqlExcel, "MIGRACION_HISTORIAL_RESGUARDOS.xls");
        DataTable Dt_Historial = Ds_Historial.Tables[0];
        return Dt_Historial;
    }

    private String Obtener_Operacion_Bien(String Bien_Mueble_ID)
    {
        String Operacion = null;
        String Mi_SQL = null;
        Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + "='" + Bien_Mueble_ID.Trim() + "'";
        DataSet Ds_Aux_Bienes = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        if (Ds_Aux_Bienes != null && Ds_Aux_Bienes.Tables.Count > 0)
        {
            DataTable Dt_Aux_Bienes = Ds_Aux_Bienes.Tables[0];
            if (Dt_Aux_Bienes != null && Dt_Aux_Bienes.Rows.Count > 0)
            {
                Operacion = Dt_Aux_Bienes.Rows[0][Ope_Pat_Bienes_Muebles.Campo_Operacion].ToString().Trim();
            }
        }
        return Operacion;
    }

}
