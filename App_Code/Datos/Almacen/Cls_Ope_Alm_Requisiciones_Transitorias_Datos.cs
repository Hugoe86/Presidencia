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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Requisiciones_Transitorias.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Generar_Requisicion.Negocio;

namespace Presidencia.Requisiciones_Transitorias.Datos
{

public class Cls_Ope_Alm_Requisiciones_Transitorias_Datos
{	
		///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para consultar las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/junio/2011 
        ///MODIFICO:             Jesus Toledo
        ///FECHA_MODIFICO:       12 JULIO 2011
        ///CAUSA_MODIFICACIÓN:   consulta de requsiciones transitorias
        ///*******************************************************************************
        public static DataTable Consulta_Requisiciones(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;  
            DataTable Dt_Requisiciones = new DataTable();

            Mi_SQL = "SELECT distinct REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ""; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " AS FOLIO_REQ";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " AS FECHA";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total_Cotizado + " AS MONTO_TOTAL";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + "";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + "";

            Mi_SQL = Mi_SQL + ", (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra+ " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " WHERE ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " = ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra+ ") as FOLIO_OC ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES";
            Mi_SQL = Mi_SQL + " LEFT JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID ;

            Mi_SQL = Mi_SQL + " WHERE  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "";
            Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Tipo + " = 'TRANSITORIA'";

            Mi_SQL = Mi_SQL + " AND (REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Resguardado + " = 'SI'"; // Con un producto que se resguarde o se le indique que las orden de compra tiene que estar resgaurdada o 
            Mi_SQL = Mi_SQL + " OR REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Resguardado + " = 'NO')"; // Si la requisición tiene

            Mi_SQL = Mi_SQL + " AND ( REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " = 'SURTIDA'"; // Nota en esta parte debe ir Surtida
            Mi_SQL = Mi_SQL + " OR REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + " = 'COMPRA'";
            Mi_SQL = Mi_SQL + " OR  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Estatus + "= '" + "PARCIAL' )";

            if (Datos.P_No_Requisicion != null)
            {
                Mi_SQL = Mi_SQL + "AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " like '%" + Datos.P_No_Requisicion + "%'";
            }
            if (Datos.P_No_Orden_Compra != null)
            {
                Mi_SQL = Mi_SQL + "AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
            }
            if ((Datos.P_Fecha_Inicial != null) && (Datos.P_Fecha_Final != null))
            {
                Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicial + "'" +
                " AND '" + Datos.P_Fecha_Final + "'";
            }
            Mi_SQL = Mi_SQL + " order by REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;

            Dt_Requisiciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            DataTable Dt_Salidas_Req = new DataTable(); // Se crea una nueva tabla que contendra las requisiciones que se deben de consultar
            Dt_Salidas_Req = Dt_Requisiciones.Clone();
            DataRow[] Registro;
            // Se filtra para ver si esta orden de compra debe ser mostrada en el grid para elaborar la Orden de salida
            if (Dt_Requisiciones.Rows.Count > 0)
            {
                for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
                {
                    String No_Orden_Compra = Dt_Requisiciones.Rows[i]["NO_ORDEN_COMPRA"].ToString().Trim(); // Se asigna la Orden de Compra
                    String No_Requisicion = Dt_Requisiciones.Rows[i]["NO_REQUISICION"].ToString().Trim();   // Se asigna el numero de Requisicion

                    Boolean Registrar_OC = Verificar_Orden_Compra(No_Orden_Compra); // Se verifica si la orden de compra contiene prosducto cuyo registro = unidad, totalidad, etc. 

                    if (Registrar_OC)// Si la orden de compra contiene productos que deben ser registrados
                    {
                        Registro = Dt_Requisiciones.Select("NO_REQUISICION='" + No_Requisicion.Trim() + "'");
                        DataRow Dr_Req = Dt_Salidas_Req.NewRow();

                        if (Registro[0]["NO_REQUISICION"].ToString().Trim() != "")
                            Dr_Req["NO_REQUISICION"] = Registro[0]["NO_REQUISICION"].ToString().Trim();

                        if (Registro[0]["UNIDAD_RESPONSABLE"].ToString().Trim() != "")
                            Dr_Req["UNIDAD_RESPONSABLE"] = Registro[0]["UNIDAD_RESPONSABLE"].ToString().Trim();

                        if (Registro[0]["UNIDAD_RESPONSABLE_ID"].ToString().Trim() != "")
                            Dr_Req["UNIDAD_RESPONSABLE_ID"] = Registro[0]["UNIDAD_RESPONSABLE_ID"].ToString().Trim();

                        if (Registro[0]["FECHA"].ToString().Trim() != "")
                            Dr_Req["FECHA"] = Registro[0]["FECHA"].ToString().Trim();
                        else
                            Dr_Req["FECHA"] = DateTime.Now.ToString("dd/MMM/yyyy");

                        if (Registro[0]["MONTO_TOTAL"].ToString().Trim() != "")
                            Dr_Req["MONTO_TOTAL"] = Registro[0]["MONTO_TOTAL"].ToString().Trim();

                        if (Registro[0]["ESTATUS"].ToString().Trim() != "")
                            Dr_Req["ESTATUS"] = Registro[0]["ESTATUS"].ToString().Trim();

                        if (Registro[0]["NO_ORDEN_COMPRA"].ToString().Trim() != "")
                            Dr_Req["NO_ORDEN_COMPRA"] = Registro[0]["NO_ORDEN_COMPRA"].ToString().Trim();

                        if (Registro[0]["FOLIO_OC"].ToString().Trim() != "")
                            Dr_Req["FOLIO_OC"] = Registro[0]["FOLIO_OC"].ToString().Trim();

                        if (Registro[0]["FOLIO_REQ"].ToString().Trim() != "")
                            Dr_Req["FOLIO_REQ"] = Registro[0]["FOLIO_REQ"].ToString().Trim();

                        Int16 Longitud = Convert.ToInt16(Dt_Salidas_Req.Rows.Count);
                        if (Longitud == 0)
                            Dt_Salidas_Req.Rows.InsertAt(Dr_Req, Longitud);
                        else
                            Dt_Salidas_Req.Rows.InsertAt(Dr_Req, (Longitud + 1));
                    }
                }
            }
            Dt_Salidas_Req = Dt_Requisiciones;
            return Dt_Salidas_Req;
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
            DataTable Dt_Productos_Requisicion = new DataTable();

            // Asignar consulta
            Mi_SQL = " SELECT REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + ", ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ", ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Resguardado + "  as PRODUCTO_RESGUARDADO, ";
            Mi_SQL = Mi_SQL + " ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Resguardada + " as O_C_RESGUARDADA ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + " = " + No_Orden_Compra.Trim();
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + "  = ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;

            Dt_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; // Se ejecuta la Consulta

            if (Dt_Productos_Requisicion.Rows.Count > 0) // Si la tabla contiene productos
            {
                for (int j = 0; j < Dt_Productos_Requisicion.Rows.Count; j++) // Se realiza el recorrido de la tabla
                {
                    String Producto_Resguardado = Dt_Productos_Requisicion.Rows[j]["PRODUCTO_RESGUARDADO"].ToString().Trim();
                    String OC_Resguardada = Dt_Productos_Requisicion.Rows[j]["O_C_RESGUARDADA"].ToString().Trim();
                    if ((Producto_Resguardado == "NO" || Producto_Resguardado == "SI")) // Si el un producto no necesita resguardo
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else 
                    if ((Producto_Resguardado == "SI") & (OC_Resguardada == "SI")) // Si el producto necesita resgurdo y esta resguardado
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else
                        Registrar = false;
                }
            }
            else
                Registrar = false; 
            return Registrar;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Verificar_Producto
        /// DESCRIPCION:            Realiza una consulta para verificar si  el producto esta resgaurdado y la orden de compra completa a la 
        ///                         que pertenece el mismo, tambien esta resgaurdada ( con esto indica que todos los productos de la orden de compra estan resga)
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos.                    
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            02/Agosto/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Boolean Verificar_Producto(String No_Orden_Compra, String Producto_ID)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //variable apra las consultas
            Boolean Registrar = false;
            DataTable Dt_Productos_Req = new DataTable();

            // Asignar consulta
            Mi_SQL = " SELECT REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + ", ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + ", ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Resguardado + "  as PRODUCTO_RESGUARDADO, ";
            Mi_SQL = Mi_SQL + " ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Resguardada + " as O_C_RESGUARDADA ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + " = " + No_Orden_Compra.Trim();
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            Mi_SQL = Mi_SQL + " = '" + Producto_ID.Trim() + "'";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
            Mi_SQL = Mi_SQL + "  = ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;

            Dt_Productos_Req= OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; // Se ejecuta la Consulta

            if (Dt_Productos_Req.Rows.Count > 0) // Si la tabla contiene productos
            {
                for (int j = 0; j < Dt_Productos_Req.Rows.Count; j++) // Se realiza el recorrido de la tabla
                {
                    String Producto_Resguardado = Dt_Productos_Req.Rows[j]["PRODUCTO_RESGUARDADO"].ToString().Trim();
                    String OC_Resguardada = Dt_Productos_Req.Rows[j]["O_C_RESGUARDADA"].ToString().Trim();

                    if ((Producto_Resguardado == "NO")) // Si el producto no necesita resguardo
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else if ((Producto_Resguardado == "SI") & (OC_Resguardada == "SI")) // Si el producto necesita resgurdo y esta resguardado
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else
                        Registrar = false;
                }
            }
            else
                Registrar = false;

            return Registrar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para consultar los detalles de la requisicion
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Detalles_Requisicion(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Requisicion = new DataTable();

            Mi_SQL = "SELECT REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ""; //NO_REQUISICION
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " as UNIDAD_RESPONSABLE";
            Mi_SQL = Mi_SQL + ", DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " as UNIDAD_RESPONSABLE_ID";
            
            Mi_SQL = Mi_SQL + ",( SELECT PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " FROM " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + " PARTIDA ";
            Mi_SQL = Mi_SQL + " WHERE PARTIDA." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID + " = REQUISICIONES." + Ope_Com_Requisiciones.Campo_Partida_ID;
            Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "' ) as PARTIDA ";

            //Mi_SQL = Mi_SQL + ",( SELECT ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            //Mi_SQL = Mi_SQL + " WHERE ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " = REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
            //Mi_SQL = Mi_SQL + " AND REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "' ) as FOLIO_OC ";
            Mi_SQL = Mi_SQL + ",'OC-'||''||REQUISICIONES.NO_ORDEN_COMPRA as FOLIO_OC"; 

            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion+ "";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Justificacion_Compra + " as COMENTARIOS";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_IVA_Cotizado + " as MONTO_IVA";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Subtotal_Cotizado+ " as SUBTOTAL";
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Total_Cotizado + " AS MONTO_TOTAL";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " ON REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " = DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID;
            Mi_SQL = Mi_SQL + " WHERE  REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "= '" +  Datos.P_No_Requisicion+ "'";
            
            Dt_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Requisicion;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN:          Método utilizado el programa y la fuente de financiomiento
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           23/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Pragrama_Financiamiento(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            DataTable Dt_Consulta = new DataTable();
            String Mi_SQL = null;

            Mi_SQL = " SELECT DISTINCT" + " PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Descripcion + " as PROYECTO_PROGRAMA ";
            Mi_SQL = Mi_SQL + ", PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " ";
            //Mi_SQL = Mi_SQL + ", (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + " FROM ";
            //Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            //Mi_SQL = Mi_SQL + " WHERE ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " = ";
            //Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID;
            //Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + " = ";
            //Mi_SQL = Mi_SQL + " PROYECTOS_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            //Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + " = ";
            //Mi_SQL = Mi_SQL + " FINANCIAMIENTO. " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " ";
            //Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            //Mi_SQL = Mi_SQL + " '" + Datos.P_No_Requisicion + "' ) as NO_ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + ", FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " as NO_ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + ", FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion+ " as FINANCIAMIENTO ";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROYECTOS_PROGRAMAS ";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
            Mi_SQL = Mi_SQL + ", " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FINANCIAMIENTO ";
            Mi_SQL = Mi_SQL + " WHERE REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + " = ";
            Mi_SQL = Mi_SQL + " PROYECTOS_PROGRAMAS. " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " ";
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + " = ";
            Mi_SQL = Mi_SQL + " FINANCIAMIENTO. " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " ";
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO. " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " '" + Datos.P_No_Requisicion + "' ";

            Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Consulta;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos de 
        ///                      las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Requisicion(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Productos_Requisicion = new DataTable();

            Mi_SQL = " SELECT DISTINCT" + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as PRODUCTO_ID";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra+ "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Clave+ "";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " as NOMBRE_PRODUCTO";
            Mi_SQL = Mi_SQL + ", PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " as DESCRIPCION";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + " as CANTIDAD_SOLICITADA";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " ";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado+ " as PRECIO";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Porcentaje_IVA;
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Total_Cotizado + "";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Monto_IVA + "";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " AS SUBTOTAL";
            Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Partida_ID + "";
            Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = ( SELECT " + Cat_Com_Unidades.Campo_Unidad_ID + "  FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID ;
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = '" + Datos.P_No_Requisicion + "'"; 
            Mi_SQL = Mi_SQL + " )) AS UNIDAD ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS";
            Mi_SQL = Mi_SQL + " ON REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            Mi_SQL = Mi_SQL + " = PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID;

            Mi_SQL = Mi_SQL + " WHERE  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + "= '" + Datos.P_No_Requisicion + "'";
            Mi_SQL = Mi_SQL + " AND  ( REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Resguardado + " = 'SI'"; // Con un producto que se resguarde o se le indique que las orden de compra tiene que estar resgaurdada o 
            Mi_SQL = Mi_SQL + " OR  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Resguardado + " = 'NO' )"; // Si la requisición tiene
            
            Dt_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            DataTable Dt_Salidas_Productos = new DataTable(); // Se crea una nueva tabla que contendra los produtos de los cuales se puede elaborar la orden de salida
            Dt_Salidas_Productos = Dt_Productos_Requisicion.Clone();
            DataRow[] Registro; // Se crea el registro

            // Se filtra para ver si esta orden de compra debe ser mostrada en el grid para realziar la orden de salida
            if (Dt_Productos_Requisicion.Rows.Count > 0)
            {
                for (int i = 0; i < Dt_Productos_Requisicion.Rows.Count; i++)
                {
                    String No_Orden_Compra = Dt_Productos_Requisicion.Rows[i]["NO_ORDEN_COMPRA"].ToString().Trim(); // Se asigna la Orden de Compra
                    String Producto_ID = Dt_Productos_Requisicion.Rows[i]["PRODUCTO_ID"].ToString().Trim(); // Se asigna la Orden de Compra

                    Boolean Registrar_P = Verificar_Producto(No_Orden_Compra, Producto_ID); // Se verifica si se pude realizar una orden de salidad de este producto

                    if (Registrar_P)// Si se el producto esta resguardado y la orden de compra esta resguardada
                    {
                        Registro = Dt_Productos_Requisicion.Select("PRODUCTO_ID='" + Producto_ID.Trim() + "'");
                        DataRow Dr_Prod = Dt_Salidas_Productos.NewRow();

                        Dr_Prod["PRODUCTO_ID"] = Registro[0]["PRODUCTO_ID"].ToString().Trim();
                        Dr_Prod["CLAVE"] = Registro[0]["CLAVE"].ToString().Trim();
                        Dr_Prod["NOMBRE_PRODUCTO"] = Registro[0]["NOMBRE_PRODUCTO"].ToString().Trim();
                        Dr_Prod["DESCRIPCION"] = Registro[0]["DESCRIPCION"].ToString().Trim();

                        if (Registro[0]["CANTIDAD_SOLICITADA"].ToString().Trim() != "")
                        Dr_Prod["CANTIDAD_SOLICITADA"] = Registro[0]["CANTIDAD_SOLICITADA"].ToString().Trim();

                      if(Registro[0]["CANTIDAD_ENTREGADA"].ToString().Trim() != "")
                        Dr_Prod["CANTIDAD_ENTREGADA"] = Registro[0]["CANTIDAD_ENTREGADA"].ToString().Trim();
                      else
                        Dr_Prod["CANTIDAD_ENTREGADA"] = 0;

                      if (Registro[0]["PRECIO"].ToString().Trim() != "")
                        Dr_Prod["PRECIO"] = Registro[0]["PRECIO"].ToString().Trim();

                      if (Registro[0]["PORCENTAJE_IVA"].ToString().Trim() != "")
                        Dr_Prod["PORCENTAJE_IVA"] = Registro[0]["PORCENTAJE_IVA"].ToString().Trim();

                      if (Registro[0]["TOTAL_COTIZADO"].ToString().Trim() != "")
                        Dr_Prod["TOTAL_COTIZADO"] = Registro[0]["TOTAL_COTIZADO"].ToString().Trim();

                      if (Registro[0]["MONTO_IVA"].ToString().Trim() != "")
                        Dr_Prod["MONTO_IVA"] = Registro[0]["MONTO_IVA"].ToString().Trim();

                      if (Registro[0]["SUBTOTAL"].ToString().Trim() != "")
                        Dr_Prod["SUBTOTAL"] = Registro[0]["SUBTOTAL"].ToString().Trim();

                      if (Registro[0]["PARTIDA_ID"].ToString().Trim() != "")
                        Dr_Prod["PARTIDA_ID"] = Registro[0]["PARTIDA_ID"].ToString().Trim();

                      if (Registro[0]["UNIDAD"].ToString().Trim() != "")
                        Dr_Prod["UNIDAD"] = Registro[0]["UNIDAD"].ToString().Trim();

                        Int16 Longitud = Convert.ToInt16(Dt_Salidas_Productos.Rows.Count);
                        if (Longitud == 0)
                            Dt_Salidas_Productos.Rows.InsertAt(Dr_Prod, Longitud);
                        else
                            Dt_Salidas_Productos.Rows.InsertAt(Dr_Prod, (Longitud + 1));
                    }
                }
            }

            return Dt_Salidas_Productos;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Cantidad_Entregada
        ///DESCRIPCIÓN:          Método utilizado para consultar loas dependencias y las áreas
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Consultar_Cantidad_Entregada(string P_Producto_ID, string P_No_Requisicion)
        {
            // Declaración de variables
            String Mi_SQL = null;
            DataSet Ds_Productos_Requisicion = null;
            DataTable Dt_Productos_Requisicion = new DataTable();
            String Resultado;

            Mi_SQL = "SELECT " + " SUM(SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + ") as CANTIDAD_ENTREGADA";
            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES";
            Mi_SQL = Mi_SQL + " JOIN " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS";
            Mi_SQL = Mi_SQL + " ON SALIDAS." + Alm_Com_Salidas.Campo_No_Salida;
            Mi_SQL = Mi_SQL + " = SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida;

            Mi_SQL = Mi_SQL + " WHERE  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + "= '" + P_Producto_ID + "'";
            Mi_SQL = Mi_SQL + " AND  SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "= '" + P_No_Requisicion + "'";

            Dt_Productos_Requisicion = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            Resultado = Convert.ToString(Dt_Productos_Requisicion.Rows[0]["CANTIDAD_ENTREGADA"]);
            return Resultado;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:          Método utilizado para consultar loas dependencias y las áreas
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_DataTable(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            // Declaración de Variables
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                if (Datos.P_Tipo_Data_Table.Equals("DEPENDENCIAS"))
                {
                    Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + " AS DEPENDENCIA_ID, " + Cat_Dependencias.Campo_Nombre + " AS NOMBRE";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Dependencias.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Data_Table.Equals("EMPLEADOS_UR"))
                {
                    Mi_SQL = Mi_SQL + " SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "  as EMPLEADO, ";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " as EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + "=";
                    Mi_SQL = Mi_SQL + "" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID;
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                    Mi_SQL = Mi_SQL + "'" + Datos.P_No_Requisicion + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Data_Table.Equals("EMPLEADOS"))
                {
                    Mi_SQL = Mi_SQL + " SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + "  ||' '||";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + "  as EMPLEADO, ";
                    Mi_SQL = Mi_SQL + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + " as EMPLEADO_ID";
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados;
                    Mi_SQL = Mi_SQL + " WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado;
                    Mi_SQL = Mi_SQL + " = '" + Datos.P_No_Empleado + "'";
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre;
                }

                if (Mi_SQL != null && Mi_SQL.Trim().Length > 0)
                {
                    Ds_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                if (Ds_Consulta != null)
                {
                    Dt_consulta= Ds_Consulta.Tables[0];
                }
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar consultar los registros. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
            return Dt_consulta;
        }



        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Alta_orden_Salida
        /// DESCRIPCION:            Dar de alta la orden de salida de material
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos para al operacion
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            23/Junio/2010 

        ///*******************************************************************************/
        public static long Alta_Orden_Salida(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            // Declaracion de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion;
            OracleCommand Obj_Comando;
            String Mi_SQL = String.Empty;
            Object Aux; // Variable auxiliar para las consultas
            String Mensaje = String.Empty; //Variable para el mensaje de error
            DataTable Dt_Aux = new DataTable(); //Tabla auxiliar para las consultas
            OracleDataAdapter Obj_Adaptador; //Adapatador para el llenado de las tablas
            
            Double Monto_Comprometido = 0.0; // Variable para el monto comprometido
            Double Monto_Ejercido = 0.0;    // Variable para el monto ejercido

            String No_Asignacion = String.Empty; // Variable para el No de Asignacion            
            String Partida_ID = String.Empty; // Variable para el ID de la partida
            String Proyecto_Programa_ID = String.Empty; // Variable para el ID del programa o proyecto
            String Dependencia_ID = String.Empty; // Variable para el ID de la dependencia
            Double Monto_Total = 0.0; // Variable para el monto total de los detalles de la requisicion

            // Variables utilizadas para actualizar los productos
            Int64 Cantidad_Comprometida = 0; // Variable para la cantidad Comprometida
            Int64 Cantidad_Existente = 0; // Variable para la cantidad Existente
            String Tipo_Salida_ID = "";

            Double SubTotal_Prod_Req = 0.0;
            Double IVA_Prod_Req = 0.0;
            Double Total_Prod_Req = 0.0;
            Int64 Cantidad_Entregada = 0;
            Int64 Cantidad_A_Entregar = 0;

            try
            {
                Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
                Obj_Comando = new OracleCommand();
                Obj_Adaptador = new OracleDataAdapter();
                Obj_Conexion.Open();
                Obj_Transaccion = Obj_Conexion.BeginTransaction();
                Obj_Comando.Transaction = Obj_Transaccion;
                Obj_Comando.Connection = Obj_Conexion;

                //Asignar consulta para el Maximo ID
                Mi_SQL = "SELECT NVL(MAX(" + Alm_Com_Salidas.Campo_No_Salida + "), 0) FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas;

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Aux = Obj_Comando.ExecuteScalar();

                //Verificar si no es nulo
                if (Convert.IsDBNull(Aux) == false)
                    Datos.P_No_Orden_Salida= Convert.ToInt64(Aux) + 1;
                else
                    Datos.P_No_Orden_Salida = 1;

                // Consulta para los ID de la dependencia, area, etc
                Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " + Ope_Com_Requisiciones.Campo_Area_ID + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                //Ejecutar consulta
                Dt_Aux = new DataTable();
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Adaptador.SelectCommand = Obj_Comando;
                Obj_Adaptador.Fill(Dt_Aux);

                //Verificar si la consulta arrojo resultado
                if (Dt_Aux.Rows.Count > 0)
                {
                    Datos.P_Dependencia_ID = Dt_Aux.Rows[0][0].ToString().Trim(); // Colocar los valores en las variables
                    Datos.P_Area_ID = Dt_Aux.Rows[0][1].ToString().Trim();
                }
                else
                {
                    throw new Exception("Datos no encontrados requisicion No. " + Datos.P_No_Requisicion.ToString().Trim());
                }

            

                // For utilizado para calcular los montos de la requisición
                for (int j = 0; j < Datos.P_Dt_Productos_Requisicion.Rows.Count; j++) 
                {
                    SubTotal_Prod_Req = SubTotal_Prod_Req + Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[j]["SUBTOTAL"]);
                    IVA_Prod_Req = IVA_Prod_Req + Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[j]["MONTO_IVA"]);
                    Total_Prod_Req = Total_Prod_Req + Convert.ToDouble(Datos.P_Dt_Productos_Requisicion.Rows[j]["TOTAL"]);
                }

                // Consulta para dar de alta la salida
                Mi_SQL = "INSERT INTO " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " (" + Alm_Com_Salidas.Campo_No_Salida + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Dependencia_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ", " + Alm_Com_Salidas.Campo_Requisicion_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Fecha_Creo + ", " + Alm_Com_Salidas.Campo_Empleado_Almacen_ID + ", ";
                Mi_SQL = Mi_SQL + Alm_Com_Salidas.Campo_Subtotal + " , " + Alm_Com_Salidas.Campo_IVA + ", " + Alm_Com_Salidas.Campo_Total + ") ";
                Mi_SQL = Mi_SQL + " VALUES(" + Datos.P_No_Orden_Salida + ", ";
                Mi_SQL = Mi_SQL + "'" + Datos.P_Dependencia_ID + "', '" + Datos.P_Empleado_Recibio_ID + "', ";
                Mi_SQL = Mi_SQL + Datos.P_No_Requisicion.ToString().Trim() + ", ";
                //Mi_SQL = Mi_SQL + "'" + Datos.P_Nombre_Empleado_Almacen + "', SYSDATE, '" + Datos.P_Empleado_Almacen_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + Cls_Sessiones.Nombre_Empleado + "', SYSDATE, '" + Cls_Sessiones.Empleado_ID + "', ";
                Mi_SQL = Mi_SQL + SubTotal_Prod_Req + ", " + IVA_Prod_Req + ", " + Total_Prod_Req + " )";

                //String No_Salida = Convert.ToString(Datos.P_No_Orden_Salida);
                // Se registra  el Insert en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", No_Salida, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                // Consulta para la actualizacion del estatus de la requisicion 
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                Mi_SQL = Mi_SQL + "SET " + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Datos.P_Estatus.ToString().Trim() + "', ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " = SYSDATE, ";
                //Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Datos.P_Empleado_Almacen_ID + "' ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID + " = '" + Cls_Sessiones.Empleado_ID + "' ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                //String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);
                // Se registra  el update en la bitacora
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", No_Requisicion, Mi_SQL);

                //Ejecutar consulta
                Obj_Comando.CommandText = Mi_SQL;
                Obj_Comando.ExecuteNonQuery();

                // Se Guarda el Historial de la requisición
                Cls_Ope_Com_Requisiciones_Negocio Requisiciones = new Cls_Ope_Com_Requisiciones_Negocio();
                Requisiciones.Registrar_Historial(Datos.P_Estatus.ToString().Trim(), Datos.P_No_Requisicion.ToString().Trim());

                // Verificar si tiene datos la tabla enviada con las cantidades entregadas
                if (Datos.P_Dt_Productos_Requisicion.Rows.Count > 0)
                {
                    // Ciclo para el desplazamiento de la tabla
                    for (int Cont_Elementos = 0; Cont_Elementos < Datos.P_Dt_Productos_Requisicion.Rows.Count; Cont_Elementos++)
                    {
                        if (Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() == null || Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() == "") // Se realiza esta validación por que luego el precio es 0 para que no marque error
                            Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"] = 0;  

                        //Consulta para dar de alta los detalles de la salida
                        Mi_SQL = "INSERT INTO " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " (" + Alm_Com_Salidas_Detalles.Campo_No_Salida + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ", " + Alm_Com_Salidas_Detalles.Campo_Cantidad + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Costo + ", " + Alm_Com_Salidas_Detalles.Campo_Costo_Promedio + ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Subtotal + ", " + Alm_Com_Salidas_Detalles.Campo_IVA+ ", ";
                        Mi_SQL = Mi_SQL + Alm_Com_Salidas_Detalles.Campo_Importe + ") VALUES(" + Datos.P_No_Orden_Salida + ", ";
                        Mi_SQL = Mi_SQL + "'" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "', ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRECIO"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["SUBTOTAL"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["MONTO_IVA"].ToString().Trim() + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["TOTAL"].ToString().Trim() + ")";

                        //String N_Salida = Convert.ToString(Datos.P_No_Salida);
                        // Se registra  el Insert en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Alm_Com_Orden_Salida.aspx", N_Salida, Mi_SQL);

                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();



                // SE ACTUALIZA LA CANTIDAD ENTREGADA DE PRODUCTOS EN LA TABLA REQ_PRODUCTOS
                        Cantidad_Entregada = Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_ENTREGADA"].ToString().Trim());
                        Cantidad_A_Entregar = Convert.ToInt64(Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["CANTIDAD_A_ENTREGAR"].ToString().Trim());

                        Cantidad_Entregada = Cantidad_Entregada + Cantidad_A_Entregar; // Se suman las cantidades, lo que se va a entregar, con lo que se entrego

                        // Consulta para la actualizacion del estatus de la requisicion 
                        Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " ";
                        Mi_SQL = Mi_SQL + "SET " + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " = " + Cantidad_Entregada + " ";
                        Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = '" + Datos.P_Dt_Productos_Requisicion.Rows[Cont_Elementos]["PRODUCTO_ID"].ToString().Trim() + "' ";
                        Mi_SQL = Mi_SQL + " AND " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Datos.P_No_Requisicion.ToString().Trim() + " ";

                        //String No_Requisicion = Convert.ToString(Datos.P_No_Requisicion);
                        // Se registra  el update en la bitacora
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Alm_Com_Orden_Salida.aspx", No_Requisicion, Mi_SQL);

                        //Ejecutar consulta
                        Obj_Comando.CommandText = Mi_SQL;
                        Obj_Comando.ExecuteNonQuery();
                    }
                }
                //Ejecutar transaccion
                Obj_Transaccion.Commit();

                //Entregar resultado
                return Datos.P_No_Orden_Salida;
            }
            catch (OracleException Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                }
                switch (Ex.Code.ToString())
                {
                    case "2291":
                        Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
                        break;
                    case "923":
                        Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
                        break;
                    case "12170":
                        Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
                        break;
                    default:
                        Mensaje = "Error:  [" + Ex.Message + "]";
                        break;
                }
                throw new Exception(Mensaje, Ex);
            }
            finally
            {
                Obj_Comando = null;
                Obj_Conexion = null;
                Obj_Transaccion = null;
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Informacion_General_OS
        ///DESCRIPCIÓN:          Método donde se consulta la información general de la orden de salida que se genero
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Informacion_General_OS(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            //// Declaración de variables
            //String Mi_SQL = null;
            //DataTable Dt_Cabecera = new DataTable();

            //Mi_SQL = "SELECT " + "SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " as NO_ORDEN_SALIDA"; 
            //Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS."+ Cat_Dependencias.Campo_Nombre + " from ";
            //Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS "; 
            //Mi_SQL = Mi_SQL + " where SALIDAS."+ Alm_Com_Salidas.Campo_Dependencia_ID + " = DEPENDENCIAS." ;
            //Mi_SQL = Mi_SQL+ Cat_Dependencias.Campo_Dependencia_ID + ")as UNIDAD_RESPONSABLE";

            //Mi_SQL = Mi_SQL + ",(select distinct (FINANCIAMIENTO."+ Cat_SAP_Fuente_Financiamiento.Campo_Descripcion +")";
            //Mi_SQL = Mi_SQL + " from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FINANCIAMIENTO "; 
            //Mi_SQL = Mi_SQL + "  where FINANCIAMIENTO."+ Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            //Mi_SQL = Mi_SQL + " = (select distinct(REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ") from ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO "; 
            //Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." +  Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            //Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "))as F_FINANCIAMIENTO" ;
            
            //Mi_SQL = Mi_SQL + ",(select distinct (PROY_PROGRAMAS."+ Cat_Com_Proyectos_Programas.Campo_Descripcion + ")" ;
            //Mi_SQL = Mi_SQL + " from " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROY_PROGRAMAS "; 
            //Mi_SQL = Mi_SQL + "  where PROY_PROGRAMAS."+ Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            //Mi_SQL = Mi_SQL + " =(select distinct (REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ") from ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO "; 
            //Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." +  Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            //Mi_SQL = Mi_SQL + " SALIDAS." +Alm_Com_Salidas.Campo_Requisicion_ID + "))as PROGRAMA" ;

            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio;
            //Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
            //Mi_SQL = Mi_SQL + ", SALIDAS." + Alm_Com_Salidas.Campo_Usuario_Creo + " as ENTREGO ";
            
            //Mi_SQL = Mi_SQL + ", (select EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '||";
            //Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '||";
            //Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Nombre;
            //Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
            //Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            //Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ") as RECIBIO";
            
            //Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas+ " SALIDAS ";
            //Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones+ " REQUISICIONES ";
            //Mi_SQL = Mi_SQL + " where REQUISICIONES." +  Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            //Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID+ "";
            //Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_No_Salida+ " = ";
            //Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            //Dt_Cabecera = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //return Dt_Cabecera;

            // Declaración de variables

            String Mi_SQL = null;
            DataTable Dt_Cabecera = new DataTable();

            Mi_SQL = "SELECT " + "SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " as NO_ORDEN_SALIDA";
            Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS ";
            Mi_SQL = Mi_SQL + " where SALIDAS." + Alm_Com_Salidas.Campo_Dependencia_ID + " = DEPENDENCIAS.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + ")as UNIDAD_RESPONSABLE";

            Mi_SQL = Mi_SQL + ",(select distinct (FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + ")";
            Mi_SQL = Mi_SQL + " from " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FINANCIAMIENTO ";
            Mi_SQL = Mi_SQL + "  where FINANCIAMIENTO." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID;
            Mi_SQL = Mi_SQL + " = (select distinct(REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ") from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "))as F_FINANCIAMIENTO";

            Mi_SQL = Mi_SQL + ",(select distinct (PROY_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Descripcion + ")";
            Mi_SQL = Mi_SQL + " from " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROY_PROGRAMAS ";
            Mi_SQL = Mi_SQL + "  where PROY_PROGRAMAS." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID;
            Mi_SQL = Mi_SQL + " =(select distinct (REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ") from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "))as PROGRAMA";

            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio;
            Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion;
            Mi_SQL = Mi_SQL + ", SALIDAS." + Alm_Com_Salidas.Campo_Usuario_Creo + " as ENTREGO ";

            Mi_SQL = Mi_SQL + ", (select EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " ||' '||";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " ||' '||";
            Mi_SQL = Mi_SQL + " EMPLEADOS." + Cat_Empleados.Campo_Nombre;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS ";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Empleado_Solicito_ID + ") as RECIBIO";

            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " where REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + "";
            Mi_SQL = Mi_SQL + " AND SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            Dt_Cabecera = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Cabecera;
        }



        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Orden_Salida
        ///DESCRIPCIÓN:          Método donde se consultan los detalles de la orden de salida que se genero
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Detalles_Orden_Salida(Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio Datos)
        {
            //// Declaración de variables
            //String Mi_SQL = null;
            //DataTable Dt_Detalles = new DataTable();

            //Mi_SQL = "SELECT SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " as NO_ORDEN_SALIDA"; 
            //Mi_SQL = Mi_SQL + ",(select PRODUCTOS."+ Cat_Com_Productos.Campo_Clave+ " from ";
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS "; 
            //Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES."+ Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS." ;
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE";

            //Mi_SQL = Mi_SQL + ",(select PRODUCTOS."+ Cat_Com_Productos.Campo_Nombre+ " ||' '|| ";
            //Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion+ " from " ;
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS "; 
            //Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES."+ Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS." ;
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO";

            //Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS."+ Ope_Com_Req_Producto.Campo_Cantidad+ " from ";
            //Mi_SQL = Mi_SQL +  Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto+ " REQ_PRODUCTOS " ;
            //Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = "; 
            //Mi_SQL = Mi_SQL + " (select SALIDAS."+ Alm_Com_Salidas.Campo_Requisicion_ID+ " from ";
            //Mi_SQL = Mi_SQL +  Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS " ;
            //Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = "; 
            //Mi_SQL = Mi_SQL +  " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            //Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS."+ Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            //Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as CANTIDAD_SOLICITADA ";

            //Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + " as CANTIDAD_ENTREGADA"; 
            
            //Mi_SQL = Mi_SQL + ",(select UNIDADES."+ Cat_Com_Unidades.Campo_Abreviatura+ " from ";
            //Mi_SQL = Mi_SQL +  Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES " ;
            //Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID+ " = "; 
            //Mi_SQL = Mi_SQL + " (select PRODUCTOS."+ Cat_Com_Productos.Campo_Unidad_ID + " from ";
            //Mi_SQL = Mi_SQL +  Cat_Com_Productos.Tabla_Cat_Com_Productos+ " PRODUCTOS " ;
            //Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = ";
            //Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDADES";

            //Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Costo + " as PRECIO"; 
            //Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Subtotal + "";          
            //Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_IVA + "";
            //Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Importe + " as TOTAL"; 
          
            //Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES"; 
            //Mi_SQL = Mi_SQL + " WHERE SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            //Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            //Dt_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //return Dt_Detalles;

            // Declaración de variables
            String Mi_SQL = null;
            DataTable Dt_Detalles = new DataTable();

            Mi_SQL = "SELECT SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " as NO_ORDEN_SALIDA";
            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " ||' '|| ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " (select SALIDAS." + Alm_Com_Salidas.Campo_Requisicion_ID + " from ";
            Mi_SQL = Mi_SQL + Alm_Com_Salidas.Tabla_Alm_Com_Salidas + " SALIDAS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS." + Alm_Com_Salidas.Campo_No_Salida + ")";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + ") as CANTIDAD_SOLICITADA ";

            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Cantidad + " as CANTIDAD_ENTREGADA";

            Mi_SQL = Mi_SQL + ",(select UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDADES";

            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Costo + " as PRECIO";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Subtotal + "";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_IVA + "";
            Mi_SQL = Mi_SQL + ",SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_Importe + " as TOTAL";

            Mi_SQL = Mi_SQL + " FROM " + Alm_Com_Salidas_Detalles.Tabla_Alm_Com_Salidas_Detalles + " SALIDAS_DETALLES";
            Mi_SQL = Mi_SQL + " WHERE SALIDAS_DETALLES." + Alm_Com_Salidas_Detalles.Campo_No_Salida + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Orden_Salida;

            Dt_Detalles = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Detalles;
        }
	}                   
}
