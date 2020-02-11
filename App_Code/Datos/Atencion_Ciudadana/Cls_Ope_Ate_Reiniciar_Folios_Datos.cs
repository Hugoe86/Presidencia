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
using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Registro_Peticion.Negocios;
using Presidencia.Constantes;
using Presidencia.Operacion_Atencion_Ciudadana_Reiniciar_Folios.Negocios;

namespace Presidencia.Operacion_Atencion_Ciudadana_Reiniciar_Folios.Datos
{

    public class Cls_Ope_Ate_Reiniciar_Folios_Datos
    {

        ///*******************************************************************************
        ///NOMBRE DE LA METODO: Reiniciar_Folios
        ///        DESCRIPCIÓN: Forma y ejecuta una consulta para desactivar las restricciones 
        ///                 de las relaciones entre las tablas de observaciones, seguimiento y archivo hacia 
        ///                 ope_ate_peticiones, actualizar folio y año y vuelve a activar las relaciones
        ///         PARAMETROS: 
        ///                     1. Datos: instancia de la clase de negocio con parámetros para la consulta
        ///               CREO: Roberto González Oseguera
        ///         FECHA_CREO: 24-may-2012
        ///           MODIFICO: 
        ///     FECHA_MODIFICO: 
        /// CAUSA_MODIFICACIÓN: 
        ///*******************************************************************************
        public static int Reiniciar_Folios(Cls_Ope_Ate_Reiniciar_Folios_Negocio Datos)
        {
            // Declaración de variables
            OracleTransaction Obj_Transaccion = null;
            OracleConnection Obj_Conexion = null;
            OracleCommand Obj_Comando;
            String Mi_SQL;
            int Filas_Afectadas = 0;

            // Inicialización de variables
            Obj_Conexion = new OracleConnection(Cls_Constantes.Str_Conexion);
            Obj_Comando = new OracleCommand();
            Obj_Conexion.Open();
            Obj_Transaccion = Obj_Conexion.BeginTransaction();
            Obj_Comando.Transaction = Obj_Transaccion;
            Obj_Comando.Connection = Obj_Conexion;

            // validar que se recibieron como parámetro el programa y el prefijo
            if (string.IsNullOrEmpty(Datos.P_Prefijo_Folio) || string.IsNullOrEmpty(Datos.P_Programa_ID))
            {
                return 0;
            }

            try
            {

                // Se forma consulta para deshabilitar restricciones de la relación Ope_Ate_Observaciones_Peticiones --> ope_ate_peticiones
                Mi_SQL = "ALTER TABLE " + Ope_Ate_Observaciones_Peticiones.Tabla_Ope_Ate_Observaciones_Peticiones 
                    + " MODIFY CONSTRAINT FKEY_OBSER_PET_ANIO_PROG DISABLE";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Obj_Comando.ExecuteNonQuery();
                // Se forma consulta para deshabilitar restricciones de la relación Ope_Ate_Seguimiento_Peticiones --> ope_ate_peticiones
                Mi_SQL = "ALTER TABLE " + Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones
                    + " MODIFY CONSTRAINT FKEY_ATE_SEG_PET_ANIO DISABLE";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Obj_Comando.ExecuteNonQuery();
                // Se forma consulta para deshabilitar restricciones de la relación Ope_Ate_Archivos_Peticiones --> ope_ate_peticiones
                Mi_SQL = "ALTER TABLE " + Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones
                    + " MODIFY CONSTRAINT OPE_ATE_ARCH_P_OPE_ATE_PET_FK DISABLE";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Obj_Comando.ExecuteNonQuery();

                // actualizar año de los registros en Ope_Ate_Observaciones_Peticiones
                Mi_SQL = "UPDATE "
                    + Ope_Ate_Observaciones_Peticiones.Tabla_Ope_Ate_Observaciones_Peticiones + " SET "
                    + Ope_Ate_Observaciones_Peticiones.Campo_Anio_Peticion + " = " + Ope_Ate_Observaciones_Peticiones.Campo_Anio_Peticion + " * -1, "
                    + Ope_Ate_Observaciones_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "', "
                    + Ope_Ate_Observaciones_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " + " WHERE "
                    + Ope_Ate_Observaciones_Peticiones.Campo_Anio_Peticion + " > 0";
                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    Mi_SQL += " AND " + Ope_Ate_Observaciones_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();

                // actualizar año de los registros en OPE_ATE_SEGUIMIENTO_PETICIONES
                Mi_SQL = "UPDATE "
                    + Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones + " SET "
                    + Ope_Ate_Seguimiento_Peticiones.Campo_Anio_Peticion + " = " + Ope_Ate_Seguimiento_Peticiones.Campo_Anio_Peticion + " * -1  WHERE "
                    + Ope_Ate_Seguimiento_Peticiones.Campo_Anio_Peticion + " > 0";
                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    Mi_SQL += " AND " + Ope_Ate_Seguimiento_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();

                // actualizar año de los registros en OPE_ATE_ARCHIVOS_PETICIONES
                Mi_SQL = "UPDATE "
                    + Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones + " SET "
                    + Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + " = " + Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + " * -1, "
                    + Ope_Ate_Archivos_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "', "
                    + Ope_Ate_Archivos_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " + " WHERE "
                    + Ope_Ate_Archivos_Peticiones.Campo_Anio_Peticion + " > 0";
                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    Mi_SQL += " AND " + Ope_Ate_Archivos_Peticiones.Campo_Programa_Id + " = '" + Datos.P_Programa_ID + "'";
                }
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();

                // actualizar año y folio de los registros en OPE_ATE_PETICIONES
                Mi_SQL = "UPDATE "
                    + Ope_Ate_Peticiones.Tabla_Ope_Ate_Peticiones + " SET "
                    + Ope_Ate_Peticiones.Campo_Anio_Peticion + " = " + Ope_Ate_Peticiones.Campo_Anio_Peticion + " * -1, "
                    + Ope_Ate_Peticiones.Campo_Folio + " = '" + Datos.P_Prefijo_Folio + "' || " + Ope_Ate_Peticiones.Campo_Folio + ", "
                    + Ope_Ate_Peticiones.Campo_Usuario_Modifico + " = '" + Datos.P_Usuario_Modifico + "', "
                    + Ope_Ate_Peticiones.Campo_Fecha_Modifico + " = SYSDATE " + " WHERE "
                    + Ope_Ate_Peticiones.Campo_Anio_Peticion + " > 0";
                if (!string.IsNullOrEmpty(Datos.P_Programa_ID))
                {
                    Mi_SQL += " AND " + Ope_Ate_Peticiones.Campo_Programa_ID + " = '" + Datos.P_Programa_ID + "'";
                }
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas = Obj_Comando.ExecuteNonQuery();


                //Se forma consulta para habilitar restricciones de la relación con la tabla ope_ate_peticiones.
                Mi_SQL = "ALTER TABLE " + Ope_Ate_Observaciones_Peticiones.Tabla_Ope_Ate_Observaciones_Peticiones
                    + " MODIFY CONSTRAINT FKEY_OBSER_PET_ANIO_PROG ENABLE";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Obj_Comando.ExecuteNonQuery();
                // Se forma consulta para habilitar restricciones de la relación Ope_Ate_Seguimiento_Peticiones --> ope_ate_peticiones
                Mi_SQL = "ALTER TABLE " + Ope_Ate_Seguimiento_Peticiones.Tabla_Ope_Ate_Seguimiento_Peticiones
                    + " MODIFY CONSTRAINT FKEY_ATE_SEG_PET_ANIO ENABLE";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Obj_Comando.ExecuteNonQuery();
                // Se forma consulta para habilitar restricciones de la relación Ope_Ate_Archivos_Peticiones --> ope_ate_peticiones
                Mi_SQL = "ALTER TABLE " + Ope_Ate_Archivos_Peticiones.Tabla_Ope_Ate_Archivos_Peticiones
                    + " MODIFY CONSTRAINT OPE_ATE_ARCH_P_OPE_ATE_PET_FK ENABLE";
                Obj_Comando.CommandText = Mi_SQL;
                Filas_Afectadas += Obj_Comando.ExecuteNonQuery();


                // aplicar cambios a la base de datos
                Obj_Transaccion.Commit();
            }
            catch (Exception Ex)
            {
                if (Obj_Transaccion != null)
                {
                    Obj_Transaccion.Rollback();
                    Filas_Afectadas = -1;
                }
                throw new Exception(Ex.ToString());
            }
            finally
            {
                if (Obj_Conexion != null)
                {
                    Obj_Conexion.Close();
                }
            }

            return Filas_Afectadas;
        }

    }
}