using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Reportes_Productos.Negocio;



/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Datos
/// </summary>
/// 

namespace Presidencia.Reportes_Productos.Datos
{
    public class Cls_Ope_Com_Alm_Rpts_Productos_Datos
    {
        public Cls_Ope_Com_Alm_Rpts_Productos_Datos()
        {
        }

        #region Metodos
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Productos
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           06/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Productos(Cls_Ope_Com_Alm_Rpts_Productos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_Consulta = new DataTable();
            Boolean where =true;

        try
        {
            Mi_SQL = " SELECT  PRODUCTOS." + Cat_Com_Productos.Campo_Clave + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " as PRODUCTO";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Estatus + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Existencia + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Comprometido + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Disponible + "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Stock + "";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Modelos.Campo_Nombre + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Modelo_ID + " = " + " PRODUCTOS." + Cat_Com_Productos.Campo_Modelo_ID + ")  as MODELO ";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Marcas.Campo_Nombre + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Marca_ID + " = " + "PRODUCTOS." + Cat_Com_Productos.Campo_Marca_ID + ")  as MARCA ";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Unidades.Campo_Nombre + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = " + "PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID+ ")  as UNIDAD ";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = " + "PRODUCTOS." + Cat_Com_Productos.Campo_Partida_ID + ")  as PARTIDA_ESPECIFICA ";
            Mi_SQL = Mi_SQL + " FROM " +  Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";

            if(Datos.P_Partida_Especifica_ID !=null){  // CONSULTA EN BASE A LA PARTIDA ESPECIFICA 

                Mi_SQL = Mi_SQL + " WHERE PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_ID + " = '" + Datos.P_Partida_Especifica_ID + "'";
                where = false;
            }


            if (Datos.P_Modelo_ID != null)  // CONSULTA EN BASE AL MODELO
            {
                if (where == true)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + "PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Modelo_ID + " = '" + Datos.P_Modelo_ID + "'";
                    where = false;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " AND " + "PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Modelo_ID + " = '" + Datos.P_Modelo_ID + "'";
                }
            }


            if (Datos.P_Marca_ID != null) // CONSULTA EN BASE A LA MARCA
            {
                if (where == true)
                {
                    Mi_SQL = Mi_SQL + " WHERE PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "'";
                    where = false;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " AND PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "'";
                }
            }


            if (Datos.P_Proveedor_ID != null) // CONSULTA EN BASE AL PROVEEDOR
            {
                if (where == true)
                {
                    Mi_SQL = Mi_SQL + " WHERE PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";
                    where = false;
                }
                else
                {
                    Mi_SQL = Mi_SQL + " AND PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";
                }
            }


            if ((Datos.P_Letra_Inicial != null) && (Datos.P_Letra_Final != null)) // CONSULTA EN BASE A LA DESCRIPCION A-Z 
            {
                String Letra_Inicial = "" + Datos.P_Letra_Inicial.ToString();
                String Letra_Final = "" + Datos.P_Letra_Final.ToString();

                if ((Letra_Final == "Z") | (Letra_Final == "z"))
                {
                    if (where == true)
                    {
                        Mi_SQL = Mi_SQL + " WHERE UPPER( PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ")" +
                           " between  UPPER('" + Letra_Inicial + "%')  and UPPER('" + "Y" + "%') or UPPER(PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ") like UPPER('Z%')";
                        where = false;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND UPPER( PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ")" +
                       " between  UPPER('" + Letra_Inicial + "%')  and UPPER('" + "Y" + "%') or UPPER(PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ") like UPPER('Z%')";
                    }
                }
                else
                {
                    int Asqui_Letra_Final = Encoding.ASCII.GetBytes(Letra_Final)[0];
                    Asqui_Letra_Final = (Asqui_Letra_Final + 1);
                    char L_fin = Convert.ToChar(Asqui_Letra_Final);
                    Letra_Final = Convert.ToString(L_fin);

                    if (where == true)
                    {
                        Mi_SQL = Mi_SQL + " WHERE UPPER(PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ")" +
                        " between UPPER('" + Letra_Inicial + "%')  and UPPER('" + Letra_Final + "%')";
                        where = false;
                    }
                    else
                    {
                        Mi_SQL = Mi_SQL + " AND UPPER(PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + ")" +
                        " between UPPER('" + Letra_Inicial + "%')  and UPPER('" + Letra_Final + "%')";
                    }
                }
            }

            if (Datos.P_Tipo_Producto != null ) // CONSULTA SOLO PRODUCTOS STOCK o TRANSITORIOS
            {
                if (where == true)
                {
                    if (Datos.P_Tipo_Producto == "TRANSITORIOS")
                    {

                        Mi_SQL = Mi_SQL + " WHERE " + " PRODUCTOS.";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Stock + " = 'NO'";
                    }
                    else if (Datos.P_Tipo_Producto == "STOCK")
                    {
                        Mi_SQL = Mi_SQL + " WHERE " + " PRODUCTOS.";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Stock + " = 'SI'";
                    }
                }
                else
                {
                    if (Datos.P_Tipo_Producto == "TRANSITORIOS")
                    {
                        Mi_SQL = Mi_SQL + " AND  PRODUCTOS.";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Stock + " = 'NO'";
                    }
                    else if (Datos.P_Tipo_Producto == "STOCK")
                    {
                        Mi_SQL = Mi_SQL + " AND PRODUCTOS.";
                        Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Stock + " = 'SI'";
                    }
                }
            }

            Mi_SQL = Mi_SQL + " ORDER BY PRODUCTOS. " +  Cat_Com_Productos.Campo_Nombre; // PARA LA ORDENACIÓN DE LOS PRODUCTOS

            if (Mi_SQL != null && Mi_SQL.Trim().Length > 0) 
             {
                 Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
             }

             return Dt_Consulta;
        }
      
        catch (Exception Ex)
        {
            String Mensaje = "Error al intentar consultar los inventarios. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            throw new Exception(Mensaje);
        }
      }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tablas
        ///DESCRIPCIÓN:          Método utilizado para consultar las Partidas Especificas,
        ///                      Modelos, Marcas y Proveedores
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           05/Mayo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Tablas(Cls_Ope_Com_Alm_Rpts_Productos_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataTable Dt_consulta = new DataTable();

            try
             {
                 if (Datos.P_Nombre_Tabla == "PARTIDAS_ESPECIFICAS")
                 {
                     Mi_SQL = " SELECT DISTINCT " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "."+ Cat_Sap_Partidas_Especificas.Campo_Partida_ID + ",";
                     Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." +Cat_Sap_Partidas_Especificas.Campo_Nombre + " as PARTIDA_ESPECIFICA, ";
                     Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " ";

                     Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                     Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                     Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "."+ Cat_Com_Productos.Campo_Partida_Especifica_ID;
                     Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +"."+ Cat_Sap_Partidas_Especificas.Campo_Nombre;

                 }
                 else if (Datos.P_Nombre_Tabla == "MODELOS")
                 {
                     Mi_SQL = " SELECT DISTINCT " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + ", ";
                     Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " as MODELO";
                     Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;
                     Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                     Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "."+ Cat_Com_Modelos.Campo_Modelo_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos+ "." + Cat_Com_Productos.Campo_Modelo_ID;
                     Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "."+ Cat_Com_Modelos.Campo_Nombre;

                 }
                 else if (Datos.P_Nombre_Tabla == "MARCAS")
                 {
                     Mi_SQL = " SELECT DISTINCT " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + ", ";
                     Mi_SQL = Mi_SQL +   Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." +Cat_Com_Marcas.Campo_Nombre + " as MARCA";
                     Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                     Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                     Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Marca_ID;
                     Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre;

                 }
                 else if (Datos.P_Nombre_Tabla == "PROVEEDORES")
                 {
                     Mi_SQL = " SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                     Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." +Cat_Com_Proveedores.Campo_Compañia + " as PROVEEDOR";
                     Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                     Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                     Mi_SQL = Mi_SQL + " WHERE " +Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                     Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "."+ Cat_Com_Productos.Campo_Proveedor_ID;
                     Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Compañia;
                 }
                 Dt_consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                 return Dt_consulta;
             }
             catch (Exception Ex)
             {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
             }
        }
        #endregion
    }

}
