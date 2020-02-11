﻿using System;
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
using Presidencia.Operacion_Com_Parametros_Directores.Negocio;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

namespace Presidencia.Operacion_Com_Parametros_Directores.Datos
{

public class Cls_Ope_Com_Parametros_Directores_Datos
{
    public Cls_Ope_Com_Parametros_Directores_Datos()
	{		
	}
    ///*******************************************************************************
    /// NOMBRE DE LA CLASE:     Modificar_Paramentros
    /// DESCRIPCION:            Modificar los parametros
    /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos a modificar
    /// CREO       :            Jesus Toledo Rodriguez
    /// FECHA_CREO :            20/Abril/2010 13:42 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public static void Modificar_Paramentros(Cls_Ope_Com_Directores_Parametros_Negocio Datos)
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

            //Asignar consulta para la modificacion
            Mi_SQL = "UPDATE " + Cat_Com_Directores.Tabla_Cat_Com_Directores;
            Mi_SQL = Mi_SQL + " SET " + Cat_Com_Directores.Campo_Director_Adquisiciones + " = '" + Datos.P_Director_Adquisiciones + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Directores.Campo_Oficial_Mayor + " = '" + Datos.P_Oficial_Mayor + "', ";
            Mi_SQL = Mi_SQL + Cat_Com_Directores.Campo_Tesorero + " = '" + Datos.P_Tesorero + "' ";            

            //Ejecutar consulta
            Obj_Comando.CommandText = Mi_SQL;
            Obj_Comando.ExecuteNonQuery();

            //Ejecutar transaccion
            Obj_Transaccion.Commit();
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
    /// NOMBRE DE LA CLASE:     Consulta_Paramentros
    /// DESCRIPCION:            Consultar los parametros
    /// PARAMETROS :            
    /// CREO       :            Jesus Toledo Rodriguez
    /// FECHA_CREO :            20/Abril/2010 13:42 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************/
    public static DataTable Consulta_Paramentros()
    {
        //Declaracion de Variables
        String Mi_SQL = String.Empty; //Variable para las consultas

        try
        {            
            //Asignar consulta para la modificacion
            Mi_SQL = "SELECT * FROM " + Cat_Com_Directores.Tabla_Cat_Com_Directores;

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

}
}