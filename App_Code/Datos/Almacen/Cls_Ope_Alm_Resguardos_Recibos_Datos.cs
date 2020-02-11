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
using System.Xml.Linq;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Resguardos_Recibos.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Generar_Requisicion.Datos;
namespace Presidencia.Resguardos_Recibos.Datos
{
    public class Cls_Ope_Alm_Resguardos_Recibos_Datos
    {
        public Cls_Ope_Alm_Resguardos_Recibos_Datos()
        {
        }

        public static DataTable Consultar_Datos_G_Ordenes_Compra(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + " " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura + ", ";
                Mi_SQL = Mi_SQL + " " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
                Mi_SQL = Mi_SQL + " = " +  Datos.P_No_Contra_Recibo.Trim() +" ";

                //Entregar resultado
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
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Ordenes_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar las ordenes de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Compra(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Ordenes_Compra = new DataTable();
            DataRow []Registro;
            try
            {
                Mi_SQL = "SELECT distinct " + "REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ""; //NO_REQUISICION
                Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
                Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";
                Mi_SQL = Mi_SQL + ", PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " as PROVEEDOR";
                Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " AS FECHA";
                Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total_Cotizado + " AS TOTAL";
                Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + "";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + "";
                Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQ";
                Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo+ " ";

                Mi_SQL = Mi_SQL + ",(select " + Ope_Com_Ordenes_Compra.Campo_Folio + " from ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " where ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + " ) as FOLIO_OC ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PROD_CONTRARECIBO";
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO";
                Mi_SQL = Mi_SQL + " ON REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra +  " =";
                Mi_SQL = Mi_SQL + " (select " + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " from ";
                Mi_SQL = Mi_SQL +  Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " where ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
                Mi_SQL = Mi_SQL + " and  ( (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " = 'SI'";
                Mi_SQL = Mi_SQL + " or  PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " = 'SI') and ";
                Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Registrado + " = 'SI'))";

                Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES";
                Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " = REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
                
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
                Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
                Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;

                Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
                Mi_SQL = Mi_SQL + " ON PROVEEDORES." + Cat_Com_Proveedores.Campo_Proveedor_ID + " =";
                Mi_SQL = Mi_SQL + " (select " + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " from ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " where ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
                Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
                Mi_SQL = Mi_SQL + " and  ( (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " = 'SI'";
                Mi_SQL = Mi_SQL + " or  PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " = 'SI') and ";
                Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Registrado + " = 'SI'))";

                Mi_SQL = Mi_SQL + " WHERE ((PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " =  'SI'";
                Mi_SQL = Mi_SQL + " OR PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " = 'SI')  and ";
                Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Registrado + " = 'SI')";

                Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " <> 'CERRADA' ";

                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + "AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra.Trim() + "%'";
                }

                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + "AND REQ_PRODUCTO." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " like '%" + Datos.P_No_Requisicion.Trim() + "%'";
                }

                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR( REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                  " AND '" + Datos.P_Fecha_Fin_B + "'";
                }

                Mi_SQL = Mi_SQL + " Order by REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;

                // Se guardan las ordenes de compra en la tabla "Dt_Ordenes_Compra"
                Dt_Ordenes_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                DataTable Dt_Ordenes_C_RESG = new DataTable();
                Dt_Ordenes_C_RESG = Dt_Ordenes_Compra.Clone();

                if (Dt_Ordenes_Compra.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt_Ordenes_Compra.Rows.Count; i++)
                    {
                        String No_Orden_Compra = Dt_Ordenes_Compra.Rows[i]["NO_ORDEN_COMPRA"].ToString().Trim();
                        Boolean Registrar_OC = Verificar_Orden_Compra(No_Orden_Compra); // Se verifica si la orden de compra contiene prosducto cuyo registro = unidad, totalidad, etc. 

                        if (Registrar_OC)// Si la orden de compra contiene productos que deben ser registrados
                        {
                            Registro = Dt_Ordenes_Compra.Select("NO_ORDEN_COMPRA='" + No_Orden_Compra.Trim() + "'");
                            DataRow Dr_Orden_Compra = Dt_Ordenes_C_RESG.NewRow();

                            if (Registro[0]["NO_REQUISICION"].ToString().Trim() != "")
                            Dr_Orden_Compra["NO_REQUISICION"] = Registro[0]["NO_REQUISICION"].ToString().Trim();

                            if (Registro[0]["NO_ORDEN_COMPRA"].ToString().Trim() != "")
                            Dr_Orden_Compra["NO_ORDEN_COMPRA"] = Registro[0]["NO_ORDEN_COMPRA"].ToString().Trim();

                            if (Registro[0]["UNIDAD_RESPONSABLE"].ToString().Trim() != "")
                            Dr_Orden_Compra["UNIDAD_RESPONSABLE"] = Registro[0]["UNIDAD_RESPONSABLE"].ToString().Trim();

                            if (Registro[0]["PROVEEDOR"].ToString().Trim() != "")
                            Dr_Orden_Compra["PROVEEDOR"] = Registro[0]["PROVEEDOR"].ToString().Trim();
                            
                            if (Registro[0]["FECHA"].ToString().Trim() !="")
                            Dr_Orden_Compra["FECHA"] = Registro[0]["FECHA"].ToString().Trim();

                            if (Registro[0]["ESTATUS"].ToString().Trim() != "")
                            Dr_Orden_Compra["ESTATUS"] = Registro[0]["ESTATUS"].ToString().Trim();

                            if (Registro[0]["TOTAL"].ToString().Trim() != "")
                            Dr_Orden_Compra["TOTAL"] = Registro[0]["TOTAL"].ToString().Trim();

                            if (Registro[0]["FOLIO_REQ"].ToString().Trim() != "")
                                Dr_Orden_Compra["FOLIO_REQ"] = Registro[0]["FOLIO_REQ"].ToString().Trim();

                            if (Registro[0]["FOLIO_OC"].ToString().Trim() != "")
                                Dr_Orden_Compra["FOLIO_OC"] = Registro[0]["FOLIO_OC"].ToString().Trim();

                            if (Registro[0]["NO_CONTRA_RECIBO"].ToString().Trim() != "")
                            Dr_Orden_Compra["NO_CONTRA_RECIBO"] = Registro[0]["NO_CONTRA_RECIBO"].ToString().Trim();

                            Int16 Longitud = Convert.ToInt16(Dt_Ordenes_Compra.Rows.Count);
                            if (Longitud == 0)
                                Dt_Ordenes_C_RESG.Rows.InsertAt(Dr_Orden_Compra, Longitud);
                            else
                                Dt_Ordenes_C_RESG.Rows.InsertAt(Dr_Orden_Compra, (Longitud + 1));
                        }
                    }
                }

                return Dt_Ordenes_C_RESG;
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
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Productos_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de la orden de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Productos_Requisicion(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos = new DataTable();

            Mi_SQL = "SELECT  " + "PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + ""; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + " AS NO_INVENTARIO";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Serie + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id+ " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Marca_Id + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Modelo + " ";  // Estos se agregaron nuevos
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Garantia + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Resguardado+ " ";
            Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " ";
            Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " ";
            Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " AS OPERACION ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Compra.Trim() + ") AS COSTO";

            Mi_SQL = Mi_SQL + ",'' AS DEPENDENCIA_ID ";
            Mi_SQL = Mi_SQL + ",'' AS AREA_ID ";
            Mi_SQL = Mi_SQL + ",'' AS EMPLEADO_ID ";
            Mi_SQL = Mi_SQL + ",'0' AS NO_REGISTRO ";
            Mi_SQL = Mi_SQL + ",( select DESCRIPCION from " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " where ";
            Mi_SQL = Mi_SQL + Cat_Pat_Colores.Campo_Color_ID + " = INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id;
            Mi_SQL = Mi_SQL + " )as COLOR";
            Mi_SQL = Mi_SQL + ",( select DESCRIPCION from " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " where ";
            Mi_SQL = Mi_SQL + Cat_Pat_Materiales.Campo_Material_ID + " = INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id;
            Mi_SQL = Mi_SQL + " )as MATERIAL";

            Mi_SQL = Mi_SQL + ",( select NOMBRE from " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID+ " = INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id;
            Mi_SQL = Mi_SQL + " )as PRODUCTO";
            Mi_SQL = Mi_SQL + ",( select DESCRIPCION from " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id;
            Mi_SQL = Mi_SQL + " )as DESCRIPCION";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PROD_CONTRARECIBO";
            Mi_SQL = Mi_SQL + ", " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " INV_B_MUEBLES";
            Mi_SQL = Mi_SQL + " WHERE PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + " ( select NO_CONTRA_RECIBO from " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " where ";
            Mi_SQL = Mi_SQL + " NO_ORDEN_COMPRA =" + Datos.P_No_Orden_Compra.Trim() + ") ";
            Mi_SQL = Mi_SQL + " and ( (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " ='SI' ";
            Mi_SQL = Mi_SQL + " OR PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " = 'SI') and " ;
            Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Registrado + " = 'SI')";
            Mi_SQL = Mi_SQL + " and ( INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + "  ";
            Mi_SQL = Mi_SQL + " and  INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " )";
            //Mi_SQL = Mi_SQL + " and  INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Resguardado+ " IS NULL) ";

            Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            return Dt_Productos;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Productos_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de la orden de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Dts_Generarles_Producto_Requisicion(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos = new DataTable();

            //Mi_SQL = "SELECT  " + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Producto_ID + ""; //NO_REQUISICION
            //Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Monto_Total + " AS NO_INVENTARIO";
            //Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_OB+ " ";
            //Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Color_Id + " ";
            //Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Material_Id + " ";
            //Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " ";
            //Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " ";
            //Mi_SQL = Mi_SQL + ", PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " AS OPERACION ";

            // COSTO Y FACTURA

            Dt_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            return Dt_Productos;

        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Productos_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de la orden de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Resguardar_Producto(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
          
            //Mi_SQL = " UPDATE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
            //Mi_SQL = Mi_SQL + " SET " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Resguardado+ " ='SI'";
            //Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + " = " + Datos.P_No_Inventario.Trim();
            //Mi_SQL = Mi_SQL + " and " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = '" + Datos.P_Producto_ID.Trim() + "'";

            //Ejecutar consulta
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
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
            DataTable Dt_OC = new DataTable();

            // Asignar consulta
            Mi_SQL = "SELECT ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio + " ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + "  where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + "" + No_Orden_Compra.Trim() + "";

            Dt_OC = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            String Recibo_Transitorio = Dt_OC.Rows[0]["RECIBO_TRANSITORIO"].ToString().Trim();
            
            if (Recibo_Transitorio == "SI")
            {
                Registrar = true;
                return Registrar;
            }
            else
            {
                Registrar = false;
                return Registrar;
            }

            return Registrar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Llenar_Combo
        ///DESCRIPCIÓN:             Método utilizado para consultar las marcas y los modelos
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Llenar_Combo(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                if (Datos.P_Tipo_Combo.Equals("DEPENDENCIAS"))
                {
                    Mi_SQL = "SELECT DISTINCT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Clave + "||'-'|| " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Combo.Equals("AREAS"))
                {
                    Mi_SQL = "SELECT DISTINCT " + Cat_Areas.Campo_Area_ID + " AS AREA_ID, " + Cat_Areas.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Areas.Tabla_Cat_Areas + " ORDER BY " + Cat_Areas.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Combo.Equals("EMPLEADOS"))
                {
                    Mi_SQL = "SELECT " + Cat_Empleados.Campo_Empleado_ID + " AS EMPLEADO_ID, " + Cat_Empleados.Campo_Apellido_Paterno + " ||' '|| " + Cat_Empleados.Campo_Apellido_Materno;
                    Mi_SQL = Mi_SQL + " ||' '|| " + Cat_Empleados.Campo_Nombre + " AS NOMBRE FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Campo_Dependencia_ID + " = '" + Datos.P_Unidad_Responsable_ID + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Apellido_Paterno + ", " + Cat_Empleados.Campo_Apellido_Materno + ", " + Cat_Empleados.Campo_Nombre;
                }
                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Consulta != null)
                {
                    Dt_consulta = Ds_Consulta.Tables[0];
                }
                return Dt_consulta;
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Actualizar_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para actulizar a resguardada = "SI"
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Actualizar_Orden_Compra( Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            //Declaracion de variables
            String Mensaje = "";
            String Mi_SQL = String.Empty;
          
            try
            {
                // Se le asignan un SI al campo "RESGUARDADA" de la orden de compra correspondiente
                Mi_SQL = " UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ";
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Resguardada + " = 'SI'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + " ( Select " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " where ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno+ " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + " )";

                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                if (Ex.Code == 8152)
                {
                    Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                }
                else if (Ex.Code == 2627)
                {
                    if (Ex.Message.IndexOf("PRIMARY") != -1)
                    {
                        Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
                    Mensaje = "Error al intentar Modificar el Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                }
                //Indicamos el mensaje 
                throw new Exception(Mensaje);
            }
            finally
            {
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Alta_Resguardo
        ///DESCRIPCIÓN:             Método utilizado para consultar las marcas y los modelos
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernadnez Ramirez
        ///FECHA_MODIFICO:          23/Agosto/2011
        ///CAUSA_MODIFICACIÓN:      Se agrego la inserción del modelo, Marca_ID y Garantia
        ///*******************************************************************************
        public static String Alta_Resguardo_Recibo(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            Int64 Recibo_Resguardo_ID = 0;
            String Mensaje = "";
            String Mi_SQL = String.Empty;
            Object Aux; // Variable auxiliar para las consultas
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            OracleDataAdapter Obj_Adaptador;

            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Adaptador = new OracleDataAdapter();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

             try
                {
                    String Existe = Existe_Bien_Mueble(Datos);
                    if (!String.IsNullOrEmpty(Existe)) { return Existe; }
                    Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Nombre + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Producto_ID + "'";
                    Object Obj_Nombre_Producto = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    String Nombre_Producto = "";
                    if (!String.IsNullOrEmpty(Obj_Nombre_Producto.ToString())) {
                        Nombre_Producto = Obj_Nombre_Producto.ToString().Trim();
                    }

                    // Asignar consulta para el Maximo ID tabla OPE_PAT_BIENES_MUEBLES
                    String Bien_Mueble_ID = Obtener_ID_Consecutivo(Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles, Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID, 10);

                    // Consulta para dar de alta la salida
                    Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " (";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Producto_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID+ ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Area_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Material_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Marca_ID+ ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Modelo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Garantia + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Color_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Factura + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Costo_Alta + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Procedencia + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Operacion + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Cantidad + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Fecha_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Nombre + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_No_Inventario_Anterior + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Estado + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Muebles.Campo_Estatus + " )";                                     

                    Mi_SQL = Mi_SQL + " VALUES('" + Bien_Mueble_ID + "', ";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Producto_ID + "', '";
                    Mi_SQL = Mi_SQL + "" + Datos.P_Proveedor_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Unidad_Responsable_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Area_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Material_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Marca_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Modelo+ "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Garantia + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Color_ID + "', ";
                    Mi_SQL = Mi_SQL + Datos.P_No_Inventario + ", '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Factura + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Serie + "', ";
                    Mi_SQL = Mi_SQL + Datos.P_Costo + ", ";
                    Mi_SQL = Mi_SQL + Datos.P_Costo + ", '";
                    Mi_SQL = Mi_SQL + "" + "', '";
                    Datos.P_Fecha_Inventario = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Inventario));
                    Mi_SQL = Mi_SQL + Datos.P_Fecha_Inventario + "', '";
                    Datos.P_Fecha_Adquisicion = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Adquisicion));
                    Mi_SQL = Mi_SQL + Datos.P_Fecha_Adquisicion + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Operacion + "'";
                    Mi_SQL = Mi_SQL + ", 1 ";
                    Mi_SQL = Mi_SQL + ", SYSDATE,'";
                    Mi_SQL = Mi_SQL + Nombre_Producto + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_No_Inventario + "', '";
                    Mi_SQL = Mi_SQL + "BUENO', ";
                    Mi_SQL = Mi_SQL + "'VIGENTE')";

                    ////Ejecutar consulta
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Ejecutar consulta
                    //Obj_Comando.CommandText = Mi_SQL;
                    //Obj_Comando.ExecuteNonQuery();

                    // Optener el Maximo Identificador
                    if (Datos.P_Operacion == "RESGUARDO") //Asignar consulta para el Maximo ID
                    {
                        Mi_SQL = "SELECT NVL(MAX(" + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + "), 0) FROM " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos;
                    }
                    else if (Datos.P_Operacion == "RECIBO") //Asignar consulta para el Maximo ID
                    {
                        Mi_SQL = "SELECT NVL(MAX(" + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + "), 0) FROM " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos;
                    }

                    //Ejecutar consulta
                    Obj_Comando.CommandText = Mi_SQL;
                    Aux = Obj_Comando.ExecuteScalar();

                    //Verificar si no es nulo
                    if (Convert.IsDBNull(Aux) == false)
                        Recibo_Resguardo_ID = Convert.ToInt64(Aux) + 1;
                    else
                        Recibo_Resguardo_ID = 1;

                    if (Datos.P_Operacion == "RESGUARDO")
                    {
                        // Consulta para dar de alta el resguardo 
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " (";
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Bien_Resguardo_ID + ", ";
                    }
                    else if (Datos.P_Operacion == "RECIBO")
                    {
                        // Consulta para dar de alta el recibo
                        Mi_SQL = "INSERT INTO " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + " (";
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Campo_Bien_Recibo_ID + ", ";
                    }

                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Tipo + ", ";

                    if (Datos.P_Operacion == "RESGUARDO")
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + ", ";
                    else if (Datos.P_Operacion == "RECIBO")
                        Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + ", ";

                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Usuario_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Creo + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Fecha_Inicial + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Estatus + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Estado + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Dependencia_ID + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Movimiento_Alta + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Observaciones + ", ";
                    Mi_SQL = Mi_SQL + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Almacen_ID + " )";
                    Mi_SQL = Mi_SQL + " VALUES(" + Recibo_Resguardo_ID + ", ";
                    Mi_SQL = Mi_SQL + "'" + Bien_Mueble_ID + "', '";
                    Mi_SQL = Mi_SQL + "BIEN_MUEBLE', '";
                    Mi_SQL = Mi_SQL + Datos.P_Responsable_ID + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', ";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_Usuario_Creo + "', SYSDATE,";
                    Mi_SQL = Mi_SQL + " SYSDATE, ";
                    Mi_SQL = Mi_SQL + "'VIGENTE', '";
                    Mi_SQL = Mi_SQL + "BUENO', '";
                    Mi_SQL = Mi_SQL + Datos.P_Unidad_Responsable_ID + "', '";
                    Mi_SQL = Mi_SQL + "SI', '";
                    Mi_SQL = Mi_SQL + Datos.P_Observaciones + "', '";
                    Mi_SQL = Mi_SQL + Datos.P_Empleado_Almacen_ID + "')";

                    //Ejecutar consulta
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Ejecutar consulta
                    //Obj_Comando.CommandText = Mi_SQL;
                    //Obj_Comando.ExecuteNonQuery();

                 //AQUI SE PONE RESGUARDADO SI EL BIEN QUE SE ENTREGA O RESGUARDA

                    Mi_SQL = " UPDATE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles;
                    Mi_SQL = Mi_SQL + " SET " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Resguardado + " ='SI'";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + " = " + Datos.P_No_Inventario.Trim();
                    Mi_SQL = Mi_SQL + " and " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = '" + Datos.P_Producto_ID.Trim() + "'";
                    Mi_SQL = Mi_SQL + " and " + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Operacion + " = '" + Datos.P_Operacion.Trim() + "'";

                    //Ejecutar consulta
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    ////Ejecutar consulta
                    //Obj_Comando.CommandText = Mi_SQL;
                    //Obj_Comando.ExecuteNonQuery();

                    //Ejecutar transaccion
                    //Obj_Transaccion.Commit();
                    Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("SURTIDA / RESGUARDO-RECIBO", Datos.P_No_Requisicion);
                    return Bien_Mueble_ID; // Se retorna el No. Resguardo
                 }
                 catch (OracleException Ex)
                 {
                 //configuracion del mensaje de acuerdo al numero de error devuelto por la MRDB 
                 if (Ex.Code == 8152)
                 {
                     Mensaje = "Existen datos demasiados extensos, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
                 }
                 else if (Ex.Code == 2627)
                 {
                     if (Ex.Message.IndexOf("PRIMARY") != -1)
                     {
                         Mensaje = "Error por intentar grabar valores duplicados en campos clave, corrija el problema y vuelva a intentar. Error: [" + Ex.Message + "]";
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
                     Mensaje = "Error al intentar Modificar el Bien Mueble. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                 }
                 //Indicamos el mensaje 
                 throw new Exception(Mensaje);
             }
             finally
             {
             }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_ID_Consecutivo
        ///DESCRIPCIÓN: Obtiene el ID Cosnecutivo disponible para dar de alta un Registro en la Tabla
        ///PARÁMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        public static String Obtener_ID_Consecutivo(String Tabla, String Campo, Int32 Longitud_ID)
        {
            String Id = Convertir_A_Formato_ID(1, Longitud_ID); 
            try
            {
                String Mi_SQL = "SELECT MAX(" + Campo + ") FROM " + Tabla;
                Object Obj_Temp = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (!(Obj_Temp is Nullable) && !Obj_Temp.ToString().Equals(""))
                {
                    Id = Convertir_A_Formato_ID((Convert.ToInt32(Obj_Temp) + 1), Longitud_ID);
                }
            }
            catch (OracleException Ex)
            {
                new Exception(Ex.Message);
            }
            return Id;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Alta_Recibo
        ///DESCRIPCIÓN:             Método utilizado para consultar las marcas y los modelos
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernández Ramírez
        ///FECHA_MODIFICO:          23/Agosto/2011
        ///CAUSA_MODIFICACIÓN:      Se realiza la consulta del Modelo, Marca_ID y Garantia
        ///*******************************************************************************
        public static DataTable Consulta_Recibos_Resguardos(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Resultado= new DataTable();

            Mi_SQL = "SELECT distinct INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " AS NUMERO_INVENTARIO "; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID + "";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID + "";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Area_ID + "";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Modelo + "";  // Nuevo Campo
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Garantia + ""; // Nuevo Campo
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Factura + "";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Numero_Serie + "";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS COSTO_UNITARIO ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Costo_Actual + " AS COSTO_TOTAL ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Estatus + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Inventario + " AS FECHA_INVENTARIO ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Fecha_Adquisicion + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Usuario_Creo + " ";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Operacion + " ";
            Mi_SQL = Mi_SQL + ",'B' AS ESTADO";
            Mi_SQL = Mi_SQL + ", INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Cantidad + " ";
            Mi_SQL = Mi_SQL + ",( select nombre from " + Cat_Dependencias.Tabla_Cat_Dependencias + " where ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " )as DEPENDENCIA";  
            Mi_SQL = Mi_SQL + ",( select nombre from " + Cat_Areas.Tabla_Cat_Areas + " where ";
            Mi_SQL = Mi_SQL + Cat_Areas.Campo_Area_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Area_ID;
            Mi_SQL = Mi_SQL + " )as AREA";
            Mi_SQL = Mi_SQL + ",( select NOMBRE from " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + " )as PRODUCTO";
            Mi_SQL = Mi_SQL + ",( select DESCRIPCION from " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + " )as PROCEDENCIA";

            Mi_SQL = Mi_SQL + ",( select COMPANIA from " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Proveedor_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " )as PROVEEDOR";

            Mi_SQL = Mi_SQL + ",( select nombre from " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Marca_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Marca_ID;
            Mi_SQL = Mi_SQL + " )as MARCA";
            Mi_SQL = Mi_SQL + ",( select nombre from " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " where ";
            Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Modelo_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Modelo_ID;
            Mi_SQL = Mi_SQL + " )as MODELO";
            Mi_SQL = Mi_SQL + ",( select DESCRIPCION from " + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " where ";
            Mi_SQL = Mi_SQL + Cat_Pat_Colores.Campo_Color_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Color_ID;
            Mi_SQL = Mi_SQL + " )as COLOR";
            Mi_SQL = Mi_SQL + ",( select DESCRIPCION from " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " where ";
            Mi_SQL = Mi_SQL + Cat_Pat_Materiales.Campo_Material_ID + " = INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Material_ID;
            Mi_SQL = Mi_SQL + " )as MATERIAL";
             
             if (Datos.P_Operacion == "RESGUARDO")
             {
                 Mi_SQL = Mi_SQL + ", ( select EMPLEADOS." + Cat_Empleados.Campo_Nombre + "";
                 Mi_SQL = Mi_SQL + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + "";
                 Mi_SQL = Mi_SQL + " ||' '||EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno;
                 Mi_SQL = Mi_SQL + "  FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
                 Mi_SQL = Mi_SQL + "  WHERE EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
                 Mi_SQL = Mi_SQL + "  RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " ";
                 Mi_SQL = Mi_SQL + " ) AS RESGUARDANTES ";

                 Mi_SQL = Mi_SQL + ", RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Comentarios + " as OBSERVACIONES ";
                 
                 Mi_SQL = Mi_SQL + ", ( select EMPLEADOS." + Cat_Empleados.Campo_RFC + "";
                 Mi_SQL = Mi_SQL + "  FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
                 Mi_SQL = Mi_SQL + "  WHERE EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
                 Mi_SQL = Mi_SQL + "  RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Empleado_Resguardo_ID + " ";
                 Mi_SQL = Mi_SQL + " ) AS RFC ";

                 Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " INV_B_MUEBLES ";
                 Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Resguardos.Tabla_Ope_Pat_Bienes_Resguardos + " RESGUARDOS ";
                 Mi_SQL = Mi_SQL + " WHERE RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Bien_ID + " = ";
                 Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " ";
                 Mi_SQL = Mi_SQL + " AND INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = ";
                 Mi_SQL = Mi_SQL + "" + Datos.P_No_Inventario.Trim();
                 Mi_SQL = Mi_SQL + " AND RESGUARDOS." + Ope_Pat_Bienes_Resguardos.Campo_Tipo + " = 'BIEN_MUEBLE' ";
            }
             else if (Datos.P_Operacion == "RECIBO")
            {
                Mi_SQL = Mi_SQL + ", ( select EMPLEADOS." + Cat_Empleados.Campo_Nombre + "";
                Mi_SQL = Mi_SQL + " ||' '|| EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + "";
                Mi_SQL = Mi_SQL + " ||' '||EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno;
                Mi_SQL = Mi_SQL + "  FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
                Mi_SQL = Mi_SQL + "  WHERE EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
                Mi_SQL = Mi_SQL + "  RECIBOS." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " ";
                Mi_SQL = Mi_SQL + " ) AS RESGUARDANTES ";

                Mi_SQL = Mi_SQL + ", RECIBOS." + Ope_Pat_Bienes_Recibos.Campo_Comentarios + " as OBSERVACIONES ";

                Mi_SQL = Mi_SQL + ", ( select EMPLEADOS." + Cat_Empleados.Campo_RFC+ "";
                Mi_SQL = Mi_SQL + "  FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
                Mi_SQL = Mi_SQL + "  WHERE EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
                Mi_SQL = Mi_SQL + "  RECIBOS." + Ope_Pat_Bienes_Recibos.Campo_Empleado_Recibo_ID + " ";
                Mi_SQL = Mi_SQL + " ) AS RFC ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " INV_B_MUEBLES ";
                Mi_SQL = Mi_SQL + ", " + Ope_Pat_Bienes_Recibos.Tabla_Ope_Pat_Bienes_Recibos + " RECIBOS ";
                Mi_SQL = Mi_SQL + " WHERE RECIBOS." + Ope_Pat_Bienes_Recibos.Campo_Bien_ID + " = ";
                Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " ";
                Mi_SQL = Mi_SQL + " AND INV_B_MUEBLES." + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Inventario.Trim();
                Mi_SQL = Mi_SQL + " AND RECIBOS." + Ope_Pat_Bienes_Recibos.Campo_Tipo + " = 'BIEN_MUEBLE' ";
            }
 
            Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Resultado;
        }

        public static String Consulta_Datos_Reimpresion_Resguardo_Recibo(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos)
        {
            String Bien_Mueble_ID_Rpt = "";
            String Mi_SQL = "";
            Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID + " FROM " +
            Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + " WHERE " +
            Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Datos.P_No_Inventario + "'" +
            " AND " +Ope_Pat_Bienes_Muebles.Campo_Operacion + " = '" + Datos.P_Operacion + "'";
            Object Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Bien_Mueble_ID_Rpt = Convert.ToString(Obj);
            return Bien_Mueble_ID_Rpt;
        }

        public static String Existe_Bien_Mueble(Cls_Ope_Alm_Resguardos_Recibos_Negocio Datos) {
            DataSet Ds_Datos = null;
            DataTable Dt_Datos = null;
            String Bien_Mueble_ID = String.Empty;
            try {
                String Mi_SQL = "SELECT * FROM " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles
                                                 + " WHERE " + Ope_Pat_Bienes_Muebles.Campo_Numero_Inventario + " = '" + Datos.P_No_Inventario + "'"
                                                 + " AND " + Ope_Pat_Bienes_Muebles.Campo_Operacion + " = '" + Datos.P_Operacion + "'";
                Ds_Datos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Datos != null) {
                    if (Ds_Datos.Tables.Count > 0) {
                        Dt_Datos = Ds_Datos.Tables[0];
                        if (Dt_Datos != null) {
                            if (Dt_Datos.Rows.Count > 0) {
                                Bien_Mueble_ID = Dt_Datos.Rows[0][Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID].ToString().Trim();
                            }
                        }
                    }
                }

            } catch (OracleException Ex) {
                throw new Exception(Ex.Message);
            } catch (Exception Ex) {
                throw new Exception(Ex.Message);
            }
            return Bien_Mueble_ID;
        }

    }
}