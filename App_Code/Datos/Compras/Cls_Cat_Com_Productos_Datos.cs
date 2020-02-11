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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Productos.Negocio;


namespace Presidencia.Catalogo_Compras_Productos.Datos
{
    public class Cls_Cat_Com_Productos_Datos
    {
        public Cls_Cat_Com_Productos_Datos()
        {
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Producto_Para_Excel
        /// 	DESCRIPCIÓN: Consulta los datos de los productos registrados en la base de datos
        /// 	PARÁMETROS:
        /// 	CREO: Susana Trigueros
        /// 	FECHA_CREO: 02-feb-2013
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Producto_Para_Excel()
        {
            String Mi_SQL; //Variable para la consulta de los productos
            DataTable Dt_Productos;
            try
            {
                //Consulta todos los datos de los producto 

                Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Tipo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Costo_Promedio + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Nombre + " AS UNIDAD, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Existencia + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Comprometido + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Disponible + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Reorden + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Tabla_Cat_Impuestos + "." + Cat_Com_Impuestos.Campo_Nombre + " AS IMPUESTO,  ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " || '-' || ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA, ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Stock + " AS STOCK ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " LEFT JOIN " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades;
                Mi_SQL = Mi_SQL + " ON " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                Mi_SQL = Mi_SQL + "  ON " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Partida_ID + " = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
                Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " " + Cat_Com_Impuestos.Tabla_Cat_Impuestos;
                Mi_SQL = Mi_SQL + " ON " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Impuesto_ID + " = " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + "." + Cat_Com_Impuestos.Campo_Impuesto_ID;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Estatus + " != 'INICIAL'";

              
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Alta_Producto
        /// 	DESCRIPCIÓN: 1.Consulta el último ID dado de alta para poder ingresar el siguiente
        ///                  2. Da de Alta el Producto en la BD con los datos proporcionados por el usuario
        /// 	PARÁMETROS:
        /// 		        1. Datos: Contiene los datos que serán insertados en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 03-feb-2011
        /// 	MODIFICÓ: Jesus Toledo Rodriguez 
        /// 	FECHA_MODIFICÓ: 04-Abril-2011
        /// 	CAUSA_MODIFICACIÓN: Modificacion general del fomrulario
        ///*******************************************************************************************************
        public static String Alta_Producto(Cls_Cat_Com_Productos_Negocio Datos)
        {
            String Mi_SQL;  //Obtiene la cadena de inserción hacía la base de datos
            Object Producto_ID; //Obtiene el ID con la cual se guardo los datos en la base de datos
            object Aux;
            String Clave;

            try
            {
                //Consulta para generar Clave
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(SUBSTR(" + Cat_Com_Productos.Campo_Clave + ",7,4)),'0000') ";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Partida_Especifica_ID + " = '";
                Mi_SQL = Mi_SQL + Datos.P_Partida_Especifica_ID + "'";

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Aux))
                {
                    Clave = Datos.P_Clave + "PR" + "0001";
                }
                else
                {
                    try
                    {
                        Clave = String.Format("{0:0000}", Convert.ToInt32(Aux) + 1);
                    }
                    catch (Exception Ex)
                    {
                        Clave = Datos.P_Clave + "PR" + "0001";
                    }
                    String respaldo = Clave;
                    Clave = Datos.P_Clave + "PR" + Clave;
                    if (Clave.Length > 13)
                        Clave = respaldo;                    

                }

                //Consulta para generar ID
                Mi_SQL = "";
                Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Productos.Campo_Producto_ID + "),'0000000000')";
                Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                Producto_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Producto_ID))
                {
                    Datos.P_Producto_ID = "0000000001";
                }
                else
                {
                    Datos.P_Producto_ID = String.Format("{0:0000000000}", Convert.ToInt32(Producto_ID) + 1);
                }

                Clave = int.Parse(Datos.P_Producto_ID) +"";// Para que se le quiten los ceros y nada mas de el numero entero

                //Consulta para la inserción del producto con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " (";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ", " + Cat_Com_Productos.Campo_Unidad_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_Especifica_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Impuesto_ID + ", " + Cat_Com_Productos.Campo_Impuesto_2_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Clave + ", " + Cat_Com_Productos.Campo_Nombre + ", " + Cat_Com_Productos.Campo_Descripcion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo + ", " + Cat_Com_Productos.Campo_Costo_Promedio + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Estatus + ", " + Cat_Com_Productos.Campo_Existencia + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Comprometido + ", " + Cat_Com_Productos.Campo_Disponible + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Maximo + ", " + Cat_Com_Productos.Campo_Minimo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Reorden + ", " + Cat_Com_Productos.Campo_Ubicacion + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Tipo + ", " + Cat_Com_Productos.Campo_Stock + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Resguardo + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Ruta_Foto + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Usuario_Creo + ", " + Cat_Com_Productos.Campo_Fecha_Creo + ") VALUES ('";
                Mi_SQL = Mi_SQL + Datos.P_Producto_ID + "', '" + Datos.P_Unidad_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Partida_Especifica_ID + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Impuesto_ID + "', '" + Datos.P_Impuesto_2_ID + "', '";
                Mi_SQL = Mi_SQL + Clave + "', '" + Datos.P_Nombre + "', '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Datos.P_Costo + ", " + Datos.P_Costo_Promedio + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Estatus + "', " + Datos.P_Existencia + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Comprometido + ", " + Datos.P_Disponible + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Maximo + ", " + Datos.P_Minimo + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Reorden + ", '" + Datos.P_Ubicacion + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Tipo + "', '" + Datos.P_Stock + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Resguardo + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Ruta_Foto + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                return Datos.P_Producto_ID.Trim();
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Producto
        /// 	DESCRIPCIÓN: Modifica los datos del Producto con los que fueron introducidos por el usuario
        /// 	PARÁMETROS:
        /// 		1. Datos: Contiene los datos que serán modificados en la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 03-feb-2011 
        /// 	MODIFICÓ: Jesus Toledo Rodriguez
        /// 	FECHA_MODIFICÓ: 05-Abril-2011
        /// 	CAUSA_MODIFICACIÓN: Modificacion estructura de tabla de productos
        ///*******************************************************************************************************
        public static void Modificar_Producto(Cls_Cat_Com_Productos_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            object Aux;
            String Clave;

            try
            {
                //// Consulta para generar Clave, esta si se debe generar, ya que como se le cambia de partida
                //// Una nueva clave se debe generar para actualizarce el registro.
                //Mi_SQL = "";
                //Mi_SQL = "SELECT NVL(MAX(SUBSTR(" + Cat_Com_Productos.Campo_Clave + ",7,4)),'0000') ";
                //Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                //Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Productos.Campo_Partida_Especifica_ID + " = '";
                //Mi_SQL = Mi_SQL + Datos.P_Partida_Especifica_ID + "'";

                //Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //if (Convert.IsDBNull(Aux))
                //{
                //    Clave = Datos.P_Clave + "PR" + "0001";
                //}
                //else
                //{
                //    try
                //    {
                //        Clave = String.Format("{0:0000}", Convert.ToInt32(Aux) + 1);
                //    }
                //    catch (Exception Ex)
                //    {
                //        Clave = Datos.P_Clave + "PR" + "0001";
                //    }
                //    String respaldo = Clave;
                //    Clave = Datos.P_Clave + "PR" + Clave;
                //    if (Clave.Length > 13)                    
                //        Clave = respaldo;                    
                //}

                //Consulta para la inserción del producto con los datos proporcionados por el usuario
                //Mi_SQL = "INSERT INTO " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " (";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ", " + Cat_Com_Productos.Campo_Unidad_ID + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_Especifica_ID + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Impuesto_ID + ", " + Cat_Com_Productos.Campo_Impuesto_2_ID + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Clave + ", " + Cat_Com_Productos.Campo_Nombre + ", " + Cat_Com_Productos.Campo_Descripcion + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo + ", " + Cat_Com_Productos.Campo_Costo_Promedio + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Estatus + ", " + Cat_Com_Productos.Campo_Existencia + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Comprometido + ", " + Cat_Com_Productos.Campo_Disponible + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Maximo + ", " + Cat_Com_Productos.Campo_Minimo + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Reorden + ", " + Cat_Com_Productos.Campo_Ubicacion + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Tipo + ", " + Cat_Com_Productos.Campo_Stock + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Resguardo + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Ruta_Foto + ", ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Usuario_Creo + ", " + Cat_Com_Productos.Campo_Fecha_Creo + ") VALUES ('";
                //Mi_SQL = Mi_SQL + Datos.P_Producto_ID + "', '" + Datos.P_Unidad_ID + "', '";
                //Mi_SQL = Mi_SQL + Datos.P_Partida_Especifica_ID + "', '";
                //Mi_SQL = Mi_SQL + Datos.P_Impuesto_ID + "', '" + Datos.P_Impuesto_2_ID + "', '";
                //Mi_SQL = Mi_SQL + Clave + "', '" + Datos.P_Nombre + "', '" + Datos.P_Descripcion + "', ";
                //Mi_SQL = Mi_SQL + Datos.P_Costo + ", " + Datos.P_Costo_Promedio + ", '";
                //Mi_SQL = Mi_SQL + Datos.P_Estatus + "', " + Datos.P_Existencia + ", ";
                //Mi_SQL = Mi_SQL + Datos.P_Comprometido + ", " + Datos.P_Disponible + ", ";
                //Mi_SQL = Mi_SQL + Datos.P_Maximo + ", " + Datos.P_Minimo + ", ";
                //Mi_SQL = Mi_SQL + Datos.P_Reorden + ", '" + Datos.P_Ubicacion + "', '";
                //Mi_SQL = Mi_SQL + Datos.P_Tipo + "', '" + Datos.P_Stock + "', '";
                //Mi_SQL = Mi_SQL + Datos.P_Resguardo + "', '";
                //Mi_SQL = Mi_SQL + Datos.P_Ruta_Foto + "', '";
                //Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', SYSDATE)";

                 //Consulta para actualizar el producto con los datos proporcionados por el usuario
                Mi_SQL = " UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " SET ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Clave + " = '" + Datos.P_Clave + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Resguardo + " = '" + Datos.P_Resguardo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Unidad_ID + " = '" + Datos.P_Unidad_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Partida_Especifica_ID + " = '" + Datos.P_Partida_Especifica_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Impuesto_ID + " = '" + Datos.P_Impuesto_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Impuesto_2_ID + " = '" + Datos.P_Impuesto_2_ID + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Descripcion + " = '" + Datos.P_Descripcion + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Estatus + " = '" + Datos.P_Estatus + "', ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Existencia + " = '" + Datos.P_Existencia + "', ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Comprometido + " = '" + Datos.P_Comprometido + "', ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Disponible + " = '" + Datos.P_Disponible + "', ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Maximo + " = '" + Datos.P_Maximo + "', ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Minimo + " = '" + Datos.P_Minimo + "', ";
                //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Reorden + " = '" + Datos.P_Reorden + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Ubicacion + " = '" + Datos.P_Ubicacion + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Tipo + " = '" + Datos.P_Tipo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Stock + " = '" + Datos.P_Stock + "', ";

                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo + " = '" + Datos.P_Costo + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo_Promedio + " = '" + Datos.P_Costo_Promedio + "', ";

                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "', ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Fecha_Modifico + " = SYSDATE WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Modificar_Foto_Producto
        /// 	DESCRIPCIÓN:    Modifica la foto del producto
        /// 	PARÁMETROS:     1. Datos: Contiene los datos que serán modificados en la base de datos
        /// 	CREO:           Salvador Hernández Ramìrez
        /// 	FECHA_CREO:     20-Junio-2011 
        /// 	MODIFICÓ:        
        /// 	FECHA_MODIFICÓ:
        /// 	CAUSA_MODIFICACIÓN:
        ///*******************************************************************************************************
        public static void Modificar_Foto_Producto(Cls_Cat_Com_Productos_Negocio Datos)
        {
            String Mi_SQL; // Obtiene la cadena de modificación hacía la base de datos

            //Consulta para actualizar el producto con los datos proporcionados por el usuario
            Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " SET ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Ruta_Foto + " = '" + Datos.P_Ruta_Foto + "'";
            Mi_SQL = Mi_SQL + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";

            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        }



        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Datos_Producto
        /// 	DESCRIPCIÓN: Consulta todos los datos de los productos dados de alta en la BD y sus relaciones
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica que registro se desea consultar a la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 03-feb-2011
        /// 	MODIFICÓ: Jesus Toledo Rodriguez
        /// 	FECHA_MODIFICÓ: *05-Abril-2011*
        /// 	CAUSA_MODIFICACIÓN: Modificacion estructura de tabla de productos
        ///*******************************************************************************************************
        public static DataTable Consulta_Datos_Producto(Cls_Cat_Com_Productos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de los productos
            object Aux;
            DataTable Dt_Productos;            
            try
            {
                    //Consulta todos los datos del producto seleccionado por el usuario
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + ".*, ";
                    Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " AS MARCA_NOMBRE, ";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " AS MODELOS_NOMBRE, ";
                    Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Nombre + " AS UNIDADES_NOMBRE, ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR_NOMBRE, ";
                    Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS DESCRIPCION_P_ESPECIFICA,";
                    Mi_SQL = Mi_SQL + " NULL AS PARTIDA_GENERICA_ID, NULL AS CONCEPTO_ID, NULL AS CAPITULO_ID,";
                    Mi_SQL = Mi_SQL + " NULL AS P_ESPECIFICA_DESCRIPCION, NULL AS P_GENERICA_DESCRIPCION, NULL AS CONCEPTO_DESCRIPCION, NULL AS CAPITULO_DESCRIPCION";                    

                    Mi_SQL = Mi_SQL + " FROM ";

                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos;

                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " ON " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos +"."+ Cat_Com_Productos.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " ON " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " ON " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades +"."+ Cat_Com_Unidades.Campo_Unidad_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ON " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +"."+ Cat_Com_Proveedores.Campo_Proveedor_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " ON " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +"."+ Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Partida_ID;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " IS NOT NULL";

                    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Estatus + " IN ('" + Datos.P_Estatus + "')";    

                    if (Datos.P_Producto_ID != null)
                    {
                        Mi_SQL = Mi_SQL + " and " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                    }
                    else if (Datos.P_Nombre != null)
                    {
                        Mi_SQL = Mi_SQL + " and (UPPER(" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                        Mi_SQL = Mi_SQL + " OR UPPER(" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Nombre + "%'))";
                    }
                    if (Datos.P_Descripcion != null)
                    {
                        Mi_SQL = Mi_SQL + " and UPPER(" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Descripcion + ") LIKE UPPER('%" + Datos.P_Descripcion + "%')";
                    }
                    if (Datos.P_Clave != null)
                    {
                        //Mi_SQL = Mi_SQL + " AND UPPER (" + Cat_Com_Productos.Campo_Clave + ") = UPPER('" + Datos.P_Clave + "')";
                        Mi_SQL = Mi_SQL + " and UPPER(" + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + ") LIKE UPPER('%" + Datos.P_Clave + "%')";
                    }
                    if (Datos.P_Tipo != null) {
                        Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Tipo + "='" + Datos.P_Tipo.Trim() + "'";
                    }
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre;                
                
                    return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// 	NOMBRE_FUNCIÓN: Consulta_Productos
        /// 	DESCRIPCIÓN: Consulta los productos (Nombre y Producto_ID) en la BD filtrados por campo
        /// 	PARÁMETROS:
        /// 		1. Datos: Indica qué registro se desea consultar a la base de datos
        /// 	CREO: Roberto González
        /// 	FECHA_CREO: 03-feb-2011
        /// 	MODIFICÓ: 
        /// 	FECHA_MODIFICÓ: 
        /// 	CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Productos(Cls_Cat_Com_Productos_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de llos productos

            try
            {
                Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Producto_ID + ", " + Cat_Com_Productos.Campo_Nombre;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                if (Datos.P_Producto_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                }
                if (Datos.P_Nombre != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER(" + Cat_Com_Productos.Campo_Nombre + ") LIKE UPPER('%" + Datos.P_Nombre + "%')";
                }
                if (Datos.P_Descripcion != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Descripcion + " LIKE '%" + Datos.P_Descripcion + "%'";
                }
                if (Datos.P_Clave != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE UPPER (" + Cat_Com_Productos.Campo_Clave + ") = UPPER('" + Datos.P_Clave + "')";
                }
                if (Datos.P_Modelo_ID != null)
                {
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Modelo_ID + " = '" + Datos.P_Modelo_ID + "'";
                }
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Productos.Campo_Nombre;
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        internal static string Consulta_Descripcion(string Clave)
        {
            String Mi_SQL; //Variable para la consulta de llos productos

            try
            {
                Mi_SQL = "SELECT " + Cat_Sap_Partidas_Especificas.Campo_Descripcion;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Sap_Partidas_Especificas.Campo_Clave + "='" + Clave + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0].Rows[0][0].ToString();
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }


        public static String Consulta_Foto_Producto(Cls_Cat_Com_Productos_Negocio Datos)
        {
            String Mi_SQL; //Obtiene la cadena de modificación hacía la base de datos
            object Aux;
           
            String Ruta_Foto = "";

            try
            {
                Mi_SQL = " SELECT " + Cat_Com_Productos.Campo_Ruta_Foto;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = ";
                Mi_SQL = Mi_SQL + "" + Datos.P_Producto_ID + "";

                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (!Convert.IsDBNull(Aux))
                    Ruta_Foto = Convert.ToString(Aux);
                else
                    Ruta_Foto = "";

               
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }

            return Ruta_Foto;
        }


        internal static DataTable Consulta_Indices_Producto(String Partida_Especifica_ID)
        {
            String Mi_SQL; //Variable para la consulta de llos productos

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + ", "; ;
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Descripcion + " AS P_ESPECIFICA_DESCRIPCION , "; ;
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID + ", "; ;

                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Descripcion + " AS P_GENERICA_DESCRIPCION, "; ;
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Concepto_ID + ", "; ;

                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " AS CONCEPTO_DESCRIPCION, "; ;
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID + ", "; ;

                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Descripcion + " AS CAPITULO_DESCRIPCION";

                Mi_SQL = Mi_SQL + " FROM ";
                
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas +", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + ", ";
                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + " ";

                Mi_SQL = Mi_SQL + " WHERE ";

                Mi_SQL = Mi_SQL + Cat_SAP_Capitulos.Tabla_Cat_SAP_Capitulos + "." + Cat_SAP_Capitulos.Campo_Capitulo_ID + " = " ;
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Capitulo_ID;

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Concepto_ID;

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Cat_SAP_Partida_Generica.Tabla_Cat_SAP_Partida_Generica + "." + Cat_SAP_Partida_Generica.Campo_Partida_Generica_ID + " = ";
                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_Generica_ID;

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = ";
                Mi_SQL = Mi_SQL + "'" + Partida_Especifica_ID + "'";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        public static DataTable Consultar_Productos_Ocupados(Cls_Cat_Com_Productos_Negocio Datos)
        {

            String Mi_SQL; //Variable para la consulta de llos productos

            try
            {
                Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " "; ;
                
                Mi_SQL = Mi_SQL + " FROM ";

                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                Mi_SQL = Mi_SQL + " ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID; ;
                Mi_SQL = Mi_SQL + "=" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +"." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " WHERE ";

                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Producto_ID.Trim() + "'";

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo_Articulo + " ='PRODUCTO' ";
                
                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + " NOT IN ('CERRADA', 'CANCELADA','COMPLETA')";
                
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
            
        }

    }


}
