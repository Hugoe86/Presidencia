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
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Data.OracleClient;
using System.Data.OleDb;


public partial class paginas_Compras_Frm_Ope_Layaut_Actualizar_Proveedores : System.Web.UI.Page
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
    public DataSet Leer_Excel(String sqlExcel)
    {
        //Para empezar definimos la conexión OleDb a nuestro fichero Excel.
        String Rta = @MapPath("../../Archivos/Proveedores.xlsx");
        string sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
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
        return DS;
    }


    protected void Btn_Actualizar_Proveedores_Click(object sender, EventArgs e)
    {
        DataSet Ds_Proveedores = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        String SqlExcel = "Select * From [Proveedores$]";
        Ds_Proveedores = Leer_Excel(SqlExcel);
        Generar_Sentencia_Proveedores(Ds_Proveedores);
        

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Proveedores
    ///DESCRIPCIÓN:          Metodo que inserta los datos de los proveedores del archivo excel a la tabla CAT_COM_PROVEEDORES. 
    ///PARAMETROS:           Ds_Proveedores.- DataSet que contiene los proveedores del archivo de excel
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           24/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Proveedores(DataSet Ds_Proveedores)
    {
        String Mi_SQL = "";
        String Proveedor_ID = "";
        String Estatus = "";
        try
        {
            for (int i = 0; i < Ds_Proveedores.Tables[0].Rows.Count; i++)
            {
                //Validamos el Estatus 
                switch (Ds_Proveedores.Tables[0].Rows[i]["ESTATUS"].ToString().Trim())
                {
                    case "VIGENTE":
                        Estatus = "ACTIVO";
                        break;
                    case "BAJA":
                        Estatus = "INACTIVO";
                        break;
                    case "PROVISIONAL":
                        Estatus = "PROVISIONAL";
                        break;

                }

                //vALIDAMOS K NO EXISTA EL PROVEEDOR

                Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + "='" + String.Format("{0:0000000000}", Convert.ToInt32(Ds_Proveedores.Tables[0].Rows[i]["N_PROV"].ToString().Trim())) + "'";
                Proveedor_ID = Ds_Proveedores.Tables[0].Rows[i]["N_PROV"].ToString().Trim();
                DataTable Dt_Proveedores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                if (Dt_Proveedores.Rows.Count == 0)
                {
                    Mi_SQL = "INSERT INTO " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                    Mi_SQL = Mi_SQL + " (" + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Nombre + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Compañia + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_RFC + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Contacto + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Estatus + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Direccion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Colonia + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Ciudad + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Estado + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_1 + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_2 + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Nextel + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fax + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Correo_Electronico + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Representante_Legal + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fecha_Registro + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fecha_Actualizacion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fecha_Creo + ") ";
                    //Mi_SQL = Mi_SQL + "VALUES('" + Ds_Proveedores.Tables[0].Rows[i]["N_PROV"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + "VALUES('" + String.Format("{0:0000000000}", Convert.ToInt32(Ds_Proveedores.Tables[0].Rows[i]["N_PROV"].ToString().Trim())) + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["RAZON_SOCIAL"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["NOMBRE_COMERCIAL"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["RFC"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["CONTACTO"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Estatus + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["DOMICILIO"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["COLONIA"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["CIUDAD"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["ESTADO"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["TELEFONO"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["CEL"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["NEXTEL"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["FAX"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["E-MAIL"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["REPRESENTANTE_LEGAL"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["FECHA_DE_REGISTRO"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["FECHA_ACTUALIZACION"].ToString().Trim() + "','";
                    Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";
                    //Damos de alta a los Proveedores
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Damos de alta el Concepto perteneciente a este proveedor o la partida
                    String[] Arr = Ds_Proveedores.Tables[0].Rows[i]["Conceptos"].ToString().Trim().Split(',');
                    if (Arr.Length > 0)
                    {
                        for (int y = 0; y < Arr.Length; y++)
                        {
                            Alta_Concepto_Proveedores(String.Format("{0:0000000000}", Convert.ToInt32(Ds_Proveedores.Tables[0].Rows[i]["N_PROV"].ToString().Trim())), Arr[y]);
                        }
                    }
                }
                else
                {
                    //Si no esta dado de alta actualizamos el Proveedor
                    Mi_SQL = "";
                    Mi_SQL = "UPDATE " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                    Mi_SQL = Mi_SQL + " SET ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Nombre + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["RAZON_SOCIAL"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Compañia + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["NOMBRE_COMERCIAL"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_RFC + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["RFC"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Contacto + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["CONTACTO"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Estatus + "='";
                    Mi_SQL = Mi_SQL + Estatus + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Direccion + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["DOMICILIO"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Colonia + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["COLONIA"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Ciudad + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["CIUDAD"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Estado + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["ESTADO"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_1 + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["TELEFONO"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_2 + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["CEL"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Nextel + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["NEXTEL"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Correo_Electronico + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["E-MAIL"].ToString().Trim() + "',";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Representante_Legal + "='";
                    Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["REPRESENTANTE_LEGAL"].ToString().Trim() + "'";
                    if (Ds_Proveedores.Tables[0].Rows[i]["FECHA_DE_REGISTRO"].ToString().Trim() != String.Empty)
                    {

                        Mi_SQL = Mi_SQL + ", " +  Cat_Com_Proveedores.Campo_Fecha_Registro + "='";
                        Mi_SQL = Mi_SQL + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Ds_Proveedores.Tables[0].Rows[i]["FECHA_DE_REGISTRO"].ToString().Trim())) + "'";
                    }
                    if (Ds_Proveedores.Tables[0].Rows[i]["FECHA_ACTUALIZACION"].ToString().Trim() != String.Empty)
                    {
                        Mi_SQL = Mi_SQL + ", " + Cat_Com_Proveedores.Campo_Fecha_Actualizacion + "='";
                        Mi_SQL = Mi_SQL + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime( Ds_Proveedores.Tables[0].Rows[i]["FECHA_ACTUALIZACION"].ToString().Trim())) + "'";
                    }
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + "='" + String.Format("{0:0000000000}", Convert.ToInt32(Ds_Proveedores.Tables[0].Rows[i]["N_PROV"].ToString().Trim())) + "'";
                    
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }

            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('Proveedores Guardados ');", true);
        }//fin del try
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message + " " +Proveedor_ID);
            //EN CASO DE EXISTIR UN ERROR ELIMINAMOS LOS GIROS 
            Mi_SQL = "DELETE FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            //ELIMINAMOS LOS PROVEEDORES EN CASO DE OCURRIR UN ERROR
            Mi_SQL = "DELETE FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

    }//fin del metodo Generar_Sentencia

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Alta_Concepto_Proveedores
    ///DESCRIPCIÓN:          Metodo que inserta los CONCEPTOS de los proveedores del archivo excel a la tabla CAT_COM_GIRO_PROVEEDORES. 
    ///PARAMETROS:           String Proveedor_ID- Id del proveedor al que le pertenece el concepto
    ///                      String Concepto    
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           24/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Alta_Concepto_Proveedores(String Proveedor_ID, String Concepto_Partida)
    {
        //Verificamos el tipo si es concepto es partida.
        String Mi_SQL = "";

        Mi_SQL = "SELECT " + Cat_Sap_Concepto.Campo_Clave;
        Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
        Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Clave;
        Mi_SQL = Mi_SQL + "='" + Concepto_Partida.Trim() + "'";

        DataTable Dt_Conceptos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        Mi_SQL = "SELECT " + Cat_Sap_Partidas_Genericas.Campo_Clave;
        Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
        Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Genericas.Campo_Clave;
        Mi_SQL = Mi_SQL + "='" + Concepto_Partida.Trim() + "'";

        DataTable Dt_Partidas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

        if (Dt_Conceptos.Rows.Count > 0)
        {
            Mi_SQL = "INSERT INTO " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;
            Mi_SQL = Mi_SQL + " (" + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + ",";
            Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Giro_ID + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Usuario_Creo + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo + ")";
            Mi_SQL = Mi_SQL + " VALUES('" + Proveedor_ID + "',(SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Clave + "= '" + Concepto_Partida.Trim() + "'),";
            Mi_SQL = Mi_SQL + "'" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";

            //Damos de alta el concepto de los proveedores
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }

        if (Dt_Partidas.Rows.Count > 0)
        {

            Mi_SQL = "INSERT INTO " + Cat_Com_Det_Part_Prov.Tabla_Cat_Com_Det_Part_Prov;
            Mi_SQL = Mi_SQL + " (" + Cat_Com_Det_Part_Prov.Campo_Proveedor_ID + ",";
            Mi_SQL = Mi_SQL + Cat_Com_Det_Part_Prov.Campo_Partida_Generica_ID + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Det_Part_Prov.Campo_Usuario_Creo + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Det_Part_Prov.Campo_Fecha_Creo + ")";
            Mi_SQL = Mi_SQL + " VALUES('" + Proveedor_ID + "',(SELECT " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID;
            Mi_SQL = Mi_SQL + " FROM " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica;
            Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Partida_Generica.Campo_Clave + "= '" + Concepto_Partida.Trim() + "'),";
            Mi_SQL = Mi_SQL + "'CARGA INICIAL',SYSDATE)";

            //Damos de alta el concepto de los proveedores
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }




    }
}
