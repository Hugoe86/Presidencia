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
using Presidencia.Constantes;
using System.Data.OracleClient;
using Presidencia.Sessiones;
using Presidencia.Bitacora_Eventos;
using Presidencia.Almacen_Registro_Datos.Negocio;
using System.Collections;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Generar_Requisicion.Datos;
/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Registro_De_Datos_Datos
/// </summary>
/// 
namespace Presidencia.Almacen_Registro_Datos.Datos
{
    public class Cls_Ope_Com_Alm_Registro_De_Datos_Datos
    {

        #region (Variables Locales)

        #endregion

        #region (Variables Publicas)

        #endregion

        #region (Metodos)

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Método utilizado para consultar las ordenes de compra que se encuentren en estatus "SURTIDA"
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Ordenes_Compra(Cls_Ope_Com_Alm_Registro_De_Datos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Ordenes_Compra = new DataTable(); // Tabla donde se aguardaran las ordenes de compra
            DataTable Dt_Ordenes_C_Registrar = new DataTable(); // Tabla donde se guardaran las ordenes de compra que tieenen productos a registrar
            DataRow[] Registro;
            
            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, "; 
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " AS FECHA_CONSTRUCCION, ";
                Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS REQUISICION,";

                Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS NO_REQUISICION,";

                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + "";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'SURTIDA'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno+ " is not null "; // Cuando se crea un contra recibo, se agrega el No_Factura_Interno a la Orden_Compra, es por eso que esta condision me sirve para optener las ordenes de compra que ahun no se les ha generado su contra recibo
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Registrada + " is null "; // indicando que ahun no se ha registrado
               
                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
                }

                if (Datos.P_Proveedor_ID != null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";
                }

                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " like '%" + Datos.P_No_Requisicion + "%'";
                }

                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                  " AND '" + Datos.P_Fecha_Fin_B + "'";
                }

                Mi_SQL = Mi_SQL + " Order by " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ""; 


                Dt_Ordenes_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                // Se clona la tabla
                Dt_Ordenes_C_Registrar = Dt_Ordenes_Compra.Clone();

                if (Dt_Ordenes_Compra.Rows.Count > 0) // Se verifica que la consulta arroje resultados
                {
                    for(int i =0; i<Dt_Ordenes_Compra.Rows.Count; i++ )
                    {
                        String No_Orden_Compra = Dt_Ordenes_Compra.Rows[i]["NO_ORDEN_COMPRA"].ToString().Trim();
                        Boolean Registrar_OC = Verificar_Orden_Compra(No_Orden_Compra);

                        if (Registrar_OC)// Si la orden de compra contiene productos que deben ser registrados
                        {
                            Registro = Dt_Ordenes_Compra.Select("NO_ORDEN_COMPRA='" + No_Orden_Compra.Trim() + "'");
                            DataRow Dr_Orden_Compra = Dt_Ordenes_C_Registrar.NewRow();

                            Dr_Orden_Compra["NO_ORDEN_COMPRA"] = Registro[0]["NO_ORDEN_COMPRA"].ToString().Trim();
                            Dr_Orden_Compra["PROVEEDOR"] = Registro[0]["PROVEEDOR"].ToString().Trim();
                            Dr_Orden_Compra["FECHA_CONSTRUCCION"] = Registro[0]["FECHA_CONSTRUCCION"].ToString().Trim();
                            Dr_Orden_Compra["ESTATUS"] = Registro[0]["ESTATUS"].ToString().Trim();
                            Dr_Orden_Compra["TOTAL"] = Registro[0]["TOTAL"].ToString().Trim();
                            Dr_Orden_Compra["FOLIO"] = Registro[0]["FOLIO"].ToString().Trim();
                            Dr_Orden_Compra["REQUISICION"] = Registro[0]["REQUISICION"].ToString().Trim();
                            Int16 Longitud = Convert.ToInt16(Dt_Ordenes_Compra.Rows.Count);
                            if(Longitud==0)
                                Dt_Ordenes_C_Registrar.Rows.InsertAt(Dr_Orden_Compra, Longitud);
                            else
                                Dt_Ordenes_C_Registrar.Rows.InsertAt(Dr_Orden_Compra, (Longitud + 1));
                        }
                    }
                }

                //Entregar resultado
                return Dt_Ordenes_C_Registrar;
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
        /// NOMBRE DE LA CLASE:     Verificar_Orden_Compra
        /// DESCRIPCION:            Realiza una consulta para verificar si la orden de compra tiene productos que deben ser registrados
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos.                    
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            04/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Boolean Verificar_Orden_Compra(String No_Orden_Compra)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //variable apra las consultas
            Boolean Registrar = false;
            DataTable Dt_Productos_Registrar = new DataTable();

            // Asignar consulta
            Mi_SQL = "SELECT PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Unidad + ", ";
            Mi_SQL = Mi_SQL + "PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + ", ";
            Mi_SQL = Mi_SQL + "PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo_Transitorio+ " ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PRODUCTOS_CONTRARECIBO, ";
            Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURAS_PROVEEDORES, ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA ";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " AND FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + No_Orden_Compra.Trim();
            
            Dt_Productos_Registrar = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            if (Dt_Productos_Registrar.Rows.Count > 0)
            {
                for (int j = 0; j < Dt_Productos_Registrar.Rows.Count; j++)
                {
                    String Unidad =Dt_Productos_Registrar.Rows[j]["UNIDAD"].ToString().Trim();
                    String Totalidad = Dt_Productos_Registrar.Rows[j]["TOTALIDAD"].ToString().Trim();
                    String Recibo_Transitorio = Dt_Productos_Registrar.Rows[j]["RECIBO_TRANSITORIO"].ToString().Trim();

                    if (((Unidad == "SI") | (Totalidad == "SI")) & (Recibo_Transitorio=="NO"))
                    {
                        Registrar = true;
                        return Registrar;
                    }else
                        Registrar = false;
                }
            }
            else
                Registrar = false;

            return Registrar;
        }
        
        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Tablas
        /// DESCRIPCION:            Consultar los datos de los proveedores
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para la busqueda
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Tablas(Cls_Ope_Com_Alm_Registro_De_Datos_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //variable apra las consultas

            try
            {
                if (Datos.P_Tipo_Tabla == "PROVEEDORES")
                {
                    //Consulta
                    Mi_SQL = " SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS " + Cat_Com_Proveedores.Campo_Compañia + "";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                    Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                    Mi_SQL = Mi_SQL + " and " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'SURTIDA' ";
                    Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre; // Ordenamiento
                }
                else if (Datos.P_Tipo_Tabla == "MATERIALES")
                {
                    //Consulta
                    Mi_SQL = " SELECT DISTINCT " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Material_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion + " as MATERIAL";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion; // Ordenamiento
                }
                else if (Datos.P_Tipo_Tabla == "COLORES")
                {
                    //Consulta
                    Mi_SQL = " SELECT DISTINCT " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Color_ID + ", ";
                    Mi_SQL = Mi_SQL + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion+ " as COLOR";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + "." + Cat_Pat_Colores.Campo_Descripcion; // Ordenamiento
                }
                else if (Datos.P_Tipo_Tabla == "MARCAS")
                {
                    //Consulta
                    Mi_SQL = " SELECT DISTINCT " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID+ ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre+ " as MARCA";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre; // Ordenamiento
                }
                else if (Datos.P_Tipo_Tabla == "MODELOS")
                {
                    //Consulta
                    Mi_SQL = " SELECT DISTINCT " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID+ ", ";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " as MODELO";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre; // Ordenamiento
                }
                else if (Datos.P_Tipo_Tabla == "DATOS_GENERALES_OC")
                {
                    //Consulta
                    Mi_SQL = " SELECT ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                    Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ", ";
                    Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + ", "; //  Se consulta el No_Contra_Recibo
                    Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " AS NO_REQUISICION, "; //  Se consulta el No_Contra_Recibo

                    Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                    Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                    Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                    Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS REQUISICION,";
                    Mi_SQL = Mi_SQL + " FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + ", ";
                    Mi_SQL = Mi_SQL + " FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + ", ";
                    Mi_SQL = Mi_SQL + " PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " AS " + Cat_Com_Proveedores.Campo_Compañia;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra+ " ORDENES_COMPRA, ";
                    Mi_SQL = Mi_SQL + " " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores+ " FACTURAS_PROVEEDORES, ";
                    Mi_SQL = Mi_SQL + " " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores+ " PROVEEDORES ";
                    Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID+ " = ";
                    Mi_SQL = Mi_SQL + " PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                    Mi_SQL = Mi_SQL + " AND FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
                    Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
                    Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra+ " = ";
                    Mi_SQL = Mi_SQL + Datos.P_No_Orden_Compra.Trim();
                }

                // Resultado
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
        /// NOMBRE DE LA CLASE:     Alta_Productos_Inventario
        /// DESCRIPCION:            Metodo para dar de alta los productos que se serializaron
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            06/Julio/2011 
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:     Se agregó el campo CANTIDAD
        ///*******************************************************************************/
        public static void Alta_Productos_Inventario(Cls_Ope_Com_Alm_Registro_De_Datos_Negocio Datos)
        {            
            //Declaracion de variables
            String Mensaje = "";
            String Mi_SQL = String.Empty;
            String Estatus_Req = "";
            Object Aux;
            Int64 No_Inventario;
            Int64 Inventario_Recibo;
            Int64 Inventario_Resguardo;
            Int64 Inventario = 0;
            String Operacion = "";
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;           
            DataTable Dt_Productos_Serializados = new DataTable(); // Se crea la tabla para guardar los productos serializados
            Dt_Productos_Serializados =(DataTable) Datos.P_Dt_Productos_Serializados;
            try
            {
                // Se consulta el maximo numero de Inventario //ID
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                //Ejecutar consulta
                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Verificar si no es nulo
                if (Aux != null && Convert.IsDBNull(Aux) == false)
                    No_Inventario =  Convert.ToInt64(Aux) + 1;
                else
                    No_Inventario =  1;

                ////Consultar Inventario para RESGUARDO
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + " = 'RESGUARDO'";
                    //Ejecutar consulta
                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Verificar si no es nulo
                if (Aux != null && Convert.IsDBNull(Aux) == false)
                    Inventario_Resguardo = Convert.ToInt64(Aux) + 1;
                else
                    Inventario_Resguardo = 1;

                //Consultar Inventario para RECIBO
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + " = 'RECIBO'";
                    //Ejecutar consulta
                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                //Verificar si no es nulo
                if (Aux != null && Convert.IsDBNull(Aux) == false)
                    Inventario_Recibo = Convert.ToInt64(Aux) + 1;
                else
                    Inventario_Recibo = 1;

                DataTable Tabla = null;
                  

                for (int i = 0; i < Dt_Productos_Serializados.Rows.Count; i++ ){
                    //Verificar si el producto es resguardo o recibo
                    Mi_SQL = "SELECT NVL(RESGUARDO,'NO') RESGUARDO, NVL(RECIBO,'NO') RECIBO, " +
                        "PRODUCTO_ID FROM OPE_ALM_PROD_CONTRARECIBO WHERE NO_CONTRA_RECIBO = " + Datos.P_No_ContraRecibo.ToString().Trim() +
                        " AND PRODUCTO_ID = '" + Dt_Productos_Serializados.Rows[i]["PRODUCTO_ID"].ToString().Trim() + "'";
                    try
                    {
                        Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    }
                    catch(Exception Ex)
                    {
                        throw new Exception(Ex.ToString());
                    }
                    if (Tabla.Rows[0]["RECIBO"].ToString() == "SI" && Tabla.Rows[0]["RESGUARDO"].ToString() == "NO")
                    {
                        Operacion = "RECIBO";
                        Inventario = Inventario_Recibo;
                        Inventario_Recibo++;
                    }
                    else if (Tabla.Rows[0]["RECIBO"].ToString() == "NO" && Tabla.Rows[0]["RESGUARDO"].ToString() == "SI")
                    {
                        Operacion = "RESGUARDO";
                        Inventario = Inventario_Resguardo;
                        Inventario_Resguardo++;
                    }
                    // Asignar consulta para ingresar la factura
                    Mi_SQL = "INSERT INTO " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " (";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + ", "; // Es el No de  contra recibo
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Marca_Id + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Modelo+ ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Garantia + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Serie + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Cantidad + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Contra_Recibo+ ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Fecha_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Observaciones + ",";
                    Mi_SQL = Mi_SQL + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + ") ";
                    Mi_SQL = Mi_SQL + "VALUES(" + No_Inventario + ", ";
                    Mi_SQL = Mi_SQL + Inventario.ToString() + ", ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["PRODUCTO_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["MARCA_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["MODELO"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["GARANTIA"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["COLOR_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["MATERIAL_ID"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["NO_SERIE"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["CANTIDAD"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + Datos.P_No_ContraRecibo.ToString().Trim() + ", ";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Usuario_Creo.ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "SYSDATE,";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["OBSERVACIONES"].ToString().Trim() + "', ";
                    Mi_SQL = Mi_SQL + "'" + Operacion + "')";

                    //Ejecutar consulta
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación

                    //return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
                    No_Inventario++; // Se incrementa el No. Inventario

                    // Se actualiza el producto para indicar que ya estan registrados
                    Mi_SQL = "UPDATE " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Alm_Productos_Contrarecibo.Campo_Registrado + " = " + "'SI'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = ";
                    Mi_SQL = Mi_SQL + "'" + Dt_Productos_Serializados.Rows[i]["PRODUCTO_ID"].ToString().Trim() + "' ";
                    Mi_SQL = Mi_SQL + " and " + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + " = ";
                    Mi_SQL = Mi_SQL + "" + Datos.P_No_ContraRecibo + " ";

                    //Ejecutar consulta
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación
                }

              //SE ACTUALIZA LA ORDEN DE COMPRA
                Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Registrada + "=" + "'SI'";
                Mi_SQL = Mi_SQL + ", " + Ope_Com_Ordenes_Compra.Campo_Estatus + "=" + "'TRANSITORIO'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " =" + Datos.P_No_Orden_Compra;

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación

                // Se determina el estatus de la requisicion
                Mi_SQL = " SELECT " + Ope_Com_Requisiciones.Campo_Estatus;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " where " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = " + Datos.P_No_Requisicion.Trim();

                //Ejecutar consulta
                Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Aux != null && Convert.IsDBNull(Aux) == false)
                    Estatus_Req  = Convert.ToString(Aux);

                if (Estatus_Req.Trim() == "COMPLETA")
                {
                    //SE ACTUALIZA LA ORDEN DE COMPRA
                    Mi_SQL = " UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus + "='PARCIAL'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " =" + Datos.P_No_Requisicion.Trim();
                    //Ejecutar consulta
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación
                }
                Trans.Commit(); // Se ejecuta la transacciones
                Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("SURTIDA / REGISTRO DATOS", Datos.P_No_Requisicion);
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave,  Vuelva a intentar. Error: [" + Ex.Message + "]";
                    }
                    else if (Ex.Message.IndexOf("UNIQUE") != -1)
                    {
                        Mensaje = "Esta intentando grabar un registro que ya existe, verifiquelo por favor. Error: [" + Ex.Message + "]";
                    }
                    else
                    {
                        Mensaje = "Error general en la base de datos. Error: [" + Ex.Message + "]";
                    }
                }
                else if (Ex.Code == 547)
                {
                    Mensaje = "Esta intentando introducir algún dato que no existe y que esta relacionado con otra tabla. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 515)
                {
                    Mensaje = "Algunos datos no han sido ingresados y son necesarios para completar la operación, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else
                {
                    Mensaje = "Intente nuevamente por favor. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                throw new Exception(Mensaje); // Se indica el mensaje 
            }
            finally
            {
                Cn.Close();
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Productos_Orden_Compra
        /// DESCRIPCION:            Consulta los productos de la orden de compra seleccionada por el usuarioa
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            01/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Productos_Orden_Compra(Cls_Ope_Com_Alm_Registro_De_Datos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + ", "; 
                Mi_SQL = Mi_SQL + "PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Unidad + ", ";
                Mi_SQL = Mi_SQL + "PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + ", ";

                Mi_SQL = Mi_SQL + "PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + ", ";
                Mi_SQL = Mi_SQL + "PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + ", ";

                Mi_SQL = Mi_SQL + "REPLACE(" + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + ", " + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + ", '') AS NO_INVENTARIO ";
                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE";

                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Tipo + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as TIPO";

                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO ";

                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION ";

                Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + " from ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
                Mi_SQL = Mi_SQL + " where PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = REQ_PRODUCTO.";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +  " AND REQ_PRODUCTO." +Ope_Com_Req_Producto.Campo_No_Orden_Compra;
                Mi_SQL = Mi_SQL  +  " = "+ Datos.P_No_Orden_Compra.Trim() + ")as CANTIDAD";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PRODUCTOS_CONTRARECIBO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURAS_PROVEEDORES, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA ";
                Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;  // Campo_No_Factura_Interno Contiene el Numero de Contra Recibo
                Mi_SQL = Mi_SQL + " AND FACTURAS_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = "; // Campo_No_Factura_Interno Contiene el Numero de Contra Recibo
                Mi_SQL = Mi_SQL + " PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
                Mi_SQL = Mi_SQL + " AND ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Orden_Compra.Trim();
                Mi_SQL = Mi_SQL + " AND PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo_Transitorio + " = 'NO' "; // Campo_No_Factura_Interno Contiene el Numero de Contra Recibo
  
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
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
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Método utilizado para consultar las ordenes de compra que se encuentren en estatus "SURTIDA"
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene la información para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Int64 Consulta_Consecutivo(Cls_Ope_Com_Alm_Registro_De_Datos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas
            Object Consecutivo;
            Int64 No_Consecutivo;
            
            try
            {
                if(Datos.P_Tipo_Tabla=="BIENES_MUEBLES"){                
                // Consulta 
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario+ "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                }
                else if (Datos.P_Tipo_Tabla == "VEHICULO")
                {
                    // Consulta 
                    Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Pat_Inv_Vehiculos.Campo_Inventario + "), 0) ";
                    Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Pat_Inv_Vehiculos.Tabla_Ope_Alm_Pat_Inv_Vehiculos;
                }     

                //Ejecutar consulta
                Consecutivo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Consecutivo != null && Convert.IsDBNull(Consecutivo) == false)
                    No_Consecutivo = Convert.ToInt64(Consecutivo) + 1;
                else
                    No_Consecutivo = 1;

                return No_Consecutivo;
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

        #endregion
    }
}
