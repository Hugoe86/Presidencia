using System;
using System.Collections;
using System.Configuration;
using System.Data;
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
using Presidencia.Sessiones;
using System.Text;
using Presidencia.Asignar_Presupuesto_Programas.Negocio;



namespace Presidencia.Asignar_Presupuesto_Programas.Datos
{
    public class Cls_Ope_Asignar_Presupuesto_Programas_Datos
    {
        #region METODOS
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Presupuesto_Programas
        ///DESCRIPCIÓN: consultar los elementos dentro de la base de datos
        ///PARAMETROS: 1.- Cls_Ope_Asignar_Presupuesto_Programas_Negocio
        ///CREO: Leslie González Vázquez
        ///FECHA_CREO: 01/Marzo/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static DataSet Consulta_Presupuesto_Programas(Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programas)
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenara la consulta.
            DataSet Ds_Presupuesto_Programas = null;//Listado de Presupuesto_Programas.

            try
            {
                Mi_SQL.Append("SELECT PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID + ",");
                Mi_SQL.Append(" PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID + ",");
                Mi_SQL.Append(" PROGRAMA." +Cat_Com_Proyectos_Programas.Campo_Clave + ",");
                Mi_SQL.Append(" PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Nombre + ",");
                Mi_SQL.Append(" PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Anio_Presupuesto + ",");
                Mi_SQL.Append(" PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Presupuestal + ",");
                Mi_SQL.Append(" PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + ",");
                Mi_SQL.Append(" PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Ejercido);
                Mi_SQL.Append(", PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Comprometido + ",");
                Mi_SQL.Append(" PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Fecha_Creo);
                Mi_SQL.Append(" FROM " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy + " PRE_PROY ");
                Mi_SQL.Append(" JOIN " + Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " PROGRAMA ");
                Mi_SQL.Append(" ON PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID);
                Mi_SQL.Append(" = PRE_PROY." + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID);

                if (Presupuesto_Programas.P_Nombre != null)
                {
                    Mi_SQL.Append(" WHERE PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Nombre + " LIKE '%" + Presupuesto_Programas.P_Nombre + "%'");
                }
                Mi_SQL.Append(" ORDER BY PROGRAMA." + Cat_Com_Proyectos_Programas.Campo_Nombre);
                Ds_Presupuesto_Programas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al invocar Consulta_Presupuesto_Programas. Error: [" + Ex.Message + "]");
            }
            return (Ds_Presupuesto_Programas is DataSet) ? Ds_Presupuesto_Programas : new DataSet();
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Consultar_DataTable
        ///DESCRIPCIÓN          : Obtiene todos los programas que estan dados de alta en la base de datos
        ///PARAMETROS           :   
        ///CREO                 : Leslie González Vázquez
        ///FECHA_CREO           : 01/Marzo/2011 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        public static DataTable Consultar_Proyectos_Programas()
        {
            StringBuilder Mi_SQL = new StringBuilder();//Variable que almacenará la consulta.
            DataTable Dt_Proyectos_Programas = null;//Variable que almacenará un listado de los giros que existen actualmente en el sistema.

            try
            {
                Mi_SQL.Append("SELECT " + Cat_Com_Proyectos_Programas.Campo_Proyecto_Programa_ID + ", ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Campo_Clave + "|| '  ' ||" + Cat_Com_Proyectos_Programas.Campo_Nombre + " FROM ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Tabla_Cat_Com_Proyectos_Programas + " WHERE ");
                Mi_SQL.Append(Cat_Com_Proyectos_Programas.Campo_Estatus + " = 'ACTIVO'");

                Dt_Proyectos_Programas = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al consultar los Programas. Error: [" + Ex.Message + "]");
            }
            return (Dt_Proyectos_Programas is DataTable) ? Dt_Proyectos_Programas : new DataTable();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Presupuesto_Programas
        ///DESCRIPCIÓN:Da de alta un Presupuesto_Programas en la base de datos
        ///PARAMETROS:  1.- Cls_Ope_Asignar_Presupuesto_Programas_Negocio
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 01/Marzo/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static Boolean Alta_Presupuesto_Programas(Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programas)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que almacenara la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            try
            {
                Mi_SQL.Append("INSERT INTO " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy);
                Mi_SQL.Append("(" + Ope_SAP_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID + ", " + Ope_SAP_Pres_Prog_Proy.Campo_Proyecto_Programa_ID);
                Mi_SQL.Append(", " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Presupuestal + ", " + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible);
                Mi_SQL.Append("," + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Comprometido + ", " + Ope_SAP_Pres_Prog_Proy.Campo_Anio_Presupuesto);
                Mi_SQL.Append("," + Ope_SAP_Pres_Prog_Proy.Campo_Monto_Ejercido + ", " + Ope_SAP_Pres_Prog_Proy.Campo_Usuario_Creo);
                Mi_SQL.Append(", " + Ope_SAP_Pres_Prog_Proy.Campo_Fecha_Creo + ") VALUES ('" + Presupuesto_Programas.P_Pres_Prog_Proy_ID);
                Mi_SQL.Append("', '" + Presupuesto_Programas.P_Proyecto_Programa_ID + "', '" + Presupuesto_Programas.P_Monto_Presupuestal);
                Mi_SQL.Append("', '" + Presupuesto_Programas.P_Monto_Disponible + "', '" + Presupuesto_Programas.P_Monto_Comprometido);
                Mi_SQL.Append("', '" + Presupuesto_Programas.P_Anio_Presupuesto + "', '" + Presupuesto_Programas.P_Monto_Ejercido);
                Mi_SQL.Append("', '" + Presupuesto_Programas.P_Usuario_Creo + "', SYSDATE)");

                //Sentencia que ejecuta el query
                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al Alta_Presupuesto_Programas. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        //*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Modificar_Presupuesto_Programas
        ///DESCRIPCIÓN: Modificar el Presupuesto_Programas en la base de datos
        ///PARAMETROS: 1.- Cls_Ope_Asignar_Presupuesto_Programas_Negocio
        ///CREO: Leslie Gonzalez Vazquez
        ///FECHA_CREO: 01/Marzo/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************/
        public static Boolean Modificar_Presupuesto_Programas(Cls_Ope_Asignar_Presupuesto_Programas_Negocio Presupuesto_Programas)
        {
            StringBuilder MI_SQL = new StringBuilder();//Variable que almacenará la consulta.
            Boolean Operacion_Completa = false;//Estado de la operacion.

            try
            {
                MI_SQL.Append("UPDATE " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy + " SET ");
                MI_SQL.Append(Ope_SAP_Pres_Prog_Proy.Campo_Monto_Presupuestal + " = '" + Presupuesto_Programas.P_Monto_Presupuestal + "', ");
                MI_SQL.Append(Ope_SAP_Pres_Prog_Proy.Campo_Monto_Disponible + " = '" + Presupuesto_Programas.P_Monto_Disponible + "', ");
                MI_SQL.Append(Ope_SAP_Pres_Prog_Proy.Campo_Usuario_Modifico + " = '" + Presupuesto_Programas.P_Usuario_Modifico + "', ");
                MI_SQL.Append(Ope_SAP_Pres_Prog_Proy.Campo_Fecha_Modifico + " = SYSDATE");
                MI_SQL.Append(" WHERE " + Ope_SAP_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID + " = '" + Presupuesto_Programas.P_Pres_Prog_Proy_ID +"'");

                OracleHelper.ExecuteNonQuery(Cls_Constantes.Str_Conexion, CommandType.Text, MI_SQL.ToString());
                Operacion_Completa = true;
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al ejecutar Modificar_Presupuesto_Programas. Error: [" + Ex.Message + "]");
            }
            return Operacion_Completa;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consecutivo
        ///DESCRIPCIÓN: Metodo que verfifica el consecutivo en la tabla y ayuda a generar el nuevo Id. 
        ///PARAMETROS: 
        ///CREO:    Leslie González Vázquez
        ///FECHA_CREO: 02/Marzo/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public static String Consecutivo_ID()
        {
            String Consecutivo = "";
            StringBuilder Mi_SQL = new StringBuilder();
            object Pres_Pro_id; //Obtiene el ID con la cual se guardo los datos en la base de datos

            Mi_SQL.Append("SELECT NVL(MAX (" + Ope_SAP_Pres_Prog_Proy.Campo_Pres_Prog_Proy_ID + "), '0000000000')");
            Mi_SQL.Append(" FROM " + Ope_SAP_Pres_Prog_Proy.Tabla_Ope_SAP_Pres_Prog_Proy);

            Pres_Pro_id = OracleHelper.ExecuteScalar(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString());

            if (Convert.IsDBNull(Pres_Pro_id))
            {
                Consecutivo = "0000000001";
            }
            else
            {
                Consecutivo = string.Format("{0:0000000000}", Convert.ToInt32(Pres_Pro_id) + 1);
            }
            return Consecutivo;
        }//fin de consecutivo         
        #endregion 
    }
}