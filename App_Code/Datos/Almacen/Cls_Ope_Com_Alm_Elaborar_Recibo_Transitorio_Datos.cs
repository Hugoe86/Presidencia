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
using System.Data.OracleClient;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Bitacora_Eventos;
using Presidencia.Almacen_Elaborar_Recibo_Transitorio.Negocio;
using Presidencia.Generar_Requisicion.Datos;
/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos
/// </summary>
/// 
namespace Presidencia.Almacen_Elaborar_Recibo_Transitorio.Datos
{
    public class Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos
    {
        #region (Variables Locales)

        #endregion

        #region (Variables Publicas)

        #endregion

        #region (Métodos)


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Actualizar_Productos
        ///DESCRIPCIÓN:             Método utilizado actualizar los productos y se le asignan un "SI" al campo "RECIBO_TRANSITORIO" 
        ///                         de la orden de compra correspondiente.
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static void Actualizar_Orden_Compra(String No_Orden_Compra)
        {
            //Declaracion de variables
            String Mensaje = "";
            String Mi_SQL = String.Empty;
            Object Aux;
            OracleConnection Cn = new OracleConnection();
            OracleCommand Cmd = new OracleCommand();
            OracleTransaction Trans;
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;

            OracleDataAdapter Da = new OracleDataAdapter(); // Adaptador para el llenado de los datatable
            DataTable Dt_Aux = new DataTable(); // Tabla auxiliar para las consultas
            long Existencia; // Variable para el calculo de la existencia
            long Disponible; // variable para el calculo del disponible

            try
            { 
                //Se le asignan un SI al campo "RECIBO_TRANSITORIO" de la orden de compra correspondiente
                Mi_SQL = " UPDATE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ";
                Mi_SQL = Mi_SQL + " SET " + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio + " = 'SI'";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + No_Orden_Compra + "";

                OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                //Cmd.CommandText = Mi_SQL;
                //Cmd.ExecuteNonQuery(); // Se ejecuta la operación  

                //Trans.Commit(); // Se ejecuta la transacciones
            }
            catch (OracleException Ex)
            {
                Trans.Rollback();
                //variable para el mensaje 
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
                Cn.Close();
            }
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Ordenes_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar las ordenes de compra que 
        ///                         tengan estatus "SURTIDA", que no tenga NO_RECIBO_TRANSITORIO
        ///                         y que tenga productos cuyo valor es > 20 salarios minimos
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              23/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Ordenes_Compra(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de Variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Ordenes_Compra = new DataTable();
            DataTable Dt_OrdenesC_Transitorias = new DataTable();
            DataTable Dt_Productos_OC = new DataTable();
            DataRow[] Registro;

            try
            {
                // Consulta ordenes de compra Surtidas y que no tengan Numero de recibo transitorio
                Mi_SQL = " SELECT " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ", ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS PROVEEDOR, ";
                Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " AS FECHA, ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Folio + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + ", ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Total + ", ";
                Mi_SQL = Mi_SQL + " (select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
                Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
                Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ".";
                Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS NO_REQUISICION,  ";
                Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + " ";
                Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "";
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + " = " + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + "." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno + " ";
                Mi_SQL = Mi_SQL + " AND (" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'SURTIDA' ";
                Mi_SQL = Mi_SQL + " or " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'TRANSITORIO') ";
                // de la OC en el contra recibo se le asigno que llevan contra recibo o que lleva resguardo, recibo, unidad o totalidad
                Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Recibo_Transitorio + " is null "; // No se le ha asignado recibo transitorio

                if (Datos.P_No_Orden_Compra != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " like '%" + Datos.P_No_Orden_Compra + "%'";
                }
                if (Datos.P_No_Requisicion != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + " like '%" + Datos.P_No_Requisicion + "%'";
                }
                if (Datos.P_Proveedor_ID != null)
                {
                    Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = '" + Datos.P_Proveedor_ID + "'";
                }
                if ((Datos.P_Fecha_Inicio_B != null) && (Datos.P_Fecha_Fin_B != null))
                {
                    Mi_SQL = Mi_SQL + " AND TO_DATE(TO_CHAR(" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + ",'DD/MM/YY')) BETWEEN '" + Datos.P_Fecha_Inicio_B + "'" +
                  " AND '" + Datos.P_Fecha_Fin_B + "'";
                }

                Mi_SQL = Mi_SQL + " Order by " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + ""; 

                // Se guardan las ordenes de compra en la tabla "Dt_Ordenes_Compra"
                Dt_Ordenes_Compra = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

                DataTable Dt_Ordenes_C_TR = new DataTable();
                Dt_Ordenes_C_TR = Dt_Ordenes_Compra.Clone();

                if (Dt_Ordenes_Compra.Rows.Count > 0)
                {
                    for (int i = 0; i < Dt_Ordenes_Compra.Rows.Count; i++)
                    {
                        String No_Contra_Recibo = Dt_Ordenes_Compra.Rows[i]["NO_CONTRA_RECIBO"].ToString().Trim();
                        Boolean Registrar_OC = Verificar_Orden_Compra(No_Contra_Recibo); // Se verifica si la orden de compra contiene prosducto cuyo registro = unidad, totalidad, etc. 

                        if (Registrar_OC)// Si la orden de compra contiene productos que deben ser registrados
                        {
                            Registro = Dt_Ordenes_Compra.Select("NO_CONTRA_RECIBO='" + No_Contra_Recibo.Trim() + "'");
                            DataRow Dr_Orden_Compra = Dt_Ordenes_C_TR.NewRow();

                            Dr_Orden_Compra["NO_CONTRA_RECIBO"] = Registro[0]["NO_CONTRA_RECIBO"].ToString().Trim();
                            Dr_Orden_Compra["NO_ORDEN_COMPRA"] = Registro[0]["NO_ORDEN_COMPRA"].ToString().Trim();
                            Dr_Orden_Compra["PROVEEDOR"] = Registro[0]["PROVEEDOR"].ToString().Trim();
                            Dr_Orden_Compra["FECHA"] = Registro[0]["FECHA"].ToString().Trim();
                            Dr_Orden_Compra["ESTATUS"] = Registro[0]["ESTATUS"].ToString().Trim();
                            Dr_Orden_Compra["TOTAL"] = Registro[0]["TOTAL"].ToString().Trim();
                            Dr_Orden_Compra["FOLIO"] = Registro[0]["FOLIO"].ToString().Trim();
                            Dr_Orden_Compra["PROVEEDOR_ID"] = Registro[0]["PROVEEDOR_ID"].ToString().Trim();
                            Dr_Orden_Compra["NO_FACTURA_PROVEEDOR"] = Registro[0]["NO_FACTURA_PROVEEDOR"].ToString().Trim();
                            Dr_Orden_Compra["NO_REQUISICION"] = Registro[0]["NO_REQUISICION"].ToString().Trim();

                            Int16 Longitud = Convert.ToInt16(Dt_Ordenes_Compra.Rows.Count);
                            if (Longitud == 0)
                                Dt_Ordenes_C_TR.Rows.InsertAt(Dr_Orden_Compra, Longitud);
                            else
                                Dt_Ordenes_C_TR.Rows.InsertAt(Dr_Orden_Compra, (Longitud + 1));
                        }
                    }
                }
                return Dt_Ordenes_C_TR;
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
            DataTable Dt_Productos_Recibo_Transitorio = new DataTable();

            // Asignar consulta
            Mi_SQL = "SELECT PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo_Transitorio + ", ";
            Mi_SQL = Mi_SQL + " PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Unidad + ", ";
            Mi_SQL = Mi_SQL + " PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + ", ";
            Mi_SQL = Mi_SQL + " PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + ", ";
            Mi_SQL = Mi_SQL + " PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + ", ";
            Mi_SQL = Mi_SQL + " PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Registrado + " ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PRODUCTOS_CONTRARECIBO ";
            Mi_SQL = Mi_SQL + " WHERE PRODUCTOS_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " = " + No_Orden_Compra.Trim();

            Dt_Productos_Recibo_Transitorio = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];

            if (Dt_Productos_Recibo_Transitorio.Rows.Count > 0)
            {
                for (int j = 0; j < Dt_Productos_Recibo_Transitorio.Rows.Count; j++)
                {
                    String Recibo_Transitorio = Dt_Productos_Recibo_Transitorio.Rows[j]["RECIBO_TRANSITORIO"].ToString().Trim();
                    String Recibo= Dt_Productos_Recibo_Transitorio.Rows[j]["RECIBO"].ToString().Trim();
                    String Resguardo=Dt_Productos_Recibo_Transitorio.Rows[j]["RESGUARDO"].ToString().Trim();
                    String Unidad=Dt_Productos_Recibo_Transitorio.Rows[j]["UNIDAD"].ToString().Trim();
                    String Totalidad=Dt_Productos_Recibo_Transitorio.Rows[j]["TOTALIDAD"].ToString().Trim();
                    String Registrado = Dt_Productos_Recibo_Transitorio.Rows[j]["REGISTRADO"].ToString().Trim();

                    if ((Recibo_Transitorio == "SI") & (Totalidad == "SI"))
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else if ((Recibo == "SI") & (Unidad == "SI") & (Registrado == "SI"))
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else if ((Resguardo == "SI") & (Unidad == "SI") & (Registrado == "SI"))
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else if ((Recibo == "SI") & (Totalidad == "SI") & (Registrado == "SI"))
                    {
                        Registrar = true;
                        return Registrar;
                    }
                    else if ((Resguardo == "SI") & (Totalidad == "SI") & (Registrado == "SI"))
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
        ///NOMBRE DE LA FUNCIÓN:    Llenar_Combo
        ///DESCRIPCIÓN:             Método utilizado para consultar las marcas y los modelos
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Llenar_Combo(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            String Mi_SQL = null;
            DataSet Ds_Consulta = null;
            DataTable Dt_consulta = new DataTable();

            try
            {
                if (Datos.P_Tipo_Combo.Equals("MODELOS"))
                {
                    Mi_SQL = "SELECT DISTINCT ";
                    Mi_SQL = Mi_SQL + Cat_Com_Modelos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Modelos.Campo_Modelo_ID;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Modelos.Campo_Nombre;
                }
                else if (Datos.P_Tipo_Combo.Equals("MARCAS"))
                {
                    Mi_SQL = "SELECT DISTINCT ";
                    Mi_SQL = Mi_SQL + Cat_Com_Marcas.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Marcas.Campo_Marca_ID;
                    Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas;
                    Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Marcas.Campo_Nombre;
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
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Facturas
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de las ordenes de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernández Ramírez
        ///FECHA_MODIFICO:          05/Mayo/2011
        ///CAUSA_MODIFICACIÓN:      Se agregaron las subconsultas para el modelo y la marca
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Generales(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos_OC = new DataTable();

            Mi_SQL = "SELECT ";
            Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + "";
            Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + " as IMPORTE";
            
            Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " WHERE DEPENDENCIAS." + Cat_Dependencias.Campo_Dependencia_ID + " = ";
            Mi_SQL = Mi_SQL + "( select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " ORDENES_COMPRA." +
            Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ")) AS UNIDAD_RESPONSABLE ";

            Mi_SQL = Mi_SQL + ",(select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Folio + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " ORDENES_COMPRA." +
            Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS NO_REQUISICION ";

            Mi_SQL = Mi_SQL + ",(select REQUISICIONES." + Ope_Com_Requisiciones.Campo_Dependencia_ID + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " ORDENES_COMPRA." +
            Ope_Com_Ordenes_Compra.Campo_Lista_Requisiciones + ") AS UNIDAD_RESPONSABLE_ID ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra+ " ORDENES_COMPRA ";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo ;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Datos_Generales_Recibo_Transitorio
        ///DESCRIPCIÓN:             Método utilizado para consultar los datos geenrales del rec
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernández Ramírez
        ///FECHA_MODIFICO:          05/Mayo/2011
        ///CAUSA_MODIFICACIÓN:      Se agregaron las subconsultas para el modelo y la marca
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Generales_Recibo_Transitorio(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            Mi_SQL = "SELECT distinct REQ_PRODUCTO.";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Requisicion_ID + " as REQUISICION";
            Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + " as ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + " as IMPORTE_OC";
            //Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " as FECHA ";
            
            Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Cat_Dependencias.Campo_Dependencia_ID + " = DEPENDENCIAS.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + ") as DEPENDENCIA";

            Mi_SQL = Mi_SQL + ",(select FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES";
            Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ") as FACTURA";

            Mi_SQL = Mi_SQL + ",(select FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES";
            Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ") as FECHA";

            Mi_SQL = Mi_SQL + ",(select EMPLEADOS." + Cat_Empleados.Campo_Nombre +  " || ' ' || ";
            Mi_SQL = Mi_SQL + "EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
            Mi_SQL = Mi_SQL + "EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " AS RESPONSABLE";
            Mi_SQL = Mi_SQL + " from  " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + "( select RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_Responsable_ID+ " from ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + " RECIBO_TRANSITORIO ";
            Mi_SQL = Mi_SQL + " where  ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " and RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + " " + Datos.P_No_Contra_Recibo.Trim() + ")) as RESPONSABLE ";

            Mi_SQL = Mi_SQL + ",(select EMPLEADOS." + Cat_Empleados.Campo_RFC + " ";
            Mi_SQL = Mi_SQL + " from  " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + "(select RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_Responsable_ID + " from ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + " RECIBO_TRANSITORIO ";
            Mi_SQL = Mi_SQL + " where  ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " and RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + " " + Datos.P_No_Contra_Recibo.Trim() + ")) as RFC ";

            Mi_SQL = Mi_SQL + ",(select PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = PROVEEDORES.";
            Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Proveedor_ID+ ") as PROVEEDOR ";

            Mi_SQL = Mi_SQL + ",(select RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo+ " FROM ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + " RECIBO_TRANSITORIO";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = RECIBO_TRANSITORIO.";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo + ") as NO_RECIBO";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
            Mi_SQL = Mi_SQL + "" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA, ";
            Mi_SQL = Mi_SQL + "" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";

            Mi_SQL = Mi_SQL + " where ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " and ";
            Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Datos_Generales_Recibo_Transitorio
        ///DESCRIPCIÓN:             Método utilizado para consultar los datos geenrales del rec
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernández Ramírez
        ///FECHA_MODIFICO:          05/Mayo/2011
        ///CAUSA_MODIFICACIÓN:      Se agregaron las subconsultas para el modelo y la marca
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Generales_Recibo_Transitorio_Totalidad(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            Mi_SQL = "SELECT distinct REQ_PRODUCTO.";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Campo_Requisicion_ID + " as REQUISICION";
            Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Folio + " as ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Total + " as IMPORTE_OC";
            //Mi_SQL = Mi_SQL + ", ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Fecha_Creo + " as FECHA ";

            Mi_SQL = Mi_SQL + ",(select DEPENDENCIAS." + Cat_Dependencias.Campo_Nombre + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIAS";
            Mi_SQL = Mi_SQL + " WHERE REQUISICIONES." + Cat_Dependencias.Campo_Dependencia_ID + " = DEPENDENCIAS.";
            Mi_SQL = Mi_SQL + Cat_Dependencias.Campo_Dependencia_ID + ") as DEPENDENCIA";

            Mi_SQL = Mi_SQL + ",(select FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Proveedor + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES";
            Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ") as FACTURA";

            Mi_SQL = Mi_SQL + ",(select FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_Fecha_Factura+ " FROM ";
            Mi_SQL = Mi_SQL + Ope_Com_Facturas_Proveedores.Tabla_Ope_Com_Facturas_Proveedores + " FACTURA_PROVEEDORES";
            Mi_SQL = Mi_SQL + " WHERE FACTURA_PROVEEDORES." + Ope_Com_Facturas_Proveedores.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ") as FECHA";

            Mi_SQL = Mi_SQL + ",(select EMPLEADOS." + Cat_Empleados.Campo_Nombre + " || ' ' || ";
            Mi_SQL = Mi_SQL + "EMPLEADOS." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ";
            Mi_SQL = Mi_SQL + "EMPLEADOS." + Cat_Empleados.Campo_Apellido_Materno + " AS RESPONSABLE";
            Mi_SQL = Mi_SQL + " from  " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + "( select RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_Responsable_ID + " from ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios_Totalidad.Tabla_Ope_Alm_Recibos_Transitorios_Totalidad + " RECIBO_TRANSITORIO ";
            Mi_SQL = Mi_SQL + " where  ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " and RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + " " + Datos.P_No_Contra_Recibo.Trim() + ")) as RESPONSABLE ";

            Mi_SQL = Mi_SQL + ",(select EMPLEADOS." + Cat_Empleados.Campo_RFC + " ";
            Mi_SQL = Mi_SQL + " from  " + Cat_Empleados.Tabla_Cat_Empleados + " EMPLEADOS";
            Mi_SQL = Mi_SQL + " where EMPLEADOS." + Cat_Empleados.Campo_Empleado_ID + " = ";
            Mi_SQL = Mi_SQL + "(select RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_Responsable_ID + " from ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + " RECIBO_TRANSITORIO ";
            Mi_SQL = Mi_SQL + " where  ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + " RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " and RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + " " + Datos.P_No_Contra_Recibo.Trim() + ")) as RFC ";

            Mi_SQL = Mi_SQL + ",(select PROVEEDORES." + Cat_Com_Proveedores.Campo_Nombre + " FROM ";
            Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + " PROVEEDORES";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID + " = PROVEEDORES.";
            Mi_SQL = Mi_SQL + Cat_Com_Proveedores.Campo_Proveedor_ID + ") as PROVEEDOR ";

            Mi_SQL = Mi_SQL + ",(select RECIBO_TRANSITORIO." + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Recibo + " FROM ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios_Totalidad.Tabla_Ope_Alm_Recibos_Transitorios_Totalidad + " RECIBO_TRANSITORIO";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = RECIBO_TRANSITORIO.";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Contra_Recibo + ") as NO_RECIBO";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTO, ";
            Mi_SQL = Mi_SQL + "" + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA, ";
            Mi_SQL = Mi_SQL + "" + Ope_Com_Requisiciones.Tabla_Ope_Com_Requisiciones + " REQUISICIONES ";

            Mi_SQL = Mi_SQL + " where ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " and ";
            Mi_SQL = Mi_SQL + " ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " AND REQ_PRODUCTO." + Ope_Com_Req_Producto.Campo_Requisicion_ID + " = ";
            Mi_SQL = Mi_SQL + " REQUISICIONES." + Ope_Com_Requisiciones.Campo_Requisicion_ID + " ";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Facturas
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de las ordenes de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernández Ramírez
        ///FECHA_MODIFICO:          05/Mayo/2011
        ///CAUSA_MODIFICACIÓN:      Se agregaron las subconsultas para el modelo y la marca
        ///*******************************************************************************
        public static DataTable Consulta_Facturas(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos_OC = new DataTable();

            Mi_SQL = "SELECT FACTURA.";
            Mi_SQL = Mi_SQL + Ope_Alm_Registro_Facturas.Campo_Factura_Proveedor;
            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Registro_Facturas.Tabla_Ope_Alm_Registro_Facturas + " FACTURA ";
            Mi_SQL = Mi_SQL + " WHERE FACTURA." + Ope_Alm_Registro_Facturas.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        
        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Productos_Requision
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de la requisición
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              21/Julio/2011 
        ///MODIFICO:             
        ///FECHA_MODIFICO:         
        ///CAUSA_MODIFICACIÓN:      
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Requision(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            Mi_SQL = "SELECT  PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID;
            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO ";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION ";

            Mi_SQL = Mi_SQL + ",(select NVL(MODELO." + Cat_Com_Modelos.Campo_Nombre + ", 'INDISTINTO') from ";
            Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + " MODELO ";
            Mi_SQL = Mi_SQL + " where  MODELO." + Cat_Com_Modelos.Campo_Modelo_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Modelo_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID+ " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as MODELO ";

            Mi_SQL = Mi_SQL + ",(select NVL(MARCA." + Cat_Com_Marcas.Campo_Nombre + ", 'INDISTINTA') from ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + " MARCA ";
            Mi_SQL = Mi_SQL + " where  MARCA." + Cat_Com_Marcas.Campo_Marca_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Marca_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as MARCA ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Total_Cotizado + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as MONTO_TOTAL ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as PRECIO ";

            Mi_SQL = Mi_SQL + ",(select UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID+ " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDAD ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as CANTIDAD "; // Cantidad Solicitada

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as EXISTENCIA "; // Cantidad Entregada


            //Mi_SQL = Mi_SQL + ",( select  nvl ((REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad + " - ";
            //Mi_SQL = Mi_SQL + " REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Cantidad_Entregada + " ), 0)";
            //Mi_SQL = Mi_SQL + " from " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            //Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            //Mi_SQL = Mi_SQL + " PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Producto_ID + "";
            //Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            //Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            //Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            //Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            //Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as EXISTENCIA ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PROD_CONTRARECIBO ";
            Mi_SQL = Mi_SQL + " WHERE PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo + " = ";
            Mi_SQL = Mi_SQL + Datos.P_No_Contra_Recibo.Trim() + "";
            Mi_SQL = Mi_SQL + " And (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo_Transitorio + " = 'SI'";
            Mi_SQL = Mi_SQL + " And PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + " = 'SI')";

            Mi_SQL = Mi_SQL + "  order by PRODUCTO";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Productos_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de las ordenes de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              22/Marzo/2011 
        ///MODIFICO:                Salvador Hernández Ramírez
        ///FECHA_MODIFICO:          05/Mayo/2011
        ///CAUSA_MODIFICACIÓN:      Se agregaron las subconsultas para el modelo y la marca
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Orden_Compra(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            
            // Consultar los productos de la orden de compra seleccionada
            Mi_SQL = "SELECT distinct INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Inventario + ", ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Observaciones + ", ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Serie + ", ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Cantidad + " ";
            
            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO ";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION ";

            Mi_SQL = Mi_SQL + ",(select UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDAD ";

            Mi_SQL = Mi_SQL + ",(select MATERIAL." + Cat_Pat_Materiales.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIAL";
            Mi_SQL = Mi_SQL + " where  MATERIAL." + Cat_Pat_Materiales.Campo_Material_ID + " = ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id + " )  as MATERIAL";

            Mi_SQL = Mi_SQL + ",(select COLOR." + Cat_Pat_Colores.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLOR";
            Mi_SQL = Mi_SQL + " where  COLOR." + Cat_Pat_Colores.Campo_Color_ID + " = ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id + " )  as COLOR";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as MONTO ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " INV_B_MUEBLES ";
            Mi_SQL = Mi_SQL + "," + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PROD_CONTRARECIBO ";
            Mi_SQL = Mi_SQL + "," + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDENES_COMPRA ";
            Mi_SQL = Mi_SQL + " WHERE ORDENES_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim();
            Mi_SQL = Mi_SQL + " and INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim();
            Mi_SQL = Mi_SQL + " and PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim();
            Mi_SQL = Mi_SQL + " and (( PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Recibo + " = 'SI'";
            Mi_SQL = Mi_SQL + " and (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Unidad + " = 'SI'";
            Mi_SQL = Mi_SQL + " or PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + " = 'SI'))";
            Mi_SQL = Mi_SQL + " or (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Resguardo + " = 'SI'";
            Mi_SQL = Mi_SQL + " and (PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Unidad + " = 'SI'";
            Mi_SQL = Mi_SQL + " or PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_Totalidad + " = 'SI')))";

            Mi_SQL = Mi_SQL + "  order by PRODUCTO";
            

            // Ejecutar consulta
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consulta_Productos_Recibo_Transitorio
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos de el recibo transitorio que se genero
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              09/Julio/2011 
        ///MODIFICO:                
        ///FECHA_MODIFICO:          
        ///CAUSA_MODIFICACIÓN:      
        ///*******************************************************************************
        public static DataTable Consulta_Productos_Recibo_Transitorio(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos_OC = new DataTable();
            DataTable Dt_Productos_Transitorios = new DataTable();

            // Consultar los productos de la orden de compra seleccionada
            // Asignar consulta

            Mi_SQL = "SELECT distinct INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Inventario + " AS NO_INVENTARIO, ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + ", ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Observaciones + ", ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Serie + " ";
            
            Mi_SQL = Mi_SQL + ",(select MATERIAL." + Cat_Pat_Materiales.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + " MATERIAL ";
            Mi_SQL = Mi_SQL + " where  MATERIAL." + Cat_Pat_Materiales.Campo_Material_ID + " = ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Material_Id + " )  as MATERIAL";

            Mi_SQL = Mi_SQL + ",(select COLOR." + Cat_Pat_Colores.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Pat_Colores.Tabla_Cat_Pat_Colores + " COLOR ";
            Mi_SQL = Mi_SQL + " where  COLOR." + Cat_Pat_Colores.Campo_Color_ID + " = ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Color_Id + " )  as COLOR";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Nombre + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as PRODUCTO ";

            Mi_SQL = Mi_SQL + ",(select PRODUCTOS." + Cat_Com_Productos.Campo_Descripcion + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = PRODUCTOS.";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Campo_Producto_ID + ")as DESCRIPCION ";

            Mi_SQL = Mi_SQL + ",(select UNIDADES." + Cat_Com_Unidades.Campo_Abreviatura + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + " UNIDADES ";
            Mi_SQL = Mi_SQL + " where  UNIDADES." + Cat_Com_Unidades.Campo_Unidad_ID + " = ";
            Mi_SQL = Mi_SQL + " (select PRODUCTOS." + Cat_Com_Productos.Campo_Unidad_ID + " from ";
            Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + " PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " = ";
            Mi_SQL = Mi_SQL + " PRODUCTOS." + Cat_Com_Productos.Campo_Producto_ID + ")) as UNIDAD ";

            Mi_SQL = Mi_SQL + ",(select REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + " REQ_PRODUCTOS ";
            Mi_SQL = Mi_SQL + " where  REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " = ";
            Mi_SQL = Mi_SQL + " INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_Producto_Id + " ";
            Mi_SQL = Mi_SQL + " and REQ_PRODUCTOS." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " = ";
            Mi_SQL = Mi_SQL + " (select ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " from ";
            Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ORDEN_COMPRA ";
            Mi_SQL = Mi_SQL + " where  ORDEN_COMPRA." + Ope_Com_Ordenes_Compra.Campo_No_Factura_Interno;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ")) as MONTO ";

            Mi_SQL = Mi_SQL + ",(select RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo + " from ";
            Mi_SQL = Mi_SQL + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios + " RECIBOS_TRANSITORIOS ";
            Mi_SQL = Mi_SQL + " where  RECIBOS_TRANSITORIOS." + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim() + ") as NO_RECIBO ";

            Mi_SQL = Mi_SQL + " FROM " + Ope_Alm_Pat_Inv_Bienes_Muebles.Tabla_Ope_Alm_Pat_Inv_Bienes_Muebles + " INV_B_MUEBLES ";
            Mi_SQL = Mi_SQL + "," + Ope_Alm_Productos_Contrarecibo.Tabla_Ope_Alm_Productos_Contrarecibo + " PROD_CONTRARECIBO ";
            Mi_SQL = Mi_SQL + " WHERE INV_B_MUEBLES." + Ope_Alm_Pat_Inv_Bienes_Muebles.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim();
            Mi_SQL = Mi_SQL + " and PROD_CONTRARECIBO." + Ope_Alm_Productos_Contrarecibo.Campo_No_Contra_Recibo;
            Mi_SQL = Mi_SQL + " = " + Datos.P_No_Contra_Recibo.Trim();

            Mi_SQL = Mi_SQL + "  order by PRODUCTO";

            // Ejecutar consulta
            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];  
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Empleados_Almacen
        /// DESCRIPCION:            Consultar los empleados que pertenecen al Area "Almacén"
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            08/Junio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Empleados_Almacen(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas

            Mi_SQL = Mi_SQL + " Select " + Cat_Empleados.Campo_Empleado_ID + ", ";
            Mi_SQL = Mi_SQL + "" + Cat_Empleados.Campo_Nombre + "||' '||";
            Mi_SQL = Mi_SQL + "" + Cat_Empleados.Campo_Apellido_Paterno + "||' '||";
            Mi_SQL = Mi_SQL + "" + Cat_Empleados.Campo_Apellido_Materno + " as EMPLEADO ";
            Mi_SQL = Mi_SQL + " FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE ";
            Mi_SQL = Mi_SQL + Cat_Empleados.Campo_Area_ID + " = '" + Datos.P_Unidad_Responsable_ID + "'";  
            Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Empleados.Campo_Nombre + "";

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Productos_Serializados
        ///DESCRIPCIÓN:             Método utilizado para consultar los productos 
        ///                         de las ordenes de compra que han sido serializados
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              25/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Productos_Serializados(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; //Variable para las consultas
            DataTable Dt_Productos_Serializados = new DataTable();

            // Asignar consulta
            //Mi_SQL = "SELECT " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Clave + " as Clave, ";
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre + " as Producto, ";
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Existencia + " as Cantidad_Existente, ";
            //Mi_SQL = Mi_SQL + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Nombre + " as Modelo, ";
            //Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Nombre + " as Marca, ";
            //Mi_SQL = Mi_SQL + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Serie + " as Serie, ";
            //Mi_SQL = Mi_SQL + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_No_Recibo + " as No_Recibo, ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Cantidad + " as Cantidad_Solicitada, ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado + " as Costo_Unitario, ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " as Producto_Id, ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_IVA_Cotizado + ", ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_IEPS_Cotizado + ", ";
            //Mi_SQL = Mi_SQL + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Subtota_Cotizado + " as Importe, ";
            //Mi_SQL = Mi_SQL + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Nombre + " as Unidad, ";
            //Mi_SQL = Mi_SQL + "REPLACE(" + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ".ROWID,";
            //Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ".ROWID, 'PRODUCTO') AS TIPO FROM ";
            //Mi_SQL = Mi_SQL + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + ", " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + ", ";
            //Mi_SQL = Mi_SQL + Cat_Com_Productos.Tabla_Cat_Com_Productos + ", " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + ", ";
            //Mi_SQL = Mi_SQL + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + ", " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + ", ";
            //Mi_SQL = Mi_SQL + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + " ";
            //Mi_SQL = Mi_SQL + "WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
            //Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_No_Orden_Compra + " ";
            //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID;
            //Mi_SQL = Mi_SQL + " = " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Producto_ID + " ";
            //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra; // Con estas lineas se esta limitando para que solo agarre los produscos cuyo monto sea para resguardo
            //Mi_SQL = Mi_SQL + " = " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_No_Orden_Compra + " ";
            //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Modelo_Id; // De esta manera se trae el modelo y la marca correspondiente
            //Mi_SQL = Mi_SQL + " = " + Cat_Com_Modelos.Tabla_Cat_Com_Modelos + "." + Cat_Com_Modelos.Campo_Modelo_ID + " ";
            //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Marca_Id;
            //Mi_SQL = Mi_SQL + " = " + Cat_Com_Marcas.Tabla_Cat_Com_Marcas + "." + Cat_Com_Marcas.Campo_Marca_ID + " ";
            //Mi_SQL = Mi_SQL + "AND " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Unidad_ID;
            //Mi_SQL = Mi_SQL + " = " + Cat_Com_Unidades.Tabla_Cat_Com_Unidades + "." + Cat_Com_Unidades.Campo_Unidad_ID + " ";
            //Mi_SQL = Mi_SQL + "AND " + Ope_Com_Series_Productos.Tabla_Ope_Com_Series_Productos + "." + Ope_Com_Series_Productos.Campo_Producto_Id; // De esta manera se trae el modelo y la marca correspondiente
            //Mi_SQL = Mi_SQL + " = " + Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto + "." + Ope_Com_Req_Producto.Campo_Prod_Serv_ID + " ";
            //Mi_SQL = Mi_SQL + " AND " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra;
            //Mi_SQL = Mi_SQL + " = " + Datos.P_No_Orden_Compra + " ";

            // Ejecutar consulta
            Dt_Productos_Serializados = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Productos_Serializados;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Guardar_Recibo
        ///DESCRIPCIÓN:             Método utilizado guardar el recibo transitorio
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Int64 Guardar_Recibo(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            String Mi_SQL = "";
            Object Aux;
            Int64 No_Recibo_Transitorio=0;

            if (Datos.P_Tipo_Recibo == "UNIDAD")
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios;
            }
            else if (Datos.P_Tipo_Recibo == "TOTALIDAD")
            {
                Mi_SQL = "SELECT NVL(MAX(" + Ope_Alm_Recibos_Transitorios_Totalidad.Campo_No_Recibo + "), 0) ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Alm_Recibos_Transitorios_Totalidad.Tabla_Ope_Alm_Recibos_Transitorios_Totalidad;
            }

            // Ejecutar consulta
            Aux = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            //Verificar si no es nulo
            if (Aux != null && Convert.IsDBNull(Aux) == false)
                No_Recibo_Transitorio = Convert.ToInt64(Aux) + 1;
            else
                No_Recibo_Transitorio = 1;


            if (Datos.P_Tipo_Recibo == "UNIDAD")
                Mi_SQL = "INSERT INTO " + Ope_Alm_Recibos_Transitorios.Tabla_Ope_Alm_Recibos_Transitorios;
            else if (Datos.P_Tipo_Recibo == "TOTALIDAD")
                Mi_SQL = "INSERT INTO " + Ope_Alm_Recibos_Transitorios_Totalidad.Tabla_Ope_Alm_Recibos_Transitorios_Totalidad;

            Mi_SQL = Mi_SQL + " (" + Ope_Alm_Recibos_Transitorios.Campo_No_Recibo +
            ", " + Ope_Alm_Recibos_Transitorios.Campo_No_Contra_Recibo+
            ", " + Ope_Alm_Recibos_Transitorios.Campo_Tipo +
            ", " + Ope_Alm_Recibos_Transitorios.Campo_Responsable_ID +
            ", " + Ope_Alm_Recibos_Transitorios.Campo_Usuario_Creo +
            ", " + Ope_Alm_Recibos_Transitorios.Campo_Fecha_Creo +
            ") VALUES (" +
             No_Recibo_Transitorio + ", " +
             Datos.P_No_Contra_Recibo+ ",'" +
             Datos.P_Tipo_Recibo + "','" +
             Datos.P_Responsable_ID + "','" +
             Datos.P_Usuario_Creo + "'," +
             "SYSDATE)";

            Actualizar_Orden_Compra(Datos.P_No_Orden_Compra); // Se actualiza la Orden de Compra
            //String No_Recibo = Convert.ToString(No_Recibo_Transitorio);  // Para la bitacora
            // Se da de alta la operación en el método "Alta_Bitacora"
            //Cls_Bitacora.Alta_Bitacora(Cls_Sessiones.Empleado_ID, Ope_Bitacora.Accion_Alta, "Frm_Ope_Alm_Elaborar_Recibo_Transitorio.aspx", No_Recibo, Mi_SQL);

            OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Cls_Ope_Com_Requisiciones_Datos.Registrar_Historial("SURTIDA / RECIBO TRANSITORIO", Datos.P_No_Requisicion);
            return No_Recibo_Transitorio;
        }

        

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Proveedores
        /// DESCRIPCION:            Consultar los datos de los proveedores
        /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene 
        ///                         los datos para la busqueda
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            11/Abril/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Proveedores(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            //Declaracion de variables
            String Mi_SQL = String.Empty; // Variable para las consultas

            try
            {
                // Asignar consulta
                Mi_SQL = "SELECT DISTINCT " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + ", " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre + " AS " + Cat_Com_Proveedores.Campo_Compañia;
                Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + ", " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra;
                Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Proveedor_ID;
                Mi_SQL = Mi_SQL + " = " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Proveedor_ID + " ";
                Mi_SQL = Mi_SQL + " and " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + "." + Ope_Com_Ordenes_Compra.Campo_Estatus + " = 'SURTIDA' ";
                Mi_SQL = Mi_SQL + " ORDER BY " + Cat_Com_Proveedores.Tabla_Cat_Com_Proveedores + "." + Cat_Com_Proveedores.Campo_Nombre; //Ordenamiento

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
        /// NOMBRE DE LA CLASE:     Consulta_Observaciones_Orden_Compra
        /// DESCRIPCION:            Consultar las Observaciones de la Orden de Compra
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            25/Abril/2011  
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public static DataTable Consulta_Observaciones_Orden_Compra(Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio Datos)
        {
            DataTable Dt_Observaciones = new DataTable();
            String Mi_SQL = String.Empty; //variable apra las consultas

            // Consulta para obtener las cantidades a modificar en la tabla de los productos
            Mi_SQL = "SELECT " + Ope_Com_Ordenes_Compra.Campo_Comentarios+ " ";
            Mi_SQL = Mi_SQL + " FROM " + Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra + " ";
            Mi_SQL = Mi_SQL + " WHERE " + Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + Datos.P_No_Orden_Compra + "";

            // Se guardan las ordenes de compra en la tabla "Dt_Observaciones"
            Dt_Observaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            return Dt_Observaciones;
        }

        #endregion
    }
}