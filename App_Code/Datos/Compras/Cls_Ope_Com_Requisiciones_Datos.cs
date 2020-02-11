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
using Presidencia.Generar_Requisicion.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Administrar_Requisiciones.Negocios;
using System.Data.OracleClient;
using System.Text;
using Presidencia.Stock;
using Presidencia.Manejo_Presupuesto_SAP.Datos;

namespace Presidencia.Generar_Requisicion.Datos
{
    public class Cls_Ope_Com_Requisiciones_Datos
    {
        public Cls_Ope_Com_Requisiciones_Datos()
        {

        }
        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region UTILERIAS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Consecutivo
        ///DESCRIPCIÓN: Obtiene el numero consecutivo para las tablas ocupadas en esta clase
        ///PARAMETROS: 1.-Campo del cual se obtendra el consecutivo
        ///            2.-Nombre de la tabla
        ///CREO: Gustavo Angeles Cruz
        ///FECHA_CREO: 10/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public static int Obtener_Consecutivo(String Campo_ID, String Tabla)
        {
            int Consecutivo = 0;
            String Mi_Sql;
            Object Obj; //Obtiene el ID con la cual se guardo los datos en la base de datos
            Mi_Sql = "SELECT NVL(MAX (" + Campo_ID + "),0) FROM " + Tabla;
            Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            Consecutivo = (Convert.ToInt32(Obj) + 1);
            return Consecutivo;
        }

        public static DataTable Consultar_Columnas_De_Tabla_BD(String Nombre_Tabla)
        {
            String Mi_Sql = "SELECT COLUMN_NAME AS COLUMNA FROM ALL_TAB_COLUMNS WHERE TABLE_NAME = '" + Nombre_Tabla + "'";
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql).Tables[0];
        }

        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************

        #region GUARDAR REQUISICION / INSERTAR REQUISICION, GUARDAR SUS DETALLES 
        public static String Guardar_Nueva_Requisicion(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mensaje = "";
            String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
            String Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
            String Mi_SQL = "";
 
            //INSERTAR LA REQUISICION   
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
                Requisicion_Negocio.P_Requisicion_ID = "" + Obtener_Consecutivo(Ope_Com_Requisiciones.Campo_Requisicion_ID, Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones);
                String Requisicion_Num = Requisicion_Negocio.P_Requisicion_ID + "";
                Requisicion_Negocio.P_Folio = "RQ-" + Requisicion_Negocio.P_Requisicion_ID;
                //Requisicion_Negocio.P_Codigo_Programatico = "";
                Mi_SQL = "SELECT CLAVE FROM CAT_SAP_AREA_FUNCIONAL WHERE AREA_FUNCIONAL_ID = " +
                    "(SELECT AREA_FUNCIONAL_ID FROM CAT_DEPENDENCIAS WHERE DEPENDENCIA_ID = '" + Requisicion_Negocio.P_Dependencia_ID + "')";
                DataSet Ds_Area_Funcional = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Ds_Area_Funcional != null && Ds_Area_Funcional.Tables[0].Rows.Count > 0)
                {
                    String Area = Ds_Area_Funcional.Tables[0].Rows[0]["CLAVE"].ToString();
                    Requisicion_Negocio.P_Codigo_Programatico = Requisicion_Negocio.P_Codigo_Programatico.Replace("1.1.1", Area);
                }
                //insertar cuando la requisicion quedo en estatus de contruccion
                Mi_SQL = "INSERT INTO " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " (" + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                ", " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ", " + Ope_Com_Requisiciones.Campo_Area_ID +
                ", " + Ope_Com_Requisiciones.Campo_Folio +
                ", " + Ope_Com_Requisiciones.Campo_Estatus +
                ", " + Ope_Com_Requisiciones.Campo_Codigo_Programatico +
                ", " + Ope_Com_Requisiciones.Campo_Elemento_PEP +
                ", " + Ope_Com_Requisiciones.Campo_Tipo +
                ", " + Ope_Com_Requisiciones.Campo_Fase +
                ", " + Ope_Com_Requisiciones.Campo_Subtotal +
                ", " + Ope_Com_Requisiciones.Campo_IVA +
                ", " + Ope_Com_Requisiciones.Campo_IEPS +
                ", " + Ope_Com_Requisiciones.Campo_Total +
                ", " + Ope_Com_Requisiciones.Campo_Usuario_Creo +
                ", " + Ope_Com_Requisiciones.Campo_Fecha_Creo +
                ", " + Ope_Com_Requisiciones.Campo_Justificacion_Compra +
                ", " + Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv +
                ", " + Ope_Com_Requisiciones.Campo_Verificaion_Entrega +
                ", " + Ope_Com_Requisiciones.Campo_Consolidada +
                ", " + Ope_Com_Requisiciones.Campo_Tipo_Articulo +
                ", " + Ope_Com_Requisiciones.Campo_Partida_ID +
                ", " + Ope_Com_Requisiciones.Campo_Especial_Ramo_33;
                if (Requisicion_Negocio.P_Estatus == "GENERADA")
                {
                    Mi_SQL = Mi_SQL +
                             ", " + Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID +
                             ", " + Ope_Com_Requisiciones.Campo_Fecha_Generacion;
                }
                else if (Requisicion_Negocio.P_Estatus == "EN CONSTRUCCION")
                {
                    Mi_SQL = Mi_SQL +
                             ", " + Ope_Com_Requisiciones.Campo_Empleado_Construccion_ID +
                             ", " + Ope_Com_Requisiciones.Campo_Fecha_Construccion;
                }
                Mi_SQL = Mi_SQL +
                ") VALUES (" +
                Requisicion_Negocio.P_Requisicion_ID + ",'" +
                Requisicion_Negocio.P_Dependencia_ID + "','" +
                    //Requisicion_Negocio.P_Area_ID + "','" +
                Cls_Sessiones.Area_ID_Empleado.Trim() + "','" +
                Requisicion_Negocio.P_Folio + "','" +
                Requisicion_Negocio.P_Estatus + "','" +
                Requisicion_Negocio.P_Codigo_Programatico + "','" +
                Requisicion_Negocio.P_Elemento_PEP + "','" +
                Requisicion_Negocio.P_Tipo + "','" +
                Requisicion_Negocio.P_Fase + "'," +
                Requisicion_Negocio.P_Subtotal + ", " +
                Requisicion_Negocio.P_IVA + ", " +
                Requisicion_Negocio.P_IEPS + ", " +
                Requisicion_Negocio.P_Total + ",'" +
                Usuario_Creo + "','" +
                Fecha_Creo + "','" +
                Requisicion_Negocio.P_Justificacion_Compra + "','" +
                Requisicion_Negocio.P_Especificacion_Productos + "','" +
                Requisicion_Negocio.P_Verificacion_Entrega + "','NO','" +
                Requisicion_Negocio.P_Tipo_Articulo + "','" +
                Requisicion_Negocio.P_Partida_ID + "','" +
                Requisicion_Negocio.P_Especial_Ramo33 + "','" +
                Cls_Sessiones.Empleado_ID + "','" +
                Fecha_Creo + "')";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //insertar los productos que se hayn seleccionado 
                //para la requisa validando q si hay productos agregados              
                int Consecutivo_Productos_Requisa =
                    Obtener_Consecutivo(Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID, Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                foreach (DataRow Renglon in Requisicion_Negocio.P_Dt_Productos_Servicios.Rows)
                {
                    Mi_SQL = "INSERT INTO " +
                        Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                        " (" + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Clave +
                        ", " + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                        ", " + Ope_Com_Req_Producto.Campo_Partida_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Cantidad +
                        ", " + Ope_Com_Req_Producto.Campo_Tipo +
                        ", " + Ope_Com_Req_Producto.Campo_Giro_ID +
                        ", " + Ope_Com_Req_Producto.Campo_Nombre_Giro +
                        ", " + Ope_Com_Req_Producto.Campo_Usuario_Creo +
                        ", " + Ope_Com_Req_Producto.Campo_Fecha_Creo +
                        ", " + Ope_Com_Req_Producto.Campo_Precio_Unitario +
                        ", " + Ope_Com_Req_Producto.Campo_Importe +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_IVA +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_IEPS +
                        ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IVA +
                        ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IEPS +
                        ", " + Ope_Com_Req_Producto.Campo_Monto_Total + " ) VALUES " +
                        "(" + Consecutivo_Productos_Requisa +
                        "," + Requisicion_Negocio.P_Requisicion_ID +
                        ",'" + Renglon["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() +
                        "','" + Renglon["Prod_Serv_ID"].ToString().Trim() +
                        "','" + Renglon["Clave"].ToString().Trim() +
                        "','" + Renglon["Nombre_Producto_Servicio"].ToString().Trim() +
                        "','" + Renglon["Partida_ID"].ToString().Trim() +
                        "','" + Renglon["Proyecto_Programa_ID"].ToString().Trim() +
                        "', " + Renglon["Cantidad"].ToString().Trim() +
                        //", '" + Requisicion_Negocio.P_Tipo_Articulo +
                        ", '" + Renglon["Tipo"].ToString().Trim() +
                        "','" + Renglon["Concepto_ID"].ToString().Trim() +
                        "','" + Renglon["Nombre_Concepto"].ToString().Trim() +
                        "','" + Cls_Sessiones.Nombre_Empleado +
                        "', " + "SYSDATE" +
                        ", " + Renglon["Precio_Unitario"].ToString().Trim() +
                        ", " + Renglon["Monto"].ToString().Trim() +
                        ", " + Renglon["Monto_IVA"].ToString().Trim() +
                        ", " + Renglon["Monto_IEPS"].ToString().Trim() +
                        ", " + Renglon["Porcentaje_IVA"].ToString().Trim() +
                        ", " + Renglon["Porcentaje_IEPS"].ToString().Trim() +
                        ", " + Renglon["Monto_Total"].ToString().Trim() + ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Consecutivo_Productos_Requisa++;
                }
                //Comprometer presupuesto partida
                double Total_Requisicion = 0;
                String Partida_ID = "";
                int Registros = 0;
                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Partida_ID))
                {
                    Partida_ID = Requisicion_Negocio.P_Partida_ID;
                    Total_Requisicion = double.Parse(Requisicion_Negocio.P_Total);
                    //Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                    //    Requisicion_Negocio.P_Dependencia_ID,
                    //    Requisicion_Negocio.P_Fuente_Financiamiento,
                    //    Requisicion_Negocio.P_Proyecto_Programa_ID,
                    //    Partida_ID,
                    //    DateTime.Now.Year,
                    //    Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                    //    Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                    //    Total_Requisicion);
                    //Realizar el movimiento presupuestal solicitado
                    Mi_SQL = "UPDATE " + Ope_Sap_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " SET " +
                    Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO + " = " + Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO + " + " + Total_Requisicion + ", " +
                    Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE + " = " + Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE + " - " + Total_Requisicion +
                    " WHERE " +
                    Ope_Sap_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Requisicion_Negocio.P_Dependencia_ID + "' AND " +
                    Ope_Sap_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " = '" + Requisicion_Negocio.P_Fuente_Financiamiento + "' AND " +
                    Ope_Sap_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Requisicion_Negocio.P_Proyecto_Programa_ID + "' AND " +
                    Ope_Sap_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_ID + "' AND " +
                    Ope_Sap_Dep_Presupuesto.Campo_Anio_Presupuesto + " = " + DateTime.Now.Year;
                    //Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Cmd.CommandText = Mi_SQL;
                    Registros =  Cmd.ExecuteNonQuery();
                    if (Registros == 0)
                        Mensaje = "FALLO";
                    else
                        Mensaje = "EXITO";
                }
                //Si la requisición es de STOCK se comprometen los productos
                if (Requisicion_Negocio.P_Tipo == "STOCK" && Mensaje == "EXITO")
                {
                    //inicializamos a cero
                    Registros =0;

                    foreach (DataRow Dr_Producto in Requisicion_Negocio.P_Dt_Productos_Servicios.Rows)
                    {
                        //SENTENCIA SQL PARA COMPROMETER
                        Mi_SQL = "UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                            " SET " +
                            Cat_Com_Productos.Campo_Comprometido + " = " + Cat_Com_Productos.Campo_Comprometido + " + " +
                            Dr_Producto["CANTIDAD"].ToString() + "," +
                            Cat_Com_Productos.Campo_Disponible + " = " + Cat_Com_Productos.Campo_Disponible + " - " +
                            Dr_Producto["CANTIDAD"].ToString() +
                            " WHERE " +
                            Cat_Com_Productos.Campo_Producto_ID + " = '" + Dr_Producto["PROD_SERV_ID"].ToString() + "'" +
                            " AND " + Cat_Com_Productos.Campo_Disponible + " >= " + Dr_Producto["CANTIDAD"].ToString();
                        Cmd.CommandText = Mi_SQL;
                        Registros = Cmd.ExecuteNonQuery();

                        if (Registros == 0)
                        {
                            //En caso de que algun producto no se halla afectado 
                            //rompemos el ciclo
                            Mensaje = "FALLO";
                            break;
                        }
                        else
                        {
                            Mensaje = "EXITO";
                        }                   
                    }
                    
                }
                //En caso de el mensaje sea EXITO ejecuta la sentencia 
                if (Mensaje == "EXITO")
                {
                    Trans.Commit();
                    Mensaje += "-" + Requisicion_Negocio.P_Requisicion_ID;
                    Registrar_Historial(Requisicion_Negocio.P_Estatus, Requisicion_Negocio.P_Requisicion_ID);
                }
                else//Si es fallo hacemos el rollback 
                {
                    Trans.Rollback();
                    Mensaje = "NO SE PUDO GUARDAR LA REQUISICION, Verifique Existencia Producto y Presupuesto";
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
                Trans.Rollback();
                Mensaje = "NO SE PUDO GUARDAR LA REQUISICION, CONSULTE A SU ADMINISTRADOR";

            }
            finally
            {
                Cn.Close();
            }
            return Mensaje;
        }


        public static String Proceso_Insertar_Requisicion(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            if (Cls_Sessiones.Nombre_Empleado != null && Cls_Sessiones.Area_ID_Empleado != null)
            {
                return Guardar_Nueva_Requisicion(Requisicion_Negocio);
            }
            else
            {
                return "No se guardó la requisición, reinicie sesión";
            }
        }


        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region MODIFICAR REQUISICION
       
        public static String Proceso_Actualizar_Requisicion(
            Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mensaje = "";
            String Listado = "NO";
            if (Requisicion_Negocio.P_Estatus == "CANCELADA PARCIAL" || Requisicion_Negocio.P_Estatus == "CANCELADA TOTAL")
            {
                Listado = "SI";
            }
            if (Requisicion_Negocio.P_Listado_Almacen != null &&
                Requisicion_Negocio.P_Listado_Almacen == "SI" &&
                Listado == "SI")
            {
                Mensaje = Cancelar_Requisición_De_Listado(Requisicion_Negocio.P_Requisicion_ID, Requisicion_Negocio.P_Comentarios, Requisicion_Negocio.P_Estatus);
            }
            else
            {
                if (Requisicion_Negocio.P_Estatus == "CANCELADA")
                    Mensaje = Cancelar_Requisicion_Stock_Transitoria(
                        Requisicion_Negocio.P_Requisicion_ID, 
                        Requisicion_Negocio.P_Tipo, 
                        Cls_Sessiones.Nombre_Empleado, 
                        Requisicion_Negocio.P_Comentarios);
                else
                    Mensaje = Actualizar_Requisicion_De_Unidad_Responsable(Requisicion_Negocio);
            }
            return Mensaje;
        }
        private static String Cancelar_Requisición_De_Listado(String No_Requisicion, String Comentarios, String Estatus) 
        {
            String Mi_SQL = "";
            String Mensaje = "EXITO";
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
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'CANCELADA' WHERE " +
                Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();                
                if (Estatus == "CANCELADA PARCIAL")
                {
                    Estatus = "AUTORIZADA";
                    Mi_SQL = "UPDATE " + Ope_Com_Listado.Tabla_Ope_Com_Listado + " SET " + Ope_Com_Listado.Campo_Estatus + " = '" + Estatus + "' WHERE " +
                        Ope_Com_Listado.Campo_Listado_ID + " = (SELECT DISTINCT(" + Ope_Com_Listado_Detalle.Campo_No_Listado_ID + ")" +
                        " FROM " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " WHERE " + Ope_Com_Listado_Detalle.Campo_No_Requisicion +
                        " = " + No_Requisicion + ")";
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                    Mi_SQL = "UPDATE " + Ope_Com_Listado_Detalle.Tabla_Ope_Com_Listado_Detalle + " SET " +
                        Ope_Com_Listado_Detalle.Campo_No_Requisicion + " = NULL WHERE " +
                        Ope_Com_Listado_Detalle.Campo_No_Requisicion + " = " + No_Requisicion;
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery();
                }
                //Guardar Comentarios
                if (Comentarios.Length > 0)
                {
                    Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                        new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                    Administrar_Requisicion.P_Requisicion_ID = No_Requisicion;
                    Administrar_Requisicion.P_Comentario = Comentarios;
                    Administrar_Requisicion.P_Estatus = "CANCELADA";
                    Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                    Administrar_Requisicion.Alta_Observaciones();
                }
                Trans.Commit();
                Registrar_Historial("CANCELADA", No_Requisicion);
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                Mensaje = ex.ToString();               
            }
            finally
            {
                Cn.Close();
            }
            return Mensaje;
        }


        public static String Actualizar_Requisicion_De_Unidad_Responsable(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {          
            String Mensaje = "";
            String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
            String Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
           
            String Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
            String Mi_SQL = "";
            //ACTUALIZAR LA REQUISICION
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
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " +
                Ope_Com_Requisiciones.Campo_Estatus + " = '" + Requisicion_Negocio.P_Estatus + "', " +
                Ope_Com_Requisiciones.Campo_Subtotal + " = " + Requisicion_Negocio.P_Subtotal + ", " +
                Ope_Com_Requisiciones.Campo_IVA + " = " + Requisicion_Negocio.P_IVA + ", " +
                Ope_Com_Requisiciones.Campo_IEPS + " = " + Requisicion_Negocio.P_IEPS + ", " +
                Ope_Com_Requisiciones.Campo_Total + " = " + Requisicion_Negocio.P_Total + ", " +
                Ope_Com_Requisiciones.Campo_Usuario_Modifico + " ='" + Usuario + "', " +
                Ope_Com_Requisiciones.Campo_Fecha_Modifico + " = SYSDATE, " +
                Ope_Com_Requisiciones.Campo_Justificacion_Compra + " ='" + Requisicion_Negocio.P_Justificacion_Compra + "'," +
                Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv + " ='" + Requisicion_Negocio.P_Especificacion_Productos + "'," +
                Ope_Com_Requisiciones.Campo_Verificaion_Entrega + "='" + Requisicion_Negocio.P_Verificacion_Entrega + "'";
                if (Requisicion_Negocio.P_Estatus == "GENERADA")
                {
                    Mi_SQL = Mi_SQL + "," +
                    Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID + "='" + Cls_Sessiones.Empleado_ID.ToString() + "'," +
                    Ope_Com_Requisiciones.Campo_Fecha_Generacion + "=SYSDATE";
                }

                Mi_SQL = Mi_SQL +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = '" + Requisicion_Negocio.P_Requisicion_ID + "'";
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Se borran los detalles, productos o servicios
                Mi_SQL = "DELETE FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                    " = " + Requisicion_Negocio.P_Requisicion_ID;
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                //Se guardan los detalles, productps o sevicios
                // Guarda_Productos_O_Servicios_Requisicion(Requisicion_Negocio);
                //Se guardan los detalles, productps o sevicios
                    int Consecutivo_Productos_Requisa =
                        Obtener_Consecutivo(Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID, Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                    foreach (DataRow Renglon in Requisicion_Negocio.P_Dt_Productos_Servicios.Rows)
                    {
                        Mi_SQL = "INSERT INTO " +
                            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                            " (" + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Clave +
                            ", " + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio +
                            ", " + Ope_Com_Req_Producto.Campo_Partida_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Cantidad +
                            ", " + Ope_Com_Req_Producto.Campo_Tipo +
                            ", " + Ope_Com_Req_Producto.Campo_Giro_ID +
                            ", " + Ope_Com_Req_Producto.Campo_Nombre_Giro +
                            ", " + Ope_Com_Req_Producto.Campo_Usuario_Creo +
                            ", " + Ope_Com_Req_Producto.Campo_Fecha_Creo +
                            ", " + Ope_Com_Req_Producto.Campo_Precio_Unitario +
                            ", " + Ope_Com_Req_Producto.Campo_Importe +
                            ", " + Ope_Com_Req_Producto.Campo_Monto_IVA +
                            ", " + Ope_Com_Req_Producto.Campo_Monto_IEPS +
                            ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IVA +
                            ", " + Ope_Com_Req_Producto.Campo_Porcentaje_IEPS +
                            ", " + Ope_Com_Req_Producto.Campo_Monto_Total + " ) VALUES " +
                            "(" + Consecutivo_Productos_Requisa +
                            "," + Requisicion_Negocio.P_Requisicion_ID +
                            ",'" + Renglon["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim() +
                            "','" + Renglon["Prod_Serv_ID"].ToString().Trim() +
                            "','" + Renglon["Clave"].ToString().Trim() +
                            "','" + Renglon["Nombre_Producto_Servicio"].ToString().Trim() +
                            "','" + Renglon["Partida_ID"].ToString().Trim() +
                            "','" + Renglon["Proyecto_Programa_ID"].ToString().Trim() +
                            "', " + Renglon["Cantidad"].ToString().Trim() +
                            //", '" + Requisicion_Negocio.P_Tipo_Articulo +
                            ", '" + Renglon["Tipo"].ToString().Trim() +
                            "','" + Renglon["Concepto_ID"].ToString().Trim() +
                            "','" + Renglon["Nombre_Concepto"].ToString().Trim() +
                            "','" + Cls_Sessiones.Nombre_Empleado +
                            "', " + "SYSDATE" +
                            ", " + Renglon["Precio_Unitario"].ToString().Trim() +
                            ", " + Renglon["Monto"].ToString().Trim() +
                            ", " + Renglon["Monto_IVA"].ToString().Trim() +
                            ", " + Renglon["Monto_IEPS"].ToString().Trim() +
                            ", " + Renglon["Porcentaje_IVA"].ToString().Trim() +
                            ", " + Renglon["Porcentaje_IEPS"].ToString().Trim() +
                            ", " + Renglon["Monto_Total"].ToString().Trim() + ")";
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery();
                        Consecutivo_Productos_Requisa++;
                }

                //Comprometer presupuesto partida
                double Total_Requisicion = 0;
                String Partida_ID = Requisicion_Negocio.P_Partida_ID;
                int Registros = 0;
                DataTable Dt_Codigo_Programatico = null;
                Total_Requisicion = double.Parse(Requisicion_Negocio.P_Total);
                //Consultar CODIGO PROGRAMATICO
                //Mi_SQL = "SELECT RQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " +
                //    "DETALLE." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ", " +
                //    "DETALLE." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ", " +
                //    "DETALLE." + Ope_Com_Req_Producto.Campo_Partida_ID +
                //    " FROM " +
                //    Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DETALLE, " +
                //    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " RQ " +
                //    " WHERE DETALLE." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                //Dt_Codigo_Programatico = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                //String Fuente = Dt_Codigo_Programatico.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString();
                //String Dependencia = Dt_Codigo_Programatico.Rows[0]["DEPENDENCIA_ID"].ToString();
                //String Programa = Dt_Codigo_Programatico.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString();
                //String Partida = Dt_Codigo_Programatico.Rows[0]["PARTIDA_ID"].ToString();
                Mi_SQL = "SELECT EXTRACT (YEAR FROM FECHA_CREO) FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Requisicion_Negocio.P_Requisicion_ID;
                Object Obj_Anio = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                int Anio = Convert.ToInt32(Obj_Anio);

                //Consultar TOTAL anterior de la requisicion para descomprometerlo
                Mi_SQL =
                "SELECT " + Ope_Com_Requisiciones.Campo_Total +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Requisicion_Negocio.P_Requisicion_ID;
                Object Obj_Total = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                double Total_Anterior = Convert.ToDouble(Obj_Total);

                if (Requisicion_Negocio.P_Tipo == "TRANSITORIA")
                {
                    //Descomprometer presupuesto anterior
                    Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                        Requisicion_Negocio.P_Dependencia_ID,
                        Requisicion_Negocio.P_Fuente_Financiamiento,
                        Requisicion_Negocio.P_Proyecto_Programa_ID,
                        Partida_ID,
                        Anio,
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                        Total_Anterior);

                    //comprometer presupuesto
                    Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                        Requisicion_Negocio.P_Dependencia_ID,
                        Requisicion_Negocio.P_Fuente_Financiamiento,
                        Requisicion_Negocio.P_Proyecto_Programa_ID,
                        Partida_ID,
                        Anio,
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                        Total_Requisicion);
                    Mensaje = "EXITO";
                }
                //Si la requisición es de STOCK se comprometen los productos
                if (Requisicion_Negocio.P_Tipo == "STOCK")
                {
                    //descomprometer lo que ya habia comprometido de esa requisicion
                    Mi_SQL =
                    "SELECT " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "," + Ope_Com_Req_Producto.Campo_Cantidad +
                    " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Requisicion_Negocio.P_Requisicion_ID;
                    DataTable Dt_Productos_Anteriores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];                 
                    Registros = Cls_Ope_Alm_Stock.Descomprometer_Producto(Dt_Productos_Anteriores, "PROD_SERV_ID", "CANTIDAD");
                    if (Registros > 0)
                    {                
                        //Comprometemos lo actual de la requisicion
                        Registros = Cls_Ope_Alm_Stock.Comprometer_Producto(Requisicion_Negocio.P_Dt_Productos_Servicios, "PROD_SERV_ID", "CANTIDAD");
                        if (Registros > 0)
                            Mensaje = "EXITO";
                        else
                        {
                            Mensaje = "NO SE PUEDEN COMPROMETER PRODUCTOS/SERVICIOS";
                            //se vuelve a comprometer lo que ya habia comprometido para dejar todo igual
                            Registros = Cls_Ope_Alm_Stock.Comprometer_Producto(Dt_Productos_Anteriores, "PROD_SERV_ID", "CANTIDAD");
                        }
                    }
                    else
                    {
                        Mensaje = Mensaje = "NO SE PUEDEN D.COMPROMETER PRODUCTOS/SERVICIOS "; ;
                    }
                    if (Mensaje == "EXITO")
                    {
                        //Descomprometer presupuesto anterior
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                            Requisicion_Negocio.P_Dependencia_ID,
                            Requisicion_Negocio.P_Fuente_Financiamiento,
                            Requisicion_Negocio.P_Proyecto_Programa_ID,
                            Partida_ID,
                            Anio,
                            Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                            Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                            Total_Anterior);

                        //comprometer presupuesto
                        Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                            Requisicion_Negocio.P_Dependencia_ID,
                            Requisicion_Negocio.P_Fuente_Financiamiento,
                            Requisicion_Negocio.P_Proyecto_Programa_ID,
                            Partida_ID,
                            Anio,
                            Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                            Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                            Total_Requisicion);                      
                    }
                }
              
                if (Mensaje == "EXITO")
                {
                    Trans.Commit();
                    Registrar_Historial(Requisicion_Negocio.P_Estatus, Requisicion_Negocio.P_Requisicion_ID);
                    //Guardar Comentarios
                    if (Requisicion_Negocio.P_Comentarios.Length > 0)
                    {
                        Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                            new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        Administrar_Requisicion.P_Requisicion_ID = Requisicion_Negocio.P_Requisicion_ID;
                        Administrar_Requisicion.P_Comentario = Requisicion_Negocio.P_Comentarios;
                        Administrar_Requisicion.P_Estatus = Requisicion_Negocio.P_Estatus;
                        Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                        Administrar_Requisicion.Alta_Observaciones();
                    }
                }
                else                
                {
                    Trans.Rollback();
                    //Mensaje = "NO SE PUDO ACTUALIZAR, VERIFIQUE DATOS DE REQUISICION";
                }
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                Mensaje = ex.ToString();
                throw new Exception(ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Mensaje;
        }

//#############

        public static String Cancelar_Requisicion_Stock_Transitoria(String No_Requisicion, String Tipo, String Usuario, String Motivo)
        {
            No_Requisicion = No_Requisicion.Trim();
            String Mensaje = "";
            String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
            //String Usuario_Creo = Usuario;
            String Mi_SQL = "";           
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
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " +
                Ope_Com_Requisiciones.Campo_Estatus + " = 'CANCELADA', " +
                Ope_Com_Requisiciones.Campo_Usuario_Modifico + " ='" + Usuario + "', " +
                Ope_Com_Requisiciones.Campo_Fecha_Modifico + " = SYSDATE, " +               
                Ope_Com_Requisiciones.Campo_Empleado_Cancelada_ID + " = '" + Cls_Sessiones.Empleado_ID.ToString() + "'," +
                Ope_Com_Requisiciones.Campo_Fecha_Cancelada + " = SYSDATE" +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();
                

                //desComprometer presupuesto partida                           
                int Registros = 0;
                DataTable Dt_Codigo_Programatico = null;

                    //Consultar CODIGO PROGRAMATICO
                    Mi_SQL = "SELECT RQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " +
                        "DETALLE." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ", " +
                        "DETALLE." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ", " +
                        "DETALLE." + Ope_Com_Req_Producto.Campo_Partida_ID +
                        " FROM " +
                        Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DETALLE, " +
                        Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " RQ " +
                        " WHERE DETALLE." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                        " AND RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion; 
                    Dt_Codigo_Programatico = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    String Fuente = Dt_Codigo_Programatico.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString();
                    String Dependencia = Dt_Codigo_Programatico.Rows[0]["DEPENDENCIA_ID"].ToString();
                    String Programa = Dt_Codigo_Programatico.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString();
                    String Partida = Dt_Codigo_Programatico.Rows[0]["PARTIDA_ID"].ToString();
                    Mi_SQL = "SELECT EXTRACT (YEAR FROM FECHA_CREO) FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                    Object Obj_Anio = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    int Anio = Convert.ToInt32(Obj_Anio);
                    //Consultar TOTAL anterior de la requisicion para descomprometerlo
                    Mi_SQL =
                    "SELECT " + Ope_Com_Requisiciones.Campo_Total +
                    " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                    Object Obj_Total = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    double Total_Anterior = Convert.ToDouble(Obj_Total);
                    //Descomprometer presupuesto anterior
                    Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                        Dependencia,
                        Fuente,
                        Programa,
                        Partida,
                        Anio,
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                        Total_Anterior);
                    //if (Registros > 0)
                        Mensaje = "EXITO";
                    //else
                      //  Mensaje = "VERIFIQUE PRESUPUESTO";

              
                if (Tipo == "STOCK" && Mensaje == "EXITO")
                {
                    //descomprometer
                    Mi_SQL =
                    "SELECT " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + "," + Ope_Com_Req_Producto.Campo_Cantidad +
                    " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + No_Requisicion;
                    DataTable Dt_Productos_Anteriores = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    if (Dt_Productos_Anteriores != null)
                    {
                        Registros = Cls_Ope_Alm_Stock.Descomprometer_Producto(Dt_Productos_Anteriores, "PROD_SERV_ID", "CANTIDAD");
                        if (Registros > 0)
                        {
                            Mensaje = "EXITO";
                        }
                        else
                        {
                            Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                                Dependencia,
                                Fuente,
                                Programa,
                                Partida,
                                Anio,
                                Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                                Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                                Total_Anterior);
                            Mensaje = "VERIFIQUE EXISTENCIA DE PRODUCTOS";
                        }
                    }
                }

                
                if (Mensaje == "EXITO")
                {
                    Trans.Commit();
                    Registrar_Historial("CANCELADA", No_Requisicion,Usuario);
                    //Guardar Comentarios
                    if (Motivo.Length > 0)
                    {
                        Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                            new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        Administrar_Requisicion.P_Requisicion_ID = No_Requisicion;
                        Administrar_Requisicion.P_Comentario = Motivo;
                        Administrar_Requisicion.P_Estatus = "CANCELADA";
                        Administrar_Requisicion.P_Usuario = Usuario;
                        Administrar_Requisicion.Alta_Observaciones();
                    }
                }
                else
                {
                    Trans.Rollback();
                    Mensaje = "ERROR";
                }
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                Mensaje = ex.ToString();
                throw new Exception(ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Mensaje;
        }


        //public static String Cancelar_Requisicion_Stock_Parcial(String No_Requisicion, String Usuario, String Motivo)
        //{
        //    No_Requisicion = No_Requisicion.Trim();
        //    String Mensaje = "VERIFIQUE";
        //    String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
        //    DataTable Dt_Productos_No_Entregados = null;
        //    //String Usuario_Creo = Usuario;
        //    String Mi_SQL = "";
        //    OracleConnection Cn = new OracleConnection();
        //    OracleCommand Cmd = new OracleCommand();
        //    OracleTransaction Trans;
        //    Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
        //    Cn.Open();
        //    Trans = Cn.BeginTransaction();
        //    Cmd.Connection = Cn;
        //    Cmd.Transaction = Trans;
        //    try
        //    {
        //        Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
        //        " SET " +
        //        Ope_Com_Requisiciones.Campo_Estatus + " = 'CANCELADA', " +
        //        Ope_Com_Requisiciones.Campo_Usuario_Modifico + " ='" + Usuario + "', " +
        //        Ope_Com_Requisiciones.Campo_Fecha_Modifico + " = SYSDATE, " +
        //        Ope_Com_Requisiciones.Campo_Empleado_Cancelada_ID + " = '" + Cls_Sessiones.Empleado_ID.ToString() + "'," +
        //        Ope_Com_Requisiciones.Campo_Fecha_Cancelada + " = SYSDATE" +
        //        " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
        //        Cmd.CommandText = Mi_SQL;
        //        Cmd.ExecuteNonQuery();


        //        //desComprometer presupuesto partida                           
        //        int Registros = 0;
        //        DataTable Dt_Codigo_Programatico = null;

        //        //Consultar CODIGO PROGRAMATICO
        //        Mi_SQL = "SELECT RQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " +
        //            "DETALLE." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ", " +
        //            "DETALLE." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ", " +
        //            "DETALLE." + Ope_Com_Req_Producto.Campo_Partida_ID +
        //            " FROM " +
        //            Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DETALLE, " +
        //            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " RQ " +
        //            " WHERE DETALLE." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
        //            " AND RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
        //        Dt_Codigo_Programatico = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        //        String Fuente = Dt_Codigo_Programatico.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString();
        //        String Dependencia = Dt_Codigo_Programatico.Rows[0]["DEPENDENCIA_ID"].ToString();
        //        String Programa = Dt_Codigo_Programatico.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString();
        //        String Partida = Dt_Codigo_Programatico.Rows[0]["PARTIDA_ID"].ToString();
        //        Mi_SQL = "SELECT EXTRACT (YEAR FROM FECHA_CREO) FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
        //            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
        //        Object Obj_Anio = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
        //        int Anio = Convert.ToInt32(Obj_Anio);


        //        //Consultar TOTAL anterior de la requisicion para descomprometerlo
        //        Mi_SQL = "SELECT no_requisicion,prod_serv_id,cantidad,cantidad_entregada, " +
        //        " cantidad - nvl(cantidad_entregada,0) cantidad_diferencia, " +
        //        " monto_total / cantidad * (cantidad - nvl(cantidad_entregada,0)) monto_diferencia " +
        //        " from ope_com_req_producto " +
        //        " where no_requisicion = " + No_Requisicion  +
        //        " and  cantidad - nvl(cantidad_entregada,0) > 0";
        //        Dt_Productos_No_Entregados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        //        double Total = 0;
        //        foreach(DataRow Renglon in Dt_Productos_No_Entregados.Rows)
        //        {
        //            Total += Convert.ToDouble(Renglon["MONTO_DIFERENCIA"]);
        //        }
        //        //Mi_SQL =
        //        //"SELECT " + Ope_Com_Requisiciones.Campo_Total +
        //        //" FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
        //        //" WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
        //        //Object Obj_Total = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                
        //        //Descomprometer presupuesto anterior
        //        Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
        //            Dependencia,
        //            Fuente,
        //            Programa,
        //            Partida,
        //            Anio,
        //            Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
        //            Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
        //            Total);
        //        //if (Registros > 0)
        //        Mensaje = "EXITO";
        //        //else
        //        //  Mensaje = "VERIFIQUE PRESUPUESTO";


        //        if (Mensaje == "EXITO")
        //        {
        //            //descomprometer productos
        //            if (Dt_Productos_No_Entregados != null )
        //            {
        //                Registros = Cls_Ope_Alm_Stock.Descomprometer_Producto(Dt_Productos_No_Entregados, "PROD_SERV_ID", "CANTIDAD_DIFERENCIA");
        //                if (Registros > 0)
        //                {
        //                    Mensaje = "EXITO";
        //                }
        //                else
        //                {
        //                    Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
        //                        Dependencia,
        //                        Fuente,
        //                        Programa,
        //                        Partida,
        //                        Anio,
        //                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
        //                        Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
        //                        Total);
        //                    Mensaje = "VERIFIQUE EXISTENCIA DE PRODUCTOS";
        //                }
        //            }
        //        }


        //        if (Mensaje == "EXITO")
        //        {
        //            Trans.Commit();
        //            Registrar_Historial("CANCELADA", No_Requisicion, Usuario);
        //            //Guardar Comentarios
        //            if (Motivo.Length > 0)
        //            {
        //                Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
        //                    new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        //                Administrar_Requisicion.P_Requisicion_ID = No_Requisicion;
        //                Administrar_Requisicion.P_Comentario = Motivo;
        //                Administrar_Requisicion.P_Estatus = "CANCELADA";
        //                Administrar_Requisicion.P_Usuario = Usuario;
        //                Administrar_Requisicion.Alta_Observaciones();
        //            }
        //        }
        //        else
        //        {
        //            Trans.Rollback();
        //            //Mensaje = "ERROR";
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Trans.Rollback();
        //        Mensaje = ex.ToString();
        //        throw new Exception(ex.ToString());
        //    }
        //    finally
        //    {
        //        Cn.Close();
        //    }
        //    return Mensaje;
        //}

        public static String Cancelar_Requisicion_Stock_Parcial(String No_Requisicion, String Usuario, String Motivo)
        {
            No_Requisicion = No_Requisicion.Trim();
            String Mensaje = "VERIFIQUE";
            String Fecha_Creo = DateTime.Now.ToString("dd/MM/yy").ToUpper();
            DataTable Dt_Productos_No_Entregados = null;
            //String Usuario_Creo = Usuario;
            String Mi_SQL = "";
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
                Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " SET " +
                Ope_Com_Requisiciones.Campo_Estatus + " = 'CANCELADA', " +
                Ope_Com_Requisiciones.Campo_Usuario_Modifico + " ='" + Usuario + "', " +
                Ope_Com_Requisiciones.Campo_Fecha_Modifico + " = SYSDATE, " +
                Ope_Com_Requisiciones.Campo_Empleado_Cancelada_ID + " = '" + Cls_Sessiones.Empleado_ID.ToString() + "'," +
                Ope_Com_Requisiciones.Campo_Fecha_Cancelada + " = SYSDATE" +
                " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery();


                //desComprometer presupuesto partida                           
                int Registros = 0;
                DataTable Dt_Codigo_Programatico = null;

                //Consultar CODIGO PROGRAMATICO
                Mi_SQL = "SELECT RQ." + Ope_Com_Requisiciones.Campo_Dependencia_ID + ", " +
                    "DETALLE." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID + ", " +
                    "DETALLE." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + ", " +
                    "DETALLE." + Ope_Com_Req_Producto.Campo_Partida_ID +
                    " FROM " +
                    Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " DETALLE, " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " RQ " +
                    " WHERE DETALLE." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                    " AND RQ." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                Dt_Codigo_Programatico = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                String Fuente = Dt_Codigo_Programatico.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString();
                String Dependencia = Dt_Codigo_Programatico.Rows[0]["DEPENDENCIA_ID"].ToString();
                String Programa = Dt_Codigo_Programatico.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString();
                String Partida = Dt_Codigo_Programatico.Rows[0]["PARTIDA_ID"].ToString();
                Mi_SQL = "SELECT EXTRACT (YEAR FROM FECHA_CREO) FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + No_Requisicion;
                Object Obj_Anio = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                int Anio = Convert.ToInt32(Obj_Anio);


                //Consultar TOTAL anterior de la requisicion para descomprometerlo
                Mi_SQL = "SELECT no_requisicion,prod_serv_id,cantidad,cantidad_entregada, " +
                " (round(cantidad - nvl(cantidad_entregada,0),2)) cantidad_diferencia, " +
                " (round(monto_total / cantidad * (cantidad - nvl(cantidad_entregada,0)),2)) monto_diferencia " +
                " from ope_com_req_producto " +
                " where no_requisicion = " + No_Requisicion +
                " and  (round(cantidad - nvl(cantidad_entregada,0),2)) > 0";
                Dt_Productos_No_Entregados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                double Total = 0;
                foreach (DataRow Renglon in Dt_Productos_No_Entregados.Rows)
                {
                    Total += Convert.ToDouble(Renglon["MONTO_DIFERENCIA"]);
                }
                //Descomprometer presupuesto anterior
                Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                    Dependencia,
                    Fuente,
                    Programa,
                    Partida,
                    Anio,
                    Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                    Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                    Total);

                Mensaje = "EXITO";

                if (Mensaje == "EXITO")
                {
                    //descomprometer productos
                    if (Dt_Productos_No_Entregados != null)
                    {
                        Registros = Cls_Ope_Alm_Stock.Descomprometer_Producto(Dt_Productos_No_Entregados, "PROD_SERV_ID", "CANTIDAD_DIFERENCIA");
                        if (Registros > 0)
                        {
                            Mensaje = "EXITO";
                        }
                        else
                        {
                            Registros = Cls_Ope_Psp_Manejo_Presupuesto_SAP.Actualizar_Momentos_Presupuestales(
                                Dependencia,
                                Fuente,
                                Programa,
                                Partida,
                                Anio,
                                Cls_Ope_Psp_Manejo_Presupuesto_SAP.COMPROMETIDO,
                                Cls_Ope_Psp_Manejo_Presupuesto_SAP.DISPONIBLE,
                                Total);
                            Mensaje = "VERIFIQUE EXISTENCIA DE PRODUCTOS";
                        }
                    }
                }


                if (Mensaje == "EXITO")
                {
                    Trans.Commit();
                    Registrar_Historial("CANCELADA", No_Requisicion, Usuario);
                    //Guardar Comentarios
                    if (Motivo.Length > 0)
                    {
                        Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                            new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        Administrar_Requisicion.P_Requisicion_ID = No_Requisicion;
                        Administrar_Requisicion.P_Comentario = Motivo;
                        Administrar_Requisicion.P_Estatus = "CANCELADA";
                        Administrar_Requisicion.P_Usuario = Usuario;
                        Administrar_Requisicion.Alta_Observaciones();
                    }
                }
                else
                {
                    Trans.Rollback();
                    //Mensaje = "ERROR";
                }
            }
            catch (Exception ex)
            {
                Trans.Rollback();
                Mensaje = ex.ToString();
                throw new Exception(ex.ToString());
            }
            finally
            {
                Cn.Close();
            }
            return Mensaje;
        }


        //#############
        public static bool Registrar_Historial(String Estatus, String No_Requisicion) 
        {
            No_Requisicion = No_Requisicion.Replace("RQ-","");
            bool Resultado = false;
            try
            {
                int Consecutivo =
                    Obtener_Consecutivo(Ope_Com_Historial_Req.Campo_No_Historial, Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req);
                String Mi_SQL = "";
                Mi_SQL = "INSERT INTO " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req + " (" +
                    Ope_Com_Historial_Req.Campo_No_Historial + "," +
                    Ope_Com_Historial_Req.Campo_No_Requisicion + "," +
                    Ope_Com_Historial_Req.Campo_Estatus + "," +
                    Ope_Com_Historial_Req.Campo_Fecha + "," +
                    Ope_Com_Historial_Req.Campo_Empleado + "," +
                    Ope_Com_Historial_Req.Campo_Usuario_Creo + "," +
                    Ope_Com_Historial_Req.Campo_Fecha_Creo + ") VALUES (" +
                    Consecutivo + ", " +
                    No_Requisicion + ", '" +
                    Estatus + "', " +
                    "SYSTIMESTAMP, '" +
                    Cls_Sessiones.Nombre_Empleado + "', '" +
                    Cls_Sessiones.Nombre_Empleado + "', " +
                    "SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Estatus.Trim() == "ALMACEN")
                {
                    Consecutivo =
                    Obtener_Consecutivo(Ope_Com_Historial_Req.Campo_No_Historial, Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req);
                    //Se realiza el registro de que se dio el Aviso a Dependencia que pueden pasar por su requisicion al Almacen 
                    Mi_SQL = "INSERT INTO " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req + " (" +
                   Ope_Com_Historial_Req.Campo_No_Historial + "," +
                   Ope_Com_Historial_Req.Campo_No_Requisicion + "," +
                   Ope_Com_Historial_Req.Campo_Estatus + "," +
                   Ope_Com_Historial_Req.Campo_Fecha + "," +
                   Ope_Com_Historial_Req.Campo_Empleado + "," +
                   Ope_Com_Historial_Req.Campo_Usuario_Creo + "," +
                   Ope_Com_Historial_Req.Campo_Fecha_Creo + ") VALUES (" +
                   Consecutivo + ", " +
                   No_Requisicion + ", 'AVISO A DEPENDENCIA', " +
                   "SYSTIMESTAMP, '" +
                   Cls_Sessiones.Nombre_Empleado + "', '" +
                   Cls_Sessiones.Nombre_Empleado + "', " +
                   "SYSDATE)";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }

                
                Resultado = true;
            }
            catch (Exception Ex) 
            {
                String Str_Ex = Ex.ToString();
                Resultado = false;
            }
            return Resultado;
        }
        public static bool Registrar_Historial(String Estatus, String No_Requisicion, String Usuario)
        {
            No_Requisicion = No_Requisicion.Replace("RQ-", "");
            bool Resultado = false;
            try
            {
                int Consecutivo =
                    Obtener_Consecutivo(Ope_Com_Historial_Req.Campo_No_Historial, Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req);
                String Mi_SQL = "";
                Mi_SQL = "INSERT INTO " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req + " (" +
                    Ope_Com_Historial_Req.Campo_No_Historial + "," +
                    Ope_Com_Historial_Req.Campo_No_Requisicion + "," +
                    Ope_Com_Historial_Req.Campo_Estatus + "," +
                    Ope_Com_Historial_Req.Campo_Fecha + "," +
                    Ope_Com_Historial_Req.Campo_Empleado + "," +
                    Ope_Com_Historial_Req.Campo_Usuario_Creo + "," +
                    Ope_Com_Historial_Req.Campo_Fecha_Creo + ") VALUES (" +
                    Consecutivo + ", " +
                    No_Requisicion + ", '" +
                    Estatus + "', " +
                    "SYSTIMESTAMP, '" +
                    Usuario + "', '" +
                    Usuario + "', " +
                    "SYSDATE)";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                if (Estatus.Trim() == "ALMACEN")
                {
                    Consecutivo =
                    Obtener_Consecutivo(Ope_Com_Historial_Req.Campo_No_Historial, Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req);
                    //Se realiza el registro de que se dio el Aviso a Dependencia que pueden pasar por su requisicion al Almacen 
                    Mi_SQL = "INSERT INTO " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req + " (" +
                   Ope_Com_Historial_Req.Campo_No_Historial + "," +
                   Ope_Com_Historial_Req.Campo_No_Requisicion + "," +
                   Ope_Com_Historial_Req.Campo_Estatus + "," +
                   Ope_Com_Historial_Req.Campo_Fecha + "," +
                   Ope_Com_Historial_Req.Campo_Empleado + "," +
                   Ope_Com_Historial_Req.Campo_Usuario_Creo + "," +
                   Ope_Com_Historial_Req.Campo_Fecha_Creo + ") VALUES (" +
                   Consecutivo + ", " +
                   No_Requisicion + ", 'AVISO A DEPENDENCIAS', " +
                   "SYSTIMESTAMP, '" +
                   Cls_Sessiones.Nombre_Empleado + "', '" +
                   Cls_Sessiones.Nombre_Empleado + "', " +
                   "SYSDATE)";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                }
                Resultado = true;
            }
            catch (Exception Ex)
            {
                String Str_Ex = Ex.ToString();
                Resultado = false;
            }
            return Resultado;
        }

        public static DataTable Consultar_Historial_Requisicion(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            try
            {
                String Mi_SQL = "";
                Mi_SQL =
                "SELECT * FROM " + Ope_Com_Historial_Req.Tabla_Ope_Com_Historial_Req +
                " WHERE " + Ope_Com_Historial_Req.Campo_No_Requisicion + " = " +
                Requisicion_Negocio.P_Requisicion_ID +
                " ORDER BY " + Ope_Com_Historial_Req.Campo_Fecha + " ASC";
                DataTable Data_Table =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Data_Table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        //public static bool Verificar_Rango_Caja_Chica(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        //{
        //    bool Respuesta = false;
        //    double Monto_Inicio = 0;
        //    double Monto_Fin = 0;
        //    String Mi_Sql = "";
        //    Mi_Sql = "SELECT " +
        //        Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Ini + ", " +
        //        Cat_Com_Monto_Proceso_Compra.Campo_Monto_Compra_Directa_Fin +
        //        " FROM " +
        //        Cat_Com_Monto_Proceso_Compra.Tabla_Cat_Com_Monto_Proceso_Compra +
        //        " WHERE " +
        //        Cat_Com_Monto_Proceso_Compra.Campo_Tipo + " = '" + Requisicion_Negocio.P_Tipo_Articulo + "'";
        //    DataSet _DSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
        //    DataTable Dt_Tmp = null;
        //    if (_DSet != null && _DSet.Tables.Count > 0 && _DSet.Tables[0].Rows.Count > 0)
        //    {
        //        Dt_Tmp = _DSet.Tables[0];
        //        Monto_Inicio = Convert.ToDouble(Dt_Tmp.Rows[0]["MONTO_COMPRA_DIRECTA_INI"].ToString().Trim());
        //        Monto_Fin = Convert.ToDouble(Dt_Tmp.Rows[0]["MONTO_COMPRA_DIRECTA_FIN"].ToString().Trim());
        //        double Total = Convert.ToDouble(Requisicion_Negocio.P_Total);
        //        Respuesta = Total >= Monto_Inicio && Total <= Monto_Fin ? true : false;
        //    }
        //    return Respuesta;
        //}

        public static int Asignar_Reserva(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio) 
        {
            int Registros = 0;
            try
            {
                String Mi_Sql = "";
                Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " SET NO_RESERVA ='" + Requisicion_Negocio.P_No_Reserva + "'," +
                    Ope_Com_Requisiciones.Campo_Estatus + " = '" + Requisicion_Negocio.P_Estatus + "'" + 
                    " WHERE " +
                    Ope_Com_Requisiciones.Campo_Folio + " ='" + Requisicion_Negocio.P_Folio + "'";
                Registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);                                
            }
            catch(Exception Ex)
            {
                Ex.ToString();
                Registros = 0;
            }
            if (Registros > 0)
                Registrar_Historial("ALMACEN", Requisicion_Negocio.P_Folio);
            return Registros;
        }
        public static int Rechaza_Contabilidad(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            int Registros = 0;
            try
            {
                String Mi_Sql = "";
                Mi_Sql = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " SET " + Ope_Com_Requisiciones.Campo_Estatus + " = '" + Requisicion_Negocio.P_Estatus + "'," +
                    " ALERTA ='ROJA'" +
                    " WHERE " +
                    Ope_Com_Requisiciones.Campo_Folio + " ='" + Requisicion_Negocio.P_Folio + "'";
                Registros = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
                if (Registros > 0)
                {
                    Cls_Ope_Com_Administrar_Requisiciones_Negocio Admin_Req = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                    Admin_Req.P_Requisicion_ID = Requisicion_Negocio.P_Requisicion_ID;
                    Admin_Req.P_Comentario = Requisicion_Negocio.P_Comentarios;
                    Admin_Req.P_Estatus = Requisicion_Negocio.P_Estatus;
                    Admin_Req.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Admin_Req.Alta_Observaciones();
                }
            }
            catch (Exception Ex)
            {
                Ex.ToString();
                Registros = 0;
            }
            if (Registros > 0)
                Registrar_Historial(Requisicion_Negocio.P_Estatus, Requisicion_Negocio.P_Folio);
            return Registros;
        }
        public static int Actualizar_Requisicion_Estatus(Cls_Ope_Com_Requisiciones_Negocio Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql = "UPDATE " +
            Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " SET " +
            Ope_Com_Requisiciones.Campo_Estatus + " = '" + Negocio.P_Estatus + "' " +

            " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Negocio.P_Requisicion_ID;
            int Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            return Registros_Afectados;
        }
        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region CONSULTAS / FTE FINANCIAMIENTO, PROYECTOS, PARTIDAS, PRESUPUESTOS
       
        public static DataTable Consultar_Fuentes_Financiamiento(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            try
            {
                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Especial_Ramo33) && Requisicion_Negocio.P_Especial_Ramo33 == "NO")
                {
                    Requisicion_Negocio.P_Especial_Ramo33 = null;
                }
                String Mi_SQL = "";
                Mi_SQL =
                "SELECT FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + "," +
                " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " ||' '||" +
                " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion +
                " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + " FUENTE" +
                " JOIN " + Cat_SAP_Det_Fte_Dependencia.Tabla_Cat_SAP_Det_Fte_Financiamiento_Dependencia + " DETALLE" +
                " ON FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + " = " +
                " DETALLE." + Cat_SAP_Det_Fte_Dependencia.Campo_Fuente_Financiamiento_ID +
                " WHERE DETALLE." + Cat_SAP_Det_Fte_Dependencia.Campo_Dependencia_ID + " = " +
                "'" + Requisicion_Negocio.P_Dependencia_ID + "'" +
                " AND FUENTE." + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID + " IN " +
                "(SELECT " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = " +
                "'" + Requisicion_Negocio.P_Dependencia_ID + "'" + ")";
                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Especial_Ramo33) && Requisicion_Negocio.P_Especial_Ramo33 == "SI")
                {
                    Mi_SQL += " AND FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " = 'SI'";
                }
                //if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Especial_Ramo33) && Requisicion_Negocio.P_Especial_Ramo33 == "NO")
                //{
                //    Mi_SQL += " AND FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " IS NULL OR ";
                //    Mi_SQL += " FUENTE." + Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " = 'NO' ";
                //}
                Mi_SQL += " ORDER BY " + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " ASC";
                DataTable Data_Table =
                    OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Data_Table;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        //GUS  
        public static DataTable Consultar_Proyectos_Programas(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";
            Mi_SQL =
            "SELECT PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + "," +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Clave + " ||' '||" +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Nombre + "," +
            " PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Elemento_PEP +
            " FROM " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROGRAMA" +            
            " JOIN " + Cat_SAP_Det_Prog_Dependencia.Tabla_Cat_SAP_Det_Programa_Dependencia + " DETALLE" +
            " ON PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + " = " +
            " DETALLE." + Cat_SAP_Det_Prog_Dependencia.Campo_Proyecto_Programa_ID +
            " WHERE DETALLE." + Cat_SAP_Det_Prog_Dependencia.Campo_Dependencia_ID + " = " +
            "'" + Requisicion_Negocio.P_Dependencia_ID + "' ORDER BY " + 
            Cat_Com_Proyectos_Programas.Campo_Descripcion + " ASC";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }


        //CONSULTAR PARTIDAS DE UN PROGRAMA
        public static DataTable Consultar_Partidas_De_Un_Programa(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + ", " +
            " PARTIDA." + Cat_Com_Partidas.Campo_Clave + " ||' '||" +
            " PARTIDA." + Cat_Com_Partidas.Campo_Nombre +
            " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas + " PARTIDA" +
            " JOIN " + Cat_Sap_Det_Prog_Partidas.Tabla_Cat_Sap_Det_Prog_Partidas + " DETALLE" +
            " ON PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + " = " +
            " DETALLE." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Partida_ID +
            " WHERE DETALLE." + Cat_Sap_Det_Prog_Partidas.Campo_Det_Proyecto_Programa_ID + " = " +
            "'" + Requisicion_Negocio.P_Proyecto_Programa_ID + "'";
            if (Requisicion_Negocio.P_Tipo == "STOCK")
            {
                Mi_SQL = Mi_SQL + " AND " + " PARTIDA." + Cat_Com_Partidas.Campo_Partida_ID + " IN " +
                    "(SELECT DISTINCT(" + Cat_Com_Productos.Campo_Partida_ID + ") FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " + Cat_Com_Productos.Campo_Stock + " = 'SI')";
            }
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Partidas.Campo_Nombre + " ASC";
            DataTable Data_Table =
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        ////OBTINE PARTIDAS ESPECIFICAS CON PRESPUESTOS A PARTIR DE LA DEPENDENCIA Y EL PROYECTO
        public static DataTable Consultar_Presupuesto_Partidas(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";

            Mi_SQL =
            "SELECT " +
            Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ", " +
                "(SELECT " + Cat_Com_Partidas.Campo_Nombre + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") NOMBRE, " +

                "(SELECT " + Cat_Com_Partidas.Campo_Clave + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") CLAVE, " +

                "(SELECT " + Cat_Com_Partidas.Campo_Clave + " ||' '||" +
                Cat_Com_Partidas.Campo_Nombre + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                " WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") CLAVE_NOMBRE, " +

                //con esto obtengo el giro de la partida
                //"(SELECT " + Cat_Com_Partidas.Campo_Giro_ID + " FROM " + Cat_Com_Partidas.Tabla_Cat_Com_Partidas +
                //" WHERE " + Cat_Com_Partidas.Campo_Partida_ID + " = " +
                //Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                //Cat_Com_Dep_Presupuesto.Campo_Partida_ID + ") GIRO_ID, " +

            Cat_Com_Dep_Presupuesto.Campo_Monto_Presupuestal + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + " MONTO_DISPONIBLE, " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + ", " +
            Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ", " +
            Cat_Com_Dep_Presupuesto.Campo_Fecha_Creo +
                //" TO_CHAR(FECHA_CREO ,'DD/MM/YY') FECHA_CREO" + 
            " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
            " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
            " = '" + Requisicion_Negocio.P_Proyecto_Programa_ID + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
            " = '" + Requisicion_Negocio.P_Dependencia_ID + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
            " = '" + Requisicion_Negocio.P_Fuente_Financiamiento + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
            " IN (" + Requisicion_Negocio.P_Partida_ID + ")" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
            " = '" + Requisicion_Negocio.P_Anio_Presupuesto + "'" +
            " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " +
                "(SELECT MAX(" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ")" +
                " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto +
                " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                            Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID +
                            " = '" + Requisicion_Negocio.P_Proyecto_Programa_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." +
                            Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID +
                            " = '" + Requisicion_Negocio.P_Dependencia_ID + "'" +
                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Fuente_Financiamiento_ID +
                            " = '" + Requisicion_Negocio.P_Fuente_Financiamiento + "'" +

                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto +
                            " = '" + Requisicion_Negocio.P_Anio_Presupuesto + "'" +

                            " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID +
                            " IN (" + Requisicion_Negocio.P_Partida_ID + "))";

            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        #endregion


        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region MANEJO DE PRODUCTOS STOCK / COMPROMETER, DESCOMPROMETER

 
        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************
        #region BUSQUEDA DE PRODUCTOS Y DETALLES

        public static DataTable Consultar_Poducto_Por_ID(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos +".*," + 
            Cat_Com_Productos.Campo_Nombre + " ||', '||" + Cat_Com_Productos.Campo_Descripcion +
            " AS NOMBRE_DESCRIPCION" +

            ", (SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE " +
                Cat_Com_Unidades.Campo_Unidad_ID + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + 
                Cat_Com_Productos.Campo_Unidad_ID + ") AS UNIDAD " +

            " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
            " WHERE " + Cat_Com_Productos.Campo_Producto_ID +
            " IN (" + Requisicion_Negocio.P_Producto_ID + ")";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        //BUSQUEDA EN MODAL POPUP DE UN PRODUCTO
        public static DataTable Consultar_Productos(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";
            String Stock = "";
            if (Requisicion_Negocio.P_Tipo == "STOCK")
            {
                Stock = "SI";
            }
            else
            {
                Stock = "NO";
            }
            Mi_SQL = "SELECT " + Cat_Com_Productos.Campo_Producto_ID + " AS ID, " +
            Cat_Com_Productos.Tabla_Cat_Com_Productos + ".*, " + 
            Cat_Com_Productos.Campo_Nombre + " ||'; '|| " + Cat_Com_Productos.Campo_Descripcion + " AS NOMBRE_DESCRIPCION " +

            ",(SELECT ABREVIATURA FROM CAT_COM_UNIDADES WHERE UNIDAD_ID = CAT_COM_PRODUCTOS.UNIDAD_ID) AS UNIDAD" +
            ",(SELECT NOMBRE FROM CAT_COM_MODELOS WHERE MODELO_ID = CAT_COM_PRODUCTOS.MODELO_ID) AS MODELO" +


            " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
            " WHERE " + Cat_Com_Productos.Campo_Stock + " = '" + Stock + "'" + 
            " AND " + Cat_Com_Productos.Campo_Estatus + " = 'ACTIVO'";
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Partida_ID))
            {
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Productos.Campo_Partida_ID +
                    " = '" + Requisicion_Negocio.P_Partida_ID + "'";
            }
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Nombre_Producto_Servicio))
            {
                Mi_SQL = Mi_SQL + " AND UPPER(" + Cat_Com_Productos.Campo_Nombre +
                    ") LIKE UPPER('%" + Requisicion_Negocio.P_Nombre_Producto_Servicio + "%') ";
            }
            Mi_SQL = Mi_SQL + " ORDER BY " +
                    Cat_Com_Productos.Campo_Nombre + " ASC";
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        public static DataSet Consultar_Impuesto(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Impuestos.Campo_Nombre + ", " +
                Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " FROM " +
                Cat_Com_Impuestos.Tabla_Cat_Impuestos +
                " WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID + " = '" + Requisicion_Negocio.P_Impuesto_ID + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************                               
        #region CONSULTA REQUISICIONES
        public static bool Proceso_Filtrar(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            bool Actualizado = false;
            try
            {
                String Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " SET " + 
                    Ope_Com_Requisiciones.Campo_Estatus + " = '" + Requisicion_Negocio.P_Estatus + "'," +
                    Ope_Com_Requisiciones.Campo_Fecha_Filtrado + " = SYSDATE" +
                    " WHERE " +
                    Ope_Com_Requisiciones.Campo_Folio + " = '" + Requisicion_Negocio.P_Folio + "'";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Actualizado = true;
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                Actualizado = false;
            }
            return Actualizado;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Requisiciones
        ///DESCRIPCIÓN: crea una sentencia sql para conultar una Requisa en la base de datos
        ///PARAMETROS: 1.-Clase de Negocio
        ///            2.-Usuario que crea la requisa
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: Noviembre/2010 
        ///MODIFICO:Gustavo Angeles Cruz
        ///FECHA_MODIFICO: 25/Ene/2011
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        public static DataTable Consultar_Requisiciones(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {

            Requisicion_Negocio.P_Fecha_Inicial = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Requisicion_Negocio.P_Fecha_Inicial));
            Requisicion_Negocio.P_Fecha_Final = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Requisicion_Negocio.P_Fecha_Final));
            if (Requisicion_Negocio.P_Estatus == "CONST ,GENERADA, REVISAR")
            {
                Requisicion_Negocio.P_Estatus = "EN CONSTRUCCION,GENERADA,REVISAR,RECHAZADA,ALMACEN,SURTIDA";
            }
            Requisicion_Negocio.P_Estatus = Requisicion_Negocio.P_Estatus.Replace(",","','");
            Requisicion_Negocio.P_Tipo = Requisicion_Negocio.P_Tipo.Replace(",","','");
            String Mi_Sql = "";
            Mi_Sql =
            "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") NOMBRE_DEPENDENCIA " +
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " + 
            Ope_Com_Requisiciones.Campo_Estatus + " IN ('" + Requisicion_Negocio.P_Estatus + "')" +
            " AND " + Ope_Com_Requisiciones.Campo_Tipo + " IN ('" + Requisicion_Negocio.P_Tipo + "')";
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Dependencia_ID) && Requisicion_Negocio.P_Dependencia_ID != "0")
            {
                Mi_Sql += " AND " + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                " = '" + Requisicion_Negocio.P_Dependencia_ID + "'";
            }
            if (Requisicion_Negocio.P_Estatus == "GENERADA")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Generacion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "CANCELADA")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Cancelada + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Cancelada + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "EN CONSTRUCCION")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Construccion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Construccion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "AUTORIZADA")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "CONFIRMADA")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Confirmacion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Confirmacion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "COTIZADA")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "REVISAR")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else if (Requisicion_Negocio.P_Estatus == "COMPRA")
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Cotizacion + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            else 
            {
                Mi_Sql = Mi_Sql + " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            }
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Requisicion_ID))
            {
                Mi_Sql +=
                " AND " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                " = " + Requisicion_Negocio.P_Requisicion_ID;
            }
            if (Requisicion_Negocio.P_Cotizador_ID != null && Requisicion_Negocio.P_Cotizador_ID != "0")
            {
                Mi_Sql +=
                " AND " + Ope_Com_Requisiciones.Campo_Cotizador_ID +
                " = '" + Requisicion_Negocio.P_Cotizador_ID + "'";
            }

            
            Mi_Sql = Mi_Sql + " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " DESC";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        public static DataTable Consultar_Requisiciones_Generales(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            Requisicion_Negocio.P_Fecha_Inicial = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Requisicion_Negocio.P_Fecha_Inicial));
            Requisicion_Negocio.P_Fecha_Final = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Requisicion_Negocio.P_Fecha_Final));
            if (Requisicion_Negocio.P_Tipo == "TODOS") 
            {
                Requisicion_Negocio.P_Tipo = "STOCK','TRANSITORIA";
            }
             String Mi_Sql = "";
            Mi_Sql =
            "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") NOMBRE_DEPENDENCIA " +
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " +
            Ope_Com_Requisiciones.Campo_Tipo + " IN ('" + Requisicion_Negocio.P_Tipo + "')" +
            " AND TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                        " >= '" + Requisicion_Negocio.P_Fecha_Inicial + "' AND " +
                "TO_DATE(TO_CHAR(" + Ope_Com_Requisiciones.Campo_Fecha_Creo + ",'DD-MM-YYYY'))" +
                        " <= '" + Requisicion_Negocio.P_Fecha_Final + "'";
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Dependencia_ID) && Requisicion_Negocio.P_Dependencia_ID != "0")
            {
                Mi_Sql = Mi_Sql + " AND " + Ope_Com_Requisiciones.Campo_Dependencia_ID + " = " +
                    "'" + Requisicion_Negocio.P_Dependencia_ID + "'";
            }            
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Requisicion_ID))
            {
                Mi_Sql +=
                " AND " + Ope_Com_Requisiciones.Campo_Requisicion_ID +
                " = " + Requisicion_Negocio.P_Requisicion_ID;
            }
            if (!string.IsNullOrEmpty(Requisicion_Negocio.P_Estatus))
            {
                Mi_Sql +=
                " AND " + Ope_Com_Requisiciones.Campo_Estatus +
                " IN ('" + Requisicion_Negocio.P_Estatus + "')";
            }

            Mi_Sql = Mi_Sql + " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " DESC";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        //
        public static DataTable Consultar_Requisiciones_En_Web(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {

            String Mi_Sql = "";
            if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Folio))
            {
                Requisicion_Negocio.P_Folio = Requisicion_Negocio.P_Folio.Trim();
                Mi_Sql =
                "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, " +
                    "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                    ") NOMBRE_DEPENDENCIA " +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " +
                Ope_Com_Requisiciones.Campo_Tipo + " IN ('" + Requisicion_Negocio.P_Tipo + "')" +
                " AND " + Ope_Com_Requisiciones.Campo_Estatus +
                " NOT IN ('EN CONSTRUCCION','GENERADA','AUTORIZADA','RECHAZADA','FILTRADA','REVISAR') " +
                " AND " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Requisicion_Negocio.P_Folio;
            }
            else
            {
                Mi_Sql =
                "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, " +
                    "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                    " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                    ") NOMBRE_DEPENDENCIA " +
                " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                " WHERE " +
                Ope_Com_Requisiciones.Campo_Tipo + " IN ('" + Requisicion_Negocio.P_Tipo + "')" +
                " AND " + Ope_Com_Requisiciones.Campo_Estatus +
                " IN ('PROVEEDOR','COTIZADA','CONFIRMADA','COMPRA') ";       
            }
            //if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Cotizador_ID) )
            //{
            //    if (Requisicion_Negocio.P_Cotizador_ID != "0")
            //    {
            //        Mi_Sql += " AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." +
            //            Ope_Com_Requisiciones.Campo_Cotizador_ID + " = '" + Requisicion_Negocio.P_Cotizador_ID + "' ";
            //    }
            //}


            Mi_Sql += " ORDER BY " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " DESC";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            }
        }

        public static DataTable Consultar_Requisicion_Por_ID(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_Sql = "";
            Mi_Sql =
            "SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + ".*, " +
                "(SELECT " + Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias +
                " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID +
                ") NOMBRE_DEPENDENCIA " +
            " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
            " WHERE " +
            Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Requisicion_Negocio.P_Requisicion_ID;
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql);
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return (Data_Set.Tables[0]);
            }
            else
            {
                return null;
            } 
        }

        //Consulta los detalles de la requisición
        public static DataTable Consultar_Productos_Servicios_Requisiciones(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "";
            if (Requisicion_Negocio.P_Tipo_Articulo != null && Requisicion_Negocio.P_Tipo_Articulo.Trim() == "PRODUCTO")
            {
                Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".*," +
                "(SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE " +
                Cat_Com_Unidades.Campo_Unidad_ID + " = (SELECT " + Cat_Com_Productos.Campo_Unidad_ID + " FROM " +
                Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = " +
                Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + ")) AS UNIDAD ";
                
            }
            else 
            {
                Mi_SQL = "SELECT " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ".*," +
                "'S/U' AS UNIDAD ";
            }
             Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                " WHERE " + 
                Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." +
                Ope_Com_Req_Producto.Campo_Requisicion_ID + 
                " = '" + Requisicion_Negocio.P_Requisicion_ID + "'";
            Mi_SQL = Mi_SQL + " ORDER BY " + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio;
            
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            DataTable _DataTable = null;
            if (_DataSet != null && _DataSet.Tables.Count > 0 && _DataSet.Tables[0].Rows.Count > 0)
            {
                _DataTable = _DataSet.Tables[0];
            }
            return _DataTable;
        }

        #endregion

        //*********************************************************************************************
        //*********************************************************************************************
        //*********************************************************************************************                               
        #region SERVICIOS

        public static DataTable Consultar_Servicio_Por_ID(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            //Cat_Com_Productos.Campo_Nombre + " ||'; '|| " + Cat_Com_Productos.Campo_Descripcion
            String Mi_SQL = "SELECT " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + ".*, " +  
                Cat_Com_Servicios.Campo_Nombre + "||', '||" + Cat_Com_Servicios.Campo_Comentarios +
                " AS NOMBRE_DESCRIPCION, 'S/U' AS UNIDAD " +
                " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios +

                " WHERE " + Cat_Com_Servicios.Campo_Servicio_ID +
                " IN (" + Requisicion_Negocio.P_Producto_ID + ")";//el Producto ID de Negocio sirve para pasar el Servicio_ID
            DataSet _DataSet = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (_DataSet != null && _DataSet.Tables[0].Rows.Count > 0)
            {
                return (_DataSet.Tables[0]);
            }
            else
            {
                return null;
            }
        }
        
        public static DataTable Consultar_Servicios(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            String Mi_SQL = "SELECT " +
                Cat_Com_Servicios.Campo_Clave + ", " +
                Cat_Com_Servicios.Campo_Servicio_ID + " AS ID, " +

                Cat_Com_Servicios.Campo_Nombre + "||', '||" + Cat_Com_Servicios.Campo_Comentarios + " AS NOMBRE_DESCRIPCION, " + 

                Cat_Com_Servicios.Tabla_Cat_Com_Servicios + ".*, '0' AS DISPONIBLE, 'SRV' AS MODELO" +
                //Cat_Com_Servicios.Campo_Nombre + " AS NOMBRE, " + 
                //Cat_Com_Servicios.Campo_Costo + 
                " FROM " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios +
                " WHERE " + 
                Cat_Com_Partidas.Campo_Partida_ID + " = '" + Requisicion_Negocio.P_Partida_ID + "'";

            if (Requisicion_Negocio.P_Nombre_Producto_Servicio != null)
            {
                Mi_SQL = Mi_SQL + " AND UPPER (" + Cat_Com_Servicios.Campo_Nombre +
                    ") LIKE UPPER ('%" + Requisicion_Negocio.P_Nombre_Producto_Servicio + "%')";
            }
            DataTable Data_Table = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Data_Table;
        }

        public static DataTable Consultar_Partida_Presupuestal_Por_Tipo_Requisicion(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            DataTable Tabla = null;
            String Mi_SQL =
            "SELECT OPE_SAP_DEP_PRESUPUESTO.FUENTE_FINANCIAMIENTO_ID,OPE_SAP_DEP_PRESUPUESTO.AREA_FUNCIONAL_ID, " +
            "OPE_SAP_DEP_PRESUPUESTO.PROYECTO_PROGRAMA_ID, " +
            "OPE_SAP_DEP_PRESUPUESTO.DEPENDENCIA_ID, OPE_SAP_DEP_PRESUPUESTO.PARTIDA_ID, MONTO_DISPONIBLE, " +
            "(" +
            "REPLACE(CAT_SAP_FTE_FINANCIAMIENTO.CLAVE,' ','') " +
            "||'-'|| " +
            "REPLACE(CAT_SAP_AREA_FUNCIONAL.CLAVE,' ','') " +
            "||'-'|| " +
            "REPLACE(CAT_SAP_PROYECTOS_PROGRAMAS.CLAVE,' ','') " +
            "||'-'|| " +
            "REPLACE(CAT_DEPENDENCIAS.CLAVE,' ','') " +
            "||'-'|| " +
            "REPLACE(CAT_SAP_PARTIDAS_ESPECIFICAS.CLAVE,' ','') " +
            ") " +
            "AS CODIGO, " +
            "(REPLACE(CAT_SAP_PARTIDAS_ESPECIFICAS.CLAVE,' ','') ||' - '|| UPPER(CAT_SAP_PARTIDAS_ESPECIFICAS.NOMBRE)) AS PARTIDA " +
            "FROM OPE_SAP_DEP_PRESUPUESTO " +
            "LEFT JOIN CAT_SAP_PARTIDAS_ESPECIFICAS " +
            "ON OPE_SAP_DEP_PRESUPUESTO.PARTIDA_ID = CAT_SAP_PARTIDAS_ESPECIFICAS.PARTIDA_ID " +
            "LEFT JOIN CAT_DEPENDENCIAS " +
            "ON OPE_SAP_DEP_PRESUPUESTO.DEPENDENCIA_ID = CAT_DEPENDENCIAS.DEPENDENCIA_ID " +
            "LEFT JOIN CAT_SAP_PROYECTOS_PROGRAMAS " +
            "ON OPE_SAP_DEP_PRESUPUESTO.PROYECTO_PROGRAMA_ID = CAT_SAP_PROYECTOS_PROGRAMAS.PROYECTO_PROGRAMA_ID " +
            "LEFT JOIN CAT_SAP_AREA_FUNCIONAL " +
            "ON OPE_SAP_DEP_PRESUPUESTO.AREA_FUNCIONAL_ID = CAT_SAP_AREA_FUNCIONAL.AREA_FUNCIONAL_ID " +
            "LEFT JOIN CAT_SAP_FTE_FINANCIAMIENTO " +
            "ON OPE_SAP_DEP_PRESUPUESTO.FUENTE_FINANCIAMIENTO_ID = CAT_SAP_FTE_FINANCIAMIENTO.FUENTE_FINANCIAMIENTO_ID " +
            "WHERE OPE_SAP_DEP_PRESUPUESTO.DEPENDENCIA_ID = '" + Requisicion_Negocio.P_Dependencia_ID + "' " +
            "AND OPE_SAP_DEP_PRESUPUESTO.ANIO_PRESUPUESTO = " + Requisicion_Negocio.P_Anio_Presupuesto;
            if (Requisicion_Negocio.P_Tipo == "STOCK")
            {
                Mi_SQL = Mi_SQL + " AND OPE_SAP_DEP_PRESUPUESTO.PARTIDA_ID IN " +
                    "(SELECT DISTINCT(" + Cat_Com_Productos.Campo_Partida_ID + ") FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos +
                    " WHERE " + Cat_Com_Productos.Campo_Stock + " = 'SI' AND ESTATUS = 'ACTIVO')";
            }
            else if (Requisicion_Negocio.P_Tipo == "TRANSITORIA" && Requisicion_Negocio.P_Especial_Ramo33 == "SI")
            {
                Mi_SQL = Mi_SQL + " AND OPE_SAP_DEP_PRESUPUESTO.FUENTE_FINANCIAMIENTO_ID IN " +
                    "(SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID + 
                    " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento +
                    " WHERE " +
                    Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " = 'SI')";
            }
            else
            {
                Mi_SQL = Mi_SQL + " AND OPE_SAP_DEP_PRESUPUESTO.FUENTE_FINANCIAMIENTO_ID IN " +
                    "(SELECT " + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID +
                    " FROM " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento +
                    " WHERE " +
                    Cat_SAP_Fuente_Financiamiento.Campo_Especiales_Ramo_33 + " = 'NO')";                
            }
            
            Mi_SQL += " ORDER BY CAT_SAP_PARTIDAS_ESPECIFICAS.CLAVE ";

            try
            {
                Tabla = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            } catch(Exception Ex)
            {
                throw new Exception(Ex.ToString());
            }
            return Tabla;
        }

        public static String Elaborar_Requisicion_A_Partir_De_Otra(int No_Requisicion)
        {
            //DataTable Dt_Requisiciones = null;
            DataTable Dt_Aux = null;
            String Mi_SQL = "";
            //Consultar_Columnas_De_Tabla_BD los Proveedores para saber cuantas Requisiciones_Parciales se van a aelaborar
            Mi_SQL = "SELECT DISTINCT(" + Ope_Com_Req_Producto.Campo_Proveedor_ID + "), " +
            Ope_Com_Req_Producto.Campo_Nombre_Proveedor +
            " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
            " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID +
            " IN (" + No_Requisicion + ")";
            DataTable Dt_Proveedores =
                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            //Consultar encabezado de la requisición

            String Requisiciones_Creadas = "";
            Cls_Ope_Com_Requisiciones_Negocio Requisicion = new Cls_Ope_Com_Requisiciones_Negocio();
            Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion =
                new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
            int Consecutivo_RQ = Obtener_Consecutivo(Ope_Com_Requisiciones.Campo_Requisicion_ID, Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones);
            int Consecutivo_Productos_Requisa =
                    Obtener_Consecutivo(Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID, Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos
            int x = 0;
            try
            {
                //Recorremos todos los pproveedores para crearles su requisicion
                foreach (DataRow Dr_Proveedor in Dt_Proveedores.Rows)
                {
                    //borrar cualquier registro de la tabla copia que coincida con la requisicion a dividir
                    Mi_SQL = "DELETE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "_COPIA " +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + No_Requisicion;
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Insertar encabezado para nueva requisicion
                    //cOPIAMOS EL REGISTRO DE LA REQUISICION A UN ATABLA ESPEJO
                    Mi_SQL = "INSERT INTO OPE_COM_REQUISICIONES_COPIA SELECT * FROM " + 
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + No_Requisicion + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //x = Comando_SQL.ExecuteNonQuery();

                    //ACTUALIZAMOS ENLA TABLA ESPEJO LOS DATOS DE LA REQUISICION COMO ID Y FOLIO
                    Mi_SQL = "UPDATE OPE_COM_REQUISICIONES_COPIA SET " +
                    Ope_Com_Requisiciones.Campo_Requisicion_ID + " = " + Consecutivo_RQ + ", " +
                    Ope_Com_Requisiciones.Campo_Folio + " = 'RQ-" + Consecutivo_RQ + "' " +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + No_Requisicion + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //x = Comando_SQL.ExecuteNonQuery();
                    
                    //INSERTAMOS EL NUEVO REGISTRO EN LA TABLA DE REQUISICIONES
                    Mi_SQL = "INSERT INTO " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " SELECT * FROM " +
                    Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "_COPIA " +
                    " WHERE " + Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Consecutivo_RQ + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //x = Comando_SQL.ExecuteNonQuery();

                    //BORRAMOS EL REGISTRO DE LA TABLA ESPEJO
                    Mi_SQL = "DELETE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "_COPIA WHERE " +
                    Ope_Com_Requisiciones.Campo_Requisicion_ID + " IN (" + Consecutivo_RQ + ")";
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //x = Comando_SQL.ExecuteNonQuery();

                    //INSERTAMOS LOS DETALLES DE LA NUEVA REQUISICION

                    //COPIAMOS EL REGISTRO DE LOS DETALLES DE LA REQUISICION A UNA TABLA ESPEJO
                    Mi_SQL = "INSERT INTO " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "_COPIA" + " SELECT * FROM " +
                    Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + No_Requisicion + " AND " +
                    Ope_Com_Req_Producto.Campo_Proveedor_ID + " = " + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString();
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //Comando_SQL.ExecuteNonQuery();

                    //ACTUALIZAMOS EN LA TABLA ESPEJO LOS ID's ID Y FOLIO DE LOS DETALLES PARA NO CAUSAR CONFLICTO CON LOS DE LA TABLA ORIGINAL
                    //BUSCAMOS LOS DETALLES DE LA REQUISICION
                    Mi_SQL = "SELECT * FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "_COPIA " +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + No_Requisicion + " AND " +
                    Ope_Com_Req_Producto.Campo_Proveedor_ID + " = " + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString();                    
                    Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                    //Consecutivo_Productos_Requisa =
                    //    Obtener_Consecutivo(Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID, Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                    foreach (DataRow Producto in Dt_Aux.Rows)
                    {
                        Mi_SQL = "UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "_COPIA " +
                        " SET " + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + " = " + Consecutivo_Productos_Requisa + "," +
                        Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Consecutivo_RQ +
                        " WHERE " +
                        Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + " = " + Producto[Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID].ToString();

                        //Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + No_Requisicion + " AND " +
                        //Ope_Com_Req_Producto.Campo_Proveedor_ID + " = " + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString();
                        
                        OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                        Consecutivo_Productos_Requisa = Consecutivo_Productos_Requisa + 1;
                    }
                        

                    //INSERTAMOS EL NUEVO REGISTRO EN LA TABLA DE REQUISICIONES
                    Mi_SQL = "INSERT INTO " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " SELECT * FROM " +
                    Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "_COPIA " +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Consecutivo_RQ + " AND " +
                    Ope_Com_Req_Producto.Campo_Proveedor_ID + " = " + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString();
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //Comando_SQL.ExecuteNonQuery();

                    //BORRAMOS EL REGISTRO DE LA TABLA ESPEJO
                    Mi_SQL = "DELETE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "_COPIA " +
                    " WHERE " + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = " + Consecutivo_RQ + " AND " +
                    Ope_Com_Req_Producto.Campo_Proveedor_ID + " = " + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString();
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    //Comando_SQL.CommandText = Mi_SQL;
                    //Comando_SQL.ExecuteNonQuery();

                    //REGISTRAR HISTORIAL A LA NUEVA REQUISICION
                    Registrar_Historial("CONFIRMADA", Consecutivo_RQ.ToString());

                    //PONGO COMENTARIO A LA NUEVA REQUISICION
                    Administrar_Requisicion = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                    Administrar_Requisicion.P_Requisicion_ID = Consecutivo_RQ.ToString();
                    Administrar_Requisicion.P_Comentario = "Requisición creada a partir de la requisición RQ-" + No_Requisicion + " por separación de productos";
                    Administrar_Requisicion.P_Estatus = "CONFIRMADA";
                    Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                    Administrar_Requisicion.Alta_Observaciones();

                    //ACTUALIZO LOS COSTOS DE LA REQUISICION SEGUN LOS PRODUCTOS QUE CONTENGA
                    Mi_SQL = "" +
                    "UPDATE OPE_COM_REQUISICIONES SET " +
                    " SUBTOTAL_COTIZADO = (SELECT SUM(SUBTOTAL_COTIZADO) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "')," +
                    " IEPS_COTIZADO = (SELECT SUM(IEPS_COTIZADO) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "')," +
                    " IVA_COTIZADO = (SELECT SUM(IVA_COTIZADO) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "')," +
                    " TOTAL_COTIZADO = (SELECT SUM(TOTAL_COTIZADO) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "'), " +
                    
                    " SUBTOTAL = (SELECT SUM(MONTO) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "')," +
                    " IEPS = (SELECT SUM(MONTO_IEPS) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "')," +
                    " IVA = (SELECT SUM(MONTO_IVA) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "')," +
                    " TOTAL = (SELECT SUM(MONTO_TOTAL) FROM OPE_COM_REQ_PRODUCTO WHERE NO_REQUISICION = " + Consecutivo_RQ + " AND PROVEEDOR_ID = '" + Dr_Proveedor[Ope_Com_Req_Producto.Campo_Proveedor_ID].ToString() + "'), " +
                    " REQ_ORIGEN_ID = " + No_Requisicion + 
                    " WHERE NO_REQUISICION = " + Consecutivo_RQ;
                    OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                    Requisiciones_Creadas += "RQ-" + Consecutivo_RQ + ",";
                    Consecutivo_RQ++;                    
                }
                //CANCELO LA REQUISICION ORIGINAL
                Mi_SQL = "UPDATE OPE_COM_REQUISICIONES SET ESTATUS = 'CANCELADA' WHERE NO_REQUISICION = " + No_Requisicion;
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //REGISTRAR HISTORIAL DE LA REQUISICION ORIGINAL
                Registrar_Historial("CANCELADA", No_Requisicion.ToString());

                //PONGO COMENTARIO DE MOTIVO CANCELACION
                Administrar_Requisicion.P_Requisicion_ID = No_Requisicion.ToString();
                Administrar_Requisicion.P_Comentario = "La Requisición se canceló para crear las nuevas requisiciones " + Requisiciones_Creadas + " por separación de productos";
                Administrar_Requisicion.P_Estatus = "CONFIRMADA";
                Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToString();
                Administrar_Requisicion.Alta_Observaciones();

                //Transaccion_SQL.Commit();
            }
            catch (OracleException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
               // Dt_Orden_Compra = null;
                throw new Exception("Información: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
               // Dt_Orden_Compra = null;
                throw new Exception("Los datos fueron actualizados por otro Usuario. Información: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
              //  Dt_Orden_Compra = null;
                throw new Exception("Información: " + Ex.Message);
            }
            finally
            {
                Conexion_Base.Close();
            } 

            return Requisiciones_Creadas;
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Requisiciones
        ///DESCRIPCIÓN          : Consulta para obtener las ordenes de compreas
        ///PARAMETROS           1 Orden_Compra_Negocio: Conexion con la capa de negocios
        ///CREO                 : Gustavo Angeles Cruz
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        public static DataTable Consultar_Requisiciones_Reporte_Gerencial(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + " AS NO_REQUISICION,");
                //Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ", ");
                //Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ");
                Mi_Sql.Append(Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + ", ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE, ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Descripcion + " AS FUENTE_FINANCIAMIENTO, ");
                Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_ESPECIFICA, ");
                Mi_Sql.Append(Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " AS CONCEPTO, ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " AS COTIZADOR, ");

                Mi_Sql.Append(Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + ", ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'EN CONSTRUCCION' " +
                             " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_CONSTRUCCION, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'AUTORIZADA' " +
                             " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_AUTORIZADA, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'CANCELADA' " +
                             " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_CANCELADA, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'FILTRADA' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_FILTRADA, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'COTIZADA' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_COTIZADA, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'COMPRA' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_COMPRA, ");

                Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ");                
                Mi_Sql.Append(Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Proveedor + ", ");

                Mi_Sql.Append(" CASE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo);
                Mi_Sql.Append(" WHEN 'TRANSITORIA' THEN ");
                    Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total);
                Mi_Sql.Append(" WHEN 'STOCK' THEN ");
                    Mi_Sql.Append(Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Total + " END AS TOTALES, ");



                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'SURTIDA' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_SURTIDA, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'ALMACEN' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_ALMACEN, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'CERRADA' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_CERRADA, ");

                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'COMPLETA' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_COMPLETA, ");

                Mi_Sql.Append(" '' AS TOTAL, '' AS OBSERVACIONES ");
                Mi_Sql.Append(" FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                //Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra);
                //Mi_Sql.Append(" = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra);
                Mi_Sql.Append(" = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra);

                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID);
                Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Partida_ID);
                Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Cotizador_ID);
                Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID);
                Mi_Sql.Append(" = (SELECT MAX(" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ")");
                Mi_Sql.Append(" FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ")");
                Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " IN ('TRANSITORIA','STOCK') ");
                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Fecha_Inicial) && !String.IsNullOrEmpty(Requisicion_Negocio.P_Fecha_Final))
                {
                    Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo);
                    Mi_Sql.Append(" BETWEEN TO_DATE ('" + Requisicion_Negocio.P_Fecha_Inicial + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                    Mi_Sql.Append(" AND TO_DATE('" + Requisicion_Negocio.P_Fecha_Final + "23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                }

                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Estatus))
                {
                    Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus);
                    Mi_Sql.Append(" = '" + Requisicion_Negocio.P_Estatus + "'");
                }
                Mi_Sql.Append(" ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las ordenes de compras. Error: [" + Ex.Message + "]");
            }
        }


        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Requisiciones
        ///DESCRIPCIÓN          : Consulta para obtener las ordenes de compreas
        ///PARAMETROS           1 Orden_Compra_Negocio: Conexion con la capa de negocios
        ///CREO                 : Gustavo Angeles Cruz
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        public static DataTable Consultar_Requisiciones_Entrega_Bienes_Transitorios(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT  " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + " AS NO_REQUISICION,");
                
                Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ");
                Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + " AS TOTALES, ");
                Mi_Sql.Append(" '' AS TOTAL, ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE, ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");                
                Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_ESPECIFICA, ");
                Mi_Sql.Append(Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " AS CONCEPTO, ");
                Mi_Sql.Append(Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + ", ");
                

                Mi_Sql.Append(" " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo + " AS FECHA_CREO");
                Mi_Sql.Append(", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Autorizacion + " AS FECHA_AUTORIZACION");
                //Mi_Sql.Append(", " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Surtido + " AS FECHA_ENTREGA_MATERIAL");
                Mi_Sql.Append(", ( SELECT " + Ope_Com_Facturas_Proveedores.Campo_Fecha_Creo + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores);
                Mi_Sql.Append(" WHERE NO_CONTRA_RECIBO=" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_Sql.Append(".NO_CONTRA_RECIBO) AS FECHA_CONTRARECIBO, ");
                Mi_Sql.Append("( SELECT " + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores);
                Mi_Sql.Append(" WHERE NO_CONTRA_RECIBO=" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_Sql.Append(".NO_CONTRA_RECIBO) AS FECHA_PAGO, ");
                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus IN ('SURTIDA','SURTIDA / EN ALMACÉN')" +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_SURTIDA, ");
                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus IN ('SURTIDA','SURTIDA / EN ALMACÉN','AVISO A DEPENDENCIA')" +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_AVISO_DEPENDENCIAS, ");

                Mi_Sql.Append(" (SELECT MIN(NO_SALIDA) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS SALIDA, ");
                Mi_Sql.Append(" (SELECT FECHA_CREO FROM ALM_COM_SALIDAS WHERE NO_SALIDA = " +
                              "(SELECT MIN(NO_SALIDA) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = ope_com_requisiciones.no_requisicion)" +
                              ") AS FECHA_SALIDA ");
                
                Mi_Sql.Append(" FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra);
                Mi_Sql.Append(" = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra);

                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID);
                Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Partida_ID);
                Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Cotizador_ID);
                Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID);
                Mi_Sql.Append(" = (SELECT MAX(" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ")");
                Mi_Sql.Append(" FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ")");
                Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'TRANSITORIA' ");
                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Fecha_Inicial) && !String.IsNullOrEmpty(Requisicion_Negocio.P_Fecha_Final))
                {
                    Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo);
                    Mi_Sql.Append(" BETWEEN TO_DATE ('" + Requisicion_Negocio.P_Fecha_Inicial + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                    Mi_Sql.Append(" AND TO_DATE('" + Requisicion_Negocio.P_Fecha_Final + "23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                }

                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Estatus))
                {
                    Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus);
                    Mi_Sql.Append(" = '" + Requisicion_Negocio.P_Estatus + "'");
                }
                Mi_Sql.Append(" ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las ordenes de compras. Error: [" + Ex.Message + "]");
            }
        }

        ///*****************************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Requisiciones
        ///DESCRIPCIÓN          : Consulta para obtener las ordenes de compreas
        ///PARAMETROS           1 Orden_Compra_Negocio: Conexion con la capa de negocios
        ///CREO                 : Gustavo Angeles Cruz
        ///FECHA_CREO           : 28/Diciembre/2011
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*****************************************************************************************************************
        public static DataTable Consultar_Requisiciones_Entrega_Stock(Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                //OBTENEMOS LAS DEPENDENCIAS DEL CATALOGO
                Mi_Sql.Append("SELECT " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Folio + " AS NO_REQUISICION,");
                //Mi_Sql.Append(Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ");
                Mi_Sql.Append(Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Total + " AS TOTALES, ");
                Mi_Sql.Append(" '' AS TOTAL, ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS UNIDAD_RESPONSABLE, ");
                Mi_Sql.Append(Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Clave + " || ' ' || ");
                Mi_Sql.Append(Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Nombre + " AS PARTIDA_ESPECIFICA, ");
                Mi_Sql.Append(Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " AS CONCEPTO, ");
                Mi_Sql.Append(Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus + ", ");
                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus = 'ALMACEN' " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_LLEGO_ALMACEN, ");
                Mi_Sql.Append(" (SELECT MAX(fecha_creo) FROM ope_com_historial_req WHERE estatus IN ('ALMACEN','AVISO A DEPENDENCIA') " +
                              " AND NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS FECHA_AVISO_A_DEPENDENCIA, ");

                Mi_Sql.Append(" (SELECT MIN(NO_SALIDA) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = ope_com_requisiciones.no_requisicion) AS SALIDA, ");
                Mi_Sql.Append(" (SELECT FECHA_CREO FROM ALM_COM_SALIDAS WHERE NO_SALIDA = " +
                              "(SELECT MIN(NO_SALIDA) FROM ALM_COM_SALIDAS WHERE NO_REQUISICION = ope_com_requisiciones.no_requisicion)" +
                              ") AS FECHA_SALIDA ");


                Mi_Sql.Append(" FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_No_Orden_Compra);
                Mi_Sql.Append(" = " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra);

                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Dependencia_ID);
                Mi_Sql.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento);
                Mi_Sql.Append(" ON " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" = " + Cat_SAP_Fuente_Financiamiento.Tabla_Cat_SAP_Fuente_Financiamiento + "." + Cat_SAP_Fuente_Financiamiento.Campo_Fuente_Financiamiento_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Partida_ID);
                Mi_Sql.Append(" = " + Cat_Sap_Partidas_Especificas.Tabla_Cat_SAP_Partidas_Especificas + "." + Cat_Sap_Partidas_Especificas.Campo_Partida_ID);
                Mi_Sql.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append(" ON " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Cotizador_ID);
                Mi_Sql.Append(" = " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID);
                Mi_Sql.Append(" = (SELECT MAX(" + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Ope_Com_Req_Producto_ID + ")");
                Mi_Sql.Append(" FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
                Mi_Sql.Append(" WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Requisicion_ID);
                Mi_Sql.Append(" = " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + ")");
                Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Tipo + " = 'STOCK' ");
                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Fecha_Inicial) && !String.IsNullOrEmpty(Requisicion_Negocio.P_Fecha_Final))
                {
                    Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Fecha_Creo);
                    Mi_Sql.Append(" BETWEEN TO_DATE ('" + Requisicion_Negocio.P_Fecha_Inicial + " 00:00:00', 'DD/MM/YYYY HH24:MI:SS')");
                    Mi_Sql.Append(" AND TO_DATE('" + Requisicion_Negocio.P_Fecha_Final + "23:59:00', 'DD/MM/YYYY HH24:MI:SS')");
                }

                if (!String.IsNullOrEmpty(Requisicion_Negocio.P_Estatus))
                {
                    Mi_Sql.Append(" AND " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Estatus);
                    Mi_Sql.Append(" = '" + Requisicion_Negocio.P_Estatus + "'");
                }
                Mi_Sql.Append(" ORDER BY " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + "." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros de las ordenes de compras. Error: [" + Ex.Message + "]");
            }
        }

        public static void Cancelar_RQ_Stock_Por_Cierre_Mensual()
        {
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            DataTable Dt_Requisiciones = null;
            if (dias <= 10)
            {
                dias = dias * -1;
                dias++;
                _DateTime = _DateTime.AddDays(dias);
                String Fecha = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
                String Mi_SQL = "SELECT " + Ope_Com_Requisiciones.Campo_Requisicion_ID + ", " + Ope_Com_Requisiciones.Campo_Tipo + ", " + Ope_Com_Requisiciones.Campo_Fecha_Creo +
                    " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " WHERE " + Ope_Com_Requisiciones.Campo_Fecha_Creo + " < " +
                    " TO_DATE('" + Fecha + "00:00:00', 'DD/MM/YYYY HH24:MI:SS') AND ESTATUS IN ('EN CONSTRUCCION','GENERADA','AUTORIZADA','RECHAZADA') AND TIPO = 'STOCK'";
                try
                {
                    Dt_Requisiciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                }
                catch(Exception Ex)
                {
                    throw new Exception(Ex.ToString());
                }
                String Mensaje = "";
                foreach (DataRow RQ in Dt_Requisiciones.Rows)
                {
                    Mensaje = Cancelar_Requisicion_Stock_Transitoria(RQ["NO_REQUISICION"].ToString(), RQ["TIPO"].ToString(), "SISTEMA", "Cancelada por cierre de mes");
                }
            }
        } 

        #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Grupo_Dependencia
        ///DESCRIPCIÓN          : consulta para obtener los datos del grupo dependencia
        ///PARAMETROS           1 Negocio:. conexion con la capa de negocios 
        ///CREO                 : Leslie Gonzalez Vázquez
        ///FECHA_CREO           : 17/Enero/2013
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        internal static DataTable Consultar_Grupo_Dependencia(Cls_Ope_Com_Requisiciones_Negocio Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID + ", ");
                Mi_Sql.Append(Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Clave);
                Mi_Sql.Append(" FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" INNER JOIN " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias);
                Mi_Sql.Append(" ON " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID);
                Mi_Sql.Append(" = " + Cat_Grupos_Dependencias.Tabla_Cat_Grupos_Dependencias + "." + Cat_Grupos_Dependencias.Campo_Grupo_Dependencia_ID);
                Mi_Sql.Append(" WHERE " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID);
                Mi_Sql.Append(" = '" + Negocio.P_Dependencia_ID.Trim() + "'");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar el grupo dependencia. Error: [" + Ex.Message + "]");
            }
        }
    }
}
