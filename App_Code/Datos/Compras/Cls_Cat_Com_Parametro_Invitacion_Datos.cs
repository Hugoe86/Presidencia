using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using Presidencia.Parametro_Invitacion.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
namespace Presidencia.Parametro_Invitacion.Datos
{
    public class Cls_Cat_Com_Parametro_Invitacion_Datos
    {
        #region Metodos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consular_Parametros
        ///DESCRIPCIÓN: Metodo que ayuda a consultar los datos de los parametros para la invitacion al proveedor
        ///PARAMETROS:   1.- Cls_Cat_Com_Parametro_Invitacion_Negocio Clase_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 1/Noviembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consular_Parametros(Cls_Cat_Com_Parametro_Invitacion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "SELECT " + Cat_Com_Parametros.Campo_Invitacion_Proveedores;
            Mi_SQL = Mi_SQL + " FROM " + Cat_Com_Parametros.Tabla_Cat_Com_Parametros;

            return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Actualizar_Paremetros
        ///DESCRIPCIÓN: Metodo que ayuda a Actualizar los datos del parametro de invitacion
        ///PARAMETROS:   1.- Cls_Cat_Com_Parametro_Invitacion_Negocio Clase_Negocio objeto de la clase negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 1/Noviembre/2012
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static int Actualizar_Parametros(Cls_Cat_Com_Parametro_Invitacion_Negocio Clase_Negocio)
        {
            String Mi_SQL = "UPDATE " +  Cat_Com_Parametros.Tabla_Cat_Com_Parametros;
            Mi_SQL = Mi_SQL + " SET " + Cat_Com_Parametros.Campo_Invitacion_Proveedores;
            Mi_SQL = Mi_SQL + "='" + Clase_Negocio.P_Invitacion_Proveedores.Trim() + "'";
            int Registros_Afectados = OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Registros_Afectados;
        }
        #endregion
    }//fin class
}//namespace