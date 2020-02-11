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
using Presidencia.Almacen_Elaborar_Contrarecibo.Negocio;
using System.Collections;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Generar_Requisicion.Negocio;
using Presidencia.Generar_Requisicion.Datos;
using Presidencia.Stock;
/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos
/// </summary>
/// 
namespace Presidencia.Almacen_Elaborar_Contrarecibo.Datos
{
    public class Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos
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
        public static DataTable Consulta_Ordenes_Compra(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", "; 
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, "; 
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " AS FECHA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ", "; 
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + "(select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS NO_REQUISICION, ";

                Mi_SQL = Mi_SQL + "(select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Listado_Almacen+ " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") LISTADO_ALMACEN, ";

                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Tipo_Articulo + "";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'SURTIDA'";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " is null "; // Cuando no tiene asignado un numero de contra recibo,

                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
                }

                if (Datos.P_No_Requisicion!= null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " like '%" + Datos.P_No_Requisicion+ "%'";
                }

                if (Datos.P_Proveedor_ID != null)
                {
                    Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";
                }

                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                  " AND '" + Datos.P_Fecha_Fin_B + "'";
                }

                Mi_SQL = Mi_SQL + " order by " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra; 

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
        /// NOMBRE DE LA CLASE:     Consulta_Documentos_Soporte
        /// DESCRIPCION:            Método utilizado para consultar los Documentos de Soporte de la tabla "CAT_COM_DOCUMENTOS"
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Documentos_Soporte(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            DataTable Dt_Doc_Soporte = new DataTable();

            String Mi_SQL = null;
            DataSet Ds_Documentos_S = null;

            Mi_SQL = " SELECT " + "DOCUMENTOS_S." + Cat_Com_Documentos.Campo_Documento_ID+ "";
            Mi_SQL = Mi_SQL + ", DOCUMENTOS_S." + Cat_Com_Documentos.Campo_Nombre+ "";
            Mi_SQL = Mi_SQL + ", DOCUMENTOS_S." + Cat_Com_Documentos.Campo_Comentarios + " as DESCRIPCION";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + " DOCUMENTOS_S";
            Mi_SQL = Mi_SQL + " WHERE  DOCUMENTOS_S." + Cat_Com_Documentos.Campo_Tipo + " = '" + "SOPORTE'";

            if ((Datos.P_Documento_ID != null))
            {
                Mi_SQL = Mi_SQL + " AND  DOCUMENTOS_S." + Cat_Com_Documentos.Campo_Documento_ID + "= '" + Datos.P_Documento_ID +"'";
            }

            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Documentos.Campo_Nombre;

            Ds_Documentos_S = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Dt_Doc_Soporte = Ds_Documentos_S.Tables[0];
           
            return Dt_Doc_Soporte;
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Proveedores
        /// DESCRIPCION:            Consultar los datos de los proveedores
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para la busqueda
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Proveedores(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //variable apra las consultas

            try
            {
                //Consulta
                Mi_SQL = " SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Compañia + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " and " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'SURTIDA' ";
                Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Compañia; // Ordenamiento

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


        ///******************************************************************************* // Este se ocupa en la clase "ContraREcibo_Datos"
        /// NOMBRE DE LA CLASE:     Guardar_Contra_Recibo
        /// DESCRIPCION:            Se guardaa la información que forma parte del contra recibo
        /// PARAMETROS :                                 
        /// CREO       :            Salvador Hernández Ramírez  
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static Int64 Guardar_Contra_Recibo(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            //Declaracion de variables
            String Mensaje = "";
            String Mi_SQL = String.Empty;
            Object Aux, No_Contrarecibo, Marbete, Factura, Registro;
            Int64 No_Marbete, Factura_ID, Registro_ID;
            Double Importe_Total_Facturas =0;
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
       
              // SE GUARDA EL CONTRARECIBO
                //Asignar consulta para el maximo No_Contrarecibo
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "), 0) "; // Este es el NO_CONTRA_RECIBO
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores;

                //Ejecutar consulta
                No_Contrarecibo = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (No_Contrarecibo != null && Convert.IsDBNull(No_Contrarecibo) == false)
                    Datos.P_No_Contra_Recibo = Convert.ToInt64(No_Contrarecibo) + 1;
                else
                    Datos.P_No_Contra_Recibo = 1;

                //Asignar consulta para ingresar la factura
                Mi_SQL = "INSERT INTO " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " (";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + ", "; // Es el No de  contra recibo
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_SubTotal_Sin_Impuesto + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_IVA + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Total + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Comentarios + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Fecha_Creo + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Campo_Empleado_Almacen_ID + ") ";
                Mi_SQL = Mi_SQL + "VALUES(" + Datos.P_No_Contra_Recibo + ", '"; 
                Mi_SQL = Mi_SQL + Datos.P_No_Factura_Proveedor + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Proveedor_ID + "', ";
                Mi_SQL = Mi_SQL + "'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Factura)) + "', ";//
                Mi_SQL = Mi_SQL + " SYSDATE, ";
                Mi_SQL = Mi_SQL + "'" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Fecha_Pago)) + "', ";
                Mi_SQL = Mi_SQL + Datos.P_SubTotal.ToString().Trim() + ", ";
                Mi_SQL = Mi_SQL + Datos.P_IVA.ToString().Trim() + ", ";
                Mi_SQL = Mi_SQL + Datos.P_Total.ToString().Trim() + ", '";
                Mi_SQL = Mi_SQL + Datos.P_Observaciones.ToString().Trim() + "', '";
                Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo.ToString().Trim() + "', ";
                Mi_SQL = Mi_SQL + " SYSDATE, '";
                Mi_SQL = Mi_SQL + Datos.P_Empleado_Almacen_ID + "')";

                //String No_Factura = Convert.ToString(Datos.P_No_Factura_Interno.ToString());
                // Se da de alta la operación en el método "Alta_Bitacora"
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Alm_Elaborar_Contrarecibo.aspx", No_Factura, Mi_SQL);

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación  


    // SE ACTUALIZA LA ORDEN DE COMPRA (Se le asigna su numero de factura interno, registrada, fecha y usuario modificó)
                //Consulta para colocar el numero de factura a la orden de compra
                Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ";
                Mi_SQL = Mi_SQL + "SET " + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo + "', ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_Fecha_Modifico + " = SYSDATE ";

                if (Datos.P_Tipo_Orden_Compra.Trim() == "LISTADO_ALMACEN") // Si es un listado de almacén
                    Mi_SQL = Mi_SQL +  " , " +Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'LIS_ALM_SURTIDO' ";

                  Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim() + "";

                //Ejecutar consulta
                Cmd.CommandText = Mi_SQL;
                Cmd.ExecuteNonQuery(); // Se ejecuta la operación 

                if (Datos.P_Tipo_Articulo.Trim() == "SERVICIO")   // Si es una requisicion de servicios
                { 
                    Mi_SQL = "UPDATE " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " ";
                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Requisiciones.Campo_Estatus + " = 'SERVICIOS_SURTIDA' ";
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim() + "";
                    
                    //Ejecutar consulta
                    Cmd.CommandText = Mi_SQL;
                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 


                    // Se consulta el Numero de Requisición
                    Mi_SQL = "SELECT distinct " + Ope_Com_Requisiciones.Campo_Requisicion_ID ;
                    Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim() + "";

                    //Ejecutar consulta
                    Object Req = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                    //Verificar si no es nulo
                    if (Req != null && Convert.IsDBNull(Req) == false)
                        Datos.P_No_Requisicion = Convert.ToInt32(Req).ToString();//Convert.ToString(Req) + 1;

                    // Se Guarda el Historial de la requisición
                    Cls_Ope_Com_Requisiciones_Negocio Requisiciones = new Cls_Ope_Com_Requisiciones_Negocio();
                    //Requisiciones.Registrar_Historial("SERVICIOS_SURTIDA", Datos.P_No_Requisicion.ToString().Trim());
                }


                // SE GUARDAN LOS DOCUMENTOS SOPORTE
                if (Datos.P_Dt_Documentos_Soporte != null)
                {
                        //Asignar consulta para el maximo Marbete de la tabla OPE_COM_DET_DOC_SOPORTE
                        Mi_SQL = "SELECT NVL(MAX(" + Ope_Com_Det_Doc_Soporte.Campo_Marbete + "), 0) ";
                        Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte;

                        //Ejecutar consulta
                        Marbete = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //Verificar si no es nulo
                        if (Marbete != null && Convert.IsDBNull(Marbete) == false)
                            No_Marbete = Convert.ToInt64(Marbete) + 1;
                        else
                            No_Marbete = 1;

                    //Ciclo para colocar el numero de factura a la orden de compra
                    for (int Cont = 0; Cont < Datos.P_Dt_Documentos_Soporte.Rows.Count; Cont++)
                    {
                        //Asignar consulta para ingresar la factura
                        Mi_SQL = "INSERT INTO " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + " (";
                        Mi_SQL = Mi_SQL + Ope_Com_Det_Doc_Soporte.Campo_Documento_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Det_Doc_Soporte.Campo_No_Factura_Interno + ", "; // Este es el No_Contra_Recibo
                        Mi_SQL = Mi_SQL + Ope_Com_Det_Doc_Soporte.Campo_Usuario_Creo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Det_Doc_Soporte.Campo_Fecha_Creo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Com_Det_Doc_Soporte.Campo_Marbete + ") ";
                        Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Dt_Documentos_Soporte.Rows[Cont]["DOCUMENTO_ID"].ToString().Trim() +"', '";
                        Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + "', '";
                        Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', ";
                        Mi_SQL = Mi_SQL + " SYSDATE, ";
                        Mi_SQL = Mi_SQL + No_Marbete + ")";
                        // String Agregar_Marbete = Convert.ToString(No_Marbete);
                        // Se da de alta la operación en el método "Alta_Bitacora"
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Alm_Elaborar_Contrarecibo.aspx", Agregar_Marbete, Mi_SQL);
                        //Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery(); // Se ejecuta la operación
                        No_Marbete = No_Marbete + 1; // Se incrementa
                    }
                }

                // SE GUARDAN LAS FACTURAS
                if (Datos.P_Dt_Facturas_Proveedor != null)
                {
                        //Asignar consulta para el maximo Marbete de la tabla OPE_COM_DET_DOC_SOPORTE
                        Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Registro_Facturas.Campo_Factura_ID + "), 0) ";
                        Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Registro_Facturas.Tabla_Ope_Alm_Registro_Facturas;

                        //Ejecutar consulta
                        Factura = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                        //Verificar si no es nulo
                        if (Factura != null && Convert.IsDBNull(Factura) == false)
                            Factura_ID = Convert.ToInt64(Factura) + 1;
                        else
                            Factura_ID = 1;

                    //Ciclo para colocar los numeros de factura
                    for (int Cont = 0; Cont < Datos.P_Dt_Facturas_Proveedor.Rows.Count; Cont++)
                    {
                        //Asignar consulta para ingresar la factura
                        Mi_SQL = "INSERT INTO " + Ope_Alm_Registro_Facturas.Tabla_Ope_Alm_Registro_Facturas + " (";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Factura_ID + ", ";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Factura_Proveedor + ", ";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_No_Contra_Recibo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Importe_Factura + ", ";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Fecha_Factura + ", ";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Usuario_Creo + ", ";
                        Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Fecha_Creo + ") ";
                        Mi_SQL = Mi_SQL + "VALUES(" + Factura_ID + ", '";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Facturas_Proveedor.Rows[Cont]["NO_FACTURA_PROVEEDOR"].ToString().Trim() + "', ";
                        Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + ", ";
                        Mi_SQL = Mi_SQL + Datos.P_Dt_Facturas_Proveedor.Rows[Cont]["IMPORTE_FACTURA"].ToString().Trim() + ", '";
                        Mi_SQL = Mi_SQL + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Datos.P_Dt_Facturas_Proveedor.Rows[Cont]["FECHA_FACTURA"].ToString().Trim())) + "', '";
                        Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', ";
                        Mi_SQL = Mi_SQL + " SYSDATE )";

                        //string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime())
                        // Se calcula el importe total de las facturas, dato utilizado por si se va a diminuir el monto de las facturas o se disminulle el monto por cada producto
                        Importe_Total_Facturas= Importe_Total_Facturas + Convert.ToDouble(Datos.P_Dt_Facturas_Proveedor.Rows[Cont]["IMPORTE_FACTURA"].ToString().Trim());

                        //String Agregar_Marbete = Convert.ToString(No_Marbete);
                        // Se da de alta la operación en el método "Alta_Bitacora"
                        //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Alm_Elaborar_Contrarecibo.aspx", Agregar_Marbete, Mi_SQL);

                        //Ejecutar consulta
                        Cmd.CommandText = Mi_SQL;
                        Cmd.ExecuteNonQuery(); // Se ejecuta la operación 
                        Factura_ID = Factura_ID + 1;
                    }
                }

                if (Datos.P_Tipo_Orden_Compra.Trim() == "TRANSITORIA") // SI NO ES UNA REQUISICION DE LISTADO DE STOCK
                {
                       if (Datos.P_Tipo_Articulo.Trim() != "SERVICIO")   // Si no es una requisicion de servicios y es transitoria, se  agegan los productos a la tabla
                        {
                            // SE GUARDAN LOS PRODUCTOS DEL CONTRA RECIBO
                            if (Datos.P_Dt_Productos_OC != null)
                            {
                                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Productos_Contrarecibo.Campo_No_Registro + "), 0) ";
                                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo;
                                
                                //Ejecutar consulta
                                Registro = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                                //Verificar si no es nulo
                                if (Registro != null && Convert.IsDBNull(Registro) == false)
                                    Registro_ID = Convert.ToInt64(Registro) + 1;
                                else
                                    Registro_ID = 1;

                                //Ciclo para colocar los numeros de factura
                                for (int Cont = 0; Cont < Datos.P_Dt_Productos_OC.Rows.Count; Cont++)
                                {
                                    // Asignar consulta para ingresar la factura
                                    Mi_SQL = "INSERT INTO " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " (";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_No_Registro + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Unidad + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Recibo_Transitorio + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Usuario_Creo + ", ";
                                    Mi_SQL = Mi_SQL + Ope_Alm_Productos_Contrarecibo.Campo_Fecha_Creo + ") ";
                                    Mi_SQL = Mi_SQL + "VALUES(" + Registro_ID + ", ";
                                    Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo + ", '";
                                    Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_OC.Rows[Cont]["PRODUCTO_ID"].ToString().Trim() + "', '";
                                    Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_OC.Rows[Cont]["RESGUARDO"].ToString().Trim() + "', '";
                                    Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_OC.Rows[Cont]["RECIBO"].ToString().Trim() + "', '";
                                    Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_OC.Rows[Cont]["UNIDAD"].ToString().Trim() + "', '";
                                    Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_OC.Rows[Cont]["TOTALIDAD"].ToString().Trim() + "', '";
                                    Mi_SQL = Mi_SQL + Datos.P_Dt_Productos_OC.Rows[Cont]["RECIBO_TRANSITORIO"].ToString().Trim() + "', '";
                                    Mi_SQL = Mi_SQL + Datos.P_Usuario_Creo + "', ";
                                    Mi_SQL = Mi_SQL + " SYSDATE)";

                                    //String Agregar_Marbete = Convert.ToString(No_Marbete);
                                    // Se da de alta la operación en el método "Alta_Bitacora"
                                    //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Alm_Elaborar_Contrarecibo.aspx", Agregar_Marbete, Mi_SQL);

                                    //Ejecutar consulta
                                    Cmd.CommandText = Mi_SQL;
                                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 
                                    Registro_ID = Registro_ID + 1; // Se incrementa

                                    // Se consulta el 
                                    String Recibo = "";
                                    String Resguardo = "";
                                    String Resguardado = "";

                                    Resguardo = "" + Datos.P_Dt_Productos_OC.Rows[Cont]["RESGUARDO"].ToString().Trim();
                                    Recibo = "" + Datos.P_Dt_Productos_OC.Rows[Cont]["RECIBO"].ToString().Trim();

                                    if ((Resguardo == "SI") | (Recibo == "SI"))
                                        Resguardado = "SI";
                                    else
                                        Resguardado = "NO";

                                    Mi_SQL = " UPDATE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                                    Mi_SQL = Mi_SQL + " SET " + Ope_Com_Req_Producto.Campo_Resguardado + " ='" + Resguardado + "'";
                                    Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim();
                                    Mi_SQL = Mi_SQL + " and " + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = '" + Datos.P_Dt_Productos_OC.Rows[Cont]["PRODUCTO_ID"].ToString().Trim() + "'";

                                    //Ejecutar consulta
                                    Cmd.CommandText = Mi_SQL;
                                    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 
                                }
                            }
                        }

                       
                        // Se realiza el recorrido de la tabla para actualizar su precio unitario de los productos
                        for( int j=0; j< Datos.P_Dt_Productos_OC.Rows.Count; j++){

                            DataTable Dt_Aux_Precio = new DataTable(); //  Tabla que contendra los productos que se deben actualizar
                            Double Precio_Actualizado = 0;

                            // Consulta para obtener los  montos 
                            Mi_SQL = "SELECT  " + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as PRECIO_PRODUCTO";
                            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto;
                            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra;
                            Mi_SQL = Mi_SQL + " = '" + Datos.P_No_Orden_Compra.Trim() + "'";
                            Mi_SQL = Mi_SQL + " and " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
                            Mi_SQL = Mi_SQL + " = '" + Datos.P_Dt_Productos_OC.Rows[j]["PRODUCTO_ID"].ToString().Trim() + "'";

                            Dt_Aux_Precio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                            // Verificar si es nulL
                            if (Convert.IsDBNull(Dt_Aux_Precio.Rows[0]["PRECIO_PRODUCTO"]) == false)
                                Precio_Actualizado = Convert.ToDouble(Dt_Aux_Precio.Rows[0]["PRECIO_PRODUCTO"].ToString().Trim());


                            if (Datos.P_Tipo_Articulo.Trim() == "SERVICIO")   // Si  es una requisicion de servicios
                            {
                                //  Actualizar el precio de la tabla  CAT_COM_SERVICIOS
                                Mi_SQL = " UPDATE " + Cat_Com_Servicios.Tabla_Cat_Com_Servicios + " ";
                                Mi_SQL = Mi_SQL + " SET " + Cat_Com_Servicios.Campo_Costo + " = " + Precio_Actualizado + " ";
                                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Servicios.Campo_Servicio_ID + " = '" + Datos.P_Dt_Productos_OC.Rows[j]["PRODUCTO_ID"].ToString().Trim() + "'";
                            }
                            else  //  Entonces es una requisición de productos
                            {
                                //  Actualizar el precio de la tabla CAT_COM_PRODUCTOS
                                Mi_SQL = " UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                                Mi_SQL = Mi_SQL + " SET " + Cat_Com_Productos.Campo_Costo + " = " + Precio_Actualizado + ", ";
                                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo_Promedio + " = " + Precio_Actualizado + " ";
                                Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Datos.P_Dt_Productos_OC.Rows[j]["PRODUCTO_ID"].ToString().Trim() + "'";
                            }
                            //Ejecutar consulta
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery(); // Se ejecuta la operación 
                        }
                    

                            // SE ACTUALIZAN LOS MONTOS
                            DataTable Dt_Aux = new DataTable(); //  Tabla que contendra los productos que se deben actualizar
                            String No_Requsicion = "";
                            String Proyecto_Programa_ID = "";
                            String Dependencia_ID = "";
                            String Partida_ID = "";
                            String No_Asignacion = "";
                            Double Monto_Ejercido = 0;
                            Double Monto_Comprometido = 0;

                            Proyecto_Programa_ID = Datos.P_Proyecto_Programa_ID.Trim();
                            Dependencia_ID = Datos.P_Dependencia_ID.Trim();
                            Partida_ID = Datos.P_Partida_ID.Trim();
                            //@@
                            //**************************************************************
                            // Consulta para obtener el mayor numero de asignación
                            //Mi_SQL = "SELECT  MAX (" + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + ") as NO_ASIGNACION_ANIO ";
                            //Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                            //Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                            //Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                            //Mi_SQL = Mi_SQL + " = '" + Partida_ID + "'";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                            //Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                            //Mi_SQL = Mi_SQL + " = extract(year from sysdate)";

                            //Dt_Aux = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                            //// Verificar si es nulo
                            //if (Convert.IsDBNull(Dt_Aux.Rows[0]["NO_ASIGNACION_ANIO"]) == false)
                            //    No_Asignacion = Dt_Aux.Rows[0]["NO_ASIGNACION_ANIO"].ToString().Trim();

                            //// Consulta para obtener los  montos 
                            //Mi_SQL = "SELECT  " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + ", ";
                            //Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible + ", ";
                            //Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + "";
                            //Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;
                            //Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID;
                            //Mi_SQL = Mi_SQL + " = '" + Dependencia_ID + "'";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Partida_ID;
                            //Mi_SQL = Mi_SQL + " = '" + Partida_ID + "'";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID;
                            //Mi_SQL = Mi_SQL + " = '" + Proyecto_Programa_ID + "'";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto;
                            //Mi_SQL = Mi_SQL + " = extract(year from sysdate)";
                            //Mi_SQL = Mi_SQL + " and " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + "." + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio;
                            //Mi_SQL = Mi_SQL + " = " + No_Asignacion;

                            ////Ejecutar consulta
                            //DataTable Dt_Aux_Presupuestos = new DataTable();
                            //Dt_Aux_Presupuestos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                            //// Verificar si la consulta tiene elementos
                            //if (Dt_Aux_Presupuestos.Rows.Count > 0)
                            //{
                            //    if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_EJERCIDO"]) != false) // Si no tiene un monto ejercido entra
                            //        Monto_Ejercido = Importe_Total_Facturas; // Obtener el nuevo monto ejercido 
                            //    else
                            //        Monto_Ejercido = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_EJERCIDO"]) + Importe_Total_Facturas;// Obtener el  monto ejercido y lo suma al monto Total del producto

                            //    if (Convert.IsDBNull(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) == false)
                            //        Monto_Comprometido = Convert.ToDouble(Dt_Aux_Presupuestos.Rows[0]["MONTO_COMPROMETIDO"]) - Importe_Total_Facturas; // Obtener el  MONTO COMPROMETIDO y le resta el MONTO TOTAL del producto
                            //    else
                            //        Monto_Comprometido = 0;

                            //    // Actualizar la tabla de los presupuestos
                            //    Mi_SQL = " UPDATE " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto + " ";
                            //    Mi_SQL = Mi_SQL + " SET " + Cat_Com_Dep_Presupuesto.Campo_Monto_Ejercido + " = " + Monto_Ejercido + ", ";
                            //    Mi_SQL = Mi_SQL + Cat_Com_Dep_Presupuesto.Campo_Monto_Comprometido + " = " + Monto_Comprometido + " ";
                            //    Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + " = '" + Dependencia_ID + "'";
                            //    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Partida_ID + " = '" + Partida_ID + "'";
                            //    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Proyecto_Programa_ID + " = '" + Proyecto_Programa_ID + "'";
                            //    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_Anio_Presupuesto + " = extract(year from sysdate)";
                            //    Mi_SQL = Mi_SQL + " AND " + Cat_Com_Dep_Presupuesto.Campo_No_Asignacion_Anio + " = " + No_Asignacion;

                            //    //Ejecutar consulta
                            //    Cmd.CommandText = Mi_SQL;
                            //    Cmd.ExecuteNonQuery(); // Se ejecuta la operación 
                            //}
                            //else
                            //{
                            //    // Escribir un mensaje que indica que no se actualizó el presupuesto  
                            //}
                            //**********************************
                }
                else if (Datos.P_Tipo_Orden_Compra.Trim() == "LISTADO_ALMACEN")   // SE AUMENTAN LOS PRODUCTOS en la tabla CAT_COM_PRODUCTOS
                {
                    //int Registros = Cls_Ope_Alm_Stock.Comprometer_Producto(Datos.P_Dt_Actualizar_Productos, "PRODUCTO_ID", "CANTIDAD");

                    DataTable Dt_Productos_Actualzar = new DataTable();    // Se Crea la tabla utilziada para actualizar los productos

                    if (Datos.P_Dt_Actualizar_Productos.Rows.Count > 0) // Si hay productos que deben ser actualziados
                    {
                        Dt_Productos_Actualzar = Datos.P_Dt_Actualizar_Productos;

                        String Producto_ID = "";
                        Int64 Cantidad_Productos = 0;
                        Int64 Existencia = 0;
                        Int64 Disponible = 0;
                        Double Precio_Cotizado = 0;

                        for (int i = 0; i < Dt_Productos_Actualzar.Rows.Count; i++)
                        {

                            if (Dt_Productos_Actualzar.Rows[i]["PRODUCTO_ID"].ToString().Trim() != "")
                                Producto_ID = Dt_Productos_Actualzar.Rows[i]["PRODUCTO_ID"].ToString().Trim();
                            else
                                Producto_ID = "";

                            if (Dt_Productos_Actualzar.Rows[i]["CANTIDAD"].ToString().Trim() != "")
                                Cantidad_Productos = Convert.ToInt64(Dt_Productos_Actualzar.Rows[i]["CANTIDAD"].ToString().Trim());
                            else
                                Cantidad_Productos = 0;

                            if (Dt_Productos_Actualzar.Rows[i]["PRECIO_U"].ToString().Trim() != "")
                                Precio_Cotizado = double.Parse(Dt_Productos_Actualzar.Rows[i]["PRECIO_U"].ToString().Trim());
                            else
                                Precio_Cotizado = 0;

                            DataTable Dt_Existencia_Productos = new DataTable(); // Tabla creada para consultar las existencias y disponible de los prioductos

                            // Se Consulta la existencia  y disponibilidad de cada producto que pertenezca a la orden de compra
                            Mi_SQL = " SELECT  " + Cat_Com_Productos.Campo_Producto_ID;
                            Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Campo_Existencia;
                            Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Campo_Disponible;
                            Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Campo_Costo;
                            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Productos.Tabla_Cat_Com_Productos;
                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID;
                            Mi_SQL = Mi_SQL + " = '" + Producto_ID + "'";

                            Dt_Existencia_Productos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                            if (Convert.IsDBNull(Dt_Existencia_Productos.Rows[0][1]) != false) // Si no hay Existencias
                                Existencia = Cantidad_Productos;
                            else
                                Existencia = Convert.ToInt64(Dt_Existencia_Productos.Rows[0][1]) + Cantidad_Productos;

                            if (Convert.IsDBNull(Dt_Existencia_Productos.Rows[0][2]) != false) // Si no hay Disponible
                                Disponible = Cantidad_Productos;
                            else
                                Disponible = Convert.ToInt64(Dt_Existencia_Productos.Rows[0][2]) + Cantidad_Productos;

                            //  Actualizar la la existencia y el disponible en la tabla de CAT_COM_PRODUCTOS
                            Mi_SQL = " UPDATE " + Cat_Com_Productos.Tabla_Cat_Com_Productos + " ";
                            Mi_SQL = Mi_SQL + " SET " + Cat_Com_Productos.Campo_Existencia + " = " + Existencia + ",  ";
                            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Disponible + " = " + Disponible + ", ";
                            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo + " = " + Precio_Cotizado + ", ";
                            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Costo_Promedio + " = " + Precio_Cotizado;

                            Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = '" + Producto_ID + "'";

                            //Ejecutar consulta
                            Cmd.CommandText = Mi_SQL;
                            Cmd.ExecuteNonQuery(); // Se ejecuta la operación 
                        }
                    }
                }
                Trans.Commit(); // Se ejecuta la transacciones


                // Se consulta el Numero de Requisición
                int Num_RQ = 0;
                Mi_SQL = "SELECT distinct " + Ope_Com_Requisiciones.Campo_Requisicion_ID;
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra.Trim() + "";

                //Ejecutar consulta
                Object Obj = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Verificar si no es nulo
                if (Obj != null && Convert.IsDBNull(Obj) == false)
                    Num_RQ = Convert.ToInt32(Obj);//Convert.ToString(Req) + 1;

                // Se Guarda el Historial de la requisición

                Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("SURTIDA / CONTRARECIBO", Num_RQ.ToString());
                return Convert.ToInt64(No_Contrarecibo) + 1; // Se regresa el No. de Contra recibo
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
        /// NOMBRE DE LA CLASE:     Montos_Orden_Compra
        /// DESCRIPCION:            Se obtienen los montos de la orden de compra seleccionada por el usuario
        /// PARAMETROS :                                 
        /// CREO       :            Salvador Hernández Ramírez  
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :     
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Montos_Orden_Compra(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = String.Empty; //Variable para las consultas
            try
            {
                //Asignar consulta
                Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", " + Ope_Com_Ordenes_Compra.Campo_Subtotal + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + ", " + Ope_Com_Ordenes_Compra.Campo_Total_IVA + ", " + Ope_Com_Ordenes_Compra.Campo_Total + " ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ";
                Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra;

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
        /// NOMBRE DE LA CLASE:     Actualizar_Orden_Compra
        /// DESCRIPCION:            Se actualizan las ordenes de compra al estatus "RECIBIDA", y se resguarda la orden de compra
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los numero de orden de compra
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static void Actualizar_Orden_Compra(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            DataTable DataTable_Temporal = null;
            String Mi_SQL;
            DataTable_Temporal = Datos.P_Dt_Ordenes_Compra;

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
                Mi_SQL = "UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Estatus + "='SURTIDA'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " =" + Datos.P_No_Orden_Compra;

                // Se da de alta la operación en el método "Alta_Bitacora"
                //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Modificar, "Frm_Ope_Alm_Elaborar_Contrarecibo.aspx", Datos.P_No_Orden_Compra, Mi_SQL);

                //Ejecutar consulta
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (Exception Ex)
            {
                String Mensaje = "Error al intentar realizar las  transacción. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
                throw new Exception(Mensaje);
            }
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Datos_Generales_ContraRecibo
        /// DESCRIPCION:            Método utilizado para consultar 
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos para realizar la consulta
        ///                         
        /// CREO       :            Salvador Hernandez Ramirez
        /// FECHA_CREO :            14/Marzo/2011  
        /// MODIFICO          :     
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Datos_Generales_ContraRecibo(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            DataTable Dt_Datos_Generales = new DataTable();
            String Mi_SQL = null;

            // Consulta
            Mi_SQL = "SELECT Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " as No_Contrarecibo";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + " as No_Proveedor";
            Mi_SQL = Mi_SQL + ", Proveedores." + Cat_Com_Proveedores.Campo_Nombre + " as Proveedor";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + " as Fecha_Recepcion";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago + " as Fecha_Pago";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + " as Empleado_Almacen";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Comentarios + " as Observaciones";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_SubTotal_Sin_Impuesto + " as SubTotal";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_IVA + " as IVA";
            Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Total + " as Importe";
            Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Folio + " as Folio";

            Mi_SQL = Mi_SQL + ", (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + "Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS Requisicion  ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " Facturas_P";
            Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " Proveedores";
            Mi_SQL = Mi_SQL + " ON Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " = Proveedores." + Cat_Com_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " Ordenes_Compra";
            Mi_SQL = Mi_SQL + " ON Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno; // Este es el No_Contra_Recibo
            Mi_SQL = Mi_SQL + " = Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;// Este es el No_Contra_Recibo
            Mi_SQL = Mi_SQL + " WHERE  Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo + "";

            Dt_Datos_Generales = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Datos_Generales;
        }


        public static DataTable Consultar_Facturas_ContraRecibo(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = null;
            // Consulta
            Mi_SQL = "SELECT  REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_No_Contra_Recibo  + " No_Contrarecibo ";
            Mi_SQL = Mi_SQL + ", REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_Factura_Proveedor + " as No_Factura_Proveedor "; 
            Mi_SQL = Mi_SQL + ", REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_Fecha_Factura + " as Fecha_Factura "; 
            Mi_SQL = Mi_SQL + ", REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_Importe_Factura + " as Importe "; 
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Registro_Facturas.Tabla_Ope_Alm_Registro_Facturas + " REG_FACTURAS";
            Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURAS_P";
            Mi_SQL = Mi_SQL + " ON FACTURAS_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " REG_FACTURAS." + Ope_Alm_Registro_Facturas.Campo_No_Contra_Recibo+ "";
            Mi_SQL = Mi_SQL + " WHERE  FACTURAS_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo.ToString().Trim() + ""; // Este es el No_Contra_Recibo

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Mostrar_Contrarecibo
        /// DESCRIPCION:            Consultar de la tabla facturas la informaciòn para generar el ContraRecibo
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Documentos_Contrarecibo(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                DataTable Dt_Documentos = new DataTable();
                
                Mi_SQL = "SELECT " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + "." + Cat_Com_Documentos.Campo_Nombre + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + "." + Cat_Com_Documentos.Campo_Comentarios + " ";
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + ", " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + "," + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " ";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " = " + Datos.P_No_Contra_Recibo;
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + "." + Ope_Com_Det_Doc_Soporte.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + "";
                Mi_SQL = Mi_SQL + " AND " + Cat_Com_Documentos.Tabla_Cat_Com_Documentos + "." + Cat_Com_Documentos.Campo_Documento_ID + " ";
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Det_Doc_Soporte.Tabla_Ope_Com_Det_Doc_Soporte + "." + Ope_Com_Det_Doc_Soporte.Campo_Documento_ID + "";

                Dt_Documentos = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
                return Dt_Documentos;

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
        /// NOMBRE DE LA CLASE:     Mostrar_Contrarecibo
        /// DESCRIPCION:            Consultar de la tabla facturas la informaciòn para generar el ContraRecibo
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            16/Marzo/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consultar_Contrarecibo(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                // Consulta
                Mi_SQL = "SELECT " + "Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_ContraRecibo + " as No_Contrarecibo";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID + " as No_Proveedor";
                Mi_SQL = Mi_SQL + ", Proveedores." + Cat_Com_Proveedores.Campo_Compañia + " as Proveedor";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Recepcion + " as Fecha_Recepcion";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Pago + " as Fecha_Pago";
                Mi_SQL = Mi_SQL + ", Empleado." + Cat_Empleados.Campo_Nombre + "  ||' '||";
                Mi_SQL = Mi_SQL + " Empleado." + Cat_Empleados.Campo_Apellido_Paterno + "  ||' '||";
                Mi_SQL = Mi_SQL + " Empleado." + Cat_Empleados.Campo_Apellido_Materno + "  as Empleado_Almacen";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Comentarios + " as Observaciones";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura+ " as Fecha_Factura";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + " as Factura";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Total + " as Importe_Factura"; // Totales de la factura, la cual incluye varias ordenes de compra
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_SubTotal_Con_Impuesto + "";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_SubTotal_Sin_Impuesto + "";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_IVA + " as IVA_Factura";
                Mi_SQL = Mi_SQL + ", Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_IEPS + " as IEPS_Factura";
                Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Folio + " as Orden_Compra"; 
                Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Subtotal + " as Sup_Total_OC"; // Totales y Subtotales de la ordn de compra
                Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Total_IEPS + " as Total_IEPS_OC";
                Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Total_IVA + " as Total_IVA_OC";
                Mi_SQL = Mi_SQL + ", Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_Total + " as Total_OC";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " Facturas_P";
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " Proveedores";
                Mi_SQL = Mi_SQL + " ON Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = Proveedores." + Cat_Com_Proveedores.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " JOIN " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " Ordenes_Compra";
                Mi_SQL = Mi_SQL + " ON Ordenes_Compra." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno; // Este es el No_Contra_Recibo
                Mi_SQL = Mi_SQL + " = Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;// Este es el No_Contra_Recibo
                Mi_SQL = Mi_SQL + " JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " Empleado";
                Mi_SQL = Mi_SQL + " on Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_Usuario_Creo + " ";
                Mi_SQL = Mi_SQL + " = Empleado." + Cat_Empleados.Campo_Empleado_ID;
                Mi_SQL = Mi_SQL + " WHERE  Facturas_P." + Ope_Com_Facturas_Proveedores.Campo_No_ContraRecibo + "= '" + Datos.P_No_Contra_Recibo + "'";

                //  Entregar resultado
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
        /// NOMBRE DE LA CLASE:     Consulta_Montos_Orden_Compra
        /// DESCRIPCION:            Consulta los montos de la orden de compra seleccionada por el usuario
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            02/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Montos_Orden_Compra(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = "";
            
            try
            {
                // Consulta
                Mi_SQL = "SELECT " + " ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Subtotal + " ";
                Mi_SQL = Mi_SQL + ", ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total_IVA + "";
                Mi_SQL = Mi_SQL + ", ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA";
                Mi_SQL = Mi_SQL + " WHERE  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Orden_Compra + "";

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
        /// NOMBRE DE LA CLASE:     Consulta_Productos_Orden_Compra
        /// DESCRIPCION:            Consulta los productos de la orden de compra seleccionada por el usuarioa
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            01/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Productos_Orden_Compra(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                // Consulta
                Mi_SQL = "SELECT  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as PRODUCTO_ID";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Cantidad + "";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as PRECIO_U ";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " as PRECIO_AC ";
                Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "";
                Mi_SQL = Mi_SQL + ", REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + "";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Partida_ID + "";
                Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Proyecto_Programa_ID + "";

                Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Clave + " from ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as CLAVE";

                if (Datos.P_Tipo_Articulo == "PRODUCTO")
                {
                    Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                    Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as NOMBRE";

                    Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
                    Mi_SQL = Mi_SQL + " where REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = PRODUCTOS.";
                    Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION";
                }
                else if (Datos.P_Tipo_Articulo == "SERVICIO")
                {
                    Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Producto_Servicio + " AS  NOMBRE ";
                    Mi_SQL = Mi_SQL + ", REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Nombre_Giro + " AS  DESCRIPCION";
                }

                Mi_SQL = Mi_SQL + ", ( SELECT " + Cat_Com_Unidades.Campo_Abreviatura + " FROM " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " WHERE ";
                Mi_SQL = Mi_SQL + Cat_Com_Unidades.Campo_Unidad_ID + " = ( SELECT " + Cat_Com_Unidades.Campo_Unidad_ID + "  FROM ";
                Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " WHERE " + Cat_Com_Productos.Campo_Producto_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " )) AS UNIDAD ";

                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                
                Mi_SQL = Mi_SQL + " WHERE  REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + "";
                Mi_SQL = Mi_SQL + " AND  REQ_PRODUCTO." + Ope_Com_Requisiciones.Campo_No_Orden_Compra + " = ";
                Mi_SQL = Mi_SQL + Datos.P_No_Orden_Compra;

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
        #endregion

        #region Registro de póliza
        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Generar_Poliza
        ///// DESCRIPCION:            Consultar los datos de los proveedores
        ///// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        /////                         los datos para la busqueda
        ///// CREO       :            Salvador Hernández Ramírez
        ///// FECHA_CREO :            16/Marzo/2011 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        public static bool Generar_Poliza(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {

            String Mi_SQL;                          //Obtiene la cadena de inserción hacía la base de datos
            Object No_Poliza = null;                //Obtiene el No con la cual se guardo los datos en la base de datos
            OracleConnection Conexion_Base = new OracleConnection(Cls_Constantes.Str_Conexion); //Variable para la conexión para la base de datos        
            OracleCommand Comando_SQL = new OracleCommand();                                    //Sirve para la ejecución de las operaciones a la base de datos
            OracleTransaction Transaccion_SQL;                                                  //Sirve para guardar la transacción en memoria hasta que se ejecute completo el proceso        
            String Datos_No_Poliza = "";
            String Tipo_Poliza = "";
            String[] Arr_Fecha = DateTime.Now.ToString("dd/MM/yy").Split('/');
            String Mes_Ano = Arr_Fecha[1] + Arr_Fecha[2];
            bool Operacion_Realizada = false;
            bool Exito = false;
            String Concepto = "";
            //Consultamos el no_factura 
            DataTable Dt_Factura = Consultar_Facturas(Datos);

            if (Conexion_Base.State != ConnectionState.Open)
            {
                Conexion_Base.Open(); //Abre la conexión a la base de datos            
            }
            Transaccion_SQL = Conexion_Base.BeginTransaction(IsolationLevel.ReadCommitted);  //Asigna el espacio de memoria para guardar los datos del proceso de manera temporal
            Comando_SQL.Connection = Conexion_Base;                                          //Establece la conexión a la base de datos
            Comando_SQL.Transaction = Transaccion_SQL;                                       //Abre la transacción para la ejecución en la base de datos

            try
            {
                //Consulta para la obtención del último ID dado de alta en el  catálogo de empleados
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Con_Polizas.Campo_No_Poliza + "),'0000000000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas;

                Comando_SQL.CommandText = Mi_SQL; //Realiza la ejecuón de la obtención del ID del empleado
                No_Poliza = Comando_SQL.ExecuteScalar();

                //Valida si el ID es nulo para asignarle automaticamente el primer registro
                if (Convert.IsDBNull(No_Poliza))
                {
                    Datos_No_Poliza = "0000000001";
                }
                //Si no esta vacio el registro entonces al registro que se obtenga se le suma 1 para poder obtener el último registro
                else
                {
                    Datos_No_Poliza = String.Format("{0:0000000000}", Convert.ToInt32(No_Poliza) + 1);
                }
                //Consulta para la inserción del Empleado con los datos proporcionados por el usuario
                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas.Tabla_Ope_Con_Polizas + " (";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_No_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Tipo_Poliza_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Fecha_Poliza + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Total_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_Total_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas.Campo_No_Partidas + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Usuario_Creo + ", ";
                Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Fecha_Creo + ") VALUES (";
                Mi_SQL = Mi_SQL + "'" + Datos_No_Poliza + "',";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='COMPRAS')";
                Mi_SQL = Mi_SQL + " ,'" + Mes_Ano + "'";
                Mi_SQL = Mi_SQL + ",SYSDATE";
                Mi_SQL = Mi_SQL + ",'FACTURA " + Dt_Factura.Rows[0]["NO_FACTURA_PROVEEDOR"].ToString().Trim();
                Mi_SQL = Mi_SQL + "','" + Datos.P_Total;
                Mi_SQL = Mi_SQL + "','" + Datos.P_Total;
                Mi_SQL = Mi_SQL + "','3','";
                Mi_SQL = Mi_SQL + Cls_Sessiones.Nombre_Empleado + "', SYSDATE)";


                Comando_SQL.CommandText = Mi_SQL; //Asigna la inserción para ser ejecutada
                Comando_SQL.ExecuteNonQuery();    //Ejecuta la inserción en memoria antes de pasarla a la base de datos  
                Transaccion_SQL.Commit(); //Pasa todo el proceso que se encuentra en memoria a la base de datos para ser almacenados los datos
                Concepto = "FACTURA " + Dt_Factura.Rows[0]["NO_FACTURA_PROVEEDOR"].ToString().Trim();
                Exito = true;
            }
            catch (Exception Ex)
            {
                if (Transaccion_SQL != null)
                {
                    Transaccion_SQL.Rollback();
                }
                throw new Exception("Error: " + Ex.Message);

                Operacion_Realizada = false;
            }
            finally
            {
                Conexion_Base.Close();
                if (Exito)
                {
                    Operacion_Realizada = Alta_Detalles_Poliza(Datos, Datos_No_Poliza, Mes_Ano, Tipo_Poliza, Concepto, Dt_Factura);
                }
            }
            return Operacion_Realizada;
        }
        public static bool Alta_Detalles_Poliza(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos, String No_Poliza, String Mes_Ano, String Tipo_Poliza, String Concepto, DataTable Dt)
        {
            String Mi_SQL = "";
            bool Operacion_Realizada = false;
            //REalizamos el Insert para la primer Cuenta 
            try
            {
                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='COMPRAS')";
                Mi_SQL = Mi_SQL + ", '";
                Mi_SQL = Mi_SQL + Mes_Ano + "',1,";
                Mi_SQL = Mi_SQL + "(SELECT ALMACEN_GENERAL FROM CAT_CON_CUENTAS_FIJAS),";
                Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                Mi_SQL = Mi_SQL + Convert.ToDouble(Datos.P_SubTotal) + ",0,";
                Mi_SQL = Mi_SQL + Convert.ToDouble(Datos.P_SubTotal) + ", ";
                Mi_SQL = Mi_SQL + "SYSDATE,'";
                Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='COMPRAS')";
                Mi_SQL = Mi_SQL + ", '";
                Mi_SQL = Mi_SQL + Mes_Ano + "',2,";
                Mi_SQL = Mi_SQL + "(SELECT IVA_ACREDITABLE_COMPRAS FROM CAT_CON_CUENTAS_FIJAS),";
                Mi_SQL = Mi_SQL + "'" + Concepto + "', ";
                Mi_SQL = Mi_SQL + Convert.ToDouble(Datos.P_IVA) + ",0,";
                Mi_SQL = Mi_SQL + Convert.ToDouble(Datos.P_IVA) + ", ";
                Mi_SQL = Mi_SQL + "SYSDATE,'";
                Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);


                Mi_SQL = "INSERT INTO " + Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles;
                Mi_SQL = Mi_SQL + " (" + Ope_Con_Polizas_Detalles.Campo_No_Poliza;
                Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + ", " + Ope_Con_Polizas_Detalles.Campo_Mes_Ano + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Partida + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Concepto + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Debe + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Haber + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Saldo + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Fecha + ", ";
                Mi_SQL = Mi_SQL + Ope_Con_Polizas_Detalles.Campo_Consecutivo;
                Mi_SQL = Mi_SQL + ") VALUES(" + "'" + No_Poliza + "',";
                Mi_SQL = Mi_SQL + "(SELECT " + Cat_Con_Tipo_Polizas.Campo_Tipo_Poliza_ID;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Con_Tipo_Polizas.Tabla_Cat_Con_Tipo_Polizas;
                Mi_SQL = Mi_SQL + " WHERE " + Cat_Con_Tipo_Polizas.Campo_Descripcion + "='COMPRAS')";
                Mi_SQL = Mi_SQL + ", '";
                Mi_SQL = Mi_SQL + Mes_Ano + "',3,";

                Mi_SQL = Mi_SQL + "'00005',";
                //Mi_SQL = Mi_SQL + "(SELECT " + Cat_Com_Proveedores.Campo_Cuenta + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores;
                //Mi_SQL = Mi_SQL + " WHERE " + Cat_Com_Proveedores.Campo_Proveedor_ID + "='" + Dt.Rows[0][1].ToString().Trim() + "'),";
                Mi_SQL = Mi_SQL + "'" + Concepto + "',0";
                Mi_SQL = Mi_SQL + "," + Convert.ToDouble(Datos.P_Total);
                Mi_SQL = Mi_SQL + "," + Convert.ToDouble(Datos.P_Total) + ", ";
                Mi_SQL = Mi_SQL + "SYSDATE,'";
                Mi_SQL = Mi_SQL + Cls_Util.Obtener_Consecutivo(Ope_Con_Polizas_Detalles.Campo_Consecutivo, Ope_Con_Polizas_Detalles.Tabla_Ope_Con_Polizas_Detalles) + "')";

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
                Operacion_Realizada = true;

            }
            catch (Exception Ex)
            {

                throw new Exception("Error: " + Ex.Message);

                Operacion_Realizada = false;
            }

            return Operacion_Realizada;
        }

        public static DataTable Consultar_Facturas(Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio Datos)
        {
            String Mi_SQL = "SELECT " + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor;
            Mi_SQL = Mi_SQL + ", " + Ope_Com_Facturas_Proveedores.Campo_Proveedor_ID;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores;
            Mi_SQL = Mi_SQL + " WHERE NO_CONTRA_RECIBO";
            Mi_SQL = Mi_SQL + "='" + Datos.P_No_Contra_Recibo + "'";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0]; //  Entregar resultado

        }
        #endregion
    }
}
