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
using System.Text;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Ope_Pre_Rpt_Impresion_Constancias.Negocio;

/// <summary>
/// Summary description for Cls_Ope_Pre_Rpt_Impresion_Constancias_Datos
/// </summary>

namespace Presidencia.Ope_Pre_Rpt_Impresion_Constancias.Datos
{
    public class Cls_Ope_Pre_Rpt_Impresion_Constancias_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Constancias_No_Propiedad
        ///DESCRIPCIÓN: Consulta los datos de las constancias de no propiedad de acuerdo a
        ///             los filtros que selecciono el usuario
        ///PARAMENTROS: Cnp:              Contiene los filtros elegidos por el usuario.
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 12/Enero/2012 
        ///MODIFICO          : 
        ///FECHA_MODIFICO    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static DataTable Consultar_Constancias_No_Propiedad(Cls_Ope_Pre_Rpt_Impresion_Constancias_Negocio Cnp)
        {
            String Mi_SQL = ""; //Obtiene la consulta a realizar a la base de datos
            try
            {
                //Consulta los datos generales de la fecha que fue proporcionada por el usuario
                Mi_SQL="SELECT C."+Ope_Pre_Constancias.Campo_Folio+", C."+Ope_Pre_Constancias.Campo_Solicitante+", C."+Ope_Pre_Constancias.Campo_Fecha+", C."+Ope_Pre_Constancias.Campo_Estatus+
                    ",CO." + Cat_Pre_Tipos_Constancias.Campo_Costo + ", (SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " WHERE TRIM(" + Ope_Ing_Pasivo.Campo_Referencia + ")=TRIM(C." + Ope_Pre_Constancias.Campo_Folio + ") AND " + Ope_Ing_Pasivo.Campo_Estatus + "='PAGADO') AS PASIVO FROM " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " C LEFT OUTER JOIN " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + " CO " +
                    " ON C."+Ope_Pre_Constancias.Campo_Tipo_Constancia_ID+"=CO."+Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID+" WHERE (C."+Ope_Pre_Constancias.Campo_Fecha+" BETWEEN TO_DATE('"+Cnp.P_Fecha_Inicial+" 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND TO_DATE('"+Cnp.P_Fecha_Final+" 23:59:59', 'DD-MM-YYYY HH24:MI:SS')) AND C."
                    + Ope_Pre_Constancias.Campo_Folio + " LIKE '%CNP%' AND C." + Ope_Pre_Constancias.Campo_Estatus + " IN "+Cnp.P_Estatus;
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

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Constancias_Y_Certificaciones
        ///DESCRIPCIÓN: Consulta los valores que estan dados de alta de acuerdo a la fecha,
        ///             tipo y estatus proporcionada por el usurio
        ///PARAMENTROS: Con_Cer:          Contiene los filtros que eligió el usuario.
        ///CREO       : Miguel Angel Bedolla Moreno.
        ///FECHA_CREO : 13/Enero/2012 
        ///MODIFICO          : 
        ///FECHA_MODIFICO    : 
        ///CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static DataTable Consultar_Constancias_Y_Certificaciones(Cls_Ope_Pre_Rpt_Impresion_Constancias_Negocio Con_Cer)
        {
            String Mi_SQL = ""; //Obtiene la consulta a realizar a la base de datos
            try
            {
                //Consulta los datos generales de la fecha que fue proporcionada por el usuario
                Mi_SQL = " SELECT (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = C." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ") AS CUENTA_PREDIAL, C."
                    + Ope_Pre_Constancias.Campo_Folio + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = C." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS NOMBRE_PROPIETARIO, C." + Ope_Pre_Constancias.Campo_Fecha + ", C." + Ope_Pre_Constancias.Campo_Estatus + ", CO." + Cat_Pre_Tipos_Constancias.Campo_Costo + ", (SELECT SUM(" + Ope_Ing_Pasivo.Campo_Monto + ") FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " WHERE TRIM(" + Ope_Ing_Pasivo.Campo_Referencia + ")=TRIM(C." + Ope_Pre_Constancias.Campo_Folio + ") AND " + Ope_Ing_Pasivo.Campo_Estatus + "='PAGADO') AS PASIVO, (SELECT " + Ope_Ing_Pasivo.Campo_Contribuyente + " FROM " + Ope_Ing_Pasivo.Tabla_Ope_Ing_Pasivo + " WHERE TRIM(" + Ope_Ing_Pasivo.Campo_Referencia + ")=TRIM(C." + Ope_Pre_Constancias.Campo_Folio + ")  AND ROWNUM=1 GROUP BY " + Ope_Ing_Pasivo.Campo_Contribuyente + ") AS PROPIETARIO FROM " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + " C LEFT OUTER JOIN " + Cat_Pre_Tipos_Constancias.Tabla_Cat_Pre_Tipos_Constancias + " CO ON C." + Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + "=CO." + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " WHERE (C." + Ope_Pre_Constancias.Campo_Fecha + " BETWEEN TO_DATE('" + Con_Cer.P_Fecha_Inicial + " 00:00:00', 'DD-MM-YYYY HH24:MI:SS') AND TO_DATE('" + Con_Cer.P_Fecha_Final + " 23:59:59', 'DD-MM-YYYY HH24:MI:SS')) AND C." + Ope_Pre_Constancias.Campo_Folio + " LIKE '%" + Con_Cer.P_Tipo + "%' AND C." + Ope_Pre_Constancias.Campo_Estatus + " IN " + Con_Cer.P_Estatus;
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