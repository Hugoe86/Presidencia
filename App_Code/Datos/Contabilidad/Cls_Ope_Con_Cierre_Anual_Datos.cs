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
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Cierre_Anual.Negocio;

namespace Presidencia.Cierre_Anual.Datos
{
    public class Cls_Ope_Con_Cierre_Anual_Datos
    {
        #region (Metodos Externos)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consulta_Datos_Cuentas_Contables
        /// DESCRIPCION : Consulta las Cuentas Contables que estan dadas de alta en la BD
        ///               con todos sus datos
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 24/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consulta_Datos_Cuentas_Contables(Cls_Ope_Con_Cierre_Anual_Negocio Datos)
        {
            String Mi_SQL; //Variable para la consulta de las Cuentas Contables

            try
            {
                Mi_SQL = "SELECT " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + ".*, ";
                Mi_SQL += Cat_Con_Niveles.Tabla_Cat_Con_Niveles + "." + Cat_Con_Niveles.Campo_Descripcion + " AS Nivel";
                Mi_SQL += " FROM " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + " LEFT JOIN " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles;
                Mi_SQL += " ON " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Nivel_ID + " = " + Cat_Con_Niveles.Tabla_Cat_Con_Niveles + "." + Cat_Con_Niveles.Campo_Nivel_ID;

                if (Datos.P_Cuenta_Contable_Rango == null)
                    Datos.P_Cuenta_Contable_Rango = false;

                if (Datos.P_Cuenta_Contable_Rango == true)
                {
                    if (Datos.P_Cuenta_Final == null)
                        Mi_SQL += " WHERE (" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " > '" + Datos.P_Cuenta_Inicial + "')";
                    if (!String.IsNullOrEmpty(Datos.P_Cuenta_Final))
                    {
                        Mi_SQL += " WHERE (" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " >= '" + Datos.P_Cuenta_Inicial + "' AND " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " <= '" + Datos.P_Cuenta_Final + "')";
                    }
                }
                else if (!String.IsNullOrEmpty(Datos.P_Cuenta_Inicial) && Datos.P_Cuenta_Contable_Rango == false)
                {
                    Mi_SQL += " WHERE (" + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " = '" + Datos.P_Cuenta_Inicial + "')";
                }

                Mi_SQL += " ORDER BY " + Cat_Con_Cuentas_Contables.Tabla_Cat_Con_Cuentas_Contables + "." + Cat_Con_Cuentas_Contables.Campo_Cuenta + " ASC";

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL).Tables[0];
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        #endregion

        #region (Alta - Modificar - Eliminar)
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Alta_Cierre_Mensual
        /// DESCRIPCION : 1.Consulta el último ID dado de alta en la tabla.
        ///               2. Da de alta el nuevo registro con los datos proporcionados por
        ///                  el usuario.
        /// PARAMETROS  : Datos: Almacena los datos a insertarse en la BD.
        /// CREO        : Salvador L. Rea Ayala
        /// FECHA_CREO  : 25/Octubre/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static void Alta_Cierre_Mensual(Cls_Ope_Con_Cierre_Anual_Negocio Datos)
        {
            String Mi_SQL;   //Variable de Consulta para la Alta del de una Nueva Mascara
            Object Parametros_ID; //Variable que contendrá el ID de la consulta

            try
            {
                //Busca el maximo ID de la tabla Parametros.
                Mi_SQL = "SELECT NVL(MAX (" + Ope_Con_Cierre_Anual.Campo_No_Cierre_Anual + "),'00000') ";
                Mi_SQL = Mi_SQL + "FROM " + Ope_Con_Cierre_Anual.Tabla_Ope_Con_Cierre_Anual;
                Parametros_ID = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Parametros_ID)) //Si no existen valores en la tabla, asigna el primer valor manualmente.
                {
                    Datos.P_No_Cierre_Anual = "00001";
                }
                else // Si ya existen registros, toma el valor maximo y le suma 1 para el nuevo registro.
                {
                    Datos.P_No_Cierre_Anual = String.Format("{0:00000}", Convert.ToInt32(Parametros_ID) + 1);
                }
                //Da de Alta los datos del Nuevo Parametro con los datos proporcionados por el usuario.
                Mi_SQL = "INSERT INTO " + Ope_Con_Cierre_Anual.Tabla_Ope_Con_Cierre_Anual + " (";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Anio + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Cuenta_Contable_ID + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Cuenta_Contable_ID_Fin + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Cuenta_Contable_ID_Inicio + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Descripcion + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Diferencia + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Fecha_Creo + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_No_Cierre_Anual + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Total_Debe + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Total_Haber + ", ";
                Mi_SQL += Ope_Con_Cierre_Anual.Campo_Usuario_Creo + ") VALUES ('";
                Mi_SQL += Datos.P_Anio + "', '";
                Mi_SQL += Datos.P_Cuenta_Contable_ID + "', '";
                Mi_SQL += Datos.P_Cuenta_Contable_ID_Fin + "', '";
                Mi_SQL += Datos.P_Cuenta_Contable_ID_Inicio + "', '";
                Mi_SQL += Datos.P_Descripcion + "', ";
                Mi_SQL += Datos.P_Diferencia + ", ";
                Mi_SQL += "SYSDATE, '";
                Mi_SQL += Datos.P_No_Cierre_Anual + "', ";
                Mi_SQL += Datos.P_Total_Debe + ", ";
                Mi_SQL += Datos.P_Total_Haber + ", '";
                Mi_SQL += Datos.P_Usuario_Creo + "')";

                //Manda Mi_SQL para ser procesada por ORACLE.
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            }
            catch (OracleException Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
            catch (DBConcurrencyException Ex)
            {
                throw new Exception("Lo siento, los datos fueron actualizados por otro Usuario. Error: [" + Ex.Message + "]");
            }
            catch (Exception Ex)
            {
                throw new Exception("Error: " + Ex.Message);
            }
        }
        #endregion
    }
}