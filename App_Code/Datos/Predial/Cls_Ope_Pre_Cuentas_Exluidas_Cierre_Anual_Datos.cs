using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Cuentas_Exluidas_Cierre_Anual.Negocio;

namespace Presidencia.Operacion_Predial_Cuentas_Exluidas_Cierre_Anual.Datos
{
    public class Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Datos
    {
        public Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Datos()
        {
        }


        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Total_Cuentas
        /// DESCRIPCIÓN: Consulta el total de cuentas filtradas por estatus
        ///             Regresa un valor entero
        /// PARÁMETROS:
        /// 	1. Estatus: parametro para filtrar la cuentas suspendidas por tipo
        /// 	2. Excluir_Tipo_Suspension: Lista entre comillas separada por comas de los estatus a excluir ej: IN ('AMBAS','PREDIAL')
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 31-oct-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static Int32 Consultar_Total_Cuentas(String Filtro_Estatus, String Filtro_Tipo_Suspension)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = "";
            Int32 Total_Cuentas;
            object Total;

            try
            {
                Mi_SQL = "SELECT COUNT(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                Mi_SQL += ") FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas;
                // filtrar por estatus
                if (!String.IsNullOrEmpty(Filtro_Estatus))
                {
                    Filtro_SQL = " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Filtro_Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Filtro_Tipo_Suspension))
                {
                    if (Filtro_SQL.Length > 0)
                    {
                        Filtro_SQL += " AND NVL(" + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Filtro_Tipo_Suspension;
                    }
                    else
                    {
                        Filtro_SQL += " WHERE NVL(" + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Filtro_Tipo_Suspension;
                    }
                }

                Mi_SQL += Filtro_SQL;

                Total = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

                if (Convert.IsDBNull(Total))    // si no se obtuvo resultado de la consulta
                {
                    return (Int32)0;              // regresar 0
                }
                else
                {
                    Int32.TryParse(Total.ToString(), out Total_Cuentas);    //si se obtiene un valor entero de la consulta, regresar ese valor
                    return Total_Cuentas;
                }
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
            finally
            {
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Cuentas_Por_Estatus
        /// DESCRIPCIÓN: Consulta el numero de cuenta predial y el propietario de cuentas filtradas por estatus
        ///             Regresa un datatable
        /// PARÁMETROS:
        /// 	1. Estatus: parametro para filtrar la cuentas suspendidas por tipo
        /// 	2. Excluir_Tipo_Suspension: Lista entre comillas separada por comas de los estatus a excluir ej: IN ('AMBAS','PREDIAL')
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 1-nov-2011
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consultar_Cuentas_Por_Estatus(String Filtro_Estatus, String Filtro_Tipo_Suspension)
        {
            String Mi_SQL; //Variable para la consulta SQL
            String Filtro_SQL = "";

            try
            {
                Mi_SQL = "SELECT CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial;
                // agregar subsonculta de nombre de contribuyente
                Mi_SQL += ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                    + Cat_Pre_Contribuyentes.Campo_Nombre;
                Mi_SQL += " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes
                    + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = ";
                // subconsulta de contribuyente para obtener el id del propietario o poseedor de la cuenta
                Mi_SQL += " (SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID
                    + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios
                    + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                    + " AND " + Cat_Pre_Propietarios.Campo_Tipo + " IN ('PROPIETARIO','POSEEDOR') AND ROWNUM=1 )";
                Mi_SQL += " ) NOMBRE_PROPIETARIO";
                Mi_SQL += " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CP";
                // filtrar por estatus
                if (!String.IsNullOrEmpty(Filtro_Estatus))
                {
                    Filtro_SQL = " WHERE CP." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Filtro_Estatus + "'";
                }
                if (!String.IsNullOrEmpty(Filtro_Tipo_Suspension))
                {
                    if (Filtro_SQL.Length > 0)
                    {
                        Filtro_SQL += " AND NVL(CP." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Filtro_Tipo_Suspension;
                    }
                    else
                    {
                        Filtro_SQL += " WHERE NVL(CP." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Suspencion + ",' ') " + Filtro_Tipo_Suspension;
                    }
                }

                Mi_SQL += Filtro_SQL;

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
            finally
            {
            }
        }

    }
}