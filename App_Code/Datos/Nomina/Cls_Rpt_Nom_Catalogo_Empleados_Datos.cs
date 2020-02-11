using System;
using System.Data;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Constantes;
using System.Text;
using Presidencia.Reportes_Nomina_Catalogo_Empleados.Negocio;

namespace Presidencia.Reportes_Nomina_Catalogo_Empleados.Datos
{
    public class Cls_Rpt_Nom_Catalogo_Empleados_Datos
    {
        #region METODOS

        ///*******************************************************************************************************
        /// NOMBRE_FUNCIÓN: Consultar_Catalogo_Empleados
        /// DESCRIPCIÓN: Genera la consulta del catálogo de empleados de nómina con filtros por empleado, nómina y unidad responsable
        /// PARÁMETROS:
        /// 		1. Empleados_Negocio: isntancia de la capa de negocio con fitros para la consulta
        /// CREO: Roberto González Oseguera
        /// FECHA_CREO: 05-abr-2012
        /// MODIFICÓ: 
        /// FECHA_MODIFICÓ: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************************************
        internal static DataTable Consultar_Catalogo_Empleados(Cls_Rpt_Nom_Catalogo_Empleados_Negocio Empleados_Negocio)
        {
            StringBuilder Mi_Sql = new StringBuilder();
            try
            {
                Mi_Sql.Append("SELECT " + Cat_Empleados.Campo_Empleado_ID);
                Mi_Sql.Append("," + Cat_Empleados.Campo_No_Empleado);
                Mi_Sql.Append("," + Cat_Empleados.Campo_SAP_Codigo_Programatico);
                Mi_Sql.Append("," + Cat_Empleados.Campo_RFC);

                //  datos del banco, cuenta bancaria, no tarjeta
                Mi_Sql.Append("," + Cat_Empleados.Campo_No_Cuenta_Bancaria);
                Mi_Sql.Append("," + Cat_Empleados.Campo_No_Tarjeta);
                Mi_Sql.Append(",(SELECT " + Cat_Nom_Bancos.Campo_Nombre + " FROM ");
                Mi_Sql.Append(Cat_Nom_Bancos.Tabla_Cat_Nom_Bancos + " WHERE " + Cat_Nom_Bancos.Campo_Banco_ID + "=" + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append("." + Cat_Empleados.Campo_Banco_ID + ") AS BANCO");
                Mi_Sql.Append(",to_char ( ( months_between (sysdate , " + Cat_Empleados.Campo_Fecha_Inicio + ") ) / 12 , '9999.99') as FECHA");

                Mi_Sql.Append(",TO_NUMBER(" + Cat_Empleados.Campo_Tipo_Nomina_ID + ") TIPO_NOMINA_ID");
                Mi_Sql.Append(",TO_NUMBER(" + Cat_Empleados.Campo_Sindicato_ID + ") NUMERO_SINDICATO");
                Mi_Sql.Append(",TO_NUMBER(" + Cat_Empleados.Campo_Sindicato_ID + ") || ' ' || (SELECT ");
                Mi_Sql.Append(Cat_Nom_Sindicatos.Campo_Nombre + " FROM " + Cat_Nom_Sindicatos.Tabla_Cat_Nom_Sindicatos + " WHERE ");
                Mi_Sql.Append(Cat_Nom_Sindicatos.Campo_Sindicato_ID + "=" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + ") SINDICATO");
                Mi_Sql.Append("," + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                Mi_Sql.Append(Cat_Empleados.Campo_Nombre + " AS NOMBRE_EMPLEADO");

                Mi_Sql.Append(",(SELECT " + Cat_Puestos.Campo_Nombre + " FROM ");
                Mi_Sql.Append(Cat_Puestos.Tabla_Cat_Puestos + " WHERE " + Cat_Puestos.Campo_Puesto_ID + "=" + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append("." + Cat_Empleados.Campo_Puesto_ID + ") AS PUESTO");

                Mi_Sql.Append(",NVL((SELECT " + Cat_Puestos.Campo_Salario_Mensual + " FROM ");
                Mi_Sql.Append(Cat_Puestos.Tabla_Cat_Puestos + " WHERE " + Cat_Puestos.Campo_Puesto_ID + "=" + Cat_Empleados.Tabla_Cat_Empleados);
                Mi_Sql.Append("." + Cat_Empleados.Campo_Puesto_ID + "),0) AS SUELDO_MENSUAL");

                Mi_Sql.Append(",NVL((SELECT " + Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Cantidad + " FROM ");
                Mi_Sql.Append(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Tabla_Cat_Nom_Sin_Per_Ded_Det + " WHERE ");
                Mi_Sql.Append(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Sindicato_ID + "=");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Sindicato_ID + " AND ");
                Mi_Sql.Append(Cat_Nom_Sindicatos_Percepciones_Deducciones_Detalles.Campo_Percepcion_Deduccion_ID + "='");
                Mi_Sql.Append(Empleados_Negocio.P_Percepcion_Deduccion_ID + "'),0) AS DESPENSA");

                Mi_Sql.Append(",(SELECT " + Cat_Dependencias.Campo_Clave + " || ' - ' ||");
                Mi_Sql.Append(Cat_Dependencias.Campo_Nombre + " FROM " + Cat_Dependencias.Tabla_Cat_Dependencias);
                Mi_Sql.Append(" WHERE " + Cat_Dependencias.Campo_Dependencia_ID + " = ");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Dependencia_ID + ") AS UNIDAD_RESPONSABLE");

                Mi_Sql.Append(",(SELECT " + Cat_Nom_Tipos_Nominas.Campo_Nomina + " FROM " + Cat_Nom_Tipos_Nominas.Tabla_Cat_Nom_Tipos_Nominas);
                Mi_Sql.Append(" WHERE " + Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID + "=");
                Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Tipo_Nomina_ID + ") AS TIPO_NOMINA ");
                Mi_Sql.Append(" FROM " + Cat_Empleados.Tabla_Cat_Empleados + " WHERE ");

                //filtro por numero de empleado
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_No_Empleado))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_No_Empleado + " = '" + Empleados_Negocio.P_No_Empleado + "' AND ");
                }

                //filtro por ESTATUS EMPLEADO
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Estatus_Empleado))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_Estatus + " = '" + Empleados_Negocio.P_Estatus_Empleado + "' AND ");
                }

                //filtro por RFC
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_RFC_Empleado))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_RFC + " LIKE '%" + Empleados_Negocio.P_RFC_Empleado + "%' AND ");
                }

                //filtro por CURP
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_CURP_Empleado))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_CURP + " LIKE '%" + Empleados_Negocio.P_CURP_Empleado + "%' AND ");
                }

                //filtro por tipo de nomina
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Tipo_Nomina_ID))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_Tipo_Nomina_ID + " = '" + Empleados_Negocio.P_Tipo_Nomina_ID + "' AND ");
                }

                //filtro por sindicato
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Sindicato_ID))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_Sindicato_ID+ " = '" + Empleados_Negocio.P_Sindicato_ID + "' AND ");
                }

                //filtro por unidad responsable
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Dependencia_ID))
                {
                    Mi_Sql.Append(Cat_Empleados.Campo_Dependencia_ID + " = '" + Empleados_Negocio.P_Dependencia_ID + "' AND ");
                }

                //filtro por nombre de empleado
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Nombre_Empleado))
                {
                    Mi_Sql.Append(" UPPER(" + Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Paterno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Apellido_Materno + " || ' ' || ");
                    Mi_Sql.Append(Cat_Empleados.Tabla_Cat_Empleados + "." + Cat_Empleados.Campo_Nombre);
                    Mi_Sql.Append(") LIKE '%" + Empleados_Negocio.P_Nombre_Empleado.ToUpper() + "%' AND ");
                }

                //filtro dinamico
                if (!String.IsNullOrEmpty(Empleados_Negocio.P_Filtro_Dinamico))
                {
                    Mi_Sql.Append(Empleados_Negocio.P_Filtro_Dinamico);
                }

                // eliminar AND o WHERE del final del string
                if (Mi_Sql.ToString().EndsWith(" AND "))
                {
                    Mi_Sql.Remove(Mi_Sql.Length - 5, 5);
                }
                else if (Mi_Sql.ToString().EndsWith(" WHERE "))
                {
                    Mi_Sql.Remove(Mi_Sql.Length - 7, 7);
                }

                //Mi_Sql.Append(" ORDER BY " + Cat_Empleados.Campo_No_Empleado + " ASC");

                return OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_Sql.ToString()).Tables[0];
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al intentar consultar los registros del reporte del catálogo de empleados. Error: [" + Ex.Message + "]");
            }
        }

        #endregion
    }
}
