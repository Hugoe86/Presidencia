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
using System.Text;
using Presidencia.Periodos_Vacacionales.Negocio;
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Constantes;

namespace Presidencia.Periodos_Vacacionales.Datos
{
    public class Cls_Cat_Nom_Periodos_Vacacionales_Datos
    {
        #region (Métodos) 
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Periodos_Vacacionales
        ///DESCRIPCIÓN          : Metodo para consultar los periodos vacacionales de los empleados.
        ///PROPIEDADES          :
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : Diciembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        public static DataTable Consultar_Periodos_Vacacionales(Cls_Cat_Nom_Periodos_Vacacionales_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Periodo_Vacacionales = null;//Variable que almacenara un listado con los periodos vacacionales.

            try
            {
                Mi_SQL.Append("SELECT ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Anio + ", ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles + ", ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados + ", ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + ", ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Estatus + ", ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Periodo_Vacacional + ", ");
                
                Mi_SQL.Append(" (SELECT ('[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") FROM "); 
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det + "." + Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + ") AS EMPLEADO ");

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Tabla_Ope_Nom_Vacaciones_Empl_Det);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + " IN ");

                Mi_SQL.Append(" (SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "')");

                Dt_Periodo_Vacacionales = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los periodos vacacionales. Error: [" + Ex.Message + "]");
            }
            return Dt_Periodo_Vacacionales;
        }
        ///********************************************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_Vacaciones_Tomadas
        ///DESCRIPCIÓN          : Metodo para consultar los días de vacaciones que a tomado el empleado.
        ///PROPIEDADES          :
        ///CREO                 : Juan Alberto Hernandez Negrete
        ///FECHA_CREO           : Diciembre/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN...:
        ///*********************************************************************************************************
        public static DataTable Consultar_Vacaciones_Tomadas(Cls_Cat_Nom_Periodos_Vacacionales_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataTable Dt_Vacaciones = null;//Variable que almacenara el listado de empleados.

            try
            {
                Mi_SQL.Append("SELECT ");

                Mi_SQL.Append(" (SELECT ('[' || " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " || '] ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre + ") FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + "=");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Empleado_ID + ") AS EMPLEADO, ");

                Mi_SQL.Append(" (SELECT ('[' || " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Clave + " || '] ' || ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + ") FROM ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Dependencia_ID + "=");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Dependencia_ID + ") AS UR, ");

                Mi_SQL.Append(" (SELECT " + Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Anio);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Nom_Calendario_Nominas.Tabla_Cat_Nom_Calendario_Nominas + "." + Cat_Nom_Calendario_Nominas.Campo_Nomina_ID + "=");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado + "." + OPE_NOM_VACACIONES_EMPLEADO.Campo_Nomina_ID + ") AS NOMINA, ");

                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_No_Nomina + ", ");
                Mi_SQL.Append(" TO_CHAR(" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + ", 'DD-MON-YYYY') AS " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Inicio + ", ");
                Mi_SQL.Append(" TO_CHAR(" + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + ", 'DD-MON-YYYY') AS " + OPE_NOM_VACACIONES_EMPLEADO.Campo_Fecha_Termino + ", ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Campo_Cantidad_Dias);

                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(OPE_NOM_VACACIONES_EMPLEADO.Tabla_Ope_Nom_Vacaciones_Empleado);

                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Empleado_ID + " IN ");

                Mi_SQL.Append(" (SELECT ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" FROM ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados);
                Mi_SQL.Append(" WHERE ");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + "='" + Datos.P_No_Empleado + "')");

                Dt_Vacaciones = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los días de vacaciones tomadas por los empleados. Error: [" + Ex.Message + "]");
            }
            return Dt_Vacaciones;
        }
        #endregion
    }
}
