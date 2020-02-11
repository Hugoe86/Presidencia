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
using Presidencia.Bandeja_Pendientes_Atencion_Ciudadana.Negocios;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
/// <summary>
/// Summary description for Cls_Bandeja_Pendientes_Datos
/// </summary>
/// 

namespace Presidencia.Bandeja_Pendientes_Atencion_Ciudadana.Datos
{
    public class Cls_Bandeja_Pendientes_Datos
    {
        #region Métodos
        public Cls_Bandeja_Pendientes_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Parametros
        ///DESCRIPCIÓN: crea una sentencia sql para 
        ///Consultar todos los campos de la Tabla Apl_Parametros
        ///PARAMETROS: 
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consultar_Parametros(Cls_Bandeja_Pendientes_Negocio Bandeja)
        {
           
            String Mi_SQL = "SELECT "+Apl_Cat_Roles.Campo_Grupo_Roles_ID+
                " FROM " + Apl_Cat_Roles.Tabla_Apl_Cat_Roles+
                " WHERE "+ Apl_Cat_Roles.Campo_Rol_ID + 
                " = '"+Bandeja.P_Rol_ID+"'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peticiones_Empleado
        ///DESCRIPCIÓN: crea una sentencia sql para 
        ///Consultar las peticiones que tiene pendientes un rol de empleado logueado
        ///PARAMETROS: 1.-Bandeja, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consultar_Peticiones_Empleado(Cls_Bandeja_Pendientes_Negocio Bandeja)
        {
            String Mi_SQL = "SELECT PETICION." + Ope_Ate_Peticiones.Campo_Folio +
                ", RPAD(SUBSTR(PETICION." + Ope_Ate_Peticiones.Campo_Descripcion_Peticion +
                ",1,27),30,'.') AS DESCRIPCION_PETICION, PETICION." +
                Ope_Ate_Peticiones.Campo_Nivel_Importancia + " FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +
                " PETICION JOIN " + Ope_Ate_Asigna_Pet_Empleado.Tabla_Ope_Asigna_Pet_Empleado +
                " ASIGNA ON PETICION." + Ope_Ate_Peticiones.Campo_Peticion_ID + " = ASIGNA." +
                Ope_Ate_Asigna_Pet_Empleado.Campo_Peticion_ID + " WHERE ASIGNA." +
                Ope_Ate_Asigna_Pet_Empleado.Campo_Empleado_ID + " = '" + Bandeja.P_Empleado_ID + "' AND ASIGNA." +
                Ope_Ate_Asigna_Pet_Empleado.Campo_Vigente + " = 'S' AND PETICION." +
                Ope_Ate_Peticiones.Campo_Estatus + " IN('PROCESO','PENDIENTE')";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Area_Empleado
        ///DESCRIPCIÓN: crea una sentencia sql para 
        ///Consultar las a que area pertence un jefe de area logueado
        ///PARAMETROS: 1.-Bandeja, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        //////*******************************************************************************
        public static DataSet Consultar_Area_Empleado(Cls_Bandeja_Pendientes_Negocio Bandeja)
        {
            String Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID + " FROM " +
                Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                " = '" + Bandeja.P_Empleado_ID + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peticiones_Jefe_Area
        ///DESCRIPCIÓN: crea una sentencia sql para 
        ///Consultar las peticiones que tiene pendientes un rol de jefe de area logueado
        ///PARAMETROS: 1.-Bandeja, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consultar_Peticiones_Jefe_Area(Cls_Bandeja_Pendientes_Negocio Bandeja)
        {
            String Mi_SQL = "SELECT PETICION." + Ope_Ate_Peticiones.Campo_Folio + ", " +
                "RPAD(SUBSTR(PETICION." + Ope_Ate_Peticiones.Campo_Descripcion_Peticion +
                ",1,27),30,'.') AS DESCRIPCION_PETICION, PETICION." +
                Ope_Ate_Peticiones.Campo_Nivel_Importancia + " FROM " +
                Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " PETICION WHERE PETICION." +
                Ope_Ate_Peticiones.Campo_Area_ID + " = '" + Bandeja.P_Area_ID + "' AND PETICION." +
                Ope_Ate_Peticiones.Campo_Estatus + " IN('PROCESO','PENDIENTE')";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencia_Empleado
        ///DESCRIPCIÓN: crea una sentencia sql para 
        ///Consultar las a que area pertence un jefe de dependencia logueado
        ///PARAMETROS: 1.-Bandeja, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        //////*******************************************************************************
        public static DataSet Consultar_Dependencia_Empleado(Cls_Bandeja_Pendientes_Negocio Bandeja)
        {
            String Mi_SQL = "SELECT " + Cat_Areas.Campo_Dependencia_ID + " FROM " +
                Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID +
                " = '" + Bandeja.P_Empleado_ID + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Peticiones_Jefe_Dependencias
        ///DESCRIPCIÓN: crea una sentencia sql para 
        ///Consultar las peticiones que tiene pendientes un rol de jefe de dependencia logueado
        ///PARAMETROS: 1.-Bandeja, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consultar_Peticiones_Jefe_Dependencias(Cls_Bandeja_Pendientes_Negocio Bandeja) {
            String Mi_SQL = "";
        
            for (int i = 0; i < Bandeja.P_Areas.Length; i++) {
                Mi_SQL = Mi_SQL + "SELECT DISTINCT (SELECT COUNT(PETICION."+
                    Ope_Ate_Peticiones.Campo_Peticion_ID+") "+
                    Complementa_Sentencia_Sql(Bandeja) + " AND PETICION."+
                    Ope_Ate_Peticiones.Campo_Area_ID+" = '"+Bandeja.P_Areas[i]+
                    "' AND PETICION." +
                Ope_Ate_Peticiones.Campo_Estatus + " IN('PROCESO','PENDIENTE')) AS PETICIONES, AREA." + Cat_Areas.Campo_Nombre + " AS AREA " +
                    Complementa_Sentencia_Sql(Bandeja)+" AND PETICION.AREA_ID = '"+
                    Bandeja.P_Areas[i] + "' AND PETICION." +
                Ope_Ate_Peticiones.Campo_Estatus + " IN('PROCESO','PENDIENTE')";

                if (i < (Bandeja.P_Areas.Length - 1)) {
                    Mi_SQL = Mi_SQL + " UNION ALL ";
                }
             }
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;        
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Complementa_Sentencia_Sql
        ///DESCRIPCIÓN: crea un complemento de para sentencia sql con un codigo
        ///sql repetitivo en la sentencia original
        ///PARAMETROS: 1.-Bandeja, objeto de la calse de negocio que contiene los datos para realizar la consulta   
        ///CREO: Silvia Morales Portuhondo
        ///FECHA_CREO: 1/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        //////*******************************************************************************
        public static String Complementa_Sentencia_Sql(Cls_Bandeja_Pendientes_Negocio Bandeja) {
            String Mi_SQL = "FROM "+Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones+
                " PETICION JOIN "+Cat_Areas.Tabla_Cat_Areas+" AREA ON AREA."+
                Cat_Areas.Campo_Area_ID+" = PETICION."+Ope_Ate_Peticiones.Campo_Area_ID+
                " WHERE PETICION."+Ope_Ate_Peticiones.Campo_Dependencia_ID+" = '"+
                Bandeja.P_Dependencia_ID+"'";
            return Mi_SQL;
        }
        #endregion 
    }
}