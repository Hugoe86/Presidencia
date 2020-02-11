using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using System.Data;

/// <summary>
/// Summary description for Cls_Ope_Pre_Pae_Depositario_Datos
/// </summary>
public class Cls_Ope_Pre_Pae_Depositario_Datos
{
	public Cls_Ope_Pre_Pae_Depositario_Datos()
	{}
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Alta_Depositario
    ///DESCRIPCIÓN: Da de alta en la Base de Datos un nuevo Depositario.
    ///PARAMENTROS:     
    ///CREO: Angel Antonio Escamilla Trejo
    ///FECHA_CREO: 21/Marzo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO 
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    public static void Alta_Pae_Det_Etapas_Depositario(Cls_Ope_Pre_Pae_Depositario_Negocios Pae_Depositario)
    {
        String Mi_SQL = "";
        String No_Cambio_Depositario = Obtener_ID_Consecutivo(Ope_Pre_Pae_Depositarios.Tabla_Ope_Pre_Pae_Depositarios, Ope_Pre_Pae_Depositarios.Campo_No_Cambio_Depositario, 5);
        Mi_SQL = "INSERT INTO " + Ope_Pre_Pae_Depositarios.Tabla_Ope_Pre_Pae_Depositarios;
        Mi_SQL += " (" + Ope_Pre_Pae_Depositarios.Campo_No_Cambio_Depositario + ", " + Ope_Pre_Pae_Depositarios.Campo_No_Detalle_Etapa;
        Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Figura + ", " + Ope_Pre_Pae_Depositarios.Campo_Nombre_Depositario;
        Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Domicilio_Depositario + ", " + Ope_Pre_Pae_Depositarios.Campo_Fecha_Remocion;
        Mi_SQL += ", " + Ope_Pre_Pae_Depositarios.Campo_Usuario_Creo + ", " + Ope_Pre_Pae_Depositarios.Campo_Fecha_Creo + ")";
        Mi_SQL += " VALUES ('" + No_Cambio_Depositario + "'";
        Mi_SQL += ", '" + Pae_Depositario.P_No_Detalle_Etapa + "'";
        Mi_SQL += ", '" + Pae_Depositario.P_Figura + "'";
        Mi_SQL += ", '" + Pae_Depositario.P_Nombre_Depositario + "'";
        Mi_SQL += ", '" + Pae_Depositario.P_Domicilio_Depositario + "'";
        Mi_SQL += ", SYSDATE";
        Mi_SQL += ", '" + Cls_Sessiones.Nombre_Empleado.ToUpper() + "'";
        Mi_SQL += ", SYSDATE";
        Mi_SQL += ")";
        Ejecuta_Consulta(Mi_SQL);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_No_Entrega
    ///DESCRIPCIÓN: Obtiene el numero de entrega que tiene cierto despacho
    ///PARÁMETROS:     
    ///CREO: Armando Zavala Moreno.
    ///FECHA_CREO: 16/Febrero/2012 
    ///MODIFICO             : 
    ///FECHA_MODIFICO       : 
    ///CAUSA_MODIFICACIÓN   : 
    ///*******************************************************************************
    private static void Ejecuta_Consulta(String Mi_SQL)
    {
        String Mensaje = "";
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
            Cmd.CommandText = Mi_SQL;
            Cmd.ExecuteNonQuery();
            Trans.Commit();
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
                Mensaje = "Error al intentar dar de Alta un Registro del PAE Etapas. Error: [" + Ex.Message + "]"; //"Error general en la base de datos"
            }
            //Indicamos el mensaje 
            throw new Exception(Mensaje);
        }
        finally
        {
            if (Cn.State == ConnectionState.Open)
            {
                Cn.Close();
            }
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
        String Id = Convertir_A_Formato_ID(1, Longitud_ID); ;
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
}
