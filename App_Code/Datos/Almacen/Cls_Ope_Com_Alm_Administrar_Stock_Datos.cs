using System;
using System.Data;
using System.Collections;
using System.Configuration;
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
using System.Text;
using Presidencia.Constantes;
using Presidencia.Administrar_Stock.Negocios;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using System.Web;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Administrar_Stock_Datos
/// </summary>
/// 
namespace Presidencia.Administrar_Stock.Datos
{
    public class Cls_Ope_Com_Alm_Administrar_Stock_Datos
    {
        //****************************************************************************************
        //    NOMBRE DE LA FUNCION: Consulta_Inventario_Fisico
        //    DESCRIPCION :         Consultalos productos que hay en el Stock de almacén                  
        //    PARAMETROS  :         Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO        :         Salvador Hernández Ramírez.
        //    FECHA_CREO  :         05-Enero-2011
        //    MODIFICO          :   Salvador Hernández Ramírez
        //    FECHA_MODIFICO    :   05/Mayo/2011
        //    CAUSA_MODIFICACION:   Se agregó una subconsulta para consultar modelos y marcas de forma diferente,
        //                          ya que no todos los productos pueden tener estos datos
        //****************************************************************************************/
        public static DataSet Consulta_Inventario_Fisico(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            DataSet DataSet_Temporal = null;
            String Mi_SQL;

                try
                {
                          Mi_SQL = "SELECT " +
                          "PRODUCTO."+Cat_Com_Productos.Campo_Producto_ID + ", " +
                          "PRODUCTO." + Cat_Com_Productos.Campo_Nombre + " ||' '|| " +
                          "PRODUCTO." + Cat_Com_Productos.Campo_Descripcion + " as PRODUCTO,  " +
                          " ( SELECT " + Cat_Com_Modelos.Campo_Nombre + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE " +
                          Cat_Com_Modelos.Campo_Modelo_ID + " = PRODUCTO." + Cat_Com_Productos.Campo_Modelo_ID + ")  as MODELO ," +
                          " ( SELECT " + Cat_Com_Marcas.Campo_Nombre + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " WHERE " +
                          Cat_Com_Marcas.Campo_Marca_ID + " = PRODUCTO." + Cat_Com_Productos.Campo_Marca_ID + ")  as MARCA , " +
                          " ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE " +
                          Cat_Com_Unidades.Campo_Unidad_ID + " = PRODUCTO." + Cat_Com_Productos.Campo_Unidad_ID + ")  as UNIDAD , " +
                          "PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Clave + " as FAMILIA, " +
                          "PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Clave + " as SUBFAMILIA, " +
                          "PRODUCTO." + Cat_Com_Productos.Campo_Clave + ", " +
                          "PRODUCTO." + Cat_Com_Productos.Campo_Existencia +
                          " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO"+
                          " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDAS_ESPECIFICAS " +
                          " ON PRODUCTO." + Cat_Com_Productos.Campo_Partida_ID + "= PARTIDAS_ESPECIFICAS." +
                          Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                          " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GENERICA " +
                          " ON PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + "= PARTIDA_GENERICA." +
                          Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                          " WHERE " + Cat_Com_Productos.Campo_Stock + " = '" + "SI" + "'";
                          Mi_SQL = Mi_SQL + " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;

                        DataSet_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        return DataSet_Temporal;
                }catch (Exception Ex)
                 {
                     String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                     throw new Exception(Mensaje);
                 }
          }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Inventario_Selectivo
        ///DESCRIPCIÓN:          Realiza una consulta a la base de datos para buscar informacion 
        ///                      de una captura de 
        ///PARAMETROS:           Negocio: Objeto de la calse de Negocio que contiene los datos para realizar la consulta
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Enero/2011
        ///MODIFICO:             Salvador Hernández Ramírez
        ///FECHA_MODIFICO:       05/Mayo/2011
        ///CAUSA_MODIFICACIÓN:   Se agregó una subconsulta para consultar modelos y marcas de forma diferente,
        ///                      ya que no todos los productos pueden tener estos datos
        ///*******************************************************************************
        public static DataSet Consulta_Inventario_Selectivo(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            DataSet Data_Set = null;
            String Mi_SQL;

             try
             {
                   Mi_SQL = "SELECT " +
                   "PRODUCTO." + Cat_Com_Productos.Campo_Producto_ID + ", " +
                   "PRODUCTO." + Cat_Com_Productos.Campo_Nombre + " ||' '|| " +
                   "PRODUCTO." + Cat_Com_Productos.Campo_Descripcion + " as PRODUCTO, " +
                   " ( SELECT " + Cat_Com_Modelos.Campo_Nombre + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE " +
                   Cat_Com_Modelos.Campo_Modelo_ID + " = PRODUCTO." + Cat_Com_Productos.Campo_Modelo_ID + ")  as MODELO ," +
                   " ( SELECT " + Cat_Com_Marcas.Campo_Nombre + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " WHERE " +
                   Cat_Com_Marcas.Campo_Marca_ID + " = PRODUCTO." + Cat_Com_Productos.Campo_Marca_ID + ") as MARCA ," +
                   " ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE " +
                   Cat_Com_Unidades.Campo_Unidad_ID + " = PRODUCTO." + Cat_Com_Productos.Campo_Unidad_ID + ")  as UNIDAD , " +
                   "PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Clave + " as FAMILIA, " +
                   "PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Clave + " as SUBFAMILIA, " +
                   "PRODUCTO." + Cat_Com_Productos.Campo_Clave + ", " +
                   "PRODUCTO." + Cat_Com_Productos.Campo_Existencia +
                   " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTO" +
                   " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDAS_ESPECIFICAS " +
                   " ON PRODUCTO." + Cat_Com_Productos.Campo_Partida_ID + "= PARTIDAS_ESPECIFICAS." +
                   Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                   " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GENERICA " +
                   " ON PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + "= PARTIDA_GENERICA." +
                   Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                   " WHERE " + Cat_Com_Productos.Campo_Stock + " = '" + "SI" + "'";
                    
                 if (Datos.P_Familia_ID != null)
                 {
                     Mi_SQL = Mi_SQL + " AND PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                     " like '%" + Datos.P_Familia_ID + "%'";
                 }

                 if (Datos.P_Subfamilia_ID != null)
                 {
                     Mi_SQL = Mi_SQL + " AND PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                        " like '%" + Datos.P_Subfamilia_ID + "%'";
                 }

                 if (Datos.P_Marca_ID != null)
                 {
                     Mi_SQL = Mi_SQL + " AND PRODUCTO." + Cat_Com_Productos.Campo_Marca_ID +
                      " like '%" + Datos.P_Marca_ID + "%'";
                 }

                 if ((Datos.P_Letra_Inicio != null) && (Datos.P_Letra_Fin != null))
                 {
                     String Letra_Inicial = "" + Datos.P_Letra_Inicio.ToString();
                     String Letra_Final = "" + Datos.P_Letra_Fin.ToString();

                     if ((Letra_Final == "Z") | (Letra_Final == "z"))
                     {
                         Mi_SQL = Mi_SQL + " AND UPPER( PRODUCTO." + Cat_Com_Productos.Campo_Nombre + ")" +
                             " between  UPPER('" + Letra_Inicial + "%')  and UPPER('" + "Y" + "%') or UPPER(PRODUCTO." + Cat_Com_Productos.Campo_Nombre + ") like UPPER('Z%')";
                     }
                     else
                     {
                         int Asqui_Letra_Final = Encoding.ASCII.GetBytes(Letra_Final)[0];
                         Asqui_Letra_Final = (Asqui_Letra_Final + 1);
                         char L_fin = Convert.ToChar(Asqui_Letra_Final);
                         Letra_Final = Convert.ToString(L_fin);

                         Mi_SQL = Mi_SQL + " AND UPPER(PRODUCTO." + Cat_Com_Productos.Campo_Nombre + ")" +
                             " between UPPER('" + Letra_Inicial + "%')  and UPPER('" + Letra_Final + "%')";
                     }
                 }
                 Mi_SQL = Mi_SQL + " ORDER BY PRODUCTO." + Cat_Com_Productos.Campo_Nombre;

             }catch (Exception Ex)
             {
                 String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                 throw new Exception(Mensaje);
             }
                    Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    return Data_Set;
         }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Id_Consecutivo
        ///DESCRIPCIÓN:          Crea una sentencia sql para insertar un inventario en la base de datos
        ///PARAMETROS:           1.-Campo_ID, nombre del campo de la tabla al cual se quiere sacar el ultimo valor
        ///                      2.-Tabla, nombre de la tabla que se va a consultar
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           07/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static String Obtener_Id_Consecutivo(String Campo_ID, String Tabla)
        {
            String Consecutivo = "";
            String Mi_SQL;         
            Object Obj; 
         try
            {
                Mi_SQL = "SELECT NVL(MAX (" + Campo_ID + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Tabla;
                Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Obj))
                {
                    Consecutivo = "0000000001";
                }
                else
                {
                    Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Obj) + 1);
                }
         }catch (Exception Ex)
         {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
         }
            return Consecutivo;
       }


        ///*******************************************************************************
        ///    NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///    DESCRIPCIÓN:           Obtiene datos de la Base de Datos y los regresa en un DataTable.
        ///    PARAMETROS:            Datos: Contiene los parametros que se van a utilizar para hacer la consulta de la Base de Datos.
        ///    CREO:                  Salvador Hernández Ramírez
        ///    FECHA_CREO:            06/Enero/2011 
        ///    MODIFICO:              Salvador Hérnandez Ramírez
        ///    FECHA_MODIFICO:        07/Abril/2011
        ///    CAUSA_MODIFICACIÓN:    Se modifico la consulta, ya que se cambio familia por partida generica
        ///                           y subfamilia por partida especifica
        ///*******************************************************************************
        public static System.Data.DataTable Consultar_DataTable(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            String Mi_SQL=null;
            DataSet Ds_Stock = null;
            System.Data.DataTable Dt_Stok_Almacen = new System.Data.DataTable();
            try
            {
                if (Datos.P_Tipo_DataTable.Equals("FAMILIA"))
                {
                    Mi_SQL = "SELECT DISTINCT" +
                    "  PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID  + " as FAMILIA_ID" +
                    ", PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Descripcion + " as FAMILIA " +
                    " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS" +
                    " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDAS_ESPECIFICAS " +
                    " ON PRODUCTOS." + Cat_Com_Productos.Campo_Partida_ID + "= PARTIDAS_ESPECIFICAS." +
                    Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                    " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GENERICA " +
                    " ON PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + "= PARTIDA_GENERICA." +
                    Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                    " WHERE " + Cat_Com_Productos.Campo_Stock + "='" + "SI" + "'";
                    Mi_SQL = Mi_SQL + " Order By  PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Descripcion;  

                      
                }else if (Datos.P_Tipo_DataTable.Equals("SUBFAMILIA")){

                    Mi_SQL = "SELECT DISTINCT" +
                    "  PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " as SUBFAMILIA_ID" +
                    ", PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " as SUBFAMILIA " +
                    " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS" +
                    " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDAS_ESPECIFICAS " +
                    " ON PRODUCTOS." + Cat_Com_Productos.Campo_Partida_ID + "= PARTIDAS_ESPECIFICAS." +
                    Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                    " WHERE " + Cat_Com_Productos.Campo_Stock + "='" + "SI" + "'";
                    Mi_SQL = Mi_SQL + " Order By PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Descripcion;  

                }else if (Datos.P_Tipo_DataTable.Equals("MARCAS")) {
                    
                    Mi_SQL = " SELECT DISTINCT" +
                        "  MARCAS." + Cat_Com_Marcas.Campo_Nombre +
                        ", MARCAS." + Cat_Com_Marcas.Campo_Marca_ID +
                        " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS" +
                        " JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCAS " +
                        " ON PRODUCTOS." + Cat_Com_Productos.Campo_Marca_ID + "= MARCAS." +
                        Cat_Com_Marcas_Productos.Campo_Marca_ID +
                        " WHERE " + Cat_Com_Productos.Campo_Stock + "='" + "SI" + "'";
                        Mi_SQL = Mi_SQL + " Order By  MARCAS." + Cat_Com_Marcas.Campo_Nombre;  
                }
                else if (Datos.P_Tipo_DataTable.Equals("INVENTARIOS"))
                {
                    Mi_SQL = "SELECT * " +
                        " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock;
                    
                    if (Datos.P_Estatus == "CAPTURADO")
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + " ='"+ Datos.P_Estatus + "'";
                    }

                    if (Datos.P_Estatus == "PENDIENTE")
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + " ='" + Datos.P_Estatus + "'";
                    }

                    if (Datos.P_No_Inventario != null)
                    {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " LIKE '%" + Datos.P_No_Inventario + "%'";
                    }

                    if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
                    {
                        Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + "." + Ope_Com_Cap_Inv_Stock.Campo_Fecha + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                      " AND '" + Datos.P_Fecha_Final+ "'";
                    }
                    Mi_SQL = Mi_SQL + " Order By " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + "." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario;  
                }
                else if (Datos.P_Tipo_DataTable.Equals("TODOS_INVENTARIOS"))
                {
                    Mi_SQL = "SELECT * from ( " +
                    " SELECT * from " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                    " Order By " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + "." +
                    Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " desc )" +
                    " where rownum <= 50 ";    // Para que nada mas me de los ultimos 50 registros
                    //Mi_SQL = Mi_SQL + " Order By " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + "." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario;   // para que estos registros los ordene de forma ascendente
                }

                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Stock = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Stock != null)
                {
                    Dt_Stok_Almacen = Ds_Stock.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_Stok_Almacen;
        }


        ///*******************************************************************************
        ///     NOMBRE DE LA FUNCIÓN : Guardar_Inventario
        ///     DESCRIPCIÓN:           Guardar el inventario generado por el usuario
        ///     PARAMETROS:            Datos.   Contiene los parametros que se van a utilizar para hacer la consulta de la Base de Datos.
        ///     CREO:                  Salvador Hernández Ramírez
        ///     FECHA_CREO:            08/Enero/2011 
        ///     MODIFICO: 
        ///     FECHA_MODIFICO:        28/Enero/2011
        ///     CAUSA_MODIFICACIÓN:    Se instancio el método "Alta_Bitacora" para dar de alta el registro
        ///*******************************************************************************
        public static String Guardar_Inventario(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            String Mensaje = String.Empty; //Variable para el mensaje de error
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            try
            {
            String Fecha_Creo = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();
            String Mi_SQL = "INSERT INTO " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                " (" + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario +
                ", " + Ope_Com_Cap_Inv_Stock.Campo_Fecha +
                ", " + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo +
                ", " + Ope_Com_Cap_Inv_Stock.Campo_Fecha_Creo+
                ", " + Ope_Com_Cap_Inv_Stock.Campo_Estatus +
                ", " + Ope_Com_Cap_Inv_Stock.Campo_Tipo +
                ", " + Ope_Com_Cap_Inv_Stock.Campo_Observaciones +            
                ") VALUES ('" +
                Datos.P_No_Inventario+ "','" +
                Fecha_Creo + "','" +
                Datos.P_Usuario_Creo+ "','" +
                Fecha_Creo + "','" +
                Datos.P_Estatus + "','" +
                Datos.P_Tipo + "','" +
                Datos.P_Observaciones + "')";
             
             Cmd.CommandText = Mi_SQL;
             Cmd.ExecuteNonQuery(); // Se ejecuta la operación 1 
             //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);
 
             if (Datos.P_Inventario_Stock != null)
             {
                 DataTable DT_Temporal = new DataTable();
                 DT_Temporal=Datos.P_Inventario_Stock;

                 for (int i = 0; i < DT_Temporal.Rows.Count; i++)
                 {
                      Mi_SQL = "INSERT INTO " + Ope_Com_Cap_Stock_Detalles.Tabla_Ope_Com_Cap_Stock_Detalles +
                      " (" + Ope_Com_Cap_Stock_Detalles.Campo_No_Inventario +
                      ", " + Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id +
                      ", " + Ope_Com_Cap_Stock_Detalles.Campo_Contados_Sistema +
                      ", " + Ope_Com_Cap_Stock_Detalles.Campo_Marbete +
                      ") VALUES ('" +
                      Datos.P_No_Inventario + "','" +
                      DT_Temporal.Rows[i]["PRODUCTO_ID"].ToString() + "','" +
                      DT_Temporal.Rows[i]["EXISTENCIA"].ToString() + "'," + "MARBETE.NEXTVAL" + ")";
                     
                     Cmd.CommandText = Mi_SQL;
                     Cmd.ExecuteNonQuery(); // Se ejecutan las operación 2 
                     //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Alm_Generar_Inventario_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);
                 }
             }
             Trans.Commit(); // Se ejecuta la transacciones
            }
            catch (OracleException ex)
            {
                if (ex.Code.ToString() == "1")
                {
                    return "00001";
                }
                else
                {
                    String Mensaje2 = "Error al intentar consultar los registros. Error: [" + ex.Message + "]"; //"Error general en la base de datos"
                    throw new Exception(Mensaje2);
                }
            }
            return "";
        }


        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION:  Consulta_Inventario_General
        //    DESCRIPCION :          Consultalos productos que pertenecen al inventario             
        //    PARAMETROS  :          No_Inventario: Es el numero de inventario que se va consultar
        //    CREO        :          Salvador Hernández Ramírez.
        //    FECHA_CREO  :          13-Enero-2011
        //    MODIFICO          :
        //    FECHA_MODIFICO    :
        //    CAUSA_MODIFICACION:
        //****************************************************************************************/
        public static DataSet Consulta_Inventarios_General(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            DataSet DataSet_Temporal = null;
            String Mi_SQL;
            try
            {
                Mi_SQL = "SELECT " +
                          "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario+ ", " +
                          "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Fecha + ", " +
                          "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Usuario_Creo + ", " +
                          "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Estatus + ", " +
                          "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Tipo + ", " +
                          "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Observaciones + ", " +
                          "STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id + ", " +
                          "STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Contados_Sistema + ", " +
                          "STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Contados_Usuario + ", " +
                          "STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Diferencia + ", " +
                          "STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Marbete + ", " +
                          "PRODUCTOS." + Cat_Com_Productos.Campo_Clave + ", " +
                          "PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " ||' '|| " +
                          "PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " as PRODUCTO, " +
                          "PRODUCTOS." + Cat_Com_Productos.Campo_Existencia + ", " +
                          " ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades+ " WHERE " +
                          Cat_Com_Unidades.Campo_Unidad_ID + " = PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + ")  as UNIDAD ," +
                           " ( SELECT " + Cat_Com_Modelos.Campo_Nombre + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE " +
                          Cat_Com_Modelos.Campo_Modelo_ID + " = PRODUCTOS." + Cat_Com_Productos.Campo_Modelo_ID + ")  as MODELO ," +
                          "PARTIDA_GENERICA." + Cat_SAP_Partida_Generica.Campo_Clave + " as FAMILIA, " +
                          "PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Clave + " as SUBFAMILIA " +
                    " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + " INV_STOCK" +
                    " JOIN " + Ope_Com_Cap_Stock_Detalles.Tabla_Ope_Com_Cap_Stock_Detalles + " STOCK_DETALLES " +
                    " ON INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + "= STOCK_DETALLES." +
                    Ope_Com_Cap_Inv_Stock.Campo_No_Inventario +
                    " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS " +
                    " ON PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + "= STOCK_DETALLES." +
                    Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id+
                    " JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDAS_ESPECIFICAS " +
                    " ON PRODUCTOS." + Cat_Com_Productos.Campo_Partida_ID + "= PARTIDAS_ESPECIFICAS." +
                    Cat_Sap_Partidas_Especificas.Campo_Partida_ID +
                    " JOIN " + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + " PARTIDA_GENERICA " +
                    " ON PARTIDAS_ESPECIFICAS." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + "= PARTIDA_GENERICA." +
                    Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID +
                    " WHERE " + "INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " = '" + Datos.P_No_Inventario + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY PRODUCTOS." + Cat_Com_Productos.Campo_Nombre;
                
                DataSet_Temporal = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                return DataSet_Temporal;
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
            finally
            {
            }
        }

        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION: Guardar_Inventario_Capturado
        //    DESCRIPCION:          Funcion utilizada para guardar las cantidades de productos que sean capturadas por el usuario            
        //    PARAMETROS:           Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO:                 Salvador Hernández Ramírez.
        //    FECHA_CREO:           17-Enero-2011
        //    MODIFICO:
        //    FECHA_MODIFICO:
        //    CAUSA_MODIFICACION:
        //   ****************************************************************************************/
        public static void Guardar_Inventarios_Capturado(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            String Fecha_Captura = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();
            DataTable DataTable_Temporal = null;
            String Mi_SQL;
            String No_inventario = Datos.P_No_Inventario;

            DataTable_Temporal = Datos.P_Datos_Productos;
            try
            { 
                for (int i = 0; i < DataTable_Temporal.Rows.Count; i++)
                {
                    String Marbete = DataTable_Temporal.Rows[i]["MARBETE"].ToString();
                    String Producto_Id = DataTable_Temporal.Rows[i]["PRODUCTO_ID"].ToString();
                    String Cantidad = DataTable_Temporal.Rows[i]["CONTADOS_USUARIO"].ToString();
                    String Diferencia = DataTable_Temporal.Rows[i]["DIFERENCIA"].ToString();
                    Mi_SQL = "UPDATE " + Ope_Com_Cap_Stock_Detalles.Tabla_Ope_Com_Cap_Stock_Detalles +
                    " SET " + Ope_Com_Cap_Stock_Detalles.Campo_Contados_Usuario + "=" +
                    Cantidad + ", " + Ope_Com_Cap_Stock_Detalles.Campo_Diferencia + "=" + Diferencia + " WHERE " + Ope_Com_Cap_Stock_Detalles.Campo_Marbete + " = " + Marbete + " AND " + Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id + " = '" + Producto_Id + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operacion 1 
                    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx", Marbete, Mi_SQL);
                }
                  
                   Mi_SQL = "UPDATE " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                   " SET " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "=" +
                   " '" + Datos.P_Estatus + "'" + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " = '" + No_inventario + "'";
               
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operacion 2 
                   //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx", Marbete, Mi_SQL);


                   // Se inserta  del inventario  capturado en la tabla OPE_COM_AJUATES_INV_STOCK
                   Mi_SQL = "INSERT INTO " + Ope_Com_Ajustes_Inv_Stock.Tabla_Ope_Com_Ajustes_Inv_Stock +
                            " (" + Ope_Com_Ajustes_Inv_Stock.Campo_No_Inventario +
                            ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Tipo_Ajuste +
                            ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Fecha +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Usuario_Ajusto +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_No_Empleado +
                              ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Justificacion +
                            ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Marbete +
                            ") VALUES ('" +
                            Datos.P_No_Inventario + "','" +
                            Datos.P_Tipo_Ajuste + "','" +
                            Fecha_Captura + "','" +
                            Datos.P_Usuario_Modifico + "','" +
                            Datos.P_No_Empleado + "','" +
                            Datos.P_Justificacion + "'," + "SEQ_MARBETE.NEXTVAL" + ")";

                   Cmd.CommandText = Mi_SQL;
                   Cmd.ExecuteNonQuery(); // Se ejecuta la operacion 3 
                   //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Alm_Capturar_Inventario_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);

                   Trans.Commit(); // Se ejecuta la transacciones
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


        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION: Aplicar_Inventario
        //    DESCRIPCION:          Actualizar las cantidades de los productos contados por el 
        //                          sistema con la cantidad de productos que han sido capturados.       
        //    PARAMETROS:           Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO:                 Salvador Hernández Ramírez.
        //    FECHA_CREO:           13-Enero-2011
        //    MODIFICO:
        //    FECHA_MODIFICO:
        //    CAUSA_MODIFICACION:
        //****************************************************************************************/
        public static void Aplicar_Inventario(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            String Fecha_Aplico = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();
            DataTable DataTable_Temporal = null;
            DataTable DataTable_Comprometido= new DataTable();
            String Mi_SQL;
            DataTable_Temporal = Datos.P_Datos_Productos;
            Double Disponible = 0;
            Double Comprometido = 0;

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

          try
            {
                for (int i = 0; i < DataTable_Temporal.Rows.Count; i++)
                {
                    String Producto_Id = DataTable_Temporal.Rows[i]["PRODUCTO_ID"].ToString();
                    Double Contados_Usuario = Convert.ToDouble(DataTable_Temporal.Rows[i]["CONTADOS_USUARIO"].ToString());

                    // Se consulta la cantidad disponible que actualmente se tiene en la tabla de productos
                    Mi_SQL = "SELECT " +
                    Cat_Com_Productos.Campo_Comprometido + "" +
                    " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "" +
                    " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " ='" + Producto_Id + "'";
                    
                    DataTable_Comprometido = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    Comprometido= Convert.ToDouble(""+ DataTable_Comprometido.Rows[0][0]);

                    if (Convert.IsDBNull(Comprometido) == false) // Si no es null entra
                    {
                        Disponible = Contados_Usuario - Comprometido;
                    }
                    else
                    {
                        Disponible = Contados_Usuario;
                    }

                    Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " SET " + Cat_Com_Productos.Campo_Existencia + "=" + Contados_Usuario +
                    ", " + Cat_Com_Productos.Campo_Disponible + "=" + Disponible + 
                    " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_Id + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 1 
                   //Cls_Bitacora.Alta_Bitacora(Datos.P_Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx", Producto_Id, Mi_SQL);
                }

                    Mi_SQL = "UPDATE " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                    " SET " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "=" +
                    " '" + Datos.P_Estatus + "'" + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " = '" + Datos.P_No_Inventario + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 2 
                    //Cls_Bitacora.Alta_Bitacora(Datos.P_Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);

                    // Se inserta  el inventario en la tabla OPE_COM_AJUATES_INV_STOCK
                    Mi_SQL = "INSERT INTO " + Ope_Com_Ajustes_Inv_Stock.Tabla_Ope_Com_Ajustes_Inv_Stock +
                         " (" + Ope_Com_Ajustes_Inv_Stock.Campo_No_Inventario +
                         ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Tipo_Ajuste +
                         ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Fecha +
                         ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Usuario_Ajusto +
                         ", " + Ope_Com_Ajustes_Inv_Stock.Campo_No_Empleado+
                         ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Justificacion +
                         ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Marbete +
                         ") VALUES ('" +
                         Datos.P_No_Inventario + "','" +
                         Datos.P_Tipo_Ajuste + "','" +
                         Fecha_Aplico + "','" +
                         Datos.P_Usuario_Modifico + "','" +
                         Datos.P_No_Empleado + "','" +
                         Datos.P_Justificacion + "'," + "SEQ_MARBETE.NEXTVAL" + ")";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 3 
                    //Cls_Bitacora.Alta_Bitacora(Datos.P_Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Alm_Aplicar_Inventario_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);

                    Trans.Commit(); // Se ejecuta la transacciones
          }
          catch (Exception Ex)
          {
              String Mensaje = "Error al intentar realizar las  transacción. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
              throw new Exception(Mensaje);
          }
        }


        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION: Consulta_Productos_En_Inventarios
        //    DESCRIPCION:          Consultar los productos que ya forman parte de otros inventarios
        //    PARAMETROS:           Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO:                 Salvador Hernández Ramírez.
        //    FECHA_CREO:           15-Enero-2011
        //    MODIFICO:             Salvador Hernández Ramírez.
        //    FECHA_MODIFICO:       12-Mayo-2011
        //    CAUSA_MODIFICACION:   Se consuslto la clave del producto
        //****************************************************************************************/
        public static System.Data.DataTable Consulta_Productos_En_Inventarios(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            System.Data.DataTable Dt_Productos = null;
            DataSet Ds_Consulta = null;
            String Mi_SQL = null;

            try
            {
                Mi_SQL = "SELECT " +
                      "  INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario +
                      ", INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_Estatus +
                      ", STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id +
                      ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave +
                      "  FROM " + Ope_Com_Cap_Stock_Detalles.Tabla_Ope_Com_Cap_Stock_Detalles + " STOCK_DETALLES " +
                      "  JOIN " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + " INV_STOCK " +
                      "  ON INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + "= STOCK_DETALLES." +
                      Ope_Com_Cap_Stock_Detalles.Campo_No_Inventario +
                      "  JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS " +
                      "  ON STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id + "= PRODUCTOS." +
                      Cat_Com_Productos.Campo_Producto_ID +
                      "  WHERE STOCK_DETALLES." + Ope_Com_Cap_Stock_Detalles.Campo_Producto_Id + "='" + Datos.P_Producto_ID + "'";
                      Mi_SQL = Mi_SQL + " ORDER BY INV_STOCK." + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario;

                Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Dt_Productos = Ds_Consulta.Tables[0];
                return Dt_Productos;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION: Consulta_Productos_En_Inventarios
        //    DESCRIPCION:          Realizar la busqueda de inventarios en base a su estatus
        //    PARAMETROS:           Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO:                 Salvador Hernández Ramírez.
        //    FECHA_CREO:           18-Enero-2011
        //    MODIFICO:
        //    FECHA_MODIFICO:
        //    CAUSA_MODIFICACION:
        //****************************************************************************************/
        public static DataTable Busqueda_Simple(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            DataTable Dt_Inventarios= null;
            DataSet Ds_Consulta = null;
            String Mi_SQL = null;

            try
            {
                if (Datos.P_Estatus != "0")
                {
                    Mi_SQL = "SELECT *" +
                     " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                     " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                }
                else
                {
                    Mi_SQL = "SELECT * from ( " +
                   " SELECT * from " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                   " Order By " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock + "." +
                   Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " desc )" +
                   " where rownum <= 50 ";    // Para que nada mas de los ultimos 50 registros
                }

                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario; // Para Qeu los muestre ordenados


                Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Dt_Inventarios = Ds_Consulta.Tables[0];
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
                return Dt_Inventarios;
        }

        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION: Consulta_Productos_En_Inventarios
        //    DESCRIPCION :         Realizar la busqueda de inventarios en base a su estatus y a su fecha
        //    PARAMETROS  :         Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO        :         Salvador Hernández Ramírez.
        //    FECHA_CREO  :         18-Enero-2011
        //    MODIFICO          :
        //    FECHA_MODIFICO    :
        //    CAUSA_MODIFICACION:
        //****************************************************************************************/
        public static DataTable Busqueda_Avanzada_Inventarios(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            DataTable Dt_Inventarios = null;
            DataSet Ds_Consulta = null;
            String Mi_SQL = null;
            
            try
            {
                Mi_SQL = " SELECT *" +
                         " FROM " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock;

                if (Datos.P_Estatus != "0")
                {
                    Mi_SQL= Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "='" + Datos.P_Estatus + "'";
                }

                if (Datos.P_Estatus != "0")
                {
                    if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
                    {
                        Mi_SQL = Mi_SQL + " AND " + Ope_Com_Cap_Inv_Stock.Campo_Fecha + " BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                        " AND '" + Datos.P_Fecha_Final + "'";
                    }
                }
                else
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_Fecha + " BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                      " AND '" + Datos.P_Fecha_Final + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario;


                Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Dt_Inventarios = Ds_Consulta.Tables[0];

            }catch (Exception Ex)
             {
                 String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                 throw new Exception(Mensaje);
             }
                return Dt_Inventarios;
        }
       
        ///****************************************************************************************
        //    NOMBRE DE LA FUNCION: Consulta_Productos_En_Inventarios
        //    DESCRIPCION :         Realizar el cambio de estatus a cancelado al inventario seleccionado
        //    PARAMETROS  :         Datos: Datos que son enviados de la capa de Negocios para consultar la peticion
        //    CREO        :         Salvador Hernández Ramírez.
        //    FECHA_CREO  :         20-Enero-2011
        //    MODIFICO          :
        //    FECHA_MODIFICO    :
        //    CAUSA_MODIFICACION:
        //   ****************************************************************************************/
        public static void Cambiar_Estatus(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            String Fecha_Cancelo = DateTime.Now.ToString("dd/MM/yyyy").ToUpper();
            String Mi_SQL = null;

            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

             try
                {
                    Mi_SQL = "UPDATE " + Ope_Com_Cap_Inv_Stock.Tabla_Ope_Com_Cap_Inv_Stock +
                        " SET " + Ope_Com_Cap_Inv_Stock.Campo_Estatus + "=" +
                        " '" + Datos.P_Estatus + "'" + " WHERE " + Ope_Com_Cap_Inv_Stock.Campo_No_Inventario + " = '" + Datos.P_No_Inventario + "'";

                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 1 
                    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Com_Alm_Mostrar_Productos_Inv_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);


                    // Se inserta el inventario en la tabla OPE_COM_AJUATES_INV_STOCK
                    Mi_SQL = "INSERT INTO " + Ope_Com_Ajustes_Inv_Stock.Tabla_Ope_Com_Ajustes_Inv_Stock +
                             " (" + Ope_Com_Ajustes_Inv_Stock.Campo_No_Inventario +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Tipo_Ajuste +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Fecha +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Usuario_Ajusto +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_No_Empleado +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Justificacion +
                             ", " + Ope_Com_Ajustes_Inv_Stock.Campo_Marbete +
                             ") VALUES ('" +
                             Datos.P_No_Inventario + "','" +
                             Datos.P_Tipo_Ajuste + "','" +
                             Fecha_Cancelo + "','" +
                             Datos.P_Usuario_Modifico + "','" +
                             Datos.P_No_Empleado + "','" +
                             Datos.P_Justificacion + "'," + "SEQ_MARBETE.NEXTVAL" + ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 2 
                    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Com_Alm_Mostrar_Productos_Inv_Stock.aspx", Datos.P_No_Inventario, Mi_SQL);
                    Trans.Commit(); // Se ejecuta la transacciones
                }
             catch (Exception Ex)
             {
                 String Mensaje = "Error al intentar realizar la transacción. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                 throw new Exception(Mensaje);
             }
        }

        ///*******************************************************************************
        ///     NOMBRE DE LA FUNCIÓN: Consulta_Datos_Usuario
        ///     DESCRIPCIÓN:          Realiza una consulta a la base de datos para consultar la informción sobre el usuario
        ///     PARAMETROS:           Negocio, objeto de la clase de Negocio que contiene los datos para realizar la consulta
        ///     CREO:                 Salvador Hernández Ramírez
        ///     FECHA_CREO:           26/Enero/2011 
        ///     MODIFICO:
        ///     FECHA_MODIFICO:
        ///     CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Usuario(Cls_Ope_Com_Alm_Administrar_Stock_Negocios Datos)
        {
            DataTable Dt_Table;

            try
            {
                String Mi_SQL = "SELECT " +
                             "  EMPLEADOS." + Cat_Empleados.Campo_No_Empleado +
                             ", EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID +
                             ", EMPLEADOS." + Cat_Empleados.Campo_Password +
                             ", EMPLEADOS." + Cat_Empleados.Campo_Nombre+
                             ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno +
                             ", EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno +
                             ", G_ROLES." + Apl_Grupos_Roles.Campo_Grupo_Roles_ID+
                             " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS" +
                             " JOIN " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles + " C_ROLES " +
                             " ON EMPLEADOS." + Cat_Empleados.Campo_Rol_ID + "= C_ROLES." +
                             Apl_Cat_Roles.Campo_Rol_ID +
                              " JOIN " + Apl_Grupos_Roles.Tabla_Apl_Grupos_Roles + " G_ROLES " +
                             " ON G_ROLES." + Apl_Grupos_Roles.Campo_Grupo_Roles_ID + "= C_ROLES." +
                             Apl_Cat_Roles.Campo_Grupo_Roles_ID +
                             " WHERE " + " EMPLEADOS." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "'" +
                             " AND EMPLEADOS."+ Cat_Empleados.Campo_Password + "='" + Datos.P_Password + "'";

                DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Dt_Table = Data_Set.Tables[0];

            }catch (Exception Ex)
             {
                 String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                 throw new Exception(Mensaje);
             }
            return Dt_Table;
        }
    }
}