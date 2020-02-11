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

/// <summary>
/// Summary description for Cls_Cat_Com_Marcas_Productos_Datos
/// </summary>
public class Cls_Cat_Com_Marcas_Productos_Datos
{
	public Cls_Cat_Com_Marcas_Productos_Datos()
	{
	}

    ///****************************************************************************************
    /// NOMBRE DE LA FUNCION:  Alta_Marca_Producto
    /// DESCRIPCION :          1.Consulta el ultimo ID dado de alta para poder ingresar el siguiente
    ///                        2. Da de Alta la Marca del Producto en la BD con los datos proporcionados por el usuario
    /// PARAMETROS  :          Datos: Variable que contiene los datos que serán insertados en la base de datos
    /// CREO        :          Luz Veronica Gomez Garcia
    /// FECHA_CREO  :          02/Octubre/2010 9:58
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************/
    public static void Alta_Marca_Producto(Cls_Cat_Com_Marcas_Productos_Negocio Datos)
    {
        //Declaracion de variables
        OracleTransaction Obj_Transaccion = null;
        OracleConnection Obj_Conexion;
        OracleCommand Obj_Comando;
        String Mi_SQL = String.Empty;
        Object Aux; //Variable auxiliar para las consultas
        String Mensaje = String.Empty; //Variable para el mensaje de error
        
        try
        {
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            //Asignar consulta del maximo ID
            Mi_SQL = "SELECT NVL(MAX(" + Cat_Com_Marcas_Productos.Campo_Marca_ID + "), '00000') ";
            Mi_SQL = Mi_SQL + "FROM " + Cat_Com_Marcas_Productos.Tabla_Cat_Com_Marcas_Productos;

            //Ejecutar consulta
            Obj_Comando.CommandText = Mi_SQL;
            Aux = Obj_Comando.ExecuteScalar();

            //Verificar si no es nulo
            if (Convert.IsDBNull(Aux) == false)
            {
                Datos.P_Marca_ID = String.Format("{0:00000}", Convert.ToInt32(Aux) + 1);
            }
            else
                Datos.P_Marca_ID = "00001";

            //Asignar consulta para insercion
            Mi_SQL = "INSERT INTO " + Cat_Com_Marcas_Productos.Tabla_Cat_Com_Marcas_Productos + "(" + Cat_Com_Marcas_Productos.Campo_Marca_ID + ",";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas_Productos.Campo_Nombre + "," + Cat_Com_Marcas_Productos.Campo_Comentarios + ",";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas_Productos.Campo_Usuario_Creo + "," + Cat_Com_Marcas_Productos.Campo_Fecha_Creo + ") VALUES(";
            Mi_SQL = Mi_SQL + "'" + Datos.P_Marca_ID + "','" + Datos.P_Nombre + "','" + Datos.P_Comentarios + "','" + Datos.P_Usuario_Creo + "',SYSDATE)";

            //Ejecutar consulta
            Obj_Comando.CommandText = Mi_SQL;
            Obj_Comando.ExecuteNonQuery();

            //Ejecutar transaccion
            Obj_Transaccion.Commit();
            Obj_Conexion.Close();
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

    ///****************************************************************************************
    /// NOMBRE DE LA FUNCION:  Baja_Marca_Producto
    /// DESCRIPCION :          Eliminar una Marca de Producto existente de acuerdo a los datos proporcionados por el usuario
    /// PARAMETROS  :          Datos: Variable que contiene los datos del elemento a eliminar
    /// CREO        :          Luz Veronica Gomez Garcia
    /// FECHA_CREO  :          02/Octubre/2010 10:03
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************/
    public static void Baja_Marca_Producto(Cls_Cat_Com_Marcas_Productos_Negocio Datos)
    {
        //Declaracion de variables
        OracleTransaction Obj_Transaccion = null;
        OracleConnection Obj_Conexion;
        OracleCommand Obj_Comando;
        String Mi_SQL = String.Empty;
        String Mensaje = String.Empty; //Variable para el mensaje de error

        try
        {
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            //Asignar la consulta de la baja
            Mi_SQL = "DELETE FROM " + Cat_Com_Marcas_Productos.Tabla_Cat_Com_Marcas_Productos + " ";
            Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Marcas_Productos.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "'";

            //Ejecutar consulta
            Obj_Comando.CommandText = Mi_SQL;
            Obj_Comando.ExecuteNonQuery();

            //Ejecutar transaccion
            Obj_Transaccion.Commit();
            Obj_Conexion.Close();
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

    ///****************************************************************************************
    /// NOMBRE DE LA FUNCION:  Cambio_Marca_Producto
    /// DESCRIPCION :          Modificar una Marca de Producto existente de acuerdo a los datos proporcionados por el usuario
    /// PARAMETROS  :          Datos: Variable que contiene los datos del elemento a modificar
    /// CREO        :          Luz Veronica Gomez Garcia
    /// FECHA_CREO  :          02/Octubre/2010 10:10
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************/
    public static void Cambio_Marca_Producto(Cls_Cat_Com_Marcas_Productos_Negocio Datos)
    {
        //Declaracion de variables
        OracleTransaction Obj_Transaccion = null;
        OracleConnection Obj_Conexion;
        OracleCommand Obj_Comando;
        String Mi_SQL = String.Empty;
        String Mensaje = String.Empty; //Variable para el mensaje de error

        try
        {
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            //Asignar consulta para modificacion de una marca de producto
            Mi_SQL = "UPDATE " + Cat_Com_Marcas_Productos.Tabla_Cat_Com_Marcas_Productos + " ";
            Mi_SQL = Mi_SQL + "SET " + Cat_Com_Marcas_Productos.Campo_Nombre + " = '" + Datos.P_Nombre + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas_Productos.Campo_Comentarios + " = '" + Datos.P_Comentarios + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas_Productos.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Creo + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas_Productos.Campo_Fecha_Modifico + " = SYSDATE ";
            Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Marcas_Productos.Campo_Marca_ID + " = '" + Datos.P_Marca_ID + "'";

            //Ejecutar consulta
            Obj_Comando.CommandText = Mi_SQL;
            Obj_Comando.ExecuteNonQuery();

            //Ejecutar transaccion
            Obj_Transaccion.Commit();
            Obj_Conexion.Close();
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

    ///****************************************************************************************
    /// NOMBRE DE LA FUNCION:  Consulta_Proveedores
    /// DESCRIPCION :          COnsultar los proveedores de acuerdo al criterio de busqueda proporcionado por el usuario
    /// PARAMETROS  :          Datos: Variable que contiene los datos para la busqueda
    /// CREO        :          Noe Mosqueda Valadez
    /// FECHA_CREO  :          27/Septiembre/2010 18:34
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///****************************************************************************************/
    public static DataTable Consulta_Marcas_Productos(Cls_Cat_Com_Marcas_Productos_Negocio Datos)
    {
        String Mi_SQL = ""; //Variable para las consultas de SQL

        try
        {
            //Asignar consulta para la busqueda
            Mi_SQL = "SELECT " + Cat_Com_Marcas_Productos.Campo_Marca_ID + ", " + Cat_Com_Marcas_Productos.Campo_Nombre + ", ";
            Mi_SQL = Mi_SQL + Cat_Com_Marcas_Productos.Campo_Comentarios + " FROM " + Cat_Com_Marcas_Productos.Tabla_Cat_Com_Marcas_Productos + " ";

            //Condiciones de la busqueda
            Mi_SQL = Mi_SQL + "WHERE " + Cat_Com_Marcas_Productos.Campo_Nombre + " LIKE '%" + Datos.P_Nombre + "%'";

            //Entregar consulta
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
}
