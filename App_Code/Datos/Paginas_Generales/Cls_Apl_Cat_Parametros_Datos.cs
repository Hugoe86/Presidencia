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
using Presidencia.Parametros.Negocios;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Apl_Cat_Parametros_Datos
/// </summary>

namespace Presidencia.Parametros.Datos
{
    public class Cls_Apl_Cat_Parametros_Datos
    {
        public Cls_Apl_Cat_Parametros_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Paramentros
        ///DESCRIPCIÓN:Metodo que ejecuta la sentencia de oracle  
        ///PARAMETROS:  1.- Cls_Apl_Cat_Parametros_Negocio Parametros
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 06/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public void Modificar_Paramentros(Cls_Apl_Cat_Parametros_Negocio Parametros)
        {
            String Mi_SQL = "UPDATE " + Apl_Parametros.Tabla_Apl_Parametros +
                        " SET " + Apl_Parametros.Campo_Correo_Saliente + " = '" + Parametros.P_Correo_Saliente +"', " +
                        Apl_Parametros.Campo_Servidor_Correo + " = '" + Parametros.P_Servidor_Correo + "', " +
                        Apl_Parametros.Campo_Usuario_Correo + " = '" + Parametros.P_Usuario_Correo + "'," +
                        Apl_Parametros.Campo_Password_Correo + " = '" + Parametros.P_Password_Correo + "'," +
                        Apl_Parametros.Campo_Usuario_Modifico + " = '" + Parametros.P_Usuario + "'," +
                        Apl_Parametros.Campo_Fecha_Modifico + " = SYSDATE ";
                        
            //Sentencia que ejecuta el query
            OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Parametros
        ///DESCRIPCIÓN:Metodo que ejecuta la sentencia de oracle  
        ///PARAMETROS:  1.- Cls_Apl_Cat_Parametros_Negocio Parametros
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 06/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Parametros()
        {
            String Mi_SQL = "SELECT " + Apl_Parametros.Campo_Correo_Saliente + ", " +
                            Apl_Parametros.Campo_Servidor_Correo + ", " +
                            Apl_Parametros.Campo_Usuario_Correo + ", " +
                            Apl_Parametros.Campo_Password_Correo + " FROM " +
                            Apl_Parametros.Tabla_Apl_Parametros;
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

    }
}