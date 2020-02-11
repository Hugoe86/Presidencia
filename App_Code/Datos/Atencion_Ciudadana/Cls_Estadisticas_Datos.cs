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
using Presidencia.Constantes;
using System.Windows.Forms;
using Presidencia.Estadisticas.Negocios;


namespace Presidencia.Estadisticas.Datos
{
    public class Cls_Estadisticas_Datos
    {
        public Cls_Estadisticas_Datos()
        {
        }
        /********************************************************************************************************
        * Seccion de Metodos
        *********************************************************************************************************/
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Dependencia
        ///DESCRIPCIÓN: Metodo que obtiene el dataset de las dependencia existentes
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Dependencia(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "";

            Mi_SQL = "SELECT " + Cat_Dependencias.Campo_Dependencia_ID + "," + Cat_Dependencias.Campo_Nombre +
                     " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias;
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Asuntos
        ///DESCRIPCIÓN: Metodo que obtiene el dataset los asuntos dependiendo de la dependencia seleccionada
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Asuntos(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = " SELECT " + Cat_Ate_Asuntos.Campo_AsuntoID + " FROM " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos +
                   " WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = '" + Estadisticas.P_Dependencias[0] + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Areas
        ///DESCRIPCIÓN: Metodo que obtiene el dataset de las areas dependiendo de la dependencia seleccionada
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 08/Septiembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Consulta_Areas(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "SELECT " + Cat_Areas.Campo_Area_ID + "," + Cat_Areas.Campo_Nombre + " FROM " + Cat_Areas.Tabla_Cat_Areas +
                   " WHERE " + Cat_Areas.Campo_Dependencia_ID + " = " + Estadisticas.P_Dependencia_Area;
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grafica_Pastel
        ///DESCRIPCIÓN: Metodo que obtiene el dataset para realizar la grafica de pastel. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Grafica_Pastel(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "SELECT " + Ope_Ate_Peticiones.Campo_Estatus + " FROM " +
                   Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " WHERE TO_DATE(TO_CHAR(" +
                   Ope_Ate_Peticiones.Campo_Fecha_Peticion +
                   ",'DD/MM/YY')) BETWEEN " +
                   "'" + Estadisticas.P_Fechas_Inicio + "' AND '" + Estadisticas.P_Fecha_Fin +
                   "' UNION ALL SELECT 'TOTAL' AS " + Ope_Ate_Peticiones.Campo_Estatus + " FROM " +
                   Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +
                   " WHERE TO_DATE(TO_CHAR(" + Ope_Ate_Peticiones.Campo_Fecha_Peticion +
                   ",'DD/MM/YY')) BETWEEN " +
                   "'" + Estadisticas.P_Fechas_Inicio + "' AND '" + Estadisticas.P_Fecha_Fin + "'";
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Dependencias_Acumulados
        ///DESCRIPCIÓN: Metodo que obtiene el dataset para realizar la grafica de acumulados por dependencias. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Dependencias_Acumulados(Cls_Estadisticas_Negocio Estadisticas)
        {

            String Mi_SQL = "";

            for (int i = 0; i < Estadisticas.P_Dependencias.Length; i++)
            {
                Mi_SQL = Mi_SQL + "SELECT DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                " AS DEPENDENCIA, PETICION." + Ope_Ate_Peticiones.Campo_Estatus +
                " " + Auxiliar_Dependencias(Estadisticas) +
                "  AND PETICION." + Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                Estadisticas.P_Dependencias[i] + "' UNION" +
                " ALL SELECT DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                " AS DEPENDENCIA, 'TOTAL' AS " + Ope_Ate_Peticiones.Campo_Estatus +
                " " + Auxiliar_Dependencias(Estadisticas) + " AND PETICION." +
                Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" + Estadisticas.P_Dependencias[i] + "'";
                if (i < (Estadisticas.P_Dependencias.Length - 1))
                {
                    Mi_SQL = Mi_SQL + " UNION ALL ";
                }//fin del if
            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            return Data_Set;
        }//fin de Generar_Grafica_Dependencias_Acumulados

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Auxiliar_Dependencias
        ///DESCRIPCIÓN: Metodo auxiliar que contiene los join's y los where(condiciones) para obtener los datos
        ///             que cumplan esa condicion. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Auxiliar_Dependencias(Cls_Estadisticas_Negocio Estadisticas)
        {

            String Mi_SQL = "FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +
                           " PETICION JOIN " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " ASUNTO ON" +
                           " PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = ASUNTO." +
                           Cat_Ate_Asuntos.Campo_AsuntoID + " JOIN " +
                           Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA" +
                           " ON ASUNTO." + Cat_Ate_Asuntos.Campo_DependenciaID + " = DEPENDENCIA." +
                           Cat_Dependencias.Campo_Dependencia_ID +
                           " WHERE TO_DATE(TO_CHAR(PETICION." +
                           Ope_Ate_Peticiones.Campo_Fecha_Peticion + ",'DD/MM/YY')) BETWEEN '" +
                           Estadisticas.P_Fechas_Inicio + "' AND '" + Estadisticas.P_Fecha_Fin + "'";
            return Mi_SQL;
        }//fin de Auxiliar_Dependencias

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Areas_Acumulados
        ///DESCRIPCIÓN: Metodo que obtiene el dataset para realizar la grafica de acumulados de areas por dependencia. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Areas_Acumulados(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "";


            for (int i = 0; i < Estadisticas.P_Areas.Length; i++)
            {
                Mi_SQL = Mi_SQL + "SELECT DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                   " AS DEPENDENCIA, AREA." + Cat_Areas.Campo_Nombre + " AS AREA," +
                   " PETICION.ESTATUS " + Auxiliar_Areas(Estadisticas) + " AND " +
                   " PETICION." + Ope_Ate_Peticiones.Campo_Area_ID + " = '" + Estadisticas.P_Areas[i] +
                   "' UNION ALL" +
                   " SELECT DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                   " AS DEPENDENCIA, AREA." + Cat_Areas.Campo_Nombre + " AS AREA, 'TODOS' AS " +
                   Ope_Ate_Peticiones.Campo_Estatus + " " + Auxiliar_Areas(Estadisticas) +
                   " AND PETICION." + Ope_Ate_Peticiones.Campo_Area_ID + " =  '" + Estadisticas.P_Areas[i] + "'";

                if (i < (Estadisticas.P_Areas.Length - 1))
                {
                    Mi_SQL = Mi_SQL + " UNION ALL ";
                }//fin del if
            }
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Auxiliar_Areas
        ///DESCRIPCIÓN: Metodo auxiliar que contiene los join's y los where(condiciones) para obtener los datos
        ///             que cumplan esa condicion. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Auxiliar_Areas(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "FROM " + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones +
                           " PETICION JOIN " + Cat_Ate_Asuntos.Tabla_Cat_Ate_Asuntos + " ASUNTO ON" +
                           " PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = ASUNTO." +
                           Cat_Ate_Asuntos.Campo_AsuntoID + " JOIN " +
                           Cat_Areas.Tabla_Cat_Areas + " AREA ON AREA." +
                           Cat_Areas.Campo_Area_ID + " = ASUNTO." + Cat_Ate_Asuntos.Campo_AreaID +
                           " JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " DEPENDENCIA" +
                           " ON DEPENDENCIA." + Cat_Dependencias.Campo_Dependencia_ID + " = " +
                           "AREA." + Cat_Areas.Campo_Dependencia_ID + " WHERE TO_DATE(TO_CHAR(PETICION." +
                           Ope_Ate_Peticiones.Campo_Fecha_Peticion + ",'DD/MM/YY')) BETWEEN '" +
                           Estadisticas.P_Fechas_Inicio + "' AND '" + Estadisticas.P_Fecha_Fin + "'";
            return Mi_SQL;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Tiempos_Dependencias
        ///DESCRIPCIÓN: Metodo que obtiene el dataset para realizar la grafica de tiempos por dependencias. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************

        public DataSet Generar_Grafica_Tiempos_Dependencias(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "";

            for (int i = 0; i < Estadisticas.P_Dependencias.Length; i++)
            {
                Mi_SQL = Mi_SQL + "SELECT DISTINCT DEPENDENCIA." + Cat_Dependencias.Campo_Nombre +
                    " AS DEPENDENCIA, (SELECT AVG (TO_DATE(TO_DATE" +
                    "(TO_CHAR(PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real + ",'DD/MM/YY')))" +
                    " - (TO_DATE(TO_CHAR(PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion +
                    ",'DD/MM/YY')))) " + Auxiliar_Dependencias(Estadisticas) +
                    " AND  PETICION." + Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                    Estadisticas.P_Dependencias[i] + "' AND " +
                    "PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " IN('POSITIVO','NEGATIVO')) AS" +
                    " DIAS " + Auxiliar_Dependencias(Estadisticas) +
                    " AND PETICION." + Ope_Ate_Peticiones.Campo_Dependencia_ID + " = '" +
                    Estadisticas.P_Dependencias[i] + "' AND " +
                    "PETICION." + Ope_Ate_Peticiones.Campo_Estatus + " IN('POSITIVO','NEGATIVO') ";

                if (i < (Estadisticas.P_Dependencias.Length - 1))
                {
                    Mi_SQL = Mi_SQL + " UNION ALL ";
                }//fin del if
            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Grafica_Tiempos_Asuntos
        ///DESCRIPCIÓN: Metodo que obtiene el dataset para realizar la grafica de tiempos por Asuntos por dependencia. 
        ///PARAMETROS:  1.- Cls_Estadisticas_Negocio Estadisticas objeto de la clase de negocios
        ///CREO: Silvia Morales, Susana Trigueros Armenta
        ///FECHA_CREO: 05/Octubre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataSet Generar_Grafica_Tiempos_Asuntos(Cls_Estadisticas_Negocio Estadisticas)
        {
            String Mi_SQL = "";
            for (int i = 0; i < Estadisticas.P_Asuntos.Length; i++)
            {
                Mi_SQL = Mi_SQL + "SELECT DISTINCT DEPENDENCIA." +
                    Cat_Dependencias.Campo_Nombre + " AS DEPENDENCIA, ASUNTO." +
                    Cat_Ate_Asuntos.Campo_Nombre + " AS ASUNTO," +
                    "(SELECT AVG (TO_DATE(TO_DATE" +
                    "(TO_CHAR(PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Solucion_Real +
                    ",'DD/MM/YY')))" +
                    " - (TO_DATE(TO_CHAR(PETICION." + Ope_Ate_Peticiones.Campo_Fecha_Peticion +
                    ",'DD/MM/YY')))) " +
                    Auxiliar_Dependencias(Estadisticas) + " AND " +
                    " PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" +
                    Estadisticas.P_Asuntos[i] + "' AND PETICION." +
                    Ope_Ate_Peticiones.Campo_Estatus + " IN('POSITIVO','NEGATIVO')) AS DIAS " +
                    Auxiliar_Dependencias(Estadisticas) +
                    " AND PETICION." + Ope_Ate_Peticiones.Campo_Asunto_ID + " = '" +
                    Estadisticas.P_Asuntos[i] + "' AND PETICION." +
                    Ope_Ate_Peticiones.Campo_Estatus + " IN('POSITIVO','NEGATIVO')";

                if (i < (Estadisticas.P_Asuntos.Length - 1))
                {
                    Mi_SQL = Mi_SQL + " UNION ALL ";
                }//fin del if
            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        
        #endregion
    }
}