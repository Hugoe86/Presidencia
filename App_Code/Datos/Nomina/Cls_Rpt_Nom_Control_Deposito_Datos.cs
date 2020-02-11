using System;
using System.IO;
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
using System.Text;
using System.Collections.Generic;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Reportes_Nomina_Control_Deposito.Negocio;

namespace Presidencia.Reportes_Nomina_Control_Deposito.Datos
{
    public class Cls_Rpt_Nom_Control_Deposito_Datos
    {
        #region Consulta
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Control_Deposito
        /// DESCRIPCION : Consulta los depositos que se realizan a los empleados
        /// PARAMETROS  : Datos: Indica que registro se desea consultar a la base de datos
        /// CREO        :Hugo Enrique Ramírez Aguilera
        /// FECHA_CREO  : 09/Abril/2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Control_Deposito(Cls_Rpt_Nom_Control_Deposito_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable para la consulta para del Empleado
            DataTable Dt_Consulta = new DataTable();
            try
            {
                Mi_SQL.Append("Select ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + ",");
                Mi_SQL.Append(Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin + ",");

                Mi_SQL.Append("(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno);
                Mi_SQL.Append(" || ' ' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") as Nombre_Empleado,");
                
                Mi_SQL.Append(" to_number(" + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID + " )as Banco_Id,");
                Mi_SQL.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Nombre + " as Nombre_Banco,");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Cuenta_Bancaria + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Tarjeta + ",");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Total_Nomina + ",");
                Mi_SQL.Append(" to_number(" + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + " )as Tipo_Nomina_Id,");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + "");
             
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados);

                Mi_SQL.Append("  left outer join " + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" on " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append("=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);

                Mi_SQL.Append("  left outer join " + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles);
                Mi_SQL.Append(" on " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_Nomina_ID);
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina);
                Mi_SQL.Append("=" + Cat_Nom_Nominas_Detalles.Tabla_Cat_Nom_Nominas_Detalles + "." + Cat_Nom_Nominas_Detalles.Campo_No_Nomina);

                Mi_SQL.Append("  left outer join " + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos);
                Mi_SQL.Append(" on " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Banco_ID);
                Mi_SQL.Append("=" + Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + "." + Cat_Nom_Bancos.Campo_Banco_ID);

                Mi_SQL.Append("  left outer join " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_SQL.Append(" on " + Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append("=" + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID);

                Mi_SQL.Append(" where ");

                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Nomina_ID + "='" + Datos.P_Nomina_id + "'");
                Mi_SQL.Append(" and ");
                Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_No_Nomina + "=" + Datos.P_No_Nomina + "");

                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    Mi_SQL.Append(" and ");
                    Mi_SQL.Append(Ope_Nom_Recibos_Empleados.Tabla_Ope_Nom_Recibos_Empleados + "." + Ope_Nom_Recibos_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina_ID + "'");
                }

                Mi_SQL.Append(" order by " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + ",");
                Mi_SQL.Append("Nombre_Banco,Nombre_Empleado");
             
                Dt_Consulta = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
                return Dt_Consulta;
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
        #endregion
    }
}
