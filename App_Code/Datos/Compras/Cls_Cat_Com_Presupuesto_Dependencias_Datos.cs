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
using Presidencia.Catalogo_Compras_Presupuesto_Dependencias.Negocio;
using Presidencia.Constantes;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Com_Presupuesto_Dependencias_Datos
/// </summary>
namespace Presidencia.Catalogo_Compras_Presupuesto_Dependencias1.Datos
{
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:    Cls_Cat_Com_Presupuesto_Dependencias_Datos
    /// DESCRIPCION:           Clase que contiene las operaciones de la base de datos 
    ///                        para la tabla Cat_Com_Presupuesto_Dependencias_Datos
    /// PARAMETROS :     
    /// CREO       :           José Antonio López Hernández
    /// FECHA_CREO :           04/Enero/2011 13:33
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public class Cls_Cat_Com_Presupuesto_Dependencias_Datos
    {
        
        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Alta_Presupuesto_Dependencia
        ///// DESCRIPCION:            Dar de Alta un nuevo presupuesto de dependencia a la base de datos
        ///// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a ingresar
        ///// CREO       :            José Antonio López Hernández
        ///// FECHA_CREO :            04/Enero/2011 13:42 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        //public static void Alta_Presupuesto_Dependencia(Cls_Cat_Com_Presupuesto_Dependencias Datos)
        //{
        //    //Declaracion de variables
        //    OracleTransaction Obj_Transaccion = null;
        //    OracleConnection Obj_Conexion;
        //    OracleCommand Obj_Comando;
        //    String Mi_SQL = String.Empty;
        //    Object Aux; //Variable auxiliar para las consultas
        //    String Mensaje = String.Empty; //Variable para el mensaje de error

        //    try
        //    {
        //        Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
        //        Obj_Comando = new OracleCommand();
        //        Obj_Conexion.Open();
        //        Obj_Transaccion = Obj_Conexion.BeginTransaction();
        //        Obj_Comando.Transaction = Obj_Transaccion;
        //        Obj_Comando.Connection = Obj_Conexion;

        //        //Consulta para el ID
        //        Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Dep_Presupuesto.Campo_Dependencia_ID + "), '00000') FROM " + Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto;

        //        //Ejecutar consulta
        //        Obj_Comando.CommandText = Mi_SQL;
        //        Aux = Obj_Comando.ExecuteScalar();

        //        //Verificar si no es nulo
        //        if (Convert.IsDBNull(Aux) == false)
        //        {
        //            Datos.P_Impuesto_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
        //        }
        //        else
        //            Datos.P_Impuesto_ID = "00001";

        //        //Consulta para el alta de los impuestos
        //        Mi_SQL = "INSERT INTO " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " (" + Cat_Com_Impuestos.Campo_Impuesto_ID + ",";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Nombre + "," + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + ",";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Comentarios + "," + Cat_Com_Impuestos.Campo_Usuario_Creo + "," + Cat_Com_Impuestos.Campo_Fecha_Creo + ") ";
        //        Mi_SQL = Mi_SQL + "VALUES('" + Datos.P_Impuesto_ID + "','" + Datos.P_Nombre + "'," + Datos.P_Porcentaje_Impuesto + ",";
        //        Mi_SQL = Mi_SQL + "'" + Datos.P_Comentarios + "','" + Datos.P_Usuario_Creo + "',SYSDATE)";

        //        //Ejecutar consulta
        //        Obj_Comando.CommandText = Mi_SQL;
        //        Obj_Comando.ExecuteNonQuery();

        //        //Ejecutar transaccion
        //        Obj_Transaccion.Commit();
        //        Obj_Conexion.Close();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        if (Obj_Transaccion != null)
        //        {
        //            Obj_Transaccion.Rollback();
        //        }
        //        switch (Ex.Code.ToString())
        //        {
        //            case "2291":
        //                Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
        //                break;
        //            case "923":
        //                Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
        //                break;
        //            case "12170":
        //                Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
        //                break;
        //            default:
        //                Mensaje = "Error:  [" + Ex.Message + "]";
        //                break;
        //        }

        //        throw new Exception(Mensaje, Ex);
        //    }
        //    finally
        //    {
        //        Obj_Comando = null;
        //        Obj_Conexion = null;
        //        Obj_Transaccion = null;
        //    }
        //}

        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Baja_Impuestos
        ///// DESCRIPCION:            Eliminar un impuesto existente de la base de datos
        ///// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a eliminar
        ///// CREO       :            Noe Mosqueda Valadez
        ///// FECHA_CREO :            21/Octubre/2010 16:30 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        //public static void Baja_Impuestos(Cls_Cat_Com_Impuestos_Negocio Datos)
        //{
        //    //Declaracion de variables
        //    OracleTransaction Obj_Transaccion = null;
        //    OracleConnection Obj_Conexion;
        //    OracleCommand Obj_Comando;
        //    String Mi_SQL = String.Empty;
        //    String Mensaje = String.Empty; //Variable para el mensaje de error

        //    try
        //    {
        //        Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
        //        Obj_Comando = new OracleCommand();
        //        Obj_Conexion.Open();
        //        Obj_Transaccion = Obj_Conexion.BeginTransaction();
        //        Obj_Comando.Transaction = Obj_Transaccion;
        //        Obj_Comando.Connection = Obj_Conexion;

        //        //Asignar consulta para la baja de impuestos
        //        Mi_SQL = "DELETE FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " ";
        //        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID + " = '" + Datos.P_Impuesto_ID + "'";

        //        //Ejecutar consulta
        //        Obj_Comando.CommandText = Mi_SQL;
        //        Obj_Comando.ExecuteNonQuery();

        //        //Ejecutar transaccion
        //        Obj_Transaccion.Commit();
        //        Obj_Conexion.Close();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        if (Obj_Transaccion != null)
        //        {
        //            Obj_Transaccion.Rollback();
        //        }
        //        switch (Ex.Code.ToString())
        //        {
        //            case "2291":
        //                Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
        //                break;
        //            case "923":
        //                Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
        //                break;
        //            case "12170":
        //                Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
        //                break;
        //            default:
        //                Mensaje = "Error:  [" + Ex.Message + "]";
        //                break;
        //        }

        //        throw new Exception(Mensaje, Ex);
        //    }
        //    finally
        //    {
        //        Obj_Comando = null;
        //        Obj_Conexion = null;
        //        Obj_Transaccion = null;
        //    }
        //}

        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Cambio_Impuestos
        ///// DESCRIPCION:            Modificar un impuesto existente de la base de datos
        ///// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        ///// CREO       :            Noe Mosqueda Valadez
        ///// FECHA_CREO :            21/Octubre/2010 16:44 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        //public static void Cambio_Impuestos(Cls_Cat_Com_Impuestos_Negocio Datos)
        //{
        //    //Declaracion de variables
        //    OracleTransaction Obj_Transaccion = null;
        //    OracleConnection Obj_Conexion;
        //    OracleCommand Obj_Comando;
        //    String Mi_SQL = String.Empty;
        //    String Mensaje = String.Empty; //Variable para el mensaje de error

        //    try
        //    {
        //        Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
        //        Obj_Comando = new OracleCommand();
        //        Obj_Conexion.Open();
        //        Obj_Transaccion = Obj_Conexion.BeginTransaction();
        //        Obj_Comando.Transaction = Obj_Transaccion;
        //        Obj_Comando.Connection = Obj_Conexion;

        //        //Asignar consulta para la modificacion de los impuestos
        //        Mi_SQL = "UPDATE " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " ";
        //        Mi_SQL = Mi_SQL + "SET " + Cat_Com_Impuestos.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + " = " + Datos.P_Porcentaje_Impuesto.ToString().Trim() + ", ";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo + "', ";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Fecha_Modifico + " = SYSDATE ";
        //        Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID + " = '" + Datos.P_Impuesto_ID + "'";

        //        //Ejecutar consulta
        //        Obj_Comando.CommandText = Mi_SQL;
        //        Obj_Comando.ExecuteNonQuery();

        //        //Ejecutar transaccion
        //        Obj_Transaccion.Commit();
        //        Obj_Conexion.Close();
        //    }
        //    catch (OracleException Ex)
        //    {
        //        if (Obj_Transaccion != null)
        //        {
        //            Obj_Transaccion.Rollback();
        //        }
        //        switch (Ex.Code.ToString())
        //        {
        //            case "2291":
        //                Mensaje = "Error: No existe un registro relacionado con esta operacion [" + Ex.Message + "]";
        //                break;
        //            case "923":
        //                Mensaje = "Error: Consulta SQL [" + Ex.Message + "]";
        //                break;
        //            case "12170":
        //                Mensaje = "Error: Conexion con el Servidor [" + Ex.Message + "]";
        //                break;
        //            default:
        //                Mensaje = "Error:  [" + Ex.Message + "]";
        //                break;
        //        }

        //        throw new Exception(Mensaje, Ex);
        //    }
        //    finally
        //    {
        //        Obj_Comando = null;
        //        Obj_Conexion = null;
        //        Obj_Transaccion = null;
        //    }
        //}

        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Consulta_Impuestos
        ///// DESCRIPCION:            Realizar la consulta de los impuestos por criterio de busqueda o por un ID
        ///// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
        ///// CREO       :            Noe Mosqueda Valadez
        ///// FECHA_CREO :            21/Octubre/2010 16:57 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        //public static DataTable Consulta_Impuestos(Cls_Cat_Com_Impuestos_Negocio Datos)
        //{
        //    //Declaracion de variables
        //    String Mi_SQL = String.Empty;

        //    try
        //    {
        //        //Asignar consulta
        //        Mi_SQL = "SELECT " + Cat_Com_Impuestos.Campo_Impuesto_ID + "," + Cat_Com_Impuestos.Campo_Nombre + ",";
        //        Mi_SQL = Mi_SQL + Cat_Com_Impuestos.Campo_Porcentaje_Impuesto + "," + Cat_Com_Impuestos.Campo_Comentarios + " ";
        //        Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos + " ";

        //        //Verificar si hay ID
        //        if (Datos.P_Impuesto_ID != "" && Datos.P_Impuesto_ID != null && Datos.P_Impuesto_ID != String.Empty)
        //            Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Impuestos.Campo_Impuesto_ID + " = '" + Datos.P_Impuesto_ID + "' ";
        //        else
        //            Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Impuestos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%' ";

        //        //Ordenarlo
        //        Mi_SQL = Mi_SQL + "ORDER BY " + Cat_Com_Impuestos.Campo_Nombre;

        //        //Entregar resultado
        //        return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        //    }
        //    catch (OracleException ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //    catch (DBConcurrencyException ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //    }
        //}

        /////*******************************************************************************
        ///// NOMBRE DE LA CLASE:     Maximo_Impuesto_ID
        ///// DESCRIPCION:            Realizar la consulta del Maximo ID registro de los impuestos
        ///// PARAMETROS :            
        ///// CREO       :            Noe Mosqueda Valadez
        ///// FECHA_CREO :            21/Octubre/2010 17:04 
        ///// MODIFICO          :
        ///// FECHA_MODIFICO    :
        ///// CAUSA_MODIFICACION:
        /////*******************************************************************************/
        //public static string Maximo_Impuesto_ID()
        //{
        //    //Declaracion de variables
        //    String Mi_SQL = String.Empty;

        //    try
        //    {
        //        //Asignar consulta para el Maximo ID
        //        Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Impuestos.Campo_Impuesto_ID + "), '00000') FROM " + Cat_Com_Impuestos.Tabla_Cat_Impuestos;

        //        //Entregar resultado
        //        return Convert.ToString(OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL)).Trim();
        //    }
        //    catch (OracleException ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //    catch (DBConcurrencyException ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new Exception("Error: " + ex.Message);
        //    }
        //    finally
        //    {
        //}
       //}
    }
}