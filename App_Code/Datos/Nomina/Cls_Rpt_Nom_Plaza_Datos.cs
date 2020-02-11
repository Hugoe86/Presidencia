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
using Presidencia.Generar_Reservas.Negocio;
using Presidencia.Constantes;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Reporte_Plazas.Negocio;
using Presidencia.Administrar_Requisiciones.Negocios;
using System.Data.OracleClient;
using System.Text;
namespace Presidencia.Reporte_Plazas.Datos
{
    public class Cls_Rpt_Nom_Plaza_Datos
    {
        public Cls_Rpt_Nom_Plaza_Datos()
        {

        }
        #region consulta
        ///*******************************************************************************
        /// NOMBRE DE LA FUNCION: Consultar_Puestos_Depenencia
        /// DESCRIPCION: Consulta los puestos que pertenescan a la unidad responsable seleccionada
        /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
        /// CREO       : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO : 11-Abril-2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Puestos_Depenencia(Cls_Rpt_Nom_Plazas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
            try
            {
                //select puesto.puesto_id, puesto.nombre from cat_puestos puesto,cat_nom_dep_puestos_det dep where puesto.puesto_id=dep.puesto_id 
                //and dependencia_id='00008' group by puesto.puesto_id,puesto.nombre
                Mi_SQL.Append("SELECT  PUESTO." + Cat_Puestos.Campo_Puesto_ID + ", PUESTO." + Cat_Puestos.Campo_Nombre+" FROM ");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos +" PUESTO, "+Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det +" DEPENDENCIA");
                Mi_SQL.Append(" WHERE PUESTO."+Cat_Puestos.Campo_Puesto_ID +"=DEPENDENCIA."+ Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID);
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND DEPENDENCIA." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" +Datos.P_Dependencia_ID +"'");
                }
                Mi_SQL.Append(" GROUP BY PUESTO."+Cat_Puestos.Campo_Puesto_ID+", PUESTO."+Cat_Puestos.Campo_Nombre);

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        /// NOMBRE DE LA FUNCION: Consultar_Tipo_Nomina
        /// DESCRIPCION: Consulta los tipos de nomina 
        /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
        /// CREO       : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO : 11-Abril-2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Tipo_Nomina(Cls_Rpt_Nom_Plazas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
            try
            {
                //select Tipo_Nomina_ID, Nomina from Cat_Nom_Tipos_Nominas
                Mi_SQL.Append("SELECT " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + ", " + Cat_Nom_Tipos_Nominas.Campo_Nomina + " FROM ");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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
        /// NOMBRE DE LA FUNCION: Consultar_Plazas
        /// DESCRIPCION: Consulta las plazas por dependencia 
        /// PARAMETROS : Datos: Valores que son pasados desde la capa de negocios
        /// CREO       : Sergio Manuel Gallardo Andrade
        /// FECHA_CREO : 11-Abril-2012
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************
        public static DataTable Consultar_Plazas(Cls_Rpt_Nom_Plazas_Negocio Datos)
        {
            StringBuilder Mi_SQL = new StringBuilder(); //Variable que tendra la consulta a realizar a la base de datos
            try
            {
                Mi_SQL.Append("SELECT (Dependencia." + Cat_Dependencias.Campo_Clave + "||'-'|| Dependencia." + Cat_Dependencias.Campo_Nombre +")as DIRECCION_GENERAL, Dep.");
                Mi_SQL.Append(Cat_Nom_Dep_Puestos_Det.Campo_Estatus + ", Emp." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ", Puesto." + Cat_Puestos.Campo_Salario_Mensual + " As SUELDO_MENSUAL, Puesto." + Cat_Puestos.Campo_Nombre + " As NIVEL, (SELECT (");
                Mi_SQL.Append(Cat_Empleados.Campo_Apellido_Paterno + "||' '||" + Cat_Empleados.Campo_Apellido_Materno + "||' '||" + Cat_Empleados.Campo_Nombre + ") FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE " + Cat_Empleados.Campo_Empleado_ID + "='" + Datos.P_Usuario_Creo + "') As USUARIO_CREO, Dep."+Cat_Nom_Dep_Puestos_Det.Campo_Tipo_Plaza );
                Mi_SQL.Append(" FROM "+Cat_Nom_Dep_Puestos_Det.Tabla_Cat_Nom_Dep_Puestos_Det +" Dep ");
                Mi_SQL.Append(" LEFT OUTER JOIN "+ Cat_Dependencias.Tabla_Cat_Dependencias +" Dependencia ON Dep."+ Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID+"=Dependencia."+Cat_Dependencias.Campo_Dependencia_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Empleados.Tabla_Cat_Empleados + " Emp ON Dep." + Cat_Nom_Dep_Puestos_Det.Campo_Empleado_ID + "=Emp." + Cat_Empleados.Campo_Empleado_ID);
                Mi_SQL.Append(" LEFT OUTER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " Puesto ON Emp." + Cat_Empleados.Campo_Puesto_ID + "=Puesto." + Cat_Puestos.Campo_Puesto_ID);
                Mi_SQL.Append(" WHERE Dependencia."+Cat_Dependencias.Campo_Estatus +" ='ACTIVO' AND Emp."+Cat_Empleados.Campo_Estatus+" ='ACTIVO'");
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    Mi_SQL.Append(" AND Dep." + Cat_Nom_Dep_Puestos_Det.Campo_Dependencia_ID + "='" + Datos.P_Dependencia_ID + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Puesto_ID))
                {
                    Mi_SQL.Append(" AND Dep." + Cat_Nom_Dep_Puestos_Det.Campo_Puesto_ID + "='" + Datos.P_Puesto_ID + "'");
                }
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina))
                {
                    Mi_SQL.Append(" AND Emp." + Cat_Empleados.Campo_Tipo_Nomina_ID + "='" + Datos.P_Tipo_Nomina + "'");
                }
                Mi_SQL.Append(" ORDER BY Dependencia." + Cat_Dependencias.Campo_Dependencia_ID);
                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];
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