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
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Text;
using Presidencia.Reporte_Nombramientos.Negocio;
using Presidencia.Constantes;
using Presidencia.Ayudante_Informacion;

namespace Presidencia.Reporte_Nombramientos.Datos
{
    public class Cls_Rpt_Nom_Nombramientos_Datos
    {
        public Cls_Rpt_Nom_Nombramientos_Datos()
        {
        }

        #region (Metodos)
        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Nombramientos
        /// DESCRIPCIÓN: Realizar la consulta de los nombramientos de los empleados con diversos criterios de busqueda
        /// PARÁMETROS:  Datos: Variable de la capa de negocios
        /// CREO: Noe Mosqueda Valadez
        /// FECHA_CREO: 09/Abril/2012 21:41
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Nombramientos(Cls_Rpt_Nom_Nombramientos_Negocio Datos)
        {
            //Declaracion de variables
            DataTable Dt_Resultado = new DataTable(); //tabla para la consulta
            StringBuilder Mi_SQL = new StringBuilder(); //Cadena de texto para la consulta
            Boolean Where_Utilizado = false; //variable que indica que la clausula where ya ha sido utilizada

            try
            {
                //Construir la consulta
                Mi_SQL.Append("SELECT " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_SAP_Codigo_Programatico + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Empleado_ID + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_RFC + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Salario_Diario + ",");
                Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Fecha_Inicio + ",");
                Mi_SQL.Append(Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Nombre + " AS Puesto,");
                Mi_SQL.Append(Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre + " AS Area,");
                Mi_SQL.Append(Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre + " AS Dependencia,");
                Mi_SQL.Append(Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Nomina + " AS Tipo_Nomina,");
                Mi_SQL.Append("SYSDATE AS Fecha_Elaboracion ");
                Mi_SQL.Append("FROM " + Cat_Empleados.Tabla_Cat_Empleados + " ");
                Mi_SQL.Append("INNER JOIN " + Cat_Puestos.Tabla_Cat_Puestos + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Puesto_ID);
                Mi_SQL.Append(" = " + Cat_Puestos.Tabla_Cat_Puestos + "." + Cat_Puestos.Campo_Puesto_ID + " ");
                Mi_SQL.Append("INNER JOIN " + Cat_Areas.Tabla_Cat_Areas + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Area_ID);
                Mi_SQL.Append(" = " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Area_ID + " ");
                Mi_SQL.Append("INNER JOIN " + Cat_Dependencias.Tabla_Cat_Dependencias + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID);
                Mi_SQL.Append(" = " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Grupo_Dependencia_ID + " ");
                Mi_SQL.Append("INNER JOIN " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + " ON " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID);
                Mi_SQL.Append(" = " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas + "." + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + " ");
                
                //Filtros para la consulta
                //No_Empleado
                if (!String.IsNullOrEmpty(Datos.P_No_Empleado))
                {
                    Mi_SQL.Append("WHERE " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " = '" + Datos.P_No_Empleado + "' ");

                    Where_Utilizado = true;
                }

                //Estatus
                if (!String.IsNullOrEmpty(Datos.P_Estatus))
                {
                    //verificar si ya ha sido utilizada la clausula where
                    if (Where_Utilizado == true)
                    {
                        Mi_SQL.Append("AND ");
                    }
                    else
                    {
                        Mi_SQL.Append("WHERE ");

                        Where_Utilizado = true;
                    }

                    //Resto de la consulta
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Estatus + " = '" + Datos.P_Estatus + "' ");
                }
                
                //Nombre
                if (!String.IsNullOrEmpty(Datos.P_Nombre))
                {
                    //verificar si ya ha sido utilizada la clausula where
                    if (Where_Utilizado == true)
                    {
                        Mi_SQL.Append("AND ");
                    }
                    else
                    {
                        Mi_SQL.Append("WHERE ");

                        Where_Utilizado = true;
                    }

                    //Resto de la consulta
                    Mi_SQL.Append(" UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre);
                    Mi_SQL.Append(") LIKE '%" + Datos.P_Nombre.ToUpper() + "%' ");
                }

                //Tipo Nomina
                if (!String.IsNullOrEmpty(Datos.P_Tipo_Nomina_ID))
                {
                    //verificar si ya ha sido utilizada la clausula where
                    if (Where_Utilizado == true)
                    {
                        Mi_SQL.Append("AND ");
                    }
                    else
                    {
                        Mi_SQL.Append("WHERE ");

                        Where_Utilizado = true;
                    }

                    //Resto de la consulta
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + " = '" + Datos.P_Tipo_Nomina_ID + "' ");
                }

                //Unidad responsable
                if (!String.IsNullOrEmpty(Datos.P_Dependencia_ID))
                {
                    //verificar si ya ha sido utilizada la clausula where
                    if (Where_Utilizado == true)
                    {
                        Mi_SQL.Append("AND ");
                    }
                    else
                    {
                        Mi_SQL.Append("WHERE ");

                        Where_Utilizado = true;
                    }

                    //Resto de la consulta
                    Mi_SQL.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + " = '" + Datos.P_Dependencia_ID + "' ");
                }

                //Orden de la consulta
                Mi_SQL.Append("ORDER BY " + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_No_Empleado + " ");

                //Ejecutar consulta
                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                //Entregar resultado
                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString(), Ex);
            }
        }

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consulta_Promociones_Empleado
        /// DESCRIPCIÓN: Realizar la consulta de las propociones de un empleado por orden descendente
        /// PARÁMETROS:  Datos: Variable de la capa de negocios
        /// CREO: Noe Mosqueda Valadez
        /// FECHA_CREO: 09/Abril/2012 23:04
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        public static DataTable Consulta_Promociones_Empleado(Cls_Rpt_Nom_Nombramientos_Negocio Datos)
        {
            //Declaracion de variables
            StringBuilder Mi_SQL = new StringBuilder(); //variable para las consultas
            DataTable Dt_Resultado = new DataTable(); //Tabla para el resultado

            try
            {
                //Construir la consulta
                Mi_SQL.Append("SELECT " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + "," + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " ");
                Mi_SQL.Append("FROM " + Cat_Emp_Movimientos_Det.Tabla_Cat_Emp_Movimientos_Det + " ");
                Mi_SQL.Append("WHERE " + Cat_Emp_Movimientos_Det.Campo_Empleado_ID + " = '" + Datos.P_Empleado_ID + "' ");
                Mi_SQL.Append("AND " + Cat_Emp_Movimientos_Det.Campo_Tipo_Movimiento + " = 'PROMOCION' ");
                Mi_SQL.Append("ORDER BY " + Cat_Emp_Movimientos_Det.Campo_Fecha_Creo + " DESC ");

                //Ejecutar consulta
                Dt_Resultado = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL.ToString()).Tables[0];

                //Entregar resultado
                return Dt_Resultado;
            }
            catch (Exception Ex)
            {
                throw new Exception(Ex.ToString(), Ex);
            }
        }
        #endregion
    }
}