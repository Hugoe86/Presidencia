using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;using Presidencia.Generar_Requisicion.Negocio;
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
using Presidencia.Generar_Requisicion.Datos;

//using Microsoft.Office.Interop.Excel;
public partial class paginas_Compras_Frm_Archivo_Excel : System.Web.UI.Page
{

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Cls_Sessiones.Mostrar_Menu = true;
            Cls_Sessiones.Nombre_Empleado = "CARGA INICIAL";
        }
    }

    #region Metodos

    #region Capitulos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Capitulos
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_SAP_CAPITULOS. 
    ///PARAMETROS:           Ds_Capitulos.- DataSet que contiene los capitulos del archivo de excel
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           30/Mayo/2011 
    ///MODIFICO:             
    ///FECHA_MODIFICO:       
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Capitulos(DataSet Ds_Capitulos)
    {
        String Mi_SQL = "";
        String Capitulo_ID = "";
        try
        {
        for (int i = 0; i < Ds_Capitulos.Tables[0].Rows.Count; i++)
        {
            Capitulo_ID = Consecutivo(Cat_SAP_Capitulos.Campo_Capitulo_ID,Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos);
            Mi_SQL = "INSERT INTO " + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + " (";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Capitulo_ID + ", ";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Clave + ", ";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Descripcion + ", ";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Estatus + ", ";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Usuario_Creo + ", ";
            Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Campo_Fecha_Creo + ") VALUES ('";
            Mi_SQL = Mi_SQL + Capitulo_ID+ "', '";
            Mi_SQL = Mi_SQL + Ds_Capitulos.Tables[0].Rows[i]["CLAVE"].ToString().Trim().ToUpper() + "', '";
            Mi_SQL = Mi_SQL + Ds_Capitulos.Tables[0].Rows[i]["DESCRIPCION"].ToString().Trim().ToUpper() + "', '";
            Mi_SQL = Mi_SQL + Ds_Capitulos.Tables[0].Rows[i]["ESTATUS"].ToString().Trim().ToUpper() + "', '";
            Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Capitulos", "alert('Capitulos Guardados ');", true);
        }//FIN DEL TRY
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
            //EN CASO DE EXISTIR UN ERROR ELIMINAMOS LOS CONCEPTOS
            Mi_SQL = "DELETE FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
    }

    #endregion

    #region Conceptos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Conceptos
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_SAP_CONCEPTOS. 
    ///PARAMETROS:           Ds_Conceptos.- DataSet que contiene los conceptos del archivo de excel
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           30/Mayo/2011 
    ///MODIFICO:             
    ///FECHA_MODIFICO:       
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Conceptos(DataSet Ds_Conceptos)
    {
        String Mi_SQL = "";
        String Concepto_ID = "";
        try
        {
            for (int i = 0; i < Ds_Conceptos.Tables[0].Rows.Count; i++)
            {
                Concepto_ID = Consecutivo(Cat_Sap_Concepto.Campo_Concepto_ID, Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto);
                Mi_SQL = "INSERT INTO " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
                Mi_SQL = Mi_SQL + " (" + Cat_Sap_Concepto.Campo_Concepto_ID + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Capitulo_ID + "," + Cat_Sap_Concepto.Campo_Clave + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Estatus + "," + Cat_Sap_Concepto.Campo_Descripcion + ",";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Campo_Usuario_Creo + "," + Cat_Sap_Concepto.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Concepto_ID + "',(SELECT " + Cat_SAP_Capitulos.Campo_Capitulo_ID + " FROM ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + " WHERE " + Cat_SAP_Capitulos.Campo_Clave;
                Mi_SQL = Mi_SQL + "='" + Ds_Conceptos.Tables[0].Rows[i]["CAPITULO"].ToString().Trim().ToUpper() + "')";
                Mi_SQL = Mi_SQL + ",'" + Ds_Conceptos.Tables[0].Rows[i]["CLAVE"].ToString().Trim().ToUpper();
                Mi_SQL = Mi_SQL + "','" + Ds_Conceptos.Tables[0].Rows[i]["ESTATUS"].ToString().Trim().ToUpper();
                Mi_SQL = Mi_SQL + "','" + Ds_Conceptos.Tables[0].Rows[i]["DESCRIPCION"].ToString().Trim().ToUpper() + "',";
                Mi_SQL = Mi_SQL + "'" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);



            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
            //EN CASO DE EXISTIR UN ERROR ELIMINAMOS LOS CONCEPTOS
            Mi_SQL = "DELETE FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
    }
    #endregion 

    #region Partidas_Generales

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Partidas_Generales
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_SAP_CONCEPTOS. 
    ///PARAMETROS:           Ds_Partidas_Generales.- DataSet que contiene las paryidas generales del archivo de excel
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           30/Mayo/2011 
    ///MODIFICO:             
    ///FECHA_MODIFICO:       
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Partidas_Generales(DataSet Ds_Partidas_Generales)
    {
        String Mi_SQL = "";
        String Partida_Generica_ID = "";
        try
        {
            for (int i = 0; i < Ds_Partidas_Generales.Tables[0].Rows.Count; i++)
            {
                Partida_Generica_ID = Consecutivo(Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID, Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica);
                Mi_SQL = "INSERT INTO " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas + " (";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Partida_Generica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Concepto_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Genericas.Campo_Fecha_Creo + ") VALUES(";
                Mi_SQL = Mi_SQL + "'" + Partida_Generica_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Ds_Partidas_Generales.Tables[0].Rows[i]["CLAVE"].ToString().Trim().ToUpper() + "', ";
                Mi_SQL = Mi_SQL + "'" + Ds_Partidas_Generales.Tables[0].Rows[i]["DESCRIPCION"].ToString().Trim().ToUpper() + "', ";
                Mi_SQL = Mi_SQL + "'" + Ds_Partidas_Generales.Tables[0].Rows[i]["ESTATUS"].ToString().Trim().ToUpper() + "', ";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Clave + "='" + 
                    Ds_Partidas_Generales.Tables[0].Rows[i]["CONCEPTO"].ToString().Trim().ToUpper() + "')";
                Mi_SQL = Mi_SQL + ",'" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }//fin del for
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Partidas Generales", "alert('Partidas Generales Guardadas ');", true);
        }//Fin del try
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
            //EN CASO DE EXISTIR UN ERROR ELIMINAMOS LOS CONCEPTOS
            Mi_SQL = "DELETE FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
    }

    #endregion

    #region Partidas_Especificas
     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Partidas_Especificas
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_SAP_CONCEPTOS. 
    ///PARAMETROS:           Ds_Partidas_Generales.- DataSet que contiene las paryidas generales del archivo de excel
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           30/Mayo/2011 
    ///MODIFICO:             
    ///FECHA_MODIFICO:       
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Partidas_Especificas(DataSet Ds_Partidas_Especificas)
    {
        String Mi_SQL = "";
        String Partida_Especifica_ID = "";
        try
        {
            for (int i = 0; i < Ds_Partidas_Especificas.Tables[0].Rows.Count; i++)
            {
                Partida_Especifica_ID = Consecutivo_10(Cat_Sap_Partidas_Especificas.Campo_Partida_ID, Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_SQL = "INSERT INTO " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " (";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Partida_Generica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Partidas.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Partida_Especifica_ID + "', ";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + " FROM " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_SAP_Partida_Generica.Campo_Clave + "='" + Ds_Partidas_Especificas.Tables[0].Rows[i]["PARTIDA_GENERICA"].ToString().Trim().ToUpper() + "'), '";
                Mi_SQL = Mi_SQL + Ds_Partidas_Especificas.Tables[0].Rows[i]["CLAVE"].ToString().Trim().ToUpper() + "', '";
                Mi_SQL = Mi_SQL + Ds_Partidas_Especificas.Tables[0].Rows[i]["NOMBRE"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Partidas_Especificas.Tables[0].Rows[i]["DESCRIPCION"].ToString().Trim().ToUpper() + "', '";
                Mi_SQL = Mi_SQL + Ds_Partidas_Especificas.Tables[0].Rows[i]["ESTATUS"].ToString().Trim().ToUpper() + "', '";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }//fin del for
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Partidas Especificas", "alert('Partidas Especificas Guardadas ');", true);
        }//Fin del try
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
            //EN CASO DE EXISTIR UN ERROR ELIMINAMOS LOS CONCEPTOS
            Mi_SQL = "DELETE FROM " + Cat_Sap_Partidas_Genericas.Tabla_Cat_Sap_Partidas_Genericas;
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }
    }


    #endregion

    #region Modelos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Modelos
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_COM_MODELOS. 
    ///PARAMETROS:           Ds_Proveedores.- DataSet que contiene los modelos del archivo de excel
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           26/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Modelos(DataSet Ds_Modelos)
    {
        String Mi_SQL = "";
        String Modelo_ID = "";
        String Fecha = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();

        try
        {
            for (int i = 0; i < Ds_Modelos.Tables[0].Rows.Count; i++)
            {
                if (Ds_Modelos.Tables[0].Rows[i]["Nombre"].ToString().Trim() != "")
                {
                    Modelo_ID = Consecutivo(Cat_Com_Modelos.Campo_Modelo_ID, Cat_Com_Modelos.Tabla_Cat_Com_Modelos);
                    Mi_SQL = "INSERT INTO " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " (";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Modelo_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Nombre + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES('" + Modelo_ID + "', '";
                    Mi_SQL = Mi_SQL + Ds_Modelos.Tables[0].Rows[i]["Nombre"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', '";
                    Mi_SQL = Mi_SQL + Fecha + "')";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Modelos", "alert('Modelos Guardadas ');", true);
        }

        catch (OracleException ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
        catch (DBConcurrencyException ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    #endregion Modelos

    #region Productos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Productos
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_COM_PRODUCTOS. 
    ///PARAMETROS:           Ds_Productos.- DataSet que contiene los productos del archivo de excel
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           26/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Productos(DataSet Ds_Productos)
    {
        String Mi_SQL = "";
        String Producto_ID = "";
        String Unidad_ID = "";
        String Modelo_ID = "";
        String Impuesto1_ID = "";
        String Impuesto2_ID = "";
        String Partida_Especifica_ID = "";
        String Fecha = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();

        try
        {
            for (int i = 0; i < Ds_Productos.Tables[0].Rows.Count; i++)
            {
                if ((Ds_Productos.Tables[0].Rows[i]["Descripcion Corta"].ToString().Trim() != "") && (Ds_Productos.Tables[0].Rows[i]["Descripcion Larga"].ToString().Trim()) != "")
                {
                    if (Ds_Productos.Tables[0].Rows[i]["Unidad"].ToString().Trim() != "")
                        Unidad_ID = Consulta_Unidad_ID(Ds_Productos.Tables[0].Rows[i]["Unidad"].ToString().Trim());

                    Modelo_ID = Ds_Productos.Tables[0].Rows[i]["Modelo"].ToString().Trim();
                    //if (Ds_Productos.Tables[0].Rows[i]["Modelo"].ToString().Trim() != "")
                      //  Modelo_ID = Consulta_Modelo_ID(Ds_Productos.Tables[0].Rows[i]["Modelo"].ToString().Trim());

                    if (Ds_Productos.Tables[0].Rows[i]["Partida Especifica"].ToString().Trim() != "")
                        Partida_Especifica_ID = Consulta_Partida_Especifica_ID(Ds_Productos.Tables[0].Rows[i]["Partida Especifica"].ToString().Trim());

                    if (Ds_Productos.Tables[0].Rows[i]["Impuesto1"].ToString().Trim() != "")
                        Impuesto1_ID = Consulta_Impuesto_ID(Ds_Productos.Tables[0].Rows[i]["Impuesto1"].ToString().Trim());

                    if (Ds_Productos.Tables[0].Rows[i]["Impuesto2"].ToString().Trim() != "")
                        Impuesto2_ID = Consulta_Impuesto_ID(Ds_Productos.Tables[0].Rows[i]["Impuesto2"].ToString().Trim());

                    Producto_ID = Consecutivo_Diez_Digitos(Cat_Com_Productos.Campo_Producto_ID, Cat_Com_Productos.Tabla_Cat_Com_Productos);
                    Mi_SQL = "INSERT INTO " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " (";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Clave + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Nombre + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Descripcion + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Unidad_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo_Promedio + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Impuesto_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Impuesto_2_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Modelo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Stock + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_ID + ", ";

                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Minimo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Maximo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Reorden + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Existencia + ", ";

                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES('" + Producto_ID + "', '";
                    Mi_SQL = Mi_SQL + int.Parse(Producto_ID) +"','";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["Descripcion Corta"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["Descripcion Larga"].ToString().Trim() + " ";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["Descripcion"].ToString().Trim();

                    Mi_SQL = Mi_SQL + "', '";
                    Mi_SQL = Mi_SQL + Unidad_ID + "','";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["Costo Promedio Sin Impuesto"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Impuesto1_ID + "', '";
                    Mi_SQL = Mi_SQL + Impuesto2_ID + "', '";
                    Mi_SQL = Mi_SQL + Modelo_ID + "', '";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["Stock"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Partida_Especifica_ID + "', ";

                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["MINIMO"].ToString().Trim() + ", ";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["MAXIMO"].ToString().Trim() + ", ";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["REORDEN"].ToString().Trim() + ", ";
                    Mi_SQL = Mi_SQL + Ds_Productos.Tables[0].Rows[i]["EXISTENCIA"].ToString().Trim() + ", '";

                    Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', '";
                    Mi_SQL = Mi_SQL + Fecha + "')";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Productos", "alert('Productos Guardados');", true);
        }
        catch (OracleException ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
        catch (DBConcurrencyException ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    #endregion Productos

    #region Unidades
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Sentencia_Unidades
    ///DESCRIPCIÓN:          Metodo que inserta los datos de la tabla de excel a la tabla CAT_COM_UNIDADES. 
    ///PARAMETROS:           Ds_Productos.- DataSet que contiene las Unidades del archivo de excel
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           26/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Sentencia_Unidades(DataSet Ds_Unidades)
    {
        String Mi_SQL = "";
        String Unidad_ID = "";
        String Fecha = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();

        try
        {
            for (int i = 0; i < Ds_Unidades.Tables[0].Rows.Count; i++)
            {
                if ((Ds_Unidades.Tables[0].Rows[i]["Nombre"].ToString().Trim() != "") && (Ds_Unidades.Tables[0].Rows[i]["Abreviatura"].ToString().Trim()) != "")
                {
                    Unidad_ID = Consecutivo(Cat_Com_Unidades.Campo_Unidad_ID, Cat_Com_Unidades.Tabla_Cat_Com_Unidades);
                    Mi_SQL = "INSERT INTO " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " (";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Nombre + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Abreviatura + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Fecha_Creo + ") ";
                    Mi_SQL = Mi_SQL + "VALUES('" + Unidad_ID + "', '";
                    Mi_SQL = Mi_SQL + Ds_Unidades.Tables[0].Rows[i]["Nombre"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Ds_Unidades.Tables[0].Rows[i]["Abreviatura"].ToString().Trim() + "', '";
                    Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', '";
                    Mi_SQL = Mi_SQL + Fecha + "')";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Unidades", "alert('Unidades Guardadas ');", true);
        }
        catch (OracleException ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
        catch (DBConcurrencyException ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
        }
    }

    #endregion 
      
    #region Servicios

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Alta_Servicios
    ///DESCRIPCIÓN:          Metodo que inserta los datos de los servicios de excel a la tabla CAT_COM_SERVICIOS. 
    ///PARAMETROS:           Ds_Servicios.- DataSet que contiene los servicios del archivo de excel
    ///CREO:                 Susana Trigueros Armenta
    ///FECHA_CREO:           24/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Alta_Servicios(DataSet Ds_Servicios)
    {
        String Mi_SQL = "";
        String Servicio_ID = "";
        String Impuesto_ID = "";
        int Clave = 0;
        DataTable Dt_Impuesto = new DataTable();
        try
        {
            for (int i = 0; i < Ds_Servicios.Tables[0].Rows.Count; i++)
            {
                Servicio_ID = Consecutivo(Cat_Com_Servicios.Campo_Servicio_ID, Cat_Com_Servicios.Tabla_Cat_Com_Servicios);
                Clave = int.Parse(Servicio_ID);
                //Validamos si tiene presupuesto y si tiene obtenemos el ID de este 
                if (Ds_Servicios.Tables[0].Rows[i]["Impuesto"].ToString().Trim() == "SI")
                {
                    Mi_SQL = "SELECT " + Cat_Com_Impuestos.Campo_Impuesto_ID +
                        " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                        " WHERE " + Cat_Com_Impuestos.Campo_Nombre + "='IVA'";
                    Dt_Impuesto = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Mi_SQL = "INSERT INTO " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " ( ";
                    Mi_SQL += Cat_Com_Servicios.Campo_Servicio_ID + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Clave + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Nombre + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Costo + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Impuesto_ID + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Partida_ID + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Usuario_Creo + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Fecha_Creo + " )";

                    Mi_SQL += " VALUES( '";
                    Mi_SQL += Servicio_ID + "','";
                    Mi_SQL += Clave + "','";
                    Mi_SQL += Ds_Servicios.Tables[0].Rows[i]["Descripcion"].ToString().Trim() + "', ";
                    Mi_SQL += Ds_Servicios.Tables[0].Rows[i]["CostoSI"].ToString().Trim() + ",'";
                    Mi_SQL += Dt_Impuesto.Rows[0][0] + "', ";
                    Mi_SQL += "(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                    Mi_SQL += " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + "='" + Ds_Servicios.Tables[0].Rows[i]["Partida"].ToString().Trim() + "'),'";
                    Mi_SQL += Cls_Sessiones.Nombre_Empleado + "', SYSDATE )";
                }
                else
                {
                    Mi_SQL = "INSERT INTO " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " ( ";
                    Mi_SQL += Cat_Com_Servicios.Campo_Servicio_ID + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Clave + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Nombre + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Costo + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Partida_ID + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Usuario_Creo + ",";
                    Mi_SQL += Cat_Com_Servicios.Campo_Fecha_Creo + " )";

                    Mi_SQL += " VALUES( '";
                    Mi_SQL += Servicio_ID + "','";
                    Mi_SQL += Clave + "','";
                    Mi_SQL += Ds_Servicios.Tables[0].Rows[i]["Descripcion"].ToString().Trim() + "', ";
                    Mi_SQL += Ds_Servicios.Tables[0].Rows[i]["CostoSI"].ToString().Trim() + ",";
                    Mi_SQL += "(SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                    Mi_SQL += " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                    Mi_SQL += " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + "='" + Ds_Servicios.Tables[0].Rows[i]["Partida"].ToString().Trim() + "'),'";
                    Mi_SQL += Cls_Sessiones.Nombre_Empleado + "', SYSDATE )";
                }
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }//Fin del For

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Servicios", "alert('Servicios Guardados ');", true);
        }//FIN DEL TRY
        catch (Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
            //EN CASO DE EXISTIR UN ERROR ELIMINAMOS LOS SERVICIOS
            Mi_SQL = "DELETE FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios;
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }
    }

    #endregion servicios

    #region Proveedores
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
        String Proveedor_ID="";
        try{
            for (int i = 0; i < Ds_Proveedores.Tables[0].Rows.Count; i++)
            {
                Proveedor_ID = Consecutivo(Cat_Com_Proveedores.Campo_Proveedor_ID, Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores);
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
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_CP + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_1 + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Telefono_2 + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Nextel + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Correo_Electronico + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Forma_Pago + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Fecha_Creo + ") ";
                Mi_SQL = Mi_SQL + "VALUES('" + Proveedor_ID + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Razon Social"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Razon Social"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["RFC"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Contacto"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + "ACTIVO', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Direccion"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Colonia"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Ciudad"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Estado"].ToString().Trim() + "','";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Codigo Postal"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Telefono1"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Telefono2"].ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Nextel"].ToString().Trim() + "','";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Email"].ToString().Trim() + "','";
                Mi_SQL = Mi_SQL + Ds_Proveedores.Tables[0].Rows[i]["Forma de Pago"].ToString().Trim() + "','";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";
                //Damos de alta a los Proveedores
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Damos de alta el Concepto perteneciente a este proveedor
                String[] Arr = Ds_Proveedores.Tables[0].Rows[i]["Conceptos"].ToString().Trim().Split(',');
                if (Arr.Length > 0)
                {
                    for (int y = 0; y < Arr.Length; y++)
                    {
                        Alta_Concepto_Proveedores(Proveedor_ID, Arr[y]);
                    }
                }
                
            }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores", "alert('Proveedores Guardados ');", true);
        }//fin del try
        catch(Exception ex)
        {
            throw new Exception("Error: " + ex.Message);
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
    public void Alta_Concepto_Proveedores(String Proveedor_ID, String Concepto)
    {
        String Mi_SQL = "";
        Mi_SQL = "INSERT INTO " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor;
        Mi_SQL = Mi_SQL + " (" + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + ",";
        Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Giro_ID + ", ";
        Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Usuario_Creo + ", ";
        Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo + ")";
        Mi_SQL = Mi_SQL + " VALUES('" + Proveedor_ID + "',(SELECT " + Cat_Sap_Concepto.Campo_Concepto_ID;
        Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;
        Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Concepto.Campo_Clave + "= '" + Concepto + "'),";
        Mi_SQL = Mi_SQL + "'" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
        //Damos de alta el concepto de los proveedores
        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
    }

    #endregion Proveedores

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
        //String Rta = @MapPath("../../Archivos/PRESUPUESTO_IRAPUATO.xls");
        String Rta = @MapPath("../../Archivos/Datos Compras.xls");
        string sConnectionString = "";// @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Rta + ";Extended Properties=Excel 8.0;";



        if (Rta.Contains(".xlsx"))       // Formar la cadena de conexion si el archivo es Exceml xml
        {
            sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Rta + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        }
        else if (Rta.Contains(".xls"))   // Formar la cadena de conexion si el archivo es Exceml binario
        {
            sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Rta + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
            //sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
            //        "Data Source=" + Rta + ";" +
            //        "Extended Properties=Excel 8.0;";
        }

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


    public DataSet Leer_Excel(String sqlExcel, String Path)
    {
        //Para empezar definimos la conexión OleDb a nuestro fichero Excel.
        //String Rta = @MapPath("../../Archivos/PRESUPUESTO_IRAPUATO.xls");
        String Rta = @MapPath(Path);
        string sConnectionString = "";// @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Rta + ";Extended Properties=Excel 8.0;";

        if (Rta.Contains(".xlsx"))       // Formar la cadena de conexion si el archivo es Exceml xml
        {
            sConnectionString = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Rta + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        }
        else if (Rta.Contains(".xls"))   // Formar la cadena de conexion si el archivo es Exceml binario
        {
            sConnectionString = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + Rta + ";" +
                    "Extended Properties=Excel 8.0;";
        }

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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consecutivo
    ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
    ///PARAMETROS: 
    ///CREO:
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Consecutivo(String ID, String Tabla)
    {
        String Consecutivo = "";
        String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
        Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

        Mi_SQL = "SELECT NVL(MAX (" + ID + "),'00000') ";
        Mi_SQL = Mi_SQL + "FROM " + Tabla;
        Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        if (Convert.IsDBNull(Asunto_ID))
        {
            Consecutivo = "00001";
        }
        else
        {
            Consecutivo = string.Format("{0:00000}", Convert.ToInt32(Asunto_ID) + 1);
        }
        return Consecutivo;
    }//fin de consecutivo

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consecutivo
    ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
    ///PARAMETROS: 
    ///CREO:
    ///FECHA_CREO: 24/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Consecutivo_10(String ID, String Tabla)
    {
        String Consecutivo = "";
        String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
        Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

        Mi_SQL = "SELECT NVL(MAX (" + ID + "),'0000000000') ";
        Mi_SQL = Mi_SQL + "FROM " + Tabla;
        Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        if (Convert.IsDBNull(Asunto_ID))
        {
            Consecutivo = "0000000001";
        }
        else
        {
            Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Asunto_ID) + 1);
        }
        return Consecutivo;
    }//fin de consecutivo

    #endregion

    #region Eventos
    
    protected void Btn_Capitulos_Click(object sender, EventArgs e)
    {
        DataSet Ds_Capitulos = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Capitulos$]";
        Ds_Capitulos = Leer_Excel(SqlExcel);
        Generar_Sentencia_Capitulos(Ds_Capitulos);
    }



    protected void Btn_Conceptos_Click(object sender, EventArgs e)
    {
        DataSet Ds_Conceptos = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Conceptos$]";
        Ds_Conceptos = Leer_Excel(SqlExcel);
        Generar_Sentencia_Conceptos(Ds_Conceptos);
    }

    protected void Btn_Partidas_Generales_Click(object sender, EventArgs e)
    {
        DataSet Ds_Partidas_Generales = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Partidas_Genericas$]";
        Ds_Partidas_Generales = Leer_Excel(SqlExcel);
        Generar_Sentencia_Partidas_Generales(Ds_Partidas_Generales);
    }

    protected void Btn_partidas_Especificas_Click(object sender, EventArgs e)
    {
        DataSet Ds_Partidas_Especificas = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Partidas_Especificas$]";
        Ds_Partidas_Especificas = Leer_Excel(SqlExcel);
        Generar_Sentencia_Partidas_Especificas(Ds_Partidas_Especificas);
    }

    protected void Btn_Subir_Proveedores_Click(object sender, EventArgs e)
    {
        DataSet Ds_Proveedores = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        String SqlExcel = "Select * From [Proveedores$]";
        Ds_Proveedores = Leer_Excel(SqlExcel);

        Generar_Sentencia_Proveedores(Ds_Proveedores);
    }


    protected void Btn_Subir_Servicios_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        DataSet Ds_Servicios = Leer_Excel("Select * From [Servicios$]");
        Alta_Servicios(Ds_Servicios);

    }

     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Modelos_Click
     ///DESCRIPCIÓN:          Evento utilizado para instanciar los métodos necesarios para 
     ///                      Subir los modelos del archivo de excel. 
     ///PARAMETROS:           
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     protected void Btn_Subir_Modelos_Click(object sender, EventArgs e)
     {
        DataSet Ds_Modelos = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Modelos$]";
        Ds_Modelos = Leer_Excel(SqlExcel);

        Generar_Sentencia_Modelos(Ds_Modelos);
     }

     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Productos_Click
     ///DESCRIPCIÓN:          Evento utilizado para instanciar los métodos necesarios para 
     ///                      Subir los productos del archivo de excel. 
     ///PARAMETROS:           
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     protected void Btn_Subir_Productos_Click(object sender, EventArgs e)
     {
        DataSet Ds_Productos = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        String SqlExcel = "Select * From [Productos$]";
        Ds_Productos = Leer_Excel(SqlExcel);
        Generar_Sentencia_Productos(Ds_Productos);
     }

     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Unidades_Click
     ///DESCRIPCIÓN:          Evento utilizado para instanciar los métodos necesarios para 
     ///                      Subir las unidades del archivo de excel. 
     ///PARAMETROS:           
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     protected void Btn_Subir_Unidades_Click(object sender, EventArgs e)
     {
        DataSet Ds_Unidades = new DataSet();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        String SqlExcel = "Select * From [Unidades$]";
        Ds_Unidades = Leer_Excel(SqlExcel);

        Generar_Sentencia_Unidades(Ds_Unidades);
     }
     
    ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Unidades_Click
     ///DESCRIPCIÓN:          Evento utilizado para instanciar los métodos necesarios para 
     ///                      Subir las unidades del archivo de excel. 
     ///PARAMETROS:           
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     protected void Btn_Dependencias_Click(object sender, EventArgs e)
     {
         DataSet Ds_Dependencias = new DataSet();
         Div_Contenedor_Msj_Error.Visible = false;
         Lbl_Mensaje_Error.Text = "";

         String SqlExcel = "Select * From [Dependencias$]";
         Ds_Dependencias = Leer_Excel(SqlExcel);

         Generar_Sentencia_dependencias(Ds_Dependencias);


     }

     public void Generar_Sentencia_dependencias(DataSet Ds_Dependencia)
     {
         String Mi_SQL = "";
         String Dependencia_ID="";
         for(int i=0;i<Ds_Dependencia.Tables[0].Rows.Count; i++)
         {
            Dependencia_ID = Consecutivo(Cat_Dependencias.Campo_Dependencia_ID, Cat_Dependencias.Tabla_Cat_Dependencias);
            Mi_SQL = "INSERT INTO " + Cat_Dependencias.Tabla_Cat_Dependencias;
            Mi_SQL = Mi_SQL + "(" + Cat_Dependencias.Campo_Dependencia_ID + ",";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Clave + ",";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Nombre + ",";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Comentarios + ",";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Estatus + ",";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Usuario_Creo + ",";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Fecha_Creo;
            Mi_SQL = Mi_SQL + ") VALUES ('" + Dependencia_ID;
            Mi_SQL = Mi_SQL + "','" + Ds_Dependencia.Tables[0].Rows[i]["Clave"].ToString();
            Mi_SQL = Mi_SQL + "','" + Ds_Dependencia.Tables[0].Rows[i]["Nombre"].ToString();
            Mi_SQL = Mi_SQL + "','" + Ds_Dependencia.Tables[0].Rows[i]["Descripcion"].ToString();
            Mi_SQL = Mi_SQL + "','ACTIVO"; 
            Mi_SQL = Mi_SQL + "','" + Cls_Sessiones.Nombre_Empleado + "',SYSDATE)";
            //Damos de alta el concepto de los proveedores
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Dependencias", "alert('Dependencias Guardadas ');", true);
        
     }

     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Consecutivo_Diez_Digitos
     ///DESCRIPCIÓN:          Metodo que verfifica el consecutivo en la tabla y ayuda a generar el 
     ///                      nuevo Id con el formatod e 10 caracteres. 
     ///PARAMETROS:           ID.- Contiene el nombre del campo
     ///                      Tabla.- Contiene el nombre de la tabla de la cuals e va a consultar el 
     ///                              identificador mayor
     ///CREO:                 Salvador Hernandez Ramirez
     ///FECHA_CREO:           24/Mayo/2011 
     ///MODIFICO:          
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     public String Consecutivo_Diez_Digitos(String ID, String Tabla)
     {
         String Consecutivo = "";
         String Mi_SQL;         //Obtiene la cadena de inserción hacía la base de datos
         Object Asunto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos

         Mi_SQL = "SELECT NVL(MAX (" + ID + "),'00000') ";
         Mi_SQL = Mi_SQL + "FROM " + Tabla;
         Asunto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

         if (Convert.IsDBNull(Asunto_ID))
         {
             Consecutivo = "0000000001";
         }
         else
         {
             Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Asunto_ID) + 1);
         }
         return Consecutivo;
     }


     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: ID_Partida_Especifica
     ///DESCRIPCIÓN:          Metodo que consulta el Id de la partida especifica. 
     ///PARAMETROS:           Clave.- Clave de la partida especifica
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     public String Consulta_Partida_Especifica_ID(String Clave)
     {
         String Partida_ID = "";
         String Mi_SQL;
         Object Valor;

         Mi_SQL = " SELECT " + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
         Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
         Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + " = '" + Clave + "'";

         Valor = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

         if (!Convert.IsDBNull(Valor))
             Partida_ID = Convert.ToString(Valor);

         return Partida_ID;
     }


     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Consulta_Unidad_ID
     ///DESCRIPCIÓN:          Metodo que consulta el Id de Unidad. 
     ///PARAMETROS:           Unidad.- Nombre de la Unidad
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     public String Consulta_Unidad_ID(String Unidad)
     {
         String Unidad_ID = "";
         String Mi_SQL;
         Object Valor;

         Mi_SQL = " SELECT " + Cat_Com_Unidades.Campo_Unidad_ID;
         Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades;
         Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Unidades.Campo_Abreviatura + " = '" + Unidad + "'";

         Valor = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

         if (!Convert.IsDBNull(Valor))
             Unidad_ID = Convert.ToString(Valor);

         return Unidad_ID;
     }

     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Consulta_Unidad_ID
     ///DESCRIPCIÓN:          Metodo que consulta el Id de Unidad. 
     ///PARAMETROS:           Unidad.- Nombre de la Unidad
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     public String Consulta_Modelo_ID(String Modelo)
     {
         String Modelo_ID = "";
         String Mi_SQL;
         Object Valor;

         Mi_SQL = " SELECT " + Cat_Com_Modelos.Campo_Modelo_ID;
         Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;
         Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Modelos.Campo_Nombre + " = '" + Modelo + "'";

         Valor = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

         if (!Convert.IsDBNull(Valor))
             Modelo_ID = Convert.ToString(Valor);

         return Modelo_ID;
     }


     ///*******************************************************************************
     ///NOMBRE DE LA FUNCIÓN: Consulta_Impuesto_ID
     ///DESCRIPCIÓN:          Metodo que consulta el Id de Impuestos. 
     ///PARAMETROS:           Impuesto.- Nombre del impuesto
     ///CREO:                 Salvador Hernández Ramírez
     ///FECHA_CREO:           26/Mayo/2011 
     ///MODIFICO:
     ///FECHA_MODIFICO:
     ///CAUSA_MODIFICACIÓN:
     ///*******************************************************************************
     public String Consulta_Impuesto_ID(String Impuesto)
     {
         String Impuesto_ID = "";
         String Mi_SQL;
         Object Valor;

         Mi_SQL = " SELECT " + Cat_Com_Impuestos.Campo_Impuesto_ID;
         Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos;
         Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Impuestos.Campo_Nombre + " = '" + Impuesto + "'";

         Valor = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

         if (!Convert.IsDBNull(Valor))
             Impuesto_ID = Convert.ToString(Valor);

         return Impuesto_ID;
     } 
    
    #endregion


     protected void Btn_Programas_Click(object sender, EventArgs e)
     {
         DataSet Ds_Programas = new DataSet();
         Div_Contenedor_Msj_Error.Visible = false;
         Lbl_Mensaje_Error.Text = "";

         String SqlExcel = "Select * From [Programas$]";
         Ds_Programas = Leer_Excel(SqlExcel);

         Generar_Sentencia_Programas(Ds_Programas);
     }

     private String Consultar_Tablas(String Campo, String Tabla, String Where, String Condicion) 
     {
         String Mi_Sql = "";
         String Dato = "";
         Mi_Sql = "SELECT " + Campo + " FROM " + Tabla + " WHERE " + Where + " = '" + Condicion + "'";
         DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
         if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0)
         {
             Dato = _DataSet.Tables[0].Rows[0][Campo].ToString().Trim();
         }
         else 
         {
             Dato = "";
         }
         return Dato;
     }

    protected void Btn_Presupuestos_Click(object sender, EventArgs e)
     {
         DataSet Ds_Programas = new DataSet();
         Div_Contenedor_Msj_Error.Visible = false;
         Lbl_Mensaje_Error.Text = "";

         String SqlExcel = "Select * From [PRESUPUESTO$]";
         Ds_Programas = Leer_Excel(SqlExcel,"../../Archivos/PRESUPUESTO_IRAPUATO.xlsx");
         DataTable Dt_Presupuesto = Ds_Programas.Tables[0];
         String Clave_Presupuesto_Total = "";
         String Clave_Fuente = "";
         String Clave_Area = "";         
         String Clave_Programa = "";
         String Clave_Unidad = "";
         String Clave_Capitulo = "";
         String Clave_Partida = "";
         String Aux = "";
         String Mi_Sql = "";

         String ConsecutivoID = "";
         String Clave_Aux = "";
         String [] Arreglo = null;
         String Descripcion = "";

         int gral = 0;

         char[] ch = { ' ' };
        
         for (int i = Dt_Presupuesto.Rows.Count-1; i >= 4; i-- )
         {

             Aux = Dt_Presupuesto.Rows[i][0].ToString().Trim();
             if (Aux.Contains("******"))
             {
                 Clave_Presupuesto_Total = Aux;
             }
                 //FUENTE DE FINANCIAMIENTO
             else if (Aux.Contains("*****"))
             {
                 Aux = Aux.Trim();
                 Arreglo = Aux.Split(ch);
                 Aux = Arreglo[1].Trim();
                 Clave_Fuente = Arreglo[1].Trim();
                 Aux = Consultar_Tablas("FUENTE_FINANCIAMIENTO_ID", "CAT_SAP_FTE_FINANCIAMIENTO","CLAVE", Aux);//5
                 //si no existe fuente insertarla
                 if (Aux != "")
                 {
                     Clave_Fuente = Aux;
                 }
                 else
                 {
                     Descripcion = "";
                     for (int j = 2; j < Arreglo.Length; j++)
                     {
                         Descripcion += Arreglo[j].Trim();
                     }
                     ConsecutivoID = Consecutivo("FUENTE_FINANCIAMIENTO_ID", "CAT_SAP_FTE_FINANCIAMIENTO");
                     Mi_Sql = "INSERT INTO CAT_SAP_FTE_FINANCIAMIENTO (FUENTE_FINANCIAMIENTO_ID," +
                         "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                         "('" + ConsecutivoID + "','" + Clave_Fuente + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     Clave_Fuente = ConsecutivoID;
                 }
             }
             else if (Aux.Contains("****"))
             {
                 Aux = Aux.Trim();
                 Arreglo = Aux.Split(ch);
                 Aux = Arreglo[1].Trim();
                 Clave_Area = Arreglo[1].Trim();
                 Aux = Consultar_Tablas("AREA_FUNCIONAL_ID", "CAT_SAP_AREA_FUNCIONAL", "CLAVE", Aux);//5
                 //si no existe fuente insertarla
                 if (Aux != "")
                 {
                     Clave_Area = Aux;
                 }
                 else
                 {
                     Descripcion = "";
                     for (int j = 2; j < Arreglo.Length; j++)
                     {
                         Descripcion += Arreglo[j].Trim();
                     }
                     ConsecutivoID = Consecutivo("AREA_FUNCIONAL_ID", "CAT_SAP_AREA_FUNCIONAL");
                     Mi_Sql = "INSERT INTO CAT_SAP_AREA_FUNCIONAL (AREA_FUNCIONAL_ID," +
                         "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                         "('" + ConsecutivoID + "','" + Clave_Area + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     Clave_Area = ConsecutivoID;
                 }
             }
             else if (Aux.Contains("***"))
             {

                 Aux = Aux.Trim();
                 Arreglo = Aux.Split(ch);
                 Aux = Arreglo[1].Trim();
                 Clave_Programa = Arreglo[1].Trim();
                 Aux = Consultar_Tablas("PROYECTO_PROGRAMA_ID", "CAT_SAP_PROYECTOS_PROGRAMAS", "CLAVE", Aux);//10
                 //si no existe fuente insertarla
                 if (Aux != "")
                 {
                     Clave_Programa = Aux;
                 }
                 else
                 {
                     Descripcion = "";
                     for (int j = 2; j < Arreglo.Length; j++)
                     {
                         Descripcion += Arreglo[j].Trim();
                     }
                     ConsecutivoID = Consecutivo_10("PROYECTO_PROGRAMA_ID", "CAT_SAP_PROYECTOS_PROGRAMAS");
                     Mi_Sql = "INSERT INTO CAT_SAP_PROYECTOS_PROGRAMAS (PROYECTO_PROGRAMA_ID," +
                         "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                         "('" + ConsecutivoID + "','" + Clave_Programa + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     Clave_Programa = ConsecutivoID;
                 }
             }
             else if (Aux.Contains("**"))
             {
                 Aux = Aux.Trim();
                 Arreglo = Aux.Split(ch);
                 Aux = Arreglo[1].Trim();
                 Clave_Unidad = Arreglo[1].Trim();
                 Aux = Consultar_Tablas("DEPENDENCIA_ID", "CAT_DEPENDENCIAS", "CLAVE", Aux);//5
                 //si no existe fuente insertarla
                 if (Aux != "")
                 {
                     Clave_Unidad = Aux;
                     Mi_Sql = "UPDATE CAT_DEPENDENCIAS SET AREA_FUNCIONAL_ID ='" + Clave_Area + "' WHERE DEPENDENCIA_ID ='" +
                         Clave_Unidad + "'";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 }
                 else
                 {
                     Descripcion = "";
                     for (int j = 2; j < Arreglo.Length; j++)
                     {
                         Descripcion += Arreglo[j].Trim();
                     }
                     ConsecutivoID = Consecutivo("DEPENDENCIA_ID", "CAT_DEPENDENCIAS");
                     Mi_Sql = "INSERT INTO CAT_DEPENDENCIAS (DEPENDENCIA_ID," +
                         "CLAVE,NOMBRE,USUARIO_CREO,FECHA_CREO,AREA_FUNCIONAL_ID,ESTATUS) VALUES " +
                         "('" + ConsecutivoID + "','" + Clave_Unidad + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'" +
                         Clave_Area + "','ACTIVO')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     Clave_Unidad = ConsecutivoID;
                 }
                 //Insertar la relacion enter programas y unidades
                 //verificar si ya existe
                 Mi_Sql = "SELECT * FROM CAT_SAP_DET_PROG_DEPENDENCIA WHERE DEPENDENCIA_ID = '" + Clave_Unidad + "'";
                 Mi_Sql += " AND PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                 DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0){ }
                 else
                 {
                     Mi_Sql = "INSERT INTO CAT_SAP_DET_PROG_DEPENDENCIA (DEPENDENCIA_ID,PROYECTO_PROGRAMA_ID) VALUES " +
                     "('" + Clave_Unidad + "','" + Clave_Programa + "')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 }
                 //Insertar relación entre Fuentes de Financiamiento y UR
                 Mi_Sql = "SELECT * FROM CAT_SAP_DET_FTE_DEPENDENCIA WHERE DEPENDENCIA_ID = '" + Clave_Unidad + "'";
                 Mi_Sql += " AND FUENTE_FINANCIAMIENTO_ID = '" + Clave_Fuente + "'";
                 _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                 else
                 {
                     Mi_Sql = "INSERT INTO CAT_SAP_DET_FTE_DEPENDENCIA (DEPENDENCIA_ID,FUENTE_FINANCIAMIENTO_ID) VALUES " +
                     "('" + Clave_Unidad + "','" + Clave_Fuente + "')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 }                 
             }
             else if (Aux.Contains("*"))
             {
                 Aux = Aux.Trim();
                 Arreglo = Aux.Split(ch);
                 Aux = Arreglo[1].Trim();
                 Clave_Capitulo = Arreglo[1].Trim();
                 Aux = Consultar_Tablas("CAPITULO_ID", "CAT_SAP_CAPITULO", "CLAVE", Aux);//5
                 //si no existe fuente insertarla
                 if (Aux != "")
                 {
                     Clave_Capitulo = Aux;
                 }
                 else
                 {
                     Descripcion = "";
                     for (int j = 2; j < Arreglo.Length; j++)
                     {
                         Descripcion += Arreglo[j].Trim();
                     }
                     ConsecutivoID = Consecutivo("CAPITULO_ID", "CAT_SAP_CAPITULO");
                     Mi_Sql = "INSERT INTO CAT_SAP_CAPITULO (CAPITULO_ID," +
                         "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                         "('" + ConsecutivoID + "','" + Clave_Capitulo + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     Clave_Capitulo = ConsecutivoID;
                 }
                
                 Clave_Partida = "";                                  
             }
                 //AQUI ENTRA A PARTIDA
             else if (Aux.Contains("*") == false)
             {

                 Aux = Aux.Trim();
                 Arreglo = Aux.Split(ch);
                 Aux = Arreglo[0].Trim();
                 Clave_Partida = Arreglo[0].Trim();
                 Aux = Consultar_Tablas("PARTIDA_ID", "CAT_SAP_PARTIDAS_ESPECIFICAS", "CLAVE", Aux);//5
                 //si no existe fuente insertarla
                 if (Aux != "")
                 {
                     Clave_Partida = Aux;
                 }
                 else
                 {
                     Descripcion = "";
                     for (int j = 2; j < Arreglo.Length; j++)
                     {
                         Descripcion += Arreglo[j].Trim();
                     }
                     //double Double_Clave_Partida = double.Parse(Clave_Partida);
                     String Clave_Partida_Generica = Clave_Partida.Substring(0, 3) + "0";
                     Clave_Partida_Generica = Consultar_Tablas("PARTIDA_GENERICA_ID", "CAT_SAP_PARTIDA_GENERICA", "CLAVE", Clave_Partida_Generica);//5
                     ConsecutivoID = Consecutivo_10("PARTIDA_ID", "CAT_SAP_PARTIDAS_ESPECIFICAS");
                     Mi_Sql = "INSERT INTO CAT_SAP_PARTIDAS_ESPECIFICAS (PARTIDA_ID," +
                         "CLAVE,DESCRIPCION,NOMBRE,USUARIO_CREO,FECHA_CREO,ESTATUS,PARTIDA_GENERICA_ID) VALUES " +
                         "('" + ConsecutivoID + "','" + Clave_Partida + "','" + Descripcion + "','" + Descripcion + "','CARGA INICIAL',SYSDATE,'ACTIVO','" +
                         Clave_Partida_Generica +"')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     Clave_Partida = ConsecutivoID;
                     ////Insertar la relación entre Programas y Partidas
                     //Mi_Sql = "SELECT * FROM CAT_SAP_DET_PROG_PARTIDAS WHERE PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                     //Mi_Sql += " AND PARTIDA_ID = '" + Clave_Partida + "'";
                     //DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     //if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                     //else
                     //{
                     //    String ID = Consecutivo("DET_PROG_PARTIDAS_ID", "CAT_SAP_DET_PROG_PARTIDAS");
                     //    Mi_Sql = "INSERT INTO CAT_SAP_DET_PROG_PARTIDAS (DET_PROG_PARTIDAS_ID,DEPENDENCIA_ID,FUENTE_FINANCIAMIENTO_ID) VALUES " +
                     //    "('" + ID + "','" + Clave_Programa + "','" + Clave_Partida + "')";
                     //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     //}
                 }


                 //Insertar la relación entre Programas y Partidas

                 Mi_Sql = "SELECT * FROM CAT_SAP_DET_PROG_PARTIDAS WHERE PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                 Mi_Sql += " AND PARTIDA_ID = '" + Clave_Partida + "'";
                 DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                 else
                 {
                     String ID = Consecutivo("DET_PROG_PARTIDAS_ID", "CAT_SAP_DET_PROG_PARTIDAS");
                     Mi_Sql = "INSERT INTO CAT_SAP_DET_PROG_PARTIDAS (DET_PROG_PARTIDAS_ID,PROYECTO_PROGRAMA_ID,PARTIDA_ID) VALUES " +
                     "('" + ID + "','" + Clave_Programa + "','" + Clave_Partida + "')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 } 
                 
                 //##################################

                 //insertamos el presupuesto

                 if (Clave_Presupuesto_Total != "" && Clave_Fuente != "" &&
                     Clave_Area != "" && Clave_Programa != "" && Clave_Unidad != "" &&
                     Clave_Capitulo != "" && Clave_Partida != "")
                 {
                     //verificar si ya existe
                     Mi_Sql = "SELECT * FROM OPE_SAP_DEP_PRESUPUESTO WHERE DEPENDENCIA_ID = '" + Clave_Unidad + "'";
                     Mi_Sql += " AND FUENTE_FINANCIAMIENTO_ID = '" + Clave_Fuente + "'";
                     Mi_Sql += " AND PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "'";
                     Mi_Sql += " AND PARTIDA_ID = '" + Clave_Partida + "'";
                     DataSet _DataSet1 = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     String Insert = "";
                     if (_DataSet1 != null && _DataSet1.Tables[0].Rows.Count > 0)
                     {
                         //Si existe lo actualiza
                         Mi_Sql = "UPDATE OPE_SAP_DEP_PRESUPUESTO SET " +
                          "MONTO_PRESUPUESTAL = ";
                         Insert = (Dt_Presupuesto.Rows[i][1].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][1].ToString().Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Mi_Sql += "MONTO_AMPLIACION = " +
                             (Insert = (Dt_Presupuesto.Rows[i][2].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][2].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_REDUCCION = " + (Insert = (Dt_Presupuesto.Rows[i][3].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][3].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_MODIFICADO = " + (Insert = (Dt_Presupuesto.Rows[i][4].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][4].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_DEVENGADO = " + (Insert = (Dt_Presupuesto.Rows[i][5].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][5].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_PAGADO = " + (Insert = (Dt_Presupuesto.Rows[i][6].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][6].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_DEVEGANDO_PAGADO = " + (Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_COMPROMETIDO_REAL = " + (Insert = (Dt_Presupuesto.Rows[i][9].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][9].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_DISPONIBLE = " + (Insert = (Dt_Presupuesto.Rows[i][11].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][11].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "MONTO_EJERCIDO = " + (Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0")) + "," +
                         "FECHA_MODIFICO = SYSDATE," +
                         "USUARIO_MODIFICO = '" + Cls_Sessiones.Nombre_Empleado + "'" +
                         " WHERE " +
                         "FUENTE_FINANCIAMIENTO_ID = '" + Clave_Fuente + "' " +
                         "AND PROYECTO_PROGRAMA_ID = '" + Clave_Programa + "' " +
                         "AND DEPENDENCIA_ID = '" + Clave_Unidad + "' " +
                         "AND PARTIDA_ID = '" + Clave_Partida + "'";
                         OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     }
                     else
                     {
                         //Si no existe lo da de alta

                         Mi_Sql = "INSERT INTO OPE_SAP_DEP_PRESUPUESTO " +
                         "(FUENTE_FINANCIAMIENTO_ID,PROYECTO_PROGRAMA_ID,DEPENDENCIA_ID,PARTIDA_ID,CAPITULO_ID,AREA_FUNCIONAL_ID," +
                         "MONTO_PRESUPUESTAL, MONTO_AMPLIACION, MONTO_REDUCCION, MONTO_MODIFICADO," +
                         "MONTO_DEVENGADO, MONTO_PAGADO, MONTO_DEVEGANDO_PAGADO, MONTO_COMPROMETIDO_REAL," +
                         "MONTO_DISPONIBLE, MONTO_COMPROMETIDO,MONTO_EJERCIDO,ANIO_PRESUPUESTO,NO_ASIGNACION_ANIO,FECHA_ASIGNACION,FECHA_CREO,USUARIO_CREO)" +
                         " VALUES (" +
                         "'" + Clave_Fuente + "'," +
                         "'" + Clave_Programa + "'," +
                         "'" + Clave_Unidad + "'," +
                         "'" + Clave_Partida + "'," +
                         "'" + Clave_Area + "'," +
                         "'" + Clave_Capitulo + "',";
                         Insert = (Dt_Presupuesto.Rows[i][1].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][1].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][2].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][2].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][3].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][3].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][4].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][4].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][5].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][5].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][6].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][6].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][9].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][9].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Insert = (Dt_Presupuesto.Rows[i][11].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][11].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Mi_Sql += "0.00,";
                         Insert = (Dt_Presupuesto.Rows[i][7].ToString().Trim().Length > 0 ? Dt_Presupuesto.Rows[i][7].ToString().Replace(",", "").Trim() : "0");
                         Mi_Sql += Insert + ",";
                         Mi_Sql += "2011,1,SYSDATE," +
                         "SYSDATE,'" + Cls_Sessiones.Nombre_Empleado + "')";
                         OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                     }
                 }
                 //##################################
             }
         }
         //System.Windows.Forms.MessageBox.Show("Presupuestos Actualizados");
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas", "alert('Presupuestos Actualizados');", true);
        // Generar_Sentencia_Programas(Ds_Programas);
     }


     
     public void Generar_Sentencia_Programas(DataSet Ds_Programas)
     {
         String Mi_SQL = "";
         String Programa_ID = "";
         //for (int i = 0; i < Ds_Programas.Tables[0].Rows.Count; i++)
         //{
         //    Programa_ID = Consecutivo_10(Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID, Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas);
         //    Mi_SQL = "INSERT INTO " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
         //    Mi_SQL = Mi_SQL + "(" + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + ",";
         //    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Clave + ",";
         //    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Nombre + ",";
         //    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Descripcion + ",";
         //    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Estatus+ ",";
         //    Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP;
         //    Mi_SQL = Mi_SQL + ") VALUES ('" + Programa_ID;
         //    Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["Clave"].ToString();
         //    Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["Nombre"].ToString();
         //    Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["Descripcion"].ToString();
         //    Mi_SQL = Mi_SQL + "','ACTIVO','1111')";
         //    //Damos de alta el concepto de los proveedores
         //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
         //}

         for (int i = 0; i < Ds_Programas.Tables[0].Rows.Count; i++)
         {
             String Resultado = Consultar_Tablas("CLAVE", Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas, "CLAVE", Ds_Programas.Tables[0].Rows[i]["Clave"].ToString());
             if (String.IsNullOrEmpty(Resultado))
             {
                 Programa_ID = Consecutivo_10
                     (Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID,
                      Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas);
                 Mi_SQL = "INSERT INTO " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
                 Mi_SQL = Mi_SQL + "(" + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + ",";
                 Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Clave + ",";
                 Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Nombre + ",";
                 Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Descripcion + ",";
                 Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Estatus + ",";
                 Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP;
                 Mi_SQL = Mi_SQL + ") VALUES ('" + Programa_ID;
                 Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["Clave"].ToString();
                 Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["Nombre"].ToString();
                 Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["Descripcion"].ToString();
                 Mi_SQL = Mi_SQL + "','ACTIVO','1111')";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
             }
             else
             {

             }
             
         }


         ScriptManager.RegisterStartupScript(this, this.GetType(), "Programas", "alert('Programas Guardadas ');", true);
     }


     protected void Btn_Cuentas_Contables_Click(object sender, EventArgs e)
     {
         Lista_Cuentas_Contables();
     }
    //Sube las cuentas contable sy las relaciona con las partidas
     private void Lista_Cuentas_Contables()
     {
         DataSet _DataSet = Leer_Excel("Select * From [Sheet1$]", "../../Archivos/lista_cuentas.xlsx");
         DataTable Dt_Cuentas_Contables = _DataSet.Tables[0];
         String ID = "";
         String Mi_Sql = "";
         for (int i = 0; i < Dt_Cuentas_Contables.Rows.Count-1; i++)
         {
             //verificar si ya existe
             Mi_Sql = "SELECT * FROM CAT_CON_CUENTAS_CONTABLES WHERE CUENTA = '" + Dt_Cuentas_Contables.Rows[i][8].ToString().Trim() + "'";
             DataSet _DataSet_Existe = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             if (_DataSet_Existe != null && _DataSet_Existe.Tables[0].Rows.Count > 0)
             {
                 int x = 1;
             }
             else
             {
                 ID = Consecutivo("CUENTA_CONTABLE_ID", "CAT_CON_CUENTAS_CONTABLES");
                 String Descripcion = Dt_Cuentas_Contables.Rows[i][9].ToString().Trim();
                 Descripcion = Descripcion.Replace("'"," ");
                 Mi_Sql = "INSERT INTO CAT_CON_CUENTAS_CONTABLES (CUENTA_CONTABLE_ID,SUBCUENTA,CUENTA,DESCRIPCION,AFECTABLE) VALUES (" +
                     "'" + ID + 
                     "','" + Dt_Cuentas_Contables.Rows[i][7].ToString().Trim() +
                     "','" + Dt_Cuentas_Contables.Rows[i][8].ToString().Trim() + 
                     "','" + Descripcion + "','SI')";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 Mi_Sql = "UPDATE CAT_SAP_PARTIDAS_ESPECIFICAS SET CUENTA = '" + Dt_Cuentas_Contables.Rows[i][8].ToString().Trim() + "'," +
                     " CUENTA_CONTABLE_ID ='" + ID + "' WHERE CLAVE ='" + Dt_Cuentas_Contables.Rows[i][7].ToString().Trim() + "'";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             }
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(),"Cuentas Contables", "alert('Cuentas Actualizadas');", true);
     }

     protected void Btn_Elementos_PEP_Click(object sender, EventArgs e)
     {
         DataSet _DataSet = Leer_Excel("Select * From [15$]", "../../Archivos/elemento_pep.xlsx");
         DataTable Dt_Elementos_Pep = _DataSet.Tables[0];
         String ID = "";
         String Mi_Sql = "";
         for (int i = 0; i < Dt_Elementos_Pep.Rows.Count; i++)
         {
                 
                 Mi_Sql = "UPDATE CAT_SAP_PROYECTOS_PROGRAMAS SET ELEMENTO_PEP = '" + Dt_Elementos_Pep.Rows[i][2].ToString().Trim() + "'" +
                     " WHERE CLAVE ='" + Dt_Elementos_Pep.Rows[i][0].ToString().Trim() + "'";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Cuentas Contables", "alert('Elemento PEP Actualizado');", true);      
     }

     protected void Btn_Crear_Areas_Click(object sender, EventArgs e)
     {
         String Mi_Sql = "SELECT * FROM CAT_DEPENDENCIAS";
         DataTable Dt_UR = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
         foreach(DataRow Renglon in Dt_UR.Rows)
         {

             Mi_Sql = "INSERT INTO CAT_AREAS (AREA_ID,NOMBRE,ESTATUS,DEPENDENCIA_ID,USUARIO_CREO) VALUES (" +
             "'" + Renglon["DEPENDENCIA_ID"].ToString().Trim() + "'," + 
             "'" + Renglon["NOMBRE"].ToString().Trim() + "'," +
             "'" + "ACTIVO" + "'," +
             "'" + Renglon["DEPENDENCIA_ID"].ToString().Trim() + "'," +
             "'" + "CARGA INICIAL" + "'" +
             ")";
             OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);

         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Áreas Creadas');", true);
     }

     protected void Btn_Menus_Click(object sender, EventArgs e)
     {
         DataSet _DataSet = Leer_Excel("Select * From [Hoja1$]", "../../Archivos/Menus.xlsx");
         DataTable Dt_Menus_Excel = _DataSet.Tables[0];
         String Mi_Sql = "";
         int cintador = 0;
         try
         {
             foreach (DataRow Renglon in Dt_Menus_Excel.Rows)
             {
                 Mi_Sql = "UPDATE APL_CAT_MENUS SET " +
                 "PARENT_ID = " + Renglon["PARENT_ID"].ToString().Trim() + "," +
                 "MENU_DESCRIPCION = '" + Renglon["MENU_DESCRIPCION"].ToString().Trim() + "'," +
                 "URL_LINK = '" + Renglon["URL_LINK"].ToString().Trim() + "'," +
                 "ORDEN = " + Renglon["ORDEN"].ToString().Trim() + "," +
                 "USUARIO_CREO = '" + "CARGA INICIAL" + "'," +
                 "FECHA_CREO = " + "SYSDATE" + "," +
                 "USUARIO_MODIFICO = '" + "CARGA INICIAL" + "'," +
                 "FECHA_MODIFICO = " + "SYSDATE" + "," +
                 "CLASIFICACION = '" + Renglon["CLASIFICACION"].ToString().Trim() + "'," +
                 "PAGINA = '" + Renglon["PAGINA"].ToString().Trim() + "'" +
                 " WHERE MENU_ID = " + Renglon["MENU_ID"].ToString().Trim();
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 cintador++;
             }
         }
         catch(Exception Ex){
             //System.Windows.Forms.MessageBox.Show("" + cintador);
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Menús Actualizadas');", true);
     }

     protected void Btn_Actualizar_Password_Click(object sender, EventArgs e)
     {
         DataTable Dt_Password = null;
         String Mi_Sql = "";
         Mi_Sql = "SELECT EMPLEADO_ID, PASSWORD FROM CAT_EMPLEADOS";
         Dt_Password = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
         foreach (DataRow Renglon in Dt_Password.Rows)
         {
             String Crypt = Cls_Util.Encriptar(Renglon["PASSWORD"].ToString().Trim());
             Mi_Sql = "UPDATE CAT_EMPLEADOS SET PASSWORD = '" + Crypt + "' WHERE EMPLEADO_ID = '" + Renglon["EMPLEADO_ID"].ToString().Trim() + "'";
             OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Password Actualizados');", true);
     }

     protected void Btn_Actualizar_Descripcion_Productos_Click(object sender, EventArgs e)
     {
         DataSet Ds_Productos = new DataSet();
         Div_Contenedor_Msj_Error.Visible = false;
         Lbl_Mensaje_Error.Text = "";
         String SqlExcel = "Select * From [Productos$]";
         Ds_Productos = Leer_Excel(SqlExcel);
         DataTable Dt_Productos = Ds_Productos.Tables[0];
         String Descripcion = "";
         String Mi_Sql = "";
         foreach (DataRow Producto in Dt_Productos.Rows)
         {
             //if (!(Producto["Descripcion Larga"].ToString().Contains(Producto["Modelo"].ToString().Trim())))
             //{
             Descripcion = Producto["Descripcion Larga"].ToString().Trim();
             if (Producto["Descripcion"].ToString().Trim().Length > 0)
                 Descripcion += ", " + Producto["Descripcion"].ToString().Trim();
             if (Producto["Modelo"].ToString().Trim().Length > 0)
                 Descripcion += ", " + Producto["Modelo"].ToString().Trim();
                 Mi_Sql = "UPDATE CAT_COM_PRODUCTOS SET DESCRIPCION = '" + Descripcion + 
                     "' WHERE PRODUCTO_ID = '" + Producto["ID"].ToString().Trim() + "'";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             //}
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Productos ACtualizados');", true);
     }

     //protected void Btn_Test_Click(object sender, EventArgs e)
     //{
     //    DataSet Ds_Productos = new DataSet();
     //    String Mi_Sql = "";
     //    Mi_Sql = "UPDATE CAT_EMPLEADOS SET PASSWORD = NO_EMPLEADO";
     //    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
     //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Productos ACtualizados');", true);
     //}

     protected void Btn_Ingresos_Click(object sender, EventArgs e)
     {
         DataSet _DataSet = Leer_Excel("Select * From [SICATFOR$]", "../../Archivos/ingresos2011.xlsx");
         DataTable Dt_Menus_Excel = _DataSet.Tables[0];
         String Mi_Sql = "";
         int cintador = 0;
         String Rama_ID = "";
         String Grupo_ID = "";
         String Subgrupo_ID = "";
         //try
         //{
             foreach (DataRow Renglon in Dt_Menus_Excel.Rows)
             {

                 if (Renglon["CLAVE"].ToString().Trim().Length == 2)
                 { 
                    //Dar de alta RAMA
                     Rama_ID = Consecutivo("RAMA_ID", "CAT_PRE_RAMAS");
                     Mi_Sql = "INSERT INTO CAT_PRE_RAMAS (RAMA_ID,CLAVE,NOMBRE,DESCRIPCION,ESTATUS,USUARIO_CREO,FECHA_CREO) VALUES (" +
                     "'" + Rama_ID + "','" +
                     Renglon["CLAVE"].ToString().Trim() + "','" +
                     Renglon["CONCEPTO"].ToString().Replace("'","").Trim() + "','" +
                     Renglon["CONCEPTO"].ToString().Replace("'", "").Trim() + "','" +
                     "VIGENTE" + "','" +
                     "CARGA INICIAL" + "'," +
                     "SYSDATE)";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 }
                 else if (Renglon["CLAVE"].ToString().Trim().Length == 4)
                 { 
                    //Dar de alta GRUPO
                     Grupo_ID = Consecutivo("GRUPO_ID", "CAT_PRE_GRUPOS");
                     Mi_Sql = "INSERT INTO CAT_PRE_GRUPOS (GRUPO_ID,RAMA_ID,CLAVE,NOMBRE,DESCRIPCION,ESTATUS,USUARIO_CREO,FECHA_CREO) VALUES (" +
                     "'" + Grupo_ID + "','" +
                     Rama_ID + "','" +
                     Renglon["CLAVE"].ToString().Trim() + "','" +
                     Renglon["CONCEPTO"].ToString().Replace("'", "").Trim() + "','" +
                     Renglon["CONCEPTO"].ToString().Replace("'", "").Trim() + "','" +
                     "VIGENTE" + "','" +
                     "CARGA INICIAL" + "'," +
                     "SYSDATE)";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 }
                 else if (Renglon["CLAVE"].ToString().Trim().Length == 8)
                 {
                     //Dar de alta SUBGRUPO
                     Subgrupo_ID = Consecutivo("CLAVE_INGRESO_ID", "CAT_PRE_CLAVES_INGRESO");
                     Mi_Sql = "INSERT INTO CAT_PRE_CLAVES_INGRESO (CLAVE_INGRESO_ID ,GRUPO_ID,CLAVE," + 
                         "DESCRIPCION,ESTATUS,USUARIO_CREO,FECHA_CREO,DEPENDENCIA_ID,CUENTA_CONTABLE_ID) VALUES (" +
                     "'" + Subgrupo_ID + "','" +
                     Grupo_ID + "','" +
                     Renglon["CLAVE"].ToString().Trim() + "','" +
                     Renglon["CONCEPTO"].ToString().Replace("'", "").Trim() + "','" +
                     "VIGENTE" + "','" +
                     "CARGA INICIAL" + "'," +
                     "SYSDATE,'" +
                     Renglon["DEPENDENCIA_ID"].ToString().Trim() + "','" +
                     "00001')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                 }
             }
         //}
         //catch (Exception Ex)
         //{
         //    //System.Windows.Forms.MessageBox.Show("" + cintador);
         //}
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Actualizado');", true);
     }

     protected void Btn_Inventario_inicial_Click(object sender, EventArgs e)
     {
         DataSet Ds_Productos = new DataSet();
         Div_Contenedor_Msj_Error.Visible = false;
         Lbl_Mensaje_Error.Text = "";
         String SqlExcel = "Select * From [Productos$]";
         Ds_Productos = Leer_Excel(SqlExcel);
         DataTable Dt_Productos = Ds_Productos.Tables[0];
         String Descripcion = "";
         String Mi_Sql = "";
         int clave = 0;
         foreach (DataRow Producto in Dt_Productos.Rows)
         {
             clave++;
             Mi_Sql = "UPDATE CAT_COM_PRODUCTOS SET INICIAL = " + Producto["EXISTENCIA"].ToString().Trim() +
                 " WHERE CLAVE='" + clave + "'";
             OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Inventario Inicial Cargado');", true);
     }

     protected void Btn_Actualizar_Clave_UR_Click(object sender, EventArgs e)
     {
         DataSet _DataSet = Leer_Excel("Select * From [URNUEVAS$]", "../../Archivos/Unidades Responsables Nuevas.xlsx");
         String Mi_Sql = "";
         String ID = "";
         DataSet Ds_Busqueda = null;
         foreach (DataRow Renglon in _DataSet.Tables[0].Rows)
         {
             //busacr ur
             Mi_Sql = "SELECT DEPENDENCIA_ID FROM CAT_DEPENDENCIAS WHERE CLAVE = '" + Renglon["CLAVE_ANTERIOR"].ToString().Trim() + "'";
             Ds_Busqueda = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             if (Ds_Busqueda != null && Ds_Busqueda.Tables.Count > 0)
             {
                //Actualizar UR
                Mi_Sql = "UPDATE " + Cat_Dependencias.Tabla_Cat_Dependencias + " SET " +                
                Cat_Dependencias.Campo_Clave + " = '" +  Renglon["CLAVE_NUEVA"].ToString().Trim() + "', " +
                Cat_Dependencias.Campo_Nombre + " = '" +  Renglon["NOMBRE"].ToString().Trim() + "', " +
                Cat_Dependencias.Campo_Comentarios + " = '" +  Renglon["NOMBRE"].ToString().Trim() + "', " +
                Cat_Dependencias.Campo_Usuario_Modifico + " = 'ADMINISTRADOR', " +
                Cat_Dependencias.Campo_Fecha_Modifico + " = SYSDATE " +
                " WHERE " + Cat_Dependencias.Campo_Clave + " = '" + Renglon["CLAVE_ANTERIOR"].ToString().Trim() + "'";               
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             }
             else
             {
                 //Insertar ur
                 ID = Consecutivo("DEPENDENCIA_ID", "CAT_DEPENDENCIAS");
                 Mi_Sql = "INSERT INTO " + Cat_Dependencias.Tabla_Cat_Dependencias + " ( " +
                 Cat_Dependencias.Campo_Dependencia_ID + "," +
                 Cat_Dependencias.Campo_Clave + ", " +
                 Cat_Dependencias.Campo_Nombre + ", " +
                 Cat_Dependencias.Campo_Comentarios + ", " +
                 Cat_Dependencias.Campo_Usuario_Creo + ", " +
                 Cat_Dependencias.Campo_Fecha_Creo + "" +
                 ") VALUES (" +
                 "'" + ID + "'," +
                 "'" + Renglon["CLAVE_NUEVA"].ToString().Trim() + "'," +
                 "'" + Renglon["NOMBRE"].ToString().Trim() + "'," +
                 "'" + Renglon["NOMBRE"].ToString().Trim() + "'," +
                 "'ADMINISTRADOR',SYSDATE)";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
             }
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Claves de UR actualizadas');", true);
     }


     protected void Btn_Actualizar_ProgramasX_Click(object sender, EventArgs e)
     {
         String Programa_ID = "";
         String Mi_SQL = "";
         DataSet Ds_Programas = null;
         String Resultado = "";
         String SqlExcel = "Select * From [PROGRAMA$]";
         Ds_Programas = Leer_Excel(SqlExcel, "../../Archivos/PROGRAMAS.xlsx");
         for (int i = 0; i < Ds_Programas.Tables[0].Rows.Count; i++)
         {
             Resultado = Consultar_Tablas("CLAVE", Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas, "CLAVE", Ds_Programas.Tables[0].Rows[i]["CLAVE_ACTUAL"].ToString());
             if (!String.IsNullOrEmpty(Resultado))
             {
                 Mi_SQL = "UPDATE " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " SET " +
                 " CLAVE = '" + Ds_Programas.Tables[0].Rows[i]["CLAVE_NUEVA"].ToString() + "', NOMBRE = '" + Ds_Programas.Tables[0].Rows[i]["DESCRIPCION_NUEVA"].ToString() + "'" +
                 ", DESCRIPCION = '" + Ds_Programas.Tables[0].Rows[i]["DESCRIPCION_NUEVA"].ToString() + "'" +
                 " WHERE CLAVE ='" + Ds_Programas.Tables[0].Rows[i]["CLAVE_ACTUAL"].ToString() + "'";
                 OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
             }
             else
             {
                 Resultado = Consultar_Tablas("CLAVE", Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas, "CLAVE", Ds_Programas.Tables[0].Rows[i]["CLAVE_NUEVA"].ToString());
                 if (String.IsNullOrEmpty(Resultado))
                 {
                     Programa_ID = Consecutivo_10
                         (Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID,
                          Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas);
                     Mi_SQL = "INSERT INTO " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas;
                     Mi_SQL = Mi_SQL + "(" + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + ",";
                     Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Clave + ",";
                     Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Nombre + ",";
                     Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Descripcion + ",";
                     Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Estatus + ",";
                     Mi_SQL = Mi_SQL + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP;
                     Mi_SQL = Mi_SQL + ") VALUES ('" + Programa_ID;
                     Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["CLAVE_NUEVA"].ToString();
                     Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["DESCRIPCION_NUEVA"].ToString();
                     Mi_SQL = Mi_SQL + "','" + Ds_Programas.Tables[0].Rows[i]["DESCRIPCION_NUEVA"].ToString();
                     Mi_SQL = Mi_SQL + "','ACTIVO','1111')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 }
             }            
         }
         //for (int i = 0; i < Ds_Programas.Tables[0].Rows.Count; i++)
         //{
         //    Resultado = Consultar_Tablas("CLAVE", Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas, "CLAVE", Ds_Programas.Tables[0].Rows[i]["CLAVE_NUEVA"].ToString());
         //    if (!String.IsNullOrEmpty(Resultado))
         //    {
         //        Mi_SQL = "UPDATE " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " SET " +
         //        " REVISADO = 'SI'" +
                 
         //        " WHERE CLAVE ='" + Ds_Programas.Tables[0].Rows[i]["CLAVE_NUEVA"].ToString() + "'";
         //        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
         //    }
         //}
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('Programas ACtuales');", true);
     }

     protected void Btn_Actualizar_Relacion_Programas_UR_Click(object sender, EventArgs e)
     {
         String Fuente_ID = "";
         String Programa_ID = "";
         String UR_ID = "";
         String Partida_ID = "";
         String Area_Funcional_ID = "";
         String Mi_SQL = "";
         DataSet Ds_Programas = null;
         String Resultado = "";

         String SqlExcel = "Select * From [PGRUR$]";
         Ds_Programas = Leer_Excel(SqlExcel, "../../Archivos/PROGRAMAS_UR.xlsx");
         for (int i = 0; i < Ds_Programas.Tables[0].Rows.Count; i++)
         {
             Fuente_ID = Consultar_Tablas(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID, 
                 Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento, 
                 "CLAVE", Ds_Programas.Tables[0].Rows[i]["FUENTE"].ToString());
             Programa_ID = Consultar_Tablas(Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID, 
                 Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas, 
                 "CLAVE", Ds_Programas.Tables[0].Rows[i]["PGR"].ToString());
             UR_ID = Consultar_Tablas("DEPENDENCIA_ID",
                 Cat_Dependencias.Tabla_Cat_Dependencias,
                 "CLAVE", Ds_Programas.Tables[0].Rows[i]["UR"].ToString());
             Partida_ID = Consultar_Tablas("PARTIDA_ID",
                 Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas,
                 "CLAVE", Ds_Programas.Tables[0].Rows[i]["PARTIDA"].ToString());
             Area_Funcional_ID = Consultar_Tablas("AREA_FUNCIONAL_ID", 
                 "CAT_SAP_AREA_FUNCIONAL", "CLAVE", 
                 Ds_Programas.Tables[0].Rows[i]["B"].ToString());
             DataSet _DataSet = null;
             if (!String.IsNullOrEmpty(Fuente_ID) && !String.IsNullOrEmpty(UR_ID) && !String.IsNullOrEmpty(Programa_ID) && !String.IsNullOrEmpty(Partida_ID))
             {
                 ////Insertar relación entre Fuentes de Financiamiento y UR
                 //Mi_SQL = "SELECT * FROM CAT_SAP_DET_FTE_DEPENDENCIA WHERE DEPENDENCIA_ID = '" + UR_ID + "'";
                 //Mi_SQL += " AND FUENTE_FINANCIAMIENTO_ID = '" + Fuente_ID + "'";
                 //_DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 //if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                 //else
                 //{
                 //    Mi_SQL = "INSERT INTO CAT_SAP_DET_FTE_DEPENDENCIA (DEPENDENCIA_ID,FUENTE_FINANCIAMIENTO_ID) VALUES " +
                 //    "('" + UR_ID + "','" + Fuente_ID + "')";
                 //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 //}

                 ////Insertar la relacion enter programas y unidades
                 ////verificar si ya existe
                 //Mi_SQL = "SELECT * FROM CAT_SAP_DET_PROG_DEPENDENCIA WHERE DEPENDENCIA_ID = '" + UR_ID + "'";
                 //Mi_SQL += " AND PROYECTO_PROGRAMA_ID = '" + Programa_ID + "'";
                 //_DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 //if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                 //else
                 //{
                 //    Mi_SQL = "INSERT INTO CAT_SAP_DET_PROG_DEPENDENCIA (DEPENDENCIA_ID,PROYECTO_PROGRAMA_ID) VALUES " +
                 //    "('" + UR_ID + "','" + Programa_ID + "')";
                 //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 //}

                 ////Insertar la relación entre Programas y Partidas

                 //Mi_SQL = "SELECT * FROM CAT_SAP_DET_PROG_PARTIDAS WHERE PROYECTO_PROGRAMA_ID = '" + Programa_ID + "'";
                 //Mi_SQL += " AND PARTIDA_ID = '" + Partida_ID + "'";
                 //_DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 //if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0) { }
                 //else
                 //{
                 //    String ID = Consecutivo("DET_PROG_PARTIDAS_ID", "CAT_SAP_DET_PROG_PARTIDAS");
                 //    Mi_SQL = "INSERT INTO CAT_SAP_DET_PROG_PARTIDAS (DET_PROG_PARTIDAS_ID,PROYECTO_PROGRAMA_ID,PARTIDA_ID) VALUES " +
                 //    "('" + ID + "','" + Programa_ID + "','" + Partida_ID + "')";
                 //    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 //}

                 //Insertar la relación entre Dependencia y area funcional
                 //verificar si ya existe
                 if (String.IsNullOrEmpty(Area_Funcional_ID))
                 {
                     String ConsecutivoID = Consecutivo("AREA_FUNCIONAL_ID", "CAT_SAP_AREA_FUNCIONAL");
                     Mi_SQL = "INSERT INTO CAT_SAP_AREA_FUNCIONAL (AREA_FUNCIONAL_ID," +
                         "CLAVE,DESCRIPCION,USUARIO_CREO,FECHA_CREO,ESTATUS) VALUES " +
                         "('" + ConsecutivoID + "','" + Ds_Programas.Tables[0].Rows[i]["B"].ToString() +
                         "','" + Ds_Programas.Tables[0].Rows[i]["B"].ToString() + "','CARGA INICIAL',SYSDATE,'ACTIVO')";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                     Mi_SQL = "UPDATE CAT_DEPENDENCIAS SET AREA_FUNCIONAL_ID ='" + ConsecutivoID + "' WHERE DEPENDENCIA_ID ='" +
                        UR_ID + "'";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 }
                 else
                 {
                     Mi_SQL = "UPDATE CAT_DEPENDENCIAS SET AREA_FUNCIONAL_ID ='" + Area_Funcional_ID + "' WHERE DEPENDENCIA_ID ='" +
                        UR_ID + "'";
                     OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                 }
             }
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('ACTUALIZAD');", true);
     }
     protected void Btn_Psp_2012_UR_Click(object sender, EventArgs e)
     {
         String Fuente_ID = "";
         String Programa_ID = "";
         String UR_ID = "";
         String Partida_ID = "";
         String Area_Funcional_ID = "";
         String Mi_SQL = "";

         String SqlExcel = "Select * From [PRESUPUESTOS$]";
         DataSet Ds_Psp = Leer_Excel(SqlExcel, "../../Archivos/PSP2012.xlsx");
         DataTable Dt_Psp = Ds_Psp.Tables[0];
         foreach (DataRow Renglon in Dt_Psp.Rows)
         {
             Fuente_ID = Consultar_Tablas(Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID, 
                 Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento, 
                 "CLAVE", Renglon["FUENTE"].ToString());
             Programa_ID = Consultar_Tablas(Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID, 
                 Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas, 
                 "CLAVE", Renglon["PROGRAMA"].ToString());
             UR_ID = Consultar_Tablas("DEPENDENCIA_ID",
                 Cat_Dependencias.Tabla_Cat_Dependencias,
                 "CLAVE", Renglon["UR"].ToString());
             Partida_ID = Consultar_Tablas("PARTIDA_ID",
                 Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas,
                 "CLAVE", Renglon["PARTIDA"].ToString());
             Area_Funcional_ID = Consultar_Tablas("AREA_FUNCIONAL_ID", 
                 "CAT_SAP_AREA_FUNCIONAL", "CLAVE", 
                 Renglon["AFUNCIONAL"].ToString());
             
             if (!String.IsNullOrEmpty(Fuente_ID) && !String.IsNullOrEmpty(UR_ID) && !String.IsNullOrEmpty(Programa_ID) && !String.IsNullOrEmpty(Partida_ID))
             {
                 //Mi_SQL = "Insert into OPE_PSP_PRESUPUESTO_APROBADO DEPENDENCIA_ID,ANIO_PRESUPUESTO,MONTO_COMPROMETIDO," +
                 //    "MONTO_PRESUPUESTAL,MONTO_DISPONIBLE,COMENTARIOS,USUARIO_CREO,FECHA_CREO,USUARIO_MODIFICO,FECHA_MODIFICO," +
                 //    "PRESUPUESTO_ID,PARTIDA_ID,MONTO_EJERCIDO,NO_ASIGNACION_ANIO,FECHA_ASIGNACION,PROYECTO_PROGRAMA_ID," +
                 //    "FUENTE_FINANCIAMIENTO_ID,MONTO_AMPLIACION,MONTO_REDUCCION,MONTO_MODIFICADO,MONTO_DEVENGADO,MONTO_PAGADO," +
                 //    "MONTO_DEVEGANDO_PAGADO,MONTO_COMPROMETIDO_REAL,NO_PRESUPUESTO,CAPITULO_ID,AREA_FUNCIONAL_ID) " +
                 //    " values " +
                 //    "('" + UR_ID "',2012,0,0,5000,null,'CASTRO RODRIGUEZ JESUS SALVADOR',to_date('28/11/11','DD/MM/RR'),'CASTRO RODRIGUEZ JESUS SALVADOR',to_date('13/12/11','DD/MM/RR'),null,'0000000192',0,1,to_date('28/11/11','DD/MM/RR'),'0000000061','00014',5000,0,5000,0,0,0,0,null,'00019','00003');";
             
             }
         }
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('ACTUALIZAD');", true);
     }
     protected void Btn_Nueva_Requisa_Click(object sender, EventArgs e)
     {
         Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
         Requisicion_Negocio.Elaborar_Requisicion_A_Partir_De_Otra(136);
         ScriptManager.RegisterStartupScript(this, this.GetType(), "Áreas", "alert('ACTUALIZAD');", true);
     }
}
