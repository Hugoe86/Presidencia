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
using Presidencia.Catalogo_Compras_Giro_Proveedor.Negocio;
using Presidencia.Catalogo_Compras_Giros.Negocio;
using Presidencia.Proveedores.Negocios;
using System.Data.OracleClient;


namespace Presidencia.Giro_Proveedor.Datos
{
    public class Cls_Cat_Com_Giro_Proveedor_Datos
    {
        public Cls_Cat_Com_Giro_Proveedor_Datos()
        {

        }

        #region METODOS
            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Consulta_Giro_Proveedor
            ///DESCRIPCIÓN: Busca un elemento dentro del grid view de acuerdo al nombre del proveedor
            ///PARAMETROS: 
            ///CREO: Leslie González Vázquez
            ///FECHA_CREO: 04/Febrero/2011 
            ///MODIFICO: Roberto González Oseguera
            ///FECHA_MODIFICO: 16/Febrero/2011 
            ///CAUSA_MODIFICACIÓN: Agregar los campos USUARIO_CREO y FECHA_CREO a la consulta
            ///*******************************************************************************
            public DataTable Consulta_Giro_Proveedor(Cls_Cat_Com_Giro_Proveedor_Negocio Giro_Proveedor)
            {
                try
                {
                String Mi_SQL = "SELECT ";
                Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + "." + Cat_Com_Giro_Proveedor.Campo_Giro_ID + " AS CONCEPTO_ID, ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Clave + " || " ;
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Descripcion + " AS CONCEPTO ";

                Mi_SQL = Mi_SQL + " FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", ";
                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + " ";

                Mi_SQL = Mi_SQL + " WHERE ";

                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " = ";

                Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + "." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + " ";

                Mi_SQL = Mi_SQL + " AND ";

                Mi_SQL = Mi_SQL + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto + "." + Cat_Sap_Concepto.Campo_Concepto_ID + "=";

                Mi_SQL = Mi_SQL + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + "." + Cat_Com_Giro_Proveedor.Campo_Giro_ID;

                

                    
                //Mi_SQL = "SELECT GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID;
                //Mi_SQL = Mi_SQL + ", PROVEEDOR." + Cat_Com_Proveedores.Campo_Nombre + " AS NOMBRE_PROVEEDOR";
                //Mi_SQL = Mi_SQL + ", GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Giro_ID;
                //Mi_SQL = Mi_SQL + "," + Cat_Com_Giros.Campo_Nombre + " AS NOMBRE_GIRO, ";
                //Mi_SQL = Mi_SQL + "GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Usuario_Creo;
                //Mi_SQL = Mi_SQL + ", GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo;
                //Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor + " GIRO_PROVEEDOR";
                //Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDOR";
                //Mi_SQL = Mi_SQL + " ON PROVEEDOR." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                //Mi_SQL = Mi_SQL + " = GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID;
                //Mi_SQL = Mi_SQL + " JOIN " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto;// +" GIRO ";
                //Mi_SQL = Mi_SQL + " ON " + Cat_Sap_Concepto.Tabla_Cat_SAP_Concepto +"."+ Cat_Sap_Concepto.Campo_Concepto_ID;
                //Mi_SQL = Mi_SQL + " = GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Giro_ID;

                 if (Giro_Proveedor.P_Proveedor_ID != null)
                 {
                     Mi_SQL = Mi_SQL + " AND (" + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." +
                                      Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + " = '" + Giro_Proveedor.P_Proveedor_ID + "')";
                                      //OR (" +
                                      //"GIRO_PROVEEDOR." + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID +
                                      //" IN " +
                                      //" (Select " + Cat_Com_Proveedores.Campo_Proveedor_ID + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores +
                                      //" WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + " LIKE '%" + Giro_Proveedor.P_Proveedor_ID + "%'))";
                 }

                 return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    
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


            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_Giros
            ///DESCRIPCIÓN          : Obtiene todos los Giros que estan dados de alta en la base de datos
            ///PARAMETROS           :   
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 08/Febrero/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public DataTable Consultar_Giros()
            {
                String Mi_SQL = null;
                DataSet Ds_Giros = null;
                DataTable Dt_Giros = null;
                Mi_SQL = "SELECT " + Cat_Com_Giros.Campo_Giro_ID + " AS GIRO_ID, " + 
                                    Cat_Com_Giros.Campo_Nombre + " FROM " + 
                                    Cat_Com_Giros.Tabla_Cat_Com_Giros;

                Ds_Giros = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Giros == null)
                {
                    Dt_Giros = new DataTable();
                }
                else
                {
                    Dt_Giros = Ds_Giros.Tables[0];
                }
                return Dt_Giros;
            }
           
             ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
            ///DESCRIPCIÓN          : Obtiene todos los Giros que estan dados de alta en la base de datos
            ///PARAMETROS           :   
            ///CREO                 : Leslie González Vázquez
            ///FECHA_CREO           : 08/Febrero/2011 
            ///MODIFICO             :
            ///FECHA_MODIFICO       :
            ///CAUSA_MODIFICACIÓN   :
            ///*******************************************************************************
            public DataTable Consultar_Proveedores()
            {
                String Mi_SQL = "SELECT " + Cat_Com_Proveedores.Campo_Proveedor_ID +
                                ", " + Cat_Com_Proveedores.Campo_Nombre +
                                " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;

                DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Data_Table;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Eliminar_Giro_Proveedor
            ///DESCRIPCIÓN:Elimina un giro proveedor en la base de datos
            ///PARAMETROS:  1.- Cls_Cat_Com_Giro_Proveedor_Negocio
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 09/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public void Eliminar_Giro_Proveedor(Cls_Cat_Com_Giro_Proveedor_Negocio Giro_Proveedor)
            {
                String Mi_SQL = " DELETE FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
                    " WHERE " + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + " = '" + Giro_Proveedor.P_Proveedor_ID + "' " +
                    "AND " + Cat_Com_Giro_Proveedor.Campo_Giro_ID + " = '" + Giro_Proveedor.P_Giro_ID + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Eliminar_Giros_Del_Proveedor
            ///DESCRIPCIÓN: Elimina todos los giros en la base de datos del proveedor proporcionado
            ///PARAMETROS:  
            ///         1.Datos: Clase de negocio de la que se va a tomar el ID del proveedor
            ///CREO: Roberto González Oseguera
            ///FECHA_CREO: 16/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public void Eliminar_Giros_Del_Proveedor(Cls_Cat_Com_Giro_Proveedor_Negocio Datos)
            {
                //Verificar que P_Proveedor_ID no sea nulo, para eliminar todos los giros del proveedor
                if (Datos.P_Proveedor_ID != null && Datos.P_Proveedor_ID != "")
                {
                    String Mi_SQL = " DELETE FROM " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
                        " WHERE " + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";

                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Alta_Giro_Proveedor
            ///DESCRIPCIÓN:Da de alta un giro proveedor en la base de datos
            ///PARAMETROS:  1.- Cls_Cat_Com_Giro_Proveedor_Negocio
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 09/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************

            public void Alta_Giro_Proveedor(Cls_Cat_Com_Giro_Proveedor_Negocio Giro_Proveedor)
            {
                try
                {
                String Mi_SQL = "INSERT INTO " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
                    "(" + Cat_Com_Giro_Proveedor.Campo_Giro_ID + ", " + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID +
                    ", " + Cat_Com_Giro_Proveedor.Campo_Usuario_Creo + ", " + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo
                    + ") VALUES ('" + Giro_Proveedor.P_Giro_ID + "', '" + Giro_Proveedor.P_Proveedor_ID +
                    "', '" + Giro_Proveedor.P_Usuario + "', SYSDATE)";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Alta_Giro_Proveedor
            ///DESCRIPCIÓN: Sobrecarga para dar de alta un giro proveedor en la base de datos 
            ///con una fecha determinada
            ///PARAMETROS:  
            ///         1. Cls_Cat_Com_Giro_Proveedor_Negocio: propiedades de la capa de negocio
            ///         2. Fecha: fecha a insertar en la consulta en lugar de la actual
            ///CREO: Roberto González Oseguera
            ///FECHA_CREO: 16/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            public void Alta_Giro_Proveedor(Cls_Cat_Com_Giro_Proveedor_Negocio Giro_Proveedor, String Fecha)
            {
                        //Comprobar el contenido de la cadena Fecha
                if (Fecha == null || Fecha == "")
                    Fecha = "SYSDATE";

                            // Generar la cadena de consulta
                String Mi_SQL = "INSERT INTO " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
                    "(" + Cat_Com_Giro_Proveedor.Campo_Giro_ID + ", " + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID +
                    ", " + Cat_Com_Giro_Proveedor.Campo_Usuario_Creo + ", " + Cat_Com_Giro_Proveedor.Campo_Fecha_Creo
                    + ") VALUES ('" + Giro_Proveedor.P_Giro_ID + "', '" + Giro_Proveedor.P_Proveedor_ID +
                    "', '" + Sessiones.Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";
                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }

            //*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Modificar_Giro_Proveedor
            ///DESCRIPCIÓN: Modificar el giro de un proveedor en la base de datos
            ///PARAMETROS:  1.- Cls_Cat_Com_Giro_Proveedor_Negocio
            ///CREO: Leslie Gonzalez Vazquez
            ///FECHA_CREO: 09/Febrero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************/

            public void Modificar_Giro_Proveedor(Cls_Cat_Com_Giro_Proveedor_Negocio Giro_Proveedor)
            {
                String MI_SQL = "UPDATE " + Cat_Com_Giro_Proveedor.Tabla_Cat_Com_Giro_Proveedor +
                    " SET " + Cat_Com_Giro_Proveedor.Campo_Giro_ID + " = '" + Giro_Proveedor.P_Giro_ID + "', " +
                    Cat_Com_Giro_Proveedor.Campo_Usuario_Modifico + " = '" + Giro_Proveedor.P_Usuario + "', " +
                    Cat_Com_Giro_Proveedor.Campo_Fecha_Modifico + " = SYSDATE" +
                    " WHERE " + Cat_Com_Giro_Proveedor.Campo_Proveedor_ID + " = '" + Giro_Proveedor.P_Proveedor_ID + "' AND " +
                    Cat_Com_Giro_Proveedor.Campo_Giro_ID + "='" + Giro_Proveedor.P_Giro_Id_Anterior + "'";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, MI_SQL);
            }
        #endregion
    }
}


