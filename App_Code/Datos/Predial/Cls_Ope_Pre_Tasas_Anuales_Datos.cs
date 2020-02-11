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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;

namespace Presidencia.Operacion.Predial_Tasas_Anuales.Datos
{
    
    public class Cls_Ope_Pre_Tasas_Anuales_Datos
    {

        #region
        public Cls_Ope_Pre_Tasas_Anuales_Datos()
        {            
        }
        ///******************************************************************************* 
        ///NOMBRE DE LA FUNCIÓN: Consultar_Tasas_Anuales
        ///DESCRIPCIÓN: Realizar la consulta y llenar el grid con estos datos
        ///PARAMETROS: 
        ///CREO: jtoledo
        ///FECHA_CREO: 21/Ago/2011 12:14:35 p.m.
        ///MODIFICO: 
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataTable Consultar_Tasas_Anuales(Cls_Ope_Pre_Tasas_Anuales_Negocio Datos)
        {
            String Mi_SQL = ""; //Variable para la consulta SQL                        
            
            try
            {
                Mi_SQL = "";
                Mi_SQL += "SELECT ";
                Mi_SQL += " TASA." +
                Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID + " AS TASA_ID, " + "TASA." +
                Cat_Pre_Tasas_Predial.Campo_Identificador + " AS IDENTIFICADOR, " + "TASA." +
                Cat_Pre_Tasas_Predial.Campo_Descripcion + " AS DESCRIPCION, ";
                Mi_SQL += " ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " AS TASA_ANUAL_ID, " + "ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID + " AS TASA_PREDIAL_ID, " + "ANUAL." +
                Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " AS TASA_ANUAL, " + "ANUAL." +                
                Cat_Pre_Tasas_Predial_Anual.Campo_Año + " AS ANIO ";
                Mi_SQL += " FROM " + Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial + " TASA LEFT OUTER JOIN " +
                    Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " ANUAL ON ANUAL." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Predial_ID +
                    " = TASA." + Cat_Pre_Tasas_Predial.Campo_Tasa_Predial_ID;

                if (Datos.P_Tasa_Predial_ID != "" && Datos.P_Tasa_Predial_ID != null)
                {
                    Mi_SQL += " WHERE " 
                        + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + " = '" + Datos.P_Tasa_Predial_ID + "'";
                }
                else
                {
                    if (Datos.P_Anio != "" && Datos.P_Anio != null)
                    {
                        Mi_SQL += " WHERE "
                            + Cat_Pre_Tasas_Predial_Anual.Campo_Año + " = '" + Datos.P_Anio + "'";
                        if (Datos.P_Descripcion != "" && Datos.P_Descripcion != null)
                            Mi_SQL += " AND " + Cat_Pre_Tasas_Predial.Campo_Descripcion + " = '" + Datos.P_Descripcion + "'";

                    }
                    else if (Datos.P_Identificador != "" && Datos.P_Identificador != null)
                    {
                        Mi_SQL += " WHERE "
                            + Cat_Pre_Tasas_Predial.Campo_Identificador + " = '" + Datos.P_Identificador + "'";
                    }
                    else if (Datos.P_Descripcion != "" && Datos.P_Descripcion != null)
                    {
                        Mi_SQL += " WHERE "
                               + Cat_Pre_Tasas_Predial.Campo_Descripcion + " = '" + Datos.P_Descripcion + "'";
                    }
                    if (Mi_SQL.Contains("WHERE"))
                        Mi_SQL += " AND TASA." + Cat_Pre_Tasas_Predial.Campo_Estatus + " NOT IN ('BAJA') ";
                    else
                        Mi_SQL += " WHERE TASA." + Cat_Pre_Tasas_Predial.Campo_Estatus + " NOT IN ('BAJA') ";
                }
                Mi_SQL += " ORDER BY ANUAL." + Cat_Pre_Tasas_Predial_Anual.Campo_Año +" DESC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Mi_SQL + Ex.Message);
            }

            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            finally
            {
            }
        }

        #endregion
    }
}