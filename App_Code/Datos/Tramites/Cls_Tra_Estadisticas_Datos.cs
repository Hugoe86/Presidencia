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
using Presidencia.Estadisticas_Tramites.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Tra_Estadisticas_Datos
/// </summary>
/// 
namespace Presidencia.Estadisticas_Tramites.Datos
{

    public class Cls_Tra_Estadisticas_Datos
    {
        public Cls_Tra_Estadisticas_Datos()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        
        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Tramites
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet de la tabla de Cat_Tra_Tramites para llenar el Grid_View
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 14/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public DataSet Consulta_Tramites()
        {
            String Mi_SQL = "SELECT " + Cat_Tra_Tramites.Campo_Nombre +
                        " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites +
                        " ORDER BY " + Cat_Tra_Tramites.Campo_Nombre;
            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Solicitudes
        ///DESCRIPCIÓN: Metodo que obtiene el DataSet de los acumulados de las colicitudes
        ///PARAMETROS:  1.- Cls_Tra_Estadisticas_Negocio Estadisticas_Negocio= Objeto de la clase de Negocio
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 18/Octubre/2010 
        ///MODIFICO:  
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///******************************************************************************
        public DataSet Consulta_Solicitudes(Cls_Tra_Estadisticas_Negocio Estadisticas_Negocio)
        {
            String Mi_SQL = "";
            Boolean Buscar_Totales = false;

            for (int Contador_For = 0; Contador_For < Estadisticas_Negocio.P_Tramites.Length; Contador_For++)
            {
                Mi_SQL = Mi_SQL + "SELECT TRAMITES." + Cat_Tra_Tramites.Campo_Nombre + " AS TRAMITE," +
                        " SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus +
                        " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITES" +
                        " JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                        " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = " +
                        " TRAMITES." + Cat_Tra_Tramites.Campo_Tramite_ID;

                Mi_SQL += " WHERE TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                        ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Estadisticas_Negocio.P_Fecha_Inicial)) +
                        "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo + 
                        ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Estadisticas_Negocio.P_Fecha_Final)) + "')" +
                        " AND TRAMITES." + Cat_Tra_Tramites.Campo_Nombre + " = '" + Estadisticas_Negocio.P_Tramites[Contador_For] + "'";

                //  filtro de estatus
                if (!String.IsNullOrEmpty(Estadisticas_Negocio.P_Estatus))
                {
                    Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Estatus + "='" + Estadisticas_Negocio.P_Estatus + "' ";
                    Buscar_Totales = true;
                }
                //  filtro de porcentaje de avance
                if (!String.IsNullOrEmpty(Estadisticas_Negocio.P_Porcentaje))
                {
                    Mi_SQL += " AND SOLICITUD." + Ope_Tra_Solicitud.Campo_Porcentaje_Avance + "='" + Estadisticas_Negocio.P_Porcentaje + "' ";
                    Buscar_Totales = true;
                }
                
                //  para obtener el total de las solicitudes
                if (Buscar_Totales == false)
                {
                    Mi_SQL += " UNION ALL SELECT TRAMITES." + Cat_Tra_Tramites.Campo_Nombre +
                        " AS TRAMITE, 'TOTAL' AS ESTATUS" +
                        " FROM " + Cat_Tra_Tramites.Tabla_Cat_Tra_Tramites + " TRAMITES" +
                        " JOIN " + Ope_Tra_Solicitud.Tabla_Ope_Tra_Solicitud + " SOLICITUD" +
                        " ON SOLICITUD." + Ope_Tra_Solicitud.Campo_Tramite_ID + " = " +
                        " TRAMITES." + Cat_Tra_Tramites.Campo_Tramite_ID;
                    Mi_SQL += " WHERE TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                      ") >= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Estadisticas_Negocio.P_Fecha_Inicial)) +
                      "') AND TO_DATE(SOLICITUD." + Ope_Tra_Solicitud.Campo_Fecha_Creo +
                      ") <= TO_DATE('" + String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Estadisticas_Negocio.P_Fecha_Final)) + "')" +
                      " AND TRAMITES." + Cat_Tra_Tramites.Campo_Nombre + " = '" + Estadisticas_Negocio.P_Tramites[Contador_For] + "'";
                }

                //  para unir las consultas
                if (Contador_For < (Estadisticas_Negocio.P_Tramites.Length - 1))
                {
                    Mi_SQL = Mi_SQL + " UNION ALL ";
                }
                //  se reinicia el Boolean
                Buscar_Totales = false;
            }

            DataSet Data_Set = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            return Data_Set;

        }
        #endregion

    }
}